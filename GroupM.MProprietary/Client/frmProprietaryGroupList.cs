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
    public partial class frmProprietaryGroupList : Form
    {
        public frmProprietaryGroupList()
        {
            InitializeComponent();
        }
        
        private DBAccess connect = new DBAccess();
        private DataTable dtGvdetail = new DataTable();
        public string filter;
        public ArrayList SelectedGridRow = new ArrayList();

        private void DataLoading()
        {
            dtGvdetail = connect.SelectProprietaryGroupList(filter);
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
                MessageBox.Show("Proprietary group not found! ", "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (gvDetail.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select proprietary group! ", "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    for (int i = 0; i < gvDetail.SelectedRows.Count; i++)
                    {
                        SelectedGridRow.Add(((DataRowView)gvDetail.SelectedRows[i].DataBoundItem).Row);
                    }
                    DialogResult = DialogResult.OK;
                }
            }
        }

        private void frmProprietaryGroupList_Load(object sender, EventArgs e)
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

        private void gvDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            AddSelectData();
        }

        private void gvDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (gvDetail.Rows.Count > 0 && gvDetail.SelectedRows.Count > 0)
                {
                    DataRow dr = ((DataRowView)gvDetail.SelectedRows[0].DataBoundItem).Row;
                    frmProprietaryPermission frm = new frmProprietaryPermission(dr);
                    frm.Show();
                    e.Handled = false;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddSelectData();
        }
    }
}
