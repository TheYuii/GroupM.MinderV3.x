using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using GroupM.UTL;

namespace  GroupM.Minder
{
    public partial class MPA008_TVCalendar : Form
    {
        string UserName = "";
        string Password = "";
        public MPA008_TVCalendar(string strUserName, string strPassword)
        {
            this.UserName = strUserName;
            this.Password = strPassword;
            InitializeComponent();
        }
        private void DataLoading()
        {
            if (cboMonth.Text == "" || cboYear.Text == "")
                return;
            SqlConnection conn = new SqlConnection(Connection.ConnectionStringMinder);
            string strSQL = "";
                strSQL = @"
    SELECT DISTINCT NULL chk, 
    Buying_Brief.Buying_Brief_ID BuyingBriefID
    ,spotversion.Status
    ,spotversion.Version
    ,Buying_Brief.Media_Sub_Type MediaSubType
    ,Target.Short_Name Target
    ,Buying_Brief.Product_ID ProductID
    ,Product.Short_Name ProductName
    ,Client.English_Name ClientName
    ,Buying_Brief.Campaign_Start_Date StartDate
    ,Buying_Brief.Campaign_End_Date EndDate
    FROM Buying_Brief with(nolock)
		INNER JOIN (select Distinct Buying_Brief_ID,Version 
		,case 
	 when Approve = 0 then 'DRAFT' 
	 when Approve = 4 then 'APPROVE'  
	 when Approve = 5 then 'EXECUTE' 
	 when Approve = 8 then 'ACTUAL' END Status
		from Spot_Plan_Version


where 1=1 {0}


) spotversion
			ON spotversion.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
	    INNER JOIN Client ON Client.Client_ID = Buying_Brief.Client_ID
	    INNER JOIN Product ON Product.Product_ID = Buying_Brief.Product_ID
	    INNER JOIN Target ON Target.Target_ID = Buying_Brief.Primary_Target
	    INNER JOIN dbo.[fn_User_CheckPermission](@UserName,NULL,NULL) ClientPermission
	     ON ClientPermission.Client_ID = Buying_Brief.Client_ID
    	
    WHERE Buying_Brief.Media_Type IN ('TV','TS')
    	
	    AND ((CONVERT(DATETIME,Buying_Brief.Campaign_Start_Date) BETWEEN @StartDate AND @EndDate
			    OR CONVERT(DATETIME,Buying_Brief.Campaign_End_Date) BETWEEN @StartDate AND @EndDate)
			    )";
            if (cboStatus.Text == "Draft")
                strSQL = string.Format(strSQL, " AND Approve = 0 and len(version) = 1");
            else if (cboStatus.Text == "Lasted")
                strSQL = string.Format(strSQL, " AND len(version) = 1");

            SqlDataAdapter da = new SqlDataAdapter(strSQL, conn);
            da.SelectCommand.CommandType = CommandType.Text;
            da.SelectCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = Connection.USERID.Replace(".", "");
            da.SelectCommand.Parameters.Add("@StartDate", SqlDbType.VarChar).Value = StartDate();
            da.SelectCommand.Parameters.Add("@EndDate", SqlDbType.VarChar).Value = EndDate();
            da.SelectCommand.CommandTimeout = 0;
            DataSet ds = new DataSet();
            da.Fill(ds);
            gvClient.AutoGenerateColumns = false;
            gvClient.DataSource = ds.Tables[0];
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvClient.SelectAll();
            }
        }
        private int GetMonthNoString(string strMonth)
        {
            switch (strMonth)
            {
                case "JAN": return 1; 
                case "FEB": return 2; 
                case "MAR": return 3; 
                case "APR": return 4;
                case "MAY": return 5; 
                case "JUN": return 6; 
                case "JUL": return 7; 
                case "AUG": return 8; 
                case "SEP": return 9;
                case "OCT": return 10;
                case "NOV": return 11; 
                case "DEC": return 12;
            }
            return 0;
        }
        private string StartDate()
        {
            DateTime dt = new DateTime(Convert.ToInt32(cboYear.Text), GetMonthNoString(cboMonth.Text.Trim()), 1);
            return dt.ToString("yyyyMMdd");
        }

