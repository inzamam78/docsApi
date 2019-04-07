using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI.Models;

namespace WebAPI.Models
{
    public class ResponseMessage
    {
        public int Id { get; set; }
        public MainResponse MainResponse { get; set; }
        public string Message { get; set; }
    }
}