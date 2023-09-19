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
    public partial class Implementation_BuyingBrief_IncidentTracking : Form
    {
        enum eScreenMode { Add, Edit }

        public DataRow drTracking { get; set; }

        private string username = "";
        private string bbid = "";

        eScreenMode m_screemMode;

        DBManager m_db;

        private void SetScreenMode(eScreenMode mode)
        {
            m_screemMode = mode;
        }

        public Implementation_BuyingBrief_IncidentTracking(string screenMode, string Username, string BBID)
        {
            InitializeComponent();
            username = Username;
            bbid = BBID;
            m_db = new DBManager();
            if (screenMode == "Add")
                SetScreenMode(eScreenMode.Add);
            else
                SetScreenMode(eScreenMode.Edit);
        }

        private string GenerateIncidentID()
        {
            int maxNo = 0;
            DataTable dt = m_db.SelectMaxIncedentTracking(bbid);
            if (dt.Rows.Count > 0)
            {
                maxNo = Convert.ToInt32(dt.Rows[0]["Max_No"]);
            }
            string incident = bbid + "-" + (maxNo + 1).ToString();
            return incident;
        }

        private void Dataloading()
        {
            DataTable dt = m_db.SelectIncedentTrackingHeader(drTracking["Incident_ID"].ToString());
            DataRow dr = dt.Rows[0];
            txtTrackingID.Text = dr["Incident_ID"].ToString();
            txtTitle.Text = dr["Title"].ToString();
            txtRemark.Text = dr["Remark"].ToString();
            dtIncidentDate.Value = Convert.ToDateTime(dr["Incident_Date"].ToString());
            cboStatus.Text = dr["Status"].ToString();
            txtDescription.Text = dr["Description"].ToString();
            DataTable dtDetail = m_db.SelectIncedentTrackingHistory(dr["Incident_ID"].ToString());
            gvDetail.AutoGenerateColumns = false;
            gvDetail.DataSource = dtDetail;
        }

        private void Implementation_BuyingBrief_IncidentTracking_Load(object sender, EventArgs e)
        {
            if (m_screemMode == eScreenMode.Add)
            {
                txtTrackingID.Text = GenerateIncidentID();
                cboStatus.SelectedItem = "New";
            }
            else
            {
                Dataloading();
            }
        }

        private bool CheckDataBeforeSave()
        {
            if (txtTitle.Text == "")
            {
                GMessage.MessageWarning("Please input Title.");
                txtTitle.Focus();
                return false;
            }
            if (txtDescription.Text == "")
            {
                GMessage.MessageWarning("Please input Description.");
                txtDescription.Focus();
                return false;
            }
            return true;
        }

        private DataRow Packing()
        {
            DataTable dt = m_db.SelectIncedentTrackingHeader("dummycolumn");
            DataRow dr = dt.NewRow();
            dr["Incident_ID"] = txtTrackingID.Text;
            dr["Buying_Brief_ID"] = bbid;
            dr["Status"] = cboStatus.Text;
            dr["Title"] = txtTitle.Text;
            dr["Description"] = txtDescription.Text;
            dr["Incident_Date"] = dtIncidentDate.Value;
            dr["Create_Date"] = m_screemMode == eScreenMode.Add ? DateTime.Now : drTracking["Create_Date"];
            if (m_screemMode == eScreenMode.Edit)
                dr["Modify_Date"] = DateTime.Now;
            dr["User_ID"] = username.Replace(".", "");
            dr["Remark"] = txtRemark.Text;
            return dr;
        }

        private bool OnCommandSave()
        {
            if (!CheckDataBeforeSave())
                return false;
            try
            {
                if (m_screemMode == eScreenMode.Add)
                {
                    if (m_db.InsertIncidentTracking(Packing()) == -1)
                        return false;
                }
                else
                {
                    if (m_db.UpdateIncidentTracking(Packing()) == -1)
                        return false;
                }
                GMessage.MessageInfo("Save Completed");
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OnCommandSave())
                DialogResult = DialogResult.Yes;
        }

        private void CancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to exit?", "Incident Tracking", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Dispose();
            }
        }
    }
}
