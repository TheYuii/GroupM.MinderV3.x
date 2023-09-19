using GroupM.AutoGrant.API.Models.Json_Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace GroupM.AutoGrant.API.Models.Json_Service
{
    public class ConvertToJson
    {
        public string ConvertDataTabletoJsonString(DataTable table)
        {
            try
            {
                string json = "";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<Dictionary<string, object>> listRows = new List<Dictionary<string, object>>();
                Dictionary<string, object> Row;
                foreach (DataRow row in table.Rows)
                {
                    Row = new Dictionary<string, object>();
                    foreach (DataColumn column in table.Columns)
                    {
                        Row.Add(column.ColumnName, row[column]);
                    }
                    listRows.Add(Row);
                }
                json = serializer.Serialize(listRows);
                return json;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public string ConvertListtoJsonString<T>(List<T> list)
        {
            try
            {
                string json = "";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                json = serializer.Serialize(list);
                return json;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public string ConvertObjecttoJsonString(CreateRequest body)
        {
            try
            {
                string json = "";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                json = serializer.Serialize(body);
                return json;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public string ConvertObjecttoJsonString(RequestCancel body)
        {
            try
            {
                string json = "";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                json = serializer.Serialize(body);
                return json;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public string ConvertObjecttoJsonString(RequestToken body)
        {
            try
            {
                string json = "";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                json = serializer.Serialize(body);
                return json;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public string ConvertObjecttoJsonString(RejectReason body)
        {
            try
            {
                string json = "";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                json = serializer.Serialize(body);
                return json;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}