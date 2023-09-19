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
    public partial class Master_AdeptMediaTypeList : Form
    {
        DBManager m_db = null;
        private string username = "";
        private string screenmode = "";

        public Master_AdeptMediaTypeList(string Username, string ScreenPermission)
        {
            InitializeComponent();
            m_db = new DBManager();
            username = Username;
            screenmode = ScreenPermission;
        }

        private void InitailControl()
        {
            cboStatus.SelectedItem = "Active";
            ControlManipulate.ClearControl(txtAdeptMediaTypeCodeSearch, txtAdeptMediaTypeCode, txtAdeptMediaType, txtDisplayName);
            if (screenmode == "View")
                newToolStripMenuItem.Enabled = false;
        }

        private DataRow Packing()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("AdeptMediaTypeCode");
            dt.Columns.Add("Adept_Media_Type");
            dt.Columns.Add("Adept_Media_Type_Name");
            dt.Columns.Add("IsActive");
            DataRow dr = dt.NewRow();
            if (txtAdeptMediaTypeCodeSearch.Text != "")
                dr["AdeptMediaTypeCode"] = txtAdeptMediaTypeCodeSearch.Text;
            if (txtAdeptMediaTypeCode.Text != "")
                dr["Adept_Media_Type"] = txtAdeptMediaTypeCode.Text;
            if (txtDisplayName.Text != "")
                dr["Adept_Media_Type_Name"] = txtDisplayName.Text;
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
            DataTable dt = m_db.SelectAdeptMediaType(Packing());
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

        private void Master_AdeptMediaType_Load(object sender, EventArgs e)
        {
            InitailControl();
            DataLoading();
        }

        private void txtAdeptMediaType_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Adept_Media_Type", "Adept_Media_Type_Name", "Adept_Media_Type", true, "", "");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtAdeptMediaType.Text = frm.SelectedGridRow["MasterName"].ToString();
                txtAdeptMediaTypeCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
            }
        }

        private void txtAdeptMediaType_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtAdeptMediaType_TextChanged(object sender, EventArgs e)
        {
            if (txtAdeptMediaType.Text == "")
            {
                txtAdeptMediaTypeCode.Text = "";
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
            Master_AdeptMediaType frm = new Master_AdeptMediaType("Add", username);
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
                Master_AdeptMediaType frm = new Master_AdeptMediaType(screenmode, username);
                frm.drMediaType = dr;
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
    }
}
