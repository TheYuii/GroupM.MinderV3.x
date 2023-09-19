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
    public partial class frmMediaList : Form
    {
        public frmMediaList()
        {
            InitializeComponent();
        }
        
        private DBAccess connect = new DBAccess();
        private DataTable dtGvdetail = new DataTable();

        private void DataLoading()
        {
            dtGvdetail = connect.SelectMediaList();
            gvDetail.AutoGenerateColumns = false;
            gvDetail.DataSource = dtGvdetail;
            DataTable dt = dtGvdetail.Clone();
            foreach (DataRow dr in dtGvdetail.Rows.Cast<DataRow>().Take(100))
            {
                dt.Rows.Add(dr.ItemArray);
            }
            gvDetail.DataSource = dt;
        }

        private void AddSelectData()
        {
            if (gvDetail.Rows.Count == 0)
            {
                MessageBox.Show("Media not found! ", "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (gvDetail.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select media! ", "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    DialogResult = DialogResult.OK;
                }
            }
        }

        private void frmMediaList_Load(object sender, EventArgs e)
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
                string strCondition = string.Format("Media_ID like '%{0}%' or Media_Name like '%{0}%' or Media_Sub_Type_ID like '%{0}%' or Media_Sub_Type_Name like '%{0}%' or Media_Type_ID like '%{0}%' or Media_Type_Name like '%{0}%'", strKeyWord);
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

        private void gvDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            AddSelectData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddSelectData();
        }
    }
}
