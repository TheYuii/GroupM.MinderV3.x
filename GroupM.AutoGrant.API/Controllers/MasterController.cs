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
    public class MasterController : ApiController
    {
        private string urlAPI = ConfigurationManager.AppSettings["URLAPI"];
        private MasterData master = new MasterData();
        private ConvertToJson convert = new ConvertToJson();
        private Log log = new Log();

        [Route("api/getAllRequestType")]
        public IHttpActionResult GetAllRequestType()
        {
            try
            {
                DataTable table = master.getRequestType();
                string json = convert.ConvertDataTabletoJsonString(table);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                bool logAPI = log.InsertLogAPI("Get All Request Type", urlAPI + "api/getAllRequestType", "", "Get", "", 200, json);
                return ResponseMessage(response);
            }
            catch (Exception e)
            {
                bool logAPI = log.InsertLogAPI("Get All Request Type", urlAPI + "api/getAllRequestType", "", "Get", "", 400, e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("api/getAllRequestStatus")]
        public IHttpActionResult GetAllRequestStatus()
        {
            try
            {
                DataTable table = master.getRequestStatus();
                string json = convert.ConvertDataTabletoJsonString(table);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                bool logAPI = log.InsertLogAPI("Get All Request Status", urlAPI + "api/getAllRequestStatus", "", "Get", "", 200, json);
                return ResponseMessage(response);
            }
            catch (Exception e)
            {
                bool logAPI = log.InsertLogAPI("Get All Request Status", urlAPI + "api/getAllRequestStatus", "", "Get", "", 400, e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("api/getRequestMe/{userId}")]
        public IHttpActionResult GetRequestMe(string userID)
        {
            try
            {
                DataTable table = master.getMe(userID);
                string json = convert.ConvertDataTabletoJsonString(table);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                bool logAPI = log.InsertLogAPI("Get Request Me", urlAPI + "api/getRequestMe/{userId}", "userId = (" + userID + ")", "Get", "", 200, json);
                return ResponseMessage(response);
            }
            catch (Exception e)
            {
                bool logAPI = log.InsertLogAPI("Get Request Me", urlAPI + "api/getRequestMe/{userId}", "userId = (" + userID + ")", "Get", "", 400, e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("api/getRequestUser")]
        public IHttpActionResult GetRequestUser()
        {
            try
            {
                DataTable table = master.getUser("");
                string json = convert.ConvertDataTabletoJsonString(table);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                bool logAPI = log.InsertLogAPI("Get Request User", urlAPI + "api/getRequestUser", "", "Get", "", 200, json);
                return ResponseMessage(response);
            }
            catch (Exception e)
            {
                bool logAPI = log.InsertLogAPI("Get Request User", urlAPI + "api/getRequestUser", "", "Get", "", 400, e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("api/getRequestUser/{userId}")]
        public IHttpActionResult GetRequestUser(string userID)
        {
            try
            {
                DataTable table = master.getUser(userID);
                string json = convert.ConvertDataTabletoJsonString(table);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                bool logAPI = log.InsertLogAPI("Get Request User", urlAPI + "api/getRequestUser/{userId}", "userId = (" + userID + ")", "Get", "", 200, json);
                return ResponseMessage(response);
            }
            catch (Exception e)
            {
                bool logAPI = log.InsertLogAPI("Get Request User", urlAPI + "api/getRequestUser/{userId}", "userId = (" + userID + ")", "Get", "", 400, e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("api/getAllMediaType")]
        public IHttpActionResult GetAllMediaType()
        {
            try
            {
                DataTable table = master.getMediaType();
                string json = convert.ConvertDataTabletoJsonString(table);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                bool logAPI = log.InsertLogAPI("Get All Media Type", urlAPI + "api/getAllMediaType", "", "Get", "", 200, json);
                return ResponseMessage(response);
            }
            catch (Exception e)
            {
                bool logAPI = log.InsertLogAPI("Get All Media Type", urlAPI + "api/getAllMediaType", "", "Get", "", 400, e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("api/getMediaTypePermissionByUser/{userId}")]
        public IHttpActionResult GetMediaTypePermissionByUser(string userID)
        {
            try
            {
                DataTable table = master.getMediaTypePermission(userID);
                string json = convert.ConvertDataTabletoJsonString(table);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                bool logAPI = log.InsertLogAPI("Get Media Type Permission By User", urlAPI + "api/getMediaTypePermissionByUser/{userId}", "userId = (" + userID + ")", "Get", "", 200, json);
                return ResponseMessage(response);
            }
            catch (Exception e)
            {
                bool logAPI = log.InsertLogAPI("Get Media Type Permission By User", urlAPI + "api/getMediaTypePermissionByUser/{userId}", "userId = (" + userID + ")", "Get", "", 400, e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("api/getAllAgency")]
        public IHttpActionResult GetAllAgency()
        {
            try
            {
                DataTable table = master.getAgency();
                string json = convert.ConvertDataTabletoJsonString(table);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                bool logAPI = log.InsertLogAPI("Get All Agency", urlAPI + "api/getAllAgency", "", "Get", "", 200, json);
                return ResponseMessage(response);
            }
            catch (Exception e)
            {
                bool logAPI = log.InsertLogAPI("Get All Agency", urlAPI + "api/getAllAgency", "", "Get", "", 400, e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("api/getAllOffice")]
        public IHttpActionResult GetAllOffice()
        {
            try
            {
                DataTable table = master.getOffice();
                string json = convert.ConvertDataTabletoJsonString(table);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                bool logAPI = log.InsertLogAPI("Get All Office", urlAPI + "api/getAllOffice", "", "Get", "", 200, json);
                return ResponseMessage(response);
            }
            catch (Exception e)
            {
                bool logAPI = log.InsertLogAPI("Get All Office", urlAPI + "api/getAllOffice", "", "Get", "", 400, e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("api/getAllClient")]
        public IHttpActionResult GetAllClient()
        {
            try
            {
                DataTable table = master.getClient();
                string json = convert.ConvertDataTabletoJsonString(table);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                bool logAPI = log.InsertLogAPI("Get All Client", urlAPI + "api/getAllClient", "", "Get", "", 200, json);
                return ResponseMessage(response);
            }
            catch (Exception e)
            {
                bool logAPI = log.InsertLogAPI("Get All Client", urlAPI + "api/getAllClient", "", "Get", "", 400, e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }

        [Route("api/getClientPermissionByUser/{userId}")]
        public IHttpActionResult GetClientPermissionByUser(string userID)
        {
            try
            {
                List<CopyPermission> listPermissionAgencyOffice = master.getCopyPermissionLevelAgencyOffice(userID);
                List<CopyPermission> listPermissionClient = master.getCopyPermissionLevelClient(userID);
                List<CopyPermission> list = new List<CopyPermission>();
                list.AddRange(listPermissionAgencyOffice);
                list.AddRange(listPermissionClient);
                string json = convert.ConvertListtoJsonString(list);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(json, Encoding.UTF8, "application/json");
                bool logAPI = log.InsertLogAPI("Get Client Permission By User", urlAPI + "api/getClientPermissionByUser/{userId}", "userId = (" + userID + ")", "Get", "", 200, json);
                return ResponseMessage(response);
            }
            catch (Exception e)
            {
                bool logAPI = log.InsertLogAPI("Get Client Permission By User", urlAPI + "api/getClientPermissionByUser/{userId}", "userId = (" + userID + ")", "Get", "", 400, e.Message);
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}
