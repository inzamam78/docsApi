using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class UserInput
    {
        private string strUserCode, strAccountNumber, strRemark, strAuthKey, strSathiID, strTPIN, strOperatorCode, strRequestID, strTermsConditionsFlag,
                       strUserID, strMobileNumber, strAmount, strServiceChargeAmount, strSubscriberNumber, strServiceCode;
        private int getServiceID;
        private int getOperator;

        public UserInput() { }
        public UserInput(ApiParameters apiparameters)
        {
            strUserID = apiparameters.UserID;
            strMobileNumber = apiparameters.MobileNumber;
          
        }

        public string UserID
        {
            set { strUserID = value; }
            get { return strUserID; }
        }
        public string MobileNumber
        {
            set { strMobileNumber = value; }
            get { return strMobileNumber; }
        }

        public void SetUserCode(String userCode)
        {
            strUserCode = userCode;
        }
        public void SetAuthKey(String authKey)
        {
            strAuthKey = authKey;
        }

        private class ListResponse
        {
            public int id { get; set; }
            public Object responseData { get; set; }
            public string message { get; set; }
        }

        public KeyValuePair<int, string> CheckAuthKey(string URL_AuthKey, string AuthKey)
        {
            try
            {
                if (URL_AuthKey != AuthKey)
                {
                    return new KeyValuePair<int, string>(0, "Invalid Auth_KEY");
                }
                return new KeyValuePair<int, string>(1, "Auth_KEY Verification Successful");
            }
            catch (Exception exc)
            {
                return new KeyValuePair<int, string>(0, exc.Message);
            }
        }


        public KeyValuePair<int, string> Verify()
        {
            Int64 iValue;
            try
            {
                if (String.IsNullOrEmpty(strUserCode))
                    return new KeyValuePair<int, string>(0, "UserCode - Header Parameter is Required");

                if (String.IsNullOrEmpty(strTPIN))
                    return new KeyValuePair<int, string>(0, "TPIN - Header Parameter is Required");
                if (!(Int64.TryParse(strTPIN, out iValue)))
                    return new KeyValuePair<int, string>(0, "TPIN - Header Parameter has Invalid Data Type");

                if (String.IsNullOrEmpty(strAuthKey))
                    return new KeyValuePair<int, string>(0, "Auth Key - Header Parameter is Required");

                if (String.IsNullOrEmpty(strServiceCode))
                    return new KeyValuePair<int, string>(0, "Service Code is Required");


                if (String.IsNullOrEmpty(strTermsConditionsFlag))
                    return new KeyValuePair<int, string>(0, "Terms And Conditions Flag is Required");
                if (strTermsConditionsFlag.Trim() != "0" && strTermsConditionsFlag.Trim() != "1")
                    return new KeyValuePair<int, string>(0, "Invalid Terms And Conditions Flag Parameter.");


                return new KeyValuePair<int, string>(1, "Verification Successful");
            }
            catch (Exception exc)
            {
                return new KeyValuePair<int, string>(0, exc.Message);
            }
        }

    }

}