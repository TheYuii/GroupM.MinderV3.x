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
    public partial class Implementation_SpotPlan_Popup_Media : Form
    {
        public string MasterCode { get; set; }
        public string MasterName { get; set; }
        public string MasterTable { get; set; }
        public string Username { get; set; }
        public string Filter { get; set; }
        public DataRow SelectedGridRow { get; set; }
        public bool IncludeInactive { get; set; }

        DBManager m_db;
        public Implementation_SpotPlan_Popup_Media(string strCode, string strName, string strTable, bool bIncludeInactive, string strUsername, string strFilter)
        {
            InitializeComponent();
            MasterCode = strCode;
            MasterName = strName;
            MasterTable = strTable;
            IncludeInactive = bIncludeInactive;
            Username = strUsername;
            Filter = strFilter;
            m_db = new DBManager();

        }
        public Implementation_SpotPlan_Popup_Media(string selectByMasterMediaByMediaTypeOrByMediaSubType,string strValue)
        {
            InitializeComponent();
            m_selectBy = selectByMasterMediaByMediaTypeOrByMediaSubType;
            m_value = strValue;
            m_db = new DBManager();

        }
        DataTable dtMaster = null;
        private string m_selectBy = "MediaType";
        private string m_value;

        private void Implementation_SpotPlan_Popup_Media_Load(object sender, EventArgs e)
        {
            gvDetail.AutoGenerateColumns = false;
            //if(m_selectBy == "MediaSubType")
            //    dtMaster = m_db.SelectMediaByMediaSubType(m_value);
            //else if (m_selectBy == "MediaType")
            //    dtMaster = m_db.SelectMediaByMediaType(m_value);
            //else if (m_selectBy == "MasterMediaType")
            //    dtMaster = m_db.SelectMediaByMasterMediaType(m_value);
            gvDetail.DataSource = dtMaster;
        }

        private void txtClient_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                DataRow[] rowsToCopy = dtMaster.Select(string.Format("Media_ID like '%{0}%' OR Short_Name like '%{0}%' OR Media_Sub_Type like '%{0}%' OR Media_Sub_Type_Name like '%{0}%' OR Media_Type like '%{0}%' OR Media_Type_Name like '%{0}%'", txtClient.Text.Replace("'", "")));
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
