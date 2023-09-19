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
    public partial class Master_VendorList : Form
    {
        DBManager m_db = null;
        private string username = "";
        private string screenmode = "";

        public Master_VendorList(string Username, string ScreenPermission)
        {
            InitializeComponent();
            m_db = new DBManager();
            username = Username;
            screenmode = ScreenPermission;
        }

        private void InitailControl()
        {
            chkVendorCondition.Checked = false;
            chkShowPeriod.Checked = false;
            cboStatus.SelectedItem = "Active";
            ControlManipulate.ClearControl(txtVendorCodeSearch, txtMasterVendor, txtMasterVendorCode, txtVendor, txtVendorCode, txtDisplayName);
            if (screenmode == "View")
                newToolStripMenuItem.Enabled = false;
        }

        private DataRow Packing()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Media_Vendor_Code");
            dt.Columns.Add("Media_Vendor_ID");
            dt.Columns.Add("Master_Group");
            dt.Columns.Add("Short_Name");
            dt.Columns.Add("InActive");
            dt.Columns.Add("Vendor_Contract");
            dt.Columns.Add("Media_Credit");
            dt.Columns.Add("Rebate");
            dt.Columns.Add("EPD");
            dt.Columns.Add("SAC");
            dt.Columns.Add("Condition");
            dt.Columns.Add("StartDate");
            dt.Columns.Add("EndDate");
            dt.Columns.Add("Display");
            DataRow dr = dt.NewRow();
            if (txtVendorCodeSearch.Text != "")
                dr["Media_Vendor_Code"] = txtVendorCodeSearch.Text;
            if (txtMasterVendorCode.Text != "")
                dr["Master_Group"] = txtMasterVendorCode.Text;
            if (txtVendorCode.Text != "")
                dr["Media_Vendor_ID"] = txtVendorCode.Text;
            if (txtDisplayName.Text != "")
                dr["Short_Name"] = txtDisplayName.Text;
            if (cboStatus.Text != "All")
            {
                if (cboStatus.Text == "Active")
                    dr["InActive"] = 0;
                else
                    dr["InActive"] = 1;
            }
            if (chkVendorCondition.Checked)
            {
                dr["Condition"] = 1;
                dr["StartDate"] = dtPeriodStartDate.Value;
                dr["EndDate"] = dtPeriodEndDate.Value;
                if (cboVendorContract.Text != "All")
                    dr["Vendor_Contract"] = cboVendorContract.SelectedItem.ToString();
                /*if (cboMediaCredit.Text != "All")
                    dr["Media_Credit"] = cboMediaCredit.SelectedItem.ToString();*/
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

        private void DataLoading()
        {
            DataTable dt = m_db.SelectVendor(Packing());
            gvDetail.AutoGenerateColumns = false;
            gvDetail.DataSource = dt;
            if (dt.Rows.Count == 0)
                GMessage.MessageInfo("No data found.");
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

        private void Master_VendorList_Load(object sender, EventArgs e)
        {
            InitailControl();
            dtPeriodStartDate.Enabled = false;
            dtPeriodEndDate.Enabled = false;
            cboVendorContract.Enabled = false;
            cboEPD.Enabled = false;
            cboRebate.Enabled = false;
            cboSAC.Enabled = false;
            ColVendorContractPeriod.Visible = false;
            ColEPDPeriod.Visible = false;
            ColMediaCreditPeriod.Visible = false;
            ColRebatePeriod.Visible = false;
            ColSACPeriod.Visible = false;
            cboVendorContract.SelectedItem = "All";
            cboEPD.SelectedItem = "All";
            cboRebate.SelectedItem = "All";
            cboSAC.SelectedItem = "All";
            DataLoading();
        }

        private void txtVendor_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Media_Vendor_ID", "Short_Name", "Media_Vendor", true, "", "");
            frm.IncludeInactive = true;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtVendor.Text = frm.SelectedGridRow["MasterName"].ToString();
                txtVendorCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
            }
        }

        private void txtVendor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtVendor_TextChanged(object sender, EventArgs e)
        {
            if (txtVendor.Text == "")
            {
                txtVendorCode.Text = "";
            }
        }

        private void txtMasterVendor_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Media_Vendor_ID", "Short_Name", "Master_Vendor", true, "", "");
            frm.IncludeInactive = true;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtMasterVendor.Text = frm.SelectedGridRow["MasterName"].ToString();
                txtMasterVendorCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
            }
        }

        private void txtMasterVendor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtMasterVendor_TextChanged(object sender, EventArgs e)
        {
            if (txtMasterVendor.Text == "")
            {
                txtMasterVendorCode.Text = "";
            }
        }

        private void chkVendorCondition_CheckedChanged(object sender, EventArgs e)
        {
            int year = DateTime.Now.Year;
            dtPeriodStartDate.Value = new DateTime(year, 1, 1);
            dtPeriodEndDate.Value = new DateTime(year, 12, 31);

            cboVendorContract.SelectedItem = "All";
            cboEPD.SelectedItem = "All";
            //cboMediaCredit.SelectedItem = "All";
            cboRebate.SelectedItem = "All";
            cboSAC.SelectedItem = "All";

            if (chkVendorCondition.Checked == true)
            {
                dtPeriodStartDate.Enabled = true;
                dtPeriodEndDate.Enabled = true;
                cboVendorContract.Enabled = true;
                cboEPD.Enabled = true;
                //cboMediaCredit.Enabled = true;
                cboRebate.Enabled = true;
                cboSAC.Enabled = true;
            }
            else
            {
                dtPeriodStartDate.Enabled = false;
                dtPeriodEndDate.Enabled = false;
                cboVendorContract.Enabled = false;
                cboEPD.Enabled = false;
                //cboMediaCredit.Enabled = false;
                cboRebate.Enabled = false;
                cboSAC.Enabled = false;
            }
        }

        private void chkShowPeriod_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPeriod.Checked == true)
            {
                ColVendorContractPeriod.Visible = true;
                ColEPDPeriod.Visible = true;
                //ColMediaCreditPeriod.Visible = true;
                ColRebatePeriod.Visible = true;
                ColSACPeriod.Visible = true;
            }
            else
            {
                ColVendorContractPeriod.Visible = false;
                ColEPDPeriod.Visible = false;
                //ColMediaCreditPeriod.Visible = false;
                ColRebatePeriod.Visible = false;
                ColSACPeriod.Visible = false;
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            DataLoading();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            InitailControl();
            DataLoading();
        }

        private void GvDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            editToolStripMenuItem.PerformClick();
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Master_Vendor frm = new Master_Vendor("Add", username);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                DataLoading();
            }
            this.Cursor = Cursors.Default;
        }

        private void EditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gvDetail.Rows.Count == 0)
                return;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataRow dr = ((DataRowView)gvDetail.Rows[gvDetail.SelectedRows[0].Index].DataBoundItem).Row;
                Master_Vendor frm = new Master_Vendor(screenmode, username);
                frm.drData = dr;
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

        private void AllVendorMasterFileListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dt = m_db.SelectVendorMasterFileReport();
            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
            this.Cursor = Cursors.WaitCursor;
            string fileName = "Vendor - " + DateTime.Now.ToString("yyyyMMddHHmmss");
            ExcelUtil.ExportFileXlsx(dt, 1, true, fileName);
            this.Cursor = Cursors.Default;
        }

        private void PreferredVendorReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dt = m_db.SelectPreferVendorReport();
            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
            }
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
                DataTable dt = m_db.SelectVendorScreenReport(Packing());
                DataSet ds = m_db.SelectVendorScreenReportDetail(Packing());
                this.Cursor = Cursors.WaitCursor;
                ExcelUtil.ExportClientVendorScreen(dt, 1, showPeriod, "Vendor", ds);
                this.Cursor = Cursors.Default;
            }
        }
    }
}
