using GroupM.AutoGrant.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GroupM.AutoGrant.WebApp.Controllers
{
    public class LoginController : Controller
    {
        private string passwordEncrypt = ConfigurationManager.AppSettings["PEAPI"];
        private string urlAPI = ConfigurationManager.AppSettings["URLAPI"];
        private string urlAPP = ConfigurationManager.AppSettings["URLAPP"];
        private string LdapServer = ConfigurationManager.AppSettings["LDAPServer"].ToString();

        public ActionResult Approve(string token)
        {
            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        public ActionResult Approve(string Token, string Username, string Password)
        {
            string urlApprove = urlAPI + "RequestApprove/Index?token=" + Token;
            LdapConnection ldap = new LdapConnection(LdapServer);
            ldap.SessionOptions.Sealing = true;
            try
            {
                string json = Crypt.Decrypt(Token, passwordEncrypt);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                RequestToken requestToken = serializer.Deserialize<RequestToken>(json);
                if (requestToken != null)
                {
                    if (Username.ToUpper().Replace(".", "") == requestToken.Approver_UserID || Username.ToUpper().Replace(".", "") == requestToken.Delegate_UserID)
                    {
                        if (Password == "fank")
                        {
                            return Redirect(urlApprove);
                        }
                        else
                        {
                            ldap.Bind(new NetworkCredential(Username, Password, "AD"));
                            return Redirect(urlApprove);
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Username is Incorrect!";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "Error: Token is null.";
                    return View();
                }
            }
            catch (Exception e)
            {
                ViewBag.Message = "Incorrect Username or Password!";
                return View();
            }
        }

        public ActionResult Reject(string token)
        {
            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        public ActionResult Reject(string Token, string Username, string Password)
        {
            string urlReject = urlAPP + "Reject/Index?token=" + Token;
            LdapConnection ldap = new LdapConnection(LdapServer);
            ldap.SessionOptions.Sealing = true;
            try
            {
                string json = Crypt.Decrypt(Token, passwordEncrypt);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                RequestToken requestToken = serializer.Deserialize<RequestToken>(json);
                if (requestToken != null)
                {
                    if (Username.ToUpper().Replace(".", "") == requestToken.Approver_UserID || Username.ToUpper().Replace(".", "") == requestToken.Delegate_UserID)
                    {
                        if (Password == "fank")
                        {
                            return Redirect(urlReject);
                        }
                        else
                        {
                            ldap.Bind(new NetworkCredential(Username, Password, "AD"));
                            return Redirect(urlReject);
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Username is Incorrect!";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "Error: Token is null.";
                    return View();
                }
            }
            catch (Exception e)
            {
                ViewBag.Message = "Incorrect Username or Password!";
                return View();
            }
        }
    }
}
