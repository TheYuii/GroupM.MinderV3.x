using GroupM.AutoGrant.API.Models;
using GroupM.AutoGrant.API.Models.Json_Data;
using GroupM.AutoGrant.API.Models.Json_Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GroupM.AutoGrant.API.Controllers
{
    public class RequestRejectController : Controller
    {
        private string passwordEncrypt = ConfigurationManager.AppSettings["PEAPI"];
        private string urlAPI = ConfigurationManager.AppSettings["URLAPI"];
        private string urlSuccess = ConfigurationManager.AppSettings["URLRJSuccess"];
        private string urlFail = ConfigurationManager.AppSettings["URLRJFail"];
        private RequestData request = new RequestData();
        private Email email = new Email();
        private Log log = new Log();

        public ActionResult Index(string token)
        {
            string url = urlAPI + "RequestReject/Index?token=" + token;
            try
            {
                string json = Crypt.Decrypt(token, passwordEncrypt);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                RejectReason rejectReason = serializer.Deserialize<RejectReason>(json);
                DataTable table = request.updateReject(rejectReason, "Email");
                bool reject = Convert.ToBoolean(table.Rows[0]["Status"]);
                string result = table.Rows[0]["Reject"].ToString();
                if (reject == true)
                {
                    // send email to user create request
                    bool sendEmail = email.SendEmailRejectToRequestUser(rejectReason, "Email");
                    bool logAPI = log.InsertLogAPI("Request Reject", url, "", "", "", 200, "Reject From Email Success.");
                    return Redirect(urlSuccess);
                }
                else
                {
                    return Redirect(urlFail);
                }
            }
            catch (Exception e)
            {
                bool logFail = log.InsertLogRequest("Reject Request", "Failed", "", e.Message, "");
                bool logAPI = log.InsertLogAPI("Request Reject", url, "", "", "", 400, "Reject From Email Fail. " + e.Message);
                return View("Error");
            }
        }
    }
}
