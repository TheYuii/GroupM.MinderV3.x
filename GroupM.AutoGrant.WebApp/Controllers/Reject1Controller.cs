using GroupM.AutoGrant.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GroupM.AutoGrant.WebApp.Controllers
{
    public class Reject1Controller : Controller
    {
        private string passwordEncrypt = ConfigurationManager.AppSettings["PEAPI"];
        private string urlAPI = ConfigurationManager.AppSettings["URLAPI"];
        private string urlAPP = ConfigurationManager.AppSettings["URLAPP"];

        public ActionResult Index(string token)
        {
            try
            {
                string json = Crypt.Decrypt(token, passwordEncrypt);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                RequestToken requestToken = serializer.Deserialize<RequestToken>(json);
                if (requestToken != null)
                {
                    ViewBag.Token = token;
                    ViewBag.RequestNo = requestToken.Request_No;
                    ViewBag.ApproverID = requestToken.Approver_ID;
                    ViewBag.ApproverUserID = requestToken.Approver_UserID;
                    ViewBag.DelegateUserID = requestToken.Delegate_UserID;
                    ViewBag.Message = "";
                    return View();
                }
                else
                {
                    ViewBag.Message = "Error: Token is null.";
                    return View();
                }
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Index(string Token, string RequestNo, string ApproverID, string ApproverUserID, string DelegateUserID, string rdReason, string Reason, string btnCheck)
        {
            try
            {
                if (btnCheck == "Submit")
                {
                    RejectReason reject = new RejectReason();
                    reject.Request_No = RequestNo;
                    reject.Approver_ID = ApproverID;
                    reject.Approver_UserID = ApproverUserID;
                    reject.Delegate_UserID = DelegateUserID;
                    reject.Approve_Result = "RJ";
                    reject.Reject_Reason = rdReason;
                    reject.Reject_Detail = Reason;
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string jsonReject = serializer.Serialize(reject);
                    string tokenReject = Crypt.Encrypt(jsonReject, passwordEncrypt);
                    string urlReject = urlAPI + "RequestReject/Index?token=" + tokenReject;
                    return Redirect(urlReject);
                }
                else if (btnCheck == "Cancel")
                {
                    return RedirectToAction("Reject", "Login");
                }
                else
                {
                    string url = urlAPP + "Reject/Index?token=" + Token;
                    return Redirect(url);
                }
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return View();
            }
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Fail()
        {
            return View();
        }
    }
}
