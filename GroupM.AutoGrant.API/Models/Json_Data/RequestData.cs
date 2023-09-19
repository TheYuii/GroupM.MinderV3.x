using GroupM.AutoGrant.API.Models.Json_Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GroupM.AutoGrant.API.Models.Json_Data
{
    public class RequestData
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["WorkFlow"].ToString();
        private string passwordEncrypt = ConfigurationManager.AppSettings["PEAPI"];

        public string insertRequest(CreateRequest request)
        {
            try
            {
                string requestNo = "";
                DataTable table = new DataTable();
                string query = "createRequest_Request";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
                adapter.SelectCommand.Parameters.Add("@Request_Type", SqlDbType.VarChar).Value = request.Request_Type;
                adapter.SelectCommand.Parameters.Add("@Request_Status", SqlDbType.VarChar).Value = request.Request_Status;
                adapter.SelectCommand.Parameters.Add("@Total_Approver", SqlDbType.Int).Value = request.listRequestPermission.Length;
                adapter.SelectCommand.Parameters.Add("@Request_By", SqlDbType.VarChar).Value = request.Request_By;
                connection.Open();
                adapter.Fill(table);
                connection.Close();
                requestNo = table.Rows[0]["Request_No"].ToString();
                return requestNo;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool insertRequestUser(string requestNo, CreateRequestUser user)
        {
            try
            {
                string query = @"createRequest_User";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 0;
                command.Parameters.Add("@Request_No", SqlDbType.VarChar).Value = requestNo;
                command.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = user.User_ID;
                command.Parameters.Add("@User_Email", SqlDbType.VarChar).Value = user.User_Email;
                command.Parameters.Add("@Agency_ID", SqlDbType.VarChar).Value = user.Agency_ID;
                command.Parameters.Add("@Office_ID", SqlDbType.VarChar).Value = user.Office_ID;
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool insertRequestMediaType(string requestNo, CreateRequestMediaType mediaType)
        {
            try
            {
                string query = @"createRequest_MediaType";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 0;
                command.Parameters.Add("@Request_No", SqlDbType.VarChar).Value = requestNo;
                command.Parameters.Add("@Media_Type", SqlDbType.VarChar).Value = mediaType.Media_Type;
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public string insertRequestApprover(string requestNo, CopyPermission approver)
        {
            try
            {
                string approverID = "";
                DataTable table = new DataTable();
                string query = @"createRequest_Approver";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
                adapter.SelectCommand.Parameters.Add("@Request_No", SqlDbType.VarChar).Value = requestNo;
                adapter.SelectCommand.Parameters.Add("@Approver_UserID", SqlDbType.VarChar).Value = approver.Approver_UserID;
                adapter.SelectCommand.Parameters.Add("@Approver_Name", SqlDbType.VarChar).Value = approver.Approver_Name;
                adapter.SelectCommand.Parameters.Add("@Approver_Email", SqlDbType.VarChar).Value = approver.Approver_Email;
                adapter.SelectCommand.Parameters.Add("@Delegate_UserID", SqlDbType.VarChar).Value = approver.Delegate_UserID == null ? "" : approver.Delegate_UserID;
                connection.Open();
                adapter.Fill(table);
                connection.Close();
                approverID = table.Rows[0]["Approver_ID"].ToString();
                return approverID;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool insertRequestClient(string requestNo, string approverID, string level, Permission permission)
        {
            try
            {
                string query = @"createRequest_Client";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 0;
                command.Parameters.Add("@Request_No", SqlDbType.VarChar).Value = requestNo;
                command.Parameters.Add("@Approver_ID", SqlDbType.VarChar).Value = approverID;
                command.Parameters.Add("@Agency_ID", SqlDbType.VarChar).Value = level == "Agency Level" ? permission.Permission_Code : "";
                command.Parameters.Add("@Office_ID", SqlDbType.VarChar).Value = level == "Office Level" ? permission.Permission_Code : "";
                command.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = level == "Client Level" ? permission.Permission_Code : "";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool insertRequestToken(string requestNo, string approverID, string approverUser, string delegateUser)
        {
            try
            {
                string jsonApprove = "{ \"Request_No\": \"" + requestNo + "\", \"Approver_ID\": \"" + approverID + "\", \"Approver_UserID\": \"" + approverUser + "\", \"Delegate_UserID\": \"" + delegateUser + "\", \"Approve_Result\": \"AP\" }";
                string jsonReject = "{ \"Request_No\": \"" + requestNo + "\", \"Approver_ID\": \"" + approverID + "\", \"Approver_UserID\": \"" + approverUser + "\", \"Delegate_UserID\": \"" + delegateUser + "\", \"Approve_Result\": \"RJ\" }";
                string tokenApprove = Crypt.Encrypt(jsonApprove, passwordEncrypt);
                string tokenReject = Crypt.Encrypt(jsonReject, passwordEncrypt);
                DataTable table = new DataTable();
                string query = @"createRequest_Token";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
                adapter.SelectCommand.Parameters.Add("@Request_No", SqlDbType.VarChar).Value = requestNo;
                adapter.SelectCommand.Parameters.Add("@Approver_ID", SqlDbType.VarChar).Value = approverID;
                adapter.SelectCommand.Parameters.Add("@Approve_Token", SqlDbType.VarChar).Value = tokenApprove;
                adapter.SelectCommand.Parameters.Add("@Reject_Token", SqlDbType.VarChar).Value = tokenReject;
                connection.Open();
                adapter.Fill(table);
                connection.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public DataTable getRequest(string userID, string type, string status, string startDate, string endDate, string keyWord)
        {
            try
            {
                DataTable table = new DataTable();
                string query = "getAllRequest_Request";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
                adapter.SelectCommand.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = userID;
                adapter.SelectCommand.Parameters.Add("@Request_Type", SqlDbType.VarChar).Value = type == "null" ? "" : type;
                adapter.SelectCommand.Parameters.Add("@Request_Status", SqlDbType.VarChar).Value = status == "null" ? "" : status;
                adapter.SelectCommand.Parameters.Add("@StartDate", SqlDbType.VarChar).Value = startDate == "null" || endDate == "null" ? "" : startDate;
                adapter.SelectCommand.Parameters.Add("@EndDate", SqlDbType.VarChar).Value = startDate == "null" || endDate == "null" ? "" : endDate;
                adapter.SelectCommand.Parameters.Add("@KeyWord", SqlDbType.VarChar).Value = keyWord == "null" ? "" : keyWord;
                connection.Open();
                adapter.Fill(table);
                connection.Close();
                return table;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public DataTable getRequestDetailUser(string requestNo)
        {
            try
            {
                DataTable table = new DataTable();
                string query = "getRequestDetail_User";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
                adapter.SelectCommand.Parameters.Add("@Request_No", SqlDbType.VarChar).Value = requestNo;
                connection.Open();
                adapter.Fill(table);
                connection.Close();
                return table;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public DataTable getRequestDetailMediaType(string requestNo)
        {
            try
            {
                DataTable table = new DataTable();
                string query = "getRequestDetail_MediaType";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
                adapter.SelectCommand.Parameters.Add("@Request_No", SqlDbType.VarChar).Value = requestNo;
                connection.Open();
                adapter.Fill(table);
                connection.Close();
                return table;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public DataTable getRequestPermission(string requestNo)
        {
            try
            {
                DataTable table = new DataTable();
                string query = "getAllRequest_Permission";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
                adapter.SelectCommand.Parameters.Add("@Request_No", SqlDbType.VarChar).Value = requestNo;
                connection.Open();
                adapter.Fill(table);
                connection.Close();
                return table;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<RequestShow> getRequestShow(string userID, string type, string status, string startDate, string endDate, string keyWord)
        {
            try
            {
                DataTable tableRequest = getRequest(userID, type, status, startDate, endDate, keyWord);
                List<RequestShow> list = new List<RequestShow>();
                for (int i = 0; i < tableRequest.Rows.Count; i++)
                {
                    RequestShow requestShow = new RequestShow();
                    requestShow.Request_No = tableRequest.Rows[i]["Request_No"].ToString();
                    requestShow.Request_Type = tableRequest.Rows[i]["Request_Type"].ToString();
                    requestShow.Request_Type_Display = tableRequest.Rows[i]["Request_Type_Display"].ToString();
                    requestShow.Request_Status = tableRequest.Rows[i]["Request_Status"].ToString();
                    requestShow.Request_Status_Display = tableRequest.Rows[i]["Request_Status_Display"].ToString();
                    requestShow.Complete_Date = tableRequest.Rows[i]["Complete_Date"].ToString() == "" ? null : Convert.ToDateTime(tableRequest.Rows[i]["Complete_Date"]).ToString("dd/MM/yyyy");
                    requestShow.Expired_Date = Convert.ToDateTime(tableRequest.Rows[i]["Expired_Date"]).ToString("dd/MM/yyyy");
                    requestShow.Total_Approver = tableRequest.Rows[i]["Total_Approver"].ToString();
                    requestShow.Total_Approved = tableRequest.Rows[i]["Total_Approved"].ToString();
                    requestShow.Create_Date = Convert.ToDateTime(tableRequest.Rows[i]["Create_Date"]).ToString("dd/MM/yyyy");
                    requestShow.Create_By = tableRequest.Rows[i]["Create_By"].ToString();
                    requestShow.Update_Date = tableRequest.Rows[i]["Update_Date"].ToString() == "" ? null : Convert.ToDateTime(tableRequest.Rows[i]["Update_Date"]).ToString("dd/MM/yyyy");
                    requestShow.Update_By = tableRequest.Rows[i]["Update_By"].ToString() == "" ? null : tableRequest.Rows[i]["Update_By"].ToString();
                    List<RequestUser> listUser = new List<RequestUser>();
                    DataTable tableUser = getRequestDetailUser(requestShow.Request_No);
                    for (int j = 0; j < tableUser.Rows.Count; j++)
                    {
                        RequestUser requestUser = new RequestUser();
                        requestUser.Username_Agency = tableUser.Rows[j]["Username_Agency"].ToString();
                        listUser.Add(requestUser);
                    }
                    requestShow.listRequestUser = listUser.ToArray();
                    List<RequestMediaType> listMediaType = new List<RequestMediaType>();
                    DataTable tableMediaType = getRequestDetailMediaType(requestShow.Request_No);
                    for (int j = 0; j < tableMediaType.Rows.Count; j++)
                    {
                        RequestMediaType requestMediaType = new RequestMediaType();
                        requestMediaType.Media_Type_Name = tableMediaType.Rows[j]["Media_Type_Name"].ToString();
                        listMediaType.Add(requestMediaType);
                    }
                    requestShow.listRequestMediaType = listMediaType.ToArray();
                    List<RequestPermission> listPermission = new List<RequestPermission>();
                    DataTable tablePermission = getRequestPermission(requestShow.Request_No);
                    for (int j = 0; j < tablePermission.Rows.Count; j++)
                    {
                        RequestPermission requestPermission = new RequestPermission();
                        requestPermission.Permission_Display_Name = tablePermission.Rows[j]["Permission_Display_Name"].ToString();
                        listPermission.Add(requestPermission);
                    }
                    requestShow.listRequestPermission = listPermission.ToArray();
                    list.Add(requestShow);
                }
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public DataTable getRequestPermissionByApprover(string requestNo, string approveID)
        {
            try
            {
                DataTable table = new DataTable();
                string query = "getRequestPermissionByApprover";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
                adapter.SelectCommand.Parameters.Add("@Request_No", SqlDbType.VarChar).Value = requestNo;
                adapter.SelectCommand.Parameters.Add("@Approver_ID", SqlDbType.VarChar).Value = approveID;
                connection.Open();
                adapter.Fill(table);
                connection.Close();
                return table;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public DataTable getRequestDetail(string requestNo)
        {
            try
            {
                DataTable table = new DataTable();
                string query = "getRequestDetail_Request";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
                adapter.SelectCommand.Parameters.Add("@Request_No", SqlDbType.VarChar).Value = requestNo;
                connection.Open();
                adapter.Fill(table);
                connection.Close();
                return table;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public DataTable getRequestDetailApprover(string requestNo)
        {
            try
            {
                DataTable table = new DataTable();
                string query = "getRequestDetail_Approver";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
                adapter.SelectCommand.Parameters.Add("@Request_No", SqlDbType.VarChar).Value = requestNo;
                connection.Open();
                adapter.Fill(table);
                connection.Close();
                return table;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public DataTable getRequestDetailAgencyOfficePermission(string requestNo, string approveID)
        {
            try
            {
                DataTable table = new DataTable();
                string query = "getRequestDetail_Permission_LevelAgencyOffice";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
                adapter.SelectCommand.Parameters.Add("@Request_No", SqlDbType.VarChar).Value = requestNo;
                adapter.SelectCommand.Parameters.Add("@Approver_ID", SqlDbType.VarChar).Value = approveID;
                connection.Open();
                adapter.Fill(table);
                connection.Close();
                return table;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public DataTable getRequestDetailClientPermissionGroupOffice(string requestNo, string approveID)
        {
            try
            {
                DataTable table = new DataTable();
                string query = "getRequestDetail_Permission_LevelClient_Office";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
                adapter.SelectCommand.Parameters.Add("@Request_No", SqlDbType.VarChar).Value = requestNo;
                adapter.SelectCommand.Parameters.Add("@Approver_ID", SqlDbType.VarChar).Value = approveID;
                connection.Open();
                adapter.Fill(table);
                connection.Close();
                return table;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public DataTable getRequestDetailClientPermissionClientList(string requestNo, string approveID, string officeID)
        {
            try
            {
                DataTable table = new DataTable();
                string query = "getRequestDetail_Permission_LevelClient_Client";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
                adapter.SelectCommand.Parameters.Add("@Request_No", SqlDbType.VarChar).Value = requestNo;
                adapter.SelectCommand.Parameters.Add("@Approver_ID", SqlDbType.VarChar).Value = approveID;
                adapter.SelectCommand.Parameters.Add("@Office_ID", SqlDbType.VarChar).Value = officeID;
                connection.Open();
                adapter.Fill(table);
                connection.Close();
                return table;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<RequestDetailPermission> getRequestDetailPermissionLevelAgencyOffice(string requestNo, string approveID)
        {
            try
            {
                DataTable table = getRequestDetailAgencyOfficePermission(requestNo, approveID);
                List<RequestDetailPermission> list = new List<RequestDetailPermission>();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    RequestDetailPermission requestDetailPermission = new RequestDetailPermission();
                    requestDetailPermission.Permission_Level = table.Rows[i]["Permission_Level"].ToString();
                    requestDetailPermission.Agency_ID = table.Rows[i]["Agency_ID"].ToString();
                    requestDetailPermission.Agency_Name = table.Rows[i]["Agency_Name"].ToString();
                    if (requestDetailPermission.Permission_Level == "Office Level")
                    {
                        requestDetailPermission.Office_ID = table.Rows[i]["Office_ID"].ToString();
                        requestDetailPermission.Office_Name = table.Rows[i]["Office_Name"].ToString();
                        requestDetailPermission.Delegate_UserID = table.Rows[i]["Delegate_UserID"].ToString();
                    }
                    requestDetailPermission.Approver_UserID = table.Rows[i]["Approver_UserID"].ToString();
                    requestDetailPermission.Approver_Name = table.Rows[i]["Approver_Name"].ToString();
                    requestDetailPermission.Approver_Email = table.Rows[i]["Approver_Email"].ToString();
                    requestDetailPermission.Approve_Result = table.Rows[i]["Approve_Result"].ToString() == "" ? null : table.Rows[i]["Approve_Result"].ToString();
                    requestDetailPermission.Approve_Result_Display = table.Rows[i]["Approve_Result_Display"].ToString() == "" ? null : table.Rows[i]["Approve_Result_Display"].ToString();
                    requestDetailPermission.Reject_Reason = table.Rows[i]["Reject_Reason"].ToString() == "" ? null : table.Rows[i]["Reject_Reason"].ToString();
                    List<Permission> listPermission = new List<Permission>();
                    Permission permission = new Permission();
                    permission.Permission_Code = table.Rows[0]["Permission_Code"].ToString();
                    permission.Permission_Name = table.Rows[0]["Permission_Name"].ToString();
                    permission.Permission_Display_Name = table.Rows[0]["Permission_Display_Name"].ToString();
                    listPermission.Add(permission);
                    requestDetailPermission.Permission_List = listPermission.ToArray();
                    list.Add(requestDetailPermission);
                }
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<RequestDetailPermission> getRequestDetailPermissionLevelClient(string requestNo, string approveID)
        {
            try
            {
                DataTable tableOfficeClient = getRequestDetailClientPermissionGroupOffice(requestNo, approveID);
                List<RequestDetailPermission> list = new List<RequestDetailPermission>();
                for (int i = 0; i < tableOfficeClient.Rows.Count; i++)
                {
                    RequestDetailPermission requestDetailPermission = new RequestDetailPermission();
                    requestDetailPermission.Agency_ID = tableOfficeClient.Rows[i]["Agency_ID"].ToString();
                    requestDetailPermission.Agency_Name = tableOfficeClient.Rows[i]["Agency_Name"].ToString();
                    requestDetailPermission.Office_ID = tableOfficeClient.Rows[i]["Office_ID"].ToString();
                    requestDetailPermission.Office_Name = tableOfficeClient.Rows[i]["Office_Name"].ToString();
                    requestDetailPermission.Approver_UserID = tableOfficeClient.Rows[i]["Approver_UserID"].ToString();
                    requestDetailPermission.Approver_Name = tableOfficeClient.Rows[i]["Approver_Name"].ToString();
                    requestDetailPermission.Approver_Email = tableOfficeClient.Rows[i]["Approver_Email"].ToString();
                    requestDetailPermission.Delegate_UserID = tableOfficeClient.Rows[i]["Delegate_UserID"].ToString() == "" ? null : tableOfficeClient.Rows[i]["Delegate_UserID"].ToString();
                    requestDetailPermission.Approve_Result = tableOfficeClient.Rows[i]["Approve_Result"].ToString() == "" ? null : tableOfficeClient.Rows[i]["Approve_Result"].ToString();
                    requestDetailPermission.Approve_Result_Display = tableOfficeClient.Rows[i]["Approve_Result_Display"].ToString() == "" ? null : tableOfficeClient.Rows[i]["Approve_Result_Display"].ToString();
                    requestDetailPermission.Reject_Reason = tableOfficeClient.Rows[i]["Reject_Reason"].ToString() == "" ? null : tableOfficeClient.Rows[i]["Reject_Reason"].ToString();
                    requestDetailPermission.Permission_Level = tableOfficeClient.Rows[i]["Permission_Level"].ToString();
                    List<Permission> listPermission = new List<Permission>();
                    DataTable tableClient = getRequestDetailClientPermissionClientList(requestNo, approveID, requestDetailPermission.Office_ID);
                    for (int j = 0; j < tableClient.Rows.Count; j++)
                    {
                        Permission permission = new Permission();
                        permission.Permission_Code = tableClient.Rows[j]["Permission_Code"].ToString();
                        permission.Permission_Name = tableClient.Rows[j]["Permission_Name"].ToString();
                        permission.Permission_Display_Name = tableClient.Rows[j]["Permission_Display_Name"].ToString();
                        listPermission.Add(permission);
                    }
                    requestDetailPermission.Permission_List = listPermission.ToArray();
                    list.Add(requestDetailPermission);
                }
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<RequestDetail> getRequestShowDetail(string requestNo)
        {
            try
            {
                DataTable tableRequest = getRequestDetail(requestNo);
                List<RequestDetail> list = new List<RequestDetail>();
                for (int i = 0; i < tableRequest.Rows.Count; i++)
                {
                    RequestDetail requestDetail = new RequestDetail();
                    requestDetail.Request_No = tableRequest.Rows[i]["Request_No"].ToString();
                    requestDetail.Request_Type = tableRequest.Rows[i]["Request_Type"].ToString();
                    requestDetail.Request_Type_Display = tableRequest.Rows[i]["Request_Type_Display"].ToString();
                    requestDetail.Request_Status = tableRequest.Rows[i]["Request_Status"].ToString();
                    requestDetail.Request_Status_Display = tableRequest.Rows[i]["Request_Status_Display"].ToString();
                    requestDetail.Complete_Date = tableRequest.Rows[i]["Complete_Date"].ToString() == "" ? null : Convert.ToDateTime(tableRequest.Rows[i]["Complete_Date"]).ToString("dd/MM/yyyy");
                    requestDetail.Expired_Date = Convert.ToDateTime(tableRequest.Rows[i]["Expired_Date"]).ToString("dd/MM/yyyy");
                    requestDetail.Total_Approver = tableRequest.Rows[i]["Total_Approver"].ToString();
                    requestDetail.Total_Approved = tableRequest.Rows[i]["Total_Approved"].ToString();
                    requestDetail.Request_User = tableRequest.Rows[i]["Request_User"].ToString();
                    requestDetail.Request_User_Email = tableRequest.Rows[i]["Request_User_Email"].ToString();
                    requestDetail.Create_Date = Convert.ToDateTime(tableRequest.Rows[i]["Create_Date"]).ToString("dd/MM/yyyy");
                    requestDetail.Create_By = tableRequest.Rows[i]["Create_By"].ToString();
                    requestDetail.Update_Date = tableRequest.Rows[i]["Update_Date"].ToString() == "" ? null : Convert.ToDateTime(tableRequest.Rows[i]["Update_Date"]).ToString("dd/MM/yyyy");
                    requestDetail.Update_By = tableRequest.Rows[i]["Update_By"].ToString() == "" ? null : tableRequest.Rows[i]["Update_By"].ToString();
                    List<RequestDetailUser> listUser = new List<RequestDetailUser>();
                    DataTable tableUser = getRequestDetailUser(requestDetail.Request_No);
                    for (int j = 0; j < tableUser.Rows.Count; j++)
                    {
                        RequestDetailUser requestDetailUser = new RequestDetailUser();
                        requestDetailUser.User_ID = tableUser.Rows[j]["User_ID"].ToString();
                        requestDetailUser.User_Full_Name = tableUser.Rows[j]["User_Full_Name"].ToString();
                        requestDetailUser.Agency_ID = tableUser.Rows[j]["Agency_ID"].ToString();
                        requestDetailUser.Agency_Name = tableUser.Rows[j]["Agency_Name"].ToString();
                        requestDetailUser.Office_ID = tableUser.Rows[j]["Office_ID"].ToString();
                        requestDetailUser.Office_Name = tableUser.Rows[j]["Office_Name"].ToString();
                        requestDetailUser.User_Email = tableUser.Rows[j]["User_Email"].ToString();
                        requestDetailUser.Username_Agency = tableUser.Rows[j]["Username_Agency"].ToString();
                        listUser.Add(requestDetailUser);
                    }
                    requestDetail.listRequestUser = listUser.ToArray();
                    List<RequestDetailMediaType> listMediaType = new List<RequestDetailMediaType>();
                    DataTable tableMediaType = getRequestDetailMediaType(requestDetail.Request_No);
                    for (int j = 0; j < tableMediaType.Rows.Count; j++)
                    {
                        RequestDetailMediaType requestDetailMediaType = new RequestDetailMediaType();
                        requestDetailMediaType.Media_Type = tableMediaType.Rows[j]["Media_Type"].ToString();
                        requestDetailMediaType.Media_Type_Name = tableMediaType.Rows[j]["Media_Type_Name"].ToString();
                        requestDetailMediaType.Media_Type_Display_Name = tableMediaType.Rows[j]["Media_Type_Display_Name"].ToString();
                        listMediaType.Add(requestDetailMediaType);
                    }
                    requestDetail.listRequestMediaType = listMediaType.ToArray();
                    List<RequestDetailPermission> listPermission = new List<RequestDetailPermission>();
                    DataTable tableApprover = getRequestDetailApprover(requestDetail.Request_No);
                    for (int j = 0; j < tableApprover.Rows.Count; j++)
                    {
                        string approveID = tableApprover.Rows[j]["Approver_ID"].ToString();
                        List<RequestDetailPermission> listPermissionAgencyOffice = getRequestDetailPermissionLevelAgencyOffice(requestDetail.Request_No, approveID);
                        List<RequestDetailPermission> listPermissionClient = getRequestDetailPermissionLevelClient(requestDetail.Request_No, approveID);
                        listPermission.AddRange(listPermissionAgencyOffice);
                        listPermission.AddRange(listPermissionClient);
                    }
                    requestDetail.listRequestPermission = listPermission.ToArray();
                    list.Add(requestDetail);
                }
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public DataTable updateCancel(RequestCancel cancel, string userID)
        {
            try
            {
                DataTable table = new DataTable();
                string query = @"cancelRequest";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
                adapter.SelectCommand.Parameters.Add("@Request_No", SqlDbType.VarChar).Value = cancel.Request_No;
                adapter.SelectCommand.Parameters.Add("@Reason", SqlDbType.NVarChar).Value = cancel.Cancel_Reason;
                adapter.SelectCommand.Parameters.Add("@Cancel_By", SqlDbType.VarChar).Value = userID;
                connection.Open();
                adapter.Fill(table);
                connection.Close();
                return table;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public DataTable updateApprove(RequestToken tokenApprove, string approveFrom)
        {
            try
            {
                DataTable table = new DataTable();
                string query = @"RequestApprove";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
                adapter.SelectCommand.Parameters.Add("@Request_No", SqlDbType.VarChar).Value = tokenApprove.Request_No;
                adapter.SelectCommand.Parameters.Add("@Approver_ID", SqlDbType.VarChar).Value = tokenApprove.Approver_ID;
                adapter.SelectCommand.Parameters.Add("@Approve_By", SqlDbType.VarChar).Value = tokenApprove.Approver_UserID;
                adapter.SelectCommand.Parameters.Add("@Approve_From", SqlDbType.VarChar).Value = approveFrom;
                connection.Open();
                adapter.Fill(table);
                connection.Close();
                return table;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public DataTable updateReject(RejectReason reject, string rejectFrom)
        {
            try
            {
                DataTable table = new DataTable();
                string query = @"RequestReject";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
                adapter.SelectCommand.Parameters.Add("@Request_No", SqlDbType.VarChar).Value = reject.Request_No;
                adapter.SelectCommand.Parameters.Add("@Approver_ID", SqlDbType.VarChar).Value = reject.Approver_ID;
                adapter.SelectCommand.Parameters.Add("@Reject_Reason", SqlDbType.NVarChar).Value = reject.Reject_Reason == null ? "" : reject.Reject_Reason;
                adapter.SelectCommand.Parameters.Add("@Reject_Detail", SqlDbType.NVarChar).Value = reject.Reject_Detail == null ? "" : reject.Reject_Detail;
                adapter.SelectCommand.Parameters.Add("@Reject_By", SqlDbType.VarChar).Value = reject.Approver_UserID;
                adapter.SelectCommand.Parameters.Add("@Reject_From", SqlDbType.VarChar).Value = rejectFrom;
                connection.Open();
                adapter.Fill(table);
                connection.Close();
                return table;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public DataTable getApprovalRequest(string userID, string type, string status, string startDate, string endDate, string keyWord)
        {
            try
            {
                DataTable table = new DataTable();
                string query = "getApprovalRequestList_Request";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
                adapter.SelectCommand.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = userID;
                adapter.SelectCommand.Parameters.Add("@Request_Type", SqlDbType.VarChar).Value = type == "null" ? "" : type;
                adapter.SelectCommand.Parameters.Add("@Request_Status", SqlDbType.VarChar).Value = status;
                adapter.SelectCommand.Parameters.Add("@StartDate", SqlDbType.VarChar).Value = startDate == "null" || endDate == "null" ? "" : startDate;
                adapter.SelectCommand.Parameters.Add("@EndDate", SqlDbType.VarChar).Value = startDate == "null" || endDate == "null" ? "" : endDate;
                adapter.SelectCommand.Parameters.Add("@KeyWord", SqlDbType.VarChar).Value = keyWord == "null" ? "" : keyWord;
                connection.Open();
                adapter.Fill(table);
                connection.Close();
                return table;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public DataTable getApprovalRequestPermission(string requestNo, string approveID)
        {
            try
            {
                DataTable table = new DataTable();
                string query = "getApprovalRequestList_Permission";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
                adapter.SelectCommand.Parameters.Add("@Request_No", SqlDbType.VarChar).Value = requestNo;
                adapter.SelectCommand.Parameters.Add("@Approver_ID", SqlDbType.VarChar).Value = approveID;
                connection.Open();
                adapter.Fill(table);
                connection.Close();
                return table;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<ApprovalShow> getApprovalShow(string userID, string type, string status, string startDate, string endDate, string keyWord)
        {
            try
            {
                DataTable tableRequest = getApprovalRequest(userID, type, status, startDate, endDate, keyWord);
                List<ApprovalShow> list = new List<ApprovalShow>();
                for (int i = 0; i < tableRequest.Rows.Count; i++)
                {
                    ApprovalShow approvalShow = new ApprovalShow();
                    approvalShow.Approver_ID = tableRequest.Rows[i]["Approver_ID"].ToString();
                    approvalShow.Request_No = tableRequest.Rows[i]["Request_No"].ToString();
                    approvalShow.Request_Type = tableRequest.Rows[i]["Request_Type"].ToString();
                    approvalShow.Request_Type_Display = tableRequest.Rows[i]["Request_Type_Display"].ToString();
                    approvalShow.Request_Status = tableRequest.Rows[i]["Request_Status"].ToString();
                    approvalShow.Request_Status_Display = tableRequest.Rows[i]["Request_Status_Display"].ToString();
                    approvalShow.Complete_Date = tableRequest.Rows[i]["Complete_Date"].ToString() == "" ? null : Convert.ToDateTime(tableRequest.Rows[i]["Complete_Date"]).ToString("dd/MM/yyyy");
                    approvalShow.Expired_Date = Convert.ToDateTime(tableRequest.Rows[i]["Expired_Date"]).ToString("dd/MM/yyyy");
                    approvalShow.Approve_Result = tableRequest.Rows[i]["Approve_Result"].ToString() == "" ? null : tableRequest.Rows[i]["Approve_Result"].ToString();
                    approvalShow.Approve_Result_Display = tableRequest.Rows[i]["Approve_Result_Display"].ToString() == "" ? null : tableRequest.Rows[i]["Approve_Result_Display"].ToString();
                    approvalShow.Total_Approver = tableRequest.Rows[i]["Total_Approver"].ToString();
                    approvalShow.Total_Approved = tableRequest.Rows[i]["Total_Approved"].ToString();
                    approvalShow.Reject_Reason = tableRequest.Rows[i]["Reject_Reason"].ToString() == "" ? null : tableRequest.Rows[i]["Reject_Reason"].ToString();
                    approvalShow.Create_Date = Convert.ToDateTime(tableRequest.Rows[i]["Create_Date"]).ToString("dd/MM/yyyy");
                    approvalShow.Create_By = tableRequest.Rows[i]["Create_By"].ToString();
                    approvalShow.Update_Date = tableRequest.Rows[i]["Update_Date"].ToString() == "" ? null : Convert.ToDateTime(tableRequest.Rows[i]["Update_Date"]).ToString("dd/MM/yyyy");
                    approvalShow.Update_By = tableRequest.Rows[i]["Update_By"].ToString() == "" ? null : tableRequest.Rows[i]["Update_By"].ToString();
                    List<RequestUser> listUser = new List<RequestUser>();
                    DataTable tableUser = getRequestDetailUser(approvalShow.Request_No);
                    for (int j = 0; j < tableUser.Rows.Count; j++)
                    {
                        RequestUser requestUser = new RequestUser();
                        requestUser.Username_Agency = tableUser.Rows[j]["Username_Agency"].ToString();
                        listUser.Add(requestUser);
                    }
                    approvalShow.listRequestUser = listUser.ToArray();
                    List<RequestMediaType> listMediaType = new List<RequestMediaType>();
                    DataTable tableMediaType = getRequestDetailMediaType(approvalShow.Request_No);
                    for (int j = 0; j < tableMediaType.Rows.Count; j++)
                    {
                        RequestMediaType requestMediaType = new RequestMediaType();
                        requestMediaType.Media_Type_Name = tableMediaType.Rows[j]["Media_Type_Name"].ToString();
                        listMediaType.Add(requestMediaType);
                    }
                    approvalShow.listRequestMediaType = listMediaType.ToArray();
                    List<RequestPermission> listPermission = new List<RequestPermission>();
                    DataTable tablePermission = getApprovalRequestPermission(approvalShow.Request_No, approvalShow.Approver_ID);
                    for (int j = 0; j < tablePermission.Rows.Count; j++)
                    {
                        RequestPermission requestPermission = new RequestPermission();
                        requestPermission.Permission_Display_Name = tablePermission.Rows[j]["Permission_Display_Name"].ToString();
                        listPermission.Add(requestPermission);
                    }
                    approvalShow.listRequestPermission = listPermission.ToArray();
                    list.Add(approvalShow);
                }
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<RequestDetail> getApprovalRequestShowDetail(string requestNo, string approveID)
        {
            try
            {
                DataTable tableRequest = getRequestDetail(requestNo);
                List<RequestDetail> list = new List<RequestDetail>();
                for (int i = 0; i < tableRequest.Rows.Count; i++)
                {
                    RequestDetail requestDetail = new RequestDetail();
                    requestDetail.Request_No = tableRequest.Rows[i]["Request_No"].ToString();
                    requestDetail.Request_Type = tableRequest.Rows[i]["Request_Type"].ToString();
                    requestDetail.Request_Type_Display = tableRequest.Rows[i]["Request_Type_Display"].ToString();
                    requestDetail.Request_Status = tableRequest.Rows[i]["Request_Status"].ToString();
                    requestDetail.Request_Status_Display = tableRequest.Rows[i]["Request_Status_Display"].ToString();
                    requestDetail.Complete_Date = tableRequest.Rows[i]["Complete_Date"].ToString() == "" ? null : Convert.ToDateTime(tableRequest.Rows[i]["Complete_Date"]).ToString("dd/MM/yyyy");
                    requestDetail.Expired_Date = Convert.ToDateTime(tableRequest.Rows[i]["Expired_Date"]).ToString("dd/MM/yyyy");
                    requestDetail.Total_Approver = tableRequest.Rows[i]["Total_Approver"].ToString();
                    requestDetail.Total_Approved = tableRequest.Rows[i]["Total_Approved"].ToString();
                    requestDetail.Request_User = tableRequest.Rows[i]["Request_User"].ToString();
                    requestDetail.Request_User_Email = tableRequest.Rows[i]["Request_User_Email"].ToString();
                    requestDetail.Create_Date = Convert.ToDateTime(tableRequest.Rows[i]["Create_Date"]).ToString("dd/MM/yyyy");
                    requestDetail.Create_By = tableRequest.Rows[i]["Create_By"].ToString();
                    requestDetail.Update_Date = tableRequest.Rows[i]["Update_Date"].ToString() == "" ? null : Convert.ToDateTime(tableRequest.Rows[i]["Update_Date"]).ToString("dd/MM/yyyy");
                    requestDetail.Update_By = tableRequest.Rows[i]["Update_By"].ToString() == "" ? null : tableRequest.Rows[i]["Update_By"].ToString();
                    List<RequestDetailUser> listUser = new List<RequestDetailUser>();
                    DataTable tableUser = getRequestDetailUser(requestDetail.Request_No);
                    for (int j = 0; j < tableUser.Rows.Count; j++)
                    {
                        RequestDetailUser requestDetailUser = new RequestDetailUser();
                        requestDetailUser.User_ID = tableUser.Rows[j]["User_ID"].ToString();
                        requestDetailUser.User_Full_Name = tableUser.Rows[j]["User_Full_Name"].ToString();
                        requestDetailUser.Agency_ID = tableUser.Rows[j]["Agency_ID"].ToString();
                        requestDetailUser.Agency_Name = tableUser.Rows[j]["Agency_Name"].ToString();
                        requestDetailUser.Office_ID = tableUser.Rows[j]["Office_ID"].ToString();
                        requestDetailUser.Office_Name = tableUser.Rows[j]["Office_Name"].ToString();
                        requestDetailUser.User_Email = tableUser.Rows[j]["User_Email"].ToString();
                        requestDetailUser.Username_Agency = tableUser.Rows[j]["Username_Agency"].ToString();
                        listUser.Add(requestDetailUser);
                    }
                    requestDetail.listRequestUser = listUser.ToArray();
                    List<RequestDetailMediaType> listMediaType = new List<RequestDetailMediaType>();
                    DataTable tableMediaType = getRequestDetailMediaType(requestDetail.Request_No);
                    for (int j = 0; j < tableMediaType.Rows.Count; j++)
                    {
                        RequestDetailMediaType requestDetailMediaType = new RequestDetailMediaType();
                        requestDetailMediaType.Media_Type = tableMediaType.Rows[j]["Media_Type"].ToString();
                        requestDetailMediaType.Media_Type_Name = tableMediaType.Rows[j]["Media_Type_Name"].ToString();
                        requestDetailMediaType.Media_Type_Display_Name = tableMediaType.Rows[j]["Media_Type_Display_Name"].ToString();
                        listMediaType.Add(requestDetailMediaType);
                    }
                    requestDetail.listRequestMediaType = listMediaType.ToArray();
                    List<RequestDetailPermission> listPermission = new List<RequestDetailPermission>();
                    List<RequestDetailPermission> listPermissionAgencyOffice = getRequestDetailPermissionLevelAgencyOffice(requestDetail.Request_No, approveID);
                    List<RequestDetailPermission> listPermissionClient = getRequestDetailPermissionLevelClient(requestDetail.Request_No, approveID);
                    listPermission.AddRange(listPermissionAgencyOffice);
                    listPermission.AddRange(listPermissionClient);
                    requestDetail.listRequestPermission = listPermission.ToArray();
                    list.Add(requestDetail);
                }
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}