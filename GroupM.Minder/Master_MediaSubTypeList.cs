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
    public partial class Master_MediaSubTypeList : Form
    {
        DBManager m_db = null;
        private string username = "";
        private string screenmode = "";

        public Master_MediaSubTypeList(string Username, string ScreenPermission)
        {
            InitializeComponent();
            m_db = new DBManager();
            username = Username;
            screenmode = ScreenPermission;
        }

        private void InitailControl()
        {
            cboStatus.SelectedItem = "Active";
            ControlManipulate.ClearControl(txtMediaSubTypeCodeSearch, txtMediaSubTypeCode, txtMediaSubType, txtDisplayName, txtMediaTypeCode, txtMediaType);
            if (screenmode == "View")
                newToolStripMenuItem.Enabled = false;
        }

        private DataRow Packing()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MediaSubTypeCode");
            dt.Columns.Add("Media_Sub_Type");
            dt.Columns.Add("Short_Name");
            dt.Columns.Add("Media_Type");
            dt.Columns.Add("isActive");
            DataRow dr = dt.NewRow();
            if (txtMediaSubTypeCodeSearch.Text != "")
                dr["MediaSubTypeCode"] = txtMediaSubTypeCodeSearch.Text;
            if (txtMediaSubTypeCode.Text != "")
                dr["Media_Sub_Type"] = txtMediaSubTypeCode.Text;
            if (txtDisplayName.Text != "")
                dr["Short_Name"] = txtDisplayName.Text;
            if (txtMediaTypeCode.Text != "")
                dr["Media_Type"] = txtMediaTypeCode.Text;
            if (cboStatus.Text != "All")
            {
                if (cboStatus.Text == "Active")
                    dr["isActive"] = 1;
                else
                    dr["isActive"] = 0;
            }
            return dr;
        }

        private void DataLoading()
        {
            DataTable dt = m_db.SelectMediaSubType(Packing());
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

        private void Master_MediaSubTypeList_Load(object sender, EventArgs e)
        {
            InitailControl();
            DataLoading();
        }

        private void TxtMediaSubType_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Media_Sub_Type", "Short_Name", "Media_Sub_Type", true, "", txtMediaTypeCode.Text);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtMediaSubType.Text = frm.SelectedGridRow["MasterName"].ToString();
                txtMediaSubTypeCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
            }
        }

        private void TxtMediaSubType_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void TxtMediaSubType_TextChanged(object sender, EventArgs e)
        {
            if (txtMediaSubType.Text == "")
            {
                txtMediaSubTypeCode.Text = "";
            }
        }

        private void TxtMediaType_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Media_Type", "Short_Name", "Media_Type", true, "", "");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtMediaType.Text = frm.SelectedGridRow["MasterName"].ToString();
                txtMediaTypeCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
            }
        }

        private void TxtMediaType_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void TxtMediaType_TextChanged(object sender, EventArgs e)
        {
            if (txtMediaType.Text == "")
            {
                txtMediaTypeCode.Text = "";
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
            Master_MediaSubType frm = new Master_MediaSubType("Add", username);
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
                Master_MediaSubType frm = new Master_MediaSubType(screenmode, username);
                frm.drMediaSubType = dr;
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

        private void mediaSubTypeBusinessDefinitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Master_MediaSubType_Report frm = new Master_MediaSubType_Report();
            frm.Show();
        }

        private void GvDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            editToolStripMenuItem.PerformClick();
        }
    }
}
