using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GroupM.DBAccess;

namespace GroupM.Minder
{
    public partial class AutoGrant_RequestList : Form
    {
        private string username = "";
        private APIManager m_api;
        private string strRequestNo = "";
        private DataTable dtRequestType = null;
        private DataTable dtRequestStatus = null;
        private bool bindData = false;
        private bool setCancel = true;
        public AutoGrant_Main frmMain;

        public AutoGrant_RequestList(string screenMode, string Username)
        {
            InitializeComponent();
            username = Username;
            m_api = new APIManager();
        }

        #region function
        private void InitialForRequestList()
        {
            if (dtRequestType == null || dtRequestStatus == null)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    bindData = true;
                    dtRequestType = m_api.APIGetAllRequestType();
                    dtRequestStatus = m_api.APIGetAllRequestStatus();

                    DataRow drRequestType = dtRequestType.NewRow();
                    drRequestType["Request_Type_Code"] = "All";
                    drRequestType["Request_Type_Name"] = "All Request Type";
                    drRequestType["Request_Type_Display_Name"] = "All - All Request Type";
                    drRequestType["Request_Type_Drop_Down"] = "All Request Type";
                    dtRequestType.Rows.InsertAt(drRequestType, 0);

                    DataRow drRequestStatus = dtRequestStatus.NewRow();
                    drRequestStatus["Request_Status_Code"] = "All";
                    drRequestStatus["Request_Status_Name"] = "All Request Status";
                    drRequestStatus["Request_Status_Display_Name"] = "All - All Request Status";
                    dtRequestStatus.Rows.InsertAt(drRequestStatus, 0);

                    cmbRequestType.DataSource = dtRequestType;
                    cmbRequestType.DisplayMember = "Request_Type_Drop_Down";
                    cmbRequestType.ValueMember = "Request_Type_Code";

                    cmbRequestStatus.DataSource = dtRequestStatus;
                    cmbRequestStatus.DisplayMember = "Request_Status_Name";
                    cmbRequestStatus.ValueMember = "Request_Status_Code";
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
            cmbRequestStatus.SelectedIndex = 0;
            dtStartDate.Value = new DateTime(year, month, startDay);
            dtEndDate.Value = new DateTime(year, month, endDay);
            txtRequestSearch.Text = "";
        }

        private void ShowRequestList()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                string type = cmbRequestType.SelectedIndex == 0 ? "null" : cmbRequestType.SelectedValue.ToString();
                string status = cmbRequestStatus.SelectedIndex == 0 ? "null" : cmbRequestStatus.SelectedValue.ToString();
                string startDate = dtStartDate.Value.ToString("yyyyMMdd");
                string endDate = dtEndDate.Value.ToString("yyyyMMdd");
                string search = txtRequestSearch.Text == "" ? "null" : txtRequestSearch.Text;
                List<RequestShow> listRequest = m_api.APIGetAllRequest(username, type, status, startDate, endDate, search);

                DataTable dtRequest = new DataTable();
                dtRequest.Columns.Add("RequestNo");
                dtRequest.Columns.Add("Request");
                dtRequest.Columns.Add("MediaTypeList");
                dtRequest.Columns.Add("UserList");
                dtRequest.Columns.Add("ClientList");
                dtRequest.Columns.Add("Status");

                foreach (RequestShow request in listRequest)
                {
                    DataRow drRequest = dtRequest.NewRow();

                    drRequest["RequestNo"] = request.Request_No;
                    drRequest["Request"] = "Request No" + Environment.NewLine + request.Request_No + Environment.NewLine + Environment.NewLine + "Request Type" + Environment.NewLine + request.Request_Type_Display;
                    if (request.Request_Status == "PA")
                        drRequest["Status"] = request.Request_Status_Display + Environment.NewLine + request.Total_Approved + "/" + request.Total_Approver;
                    else
                        drRequest["Status"] = request.Request_Status_Display;

                    List<RequestMediaType> listMediaType = request.listRequestMediaType.ToList();
                    for (int i = 0; i < listMediaType.Count; i++)
                    {
                        if (i == 0)
                        {
                            drRequest["MediaTypeList"] = "Media Type - " + listMediaType.Count + " type(s)";
                            drRequest["MediaTypeList"] = drRequest["MediaTypeList"].ToString() + Environment.NewLine + "1. " + listMediaType[0].Media_Type_Name;
                        }
                        else
                        {
                            string no = (i + 1).ToString() + ". ";
                            drRequest["MediaTypeList"] = drRequest["MediaTypeList"].ToString() + Environment.NewLine + no + listMediaType[i].Media_Type_Name;
                        }
                    }

                    List<RequestUser> listUser = request.listRequestUser.ToList();
                    for (int i = 0; i < listUser.Count; i++)
                    {
                        if (i == 0)
                        {
                            drRequest["UserList"] = "Request Access User - " + listUser.Count + " User(s)";
                            drRequest["UserList"] = drRequest["UserList"].ToString() + Environment.NewLine + "1. " + listUser[0].Username_Agency;
                        }
                        else
                        {
                            string no = (i + 1).ToString() + ". ";
                            drRequest["UserList"] = drRequest["UserList"].ToString() + Environment.NewLine + no + listUser[i].Username_Agency;
                        }
                    }

                    List<RequestPermission> listPermission = request.listRequestPermission.ToList();
                    for (int i = 0; i < listPermission.Count; i++)
                    {
                        if (i == 0)
                        {
                            drRequest["ClientList"] = "Responsible Agency/Office/Client - " + listPermission.Count + " item(s)";
                            drRequest["ClientList"] = drRequest["ClientList"].ToString() + Environment.NewLine + "1. " + listPermission[0].Permission_Display_Name;
                        }
                        else
                        {
                            string no = (i + 1).ToString() + ". ";
                            drRequest["ClientList"] = drRequest["ClientList"].ToString() + Environment.NewLine + no + listPermission[i].Permission_Display_Name;
                        }
                    }

                    dtRequest.Rows.Add(drRequest);
                }

