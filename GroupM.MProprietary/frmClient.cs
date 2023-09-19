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
    public partial class frmClient : Form
    {
        public frmClient()
        {
            InitializeComponent();
        }

        private DBAccess connect = new DBAccess();
        private DataTable dtGvdetail = new DataTable();

        private void DataLoading()
        {
            dtGvdetail = connect.SelectClient();
            gvDetail.AutoGenerateColumns = false;
            gvDetail.DataSource = dtGvdetail;
        }

        private void frmClient_Load(object sender, EventArgs e)
        {
            DataLoading();
            txtSearch.Focus();
        }
        
        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            string strKeyWord = txtSearch.Text.Trim();
            string strCondition = string.Format("Short_Name like '%{0}%'", strKeyWord);
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

        private void gvDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataRow dr = ((DataRowView)gvDetail.Rows[e.RowIndex].DataBoundItem).Row;
            frmClientMapping frm = new frmClientMapping(dr["Client_ID"].ToString(), dr["Short_Name"].ToString());
            frm.ShowDialog();
        }
    }
}
