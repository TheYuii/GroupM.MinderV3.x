using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using GroupM.UTL;
using System.Collections;

namespace  GroupM.Minder
{
    public partial class MPA006_ExportReport : Form
    {
        LoadingScreen ShowLoadingScreen = new LoadingScreen();
        private System.Threading.Thread StartThread;
        private System.Threading.Thread StopThread;

        public MPA006_ExportReport(string strUserName, string strPassword)
        {
            this.UserName = strUserName;
            this.Password = strPassword;
            InitializeComponent();
        }

        enum eReport { TVSchedulSupplierReport,TrackingGRPReport }
        string m_strConnection = Connection.ConnectionStringMinder;
        private string m_strUser;
        public string UserName
        {
            get { return m_strUser; }
            set { m_strUser = value; }
        }
        private string m_strPass;
        public string Password
        {
            get { return m_strPass; }
            set { m_strPass = value; }
        }
        eReport m_eReport = eReport.TVSchedulSupplierReport;//FirstNode

        private void SetEnvironment(eReport eSelectedReport)
        {
            m_eReport = eSelectedReport;
            switch (m_eReport)
            {
                case eReport.TVSchedulSupplierReport:
                    cboMediaSubType.SelectedText = "TV,CS";
                    cboMediaSubType.Enabled = false;
                    rdPeriodDate.Enabled = false;
                    dtpFrom.Enabled = false;
                    dtpTo.Enabled = false;
                    lbDateTo.Enabled = false;
                    
                    gvBuyingBrief.Enabled = true;

                    if (gvClient.SelectedRows.Count > 0)
                    {
                        DataTable dt = (DataTable)gvClient.DataSource;
                        RefreshBBList(dt.Rows[gvClient.SelectedRows[0].Index]["Client_ID"].ToString());
                    }
                    picThumbnail.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\img\MontlyTVScheduleSupplierReport.jpg");
                    break;
                case eReport.TrackingGRPReport:
                    cboMediaSubType.SelectedText = "TV";
                    cboMediaSubType.Enabled = false;
                    rdPeriodDate.Enabled = false;
                    dtpFrom.Enabled = false;
                    dtpTo.Enabled = false;
                    lbDateTo.Enabled = false;

                    gvBuyingBrief.DataSource = null;
                    gvBuyingBrief.Enabled = false;
                    picThumbnail.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\img\WeeklyTrackingReport.jpg");
                    break;
            }
        }

        private eReport ConvertToEReport(string strNodeName)
        {
            switch (strNodeName)
	        {
                case "TVSchedulSupplierReport" :
                    return eReport.TVSchedulSupplierReport;
                case "TrackingGRPReport":
                    return eReport.TrackingGRPReport;
            }
            return eReport.TVSchedulSupplierReport;
        }
        private string GetSheetName(string strProductName)
        {
            string strSheetName = strProductName.Replace("/", " ").Replace("\\", " ");
            if (strSheetName.Length > 30)
                strSheetName = strSheetName.Substring(0, 30);

            return strSheetName;
        }
        private void AddBuyingBrief(Excel.Workbook theWorkbook, string strProductName,Excel.Worksheet sheetSource)
        {
            Excel.Worksheet sheetDesc = null;
            for (int i = 0; i < theWorkbook.Sheets.Count; i++)
            {
                if (GetSheetName(strProductName) == ((Excel.Worksheet)theWorkbook.Sheets.get_Item(i + 1)).Name)
                {
                    sheetDesc = (Excel.Worksheet)theWorkbook.Sheets.get_Item(i + 1);
                    break;
                }
            }
            if (sheetDesc == null)
            {
                //===================================
                //Add New Sheet
                //===================================
                theWorkbook.Sheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                sheetDesc = (Excel.Worksheet)theWorkbook.Worksheets.get_Item(1);

                DataGridViewRow dgvRow = gvClient.SelectedRows[0];
                DataRowView drv = (DataRowView)dgvRow.DataBoundItem;
                DataRow dr = drv.Row;

                sheetDesc.Cells[1, 1] = "Client Name";
                sheetDesc.Cells[2, 1] = "Product Name";
                ((Excel.Range)sheetDesc.Cells[1, 1]).Font.Bold = true;
                ((Excel.Range)sheetDesc.Cells[2, 1]).Font.Bold = true;

                sheetDesc.Cells[1, 2] = dr["ClientName"].ToString();
                sheetDesc.Cells[2, 2] = strProductName;
                sheetDesc.Name = GetSheetName(strProductName);
                ((Excel.Range)sheetDesc.Columns["A", Type.Missing]).ColumnWidth = 14.71;
                ((Excel.Range)sheetDesc.Columns["F", Type.Missing]).Hidden = true;

            }
            
            Excel.Range range = (Excel.Range)sheetDesc.get_Range("A1", Missing.Value);
            Excel.Range lastCell = range.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, System.Type.Missing);
            int iNextRow = lastCell.Row + 3;

            sheetSource.get_Range("A1", "M13").Copy(Type.Missing);
            range = sheetDesc.Cells[iNextRow, 1] as Excel.Range;
            range.Select();
            sheetDesc.Paste(Type.Missing, Type.Missing);

            range = sheetDesc.Cells[1, 1] as Excel.Range;
            range.Select();

            //==========================
            // Clear sheet
            //==========================
            sheetSource.Cells[1, 2] = "";
            sheetSource.Cells[2, 2] = "";
            sheetSource.Cells[3, 2] = "";
            for (int i = 7; i <= 12; i++)
            {
                sheetSource.Cells[i, 1] = "";
                sheetSource.Cells[i, 3] = "";
                sheetSource.Cells[i, 4] = "";
                sheetSource.Cells[i, 6] = "";
            }
        }
        
