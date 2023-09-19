using GroupM.DBAccess;
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
    public partial class SpotPlan_SelectMedia : Form
    {
        public string MasterCode { get; set; }
        public string MasterName { get; set; }
        public string MasterTable { get; set; }
        public string Username { get; set; }
        public DataRow SelectedGridRow { get; set; }
        private DBManager m_db;
        DataTable dtMaster = null;


        public SpotPlan_SelectMedia(string strCode, string strName, string strTable, DataTable dtSelected, string strUsername)
        {
            InitializeComponent();

            MasterCode = strCode;
            MasterName = strName;
            MasterTable = strTable;
            Username = strUsername;

            gvDetail.DataSource = dtSelected;
            m_db = new DBManager();
        }

        private void SpotPlan_SelectMedia_Load(object sender, EventArgs e)
        {
            gvLeft.AutoGenerateColumns = false;
            dtMaster = m_db.SelectMasterCommon(MasterCode, MasterName, MasterTable, false, Username, "");
            gvLeft.DataSource = dtMaster;

            foreach (DataRow dr1 in ((DataTable)gvDetail.DataSource).Rows)
            {
                foreach (DataRow dr2 in ((DataTable)gvLeft.DataSource).Rows)
                {
                    if (dr1[0].ToString() == dr2[0].ToString())
                    {
                        ((DataTable)gvLeft.DataSource).Rows.Remove(dr2);
                        break;
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (gvLeft.SelectedRows.Count == 0)
                return;
            int iSelect = gvLeft.SelectedRows.Count;
            for (int i = iSelect - 1; i >= 0; i--)
            {
                DataRow dr = ((DataRowView)gvLeft.SelectedRows[i].DataBoundItem).Row;

                DataTable dt = (DataTable)gvDetail.DataSource;
                DataRow drInsert = dt.NewRow();
                drInsert.ItemArray = dr.ItemArray;
                dt.Rows.Add(drInsert);

                ((DataTable)gvLeft.DataSource).Rows.Remove(dr);
            }
        }

        private void btnAddAll_Click(object sender, EventArgs e)
        {
            if (gvLeft.SelectedRows.Count == 0)
                return;
            for (int i = 0; ((DataTable)gvLeft.DataSource).Rows.Count > 0; i++)
            {
                DataRow dr = ((DataTable)gvLeft.DataSource).Rows[0];

                DataTable dt = (DataTable)gvDetail.DataSource;
                DataRow drInsert = dt.NewRow();
                drInsert.ItemArray = dr.ItemArray;
                dt.Rows.Add(drInsert);

                ((DataTable)gvLeft.DataSource).Rows.Remove(dr);
            }
        }

        private void gvLeft_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnAdd.PerformClick();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (gvDetail.SelectedRows.Count == 0)
                return;
            int iSelect = gvDetail.SelectedRows.Count;
            for (int i = iSelect - 1; i >= 0; i--)
            {
                DataRow dr = ((DataRowView)gvDetail.SelectedRows[i].DataBoundItem).Row;

                DataTable dt = (DataTable)gvLeft.DataSource;
                DataRow drInsert = dt.NewRow();
                drInsert.ItemArray = dr.ItemArray;
                dt.Rows.Add(drInsert);

                ((DataTable)gvDetail.DataSource).Rows.Remove(dr);
            }
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            if (gvDetail.SelectedRows.Count == 0)
                return;
            for (int i = 0; ((DataTable)gvDetail.DataSource).Rows.Count > 0; i++)
            {
                DataRow dr = ((DataTable)gvDetail.DataSource).Rows[0];

                DataTable dt = (DataTable)gvLeft.DataSource;
                DataRow drInsert = dt.NewRow();
                drInsert.ItemArray = dr.ItemArray;
                dt.Rows.Add(drInsert);

                ((DataTable)gvDetail.DataSource).Rows.Remove(dr);
            }
        }

        private void gvDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnRemove.PerformClick();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
