using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using GroupM.DBAccess;
using GroupM.UTL;

namespace GroupM.Minder
{
    public partial class AutoGrant_Approve : Form
    {
        private string username = "";
        private APIManager m_api;
        private string strRequestNo = "";
        private string strApproverID = "";
        private string strApproverUser = null;
        private string strDelegateUser = null;
        private DataTable dtRequestType = null;
        private bool bindData = false;
        private bool setApprove = true;
        public AutoGrant_Main frmMain;

        public AutoGrant_Approve(string screenMode, string Username)
        {
            InitializeComponent();
            username = Username;
            m_api = new APIManager();
        }

        #region function
        private void InitialForApproveList()
        {
            if (dtRequestType == null)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    bindData = true;
                    dtRequestType = m_api.APIGetAllRequestType();

                    DataRow drRequestType = dtRequestType.NewRow();
                    drRequestType["Request_Type_Code"] = "All";
                    drRequestType["Request_Type_Name"] = "All Request Type";
                    drRequestType["Request_Type_Display_Name"] = "All - All Request Type";
                    drRequestType["Request_Type_Drop_Down"] = "All Request Type";
                    dtRequestType.Rows.InsertAt(drRequestType, 0);

                    cmbRequestType.DataSource = dtRequestType;
                    cmbRequestType.DisplayMember = "Request_Type_Drop_Down";
                    cmbRequestType.ValueMember = "Request_Type_Code";

                    cmbRequestType1.DataSource = dtRequestType;
                    cmbRequestType1.DisplayMember = "Request_Type_Drop_Down";
                    cmbRequestType1.ValueMember = "Request_Type_Code";
                }
                finally
                {
                    bindData = false;
                    Cursor = Cursors.Default;
                }
            }

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int startDay = 1;
            int endDay = DateTime.DaysInMonth(year, month);

            cmbRequestType.SelectedIndex = 0;
            dtStartDate.Value = new DateTime(year, month, startDay);
            dtEndDate.Value = new DateTime(year, month, endDay);
            txtRequestSearch.Text = "";

