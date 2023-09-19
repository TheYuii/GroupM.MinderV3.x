using GroupM.AutoGrant.API.Models.Json_Models;
using GroupM.AutoGrant.API.Models.Json_Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GroupM.AutoGrant.API.Models.Json_Data
{
    public class MasterData
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["WorkFlow"].ToString();
        private ConvertToJson convert = new ConvertToJson();

        public DataTable getRequestType()
        {
            try
            {
                DataTable table = new DataTable();
                string query = "getAllRequestType";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
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

        public DataTable getRequestStatus()
        {
            try
            {
                DataTable table = new DataTable();
                string query = "getAllRequestStatus";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
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

        public DataTable getMe(string userID)
        {
            try
            {
                DataTable table = new DataTable();
                string query = "getRequestMe";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
                adapter.SelectCommand.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = userID == "null" ? "" : userID;
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

        public DataTable getUser(string userID)
        {
            try
            {
                DataTable table = new DataTable();
                string query = "getRequestUser";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
                adapter.SelectCommand.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = userID == "null" ? "" : userID;
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

        public DataTable getMediaType()
        {
            try
            {
                DataTable table = new DataTable();
                string query = "getAllMediaType";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
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

        public DataTable getMediaTypePermission(string userID)
        {
            try
            {
                DataTable table = new DataTable();
                string query = "getMediaTypePermissionByUser";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
                adapter.SelectCommand.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = userID == "null" ? "" : userID;
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

        public DataTable getAgency()
        {
            try
            {
                DataTable table = new DataTable();
                string query = "getAllAgency";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
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

        public DataTable getOffice()
        {
            try
            {
                DataTable table = new DataTable();
                string query = "getAllOffice";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
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

        public DataTable getClient()
        {
            try
            {
                DataTable table = new DataTable();
                string query = "getAllClient";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
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

        public DataTable getAgencyOfficePermission(string userID)
        {
            try
            {
                DataTable table = new DataTable();
                string query = "getClientPermissionByUser_LevelAgencyOffice";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
                adapter.SelectCommand.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = userID;
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

        public DataTable getClientPermissionGroupOffice(string userID)
        {
            try
            {
                DataTable table = new DataTable();
                string query = "getClientPermissionByUser_LevelClient_Office";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
                adapter.SelectCommand.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = userID;
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

        public DataTable getClientPermissionClientList(string userID, string officeID)
        {
            try
            {
                DataTable table = new DataTable();
                string query = "getClientPermissionByUser_LevelClient_Client";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.CommandTimeout = 0;
                adapter.SelectCommand.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = userID;
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

        public List<CopyPermission> getCopyPermissionLevelAgencyOffice(string userID)
        {
            try
            {
                DataTable table = getAgencyOfficePermission(userID);
                List<CopyPermission> list = new List<CopyPermission>();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    CopyPermission copyPermission = new CopyPermission();
                    copyPermission.Permission_Level = table.Rows[i]["Permission_Level"].ToString();
                    copyPermission.Agency_ID = table.Rows[i]["Agency_ID"].ToString();
                    copyPermission.Agency_Name = table.Rows[i]["Agency_Name"].ToString();
                    if (copyPermission.Permission_Level == "Office Level")
                    {
                        copyPermission.Office_ID = table.Rows[i]["Office_ID"].ToString();
                        copyPermission.Office_Name = table.Rows[i]["Office_Name"].ToString();
                        copyPermission.Delegate_UserID = table.Rows[i]["Delegate_UserID"].ToString();
                    }
                    copyPermission.Approver_UserID = table.Rows[i]["Approver_UserID"].ToString();
                    copyPermission.Approver_Name = table.Rows[i]["Approver_Name"].ToString();
                    copyPermission.Approver_Email = table.Rows[i]["Approver_Email"].ToString();
                    List<Permission> listPermission = new List<Permission>();
                    Permission permission = new Permission();
                    permission.Permission_Code = table.Rows[i]["Permission_Code"].ToString();
                    permission.Permission_Name = table.Rows[i]["Permission_Name"].ToString();
                    permission.Permission_Display_Name = table.Rows[i]["Permission_Display_Name"].ToString();
                    listPermission.Add(permission);
                    copyPermission.Permission_List = listPermission.ToArray();
                    list.Add(copyPermission);
                }
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<CopyPermission> getCopyPermissionLevelClient(string userID)
        {
            try
            {
                DataTable tableOfficeClient = getClientPermissionGroupOffice(userID);
                List<CopyPermission> list = new List<CopyPermission>();
                for (int i = 0; i < tableOfficeClient.Rows.Count; i++)
                {
                    CopyPermission copyPermission = new CopyPermission();
                    copyPermission.Agency_ID = tableOfficeClient.Rows[i]["Agency_ID"].ToString();
                    copyPermission.Agency_Name = tableOfficeClient.Rows[i]["Agency_Name"].ToString();
                    copyPermission.Office_ID = tableOfficeClient.Rows[i]["Office_ID"].ToString();
                    copyPermission.Office_Name = tableOfficeClient.Rows[i]["Office_Name"].ToString();
                    copyPermission.Approver_UserID = tableOfficeClient.Rows[i]["Approver_UserID"].ToString();
                    copyPermission.Approver_Name = tableOfficeClient.Rows[i]["Approver_Name"].ToString();
                    copyPermission.Approver_Email = tableOfficeClient.Rows[i]["Approver_Email"].ToString();
                    copyPermission.Delegate_UserID = tableOfficeClient.Rows[i]["Delegate_UserID"].ToString();
                    copyPermission.Permission_Level = tableOfficeClient.Rows[i]["Permission_Level"].ToString();
                    List<Permission> listPermission = new List<Permission>();
                    DataTable tableClient = getClientPermissionClientList(userID, copyPermission.Office_ID);
                    for (int j = 0; j < tableClient.Rows.Count; j++)
                    {
                        Permission permission = new Permission();
                        permission.Permission_Code = tableClient.Rows[j]["Permission_Code"].ToString();
                        permission.Permission_Name = tableClient.Rows[j]["Permission_Name"].ToString();
                        permission.Permission_Display_Name = tableClient.Rows[j]["Permission_Display_Name"].ToString();
                        listPermission.Add(permission);
                    }
                    copyPermission.Permission_List = listPermission.ToArray();
                    list.Add(copyPermission);
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