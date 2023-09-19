using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupM.AutoGrant.API.Models.Json_Models
{
    public class RequestUser
    {
        public string Username_Agency { get; set; }
    }

    public class RequestMediaType
    {
        public string Media_Type_Name { get; set; }
    }

    public class RequestPermission
    {
        public string Permission_Display_Name { get; set; }
    }

    public class RequestShow
    {
        public string Request_No { get; set; }
        public string Request_Type { get; set; }
        public string Request_Type_Display { get; set; }
        public string Request_Status { get; set; }
        public string Request_Status_Display { get; set; }
        public string Complete_Date { get; set; }
        public string Expired_Date { get; set; }
        public string Total_Approver { get; set; }
        public string Total_Approved { get; set; }
        public string Create_Date { get; set; }
        public string Create_By { get; set; }
        public string Update_Date { get; set; }
        public string Update_By { get; set; }
        public RequestUser[] listRequestUser { get; set; }
        public RequestMediaType[] listRequestMediaType { get; set; }
        public RequestPermission[] listRequestPermission { get; set; }
    }
}