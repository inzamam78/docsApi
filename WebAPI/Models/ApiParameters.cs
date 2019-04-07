using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class ApiParameters
    {
        public ApiParameters() { }

        public string UserID { get; set; }

        [Required(ErrorMessage = "Mobile Number is Required")]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Mobile Number has Invalid Length")]
        public string MobileNumber { get; set; }

       


    }

}