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
    public partial class Master_ClientAgencyFee : Form
    {
        public Master_ClientAgencyFee()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //if (txtBrandCode.Text == ""
            //    || txtBrandName.Text == "")
            //{
            //    GMessage.MessageWarning("Please input all data");
            //    return;
            //}
            DialogResult = DialogResult.OK;
        }

        private void Master_ClientAgencyFee_Load(object sender, EventArgs e)
        {

            m_db = new DBManager();
            InitControl();
        }

        DBManager m_db;

        private void InitControl()
        {
            DataTable dt = m_db.SelectMediaType(true);
            cboMediaType.DataSource = dt;
            cboMediaType.DisplayMember = "Short_Name";
            cboMediaType.ValueMember = "Media_Type";

            DataTable dt2 = m_db.SelectMediaSubType_DisplayMediaType(true);
            cboMediaSubType.DataSource = dt2;
            cboMediaSubType.DisplayMember = "Short_Name";
            cboMediaSubType.ValueMember = "Media_Sub_Type";

            cboOutdoorCostType.SelectedIndex = 0;
        }

        private void rdMediaType_CheckedChanged(object sender, EventArgs e)
        {
            if (rdMediaType.Checked)
            {
                cboMediaType.Enabled = true;
                cboMediaSubType.Enabled = false;
                cboOutdoorCostType.Enabled = false;
            }
        }

        private void rdMediaSubType_CheckedChanged(object sender, EventArgs e)
        {
            if (rdMediaSubType.Checked)
            {
                cboMediaType.Enabled = false;
                cboMediaSubType.Enabled = true;
                cboOutdoorCostType.Enabled = false;
            }
        }

        private void rdOutdoorCostType_CheckedChanged(object sender, EventArgs e)
        {
            if (rdOutdoorCostType.Checked)
            {
                cboMediaType.Enabled = false;
                cboMediaSubType.Enabled = false;
                cboOutdoorCostType.Enabled = true;
            }
        }

        private void rdXaxis_CheckedChanged(object sender, EventArgs e)
        {
            if (rdXaxis.Checked)
            {
                cboMediaType.Enabled = false;
                cboMediaSubType.Enabled = false;
                cboOutdoorCostType.Enabled = false;
            }

        }

        private void rdINCA_CheckedChanged(object sender, EventArgs e)
        {
            if (rdINCA.Checked)
            {
                cboMediaType.Enabled = false;
                cboMediaSubType.Enabled = false;
                cboOutdoorCostType.Enabled = false;
            }
        }
    }
}
