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

namespace GroupM.Minder
{
    public partial class Implementation_SpotPlan_Popup_RateCard : Form
    {
        public DataRow SelectedGridRow { get; set; }
        private string m_selectBy = "MediaType";
        private string m_value;
        DBManager m_db;
        DataTable dtMaster = null;
        string m_strStartDate = "";
        string m_strEndDate = "";

        public Implementation_SpotPlan_Popup_RateCard(string selectByMasterMediaByMediaTypeOrByMediaSubType, string strValue,string strStartDate,string strEndDate)
        {
            InitializeComponent();
            m_selectBy = selectByMasterMediaByMediaTypeOrByMediaSubType;
            m_value = strValue;
            m_strStartDate = strStartDate;
            m_strEndDate = strEndDate;

            m_db = new DBManager();

        }

        private void Implementation_SpotPlan_Popup_RateCard_Load(object sender, EventArgs e)
        {
            gvDetail.AutoGenerateColumns = false;
            dtMaster = m_db.SelectRateCardByFilter(m_selectBy, m_value, m_strStartDate, m_strEndDate);
            gvDetail.DataSource = dtMaster;
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                DataRow[] rowsToCopy = dtMaster.Select(string.Format("Media_ID like '%{0}%' OR Media_Name like '%{0}%' OR Media_Sub_Type_Name like '%{0}%' OR Media_Type_Name like '%{0}%' OR Position like '%{0}%'", txtSearch.Text.Replace("'", "")));
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
                DialogResult = DialogResult.OK;
            }
        }
    }
}
