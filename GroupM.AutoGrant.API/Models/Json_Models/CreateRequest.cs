using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupM.AutoGrant.API.Models.Json_Models
{
    public class CreateRequestUser
    {
        public string User_ID { get; set; }
        public string User_Email { get; set; }
        public string Agency_ID { get; set; }
        public string Office_ID { get; set; }
    }

    public class CreateRequestMediaType
    {
        public string Media_Type { get; set; }
    }

    public class CreateRequest
    {
        public string Request_Type { get; set; }
        public string Request_Status { get; set; }
        public string Request_By { get; set; }
        public CreateRequestUser[] listRequestUser { get; set; }
        public CreateRequestMediaType[] listRequestMediaType { get; set; }
        public CopyPermission[] listRequestPermission { get; set; }
    }
}