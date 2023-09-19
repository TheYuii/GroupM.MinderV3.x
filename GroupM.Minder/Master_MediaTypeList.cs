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
    public partial class Master_MediaTypeList : Form
    {
        DBManager m_db = null;
        private string username = "";
        private string screenmode = "";

        public Master_MediaTypeList(string Username, string ScreenPermission)
        {
            InitializeComponent();
            m_db = new DBManager();
            username = Username;
            screenmode = ScreenPermission;
        }

        private void InitailControl()
        {
            cboStatus.SelectedItem = "Active";
            ControlManipulate.ClearControl(txtMediaTypeCodeSearch, txtMediaTypeCode, txtMediaType, txtMasterMediaTypeCode, txtMasterMediaType, txtDisplayName);
            if (screenmode == "View")
                newToolStripMenuItem.Enabled = false;
        }

        private DataRow Packing()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MediaTypeCode");
            dt.Columns.Add("MediaType_ID");
            dt.Columns.Add("Short_Name");
            dt.Columns.Add("Master_Media_Type");
            dt.Columns.Add("IsActive");
            DataRow dr = dt.NewRow();
            if (txtMediaTypeCodeSearch.Text != "")
                dr["MediaTypeCode"] = txtMediaTypeCodeSearch.Text;
            if (txtMediaTypeCode.Text != "")
                dr["MediaType_ID"] = txtMediaTypeCode.Text;
            if (txtMasterMediaTypeCode.Text != "")
                dr["Master_Media_Type"] = txtMasterMediaTypeCode.Text;
            if (txtDisplayName.Text != "")
                dr["Short_Name"] = txtDisplayName.Text;
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
            DataTable dt = m_db.SelectMediaType(Packing());
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

        private void Master_MediaTypeList_Load(object sender, EventArgs e)
        {
            InitailControl();
            DataLoading();
        }

        private void TxtOffice_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Media_Type", "Short_Name", "Master_Media_Type", true, "", "");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtMasterMediaType.Text = frm.SelectedGridRow["MasterName"].ToString();
                txtMasterMediaTypeCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
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
            if (txtMasterMediaType.Text == "")
            {
                txtMasterMediaTypeCode.Text = "";
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
            Master_MediaType frm = new Master_MediaType("Add", username);
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
                Master_MediaType frm = new Master_MediaType(screenmode, username);
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

        private void AllOfficeMasterFileListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dt = m_db.SelectOfficeMasterFileReport();
            this.Cursor = Cursors.WaitCursor;
            ExcelUtil.ExportXlsx(dt, 1, true);
            this.Cursor = Cursors.Default;
        }

        private void txtMediaType_TextChanged(object sender, EventArgs e)
        {
            if (txtMediaType.Text == "")
            {
                txtMediaTypeCode.Text = "";
            }
        }

        private void txtMediaType_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtMediaType_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Media_Type", "Short_Name", "Media_Type", true, "", "");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtMediaType.Text = frm.SelectedGridRow["MasterName"].ToString();
                txtMediaTypeCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
            }
        }
    }
}
