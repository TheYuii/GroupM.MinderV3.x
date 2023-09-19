using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupM.AutoGrant.API.Models.Json_Models
{
    public class ApprovalShow
    {
        public string Approver_ID { get; set; }
        public string Request_No { get; set; }
        public string Request_Type { get; set; }
        public string Request_Type_Display { get; set; }
        public string Request_Status { get; set; }
        public string Request_Status_Display { get; set; }
        public string Complete_Date { get; set; }
        public string Expired_Date { get; set; }
        public string Approve_Result { get; set; }
        public string Approve_Result_Display { get; set; }
        public string Total_Approver { get; set; }
        public string Total_Approved { get; set; }
        public string Reject_Reason { get; set; }
        public string Create_Date { get; set; }
        public string Create_By { get; set; }
        public string Update_Date { get; set; }
        public string Update_By { get; set; }
        public RequestUser[] listRequestUser { get; set; }
        public RequestMediaType[] listRequestMediaType { get; set; }
        public RequestPermission[] listRequestPermission { get; set; }
    }
}