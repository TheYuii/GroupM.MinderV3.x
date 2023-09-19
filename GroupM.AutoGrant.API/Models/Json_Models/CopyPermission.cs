using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupM.AutoGrant.API.Models.Json_Models
{
    public class Permission
    {
        public string Permission_Code { get; set; }
        public string Permission_Name { get; set; }
        public string Permission_Display_Name { get; set; }
    }

    public class CopyPermission
    {
        public string Agency_ID { get; set; }
        public string Agency_Name { get; set; }
        public string Office_ID { get; set; }
        public string Office_Name { get; set; }
        public string Approver_UserID { get; set; }
        public string Approver_Name { get; set; }
        public string Approver_Email { get; set; }
        public string Delegate_UserID { get; set; }
        public string Permission_Level { get; set; }
        public Permission[] Permission_List { get; set; }
    }
}