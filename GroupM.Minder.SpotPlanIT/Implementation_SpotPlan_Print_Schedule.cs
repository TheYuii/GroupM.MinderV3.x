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

namespace GroupM.Minder.SpotPlanIT
{
    public partial class Implementation_SpotPlan_Print_Schedule : Form
    {
        private string m_strBuyingBriefID;
        private string m_strVersion;
        private string m_strStartDate;
        private string m_strEndDate;
        private string m_strScheduleExportPath;
        private string m_strScheduleDirectPayExportPath;
        private DBManager m_db = null;
        private bool chkGv = false;
        private string statusChk = "";
        private string selectMedia = "";
        private string MediaTypeCode = "";
        private string MediaSubTypeCode = "";

        public Implementation_SpotPlan_Print_Schedule(string BB, string version, string startDate, string endDate, string template, string templateDirectPay, string strMt, string strMst)
        {
            InitializeComponent();
            m_db = new DBManager();
            m_strBuyingBriefID = BB;
            m_strVersion = version;
            m_strStartDate = startDate;
            m_strEndDate = endDate;
            m_strScheduleExportPath = template;
            m_strScheduleDirectPayExportPath = templateDirectPay;
            MediaTypeCode = strMt;
            MediaSubTypeCode = strMst;
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

        public void PrintMediaSchedule(string Template, string BuyingBriefID, string Version, string StartDate, string EndDate, string SelectMedia, string mst)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                DataSet ds = m_db.SelectSpotPlanForSchedule(BuyingBriefID, Version, StartDate, EndDate, SelectMedia);
                //======================================
                // Create Result Excel
                //======================================
                Excel.Application ExcelObjDesc = new Excel.Application();
                Excel.Workbook theWorkbook = (Excel.Workbook)(ExcelObjDesc.Workbooks.Open(Template));
                Excel.Sheets sheets = theWorkbook.Worksheets;
                Excel.Worksheet workSheet = (Excel.Worksheet)sheets.get_Item(1);//Template
                //=======================================
                // Buying Brief
                //=======================================
                DataTable dt = ds.Tables[0];
                string creativeAgency = dt.Rows[0]["Creative_Agency_Name"].ToString();
                ExcelUtil.ExcelSetValueString(workSheet, "A", 1, dt.Rows[0]["Creative_Agency_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 5, dt.Rows[0]["Client_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 6, dt.Rows[0]["Product_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 7, dt.Rows[0]["Campaign_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 8, dt.Rows[0]["Buying_Brief_ID"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 9, dt.Rows[0]["Campaign_Period"].ToString());
                if (mst == "FC") //MediaSubTypeCode
                {
                    ExcelUtil.ExcelSetValueString(workSheet, "A", 3, "FINECAST SCHEDULE");
                }
                else
                {
                    ExcelUtil.ExcelSetValueString(workSheet, "A", 3, "CONNECTED TV SCHEDULE");
                }

                //Modified by Chaiwat.i 25/08/2021 : Correct Agency logo & BARCODE
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
                // Spot Plan
                //=======================================
                dt = m_db.SelectSpotPlanForSchedule_Details(BuyingBriefID, Version, SelectMedia, StartDate, EndDate);
                ExcelUtil.ExcelSetValueStringFullTable(workSheet, 13, 1, 2, dt);
                int row = 20;
                if (dt.Rows.Count > 2)
                {
                    row += dt.Rows.Count - 2;
                }
                ExcelUtil.ExcelSetValueString(workSheet, "H", row, creativeAgency);
                //=======================================
                // Summary Spot Plan
                //=======================================
                row = 34;
                if (dt.Rows.Count > 2)
                {
                    row += dt.Rows.Count - 2;
                }
                ExcelUtil.ExcelSetValueStringFullTable(workSheet, row, 1, 2, ds.Tables[2]);
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
            this.Cursor = Cursors.Default;
        }

        public void PrintMediaSchedule_IT(string Template, string BuyingBriefID, string Version, string StartDate, string EndDate, string SelectMedia, string mt)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                bool bDirectPay = m_db.CheckDirectPaySchedule(m_strBuyingBriefID, m_strVersion, SelectMedia);
                DataSet ds = m_db.SelectSpotPlanForScheduleIT(BuyingBriefID, Version, StartDate, EndDate, SelectMedia);
                //======================================
                // Create Result Excel
                //======================================
                if (bDirectPay)
                    Template = m_strScheduleDirectPayExportPath;
                Excel.Application ExcelObjDesc = new Excel.Application();
                Excel.Workbook theWorkbook = (Excel.Workbook)(ExcelObjDesc.Workbooks.Open(Template));
                Excel.Sheets sheets = theWorkbook.Worksheets;
                Excel.Worksheet workSheet = (Excel.Worksheet)sheets.get_Item(1);//Template
                //=======================================
                // Buying Brief
                //=======================================
                DataTable dt = ds.Tables[0];
                string creativeAgency = dt.Rows[0]["Creative_Agency_Name"].ToString();
                ExcelUtil.ExcelSetValueString(workSheet, "A", 1, dt.Rows[0]["Creative_Agency_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 5, dt.Rows[0]["Client_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 8, dt.Rows[0]["Product_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 9, dt.Rows[0]["Campaign_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 10, dt.Rows[0]["Buying_Brief_ID"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 11, dt.Rows[0]["Campaign_Period"].ToString());

                //Modified by Chaiwat.i 25/08/2021 : Correct Agency logo & BARCODE
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
                    ExcelUtil.ExcelSetValueString(workSheet, 27, 1, "");
                    ExcelUtil.ExcelSetValueString(workSheet, 28, 1, "");
                }

                //=======================================
                // Spot Plan
                //=======================================

                dt = m_db.SelectSpotPlanForScheduleIT_Details(BuyingBriefID, Version, SelectMedia, StartDate, EndDate, "AG");
                ExcelUtil.ExcelSetValueStringFullTable(workSheet, 13, 1, 3, dt);

                //=======================================
                // Summary Spot Plan
                //=======================================
                int row = 37;
                if (dt.Rows.Count > 3)
                {
                    row += dt.Rows.Count - 3;
                }
                ExcelUtil.ExcelSetValueStringFullTable(workSheet, row, 1, 3, ds.Tables[2]);

                //=======================================
                // Client Direct Pay
                //=======================================
                if (bDirectPay)
                {
                    row = 45;
                    if (dt.Rows.Count > 3)
                    {
                        row += dt.Rows.Count - 3;
                    }
                    if (ds.Tables[2].Rows.Count > 3)
                    {
                        row += ds.Tables[2].Rows.Count - 3;
                    }
                    dt = m_db.SelectSpotPlanForScheduleIT_Details(BuyingBriefID, Version, SelectMedia, StartDate, EndDate, "CL");
                    ExcelUtil.ExcelSetValueStringFullTable(workSheet, row, 1, 3, dt);
                }
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
            this.Cursor = Cursors.Default;
        }

        private void Implementation_SpotPlan_Print_Schedule_Load(object sender, EventArgs e)
        {
            dtStartDate.Value = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strStartDate);
            dtEndDate.Value = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strEndDate);
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
                string sd = dtStartDate.Value.ToString("yyyyMMdd");
                string ed = dtEndDate.Value.ToString("yyyyMMdd");
                SetSelectMedia();
                bool mtit = m_db.ValidateMasterMediaTypeIT(MediaTypeCode);
                if (mtit)
                    PrintMediaSchedule_IT(m_strScheduleExportPath, m_strBuyingBriefID, m_strVersion, sd, ed, selectMedia, MediaTypeCode);
                else //FC and Connected TV
                    PrintMediaSchedule(m_strScheduleExportPath, m_strBuyingBriefID, m_strVersion, sd, ed, selectMedia, MediaSubTypeCode);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
