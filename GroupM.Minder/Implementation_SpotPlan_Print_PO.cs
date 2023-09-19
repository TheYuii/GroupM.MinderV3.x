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
    public partial class Implementation_SpotPlan_Print_PO : Form
    {
        private string m_strBuyingBriefID;
        private string m_strVersion;
        private string m_strStartDate;
        private string m_strEndDate;
        private string m_strPOExportPath;
        private string m_strParameterPath = ConfigurationManager.AppSettings["ParameterMinder"];
        private DBManager m_db = null;
        private bool chkGv = false;
        private string statusChk = "";
        private DataTable dtVendor;
        private string MediaTypeCode = "";
        private string MediaSubTypeCode = "";
        private string MasterMediaTypeCode = "";
        public string username;

        public Implementation_SpotPlan_Print_PO(string BB, string version, string startDate, string endDate, string template, string strMt, string strMst, string strMmt)
        {
            InitializeComponent();
            m_db = new DBManager();
            m_strBuyingBriefID = BB;
            m_strVersion = version;
            m_strStartDate = startDate;
            m_strEndDate = endDate;
            m_strPOExportPath = template;
            MediaTypeCode = strMt;
            MediaSubTypeCode = strMst;
            MasterMediaTypeCode = strMmt;
        }

        private int CountSelectVendor()
        {
            int i, count = 0;
            for (i = 0; i < gvDetail.Rows.Count; i++)
            {
                if (Convert.ToBoolean(gvDetail.Rows[i].Cells["SelectVendor"].Value) == true)
                {
                    count++;
                }
            }
            return count;
        }

        private void SetTableVendor()
        {
            DataTable dt = ((DataTable)gvDetail.DataSource).Clone();
            for (int i = 0; i < gvDetail.Rows.Count; i++)
            {
                if (Convert.ToBoolean(gvDetail.Rows[i].Cells["SelectVendor"].Value) == true)
                {
                    DataRow dr = dt.NewRow();
                    dr["Vendor_ID"] = gvDetail.Rows[i].Cells["VendorID"].Value;
                    dr["Vendor_Name"] = gvDetail.Rows[i].Cells["VendorName"].Value;
                    dr["PO_Media"] = gvDetail.Rows[i].Cells["POMedia"].Value;
                    dt.Rows.Add(dr);
                }
            }
            dtVendor = dt;
        }

        private string GeneratePONumber(string BuyingBriefID)
        {
            DataTable dt = m_db.GetPONo(BuyingBriefID);
            if (dt.Rows.Count == 0)
            {
                m_db.InsertPONo();
            }
            else
            {
                m_db.UpdatePONo(BuyingBriefID);
            }
            dt = m_db.GetPONo(BuyingBriefID);
            string strPO = dt.Rows[0]["Order_Date"].ToString() + Convert.ToInt32(dt.Rows[0]["Order_No"]).ToString("D5");
            return strPO;
        }

        public void PrintPO(string Template, string Parameter, string BuyingBriefID, string Version, string StartDate, string EndDate)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                bool noData = true;
                DataSet ds = m_db.SelectSpotPlanForPOHeader(BuyingBriefID, Version);
                //======================================
                // Create Result Excel
                //======================================
                Excel.Application ExcelObjDesc = new Excel.Application();
                Excel.Workbook theWorkbook = (Excel.Workbook)(ExcelObjDesc.Workbooks.Open(Template));
                Excel.Sheets sheets = theWorkbook.Worksheets;
                Excel.Worksheet workSheet = (Excel.Worksheet)sheets.get_Item(1); //Template
                Excel.Range range;
                DataTable dt = ds.Tables[0]; // PO Header
                DataTable dtMaterial = ds.Tables[1]; //Material
                for (int i = 0; i < dtVendor.Rows.Count; i++)
                {
                    string m_vendorCode = dtVendor.Rows[i]["Vendor_ID"].ToString();
                    if (dtVendor.Rows[i]["PO_Media"].ToString() == "True")
                    {
                        DataTable dtMediaCategory = m_db.SelectPOMediaCategory(BuyingBriefID, m_vendorCode);
                        for (int j = 0; j < dtMediaCategory.Rows.Count; j++)
                        {
                            string m_MCCode = dtMediaCategory.Rows[j]["Media_Category_ID"].ToString();
                            string m_mediaCode = dtMediaCategory.Rows[j]["Media_ID"].ToString();
                            workSheet = theWorkbook.Worksheets[1];
                            workSheet.Name = m_vendorCode + " - " + m_MCCode;
                            string m_strPONo = GeneratePONumber(BuyingBriefID);
                            m_db.UpdatePONoInSpotplanByMediaCategory(BuyingBriefID, Version, m_vendorCode, m_mediaCode, m_strPONo);
                            DataTable dtPOID = m_db.SelectSpotPlanForPOIDByMediaCategory(BuyingBriefID, Version, m_vendorCode, m_mediaCode);
                            //=======================================
                            // Material
                            //=======================================
                            if (dtMaterial.Rows.Count > 0)
                            {
                                ExcelUtil.ExcelSetValueString(workSheet, "B", 15, dtMaterial.Rows[0]["Material_Name"].ToString());
                            }
                            //=======================================
                            // Spot Plan (PO) Header session
                            //=======================================
                            ExcelUtil.ExcelSetValueString(workSheet, "B", 9, dt.Rows[0]["Client_Name"].ToString());
                            ExcelUtil.ExcelSetValueString(workSheet, "B", 10, dtVendor.Rows[i]["Vendor_Name"].ToString());
                            ExcelUtil.ExcelSetValueString(workSheet, "B", 11, DateTime.Now.ToString("dd/MM/yyyy")); //Order Date
                            ExcelUtil.ExcelSetValueString(workSheet, "B", 12, dt.Rows[0]["Product_Name"].ToString());
                            ExcelUtil.ExcelSetValueString(workSheet, "B", 13, dt.Rows[0]["Campaign_Name"].ToString());
                            ExcelUtil.ExcelSetValueString(workSheet, "B", 14, dt.Rows[0]["Buying_Brief_ID"].ToString());
                            ExcelUtil.ExcelSetValueString(workSheet, "G", 9, dtPOID.Rows[0]["Booking_Order_ID"].ToString());
                            // create new sheet
                            if (j < dtMediaCategory.Rows.Count - 1)
                            {
                                workSheet.Copy(theWorkbook.Worksheets[1]);
                            }
                            else
                            {
                                if (i < dtVendor.Rows.Count - 1)
                                {
                                    workSheet.Copy(theWorkbook.Worksheets[1]);
                                }
                            }
                            //=======================================
                            // Spot Plan (PO) Details session
                            //=======================================
                            DataTable dtDetails = m_db.SelectSpotPlanForPODetailsByMediaCategory(BuyingBriefID, Version, m_vendorCode, m_MCCode, StartDate, EndDate);
                            if (dtDetails.Rows.Count > 0)
                            {
                                noData = false;
                                ExcelUtil.ExcelSetValueStringFullTable(workSheet, 18, 1, 3, dtDetails);
                            }
                            //=======================================
                            // Spot Plan PO-Footer
                            //=======================================
                            bool printFooter = m_db.CheckPrintPOFooter(Parameter, m_vendorCode);
                            if (printFooter)
                            {
                                int rowFooter = 37;
                                if (dtDetails.Rows.Count > 3)
                                {
                                    rowFooter += dtDetails.Rows.Count - 3;
                                }
                                DataTable dtPOFooter = m_db.SelectPOFooter(Parameter);
                                ExcelUtil.ExcelSetValueString(workSheet, "A", rowFooter, dtPOFooter.Rows[0]["PValue"].ToString());
                                range = workSheet.get_Range($"A{rowFooter}:G{rowFooter}");
                                range.Merge();
                                range.Font.Size = 8;
                                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                                range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                                range.WrapText = true;
                                range.Rows.RowHeight = 120;
                            }
                            //=======================================
                            // Remark
                            //=======================================
                            for (int ex_rows = 1; ex_rows < 4; ex_rows++)
                            {
                                workSheet.Rows[23].Insert(Excel.XlInsertShiftDirection.xlShiftDown, workSheet.Rows[23].Copy(Type.Missing));
                            }
                            workSheet.Rows[23].font.Bold = true;
                            workSheet.Rows[23].Font.Underline = true;
                            workSheet.Rows[23].Font.Size = 10;
                            range = workSheet.get_Range($"A24:G25");
                            range.Merge();
                            range.Font.Size = 10;
                            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                            range.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
                            range.WrapText = true;
                            ExcelUtil.ExcelSetValueString(workSheet, "A", 23, "Remark");
                            ExcelUtil.ExcelSetValueString(workSheet, "A", 24, dt.Rows[0]["Remark"].ToString());
                            //=======================================
                            // Spot Plan (PO) Footer - BARCODE
                            //=======================================
                            workSheet.PageSetup.FirstPage.LeftFooter.Text = "&\"Free 3 of 9 Extended\"&30& *" + BuyingBriefID + "PO" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*\n" +
                            "" + "&\"Arial\"&10&                               *" + BuyingBriefID + "PO" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*";
                            //=======================================
                            // Spot Plan (PO) Header Logo
                            //=======================================
                            workSheet.PageSetup.FirstPage.RightHeader.Picture.Filename = dt.Rows[0][0].ToString();
                            workSheet.PageSetup.FirstPage.RightHeader.Text = "&G";
                            workSheet.PageSetup.RightHeaderPicture.Filename = dt.Rows[0][0].ToString();
                            workSheet.PageSetup.RightHeader = "&G";
                            //=======================================
                            // Spot Plan (PO) Header
                            //=======================================
                            DataTable dtPOHeader = m_db.GetPOHeaderSpecial(BuyingBriefID, m_vendorCode);
                            string strCA = dtPOHeader.Rows[0]["Creative_Agency_ID"].ToString();
                            string strVendorSpecial = dtPOHeader.Rows[0]["Master_Group"].ToString();
                            string strSetPOHeader = "";
                            if (dtPOHeader.Rows.Count > 0)
                            {
                                if ((strCA == "OMTL" || strCA == "OPRWTL" || strCA == "OOWTL" || strCA == "SOHO") && (strVendorSpecial == "MASTERCH7")) //Special for Master Channel 7
                                {
                                    switch (dtPOHeader.Rows[0]["Agency_ID"].ToString())
                                    {
                                        case "GM":
                                        case "GMM6":
                                        case "GMTH":
                                            strSetPOHeader = "GroupM";
                                            break;
                                        case "MC":
                                        case "MCTH":
                                            strSetPOHeader = "MediaCom";
                                            break;
                                        case "ME01":
                                        case "MEC":
                                        case "MEC-OM":
                                            strSetPOHeader = "WAVEMAKER";
                                            break;
                                        default:
                                            strSetPOHeader = "Mindshare";
                                            break;
                                    }
                                    DataTable dtPOHeaderSpecial = m_db.GetPOHeader(dtPOHeader.Rows[0]["Agency_ID"].ToString(), strSetPOHeader);
                                    strSetPOHeader = dtPOHeaderSpecial.Rows[0]["Booking_Order_Header"].ToString();
                                    if (dtPOHeaderSpecial.Rows.Count > 0)
                                    {
                                        workSheet.PageSetup.FirstPage.CenterHeader.Text = strSetPOHeader + "\n" + dtPOHeaderSpecial.Rows[0]["Address"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                        dtPOHeaderSpecial.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address4"].ToString();
                                        workSheet.PageSetup.CenterHeader = strSetPOHeader + "\n" + dtPOHeaderSpecial.Rows[0]["Address"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                        dtPOHeaderSpecial.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address4"].ToString();
                                    }
                                }
                                else
                                {
                                    strSetPOHeader = dtPOHeader.Rows[0]["Booking_Order_Header"].ToString();
                                    workSheet.PageSetup.FirstPage.CenterHeader.Text = strSetPOHeader + "\n" + dtPOHeader.Rows[0]["Address"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                    dtPOHeader.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address4"].ToString();
                                    workSheet.PageSetup.CenterHeader = strSetPOHeader + "\n" + dtPOHeader.Rows[0]["Address"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                    dtPOHeader.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address4"].ToString();
                                }
                            }
                        }
                    }
                    else
                    {
                        workSheet = theWorkbook.Worksheets[1];
                        workSheet.Name = m_vendorCode;
                        string m_strPONo = GeneratePONumber(BuyingBriefID);
                        m_db.UpdatePONoInSpotplanByVendor(BuyingBriefID, Version, m_vendorCode, m_strPONo);
                        DataTable dtPOID = m_db.SelectSpotPlanForPOIDByVendor(BuyingBriefID, Version, m_vendorCode);
                        //=======================================
                        // Material
                        //=======================================
                        if (dtMaterial.Rows.Count > 0)
                        {
                            ExcelUtil.ExcelSetValueString(workSheet, "B", 15, dtMaterial.Rows[0]["Material_Name"].ToString());
                        }
                        //=======================================
                        // Spot Plan (PO) Header session
                        //=======================================
                        ExcelUtil.ExcelSetValueString(workSheet, "B", 9, dt.Rows[0]["Client_Name"].ToString());
                        ExcelUtil.ExcelSetValueString(workSheet, "B", 10, dtVendor.Rows[i]["Vendor_Name"].ToString());
                        ExcelUtil.ExcelSetValueString(workSheet, "B", 11, DateTime.Now.ToString("dd/MM/yyyy")); //Order Date
                        ExcelUtil.ExcelSetValueString(workSheet, "B", 12, dt.Rows[0]["Product_Name"].ToString());
                        ExcelUtil.ExcelSetValueString(workSheet, "B", 13, dt.Rows[0]["Campaign_Name"].ToString());
                        ExcelUtil.ExcelSetValueString(workSheet, "B", 14, dt.Rows[0]["Buying_Brief_ID"].ToString());
                        ExcelUtil.ExcelSetValueString(workSheet, "G", 9, dtPOID.Rows[0]["Booking_Order_ID"].ToString());
                        // create new sheet
                        if (i < dtVendor.Rows.Count - 1)
                        {
                            workSheet.Copy(theWorkbook.Worksheets[1]);
                        }
                        //=======================================
                        // Spot Plan (PO) Details session
                        //=======================================
                        DataTable dtDetails = m_db.SelectSpotPlanForPODetailsByVendor(BuyingBriefID, Version, m_vendorCode, StartDate, EndDate);
                        if (dtDetails.Rows.Count > 0)
                        {
                            noData = false;
                            ExcelUtil.ExcelSetValueStringFullTable(workSheet, 18, 1, 3, dtDetails);
                        }
                        //=======================================
                        // Spot Plan PO-Footer
                        //=======================================
                        bool printFooter = m_db.CheckPrintPOFooter(Parameter, m_vendorCode);
                        if (printFooter)
                        {
                            int rowFooter = 37;
                            if (dtDetails.Rows.Count > 3)
                            {
                                rowFooter += dtDetails.Rows.Count - 3;
                            }
                            DataTable dtPOFooter = m_db.SelectPOFooter(Parameter);
                            ExcelUtil.ExcelSetValueString(workSheet, "A", rowFooter, dtPOFooter.Rows[0]["PValue"].ToString());
                            range = workSheet.get_Range($"A{rowFooter}:G{rowFooter}");
                            range.Merge();
                            range.Font.Size = 8;
                            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                            range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                            range.WrapText = true;
                            range.Rows.RowHeight = 90;
                        }
                        //=======================================
                        // Remark
                        //=======================================
                        for (int ex_rows = 1; ex_rows < 4; ex_rows++)
                        {
                            workSheet.Rows[23].Insert(Excel.XlInsertShiftDirection.xlShiftDown, workSheet.Rows[23].Copy(Type.Missing));
                        }
                        workSheet.Rows[23].font.Bold = true;
                        workSheet.Rows[23].Font.Underline = true;
                        workSheet.Rows[23].Font.Size = 10;
                        range = workSheet.get_Range($"A24:G25");
                        range.Merge();
                        range.Font.Size = 10;
                        range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                        range.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
                        range.WrapText = true;
                        ExcelUtil.ExcelSetValueString(workSheet, "A", 23, "Remark");
                        ExcelUtil.ExcelSetValueString(workSheet, "A", 24, dt.Rows[0]["Remark"].ToString());
                        //=======================================
                        // Spot Plan (PO) Footer - BARCODE
                        //=======================================
                        workSheet.PageSetup.FirstPage.LeftFooter.Text = "&\"Free 3 of 9 Extended\"&30& *" + BuyingBriefID + "PO" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*\n" +
                        "" + "&\"Arial\"&10&                               *" + BuyingBriefID + "PO" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*";
                        //=======================================
                        // Spot Plan (PO) Header Logo
                        //=======================================
                        workSheet.PageSetup.FirstPage.RightHeader.Picture.Filename = dt.Rows[0][0].ToString();
                        workSheet.PageSetup.FirstPage.RightHeader.Text = "&G";
                        workSheet.PageSetup.RightHeaderPicture.Filename = dt.Rows[0][0].ToString();
                        workSheet.PageSetup.RightHeader = "&G";
                        //=======================================
                        // Spot Plan (PO) Header
                        //=======================================
                        DataTable dtPOHeader = m_db.GetPOHeaderSpecial(BuyingBriefID, m_vendorCode);
                        string strCA = dtPOHeader.Rows[0]["Creative_Agency_ID"].ToString();
                        string strVendorSpecial = dtPOHeader.Rows[0]["Master_Group"].ToString();
                        string strSetPOHeader = "";
                        if (dtPOHeader.Rows.Count > 0)
                        {
                            if ((strCA == "OMTL" || strCA == "OPRWTL" || strCA == "OOWTL" || strCA == "SOHO") && (strVendorSpecial == "MASTERCH7")) //Special for Master Channel 7
                            {
                                switch (dtPOHeader.Rows[0]["Agency_ID"].ToString())
                                {
                                    case "GM":
                                    case "GMM6":
                                    case "GMTH":
                                        strSetPOHeader = "GroupM";
                                        break;
                                    case "MC":
                                    case "MCTH":
                                        strSetPOHeader = "MediaCom";
                                        break;
                                    case "ME01":
                                    case "MEC":
                                    case "MEC-OM":
                                        strSetPOHeader = "WAVEMAKER";
                                        break;
                                    default:
                                        strSetPOHeader = "Mindshare";
                                        break;
                                }
                                DataTable dtPOHeaderSpecial = m_db.GetPOHeader(dtPOHeader.Rows[0]["Agency_ID"].ToString(), strSetPOHeader);
                                strSetPOHeader = dtPOHeaderSpecial.Rows[0]["Booking_Order_Header"].ToString();
                                if (dtPOHeaderSpecial.Rows.Count > 0)
                                {
                                    workSheet.PageSetup.FirstPage.CenterHeader.Text = strSetPOHeader + "\n" + dtPOHeaderSpecial.Rows[0]["Address"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                    dtPOHeaderSpecial.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address4"].ToString();
                                    workSheet.PageSetup.CenterHeader = strSetPOHeader + "\n" + dtPOHeaderSpecial.Rows[0]["Address"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                    dtPOHeaderSpecial.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address4"].ToString();
                                }
                            }
                            else
                            {
                                strSetPOHeader = dtPOHeader.Rows[0]["Booking_Order_Header"].ToString();
                                workSheet.PageSetup.FirstPage.CenterHeader.Text = strSetPOHeader + "\n" + dtPOHeader.Rows[0]["Address"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                dtPOHeader.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address4"].ToString();
                                workSheet.PageSetup.CenterHeader = strSetPOHeader + "\n" + dtPOHeader.Rows[0]["Address"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                dtPOHeader.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address4"].ToString();
                            }
                        }
                    }
                }
                if (noData)
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

        public void PrintPO_IT(string Template, string Parameter, string BuyingBriefID, string Version, string StartDate, string EndDate)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                bool noData = true;
                bool deleteSheet = true;
                DataSet ds = m_db.SelectSpotPlanForPOHeader(BuyingBriefID, Version);
                //======================================
                // Create Result Excel
                //======================================
                Excel.Application ExcelObjDesc = new Excel.Application();
                Excel.Workbook theWorkbook = (Excel.Workbook)(ExcelObjDesc.Workbooks.Open(Template));
                Excel.Sheets sheets = theWorkbook.Worksheets;
                Excel.Worksheet workSheet = (Excel.Worksheet)sheets.get_Item(1); //Template
                Excel.Range range;
                DataTable dt = ds.Tables[0]; // PO Header
                DataTable dtMaterial = ds.Tables[1]; //Material
                for (int i = 0; i < dtVendor.Rows.Count; i++)
                {
                    string m_vendorCode = dtVendor.Rows[i]["Vendor_ID"].ToString();
                    if (dtVendor.Rows[i]["PO_Media"].ToString() == "True")
                    {
                        DataTable dtMediaCategory = m_db.SelectPOITMediaCategory(BuyingBriefID, m_vendorCode);
                        for (int j = 0; j < dtMediaCategory.Rows.Count; j++)
                        {
                            string m_MCCode = dtMediaCategory.Rows[j]["Media_Category_ID"].ToString();
                            DataTable dtDetails = m_db.SelectSpotPlanForPOITDetailsByMediaCategory(BuyingBriefID, Version, m_vendorCode, m_MCCode, StartDate, EndDate);
                            if (dtDetails.Rows.Count == 0)
                            {
                                deleteSheet = true;
                            }
                            else
                            {
                                noData = false;
                                deleteSheet = false;
                                workSheet = theWorkbook.Worksheets[1];
                                string sheetName = m_vendorCode + " - " + (m_MCCode == "" ? "NonCate" : m_MCCode);
                                workSheet.Name = sheetName;
                                string m_strPONo = GeneratePONumber(BuyingBriefID);
                                m_db.UpdatePONoInSpotplanByVendor(BuyingBriefID, Version, m_vendorCode, m_strPONo);
                                DataTable dtPOID = m_db.SelectSpotPlanForPOIDByVendor(BuyingBriefID, Version, m_vendorCode);
                                //=======================================
                                // Material
                                //=======================================
                                if (dtMaterial.Rows.Count > 0)
                                {
                                    ExcelUtil.ExcelSetValueString(workSheet, "B", 15, dtMaterial.Rows[0]["Material_Name"].ToString());
                                }
                                //=======================================
                                // Spot Plan (PO) Header session
                                //=======================================
                                ExcelUtil.ExcelSetValueString(workSheet, "B", 9, dt.Rows[0]["Client_Name"].ToString());
                                ExcelUtil.ExcelSetValueString(workSheet, "B", 10, dtVendor.Rows[i]["Vendor_Name"].ToString());
                                ExcelUtil.ExcelSetValueString(workSheet, "B", 11, DateTime.Now.ToString("dd/MM/yyyy")); //Order Date
                                ExcelUtil.ExcelSetValueString(workSheet, "B", 12, dt.Rows[0]["Product_Name"].ToString());
                                ExcelUtil.ExcelSetValueString(workSheet, "B", 13, dt.Rows[0]["Campaign_Name"].ToString());
                                ExcelUtil.ExcelSetValueString(workSheet, "B", 14, dt.Rows[0]["Buying_Brief_ID"].ToString());
                                ExcelUtil.ExcelSetValueString(workSheet, "G", 9, dtPOID.Rows[0]["Booking_Order_ID"].ToString());
                                // create new sheet
                                if (j < dtMediaCategory.Rows.Count - 1)
                                {
                                    workSheet.Copy(theWorkbook.Worksheets[1]);
                                }
                                else
                                {
                                    if (i < dtVendor.Rows.Count - 1)
                                    {
                                        workSheet.Copy(theWorkbook.Worksheets[1]);
                                    }
                                }
                                //=======================================
                                // Remark
                                //=======================================
                                ExcelUtil.ExcelSetValueString(workSheet, "A", 24, dt.Rows[0]["Remark"].ToString());
                                //=======================================
                                // Spot Plan (PO) Details session
                                //=======================================
                                if (dtDetails.Rows.Count > 2)
                                {
                                    workSheet.Rows[19].Insert(Excel.XlInsertShiftDirection.xlShiftDown, workSheet.Rows[19].Copy(Type.Missing));
                                }
                                ExcelUtil.ExcelSetValueStringFullTable(workSheet, 18, 1, 3, dtDetails);
                                //=======================================
                                // Spot Plan PO-Footer
                                //=======================================
                                bool printFooter = m_db.CheckPrintPOFooter(Parameter, m_vendorCode);
                                if (printFooter)
                                {
                                    int rowFooter = 37;
                                    if (dtDetails.Rows.Count > 3)
                                    {
                                        rowFooter += dtDetails.Rows.Count - 3;
                                    }
                                    DataTable dtPOFooter = m_db.SelectPOFooter(Parameter);
                                    ExcelUtil.ExcelSetValueString(workSheet, "A", rowFooter, dtPOFooter.Rows[0]["PValue"].ToString());
                                    range = workSheet.get_Range($"A{rowFooter}:G{rowFooter}");
                                    range.Merge();
                                    range.Font.Size = 8;
                                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                                    range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                                    range.WrapText = true;
                                    range.Rows.RowHeight = 120;
                                }
                                //=======================================
                                // Spot Plan (PO) Footer - BARCODE
                                //=======================================
                                workSheet.PageSetup.FirstPage.LeftFooter.Text = "&\"Free 3 of 9 Extended\"&30& *" + BuyingBriefID + "PO" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*\n" +
                                "" + "&\"Arial\"&10&                               *" + BuyingBriefID + "PO" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*";
                                //=======================================
                                // Spot Plan (PO) Header Logo
                                //=======================================
                                workSheet.PageSetup.FirstPage.RightHeader.Picture.Filename = dt.Rows[0][0].ToString();
                                workSheet.PageSetup.FirstPage.RightHeader.Text = "&G";
                                workSheet.PageSetup.RightHeaderPicture.Filename = dt.Rows[0][0].ToString();
                                workSheet.PageSetup.RightHeader = "&G";
                                //=======================================
                                // Spot Plan (PO) Header
                                //=======================================
                                DataTable dtPOHeader = m_db.GetPOHeaderSpecial(BuyingBriefID, m_vendorCode);
                                string strCA = dtPOHeader.Rows[0]["Creative_Agency_ID"].ToString();
                                string strVendorSpecial = dtPOHeader.Rows[0]["Master_Group"].ToString();
                                string strSetPOHeader = "";
                                if (dtPOHeader.Rows.Count > 0)
                                {
                                    if ((strCA == "OMTL" || strCA == "OPRWTL" || strCA == "OOWTL" || strCA == "SOHO") && (strVendorSpecial == "MASTERCH7")) //Special for Master Channel 7
                                    {
                                        switch (dtPOHeader.Rows[0]["Agency_ID"].ToString())
                                        {
                                            case "GM":
                                            case "GMM6":
                                            case "GMTH":
                                                strSetPOHeader = "GroupM";
                                                break;
                                            case "MC":
                                            case "MCTH":
                                                strSetPOHeader = "MediaCom";
                                                break;
                                            case "ME01":
                                            case "MEC":
                                            case "MEC-OM":
                                                strSetPOHeader = "WAVEMAKER";
                                                break;
                                            default:
                                                strSetPOHeader = "Mindshare";
                                                break;
                                        }
                                        DataTable dtPOHeaderSpecial = m_db.GetPOHeader(dtPOHeader.Rows[0]["Agency_ID"].ToString(), strSetPOHeader);
                                        strSetPOHeader = dtPOHeaderSpecial.Rows[0]["Booking_Order_Header"].ToString();
                                        if (dtPOHeaderSpecial.Rows.Count > 0)
                                        {
                                            workSheet.PageSetup.FirstPage.CenterHeader.Text = strSetPOHeader + "\n" + dtPOHeaderSpecial.Rows[0]["Address"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                            dtPOHeaderSpecial.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address4"].ToString();
                                            workSheet.PageSetup.CenterHeader = strSetPOHeader + "\n" + dtPOHeaderSpecial.Rows[0]["Address"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                            dtPOHeaderSpecial.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address4"].ToString();
                                        }
                                    }
                                    else
                                    {
                                        strSetPOHeader = dtPOHeader.Rows[0]["Booking_Order_Header"].ToString();
                                        workSheet.PageSetup.FirstPage.CenterHeader.Text = strSetPOHeader + "\n" + dtPOHeader.Rows[0]["Address"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                        dtPOHeader.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address4"].ToString();
                                        workSheet.PageSetup.CenterHeader = strSetPOHeader + "\n" + dtPOHeader.Rows[0]["Address"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                        dtPOHeader.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address4"].ToString();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        DataTable dtDetails = m_db.SelectSpotPlanForPOITDetailsByVendor(BuyingBriefID, Version, m_vendorCode, StartDate, EndDate);
                        if (dtDetails.Rows.Count == 0)
                        {
                            deleteSheet = true;
                        }
                        else
                        {
                            noData = false;
                            deleteSheet = false;
                            workSheet = theWorkbook.Worksheets[1];
                            workSheet.Name = m_vendorCode;
                            string m_strPONo = GeneratePONumber(BuyingBriefID);
                            m_db.UpdatePONoInSpotplanByVendor(BuyingBriefID, Version, m_vendorCode, m_strPONo);
                            DataTable dtPOID = m_db.SelectSpotPlanForPOIDByVendor(BuyingBriefID, Version, m_vendorCode);
                            //=======================================
                            // Material
                            //=======================================
                            if (dtMaterial.Rows.Count > 0)
                            {
                                ExcelUtil.ExcelSetValueString(workSheet, "B", 15, dtMaterial.Rows[0]["Material_Name"].ToString());
                            }
                            //=======================================
                            // Spot Plan (PO) Header session
                            //=======================================
                            ExcelUtil.ExcelSetValueString(workSheet, "B", 9, dt.Rows[0]["Client_Name"].ToString());
                            ExcelUtil.ExcelSetValueString(workSheet, "B", 10, dtVendor.Rows[i]["Vendor_Name"].ToString());
                            ExcelUtil.ExcelSetValueString(workSheet, "B", 11, DateTime.Now.ToString("dd/MM/yyyy")); //Order Date
                            ExcelUtil.ExcelSetValueString(workSheet, "B", 12, dt.Rows[0]["Product_Name"].ToString());
                            ExcelUtil.ExcelSetValueString(workSheet, "B", 13, dt.Rows[0]["Campaign_Name"].ToString());
                            ExcelUtil.ExcelSetValueString(workSheet, "B", 14, dt.Rows[0]["Buying_Brief_ID"].ToString());
                            ExcelUtil.ExcelSetValueString(workSheet, "G", 9, dtPOID.Rows[0]["Booking_Order_ID"].ToString());
                            // create new sheet
                            if (i < dtVendor.Rows.Count - 1)
                            {
                                workSheet.Copy(theWorkbook.Worksheets[1]);
                            }
                            //=======================================
                            // Remark
                            //=======================================
                            ExcelUtil.ExcelSetValueString(workSheet, "A", 24, dt.Rows[0]["Remark"].ToString());
                            //=======================================
                            // Spot Plan (PO) Details session
                            //=======================================
                            if (dtDetails.Rows.Count > 2)
                            {
                                workSheet.Rows[19].Insert(Excel.XlInsertShiftDirection.xlShiftDown, workSheet.Rows[19].Copy(Type.Missing));
                            }
                            ExcelUtil.ExcelSetValueStringFullTable(workSheet, 18, 1, 3, dtDetails);
                            //=======================================
                            // Spot Plan PO-Footer
                            //=======================================
                            bool printFooter = m_db.CheckPrintPOFooter(Parameter, m_vendorCode);
                            if (printFooter)
                            {
                                int rowFooter = 37;
                                if (dtDetails.Rows.Count > 3)
                                {
                                    rowFooter += dtDetails.Rows.Count - 3;
                                }
                                DataTable dtPOFooter = m_db.SelectPOFooter(Parameter);
                                ExcelUtil.ExcelSetValueString(workSheet, "A", rowFooter, dtPOFooter.Rows[0]["PValue"].ToString());
                                range = workSheet.get_Range($"A{rowFooter}:G{rowFooter}");
                                range.Merge();
                                range.Font.Size = 8;
                                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                                range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                                range.WrapText = true;
                                range.Rows.RowHeight = 90;
                            }
                            //=======================================
                            // Spot Plan (PO) Footer - BARCODE
                            //=======================================
                            workSheet.PageSetup.FirstPage.LeftFooter.Text = "&\"Free 3 of 9 Extended\"&30& *" + BuyingBriefID + "PO" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*\n" +
                            "" + "&\"Arial\"&10&                               *" + BuyingBriefID + "PO" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*";
                            //=======================================
                            // Spot Plan (PO) Header Logo
                            //=======================================
                            workSheet.PageSetup.FirstPage.RightHeader.Picture.Filename = dt.Rows[0][0].ToString();
                            workSheet.PageSetup.FirstPage.RightHeader.Text = "&G";
                            workSheet.PageSetup.RightHeaderPicture.Filename = dt.Rows[0][0].ToString();
                            workSheet.PageSetup.RightHeader = "&G";
                            //=======================================
                            // Spot Plan (PO) Header
                            //=======================================
                            DataTable dtPOHeader = m_db.GetPOHeaderSpecial(BuyingBriefID, m_vendorCode);
                            string strCA = dtPOHeader.Rows[0]["Creative_Agency_ID"].ToString();
                            string strVendorSpecial = dtPOHeader.Rows[0]["Master_Group"].ToString();
                            string strSetPOHeader = "";
                            if (dtPOHeader.Rows.Count > 0)
                            {
                                if ((strCA == "OMTL" || strCA == "OPRWTL" || strCA == "OOWTL" || strCA == "SOHO") && (strVendorSpecial == "MASTERCH7")) //Special for Master Channel 7
                                {
                                    switch (dtPOHeader.Rows[0]["Agency_ID"].ToString())
                                    {
                                        case "GM":
                                        case "GMM6":
                                        case "GMTH":
                                            strSetPOHeader = "GroupM";
                                            break;
                                        case "MC":
                                        case "MCTH":
                                            strSetPOHeader = "MediaCom";
                                            break;
                                        case "ME01":
                                        case "MEC":
                                        case "MEC-OM":
                                            strSetPOHeader = "WAVEMAKER";
                                            break;
                                        default:
                                            strSetPOHeader = "Mindshare";
                                            break;
                                    }
                                    DataTable dtPOHeaderSpecial = m_db.GetPOHeader(dtPOHeader.Rows[0]["Agency_ID"].ToString(), strSetPOHeader);
                                    strSetPOHeader = dtPOHeaderSpecial.Rows[0]["Booking_Order_Header"].ToString();
                                    if (dtPOHeaderSpecial.Rows.Count > 0)
                                    {
                                        workSheet.PageSetup.FirstPage.CenterHeader.Text = strSetPOHeader + "\n" + dtPOHeaderSpecial.Rows[0]["Address"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                        dtPOHeaderSpecial.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address4"].ToString();
                                        workSheet.PageSetup.CenterHeader = strSetPOHeader + "\n" + dtPOHeaderSpecial.Rows[0]["Address"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                        dtPOHeaderSpecial.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address4"].ToString();
                                    }
                                }
                                else
                                {
                                    strSetPOHeader = dtPOHeader.Rows[0]["Booking_Order_Header"].ToString();
                                    workSheet.PageSetup.FirstPage.CenterHeader.Text = strSetPOHeader + "\n" + dtPOHeader.Rows[0]["Address"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                    dtPOHeader.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address4"].ToString();
                                    workSheet.PageSetup.CenterHeader = strSetPOHeader + "\n" + dtPOHeader.Rows[0]["Address"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                    dtPOHeader.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address4"].ToString();
                                }
                            }
                        }
                    }
                }
                if (deleteSheet)
                {
                    if (theWorkbook.Worksheets.Count > 1)
                    {
                        ExcelObjDesc.DisplayAlerts = false;
                        workSheet = theWorkbook.Worksheets[1];
                        workSheet.Delete();
                        ExcelObjDesc.DisplayAlerts = true;
                    }
                }
                if (noData)
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
        public void PrintPO_OD(string Template, string Parameter, string BuyingBriefID, string Version, string StartDate, string EndDate)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                bool noData = true;
                bool deleteSheet = true;
                DataSet ds = m_db.SelectSpotPlanForPOHeader(BuyingBriefID, Version);
                //======================================
                // Create Result Excel
                //======================================
                Excel.Application ExcelObjDesc = new Excel.Application();
                Excel.Workbook theWorkbook = (Excel.Workbook)(ExcelObjDesc.Workbooks.Open(Template));
                Excel.Sheets sheets = theWorkbook.Worksheets;
                Excel.Worksheet workSheet = (Excel.Worksheet)sheets.get_Item(1); //Template
                Excel.Range range;
                DataTable dt = ds.Tables[0]; // PO Header
                DataTable dtMaterial = ds.Tables[1]; //Material
                string strOrderNo = ds.Tables[2].Rows[0]["OrderId"].ToString();//OrderId
                for (int i = 0; i < dtVendor.Rows.Count; i++)
                {
                    string m_vendorCode = dtVendor.Rows[i]["Vendor_ID"].ToString();
                    if (dtVendor.Rows[i]["PO_Media"].ToString() == "True")
                    {
                        DataTable dtMediaCategory = m_db.SelectPOITMediaCategory(BuyingBriefID, m_vendorCode);
                        for (int j = 0; j < dtMediaCategory.Rows.Count; j++)
                        {
                            string m_MCCode = dtMediaCategory.Rows[j]["Media_Category_ID"].ToString();
                            DataTable dtDetails = m_db.SelectSpotPlanForPOITDetailsByMediaCategory(BuyingBriefID, Version, m_vendorCode, m_MCCode, StartDate, EndDate);
                            if (dtDetails.Rows.Count == 0)
                            {
                                deleteSheet = true;
                            }
                            else
                            {
                                noData = false;
                                deleteSheet = false;
                                workSheet = theWorkbook.Worksheets[1];
                                string sheetName = m_vendorCode + " - " + (m_MCCode == "" ? "NonCate" : m_MCCode);
                                workSheet.Name = sheetName;
                                string m_strPONo = GeneratePONumber(BuyingBriefID);
                                m_db.UpdatePONoInSpotplanByVendor(BuyingBriefID, Version, m_vendorCode, m_strPONo);
                                DataTable dtPOID = m_db.SelectSpotPlanForPOIDByVendor(BuyingBriefID, Version, m_vendorCode);
                                //=======================================
                                // Material
                                //=======================================
                                if (dtMaterial.Rows.Count > 0)
                                {
                                    ExcelUtil.ExcelSetValueString(workSheet, "B", 15, dtMaterial.Rows[0]["Material_Name"].ToString());
                                }
                                //=======================================
                                // Spot Plan (PO) Header session
                                //=======================================
                                ExcelUtil.ExcelSetValueString(workSheet, "B", 4, dt.Rows[0]["Buying_Brief_ID"].ToString());
                                ExcelUtil.ExcelSetValueString(workSheet, "B", 5, dtVendor.Rows[i]["Vendor_Name"].ToString());
                                ExcelUtil.ExcelSetValueString(workSheet, "B", 7, dt.Rows[0]["Product_Name"].ToString()); //Order Date
                                ExcelUtil.ExcelSetValueString(workSheet, "B", 8, dt.Rows[0]["Campaign_Name"].ToString());
                                ExcelUtil.ExcelSetValueString(workSheet, "B", 9, dt.Rows[0]["Campaign_Period"].ToString());
                                ExcelUtil.ExcelSetValueString(workSheet, "I", 4, strOrderNo);
                                // create new sheet
                                if (j < dtMediaCategory.Rows.Count - 1)
                                {
                                    workSheet.Copy(theWorkbook.Worksheets[1]);
                                }
                                else
                                {
                                    if (i < dtVendor.Rows.Count - 1)
                                    {
                                        workSheet.Copy(theWorkbook.Worksheets[1]);
                                    }
                                }
                                //=======================================
                                // Remark
                                //=======================================
                                ExcelUtil.ExcelSetValueString(workSheet, "A", 24, dt.Rows[0]["Remark"].ToString());
                                //=======================================
                                // Spot Plan (PO) Details session
                                //=======================================
                                if (dtDetails.Rows.Count > 2)
                                {
                                    workSheet.Rows[13].Insert(Excel.XlInsertShiftDirection.xlShiftDown, workSheet.Rows[13].Copy(Type.Missing));
                                }
                                ExcelUtil.ExcelSetValueStringFullTable(workSheet, 18, 1, 3, dtDetails);
                                //=======================================
                                // Spot Plan PO-Footer
                                //=======================================
                                bool printFooter = m_db.CheckPrintPOFooter(Parameter, m_vendorCode);
                                if (printFooter)
                                {
                                    int rowFooter = 37;
                                    if (dtDetails.Rows.Count > 3)
                                    {
                                        rowFooter += dtDetails.Rows.Count - 3;
                                    }
                                    DataTable dtPOFooter = m_db.SelectPOFooter(Parameter);
                                    ExcelUtil.ExcelSetValueString(workSheet, "A", rowFooter, dtPOFooter.Rows[0]["PValue"].ToString());
                                    range = workSheet.get_Range($"A{rowFooter}:G{rowFooter}");
                                    range.Merge();
                                    range.Font.Size = 8;
                                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                                    range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                                    range.WrapText = true;
                                    range.Rows.RowHeight = 120;
                                }
                                //=======================================
                                // Spot Plan (PO) Footer - BARCODE
                                //=======================================
                                workSheet.PageSetup.FirstPage.LeftFooter.Text = "&\"Free 3 of 9 Extended\"&30& *" + BuyingBriefID + "PO" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*\n" +
                                "" + "&\"Arial\"&10&                               *" + BuyingBriefID + "PO" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*";
                                //=======================================
                                // Spot Plan (PO) Header Logo
                                //=======================================
                                workSheet.PageSetup.FirstPage.RightHeader.Picture.Filename = dt.Rows[0][0].ToString();
                                workSheet.PageSetup.FirstPage.RightHeader.Text = "&G";
                                workSheet.PageSetup.RightHeaderPicture.Filename = dt.Rows[0][0].ToString();
                                workSheet.PageSetup.RightHeader = "&G";
                                //=======================================
                                // Spot Plan (PO) Header
                                //=======================================
                                DataTable dtPOHeader = m_db.GetPOHeaderSpecial(BuyingBriefID, m_vendorCode);
                                string strCA = dtPOHeader.Rows[0]["Creative_Agency_ID"].ToString();
                                string strVendorSpecial = dtPOHeader.Rows[0]["Master_Group"].ToString();
                                string strSetPOHeader = "";
                                if (dtPOHeader.Rows.Count > 0)
                                {
                                    if ((strCA == "OMTL" || strCA == "OPRWTL" || strCA == "OOWTL" || strCA == "SOHO") && (strVendorSpecial == "MASTERCH7")) //Special for Master Channel 7
                                    {
                                        switch (dtPOHeader.Rows[0]["Agency_ID"].ToString())
                                        {
                                            case "GM":
                                            case "GMM6":
                                            case "GMTH":
                                                strSetPOHeader = "GroupM";
                                                break;
                                            case "MC":
                                            case "MCTH":
                                                strSetPOHeader = "MediaCom";
                                                break;
                                            case "ME01":
                                            case "MEC":
                                            case "MEC-OM":
                                                strSetPOHeader = "WAVEMAKER";
                                                break;
                                            default:
                                                strSetPOHeader = "Mindshare";
                                                break;
                                        }
                                        DataTable dtPOHeaderSpecial = m_db.GetPOHeader(dtPOHeader.Rows[0]["Agency_ID"].ToString(), strSetPOHeader);
                                        strSetPOHeader = dtPOHeaderSpecial.Rows[0]["Booking_Order_Header"].ToString();
                                        if (dtPOHeaderSpecial.Rows.Count > 0)
                                        {
                                            workSheet.PageSetup.FirstPage.CenterHeader.Text = strSetPOHeader + "\n" + dtPOHeaderSpecial.Rows[0]["Address"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                            dtPOHeaderSpecial.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address4"].ToString();
                                            workSheet.PageSetup.CenterHeader = strSetPOHeader + "\n" + dtPOHeaderSpecial.Rows[0]["Address"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                            dtPOHeaderSpecial.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address4"].ToString();
                                        }
                                    }
                                    else
                                    {
                                        strSetPOHeader = dtPOHeader.Rows[0]["Booking_Order_Header"].ToString();
                                        workSheet.PageSetup.FirstPage.CenterHeader.Text = strSetPOHeader + "\n" + dtPOHeader.Rows[0]["Address"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                        dtPOHeader.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address4"].ToString();
                                        workSheet.PageSetup.CenterHeader = strSetPOHeader + "\n" + dtPOHeader.Rows[0]["Address"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                        dtPOHeader.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address4"].ToString();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        DataTable dtDetails = m_db.SelectSpotPlanForPOODDetailsByVendor(BuyingBriefID, Version, m_vendorCode, StartDate, EndDate);
                        if (dtDetails.Rows.Count == 0)
                        {
                            deleteSheet = true;
                        }
                        else
                        {
                            noData = false;
                            deleteSheet = false;
                            workSheet = theWorkbook.Worksheets[1];
                            workSheet.Name = m_vendorCode;
                            string m_strPONo = GeneratePONumber(BuyingBriefID);
                            m_db.UpdatePONoInSpotplanByVendor(BuyingBriefID, Version, m_vendorCode, m_strPONo);
                            DataTable dtPOID = m_db.SelectSpotPlanForPOIDByVendor(BuyingBriefID, Version, m_vendorCode);
                            //=======================================
                            // Material
                            //=======================================
                            if (dtMaterial.Rows.Count > 0)
                            {
                                ExcelUtil.ExcelSetValueString(workSheet, "B", 15, dtMaterial.Rows[0]["Material_Name"].ToString());
                            }
                            //=======================================
                            // Spot Plan (PO) Header session
                            //=======================================
                            ExcelUtil.ExcelSetValueString(workSheet, "B", 4, dt.Rows[0]["Buying_Brief_ID"].ToString());
                            ExcelUtil.ExcelSetValueString(workSheet, "B", 5, dtVendor.Rows[i]["Vendor_Name"].ToString());
                            ExcelUtil.ExcelSetValueString(workSheet, "B", 6, dt.Rows[0]["Client_Name"].ToString()); //Order Date
                            ExcelUtil.ExcelSetValueString(workSheet, "B", 7, dt.Rows[0]["Product_Name"].ToString()); //Order Date
                            ExcelUtil.ExcelSetValueString(workSheet, "B", 8, dt.Rows[0]["Campaign_Name"].ToString());
                            ExcelUtil.ExcelSetValueString(workSheet, "B", 9, dt.Rows[0]["Campaign_Period"].ToString());
                            ExcelUtil.ExcelSetValueString(workSheet, "I", 4, strOrderNo);
                            // create new sheet
                            if (i < dtVendor.Rows.Count - 1)
                            {
                                workSheet.Copy(theWorkbook.Worksheets[1]);
                            }
                            //=======================================
                            // Remark
                            //=======================================
                            ExcelUtil.ExcelSetValueString(workSheet, "A", 24, dt.Rows[0]["Remark"].ToString());
                            //=======================================
                            // Spot Plan (PO) Details session
                            //=======================================
                            if (dtDetails.Rows.Count > 2)
                            {
                                workSheet.Rows[19].Insert(Excel.XlInsertShiftDirection.xlShiftDown, workSheet.Rows[19].Copy(Type.Missing));
                            }
                            ExcelUtil.ExcelSetValueStringFullTable(workSheet, 12, 1, 3, dtDetails);
                            //=======================================
                            // Spot Plan PO-Footer
                            //=======================================
                            bool printFooter = m_db.CheckPrintPOFooter(Parameter, m_vendorCode);
                            if (printFooter)
                            {
                                int rowFooter = 26;
                                if (dtDetails.Rows.Count > 3)
                                {
                                    rowFooter += dtDetails.Rows.Count - 3;
                                }
                                DataTable dtPOFooter = m_db.SelectPOFooter(Parameter);
                                ExcelUtil.ExcelSetValueString(workSheet, "A", rowFooter, dtPOFooter.Rows[0]["PValue"].ToString());
                                range = workSheet.get_Range($"A{rowFooter}:G{rowFooter}");
                                range.Merge();
                                range.Font.Size = 8;
                                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                                range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                                range.WrapText = true;
                                range.Rows.RowHeight = 150;
                            }
                            //=======================================
                            // Spot Plan (PO) Footer - BARCODE
                            //=======================================
                            workSheet.PageSetup.FirstPage.LeftFooter.Text = "&\"Free 3 of 9 Extended\"&30& *" + BuyingBriefID + "PO" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*\n" +
                            "" + "&\"Arial\"&10&                               *" + BuyingBriefID + "PO" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*";
                            //=======================================
                            // Spot Plan (PO) Header Logo
                            //=======================================
                            workSheet.PageSetup.FirstPage.RightHeader.Picture.Filename = dt.Rows[0][0].ToString();
                            workSheet.PageSetup.FirstPage.RightHeader.Text = "&G";
                            workSheet.PageSetup.RightHeaderPicture.Filename = dt.Rows[0][0].ToString();
                            workSheet.PageSetup.RightHeader = "&G";
                            //=======================================
                            // Spot Plan (PO) Header
                            //=======================================
                            DataTable dtPOHeader = m_db.GetPOHeaderSpecial(BuyingBriefID, m_vendorCode);
                            string strCA = dtPOHeader.Rows[0]["Creative_Agency_ID"].ToString();
                            string strVendorSpecial = dtPOHeader.Rows[0]["Master_Group"].ToString();
                            string strSetPOHeader = "";
                            if (dtPOHeader.Rows.Count > 0)
                            {
                                if ((strCA == "OMTL" || strCA == "OPRWTL" || strCA == "OOWTL" || strCA == "SOHO") && (strVendorSpecial == "MASTERCH7")) //Special for Master Channel 7
                                {
                                    switch (dtPOHeader.Rows[0]["Agency_ID"].ToString())
                                    {
                                        case "GM":
                                        case "GMM6":
                                        case "GMTH":
                                            strSetPOHeader = "GroupM";
                                            break;
                                        case "MC":
                                        case "MCTH":
                                            strSetPOHeader = "MediaCom";
                                            break;
                                        case "ME01":
                                        case "MEC":
                                        case "MEC-OM":
                                            strSetPOHeader = "WAVEMAKER";
                                            break;
                                        default:
                                            strSetPOHeader = "Mindshare";
                                            break;
                                    }
                                    DataTable dtPOHeaderSpecial = m_db.GetPOHeader(dtPOHeader.Rows[0]["Agency_ID"].ToString(), strSetPOHeader);
                                    strSetPOHeader = dtPOHeaderSpecial.Rows[0]["Booking_Order_Header"].ToString();
                                    if (dtPOHeaderSpecial.Rows.Count > 0)
                                    {
                                        workSheet.PageSetup.FirstPage.CenterHeader.Text = strSetPOHeader + "\n" + dtPOHeaderSpecial.Rows[0]["Address"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                        dtPOHeaderSpecial.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address4"].ToString();
                                        workSheet.PageSetup.CenterHeader = strSetPOHeader + "\n" + dtPOHeaderSpecial.Rows[0]["Address"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                        dtPOHeaderSpecial.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address4"].ToString();
                                    }
                                }
                                else
                                {
                                    strSetPOHeader = dtPOHeader.Rows[0]["Booking_Order_Header"].ToString();
                                    workSheet.PageSetup.FirstPage.CenterHeader.Text = strSetPOHeader + "\n" + dtPOHeader.Rows[0]["Address"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                    dtPOHeader.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address4"].ToString();
                                    workSheet.PageSetup.CenterHeader = strSetPOHeader + "\n" + dtPOHeader.Rows[0]["Address"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                    dtPOHeader.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address4"].ToString();
                                }
                            }
                        }
                    }
                }
                if (deleteSheet)
                {
                    if (theWorkbook.Worksheets.Count > 1)
                    {
                        ExcelObjDesc.DisplayAlerts = false;
                        workSheet = theWorkbook.Worksheets[1];
                        workSheet.Delete();
                        ExcelObjDesc.DisplayAlerts = true;
                    }
                }
                if (noData)
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

        public void PrintPO_ES(string Template, string Parameter, string BuyingBriefID, string Version, string StartDate, string EndDate)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                bool noData = true;
                bool deleteSheet = true;
                DataSet ds = m_db.SelectSpotPlanForPOHeader(BuyingBriefID, Version);
                //======================================
                // Create Result Excel
                //======================================
                Excel.Application ExcelObjDesc = new Excel.Application();
                Excel.Workbook theWorkbook = (Excel.Workbook)(ExcelObjDesc.Workbooks.Open(Template));
                Excel.Sheets sheets = theWorkbook.Worksheets;
                Excel.Worksheet workSheet = (Excel.Worksheet)sheets.get_Item(1); //Template
                Excel.Range range;
                DataTable dt = ds.Tables[0]; // PO Header
                DataTable dtMaterial = ds.Tables[1]; //Material
                for (int i = 0; i < dtVendor.Rows.Count; i++)
                {
                    string m_vendorCode = dtVendor.Rows[i]["Vendor_ID"].ToString();
                    if (dtVendor.Rows[i]["PO_Media"].ToString() == "True")
                    {
                        DataTable dtMediaCategory = m_db.SelectPOITMediaCategory(BuyingBriefID, m_vendorCode);
                        for (int j = 0; j < dtMediaCategory.Rows.Count; j++)
                        {
                            string m_MCCode = dtMediaCategory.Rows[j]["Media_Category_ID"].ToString();
                            DataTable dtDetails = m_db.SelectSpotPlanForPOESDetailsByMediaCategory(BuyingBriefID, Version, m_vendorCode, m_MCCode, StartDate, EndDate);
                            if (dtDetails.Rows.Count == 0)
                            {
                                deleteSheet = true;
                            }
                            else
                            {
                                noData = false;
                                deleteSheet = false;
                                workSheet = theWorkbook.Worksheets[1];
                                string sheetName = m_vendorCode + " - " + (m_MCCode == "" ? "NonCate" : m_MCCode);
                                workSheet.Name = sheetName;
                                string m_strPONo = GeneratePONumber(BuyingBriefID);
                                m_db.UpdatePONoInSpotplanByVendor(BuyingBriefID, Version, m_vendorCode, m_strPONo);
                                DataTable dtPOID = m_db.SelectSpotPlanForPOIDByVendor(BuyingBriefID, Version, m_vendorCode);
                                //=======================================
                                // Material
                                //=======================================
                                if (dtMaterial.Rows.Count > 0)
                                {
                                    ExcelUtil.ExcelSetValueString(workSheet, "B", 15, dtMaterial.Rows[0]["Material_Name"].ToString());
                                }
                                //=======================================
                                // Spot Plan (PO) Header session
                                //=======================================
                                ExcelUtil.ExcelSetValueString(workSheet, "B", 9, dt.Rows[0]["Client_Name"].ToString());
                                ExcelUtil.ExcelSetValueString(workSheet, "B", 10, dtVendor.Rows[i]["Vendor_Name"].ToString());
                                ExcelUtil.ExcelSetValueString(workSheet, "B", 11, DateTime.Now.ToString("dd/MM/yyyy")); //Order Date
                                ExcelUtil.ExcelSetValueString(workSheet, "B", 12, dt.Rows[0]["Product_Name"].ToString());
                                ExcelUtil.ExcelSetValueString(workSheet, "B", 13, dt.Rows[0]["Campaign_Name"].ToString());
                                ExcelUtil.ExcelSetValueString(workSheet, "B", 14, dt.Rows[0]["Buying_Brief_ID"].ToString());
                                ExcelUtil.ExcelSetValueString(workSheet, "G", 9, dtPOID.Rows[0]["Booking_Order_ID"].ToString());
                                // create new sheet
                                if (j < dtMediaCategory.Rows.Count - 1)
                                {
                                    workSheet.Copy(theWorkbook.Worksheets[1]);
                                }
                                else
                                {
                                    if (i < dtVendor.Rows.Count - 1)
                                    {
                                        workSheet.Copy(theWorkbook.Worksheets[1]);
                                    }
                                }
                                //=======================================
                                // Remark
                                //=======================================
                                ExcelUtil.ExcelSetValueString(workSheet, "A", 24, dt.Rows[0]["Remark"].ToString());
                                //=======================================
                                // Spot Plan (PO) Details session
                                //=======================================
                                if (dtDetails.Rows.Count > 2)
                                {
                                    workSheet.Rows[19].Insert(Excel.XlInsertShiftDirection.xlShiftDown, workSheet.Rows[19].Copy(Type.Missing));
                                }
                                ExcelUtil.ExcelSetValueStringFullTable(workSheet, 18, 1, 3, dtDetails);
                                //=======================================
                                // Spot Plan PO-Footer
                                //=======================================
                                bool printFooter = m_db.CheckPrintPOFooter(Parameter, m_vendorCode);
                                if (printFooter)
                                {
                                    int rowFooter = 37;
                                    if (dtDetails.Rows.Count > 3)
                                    {
                                        rowFooter += dtDetails.Rows.Count - 3;
                                    }
                                    DataTable dtPOFooter = m_db.SelectPOFooter(Parameter);
                                    ExcelUtil.ExcelSetValueString(workSheet, "A", rowFooter, dtPOFooter.Rows[0]["PValue"].ToString());
                                    range = workSheet.get_Range($"A{rowFooter}:G{rowFooter}");
                                    range.Merge();
                                    range.Font.Size = 8;
                                    range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                                    range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                                    range.WrapText = true;
                                    range.Rows.RowHeight = 120;
                                }
                                //=======================================
                                // Spot Plan (PO) Footer - BARCODE
                                //=======================================
                                workSheet.PageSetup.FirstPage.LeftFooter.Text = "&\"Free 3 of 9 Extended\"&30& *" + BuyingBriefID + "PO" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*\n" +
                                "" + "&\"Arial\"&10&                               *" + BuyingBriefID + "PO" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*";
                                //=======================================
                                // Spot Plan (PO) Header Logo
                                //=======================================
                                workSheet.PageSetup.FirstPage.RightHeader.Picture.Filename = dt.Rows[0][0].ToString();
                                workSheet.PageSetup.FirstPage.RightHeader.Text = "&G";
                                workSheet.PageSetup.RightHeaderPicture.Filename = dt.Rows[0][0].ToString();
                                workSheet.PageSetup.RightHeader = "&G";
                                //=======================================
                                // Spot Plan (PO) Header
                                //=======================================
                                DataTable dtPOHeader = m_db.GetPOHeaderSpecial(BuyingBriefID, m_vendorCode);
                                string strCA = dtPOHeader.Rows[0]["Creative_Agency_ID"].ToString();
                                string strVendorSpecial = dtPOHeader.Rows[0]["Master_Group"].ToString();
                                string strSetPOHeader = "";
                                if (dtPOHeader.Rows.Count > 0)
                                {
                                    if ((strCA == "OMTL" || strCA == "OPRWTL" || strCA == "OOWTL" || strCA == "SOHO") && (strVendorSpecial == "MASTERCH7")) //Special for Master Channel 7
                                    {
                                        switch (dtPOHeader.Rows[0]["Agency_ID"].ToString())
                                        {
                                            case "GM":
                                            case "GMM6":
                                            case "GMTH":
                                                strSetPOHeader = "GroupM";
                                                break;
                                            case "MC":
                                            case "MCTH":
                                                strSetPOHeader = "MediaCom";
                                                break;
                                            case "ME01":
                                            case "MEC":
                                            case "MEC-OM":
                                                strSetPOHeader = "WAVEMAKER";
                                                break;
                                            default:
                                                strSetPOHeader = "Mindshare";
                                                break;
                                        }
                                        DataTable dtPOHeaderSpecial = m_db.GetPOHeader(dtPOHeader.Rows[0]["Agency_ID"].ToString(), strSetPOHeader);
                                        strSetPOHeader = dtPOHeaderSpecial.Rows[0]["Booking_Order_Header"].ToString();
                                        if (dtPOHeaderSpecial.Rows.Count > 0)
                                        {
                                            workSheet.PageSetup.FirstPage.CenterHeader.Text = strSetPOHeader + "\n" + dtPOHeaderSpecial.Rows[0]["Address"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                            dtPOHeaderSpecial.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address4"].ToString();
                                            workSheet.PageSetup.CenterHeader = strSetPOHeader + "\n" + dtPOHeaderSpecial.Rows[0]["Address"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                            dtPOHeaderSpecial.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address4"].ToString();
                                        }
                                    }
                                    else
                                    {
                                        strSetPOHeader = dtPOHeader.Rows[0]["Booking_Order_Header"].ToString();
                                        workSheet.PageSetup.FirstPage.CenterHeader.Text = strSetPOHeader + "\n" + dtPOHeader.Rows[0]["Address"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                        dtPOHeader.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address4"].ToString();
                                        workSheet.PageSetup.CenterHeader = strSetPOHeader + "\n" + dtPOHeader.Rows[0]["Address"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                        dtPOHeader.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address4"].ToString();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        DataTable dtDetails = m_db.SelectSpotPlanForPOESDetailsByVendor(BuyingBriefID, Version, m_vendorCode, StartDate, EndDate);
                        if (dtDetails.Rows.Count == 0)
                        {
                            deleteSheet = true;
                        }
                        else
                        {
                            noData = false;
                            deleteSheet = false;
                            workSheet = theWorkbook.Worksheets[1];
                            workSheet.Name = m_vendorCode;
                            string m_strPONo = GeneratePONumber(BuyingBriefID);
                            m_db.UpdatePONoInSpotplanByVendor(BuyingBriefID, Version, m_vendorCode, m_strPONo);
                            DataTable dtPOID = m_db.SelectSpotPlanForPOIDByVendor(BuyingBriefID, Version, m_vendorCode);
                            //=======================================
                            // Material
                            //=======================================
                            if (dtMaterial.Rows.Count > 0)
                            {
                                ExcelUtil.ExcelSetValueString(workSheet, "B", 15, dtMaterial.Rows[0]["Material_Name"].ToString());
                            }
                            //=======================================
                            // Spot Plan (PO) Header session
                            //=======================================
                            ExcelUtil.ExcelSetValueString(workSheet, "B", 9, dt.Rows[0]["Client_Name"].ToString());
                            ExcelUtil.ExcelSetValueString(workSheet, "B", 10, dtVendor.Rows[i]["Vendor_Name"].ToString());
                            ExcelUtil.ExcelSetValueString(workSheet, "B", 11, DateTime.Now.ToString("dd/MM/yyyy")); //Order Date
                            ExcelUtil.ExcelSetValueString(workSheet, "B", 12, dt.Rows[0]["Product_Name"].ToString());
                            ExcelUtil.ExcelSetValueString(workSheet, "B", 13, dt.Rows[0]["Campaign_Name"].ToString());
                            ExcelUtil.ExcelSetValueString(workSheet, "B", 14, dt.Rows[0]["Buying_Brief_ID"].ToString());
                            ExcelUtil.ExcelSetValueString(workSheet, "G", 9, dtPOID.Rows[0]["Booking_Order_ID"].ToString());
                            // create new sheet
                            if (i < dtVendor.Rows.Count - 1)
                            {
                                workSheet.Copy(theWorkbook.Worksheets[1]);
                            }
                            //=======================================
                            // Remark
                            //=======================================
                            ExcelUtil.ExcelSetValueString(workSheet, "A", 24, dt.Rows[0]["Remark"].ToString());
                            //=======================================
                            // Spot Plan (PO) Details session
                            //=======================================
                            if (dtDetails.Rows.Count > 2)
                            {
                                workSheet.Rows[19].Insert(Excel.XlInsertShiftDirection.xlShiftDown, workSheet.Rows[19].Copy(Type.Missing));
                            }
                            ExcelUtil.ExcelSetValueStringFullTable(workSheet, 18, 1, 3, dtDetails);
                            //=======================================
                            // Spot Plan PO-Footer
                            //=======================================
                            bool printFooter = m_db.CheckPrintPOFooter(Parameter, m_vendorCode);
                            if (printFooter)
                            {
                                int rowFooter = 37;
                                if (dtDetails.Rows.Count > 3)
                                {
                                    rowFooter += dtDetails.Rows.Count - 3;
                                }
                                DataTable dtPOFooter = m_db.SelectPOFooter(Parameter);
                                ExcelUtil.ExcelSetValueString(workSheet, "A", rowFooter, dtPOFooter.Rows[0]["PValue"].ToString());
                                range = workSheet.get_Range($"A{rowFooter}:G{rowFooter}");
                                range.Merge();
                                range.Font.Size = 8;
                                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                                range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                                range.WrapText = true;
                                range.Rows.RowHeight = 90;
                            }
                            //=======================================
                            // Spot Plan (PO) Footer - BARCODE
                            //=======================================
                            workSheet.PageSetup.FirstPage.LeftFooter.Text = "&\"Free 3 of 9 Extended\"&30& *" + BuyingBriefID + "PO" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*\n" +
                            "" + "&\"Arial\"&10&                               *" + BuyingBriefID + "PO" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*";
                            //=======================================
                            // Spot Plan (PO) Header Logo
                            //=======================================
                            workSheet.PageSetup.FirstPage.RightHeader.Picture.Filename = dt.Rows[0][0].ToString();
                            workSheet.PageSetup.FirstPage.RightHeader.Text = "&G";
                            workSheet.PageSetup.RightHeaderPicture.Filename = dt.Rows[0][0].ToString();
                            workSheet.PageSetup.RightHeader = "&G";
                            //=======================================
                            // Spot Plan (PO) Header
                            //=======================================
                            DataTable dtPOHeader = m_db.GetPOHeaderSpecial(BuyingBriefID, m_vendorCode);
                            string strCA = dtPOHeader.Rows[0]["Creative_Agency_ID"].ToString();
                            string strVendorSpecial = dtPOHeader.Rows[0]["Master_Group"].ToString();
                            string strSetPOHeader = "";
                            if (dtPOHeader.Rows.Count > 0)
                            {
                                if ((strCA == "OMTL" || strCA == "OPRWTL" || strCA == "OOWTL" || strCA == "SOHO") && (strVendorSpecial == "MASTERCH7")) //Special for Master Channel 7
                                {
                                    switch (dtPOHeader.Rows[0]["Agency_ID"].ToString())
                                    {
                                        case "GM":
                                        case "GMM6":
                                        case "GMTH":
                                            strSetPOHeader = "GroupM";
                                            break;
                                        case "MC":
                                        case "MCTH":
                                            strSetPOHeader = "MediaCom";
                                            break;
                                        case "ME01":
                                        case "MEC":
                                        case "MEC-OM":
                                            strSetPOHeader = "WAVEMAKER";
                                            break;
                                        default:
                                            strSetPOHeader = "Mindshare";
                                            break;
                                    }
                                    DataTable dtPOHeaderSpecial = m_db.GetPOHeader(dtPOHeader.Rows[0]["Agency_ID"].ToString(), strSetPOHeader);
                                    strSetPOHeader = dtPOHeaderSpecial.Rows[0]["Booking_Order_Header"].ToString();
                                    if (dtPOHeaderSpecial.Rows.Count > 0)
                                    {
                                        workSheet.PageSetup.FirstPage.CenterHeader.Text = strSetPOHeader + "\n" + dtPOHeaderSpecial.Rows[0]["Address"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                        dtPOHeaderSpecial.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address4"].ToString();
                                        workSheet.PageSetup.CenterHeader = strSetPOHeader + "\n" + dtPOHeaderSpecial.Rows[0]["Address"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                        dtPOHeaderSpecial.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeaderSpecial.Rows[0]["Thai_Address4"].ToString();
                                    }
                                }
                                else
                                {
                                    strSetPOHeader = dtPOHeader.Rows[0]["Booking_Order_Header"].ToString();
                                    workSheet.PageSetup.FirstPage.CenterHeader.Text = strSetPOHeader + "\n" + dtPOHeader.Rows[0]["Address"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                    dtPOHeader.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address4"].ToString();
                                    workSheet.PageSetup.CenterHeader = strSetPOHeader + "\n" + dtPOHeader.Rows[0]["Address"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address2"].ToString() + "\n" +
                                    dtPOHeader.Rows[0]["Thai_Address3"].ToString() + "\n" + dtPOHeader.Rows[0]["Thai_Address4"].ToString();
                                }
                            }
                        }
                    }
                }
                if (deleteSheet)
                {
                    if (theWorkbook.Worksheets.Count > 1)
                    {
                        ExcelObjDesc.DisplayAlerts = false;
                        workSheet = theWorkbook.Worksheets[1];
                        workSheet.Delete();
                        ExcelObjDesc.DisplayAlerts = true;
                    }
                }
                if (noData)
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
        private void Implementation_SpotPlan_Print_PO_Load(object sender, EventArgs e)
        {
            dtStartDate.Value = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strStartDate);
            dtEndDate.Value = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strEndDate);
            gvDetail.AutoGenerateColumns = false;
            gvDetail.DataSource = m_db.SelectVendorPO(m_strBuyingBriefID, m_strVersion);
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
                        gvDetail.Rows[i].Cells["SelectVendor"].Value = false;
                    }
                }
                else
                {
                    chkSelectAll.CheckState = CheckState.Checked;
                    statusChk = "check";
                    for (i = 0; i < gvDetail.Rows.Count; i++)
                    {
                        gvDetail.Rows[i].Cells["SelectVendor"].Value = true;
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
            int totalSelectVendor = CountSelectVendor();
            if (totalSelectVendor == 0)
            {
                chkSelectAll.CheckState = CheckState.Unchecked;
            }
            else
            {
                if (totalSelectVendor == gvDetail.Rows.Count)
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
                GMessage.MessageWarning("Please Select Vendor.");
            }
            else if (gvDetail.Rows.Count == 0)
            {
                GMessage.MessageWarning("No Data!");
            }
            else
            {
                bool mtES = false;
                bool mtIT = false;
                bool mtOD = false;
                string sd = dtStartDate.Value.ToString("yyyyMMdd");
                string ed = dtEndDate.Value.ToString("yyyyMMdd");
                SetTableVendor();
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
                // print purchase order
                if (fc)
                {
                    PrintPO(m_strPOExportPath, m_strParameterPath, m_strBuyingBriefID, m_strVersion, sd, ed);
                }
                else
                {
                    if (mtES)
                        PrintPO_ES(m_strPOExportPath, m_strParameterPath, m_strBuyingBriefID, m_strVersion, sd, ed);
                    if (mtIT)
                        PrintPO_IT(m_strPOExportPath, m_strParameterPath, m_strBuyingBriefID, m_strVersion, sd, ed);
                    if (mtOD)
                        PrintPO_OD(m_strPOExportPath, m_strParameterPath, m_strBuyingBriefID, m_strVersion, sd, ed);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
