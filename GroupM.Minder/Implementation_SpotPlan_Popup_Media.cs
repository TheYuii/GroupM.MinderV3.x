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
    public partial class Implementation_SpotPlan_Popup_Media : Form
    {
        public DataRow SelectedGridRow { get; set; }
        public string m_SD;
        public string m_ED;
        private string m_selectBy;
        private string m_value;
        DBManager m_db;
        DataTable dtMaster = null;

        public Implementation_SpotPlan_Popup_Media(string selectByMasterMediaByMediaTypeOrByMediaSubType, string strValue)
        {
            InitializeComponent();
            m_selectBy = selectByMasterMediaByMediaTypeOrByMediaSubType;
            m_value = strValue;
            m_db = new DBManager();

        }

        private void Implementation_SpotPlan_Popup_Media_Load(object sender, EventArgs e)
        {
            gvDetail.AutoGenerateColumns = false;
            if (m_selectBy == "MasterMediaType" || m_selectBy == "MediaType" || m_selectBy == "MediaSubType")
                dtMaster = m_db.SelectMediaByFilter(m_selectBy, m_value);
            else if (m_selectBy == "MasterMediaTypePeriod")
                dtMaster = m_db.SelectMediaByMasterMediaTypePeriod(m_value, m_SD, m_ED);
            else if (m_selectBy == "MediaTypePeriod")
                dtMaster = m_db.SelectMediaByMediaTypePeriod(m_value, m_SD, m_ED);
            gvDetail.DataSource = dtMaster;
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                DataRow[] rowsToCopy = dtMaster.Select(string.Format("Media_ID like '%{0}%' OR Short_Name like '%{0}%' OR Media_Sub_Type like '%{0}%' OR Media_Sub_Type_Name like '%{0}%' OR Media_Type like '%{0}%' OR Media_Type_Name like '%{0}%'", txtSearch.Text.Replace("'", "")));
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
