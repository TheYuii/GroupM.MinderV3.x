using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupM.AutoGrant.API.Models.Json_Models
{
    public class RejectReason
    {
        public string Request_No { get; set; }
        public string Approver_ID { get; set; }
        public string Approver_UserID { get; set; }
        public string Delegate_UserID { get; set; }
        public string Approve_Result { get; set; }
        public string Reject_Reason { get; set; }
        public string Reject_Detail { get; set; }
    }
}