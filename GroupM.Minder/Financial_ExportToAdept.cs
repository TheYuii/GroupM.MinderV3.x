using GroupM.DBAccess;
using GroupM.UTL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupM.Minder
{
    public partial class Financial_ExportToAdept : Form
    {
        DBManager m_db = null;
        private bool chkGv = false;
        private string statusChk = "";
        private string username = "";
        public int rowMDF = 0;

        public Financial_ExportToAdept(string Username)
        {
            InitializeComponent();
            m_db = new DBManager();
            username = Username.Replace(".", "");
        }

        private void InitailControl()
        {
            rdPeriodYearMonth.Checked = true;
            dtStartDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtEndDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);
            txtYear.Text = DateTime.Now.Year.ToString();
            cboMonth.Text = DateTime.Now.Month.ToString();
            ControlManipulate.ClearControl(txtBB, txtMediaType, txtMediaTypeCode, txtClient, txtClientCode, txtAgency, txtAgencyCode, txtOffice, txtOfficeCode, txtCreativeAgency, txtCreativeAgencyCode, txtProduct, txtProductCode);
            cboStatus.SelectedIndex = 0;
            cboAC.SelectedIndex = 0;
            chkSelectBB.CheckState = CheckState.Unchecked;
            gvDetail.DataSource = null;
            lblCost.Text = "Buying Brief: Approve: 0.00 Executing/Actual: 0.00 Export: 0.00";
        }

        private void DataLoading()
        {
            // set period
            DateTime startDate = new DateTime(Convert.ToInt32(txtYear.Value), Convert.ToInt32(cboMonth.SelectedItem.ToString()), 1);
            DateTime endDate = new DateTime(Convert.ToInt32(txtYear.Value), Convert.ToInt32(cboMonth.SelectedItem.ToString()), 1).AddMonths(1).AddDays(-1);
            if (rdPeriodDate.Checked == true)
            {
                startDate = dtStartDate.Value;
                endDate = dtEndDate.Value;
            }
            string strStartDate = startDate.ToString("yyyyMMdd");
            string strEndDate = endDate.ToString("yyyyMMdd");
            // load data grid view
            this.Cursor = Cursors.WaitCursor;
            DataTable dt = m_db.SelectBuyingBriefMDF(strStartDate, strEndDate, cboStatus.SelectedItem.ToString(), cboAC.SelectedItem.ToString(), username, txtBB.Text, txtMediaTypeCode.Text, txtClientCode.Text, txtAgencyCode.Text, txtOfficeCode.Text, txtCreativeAgencyCode.Text, txtProductCode.Text);
            gvDetail.AutoGenerateColumns = false;
            gvDetail.DataSource = dt;
            // set check box select all
            if (chkSelectBB.CheckState == CheckState.Checked)
            {
                statusChk = "check";
                int i = 0;
                for (i = 0; i < gvDetail.Rows.Count; i++)
                {
                    gvDetail.Rows[i].Cells["SelectBB"].Value = true;
                }
            }
            else
            {
                chkSelectBB.CheckState = CheckState.Checked;
            }
            btnSearch.Focus();
            this.Cursor = Cursors.Default;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // search table
            if (keyData == Keys.F8 || keyData == Keys.Enter)
            {
                DataLoading();
                return true;
            }
            // clear table
            if (keyData == (Keys.Control | Keys.R))
            {
                InitailControl();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private int CountSelectBuyingBrief()
        {
            int i, count = 0;
            for (i = 0; i < gvDetail.Rows.Count; i++)
            {
                if (Convert.ToBoolean(gvDetail.Rows[i].Cells["SelectBB"].Value) == true)
                {
                    count++;
                }
            }
            return count;
        }

        public StringBuilder ExportMDF(string BBXML, DateTime dStartDate, DateTime dEndDate, string spotPlanStatus, int exportFee)
        {
            try
            {
                DataTable dt = m_db.SelectMDFDetail(BBXML, dStartDate, dEndDate, spotPlanStatus, exportFee);
                rowMDF = dt.Rows.Count;
                StringBuilder csv = new StringBuilder();
                //Header
                DateTime dtExport = DateTime.Now;
                string col1 = "0";
                string col2 = dtExport.ToString("yyyyMMddHHmmss");
                string col3 = "";
                string col4 = "";
                string col5 = dtExport.ToString("yyyy/MM/dd HH:mm:ss");
                string col6 = "0.00";
                if (dt.Rows.Count > 0)
                {
                    double detailCol33 = dt.Compute("SUM([33.BOOK_DCost_ExpRC])", "[50.BOOK_Is_Cancelled] = 0") == DBNull.Value ? 0 : Convert.ToDouble(dt.Compute("SUM([33.BOOK_DCost_ExpRC])", "[50.BOOK_Is_Cancelled] = 0"));
                    double detailCol34 = dt.Compute("SUM([34.BOOK_MCost_ExpRC])", "[50.BOOK_Is_Cancelled] = 0") == DBNull.Value ? 0 : Convert.ToDouble(dt.Compute("SUM([34.BOOK_MCost_ExpRC])", "[50.BOOK_Is_Cancelled] = 0"));
                    double detailCol37 = dt.Compute("SUM([37.x3])", "[50.BOOK_Is_Cancelled] = 0") == DBNull.Value ? 0 : Convert.ToDouble(dt.Compute("SUM([37.x3])", "[50.BOOK_Is_Cancelled] = 0"));
                    col6 = (detailCol33 + detailCol34 + detailCol37).ToString("F2"); //sum amount
                }
                string col7 = dt.Rows.Count.ToString();
                string line = $"{col1}|{col2}|{col3}|{col4}|{col5}|{col6}|{col7}|||";
                csv.AppendLine(line);
                //Detail
                foreach (DataRow dr in dt.Rows)
                {
                    line = " ";
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (i != 0)
                        {
                            line += "|";
                        }
                        if (dr[i] != null && dr[i] != DBNull.Value)
                        {
                            if (dr[63].ToString() != "2" && (i == 29 || i == 32 || i == 33 || i == 36))
                            {
                                line += Convert.ToDouble(dr[i]) == 0 ? "0" : Convert.ToDouble(dr[i]).ToString("F2");
                            }
                            else
                            {
                                if (dr[63].ToString() == "2" && i == 36)
                                {
                                    line += Convert.ToDouble(dr[i]) == 0 ? "0" : Convert.ToDouble(dr[i]).ToString("F2");
                                }
                                else
                                {
                                    line += dr[i].ToString();
                                }
                            }
                        }
                    }
                    csv.AppendLine(line);
                }
                return csv;
            }
            catch(Exception e)
            {
                GMessage.MessageError(e.Message);
                return null;
            }
        }

        private void PrepareDataAndExport(string saveFileTest)
        {
            // variable
            int i = 0;
            string selectBBXML = "";
            DateTime startDate;
            DateTime endDate;
            string numStatus = "";
            int exportFee = 0;
            string pathSaveFile = "";
            List<string> listBB = new List<string>();
            // set buying brief
            for (i = 0; i < gvDetail.Rows.Count; i++)
            {
                // check box state checked
                if (Convert.ToBoolean(gvDetail.Rows[i].Cells["SelectBB"].Value) == true)
                {
                    m_db.UpdateAdeptCodeBeforeExport(gvDetail.Rows[i].Cells["BBID"].Value.ToString());
                    selectBBXML += "<row BB=\"" + gvDetail.Rows[i].Cells["BBID"].Value.ToString() + "\" />";
                    listBB.Add(gvDetail.Rows[i].Cells["BBID"].Value.ToString());
                }
            }
            // set period
            startDate = dtStartDate.Value;
            endDate = dtEndDate.Value;
            if (rdPeriodYearMonth.Checked == true)
            {
                startDate = new DateTime(Convert.ToInt32(txtYear.Value), Convert.ToInt32(cboMonth.SelectedItem.ToString()), 1);
                endDate = new DateTime(Convert.ToInt32(txtYear.Value), Convert.ToInt32(cboMonth.SelectedItem.ToString()), 1).AddMonths(1).AddDays(-1);
            }
            // set status
            if (cboStatus.SelectedIndex == 0)
            {
                numStatus = "4";
            }
            if (cboStatus.SelectedIndex == 1)
            {
                numStatus = "5";
            }
            if (cboStatus.SelectedIndex == 2)
            {
                numStatus = "8";
            }
            if (cboStatus.SelectedIndex == 3)
            {
                numStatus = "9";
            }
            exportFee = cboAC.SelectedIndex;
            // export mdf file
            StringBuilder mdf = ExportMDF(selectBBXML, startDate, endDate, numStatus, exportFee);
            if (rowMDF == 0)
            {
                if (saveFileTest == "")
                {
                    GMessage.MessageWarning("No data to export.");
                }
            }
            else
            {
                if (saveFileTest == "")
                {
                    // show save file dialog
                    if (svFile.ShowDialog() == DialogResult.OK && string.IsNullOrEmpty(svFile.FileName) == false)
                    {
                        // set file name
                        pathSaveFile = svFile.FileName;
                        File.WriteAllText(pathSaveFile, mdf.ToString());
                        // set file read only
                        FileInfo fileInfo = new FileInfo(pathSaveFile);
                        fileInfo.IsReadOnly = true;
                        // update flag export mdf
                        for (i = 0; i < listBB.Count; i++)
                        {
                            m_db.UpdateFlagExportToAdept(listBB[i], numStatus);
                        }
                        GMessage.MessageInfo("Done.");
                    }
                }
                else
                {
                    // set file name
                    pathSaveFile = saveFileTest;
                    File.WriteAllText(pathSaveFile, mdf.ToString());
                    // update flag export mdf
                    for (i = 0; i < listBB.Count; i++)
                    {
                        m_db.UpdateFlagExportToAdept(listBB[i], numStatus);
                    }
                }
            }
        }

        private void Financial_ExportToAdept_Load(object sender, EventArgs e)
        {
            InitailControl();
            dtStartDate.Enabled = false;
            dtEndDate.Enabled = false;
        }

        private void rdPeriodDate_CheckedChanged(object sender, EventArgs e)
        {
            if (rdPeriodDate.Checked == true)
            {
                dtStartDate.Enabled = true;
                dtEndDate.Enabled = true;
                txtYear.Enabled = false;
                cboMonth.Enabled = false;
            }
        }

        private void rdPeriodYearMonth_CheckedChanged(object sender, EventArgs e)
        {
            if (rdPeriodYearMonth.Checked == true)
            {
                dtStartDate.Enabled = false;
                dtEndDate.Enabled = false;
                txtYear.Enabled = true;
                cboMonth.Enabled = true;
            }
        }

        private void txtMediaType_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Media_Type", "Short_Name", "Media_Type", false, "", "");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtMediaType.Text = frm.SelectedGridRow["MasterName"].ToString();
                txtMediaTypeCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
            }
        }

        private void txtMediaType_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtMediaType_TextChanged(object sender, EventArgs e)
        {
            if (txtMediaType.Text == "")
            {
                txtMediaTypeCode.Text = "";
            }
        }

        private void txtClient_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Client_ID", "Short_Name", "Client", false, username, "");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtClient.Text = frm.SelectedGridRow["MasterName"].ToString();
                txtClientCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
            }
        }

        private void txtClient_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtClient_TextChanged(object sender, EventArgs e)
        {
            if (txtClient.Text == "")
            {
                txtClientCode.Text = "";
            }
        }

        private void txtAgency_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Agency.Agency_ID", "Agency.Short_Name", "Agency", false, username, "");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtAgency.Text = frm.SelectedGridRow["MasterName"].ToString();
                txtAgencyCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
            }
        }

        private void txtAgency_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtAgency_TextChanged(object sender, EventArgs e)
        {
            if (txtAgency.Text == "")
            {
                txtAgencyCode.Text = "";
            }
        }

        private void txtOffice_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Office.Office_ID", "Office.Short_Name", "Office", false, username, "");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtOffice.Text = frm.SelectedGridRow["MasterName"].ToString();
                txtOfficeCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
            }
        }

        private void txtOffice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtOffice_TextChanged(object sender, EventArgs e)
        {
            if (txtOffice.Text == "")
            {
                txtOfficeCode.Text = "";
            }
        }

        private void txtCreativeAgency_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Creative_Agency_ID", "Short_Name", "Creative_Agency", false, username, "");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtCreativeAgency.Text = frm.SelectedGridRow["MasterName"].ToString();
                txtCreativeAgencyCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
            }
        }

        private void txtCreativeAgency_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtCreativeAgency_TextChanged(object sender, EventArgs e)
        {
            if (txtCreativeAgency.Text == "")
            {
                txtCreativeAgencyCode.Text = "";
            }
        }

        private void txtProduct_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Product.Product_ID", "Product.Short_Name", "Product", false, username, "");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtProduct.Text = frm.SelectedGridRow["MasterName"].ToString();
                txtProductCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
            }
        }

        private void txtProduct_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtProduct_TextChanged(object sender, EventArgs e)
        {
            if (txtProduct.Text == "")
            {
                txtProductCode.Text = "";
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataLoading();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            InitailControl();
        }

        private void gvDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // set check box state on data grid view to current state
            gvDetail.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void gvDetail_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            chkGv = true;
            // count total check box state checked
            int totalSelectBB = CountSelectBuyingBrief();
            if (totalSelectBB == 0) // unchecked all
            {
                chkSelectBB.CheckState = CheckState.Unchecked;
            }
            else
            {
                if (totalSelectBB == gvDetail.Rows.Count) // checked all
                {
                    chkSelectBB.CheckState = CheckState.Checked;
                }
                else // some checked
                {
                    chkSelectBB.CheckState = CheckState.Indeterminate;
                }
            }
            chkGv = false;
        }

        private void gvDetail_SelectionChanged(object sender, EventArgs e)
        {
            if (gvDetail.Rows.Count == 0 || gvDetail.SelectedRows.Count == 0)
            {
                lblCost.Text = "Buying Brief: Approve: 0.00 Executing/Actual: 0.00 Export: 0.00";
                return;
            }
            // variable
            string buyingBrief = "";
            DateTime startDate;
            DateTime endDate;
            string numStatus = "";
            // set period
            startDate = dtStartDate.Value;
            endDate = dtEndDate.Value;
            if (rdPeriodYearMonth.Checked == true)
            {
                startDate = new DateTime(Convert.ToInt32(txtYear.Value), Convert.ToInt32(cboMonth.SelectedItem.ToString()), 1);
                endDate = new DateTime(Convert.ToInt32(txtYear.Value), Convert.ToInt32(cboMonth.SelectedItem.ToString()), 1).AddMonths(1).AddDays(-1);
            }
            // set status
            if (cboStatus.SelectedIndex == 0)
            {
                numStatus = "4";
            }
            if (cboStatus.SelectedIndex == 1)
            {
                numStatus = "5";
            }
            if (cboStatus.SelectedIndex == 2)
            {
                numStatus = "8";
            }
            if (cboStatus.SelectedIndex == 3)
            {
                numStatus = "9";
            }
            DataRow dr = ((DataRowView)gvDetail.SelectedRows[0].DataBoundItem).Row;
            buyingBrief = dr["Buying_Brief_ID"].ToString();
            DataTable dt = m_db.getCostMDF(buyingBrief, numStatus, startDate, endDate);
            double app = Convert.ToDouble(dt.Rows[0]["Approve"]);
            double exc = Convert.ToDouble(dt.Rows[0]["Lastest"]);
            double exp = Convert.ToDouble(dt.Rows[0]["Export"]);
            string strApprove = app.ToString("N2");
            string strExecuting = exc.ToString("N2");
            string strExport = exp.ToString("N2");
            string str = $"Buying Brief: {dr["Buying_Brief_ID"].ToString()} Approve: {strApprove} Executing/Actual: {strExecuting} Export: {strExport}";
            lblCost.Text = str;
        }

        private void chkSelectBB_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkGv == true) // event from check box on data grid view
            {
                if (chkSelectBB.CheckState == CheckState.Checked)
                {
                    statusChk = "check";
                }
                if (chkSelectBB.CheckState == CheckState.Indeterminate)
                {
                    statusChk = "indeterminate";
                }
                if (chkSelectBB.CheckState == CheckState.Unchecked)
                {
                    statusChk = "uncheck";
                }
            }
            else // event from check box select all
            {
                int i = 0;
                if (statusChk == "check") // old status is checked
                {
                    // set all check box state to unchecked
                    chkSelectBB.CheckState = CheckState.Unchecked;
                    statusChk = "uncheck";
                    for (i = 0; i < gvDetail.Rows.Count; i++)
                    {
                        gvDetail.Rows[i].Cells["SelectBB"].Value = false;
                    }
                }
                else // old status is some checked or unchecked
                {
                    // set all check box state to checked
                    chkSelectBB.CheckState = CheckState.Checked;
                    statusChk = "check";
                    for (i = 0; i < gvDetail.Rows.Count; i++)
                    {
                        gvDetail.Rows[i].Cells["SelectBB"].Value = true;
                    }
                }
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gvDetail.Rows.Count == 0)
            {
                GMessage.MessageWarning("Cannot export, Buying brief not found!");
            }
            else
            {
                PrepareDataAndExport("");
            }
        }

        public void TestProgram(DateTime startDate, DateTime endDate, string status, string buyingBrief, string mediaType, string client, string agency, string office, string creativeAgency, string product, string exportFee, string saveFileName)
        {
            // set screen
            rdPeriodDate.Checked = true;
            dtStartDate.Value = startDate;
            dtEndDate.Value = endDate;
            cboStatus.SelectedItem = status;
            txtBB.Text = buyingBrief;
            txtMediaTypeCode.Text = mediaType;
            txtClientCode.Text = client;
            txtAgencyCode.Text = agency;
            txtOfficeCode.Text = office;
            txtCreativeAgencyCode.Text = creativeAgency;
            txtProductCode.Text = product;
            cboAC.SelectedItem = exportFee;
            DataLoading();
            chkSelectBB.CheckState = CheckState.Checked;
            // press export mdf
            PrepareDataAndExport(saveFileName);
        }
    }
}
