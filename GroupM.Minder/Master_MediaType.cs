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
    public partial class Master_MediaType : Form
    {
        enum eScreenMode { Add, Edit, View }
        public DataRow drMediaType { get; set; }

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
                    txtMediaTypeCode.ReadOnly = true;
                    break;
                case eScreenMode.View:
                    txtMediaTypeCode.ReadOnly = true;
                    txtDisplayName.ReadOnly = true;
                    txtMasterMediaType.ReadOnly = true;
                    cboMediaTypeGroup.Enabled = false;
                    rdActive.Enabled = false;
                    rdInactive.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    btnDelete.Enabled = false;
                    break;
            }
            m_screemMode = mode;
        }

        public Master_MediaType(string screenMode, string Username)
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
            DataTable dt = m_db.SelectMediaType(drMediaType["Media_Type"].ToString());
            DataRow dr = dt.Rows[0];
            txtMediaTypeCode.Text = dr["Media_Type"].ToString();
            txtDisplayName.Text = dr["Short_Name"].ToString();
            
            chkMaster.Checked = dr["IsMaster"].ToString() == "1" ? true : false;
            if (dr["IsMaster"].ToString() == "1")
                txtMasterMediaType.Enabled = false;
            else
                txtMasterMediaType.Enabled = true;
            txtMasterMediaType.Text = dr["Master_Media_Type_Name"].ToString();
            txtMasterMediaTypeCode.Text = dr["Master_Media_Type"].ToString();
            cboMediaTypeGroup.Text = dr["Media_Type_Group"].ToString();
            txtDescription.Text = dr["Description"].ToString();

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
            
            if (m_screemMode == eScreenMode.View)
                dtInactiveDate.Enabled = false;
        }

        private void InitControl()
        {
            DateTimeConvertUtil.DateTimeSetEmpty(dtInactiveDate);
            DataTable dtMediaTypeGroup = m_db.SelectMediaTypeGroup();
            foreach (DataRow dr in dtMediaTypeGroup.Rows)
            {
                cboMediaTypeGroup.Items.Add(dr["Media_Type_Group"]);
            }
        }

        private void Master_MediaType_Load(object sender, EventArgs e)
        {
            InitControl();
            if (m_screemMode == eScreenMode.Edit || m_screemMode == eScreenMode.View)
                Dataloading();
        }

        private void TxtMediaTypeCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)3 && e.KeyChar != (char)22 && e.KeyChar != (char)24 && regex.IsMatch(e.KeyChar.ToString()))
            {
                e.Handled = true;
            }
        }

        private void TxtMediaType_Click(object sender, EventArgs e)
        {
            if (m_screemMode == eScreenMode.Add || m_screemMode == eScreenMode.Edit)
            {
                Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Media_Type", "Short_Name", "Master_Media_Type", false, "", "");
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    txtMasterMediaType.Text = frm.SelectedGridRow["MasterName"].ToString();
                    txtMasterMediaTypeCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
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
            if (txtMasterMediaType.Text == "")
            {
                txtMasterMediaTypeCode.Text = "";
            }
        }

        private void RdActive_CheckedChanged(object sender, EventArgs e)
        {
            DateTimeConvertUtil.DateTimeSetEmpty(dtInactiveDate);
        }

        private void RdInactive_CheckedChanged(object sender, EventArgs e)
        {
            DateTimeConvertUtil.DateTimeRestoreFormat(dtInactiveDate);
            if(rdInactive.Checked)
                dtInactiveDate.Enabled = true;
            else
                dtInactiveDate.Enabled = false;
        }

        private bool CheckDataBeforeSave()
        {
            if (m_screemMode == eScreenMode.Add)
            {
                if (txtMediaTypeCode.Text == "")
                {
                    GMessage.MessageWarning("Please input Media Type Code.");
                    txtMediaTypeCode.Focus();
                    return false;
                }
                if (m_db.SelectMediaType(txtMediaTypeCode.Text).Rows.Count != 0)
                {
                    GMessage.MessageWarning(string.Format("Please change Media Type Code. The {0} already exist.", txtMediaTypeCode.Text));
                    txtMediaTypeCode.Focus();
                    return false;
                }
            }
            if (txtDisplayName.Text == "")
            {
                GMessage.MessageWarning("Please input Display Name.");
                txtDisplayName.Focus();
                return false;
            }
            if (txtMasterMediaTypeCode.Text == "")
            {
                GMessage.MessageWarning("Please input Media Type Name.");
                txtMasterMediaType.Focus();
                return false;
            }
            if (cboMediaTypeGroup.Text == "")
            {
                GMessage.MessageWarning("Please select Media Type Group.");
                cboMediaTypeGroup.Focus();
                return false;
            }
            return true;
        }

        private DataRow Packing()
        {
            DataTable dt = m_db.SelectMediaType("dummycolumn");
            DataRow dr = dt.NewRow();
            dr["Media_Type"] = txtMediaTypeCode.Text;
            dr["Short_Name"] = txtDisplayName.Text;
            dr["Master_Media_Type"] = txtMasterMediaTypeCode.Text;
            dr["Media_Type_Group"] = cboMediaTypeGroup.Text;
            dr["Description"] = txtDescription.Text;
            dr["IsMaster"] = chkMaster.Checked ? 1 : 0; // bit
            dr["IsActive"] = rdActive.Checked ? 1 : 0; // bit
            if (rdInactive.Checked)
            {
                dr["InactiveDate"] = dtInactiveDate.Value.ToString("yyyyMMdd");
            }
            else
            {
                dr["InactiveDate"] = DBNull.Value;
            }
            dr["User_ID"] = username.Replace(".", "");
            dr["Modify_Date"] = DateTime.Now.ToString("yyyyMMdd");
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
                    if (m_db.InsertMediaType(Packing()) == -1)
                        return false;
                    else
                        log = "Add Media Type:" + txtMediaTypeCode.Text + ",Name :" + txtDisplayName.Text + ",Media Type:" + txtMasterMediaTypeCode.Text + "(" + txtMasterMediaType.Text + ")";
                }
                else
                {
                    if (m_db.UpdateMediaType(Packing()) == -1)
                    {
                        return false;
                    }
                    else
                    {
                        log = "Modify Media Type:" + txtMediaTypeCode.Text + ",Name :" + txtDisplayName.Text + ",Media Type:" + txtMasterMediaTypeCode.Text + "(" + txtMasterMediaType.Text + ")";
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
            DialogResult result = MessageBox.Show("Are you sure to exit?", "Media Type", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Dispose();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (m_db.SelectMediaTypeIsUsing(txtMediaTypeCode.Text))
                {
                    string str = $"Cannot delete Media Type: {txtMediaTypeCode.Text} - {txtDisplayName.Text}  because some Buying Brief(s) or some Spot Plan(s) referred to this Media Type.";
                    GMessage.MessageWarning(str);
                    return;
                }
            }
            finally {
                Cursor = Cursors.Default;
            }
            DialogResult result = MessageBox.Show("Are you sure to delete Media Type: " + txtMediaTypeCode.Text + " - " + txtDisplayName.Text + "?", "Media Type", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                m_db.DeleteMediaType(txtMediaTypeCode.Text);
                m_db.InsertLogMinder(username.Replace(".", ""), "Delete Media Type:" + txtMediaTypeCode.Text + ",Name :" + txtDisplayName.Text, "Master File", username.Replace(".", ""));
                GMessage.MessageInfo("Delete Completed");
                DialogResult = DialogResult.Yes;
            }
        }

        private void chkMaster_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMaster.Checked == true)
            {
                txtMasterMediaType.Enabled = false;
                txtMasterMediaType.Text = txtDisplayName.Text;
                txtMasterMediaTypeCode.Text = txtMediaTypeCode.Text;
            }
            else
            {
                if (m_db.SelectMasterMediaTypeIsUsing(txtMediaTypeCode.Text))
                {
                    string listMediaType = "";
                    DataTable dt = m_db.SelectMediaTypeInMasterMediaType(txtMediaTypeCode.Text);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (listMediaType == "")
                            listMediaType += dt.Rows[i]["Media_Type"].ToString() + " - " + dt.Rows[i]["Short_Name"].ToString();
                        else
                            listMediaType += ",\n" + dt.Rows[i]["Media_Type"].ToString() + " - " + dt.Rows[i]["Short_Name"].ToString();
                    }
                    string str = "Cannot remove Master Media Type Flag because some Media Type(s) has referred as Master Media Type.\n" +
                    "Please change a new Master Media Type to those Media Type first.\n" +
                    listMediaType;
                    GMessage.MessageWarning(str);
                    chkMaster.Checked = true;
                }
                else
                {
                    txtMasterMediaType.Enabled = true;
                }
            }
        }
    }
}
