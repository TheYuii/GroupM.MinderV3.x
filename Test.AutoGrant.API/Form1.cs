using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace Test.AutoGrant.API
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string connectionString = ConfigurationManager.ConnectionStrings["WorkFlow"].ToString();
        private string serverMinder = ConfigurationManager.AppSettings["ServerMinder"];
        private string urlAPI = ConfigurationManager.AppSettings["URLAPI"];
        private string jsonCreateNewRequest = "{ \r\n\t" +
            "\"Request_Type\": \"CO\", \r\n\t" +
            "\"Request_Status\": \"PD\", \r\n\t" +
            "\"Request_By\": \"KANOKKANL\", \r\n\t" +
            "\"listRequestUser\": [ \r\n\t\t" +
                "{ \r\n\t\t\t" +
                    "\"User_ID\": \"AIYARYNP\", \r\n\t\t\t" +
                    "\"User_Email\": \"aiyaryn.piamphatanachai@mindshareworld.com\", \r\n\t\t\t" +
                    "\"Agency_ID\": \"MS\", \r\n\t\t\t" +
                    "\"Office_ID\": \"ESP\" \r\n\t\t" +
                "}, \r\n\t\t" +
                "{ \r\n\t\t\t" +
                    "\"User_ID\": \"ANYARATS\", \r\n\t\t\t" +
                    "\"User_Email\": \"anyarat.satraksa@mindshareworld.com\", \r\n\t\t\t" +
                    "\"Agency_ID\": \"MS\", \r\n\t\t\t" +
                    "\"Office_ID\": \"MS11\" \r\n\t\t" +
                "} \r\n\t" +
            "], \r\n\t" +
            "\"listRequestMediaType\": [ \r\n\t\t" +
                "{ \r\n\t\t\t" +
                    "\"Media_Type\": \"IT\" \r\n\t\t" +
                "}, \r\n\t\t" +
                "{ \r\n\t\t\t" +
                    "\"Media_Type\": \"TV\" \r\n\t\t" +
                "} \r\n\t" +
            "], \r\n\t" +
            "\"listRequestPermission\": [ \r\n\t\t" +
                "{ \r\n\t\t\t" +
                    "\"Agency_ID\": \"GMM6\", \r\n\t\t\t" +
                    "\"Agency_Name\": \"GroupM (Thailand) Co.; Ltd. - m/Six\", \r\n\t\t\t" +
                    "\"Office_ID\": null, \r\n\t\t\t" +
                    "\"Office_Name\": null, \r\n\t\t\t" +
                    "\"Approver_UserID\": \"PINYAPATTE\", \r\n\t\t\t" +
                    "\"Approver_Name\": \"Pinyapat Teerawattananon\", \r\n\t\t\t" +
                    "\"Approver_Email\": \"pinyapat.teerawattananon@groupm.com\", \r\n\t\t\t" +
                    "\"Delegate_UserID\": null, \r\n\t\t\t" +
                    "\"Permission_Level\": \"Agency Level\", \r\n\t\t\t" +
                    "\"Permission_List\": [ \r\n\t\t\t\t" +
                        "{ \r\n\t\t\t\t\t" +
                            "\"Permission_Code\": \"GMM6\", \r\n\t\t\t\t\t" +
                            "\"Permission_Name\": \"GroupM (Thailand) Co.; Ltd. - m/Six\", \r\n\t\t\t\t\t" +
                            "\"Permission_Display_Name\": \"GMM6 - GroupM (Thailand) Co.; Ltd. - m/Six\" \r\n\t\t\t\t" +
                        "} \r\n\t\t\t" +
                    "] \r\n\t\t" +
                "}, \r\n\t\t" +
                "{ \r\n\t\t\t" +
                    "\"Agency_ID\": \"MS\", \r\n\t\t\t" +
                    "\"Agency_Name\": \"WPP (THAILAND) LTD. - MINDSHARE\", \r\n\t\t\t" +
                    "\"Office_ID\": \"MS09\", \r\n\t\t\t" +
                    "\"Office_Name\": \"Amethyst\", \r\n\t\t\t" +
                    "\"Approver_UserID\": \"ANOCHAW\", \r\n\t\t\t" +
                    "\"Approver_Name\": \"Anocha Wattanajarukit\", \r\n\t\t\t" +
                    "\"Approver_Email\": \"Anocha.Wattanajarukit@mindshareworld.com\", \r\n\t\t\t" +
                    "\"Delegate_UserID\": \"CHAIWATI\", \r\n\t\t\t" +
                    "\"Permission_Level\": \"Office Level\", \r\n\t\t\t" +
                    "\"Permission_List\": [ \r\n\t\t\t\t" +
                        "{ \r\n\t\t\t\t\t" +
                            "\"Permission_Code\": \"MS09\", \r\n\t\t\t\t\t" +
                            "\"Permission_Name\": \"Amethyst\", \r\n\t\t\t\t\t" +
                            "\"Permission_Display_Name\": \"MS09 - Amethyst\" \r\n\t\t\t\t" +
                        "} \r\n\t\t\t" +
                    "] \r\n\t\t" +
                "}, \r\n\t\t" +
                "{ \r\n\t\t\t" +
                    "\"Agency_ID\": \"MS\", \r\n\t\t\t" +
                    "\"Agency_Name\": \"WPP (THAILAND) LTD. - MINDSHARE\", \r\n\t\t\t" +
                    "\"Office_ID\": \"MS05\", \r\n\t\t\t" +
                    "\"Office_Name\": \"UNILEVER\", \r\n\t\t\t" +
                    "\"Approver_UserID\": \"PANDURANGAM\", \r\n\t\t\t" +
                    "\"Approver_Name\": \"Panduranga Mattu\", \r\n\t\t\t" +
                    "\"Approver_Email\": \"panduranga.mattu@mindshareworld.com\", \r\n\t\t\t" +
                    "\"Delegate_UserID\": \"CHAIWATI\", \r\n\t\t\t" +
                    "\"Permission_Level\": \"Client Level\", \r\n\t\t\t" +
                    "\"Permission_List\": [ \r\n\t\t\t\t" +
                        "{ \r\n\t\t\t\t\t" +
                            "\"Permission_Code\": \"14UNIEXC\", \r\n\t\t\t\t\t" +
                            "\"Permission_Name\": \"UNILEVER THAI TRADING CO.;LTD\", \r\n\t\t\t\t\t" +
                            "\"Permission_Display_Name\": \"14UNIEXC - UNILEVER THAI TRADING CO.;LTD\" \r\n\t\t\t\t" +
                        "}, \r\n\t\t\t\t" +
                        "{ \r\n\t\t\t\t\t" +
                            "\"Permission_Code\": \"14UNIIND\", \r\n\t\t\t\t\t" +
                            "\"Permission_Name\": \"UNILEVER  THAI TRADING LIMITED.\", \r\n\t\t\t\t\t" +
                            "\"Permission_Display_Name\": \"14UNIIND - UNILEVER  THAI TRADING LIMITED.\" \r\n\t\t\t\t" +
                        "} \r\n\t\t\t" +
                    "] \r\n\t\t" +
                "} \r\n\t" +
            "] \r\n" +
        "}";
        private string jsonRequestApprove = "{ \r\n\t" +
            "\"Request_No\": \"requestNoX\", \r\n\t" +
            "\"Approver_ID\": \"1\", \r\n\t" +
            "\"Approver_UserID\": \"PINYAPATTE\", \r\n\t" +
            "\"Approve_Result\": \"AP\" \r\n" +
        "}";
        private string jsonRequestReject = "{ \r\n\t" +
            "\"Request_No\": \"requestNoX\", \r\n\t" +
            "\"Approver_ID\": \"2\", \r\n\t" +
            "\"Reject_Reason\": \"Reason 0123456789.\", \r\n\t" +
            "\"Reject_Detail\": null, \r\n\t" +
            "\"Approver_UserID\": \"ANOCHAW\", \r\n\t" +
            "\"Approve_Result\": \"RJ\" \r\n" +
        "}";
        private string jsonRequestRejectReason = "{ \r\n\t" +
            "\"Token\": \"tokenX\" \r\n" +
        "}";
        private string requestNo = "";
        private bool testAPI = true;

        public void RefreshData()
        {
            try
            {
                DataTable table = new DataTable();
                string query = @"select Request.Request_No, 
                Request_Approver.Approver_ID, 
                Request_Token.Approve_Token, 
                Request_Token.Reject_Token 
                from Request 
                inner join Request_Approver 
                on Request.Request_No = Request_Approver.Request_No 
                inner join Request_Token 
                on Request.Request_No = Request_Token.Request_No 
                and Request_Approver.Approver_ID = Request_Token.Approver_ID 
                where Request.Request_No in (
	                select max(Request_No) 
	                from Request
                )";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                connection.Open();
                adapter.Fill(table);
                connection.Close();
                if (table.Rows.Count > 0)
                {
                    if (requestNo == "")
                    {
                        requestNo = table.Rows[0]["Request_No"].ToString();
                    }
                    string rejectToken = table.Rows[0]["Reject_Token"].ToString();
                    jsonRequestApprove = jsonRequestApprove.Replace("requestNoX", requestNo);
                    jsonRequestReject = jsonRequestReject.Replace("requestNoX", requestNo);
                    jsonRequestRejectReason = jsonRequestRejectReason.Replace("tokenX", rejectToken);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Test Work Flow API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static string FormatJson(string json, string indent)
        {
            int indentation = 0;
            int quoteCount = 0;
            int escapeCount = 0;
            IEnumerable<string> result =
                from ch in json ?? string.Empty
                let escaped = (ch == '\\' ? escapeCount++ : escapeCount > 0 ? escapeCount-- : escapeCount) > 0
                let quotes = ch == '"' && !escaped ? quoteCount++ : quoteCount
                let unquoted = quotes % 2 == 0
                let colon = ch == ':' && unquoted ? ": " : null
                let nospace = char.IsWhiteSpace(ch) && unquoted ? string.Empty : null
                let lineBreak = ch == ',' && unquoted ? ch + Environment.NewLine + string.Concat(Enumerable.Repeat(indent, indentation)) : null
                let openChar = (ch == '{' || ch == '[') && unquoted ? ch + Environment.NewLine + string.Concat(Enumerable.Repeat(indent, ++indentation)) : ch.ToString()
                let closeChar = (ch == '}' || ch == ']') && unquoted ? Environment.NewLine + string.Concat(Enumerable.Repeat(indent, --indentation)) + ch : ch.ToString()
                select colon ?? nospace ?? lineBreak ?? (
                    openChar.Length > 1 ? openChar : closeChar
                );
            string formatJson = string.Concat(result);
            return formatJson;
        }

        public void DeleteTestData()
        {
            try
            {
                string query = @"delete from Request
                delete from Request_User
                delete from Request_Media_Type
                delete from Request_Approver
                delete from Request_Client
                delete from Request_Token
                delete from Log_Request
                dbcc checkident (Log_Request, reseed, 0)
                delete from Log_TransferClient
                dbcc checkident (Log_TransferClient, reseed, 0)
                delete from Log_API
                dbcc checkident (Log_API, reseed, 0)
                
                delete from " + serverMinder + @".dbo.User_Client where User_ID in ('AIYARYNP', 'ANYARATS') and Modify_user_ID = 'SYSTEM'
                delete from " + serverMinder + @".dbo.User_Media_Type where User_ID in ('AIYARYNP', 'ANYARATS') and Media_Type in ('IT', 'TV')";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Test Work Flow API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Test Work Flow API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
            textBox3.Text = "";
            if (comboBox1.SelectedItem.ToString() == "Get Request Me")
            {
                textBox1.Text = urlAPI + "getRequestMe/{userID}";
                comboBox2.SelectedIndex = 0;
                textBox2.Text = "";
            }
            if (comboBox1.SelectedItem.ToString() == "Get Request User")
            {
                textBox1.Text = urlAPI + "getRequestUser/{userID}";
                comboBox2.SelectedIndex = 0;
                textBox2.Text = "";
            }
            if (comboBox1.SelectedItem.ToString() == "Get All Media Type")
            {
                textBox1.Text = urlAPI + "getAllMediaType";
                comboBox2.SelectedIndex = 0;
                textBox2.Text = "";
            }
            if (comboBox1.SelectedItem.ToString() == "Get All Agency")
            {
                textBox1.Text = urlAPI + "getAllAgency/{userID}";
                comboBox2.SelectedIndex = 0;
                textBox2.Text = "";
            }
            if (comboBox1.SelectedItem.ToString() == "Get All Office")
            {
                textBox1.Text = urlAPI + "getAllOffice/{userID}";
                comboBox2.SelectedIndex = 0;
                textBox2.Text = "";
            }
            if (comboBox1.SelectedItem.ToString() == "Get All Client")
            {
                textBox1.Text = urlAPI + "getAllClient/{userID}";
                comboBox2.SelectedIndex = 0;
                textBox2.Text = "";
            }
            if (comboBox1.SelectedItem.ToString() == "Get Client Permission By User")
            {
                textBox1.Text = urlAPI + "getClientPermissionByUser/{userID}";
                comboBox2.SelectedIndex = 0;
                textBox2.Text = "";
            }
            if (comboBox1.SelectedItem.ToString() == "Create New Request")
            {
                textBox1.Text = urlAPI + "createNewRequest";
                comboBox2.SelectedIndex = 1;
                textBox2.Text = jsonCreateNewRequest;
            }
            if (comboBox1.SelectedItem.ToString() == "Request Approve")
            {
                textBox1.Text = urlAPI + "RequestApprove/{userID}";
                comboBox2.SelectedIndex = 2;
                textBox2.Text = jsonRequestApprove;
            }
            if (comboBox1.SelectedItem.ToString() == "Request Reject")
            {
                textBox1.Text = urlAPI + "RequestReject/{userID}";
                comboBox2.SelectedIndex = 2;
                textBox2.Text = jsonRequestReject;
            }
            if (comboBox1.SelectedItem.ToString() == "Request Reject Reason")
            {
                textBox1.Text = urlAPI + "RequestRejectReason";
                comboBox2.SelectedIndex = 2;
                textBox2.Text = jsonRequestRejectReason;
            }
            if (comboBox1.SelectedItem.ToString() == "Get All Request Type")
            {
                textBox1.Text = urlAPI + "getAllRequestType";
                comboBox2.SelectedIndex = 0;
                textBox2.Text = "";
            }
            if (comboBox1.SelectedItem.ToString() == "Get All Request Status")
            {
                textBox1.Text = urlAPI + "getAllRequestStatus";
                comboBox2.SelectedIndex = 0;
                textBox2.Text = "";
            }
            if (comboBox1.SelectedItem.ToString() == "Get All Request")
            {
                textBox1.Text = urlAPI + "getAllRequest/{userID}/{requestType}/{requestStatus}/{startDate}/{endDate}/{keyWord}";
                comboBox2.SelectedIndex = 0;
                textBox2.Text = "";
            }
            if (comboBox1.SelectedItem.ToString() == "Get Request Detail")
            {
                textBox1.Text = urlAPI + "getRequestDetail/{requestNo}";
                comboBox2.SelectedIndex = 0;
                textBox2.Text = "";
            }
            if (comboBox1.SelectedItem.ToString() == "Get Approval Request List")
            {
                textBox1.Text = urlAPI + "getApprovalRequestList/{userID}/{requestType}/{requestStatus}/{startDate}/{endDate}/{keyWord}";
                comboBox2.SelectedIndex = 0;
                textBox2.Text = "";
            }
            if (comboBox1.SelectedItem.ToString() == "Get Approval Request Detail")
            {
                textBox1.Text = urlAPI + "getApprovalRequestDetail/{requestNo}/{approverID}";
                comboBox2.SelectedIndex = 0;
                textBox2.Text = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select API Name", "Test Work Flow API", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Cursor = Cursors.WaitCursor;
                    // request
                    string uri = textBox1.Text.Replace(urlAPI, "");
                    Method method = Method.GET;
                    if (comboBox2.SelectedIndex == 1)
                    {
                        method = Method.POST;
                    }
                    if (comboBox2.SelectedIndex == 2)
                    {
                        method = Method.PUT;
                    }
                    RestRequest request = new RestRequest(uri, method);
                    request.Timeout = 0;
                    // parameter user id
                    if (comboBox1.SelectedItem.ToString() == "Get Request Me" ||
                        comboBox1.SelectedItem.ToString() == "Get Request User" ||
                        comboBox1.SelectedItem.ToString() == "Get All Agency" ||
                        comboBox1.SelectedItem.ToString() == "Get All Office" ||
                        comboBox1.SelectedItem.ToString() == "Get All Client" ||
                        comboBox1.SelectedItem.ToString() == "Get Client Permission By User" ||
                        comboBox1.SelectedItem.ToString() == "Get All Request" ||
                        comboBox1.SelectedItem.ToString() == "Request Approve" ||
                        comboBox1.SelectedItem.ToString() == "Request Reject")
                    {
                        request.AddUrlSegment("userID", "KANOKKANL");
                    }
                    // parameter request type, start date, end date, key word
                    if (comboBox1.SelectedItem.ToString() == "Get All Request" ||
                        comboBox1.SelectedItem.ToString() == "Get Approval Request List")
                    {
                        request.AddUrlSegment("requestType", "null");
                        request.AddUrlSegment("startDate", "null");
                        request.AddUrlSegment("endDate", "null");
                        request.AddUrlSegment("keyWord", "null");
                    }
                    // parameter request status
                    if (comboBox1.SelectedItem.ToString() == "Get All Request")
                    {
                        request.AddUrlSegment("requestStatus", "null");
                    }
                    if (comboBox1.SelectedItem.ToString() == "Get Approval Request List")
                    {
                        request.AddUrlSegment("userID", "PANDURANGAM");
                        request.AddUrlSegment("requestStatus", "Pending");
                    }
                    // parameter request no
                    if (comboBox1.SelectedItem.ToString() == "Get Request Detail" ||
                        comboBox1.SelectedItem.ToString() == "Get Approval Request Detail")
                    {
                        request.AddUrlSegment("requestNo", requestNo);
                    }
                    // parameter approver id
                    if (comboBox1.SelectedItem.ToString() == "Get Approval Request Detail")
                    {
                        request.AddUrlSegment("approverID", "1");
                    }
                    // body json
                    if (comboBox1.SelectedItem.ToString() == "Create New Request" ||
                        comboBox1.SelectedItem.ToString() == "Request Approve" ||
                        comboBox1.SelectedItem.ToString() == "Request Reject" ||
                        comboBox1.SelectedItem.ToString() == "Request Reject Reason")
                    {
                        string jsonData = textBox2.Text.Replace("\r\n", "").Replace("\t", "");
                        request.AddJsonBody(jsonData);
                    }
                    // response
                    RestClient client = new RestClient(urlAPI);
                    IRestResponse response = client.Execute(request);
                    // convert json to object
                    string responseJson = response.Content;
                    string jsonUnformat = responseJson.Replace("\\u0026", "&").Replace("\\u0027", "'");
                    string jsonFormat = FormatJson(jsonUnformat, "\t");
                    textBox3.Text = jsonFormat;
                    Cursor = Cursors.Default;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        testAPI = true;
                        if (comboBox1.SelectedItem.ToString() == "Create New Request")
                        {
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            ResponseJson jsonRequestNo = serializer.Deserialize<ResponseJson>(responseJson);
                            requestNo = jsonRequestNo.Request_No;
                        }
                    }
                    else
                    {
                        testAPI = false;
                        if (responseJson == "")
                        {
                            textBox3.Text = response.ErrorMessage;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Test Work Flow API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteTestData();
                MessageBox.Show("Delete Test Data Success.", "Test Work Flow API", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Test Work Flow API", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show("Delete Test Data Fail.", "Test Work Flow API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < comboBox1.Items.Count; i++)
                {
                    if (testAPI == true)
                    {
                        comboBox1.SelectedIndex = i;
                        button1.PerformClick();
                    }
                }
                if (testAPI == true)
                {
                    MessageBox.Show("API Automatic Test Success.", "Test Work Flow API", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("API Automatic Test Fail.", "Test Work Flow API", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Test Work Flow API", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show("API Automatic Test Fail.", "Test Work Flow API", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
