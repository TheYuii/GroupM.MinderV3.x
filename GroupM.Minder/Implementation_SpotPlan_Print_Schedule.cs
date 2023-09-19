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
using Excel = Microsoft.Office.Interop.Excel;

namespace GroupM.Minder
{
    public partial class Implementation_SpotPlan_Print_Schedule : Form
    {
        private string m_strBuyingBriefID;
        private string m_strVersion;
        private string m_strStartDate;
        private string m_strEndDate;
        private string m_strScheduleExportPath;
        private string m_strScheduleDirectPayExportPath;
        private string m_strScheduleLumpSumExportPath;
        private DBManager m_db = null;
        private bool chkGv = false;
        private string statusChk = "";
        private string selectMedia = "";
        private string MediaTypeCode = "";
        private string MediaSubTypeCode = "";
        private string MasterMediaTypeCode = "";
        public string username;

        public Implementation_SpotPlan_Print_Schedule(string BB, string version, string startDate, string endDate, string template, string templateDirectPay, string templateLumpSum, string strMt, string strMst, string strMmt)
        {
            InitializeComponent();
            m_db = new DBManager();
            m_strBuyingBriefID = BB;
            m_strVersion = version;
            m_strStartDate = startDate;
            m_strEndDate = endDate;
            m_strScheduleExportPath = template;
            m_strScheduleDirectPayExportPath = templateDirectPay;
            m_strScheduleLumpSumExportPath = templateLumpSum;
            MediaTypeCode = strMt;
            MediaSubTypeCode = strMst;
            MasterMediaTypeCode = strMmt;
        }

        private int CountSelectMedia()
        {
            int i, count = 0;
            for (i = 0; i < gvDetail.Rows.Count; i++)
            {
                if (Convert.ToBoolean(gvDetail.Rows[i].Cells["SelectMedia"].Value) == true)
                {
                    count++;
                }
            }
            return count;
        }

        private void SetSelectMedia()
        {
            DataTable dt = ((DataTable)gvDetail.DataSource).Clone();
            for (int i = 0; i < gvDetail.Rows.Count; i++)
            {
                if (Convert.ToBoolean(gvDetail.Rows[i].Cells["SelectMedia"].Value) == true)
                {
                    DataRow dr = dt.NewRow();
                    dr["Media_ID"] = gvDetail.Rows[i].Cells["MediaID"].Value;
                    dr["Media_Name"] = gvDetail.Rows[i].Cells["MediaName"].Value;
                    dt.Rows.Add(dr);
                }
            }
            DataTable dtMedia = dt;
            selectMedia = "";
            for (int i = 0; i < dtMedia.Rows.Count; i++)
            {
                if (i == dtMedia.Rows.Count - 1)
                {
                    selectMedia += "'" + dtMedia.Rows[i]["Media_ID"].ToString() + "'";
                }
                else
                {
                    selectMedia += "'" + dtMedia.Rows[i]["Media_ID"].ToString() + "', ";
                }
            }
        }

