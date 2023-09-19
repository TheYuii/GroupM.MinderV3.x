using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using GroupM.UTL;
using GroupM.DBAccess;

namespace GroupM.Minder.SpotPlanIT
{
    public partial class Implementation_SpotPlan_Popup_BuyType : Form
    {
        public string MasterCode { get; set; }
        public string MasterName { get; set; }
        public string MasterTable { get; set; }
        public string Username { get; set; }
        public string Filter { get; set; }
        public DataRow SelectedGridRow { get; set; }
        public bool IncludeInactive { get; set; }
        private string m_value = "";

        DBManager m_db;
        public Implementation_SpotPlan_Popup_BuyType()
        {
            InitializeComponent();
            m_db = new DBManager();

        }
        DataTable dtMaster = null;

        private void Implementation_SpotPlan_Popup_BuyType_Load(object sender, EventArgs e)
        {
            gvDetail.AutoGenerateColumns = false;
            dtMaster = m_db.SelectBuyType();
            gvDetail.DataSource = dtMaster;
        }

        private void txtClient_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                DataRow[] rowsToCopy = dtMaster.Select(string.Format("BuyTypeID like '%{0}%' OR BuyTypeName like '%{0}%' OR BuyTypeDisplay like '%{0}%'", txtClient.Text.Replace("'", "")));
                DataTable dtNew = dtMaster.Clone();
                foreach (DataRow dr in rowsToCopy)
                {
                    dtNew.Rows.Add(dr.ItemArray);
                }
                gvDetail.DataSource = dtNew;
            }
            catch (Exception ex)
            {
                //GMessage.MessageError(ex);
            }
        }

        private void gvDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvDetail.Rows.Count > 0)
            {
                SelectedGridRow = ((DataRowView)gvDetail.CurrentRow.DataBoundItem).Row;
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void txtClient_Click(object sender, EventArgs e)
        {
            //InputLanguage lang = GetUrduLanguage();
            //if (lang == null)
            //{
            //    GMessage.MessageWarning("Thai Kedmanee Language Keyboard not installed.");
            //}
            //InputLanguage.CurrentInputLanguage = lang;
        }

        private InputLanguage GetUrduLanguage()
        {
            foreach (InputLanguage lang in InputLanguage.InstalledInputLanguages)
            {
                if (lang.LayoutName == "Thai Kedmanee")
                    return lang;
            }
            return null;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Escape))
            {
                this.Close();
                return true;
            }
            else if (keyData == Keys.Enter)
            {
                if (gvDetail.Rows.Count <= 0)
                    return true;
                this.Close();
                SelectedGridRow = ((DataRowView)gvDetail.CurrentRow.DataBoundItem).Row;
                DialogResult = System.Windows.Forms.DialogResult.OK;
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
