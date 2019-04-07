using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Filters;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class WebAPIController : ApiController
    {

        [HttpPost]
        [Route("api/WebAPI/WebAPI")]
        public HttpResponseMessage WebAPI([FromBody]ApiParameters apiParameters)
        {
            ErrorMessage errormessage = new ErrorMessage();

            try
            {
                if (apiParameters != null)
                {
                    if (ModelState.IsValid)
                    {
                        String StrResponseMessage = String.Empty;
                        ResponseMessage responsemessage = new ResponseMessage();

                        UserInput userinput = new UserInput(apiParameters);

                        // Get Header Parameters            

                        HttpRequestHeaders headers = this.Request.Headers;
                        if (headers.Contains("UserCode"))
                        {
                            string HdUserCode = headers.GetValues("UserCode").First();
                            userinput.SetUserCode(HdUserCode);
                        }
                       
                        if (headers.Contains("AuthKey"))
                        {
                            string HdAuthKey = headers.GetValues("AuthKey").First();
                            userinput.SetAuthKey(HdAuthKey);
                        }

                        // User Input Check 

                        KeyValuePair<int, string> strMessageResult = new KeyValuePair<int, string>();
                        strMessageResult = userinput.Verify();

                        if (strMessageResult.Key == 0)
                        {
                            responsemessage.Id = strMessageResult.Key;
                            responsemessage.Message = strMessageResult.Value;
                        }
                        else
                        {
                            
                        }

                        IContentNegotiator negotiator = this.Configuration.Services.GetContentNegotiator();
                        ContentNegotiationResult result = negotiator.Negotiate(typeof(ResponseMessage), this.Request, this.Configuration.Formatters);

                        if (responsemessage == null)
                        {
                            errormessage.error = "invalid_response";
                            errormessage.error_description = "Null response";
                            return this.Request.CreateResponse(HttpStatusCode.NotFound, errormessage);
                        }
                        else
                        {
                            return this.Request.CreateResponse(HttpStatusCode.OK, responsemessage, result.Formatter, result.MediaType.MediaType);
                        }
                    }
                    else
                    {
                        return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                    }
                }
                else
                {
                    errormessage.error = "invalid_request";
                    errormessage.error_description = "The body was not as specified in the request";
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, errormessage);
                }
            }
            catch
            {
                errormessage.error = "Failed";
                errormessage.error_description = "Some Error Occured";
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, errormessage);
            }
        }

        [HttpPost]
        [Route("api/DocumentUpload/MediaUpload")]
        public async Task<HttpResponseMessage> MediaUpload()
        {
            // Check if the request contains multipart/form-data.  
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
            //access form data  
            NameValueCollection formData = provider.FormData;
            //access files  
            IList<HttpContent> files = provider.Files;

            HttpContent file1 = files[0];
            var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"');

            ////-------------------------------------For testing----------------------------------  
            //to append any text in filename.  
            //var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"') + DateTime.Now.ToString("yyyyMMddHHmmssfff"); //ToDo: Uncomment this after UAT as per Jeeevan  

            //List<string> tempFileName = thisFileName.Split('.').ToList();  
            //int counter = 0;  
            //foreach (var f in tempFileName)  
            //{  
            //    if (counter == 0)  
            //        thisFileName = f;  

            //    if (counter > 0)  
            //    {  
            //        thisFileName = thisFileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + f;  
            //    }  
            //    counter++;  
            //}  

            ////-------------------------------------For testing----------------------------------  

            string filename = String.Empty;
            Stream input = await file1.ReadAsStreamAsync();
            string directoryName = String.Empty;
            string URL = String.Empty;
            string tempDocUrl = WebConfigurationManager.AppSettings["DocsUrl"];

            if (formData["ClientDocs"] == "ClientDocs")
            {
                var path = HttpRuntime.AppDomainAppPath;
                directoryName = System.IO.Path.Combine(path, "ClientDocument");
                filename = System.IO.Path.Combine(directoryName, thisFileName);

                //Deletion exists file  
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                string DocsPath = tempDocUrl + "/" + "ClientDocument" + "/";
                URL = DocsPath + thisFileName;

            }


            //Directory.CreateDirectory(@directoryName);  
            using (Stream file = File.OpenWrite(filename))
            {
                input.CopyTo(file);
                //close file  
                file.Close();
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("DocsUrl", URL);
            return response;
        }  
    }
}
