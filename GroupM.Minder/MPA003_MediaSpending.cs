using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using GRM.UTL;
using System.Configuration;

namespace GRM.MPA
{
    public partial class MPA003_MediaSpending : System.Windows.Forms.Form
    {
        string m_strConnection = UIConstant.ConnectionStringMinder;
        string m_strDateFrom = "yyyyMMdd";
        string m_strDateTo = "yyyyMMdd";
        string m_strMedaiSubType = "TV";
        DataGridView m_gvClient = new DataGridView();
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

        enum eCol { 
            LateBrief
            ,ClientName
            ,TargetName
            ,DayPartName
            ,Version
            ,Buying_Brief_ID
            ,CampaignStartDate
            ,CampaignEndDate
            ,MonthWeek
            ,ShowDate
            ,OwnDayPart
            ,DayOfWeek
            ,Day
            ,Month
            ,Year
            ,StartTime
            ,Length
            ,Media
            ,ProductName
            ,BookingProgramName
            ,MatchedProgramName
            ,ProgramType
            ,CopyLineName
            ,SpotType
            ,MaterialName
            ,CampaignName
            ,MediaSubType
            ,VendorName
            ,MasterVendorName
            ,PIB
            ,Adex_Cost
            ,Cost
            ,Spots
            ,Rating
            ,Rating30sec
            ,CPRP
        }
        public MPA003_MediaSpending(string strUserName,string strPassword)
        {
            this.UserName = strUserName;
            this.Password = strPassword;
            InitializeComponent();
            //if (lbCondition.Text == "NONE")
            //{
            //    MPA002_SearchCondition frmSearch = new MPA002_SearchCondition();
            //    if (frmSearch.ShowDialog() == DialogResult.OK)
            //    {
            //        lbCondition.Text = frmSearch.DateFrom.ToString("dd MMM yyyy") + " - " + frmSearch.DateTo.ToString("dd MMM yyyy");

            //        m_strDateFrom = frmSearch.DateFrom.ToString("yyyyMMdd");
            //        m_strDateTo = frmSearch.DateTo.ToString("yyyyMMdd");
            //        m_strMedaiSubType = frmSearch.MediaSubType;
            //        DataLoading();
            //    }
            //    else
            //    {
            //        btnExport.Enabled = false;
            //        btnTotal.Enabled = false;
            //    }
            //}     
        }

