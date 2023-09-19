using GroupM.AutoGrant.API.Models.Json_Data;
using GroupM.AutoGrant.API.Models.Json_Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace GroupM.AutoGrant.API.Models
{
    public class Email
    {
        private string urlAPP = ConfigurationManager.AppSettings["URLAPP"];
        private string urlRequest = ConfigurationManager.AppSettings["URLRequest"];
        private RequestData request = new RequestData();
        private Log log = new Log();

        public bool SendEmailRequestToApprover(string requestNo)
        {
            try
            {
                DataTable tableRequest = request.getRequestDetail(requestNo);
                DataTable tableUser = request.getRequestDetailUser(requestNo);
                DataTable tableMediaType = request.getRequestDetailMediaType(requestNo);
                DataTable tableApprover = request.getRequestDetailApprover(requestNo);
                for (int i = 0; i < tableApprover.Rows.Count; i++)
                {
                    string approveID = tableApprover.Rows[i]["Approver_ID"].ToString();
                    string toEmail = tableApprover.Rows[i]["Approver_Email"].ToString();

                    // test -------------------------------------------------
                    string ccEmail = "Chanchai.Liangboonprakong@groupm.com";
                    // test -------------------------------------------------

                    string tokenApprove = tableApprover.Rows[i]["Approve_Token"].ToString();
                    string tokenReject = tableApprover.Rows[i]["Reject_Token"].ToString();
                    string urlApprove = urlAPP + "Login/Approve?token=" + tokenApprove;
                    string urlReject = urlAPP + "Login/Reject?token=" + tokenReject;
                    DataTable tableClient = request.getRequestPermissionByApprover(requestNo, approveID);
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("relay.nlbint.insidemedia.net");
                    SmtpServer.Port = 25;
                    mail.From = new MailAddress("GRMThailandITTeam@ad.insidemedia.net");
                    mail.To.Add(toEmail);

                    // test --------------
                    mail.CC.Add(ccEmail);
                    // test --------------

                    mail.Subject = "Permission Access (Minder) - Request No: " + requestNo;
                    mail.IsBodyHtml = true;
                    string body = "<p style = \"font-size: 24px\">แจ้งขอสิทธิ์ในการเข้าถึงข้อมูลสำหรับโปรแกรม <span style = \"font-size: 20px\">Minder</span> ของ <span style = \"font-size: 20px\">" + tableUser.Rows[0]["Username_Agency"].ToString() + "</span></p>" +
                    "<p>" +
                    "<span style = \"font-size: 20px; color: blue\">ผู้ใช้ที่ต้องการขอสิทธิ์ในการเข้าถึงข้อมูล:</span>" +
                    "<br/>";
                    for (int j = 0; j < tableUser.Rows.Count; j++)
                    {
                        body += (j + 1).ToString() + ". " + tableUser.Rows[j]["Username_Agency"].ToString() + "<br/>";
                    }
                    body += "</p>" +
                    "<p>" +
                    "<span style = \"font-size: 20px; color: blue\">ประเภท</span> <span style = \"font-size: 16px; color: blue\">Media:</span>" +
                    "<br/>";
                    for (int j = 0; j < tableMediaType.Rows.Count; j++)
                    {
                        body += (j + 1).ToString() + ". " + tableMediaType.Rows[j]["Media_Type_Display_Name"].ToString() + "<br/>";
                    }
                    body += "</p>" +
                    "<p>" +
                    "<span style = \"font-size: 20px; color: blue\">สิทธิ์ในการเห็นลูกค้า:</span>" +
                    "<br/>";
                    for (int j = 0; j < tableClient.Rows.Count; j++)
                    {
                        string level = tableClient.Rows[j]["Permission_Level"].ToString();
                        if (level == "Client Level")
                        {
                            body += (j + 1).ToString() + ". " + tableClient.Rows[j]["Permission_Display_Name"].ToString() + "<br/>";
                        }
                        else
                        {
                            body += (j + 1).ToString() + ". <span style = \"font-size: 20px\">ลูกค้าทั้งหมดของ</span> " + tableClient.Rows[j]["Permission_Display_Name"].ToString() + "<br/>";
                        }
                    }
                    body += "</p>" +
                    "<a href = \"" + urlApprove + "\" style = \"font-size: 20px; color: green\">[Approve]</a>" +
                    "&nbsp;&nbsp;&nbsp;&nbsp;" +
                    "<a href = \"" + urlReject + "\" style = \"font-size: 20px; color: gray\">[Reject]</a>" +
                    "<br/>" +
                    "<br/>" +
                    "<hr>" +
                    "<br/>" +
                    "<p style = \"font-size: 20px\">Permission Access Request for Minder of " + tableUser.Rows[0]["Username_Agency"].ToString() + "</p>" +
                    "<p>" +
                    "<span style = \"font-size: 16px; color: blue\">Request Access User(s):</span>" +
                    "<br/>";
                    for (int j = 0; j < tableUser.Rows.Count; j++)
                    {
                        body += (j + 1).ToString() + ". " + tableUser.Rows[j]["Username_Agency"].ToString() + "<br/>";
                    }
                    body += "</p>" +
                    "<p>" +
                    "<span style = \"font-size: 16px; color: blue\">Media Type:</span>" +
                    "<br/>";
                    for (int j = 0; j < tableMediaType.Rows.Count; j++)
                    {
                        body += (j + 1).ToString() + ". " + tableMediaType.Rows[j]["Media_Type_Display_Name"].ToString() + "<br/>";
                    }
                    body += "</p>" +
                    "<p>" +
                    "<span style = \"font-size: 16px; color: blue\">Request to Access Client:</span>" +
                    "<br/>";
                    for (int j = 0; j < tableClient.Rows.Count; j++)
                    {
                        string level = tableClient.Rows[j]["Permission_Level"].ToString();
                        if (level == "Client Level")
                        {
                            body += (j + 1).ToString() + ". " + tableClient.Rows[j]["Permission_Display_Name"].ToString() + "<br/>";
                        }
                        else
                        {
                            body += (j + 1).ToString() + ". All Client of " + tableClient.Rows[j]["Permission_Display_Name"].ToString() + "<br/>";
                        }
                    }
                    body += "</p>" +
                    "<a href = \"" + urlApprove + "\" style = \"font-size: 20px; color: green\">[Approve]</a>" +
                    "&nbsp;&nbsp;&nbsp;&nbsp;" +
                    "<a href = \"" + urlReject + "\" style = \"font-size: 20px; color: gray\">[Reject]</a>" +
                    "<br/>" +
                    "<br/>" +
                    "You can find all the details about this request by <a href = \"" + urlRequest + "\">View Request</a>" +
                    "<br/>" +
                    "Request No. " + requestNo +
                    "<br/>" +
                    "Submitted Date " + Convert.ToDateTime(tableRequest.Rows[0]["Create_Date"]).ToString("dd-MMM-yyyy HH:mm");
                    mail.Body = body;
                    SmtpServer.Send(mail);
                    // insert log
                    bool logSuccess = log.InsertLogRequest("Send Email Request to Approver", "Complete", requestNo, "Approver ID: " + approveID + ", Email: " + toEmail, tableRequest.Rows[0]["Create_By"].ToString());
                }
                return true;
            }
            catch (Exception e)
            {
                // insert log
                bool logFail = log.InsertLogRequest("Send Email Request to Approver", "Failed", requestNo, e.Message, "");
                return false;
            }
        }

        public bool SendEmailCancelToApprover(string requestNo)
        {
            try
            {
                DataTable tableRequest = request.getRequestDetail(requestNo);
                DataTable tableUser = request.getRequestDetailUser(requestNo);
                DataTable tableMediaType = request.getRequestDetailMediaType(requestNo);
                DataTable tableApprover = request.getRequestDetailApprover(requestNo);
                for (int i = 0; i < tableApprover.Rows.Count; i++)
                {
                    string approveID = tableApprover.Rows[i]["Approver_ID"].ToString();
                    string toEmail = tableApprover.Rows[i]["Approver_Email"].ToString();

                    // test -------------------------------------------------
                    string ccEmail = "Chanchai.Liangboonprakong@groupm.com";
                    // test -------------------------------------------------

                    DataTable tableClient = request.getRequestPermissionByApprover(requestNo, approveID);
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("relay.nlbint.insidemedia.net");
                    SmtpServer.Port = 25;
                    mail.From = new MailAddress("GRMThailandITTeam@ad.insidemedia.net");
                    mail.To.Add(toEmail);

                    // test --------------
                    mail.CC.Add(ccEmail);
                    // test --------------

                    mail.Subject = "Permission Access (Minder) - Request No: " + requestNo;
                    mail.IsBodyHtml = true;
                    string body = "<p style = \"font-size: 24px\">แจ้งยกเลิกการขอสิทธิ์ในการเข้าถึงข้อมูลสำหรับโปรแกรม <span style = \"font-size: 20px\">Minder</span> ของ <span style = \"font-size: 20px\">" + tableUser.Rows[0]["Username_Agency"].ToString() + "</span></p>" +
                    "<p>" +
                    "<span style = \"font-size: 20px; color: blue\">ผู้ใช้ที่ต้องการขอสิทธิ์ในการเข้าถึงข้อมูล:</span>" +
                    "<br/>";
                    for (int j = 0; j < tableUser.Rows.Count; j++)
                    {
                        body += (j + 1).ToString() + ". " + tableUser.Rows[j]["Username_Agency"].ToString() + "<br/>";
                    }
                    body += "</p>" +
                    "<p>" +
                    "<span style = \"font-size: 20px; color: blue\">ประเภท</span> <span style = \"font-size: 16px; color: blue\">Media:</span>" +
                    "<br/>";
                    for (int j = 0; j < tableMediaType.Rows.Count; j++)
                    {
                        body += (j + 1).ToString() + ". " + tableMediaType.Rows[j]["Media_Type_Display_Name"].ToString() + "<br/>";
                    }
                    body += "</p>" +
                    "<p>" +
                    "<span style = \"font-size: 20px; color: blue\">สิทธิ์ในการเห็นลูกค้า:</span>" +
                    "<br/>";
                    for (int j = 0; j < tableClient.Rows.Count; j++)
                    {
                        string level = tableClient.Rows[j]["Permission_Level"].ToString();
                        if (level == "Client Level")
                        {
                            body += (j + 1).ToString() + ". " + tableClient.Rows[j]["Permission_Display_Name"].ToString() + "<br/>";
                        }
                        else
                        {
                            body += (j + 1).ToString() + ". <span style = \"font-size: 20px\">ลูกค้าทั้งหมดของ</span> " + tableClient.Rows[j]["Permission_Display_Name"].ToString() + "<br/>";
                        }
                    }
                    body += "</p>" +
                    "<br/>" +
                    "<br/>" +
                    "<hr>" +
                    "<br/>" +
                    "<p style = \"font-size: 20px\">Cancel Permission Access Request for Minder of " + tableUser.Rows[0]["Username_Agency"].ToString() + "</p>" +
                    "<p>" +
                    "<span style = \"font-size: 16px; color: blue\">Request Access User(s):</span>" +
                    "<br/>";
                    for (int j = 0; j < tableUser.Rows.Count; j++)
                    {
                        body += (j + 1).ToString() + ". " + tableUser.Rows[j]["Username_Agency"].ToString() + "<br/>";
                    }
                    body += "</p>" +
                    "<p>" +
                    "<span style = \"font-size: 16px; color: blue\">Media Type:</span>" +
                    "<br/>";
                    for (int j = 0; j < tableMediaType.Rows.Count; j++)
                    {
                        body += (j + 1).ToString() + ". " + tableMediaType.Rows[j]["Media_Type_Display_Name"].ToString() + "<br/>";
                    }
                    body += "</p>" +
                    "<p>" +
                    "<span style = \"font-size: 16px; color: blue\">Request to Access Client:</span>" +
                    "<br/>";
                    for (int j = 0; j < tableClient.Rows.Count; j++)
                    {
                        string level = tableClient.Rows[j]["Permission_Level"].ToString();
                        if (level == "Client Level")
                        {
                            body += (j + 1).ToString() + ". " + tableClient.Rows[j]["Permission_Display_Name"].ToString() + "<br/>";
                        }
                        else
                        {
                            body += (j + 1).ToString() + ". All Client of " + tableClient.Rows[j]["Permission_Display_Name"].ToString() + "<br/>";
                        }
                    }
                    body += "</p>" +
                    "<br/>" +
                    "<br/>" +
                    "You can find all the details about this request by <a href = \"" + urlRequest + "\">View Request</a>" +
                    "<br/>" +
                    "Request No. " + requestNo +
                    "<br/>" +
                    "Submitted Date " + Convert.ToDateTime(tableRequest.Rows[0]["Create_Date"]).ToString("dd-MMM-yyyy HH:mm");
                    mail.Body = body;
                    SmtpServer.Send(mail);
                    // insert log
                    bool logSuccess = log.InsertLogRequest("Send Email Cancel to Approver", "Complete", requestNo, "Approver ID: " + approveID + ", Email: " + toEmail, tableRequest.Rows[0]["Create_By"].ToString());
                }
                return true;
            }
            catch (Exception e)
            {
                // insert log
                bool logFail = log.InsertLogRequest("Send Email Cancel to Approver", "Failed", requestNo, e.Message, "");
                return false;
            }
        }

        public bool SendEmailApproveToRequestUser(RequestToken requestToken, string approveFrom)
        {
            try
            {
                DataTable tableRequest = request.getRequestDetail(requestToken.Request_No);
                DataTable tableUser = request.getRequestDetailUser(requestToken.Request_No);
                DataTable tableMediaType = request.getRequestDetailMediaType(requestToken.Request_No);
                DataTable tableClient = request.getRequestPermissionByApprover(requestToken.Request_No, requestToken.Approver_ID);
                string toEmail = tableRequest.Rows[0]["Request_User_Email"].ToString();

                // test -------------------------------------------------
                string ccEmail = "Chanchai.Liangboonprakong@groupm.com";
                // test -------------------------------------------------

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("relay.nlbint.insidemedia.net");
                SmtpServer.Port = 25;
                mail.From = new MailAddress("GRMThailandITTeam@ad.insidemedia.net");
                mail.To.Add(toEmail);

                // test --------------
                mail.CC.Add(ccEmail);
                // test --------------

                mail.Subject = "Permission Access (Minder) - Request No: " + requestToken.Request_No + " has been approved.";
                mail.IsBodyHtml = true;
                string body = "<p style = \"font-size: 24px\">รายการสิทธิ์ในการเข้าถึงข้อมูลลูกค้าที่ได้รับการอนุมัติ</p>" +
                "<p>" +
                "<span style = \"font-size: 20px; color: blue\">ผู้ใช้ที่ต้องการขอสิทธิ์ในการเข้าถึงข้อมูล:</span>" +
                "<br/>";
                for (int j = 0; j < tableUser.Rows.Count; j++)
                {
                    body += (j + 1).ToString() + ". " + tableUser.Rows[j]["Username_Agency"].ToString() + "<br/>";
                }
                body += "</p>" +
                "<p>" +
                "<span style = \"font-size: 20px; color: blue\">ประเภท</span> <span style = \"font-size: 16px; color: blue\">Media:</span>" +
                "<br/>";
                for (int j = 0; j < tableMediaType.Rows.Count; j++)
                {
                    body += (j + 1).ToString() + ". " + tableMediaType.Rows[j]["Media_Type_Display_Name"].ToString() + "<br/>";
                }
                body += "</p>" +
                "<p>" +
                "<span style = \"font-size: 20px; color: blue\">สิทธิ์ในการเห็นลูกค้า:</span>" +
                "<br/>";
                for (int j = 0; j < tableClient.Rows.Count; j++)
                {
                    string level = tableClient.Rows[j]["Permission_Level"].ToString();
                    if (level == "Client Level")
                    {
                        body += (j + 1).ToString() + ". " + tableClient.Rows[j]["Permission_Display_Name"].ToString() + "<br/>";
                    }
                    else
                    {
                        body += (j + 1).ToString() + ". <span style = \"font-size: 20px\">ลูกค้าทั้งหมดของ</span> " + tableClient.Rows[j]["Permission_Display_Name"].ToString() + "<br/>";
                    }
                }
                body += "<br/>" +
                "<br/>" +
                "<hr>" +
                "<br/>" +
                "<p style = \"font-size: 20px\">Approved Clients Permission List</p>" +
                "<p>" +
                "<span style = \"font-size: 16px; color: blue\">Request Access User(s):</span>" +
                "<br/>";
                for (int j = 0; j < tableUser.Rows.Count; j++)
                {
                    body += (j + 1).ToString() + ". " + tableUser.Rows[j]["Username_Agency"].ToString() + "<br/>";
                }
                body += "</p>" +
                "<p>" +
                "<span style = \"font-size: 16px; color: blue\">Media Type:</span>" +
                "<br/>";
                for (int j = 0; j < tableMediaType.Rows.Count; j++)
                {
                    body += (j + 1).ToString() + ". " + tableMediaType.Rows[j]["Media_Type_Display_Name"].ToString() + "<br/>";
                }
                body += "</p>" +
                "<p>" +
                "<span style = \"font-size: 16px; color: blue\">Request to Access Client:</span>" +
                "<br/>";
                for (int j = 0; j < tableClient.Rows.Count; j++)
                {
                    string level = tableClient.Rows[j]["Permission_Level"].ToString();
                    if (level == "Client Level")
                    {
                        body += (j + 1).ToString() + ". " + tableClient.Rows[j]["Permission_Display_Name"].ToString() + "<br/>";
                    }
                    else
                    {
                        body += (j + 1).ToString() + ". All Client of " + tableClient.Rows[j]["Permission_Display_Name"].ToString() + "<br/>";
                    }
                }
                body += "<br/>" +
                "<br/>" +
                "You can find all the details about this request by <a href = \"" + urlRequest + "\">View Request</a>" +
                "<br/>" +
                "Request No. " + requestToken.Request_No +
                "<br/>" +
                "Submitted Date " + Convert.ToDateTime(tableRequest.Rows[0]["Create_Date"]).ToString("dd-MMM-yyyy HH:mm");
                mail.Body = body;
                SmtpServer.Send(mail);
                // insert log
                if (approveFrom == "Application")
                {
                    approveFrom = requestToken.Approver_UserID;
                }
                bool logSuccess = log.InsertLogRequest("Send Email Approve Result", "Complete", requestToken.Request_No, "Approver ID: " + requestToken.Approver_ID + ", Approve By: " + approveFrom + ", Result: Approve to " + toEmail, requestToken.Approver_UserID);
                return true;
            }
            catch (Exception e)
            {
                // insert log
                bool logFail = log.InsertLogRequest("Send Email Approve Result", "Failed", requestToken.Request_No, e.Message, requestToken.Approver_UserID);
                return false;
            }
        }

        public bool SendEmailRejectToRequestUser(RejectReason reject, string rejectFrom)
        {
            try
            {
                DataTable tableRequest = request.getRequestDetail(reject.Request_No);
                DataTable tableUser = request.getRequestDetailUser(reject.Request_No);
                DataTable tableMediaType = request.getRequestDetailMediaType(reject.Request_No);
                DataTable tableClient = request.getRequestPermissionByApprover(reject.Request_No, reject.Approver_ID);
                string reason = (reject.Reject_Reason == null || reject.Reject_Reason == "") ? reject.Reject_Detail : reject.Reject_Reason;
                string toEmail = tableRequest.Rows[0]["Request_User_Email"].ToString();

                // test -------------------------------------------------
                string ccEmail = "Chanchai.Liangboonprakong@groupm.com";
                // test -------------------------------------------------

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("relay.nlbint.insidemedia.net");
                SmtpServer.Port = 25;
                mail.From = new MailAddress("GRMThailandITTeam@ad.insidemedia.net");
                mail.To.Add(toEmail);

                // test --------------
                mail.CC.Add(ccEmail);
                // test --------------

                mail.Subject = "Permission Access (Minder) - Request No: " + reject.Request_No + " has been approved.";
                mail.IsBodyHtml = true;
                string body = "<p style = \"font-size: 24px\">รายการสิทธิ์ในการเข้าถึงข้อมูลลูกค้าที่ได้รับการปฏิเสธ</p>" +
                "<p>" +
                "<span style = \"font-size: 20px; color: blue\">เหตุผลในการปฏิเสธการเข้าใช้งาน:</span>" +
                "<br/>" +
                reason +
                "<br/>" +
                "</p>" +
                "<p>" +
                "<span style = \"font-size: 20px; color: blue\">ผู้ใช้ที่ต้องการขอสิทธิ์ในการเข้าถึงข้อมูล:</span>" +
                "<br/>";
                for (int j = 0; j < tableUser.Rows.Count; j++)
                {
                    body += (j + 1).ToString() + ". " + tableUser.Rows[j]["Username_Agency"].ToString() + "<br/>";
                }
                body += "</p>" +
                "<p>" +
                "<span style = \"font-size: 20px; color: blue\">ประเภท</span> <span style = \"font-size: 16px; color: blue\">Media:</span>" +
                "<br/>";
                for (int j = 0; j < tableMediaType.Rows.Count; j++)
                {
                    body += (j + 1).ToString() + ". " + tableMediaType.Rows[j]["Media_Type_Display_Name"].ToString() + "<br/>";
                }
                body += "</p>" +
                "<p>" +
                "<span style = \"font-size: 20px; color: blue\">สิทธิ์ในการเห็นลูกค้า:</span>" +
                "<br/>";
                for (int j = 0; j < tableClient.Rows.Count; j++)
                {
                    string level = tableClient.Rows[j]["Permission_Level"].ToString();
                    if (level == "Client Level")
                    {
                        body += (j + 1).ToString() + ". " + tableClient.Rows[j]["Permission_Display_Name"].ToString() + "<br/>";
                    }
                    else
                    {
                        body += (j + 1).ToString() + ". <span style = \"font-size: 20px\">ลูกค้าทั้งหมดของ</span> " + tableClient.Rows[j]["Permission_Display_Name"].ToString() + "<br/>";
                    }
                }
                body += "<br/>" +
                "<br/>" +
                "<hr>" +
                "<br/>" +
                "<p style = \"font-size: 20px\">Rejected Clients Permission List</p>" +
                "<p>" +
                "<span style = \"font-size: 16px; color: blue\">Reject Reason</span>" +
                "<br/>" +
                reason +
                "<br/>" +
                "</p>" +
                "<p>" +
                "<span style = \"font-size: 16px; color: blue\">Request Access User(s):</span>" +
                "<br/>";
                for (int j = 0; j < tableUser.Rows.Count; j++)
                {
                    body += (j + 1).ToString() + ". " + tableUser.Rows[j]["Username_Agency"].ToString() + "<br/>";
                }
                body += "</p>" +
                "<p>" +
                "<span style = \"font-size: 16px; color: blue\">Media Type:</span>" +
                "<br/>";
                for (int j = 0; j < tableMediaType.Rows.Count; j++)
                {
                    body += (j + 1).ToString() + ". " + tableMediaType.Rows[j]["Media_Type_Display_Name"].ToString() + "<br/>";
                }
                body += "</p>" +
                "<p>" +
                "<span style = \"font-size: 16px; color: blue\">Request to Access Client:</span>" +
                "<br/>";
                for (int j = 0; j < tableClient.Rows.Count; j++)
                {
                    string level = tableClient.Rows[j]["Permission_Level"].ToString();
                    if (level == "Client Level")
                    {
                        body += (j + 1).ToString() + ". " + tableClient.Rows[j]["Permission_Display_Name"].ToString() + "<br/>";
                    }
                    else
                    {
                        body += (j + 1).ToString() + ". All Client of " + tableClient.Rows[j]["Permission_Display_Name"].ToString() + "<br/>";
                    }
                }
                body += "<br/>" +
                "<br/>" +
                "You can find all the details about this request by <a href = \"" + urlRequest + "\">View Request</a>" +
                "<br/>" +
                "Request No. " + reject.Request_No +
                "<br/>" +
                "Submitted Date " + Convert.ToDateTime(tableRequest.Rows[0]["Create_Date"]).ToString("dd-MMM-yyyy HH:mm");
                mail.Body = body;
                SmtpServer.Send(mail);
                // insert log
                if (rejectFrom == "Application")
                {
                    rejectFrom = reject.Approver_UserID;
                }
                bool logSuccess = log.InsertLogRequest("Send Email Reject Result", "Complete", reject.Request_No, "Approver ID: " + reject.Approver_ID + ", Reject By: " + rejectFrom + ", Result: Reject to " + toEmail, reject.Approver_UserID);
                return true;
            }
            catch (Exception e)
            {
                // insert log
                bool logFail = log.InsertLogRequest("Send Email Reject Result", "Failed", reject.Request_No, e.Message, reject.Approver_UserID);
                return false;
            }
        }
    }
}