using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupM.AutoGrant.API.Models.Json_Models
{
    public class RequestDetailUser
    {
        public string User_ID { get; set; }
        public string User_Full_Name { get; set; }
        public string Agency_ID { get; set; }
        public string Agency_Name { get; set; }
        public string Office_ID { get; set; }
        public string Office_Name { get; set; }
        public string User_Email { get; set; }
        public string Username_Agency { get; set; }
    }

    public class RequestDetailMediaType
    {
        public string Media_Type { get; set; }
        public string Media_Type_Name { get; set; }
        public string Media_Type_Display_Name { get; set; }
    }

    public class RequestDetailPermission
    {
        public string Agency_ID { get; set; }
        public string Agency_Name { get; set; }
        public string Office_ID { get; set; }
        public string Office_Name { get; set; }
        public string Approver_UserID { get; set; }
        public string Approver_Name { get; set; }
        public string Approver_Email { get; set; }
        public string Delegate_UserID { get; set; }
        public string Approve_Result { get; set; }
        public string Approve_Result_Display { get; set; }
        public string Reject_Reason { get; set; }
        public string Permission_Level { get; set; }
        public Permission[] Permission_List { get; set; }
    }

    public class RequestDetail
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
        public string Request_User { get; set; }
        public string Request_User_Email { get; set; }
        public string Create_Date { get; set; }
        public string Create_By { get; set; }
        public string Update_Date { get; set; }
        public string Update_By { get; set; }
        public RequestDetailUser[] listRequestUser { get; set; }
        public RequestDetailMediaType[] listRequestMediaType { get; set; }
        public RequestDetailPermission[] listRequestPermission { get; set; }
    }
}