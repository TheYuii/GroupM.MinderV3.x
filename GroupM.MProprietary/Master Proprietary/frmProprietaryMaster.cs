using System;
using System.Collections;
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
    public partial class frmProprietaryMaster : Form
    {
        public frmProprietaryMaster()
        {
            InitializeComponent();
        }

        private DBAccess connect = new DBAccess();
        private DataTable dtGvdetail = new DataTable();

        private void DataLoading()
        {
            dtGvdetail = connect.SelectProprietaryMaster();
            gvDetail.AutoGenerateColumns = false;
            gvDetail.DataSource = dtGvdetail;
        }

        private void RefreshData()
        {
            dtGvdetail.Clear();
            dtGvdetail = connect.SelectProprietaryMaster();
            gvDetail.DataSource = dtGvdetail;
        }

        private void frmProprietaryMaster_Load(object sender, EventArgs e)
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
                string strCondition = string.Format("Media_Vendor_ID like '%{0}%' or Media_Vendor_Name like '%{0}%' or Media_ID like '%{0}%' or Media_Name like '%{0}%'", strKeyWord);
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
            frmProprietaryMasterAdd frm = new frmProprietaryMasterAdd();
            frm.ShowDialog();
            RefreshData();
        }

        private void gvDetail_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to delete this proprietary? ", "MProprietary", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                DataRow dr = ((DataRowView)e.Row.DataBoundItem).Row;
                // check proprietary master use by some proprietary group
                DataTable dtCheck = connect.SelectProprietaryInGroup((int)dr["GroupProprietaryVendorId"]);
                // not found proprietary group
                if (dtCheck.Rows.Count == 0)
                {
                    connect.DeleteProprietaryMaster((int)dr["GroupProprietaryVendorId"]);
                }
                else
                {
                    // found some proprietary group
                    MessageBox.Show("Cannot delete!, Because this proprietary is used in proprietary group.", "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
