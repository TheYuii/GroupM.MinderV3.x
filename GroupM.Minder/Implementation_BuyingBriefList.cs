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
    public partial class Implementation_BuyingBriefList : Form
    {
        DBManager m_db = null;
        public Implementation_BuyingBriefList()
        {
            InitializeComponent();
            m_db = new DBManager();
        }

        private void Implementation_BuyingBrief_Load(object sender, EventArgs e)
        {
            InitailControl();

            DataTable dt = m_db.SelectBuyingBrief();

            gvDetail.AutoGenerateColumns = false;
            gvDetail.DataSource = dt;
        }
        private void InitailControl()
        {
            txtYear.Text = DateTime.Now.Year.ToString();
            cboMonth.Text = DateTime.Now.Month.ToString();

            dtStartDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtEndDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);

            GroupM.UTL.ControlManipulate.ClearControl(txtBB,txtMediaType,txtMediaSubType,txtAgency,txtAgencyCode,txtClient,txtClientCode,txtProduct,txtProductCode);
            cboStatus.SelectedIndex = 0;

            rdBBPreifx.Checked = true;
        }
        private void TxtAgency_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Agency_ID", "Short_Name", "Agency", true, "", "");
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtAgency.Text = frm.SelectedGridRow["MasterName"].ToString();
                txtAgencyCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
            }
        }

        private void TxtClient_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Client_ID", "Short_Name", "Client", true, "", "");
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtClient.Text = frm.SelectedGridRow["MasterName"].ToString();
                txtClientCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
            }
        }

        private void TxtProduct_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Product_ID", "Short_Name", "Product", true, "", "");
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtProduct.Text = frm.SelectedGridRow["MasterName"].ToString();
                txtProductCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
            }
        }

        private void TxtMediaSubType_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Media_Sub_Type", "Short_Name", "Media_Sub_Type", true, "", "");
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtMediaSubType.Text = frm.SelectedGridRow["MasterCode"].ToString();
            }
        }

        private void TxtMediaType_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Media_Type", "Short_Name", "Media_Type", true, "", "");
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtMediaType.Text = frm.SelectedGridRow["MasterCode"].ToString();
            }

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //if (keyData == (Keys.F3))
            //{
            //    btnSearch.PerformClick();
            //    return true;
            //}
            //if (keyData == (Keys.F4))
            //{
            //    SendKeys.Send("{DEL}");
            //    return true;
            //}

            //if (keyData == (Keys.F6))
            //{
            //    btnEdit.PerformClick();
            //    return true;
            //}
            //if (keyData == (Keys.F9))
            //{
            //    btnSave.PerformClick();
            //    return true;
            //}
            //if (keyData == (Keys.F8))
            //{
            //    if (m_screenMode == eScreenMode.View)
            //        btnDelete.PerformClick();
            //    else
            //        btnCancel.PerformClick();
            //    return true;
            //}
            //if (keyData == (Keys.F10))
            //{
            //    btnPrintTranspot.PerformClick();
            //    return true;
            //}
            //if (keyData == (Keys.F11))
            //{
            //    btnPrint1.PerformClick();
            //    return true;
            //}
            //if (keyData == (Keys.F12))
            //{
            //    btnPrint2.PerformClick();
            //    return true;
            //}
            //if (keyData == (Keys.Control | Keys.O))
            //{
            //    btnCopy.PerformClick();
            //    return true;
            //}
            if (keyData == (Keys.Control | Keys.R))
            {
                InitailControl();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
