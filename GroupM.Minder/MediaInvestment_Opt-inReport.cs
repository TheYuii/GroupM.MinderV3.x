using GroupM.DBAccess;
using GroupM.UTL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupM.Minder
{
    public partial class MediaInvestment_Opt_in_Report : Form
    {
        private string reportPath = ConfigurationManager.AppSettings["ReportMinder"];
        DBManager m_db;

        public MediaInvestment_Opt_in_Report()
        {
            InitializeComponent();
            m_db = new DBManager();
        }

        private void InitControl()
        {
            // variable
            int i = 0;
            int year = DateTime.Now.Year;
            DateTime startDate = new DateTime(year, 1, 1);
            DateTime endDate = new DateTime(year, 12, 31);
            string sql = "";
            // bind data
            sql = "select * from GroupProprietary";
            DataTable dtProprietaryGroup = m_db.SelectNonParameter(sql).Clone();
            DataRow dr = dtProprietaryGroup.NewRow();
            dr["GroupProprietaryName"] = "All";
            dr["GroupProprietaryId"] = 0;
            dtProprietaryGroup.Rows.Add(dr);

            DataTable dtPG = m_db.SelectNonParameter(sql);
            for (i = 0; i < dtPG.Rows.Count; i++)
            {
                DataRow drProprietaryGroup = dtPG.Rows[i];
                DataRow drInsert = dtProprietaryGroup.NewRow();
                drInsert.ItemArray = drProprietaryGroup.ItemArray;
                dtProprietaryGroup.Rows.Add(drInsert);
            }
            cboProprietaryGroup.DataSource = dtProprietaryGroup;
            cboProprietaryGroup.DisplayMember = "GroupProprietaryName";
            cboProprietaryGroup.ValueMember = "GroupProprietaryId";
            sql = "select * from Agency";
            DataTable dtAgency = m_db.SelectNonParameter(sql);
            cboAgencyCode.Items.Add("All");
            for (i = 0; i < dtAgency.Rows.Count; i++)
            {
                cboAgencyCode.Items.Add(dtAgency.Rows[i]["Agency_ID"].ToString());
            }
            // set control value
            dtOptInStartDate.Value = startDate;
            dtOptInEndDate.Value = endDate;
            cboProprietaryGroup.Text = "All";
            cboAgencyCode.Text = "All";
            cboRegion.Text = "All";
            cboStatus.Text = "All";
        }

        private void MediaInvestment_Opt_in_Report_Load(object sender, EventArgs e)
        {
            try
            {
                InitControl();
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex.Message);
            }
        }

        private void cboAgencyCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboAgencyCode.SelectedIndex > 0)
            {
                try
                {
                    DataTable dt = m_db.SelectAgency(cboAgencyCode.Text);
                    txtAgency.Text = dt.Rows[0]["Short_Name"].ToString();
                }
                catch (Exception ex)
                {
                    GMessage.MessageError(ex.Message);
                }
            }
            else
            {
                txtAgency.Text = "";
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dtOptInStartDate.Value > dtOptInEndDate.Value)
            {
                GMessage.MessageWarning("Opt-In end date must more than or equal start date.");
            }
            else
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable dt = m_db.SelectOptInReport(dtOptInStartDate.Value, dtOptInEndDate.Value, Convert.ToInt32(cboProprietaryGroup.SelectedValue), cboAgencyCode.Text, cboRegion.Text, cboStatus.Text);
                if (dt.Rows.Count == 0)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }
                string templatePath = reportPath + "Opt-in - Report.xltx";
                ExcelUtil.ExportTemplateXlsx(dt, 3, true, templatePath, "A");
                this.Cursor = Cursors.Default;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
