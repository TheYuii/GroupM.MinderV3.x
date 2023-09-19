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
    public partial class Master_AdeptMediaType : Form
    {
        enum eScreenMode { Add, Edit, View }
        public DataRow drMediaType { get; set; }

        private string username = "";

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
                    txtAdeptMediaTypeCode.ReadOnly = true;
                    break;
                case eScreenMode.View:
                    txtAdeptMediaTypeCode.ReadOnly = true;
                    txtDisplayName.ReadOnly = true;
                    txtDescription.ReadOnly = true;
                    chkBillingRevenue.Enabled = false;
                    rdActive.Enabled = false;
                    rdInactive.Enabled = false;
                    dtInactiveDate.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    btnDelete.Enabled = false;
                    break;
            }
            m_screemMode = mode;
        }

        public Master_AdeptMediaType(string screenMode, string Username)
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
            DataTable dt = m_db.SelectAdeptMediaType(drMediaType["Adept_Media_Type"].ToString());
            DataRow dr = dt.Rows[0];
            txtAdeptMediaTypeCode.Text = dr["Adept_Media_Type"].ToString();
            txtDisplayName.Text = dr["Adept_Media_Type_Name"].ToString();
            chkBillingRevenue.Checked = dr["Billing_Type_Revenue"].ToString() == "True" ? true : false;
            txtDescription.Text = dr["Description"].ToString();

            if (Convert.ToBoolean(dr["IsActive"]))
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
                if (dr["Inactive_Date"] == DBNull.Value)
                    DateTimeConvertUtil.DateTimeSetEmpty(dtInactiveDate);
                else
                    dtInactiveDate.Value = Convert.ToDateTime(dr["Inactive_Date"]);
            }

            if (m_screemMode == eScreenMode.View)
                dtInactiveDate.Enabled = false;
        }

        private void InitControl()
        {
            DateTimeConvertUtil.DateTimeSetEmpty(dtInactiveDate);
        }

        private void Master_AdeptMediaType_Load(object sender, EventArgs e)
        {
            InitControl();
            if (m_screemMode == eScreenMode.Edit || m_screemMode == eScreenMode.View)
                Dataloading();
        }

        private void TxtAdeptMediaTypeCode_KeyPress(object sender, KeyPressEventArgs e)
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
            if (rdInactive.Checked)
                dtInactiveDate.Enabled = true;
            else
                dtInactiveDate.Enabled = false;
        }

        private bool CheckDataBeforeSave()
        {
            if (m_screemMode == eScreenMode.Add)
            {
                if (txtAdeptMediaTypeCode.Text == "")
                {
                    GMessage.MessageWarning("Please input Adept Media Type Code.");
                    txtAdeptMediaTypeCode.Focus();
                    return false;
                }
                if (m_db.SelectAdeptMediaType(txtAdeptMediaTypeCode.Text).Rows.Count != 0)
                {
                    GMessage.MessageWarning(string.Format("Please change Adept Media Type Code. The {0} already exist.", txtAdeptMediaTypeCode.Text));
                    txtAdeptMediaTypeCode.Focus();
                    return false;
                }
            }
            if (txtDisplayName.Text == "")
            {
                GMessage.MessageWarning("Please input Display Name.");
                txtDisplayName.Focus();
                return false;
            }
            return true;
        }

        private DataRow Packing()
        {
            DataTable dt = m_db.SelectAdeptMediaType("dummycolumn");
            DataRow dr = dt.NewRow();
            dr["Adept_Media_Type"] = txtAdeptMediaTypeCode.Text;
            dr["Adept_Media_Type_Name"] = txtDisplayName.Text;
            dr["Description"] = txtDescription.Text;
            dr["Billing_Type_Revenue"] = chkBillingRevenue.Checked ? 1 : 0; // bit
            dr["IsActive"] = rdActive.Checked ? 1 : 0; // bit
            if (rdInactive.Checked)
                dr["Inactive_Date"] = dtInactiveDate.Value;
            else
                dr["Inactive_Date"] = DBNull.Value;
            dr["Modify_By"] = username.Replace(".", "");
            dr["Modify_Date"] = DateTime.Now;
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
                    if (m_db.InsertAdeptMediaType(Packing()) == -1)
                        return false;
                    else
                        log = "Add Adept Media Type:" + txtAdeptMediaTypeCode.Text + ",Name :" + txtDisplayName.Text;
                }
                else
                {
                    if (m_db.UpdateAdeptMediaType(Packing()) == -1)
                        return false;
                    else
                        log = "Modify Adept Media Type:" + txtAdeptMediaTypeCode.Text + ",Name :" + txtDisplayName.Text;
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
            DialogResult result = MessageBox.Show("Are you sure to exit?", "Adept Media Type", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.Dispose();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (m_db.SelectAdeptMediaTypeIsUsing(txtAdeptMediaTypeCode.Text))
                {
                    string str = $"Cannot delete Adept Media Type: {txtAdeptMediaTypeCode.Text} - {txtDisplayName.Text}  because some Media(s) referred to this Adept Media Type.";
                    GMessage.MessageWarning(str);
                    return;
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            DialogResult result = MessageBox.Show("Are you sure to delete Adept Media Type: " + txtAdeptMediaTypeCode.Text + " - " + txtDisplayName.Text + "?", "Adept Media Type", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                m_db.DeleteAdeptMediaType(txtAdeptMediaTypeCode.Text);
                m_db.InsertLogMinder(username.Replace(".", ""), "Delete Adept Media Type:" + txtAdeptMediaTypeCode.Text + ",Name :" + txtDisplayName.Text, "Master File", username.Replace(".", ""));
                GMessage.MessageInfo("Delete Completed");
                DialogResult = DialogResult.Yes;
            }
        }
    }
}