        private DataSet GetSource()
        {
            try
            {

            
            // initialize the dataset
            DataSet ds = new DataSet("Unbound_Cube_Data");
            string strSQL = "";

            SqlConnection conn = new SqlConnection(m_strConnection);
            SqlDataAdapter da = null;

            DataTable dt = new DataTable();
            dt.Columns.Add("Client_ID");
            dt.Columns.Add("ClientName");
            for (int i = 0; i < m_gvClient.SelectedRows.Count; i++)
            {
                DataRow dr = ((DataRowView)((DataGridViewRow)m_gvClient.SelectedRows[i]).DataBoundItem).Row;
                DataRow drXML = dt.NewRow();
                drXML.ItemArray = dr.ItemArray;
                dt.Rows.Add(drXML);
            }
            
            strSQL = string.Format(@"
DECLARE @xml XML
SET @xml = '{0}'
SELECT  
       Tbl.Col.value('Client_ID[1]', 'varchar(20)') Client_ID,  
       Tbl.Col.value('ClientName[1]', 'varchar(200)') English_Name
INTO #Client
FROM   @xml.nodes('//row') Tbl(Col)  


--DECLARE @CreateBy VARCHAR(100)
--DECLARE @U VARCHAR(100)
--DECLARE @P VARCHAR(100)
--DECLARE @S VARCHAR(100)
--DECLARE @E VARCHAR(100)

--SET @CreateBy = 'pariwatk'
--SET @U = 'pariwatk'
--SET @P = 'Fank7813'
--SET @S = '20110601'
--SET @E = '20110601'

SELECT * FROM 
(
SELECT 
	CASE Buying_Brief.Late_Brief WHEN 0 THEN 'NO' ELSE 'YES' END LateBrief
	, Client.English_Name ClientName
	, Target.Short_Name TargetName
	, dbo.GetDayPart(Spot_Plan.Start_Time,Spot_Plan.End_Time,Spot_Plan.Show_Date) DayPartName
	, Spot_Plan.Buying_Brief_ID
	, Buying_Brief.Campaign_Start_Date AS CampaignStartDate
	, Buying_Brief.Campaign_End_Date AS CampaignEndDate
	, CASE LEN(Spot_Plan.Version) WHEN 2 THEN '1-APPROVE (PLANNED)' ELSE '2-LATEST (EXECUTING)' END [Version]
	, Spot_Plan.Length
	, RIGHT('0'+CONVERT(VARCHAR(2),MONTH(Spot_Plan.Show_Date)),2)
		+ RIGHT('0'+CONVERT(VARCHAR(2),dbo.fn_ISOWK(Spot_Plan.Show_Date)),2) MonthWeek
	, Spot_Plan.Show_Date [ShowDate]
	, dbo.GetOwnDayPart(Spot_Plan.Start_Time,Spot_Plan.End_Time,Spot_Plan.Show_Date,@CreateBy) OwnDayPart
	, dbo.fn_DayofWeek(Spot_Plan.Show_Date) [DayOfWeek]
	, DAY(CONVERT(DATETIME,Spot_Plan.Show_Date)) [Day]
	, MONTH(CONVERT(DATETIME,Spot_Plan.Show_Date)) [Month]
	, YEAR(CONVERT(DATETIME,Spot_Plan.Show_Date)) [Year]
	, Spot_Plan.Start_Time + '00' StartTime
	, Media.English_Name Media
	, Product.English_Name ProductName
	, Spot_Plan.Program BookingProgramName
	, '' MatchedProgramName
    , isnull(Program_Type.Short_Name,'UNKNOWN') ProgramType
	, '' CopyLineName
	, Spot_Plan.Package SpotType
	, Material.Thai_Name MaterialName
	, Buying_Brief.Description CampaignName
	, Buying_Brief.Media_Sub_Type MediaSubType
	, Media_Vendor.English_Name VendorName
	, MasterVendor.English_Name MasterVendorName
	, NULL AS PIB
	, 0 AS Adex_Cost
	, SUM(Spot_Plan.Spots) Spots
	, SUM(Spot_Plan.Net_Cost) Cost
	, SUM(Spot_Plan.Rating) Rating
	, SUM((Spot_Plan.Rating * Spot_Plan.Length)/30 ) Rating30sec
	, CASE SUM((Spot_Plan.Rating * Spot_Plan.Length)/30 )  WHEN 0 THEN 0 ELSE
	SUM(Spot_Plan.Net_Cost) / SUM((Spot_Plan.Rating * Spot_Plan.Length)/30 ) END CPRP 
FROM Spot_Plan with(nolock)
	INNER JOIN Buying_Brief		
		ON Spot_Plan.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
	INNER JOIN #Client UserPermission
		ON UserPermission.Client_ID = Buying_Brief.Client_ID  collate SQL_Latin1_General_CP1_CI_AS
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
	INNER JOIN Media_Vendor MasterVendor
		ON Media_Vendor.Master_Group = MasterVendor.Media_Vendor_ID 
	INNER JOIN Media
		ON Media.Media_ID = Spot_Plan.Media_ID
    LEFT JOIN Program_Type
        ON Program_Type.Program_Type = Spot_Plan.Program_Type
WHERE 
	Buying_Brief.Media_Sub_Type IN ('" + m_strMedaiSubType.Replace(",", "','") + @"')
	AND Spot_Plan.Show_Date BETWEEN @S AND @E
	AND Spot_Plan.Status > 0
	--AND (Spot_Plan.Rating * Spot_Plan.Length)/30  <> 0
	--AND Buying_Brief.Client_ID ='20LORMOB'
GROUP BY 
	 Target.Short_Name
	, Media.English_Name
	, Product.English_Name 
	, dbo.GetOwnDayPart(Spot_Plan.Start_Time,Spot_Plan.End_Time,Spot_Plan.Show_Date,@CreateBy) 
	, dbo.GetDayPart(Spot_Plan.Start_Time,Spot_Plan.End_Time,Spot_Plan.Show_Date) 
	, Client.English_Name
	, Spot_Plan.Program 
    , Program_Type.Short_Name
	, Spot_Plan.Package 
	, Material.Thai_Name 
	, Buying_Brief.Description
	, Buying_Brief.Media_Sub_Type
	, Media_Vendor.English_Name 
	, MasterVendor.English_Name 
	, Buying_Brief.Late_Brief
	, Spot_Plan.Buying_Brief_ID
	, Buying_Brief.Campaign_Start_Date
	, Buying_Brief.Campaign_End_Date
	, Spot_Plan.Version
	, Spot_Plan.Length
	, RIGHT('0'+CONVERT(VARCHAR(2),MONTH(Spot_Plan.Show_Date)),2)  
		+ RIGHT('0'+CONVERT(VARCHAR(2),dbo.fn_ISOWK(Spot_Plan.Show_Date)),2) 
	, Spot_Plan.Show_Date  
	, dbo.fn_DayofWeek(Spot_Plan.Show_Date)  
	, DAY(CONVERT(DATETIME,Spot_Plan.Show_Date))  
	, MONTH(CONVERT(DATETIME,Spot_Plan.Show_Date))  
	, YEAR(CONVERT(DATETIME,Spot_Plan.Show_Date))  
	, Spot_Plan.Start_Time  

UNION

SELECT 
	CASE Buying_Brief.Late_Brief WHEN 0 THEN 'NO' ELSE 'YES' END LateBrief
	, Client.English_Name ClientName
	, Target.Short_Name TargetName
	, dbo.GetDayPart(SpotsMatch.Start_Time,SpotsMatch.End_Time,SpotsMatch.Show_Date) DayPartName
	, SpotsMatch.Buying_Brief_ID
	, Buying_Brief.Campaign_Start_Date AS CampaignStartDate
	, Buying_Brief.Campaign_End_Date AS CampaignEndDate
	, '3-ACTUAL (POST)' [Version]
	, SpotsMatch.Length
	, RIGHT('0'+CONVERT(VARCHAR(2),MONTH(SpotsMatch.Show_Date)),2)
		+ RIGHT('0'+CONVERT(VARCHAR(2),dbo.fn_ISOWK(SpotsMatch.Show_Date)),2) MonthWeek
	, SpotsMatch.Show_Date [ShowDate]
	, dbo.GetOwnDayPart(SpotsMatch.Start_Time,SpotsMatch.End_Time,SpotsMatch.Show_Date,@CreateBy) OwnDayPart
	, dbo.fn_DayofWeek(SpotsMatch.Show_Date) [DayOfWeek]
	, DAY(CONVERT(DATETIME,SpotsMatch.Show_Date)) [Day]
	, MONTH(CONVERT(DATETIME,SpotsMatch.Show_Date)) [Month]
	, YEAR(CONVERT(DATETIME,SpotsMatch.Show_Date)) [Year]
	, SpotsMatch.Start_Time StartTime
	, Media.English_Name Media
	, Product.English_Name ProductName
	, SpotsMatch.BookingProgramName
	, SpotsMatch.MatchedProgramName
    , SpotsMatch.ProgramType
	, SpotsMatch.CopyLineName
	, SpotsMatch.SpotType
	, SpotsMatch.MaterialName
	, Buying_Brief.Description CampaignName
	, Buying_Brief.Media_Sub_Type MediaSubType
	, SpotsMatch.VendorName
	, SpotsMatch.MasterVendorName
	, PIB
	, SUM(Adex_Cost) AdexCost
	, SUM(SpotsMatch.Spots) Spots
	, SUM(SpotsMatch.Net_Cost) Cost
	, SUM(SpotsMatch.Actual_Rating) Rating
	, SUM((SpotsMatch.Actual_Rating * SpotsMatch.Length)/30 ) Rating30sec
	, 
	CASE SUM((SpotsMatch.Actual_Rating * SpotsMatch.Length)/30 ) WHEN 0 THEN 0 ELSE
	 SUM(SpotsMatch.Net_Cost) / SUM((SpotsMatch.Actual_Rating * SpotsMatch.Length)/30 ) END CPRP  
FROM (
		SELECT 
			Spots_Match.Buying_Brief_ID
			,Spot_Plan.Net_Cost
			,Spots_Match.Length
			,Spot_Plan.Media_ID
			,Spots_Match.SP_Show_Date Show_Date
			,Spots_Match.Actual_Time Start_Time
			,Spots_Match.Actual_Time End_Time
			,Spots_Match.Actual_Rating
			,Spot_Plan.Spots
			,Spot_Plan.Program BookingProgramName
            ,Spots_Match.[Program_Name] MatchedProgramName
            ,isnull(Program_Type.Short_Name,'UNKNOWN') ProgramType
			,Spots_Match.PDNO CopyLineName
			,Spot_Plan.Package SpotType
			,Material.Thai_Name MaterialName
			,Media_Vendor.English_Name VendorName
			,MasterVendor.English_Name MasterVendorName
			,CASE WHEN Spots_Match.PIB = 1 
				THEN 'First' 
				ELSE CASE WHEN Spots_Match.TSB - Spots_Match.PIB = 0
					THEN 'Last'
					ELSE CASE WHEN Spots_Match.PIB = 2
						THEN 'Second'
						ELSE CASE WHEN Spots_Match.PIB = 3
							THEN 'Third'
							ELSE CASE WHEN Spots_Match.TSB - Spots_Match.PIB = 1
								THEN 'Second to Last'
								ELSE CASE WHEN Spots_Match.TSB - Spots_Match.PIB = 2
									THEN 'Third to Last'
									ELSE 'Middle'
									END
								END						
							END				 
						END 			
					END
				END AS  [PIB]
			,Adex_Cost
		FROM Spot_Plan   with(nolock)
		INNER JOIN Spots_Match   with(nolock)
			ON (Spots_Match.Buying_Brief_ID = Spot_Plan.Buying_Brief_ID) 
				AND (Spots_Match.Market_ID = Spot_Plan.Market_ID) 
				AND (Spots_Match.[Version] = Spot_Plan.[Version]) 
				AND (Spots_Match.Item = Spot_Plan.Item) 
				AND (Spots_Match.SP_Show_Date = Spot_Plan.Show_Date) 
				AND (Spots_Match.ID = Spot_Plan.ID) 
				AND (Spots_Match.[Status] = Spot_Plan.[Status])
		INNER JOIN Material
			ON Material.Material_ID = Spot_Plan.Material_ID
		INNER JOIN Media_Vendor
			ON Media_Vendor.Media_Vendor_ID = Spot_Plan.Media_Vendor_ID
		INNER JOIN Media_Vendor MasterVendor
			ON Media_Vendor.Master_Group = MasterVendor.Media_Vendor_ID 
		INNER JOIN Buying_Brief		
			ON Spots_Match.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
		INNER JOIN #Client UserPermission
			ON UserPermission.Client_ID = Buying_Brief.Client_ID  collate SQL_Latin1_General_CP1_CI_AS
        LEFT JOIN Program_Type
            ON Program_Type.Program_Type = Spot_Plan.Program_Type
		WHERE Spots_Match.Actual_Rating IS NOT NULL AND  Spots_Match.Flag_Bonus = 0
		UNION ALL		
		SELECT 
			Spots_Match.Buying_Brief_ID
			, 0 Net_Cost
			,Spots_Match.Length
			,Spots_Match.Media_ID
			,Spots_Match.Show_Date
			,Spots_Match.Actual_Time Start_Time
			,Spots_Match.Actual_Time End_Time
			,Spots_Match.Actual_Rating 
			, 1 Spots
			,Spots_Match.[Program_Name] BookingProgramName
            ,Spots_Match.[Program_Name] MatchedProgramName
            ,'BONUS' ProgramType
			,Spots_Match.PDNO CopyLineName
			,'Unknown Bonus' SpotType
			,PDNO MaterialName
			,Media_Vendor.English_Name VendorName
			,MasterVendor.English_Name MasterVendorName
			,CASE WHEN Spots_Match.PIB = 1 
				THEN 'First' 
				ELSE CASE WHEN Spots_Match.TSB - Spots_Match.PIB = 0
					THEN 'Last'
					ELSE CASE WHEN Spots_Match.PIB = 2
						THEN 'Second'
						ELSE CASE WHEN Spots_Match.PIB = 3
							THEN 'Third'
							ELSE CASE WHEN Spots_Match.TSB - Spots_Match.PIB = 1
								THEN 'Second to Last'
								ELSE CASE WHEN Spots_Match.TSB - Spots_Match.PIB = 2
									THEN 'Third to Last'
									ELSE 'Middle'
									END
								END						
							END				 
						END 			
					END
				END AS  [PIB]
			,Adex_Cost
		FROM Spots_Match  with(nolock)
			INNER JOIN Media_Vendor
				ON Media_Vendor.Media_Vendor_ID = Spots_Match.Media_Vendor
			INNER JOIN Media_Vendor MasterVendor
				ON Media_Vendor.Master_Group = MasterVendor.Media_Vendor_ID 	
			INNER JOIN Buying_Brief		
				ON Spots_Match.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
			INNER JOIN #Client UserPermission
				ON UserPermission.Client_ID = Buying_Brief.Client_ID  collate SQL_Latin1_General_CP1_CI_AS
		WHERE Spots_Match.Flag_Bonus = 1 AND Spots_Match.Actual_Rating IS NOT NULL
	
		) SpotsMatch 
	INNER JOIN Buying_Brief		
		ON SpotsMatch.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
	INNER JOIN #Client UserPermission
		ON UserPermission.Client_ID = Buying_Brief.Client_ID   collate SQL_Latin1_General_CP1_CI_AS
	INNER JOIN Target 
		ON Buying_Brief.Primary_Target = Target.Target_ID
	INNER JOIN Client
		ON Buying_Brief.Client_ID = Client.Client_ID
	INNER JOIN Product
		ON Buying_Brief.Product_ID = Product.Product_ID
	INNER JOIN Media
		ON Media.Media_ID = SpotsMatch.Media_ID
WHERE 
	Buying_Brief.Media_Sub_Type IN ('" + m_strMedaiSubType.Replace(",", "','") + @"')
	AND SpotsMatch.Show_Date BETWEEN @S AND @E
	AND SpotsMatch.Actual_Rating IS NOT NULL
	--AND (SpotsMatch.Actual_Rating * SpotsMatch.Length)/30   <> 0
	--AND Buying_Brief.Client_ID ='20LORMOB'
	--AND SpotsMatch.Buying_Brief_ID = '2011050387'
GROUP BY 
	 Target.Short_Name
	, Media.English_Name
	, Product.English_Name 
	, dbo.GetOwnDayPart(SpotsMatch.Start_Time,SpotsMatch.End_Time,SpotsMatch.Show_Date,@CreateBy)
	, dbo.GetDayPart(SpotsMatch.Start_Time,SpotsMatch.End_Time,SpotsMatch.Show_Date)
	, Client.English_Name
	, SpotsMatch.BookingProgramName
	, SpotsMatch.MatchedProgramName
	, SpotsMatch.ProgramType
	, SpotsMatch.CopyLineName
	, SpotsMatch.SpotType
	, SpotsMatch.MaterialName
	, Buying_Brief.Description
	, Buying_Brief.Media_Sub_Type
	, SpotsMatch.VendorName
	, SpotsMatch.MasterVendorName
	, Buying_Brief.Late_Brief
	, SpotsMatch.Buying_Brief_ID
	, Buying_Brief.Campaign_Start_Date
	, Buying_Brief.Campaign_End_Date
	, SpotsMatch.Length
	, RIGHT('0'+CONVERT(VARCHAR(2),MONTH(SpotsMatch.Show_Date)),2)
		+ RIGHT('0'+CONVERT(VARCHAR(2),dbo.fn_ISOWK(SpotsMatch.Show_Date)),2) 
	, SpotsMatch.Show_Date  
	, dbo.fn_DayofWeek(SpotsMatch.Show_Date)  
	, DAY(CONVERT(DATETIME,SpotsMatch.Show_Date)) 
	, MONTH(CONVERT(DATETIME,SpotsMatch.Show_Date)) 
	, YEAR(CONVERT(DATETIME,SpotsMatch.Show_Date))  
	, SpotsMatch.Start_Time 
	, PIB
) rec
--ORDER BY Buying_Brief_ID
--where Buying_Brief_ID = '2012070829'


            ", GRM.UTL.XMLUtil.ConvertDataTableToXML(dt));
            da = new SqlDataAdapter(strSQL, conn);
            da.SelectCommand.Parameters.Add("@CreateBy", SqlDbType.VarChar).Value = UserName;
            da.SelectCommand.Parameters.Add("@U", SqlDbType.VarChar).Value = UserName.Replace(".", "");
            da.SelectCommand.Parameters.Add("@P", SqlDbType.VarChar).Value = Password;
            da.SelectCommand.Parameters.Add("@S", SqlDbType.VarChar).Value = m_strDateFrom;
            da.SelectCommand.Parameters.Add("@E", SqlDbType.VarChar).Value = m_strDateTo;
            da.SelectCommand.CommandTimeout = 0;
            da.Fill(ds);
            return ds;
        }
        catch (Exception ex)
        {
            GMessage.MessageError(ex);
            return null;
        }


        }
        private void AddFieldCPRP(DynamiCubeLib.Field fieldDetail, string strSourceName, string strName, DynamiCubeLib.OrientationConstants oOriCon)
        {
            fieldDetail = axDetail.Fields.Add(strSourceName, strName, oOriCon);
            fieldDetail.GroupFooterCaption = "Total";
            fieldDetail.set_GroupFooterType("sumCPRP", DynamiCubeLib.GroupFooterTypes.DCFCalculated);
            fieldDetail.set_GroupFooterExpression("sumCPRP", "sumCost/sumRating30sec");
        }
        private void DataLoading()
        {
            //===========================================================================
            // Set Column
            //===========================================================================
            //axDetail.Fields.DeleteAll();
            // get ready to populate the cube
            axDetail.Fields.DeleteAll();
            axDetail.DCConnectType = DynamiCubeLib.DCConnectTypes.DCCT_UNBOUND;
            DynamiCubeLib.Field fieldDetail = null;

            //=====================================================================================================
            // DC Row
            //=====================================================================================================
            
            AddFieldCPRP(fieldDetail, "LateBrief", "LateBrief", DynamiCubeLib.OrientationConstants.DCRow);
            AddFieldCPRP(fieldDetail, "ClientName", "ClientName", DynamiCubeLib.OrientationConstants.DCRow);
            AddFieldCPRP(fieldDetail, "TargetName", "TargetName", DynamiCubeLib.OrientationConstants.DCRow);
            AddFieldCPRP(fieldDetail, "DayPartName", "DayPartName", DynamiCubeLib.OrientationConstants.DCRow);

            //=====================================================================================================
            // DC Page
            //=====================================================================================================

            AddFieldCPRP(fieldDetail, "Buying_Brief_ID", "BuyingBriefID", DynamiCubeLib.OrientationConstants.DCPage);            
            AddFieldCPRP(fieldDetail, "CampaignStartDate", "CampaignStartDate", DynamiCubeLib.OrientationConstants.DCPage);            
            AddFieldCPRP(fieldDetail, "CampaignEndDate", "CampaignEndDate", DynamiCubeLib.OrientationConstants.DCPage);
            AddFieldCPRP(fieldDetail, "MonthWeek", "MonthWeek", DynamiCubeLib.OrientationConstants.DCPage);
            AddFieldCPRP(fieldDetail, "ShowDate", "ShowDate", DynamiCubeLib.OrientationConstants.DCPage);
            AddFieldCPRP(fieldDetail, "OwnDayPart", "OwnDayPart", DynamiCubeLib.OrientationConstants.DCPage);
            AddFieldCPRP(fieldDetail, "DayOfWeek", "DayOfWeek", DynamiCubeLib.OrientationConstants.DCPage);
            AddFieldCPRP(fieldDetail, "Day", "Day", DynamiCubeLib.OrientationConstants.DCPage);
            AddFieldCPRP(fieldDetail, "Month", "Month", DynamiCubeLib.OrientationConstants.DCPage);
            AddFieldCPRP(fieldDetail, "Year", "Year", DynamiCubeLib.OrientationConstants.DCPage);
            AddFieldCPRP(fieldDetail, "StartTime", "StartTime", DynamiCubeLib.OrientationConstants.DCPage);
            AddFieldCPRP(fieldDetail, "Length", "Length", DynamiCubeLib.OrientationConstants.DCPage);
            AddFieldCPRP(fieldDetail, "Media", "Media", DynamiCubeLib.OrientationConstants.DCPage);
            AddFieldCPRP(fieldDetail, "ProductName", "ProductName", DynamiCubeLib.OrientationConstants.DCPage);
            AddFieldCPRP(fieldDetail, "BookingProgramName", "BookingProgramName", DynamiCubeLib.OrientationConstants.DCPage);
            AddFieldCPRP(fieldDetail, "MatchedProgramName", "MatchedProgramName", DynamiCubeLib.OrientationConstants.DCPage);
            AddFieldCPRP(fieldDetail, "ProgramType", "ProgramType", DynamiCubeLib.OrientationConstants.DCPage);
            AddFieldCPRP(fieldDetail, "CopyLineName", "CopyLineName", DynamiCubeLib.OrientationConstants.DCPage);
            AddFieldCPRP(fieldDetail, "SpotType", "SpotType", DynamiCubeLib.OrientationConstants.DCPage);
            AddFieldCPRP(fieldDetail, "MaterialName", "MaterialName", DynamiCubeLib.OrientationConstants.DCPage);
            AddFieldCPRP(fieldDetail, "CampaignName", "CampaignName", DynamiCubeLib.OrientationConstants.DCPage);
            AddFieldCPRP(fieldDetail, "MediaSubType", "MediaSubType", DynamiCubeLib.OrientationConstants.DCPage);
            AddFieldCPRP(fieldDetail, "VendorName", "VendorName", DynamiCubeLib.OrientationConstants.DCPage);
            AddFieldCPRP(fieldDetail, "MasterVendorName", "MasterVendorName", DynamiCubeLib.OrientationConstants.DCPage);
            AddFieldCPRP(fieldDetail, "PIB", "PIB", DynamiCubeLib.OrientationConstants.DCPage);       
            
            //=====================================================================================================
            // DC Column
            //=====================================================================================================

            AddFieldCPRP(fieldDetail, "Version", "Version", DynamiCubeLib.OrientationConstants.DCColumn);

            //=====================================================================================================
            // DC Data
            //=====================================================================================================

            fieldDetail = axDetail.Fields.Add("Adex_Cost", "Adex_Cost", DynamiCubeLib.OrientationConstants.DCData);
            fieldDetail.AggregateFunc = DynamiCubeLib.AggregateFunctions.DCSum;
            fieldDetail.NumberFormat = "#,##0.00"; 

            fieldDetail = axDetail.Fields.Add("Cost", "Cost", DynamiCubeLib.OrientationConstants.DCData);
            fieldDetail.AggregateFunc = DynamiCubeLib.AggregateFunctions.DCSum;
            fieldDetail.NumberFormat = "#,##0.00";

            fieldDetail = axDetail.Fields.Add("Spots", "Spots", DynamiCubeLib.OrientationConstants.DCData);
            fieldDetail.AggregateFunc = DynamiCubeLib.AggregateFunctions.DCSum;
            fieldDetail.NumberFormat = "#,##0";


            fieldDetail = axDetail.Fields.Add("Rating", "GRP", DynamiCubeLib.OrientationConstants.DCData);
            fieldDetail.AggregateFunc = DynamiCubeLib.AggregateFunctions.DCSum;
            fieldDetail.NumberFormat = "#,##0.00";

            fieldDetail = axDetail.Fields.Add("Rating30sec", "Rating30sec", DynamiCubeLib.OrientationConstants.DCData);
            fieldDetail.AggregateFunc = DynamiCubeLib.AggregateFunctions.DCSum;
            fieldDetail.NumberFormat = "#,##0.00";

            fieldDetail = axDetail.Fields.Add("CPRP", "CPRP", DynamiCubeLib.OrientationConstants.DCData);
            fieldDetail.AggregateFunc = DynamiCubeLib.AggregateFunctions.DCSum;
            fieldDetail.NumberFormat = "#,##0.00";



            // refresh the cube (call FetchData event below)
            axDetail.RefreshData();
            axDetail.AutoDataRefresh = true;
            
        
        }
        private void SetSpotMatch() 
        {
            try
            {
                // initialize the dataset
                DataSet ds = new DataSet("Unbound_Cube_Data");
                string strSQL = "";

                SqlConnection conn = new SqlConnection(m_strConnection);
                SqlDataAdapter da = null;

                DataTable dt = new DataTable();
                dt.Columns.Add("Client_ID");
                dt.Columns.Add("ClientName");
                for (int i = 0; i < m_gvClient.SelectedRows.Count; i++)
                {
                    DataRow dr = ((DataRowView)((DataGridViewRow)m_gvClient.SelectedRows[i]).DataBoundItem).Row;
                    DataRow drXML = dt.NewRow();
                    drXML.ItemArray = dr.ItemArray;
                    dt.Rows.Add(drXML);
                }

                strSQL = string.Format(@"
DECLARE @xml XML
SET @xml = '{0}'
SELECT  
       Tbl.Col.value('Client_ID[1]', 'varchar(20)') Client_ID,  
       Tbl.Col.value('ClientName[1]', 'varchar(200)') English_Name
INTO #Client
FROM   @xml.nodes('//row') Tbl(Col)  


--DECLARE @U VARCHAR(100)
--DECLARE @P VARCHAR(100)
--DECLARE @S VARCHAR(100)
--DECLARE @E VARCHAR(100)

--SET @U = 'pariwatk'
--SET @P = 'Fank7813'
--SET @S = '20110601'
--SET @E = '20110601'

SELECT CONVERT(DATETIME,MIN(Spots_Match.SP_Show_Date)) MinMatch,CONVERT(DATETIME,MAX(Spots_Match.SP_Show_Date)) MaxMatch
FROM Spot_Plan   with(nolock)
INNER JOIN Spots_Match   with(nolock)
	ON (Spots_Match.Buying_Brief_ID = Spot_Plan.Buying_Brief_ID) 
		AND (Spots_Match.Market_ID = Spot_Plan.Market_ID) 
		AND (Spots_Match.[Version] = Spot_Plan.[Version]) 
		AND (Spots_Match.Item = Spot_Plan.Item) 
		AND (Spots_Match.SP_Show_Date = Spot_Plan.Show_Date) 
		AND (Spots_Match.ID = Spot_Plan.ID) 
		AND (Spots_Match.[Status] = Spot_Plan.[Status])
	INNER JOIN Buying_Brief		
		ON Spots_Match.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
	INNER JOIN #Client UserPermission
		ON UserPermission.Client_ID = Buying_Brief.Client_ID  collate SQL_Latin1_General_CP1_CI_AS
WHERE Spots_Match.Actual_Rating IS NOT NULL		
AND Buying_Brief.Media_Sub_Type IN ('TV')
AND Spots_Match.Show_Date BETWEEN @S AND @E

            ", GRM.UTL.XMLUtil.ConvertDataTableToXML(dt));
                da = new SqlDataAdapter(strSQL, conn);
                da.SelectCommand.Parameters.Add("@U", SqlDbType.VarChar).Value = UserName.Replace(".", "");
                da.SelectCommand.Parameters.Add("@P", SqlDbType.VarChar).Value = Password;
                da.SelectCommand.Parameters.Add("@S", SqlDbType.VarChar).Value = m_strDateFrom;
                da.SelectCommand.Parameters.Add("@E", SqlDbType.VarChar).Value = m_strDateTo;
                da.SelectCommand.CommandTimeout = 0;
                da.Fill(ds);
                string strMinMatch = string.Empty;
                string strMaxMatch = string.Empty;
                if (ds.Tables[0].Rows[0]["MinMatch"] != DBNull.Value) 
                    strMinMatch = ((DateTime)ds.Tables[0].Rows[0]["MinMatch"]).ToString("dd MMM yyyy"); 
                if (ds.Tables[0].Rows[0]["MaxMatch"] != DBNull.Value) 
                    strMaxMatch = ((DateTime)ds.Tables[0].Rows[0]["MaxMatch"]).ToString("dd MMM yyyy");
                
                
                if (strMaxMatch != string.Empty)
                    lbSportMatch.Text = strMinMatch + " - " + strMaxMatch;
                else
                    lbSportMatch.Text = "NONE";
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }


        
        }
        private void MPA003_MediaSpending_Load(object sender, System.EventArgs e)
        {
            if (lbCondition.Text == "NONE")
            {
                MPA002_SearchCondition frmSearch = new MPA002_SearchCondition();
                if (frmSearch.ShowDialog() == DialogResult.OK)
                {
                    lbCondition.Text = frmSearch.DateFrom.ToString("dd MMM yyyy") + " - " + frmSearch.DateTo.ToString("dd MMM yyyy");

                    m_strDateFrom = frmSearch.DateFrom.ToString("yyyyMMdd");
                    m_strDateTo = frmSearch.DateTo.ToString("yyyyMMdd");
                    m_strMedaiSubType = frmSearch.MediaSubType;
                    m_gvClient = frmSearch.DataGridClient;
                    DataLoading();
                    SetSpotMatch();
                }
                else
                {
                    btnExport.Enabled = false;
                    btnTotal.Enabled = false;
                }
            }           
        }

        private void btnTotal_Click(object sender, EventArgs e)
        {
            bool bHide = (bool)(btnTotal.Text == "Show Total");
            foreach (DynamiCubeLib.Field eachField in axDetail.Fields)
                eachField.GroupFooterVisible = bHide;
            if (bHide)
                btnTotal.Text = "Hide Total";
            else
                btnTotal.Text = "Show Total";
        }		
        private void lbCondition_Click(object sender, EventArgs e)
        {
            MPA002_SearchCondition frmSearch = new MPA002_SearchCondition();
            
            DateTime dtFrom;
            DateTime dtTo;
            if (lbCondition.Text == "NONE")
            {
                dtFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtTo = dtFrom.AddMonths(1).AddDays(-1);
                m_gvClient = null;
            }
            else
            {
                dtFrom = Convert.ToDateTime(lbCondition.Text.Split('-')[0]);
                dtTo = Convert.ToDateTime(lbCondition.Text.Split('-')[1]);
            }
            frmSearch.DateFrom = dtFrom;
            frmSearch.DateTo = dtTo;
            frmSearch.MediaSubType = m_strMedaiSubType;
            frmSearch.DataGridClient = m_gvClient;
            if (frmSearch.ShowDialog() == DialogResult.OK)
            {
                lbCondition.Text = frmSearch.DateFrom.ToString("dd MMM yyyy") + " - " + frmSearch.DateTo.ToString("dd MMM yyyy");
                m_strDateFrom = frmSearch.DateFrom.ToString("yyyyMMdd");
                m_strDateTo = frmSearch.DateTo.ToString("yyyyMMdd");
                m_strMedaiSubType = frmSearch.MediaSubType;
                m_gvClient = frmSearch.DataGridClient;
                DataLoading();
                SetSpotMatch();
            }
        }
        
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                axDetail.ExportToExcel(saveFileDialog.FileName);
                GMessage.MessageInfo("Exporting Completed.");
            }
        }

