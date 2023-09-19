using GroupM.DBAccess;
using GroupM.UTL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupM.Minder
{
    public partial class Master_ClientList : Form
    {
        DBManager m_db = null;
        private string username = "";
        private string screenmode = "";

        public Master_ClientList(string Username, string ScreenPermission)
        {
            InitializeComponent();
            m_db = new DBManager();
            username = Username;
            screenmode = ScreenPermission;
        }

        private void Master_ClientList_Load(object sender, EventArgs e)
        {
            InitailControl();
            dtPeriodStartDate.Enabled = false;
            dtPeriodEndDate.Enabled = false;
            cboAudit.Enabled = false;
            cboEPD.Enabled = false;
            cboMediaCredit.Enabled = false;
            cboRebate.Enabled = false;
            cboSAC.Enabled = false;
            ColAuditRightPeriod.Visible = false;
            ColEPDPeriod.Visible = false;
            ColMediaCreditPeriod.Visible = false;
            ColRebatePeriod.Visible = false;
            ColSACPeriod.Visible = false;
            cboAudit.SelectedItem = "All";
            cboEPD.SelectedItem = "All";
            cboMediaCredit.SelectedItem = "All";
            cboRebate.SelectedItem = "All";
            cboSAC.SelectedItem = "All";
            DataLoading();

            //test
            //btnSearch.PerformClick();
            //gvDetail.Rows[0].Selected = true;
            //editToolStripMenuItem.PerformClick();
        }
        private DataRow Packing()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ClientCode");
            dt.Columns.Add("Client_ID");
            dt.Columns.Add("Short_Name");
            dt.Columns.Add("MasterClient_ID");
            dt.Columns.Add("Agency_ID");
            dt.Columns.Add("Report_to_Agency");
            dt.Columns.Add("Office_ID");
            dt.Columns.Add("Opt_in_Signed");
            dt.Columns.Add("InactiveClient");
            dt.Columns.Add("GroupProprietaryId");
            dt.Columns.Add("Audit_Right");
            dt.Columns.Add("Media_Credit");
            dt.Columns.Add("Rebate");
            dt.Columns.Add("EPD");
            dt.Columns.Add("SAC");
            dt.Columns.Add("Condition");
            dt.Columns.Add("StartDate");
            dt.Columns.Add("EndDate");
            dt.Columns.Add("Display");

            DataRow dr = dt.NewRow();
            if (txtClientCodeSearch.Text != "")
                dr["ClientCode"] = txtClientCodeSearch.Text;
            if (txtClientCode.Text != "")
                dr["Client_ID"] = txtClientCode.Text;
            if (txtDisplayName.Text != "")
                dr["Short_Name"] = txtDisplayName.Text;
            if (txtMasterClientCode.Text != "")
                dr["MasterClient_ID"] = txtMasterClientCode.Text;
            if (txtAgencyCode.Text != "")
                dr["Agency_ID"] = txtAgencyCode.Text;
            if (txtReportToAgencyCode.Text != "")
                dr["Report_to_Agency"] = txtReportToAgencyCode.Text;
            if (txtOfficeCode.Text != "")
                dr["Office_ID"] = txtOfficeCode.Text;
            if (cboOptIn.Text != "All")
            {
                if (cboOptIn.Text == "Opt-in Signed")
                    dr["Opt_in_Signed"] = 1;
                else
                    dr["Opt_in_Signed"] = 0;
            }
            if (cboStatus.Text != "All")
            {
                if (cboStatus.Text == "Active")
                    dr["InactiveClient"] = 0;
                else
                    dr["InactiveClient"] = 1;
            }
            if (cboOptInGroup.Text != "All")
            {
                dr["GroupProprietaryId"] = cboOptInGroup.SelectedValue;
            }
            if (chkClientCondition.Checked)
            {
                dr["Condition"] = 1;
                dr["StartDate"] = dtPeriodStartDate.Value;
                dr["EndDate"] = dtPeriodEndDate.Value;
                if (cboAudit.Text != "All")
                    dr["Audit_Right"] = cboAudit.SelectedItem.ToString();
                if (cboMediaCredit.Text != "All")
                    dr["Media_Credit"] = cboMediaCredit.SelectedItem.ToString();
                if (cboRebate.Text != "All")
                    dr["Rebate"] = cboRebate.SelectedItem.ToString();
                if (cboEPD.Text != "All")
                    dr["EPD"] = cboEPD.SelectedItem.ToString();
                if (cboSAC.Text != "All")
                    dr["SAC"] = cboSAC.SelectedItem.ToString();
            }
            if (chkShowPeriod.Checked)
                dr["Display"] = 1;
            return dr;
        }

        private void InitailControl()
        {
            chkClientCondition.Checked = false;
            chkShowPeriod.Checked = false;
            cboOptIn.SelectedItem = "All";
            cboStatus.SelectedItem = "Active";
            ControlManipulate.ClearControl(txtClientCodeSearch, txtClientCode, txtClient, txtDisplayName, txtMasterClientCode, txtMasterClient, txtAgencyCode, txtAgency, txtReportToAgencyCode, txtReportToAgency, txtOfficeCode, txtOffice);

            DataTable dt = m_db.SelectOptInGroup();
            cboOptInGroup.DataSource = dt;
            cboOptInGroup.DisplayMember = "GroupProprietaryName";
            cboOptInGroup.ValueMember = "GroupProprietaryId";
            cboOptInGroup.SelectedItem = "All";

            if (screenmode == "View")
                newToolStripMenuItem.Enabled = false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F8 || keyData == Keys.Enter)
            {
                DataLoading();
                return true;
            }
            if (keyData == (Keys.Control | Keys.R))
            {
                InitailControl();
                DataLoading();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void DataLoading()
        {
            DataTable dt = m_db.SelectClient(Packing());

            gvDetail.AutoGenerateColumns = false;
            gvDetail.DataSource = dt;
            if (dt.Rows.Count == 0)
                GMessage.MessageInfo("No data found.");
        }

        private void txtClient_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Client_ID", "Short_Name", "Client", true, "", txtOfficeCode.Text);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtClient.Text = frm.SelectedGridRow["MasterName"].ToString();
                txtClientCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
            }
        }

        private void txtClient_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtClient_TextChanged(object sender, EventArgs e)
        {
            if (txtClient.Text == "")
            {
                txtClientCode.Text = "";
            }
        }

        private void txtMasterClient_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Client_ID", "Short_Name", "Master_Client", true, "", txtOfficeCode.Text);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtMasterClient.Text = frm.SelectedGridRow["MasterName"].ToString();
                txtMasterClientCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
            }
        }

        private void txtMasterClient_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtMasterClient_TextChanged(object sender, EventArgs e)
        {
            if (txtMasterClient.Text == "")
            {
                txtMasterClientCode.Text = "";
            }
        }

        private void txtAgency_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Agency_ID", "Short_Name", "Agency", true, "", "");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtAgency.Text = frm.SelectedGridRow["MasterName"].ToString();
                txtAgencyCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
            }
        }

        private void txtAgency_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtAgency_TextChanged(object sender, EventArgs e)
        {
            if (txtAgency.Text == "")
            {
                txtAgencyCode.Text = "";
            }
        }

        private void txtReportToAgency_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Agency_ID", "Short_Name", "Agency", true, "", "");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtReportToAgency.Text = frm.SelectedGridRow["MasterName"].ToString();
                txtReportToAgencyCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
            }
        }

        private void txtReportToAgency_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtReportToAgency_TextChanged(object sender, EventArgs e)
        {
            if (txtReportToAgency.Text == "")
            {
                txtReportToAgencyCode.Text = "";
            }
        }

        private void txtOffice_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Office_ID", "Short_Name", "Office", true, "", txtAgencyCode.Text);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtOffice.Text = frm.SelectedGridRow["MasterName"].ToString();
                txtOfficeCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
            }
        }

        private void txtOffice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtOffice_TextChanged(object sender, EventArgs e)
        {
            if (txtOffice.Text == "")
            {
                txtOfficeCode.Text = "";
            }
        }

        private void chkClientCondition_CheckedChanged(object sender, EventArgs e)
        {
            int year = DateTime.Now.Year;
            dtPeriodStartDate.Value = new DateTime(year, 1, 1);
            dtPeriodEndDate.Value = new DateTime(year, 12, 31);

            cboAudit.SelectedItem = "All";
            cboEPD.SelectedItem = "All";
            cboMediaCredit.SelectedItem = "All";
            cboRebate.SelectedItem = "All";
            cboSAC.SelectedItem = "All";

            if (chkClientCondition.Checked == true)
            {
                dtPeriodStartDate.Enabled = true;
                dtPeriodEndDate.Enabled = true;
                cboAudit.Enabled = true;
                cboEPD.Enabled = true;
                cboMediaCredit.Enabled = true;
                cboRebate.Enabled = true;
                cboSAC.Enabled = true;
            }
            else
            {
                dtPeriodStartDate.Enabled = false;
                dtPeriodEndDate.Enabled = false;
                cboAudit.Enabled = false;
                cboEPD.Enabled = false;
                cboMediaCredit.Enabled = false;
                cboRebate.Enabled = false;
                cboSAC.Enabled = false;
            }
        }

        private void chkShowPeriod_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPeriod.Checked == true)
            {
                ColAuditRightPeriod.Visible = true;
                ColEPDPeriod.Visible = true;
                ColMediaCreditPeriod.Visible = true;
                ColRebatePeriod.Visible = true;
                ColSACPeriod.Visible = true;
            }
            else
            {
                ColAuditRightPeriod.Visible = false;
                ColEPDPeriod.Visible = false;
                ColMediaCreditPeriod.Visible = false;
                ColRebatePeriod.Visible = false;
                ColSACPeriod.Visible = false;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataLoading();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            InitailControl();
            DataLoading();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Master_Client frm = new Master_Client("Add", username);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                DataLoading();
            }
            this.Cursor = Cursors.Default;
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gvDetail.Rows.Count == 0)
                return;
            try
            {
                this.Cursor = Cursors.WaitCursor;

                DataRow dr = ((DataRowView)gvDetail.Rows[gvDetail.SelectedRows[0].Index].DataBoundItem).Row;
                Master_Client frm = new Master_Client(screenmode, username);
                frm.drClient = dr;
                if (frm.ShowDialog() == DialogResult.Yes)
                {
                    DataLoading();
                }
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void gvDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            editToolStripMenuItem.PerformClick();
        }

        private void allClientMasterFileListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dt = m_db.SelectClientMasterFileReport(username.Replace(".", ""));
            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            this.Cursor = Cursors.WaitCursor;
            ExcelUtil.ExportXlsx(dt, 1, true);
            this.Cursor = Cursors.Default;
        }

        private void clientMasterFileReportingTeamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dt = m_db.SelectClientMasterFileReport();
            this.Cursor = Cursors.WaitCursor;
            ExcelUtil.ExportXlsx(dt, 1, true);
            this.Cursor = Cursors.Default;
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gvDetail.Rows.Count == 0)
            {
                GMessage.MessageWarning("No data to export.");
            }
            else
            {
                bool showPeriod;
                if (chkShowPeriod.Checked)
                    showPeriod = true;
                else
                    showPeriod = false;
                DataTable dt = m_db.SelectClientScreenReport(Packing());
                DataSet ds = m_db.SelectClientScreenReportDetail(Packing());
                this.Cursor = Cursors.WaitCursor;
                ExcelUtil.ExportClientVendorScreen(dt, 1, showPeriod, "Client", ds);
                this.Cursor = Cursors.Default;
            }
        }
    }
}
