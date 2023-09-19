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
    public partial class Implementation_BuyingBrief_IncidentTrackingList : Form
    {
        DBManager m_db = null;
        private string username = "";
        private string bbid = "";

        public Implementation_BuyingBrief_IncidentTrackingList(string Username, string BBID)
        {
            InitializeComponent();
            m_db = new DBManager();
            username = Username;
            bbid = BBID;
        }
        
        private void Implementation_BuyingBrief_IncidentTrackingList_Load(object sender, EventArgs e)
        {
            DataLoading();
        }
        
        private void DataLoading()
        {
            DataTable dt = m_db.SelectBuyingBrief(bbid);
            if (dt.Rows.Count == 0)
            {
                GMessage.MessageInfo("No found Buying Brief.");
            }
            else
            {
                txtBB.Text = dt.Rows[0]["Buying_Brief_ID"].ToString();
                txtStatus.Text = dt.Rows[0]["CampaignStatus"].ToString();
                txtCampaign.Text = dt.Rows[0]["Description"].ToString();
                dt = m_db.SelectIncedentTracking(bbid);
                gvDetail.AutoGenerateColumns = false;
                gvDetail.DataSource = dt;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Implementation_BuyingBrief_IncidentTracking frm = new Implementation_BuyingBrief_IncidentTracking("Add", username, bbid);
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                DataLoading();
            }
            this.Cursor = Cursors.Default;
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gvDetail.Rows.Count == 0)
                return;
            try
            {
                this.Cursor = Cursors.WaitCursor;

                DataRow dr = ((DataRowView)gvDetail.Rows[gvDetail.SelectedRows[0].Index].DataBoundItem).Row;
                Implementation_BuyingBrief_IncidentTracking frm = new Implementation_BuyingBrief_IncidentTracking("Edit", username, bbid);
                frm.drTracking = dr;
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

        private void gvDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            editToolStripMenuItem.PerformClick();
        }
    }
}
