using GroupM.DBAccess;
using GroupM.UTL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupM.Minder.SpotPlanIT
{
    public partial class Master_Vendor : Form
    {
        enum eScreenMode { Add, Edit, View }
        public DataRow drData { get; set; }

        private string username = "";

        private string masterVendor = "";

        private Regex regex = new Regex(@"[^a-zA-Z0-9\s]");

        eScreenMode m_screemMode;

        DBManager m_db;

        private void SetScreenMode(eScreenMode mode)
        {
            switch (mode)
            {
                case eScreenMode.Add:
                    btnDelete.Visible = false;
                    break;
                case eScreenMode.Edit:
                    txtVendorCode.Enabled = false;
                    break;
                case eScreenMode.View:
                    txtVendorCode.ReadOnly = true;
                    txtDisplayName.ReadOnly = true;
                    txtThaiName.ReadOnly = true;
                    txtEnglishName.ReadOnly = true;
                    txtAddress.ReadOnly = true;
                    chkMasterVendor.Enabled = false;
                    txtMasterVendor.ReadOnly = true;
                    txtAgency.ReadOnly = true;
                    txtPaymentTerm.ReadOnly = true;
                    txtRemark.ReadOnly = true;
                    txtSupplier.ReadOnly = true;
                    txtContact.ReadOnly = true;
                    txtFax.ReadOnly = true;
                    txtTel.ReadOnly = true;
                    txtSymVendorID.ReadOnly = true;
                    rdActive.Enabled = false;
                    rdInactive.Enabled = false;
                    dtInactiveDate.Enabled = false;
                    chkGPMVendor.Enabled = false;
                    chkBroker.Enabled = false;
                    chkAgencyVendor.Enabled = false;
                    chkPOBreakByMediaCategory.Enabled = false;
                    chkCDPercentage.Enabled = false;
                    txtCDPercentage.Enabled = false;
                    chkPreferVendor.Enabled = false;
                    btnAddPeriod.Enabled = false;
                    gvDetail.Enabled = false;
                    btnAddPeriodSignContract.Enabled = false;
                    gvSignContract.Enabled = false;
                    btnAddPeriodEPD.Enabled = false;
                    gvEPD.Enabled = false;
                    /*btnAddPeriodMediaCredit.Enabled = false;
                    gvMediaCredit.Enabled = false;*/
                    btnAddPeriodRebate.Enabled = false;
                    gvRebate.Enabled = false;
                    btnAddPeriodSAC.Enabled = false;
                    gvSAC.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    btnDelete.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    btnDelete.Enabled = false;
                    break;
            }
            m_screemMode = mode;
        }

        public Master_Vendor(string screenMode, string Username)
        {
            InitializeComponent();
            username = Username;
            m_db = new DBManager();
            if (screenMode == "Add")
                SetScreenMode(eScreenMode.Add);
            else if (screenMode == "Edit")
                SetScreenMode(eScreenMode.Edit);
            else
                SetScreenMode(eScreenMode.View);
        }

        private void InitControl()
        {
            DateTimeConvertUtil.DateTimeSetEmpty(dtInactiveDate);
            DataTable dt = m_db.SelectPerferVendorPeriod("");
            gvDetail.AutoGenerateColumns = false;
            gvDetail.DataSource = dt;
            gvDetail.Columns[0].DefaultCellStyle.Format = @"dd/MM/yyyy";
            gvDetail.Columns[1].DefaultCellStyle.Format = @"dd/MM/yyyy";

            DataTable dtSignContract = m_db.SelectVendorRedGreenPeriod("", "Media_Vendor_Contract");
            gvSignContract.AutoGenerateColumns = false;
            gvSignContract.DataSource = dtSignContract;
            gvSignContract.Columns[0].DefaultCellStyle.Format = @"dd/MM/yyyy";
            gvSignContract.Columns[1].DefaultCellStyle.Format = @"dd/MM/yyyy";

            DataTable dtEPD = m_db.SelectVendorRedGreenPeriod("", "Media_Vendor_EPD");
            gvEPD.AutoGenerateColumns = false;
            gvEPD.DataSource = dtEPD;
            gvEPD.Columns[0].DefaultCellStyle.Format = @"dd/MM/yyyy";
            gvEPD.Columns[1].DefaultCellStyle.Format = @"dd/MM/yyyy";

            /*DataTable dtMediaCredit = m_db.SelectVendorRedGreenPeriod("", "Media_Vendor_Media_Credit");
            gvMediaCredit.AutoGenerateColumns = false;
            gvMediaCredit.DataSource = dtMediaCredit;
            gvMediaCredit.Columns[0].DefaultCellStyle.Format = @"dd/MM/yyyy";
            gvMediaCredit.Columns[1].DefaultCellStyle.Format = @"dd/MM/yyyy";*/

            DataTable dtRebate = m_db.SelectVendorRedGreenPeriod("", "Media_Vendor_Rebate");
            gvRebate.AutoGenerateColumns = false;
            gvRebate.DataSource = dtRebate;
            gvRebate.Columns[0].DefaultCellStyle.Format = @"dd/MM/yyyy";
            gvRebate.Columns[1].DefaultCellStyle.Format = @"dd/MM/yyyy";

            DataTable dtSAC = m_db.SelectVendorRedGreenPeriod("", "Media_Vendor_SAC");
            gvSAC.AutoGenerateColumns = false;
            gvSAC.DataSource = dtSAC;
            gvSAC.Columns[0].DefaultCellStyle.Format = @"dd/MM/yyyy";
            gvSAC.Columns[1].DefaultCellStyle.Format = @"dd/MM/yyyy";
        }

        private void Dataloading()
        {
            DataTable dt = m_db.SelectVendor(drData["Media_Vendor_ID"].ToString());
            DataRow dr = dt.Rows[0];
            txtVendorCode.Text = dr["Media_Vendor_ID"].ToString();
            txtThaiName.Text = dr["Thai_Name"].ToString();
            txtEnglishName.Text = dr["English_Name"].ToString();
            txtDisplayName.Text = dr["Short_Name"].ToString();
            //txtVendorCode.Text = dr["Media_Type"].ToString();-------NOT Use??
            txtAddress.Text = dr["Address"].ToString();
            txtContact.Text = dr["Contact"].ToString();
            txtTel.Text = dr["Tel"].ToString();
            txtFax.Text = dr["Fax"].ToString();
            txtPaymentTerm.Text = dr["Payment_Term"].ToString();
            txtRemark.Text = dr["Comment"].ToString();
            //txtVendorCode.Text = dr["User_ID"].ToString();
            //txtVendorCode.Text = dr["Modify_Date"].ToString();
            txtSupplier.Text = dr["Supplier"].ToString();
            //txtVendorCode.Text = dr["LastDate"].ToString();-------NOT Use??

            chkMasterVendor.Checked = dr["Master_Vendor"].ToString() == "1";
            txtMasterVendor.Text = dr["MasterVendorName"].ToString();
            txtMasterVendorCode.Text = dr["Master_Group"].ToString();
            
            chkBroker.Checked = (bool)dr["Broker"];
            if (dr["InActive"].ToString() == "1")
            {
                rdActive.Checked = false;
                rdInactive.Checked = true;
                dtInactiveDate.Enabled = true;
                DateTimeConvertUtil.DateTimeRestoreFormat(dtInactiveDate);
                if (dr["Expire_Date"] == DBNull.Value)
                    //DateTimeConvertUtil.DateTimeSetEmpty(dtInactiveDate);
                    dtInactiveDate.Value = dtInactiveDate.MinDate;
                else
                    dtInactiveDate.Value = DateTimeConvertUtil.DateTimeConvertdd_MM_yyyyWithToDateTime(dr["Expire_Date"].ToString());

            }
            else
            {
                rdActive.Checked = true;
                rdInactive.Checked = false;
                dtInactiveDate.Enabled = false;
                DateTimeConvertUtil.DateTimeSetEmpty(dtInactiveDate);
            }
            txtAgency.Text = dr["AgencyName"].ToString();
            txtAgencyCode.Text = dr["Agency_ID"].ToString();
            //txtVendorCode.Text = dr["Merge_Sched"].ToString();-------NOT Use

            chkPreferVendor.Checked = dr["isPreferred"] == DBNull.Value ? false : Convert.ToBoolean(dr["isPreferred"]);
            //chkPromoted.Checked = dr["isPromoted"].ToString() == "1";--------NOT Use
            //txtSymVendorID.Text = dr["Mapping_Symp"].ToString();//-------------------NOT Use
            chkGPMVendor.Checked = dr["GPM_Vendor"] == DBNull.Value ? false : Convert.ToBoolean(dr["GPM_Vendor"]);
            chkPOBreakByMediaCategory.Checked = dr["POBreakByMediaFlag"] == DBNull.Value ? false : Convert.ToBoolean(dr["POBreakByMediaFlag"]);
            chkCDPercentage.Checked = dr["CDPercentageFlag"] == DBNull.Value ? false : Convert.ToBoolean(dr["CDPercentageFlag"]);
            txtCDPercentage.Value = Convert.ToDecimal(dr["CDPercentage"] == DBNull.Value ? 0 : dr["CDPercentage"]);

            txtSymVendorID.Text = dr["Sym_VendorUID"].ToString();
            chkAgencyVendor.Checked = dr["Agency_Vendor"] == DBNull.Value ? false : Convert.ToBoolean(dr["Agency_Vendor"]);

            DataTable dtPerfer = m_db.SelectPerferVendorPeriod(drData["Media_Vendor_ID"].ToString());
            gvDetail.AutoGenerateColumns = false;
            gvDetail.DataSource = dtPerfer;
            gvDetail.Columns[0].DefaultCellStyle.Format = @"dd/MM/yyyy";
            gvDetail.Columns[1].DefaultCellStyle.Format = @"dd/MM/yyyy";

            DataTable dtSignContract = m_db.SelectVendorRedGreenPeriod(drData["Media_Vendor_ID"].ToString(), "Media_Vendor_Contract");
            gvSignContract.AutoGenerateColumns = false;
            gvSignContract.DataSource = dtSignContract;
            gvSignContract.Columns[0].DefaultCellStyle.Format = @"dd/MM/yyyy";
            gvSignContract.Columns[1].DefaultCellStyle.Format = @"dd/MM/yyyy";

            DataTable dtEPD = m_db.SelectVendorRedGreenPeriod(drData["Media_Vendor_ID"].ToString(), "Media_Vendor_EPD");
            gvEPD.AutoGenerateColumns = false;
            gvEPD.DataSource = dtEPD;
            gvEPD.Columns[0].DefaultCellStyle.Format = @"dd/MM/yyyy";
            gvEPD.Columns[1].DefaultCellStyle.Format = @"dd/MM/yyyy";

            /*DataTable dtMediaCredit = m_db.SelectVendorRedGreenPeriod(drData["Media_Vendor_ID"].ToString(), "Media_Vendor_Media_Credit");
            gvMediaCredit.AutoGenerateColumns = false;
            gvMediaCredit.DataSource = dtMediaCredit;
            gvMediaCredit.Columns[0].DefaultCellStyle.Format = @"dd/MM/yyyy";
            gvMediaCredit.Columns[1].DefaultCellStyle.Format = @"dd/MM/yyyy";*/

            DataTable dtRebate = m_db.SelectVendorRedGreenPeriod(drData["Media_Vendor_ID"].ToString(), "Media_Vendor_Rebate");
            gvRebate.AutoGenerateColumns = false;
            gvRebate.DataSource = dtRebate;
            gvRebate.Columns[0].DefaultCellStyle.Format = @"dd/MM/yyyy";
            gvRebate.Columns[1].DefaultCellStyle.Format = @"dd/MM/yyyy";

            DataTable dtSAC = m_db.SelectVendorRedGreenPeriod(drData["Media_Vendor_ID"].ToString(), "Media_Vendor_SAC");
            gvSAC.AutoGenerateColumns = false;
            gvSAC.DataSource = dtSAC;
            gvSAC.Columns[0].DefaultCellStyle.Format = @"dd/MM/yyyy";
            gvSAC.Columns[1].DefaultCellStyle.Format = @"dd/MM/yyyy";

            // store master vendor
            masterVendor = txtMasterVendorCode.Text;
        }

        private void Master_Vendor_Load(object sender, EventArgs e)
        {
            InitControl();
            if (m_screemMode == eScreenMode.Edit || m_screemMode == eScreenMode.View)
                Dataloading();
        }

        private void TxtVendorCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && regex.IsMatch(e.KeyChar.ToString()))
            {
                e.Handled = true;
            }
        }

        private void RdActive_CheckedChanged(object sender, EventArgs e)
        {
            DateTimeConvertUtil.DateTimeSetEmpty(dtInactiveDate);

        }

        private void RdInactive_CheckedChanged(object sender, EventArgs e)
        {
            DateTimeConvertUtil.DateTimeRestoreFormat(dtInactiveDate);
        }

        private bool CheckDataBeforeSave()
        {
            if (m_screemMode == eScreenMode.Add)
            {
                if (txtVendorCode.Text == "")
                {
                    GMessage.MessageWarning("Please input Vendor Code.");
                    txtVendorCode.Focus();
                    return false;
                }
                if (m_db.SelectVendor(txtVendorCode.Text).Rows.Count != 0)
                {
                    GMessage.MessageWarning(string.Format("Please change Vendor Code. The {0} already exist.", txtVendorCode.Text));
                    txtVendorCode.Focus();
                    return false;
                }
            }
            if (txtDisplayName.Text == "")
            {
                GMessage.MessageWarning("Please input Display Name.");
                txtDisplayName.Focus();
                return false;
            }
            if (txtEnglishName.Text == "")
            {
                GMessage.MessageWarning("Please input English Name.");
                txtEnglishName.Focus();
                return false;
            }
            if (txtMasterVendorCode.Text == "")
            {
                GMessage.MessageWarning("Please input Master Vendor");
                txtMasterVendor.Focus();
                return false;
            }
            if (chkPreferVendor.Checked == true && gvDetail.Rows.Count == 0)
            {
                GMessage.MessageWarning("Please input Preferred Period From and To.");
                btnAddPeriod.Focus();
                return false;
            }
            if (gvDetail.Rows.Count > 0 && chkPreferVendor.Checked != true)
            {
                GMessage.MessageWarning("The prefer vendor period exist(s)." + Environment.NewLine + "Please tick at Preferred Vendor Check Box");
                chkPreferVendor.Focus();
                return false;
            }
            return true;
        }

        private DataRow Packing()
        {
            DataTable dt = m_db.SelectVendor("dummycolumn");
            DataRow dr = dt.NewRow();

            dr["Media_Vendor_ID"] = txtVendorCode.Text;
            dr["Thai_Name"] = txtThaiName.Text;
            dr["English_Name"] = txtEnglishName.Text;
            dr["Short_Name"] = txtDisplayName.Text;
            // dr["Media_Type"]-------NOT Use??=//txtVendorCode.Text ;
            dr["Address"] = txtAddress.Text;
            dr["Contact"] = txtContact.Text;
            dr["Tel"] = txtTel.Text;
            dr["Fax"] = txtFax.Text;
            dr["Payment_Term"] = txtPaymentTerm.Text;
            dr["Comment"] = txtRemark.Text;
            dr["User_ID"] = username.Replace(".", "");
            dr["Modify_Date"] = DateTime.Now.ToString("yyyyMMdd");
            dr["Supplier"] = txtSupplier.Text;
            // dr["LastDate"]-------NOT Use??=//txtVendorCode.Text ;

            dr["Master_Vendor"] = chkMasterVendor.Checked ? 1 : 0;
            dr["Master_Group"] = txtMasterVendorCode.Text;

            dr["Broker"] = chkBroker.Checked ? 1 : 0;
            dr["InActive"] = rdInactive.Checked ? 1 : 0;
            if (rdInactive.Checked)
            {
                if (dtInactiveDate.Value == dtInactiveDate.MinDate)
                    dr["Expire_Date"] = DBNull.Value;
                else
                    dr["Expire_Date"] = dtInactiveDate.Value.ToString("dd/MM/yyyy");
            }
            else
            {
                dr["Expire_Date"] = DBNull.Value;
            }
            dr["Agency_ID"] = txtAgencyCode.Text;
            // dr["Merge_Sched"]-------NOT Use

            dr["isPreferred"] = chkPreferVendor.Checked ? 1 : 0;
            // dr["isPromoted"].ToString() =//chkPromoted.Checked ;
            // dr["Mapping_Symp"]//-------------------NOT Use=//txtSymVendorID.Text ;
            dr["GPM_Vendor"] = chkGPMVendor.Checked ? 1 : 0;
            dr["POBreakByMediaFlag"] = chkPOBreakByMediaCategory.Checked ? 1 : 0;
            dr["CDPercentageFlag"] = chkCDPercentage.Checked ? 1 : 0;
            dr["CDPercentage"] = txtCDPercentage.Value;

            dr["Sym_VendorUID"] = txtSymVendorID.Text;
            dr["Agency_Vendor"] = chkAgencyVendor.Checked ? 1 : 0;

            return dr;
        }

        private bool OnCommandSave()
        {
            if (!CheckDataBeforeSave())
                return false;
            try
            {
                string log = "";
                if (m_screemMode == eScreenMode.Add)
                {
                    if (m_db.InsertVendor(Packing()) == -1)
                        return false;
                    else
                        log = $"Add new Vendor VendorCode = {txtVendorCode.Text} VendorName = {txtDisplayName.Text}";

                    m_db.InsertLogMinder(username.Replace(".", ""), $"Add (Media Vendor) Profile (Media_Vendor_ID) {txtVendorCode.Text} (Payment_Term) {txtPaymentTerm.Text}", "Master File", username.Replace(".", ""));
                    m_db.InsertLogMinder(username.Replace(".", ""), log, "Media Vendor", username.Replace(".", ""));

                }
                else
                {
                    if (m_db.UpdateVendor(Packing()) == -1)
                    {
                        return false;
                    }
                    else
                    {

                        log = $"Update (Media Vendor) Profile (Media_Vendor_ID) {txtVendorCode.Text}";
                        m_db.InsertLogMinder(username.Replace(".", ""), log, "Master File", username.Replace(".", ""));

                        string Inactive = rdInactive.Checked ? "Yes" : "No";
                        log = $"Update Vendor VendorCode = {txtVendorCode.Text} VendorName = {txtDisplayName.Text} inactived = {Inactive}";
                        m_db.InsertLogMinder(username.Replace(".", ""), log, "Media Vendor", username.Replace(".", ""));
                    }
                }
                m_db.UpdatePreferVendor((DataTable)gvDetail.DataSource, txtVendorCode.Text);
                DateTime now = DateTime.Now;
                m_db.UpdateVendorRedGreenPeriod((DataTable)gvSignContract.DataSource, "Media_Vendor_Contract", txtVendorCode.Text, now, username);
                m_db.UpdateVendorRedGreenPeriod((DataTable)gvEPD.DataSource, "Media_Vendor_EPD", txtVendorCode.Text, now, username);
                //m_db.UpdateVendorRedGreenPeriod((DataTable)gvMediaCredit.DataSource, "Media_Vendor_Media_Credit", txtVendorCode.Text, now, username);
                m_db.UpdateVendorRedGreenPeriod((DataTable)gvRebate.DataSource, "Media_Vendor_Rebate", txtVendorCode.Text, now, username);
                m_db.UpdateVendorRedGreenPeriod((DataTable)gvSAC.DataSource, "Media_Vendor_SAC", txtVendorCode.Text, now, username);
                if (chkMasterVendor.Checked)
                {
                    m_db.UpdateVendorPeriodFromMaster(txtVendorCode.Text, username);
                }
                else
                {
                    m_db.UpdateVendorPeriodClearFlag(txtVendorCode.Text);
                    if (m_screemMode == eScreenMode.Add)
                        m_db.CopyVendorRGPeriodFromMaster(txtVendorCode.Text, txtMasterVendorCode.Text, username);
                    else if (masterVendor != txtMasterVendorCode.Text)
                        m_db.CopyVendorRGPeriodFromNewMaster(txtVendorCode.Text, txtMasterVendorCode.Text, username);
                }
                GMessage.MessageInfo("Save Completed");
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OnCommandSave())
                DialogResult = DialogResult.Yes;
        }

        private void CancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to exit?", "Vendor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Dispose();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (m_db.SelectVendorIsUsing(txtVendorCode.Text))
            {
                string str = $"Cannot delete Vendor Code: {txtVendorCode.Text} - {txtDisplayName.Text}  because some Buying Brief(s) referred to this Vendor.";
                GMessage.MessageWarning(str);
                return;
            }
            DialogResult result = MessageBox.Show("Are you sure to delete Vendor: " + txtVendorCode.Text + " - " + txtDisplayName.Text + "?", "Vendor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                m_db.DeleteVendor(txtVendorCode.Text);
                m_db.InsertLogMinder(username.Replace(".", ""), "Delete Vendor ID:" + txtVendorCode.Text + ",Name :" + txtDisplayName.Text, "Master File", username.Replace(".", ""));
                GMessage.MessageInfo("Delete Completed");
                DialogResult = DialogResult.Yes;
            }
        }

        private void txtAgency_Click(object sender, EventArgs e)
        {
            if (m_screemMode == eScreenMode.Add || m_screemMode == eScreenMode.Edit)
            {
                Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Agency_ID", "Short_Name", "Agency", false, "", "");
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    txtAgency.Text = frm.SelectedGridRow["MasterName"].ToString();
                    txtAgencyCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
                }
            }
        }

        private void txtAgency_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtAgency_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Clipboard.Clear();
            }
        }

        private void txtAgency_TextChanged(object sender, EventArgs e)
        {
            if (txtAgency.Text == "")
            {
                txtAgencyCode.Text = "";
            }
        }

        private void txtMasterVendor_Click(object sender, EventArgs e)
        {
            if (m_screemMode == eScreenMode.Add || m_screemMode == eScreenMode.Edit)
            {
                Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Media_Vendor_ID", "Short_Name", "Media_Vendor", false, "", "");
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    txtMasterVendor.Text = frm.SelectedGridRow["MasterName"].ToString();
                    txtMasterVendorCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
                }
            }
        }

        private void txtMasterVendor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtMasterVendor_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Clipboard.Clear();
            }
        }

        private void txtMasterVendor_TextChanged(object sender, EventArgs e)
        {
            if (txtMasterVendor.Text == "")
            {
                txtMasterVendorCode.Text = "";
            }
        }

        private void chkMasterVendor_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMasterVendor.Checked == true)
            {
                txtMasterVendor.Enabled = false;
                txtMasterVendor.Text = txtDisplayName.Text;
                txtMasterVendorCode.Text = txtVendorCode.Text;
            }
            else
            {
                txtMasterVendor.Enabled = true;
            }
        }

        private void txtDisplayName_TextChanged(object sender, EventArgs e)
        {
            if (m_screemMode == eScreenMode.Add)
            {
                txtEnglishName.Text = txtDisplayName.Text;
                txtThaiName.Text = txtDisplayName.Text;
            }
            if (chkMasterVendor.Checked == true)
            {
                txtMasterVendor.Text = txtDisplayName.Text;
            }
        }

        private void txtVendorCode_TextChanged(object sender, EventArgs e)
        {
            if (m_screemMode == eScreenMode.Add && chkMasterVendor.Checked == true)
            {
                txtMasterVendorCode.Text = txtVendorCode.Text;
            }
        }

        private void btnAddPeriod_Click(object sender, EventArgs e)
        {
            //Master_VendorPreferPeriod frm = new Master_VendorPreferPeriod();
            //if (frm.ShowDialog() == DialogResult.OK)
            //{
            //    DataRow dr = ((DataTable)gvDetail.DataSource).NewRow();
            //    dr["start_date"] = frm.dtStartDate.Value.Date;
            //    dr["end_date"] = frm.dtEndDate.Value.Date;
            //    ((DataTable)gvDetail.DataSource).Rows.Add(dr);
            //}
        }

        private void gvDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvDetail.Rows.Count == 0)
                return;
            if (e.ColumnIndex == 2) //Del
            {
                DialogResult result = MessageBox.Show("Are you sure to delete the period?", "Vendor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DataRow dr = ((DataRowView)gvDetail.Rows[e.RowIndex].DataBoundItem).Row;
                    ((DataTable)gvDetail.DataSource).Rows.Remove(dr);
                }
            }
        }

        private void btnAddPeriodSignContract_Click(object sender, EventArgs e)
        {
            //Master_VendorContractPeriod frm = new Master_VendorContractPeriod();
            //if (frm.ShowDialog() == DialogResult.OK)
            //{
            //    DataRow dr = ((DataTable)gvSignContract.DataSource).NewRow();
            //    if (frm.rdYes.Checked)
            //        dr["Type"] = "Yes";
            //    else
            //        dr["Type"] = "No";
            //    dr["Start_Date"] = frm.dtStartDate.Value.Date;
            //    dr["End_Date"] = frm.dtEndDate.Value.Date;
            //    dr["Flag"] = "New";
            //    ((DataTable)gvSignContract.DataSource).Rows.Add(dr);
            //}
        }

        private void gvSignContract_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvSignContract.Rows.Count == 0)
                return;
            if (e.ColumnIndex == 3)
            {
                DialogResult result = MessageBox.Show("Are you sure to delete the period?", "Vendor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DataRow dr = ((DataRowView)gvSignContract.Rows[e.RowIndex].DataBoundItem).Row;
                    ((DataTable)gvSignContract.DataSource).Rows.Remove(dr);
                }
            }
        }

        private void btnAddPeriodEPD_Click(object sender, EventArgs e)
        {
            //Master_Client_VendorPeriod frm = new Master_Client_VendorPeriod();
            //frm.type = "Vendor";
            //frm.header = "EPD";
            //if (frm.ShowDialog() == DialogResult.OK)
            //{
            //    DataRow dr = ((DataTable)gvEPD.DataSource).NewRow();
            //    if (frm.rdRed.Checked)
            //        dr["Type"] = "Red";
            //    else
            //        dr["Type"] = "Green";
            //    dr["Start_Date"] = frm.dtStartDate.Value.Date;
            //    dr["End_Date"] = frm.dtEndDate.Value.Date;
            //    dr["Flag"] = "New";
            //    ((DataTable)gvEPD.DataSource).Rows.Add(dr);
            //}
        }

        private void gvEPD_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvEPD.Rows.Count == 0)
                return;
            if (e.ColumnIndex == 3)
            {
                DialogResult result = MessageBox.Show("Are you sure to delete the period?", "Vendor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DataRow dr = ((DataRowView)gvEPD.Rows[e.RowIndex].DataBoundItem).Row;
                    ((DataTable)gvEPD.DataSource).Rows.Remove(dr);
                }
            }
        }

        private void btnAddPeriodMediaCredit_Click(object sender, EventArgs e)
        {
            /*Master_Client_VendorPeriod frm = new Master_Client_VendorPeriod();
            frm.type = "Vendor";
            frm.header = "Media Credit";
            if (frm.ShowDialog() == DialogResult.OK)
            {
                DataRow dr = ((DataTable)gvMediaCredit.DataSource).NewRow();
                if (frm.rdRed.Checked)
                    dr["Type"] = "Red";
                else
                    dr["Type"] = "Green";
                dr["Start_Date"] = frm.dtStartDate.Value.Date;
                dr["End_Date"] = frm.dtEndDate.Value.Date;
                dr["Flag"] = "New";
                ((DataTable)gvMediaCredit.DataSource).Rows.Add(dr);
            }*/
        }

        private void gvMediaCredit_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            /*if (gvMediaCredit.Rows.Count == 0)
                return;
            if (e.ColumnIndex == 3)
            {
                DialogResult result = MessageBox.Show("Are you sure to delete the period?", "Vendor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DataRow dr = ((DataRowView)gvMediaCredit.Rows[e.RowIndex].DataBoundItem).Row;
                    ((DataTable)gvMediaCredit.DataSource).Rows.Remove(dr);
                }
            }*/
        }

        private void btnAddPeriodRebate_Click(object sender, EventArgs e)
        {
            //Master_Client_VendorPeriod frm = new Master_Client_VendorPeriod();
            //frm.type = "Vendor";
            //frm.header = "Rebate";
            //if (frm.ShowDialog() == DialogResult.OK)
            //{
            //    DataRow dr = ((DataTable)gvRebate.DataSource).NewRow();
            //    if (frm.rdRed.Checked)
            //        dr["Type"] = "Red";
            //    else
            //        dr["Type"] = "Green";
            //    dr["Start_Date"] = frm.dtStartDate.Value.Date;
            //    dr["End_Date"] = frm.dtEndDate.Value.Date;
            //    dr["Flag"] = "New";
            //    ((DataTable)gvRebate.DataSource).Rows.Add(dr);
            //}
        }

        private void gvRebate_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvRebate.Rows.Count == 0)
                return;
            if (e.ColumnIndex == 3)
            {
                DialogResult result = MessageBox.Show("Are you sure to delete the period?", "Vendor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DataRow dr = ((DataRowView)gvRebate.Rows[e.RowIndex].DataBoundItem).Row;
                    ((DataTable)gvRebate.DataSource).Rows.Remove(dr);
                }
            }
        }

        private void btnAddPeriodSAC_Click(object sender, EventArgs e)
        {
            //Master_Client_VendorPeriod frm = new Master_Client_VendorPeriod();
            //frm.type = "Vendor";
            //frm.header = "SAC";
            //if (frm.ShowDialog() == DialogResult.OK)
            //{
            //    DataRow dr = ((DataTable)gvSAC.DataSource).NewRow();
            //    if (frm.rdRed.Checked)
            //        dr["Type"] = "Red";
            //    else
            //        dr["Type"] = "Green";
            //    dr["Start_Date"] = frm.dtStartDate.Value.Date;
            //    dr["End_Date"] = frm.dtEndDate.Value.Date;
            //    dr["Flag"] = "New";
            //    ((DataTable)gvSAC.DataSource).Rows.Add(dr);
            //}
        }

        private void gvSAC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvSAC.Rows.Count == 0)
                return;
            if (e.ColumnIndex == 3)
            {
                DialogResult result = MessageBox.Show("Are you sure to delete the period?", "Vendor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DataRow dr = ((DataRowView)gvSAC.Rows[e.RowIndex].DataBoundItem).Row;
                    ((DataTable)gvSAC.DataSource).Rows.Remove(dr);
                }
            }
        }
    }
}