        private string EndDate()
        {
            int iYear = Convert.ToInt32(cboYear.Text);
            int iMonth = GetMonthNoString(cboMonth.Text.Trim());
            //Next Month
            iMonth += 1;
            if (iMonth == 13)
            {
                iMonth = 1;
                iYear += 1;
            }
            DateTime dt = new DateTime(iYear, iMonth, 1);
            

            return dt.AddDays(-1).ToString("yyyyMMdd");
        }
        private string GetColDayOfWeek(DayOfWeek dow)
        {
            string strResult = "";
            switch (dow)
            {
                case DayOfWeek.Monday:
                    strResult = "G";
                    break;
                case DayOfWeek.Tuesday:
                    strResult = "M";
                    break;
                case DayOfWeek.Wednesday:
                    strResult = "S";
                    break;
                case DayOfWeek.Thursday:
                    strResult = "Y";
                    break;
                case DayOfWeek.Friday:
                    strResult = "AE";
                    break;
                case DayOfWeek.Saturday:
                    strResult = "AK";
                    break;
                case DayOfWeek.Sunday:
                    strResult = "AQ";
                    break;
            }
            return strResult;
            
        }
        private string GetColShowTime(DayOfWeek dow)
        {
            string strResult = "";
            switch (dow)
            {
                case DayOfWeek.Monday:
                    strResult = "B";
                    break;
                case DayOfWeek.Tuesday:
                    strResult = "H";
                    break;
                case DayOfWeek.Wednesday:
                    strResult = "N";
                    break;
                case DayOfWeek.Thursday:
                    strResult = "T";
                    break;
                case DayOfWeek.Friday:
                    strResult = "Z";
                    break;
                case DayOfWeek.Saturday:
                    strResult = "AF";
                    break;
                case DayOfWeek.Sunday:
                    strResult = "AL";
                    break;
            }
            return strResult;
        }
        private string GetColMedia(DayOfWeek dow)
        {
            string strResult = "";
            switch (dow)
            {
                case DayOfWeek.Monday:
                    strResult = "C";
                    break;
                case DayOfWeek.Tuesday:
                    strResult = "I";
                    break;
                case DayOfWeek.Wednesday:
                    strResult = "O";
                    break;
                case DayOfWeek.Thursday:
                    strResult = "U";
                    break;
                case DayOfWeek.Friday:
                    strResult = "AA";
                    break;
                case DayOfWeek.Saturday:
                    strResult = "AG";
                    break;
                case DayOfWeek.Sunday:
                    strResult = "AM";
                    break;
            }
            return strResult;
        }
        private string GetColMaterialKey(DayOfWeek dow)
        {
            string strResult = "";
            switch (dow)
            {
                case DayOfWeek.Monday:
                    strResult = "D";
                    break;
                case DayOfWeek.Tuesday:
                    strResult = "J";
                    break;
                case DayOfWeek.Wednesday:
                    strResult = "P";
                    break;
                case DayOfWeek.Thursday:
                    strResult = "V";
                    break;
                case DayOfWeek.Friday:
                    strResult = "AB";
                    break;
                case DayOfWeek.Saturday:
                    strResult = "AH";
                    break;
                case DayOfWeek.Sunday:
                    strResult = "AN";
                    break;
            }
            return strResult;
        }
        private string GetColProduct(DayOfWeek dow)
        {
            string strResult = "";
            switch (dow)
            {
                case DayOfWeek.Monday:
                    strResult = "E";
                    break;
                case DayOfWeek.Tuesday:
                    strResult = "K";
                    break;
                case DayOfWeek.Wednesday:
                    strResult = "Q";
                    break;
                case DayOfWeek.Thursday:
                    strResult = "W";
                    break;
                case DayOfWeek.Friday:
                    strResult = "AC";
                    break;
                case DayOfWeek.Saturday:
                    strResult = "AI";
                    break;
                case DayOfWeek.Sunday:
                    strResult = "AO";
                    break;
            }
            return strResult;
        }
        private string GetColProgramName(DayOfWeek dow)
        {
            string strResult = "";
            switch (dow)
            {
                case DayOfWeek.Monday:
                    strResult = "F";
                    break;
                case DayOfWeek.Tuesday:
                    strResult = "L";
                    break;
                case DayOfWeek.Wednesday:
                    strResult = "R";
                    break;
                case DayOfWeek.Thursday:
                    strResult = "X";
                    break;
                case DayOfWeek.Friday:
                    strResult = "AD";
                    break;
                case DayOfWeek.Saturday:
                    strResult = "AJ";
                    break;
                case DayOfWeek.Sunday:
                    strResult = "AP";
                    break;
            }
            return strResult;
        }
        public int Weeks(int year, int month)
        {
            DayOfWeek wkstart = DayOfWeek.Monday;

            DateTime first = new DateTime(year, month, 1);
            int firstwkday = (int)first.DayOfWeek;
            int otherwkday = (int)wkstart;

            int offset = ((otherwkday + 7) - firstwkday) % 7;

            double weeks = (double)(DateTime.DaysInMonth(year, month) - offset) / 7d;

            return (int)Math.Ceiling(weeks);
        } 

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                bool bCheck = false;
                for (int i = 0; i < gvClient.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(gvClient.Rows[i].Cells[0].Value) == true)
                    {
                        bCheck = true;
                        break;
                    }
                }
                if (!bCheck)
                {
                    GMessage.MessageWarning("Please select Buying Brief at least one number.");
                    return;
                }

                string strBB = "";
                string strBBVersion = "";
                for (int i = 0; i < gvClient.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(gvClient.Rows[i].Cells[0].Value) == true)
                    {
                        if (strBB != "")
                        {
                            strBB += ",";
                            strBBVersion += ",";
                        }
                        strBB += "'" + gvClient.Rows[i].Cells[1].Value.ToString() + "'";
                        strBBVersion += "'" + gvClient.Rows[i].Cells[1].Value.ToString() + gvClient.Rows[i].Cells[3].Value.ToString() + "'";
                    }
                }



                string strSQLSelectMaterial = string.Format(@"
SELECT DISTINCT
	NULL AS chk
	, Buying_Brief.Buying_Brief_ID
	, Buying_Brief.Description
	, Material_Key MaterialKey
	, Buying_Brief.Product_ID ProductID
	, COALESCE( Material.Short_Name, Material.Thai_Name, Material.English_Name) MaterialName
	, Spot_Plan.Material_ID	
FROM Spot_Plan with(nolock)
	INNER JOIN Buying_Brief		
		ON Spot_Plan.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
	INNER JOIN Material
		ON Material.Material_ID = Spot_Plan.Material_ID
	INNER JOIN Product	
		ON Buying_Brief.Product_ID = Product.Product_ID
WHERE Buying_Brief.Buying_Brief_ID+Spot_Plan.Version IN ({0})
	AND LEN(Spot_Plan.Version) <> 2 --Lastest
", strBBVersion);
                SqlConnection conn = new SqlConnection(Connection.ConnectionStringMinder);
                conn.Open();
                SqlDataAdapter sda = new SqlDataAdapter(strSQLSelectMaterial, conn);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                MPA008_TVCalendar_Material frmMaterial = new MPA008_TVCalendar_Material();
                frmMaterial.gvDetail.AutoGenerateColumns = false;
                frmMaterial.gvDetail.DataSource = ds.Tables[0];
                if (frmMaterial.ShowDialog() != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
                string strAllXMLRow = "";
                for (int i = 0; i < frmMaterial.gvDetail.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(frmMaterial.gvDetail.Rows[i].Cells[0].Value) == true)
                    {
                        string strXMLRow = string.Format(@"<row><Buying_Brief_ID>{0}</Buying_Brief_ID><MaterialKey>{1}</MaterialKey><Material_ID>{2}</Material_ID></row>"
                            , frmMaterial.gvDetail.Rows[i].Cells[1].Value.ToString()
                            , frmMaterial.gvDetail.Rows[i].Cells["colM"].Value.ToString()
                            , frmMaterial.gvDetail.Rows[i].Cells["colMaterialCode"].Value.ToString());

                        strAllXMLRow += @"
" + strXMLRow;
                    }
                }
                strSQLSelectMaterial = string.Format(@"
DECLARE @Material TABLE (Buying_Brief_ID varchar(25),Material_Key nvarchar(2),Material_ID nvarchar(15))

DECLARE @xml XML
SET @xml = '
{0}
'

INSERT INTO @Material (Buying_Brief_ID ,Material_Key ,Material_ID )
SELECT  
       Tbl.Col.value('Buying_Brief_ID[1]', 'varchar(25)') AS Buying_Brief_ID,  
       Tbl.Col.value('MaterialKey[1]', 'nvarchar(2)') AS MaterialKey,  
       Tbl.Col.value('Material_ID[1]', 'nvarchar(15)') AS Material_ID
FROM   @xml.nodes('//row') Tbl(Col)  

", strAllXMLRow);



                string strSavePath = "";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    strSavePath = saveFileDialog.FileName;
                else
                    return;
                int iYear = Convert.ToInt32(cboYear.Text);
                int iMonth = GetMonthNoString(cboMonth.Text);
                DateTime date = new DateTime(iYear, iMonth, 1);
                this.Cursor = Cursors.WaitCursor;
                //strSavePath = Directory.GetCurrentDirectory() + "\\" + DateTime.Now.ToString("yyyyMMdd_hhmm_ss") + ".xlsx";


                string strTemplatePathFile = @"L:\PRG\Minder Utilities\MPA\Report Template\TemplateTVCalendarReport.xlsx";

               
                //string strDraft = "";
                //if (cboStatus.Text == "Draft")
                //    strDraft = "=";
                //else
                //    strDraft = ">";



                string strSQL = string.Format(@"
--=========================================
-- Header
--=========================================
SELECT DISTINCT Client.English_Name ClientName
FROM Buying_Brief with(nolock)
	INNER JOIN Client ON Client.Client_ID = Buying_Brief.Client_ID
WHERE Buying_Brief.Buying_Brief_ID IN ({0})


                
SELECT DISTINCT Buying_Brief.Product_ID,Product.English_Name ProductName
FROM Buying_Brief with(nolock)
INNER JOIN Product ON Product.Product_ID =  Buying_Brief.Product_ID
WHERE Buying_Brief.Buying_Brief_ID IN ({0})



SELECT DISTINCT
	mat.Material_Key MaterialKey
	, Buying_Brief.Product_ID ProductID
	, COALESCE( Material.Short_Name, Material.Thai_Name, Material.English_Name) MaterialName
	
FROM Spot_Plan with(nolock)
	INNER JOIN Buying_Brief		
		ON Spot_Plan.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
	INNER JOIN Material
		ON Material.Material_ID = Spot_Plan.Material_ID
    INNER JOIN @Material mat
        ON mat.Buying_Brief_ID = Spot_Plan.Buying_Brief_ID
            AND mat.Material_ID = Spot_Plan.Material_ID
WHERE Buying_Brief.Buying_Brief_ID+Spot_Plan.Version IN ({1})
	AND LEN(Spot_Plan.Version) <> 2 --Lastest


--=========================================
-- Detail
--=========================================

SELECT 
	 Spot_Plan.Show_Date [ShowDate]
	, Spot_Plan.Start_Time StartTime
	, Buying_Brief.Product_ID ProductID
	, Media.English_Name Media
	, mat.Material_Key MaterialKey
	, COALESCE(Program.Thai_name,Program.Program_Name,Spot_Plan.Program) ProgramName
	, Spot_Plan.Spots
	
FROM Spot_Plan with(nolock)
	INNER JOIN Buying_Brief		
		ON Spot_Plan.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
	INNER JOIN Material
		ON Material.Material_ID = Spot_Plan.Material_ID
	INNER JOIN Media
		ON Media.Media_ID = Spot_Plan.Media_ID
    LEFT JOIN Program 
		ON Program.Program_Code = Spot_Plan.Program_Code
    INNER JOIN @Material mat
        ON mat.Buying_Brief_ID = Spot_Plan.Buying_Brief_ID
            AND mat.Material_ID = Spot_Plan.Material_ID
WHERE 
	Buying_Brief.Buying_Brief_ID+Spot_Plan.Version IN ({1})
	AND  LEN(Spot_Plan.Version) <> 2 --Lastest
ORDER BY ShowDate,StartTime,ProductID,Media,mat.Material_Key
                ", strBB,strBBVersion);// GetQuery();

                if (rdEng.Checked)
                {
                    strSQL = string.Format(@"
--=========================================
-- Header
--=========================================
SELECT DISTINCT Client.English_Name ClientName
FROM Buying_Brief with(nolock)
	INNER JOIN Client ON Client.Client_ID = Buying_Brief.Client_ID
WHERE Buying_Brief.Buying_Brief_ID IN ({0})


                
SELECT DISTINCT Buying_Brief.Product_ID,Product.English_Name ProductName
FROM Buying_Brief with(nolock)
INNER JOIN Product ON Product.Product_ID =  Buying_Brief.Product_ID
WHERE Buying_Brief.Buying_Brief_ID IN ({0})



SELECT DISTINCT
	mat.Material_Key MaterialKey
	, Buying_Brief.Product_ID ProductID
	, COALESCE( Material.Short_Name, Material.Thai_Name, Material.English_Name) MaterialName
	
FROM Spot_Plan with(nolock)
	INNER JOIN Buying_Brief		
		ON Spot_Plan.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
	INNER JOIN Material
		ON Material.Material_ID = Spot_Plan.Material_ID
    INNER JOIN @Material mat
        ON mat.Buying_Brief_ID = Spot_Plan.Buying_Brief_ID
            AND mat.Material_ID = Spot_Plan.Material_ID
WHERE Buying_Brief.Buying_Brief_ID+Spot_Plan.Version IN ({1})
	AND LEN(Spot_Plan.Version) <> 2 --Lastest


--=========================================
-- Detail
--=========================================

SELECT 
	 Spot_Plan.Show_Date [ShowDate]
	, Spot_Plan.Start_Time StartTime
	, Buying_Brief.Product_ID ProductID
	, Media.English_Name Media
	, mat.Material_Key MaterialKey
	, COALESCE(Program.Program_Name,Program.Thai_name,Spot_Plan.Program) ProgramName
	, Spot_Plan.Spots
	
FROM Spot_Plan with(nolock)
	INNER JOIN Buying_Brief		
		ON Spot_Plan.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
	INNER JOIN Material
		ON Material.Material_ID = Spot_Plan.Material_ID
	INNER JOIN Media
		ON Media.Media_ID = Spot_Plan.Media_ID
    LEFT JOIN Program 
		ON Program.Program_Code = Spot_Plan.Program_Code
    INNER JOIN @Material mat
        ON mat.Buying_Brief_ID = Spot_Plan.Buying_Brief_ID
            AND mat.Material_ID = Spot_Plan.Material_ID
WHERE 
	Buying_Brief.Buying_Brief_ID+Spot_Plan.Version IN ({1})
	AND  LEN(Spot_Plan.Version) <> 2 --Lastest
ORDER BY ShowDate,StartTime,ProductID,Media,mat.Material_Key
                ", strBB, strBBVersion);// GetQuery();

                
                }
                sda = new SqlDataAdapter(strSQLSelectMaterial + strSQL, conn);
                ds = new DataSet();
                sda.Fill(ds);
                DataTable dtClient = ds.Tables[0];
                DataTable dtProduct = ds.Tables[1];
                DataTable dtHeader = ds.Tables[2];
                DataTable dt = ds.Tables[3];

                if (File.Exists(strSavePath))
                    File.Delete(strSavePath);
                File.Copy(strTemplatePathFile, strSavePath);

                Excel.Application ExcelObj = new Excel.Application();
                //Excel.Workbook theWorkbook = ExcelObj.Workbooks.Open(strPathFile, 0, true, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, true, true);
                Excel.Workbook theWorkbook = ExcelObj.Workbooks.Open(strSavePath, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value,
                Missing.Value, Missing.Value,
                Missing.Value, Missing.Value,
                Missing.Value, Missing.Value,
                Missing.Value, Missing.Value,
                Missing.Value, Missing.Value);

                Excel.Sheets sheets = theWorkbook.Worksheets;
                Excel.Worksheet worksheet = sheets.get_Item(1);
                //===========================================
                // Header Excel
                //===========================================
                string strClient = "";
                for (int i = 0; i < dtClient.Rows.Count; i++)
                {
                    if(i == 0)
                        strClient += dtClient.Rows[i][0].ToString();
                    else
                        strClient += "," + dtClient.Rows[i][0].ToString();
                }
                worksheet.Cells[2, 6] = strClient;
                string strProduct = "";
                for (int i = 0; i < dtProduct.Rows.Count; i++)
                {
                    if (i == 0)
                        strProduct += string.Format("{0}({1})", dtProduct.Rows[i][0].ToString(), dtProduct.Rows[i][1].ToString());
                    else
                        strProduct += "," + string.Format("{0}({1})", dtProduct.Rows[i][0].ToString(), dtProduct.Rows[i][1].ToString());
                }
                worksheet.Cells[3, 6] = strProduct;
                worksheet.Cells[4, 6] = date.ToString("MMM. yyyy");

                
                int iStartColMaterial = 29;
                int iNextColMaterial = 7;
                if (chkHideProduct.Checked)
                {
                    iStartColMaterial = 30;
                    iNextColMaterial = 7;
                }
                int iMaxMaterialRow = 5;
                int iStartRowMaterial = 2;
                for (int i = 0; i < dtHeader.Rows.Count; i++)
                {
                    worksheet.Cells[iStartRowMaterial + i, iStartColMaterial] = string.Format("{0}({1}):{2}", dtHeader.Rows[i]["MaterialKey"].ToString(), dtHeader.Rows[i]["ProductID"].ToString(), dtHeader.Rows[i]["MaterialName"].ToString());
                    iMaxMaterialRow--;
                    if (iMaxMaterialRow == 0)
                    {
                        iMaxMaterialRow = 5;
                        iStartColMaterial += iNextColMaterial;
                        iStartRowMaterial -= 5;
                    }
                }
                //===========================================
                // Date Calendar
                //===========================================
                int iRowHeader = 9;
                int iNextRowHeader = 4;
                for (int i = 0; date.Month == iMonth; i++)
                {
                    worksheet.Cells[iRowHeader, GetColDayOfWeek(date.DayOfWeek)] = date.Day;

                    DataRow[] aDr = dt.Select(string.Format("ShowDate = '{0}'",date.ToString("yyyyMMdd")));
                    if (date.Day == 1 || date.DayOfWeek == DayOfWeek.Monday)
                    {
                        int iMaxRow = 0;
                        DateTime dtTmp = new DateTime(date.Year,date.Month,date.Day);
                        do
                        {
                            DataRow[] aTmpDr = dt.Select(string.Format("ShowDate = '{0}'", dtTmp.ToString("yyyyMMdd")));
                            if (iMaxRow < aTmpDr.Length)
                                iMaxRow = aTmpDr.Length;
                            dtTmp = dtTmp.AddDays(1);
                        }
                        while (dtTmp.DayOfWeek != DayOfWeek.Monday);
                        if (iMaxRow >= iNextRowHeader - 1)
                        {
                            int iInsertRow = iMaxRow - 1;
                            iNextRowHeader += iInsertRow;

                            Excel.Range range = (Excel.Range)worksheet.Rows[iRowHeader + 3, System.Type.Missing];
                            while (iInsertRow > 0)
                            {
                                range.Insert(Excel.XlInsertShiftDirection.xlShiftDown, System.Type.Missing);
                                iInsertRow--;
                            }
                        }
                    }
                    int iSumSpots = 0;
                    for (int iIndex = 0; iIndex < aDr.Length; iIndex++)
                    {
                        worksheet.Cells[iRowHeader + iIndex + 2, GetColShowTime(date.DayOfWeek)] = aDr[iIndex]["StartTime"];
                        worksheet.Cells[iRowHeader + iIndex + 2, GetColMedia(date.DayOfWeek)] = aDr[iIndex]["Media"];
                        worksheet.Cells[iRowHeader + iIndex + 2, GetColMaterialKey(date.DayOfWeek)] = aDr[iIndex]["MaterialKey"];
                        worksheet.Cells[iRowHeader + iIndex + 2, GetColProduct(date.DayOfWeek)] = aDr[iIndex]["ProductID"];
                        worksheet.Cells[iRowHeader + iIndex + 2, GetColProgramName(date.DayOfWeek)] = aDr[iIndex]["ProgramName"];
                        worksheet.Cells[iRowHeader + iIndex + 2, GetColDayOfWeek(date.DayOfWeek)] = aDr[iIndex]["Spots"];
                        iSumSpots += Convert.ToInt32(aDr[iIndex]["Spots"]);
                    }
                    if(iSumSpots != 0)
                    {
                        worksheet.Cells[iRowHeader + 1, GetColShowTime(date.DayOfWeek)] = "TOTAL  SPOTS";
                        worksheet.Cells[iRowHeader + 1, GetColDayOfWeek(date.DayOfWeek)] = iSumSpots;
                    }
                    if (chkHideProduct.Checked)
                    {
                        HideColumn(worksheet, GetColProduct(DayOfWeek.Sunday));
                        HideColumn(worksheet, GetColProduct(DayOfWeek.Saturday));
                        HideColumn(worksheet, GetColProduct(DayOfWeek.Friday));
                        HideColumn(worksheet, GetColProduct(DayOfWeek.Thursday));
                        HideColumn(worksheet, GetColProduct(DayOfWeek.Wednesday));
                        HideColumn(worksheet, GetColProduct(DayOfWeek.Tuesday));
                        HideColumn(worksheet, GetColProduct(DayOfWeek.Monday));
                    }

                    if (date.DayOfWeek == DayOfWeek.Sunday)
                    {
                        iRowHeader += iNextRowHeader;
                        iNextRowHeader = 4;
                    }
                    date = date.AddDays(1);
                    //if (date.ToString("yyyyMMdd") == "20120905")
                    //    break;
                }
                theWorkbook.Save();
                ExcelObj.Visible = true;
                //ExcelObj.Quit();
                
                MessageBox.Show("Generating Report Compeleted.", "Information");
                //this.Close();
                 
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
        private void HideColumn(Excel.Worksheet oSheet, string column)
        {
            Excel.Range range = (Excel.Range)oSheet.Columns[column, Type.Missing];
            range.EntireColumn.Hidden = true;
        }
        private void MPA008_TVCalendar_Load(object sender, EventArgs e)
        {

            cboStatus.Text ="Lasted";
            cboYear.Text = DateTime.Now.Year.ToString();
            cboMonth.Text = DateTime.Now.ToString("MMM").ToUpper();
            //cboYear.Text = "2013";
            //cboMonth.Text = DateTime.Now.ToString("MMM").ToUpper();
            
            //btnGenerate.PerformClick();
            DevExpress.Export.ExportSettings.DefaultExportType = DevExpress.Export.ExportType.WYSIWYG;
        }

        private void cboMonth_TextChanged(object sender, EventArgs e)
        {
            DataLoading();
        }

        private void cboYear_TextChanged(object sender, EventArgs e)
        {
            DataLoading();
        }

        private void chkCheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gvClient.Rows.Count; i++)
            {
                gvClient.Rows[i].Cells[0].Value = true;
            }
        }

        private void chkUnCheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gvClient.Rows.Count; i++)
            {
                gvClient.Rows[i].Cells[0].Value = false;
            }
        }

        private void cboStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataLoading();
        }
    }
}