                gvRequestList.AutoGenerateColumns = false;
                gvRequestList.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                gvRequestList.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                gvRequestList.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
                gvRequestList.Columns["ColStatus"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                gvRequestList.DataSource = dtRequest;

                for (int i = 0; i < gvRequestList.Rows.Count; i++)
                {
                    if (gvRequestList.Rows[i].Cells["ColStatus"].Value.ToString() == "Pending Approval")
                        ((DataGridViewImageCell)gvRequestList.Rows[i].Cells["ColImageStatus"]).Value = Properties.Resources.Approve_Pending;
                    else if (gvRequestList.Rows[i].Cells["ColStatus"].Value.ToString() == "Complete Approval")
                        ((DataGridViewImageCell)gvRequestList.Rows[i].Cells["ColImageStatus"]).Value = Properties.Resources.Approve_Approve;
                    else if (gvRequestList.Rows[i].Cells["ColStatus"].Value.ToString() == "Cancelled")
                        ((DataGridViewImageCell)gvRequestList.Rows[i].Cells["ColImageStatus"]).Value = Properties.Resources.Approve_Reject;
                    else if (gvRequestList.Rows[i].Cells["ColStatus"].Value.ToString() == "Expried")
                        ((DataGridViewImageCell)gvRequestList.Rows[i].Cells["ColImageStatus"]).Value = Properties.Resources.Approve_Expire;
                    else
                        ((DataGridViewImageCell)gvRequestList.Rows[i].Cells["ColImageStatus"]).Value = Properties.Resources.Approve_Partial;
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ShowRequestDetail(string requestNo)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                List<RequestDetail> listRequestDetail = m_api.APIGetRequestDetail(requestNo);

                // request detail
                DataTable dtRequest = new DataTable();
                dtRequest.Columns.Add("RequestDetail");

                DataRow drRequest = dtRequest.NewRow();
                drRequest["RequestDetail"] = "Request No. : " + listRequestDetail[0].Request_No;
                dtRequest.Rows.Add(drRequest);

                drRequest = dtRequest.NewRow();
                drRequest["RequestDetail"] = "Request Type : " + listRequestDetail[0].Request_Type_Display;
                dtRequest.Rows.Add(drRequest);

                drRequest = dtRequest.NewRow();
                drRequest["RequestDetail"] = "Request Status : " + listRequestDetail[0].Request_Status_Display;
                dtRequest.Rows.Add(drRequest);

                drRequest = dtRequest.NewRow();
                drRequest["RequestDetail"] = "Request By : " + listRequestDetail[0].Request_User;
                dtRequest.Rows.Add(drRequest);

                drRequest = dtRequest.NewRow();
                if (listRequestDetail[0].Update_Date == null)
                    drRequest["RequestDetail"] = "Updated : " + listRequestDetail[0].Create_Date;
                else
                    drRequest["RequestDetail"] = "Updated : " + listRequestDetail[0].Update_Date;
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

                for (int i = 0; i < listRequestDetail[0].listRequestMediaType.Length; i++)
                {
                    DataRow drMediaType = dtMediaType.NewRow();
                    drMediaType["RequestMediaType"] = listRequestDetail[0].listRequestMediaType[i].Media_Type_Name;
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

                for (int i = 0; i < listRequestDetail[0].listRequestUser.Length; i++)
                {
                    DataRow drUser = dtUser.NewRow();
                    drUser["RequestUser"] = listRequestDetail[0].listRequestUser[i].Username_Agency;
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

                for (int i = 0; i < listRequestDetail[0].listRequestPermission.Length; i++)
                {
                    DataRow drApproval = dtApproval.NewRow();
                    drApproval["ApprovalName"] = listRequestDetail[0].listRequestPermission[i].Approver_Name + Environment.NewLine + listRequestDetail[0].listRequestPermission[i].Agency_Name;
                    drApproval["Status"] = listRequestDetail[0].listRequestPermission[i].Approve_Result_Display;
                    for (int j = 0; j < listRequestDetail[0].listRequestPermission[i].Permission_List.Length; j++)
                    {
                        drApproval["ClientList"] = drApproval["ClientList"].ToString() + Environment.NewLine + listRequestDetail[0].listRequestPermission[i].Permission_List[j].Permission_Display_Name;
                    }
                    dtApproval.Rows.Add(drApproval);
                }

                gvSummaryApprovalRequest.AutoGenerateColumns = false;
                gvSummaryApprovalRequest.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
                gvSummaryApprovalRequest.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
                gvSummaryApprovalRequest.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
                gvSummaryApprovalRequest.Columns["ColStatus1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                gvSummaryApprovalRequest.DataSource = dtApproval;

                // set image status
                for (int i = 0; i < gvSummaryApprovalRequest.Rows.Count; i++)
                {
                    if (gvSummaryApprovalRequest.Rows[i].Cells["ColStatus1"].Value.ToString() == "")
                    {
                        gvSummaryApprovalRequest.Rows[i].Cells["ColStatus1"].Value = "Pending";
                        ((DataGridViewImageCell)gvSummaryApprovalRequest.Rows[i].Cells["ColImageStatus1"]).Value = Properties.Resources.Approve_Pending;
                    }
                    else if (gvSummaryApprovalRequest.Rows[i].Cells["ColStatus1"].Value.ToString() == "Approve")
                    {
                        ((DataGridViewImageCell)gvSummaryApprovalRequest.Rows[i].Cells["ColImageStatus1"]).Value = Properties.Resources.Approve_Approve;
                    }
                    else if (gvSummaryApprovalRequest.Rows[i].Cells["ColStatus1"].Value.ToString() == "Reject")
                    {
                        ((DataGridViewImageCell)gvSummaryApprovalRequest.Rows[i].Cells["ColImageStatus1"]).Value = Properties.Resources.Approve_Reject;
                    }
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
        #endregion

        private void AutoGrant_RequestList_Load(object sender, EventArgs e)
        {
            InitialForRequestList();
        }

        #region request list
        private void txtRequestSearch_KeyUp(object sender, KeyEventArgs e)
        {
            ShowRequestList();
        }

        private void cmbRequestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRequestType.SelectedIndex != -1 && bindData == false)
                ShowRequestList();
        }

        private void cmbRequestStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbRequestStatus.SelectedIndex != -1 && bindData == false)
                ShowRequestList();
        }

        private void dtStartDate_ValueChanged(object sender, EventArgs e)
        {
            ShowRequestList();
        }

        private void dtEndDate_ValueChanged(object sender, EventArgs e)
        {
            ShowRequestList();
        }

        private void gvRequestList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvRequestList.CurrentRow.Cells["ColStatus"].Value.ToString() == "Pending Approval")
                setCancel = true;
            else
                setCancel = false;
            strRequestNo = gvRequestList.CurrentRow.Cells["ColRequestNo"].Value.ToString();
            tabControlRequest.SelectedTab = tabControlRequest.TabPages[1];
        }
        #endregion

        #region center control
        private void tabControlRequest_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlRequest.SelectedTab == tabControlRequest.TabPages[0])
            {
                ShowRequestList();
                frmMain.RequestList(false);
            }
            if (tabControlRequest.SelectedTab == tabControlRequest.TabPages[1])
            {
                btnCancelRequest.Visible = setCancel;
                ShowRequestDetail(strRequestNo);
                frmMain.RequestDetail();
            }
        }

        private void btnCreateRequest_Click(object sender, EventArgs e)
        {
            frmMain.CreateNewRequest();
        }

        private void btnDetailBack_Click(object sender, EventArgs e)
        {
            strRequestNo = "";
            tabControlRequest.SelectedTab = tabControlRequest.TabPages[0];
        }

        private void btnCancelRequest_Click(object sender, EventArgs e)
        {
            AutoGrant_Cancel form = new AutoGrant_Cancel(username.ToUpper(), strRequestNo);
            DialogResult dialogResult = form.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                strRequestNo = "";
                tabControlRequest.SelectedTab = tabControlRequest.TabPages[0];
            }
        }
        #endregion

    }
}
