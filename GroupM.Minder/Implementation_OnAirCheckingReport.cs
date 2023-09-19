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
using GroupM.DBAccess;
using GroupM.UTL;
using Excel = Microsoft.Office.Interop.Excel;

namespace GroupM.Minder
{
    public partial class Implementation_OnAirCheckingReport : Form
    {
        private string PLACE_HOLDER = "Search Code or Name ...";
        private bool bFirstLoad = true;
        private string statusChk = "";
        string UserID = "GPMOnAirChecking";
        DBManager m_db;
        public Implementation_OnAirCheckingReport()
        {
            InitializeComponent();
            m_db = new DBManager();

        }


        public void RemoveText(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text == PLACE_HOLDER)
            {
                ((TextBox)sender).Text = "";
            }
        }

        public void AddText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(((TextBox)sender).Text))
                ((TextBox)sender).Text = PLACE_HOLDER;
        }
        DataTable dtChannel;
        DataTable dtBrand;
        DataTable dtProgram;
        private void Implementation_OnAirCheckingReport_Load(object sender, EventArgs e)
        {
            InitControl();
        }

        private void InitControl() {

            txtSearchChannel.GotFocus += new System.EventHandler(RemoveText);
            txtSearchChannel.LostFocus += new System.EventHandler(AddText);

            txtSearchBrand.GotFocus += new System.EventHandler(RemoveText);
            txtSearchBrand.LostFocus += new System.EventHandler(AddText);

            txtSearchProgram.GotFocus += new System.EventHandler(RemoveText);
            txtSearchProgram.LostFocus += new System.EventHandler(AddText);

            //dtStartDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtStartDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtEndDate.Value = dtStartDate.Value.AddMonths(1).AddDays(-1);

            gvDetailChannel.AutoGenerateColumns = false;
            dtChannel = m_db.SelectChannelFromSpotPlan(dtStartDate.Value, dtEndDate.Value);
            gvDetailChannel.DataSource = dtChannel;

            gvDetailBrand.AutoGenerateColumns = false;

            bFirstLoad = false;
        }

        private void txtSearchChannel_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                DataRow[] rowsToCopy = dtChannel.Select(string.Format("Media_ID like '%{0}%' OR Short_Name like '%{0}%'", txtSearchChannel.Text.Replace("'", "")));
                DataTable dtNew = dtChannel.Clone();
                foreach (DataRow dr in rowsToCopy)
                {
                    dtNew.Rows.Add(dr.ItemArray);
                }
                gvDetailChannel.DataSource = dtNew;
                chkChannelSelectAll.Checked = true;
            }
            catch (Exception ex)
            {
                //GMessage.MessageError(ex);
            }
        }

        private void chkChannelSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkSelectAll = (CheckBox)sender;
            DataGridView gvDetail = new DataGridView();
            if (chkSelectAll.Name == "chkChannelSelectAll")
                gvDetail = gvDetailChannel;
            else if(chkSelectAll.Name == "chkBrandSelectAll")
                gvDetail = gvDetailBrand;
            else
                gvDetail = gvDetailProgram;
            for (int i = 0; i < gvDetail.Rows.Count; i++)
            {
                gvDetail.Rows[i].Cells[0].Value = chkSelectAll.Checked;
            }
            //if (chkGv == true)
            //{
            //    if (chkSelectAll.CheckState == CheckState.Checked)
            //    {
            //        statusChk = "check";
            //    }
            //    if (chkSelectAll.CheckState == CheckState.Indeterminate)
            //    {
            //        statusChk = "indeterminate";
            //    }
            //    if (chkSelectAll.CheckState == CheckState.Unchecked)
            //    {
            //        statusChk = "uncheck";
            //    }
            //}
            //else
            //{
            //    int i = 0;

            //    if (statusChk == "check")
            //    {
            //        chkSelectAll.CheckState = CheckState.Unchecked;
            //        statusChk = "uncheck";

            //        for (i = 0; i < gvDetail.Rows.Count; i++)
            //        {
            //            gvDetail.Rows[i].Cells[0].Value = false;
            //        }
            //    }
            //    else
            //    {
            //        chkSelectAll.CheckState = CheckState.Checked;
            //        statusChk = "check";
            //        for (i = 0; i < gvDetail.Rows.Count; i++)
            //        {
            //            gvDetail.Rows[i].Cells[0].Value = true;
            //        }
            //    }
            //}
        }

        private void txtSearchBrand_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                DataRow[] rowsToCopy = dtBrand.Select(string.Format("Brand_ID like '%{0}%' OR Brand_Name like '%{0}%'", txtSearchBrand.Text.Replace("'", "")));
                DataTable dtNew = dtBrand.Clone();
                foreach (DataRow dr in rowsToCopy)
                {
                    dtNew.Rows.Add(dr.ItemArray);
                }
                gvDetailBrand.DataSource = dtNew;
            }
            catch (Exception ex)
            {
                //GMessage.MessageError(ex);
            }
        }

        private void rdBrand_CheckedChanged(object sender, EventArgs e)
        {
            if (rdBrand.Checked)
            {
                dtBrand = m_db.SelectBrandFromSpotPlan(dtStartDate.Value, dtEndDate.Value);
                gvDetailBrand.DataSource = dtBrand;
                gvDetailChannel.DataSource = null;
                txtSearchChannel.Text = PLACE_HOLDER;
                //chkChannelSelectAll.Checked = false;
                chkChannelSelectAll.Enabled = false;
                chkBrandSelectAll.Enabled = true;
            }
            else
            {
                gvDetailBrand.DataSource = null;
                txtSearchBrand.Text = PLACE_HOLDER;
                //chkBrandSelectAll.Checked = false;
                chkChannelSelectAll.Enabled = true;
                chkBrandSelectAll.Enabled = false;
                dtChannel = m_db.SelectChannelFromSpotPlan(dtStartDate.Value, dtEndDate.Value);
                gvDetailChannel.DataSource = dtChannel;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            chkProgramSelectAll.Checked = true;
            txtSearchProgram.Text = PLACE_HOLDER;
            DataLoading();
            
            tabControl1.SelectedTab = tabControl1.TabPages[1];
            btnBack.Visible = true;
            btnExport.Visible = true;
            btnNext.Visible = false;

            lbPeriod.Visible = false;
            dtStartDate.Visible = false;
            dtEndDate.Visible = false;

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[0];
            btnBack.Visible = false;
            btnExport.Visible = false;
            btnNext.Visible = true;

            lbPeriod.Visible = true;
            dtStartDate.Visible = true;
            dtEndDate.Visible = true;

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                DataTable dtProgramList = ((DataTable)gvDetailProgram.DataSource);

                int iProgramCount = dtProgramList
                    .AsEnumerable()
                    .Where(w => w.Field<int>("chk") == 1)
                    .Select(r => r.Field<string>("Program_Group"))
                    .Distinct()
                    .Count();


                if (iProgramCount > 255)
                {
                    GMessage.MessageWarning("Excel can not generate more than 255 sheets, please re-select program group less than 255 groups.");
                    return;
                }


                m_db.InsertGPMProgramMapping((DataTable)gvDetailProgram.DataSource);
                Export();
                GMessage.MessageInfo("Done");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            //this.Close();

        }
        private void Export()
        {
            try
            {

                //DataTable dtTemp = new DataTable();
                //dtTemp.Columns.Add("chk");
                //dtTemp.Columns.Add("Program");
                //dtTemp.Columns.Add("Program_Group");

                //DataRow dr = dtTemp.NewRow();
                //dr["chk"] = "1";
                //dr["Program"] = "AT TEN DAY:PROPRIETARY MEDIA";
                //dr["Program_Group"] = "AT TEN";
                //dtTemp.Rows.Add(dr);

                //DataRow dr2 = dtTemp.NewRow();
                //dr2["chk"] = "1";
                //dr2["Program"] = "BANTOENG THAIRATH:PROPRIETARY MEDIA";
                //dr2["Program_Group"] = "BANTOENG THAIRATH";
                //dtTemp.Rows.Add(dr2);


                //DataTable dtProgramList = dtTemp;
                m_db.InsertLogMinder(UserID, "", "Export to Excel", "System");



                DataTable dtProgramList = ((DataTable)gvDetailProgram.DataSource);
            string strProgram = ""; 
            for (int i = 0; i < dtProgramList.Rows.Count; i++)
            {
                DataRow drProgram = dtProgramList.Rows[i];
                if (drProgram["chk"].ToString() == "1")
                {
                    if (strProgram != "")
                        strProgram += ",";
                    strProgram += "'" + drProgram["Program"].ToString() + "'";
                }
            }

            if (strProgram == "")
                strProgram = "''";

            string strFilter = rdBrand.Checked ? "Brand" : "Media";
            string strFilterValue = "";

            if (strFilter == "Media")
            {
                DataTable dtMedia = ((DataTable)gvDetailChannel.DataSource);
                for (int i = 0; i < dtMedia.Rows.Count; i++)
                {
                    DataRow drMedia = dtMedia.Rows[i];
                    if (drMedia["chk"].ToString() == "1")
                    {
                        if (strFilterValue != "")
                            strFilterValue += ",";
                        strFilterValue += "'" + drMedia["Media_ID"].ToString() + "'";
                    }
                }
                if (strFilterValue == "")
                    strFilterValue = "''";

            }
            else
            {
                DataTable dtBrand = ((DataTable)gvDetailBrand.DataSource);
                for (int i = 0; i < dtBrand.Rows.Count; i++)
                {
                    DataRow drBrand = dtBrand.Rows[i];
                    if (drBrand["chk"].ToString() == "1")
                    {
                        if (strFilterValue != "")
                            strFilterValue += ",";
                        strFilterValue += "'" + drBrand["Brand_ID"].ToString() + "'";
                    }
                }
                if (strFilterValue == "")
                    strFilterValue = "''";

            }

            DataTable dt = m_db.SelectOnAirCheckingGPM(dtStartDate.Value, dtEndDate.Value, strProgram, strFilter,strFilterValue);
            Excel.Application ExcelObjDesc = new Excel.Application();

            string strExportTemplate = ConfigurationManager.AppSettings["ReportMinder"] + "On_Air_Checking_Report_GPM.xltx";
            Excel.Workbook workbookCurrent = ExcelObjDesc.Workbooks.Open(strExportTemplate);

            // Open Export Excel File
            ExcelObjDesc.WindowState = Excel.XlWindowState.xlMaximized;
            ExcelObjDesc.Visible = true;

            int iWMRow = 8;
            int iMSRow = 8;
            int iMCRow = 8;
            int iGPMRow = 8;
            int iOtherRow = 8;
            int iCol0stDay = 17;

            string AgencyCurrent = "";
            string ProgramGroupCurrent = "";
            int iRow = iWMRow;

            Excel.Worksheet sheet = workbookCurrent.Sheets["Template"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                //Initial Export
                if (i == 0)
                {
                    sheet = workbookCurrent.Sheets["Template"];
                    sheet.Copy(Type.Missing, workbookCurrent.Sheets[workbookCurrent.Sheets.Count]); // copy
                    workbookCurrent.Sheets[workbookCurrent.Sheets.Count].Name = dr["Program_Group"].ToString();
                    sheet = workbookCurrent.Sheets[dr["Program_Group"].ToString()];

                    ProgramGroupCurrent = dr["Program_Group"].ToString();
                    AgencyCurrent = dr["MasterAgency"].ToString();

                    ExcelUtil.ExcelSetValueString(sheet, "B", 3, $"{dtStartDate.Value.ToString("dd/MM/yyyy")} - {dtEndDate.Value.ToString("dd/MM/yyyy")}");
                    ExcelUtil.ExcelSetValueString(sheet, "B", 4, dr["Media_Name"].ToString());

                    if (dr["MasterAgency"].ToString() == "MEC")
                        iRow = iWMRow;
                    else if (dr["MasterAgency"].ToString() == "MS")
                        iRow = iWMRow + iMSRow;
                    else if (dr["MasterAgency"].ToString() == "MC")
                        iRow = iWMRow + iMSRow + iMCRow;
                    else if (dr["MasterAgency"].ToString() == "GPM")
                        iRow = iWMRow + iMSRow + iMCRow + iGPMRow;
                    else 
                        iRow = iWMRow + iMSRow + iMCRow + iGPMRow + iOtherRow;
                }
                //========================================
                // New Program - Create New Sheet
                //========================================
                if (ProgramGroupCurrent != dr["Program_Group"].ToString())
                {                    
                    string value = ((Excel.Range)sheet.Cells[iRow, 1]).Value;
                    if (value == null)
                        ((Excel.Range)sheet.Rows[iRow]).Delete();

                    sheet = workbookCurrent.Sheets["Template"];
                    sheet.Copy(Type.Missing, workbookCurrent.Sheets[workbookCurrent.Sheets.Count]); // copy
                    workbookCurrent.Sheets[workbookCurrent.Sheets.Count].Name = dr["Program_Group"].ToString();
                    sheet = workbookCurrent.Sheets[dr["Program_Group"].ToString()];
                    ProgramGroupCurrent = dr["Program_Group"].ToString();

                    ExcelUtil.ExcelSetValueString(sheet, "B", 3, $"{dtStartDate.Value.ToString("dd/MM/yyyy")} - {dtEndDate.Value.ToString("dd/MM/yyyy")}");
                    ExcelUtil.ExcelSetValueString(sheet, "B", 4, dr["Media_Name"].ToString());

                    AgencyCurrent = dr["MasterAgency"].ToString();

                    if (dr["MasterAgency"].ToString() == "MEC")
                        iRow = iWMRow;
                    else if (dr["MasterAgency"].ToString() == "MS")
                        iRow = iWMRow + iMSRow;
                    else if (dr["MasterAgency"].ToString() == "MC")
                        iRow = iWMRow + iMSRow + iMCRow;
                    else if (dr["MasterAgency"].ToString() == "GPM")
                        iRow = iWMRow + iMSRow + iMCRow + iGPMRow;
                    else
                        iRow = iWMRow + iMSRow + iMCRow + iGPMRow + iOtherRow;


                }
                if (AgencyCurrent != dr["MasterAgency"].ToString())
                {
                    if (dr["MasterAgency"].ToString() == "MS")
                        iRow = iRow + iMSRow;
                    else if (dr["MasterAgency"].ToString() == "MC")
                    {
                        if (AgencyCurrent == "MS")
                            iRow = iRow + iMCRow;
                        else
                            iRow = iRow + iMSRow + iMCRow;
                    }
                    else if (dr["MasterAgency"].ToString() == "GPM")
                    {
                        if (AgencyCurrent == "MS")
                            iRow = iRow + iMCRow + iGPMRow;
                        else if (AgencyCurrent == "MC")
                            iRow = iRow + iGPMRow;
                        else//WM
                            iRow = iRow + iMSRow + iMCRow + iGPMRow;
                    }
                    else//Other
                    {
                        if (AgencyCurrent == "MS")
                            iRow = iRow + iMCRow + iGPMRow + iOtherRow;
                        else if (AgencyCurrent == "MC")
                            iRow = iRow + iGPMRow + iOtherRow;
                        else if (AgencyCurrent == "GPM")
                            iRow = iRow + iOtherRow;
                        else
                            iRow = iRow + iMSRow + iMCRow + iGPMRow + iOtherRow;
                    }
                    AgencyCurrent = dr["MasterAgency"].ToString();
                }
                

                ExcelUtil.ExcelSetValueString(sheet, "A", iRow, dr["Vendor_Name"].ToString());
                ExcelUtil.ExcelSetValueString(sheet, "B", iRow, dr["Program_Name"].ToString());
                ExcelUtil.ExcelSetValueString(sheet, "C", iRow, dr["Start_Time"].ToString());
                ExcelUtil.ExcelSetValueString(sheet, "D", iRow, dr["End_Time"].ToString());
                ExcelUtil.ExcelSetValueString(sheet, "E", iRow, dr["Brand_Name"].ToString());
                ExcelUtil.ExcelSetValueString(sheet, "F", iRow, dr["Product_Thai"].ToString());
                ExcelUtil.ExcelSetValueString(sheet, "G", iRow, dr["Buying_Brief_No"].ToString());
                ExcelUtil.ExcelSetValueString(sheet, "H", iRow, dr["Status"].ToString());
                ExcelUtil.ExcelSetValueString(sheet, "I", iRow, dr["Client_Name"].ToString());
                ExcelUtil.ExcelSetValueString(sheet, "J", iRow, dr["Campaign"].ToString());
                ExcelUtil.ExcelSetValueString(sheet, "K", iRow, dr["Material"].ToString());
                ExcelUtil.ExcelSetValueString(sheet, "L", iRow, Convert.ToDouble((Convert.ToDouble(dr["Length"])/ Convert.ToDouble(dr["Spots"]))).ToString());
                ExcelUtil.ExcelSetValueString(sheet, "M", iRow, Convert.ToDouble(dr["Net_Cost"]).ToString("#,##0.00"));
                ExcelUtil.ExcelSetValueString(sheet, "N", iRow, dr["Package"].ToString());
                ExcelUtil.ExcelSetValueString(sheet, "O", iRow, dr["Creative_Agency_Name"].ToString());
                ExcelUtil.ExcelSetValueString(sheet, "P", iRow, dr["Office_Name"].ToString());
                ExcelUtil.ExcelSetValueString(sheet, "Q", iRow, dr["Report_To_Agency"].ToString());

                string strShowDate = dr["Show_Date"].ToString();
                int Day = Convert.ToInt32(strShowDate.Substring(strShowDate.Length - 2, 2));

               ExcelUtil.ExcelSetValueStringIncremental(sheet, iRow,iCol0stDay + Day, dr["Length"].ToString());

                //Insert new row
                if (i < dt.Rows.Count - 1)
                {
                    if (AgencyCurrent == dt.Rows[i + 1]["MasterAgency"].ToString())
                    {

                        string currentRow = dr["Vendor_Name"].ToString()
                            + dr["Program_Name"].ToString()
                            + dr["Start_Time"].ToString()
                            + dr["End_Time"].ToString()
                            + dr["Brand_Name"].ToString()
                            + dr["Product_Thai"].ToString()
                            + dr["Buying_Brief_No"].ToString()
                            + dr["Status"].ToString()
                            + dr["Client_Name"].ToString()
                            + dr["Campaign"].ToString()
                            + dr["Material"].ToString()
                            + dr["Package"].ToString()
                            + dr["Creative_Agency_Name"].ToString()
                            + dr["Office_Name"].ToString()
                            + dr["Report_To_Agency"].ToString();
                        string nextRow = "";
                        if (i + 1 <= (dt.Rows.Count - 1))
                        {
                            DataRow drNext = dt.Rows[i + 1];
                            nextRow = drNext["Vendor_Name"].ToString()
                                + drNext["Program_Name"].ToString()
                                + drNext["Start_Time"].ToString()
                                + drNext["End_Time"].ToString()
                                + drNext["Brand_Name"].ToString()
                                + drNext["Product_Thai"].ToString()
                                + drNext["Buying_Brief_No"].ToString()
                                + drNext["Status"].ToString()
                                + drNext["Client_Name"].ToString()
                                + drNext["Campaign"].ToString()
                                + drNext["Material"].ToString()
                                + drNext["Package"].ToString()
                                + drNext["Creative_Agency_Name"].ToString()
                                + drNext["Office_Name"].ToString()
                                + drNext["Report_To_Agency"].ToString();
                        }
                        if (currentRow != nextRow)
                        {
                            Excel.Range rang = (Excel.Range)sheet.Rows[++iRow];
                            rang.Insert();
                            //sheet.Cells[iRow-1][49].FormulaR1C1 = sheet.Cells[iRow-2][49].FormulaR1C1;

                            Excel.Range source = sheet.Range[string.Format("AW{0}:AX{0}", (iRow - 1))];
                            Excel.Range dest = sheet.Range[string.Format("AW{0}:AX{0}", (iRow))];
                            source.Copy(dest);
                        }
                    }
                }
            }
            sheet = workbookCurrent.Sheets["Template"];
            sheet.Visible = Excel.XlSheetVisibility.xlSheetHidden;

            workbookCurrent.SaveAs(string.Format("OnAirChecking_{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss")));
            }
            catch (Exception)
            {

                throw;
            }
        }



        private void DataLoading()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                gvDetailProgram.AutoGenerateColumns = false;
                //SelectProgramFromSpotPlan
                string strFilter = "Media";
                string strList = "";
                if (rdBrand.Checked)
                {                    
                    strFilter = "Brand";
                    for (int i = 0; i < gvDetailBrand.Rows.Count; i++)
                    {
                        DataRow dr = ((DataRowView)gvDetailBrand.Rows[i].DataBoundItem).Row;
                        if (dr["chk"].ToString() == "1")
                        {
                            if (strList != "")
                                strList += ",";
                            strList += "'"+dr["Brand_ID"].ToString()+"'";
                        }
                    }
                }
                else
                {
                    strFilter = "Media";
                    for (int i = 0; i < gvDetailChannel.Rows.Count; i++)
                    {
                        DataRow dr = ((DataRowView)gvDetailChannel.Rows[i].DataBoundItem).Row;
                        if (dr["chk"].ToString() == "1")
                        {
                            if (strList != "")
                                strList += ",";
                            strList += "'" + dr["Media_ID"].ToString() + "'";
                        }
                    }
                }

                if (strList == "")
                    strList = "''";
                dtProgram = m_db.SelectProgramFromSpotPlan(dtStartDate.Value, dtEndDate.Value, strList,strFilter);
                gvDetailProgram.DataSource = dtProgram;
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

        private void txtSearchProgram_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                DataRow[] rowsToCopy = dtProgram.Select(string.Format("Program like '%{0}%' OR Program_Group like '%{0}%'", txtSearchProgram.Text.Replace("'", "")));
                DataTable dtNew = dtProgram.Clone();
                foreach (DataRow dr in rowsToCopy)
                {
                    dtNew.Rows.Add(dr.ItemArray);
                }
                gvDetailProgram.DataSource = dtNew;
            }
            catch (Exception ex)
            {
                //GMessage.MessageError(ex);
            }
        }

        private void dtStartDate_ValueChanged(object sender, EventArgs e)
        {
            if (bFirstLoad)
                return;
            try
            {
                this.Cursor = Cursors.WaitCursor;


                if (rdChannel.Checked)
                {
                    dtChannel = m_db.SelectChannelFromSpotPlan(dtStartDate.Value, dtEndDate.Value);
                    if (txtSearchChannel.Text != PLACE_HOLDER)
                        txtSearchChannel_KeyUp(txtSearchChannel, null);
                    else
                        gvDetailChannel.DataSource = dtChannel;
                }
                else
                {
                    dtBrand = m_db.SelectBrandFromSpotPlan(dtStartDate.Value, dtEndDate.Value);
                    
                    if (txtSearchBrand.Text != PLACE_HOLDER)
                        txtSearchBrand_KeyUp(txtSearchBrand, null);
                    else
                        gvDetailBrand.DataSource = dtBrand;
                }

                chkChannelSelectAll.Checked = true;
                chkBrandSelectAll.Checked = false;
                chkProgramSelectAll.Checked = true;
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
    }
}
