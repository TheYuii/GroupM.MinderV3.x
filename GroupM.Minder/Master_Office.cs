using GroupM.DBAccess;
using GroupM.UTL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupM.Minder
{
    public partial class Master_Office : Form
    {
        enum eScreenMode { Add, Edit, View }
        public DataRow drOffice { get; set; }

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
                    txtOfficeCode.ReadOnly = true;
                    break;
                case eScreenMode.View:
                    txtOfficeCode.ReadOnly = true;
                    txtDisplayName.ReadOnly = true;
                    txtThaiName.ReadOnly = true;
                    txtEnglishName.ReadOnly = true;
                    txtAgency.ReadOnly = true;
                    txtAddress.ReadOnly = true;
                    txtOfficeTelNo.ReadOnly = true;
                    txtFax.ReadOnly = true;
                    txtRemark.ReadOnly = true;
                    rdActive.Enabled = false;
                    rdInactive.Enabled = false;
                    txtEmailTo.ReadOnly = true;
                    btnEmailTo.Enabled = false;
                    gvEmailTo.Enabled = false;
                    txtEmailCc.ReadOnly = true;
                    btnEmailCc.Enabled = false;
                    gvEmailCc.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    btnDelete.Enabled = false;
                    break;
            }
            m_screemMode = mode;
        }

        public Master_Office(string screenMode, string Username)
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

        private DataTable TableEmail(string column)
        {
            DataTable table = new DataTable();
            table.Columns.Add(column, typeof(string));
            return table;
        }

        private void InitControl()
        {
            DateTimeConvertUtil.DateTimeSetEmpty(dtInactiveDate);
            DataTable dtEmailTo = TableEmail("To_Email");
            gvEmailTo.AutoGenerateColumns = false;
            gvEmailTo.DataSource = dtEmailTo;
            DataTable dtEmailCc = TableEmail("Cc_Email");
            gvEmailCc.AutoGenerateColumns = false;
            gvEmailCc.DataSource = dtEmailCc;
        }

        private DataTable TextToTable(string column, string listEmail)
        {
            DataTable table = TableEmail(column);
            string[] emails = listEmail.Replace(" ", "").Split(';');
            for (int i = 0; i < emails.Length; i++)
                if (emails[i] != "")
                    table.Rows.Add(emails[i]);
            return table;
        }

        private void Dataloading()
        {
            DataTable dt = m_db.SelectOffice(drOffice["Office_ID"].ToString());
            DataRow dr = dt.Rows[0];
            txtOfficeCode.Text = dr["Office_ID"].ToString();
            txtDisplayName.Text = dr["Short_Name"].ToString();
            txtThaiName.Text = dr["Thai_Name"].ToString();
            txtEnglishName.Text = dr["English_Name"].ToString();
            txtAgency.Text = dr["AgencyName"].ToString();
            txtAgencyCode.Text = dr["Agency_ID"].ToString();
            txtAddress.Text = dr["Address"].ToString();
            txtOfficeTelNo.Text = dr["Tel"].ToString();
            txtFax.Text = dr["Fax"].ToString();
            txtRemark.Text = dr["Remark"].ToString();
            if (dr["IsActive"].ToString() == "1")
            {
                rdActive.Checked = true;
                rdInactive.Checked = false;
                DateTimeConvertUtil.DateTimeSetEmpty(dtInactiveDate);
            }
            else
            {
                rdActive.Checked = false;
                rdInactive.Checked = true;
                dtInactiveDate.Enabled = true;
                DateTimeConvertUtil.DateTimeRestoreFormat(dtInactiveDate);
                if (dr["InactiveDate"] == DBNull.Value)
                    DateTimeConvertUtil.DateTimeSetEmpty(dtInactiveDate);
                else
                    dtInactiveDate.Value = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(dr["InactiveDate"].ToString());
            }
            gvEmailTo.DataSource = TextToTable("To_Email", dr["To_Email"].ToString());
            gvEmailCc.DataSource = TextToTable("Cc_Email", dr["Cc_Email"].ToString());
            if (m_screemMode == eScreenMode.View)
                dtInactiveDate.Enabled = false;
        }

        private void Master_Office_Load(object sender, EventArgs e)
        {
            InitControl();
            if (m_screemMode == eScreenMode.Edit || m_screemMode == eScreenMode.View)
                Dataloading();
        }

        private void TxtOfficeCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)3 && e.KeyChar != (char)22 && e.KeyChar != (char)24 && regex.IsMatch(e.KeyChar.ToString()))
            {
                e.Handled = true;
            }
        }

        private void TxtAgency_Click(object sender, EventArgs e)
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

        private void TxtAgency_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void TxtAgency_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Clipboard.Clear();
            }
        }

        private void TxtAgency_TextChanged(object sender, EventArgs e)
        {
            if (txtAgency.Text == "")
            {
                txtAgencyCode.Text = "";
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

        private bool CheckDuplicateEmail(string email, DataTable table)
        {
            bool duplicate = false;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i][0].ToString() == email)
                {
                    duplicate = true;
                }
            }
            return duplicate;
        }

        private bool ValidateEmail(string email)
        {
            bool valid = false;
            try
            {
                MailAddress mail = new MailAddress(email);
                if (mail.Address == email)
                {
                    string[] splitMail = email.Split('@');
                    string emailTail = splitMail[1];
                    valid = m_db.SelectEmailTail(emailTail);
                }
                else
                {
                    valid = false;
                }
            }
            catch
            {
                valid = false;
            }
            return valid;
        }

        private void BtnEmailTo_Click(object sender, EventArgs e)
        {
            bool duplicate = CheckDuplicateEmail(txtEmailTo.Text, (DataTable)gvEmailTo.DataSource);
            if (duplicate == true)
            {
                GMessage.MessageWarning(string.Format("This email is duplicate, Please input another email."));
                txtEmailTo.Focus();
            }
            else
            {
                bool validate = ValidateEmail(txtEmailTo.Text);
                if (validate == true)
                {
                    if (gvEmailTo.DataSource == null)
                        gvEmailTo.DataSource = TableEmail("To_Email");
                    DataRow dr = ((DataTable)gvEmailTo.DataSource).NewRow();
                    dr["To_Email"] = txtEmailTo.Text;
                    ((DataTable)gvEmailTo.DataSource).Rows.Add(dr);
                    txtEmailTo.Text = "";
                }
                else
                {
                    GMessage.MessageWarning(string.Format("Please input a correct email."));
                    txtEmailTo.Focus();
                }
            }
        }

        private void GvEmailTo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvEmailTo.Rows.Count == 0)
                return;
            if (e.ColumnIndex == 1) //Del
            {
                DataRow dr = ((DataRowView)gvEmailTo.Rows[e.RowIndex].DataBoundItem).Row;
                ((DataTable)gvEmailTo.DataSource).Rows.Remove(dr);
            }
        }

        private void BtnEmailCc_Click(object sender, EventArgs e)
        {
            bool duplicate = CheckDuplicateEmail(txtEmailCc.Text, (DataTable)gvEmailCc.DataSource);
            if (duplicate == true)
            {
                GMessage.MessageWarning(string.Format("This email is duplicate, Please input another email."));
                txtEmailCc.Focus();
            }
            else
            {
                bool validate = ValidateEmail(txtEmailCc.Text);
                if (validate == true)
                {
                    if (gvEmailCc.DataSource == null)
                        gvEmailCc.DataSource = TableEmail("Cc_Email");
                    DataRow dr = ((DataTable)gvEmailCc.DataSource).NewRow();
                    dr["Cc_Email"] = txtEmailCc.Text;
                    ((DataTable)gvEmailCc.DataSource).Rows.Add(dr);
                    txtEmailCc.Text = "";
                }
                else
                {
                    GMessage.MessageWarning(string.Format("Please input a correct email."));
                    txtEmailCc.Focus();
                }
            }
        }

        private void GvEmailCc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvEmailCc.Rows.Count == 0)
                return;
            if (e.ColumnIndex == 1) //Del
            {
                DataRow dr = ((DataRowView)gvEmailCc.Rows[e.RowIndex].DataBoundItem).Row;
                ((DataTable)gvEmailCc.DataSource).Rows.Remove(dr);
            }
        }

        private bool CheckDataBeforeSave()
        {
            if (m_screemMode == eScreenMode.Add)
            {
                if (txtOfficeCode.Text == "")
                {
                    GMessage.MessageWarning("Please input Office Code.");
                    txtOfficeCode.Focus();
                    return false;
                }
                if (m_db.SelectOffice(txtOfficeCode.Text).Rows.Count != 0)
                {
                    GMessage.MessageWarning(string.Format("Please change Office Code. The {0} already exist.", txtOfficeCode.Text));
                    txtOfficeCode.Focus();
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
            if (txtAgencyCode.Text == "")
            {
                GMessage.MessageWarning("Please input Agency Name.");
                txtAgency.Focus();
                return false;
            }
            if (gvEmailTo.Rows.Count == 0)
            {
                GMessage.MessageWarning("Please input Email To.");
                txtEmailTo.Focus();
                return false;
            }
            return true;
        }

        private string TableToText(DataTable table)
        {
            string line = "";
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (i == table.Rows.Count - 1)
                        line += table.Rows[i][0].ToString();
                    else
                        line += table.Rows[i][0].ToString() + "; ";
                }
            }
            return line;
        }

        private string SetSingleLine(string text)
        {
            string line = text.Replace("\r", "").Replace("\n", " ");
            return line;
        }

        private DataRow Packing()
        {
            DataTable dt = m_db.SelectOffice("dummycolumn");
            DataRow dr = dt.NewRow();
            dr["Office_ID"] = txtOfficeCode.Text;
            dr["Thai_Name"] = txtThaiName.Text;
            dr["English_Name"] = txtEnglishName.Text;
            dr["Short_Name"] = txtDisplayName.Text;
            dr["Agency_ID"] = txtAgencyCode.Text;
            dr["Address"] = txtAddress.Text;
            dr["Tel"] = txtOfficeTelNo.Text;
            dr["Fax"] = txtFax.Text;
            dr["Remark"] = SetSingleLine(txtRemark.Text);
            dr["IsActive"] = rdActive.Checked ? 1 : 0; // int
            if (rdInactive.Checked)
            {
                dr["InactiveDate"] = dtInactiveDate.Value.ToString("yyyyMMdd");
            }
            else
            {
                dr["InactiveDate"] = DBNull.Value;
            }
            dr["To_Email"] = TableToText((DataTable)gvEmailTo.DataSource);
            dr["Cc_Email"] = TableToText((DataTable)gvEmailCc.DataSource);
            dr["User_ID"] = username.Replace(".", "");
            dr["Modify_Date"] = DBNull.Value;
            dr["Creative_Agency_ID"] = DBNull.Value;
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
                    if (m_db.InsertOffice(Packing()) == -1)
                        return false;
                    else
                        log = "Add Office ID:" + txtOfficeCode.Text + ",Name :" + txtDisplayName.Text + ",Agency:" + txtAgencyCode.Text + "(" + txtAgency.Text + ")";
                }
                else
                {
                    if (m_db.UpdateOffice(Packing()) == -1)
                    {
                        return false;
                    }
                    else
                    {
                        log = "Modify Office ID:" + txtOfficeCode.Text + ",Name :" + txtDisplayName.Text + ",Agency:" + txtAgencyCode.Text + "(" + txtAgency.Text + ")";
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
            DialogResult result = MessageBox.Show("Are you sure to exit?", "Office", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Dispose();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (m_db.SelectOfficeIsUsing(txtOfficeCode.Text))
            {
                string str = $"Cannot delete Office Code: {txtOfficeCode.Text} - {txtDisplayName.Text}  because some Buying Brief(s) referred to this Office.";
                GMessage.MessageWarning(str);
                return;
            }
            DialogResult result = MessageBox.Show("Are you sure to delete Office: " + txtOfficeCode.Text + " - " + txtDisplayName.Text + "?", "Office", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                m_db.DeleteOffice(txtOfficeCode.Text);
                m_db.InsertLogMinder(username.Replace(".", ""), "Delete Office ID:" + txtOfficeCode.Text + ",Name :" + txtDisplayName.Text, "Master File", username.Replace(".", ""));
                GMessage.MessageInfo("Delete Completed");
                DialogResult = DialogResult.Yes;
            }
        }
    }
}
