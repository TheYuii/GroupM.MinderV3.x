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

namespace GroupM.Minder
{
    public partial class Master_MediaSubType : Form
    {
        enum eScreenMode { Add, Edit, View }
        public DataRow drMediaSubType { get; set; }

        private string username = "";

        private Regex regex = new Regex(@"[^a-zA-Z\d\s]");

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
                    txtMediaSubTypeCode.ReadOnly = true;
                    break;
                case eScreenMode.View:
                    txtMediaSubTypeCode.ReadOnly = true;
                    txtDisplayName.ReadOnly = true;
                    txtMediaType.ReadOnly = true;
                    cboServiceGroup.Enabled = false;
                    cboBillingGroup.Enabled = false;
                    txtMediaSubTypeCoreM.ReadOnly = true;
                    rdActive.Enabled = false;
                    rdInactive.Enabled = false;
                    txtBusinessDefinition.Enabled = false;
                    chkShowOnBB.Enabled = false;
                    chkFeeLock.Enabled = false;
                    chkForecast.Enabled = false;
                    txtJobTypeCode.ReadOnly = true;
                    txtPrefix.ReadOnly = true;
                    chkBillingRevenue.Enabled = false;
                    chkMediaCode.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    btnDelete.Enabled = false;
                    break;
            }
            m_screemMode = mode;
        }

        public Master_MediaSubType(string screenMode, string Username)
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

        private void Dataloading()
        {
            DataTable dt = m_db.SelectMediaSubType(drMediaSubType["Media_Sub_Type"].ToString());
            DataRow dr = dt.Rows[0];
            txtMediaSubTypeCode.Text = dr["Media_Sub_Type"].ToString();
            txtDisplayName.Text = dr["Short_Name"].ToString();
            txtMediaType.Text = dr["MediaTypeName"].ToString();
            txtMediaTypeCode.Text = dr["Media_Type"].ToString();
            cboServiceGroup.Text = dr["Service_Group"].ToString();
            cboBillingGroup.Text = dr["Billing_Group"].ToString();
            txtMediaSubTypeCoreM.Text = dr["MST_CoreM_Name"].ToString();
            txtMediaSubTypeCoreMCode.Text = dr["Media_Sub_Type_Mapping_CoreM_New"].ToString();
            if (Convert.ToBoolean(dr["isActive"]))
            {
                rdActive.Checked = true;
                rdInactive.Checked = false;
                DateTimeConvertUtil.DateTimeSetEmpty(dtInactiveDate);
            }
            else
            {
                rdActive.Checked = false;
                rdInactive.Checked = true;
                DateTimeConvertUtil.DateTimeRestoreFormat(dtInactiveDate);
                if (dr["InactiveDate"] == DBNull.Value)
                    DateTimeConvertUtil.DateTimeSetEmpty(dtInactiveDate);
                else
                    dtInactiveDate.Value = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(dr["InactiveDate"].ToString());
            }
            if (dr["BusinessDefinition"] != null && dr["BusinessDefinition"] != DBNull.Value)
                txtBusinessDefinition.Text = dr["BusinessDefinition"].ToString();
            chkShowOnBB.Checked = dr["Show_BB"].ToString() == "Y" ? true : false;
            chkFeeLock.Checked = Convert.ToBoolean(dr["FeeLocked_SP"]);
            chkForecast.Checked = Convert.ToBoolean(dr["Forecast_Input"]);
            txtJobTypeCode.Text = dr["Adept_Export_Mapping"].ToString();
            txtPrefix.Text = dr["AdeptExport_Prefix"].ToString();
            chkBillingRevenue.Checked = Convert.ToBoolean(dr["BillingType_Revenue"]);
            chkMediaCode.Checked = Convert.ToBoolean(dr["Adept_MergewithMedia"]);
            if (m_screemMode == eScreenMode.View)
                dtInactiveDate.Enabled = false;
        }

        private void InitControl()
        {
            DateTimeConvertUtil.DateTimeSetEmpty(dtInactiveDate);
            DataTable dtServiceGroup = m_db.SelectServiceGroup();
            foreach (DataRow dr in dtServiceGroup.Rows)
            {
                cboServiceGroup.Items.Add(dr["ServiceGroup"]);
            }
            DataTable dtBillingGroup = m_db.SelectBillingGroup();
            foreach (DataRow dr in dtBillingGroup.Rows)
            {
                cboBillingGroup.Items.Add(dr["Billing_Group"]);
            }
        }

        private void Master_MediaSubType_Load(object sender, EventArgs e)
        {
            InitControl();
            if (m_screemMode == eScreenMode.Edit || m_screemMode == eScreenMode.View)
                Dataloading();
        }

        private void TxtMediaSubTypeCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)3 && e.KeyChar != (char)22 && e.KeyChar != (char)24 && regex.IsMatch(e.KeyChar.ToString()))
            {
                e.Handled = true;
            }
        }

        private void TxtMediaSubTypeCode_TextChanged(object sender, EventArgs e)
        {
            if (m_screemMode == eScreenMode.Add)
            {
                txtJobTypeCode.Text = txtMediaSubTypeCode.Text;
            }
        }

        private void TxtMediaType_Click(object sender, EventArgs e)
        {
            if (m_screemMode == eScreenMode.Add || m_screemMode == eScreenMode.Edit)
            {
                Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Media_Type", "Short_Name", "Media_Type", false, "", "");
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    txtMediaType.Text = frm.SelectedGridRow["MasterName"].ToString();
                    txtMediaTypeCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
                }
            }
        }

        private void TxtMediaType_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void TxtMediaType_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Clipboard.Clear();
            }
        }

        private void TxtMediaType_TextChanged(object sender, EventArgs e)
        {
            if (txtMediaType.Text == "")
            {
                txtMediaTypeCode.Text = "";
            }
        }

        private void txtMediaSubTypeCoreM_Click(object sender, EventArgs e)
        {
            if (m_screemMode == eScreenMode.Add || m_screemMode == eScreenMode.Edit)
            {
                Utility_SearchPopup_CodeName3Col frm = new Utility_SearchPopup_CodeName3Col("TB.Media_Sub_Type", "TB.Short_Name", "MT.Short_Name", "Media Type", "Media_Sub_Type", false, "", "");
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    txtMediaSubTypeCoreM.Text = frm.SelectedGridRow["MasterName"].ToString();
                    txtMediaSubTypeCoreMCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
                }
            }
        }

        private void txtMediaSubTypeCoreM_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtMediaSubTypeCoreM_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Clipboard.Clear();
            }
        }

        private void txtMediaSubTypeCoreM_TextChanged(object sender, EventArgs e)
        {
            if (txtMediaSubTypeCoreM.Text == "")
            {
                txtMediaSubTypeCoreMCode.Text = "";
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

        private void TxtPrefix_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) == false && char.IsDigit(e.KeyChar) == false && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private bool CheckDataBeforeSave()
        {
            if (m_screemMode == eScreenMode.Add)
            {
                if (txtMediaSubTypeCode.Text == "")
                {
                    GMessage.MessageWarning("Please input Media Sub Type Code.");
                    txtMediaSubTypeCode.Focus();
                    return false;
                }
                if (m_db.SelectMediaSubType(txtMediaSubTypeCode.Text).Rows.Count != 0)
                {
                    GMessage.MessageWarning(string.Format("Please change Media Sub Type Code. The {0} already exist.", txtMediaSubTypeCode.Text));
                    txtMediaSubTypeCode.Focus();
                    return false;
                }
            }
            if (txtDisplayName.Text == "")
            {
                GMessage.MessageWarning("Please input Display Name.");
                txtDisplayName.Focus();
                return false;
            }
            if (txtMediaTypeCode.Text == "")
            {
                GMessage.MessageWarning("Please input Media Type Name.");
                txtMediaType.Focus();
                return false;
            }
            if (cboServiceGroup.Text == "")
            {
                GMessage.MessageWarning("Please select Service Group.");
                cboServiceGroup.Focus();
                return false;
            }
            if (cboBillingGroup.Text == "")
            {
                GMessage.MessageWarning("Please select Billing Group.");
                cboBillingGroup.Focus();
                return false;
            }
            if (txtMediaSubTypeCoreMCode.Text == "")
            {
                GMessage.MessageWarning("Please input Media Sub Type Core M Name.");
                txtMediaSubTypeCoreM.Focus();
                return false;
            }
            return true;
        }

        private DataRow Packing()
        {
            DataTable dt = m_db.SelectMediaSubType("dummycolumn");
            DataRow dr = dt.NewRow();
            dr["Media_Sub_Type"] = txtMediaSubTypeCode.Text;
            dr["Short_Name"] = txtDisplayName.Text;
            dr["Media_Type"] = txtMediaTypeCode.Text;
            dr["Service_Group"] = cboServiceGroup.Text;
            dr["Billing_Group"] = cboBillingGroup.Text;
            dr["Media_Sub_Type_Mapping_CoreM_New"] = txtMediaSubTypeCoreMCode.Text;
            dr["isActive"] = rdActive.Checked ? 1 : 0; // bit
            if (rdInactive.Checked)
            {
                dr["InactiveDate"] = dtInactiveDate.Value.ToString("yyyyMMdd");
            }
            else
            {
                dr["InactiveDate"] = DBNull.Value;
            }
            dr["BusinessDefinition"] = txtBusinessDefinition.Text;
            dr["Show_BB"] = chkShowOnBB.Checked ? "Y" : "N";
            dr["FeeLocked_SP"] = chkFeeLock.Checked ? 1 : 0; // bit
            dr["Forecast_Input"] = chkForecast.Checked ? 1 : 0; // bit
            dr["Adept_Export_Mapping"] = txtJobTypeCode.Text;
            dr["AdeptExport_Prefix"] = txtPrefix.Text;
            dr["BillingType_Revenue"] = chkBillingRevenue.Checked ? 1 : 0; // bit
            dr["Adept_MergewithMedia"] = chkMediaCode.Checked ? 1 : 0; // bit
            dr["User_ID"] = username.Replace(".", "");
            dr["Modify_Date"] = DBNull.Value;
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
                    if (m_db.InsertMediaSubType(Packing()) == -1)
                        return false;
                    else
                        log = "Add Media Sub Type:" + txtMediaSubTypeCode.Text + ",Name :" + txtDisplayName.Text + ",Media Type:" + txtMediaTypeCode.Text + "(" + txtMediaType.Text + ")";
                }
                else
                {
                    if (m_db.UpdateMediaSubType(Packing()) == -1)
                    {
                        return false;
                    }
                    else
                    {
                        log = "Modify Media Sub Type:" + txtMediaSubTypeCode.Text + ",Name :" + txtDisplayName.Text + ",Media Type:" + txtMediaTypeCode.Text + "(" + txtMediaType.Text + ")";
                    }
                }
                m_db.InsertLogMinder(username.Replace(".", ""), log, "Master File", username.Replace(".", ""));
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
            DialogResult result = MessageBox.Show("Are you sure to exit?", "Media Sub Type", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Dispose();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (m_db.SelectMediaSubTypeIsUsing(txtMediaSubTypeCode.Text))
            {
                string str = $"Cannot delete Media Sub Type: {txtMediaSubTypeCode.Text} - {txtDisplayName.Text}  because some Buying Brief(s) or some Spot Plan(s) referred to this Media Sub Type.";
                GMessage.MessageWarning(str);
                return;
            }
            DialogResult result = MessageBox.Show("Are you sure to delete Media Sub Type: " + txtMediaSubTypeCode.Text + " - " + txtDisplayName.Text + "?", "Media Sub Type", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                m_db.DeleteMediaSubType(txtMediaSubTypeCode.Text);
                m_db.InsertLogMinder(username.Replace(".", ""), "Delete Media Sub Type:" + txtMediaSubTypeCode.Text + ",Name :" + txtDisplayName.Text, "Master File", username.Replace(".", ""));
                GMessage.MessageInfo("Delete Completed");
                DialogResult = DialogResult.Yes;
            }
        }
    }
}