            cmbRequestType1.SelectedIndex = 0;
            dtStartDate1.Value = new DateTime(year, month, startDay);
            dtEndDate1.Value = new DateTime(year, month, endDay);
            txtRequestSearch1.Text = "";
        }

        private void ShowApproveList(string status)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                string type = "";
                string startDate = "";
                string endDate = "";
                string search = "";

                if (status == "Pending")
                {
                    type = cmbRequestType.SelectedIndex == 0 ? "null" : cmbRequestType.SelectedValue.ToString();
                    startDate = dtStartDate.Value.ToString("yyyyMMdd");
                    endDate = dtEndDate.Value.ToString("yyyyMMdd");
                    search = txtRequestSearch.Text == "" ? "null" : txtRequestSearch.Text;
                }
                if (status == "Approve")
                {
                    type = cmbRequestType1.SelectedIndex == 0 ? "null" : cmbRequestType1.SelectedValue.ToString();
                    startDate = dtStartDate1.Value.ToString("yyyyMMdd");
                    endDate = dtEndDate1.Value.ToString("yyyyMMdd");
                    search = txtRequestSearch1.Text == "" ? "null" : txtRequestSearch1.Text;
                }

                List<ApprovalShow> listApprove = m_api.APIGetApprovalRequestList(username, type, status, startDate, endDate, search);

                DataTable dtApprove = new DataTable();
                dtApprove.Columns.Add("RequestNo");
                dtApprove.Columns.Add("ApproverID");
                dtApprove.Columns.Add("Request");
                dtApprove.Columns.Add("MediaTypeList");
                dtApprove.Columns.Add("UserList");
                dtApprove.Columns.Add("ClientList");
                dtApprove.Columns.Add("Approve");

                foreach (ApprovalShow approve in listApprove)
                {
                    DataRow drApprove = dtApprove.NewRow();

                    drApprove["RequestNo"] = approve.Request_No;
                    drApprove["ApproverID"] = approve.Approver_ID;
                    drApprove["Request"] = "Request No" + Environment.NewLine + approve.Request_No + Environment.NewLine + Environment.NewLine + "Request Type" + Environment.NewLine + approve.Request_Type_Display;
                    if (approve.Approve_Result == "RJ")
                        drApprove["Approve"] = approve.Approve_Result_Display + Environment.NewLine + Environment.NewLine + "Reason : " + approve.Reject_Reason;
                    else
                        drApprove["Approve"] = approve.Approve_Result_Display;

                    List<RequestMediaType> listMediaType = approve.listRequestMediaType.ToList();
                    for (int i = 0; i < listMediaType.Count; i++)
                    {
                        if (i == 0)
                        {
                            drApprove["MediaTypeList"] = "Media Type - " + listMediaType.Count + " type(s)";
                            drApprove["MediaTypeList"] = drApprove["MediaTypeList"].ToString() + Environment.NewLine + "1. " + listMediaType[0].Media_Type_Name;
                        }
                        else
                        {
                            string no = (i + 1).ToString() + ". ";
                            drApprove["MediaTypeList"] = drApprove["MediaTypeList"].ToString() + Environment.NewLine + no + listMediaType[i].Media_Type_Name;
                        }
                    }

                    List<RequestUser> listUser = approve.listRequestUser.ToList();
                    for (int i = 0; i < listUser.Count; i++)
                    {
                        if (i == 0)
                        {
                            drApprove["UserList"] = "Request Access User - " + listUser.Count + " User(s)";
                            drApprove["UserList"] = drApprove["UserList"].ToString() + Environment.NewLine + "1. " + listUser[0].Username_Agency;
                        }
                        else
                        {
                            string no = (i + 1).ToString() + ". ";
                            drApprove["UserList"] = drApprove["UserList"].ToString() + Environment.NewLine + no + listUser[i].Username_Agency;
                        }
                    }

                    List<RequestPermission> listPermission = approve.listRequestPermission.ToList();
                    for (int i = 0; i < listPermission.Count; i++)
                    {
                        if (i == 0)
                        {
                            drApprove["ClientList"] = "Responsible Agency/Office/Client - " + listPermission.Count + " item(s)";
                            drApprove["ClientList"] = drApprove["ClientList"].ToString() + Environment.NewLine + "1. " + listPermission[0].Permission_Display_Name;
                        }
                        else
                        {
                            string no = (i + 1).ToString() + ". ";
                            drApprove["ClientList"] = drApprove["ClientList"].ToString() + Environment.NewLine + no + listPermission[i].Permission_Display_Name;
                        }
                    }

                    dtApprove.Rows.Add(drApprove);
                }

                if (status == "Pending")
                {
                    gvRequestList.AutoGenerateColumns = false;
                    gvRequestList.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    gvRequestList.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    gvRequestList.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
                    gvRequestList.Columns["ColReject"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    gvRequestList.DataSource = dtApprove;

                    for (int i = 0; i < gvRequestList.Rows.Count; i++)
                    {
                        gvRequestList.Rows[i].Cells["ColApprove"].Value = "Approve";
                        gvRequestList.Rows[i].Cells["ColReject"].Value = "Reject";
                    }
                }
                if (status == "Approve")
                {
                    gvApproveList.AutoGenerateColumns = false;
                    gvApproveList.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                    gvApproveList.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                    gvApproveList.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
                    gvApproveList.Columns["ColApprove1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    gvApproveList.DataSource = dtApprove;

                    for (int i = 0; i < gvApproveList.Rows.Count; i++)
                    {
                        if (gvApproveList.Rows[i].Cells["ColApprove1"].Value.ToString() == "Approve")
                        {
                            ((DataGridViewImageCell)gvApproveList.Rows[i].Cells["ColImageApprove1"]).Value = Properties.Resources.Approve_Approve;
                        }
                        else if (gvApproveList.Rows[i].Cells["ColApprove1"].Value.ToString().Substring(0, 6) == "Reject")
                        {
                            ((DataGridViewImageCell)gvApproveList.Rows[i].Cells["ColImageApprove1"]).Value = Properties.Resources.Approve_Reject;
                            gvApproveList.Rows[i].Cells["ColImageApprove1"].Style.Alignment = DataGridViewContentAlignment.TopCenter;
                            gvApproveList.Rows[i].Cells["ColImageApprove1"].Style.Padding = new Padding(0, 10, 0, 0);
                            gvApproveList.Rows[i].Cells["ColApprove1"].Style.Alignment = DataGridViewContentAlignment.TopLeft;
                            gvApproveList.Rows[i].Cells["ColApprove1"].Style.Padding = new Padding(0, 18, 0, 0);
                        }
                    }
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ShowApproveDetail(string requestNo, string approverID)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                List<RequestDetail> listApproveDetail = m_api.APIGetApprovalRequestDetail(requestNo, approverID);

                // request detail
                DataTable dtRequest = new DataTable();
                dtRequest.Columns.Add("RequestDetail");

                DataRow drRequest = dtRequest.NewRow();
                drRequest["RequestDetail"] = "Request No. : " + listApproveDetail[0].Request_No;
                dtRequest.Rows.Add(drRequest);

                drRequest = dtRequest.NewRow();
                drRequest["RequestDetail"] = "Request Type : " + listApproveDetail[0].Request_Type_Display;
                dtRequest.Rows.Add(drRequest);

                drRequest = dtRequest.NewRow();
                drRequest["RequestDetail"] = "Request Status : " + listApproveDetail[0].Request_Status_Display;
                dtRequest.Rows.Add(drRequest);

                drRequest = dtRequest.NewRow();
                drRequest["RequestDetail"] = "Request By : " + listApproveDetail[0].Request_User;
                dtRequest.Rows.Add(drRequest);

                drRequest = dtRequest.NewRow();
                if (listApproveDetail[0].Update_Date == null)
                    drRequest["RequestDetail"] = "Updated : " + listApproveDetail[0].Create_Date;
                else
                    drRequest["RequestDetail"] = "Updated : " + listApproveDetail[0].Update_Date;
                dtRequest.Rows.Add(drRequest);

                gvRequestDetail.AutoGenerateColumns = false;
                gvRequestDetail.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                gvRequestDetail.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                gvRequestDetail.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
                gvRequestDetail.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
                gvRequestDetail.DataSource = dtRequest;

                // media type
                DataTable dtMediaType = new DataTable();
                dtMediaType.Columns.Add("RequestMediaType");

                for (int i = 0; i < listApproveDetail[0].listRequestMediaType.Length; i++)
                {
                    DataRow drMediaType = dtMediaType.NewRow();
                    drMediaType["RequestMediaType"] = listApproveDetail[0].listRequestMediaType[i].Media_Type_Name;
                    dtMediaType.Rows.Add(drMediaType);
                }

                gvSummaryMediaType.AutoGenerateColumns = false;
                gvSummaryMediaType.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                gvSummaryMediaType.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                gvSummaryMediaType.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
                gvSummaryMediaType.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
                gvSummaryMediaType.DataSource = dtMediaType;

                // user
                DataTable dtUser = new DataTable();
                dtUser.Columns.Add("RequestUser");

                for (int i = 0; i < listApproveDetail[0].listRequestUser.Length; i++)
                {
                    DataRow drUser = dtUser.NewRow();
                    drUser["RequestUser"] = listApproveDetail[0].listRequestUser[i].Username_Agency;
                    dtUser.Rows.Add(drUser);
                }

                gvSummaryRequestUser.AutoGenerateColumns = false;
                gvSummaryRequestUser.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                gvSummaryRequestUser.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                gvSummaryRequestUser.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
                gvSummaryRequestUser.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
                gvSummaryRequestUser.DataSource = dtUser;

                // permission
                DataTable dtApproval = new DataTable();
                dtApproval.Columns.Add("ApprovalName");
                dtApproval.Columns.Add("ClientList");
                dtApproval.Columns.Add("Status");

                for (int i = 0; i < listApproveDetail[0].listRequestPermission.Length; i++)
                {
                    strApproverUser = listApproveDetail[0].listRequestPermission[i].Approver_UserID;
                    strDelegateUser = listApproveDetail[0].listRequestPermission[i].Delegate_UserID;
                    DataRow drApproval = dtApproval.NewRow();
                    drApproval["ApprovalName"] = listApproveDetail[0].listRequestPermission[i].Approver_Name + Environment.NewLine + listApproveDetail[0].listRequestPermission[i].Agency_Name;
                    if (listApproveDetail[0].listRequestPermission[i].Approve_Result == "RJ")
                        drApproval["Status"] = listApproveDetail[0].listRequestPermission[i].Approve_Result_Display + Environment.NewLine + Environment.NewLine + "Reason : " + listApproveDetail[0].listRequestPermission[i].Reject_Reason;
                    else
                        drApproval["Status"] = listApproveDetail[0].listRequestPermission[i].Approve_Result_Display;
                    for (int j = 0; j < listApproveDetail[0].listRequestPermission[i].Permission_List.Length; j++)
                    {
                        drApproval["ClientList"] = drApproval["ClientList"].ToString() + Environment.NewLine + listApproveDetail[0].listRequestPermission[i].Permission_List[j].Permission_Display_Name;
                    }
                    dtApproval.Rows.Add(drApproval);
                }

                gvSummaryApprovalRequest.AutoGenerateColumns = false;
                gvSummaryApprovalRequest.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                gvSummaryApprovalRequest.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                gvSummaryApprovalRequest.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
                gvSummaryApprovalRequest.Columns["ColStatus"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                gvSummaryApprovalRequest.DataSource = dtApproval;

                // set image status
                for (int i = 0; i < gvSummaryApprovalRequest.Rows.Count; i++)
                {
                    if (gvSummaryApprovalRequest.Rows[i].Cells["ColStatus"].Value.ToString() == "")
                    {
                        gvSummaryApprovalRequest.Rows[i].Cells["ColStatus"].Value = "Pending";
                        ((DataGridViewImageCell)gvSummaryApprovalRequest.Rows[i].Cells["ColImageStatus"]).Value = Properties.Resources.Approve_Pending;
                    }
                    else if (gvSummaryApprovalRequest.Rows[i].Cells["ColStatus"].Value.ToString() == "Approve")
                    {
                        ((DataGridViewImageCell)gvSummaryApprovalRequest.Rows[i].Cells["ColImageStatus"]).Value = Properties.Resources.Approve_Approve;
                    }
                    else
                    {
                        ((DataGridViewImageCell)gvSummaryApprovalRequest.Rows[i].Cells["ColImageStatus"]).Value = Properties.Resources.Approve_Reject;
                        gvSummaryApprovalRequest.Rows[i].Cells["ColImageStatus"].Style.Alignment = DataGridViewContentAlignment.TopCenter;
                        gvSummaryApprovalRequest.Rows[i].Cells["ColImageStatus"].Style.Padding = new Padding(0, 10, 0, 0);
                        gvSummaryApprovalRequest.Rows[i].Cells["ColStatus"].Style.Alignment = DataGridViewContentAlignment.TopLeft;
                        gvSummaryApprovalRequest.Rows[i].Cells["ColStatus"].Style.Padding = new Padding(0, 18, 0, 0);
                    }
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private string CreateJsonApprove()
        {
            // variable
            string json = "";
            RequestApprove approve = new RequestApprove();
            approve.Request_No = strRequestNo;
            approve.Approver_ID = strApproverID;
            approve.Approver_UserID = strApproverUser;
            approve.Delegate_UserID = strDelegateUser;
            approve.Approve_Result = "AP";
            // convert object to json
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            json = serializer.Serialize(approve);
            return json;
        }
        #endregion

        private void AutoGrant_Approve_Load(object sender, EventArgs e)
        {
            InitialForApproveList();
        }

        #region approve list (pending)
        private void txtRequestSearch_KeyUp(object sender, KeyEventArgs e)
        {
            ShowApproveList("Pending");
        }

        private void cmbRequestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRequestType.SelectedIndex != -1 && bindData == false)
                ShowApproveList("Pending");
        }

        private void dtStartDate_ValueChanged(object sender, EventArgs e)
        {
            ShowApproveList("Pending");
        }

        private void dtEndDate_ValueChanged(object sender, EventArgs e)
        {
            ShowApproveList("Pending");
        }

        private void gvRequestList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvRequestList.Rows.Count == 0)
                return;
            if (e.ColumnIndex == 6 || e.ColumnIndex == 7)
            {
                DialogResult dialogResult = GMessage.MessageComfirm("Do you want to approve this request?");
                if (dialogResult == DialogResult.Yes)
                {
                    Cursor = Cursors.WaitCursor;
                    strRequestNo = gvRequestList.Rows[e.RowIndex].Cells["ColRequestNo"].Value.ToString();
                    strApproverID = gvRequestList.Rows[e.RowIndex].Cells["ColApproverID"].Value.ToString();
                    string user = username.ToUpper();
                    string json = CreateJsonApprove();
                    string approve = m_api.APIRequestApprove(user, json);
                    if (approve == "Call API Unsuccessful.")
                        GMessage.MessageError("Some thing is wrong, Your request is failed to approve.");
                    else
                        GMessage.MessageInfo(approve);
                    Cursor = Cursors.Default;
                    ShowApproveList("Pending");
                }
            }
            if (e.ColumnIndex == 8 || e.ColumnIndex == 9)
            {
                strRequestNo = gvRequestList.Rows[e.RowIndex].Cells["ColRequestNo"].Value.ToString();
                strApproverID = gvRequestList.Rows[e.RowIndex].Cells["ColApproverID"].Value.ToString();
                AutoGrant_Reject_1 form = new AutoGrant_Reject_1(username.ToUpper(), strRequestNo, strApproverID, strApproverUser, strDelegateUser);
                DialogResult dialogResult = form.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    ShowApproveList("Pending");
                }
            }
        }

        private void gvRequestList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            setApprove = true;
            strRequestNo = gvRequestList.CurrentRow.Cells["ColRequestNo"].Value.ToString();
            strApproverID = gvRequestList.CurrentRow.Cells["ColApproverID"].Value.ToString();
            tabControlRequest.SelectedTab = tabControlRequest.TabPages[1];
        }
        #endregion

        #region approve list (approve)
        private void txtRequestSearch1_KeyUp(object sender, KeyEventArgs e)
        {
            ShowApproveList("Approve");
        }

        private void cmbRequestType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRequestType1.SelectedIndex != -1 && bindData == false)
                ShowApproveList("Approve");
        }

        private void dtStartDate1_ValueChanged(object sender, EventArgs e)
        {
            ShowApproveList("Approve");
        }

        private void dtEndDate1_ValueChanged(object sender, EventArgs e)
        {
            ShowApproveList("Approve");
        }

        private void gvApproveList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            setApprove = false;
            strRequestNo = gvApproveList.CurrentRow.Cells["ColRequestNo1"].Value.ToString();
            strApproverID = gvApproveList.CurrentRow.Cells["ColApproverID1"].Value.ToString();
            tabControlRequest.SelectedTab = tabControlRequest.TabPages[1];
        }
        #endregion

        #region center control
        private void tabControlRequest_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlRequest.SelectedTab == tabControlRequest.TabPages[0])
            {
                if (tabApprove.SelectedTab == tabApprove.TabPages[0])
                {
                    lblTitle.Text = "Permission Request List";
                    ShowApproveList("Pending");
                }
                if (tabApprove.SelectedTab == tabApprove.TabPages[1])
                {
                    lblTitle.Text = "Approval List";
                    ShowApproveList("Approve");
                }
                frmMain.ApproveList(false);
            }
            if (tabControlRequest.SelectedTab == tabControlRequest.TabPages[1])
            {
                btnApprove.Visible = setApprove;
                btnReject.Visible = setApprove;
                ShowApproveDetail(strRequestNo, strApproverID);
                frmMain.ApproveDetail();
            }
        }

        private void tabApprove_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabApprove.SelectedTab == tabApprove.TabPages[0])
            {
                lblTitle.Text = "Permission Request List";
                ShowApproveList("Pending");
            }
            if (tabApprove.SelectedTab == tabApprove.TabPages[1])
            {
                lblTitle.Text = "Approval List";
                ShowApproveList("Approve");
            }
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            string user = username.ToUpper();
            string json = CreateJsonApprove();
            string approve = m_api.APIRequestApprove(user, json);
            if (approve == "Call API Unsuccessful.")
                GMessage.MessageError("Some thing is wrong, Your request is failed to approve.");
            else
                GMessage.MessageInfo(approve);
            Cursor = Cursors.Default;
            tabControlRequest.SelectedTab = tabControlRequest.TabPages[0];
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            AutoGrant_Reject_1 form = new AutoGrant_Reject_1(username.ToUpper(), strRequestNo, strApproverID, strApproverUser, strDelegateUser);
            DialogResult dialogResult = form.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                tabControlRequest.SelectedTab = tabControlRequest.TabPages[0];
            }
        }

        private void btnDetailBack_Click(object sender, EventArgs e)
        {
            strRequestNo = "";
            strApproverID = "";
            tabControlRequest.SelectedTab = tabControlRequest.TabPages[0];
        }
        #endregion

    }
}
