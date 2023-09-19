using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GroupM.DBAccess
{
    //==========================================
    // Model
    //==========================================

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

    public class PermissionMediaType
    {
        public string Media_Type_Code { get; set; }
        public string Media_Type_Name { get; set; }
        public string Media_Type_Display_Name { get; set; }
    }

    public class RequestAgency
    {
        public string Agency_ID { get; set; }
        public string Agency_Name { get; set; }
        public string Agency_Display_Name { get; set; }
        public string Approver_UserID { get; set; }
        public string Approver_Name { get; set; }
        public string Approver_Email { get; set; }
        public string Delegate_UserID { get; set; }
    }

    public class RequestOffice
    {
        public string Office_ID { get; set; }
        public string Office_Name { get; set; }
        public string Agency_ID { get; set; }
        public string Agency_Name { get; set; }
        public string Office_Display_Name { get; set; }
        public string Approver_UserID { get; set; }
        public string Approver_Name { get; set; }
        public string Approver_Email { get; set; }
        public string Delegate_UserID { get; set; }
    }

    public class RequestClient
    {
        public string Client_ID { get; set; }
        public string Client_Name { get; set; }
        public string Agency_ID { get; set; }
        public string Agency_Name { get; set; }
        public string Office_ID { get; set; }
        public string Office_Name { get; set; }
        public string Client_Display_Name { get; set; }
        public string Approver_UserID { get; set; }
        public string Approver_Name { get; set; }
        public string Approver_Email { get; set; }
        public string Delegate_UserID { get; set; }
    }

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

    public class CreateRequestUser
    {
        public string User_ID { get; set; }
        public string User_Email { get; set; }
        public string Agency_ID { get; set; }
        public string Office_ID { get; set; }
    }

    public class CreateRequestMediaType
    {
        public string Media_Type { get; set; }
    }

    public class CreateRequest
    {
        public string Request_Type { get; set; }
        public string Request_Status { get; set; }
        public string Request_By { get; set; }
        public CreateRequestUser[] listRequestUser { get; set; }
        public CreateRequestMediaType[] listRequestMediaType { get; set; }
        public CopyPermission[] listRequestPermission { get; set; }
    }

    public class ResponseJson
    {
        public string Request_No { get; set; }
    }

    public class RequestType
    {
        public string Request_Type_Code { get; set; }
        public string Request_Type_Name { get; set; }
        public string Request_Type_Display_Name { get; set; }
        public string Request_Type_Drop_Down { get; set; }
    }

    public class RequestStatus
    {
        public string Request_Status_Code { get; set; }
        public string Request_Status_Name { get; set; }
        public string Request_Status_Display_Name { get; set; }
    }

    public class RequestUser
    {
        public string Username_Agency { get; set; }
    }

    public class RequestMediaType
    {
        public string Media_Type_Name { get; set; }
    }

    public class RequestPermission
    {
        public string Permission_Display_Name { get; set; }
    }

    public class RequestShow
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
        public string Create_Date { get; set; }
        public string Create_By { get; set; }
        public string Update_Date { get; set; }
        public string Update_By { get; set; }
        public RequestUser[] listRequestUser { get; set; }
        public RequestMediaType[] listRequestMediaType { get; set; }
        public RequestPermission[] listRequestPermission { get; set; }
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

    public class RequestCancel
    {
        public string Request_No { get; set; }
        public string Cancel_Reason { get; set; }
    }

    public class ResponseJsonCancel
    {
        public string Cancel_Status { get; set; }
        public string Cancel_Result { get; set; }
    }

    public class ApprovalShow
    {
        public string Approver_ID { get; set; }
        public string Request_No { get; set; }
        public string Request_Type { get; set; }
        public string Request_Type_Display { get; set; }
        public string Request_Status { get; set; }
        public string Request_Status_Display { get; set; }
        public string Complete_Date { get; set; }
        public string Expired_Date { get; set; }
        public string Approve_Result { get; set; }
        public string Approve_Result_Display { get; set; }
        public string Total_Approver { get; set; }
        public string Total_Approved { get; set; }
        public string Reject_Reason { get; set; }
        public string Create_Date { get; set; }
        public string Create_By { get; set; }
        public string Update_Date { get; set; }
        public string Update_By { get; set; }
        public RequestUser[] listRequestUser { get; set; }
        public RequestMediaType[] listRequestMediaType { get; set; }
        public RequestPermission[] listRequestPermission { get; set; }
    }

    public class RequestApprove
    {
        public string Request_No { get; set; }
        public string Approver_ID { get; set; }
        public string Approver_UserID { get; set; }
        public string Delegate_UserID { get; set; }
        public string Approve_Result { get; set; }
    }

    public class ResponseJsonApprove
    {
        public string Approve_Status { get; set; }
        public string Approve_Result { get; set; }
    }

    public class RequestReject
    {
        public string Request_No { get; set; }
        public string Approver_ID { get; set; }
        public string Reject_Reason { get; set; }
        public string Reject_Detail { get; set; }
        public string Approver_UserID { get; set; }
        public string Delegate_UserID { get; set; }
        public string Approve_Result { get; set; }
    }

    public class ResponseJsonReject
    {
        public string Reject_Status { get; set; }
        public string Reject_Result { get; set; }
    }

    //==========================================
    // Service
    //==========================================

    public class APIManager
    {
        private string urlAPI = ConfigurationManager.AppSettings["URLAPI"];

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        public DataTable APIGetRequestMe(string userID)
        {
            RestRequest request = new RestRequest("getRequestMe/{userID}", Method.GET);
            request.Timeout = 0;
            request.AddUrlSegment("userID", userID);
            // response
            RestClient client = new RestClient(urlAPI);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // convert json to object
                string responseJson = response.Content;
                List<RequestDetailUser> UserList = JsonConvert.DeserializeObject<List<RequestDetailUser>>(responseJson);
                DataTable dt = ConvertToDataTable<RequestDetailUser>(UserList);
                return dt;
            }
            else
            {
                return null;
            }
        }

        public DataTable APIGetRequestUser(string userID)
        {
            RestRequest request = new RestRequest("getRequestUser/{userID}", Method.GET);
            request.Timeout = 0;
            request.AddUrlSegment("userID", userID);
            // response
            RestClient client = new RestClient(urlAPI);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // convert json to object
                string responseJson = response.Content;
                List<RequestDetailUser> UserList = JsonConvert.DeserializeObject<List<RequestDetailUser>>(responseJson);
                DataTable dt = ConvertToDataTable<RequestDetailUser>(UserList);
                return dt;
            }
            else
            {
                return null;
            }
        }

        public DataTable APIGetMediaTypePermissionByUser(string userID)
        {
            RestRequest request = new RestRequest("getMediaTypePermissionByUser/{userID}", Method.GET);
            request.Timeout = 0;
            request.AddUrlSegment("userID", userID);
            // response
            RestClient client = new RestClient(urlAPI);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // convert json to object
                string responseJson = response.Content;
                List<PermissionMediaType> listPermission = JsonConvert.DeserializeObject<List<PermissionMediaType>>(responseJson);
                DataTable dt = ConvertToDataTable<PermissionMediaType>(listPermission);
                return dt;
            }
            else
            {
                return null;
            }
        }

        public DataTable APIGetAllAgency()
        {
            RestRequest request = new RestRequest("getAllAgency", Method.GET);
            request.Timeout = 0;
            // response
            RestClient client = new RestClient(urlAPI);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // convert json to object
                string responseJson = response.Content;
                List<RequestAgency> AgencyList = JsonConvert.DeserializeObject<List<RequestAgency>>(responseJson);
                DataTable dt = ConvertToDataTable<RequestAgency>(AgencyList);
                return dt;
            }
            else
            {
                return null;
            }
        }

        public DataTable APIGetAllOffice()
        {
            RestRequest request = new RestRequest("getAllOffice", Method.GET);
            request.Timeout = 0;
            // response
            RestClient client = new RestClient(urlAPI);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // convert json to object
                string responseJson = response.Content;
                List<RequestOffice> OfficeList = JsonConvert.DeserializeObject<List<RequestOffice>>(responseJson);
                DataTable dt = ConvertToDataTable<RequestOffice>(OfficeList);
                return dt;
            }
            else
            {
                return null;
            }
        }

        public DataTable APIGetAllClient()
        {
            RestRequest request = new RestRequest("getAllClient", Method.GET);
            request.Timeout = 0;
            // response
            RestClient client = new RestClient(urlAPI);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // convert json to object
                string responseJson = response.Content;
                List<RequestClient> ClientList = JsonConvert.DeserializeObject<List<RequestClient>>(responseJson);
                DataTable dt = ConvertToDataTable<RequestClient>(ClientList);
                return dt;
            }
            else
            {
                return null;
            }
        }

        public List<CopyPermission> APIGetClientPermissionByUser(string userID)
        {
            RestRequest request = new RestRequest("getClientPermissionByUser/{userID}", Method.GET);
            request.Timeout = 0;
            request.AddUrlSegment("userID", userID);
            // response
            RestClient client = new RestClient(urlAPI);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // convert json to object
                string responseJson = response.Content;
                List<CopyPermission> listPermission = JsonConvert.DeserializeObject<List<CopyPermission>>(responseJson);
                return listPermission;
            }
            else
            {
                return null;
            }
        }

        public string APICreateNewRequest(string jsonData)
        {
            RestRequest request = new RestRequest("createNewRequest", Method.POST);
            request.Timeout = 0;
            request.AddJsonBody(jsonData);
            // response
            RestClient client = new RestClient(urlAPI);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // convert json to object
                string responseJson = response.Content;
                ResponseJson responseJsonObject = JsonConvert.DeserializeObject<ResponseJson>(responseJson);
                string requestNo = responseJsonObject.Request_No;
                return requestNo;
            }
            else
            {
                return "Call API Unsuccessful.";
            }
        }

        public DataTable APIGetAllRequestType()
        {
            RestRequest request = new RestRequest("getAllRequestType", Method.GET);
            request.Timeout = 0;
            // response
            RestClient client = new RestClient(urlAPI);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // convert json to object
                string responseJson = response.Content;
                List<RequestType> RequestTypeList = JsonConvert.DeserializeObject<List<RequestType>>(responseJson);
                DataTable dt = ConvertToDataTable<RequestType>(RequestTypeList);
                return dt;
            }
            else
            {
                return null;
            }
        }

        public DataTable APIGetAllRequestStatus()
        {
            RestRequest request = new RestRequest("getAllRequestStatus", Method.GET);
            request.Timeout = 0;
            // response
            RestClient client = new RestClient(urlAPI);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // convert json to object
                string responseJson = response.Content;
                List<RequestStatus> RequestStatusList = JsonConvert.DeserializeObject<List<RequestStatus>>(responseJson);
                DataTable dt = ConvertToDataTable<RequestStatus>(RequestStatusList);
                return dt;
            }
            else
            {
                return null;
            }
        }

        public List<RequestShow> APIGetAllRequest(string userID, string type, string status, string startDate, string endDate, string keyWord)
        {
            RestRequest request = new RestRequest("getAllRequest/{userID}/{requestType}/{requestStatus}/{startDate}/{endDate}/{keyWord}", Method.GET);
            request.Timeout = 0;
            request.AddUrlSegment("userID", userID);
            request.AddUrlSegment("requestType", type);
            request.AddUrlSegment("requestStatus", status);
            request.AddUrlSegment("startDate", startDate);
            request.AddUrlSegment("endDate", endDate);
            request.AddUrlSegment("keyWord", keyWord);
            // response
            RestClient client = new RestClient(urlAPI);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // convert json to object
                string responseJson = response.Content;
                List<RequestShow> listRequest = JsonConvert.DeserializeObject<List<RequestShow>>(responseJson);
                return listRequest;
            }
            else
            {
                return null;
            }
        }

        public List<RequestDetail> APIGetRequestDetail(string requestNo)
        {
            RestRequest request = new RestRequest("getRequestDetail/{requestNo}", Method.GET);
            request.Timeout = 0;
            request.AddUrlSegment("requestNo", requestNo);
            // response
            RestClient client = new RestClient(urlAPI);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // convert json to object
                string responseJson = response.Content;
                List<RequestDetail> listRequestDetail = JsonConvert.DeserializeObject<List<RequestDetail>>(responseJson);
                return listRequestDetail;
            }
            else
            {
                return null;
            }
        }

        public string APICancelRequest(string userID, string jsonData)
        {
            RestRequest request = new RestRequest("cancelRequest/{userID}", Method.PUT);
            request.Timeout = 0;
            request.AddUrlSegment("userID", userID);
            request.AddJsonBody(jsonData);
            // response
            RestClient client = new RestClient(urlAPI);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // convert json to object
                string responseJson = response.Content;
                ResponseJsonCancel responseJsonObject = JsonConvert.DeserializeObject<ResponseJsonCancel>(responseJson);
                string result = responseJsonObject.Cancel_Result;
                return result;
            }
            else
            {
                return "Call API Unsuccessful.";
            }
        }

        public List<ApprovalShow> APIGetApprovalRequestList(string userID, string type, string status, string startDate, string endDate, string keyWord)
        {
            RestRequest request = new RestRequest("getApprovalRequestList/{userID}/{requestType}/{requestStatus}/{startDate}/{endDate}/{keyWord}", Method.GET);
            request.Timeout = 0;
            request.AddUrlSegment("userID", userID);
            request.AddUrlSegment("requestType", type);
            request.AddUrlSegment("requestStatus", status);
            request.AddUrlSegment("startDate", startDate);
            request.AddUrlSegment("endDate", endDate);
            request.AddUrlSegment("keyWord", keyWord);
            // response
            RestClient client = new RestClient(urlAPI);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // convert json to object
                string responseJson = response.Content;
                List<ApprovalShow> listApprove = JsonConvert.DeserializeObject<List<ApprovalShow>>(responseJson);
                return listApprove;
            }
            else
            {
                return null;
            }
        }

        public List<RequestDetail> APIGetApprovalRequestDetail(string requestNo, string approverID)
        {
            RestRequest request = new RestRequest("getApprovalRequestDetail/{requestNo}/{approverID}", Method.GET);
            request.Timeout = 0;
            request.AddUrlSegment("requestNo", requestNo);
            request.AddUrlSegment("approverID", approverID);
            // response
            RestClient client = new RestClient(urlAPI);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // convert json to object
                string responseJson = response.Content;
                List<RequestDetail> listRequestDetail = JsonConvert.DeserializeObject<List<RequestDetail>>(responseJson);
                return listRequestDetail;
            }
            else
            {
                return null;
            }
        }

        public string APIRequestApprove(string userID, string jsonData)
        {
            RestRequest request = new RestRequest("RequestApprove/{userID}", Method.PUT);
            request.Timeout = 0;
            request.AddUrlSegment("userID", userID);
            request.AddJsonBody(jsonData);
            // response
            RestClient client = new RestClient(urlAPI);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // convert json to object
                string responseJson = response.Content;
                ResponseJsonApprove responseJsonObject = JsonConvert.DeserializeObject<ResponseJsonApprove>(responseJson);
                string result = responseJsonObject.Approve_Result;
                return result;
            }
            else
            {
                return "Call API Unsuccessful.";
            }
        }

        public string APIRequestReject(string userID, string jsonData)
        {
            RestRequest request = new RestRequest("RequestReject/{userID}", Method.PUT);
            request.Timeout = 0;
            request.AddUrlSegment("userID", userID);
            request.AddJsonBody(jsonData);
            // response
            RestClient client = new RestClient(urlAPI);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // convert json to object
                string responseJson = response.Content;
                ResponseJsonReject responseJsonObject = JsonConvert.DeserializeObject<ResponseJsonReject>(responseJson);
                string result = responseJsonObject.Reject_Result;
                return result;
            }
            else
            {
                return "Call API Unsuccessful.";
            }
        }
    }
}
