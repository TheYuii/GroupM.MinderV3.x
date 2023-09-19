using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupM.AutoGrant.API.Models.Json_Models
{
    public class RequestCancel
    {
        public string Request_No { get; set; }
        public string Cancel_Reason { get; set; }
    }
}