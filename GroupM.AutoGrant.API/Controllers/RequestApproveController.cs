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
    public class RequestApproveController : Controller
    {
        private string passwordEncrypt = ConfigurationManager.AppSettings["PEAPI"];
        private string urlAPI = ConfigurationManager.AppSettings["URLAPI"];
        private string urlSuccess = ConfigurationManager.AppSettings["URLAPSuccess"];
        private string urlFail = ConfigurationManager.AppSettings["URLAPFail"];
        private RequestData request = new RequestData();
        private Email email = new Email();
        private Log log = new Log();

        public ActionResult Index(string token)
        {
            string url = urlAPI + "RequestApprove/Index?token=" + token;
            try
            {
                string json = Crypt.Decrypt(token, passwordEncrypt);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                RequestToken requestToken = serializer.Deserialize<RequestToken>(json);
                DataTable table = request.updateApprove(requestToken, "Email");
                bool approve = Convert.ToBoolean(table.Rows[0]["Status"]);
                string result = table.Rows[0]["Approve"].ToString();
                if (approve == true)
                {
                    // send email to user create request
                    bool sendEmail = email.SendEmailApproveToRequestUser(requestToken, "Email");
                    bool logAPI = log.InsertLogAPI("Request Approve", url, "", "", "", 200, "Approve From Email Success.");
                    return Redirect(urlSuccess);
                }
                else
                {
                    return Redirect(urlFail);
                }
            }
            catch (Exception e)
            {
                bool logFail = log.InsertLogRequest("Approve Request", "Failed", "", e.Message, "");
                bool logAPI = log.InsertLogAPI("Request Approve", url, "", "", "", 400, "Approve From Email Fail. " + e.Message);
                return View("Error");
            }
        }
    }
}