        private void InitControl()
        {
            //=======================================
            // Tree View
            //=======================================
            tvReportName.ExpandAll();
            tvReportName.SelectedNode = tvReportName.Nodes[0].Nodes[0];
            //=======================================
            // Combo
            //=======================================
            cboMediaSubType.SelectedIndex = 4; 
            cboMonth.SelectedIndex = DateTime.Now.Month - 1;
            cboYear.SelectedIndex = cboYear.Items.IndexOf(DateTime.Now.Year.ToString());
            //=======================================
            // Date
            //=======================================
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;
            //=======================================
            // Data Table
            //=======================================
            RefreshClientList();
        }
        private void RefreshClientList()
        {
            if (cboYear.Text == "" || cboMonth.Text == "")
                return;

            string strLastClientSelected = "";
            if (gvClient.Rows.Count > 0)
                if (gvClient.SelectedRows.Count > 0)
                    strLastClientSelected = gvClient.SelectedRows[0].Cells[0].Value.ToString();


            SqlConnection conn = new SqlConnection(m_strConnection);
            SqlDataAdapter da = new SqlDataAdapter("dbo.[MPA_get_Client]", conn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = UserName.Replace(".", "");
            da.SelectCommand.Parameters.Add("@MediaSubType", SqlDbType.VarChar).Value = cboMediaSubType.Text;
            da.SelectCommand.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = new DateTime(Convert.ToInt32(cboYear.Text),cboMonth.SelectedIndex +1,1);
            da.SelectCommand.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = new DateTime(Convert.ToInt32(cboYear.Text), cboMonth.SelectedIndex + 1, 1).AddMonths(1).AddDays(-1);
            da.SelectCommand.CommandTimeout = 0;
            DataSet ds = new DataSet();
            da.Fill(ds);
            gvClient.AutoGenerateColumns = false;
            gvClient.DataSource = ds.Tables[0];
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < gvClient.Rows.Count; i++)
                {
                    if (gvClient.Rows[i].Cells[0].Value.ToString() == strLastClientSelected)
                    {
                        gvClient.Rows[i].Selected = true;
                        break;
                    }
                }
                if(gvClient.SelectedRows.Count == 0)
                    gvClient.Rows[0].Selected = true;
            }
        }
        private void RefreshBBList(string strCleintID)
        {
            
            SqlConnection conn = new SqlConnection(m_strConnection);
            string strSQL = "dbo.[MPA_get_BuyingBrief]";
            SqlDataAdapter da = new SqlDataAdapter(strSQL, conn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = strCleintID;
            da.SelectCommand.Parameters.Add("@MediaSubType", SqlDbType.VarChar).Value = cboMediaSubType.Text;
            da.SelectCommand.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = new DateTime(Convert.ToInt32(cboYear.Text), cboMonth.SelectedIndex + 1, 1);
            da.SelectCommand.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = new DateTime(Convert.ToInt32(cboYear.Text), cboMonth.SelectedIndex + 1, 1).AddMonths(1).AddDays(-1);
            da.SelectCommand.CommandTimeout = 0;
            DataSet ds = new DataSet();
            da.Fill(ds);
            gvBuyingBrief.AutoGenerateColumns = false;
            gvBuyingBrief.DataSource = ds.Tables[0];
        }

        private int TVSchedulSupplierReport_GetRowIndex(string strMediaSubType, string strDayPartName)
        { 
            if(strMediaSubType == "TV")
            {
                int iTVStart = 28;
                switch (strDayPartName)
                {
                    case "0600-0759 Early Morning":
                        return iTVStart;
                    case "0800-1159 Late Morning Weekday":
                        return iTVStart + 1;
                    case "0800-1159 Late Morning Weekend":
                        return iTVStart + 2;
                    case "1200-1559 Afternoon Weekday":
                        return iTVStart + 3;
                    case "1200-1559 Afternoon Weekend":
                        return iTVStart + 4;
                    case "1600-1759 Early Evening":
                        return iTVStart + 5;
                    case "1800-1929 PrimeTime I Weekday":
                        return iTVStart + 6;
                    case "1800-1929 PrimeTime I Weekend":
                        return iTVStart + 7;
                    case "1930-2220 PrimeTime II Weekday":
                        return iTVStart + 8;
                    case "1930-2220 PrimeTime II Weekend":
                        return iTVStart + 9;
                    case "2221-2359 Late Fringe":
                        return iTVStart + 10;
                    case "2400-2559 Late Night I":
                        return iTVStart + 11;
                    case "2600-2959 Late Night II":
                        return iTVStart + 12;
                }
            }
            else if (strMediaSubType == "CS")
            {
                int iCSStart = 54;
                switch (strDayPartName)
                {
                    case "0600-0759 Early Morning":
                        return iCSStart;
                    case "0800-1159 Late Morning Weekday":
                        return iCSStart + 1;
                    case "0800-1159 Late Morning Weekend":
                        return iCSStart + 2;
                    case "1200-1559 Afternoon Weekday": 
                        return iCSStart + 3;
                    case "1200-1559 Afternoon Weekend":
                        return iCSStart + 4;
                    case "1600-1759 Early Evening":
                        return iCSStart + 5;
                    case "1800-1929 PrimeTime I Weekday":
                        return iCSStart + 6;
                    case "1800-1929 PrimeTime I Weekend":
                        return iCSStart + 7;
                    case "1930-2220 PrimeTime II Weekday":
                        return iCSStart + 8;
                    case "1930-2220 PrimeTime II Weekend":
                        return iCSStart + 9;
                    case "2221-2359 Late Fringe":
                        return iCSStart + 10;
                    case "2400-2559 Late Night I":
                        return iCSStart + 11;
                    case "2600-2959 Late Night II":
                        return iCSStart + 12;
                }
            }
            return 0;
        }
        
        private DataTable GetData_TrackingGRPReport()
        {

            string strYearMonth = "0" + (cboMonth.SelectedIndex + 1).ToString();
            strYearMonth = cboYear.Text + strYearMonth.Substring(strYearMonth.Length - 2, 2) + "01";

            DataGridViewRow dgvRow = gvClient.SelectedRows[0];
            DataRowView drv = (DataRowView)dgvRow.DataBoundItem;
            DataRow dr = drv.Row;

            SqlConnection conn = new SqlConnection(m_strConnection);
            string strSQL = string.Format(@"
DECLARE @Month VARCHAR(12)
DECLARE @ClientID VARCHAR(100)
SET @Month = '{0}' -- '20110901'
SET @ClientID = '{1}' --'30P&GMOB'

DECLARE @S VARCHAR(12)
DECLARE @E VARCHAR(12)
SET @S = CONVERT(VARCHAR(8),CONVERT(datetime,substring(convert(varchar(8),@Month,112),0,7)+'01'),112)
SET @E = CONVERT(VARCHAR(8),DATEADD(d,-1,convert(datetime,LEFT(convert(varchar(8),@Month,112),4)+ RIGHT('0'+CONVERT(VARCHAR(2),Month(DATEADD(m, Month(@Month),1))),2)+ '01')),112)
--SELECT @S,@E
SELECT 
	ProductName
	, Buying_Brief_ID
	, TargetName
	, CampaignStartDate
	, CampaignEndDate
	, StartDayWeek
    , GRPS
	, SUM(CASE WHEN [Version] = '1-APPROVE (PLANNED)' THEN Rating ELSE 0 END) ApproveRating
	, SUM(CASE WHEN [Version] = '3-ACTUAL (POST)' THEN Rating ELSE 0 END) ActualRating
	, SUM(CASE WHEN IsPrimeTime <> 'Regular Time' THEN Rating ELSE 0 END) PrimeRating
	--, SUM(Spot_Plan.Rating) Rating
FROM 
(
SELECT 
	--Client.English_Name ClientName, 
	Product.English_Name ProductName
	, Spot_Plan.Buying_Brief_ID
	, Target.Short_Name TargetName
	, Buying_Brief.Campaign_Start_Date AS CampaignStartDate
	, Buying_Brief.Campaign_End_Date AS CampaignEndDate
	, CASE LEN(Spot_Plan.Version) WHEN 2 THEN '1-APPROVE (PLANNED)' ELSE '2-LATEST (EXECUTING)' END [Version]
	, dbo.[fn_MondayOfWeek](Spot_Plan.Show_Date) StartDayWeek
	--, Buying_Brief.Description CampaignName
	--, Buying_Brief.Media_Sub_Type MediaSubType
	, Spot_Plan.Start_Time
	, Spot_Plan.End_Time
	, Spot_Plan.Show_Date
	, 'Regular Time' IsPrimeTime
    , Buying_Brief.GRPS
	, Spot_Plan.Rating
FROM Spot_Plan with(nolock)
	INNER JOIN Buying_Brief		
		ON Spot_Plan.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
	INNER JOIN Target 
		ON Buying_Brief.Primary_Target = Target.Target_ID
	--INNER JOIN Client
	--	ON Buying_Brief.Client_ID = Client.Client_ID
	INNER JOIN Product
		ON Buying_Brief.Product_ID = Product.Product_ID
WHERE 
	Buying_Brief.Media_Sub_Type IN ('TV')
	AND Spot_Plan.Show_Date BETWEEN @S AND @E
	AND Spot_Plan.Status > 0
	AND LEN(Spot_Plan.Version) = 2 --Approve Only
	AND  Buying_Brief.Client_ID = @ClientID
	
UNION

SELECT 
	--Client.English_Name ClientName, 
	Product.English_Name ProductName
	, SpotsMatch.Buying_Brief_ID
	, Target.Short_Name TargetName
	, Buying_Brief.Campaign_Start_Date AS CampaignStartDate
	, Buying_Brief.Campaign_End_Date AS CampaignEndDate
	, '3-ACTUAL (POST)' [Version]
	, dbo.[fn_MondayOfWeek](SpotsMatch.Show_Date) StartDayWeek
	--, Buying_Brief.Description CampaignName
	--, Buying_Brief.Media_Sub_Type MediaSubType
	,SpotsMatch.Start_Time
	,SpotsMatch.End_Time
	,SpotsMatch.Show_Date
	,dbo.GetPrimeTimeForReport(SpotsMatch.Start_Time,SpotsMatch.End_Time,SpotsMatch.Show_Date) IsPrimeTime
    ,Buying_Brief.GRPS
	,SpotsMatch.Actual_Rating Rating
FROM (
		SELECT 
			Spots_Match.Buying_Brief_ID
			,Spot_Plan.Net_Cost
			,Spots_Match.SP_Show_Date Show_Date
			,Spot_Plan.Start_Time
			,Spot_Plan.End_Time
			,Spots_Match.Actual_Rating
			,Spot_Plan.Spots
			,Spot_Plan.Package SpotType
		FROM Spot_Plan   with(nolock)
		INNER JOIN Spots_Match   with(nolock)
			ON (Spots_Match.Buying_Brief_ID = Spot_Plan.Buying_Brief_ID) 
				AND (Spots_Match.Market_ID = Spot_Plan.Market_ID) 
				AND (Spots_Match.[Version] = Spot_Plan.[Version]) 
				AND (Spots_Match.Item = Spot_Plan.Item) 
				AND (Spots_Match.SP_Show_Date = Spot_Plan.Show_Date) 
				AND (Spots_Match.ID = Spot_Plan.ID) 
				AND (Spots_Match.[Status] = Spot_Plan.[Status])
		INNER JOIN Media_Vendor
			ON Media_Vendor.Media_Vendor_ID = Spot_Plan.Media_Vendor_ID
		WHERE Spots_Match.Actual_Rating IS NOT NULL		
		UNION ALL		
		SELECT 
			Spots_Match.Buying_Brief_ID
			, 0 Net_Cost
			,Spots_Match.Show_Date
			,Spots_Match.Actual_Time Start_Time
			,Spots_Match.Actual_Time End_Time
			,Spots_Match.Actual_Rating 
			, 1 Spots
			,'Unknown Bonus' SpotType
		FROM Spots_Match  with(nolock)
			INNER JOIN Media_Vendor
				ON Media_Vendor.Media_Vendor_ID = Spots_Match.Media_Vendor
		WHERE Spots_Match.Flag_Bonus = 1 AND Spots_Match.Actual_Rating IS NOT NULL
		
		) SpotsMatch 
	INNER JOIN Buying_Brief		
		ON SpotsMatch.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
	INNER JOIN Target 
		ON Buying_Brief.Primary_Target = Target.Target_ID
	--INNER JOIN Client
	--	ON Buying_Brief.Client_ID = Client.Client_ID
	INNER JOIN Product
		ON Buying_Brief.Product_ID = Product.Product_ID
WHERE 
	Buying_Brief.Media_Sub_Type IN ('TV')
	AND SpotsMatch.Show_Date BETWEEN @S AND @E
	AND SpotsMatch.Actual_Rating IS NOT NULL
	AND Buying_Brief.Client_ID = @ClientID
) rec
GROUP BY 	ProductName
	, Buying_Brief_ID
	, TargetName
	, CampaignStartDate
	, CampaignEndDate
	, StartDayWeek
    , GRPS
	--, CampaignName
	--, MediaSubType
ORDER BY ProductName DESC,Buying_Brief_ID,StartDayWeek                     
            ", strYearMonth, dr["Client_ID"].ToString());
            SqlDataAdapter da = new SqlDataAdapter(strSQL, conn);
            da.SelectCommand.CommandTimeout = 0;
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds.Tables[0];

        }
        private DataSet GetData_TVSchedulSupplierReport(string strBB)
        {

            string strYearMonth = "0" + (cboMonth.SelectedIndex + 1).ToString();
            strYearMonth = cboYear.Text + strYearMonth.Substring(strYearMonth.Length - 2, 2) + "01";

            DataGridViewRow dgvRow = gvClient.SelectedRows[0];
            DataRowView drv = (DataRowView)dgvRow.DataBoundItem;
            DataRow dr = drv.Row;

            SqlConnection conn = new SqlConnection(m_strConnection);
            string strSQL = string.Format(@"
--DECLARE @BB VARCHAR(20)
--SET @BB = '2011040624'
select sp.Buying_Brief_ID,MAX(spv.Version) Version,AVG(spv.Population) Viewer,SUM(sp.Net_Cost/sp.length*30) Cost30  INTO #bb
from  Spot_plan sp
INNER JOIN Spot_Plan_Version spv 
	ON sp.Buying_Brief_ID = spv.Buying_Brief_ID 
	and sp.Version = spv.Version
where charindex(sp.Buying_Brief_ID,@BB)  > 0
and len(sp.Version) = 1 
group by sp.Buying_Brief_ID

-------------------------------------------
-- Header
-------------------------------------------


SELECT TOP 1 
a.English_Name AS AgencyName
,c.English_Name AS ClientName
,p.English_Name AS ProductName
,bb.Description AS CampaignName
,LEFT(RIGHT(@BB,LEN(@BB)-1),LEN(RIGHT(@BB,LEN(@BB)-1))-1) AS BuyingBriefNo
,t.Short_Name AS Target
,Campaign_Start_Date +' - '+ Campaign_End_Date Period
, bb.Agency_Commission
,SUM(bb.CPRP30) ActualCPRP
,SUM(sp.Cost30) Cost30
,AVG(sp.Viewer) Viewer
FROM Buying_Brief bb
INNER JOIN dbo.Agency a
	ON bb.Agency_ID = a.Agency_ID
INNER JOIN dbo.Client c
	ON c.Client_ID = bb.Client_ID
INNER JOIN dbo.Product p
	ON p.Product_ID = bb.Product_ID
INNER JOIN dbo.Target t
	ON t.Target_ID = bb.Primary_Target
INNER JOIN #bb sp
	ON sp.Buying_Brief_ID = bb.Buying_Brief_ID
GROUP BY a.English_Name 
,c.English_Name 
,p.English_Name 
,bb.Description 
,Campaign_Start_Date +' - '+ Campaign_End_Date
,bb.Agency_Commission
,t.Short_Name



-------------------------------------------
-- By Channel
-------------------------------------------
SELECT * FROM (
SELECT 
	0 seq
	, Media.English_Name Media
	, Spot_Plan.Package SpotType
	, SUM(Spot_Plan.Spots) Spots
	, SUM(Spot_Plan.Net_Cost) Cost
	, SUM(Spot_Plan.Rating) GRPs
FROM Spot_Plan with(nolock)
	INNER JOIN Buying_Brief		
		ON Spot_Plan.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
	INNER JOIN Target 
		ON Buying_Brief.Primary_Target = Target.Target_ID
	INNER JOIN Client
		ON Buying_Brief.Client_ID = Client.Client_ID
	INNER JOIN Product
		ON Buying_Brief.Product_ID = Product.Product_ID
	INNER JOIN Material
		ON Material.Material_ID = Spot_Plan.Material_ID
	INNER JOIN Media_Vendor
		ON Media_Vendor.Media_Vendor_ID = Spot_Plan.Media_Vendor_ID
	INNER JOIN Media
		ON Media.Media_ID = Spot_Plan.Media_ID
    INNER JOIN #bb sp
        ON sp.Buying_Brief_ID = Spot_Plan.Buying_Brief_ID
WHERE 
	Buying_Brief.Media_Sub_Type IN ('TV') 
	AND Spot_Plan.Status > 0
	AND LEN(Spot_Plan.Version) = 1 --'2-LATEST (EXECUTING)' 
GROUP BY  
	 Media.English_Name 
	, Spot_Plan.Package 

UNION

SELECT 
	0 seq
	, Media.English_Name Media
	, 'zTotal'
	, SUM(Spot_Plan.Spots) Spots
	, SUM(Spot_Plan.Net_Cost) Cost
	, SUM(Spot_Plan.Rating) GRPs
FROM Spot_Plan with(nolock)
	INNER JOIN Buying_Brief		
		ON Spot_Plan.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
	INNER JOIN Target 
		ON Buying_Brief.Primary_Target = Target.Target_ID
	INNER JOIN Client
		ON Buying_Brief.Client_ID = Client.Client_ID
	INNER JOIN Product
		ON Buying_Brief.Product_ID = Product.Product_ID
	INNER JOIN Material
		ON Material.Material_ID = Spot_Plan.Material_ID
	INNER JOIN Media_Vendor
		ON Media_Vendor.Media_Vendor_ID = Spot_Plan.Media_Vendor_ID
	INNER JOIN Media
		ON Media.Media_ID = Spot_Plan.Media_ID
    INNER JOIN #bb sp
        ON sp.Buying_Brief_ID = Spot_Plan.Buying_Brief_ID
WHERE 
	Buying_Brief.Media_Sub_Type IN ('TV','CS') 
	AND Spot_Plan.Status > 0
	AND LEN(Spot_Plan.Version) = 1 --'2-LATEST (EXECUTING)' 
GROUP BY  
	 Media.English_Name 

UNION


SELECT 
	1 seq
	, 'TV'
	, ''
	, SUM(Spot_Plan.Spots) Spots
	, SUM(Spot_Plan.Net_Cost) Cost
	, SUM(Spot_Plan.Rating) GRPs
FROM Spot_Plan with(nolock)
	INNER JOIN Buying_Brief		
		ON Spot_Plan.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
	INNER JOIN Target 
		ON Buying_Brief.Primary_Target = Target.Target_ID
	INNER JOIN Client
		ON Buying_Brief.Client_ID = Client.Client_ID
	INNER JOIN Product
		ON Buying_Brief.Product_ID = Product.Product_ID
	INNER JOIN Material
		ON Material.Material_ID = Spot_Plan.Material_ID
	INNER JOIN Media_Vendor
		ON Media_Vendor.Media_Vendor_ID = Spot_Plan.Media_Vendor_ID
	INNER JOIN Media
		ON Media.Media_ID = Spot_Plan.Media_ID
    INNER JOIN #bb sp
	    ON sp.Buying_Brief_ID = Spot_Plan.Buying_Brief_ID
WHERE 
	Buying_Brief.Media_Sub_Type = 'TV'
	AND Spot_Plan.Status > 0
	AND LEN(Spot_Plan.Version) = 1 --'2-LATEST (EXECUTING)' 
	
	
UNION


SELECT 
	2 seq
	, 'CABLE'
	, ''
	, SUM(Spot_Plan.Spots) Spots
	, SUM(Spot_Plan.Net_Cost) Cost
	, SUM(Spot_Plan.Rating) GRPs
FROM Spot_Plan with(nolock)
	INNER JOIN Buying_Brief		
		ON Spot_Plan.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
	INNER JOIN Target 
		ON Buying_Brief.Primary_Target = Target.Target_ID
	INNER JOIN Client
		ON Buying_Brief.Client_ID = Client.Client_ID
	INNER JOIN Product
		ON Buying_Brief.Product_ID = Product.Product_ID
	INNER JOIN Material
		ON Material.Material_ID = Spot_Plan.Material_ID
	INNER JOIN Media_Vendor
		ON Media_Vendor.Media_Vendor_ID = Spot_Plan.Media_Vendor_ID
	INNER JOIN Media
		ON Media.Media_ID = Spot_Plan.Media_ID
    INNER JOIN #bb sp
	    ON sp.Buying_Brief_ID = Spot_Plan.Buying_Brief_ID
WHERE 
	Buying_Brief.Media_Sub_Type = 'CS'
	AND Spot_Plan.Status > 0
	AND LEN(Spot_Plan.Version) = 1 --'2-LATEST (EXECUTING)' 
	
UNION

SELECT 
	3 seq
	, 'GRAND TOTAL'
	, ''
	, SUM(Spot_Plan.Spots) Spots
	, SUM(Spot_Plan.Net_Cost) Cost
	, SUM(Spot_Plan.Rating) GRPs
FROM Spot_Plan with(nolock)
	INNER JOIN Buying_Brief		
		ON Spot_Plan.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
	INNER JOIN Target 
		ON Buying_Brief.Primary_Target = Target.Target_ID
	INNER JOIN Client
		ON Buying_Brief.Client_ID = Client.Client_ID
	INNER JOIN Product
		ON Buying_Brief.Product_ID = Product.Product_ID
	INNER JOIN Material
		ON Material.Material_ID = Spot_Plan.Material_ID
	INNER JOIN Media_Vendor
		ON Media_Vendor.Media_Vendor_ID = Spot_Plan.Media_Vendor_ID
	INNER JOIN Media
		ON Media.Media_ID = Spot_Plan.Media_ID
    INNER JOIN #bb sp
	    ON sp.Buying_Brief_ID = Spot_Plan.Buying_Brief_ID
WHERE Spot_Plan.Status > 0
	AND LEN(Spot_Plan.Version) = 1 --'2-LATEST (EXECUTING)' 
) res
ORDER BY res.seq

---------------------------------------------------------------------
-- BY Day Part
---------------------------------------------------------------------
SELECT 
DayPart.Media_Sub_Type AS MediasubType
,DayPart.Part_Name AS DayPartName
,res.Spots
,res.Cost
,res.GRPs
FROM (select 'TV' Media_Sub_Type,Part_Name from [Day_Part] UNION select 'CS' Media_Sub_Type,Part_Name from [Day_Part]) DayPart 
LEFT JOIN (

SELECT 
	 Buying_Brief.Media_Sub_Type MediaSubType
	, dbo.GetDayPart(Spot_Plan.Start_Time,Spot_Plan.End_Time,Spot_Plan.Show_Date) DayPartName
	, SUM(Spot_Plan.Spots) Spots
	, SUM(Spot_Plan.Net_Cost) Cost
	, SUM(Spot_Plan.Rating) GRPs
FROM Spot_Plan with(nolock)
	INNER JOIN Buying_Brief		
		ON Spot_Plan.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
    INNER JOIN #bb sp
	    ON sp.Buying_Brief_ID = Spot_Plan.Buying_Brief_ID
WHERE 
	Buying_Brief.Media_Sub_Type IN ('TV','CS') 
	AND Spot_Plan.Status > 0
	AND LEN(Spot_Plan.Version) = 1 --'2-LATEST (EXECUTING)' 
GROUP BY  
	 dbo.GetDayPart(Spot_Plan.Start_Time,Spot_Plan.End_Time,Spot_Plan.Show_Date) 
	, Buying_Brief.Media_Sub_Type  
	) res 
			ON DayPart.Media_Sub_Type = res.MediaSubType
		   AND DayPart.Part_Name = res.DayPartName
ORDER BY DayPart.Media_Sub_Type DESC


---------------------------------------------------------------------
-- Material
---------------------------------------------------------------------

SELECT 
	 Buying_Brief.Media_Sub_Type MediaSubType
	, Spot_Plan.Material_KEY 
	, Material.Thai_Name MaterialName
	, Spot_Plan.Length
	, SUM(Spot_Plan.Spots) Spots
	, SUM(Spot_Plan.Net_Cost) Cost
	, SUM(Spot_Plan.Rating) GRPs
FROM Spot_Plan with(nolock)
	INNER JOIN Buying_Brief		
		ON Spot_Plan.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
	INNER JOIN Target 
		ON Buying_Brief.Primary_Target = Target.Target_ID
	INNER JOIN Material
		ON Material.Material_ID = Spot_Plan.Material_ID
    INNER JOIN #bb sp
	    ON sp.Buying_Brief_ID = Spot_Plan.Buying_Brief_ID
WHERE 
	Buying_Brief.Media_Sub_Type IN ('TV','CS') 
	AND Spot_Plan.Status > 0
	AND LEN(Spot_Plan.Version) = 1 --'2-LATEST (EXECUTING)'
GROUP BY  
	 Buying_Brief.Media_Sub_Type  
	, Spot_Plan.Material_KEY 
	, Material.Thai_Name 
	, Spot_Plan.Length

");
            SqlDataAdapter da = new SqlDataAdapter(strSQL, conn);
            da.SelectCommand.CommandTimeout = 0;
            da.SelectCommand.Parameters.Add("@BB", SqlDbType.VarChar).Value = strBB;
            DataSet ds = new DataSet();
            da.Fill(ds);
            ds.Tables[0].TableName = "Header";
            ds.Tables[1].TableName = "Channel";
            ds.Tables[2].TableName = "DayPart";
            ds.Tables[3].TableName = "Material";

            return ds;

        }
        private bool Export_TrackingGRPReport()
        {
            string strTempalte = Connection.TEMPALTE_WEEKLY_TRACKING_REPORT;
            string strSavePath = "";

            saveFileDialog1.FileName = String.Format("WeeklyTrackReport_{0:yyyyMMddHHmmss}.xlsx", DateTime.Now);
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                strSavePath = saveFileDialog1.FileName;
            }
            else return false;

            this.Cursor = Cursors.WaitCursor;

            if (File.Exists(strSavePath))
                File.Delete(strSavePath);
            File.Copy(strTempalte, strSavePath);
            //======================================
            // Create Result Excel
            //======================================
            Excel.Application ExcelObjDesc = new Excel.Application();
            Excel.Workbook theWorkbook = ExcelObjDesc.Workbooks.Open(strSavePath, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value,
                Missing.Value, Missing.Value,
                Missing.Value, Missing.Value,
                Missing.Value, Missing.Value,
                Missing.Value, Missing.Value,
                Missing.Value, Missing.Value);
            Excel.Sheets sheets = theWorkbook.Worksheets;
            Excel.Worksheet sheetSource = (Excel.Worksheet)sheets.get_Item(1);//Template
            DataTable dt = GetData_TrackingGRPReport();
            string strBB = "";
            string strProductName = "";
            int iStartRow = 7;
            int iRowCount = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                if (strBB != dr["Buying_Brief_ID"].ToString())
                {
                    if (strBB != "")
                    {
                        AddBuyingBrief(theWorkbook, strProductName, sheetSource);
                    }
                }

                if (strBB != dr["Buying_Brief_ID"].ToString())
                {

                    //===========================
                    //Add Row Header
                    //===========================
                    strBB = dr["Buying_Brief_ID"].ToString();
                    strProductName = dr["ProductName"].ToString();

                    iRowCount = 0;
                    sheetSource.Cells[1, 2] = dr["Buying_Brief_ID"];
                    sheetSource.Cells[2, 2] = dr["TargetName"];
                    sheetSource.Cells[3, 2] = String.Format("{0} - {1}", dr["CampaignStartDate"], dr["CampaignEndDate"]);

                    sheetSource.Cells[iStartRow, 1] = Convert.ToDateTime(dr["StartDayWeek"]).ToString("dd-MMM-yyyy");
                    sheetSource.Cells[iStartRow, 3] = dr["ApproveRating"];
                    sheetSource.Cells[iStartRow, 4] = dr["ActualRating"];
                    sheetSource.Cells[iStartRow, 6] = dr["PrimeRating"];

                    sheetSource.Cells[13, 2] = dr["GRPS"];
                }
                else
                {
                    //===========================
                    //Add Row Detail
                    //===========================
                    int iRow = iRowCount + iStartRow;
                    sheetSource.Cells[iRow, 1] = Convert.ToDateTime(dr["StartDayWeek"]).ToString("dd-MMM-yyyy");
                    sheetSource.Cells[iRow, 3] = dr["ApproveRating"];
                    sheetSource.Cells[iRow, 4] = dr["ActualRating"];
                    sheetSource.Cells[iRow, 6] = dr["PrimeRating"];
                }



                if (i == dt.Rows.Count - 1)
                {
                    //====================================
                    //Copy Last BB
                    //====================================
                    AddBuyingBrief(theWorkbook, dr["ProductName"].ToString(), sheetSource);
                    theWorkbook.Application.DisplayAlerts = false;
                    sheetSource.Delete();
                    theWorkbook.Application.DisplayAlerts = true;
                }

                iRowCount++;

            }
            //((Excel.Worksheet)theWorkbook.Application.ActiveWorkbook.Sheets[1]).Delete();

            theWorkbook.Save();
            ExcelObjDesc.Quit();
            return true;
        }
        private bool Export_TVSchedulSupplierReport()
        {
            string strBB = "";
            string strTarget = "";

            for (int i = 0; i < gvBuyingBrief.Rows.Count; i++)
            {
                if (gvBuyingBrief.Rows[i].Cells[0].Value.ToString() == "1")
                {
                    strBB += "," + gvBuyingBrief.Rows[i].Cells[1].Value.ToString();
                    if (strTarget == "")
                        strTarget = gvBuyingBrief.Rows[i].Cells[2].Value.ToString();
                    else
                    {
                        if (strTarget != gvBuyingBrief.Rows[i].Cells[2].Value.ToString())
                        {
                            GMessage.MessageWarning("Please select Buying Brief in the same Target.");
                            return false;
                        }
                    }
                }
            }
            strBB += ",";
            if (strBB == ",,")
            {
                GMessage.MessageWarning("Please select Buying Brief No.");
                return false;
            }


            string strTempalte = Connection.TEMPALTE_MONTHLY_TV_SCHEDULE_SUPPLIER_REPORT;
            string strSavePath = "";

            saveFileDialog1.FileName = String.Format("MontlyTVScheduleSupplierReport_{0:yyyyMMddHHmmss}.xlsx", DateTime.Now);
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                strSavePath = saveFileDialog1.FileName;
            }
            else return false;

            this.Cursor = Cursors.WaitCursor;

            if (File.Exists(strSavePath))
                File.Delete(strSavePath);
            File.Copy(strTempalte, strSavePath);
            //======================================
            // Create Result Excel
            //======================================
            Excel.Application ExcelObjDesc = new Excel.Application();
            Excel.Workbook theWorkbook = ExcelObjDesc.Workbooks.Open(strSavePath, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value,
                Missing.Value, Missing.Value,
                Missing.Value, Missing.Value,
                Missing.Value, Missing.Value,
                Missing.Value, Missing.Value,
                Missing.Value, Missing.Value);
            Excel.Sheets sheets = theWorkbook.Worksheets;
            Excel.Worksheet sheetSource = (Excel.Worksheet)sheets.get_Item(1);//Template


            


            DataSet ds = GetData_TVSchedulSupplierReport(strBB);
            DataTable dtHeader = ds.Tables["Header"];

            //======================================================
            // Header
            //======================================================
            if (dtHeader.Rows.Count == 0)
            {
                GMessage.MessageWarning("Have no data for selected buying briefs.");
                return false;
            }

            sheetSource.Cells[1, 1] = dtHeader.Rows[0]["AgencyName"].ToString();
            sheetSource.Cells[2, 3] = dtHeader.Rows[0]["ClientName"];
            sheetSource.Cells[3, 3] = dtHeader.Rows[0]["AgencyName"].ToString();
            sheetSource.Cells[4, 3] = dtHeader.Rows[0]["AgencyName"].ToString();
            sheetSource.Cells[5, 3] = dtHeader.Rows[0]["ProductName"].ToString();
            sheetSource.Cells[6, 3] = dtHeader.Rows[0]["CampaignName"].ToString();
            sheetSource.Cells[7, 3] = dtHeader.Rows[0]["BuyingBriefNo"].ToString();
            sheetSource.Cells[9, 3] = dtHeader.Rows[0]["Target"].ToString();
            sheetSource.Cells[10, 3] = dtHeader.Rows[0]["Period"].ToString();
            sheetSource.Cells[24, 3] = dtHeader.Rows[0]["ActualCPRP"].ToString();
            sheetSource.Cells[25, 3] = dtHeader.Rows[0]["Viewer"].ToString();
            sheetSource.Cells[27, 3] = dtHeader.Rows[0]["Cost30"].ToString();
            sheetSource.Cells[3, 30] = dtHeader.Rows[0]["Agency_Commission"];

            //======================================================
            // Daypart
            //======================================================
            
            DataTable dtDaypart = ds.Tables["Daypart"];
            for (int i = 0; i < dtDaypart.Rows.Count; i++)
            {
                DataRow dr = dtDaypart.Rows[i];
                //Spot
                sheetSource.Cells[TVSchedulSupplierReport_GetRowIndex(dr["MediaSubType"].ToString(), dr["DayPartName"].ToString()), 15] = dr["Spots"];
                sheetSource.Cells[TVSchedulSupplierReport_GetRowIndex(dr["MediaSubType"].ToString(), dr["DayPartName"].ToString()), 17] = dr["GRPs"];
                sheetSource.Cells[TVSchedulSupplierReport_GetRowIndex(dr["MediaSubType"].ToString(), dr["DayPartName"].ToString()), 19] = dr["Cost"];
            }
            //======================================================
            // Material
            //======================================================
            DataTable dtMaterial = ds.Tables["Material"];
            int iStartMaterialTV = 18;
            int iStartMaterialCS = 44;
            for (int i = 0; i < dtMaterial.Rows.Count; i++)
            {
                DataRow dr = dtMaterial.Rows[i];
                if (dr["MediaSubType"].ToString() == "TV")
                {
                    sheetSource.Cells[iStartMaterialTV, 13] = dr["Material_KEY"].ToString() + "(" + dr["Length"].ToString() + ")";
                    sheetSource.Cells[iStartMaterialTV, 15] = dr["Spots"];
                    sheetSource.Cells[iStartMaterialTV, 17] = dr["GRPs"];
                    sheetSource.Cells[iStartMaterialTV, 19] = dr["Cost"];
                    iStartMaterialTV++;
                }
                else if (dr["MediaSubType"].ToString() == "CS")
                {
                    sheetSource.Cells[iStartMaterialCS, 13] = dr["Material_KEY"].ToString() + "(" + dr["Length"].ToString() + ")";
                    sheetSource.Cells[iStartMaterialCS, 15] = dr["Spots"];
                    sheetSource.Cells[iStartMaterialCS, 17] = dr["GRPs"];
                    sheetSource.Cells[iStartMaterialCS, 19] = dr["Cost"];
                    iStartMaterialCS++;
                }
            }
            DataView vMaterial = new DataView(dtMaterial);
            DataTable distinctMaterial = vMaterial.ToTable(true, "Material_KEY", "MaterialName", "Length");
            string strAllMaterial = "";
            for (int i = 0; i < distinctMaterial.Rows.Count; i++)
            {
                DataRow dr = distinctMaterial.Rows[i];
                if (strAllMaterial != "")
                    strAllMaterial += ",";
                //Header
                strAllMaterial +=  string.Format("{0}:{1}({2}'')",dr["Material_KEY"].ToString() ,dr["MaterialName"].ToString(),dr["Length"].ToString() );
            }
            sheetSource.Cells[8, 3] = strAllMaterial;

            //======================================================
            // Channel
            //======================================================
            DataTable dtChannel = ds.Tables["Channel"];
            int iStartChannel = 18;
            int iTotalRow = (18 + dtChannel.Rows.Count - 1);
            for (int i = 0; i < dtChannel.Rows.Count; i++)
            {
                DataRow dr = dtChannel.Rows[i];
                if (dr["SpotType"].ToString() != "zTotal")
                {
                    sheetSource.Cells[iStartChannel, 4] = dr["Media"];
                    sheetSource.Cells[iStartChannel, 5] = dr["SpotType"].ToString();
                }
                else
                    sheetSource.Cells[iStartChannel, 5] = "Total";
                sheetSource.Cells[iStartChannel, 6] = dr["Spots"];
                sheetSource.Cells[iStartChannel, 7] = dr["Cost"];
                sheetSource.Cells[iStartChannel, 8] = string.Format("=G{0}/G{1}*100", iStartChannel, iTotalRow);
                sheetSource.Cells[iStartChannel, 9] = string.Format("=AD{0}/AD{1}*100", iStartChannel, iTotalRow);
                sheetSource.Cells[iStartChannel, 30] = dr["GRPs"];
                if (dr["Media"].ToString() == "TV"
                    || dr["Media"].ToString() == "CABLE"
                    || dr["Media"].ToString() == "GRAND TOTAL")
                {

                    Excel.Range range = sheetSource.get_Range("D" + iStartChannel, "I" + iStartChannel);
                    range.Font.Bold = true;
                    range.Interior.ColorIndex = 6;
                    range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].Color = Color.Black.ToArgb();
                    range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;

                    if (dr["Media"].ToString() == "GRAND TOTAL")
                    {
                        sheetSource.Cells[4, 30] = dr["Cost"];
                    }
                
                }
                iStartChannel++;
            }
            ArrayList arColumn = new ArrayList();
            arColumn.Add("D");
            arColumn.Add("E");
            arColumn.Add("F");
            arColumn.Add("G");
            arColumn.Add("H");
            arColumn.Add("I");

            foreach (string strColumn in arColumn)
            {
                Excel.Range range = sheetSource.get_Range(strColumn + 18, strColumn + (iStartChannel - 1));
                range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].Color = Color.Black.ToArgb();
                range.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;

            }

            Excel.Range range2 = sheetSource.get_Range("J" + 18, "J" + (iStartChannel - 1));
            range2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].Color = Color.Black.ToArgb();
            range2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
            range2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].Weight = "3";

            range2 = sheetSource.get_Range("D" + (iStartChannel - 1), "I" + (iStartChannel - 1));
            range2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Color = Color.Black.ToArgb();
            range2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
            range2.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = "3";

            //======================================================
            // Agency Comm.
            //======================================================
            sheetSource.Cells[14, 16] = string.Format("=G{0}", iTotalRow);
            theWorkbook.Save();
            ExcelObjDesc.Quit();
            return true;
        }

        private void OnCommandExport()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                bool bIsComplete = true;
                switch (m_eReport)
                {
                    case eReport.TVSchedulSupplierReport:
                        bIsComplete = Export_TVSchedulSupplierReport();
                        break;
                    case eReport.TrackingGRPReport:
                        bIsComplete = Export_TrackingGRPReport();
                        break;
                }

                if(bIsComplete)
                    GMessage.MessageInfo("Generating Report Compeleted.");
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
        
        private void MPA006_ExportReport_Load(object sender, EventArgs e)
        {
            InitControl();
            //btnExport.PerformClick();
            DevExpress.Export.ExportSettings.DefaultExportType = DevExpress.Export.ExportType.WYSIWYG;
        }
        private void cboMediaSubType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshClientList();
        }
        private void cboMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshClientList();
        }
        private void cboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshClientList();
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            OnCommandExport();
        }
        private void gvClient_SelectionChanged(object sender, EventArgs e)
        {
            if (m_eReport == eReport.TVSchedulSupplierReport)
            {
                if (gvClient.SelectedRows.Count > 0)
                {
                    DataTable dt = (DataTable)gvClient.DataSource;
                    RefreshBBList(dt.Rows[gvClient.SelectedRows[0].Index]["Client_ID"].ToString());
                }
            }
        }
        private void tvReportName_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SetEnvironment(ConvertToEReport(e.Node.Name));
        }

        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gvBuyingBrief.Rows.Count; i++)
            {
                gvBuyingBrief.Rows[i].Cells[0].Value = true;
            }
        }

        private void btnUnCheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gvBuyingBrief.Rows.Count; i++)
            {
                gvBuyingBrief.Rows[i].Cells[0].Value = false;
            }
        }

        string FileName;
        DataAccess SQLDataAccess = new DataAccess();
        private void btnImport_Click(object sender, EventArgs e)
        {
            string txtPathFile = "";
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files|*.txt;";
                    if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        txtPathFile = ofd.FileName;
                        FileName = ofd.SafeFileName;
                    }
                }
                if (txtPathFile != "")
                {
                    this.btnImport.Text = "Importing . . .";
                    this.btnImport.BackColor = Color.OrangeRed;
                    this.btnImport.ForeColor = Color.White;
                    try
                    {
                        var reader = new StreamReader(File.OpenRead(txtPathFile));

                        //"Channel","Day part","Prog Type","Programme","Day Of Week\Variables","000s per Spot"
                        List<string> listChannel = new List<string>();
                        List<string> listDayPart = new List<string>();
                        List<string> listProgType = new List<string>();
                        List<string> listProgramme = new List<string>();
                        List<string> listDayOfWeekVariables = new List<string>();
                        List<string> listPerSpot = new List<string>();

                        while (!reader.EndOfStream)
                        {
                            //var line = reader.ReadLine();
                            //var values = line.Split(',');
                            var line = reader.ReadLine();
                            string[] strOperation = new string[] { "\"," };
                            var values = line.Split(strOperation, StringSplitOptions.None);

                            string channel = values[0].Replace("\"", "");
                            listChannel.Add(channel.Replace(",", ""));

                            string DayPart = values[1].Replace("\"", "");
                            listDayPart.Add(DayPart.Replace(",", ""));

                            string ProgType = values[2].Replace("\"", "");
                            listProgType.Add(ProgType.Replace(",", ""));

                            string Programme = values[3].Replace("\"", "");
                            listProgramme.Add(Programme.Replace(",", ""));

                            string DayOfWeekVariables = values[4].Replace("\"", "");
                            listDayOfWeekVariables.Add(DayOfWeekVariables.Replace(",", ""));

                            string PerSpot = values[5].Replace("\"", "");
                            listPerSpot.Add(PerSpot.Replace(",", ""));

                            //listDayPart.Add(values[1].Replace("\", ,", ""));

                            //listProgType.Add(values[2].Replace("\", ,", ""));

                            //listProgramme.Add(values[3].Replace("\", ,", ""));

                            //listDayOfWeekVariables.Add(values[4].Replace("\", ,", ""));

                            //listPerSpot.Add(values[5].Replace("\", ,", ""));


                            Application.DoEvents();


                        }


                        int retCheck = SQLDataAccess.ImportCSVConnString(listChannel, listDayPart, listProgType, listProgramme, listDayOfWeekVariables, listPerSpot, FileName);
                        if (retCheck == 1)
                        {
                            MessageBox.Show("Import Success...", "Import CSV", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.btnImport.Text = "Import Complete...";
                            this.btnImport.BackColor = Color.GreenYellow;
                            this.btnImport.ForeColor = Color.Black;
                            this.txtTargetName.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("Import fail . . ."
                                        + Environment.NewLine
                                        + "Please input data in this format"
                                        + Environment.NewLine
                                        + "\"Channel\",\"Day part\",\"Prog Type\",\"Programme\","
                                        + Environment.NewLine
                                        + "\"Day Of Week\\Variables\",\"000s per Spot\",",
                                        "Import CSV",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                            this.btnImport.Text = "Import";
                            this.btnImport.BackColor = SystemColors.Control;
                            this.btnImport.ForeColor = Color.Black;
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Import fail . . ." 
                                        + Environment.NewLine 
                                        + "Please input data in this format"
                                        + Environment.NewLine 
                                        + "\"Channel\",\"Day part\",\"Prog Type\",\"Programme\",\"Day Of Week\\Variables\",\"000s per Spot\",", 
                                        "Import CSV", 
                                        MessageBoxButtons.OK, 
                                        MessageBoxIcon.Warning);
                        this.btnImport.Text = "Import";
                        this.btnImport.BackColor = SystemColors.Control;
                        this.btnImport.ForeColor = Color.Black;
                        throw;
                    }

                }
                else
                {
                    MessageBox.Show("Please choose file to import.", "Import CSV", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                #region exception
                if (ex.InnerException != null)
                {
                    MessageBox.Show("System Error :" + ex.InnerException.Message.ToString() + "\r\n" + ex.InnerException.StackTrace.ToString());
                }
                else
                {
                    MessageBox.Show("System Error :" + ex.Message.ToString() + "\r\n" + ex.StackTrace.ToString());
                }
                #endregion
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            StartThread = new System.Threading.Thread(fnStartThread);
            StartThread.Start();

            try
            {
                string fileTest = @"C:\TestExcel\result.xlsx";
                if (File.Exists(fileTest))
                {
                    File.Delete(fileTest);
                }

                Microsoft.Office.Interop.Excel.Application oApp;
                Microsoft.Office.Interop.Excel.Worksheet oSheet;
                Microsoft.Office.Interop.Excel.Workbook oBook;

                oApp = new Microsoft.Office.Interop.Excel.Application();
                oBook = oApp.Workbooks.Add();
                oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oBook.Worksheets.get_Item(1);

                DataTable CSVData = SQLDataAccess.GetCSV();

                for (int col = 0; col < CSVData.Rows.Count - 1; col++)
                {
                    string sheetName = CSVData.Rows[col].ItemArray[0].ToString();
                    if (!CSVData.Rows[col + 1].ItemArray[0].ToString().Equals(sheetName))
                    {

                        int r = 1;
                        oSheet = oApp.Worksheets[1];
                        oSheet.Tab.Color = RandomColorName();
                        oSheet.Name = CSVData.Rows[col + 1].ItemArray[0].ToString();
                        Microsoft.Office.Interop.Excel.Range oRange;

                        DataTable GetItemByChannel = SQLDataAccess.GetCSVByChannel(CSVData.Rows[col + 1].ItemArray[0].ToString());

                        //row 1
                        oSheet.Cells[1, 1] = "Target";
                        oSheet.Cells[1, 2] = CSVData.Columns[0].ColumnName;
                        oRange = oSheet.get_Range("A1", "B1");
                        oRange.Rows.AutoFit();
                        oRange.Columns.AutoFit();
                        oRange.Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                        oRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                        //row 2
                        oSheet.Cells[2, 1] = this.txtTargetName.Text;
                        oSheet.Cells[2, 2] = CSVData.Rows[col + 1].ItemArray[0].ToString();
                        oRange = oSheet.get_Range("A2", "B2");
                        oRange.Rows.AutoFit();
                        oRange.Columns.AutoFit();
                        oRange.Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);

                        //row 3
                        oSheet.Cells[3, 1] = "Day Part";
                        oSheet.Cells[3, 2] = "Programme\\Variables";
                        oSheet.Cells[3, 3] = "000s per Spot";
                        oRange = oSheet.get_Range("A3", "B3");
                        oRange.Rows.AutoFit();
                        oRange.Columns.AutoFit();
                        oRange.Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                        oRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                        oRange = oSheet.get_Range("C3");
                        oRange.Rows.AutoFit();
                        oRange.Columns.AutoFit();
                        oRange.Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                        oRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                        int ii = 1;
                        string tmpDayPart = "";
                        for (int i = 4; i < GetItemByChannel.Rows.Count + 3; i++)
                        {
                            if (!GetItemByChannel.Rows[ii].ItemArray[0].ToString().Equals(tmpDayPart))
                            {
                                tmpDayPart = GetItemByChannel.Rows[ii].ItemArray[0].ToString();
                                oSheet.Cells[i, 1] = GetItemByChannel.Rows[ii].ItemArray[0].ToString();
                            }
                            oSheet.Cells[i, 2] = GetItemByChannel.Rows[ii].ItemArray[1].ToString();
                            oSheet.Cells[i, 3] = GetItemByChannel.Rows[ii].ItemArray[2].ToString();

                            oRange = oSheet.get_Range("A" + i, "B3" + i);
                            oRange.Rows.AutoFit();
                            oRange.Columns.AutoFit();
                            oRange.Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                            oRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                            ii++;
                        }

                        oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oBook.Worksheets.Add();
                        r++;


                        //int r = 1;
                        //oSheet = oApp.Worksheets[1];
                        //oSheet.Name = CSVData.Rows[col + 1].ItemArray[0].ToString();
                        //oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oBook.Worksheets.Add();
                        //r++;
                    }
                    else
                    {
                        if (oApp.Application.Sheets.Count < 2)
                        {
                            int r = 1;
                            oSheet = oApp.Worksheets[1];
                            oSheet.Tab.Color = RandomColorName();
                            oSheet.Name = CSVData.Rows[col].ItemArray[0].ToString();
                            Microsoft.Office.Interop.Excel.Range oRange;

                            DataTable GetItemByChannel = SQLDataAccess.GetCSVByChannel(CSVData.Rows[col + 1].ItemArray[0].ToString());

                            //row 1
                            oSheet.Cells[1, 1] = "Target";
                            oSheet.Cells[1, 2] = CSVData.Columns[col].ColumnName;
                            oRange = oSheet.get_Range("A1", "B1");
                            oRange.Rows.AutoFit();
                            oRange.Columns.AutoFit();
                            oRange.Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                            oRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                            //row 2
                            oSheet.Cells[2, 1] = this.txtTargetName.Text;
                            oSheet.Cells[2, 2] = CSVData.Rows[col + 1].ItemArray[0].ToString();
                            oRange = oSheet.get_Range("A2", "B2");
                            oRange.Rows.AutoFit();
                            oRange.Columns.AutoFit();
                            oRange.Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);

                            //row 3
                            oSheet.Cells[3, 1] = "Day Part";
                            oSheet.Cells[3, 2] = "Programme\\Variables";
                            oSheet.Cells[3, 3] = "000s per Spot";
                            oRange = oSheet.get_Range("A3", "B3");
                            oRange.Rows.AutoFit();
                            oRange.Columns.AutoFit();
                            oRange.Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                            oRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                            oRange = oSheet.get_Range("C3");
                            oRange.Rows.AutoFit();
                            oRange.Columns.AutoFit();
                            oRange.Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                            oRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                            int ii = 1;
                            string tmpDayPart = "";
                            for (int i = 4; i < GetItemByChannel.Rows.Count + 3; i++)
                            {

                                if (!GetItemByChannel.Rows[ii].ItemArray[0].ToString().Equals(tmpDayPart))
                                {
                                    tmpDayPart = GetItemByChannel.Rows[ii].ItemArray[0].ToString();
                                    oSheet.Cells[i, 1] = GetItemByChannel.Rows[ii].ItemArray[0].ToString();
                                }

                                oSheet.Cells[i, 2] = GetItemByChannel.Rows[ii].ItemArray[1].ToString();
                                oSheet.Cells[i, 3] = GetItemByChannel.Rows[ii].ItemArray[2].ToString();

                                oRange = oSheet.get_Range("A" + i, "B3" + i);
                                oRange.Rows.AutoFit();
                                oRange.Columns.AutoFit();
                                oRange.Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                                oRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                                ii++;
                            }

                            oSheet = (Excel.Worksheet)oBook.Worksheets.Add();
                            r++;
                        }
                    }
                }

                oSheet.Delete();

                //for (int col = 0; col < CSVData.Columns.Count; col++)
                //    oSheet.Cells[r, col + 1] = CSVData.Columns[col].ColumnName;
                //r++;

                //for (int row = 0; row < CSVData.Rows.Count; row++)
                //{
                //    for (int col = 0; col < CSVData.Columns.Count; col++)
                //        oSheet.Cells[r, col + 1] = CSVData.Rows[row][col].ToString();
                //    r++;
                //}

                //Microsoft.Office.Interop.Excel.Range oRange = oSheet.Range[oSheet.Cells[1, 1], oSheet.Cells[CSVData.Rows.Count, CSVData.Columns.Count]];

                //if (oApp.Application.Sheets.Count < 2)
                //{
                //    oSheet.Name = "Data";
                //    oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oBook.Worksheets.Add();
                //}
                //else
                //{
                //    oSheet = oApp.Worksheets[2];
                //}
                //oSheet.Name = "Pivot";

                //Microsoft.Office.Interop.Excel.Range oRange2 = oSheet.Cells[1, 1];

                //Microsoft.Office.Interop.Excel.PivotCache oPivotCache = (Microsoft.Office.Interop.Excel.PivotCache)oBook.PivotCaches().Add(Microsoft.Office.Interop.Excel.XlPivotTableSourceType.xlDatabase, oRange);
                //Microsoft.Office.Interop.Excel.PivotTable oPivotTable = (Microsoft.Office.Interop.Excel.PivotTable)oSheet.PivotTables().Add(PivotCache: oPivotCache, TableDestination: oRange2, TableName: "Vendor Name");

                //Microsoft.Office.Interop.Excel.PivotField oPivotField = (Microsoft.Office.Interop.Excel.PivotField)oPivotTable.PivotFields("Vendor Name");
                //oPivotField.Orientation = Microsoft.Office.Interop.Excel.XlPivotFieldOrientation.xlDataField;
                //oPivotField.Function = Microsoft.Office.Interop.Excel.XlConsolidationFunction.xlCount;
                //oPivotField.Name = " Vendor Name";

                //oBook.SaveAs(fileTest);
                oApp.Visible = true;
                //oBook.Close();
                //oApp.Quit();

                //oBook.Open(fileTest);
                StopThread = new System.Threading.Thread(fnStopThread);
                StartThread.Abort();
                StopThread.Start();
                StopThread.Abort();

            }
            catch (Exception exHandle)
            {
                StopThread = new System.Threading.Thread(fnStopThread);
                StartThread.Abort();
                StopThread.Start();
                StopThread.Abort();
                MessageBox.Show("Exception: " + exHandle.Message, "Close Excel File", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                this.Show();
                //foreach (Process process in Process.GetProcessesByName("Excel"))
                //    process.Kill();
            }
        }

        private void fnStartThread()
        {
            ShowLoadingScreen.ShowDialog();
        }

        private void fnStopThread()
        {
            ShowLoadingScreen.Close();
        }

        private Color RandomColorName()
        {
            Random randomGen = new Random();
            KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            KnownColor randomColorName = names[randomGen.Next(names.Length)];
            Color randomColor = Color.FromKnownColor(randomColorName);
            return randomColor;
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            if (this.txtTargetName.Text != "")
            {
                this.btnExportCSV.Enabled = true;
            }
            else
            {
                this.btnExportCSV.Enabled = false;
            }
        }
    }
}