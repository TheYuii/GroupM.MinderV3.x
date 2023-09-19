using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MProprietary
{
    public partial class frmProprietary : Form
    {
        public frmProprietary()
        {
            InitializeComponent();
        }

        private DBAccess connect = new DBAccess();
        private DataTable dtGvdetail = new DataTable();

        private void DataLoading()
        {
            dtGvdetail = connect.SelectProprietaryGroup();
            gvDetail.AutoGenerateColumns = false;
            gvDetail.DataSource = dtGvdetail;
        }

        private void RefreshData()
        {
            dtGvdetail.Clear();
            dtGvdetail = connect.SelectProprietaryGroup();
            gvDetail.DataSource = dtGvdetail;
        }

        private void frmProprietary_Load(object sender, EventArgs e)
        {
            DataLoading();
            txtSearch.Focus();
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (dtGvdetail.Rows.Count > 0)
            {
                StringReplace sr = new StringReplace();
                string strKeyWord = sr.ReplaceSpecialCharacter(txtSearch.Text.Trim());
                string strCondition = string.Format("GroupProprietaryName like '%{0}%' or GroupProprietaryDescription like '%{0}%'", strKeyWord);
                DataRow[] rowsToCopy = dtGvdetail.Select(strCondition);
                DataTable dt = dtGvdetail.Clone();
                int iRowCount = 0;
                foreach (DataRow dr in rowsToCopy)
                {
                    iRowCount++;
                    dt.Rows.Add(dr.ItemArray);
                    if (iRowCount == 100)
                    {
                        break;
                    }
                }
                gvDetail.DataSource = dt;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable dt = dtGvdetail.Clone();
            DataRow dr = dt.NewRow();
            dr["GroupProprietaryId"] = 0;
            frmProprietaryMapping frm = new frmProprietaryMapping(dr);
            frm.ShowDialog();
            RefreshData();
        }

        private void gvDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataRow dr = ((DataRowView)gvDetail.Rows[e.RowIndex].DataBoundItem).Row;
            frmProprietaryMapping frm = new frmProprietaryMapping(dr);
            frm.ShowDialog();
            RefreshData();
        }

        private void gvDetail_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to delete this proprietary group? ", "MProprietary", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                DataRow dr = ((DataRowView)e.Row.DataBoundItem).Row;
                // check proprietary group use by some client
                DataTable dtCheck = connect.SelectProprietaryGroupInClient((int)dr["GroupProprietaryId"]);
                // not found client
                if (dtCheck.Rows.Count == 0)
                {
                    connect.DeleteProprietaryGroup((int)dr["GroupProprietaryId"]);
                }
                else
                {
                    // found some client
                    MessageBox.Show("Cannot delete!, Because this proprietary group is belong to any clients.", "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                }
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