        private void axDetail_FetchAttributes(object sender, AxDynamiCubeLib.IDCubeEvents_FetchAttributesEvent e)
        {
            DynamiCubeLib.Field thisField = axDetail.GetDataFieldFromColumn(e.cellAttrib.Column);
            
            if (thisField.Name == "CPRP")
            {      
                DynamiCubeLib.Field CostField = axDetail.GetDataFieldFromColumn(0);
                DynamiCubeLib.Field Rating30SecField = axDetail.GetDataFieldFromColumn(2);
                double dCost = axDetail.get_DataValue(e.cellAttrib.Row, 0);//Cost
                double dRating30sec = axDetail.get_DataValue(e.cellAttrib.Row, 2);//Rating30sec
                double dCPRP = dCost / dRating30sec;                
            }
        }
        private string ConvertyyyyMMddToDash(string sDate)
        {
            return sDate.Substring(0, 4) + "-" + sDate.Substring(4, 2) + "-" + sDate.Substring(6, 2);
        }
        DataRow m_drDebug ;
        private void axDetail_FetchData(object sender, EventArgs e)
        {
            try
            {

            // get the database into a dataset :)
            DataSet ds = GetSource();

            // used in AddRowEx() method
            object[] cubeData = new object[axDetail.Fields.Count()];
            // begin to populate your cube
            

            for(int iRow = 0;iRow < ds.Tables[0].Rows.Count;iRow++)
            {
                DataRow dr = ds.Tables[0].Rows[iRow];
                m_drDebug = dr;
                cubeData[(int)eCol.LateBrief] = dr["LateBrief"];
                cubeData[(int)eCol.ClientName] = dr["ClientName"];
                cubeData[(int)eCol.TargetName] = dr["TargetName"];
                cubeData[(int)eCol.DayPartName] = dr["DayPartName"];

                cubeData[(int)eCol.Version] = dr["Version"];

                cubeData[(int)eCol.Buying_Brief_ID] = dr["Buying_Brief_ID"];

                cubeData[(int)eCol.CampaignStartDate] = ConvertyyyyMMddToDash(dr["CampaignStartDate"].ToString());
                cubeData[(int)eCol.CampaignEndDate] = ConvertyyyyMMddToDash(dr["CampaignEndDate"].ToString());

                cubeData[(int)eCol.MonthWeek] = dr["MonthWeek"];
                cubeData[(int)eCol.ShowDate] = dr["ShowDate"];
                cubeData[(int)eCol.OwnDayPart] = dr["OwnDayPart"];
                cubeData[(int)eCol.DayOfWeek] = dr["DayOfWeek"];
                cubeData[(int)eCol.Day] = Convert.ToInt32(dr["Day"]).ToString("0#");
                cubeData[(int)eCol.Month] = Convert.ToInt32(dr["Month"]).ToString("0#");
                cubeData[(int)eCol.Year] = dr["Year"];
                string strStartTime = dr["StartTime"].ToString();
                cubeData[(int)eCol.StartTime] = string.Format("{0}:{1}:{2}",strStartTime.Substring(0,2),strStartTime.Substring(2,2),strStartTime.Substring(4,2));
                cubeData[(int)eCol.Length] = dr["Length"];
                cubeData[(int)eCol.Media] = dr["Media"];
                cubeData[(int)eCol.ProductName] = dr["ProductName"];
                cubeData[(int)eCol.BookingProgramName] = dr["BookingProgramName"];
                cubeData[(int)eCol.MatchedProgramName] = dr["MatchedProgramName"];
                cubeData[(int)eCol.ProgramType] = dr["ProgramType"];
                cubeData[(int)eCol.CopyLineName] = dr["CopyLineName"];
                cubeData[(int)eCol.SpotType] = dr["SpotType"];
                cubeData[(int)eCol.MaterialName] = dr["MaterialName"];
                cubeData[(int)eCol.CampaignName] = dr["CampaignName"];
                cubeData[(int)eCol.MediaSubType] = dr["MediaSubType"];
                cubeData[(int)eCol.VendorName] = dr["VendorName"];
                cubeData[(int)eCol.MasterVendorName] = dr["MasterVendorName"];
                cubeData[(int)eCol.PIB] = dr["PIB"];

                cubeData[(int)eCol.Cost] = dr["Cost"];
                cubeData[(int)eCol.Adex_Cost] = dr["Adex_Cost"];
                cubeData[(int)eCol.Spots] = dr["Spots"];
                cubeData[(int)eCol.Rating] = dr["Rating"];
                cubeData[(int)eCol.Rating30sec] = dr["Rating30sec"];
                //cubeData[(int)eCol.CPRP] = dr["CPRP"];
                cubeData[(int)eCol.CPRP] = Convert.ToDouble(dr["Cost"]) / Convert.ToDouble(dr["Rating30sec"]);

                // add the row to the cube
                object cubeDataObject = (object)cubeData;
                axDetail.AddRowEx(cubeDataObject);

            }

            bool bEndable = (bool)(ds.Tables[0].Rows.Count > 0);
            btnTotal.Enabled = bEndable;
            btnExport.Enabled = bEndable;
            // clean up
            ds.Dispose();

            //MessageBox.Show(string.Format(recordCount.ToString(), "0:#,##0") + " record(s) have been added to the cube");

            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }
          
        }
	}
}
