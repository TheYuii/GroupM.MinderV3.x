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
    public partial class Master_OfficeList : Form
    {
        DBManager m_db = null;
        private string username = "";
        private string screenmode = "";

        public Master_OfficeList(string Username, string ScreenPermission)
        {
            InitializeComponent();
            m_db = new DBManager();
            username = Username;
            screenmode = ScreenPermission;
        }

        private void InitailControl()
        {
            cboStatus.SelectedItem = "Active";
            ControlManipulate.ClearControl(txtOfficeCodeSearch, txtOfficeCode, txtOffice, txtDisplayName, txtAgencyCode, txtAgency);
            if (screenmode == "View")
                newToolStripMenuItem.Enabled = false;
        }

        private DataRow Packing()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("OfficeCode");
            dt.Columns.Add("Office_ID");
            dt.Columns.Add("Short_Name");
            dt.Columns.Add("Agency_ID");
            dt.Columns.Add("IsActive");
            DataRow dr = dt.NewRow();
            if (txtOfficeCodeSearch.Text != "")
                dr["OfficeCode"] = txtOfficeCodeSearch.Text;
            if (txtOfficeCode.Text != "")
                dr["Office_ID"] = txtOfficeCode.Text;
            if (txtDisplayName.Text != "")
                dr["Short_Name"] = txtDisplayName.Text;
            if (txtAgencyCode.Text != "")
                dr["Agency_ID"] = txtAgencyCode.Text;
            if (cboStatus.Text != "All")
            {
                if (cboStatus.Text == "Active")
                    dr["IsActive"] = 1;
                else
                    dr["IsActive"] = 0;
            }
            return dr;
        }

        private void DataLoading()
        {
            DataTable dt = m_db.SelectOffice(Packing());
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

        private void Master_OfficeList_Load(object sender, EventArgs e)
        {
            InitailControl();
            DataLoading();
        }

        private void TxtOffice_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Office_ID", "Short_Name", "Office", true, "", txtAgencyCode.Text);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtOffice.Text = frm.SelectedGridRow["MasterName"].ToString();
                txtOfficeCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
            }
        }

        private void TxtOffice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void TxtOffice_TextChanged(object sender, EventArgs e)
        {
            if (txtOffice.Text == "")
            {
                txtOfficeCode.Text = "";
            }
        }

        private void TxtAgency_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Agency_ID", "Short_Name", "Agency", true, "", "");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtAgency.Text = frm.SelectedGridRow["MasterName"].ToString();
                txtAgencyCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
            }
        }

        private void TxtAgency_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void TxtAgency_TextChanged(object sender, EventArgs e)
        {
            if (txtAgency.Text == "")
            {
                txtAgencyCode.Text = "";
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

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Master_Office frm = new Master_Office("Add", username);
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
                Master_Office frm = new Master_Office(screenmode, username);
                frm.drOffice = dr;
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

        private void GvDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            editToolStripMenuItem.PerformClick();
        }

        private void AllOfficeMasterFileListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dt = m_db.SelectOfficeMasterFileReport();
            this.Cursor = Cursors.WaitCursor;
            ExcelUtil.ExportXlsx(dt, 1, true);
            this.Cursor = Cursors.Default;
        }
    }
}
