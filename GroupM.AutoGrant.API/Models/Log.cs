using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GroupM.AutoGrant.API.Models
{
    public class Log
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["WorkFlow"].ToString();

        public bool InsertLogRequest(string eventType, string eventStatus, string requestNo, string description, string user)
        {
            try
            {
                string query = @"LogRequest";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 0;
                command.Parameters.Add("@Event_Type", SqlDbType.VarChar).Value = eventType;
                command.Parameters.Add("@Event_Status", SqlDbType.VarChar).Value = eventStatus;
                command.Parameters.Add("@Request_No", SqlDbType.VarChar).Value = requestNo;
                command.Parameters.Add("@Description", SqlDbType.VarChar).Value = description;
                command.Parameters.Add("@Request_By", SqlDbType.VarChar).Value = user;
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

        public bool InsertLogAPI(string apiName, string url, string parameter, string method, string body, int status, string response)
        {
            try
            {
                string query = @"Log_Call_API";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 0;
                command.Parameters.Add("@API_Name", SqlDbType.VarChar).Value = apiName;
                command.Parameters.Add("@API_URL", SqlDbType.VarChar).Value = url;
                command.Parameters.Add("@Parameter", SqlDbType.VarChar).Value = parameter;
                command.Parameters.Add("@Method", SqlDbType.VarChar).Value = method;
                command.Parameters.Add("@Json_Body", SqlDbType.VarChar).Value = body;
                command.Parameters.Add("@Response_Status", SqlDbType.Int).Value = status;
                command.Parameters.Add("@Response", SqlDbType.VarChar).Value = response;
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
    }
}