        public void PrintMediaSchedule(string Template, string BuyingBriefID, string Version, string StartDate, string EndDate, string SelectMedia)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                DataSet ds = m_db.SelectSpotPlanForSchedule(BuyingBriefID, Version, StartDate, EndDate, SelectMedia);
                //======================================
                // Create Result Excel
                //======================================
                Excel.Application ExcelObjDesc = new Excel.Application();
                Excel.Workbook theWorkbook = (Excel.Workbook)(ExcelObjDesc.Workbooks.Open(Template));
                Excel.Sheets sheets = theWorkbook.Worksheets;
                Excel.Worksheet workSheet = (Excel.Worksheet)sheets.get_Item(1);
                //=======================================
                // Buying Brief
                //=======================================
                DataTable dt = ds.Tables[0];
                string creativeAgency = dt.Rows[0]["Creative_Agency_Name"].ToString();
                ExcelUtil.ExcelSetValueString(workSheet, "A", 1, creativeAgency);
                ExcelUtil.ExcelSetValueString(workSheet, "B", 5, dt.Rows[0]["Client_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 6, dt.Rows[0]["Product_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 7, dt.Rows[0]["Campaign_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 8, dt.Rows[0]["Buying_Brief_ID"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 9, dt.Rows[0]["Campaign_Period"].ToString());
                //=======================================
                // Remark
                //=======================================
                ExcelUtil.ExcelSetValueString(workSheet, "A", 31, dt.Rows[0]["Remark"].ToString());
                if (dt.Rows[0]["Commission_Type"] != DBNull.Value
                     && dt.Rows[0]["Commission_Type"].ToString() != "")
                {
                    ExcelUtil.ExcelSetValueString(workSheet, "C", 37, dt.Rows[0]["Commission_Type"].ToString().Replace("%", ""));
                    ExcelUtil.ExcelSetValueString(workSheet, "K", 12, dt.Rows[0]["Commission_Type"].ToString());
                    ExcelUtil.ExcelSetValueString(workSheet, "L", 12, dt.Rows[0]["Commission_Type"].ToString().Replace("%", ""));
                }
                //=======================================
                // Spot Plan (Schedule) Header Logo
                //=======================================
                workSheet.PageSetup.FirstPage.RightHeader.Picture.Filename = dt.Rows[0]["Icon_Path"].ToString();
                workSheet.PageSetup.FirstPage.RightHeader.Text = "&G";
                workSheet.PageSetup.RightHeaderPicture.Filename = dt.Rows[0]["Icon_Path"].ToString();
                workSheet.PageSetup.RightHeader = "&G";
                //=======================================
                // Spot Plan (Schedule) Footer - BARCODE
                //=======================================
                workSheet.PageSetup.FirstPage.LeftFooter.Text = "&\"Free 3 of 9 Extended\"&45& *" + BuyingBriefID + "SC" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*\n" +
                "" + "&\"Arial\"&12&                                              *" + BuyingBriefID + "SC" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*";
                //=======================================
                // Material
                //=======================================
                dt = ds.Tables[1];
                ExcelUtil.ExcelSetValueString(workSheet, "B", 10, dt.Rows[0]["Material"].ToString());
                //=======================================
                // Hide Proprietary Media Remark
                //=======================================
                bool bProprietary = m_db.CheckOptInSchedule(m_strBuyingBriefID, m_strVersion, SelectMedia);
                if (!bProprietary)
                {
                    ExcelUtil.ExcelSetValueString(workSheet, 23, 1, "");
                    ExcelUtil.ExcelSetValueString(workSheet, 24, 1, "");
                    ExcelUtil.ExcelSetValueString(workSheet, 25, 1, "");
                    ExcelUtil.ExcelSetValueString(workSheet, 26, 1, "");
                    ExcelUtil.ExcelSetValueString(workSheet, 27, 1, "");
                    ExcelUtil.ExcelSetValueString(workSheet, 28, 1, "");
                }
                //=======================================
                // Spot Plan
                //=======================================
                dt = m_db.SelectSpotPlanForSchedule_Details(BuyingBriefID, Version, SelectMedia, StartDate, EndDate);
                ExcelUtil.ExcelSetValueStringFullTable(workSheet, 13, 1, 3, dt);
                //=======================================
                // Summary Spot Plan
                //=======================================
                int row = 38;
                if (dt.Rows.Count > 3)
                {
                    row += dt.Rows.Count - 3;
                }
                ExcelUtil.ExcelSetValueStringFullTable(workSheet, row, 1, 3, ds.Tables[2]);
                if (dt.Rows.Count == 0)
                {
                    GMessage.MessageWarning("No Data!");
                }
                else
                {
                    ExcelObjDesc.Visible = true;
                }
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }
            Cursor = Cursors.Default;
        }

        public void PrintMediaSchedule_IT(string Template, string BuyingBriefID, string Version, string StartDate, string EndDate, string LumpSum, string SelectMedia)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                bool bDirectPay = m_db.CheckDirectPaySchedule(m_strBuyingBriefID, m_strVersion, SelectMedia);
                bool bLumpSum = Convert.ToBoolean(LumpSum);
                DataSet ds = m_db.SelectSpotPlanForScheduleIT(BuyingBriefID, Version, StartDate, EndDate, SelectMedia);
                //======================================
                // Create Result Excel
                //======================================
                if (bDirectPay)
                    Template = m_strScheduleDirectPayExportPath;
                if (bLumpSum)
                    Template = m_strScheduleLumpSumExportPath;
                Excel.Application ExcelObjDesc = new Excel.Application();
                Excel.Workbook theWorkbook = (Excel.Workbook)(ExcelObjDesc.Workbooks.Open(Template));
                Excel.Sheets sheets = theWorkbook.Worksheets;
                Excel.Worksheet workSheet = (Excel.Worksheet)sheets.get_Item(1);
                //=======================================
                // Buying Brief
                //=======================================
                DataTable dt = ds.Tables[0];
                string creativeAgency = dt.Rows[0]["Creative_Agency_Name"].ToString();
                ExcelUtil.ExcelSetValueString(workSheet, "A", 1, dt.Rows[0]["Creative_Agency_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 5, dt.Rows[0]["Client_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 6, dt.Rows[0]["Media_Type_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 8, dt.Rows[0]["Product_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 9, dt.Rows[0]["Campaign_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 10, dt.Rows[0]["Buying_Brief_ID"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 11, dt.Rows[0]["Campaign_Period"].ToString());
                //=======================================
                // Remark
                //=======================================
                ExcelUtil.ExcelSetValueString(workSheet, "A", 37, dt.Rows[0]["Remark"].ToString());
                //=======================================
                // Change Wording By Commission Type
                //=======================================
                if (dt.Rows[0]["Commission_Type"] != DBNull.Value
                     && dt.Rows[0]["Commission_Type"].ToString() != "")
                {
                    ExcelUtil.ExcelSetValueString(workSheet, "C", 41, dt.Rows[0]["Commission_Type"].ToString().Replace("%", ""));
                    ExcelUtil.ExcelSetValueString(workSheet, "F", 20, dt.Rows[0]["Commission_Type"].ToString().Replace("%", ""));
                    ExcelUtil.ExcelSetValueString(workSheet, "K", 12, dt.Rows[0]["Commission_Type"].ToString());
                    ExcelUtil.ExcelSetValueString(workSheet, "L", 12, dt.Rows[0]["Commission_Type"].ToString().Replace("%", ""));
                }
                //=======================================
                // Spot Plan (Schedule) Header Logo
                //=======================================
                workSheet.PageSetup.FirstPage.RightHeader.Picture.Filename = dt.Rows[0]["Icon_Path"].ToString();
                workSheet.PageSetup.FirstPage.RightHeader.Text = "&G";
                workSheet.PageSetup.RightHeaderPicture.Filename = dt.Rows[0]["Icon_Path"].ToString();
                workSheet.PageSetup.RightHeader = "&G";
                //=======================================
                // Spot Plan (Schedule) Footer - BARCODE
                //=======================================
                workSheet.PageSetup.FirstPage.LeftFooter.Text = "&\"Free 3 of 9 Extended\"&45& *" + BuyingBriefID + "SC" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*\n" +
                "" + "&\"Arial\"&12&                                              *" + BuyingBriefID + "SC" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*";
                //=======================================
                // Hide Proprietary Media Remark
                //=======================================
                bool bProprietary = m_db.CheckOptInSchedule(m_strBuyingBriefID, m_strVersion, SelectMedia);
                if (!bProprietary)
                {
                    ExcelUtil.ExcelSetValueString(workSheet, 29, 1, "");
                    ExcelUtil.ExcelSetValueString(workSheet, 30, 1, "");
                    ExcelUtil.ExcelSetValueString(workSheet, 31, 1, "");
                    ExcelUtil.ExcelSetValueString(workSheet, 32, 1, "");
                    ExcelUtil.ExcelSetValueString(workSheet, 33, 1, "");
                    ExcelUtil.ExcelSetValueString(workSheet, 34, 1, "");
                }
                //=======================================
                // Hide LINE's Remark
                //=======================================
                bool bLINEVendor = m_db.CheckLINEVendor(m_strBuyingBriefID, m_strVersion);
                if (!bLINEVendor)
                {
                    if (bDirectPay)
                    {
                        ExcelUtil.ExcelSetValueString(workSheet, 55, 1, "");
                        ExcelUtil.ExcelSetValueString(workSheet, 56, 1, "");
                    }
                    else
                    {
                        ExcelUtil.ExcelSetValueString(workSheet, 47, 1, "");
                        ExcelUtil.ExcelSetValueString(workSheet, 48, 1, "");
                    }
                }
                if (bLumpSum)
                {
                    //=======================================
                    // Spot Plan
                    //=======================================
                    ExcelUtil.ExcelSetFormular(workSheet, "J", 19, $"=H17");
                    dt = m_db.SelectSpotPlanForScheduleLSIT_Details(BuyingBriefID, Version, SelectMedia, StartDate, EndDate);
                    if (dt.Rows.Count > 3)
                    {
                        workSheet.Rows[14].Insert(Excel.XlInsertShiftDirection.xlShiftDown, workSheet.Rows[14].Copy(Type.Missing));
                    }
                    ExcelUtil.ExcelSetValueStringFullTable(workSheet, 13, 1, 4, dt);
                }
                else
                {
                    //=======================================
                    // Spot Plan
                    //=======================================
                    dt = m_db.SelectSpotPlanForScheduleIT_Details(BuyingBriefID, Version, SelectMedia, StartDate, EndDate, "AG");
                    if (dt.Rows.Count > 3)
                    {
                        workSheet.Rows[14].Insert(Excel.XlInsertShiftDirection.xlShiftDown, workSheet.Rows[14].Copy(Type.Missing));
                    }
                    ExcelUtil.ExcelSetValueStringFullTable(workSheet, 13, 1, 4, dt);
                    //=======================================
                    // Summary Spot Plan
                    //=======================================
                    int row = 42;
                    if (dt.Rows.Count > 3)
                    {
                        row += dt.Rows.Count - 3;
                    }
                    if (ds.Tables[2].Rows.Count > 2)
                    {
                        workSheet.Rows[row + 1].Insert(Excel.XlInsertShiftDirection.xlShiftDown, workSheet.Rows[row + 1].Copy(Type.Missing));
                    }
                    ExcelUtil.ExcelSetValueStringFullTable(workSheet, row, 1, 3, ds.Tables[2]);
                    //=======================================
                    // Client Direct Pay
                    //=======================================
                    if (bDirectPay)
                    {
                        row = 50;
                        if (dt.Rows.Count > 3)
                        {
                            row += dt.Rows.Count - 3;
                        }
                        if (ds.Tables[2].Rows.Count > 2)
                        {
                            row += ds.Tables[2].Rows.Count - 2;
                        }
                        dt = m_db.SelectSpotPlanForScheduleIT_Details(BuyingBriefID, Version, SelectMedia, StartDate, EndDate, "CL");
                        if (dt.Rows.Count > 2)
                        {
                            workSheet.Rows[row + 1].Insert(Excel.XlInsertShiftDirection.xlShiftDown, workSheet.Rows[row + 1].Copy(Type.Missing));
                        }
                        ExcelUtil.ExcelSetValueStringFullTable(workSheet, row, 1, 3, dt);
                    }
                }
                if (dt.Rows.Count == 0)
                {
                    GMessage.MessageWarning("No Data!");
                }
                else
                {
                    workSheet.Protect(username, true);
                    ExcelObjDesc.Visible = true;
                }
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }
            Cursor = Cursors.Default;
        }

        public void PrintMediaSchedule_ES(string Template, string BuyingBriefID, string Version, string StartDate, string EndDate, string LumpSum, string SelectMedia)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                bool bDirectPay = m_db.CheckDirectPaySchedule(m_strBuyingBriefID, m_strVersion, SelectMedia);
                bool bLumpSum = Convert.ToBoolean(LumpSum);
                DataSet ds = m_db.SelectSpotPlanForScheduleES(BuyingBriefID, Version, StartDate, EndDate, SelectMedia);
                //======================================
                // Create Result Excel
                //======================================
                if (bDirectPay)
                    Template = m_strScheduleDirectPayExportPath;
                if (bLumpSum)
                    Template = m_strScheduleLumpSumExportPath;
                Excel.Application ExcelObjDesc = new Excel.Application();
                Excel.Workbook theWorkbook = (Excel.Workbook)(ExcelObjDesc.Workbooks.Open(Template));
                Excel.Sheets sheets = theWorkbook.Worksheets;
                Excel.Worksheet workSheet = (Excel.Worksheet)sheets.get_Item(1);
                //=======================================
                // Buying Brief
                //=======================================
                DataTable dt = ds.Tables[0];
                string creativeAgency = dt.Rows[0]["Creative_Agency_Name"].ToString();
                ExcelUtil.ExcelSetValueString(workSheet, "A", 1, dt.Rows[0]["Creative_Agency_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 5, dt.Rows[0]["Client_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 6, dt.Rows[0]["Media_Type_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 8, dt.Rows[0]["Product_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 9, dt.Rows[0]["Campaign_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 10, dt.Rows[0]["Buying_Brief_ID"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 11, dt.Rows[0]["Campaign_Period"].ToString());
                //=======================================
                // Remark
                //=======================================
                ExcelUtil.ExcelSetValueString(workSheet, "A", 37, dt.Rows[0]["Remark"].ToString());
                //=======================================
                // Change Wording By Commission Type
                //=======================================
                if (dt.Rows[0]["Commission_Type"] != DBNull.Value
                     && dt.Rows[0]["Commission_Type"].ToString() != "")
                {
                    ExcelUtil.ExcelSetValueString(workSheet, "C", 41, dt.Rows[0]["Commission_Type"].ToString().Replace("%", ""));
                    ExcelUtil.ExcelSetValueString(workSheet, "F", 20, dt.Rows[0]["Commission_Type"].ToString().Replace("%", ""));
                    ExcelUtil.ExcelSetValueString(workSheet, "K", 12, dt.Rows[0]["Commission_Type"].ToString());
                    ExcelUtil.ExcelSetValueString(workSheet, "L", 12, dt.Rows[0]["Commission_Type"].ToString().Replace("%", ""));
                }
                //=======================================
                // Spot Plan (Schedule) Header Logo
                //=======================================
                workSheet.PageSetup.FirstPage.RightHeader.Picture.Filename = dt.Rows[0]["Icon_Path"].ToString();
                workSheet.PageSetup.FirstPage.RightHeader.Text = "&G";
                workSheet.PageSetup.RightHeaderPicture.Filename = dt.Rows[0]["Icon_Path"].ToString();
                workSheet.PageSetup.RightHeader = "&G";
                //=======================================
                // Spot Plan (Schedule) Footer - BARCODE
                //=======================================
                workSheet.PageSetup.FirstPage.LeftFooter.Text = "&\"Free 3 of 9 Extended\"&45& *" + BuyingBriefID + "SC" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*\n" +
                "" + "&\"Arial\"&12&                                              *" + BuyingBriefID + "SC" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*";
                //=======================================
                // Hide Proprietary Media Remark
                //=======================================
                bool bProprietary = m_db.CheckOptInSchedule(m_strBuyingBriefID, m_strVersion, SelectMedia);
                if (!bProprietary)
                {
                    ExcelUtil.ExcelSetValueString(workSheet, 29, 1, "");
                    ExcelUtil.ExcelSetValueString(workSheet, 30, 1, "");
                    ExcelUtil.ExcelSetValueString(workSheet, 31, 1, "");
                    ExcelUtil.ExcelSetValueString(workSheet, 32, 1, "");
                    ExcelUtil.ExcelSetValueString(workSheet, 33, 1, "");
                    ExcelUtil.ExcelSetValueString(workSheet, 34, 1, "");
                    if (bDirectPay)
                    {
                        ExcelUtil.ExcelSetValueString(workSheet, 59, 1, "");
                        ExcelUtil.ExcelSetValueString(workSheet, 60, 1, "");
                        ExcelUtil.ExcelSetValueString(workSheet, 61, 1, "");
                        ExcelUtil.ExcelSetValueString(workSheet, 62, 1, "");
                        ExcelUtil.ExcelSetValueString(workSheet, 63, 1, "");
                    }
                    else
                    {
                        ExcelUtil.ExcelSetValueString(workSheet, 51, 1, "");
                        ExcelUtil.ExcelSetValueString(workSheet, 52, 1, "");
                        ExcelUtil.ExcelSetValueString(workSheet, 53, 1, "");
                        ExcelUtil.ExcelSetValueString(workSheet, 54, 1, "");
                        ExcelUtil.ExcelSetValueString(workSheet, 55, 1, "");
                    }
                }
                //=======================================
                // Hide LINE's Remark
                //=======================================
                bool bLINEVendor = m_db.CheckLINEVendor(m_strBuyingBriefID, m_strVersion);
                if (!bLINEVendor)
                {
                    if (bDirectPay)
                    {
                        ExcelUtil.ExcelSetValueString(workSheet, 55, 1, "");
                        ExcelUtil.ExcelSetValueString(workSheet, 56, 1, "");
                    }
                    else
                    {
                        ExcelUtil.ExcelSetValueString(workSheet, 47, 1, "");
                        ExcelUtil.ExcelSetValueString(workSheet, 48, 1, "");
                    }
                }
                if (bLumpSum)
                {
                    //=======================================
                    // Spot Plan
                    //=======================================
                    ExcelUtil.ExcelSetFormular(workSheet, "J", 19, $"=H17");
                    dt = m_db.SelectSpotPlanForScheduleLSES_Details(BuyingBriefID, Version, SelectMedia, StartDate, EndDate);
                    if (dt.Rows.Count > 3)
                    {
                        workSheet.Rows[14].Insert(Excel.XlInsertShiftDirection.xlShiftDown, workSheet.Rows[14].Copy(Type.Missing));
                    }
                    ExcelUtil.ExcelSetValueStringFullTable(workSheet, 13, 1, 4, dt);
                }
                else
                {
                    //=======================================
                    // Spot Plan
                    //=======================================
                    dt = m_db.SelectSpotPlanForScheduleES_Details(BuyingBriefID, Version, SelectMedia, StartDate, EndDate, "AG");
                    if (dt.Rows.Count > 3)
                    {
                        workSheet.Rows[14].Insert(Excel.XlInsertShiftDirection.xlShiftDown, workSheet.Rows[14].Copy(Type.Missing));
                    }
                    ExcelUtil.ExcelSetValueStringFullTable(workSheet, 13, 1, 4, dt);
                    //=======================================
                    // Summary Spot Plan
                    //=======================================
                    int row = 42;
                    if (dt.Rows.Count > 3)
                    {
                        row += dt.Rows.Count - 3;
                    }
                    if (ds.Tables[2].Rows.Count > 2)
                    {
                        workSheet.Rows[row + 1].Insert(Excel.XlInsertShiftDirection.xlShiftDown, workSheet.Rows[row + 1].Copy(Type.Missing));
                    }
                    ExcelUtil.ExcelSetValueStringFullTable(workSheet, row, 1, 3, ds.Tables[2]);

                    //=======================================
                    // Client Direct Pay
                    //=======================================
                    if (bDirectPay)
                    {
                        row = 50;
                        if (dt.Rows.Count > 3)
                        {
                            row += dt.Rows.Count - 3;
                        }
                        if (ds.Tables[2].Rows.Count > 2)
                        {
                            row += ds.Tables[2].Rows.Count - 2;
                        }
                        dt = m_db.SelectSpotPlanForScheduleES_Details(BuyingBriefID, Version, SelectMedia, StartDate, EndDate, "CL");
                        if (dt.Rows.Count > 2)
                        {
                            workSheet.Rows[row + 1].Insert(Excel.XlInsertShiftDirection.xlShiftDown, workSheet.Rows[row + 1].Copy(Type.Missing));
                        }
                        ExcelUtil.ExcelSetValueStringFullTable(workSheet, row, 1, 3, dt);
                    }
                }
                if (dt.Rows.Count == 0)
                {
                    GMessage.MessageWarning("No Data!");
                }
                else
                {
                    workSheet.Protect(username, true);
                    ExcelObjDesc.Visible = true;
                }
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }
            Cursor = Cursors.Default;
        }

        public void PrintMediaSchedule_OD(string Template, string BuyingBriefID, string Version, string StartDate, string EndDate, string LumpSum, string SelectMedia)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                bool bDirectPay = m_db.CheckDirectPaySchedule(m_strBuyingBriefID, m_strVersion, SelectMedia);
                //Modified by Chaiwat.i 10/03/2023 TFS 158558 [T2] : LINE's remark has been disappear when print online schedule on C#
                bool bLINEVendor = m_db.CheckLINEVendor(m_strBuyingBriefID, m_strVersion);
                bool bLumpSum = Convert.ToBoolean(LumpSum);
                DataSet ds = m_db.SelectSpotPlanForScheduleIT(BuyingBriefID, Version, StartDate, EndDate, SelectMedia);
                //======================================
                // Create Result Excel
                //======================================
                if (bDirectPay)
                    Template = m_strScheduleDirectPayExportPath;
                if (bLumpSum)
                    Template = m_strScheduleLumpSumExportPath;
                Excel.Application ExcelObjDesc = new Excel.Application();
                Excel.Workbook theWorkbook = (Excel.Workbook)(ExcelObjDesc.Workbooks.Open(Template));
                Excel.Sheets sheets = theWorkbook.Worksheets;
                Excel.Worksheet workSheet = (Excel.Worksheet)sheets.get_Item(1);//Template
                ExcelObjDesc.Visible = true;
                //=======================================
                // Buying Brief
                //=======================================
                DataTable dt = ds.Tables[0];
                string creativeAgency = dt.Rows[0]["Creative_Agency_Name"].ToString();
                ExcelUtil.ExcelSetValueString(workSheet, "A", 1, dt.Rows[0]["Creative_Agency_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 5, dt.Rows[0]["Client_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 6, dt.Rows[0]["Media_Type_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 8, dt.Rows[0]["Product_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 9, dt.Rows[0]["Campaign_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 10, dt.Rows[0]["Buying_Brief_ID"].ToString()); //+ (dt.Rows[0]["Revised_No"].ToString() != "" ? "  (Revised no : " + dt.Rows[0]["Revised_No"].ToString() + ")" : ""));
                ExcelUtil.ExcelSetValueString(workSheet, "B", 11, dt.Rows[0]["Campaign_Period"].ToString());
                //=======================================
                // Remark
                //=======================================
                ExcelUtil.ExcelSetValueString(workSheet, "A", 37, dt.Rows[0]["Remark"].ToString()); //Modified by Chaiwat.i 02/02/2023 : replacing Remark's row on Media schedule from 35 to 37
                //if (dt.Rows[0]["Commission_Type"] != DBNull.Value
                //     && dt.Rows[0]["Commission_Type"].ToString() != "")
                //{
                //    ExcelUtil.ExcelSetValueString(workSheet, "K", 12, dt.Rows[0]["Commission_Type"].ToString());
                //    ExcelUtil.ExcelSetValueString(workSheet, "L", 12, dt.Rows[0]["Commission_Type"].ToString().Replace("%", ""));
                //    ExcelUtil.ExcelSetValueString(workSheet, "F", 20, dt.Rows[0]["Commission_Type"].ToString().Replace("%", ""));
                //    ExcelUtil.ExcelSetValueString(workSheet, "C", 41, dt.Rows[0]["Commission_Type"].ToString().Replace("%", ""));
                //}
                //=======================================
                // Spot Plan (Schedule) Header Logo
                //=======================================
                workSheet.PageSetup.FirstPage.RightHeader.Picture.Filename = dt.Rows[0]["Icon_Path"].ToString();
                workSheet.PageSetup.FirstPage.RightHeader.Text = "&G";
                workSheet.PageSetup.RightHeaderPicture.Filename = dt.Rows[0]["Icon_Path"].ToString();
                workSheet.PageSetup.RightHeader = "&G";
                //=======================================
                // Spot Plan (Schedule) Footer - BARCODE
                //=======================================
                workSheet.PageSetup.FirstPage.LeftFooter.Text = "&\"Free 3 of 9 Extended\"&45& *" + BuyingBriefID + "SC" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*\n" +
                "" + "&\"Arial\"&12&                                              *" + BuyingBriefID + "SC" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*";
                //=======================================
                // Display Proprietary Media Remark
                //=======================================
                bool bProprietary = m_db.CheckOptInSchedule(m_strBuyingBriefID, m_strVersion, SelectMedia);
                if (!bProprietary)
                {
                    ExcelUtil.ExcelSetValueString(workSheet, 29, 1, "");
                    ExcelUtil.ExcelSetValueString(workSheet, 30, 1, "");
                    ExcelUtil.ExcelSetValueString(workSheet, 31, 1, "");
                    ExcelUtil.ExcelSetValueString(workSheet, 32, 1, "");
                    ExcelUtil.ExcelSetValueString(workSheet, 33, 1, "");
                    ExcelUtil.ExcelSetValueString(workSheet, 34, 1, "");
                }
                if (bLumpSum)
                {
                    //=======================================
                    // Spot Plan
                    //=======================================
                    ExcelUtil.ExcelSetFormular(workSheet, "J", 19, $"=H17");
                    dt = m_db.SelectSpotPlanForScheduleLSIT_Details(BuyingBriefID, Version, SelectMedia, StartDate, EndDate);
                    if (dt.Rows.Count > 3)
                    {
                        workSheet.Rows[14].Insert(Excel.XlInsertShiftDirection.xlShiftDown, workSheet.Rows[14].Copy(Type.Missing));
                    }
                    ExcelUtil.ExcelSetValueStringFullTable(workSheet, 13, 1, 4, dt);
                }
                else
                {
                    //=======================================
                    // Spot Plan
                    //=======================================
                    dt = m_db.SelectSpotPlanForScheduleOD_Details(BuyingBriefID, Version, SelectMedia, StartDate, EndDate, "AG");
                    if (dt.Rows.Count > 3)
                    {
                        workSheet.Rows[14].Insert(Excel.XlInsertShiftDirection.xlShiftDown, workSheet.Rows[14].Copy(Type.Missing));
                    }
                    ExcelUtil.ExcelSetValueStringFullTable(workSheet, 13, 1, 4, dt);
                    //=======================================
                    // Summary Spot Plan
                    //=======================================
                    int rowMediaCost = 43;
                    int rowAgencyCommission = 44;
                    int rowSubTotal = 45;
                    int rowVat = 46;
                    int rowTotal = 47;

                    int rowStart = 43;
                    int row = rowStart;
                    int rowGrandTotal = 48;

                    DataTable dtSummary = m_db.SelectSpotPlanForScheduleOD_Summary(BuyingBriefID, Version, SelectMedia, StartDate, EndDate, "AG", StartDate.Substring(0, 4));
                    DataView view = new DataView(dtSummary);
                    DataTable dtSummaryAdeptMediaType = view.ToTable(true, "Adept_Media_Type_Name");

                    string sumMediaCost = "";
                    string sumAgencyCommission = "";
                    string sumSubTotal = "";
                    string sumVat = "";
                    string sumTotal = "";


                    string sumGrandTotal = "";

                    ExcelUtil.ExcelSetValueString(workSheet, row - 1, 2, StartDate.Substring(0, 4));
                    for (int i = 0; i < dtSummaryAdeptMediaType.Rows.Count; i++)
                    {
                        if (sumMediaCost == "") sumMediaCost = "O" + rowMediaCost; else sumMediaCost = sumMediaCost + "+O" + rowMediaCost;
                        if (sumAgencyCommission == "") sumAgencyCommission = "O" + rowAgencyCommission; else sumAgencyCommission = sumAgencyCommission + "+O" + rowAgencyCommission;
                        if (sumSubTotal == "") sumSubTotal = "O" + rowSubTotal; else sumSubTotal = sumSubTotal + "+O" + rowSubTotal;
                        if (sumVat == "") sumVat = "O" + rowVat; else sumVat = sumVat + "+O" + rowVat;
                        if (sumTotal == "") sumTotal = "O" + rowTotal; else sumTotal = sumTotal + "+O" + rowTotal;

                        //Copy row template header and detail
                        Excel.Range copyRange = workSheet.Range[$"A{row}:A{row + 4}"].EntireRow;
                        Excel.Range insertRange = workSheet.Range[$"A{row}"].EntireRow;
                        insertRange.Insert(Excel.XlInsertShiftDirection.xlShiftDown, copyRange.Copy());

                        string strAdeptMediaType = dtSummaryAdeptMediaType.Rows[i]["Adept_Media_Type_Name"].ToString();
                        DataTable dtSummarySelectAdeptMediaType = dtSummary.AsEnumerable().Where(x => x.Field<string>("Adept_Media_Type_Name") == strAdeptMediaType).CopyToDataTable();
                        ExcelUtil.ExcelSetValueString(workSheet, row, 1, strAdeptMediaType);
                        ExcelUtil.ExcelSetValueRangeDouble(workSheet, $"C{row}", dtSummarySelectAdeptMediaType.Rows.Count, dtSummarySelectAdeptMediaType, 2);

                        if (sumGrandTotal == "")
                            sumGrandTotal = "[Col]" + (row + 4);
                        else
                            sumGrandTotal = sumGrandTotal + "+[Col]" + (row + 4);

                        rowMediaCost = rowMediaCost + 5;
                        rowAgencyCommission = rowAgencyCommission + 5;
                        rowSubTotal = rowSubTotal + 5;
                        rowVat = rowVat + 5;
                        rowTotal = rowTotal + 5;

                        row = row + 5;
                        rowGrandTotal = rowGrandTotal + 5;

                    }
                    if (dtSummaryAdeptMediaType.Rows.Count > 0)
                    {
                        Excel.Range deleteRow = workSheet.Range[$"A{row}:A{row + 4}"].EntireRow;
                        deleteRow.EntireRow.Delete(Excel.XlDeleteShiftDirection.xlShiftUp);
                        rowGrandTotal = rowGrandTotal - 5;
                    }
                    ExcelUtil.ExcelSetFormular(workSheet, "C", rowGrandTotal, "=" + sumGrandTotal.Replace("[Col]", "C"));
                    ExcelUtil.ExcelSetFormular(workSheet, "D", rowGrandTotal, "=" + sumGrandTotal.Replace("[Col]", "D"));
                    ExcelUtil.ExcelSetFormular(workSheet, "E", rowGrandTotal, "=" + sumGrandTotal.Replace("[Col]", "E"));
                    ExcelUtil.ExcelSetFormular(workSheet, "F", rowGrandTotal, "=" + sumGrandTotal.Replace("[Col]", "F"));
                    ExcelUtil.ExcelSetFormular(workSheet, "G", rowGrandTotal, "=" + sumGrandTotal.Replace("[Col]", "G"));
                    ExcelUtil.ExcelSetFormular(workSheet, "H", rowGrandTotal, "=" + sumGrandTotal.Replace("[Col]", "H"));
                    ExcelUtil.ExcelSetFormular(workSheet, "I", rowGrandTotal, "=" + sumGrandTotal.Replace("[Col]", "I"));
                    ExcelUtil.ExcelSetFormular(workSheet, "J", rowGrandTotal, "=" + sumGrandTotal.Replace("[Col]", "J"));
                    ExcelUtil.ExcelSetFormular(workSheet, "K", rowGrandTotal, "=" + sumGrandTotal.Replace("[Col]", "K"));
                    ExcelUtil.ExcelSetFormular(workSheet, "L", rowGrandTotal, "=" + sumGrandTotal.Replace("[Col]", "L"));
                    ExcelUtil.ExcelSetFormular(workSheet, "M", rowGrandTotal, "=" + sumGrandTotal.Replace("[Col]", "M"));
                    ExcelUtil.ExcelSetFormular(workSheet, "N", rowGrandTotal, "=" + sumGrandTotal.Replace("[Col]", "N"));

                    Excel.Range hiddenRow = workSheet.Range[$"A{rowStart}:A{row - 1}"].EntireRow;
                    hiddenRow.EntireRow.Hidden = true;



                    rowMediaCost = rowMediaCost + 3;
                    rowAgencyCommission = rowAgencyCommission + 3;
                    rowSubTotal = rowSubTotal + 3;
                    rowVat = rowVat + 3;
                    rowTotal = rowTotal + 3;

                    row = row + 3;
                    rowStart = row;
                    rowGrandTotal = rowGrandTotal + 8;
                    //2nd Year
                    if (StartDate.Substring(0, 4) == EndDate.Substring(0, 4))
                    {
                        Excel.Range deleteRow = workSheet.Range[$"A{row}:A{rowGrandTotal + 1}"].EntireRow;
                        deleteRow.EntireRow.Delete(Excel.XlDeleteShiftDirection.xlShiftUp);
                    }
                    else
                    {
                        dtSummary = m_db.SelectSpotPlanForScheduleOD_Summary(BuyingBriefID, Version, SelectMedia, StartDate, EndDate, "AG", EndDate.Substring(0, 4));
                        view = new DataView(dtSummary);
                        dtSummaryAdeptMediaType = view.ToTable(true, "Adept_Media_Type_Name");
                        sumGrandTotal = "";
                        ExcelUtil.ExcelSetValueString(workSheet, row - 1, 2, EndDate.Substring(0, 4));
                        for (int i = 0; i < dtSummaryAdeptMediaType.Rows.Count; i++)
                        {
                            if (sumMediaCost == "") sumMediaCost = "O" + rowMediaCost; else sumMediaCost = sumMediaCost + "+O" + rowMediaCost;
                            if (sumAgencyCommission == "") sumAgencyCommission = "O" + rowAgencyCommission; else sumAgencyCommission = sumAgencyCommission + "+O" + rowAgencyCommission;
                            if (sumSubTotal == "") sumSubTotal = "O" + rowSubTotal; else sumSubTotal = sumSubTotal + "+O" + rowSubTotal;
                            if (sumVat == "") sumVat = "O" + rowVat; else sumVat = sumVat + "+O" + rowVat;
                            if (sumTotal == "") sumTotal = "O" + rowTotal; else sumTotal = sumTotal + "+O" + rowTotal;

                            //Copy row template header and detail
                            Excel.Range copyRange = workSheet.Range[$"A{row}:A{row + 4}"].EntireRow;
                            Excel.Range insertRange = workSheet.Range[$"A{row}"].EntireRow;
                            insertRange.Insert(Excel.XlInsertShiftDirection.xlShiftDown, copyRange.Copy());

                            string strAdeptMediaType = dtSummaryAdeptMediaType.Rows[i]["Adept_Media_Type_Name"].ToString();
                            DataTable dtSummarySelectAdeptMediaType = dtSummary.AsEnumerable().Where(x => x.Field<string>("Adept_Media_Type_Name") == strAdeptMediaType).CopyToDataTable();
                            ExcelUtil.ExcelSetValueString(workSheet, row, 1, strAdeptMediaType);
                            ExcelUtil.ExcelSetValueRangeDouble(workSheet, $"C{row}", dtSummarySelectAdeptMediaType.Rows.Count, dtSummarySelectAdeptMediaType, 2);

                            if (sumGrandTotal == "")
                                sumGrandTotal = "[Col]" + (row + 4);
                            else
                                sumGrandTotal = sumGrandTotal + "+[Col]" + (row + 4);

                            rowMediaCost = rowMediaCost + 5;
                            rowAgencyCommission = rowAgencyCommission + 5;
                            rowSubTotal = rowSubTotal + 5;
                            rowVat = rowVat + 5;
                            rowTotal = rowTotal + 5;

                            row = row + 5;
                            rowGrandTotal = rowGrandTotal + 5;
                        }
                        if (dtSummaryAdeptMediaType.Rows.Count > 0)
                        {
                            Excel.Range deleteRow = workSheet.Range[$"A{row}:A{row + 4}"].EntireRow;
                            deleteRow.EntireRow.Delete(Excel.XlDeleteShiftDirection.xlShiftUp);
                            rowGrandTotal = rowGrandTotal - 5;
                        }
                        ExcelUtil.ExcelSetFormular(workSheet, "C", rowGrandTotal, "=" + sumGrandTotal.Replace("[Col]", "C"));
                        ExcelUtil.ExcelSetFormular(workSheet, "D", rowGrandTotal, "=" + sumGrandTotal.Replace("[Col]", "D"));
                        ExcelUtil.ExcelSetFormular(workSheet, "E", rowGrandTotal, "=" + sumGrandTotal.Replace("[Col]", "E"));
                        ExcelUtil.ExcelSetFormular(workSheet, "F", rowGrandTotal, "=" + sumGrandTotal.Replace("[Col]", "F"));
                        ExcelUtil.ExcelSetFormular(workSheet, "G", rowGrandTotal, "=" + sumGrandTotal.Replace("[Col]", "G"));
                        ExcelUtil.ExcelSetFormular(workSheet, "H", rowGrandTotal, "=" + sumGrandTotal.Replace("[Col]", "H"));
                        ExcelUtil.ExcelSetFormular(workSheet, "I", rowGrandTotal, "=" + sumGrandTotal.Replace("[Col]", "I"));
                        ExcelUtil.ExcelSetFormular(workSheet, "J", rowGrandTotal, "=" + sumGrandTotal.Replace("[Col]", "J"));
                        ExcelUtil.ExcelSetFormular(workSheet, "K", rowGrandTotal, "=" + sumGrandTotal.Replace("[Col]", "K"));
                        ExcelUtil.ExcelSetFormular(workSheet, "L", rowGrandTotal, "=" + sumGrandTotal.Replace("[Col]", "L"));
                        ExcelUtil.ExcelSetFormular(workSheet, "M", rowGrandTotal, "=" + sumGrandTotal.Replace("[Col]", "M"));
                        ExcelUtil.ExcelSetFormular(workSheet, "N", rowGrandTotal, "=" + sumGrandTotal.Replace("[Col]", "N"));

                        if (dtSummaryAdeptMediaType.Rows.Count == 0)
                        {
                            hiddenRow = workSheet.Range[$"A{rowStart - 1}:A{row + 5}"].EntireRow;
                            hiddenRow.EntireRow.Hidden = true;
                        }
                        else
                        {
                            hiddenRow = workSheet.Range[$"A{rowStart}:A{row - 1}"].EntireRow;
                            hiddenRow.EntireRow.Hidden = true;
                        }
                    }

                    ExcelUtil.ExcelSetFormular(workSheet, "Q", 19, "=" + sumMediaCost);
                    ExcelUtil.ExcelSetFormular(workSheet, "Q", 20, "=" + sumAgencyCommission);
                    ExcelUtil.ExcelSetFormular(workSheet, "Q", 21, "=" + sumSubTotal);
                    ExcelUtil.ExcelSetFormular(workSheet, "Q", 22, "=" + sumVat);
                    ExcelUtil.ExcelSetFormular(workSheet, "Q", 23, "=" + sumTotal);

                    //=======================================
                    // Client Direct Pay
                    //=======================================
                    if (bDirectPay)
                    {
                        row = 50;
                        if (dt.Rows.Count > 3)
                        {
                            row += dt.Rows.Count - 3;
                        }
                        if (ds.Tables[2].Rows.Count > 2)
                        {
                            row += ds.Tables[2].Rows.Count - 2;
                        }
                        dt = m_db.SelectSpotPlanForScheduleIT_Details(BuyingBriefID, Version, SelectMedia, StartDate, EndDate, "CL");
                        if (dt.Rows.Count > 2)
                        {
                            workSheet.Rows[row + 1].Insert(Excel.XlInsertShiftDirection.xlShiftDown, workSheet.Rows[row + 1].Copy(Type.Missing));
                        }
                        ExcelUtil.ExcelSetValueStringFullTable(workSheet, row, 1, 3, dt);
                    }
                    //Modified by Chaiwat.i 10/03/2023 TFS 158558 [T2] : LINE's remark has been disappear when print online schedule on C#
                    //=======================================
                    // LINE's remark
                    //=======================================
                    if (bLINEVendor)
                    {
                        ExcelUtil.ExcelSetValueString(workSheet, "A", row + 6, "Client acknowledges and agrees that Agency will purchase any LINE Services for client via Insert Order (\"IO\") as attached which is subjected to the terms and conditions and guidelines available online at ");
                        ExcelUtil.ExcelSetValueString(workSheet, "A", row + 7, "http://www.linebiz.com/th/terms-and-policies/ for the applicable services (collectively the \"Terms\").");
                    }
                }
                if (dt.Rows.Count == 0)
                {
                    GMessage.MessageWarning("No Data!");
                }
                else
                {
                    //Modified by Chaiwat.i 15/03/2023 TFS159033 [T2] : Set protect excel sheet when print report (C#)
                    workSheet.Protect(username, true);
                    ExcelObjDesc.Visible = true;
                }
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }
            this.Cursor = Cursors.Default;
        }

        private void Implementation_SpotPlan_Print_Schedule_Load(object sender, EventArgs e)
        {
            dtStartDate.Value = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strStartDate);
            dtEndDate.Value = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strEndDate);
            if (MediaSubTypeCode != "")
            {
                chkLumpSum.Visible = false;
            }
            gvDetail.AutoGenerateColumns = false;
            gvDetail.DataSource = m_db.SelectMediaSC(m_strBuyingBriefID, m_strVersion);
            chkSelectAll.CheckState = CheckState.Checked;
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGv == true)
            {
                if (chkSelectAll.CheckState == CheckState.Checked)
                {
                    statusChk = "check";
                }
                if (chkSelectAll.CheckState == CheckState.Indeterminate)
                {
                    statusChk = "indeterminate";
                }
                if (chkSelectAll.CheckState == CheckState.Unchecked)
                {
                    statusChk = "uncheck";
                }
            }
            else
            {
                int i = 0;
                if (statusChk == "check")
                {
                    chkSelectAll.CheckState = CheckState.Unchecked;
                    statusChk = "uncheck";
                    for (i = 0; i < gvDetail.Rows.Count; i++)
                    {
                        gvDetail.Rows[i].Cells["SelectMedia"].Value = false;
                    }
                }
                else
                {
                    chkSelectAll.CheckState = CheckState.Checked;
                    statusChk = "check";
                    for (i = 0; i < gvDetail.Rows.Count; i++)
                    {
                        gvDetail.Rows[i].Cells["SelectMedia"].Value = true;
                    }
                }
            }
        }

        private void gvDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            gvDetail.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void gvDetail_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            chkGv = true;
            int totalSelectMedia = CountSelectMedia();
            if (totalSelectMedia == 0)
            {
                chkSelectAll.CheckState = CheckState.Unchecked;
            }
            else
            {
                if (totalSelectMedia == gvDetail.Rows.Count)
                {
                    chkSelectAll.CheckState = CheckState.Checked;
                }
                else
                {
                    chkSelectAll.CheckState = CheckState.Indeterminate;
                }
            }
            chkGv = false;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (chkSelectAll.CheckState == CheckState.Unchecked)
            {
                GMessage.MessageWarning("Please Select Media.");
            }
            else
            {
                bool mtES = false;
                bool mtIT = false;
                bool mtOD = false;
                string sd = dtStartDate.Value.ToString("yyyyMMdd");
                string ed = dtEndDate.Value.ToString("yyyyMMdd");
                string ls = chkLumpSum.Checked.ToString();
                SetSelectMedia();
                // check master media type
                if (MasterMediaTypeCode == "")
                {
                    mtES = m_db.ValidateMasterMediaType("ES", MediaTypeCode);
                    mtIT = m_db.ValidateMasterMediaType("IT", MediaTypeCode);
                    mtOD = m_db.ValidateMasterMediaType("OD", MediaTypeCode);
                }
                else
                {
                    if (MasterMediaTypeCode == "ES")
                        mtES = true;
                    if (MasterMediaTypeCode == "IT")
                        mtIT = true;
                    if (MasterMediaTypeCode == "OD")
                        mtOD = true;
                }
                // check finecast / connected tv
                bool fc = (mtES || mtIT || mtOD) ? false : true;
                // print media schedule
                if (fc)
                {
                    PrintMediaSchedule(m_strScheduleExportPath, m_strBuyingBriefID, m_strVersion, sd, ed, selectMedia);
                }
                else
                {
                    if (mtES)
                        PrintMediaSchedule_ES(m_strScheduleExportPath, m_strBuyingBriefID, m_strVersion, sd, ed, ls, selectMedia);
                    if (mtIT)
                        PrintMediaSchedule_IT(m_strScheduleExportPath, m_strBuyingBriefID, m_strVersion, sd, ed, ls, selectMedia);
                    if (mtOD)
                        PrintMediaSchedule_OD(m_strScheduleExportPath, m_strBuyingBriefID, m_strVersion, sd, ed, ls, selectMedia);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
