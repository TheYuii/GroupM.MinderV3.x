using GroupM.AutoGrant.API.Models;
using GroupM.AutoGrant.API.Models.Json_Data;
using GroupM.AutoGrant.API.Models.Json_Models;
using GroupM.AutoGrant.API.Models.Json_Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace GroupM.AutoGrant.API.Controllers
{
    public class RequestController : ApiController
    {
        private string urlAPI = ConfigurationManager.AppSettings["URLAPI"];
        private RequestData request = new RequestData();
        private ConvertToJson convert = new ConvertToJson();
        private Email email = new Email();
        private Log log = new Log();

        [Route("api/getAllRequest/{userID}/{requestType}/{requestStatus}/{startDate}/{endDate}/{keyWord}")]
        public IHttpActionResult GetAllRequest(string userID, string requestType, string requestStatus, string startDate, string endDate, string keyWord)
        {
            string url = urlAPI + "api/getAllRequest/{userID}/{requestType}/{requestStatus}/{startDate}/{endDate}/{keyWord}";
            string parameter = "userID = (" + userID + "), " +
            "requestType = (" + requestType + "), " +
            "requestStatus = (" + requestStatus + "), " +
            "startDate = (" + startDate + "), " +
            "endDate = (" + endDate + "), " +
            "keyWord = (" + keyWord + ")";
            try
            {
                List<RequestShow> list = request.getRequestShow(userID, requestType, requestStatus, startDate, endDate, keyWord);
                string json = convert.ConvertListtoJsonString(list);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                bool logAPI = log.InsertLogAPI("Get All Request", url, parameter, "Get", "", 200, json);
                return ResponseMessage(response);
            }
            catch (Exception e)
            {
                bool logAPI = log.InsertLogAPI("Get All Request", url, parameter, "Get", "", 400, e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("api/getRequestDetail/{requestNo}")]
        public IHttpActionResult GetRequestDetail(string requestNo)
        {
            string url = urlAPI + "api/getRequestDetail/{requestNo}";
            string parameter = "requestNo = (" + requestNo + ")";
            try
            {
                List<RequestDetail> list = request.getRequestShowDetail(requestNo);
                string json = convert.ConvertListtoJsonString(list);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                bool logAPI = log.InsertLogAPI("Get Request Detail", url, parameter, "Get", "", 200, json);
                return ResponseMessage(response);
            }
            catch (Exception e)
            {
                bool logAPI = log.InsertLogAPI("Get Request Detail", url, parameter, "Get", "", 400, e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("api/createNewRequest")]
        public IHttpActionResult Post([FromBody] CreateRequest data)
        {
            string requestNo = "";
            string url = urlAPI + "api/createNewRequest";
            string parameter = "";
            string convertJson = convert.ConvertObjecttoJsonString(data);
            string body = convertJson == null ? "" : convertJson;
            try
            {
                // insert request
                requestNo = request.insertRequest(data);
                if (requestNo != null && requestNo != "")
                {
                    // insert user
                    bool insertUser = true;
                    for (int i = 0; i < data.listRequestUser.Length; i++)
                    {
                        if (insertUser == true)
                        {
                            insertUser = request.insertRequestUser(requestNo, data.listRequestUser[i]);
                        }
                    }
                    if (insertUser == false)
                    {
                        // insert log
                        bool logFail = log.InsertLogRequest("Submit Request", "Failed", requestNo, "insert request user fail", data.Request_By);
                        bool logAPIFail = log.InsertLogAPI("Create New Request", url, parameter, "Post", body, 400, "insert request user fail");
                        return Content(HttpStatusCode.BadRequest, "insert request user fail!");
                    }
                    // insert media type
                    bool insertMediaType = true;
                    for (int i = 0; i < data.listRequestMediaType.Length; i++)
                    {
                        if (insertMediaType == true)
                        {
                            insertMediaType = request.insertRequestMediaType(requestNo, data.listRequestMediaType[i]);
                        }
                    }
                    if (insertMediaType == false)
                    {
                        // insert log
                        bool logFail = log.InsertLogRequest("Submit Request", "Failed", requestNo, "insert request media type fail", data.Request_By);
                        bool logAPIFail = log.InsertLogAPI("Create New Request", url, parameter, "Post", body, 400, "insert request media type fail");
                        return Content(HttpStatusCode.BadRequest, "insert request media type fail!");
                    }
                    // insert approver
                    bool insertApprover = true;
                    for (int i = 0; i < data.listRequestPermission.Length; i++)
                    {
                        if (insertApprover == true)
                        {
                            string approverID = request.insertRequestApprover(requestNo, data.listRequestPermission[i]);
                            if (approverID != null && approverID != "")
                            {
                                insertApprover = true;
                                // insert token
                                bool insertToken = request.insertRequestToken(requestNo, approverID, data.listRequestPermission[i].Approver_UserID, data.listRequestPermission[i].Delegate_UserID);
                                if (insertToken == false)
                                {
                                    // insert log
                                    bool logFail = log.InsertLogRequest("Submit Request", "Failed", requestNo, "insert request token fail", data.Request_By);
                                    bool logAPIFail = log.InsertLogAPI("Create New Request", url, parameter, "Post", body, 400, "insert request token fail");
                                    return Content(HttpStatusCode.BadRequest, "insert request token fail!");
                                }
                                // insert permission
                                bool insertPermission = true;
                                for (int j = 0; j < data.listRequestPermission[i].Permission_List.Length; j++)
                                {
                                    if (insertPermission == true)
                                    {
                                        insertPermission = request.insertRequestClient(requestNo, approverID, data.listRequestPermission[i].Permission_Level, data.listRequestPermission[i].Permission_List[j]);
                                    }
                                }
                                if (insertPermission == false)
                                {
                                    // insert log
                                    bool logFail = log.InsertLogRequest("Submit Request", "Failed", requestNo, "insert request permission fail", data.Request_By);
                                    bool logAPIFail = log.InsertLogAPI("Create New Request", url, parameter, "Post", body, 400, "insert request permission fail");
                                    return Content(HttpStatusCode.BadRequest, "insert request permission fail!");
                                }
                            }
                            else
                            {
                                insertApprover = false;
                            }
                        }
                    }
                    if (insertApprover == false)
                    {
                        // insert log
                        bool logFail = log.InsertLogRequest("Submit Request", "Failed", requestNo, "insert request approver fail", data.Request_By);
                        bool logAPIFail = log.InsertLogAPI("Create New Request", url, parameter, "Post", body, 400, "insert request approver fail");
                        return Content(HttpStatusCode.BadRequest, "insert request approver fail!");
                    }
                    // insert log
                    bool logSuccess = log.InsertLogRequest("Submit Request", "Complete", requestNo, "Request Type: " + data.Request_Type + ", User ID: " + data.Request_By, data.Request_By);
                    // send email request to approver
                    bool sendEmail = email.SendEmailRequestToApprover(requestNo);
                    if (sendEmail == false)
                    {
                        bool logAPIFail = log.InsertLogAPI("Create New Request", url, parameter, "Post", body, 400, "send email request to approver fail!");
                        return Content(HttpStatusCode.BadRequest, "send email request to approver fail!");
                    }
                    // return json request number
                    string json = "{\"Request_No\":\"" + requestNo + "\"}";
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    bool logAPI = log.InsertLogAPI("Create New Request", url, parameter, "Post", body, 200, json);
                    return ResponseMessage(response);
                }
                else
                {
                    // insert log
                    bool logFail = log.InsertLogRequest("Submit Request", "Failed", "", "insert request fail", data.Request_By);
                    bool logAPI = log.InsertLogAPI("Create New Request", url, parameter, "Post", body, 400, "insert request fail");
                    return Content(HttpStatusCode.BadRequest, "insert request fail!");
                }
            }
            catch (Exception e)
            {
                // insert log
                if (requestNo != null && requestNo != "")
                {
                    bool logFail = log.InsertLogRequest("Submit Request", "Failed", requestNo, e.Message, data.Request_By);
                }
                else
                {
                    bool logFail = log.InsertLogRequest("Submit Request", "Failed", "", e.Message, data.Request_By);
                }
                bool logAPI = log.InsertLogAPI("Create New Request", url, parameter, "Post", body, 400, e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("api/cancelRequest/{userId}")]
        public IHttpActionResult PutCancel(string userID, [FromBody] RequestCancel data)
        {
            string url = urlAPI + "api/cancelRequest/{userId}";
            string parameter = "userId = (" + userID + ")";
            string convertJson = convert.ConvertObjecttoJsonString(data);
            string body = convertJson == null ? "" : convertJson;
            try
            {
                string requestNo = data.Request_No;
                DataTable table = request.updateCancel(data, userID);
                string status = table.Rows[0]["Status"].ToString();
                string result = table.Rows[0]["Cancel"].ToString();
                if (status == "True")
                {
                    // send email cancel to approver
                    bool sendEmail = email.SendEmailCancelToApprover(requestNo);
                    if (sendEmail == false)
                    {
                        bool logAPIFail = log.InsertLogAPI("Cancel Request", url, parameter, "Put", body, 400, "send email cancel to approver fail!");
                        return Content(HttpStatusCode.BadRequest, "send email cancel to approver fail!");
                    }
                }
                string json = "{ \"Cancel_Status\": \"" + status + "\", \"Cancel_Result\": \"" + result + "\" }";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                bool logAPI = log.InsertLogAPI("Cancel Request", url, parameter, "Put", body, 200, json);
                return ResponseMessage(response);
            }
            catch (Exception e)
            {
                bool logAPI = log.InsertLogAPI("Cancel Request", url, parameter, "Put", body, 400, e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("api/RequestApprove/{userId}")]
        public IHttpActionResult PutApprove(string userID, [FromBody] RequestToken data)
        {
            string url = urlAPI + "api/RequestApprove/{userId}";
            string parameter = "userId = (" + userID + ")";
            string convertJson = convert.ConvertObjecttoJsonString(data);
            string body = convertJson == null ? "" : convertJson;
            try
            {
                data.Approver_UserID = userID;
                DataTable table = request.updateApprove(data, "Application");
                string status = table.Rows[0]["Status"].ToString();
                string result = table.Rows[0]["Approve"].ToString();
                if (status == "True")
                {
                    // send email to user create request
                    bool sendEmail = email.SendEmailApproveToRequestUser(data, "Application");
                    if (sendEmail == false)
                    {
                        bool logAPIFail = log.InsertLogAPI("Request Approve", url, parameter, "Put", body, 400, "send email to request user fail!");
                        return Content(HttpStatusCode.BadRequest, "send email to request user fail!");
                    }
                }
                string json = "{ \"Approve_Status\": \"" + status + "\", \"Approve_Result\": \"" + result + "\" }";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                bool logAPI = log.InsertLogAPI("Request Approve", url, parameter, "Put", body, 200, json);
                return ResponseMessage(response);
            }
            catch (Exception e)
            {
                bool logAPI = log.InsertLogAPI("Request Approve", url, parameter, "Put", body, 400, e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("api/RequestReject/{userId}")]
        public IHttpActionResult PutReject(string userID, [FromBody] RejectReason data)
        {
            string url = urlAPI + "api/RequestReject/{userId}";
            string parameter = "userId = (" + userID + ")";
            string convertJson = convert.ConvertObjecttoJsonString(data);
            string body = convertJson == null ? "" : convertJson;
            try
            {
                data.Approver_UserID = userID;
                DataTable table = request.updateReject(data, "Application");
                string status = table.Rows[0]["Status"].ToString();
                string result = table.Rows[0]["Reject"].ToString();
                if (status == "True")
                {
                    // send email to user create request
                    bool sendEmail = email.SendEmailRejectToRequestUser(data, "Application");
                    if (sendEmail == false)
                    {
                        bool logAPIFail = log.InsertLogAPI("Request Reject", url, parameter, "Put", body, 400, "send email to request user fail!");
                        return Content(HttpStatusCode.BadRequest, "send email to request user fail!");
                    }
                }
                string json = "{ \"Reject_Status\": \"" + status + "\", \"Reject_Result\": \"" + result + "\" }";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                bool logAPI = log.InsertLogAPI("Request Reject", url, parameter, "Put", body, 200, json);
                return ResponseMessage(response);
            }
            catch (Exception e)
            {
                bool logAPI = log.InsertLogAPI("Request Reject", url, parameter, "Put", body, 400, e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("api/getApprovalRequestList/{userId}/{requestType}/{requestStatus}/{startDate}/{endDate}/{keyWord}")]
        public IHttpActionResult GetApprovalRequestList(string userID, string requestType, string requestStatus, string startDate, string endDate, string keyWord)
        {
            string url = urlAPI + "api/getApprovalRequestList/{userId}/{requestType}/{requestStatus}/{startDate}/{endDate}/{keyWord}";
            string parameter = "userID = (" + userID + "), " +
            "requestType = (" + requestType + "), " +
            "requestStatus = (" + requestStatus + "), " +
            "startDate = (" + startDate + "), " +
            "endDate = (" + endDate + "), " +
            "keyWord = (" + keyWord + ")";
            try
            {
                List<ApprovalShow> list = request.getApprovalShow(userID, requestType, requestStatus, startDate, endDate, keyWord);
                string json = convert.ConvertListtoJsonString(list);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                bool logAPI = log.InsertLogAPI("Get Approval Request List", url, parameter, "Get", "", 200, json);
                return ResponseMessage(response);
            }
            catch (Exception e)
            {
                bool logAPI = log.InsertLogAPI("Get Approval Request List", url, parameter, "Get", "", 400, e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("api/getApprovalRequestDetail/{requestNo}/{approverId}")]
        public IHttpActionResult GetApprovalRequestDetail(string requestNo, string approverID)
        {
            string url = urlAPI + "api/getApprovalRequestDetail/{requestNo}/{approverId}";
            string parameter = "requestNo = (" + requestNo + "), " +
                "approverId = (" + approverID + ")";
            try
            {
                List<RequestDetail> list = request.getApprovalRequestShowDetail(requestNo, approverID);
                string json = convert.ConvertListtoJsonString(list);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                bool logAPI = log.InsertLogAPI("Get Approval Request Detail", url, parameter, "Get", "", 200, json);
                return ResponseMessage(response);
            }
            catch (Exception e)
            {
                bool logAPI = log.InsertLogAPI("Get Approval Request Detail", url, parameter, "Get", "", 400, e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        // DELETE: api/Request/5
        public void Delete(int id)
        {
        }
    }
}
