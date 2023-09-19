using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using GroupM.UTL;
using System.Configuration;
using DevExpress.XtraPivotGrid;
using Excel = Microsoft.Office.Interop.Excel;

namespace  GroupM.Minder
{
    public partial class MPA004_PlanCPRPByBuyingBrief : System.Windows.Forms.Form
    {
        //string m_strConnection = "Data Source=BKK-SQL01;database=Minder_Thai;User id=sa;Password=Groupm#01;";
        string m_strConnection = Connection.ConnectionStringMinder;
        string m_strDateFrom = "yyyyMMdd";
        string m_strDateTo = "yyyyMMdd";
        string m_strMedaiSubType = "TV";
        int m_iSelectedTabActive = 0;
        DataTable m_dtResult = new DataTable();
        DataGridView m_gvClient = new DataGridView();
        DataGridView m_gvProduct = new DataGridView();
        DataGridView m_dgvBuyingBrief = new DataGridView();

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

        enum eCol
        {
            LateBrief
            ,
            Buying_Brief_ID
                ,
            PlannedBuget
                ,
            PlanedCPRP30
                ,
            PlannedGRP
                ,
            Version
                ,
            MonthWeek
                ,
            Length
                ,
            Media
                ,
            ProductName
                ,
            BookingProgramName
                ,
            SpotType
                ,
            MaterialName
                ,
            MediaSubType
                ,
            ClientName
                ,
            TargetName
                ,
            DayPartName
                ,
            Cost
                ,
            Spots
                ,
            Rating
                ,
            Rating30sec
                , CPRP
        }

        enum eColTarget
        {
            LateBrief
           ,
            Buying_Brief_ID
                ,
            TargetName
                ,
            PlannedBuget
                ,
            PlanedCPRP30
                ,
            PlannedGRP
                ,
            Version
                ,
            MonthWeek
                ,
            Length
                ,
            Media
                ,
            ProductName
                ,
            BookingProgramName
                ,
            SpotType
                ,
            MaterialName
                ,
            MediaSubType
                ,
            ClientName
                ,
            DayPartName
                ,
            Cost
                ,
            Spots
                ,
            Rating
                ,
            Rating30sec
                , CPRP
        }
        public MPA004_PlanCPRPByBuyingBrief(string strUserName, string strPassword)
        {
            this.UserName = strUserName;
            this.Password = strPassword;
            InitializeComponent();


        }

        private DataSet GetSource()
        {
            if (m_strPreviousUserName == UserName
                && m_strPreviousPassword == Password
                && m_strPreviousDateFrom == m_strDateFrom
                && m_strPreviousDateTo == m_strDateTo
                && m_gvPreviousClient == m_gvClient)
                return m_dsPreviousDataSource;
            try
            {
                string ClientName = "";
                for (int i = 0; i < m_gvClient.SelectedRows.Count; i++)
                {
                    DataRow dr = ((DataRowView)((DataGridViewRow)m_gvClient.SelectedRows[i]).DataBoundItem).Row;
                    ClientName = dr.ItemArray.GetValue(1).ToString();
                }
                using (SqlConnection connection = new SqlConnection(m_strConnection))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "dbo.MPA_PlanCPRPByBuyingBrief";
                    command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.Add("@UserID", SqlDbType.NVarChar, 50).Value = UserName;
                    command.Parameters.Add("@MediaSubType", SqlDbType.NVarChar, 10).Value = m_strMedaiSubType.Replace(",", "','");
                    command.Parameters.Add("@ClientName", SqlDbType.NVarChar).Value = ClientName;
                    command.Parameters.Add("@StartDate", SqlDbType.NVarChar, 8).Value = m_strDateFrom;
                    command.Parameters.Add("@EndDate", SqlDbType.NVarChar, 8).Value = m_strDateTo;
                    m_dtExecuteTime = DateTime.Now;
                    DataSet ds = new DataSet();
                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        da.Fill(ds);
                        m_strPreviousUserName = UserName;
                        m_strPreviousPassword = Password;
                        m_strPreviousDateFrom = m_strDateFrom;
                        m_strPreviousDateTo = m_strDateTo;
                        m_dsPreviousDataSource = ds;
                        return ds;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        DateTime m_dtExecuteTime = new DateTime();

        string m_strPreviousUserName = "";
        string m_strPreviousPassword = "";
        string m_strPreviousDateFrom = "";
        string m_strPreviousDateTo = "";
        DataGridView m_gvPreviousClient = null;
        DataSet m_dsPreviousDataSource = null;


        //        private DataSet GetSource()
        //        {
        //            if (m_strPreviousUserName == UserName
        //                && m_strPreviousPassword == Password
        //                && m_strPreviousDateFrom == m_strDateFrom
        //                && m_strPreviousDateTo == m_strDateTo
        //                && m_gvPreviousClient == m_gvClient)
        //                return m_dsPreviousDataSource;

        //            // initialize the dataset
        //            DataSet ds = new DataSet("Unbound_Cube_Data");
        //            string strSQL = "";

        //            SqlConnection conn = new SqlConnection(m_strConnection);
        //            SqlDataAdapter da = null;

        //            DataTable dt = new DataTable();
        //            dt.Columns.Add("Client_ID");
        //            dt.Columns.Add("ClientName");
        //            for (int i = 0; i < m_gvClient.SelectedRows.Count; i++)
        //            {
        //                DataRow dr = ((DataRowView)((DataGridViewRow)m_gvClient.SelectedRows[i]).DataBoundItem).Row;
        //                DataRow drXML = dt.NewRow();
        //                drXML.ItemArray = dr.ItemArray;
        //                dt.Rows.Add(drXML);
        //            }

        //            strSQL = string.Format(@"
        //        DECLARE @xml XML
        //        SET @xml = '{0}'
        //        SELECT  
        //               Tbl.Col.value('Client_ID[1]', 'varchar(20)') Client_ID,  
        //               Tbl.Col.value('ClientName[1]', 'varchar(200)') English_Name
        //        INTO #Client
        //        FROM   @xml.nodes('//row') Tbl(Col) 
        //        
        //        --DECLARE @U VARCHAR(100)
        //        --DECLARE @P VARCHAR(100)
        //        --DECLARE @S VARCHAR(100)
        //        --DECLARE @E VARCHAR(100)
        //        
        //        --SET @U = 'pariwatk'
        //        --SET @P = 'Fank7813'
        //        --SET @S = '20110601'
        //        --SET @E = '20110601'
        //        
        //        SELECT * FROM 
        //        (
        //        SELECT 
        //        	CASE Buying_Brief.Late_Brief WHEN 0 THEN 'NO' ELSE 'YES' END LateBrief
        //        	, Buying_Brief.Total_Budget PlannedBuget
        //        	, Buying_Brief.CPRP30 PlanedCPRP30 
        //        	, Buying_Brief.GRPS PlannedGRP
        //        	, Client.English_Name ClientName
        //        	, Target.Short_Name TargetName
        //        	, dbo.GetDayPart(Spot_Plan.Start_Time,Spot_Plan.End_Time,Spot_Plan.Show_Date) DayPartName
        //        	, Spot_Plan.Buying_Brief_ID
        //        	, CASE LEN(Spot_Plan.Version) WHEN 2 THEN '1-APPROVE (PLANNED)' ELSE '2-LATEST (EXECUTING)' END [Version]
        //        	, Spot_Plan.Length
        //        	, RIGHT('0'+CONVERT(VARCHAR(2),MONTH(Spot_Plan.Show_Date)),2)
        //        		+ RIGHT('0'+CONVERT(VARCHAR(2),dbo.fn_ISOWK(Spot_Plan.Show_Date)),2) MonthWeek
        //        	, Media.English_Name Media
        //        	, Product.English_Name ProductName
        //        	, Spot_Plan.Program BookingProgramName
        //        	, Spot_Plan.Package SpotType
        //        	, Material.Thai_Name MaterialName
        //        	, Buying_Brief.Media_Sub_Type MediaSubType
        //        	, SUM(Spot_Plan.Spots) Spots
        //        	, SUM(Spot_Plan.Net_Cost) Cost
        //        	, SUM(Spot_Plan.Rating) Rating
        //        	, SUM((Spot_Plan.Rating * Spot_Plan.Length)/30 ) Rating30sec
        //        	, CASE SUM((Spot_Plan.Rating * Spot_Plan.Length)/30 )  WHEN 0 THEN 0 ELSE
        //        	SUM(Spot_Plan.Net_Cost) / SUM((Spot_Plan.Rating * Spot_Plan.Length)/30 ) END CPRP 
        //        FROM Spot_Plan with(nolock)
        //        	INNER JOIN Buying_Brief		
        //        		ON Spot_Plan.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
        //        	INNER JOIN #Client UserPermission
        //        		ON UserPermission.Client_ID = Buying_Brief.Client_ID 
        //        	INNER JOIN Target 
        //        		ON Buying_Brief.Primary_Target = Target.Target_ID
        //        	INNER JOIN Client
        //        		ON Buying_Brief.Client_ID = Client.Client_ID
        //        	INNER JOIN Product
        //        		ON Buying_Brief.Product_ID = Product.Product_ID
        //        	INNER JOIN Material
        //        		ON Material.Material_ID = Spot_Plan.Material_ID
        //        	INNER JOIN Media
        //        		ON Media.Media_ID = Spot_Plan.Media_ID
        //        WHERE 
        //        	Buying_Brief.Media_Sub_Type IN ('" + m_strMedaiSubType.Replace(",", "','") + @"')
        //        	AND Spot_Plan.Show_Date BETWEEN @S AND @E
        //        	AND Spot_Plan.Status > 0
        //        	--AND Buying_Brief.Client_ID ='20LORMOB'
        //        GROUP BY 
        //        	 Target.Short_Name
        //        	, Media.English_Name
        //        	, Product.English_Name 
        //        	, dbo.GetDayPart(Spot_Plan.Start_Time,Spot_Plan.End_Time,Spot_Plan.Show_Date) 
        //        	, Client.English_Name
        //        	, Spot_Plan.Program 
        //        	, Spot_Plan.Package 
        //        	, Material.Thai_Name 
        //        	, Buying_Brief.Media_Sub_Type
        //        	, Buying_Brief.Late_Brief
        //        	, Buying_Brief.Total_Budget 
        //        	, Buying_Brief.CPRP30  
        //        	, Buying_Brief.GRPS 
        //        	, Spot_Plan.Buying_Brief_ID
        //        	, Spot_Plan.Version
        //        	, Spot_Plan.Length
        //        	, RIGHT('0'+CONVERT(VARCHAR(2),MONTH(Spot_Plan.Show_Date)),2)  
        //        		+ RIGHT('0'+CONVERT(VARCHAR(2),dbo.fn_ISOWK(Spot_Plan.Show_Date)),2) 
        //        
        //        UNION
        //        
        //        SELECT 
        //        	CASE Buying_Brief.Late_Brief WHEN 0 THEN 'NO' ELSE 'YES' END LateBrief
        //        	, Buying_Brief.Total_Budget PlannedBuget
        //        	, Buying_Brief.CPRP30 PlanedCPRP30 
        //        	, Buying_Brief.GRPS PlannedGRP
        //        	, Client.English_Name ClientName
        //        	, Target.Short_Name TargetName
        //        	, dbo.GetDayPart(SportsMatch.Start_Time,SportsMatch.End_Time,SportsMatch.Show_Date) DayPartName
        //        	, SportsMatch.Buying_Brief_ID
        //        	, '3-ACTUAL (POST)' [Version]
        //        	, SportsMatch.Length
        //        	, RIGHT('0'+CONVERT(VARCHAR(2),MONTH(SportsMatch.Show_Date)),2)
        //        		+ RIGHT('0'+CONVERT(VARCHAR(2),dbo.fn_ISOWK(SportsMatch.Show_Date)),2) MonthWeek
        //        	, Media.English_Name Media
        //        	, Product.English_Name ProductName
        //        	, SportsMatch.BookingProgramName
        //        	, SportsMatch.SpotType
        //        	, SportsMatch.MaterialName
        //        	, Buying_Brief.Media_Sub_Type MediaSubType
        //        	, SUM(SportsMatch.Spots) Spots
        //        	, SUM(SportsMatch.Net_Cost) Cost
        //        	, SUM(SportsMatch.Actual_Rating) Rating
        //        	, SUM((SportsMatch.Actual_Rating * SportsMatch.Length)/30 ) Rating30sec
        //        	, 
        //        	CASE SUM((SportsMatch.Actual_Rating * SportsMatch.Length)/30 ) WHEN 0 THEN 0 ELSE
        //        	 SUM(SportsMatch.Net_Cost) / SUM((SportsMatch.Actual_Rating * SportsMatch.Length)/30 ) END CPRP 
        //        FROM (
        //SELECT 
        //    Spots_Match.Buying_Brief_ID
        //    ,Spot_Plan.Net_Cost
        //    ,Spots_Match.Length
        //    ,Spot_Plan.Media_ID
        //    ,Spots_Match.SP_Show_Date Show_Date
        //    ,Spot_Plan.Start_Time
        //    ,Spot_Plan.End_Time
        //    ,Spots_Match.Actual_Rating
        //    ,Spot_Plan.Spots
        //    ,Spot_Plan.Program BookingProgramName
        //    ,Spot_Plan.Package SpotType
        //    ,Material.Thai_Name MaterialName
        //FROM Spot_Plan   with(nolock)
        //INNER JOIN Spots_Match   with(nolock)
        //    ON (Spots_Match.Buying_Brief_ID = Spot_Plan.Buying_Brief_ID) 
        //        AND (Spots_Match.Market_ID = Spot_Plan.Market_ID) 
        //        AND (Spots_Match.[Version] = Spot_Plan.[Version]) 
        //        AND (Spots_Match.Item = Spot_Plan.Item) 
        //        AND (Spots_Match.SP_Show_Date = Spot_Plan.Show_Date) 
        //        AND (Spots_Match.ID = Spot_Plan.ID) 
        //        AND (Spots_Match.[Status] = Spot_Plan.[Status])
        //INNER JOIN Material
        //    ON Material.Material_ID = Spot_Plan.Material_ID
        //INNER JOIN Buying_Brief		
        //    ON Spots_Match.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
        //INNER JOIN #Client UserPermission
        //    ON UserPermission.Client_ID = Buying_Brief.Client_ID 
        //WHERE Spots_Match.Actual_Rating IS NOT NULL		
        //UNION ALL		
        //SELECT 
        //    Spots_Match.Buying_Brief_ID
        //    , 0 Net_Cost
        //    ,Spots_Match.Length
        //    ,Spots_Match.Media_ID
        //    ,Spots_Match.Show_Date
        //    ,Spots_Match.Actual_Time Start_Time
        //    ,Spots_Match.Actual_Time End_Time
        //    ,Spots_Match.Actual_Rating 
        //    , 1 Spots
        //    ,Spots_Match.[Program_Name] BookingProgramName
        //    ,'Unknown Bonus' SpotType
        //    ,PDNO MaterialName
        //FROM Spots_Match  with(nolock) 
        //    INNER JOIN Buying_Brief		
        //        ON Spots_Match.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
        //    INNER JOIN #Client UserPermission
        //        ON UserPermission.Client_ID = Buying_Brief.Client_ID 
        //WHERE Spots_Match.Flag_Bonus = 1 AND Spots_Match.Actual_Rating IS NOT NULL
        //        		
        //        		) SportsMatch 
        //        	INNER JOIN Buying_Brief		
        //        		ON SportsMatch.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
        //        	INNER JOIN #Client UserPermission
        //        		ON UserPermission.Client_ID = Buying_Brief.Client_ID 
        //        	INNER JOIN Target 
        //        		ON Buying_Brief.Primary_Target = Target.Target_ID
        //        	INNER JOIN Client
        //        		ON Buying_Brief.Client_ID = Client.Client_ID
        //        	INNER JOIN Product
        //        		ON Buying_Brief.Product_ID = Product.Product_ID
        //        	INNER JOIN Media
        //        		ON Media.Media_ID = SportsMatch.Media_ID
        //        WHERE 
        //        	Buying_Brief.Media_Sub_Type IN ('" + m_strMedaiSubType.Replace(",", "','") + @"')
        //        	AND SportsMatch.Show_Date BETWEEN @S AND @E
        //        	AND SportsMatch.Actual_Rating IS NOT NULL
        //        	--AND Buying_Brief.Client_ID ='20LORMOB'
        //        	--AND SportsMatch.Buying_Brief_ID = '2011050387'
        //        GROUP BY 
        //        	 Target.Short_Name
        //        	, Media.English_Name
        //        	, Product.English_Name 
        //        	, dbo.GetDayPart(SportsMatch.Start_Time,SportsMatch.End_Time,SportsMatch.Show_Date)
        //        	, Client.English_Name
        //        	, SportsMatch.BookingProgramName
        //        	, SportsMatch.SpotType
        //        	, SportsMatch.MaterialName
        //        	, Buying_Brief.Media_Sub_Type
        //        	, Buying_Brief.Late_Brief
        //        	, Buying_Brief.Total_Budget 
        //        	, Buying_Brief.CPRP30  
        //        	, Buying_Brief.GRPS 
        //        	, SportsMatch.Buying_Brief_ID
        //        	, SportsMatch.Length
        //        	, RIGHT('0'+CONVERT(VARCHAR(2),MONTH(SportsMatch.Show_Date)),2)
        //        		+ RIGHT('0'+CONVERT(VARCHAR(2),dbo.fn_ISOWK(SportsMatch.Show_Date)),2) 
        //        ) rec
        //        
        //                    ", GRM.UTL.XMLUtil.ConvertDataTableToXML(dt));
        //            da = new SqlDataAdapter(strSQL, conn);
        //            da.SelectCommand.Parameters.Add("@U", SqlDbType.VarChar).Value = UserName.Replace(".", "");
        //            da.SelectCommand.Parameters.Add("@P", SqlDbType.VarChar).Value = Password;
        //            da.SelectCommand.Parameters.Add("@S", SqlDbType.VarChar).Value = m_strDateFrom;
        //            da.SelectCommand.Parameters.Add("@E", SqlDbType.VarChar).Value = m_strDateTo;
        //            da.SelectCommand.CommandTimeout = 0;
        //            m_dtExecuteTime = DateTime.Now;
        //            da.Fill(ds);


        //            m_strPreviousUserName = UserName;
        //            m_strPreviousPassword = Password;
        //            m_strPreviousDateFrom = m_strDateFrom;
        //            m_strPreviousDateTo = m_strDateTo;
        //            m_dsPreviousDataSource = ds;
        //            return ds;
        //        }



        private void DataLoading()
        {
            //===========================================================================
            // Set Column
            //===========================================================================

            gvDetail.Fields.Clear();
            m_dtResult = GetSource().Tables[0];
            gvDetail.DataSource = m_dtResult;


            //=====================================================================================================
            // DC Row
            //=====================================================================================================
            // row fields

            gvDetail.Fields.Add("LateBrief", PivotArea.RowArea);
            gvDetail.Fields.Add("Buying_Brief_ID", PivotArea.RowArea);
            if (btnTarget.Text != m_strTarget) { gvDetail.Fields.Add("TargetName", PivotArea.RowArea); }
            gvDetail.Fields.Add("PlannedBuget", PivotArea.RowArea);
            gvDetail.Fields.Add("PlanedCPRP30", PivotArea.RowArea);
            gvDetail.Fields.Add("PlannedGRP", PivotArea.RowArea);


            //=====================================================================================================
            // DC Page
            //=====================================================================================================

            gvDetail.Fields.Add("MonthWeek", PivotArea.FilterArea);
            gvDetail.Fields.Add("Length", PivotArea.FilterArea);
            gvDetail.Fields.Add("Media", PivotArea.FilterArea);
            gvDetail.Fields.Add("ProductName", PivotArea.FilterArea);
            gvDetail.Fields.Add("BookingProgramName", PivotArea.FilterArea);
            gvDetail.Fields.Add("SpotType", PivotArea.FilterArea);
            gvDetail.Fields.Add("MaterialName", PivotArea.FilterArea);
            gvDetail.Fields.Add("MediaSubType", PivotArea.FilterArea);
            gvDetail.Fields.Add("ClientName", PivotArea.FilterArea);
            if (btnTarget.Text == m_strTarget) { gvDetail.Fields.Add("TargetName", PivotArea.FilterArea); }
            gvDetail.Fields.Add("DayPartName", PivotArea.FilterArea);

            //=====================================================================================================
            // DC Column
            //=====================================================================================================

            gvDetail.Fields.Add("Version", PivotArea.ColumnArea);

            //=====================================================================================================
            // DC Data
            //=====================================================================================================
            gvDetail.Fields.Add("Cost", PivotArea.DataArea);
            gvDetail.Fields.Add("Spots", PivotArea.DataArea);
            gvDetail.Fields.Add("Rating", PivotArea.DataArea);
            gvDetail.Fields.Add("Rating30sec", PivotArea.DataArea);

            gvDetail.Fields.Add("CPRP", PivotArea.DataArea);

            //fieldCPRP.Caption = "CPRP";
            //fieldCPRP.Area = PivotArea.DataArea;
            //fieldCPRP.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Custom;
            //gvDetail.Fields.Add(fieldCPRP);

            SetFiledFormat();

            // refresh the cube (call FetchData event below)

            gvDetail.OptionsView.ShowRowTotals = false;
            gvDetail.OptionsView.ShowRowGrandTotals = false;

            //foreach (DynamiCubeLib.Field eachField in axDetail.Fields)
            //    eachField.GroupFooterVisible = false;

            SetFiledFormat();

        }
        private void SetFiledFormat()
        {
            SetFieldsN0(gvDetail.Fields["Cost"]);
            SetFieldsN0(gvDetail.Fields["Spots"]);
            if (m_strMedaiSubType.IndexOf("CS") != -1)
            {
                SetFieldsN4(gvDetail.Fields["Rating"]);
                SetFieldsN4(gvDetail.Fields["Rating30sec"]);
            }
            else
            {
                SetFieldsN2(gvDetail.Fields["Rating"]);
                SetFieldsN2(gvDetail.Fields["Rating30sec"]);
            }
            SetFieldsN0(gvDetail.Fields["CPRP"]);
            gvDetail.BestFit();
        }
        PivotGridField fieldCPRP = new PivotGridField();
        private void SetFieldsN0(PivotGridField field)
        {

            field.ValueFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            field.ValueFormat.FormatString = "N0";
            field.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            field.CellFormat.FormatString = "N0";
        }
        private void SetFieldsN2(PivotGridField field)
        {

            field.ValueFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            field.ValueFormat.FormatString = "N2";
            field.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            field.CellFormat.FormatString = "N2";
        }

        private void SetFieldsN4(PivotGridField field)
        {

            field.ValueFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            field.ValueFormat.FormatString = "N4";
            field.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            field.CellFormat.FormatString = "N4";
        }





        string m_strTarget = "Add TargetName To Row";
        private void MPA003_MediaSpending_Load(object sender, System.EventArgs e)
        {

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
            }
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (saveFileDialog.FilterIndex == 2)
                    gvDetail.OptionsPrint.MergeRowFieldValues = false;
                else
                    gvDetail.OptionsPrint.MergeRowFieldValues = true;

                gvDetail.OptionsPrint.PrintColumnHeaders = DevExpress.Utils.DefaultBoolean.False;
                gvDetail.OptionsPrint.PrintDataHeaders = DevExpress.Utils.DefaultBoolean.False;
                gvDetail.OptionsPrint.PrintFilterHeaders = DevExpress.Utils.DefaultBoolean.False;
                gvDetail.ExportToXlsx(saveFileDialog.FileName);

                Excel.Application excelApp = new Excel.Application();

                string workbookPath = saveFileDialog.FileName;
                Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(workbookPath,
                        0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "",
                        true, false, 0, true, false, false);

                Excel.Worksheet resultSheet = (Excel.Worksheet)excelWorkbook.Sheets.get_Item(1);
                resultSheet.Name = "Result";

                Excel.Worksheet newWorksheet = (Excel.Worksheet)excelWorkbook.Sheets.Add(Type.Missing,
                        excelWorkbook.Worksheets[1], Type.Missing, Type.Missing);
                newWorksheet.Name = "Condition";

                int iStartRow = 1;
                newWorksheet.Cells[iStartRow, 1] = "From:";
                newWorksheet.Cells[iStartRow, 2] = gvDesc.Rows[0].Cells[1].Value.ToString();
                iStartRow++;
                newWorksheet.Cells[iStartRow, 1] = "To:";
                newWorksheet.Cells[iStartRow, 2] = gvDesc.Rows[0].Cells[2].Value.ToString();
                iStartRow++;
                newWorksheet.Cells[iStartRow, 1] = "Matched:";
                newWorksheet.Cells[iStartRow, 2] = gvDesc.Rows[0].Cells[3].Value.ToString();
                iStartRow++;
                newWorksheet.Cells[iStartRow, 1] = "Client:";
                newWorksheet.Cells[iStartRow, 2] = gvDesc.Rows[0].Cells[4].Value.ToString();
                iStartRow++;
                newWorksheet.Cells[iStartRow, 1] = "Execute Time:";
                newWorksheet.Cells[iStartRow, 2] = m_dtExecuteTime.ToString("yyyy-MM-dd HH:mm:ss");
                iStartRow++;
                newWorksheet.Cells[iStartRow, 1] = "Export Time:";
                newWorksheet.Cells[iStartRow, 2] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                iStartRow++;
                ((Excel.Range)newWorksheet.Range[newWorksheet.Cells[1, 1], newWorksheet.Cells[iStartRow, 2]]).Columns.AutoFit();


                resultSheet.Activate();
                excelWorkbook.Save();
                excelApp.Quit();
                GMessage.MessageInfo("Exporting complete.");
            }
            //if (saveFileDialog.ShowDialog() == DialogResult.OK)
            //{
            //    axDetail.ExportToExcel(saveFileDialog.FileName);
            //    GMessage.MessageInfo("Exporting Completed.");
            //}
        }




        private DataSet SetSpotMatch()
        {
            try
            {
                string ClientName = "";
                for (int i = 0; i < m_gvClient.SelectedRows.Count; i++)
                {
                    DataRow dr = ((DataRowView)((DataGridViewRow)m_gvClient.SelectedRows[i]).DataBoundItem).Row;
                    ClientName = dr.ItemArray.GetValue(1).ToString();
                }
                using (SqlConnection connection = new SqlConnection(m_strConnection))
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "dbo.MPA_PlanCPRPByBuyingBrief_SetSpotMatch";
                    command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.Add("@UserID", SqlDbType.NVarChar, 50).Value = UserName;
                    command.Parameters.Add("@MediaSubType", SqlDbType.NVarChar, 10).Value = m_strMedaiSubType.Replace(",", "','");
                    command.Parameters.Add("@ClientName", SqlDbType.NVarChar).Value = ClientName;
                    command.Parameters.Add("@StartDate", SqlDbType.NVarChar, 8).Value = m_strDateFrom;
                    command.Parameters.Add("@EndDate", SqlDbType.NVarChar, 8).Value = m_strDateTo;
                    m_dtExecuteTime = DateTime.Now;
                    DataSet ds = new DataSet();
                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        da.Fill(ds);
                        string strMinMatch = string.Empty;
                        string strMaxMatch = string.Empty;
                        if (ds.Tables[0].Rows[0]["MinMatch"] != DBNull.Value)
                            strMinMatch = ((DateTime)ds.Tables[0].Rows[0]["MinMatch"]).ToString("dd MMM yyyy");
                        if (ds.Tables[0].Rows[0]["MaxMatch"] != DBNull.Value)
                            strMaxMatch = ((DateTime)ds.Tables[0].Rows[0]["MaxMatch"]).ToString("dd MMM yyyy");
                        return ds;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        //        private void SetSpotMatch()
        //        {
        //            try
        //            {
        //                // initialize the dataset
        //                DataSet ds = new DataSet("Unbound_Cube_Data");
        //                string strSQL = "";

        //                SqlConnection conn = new SqlConnection(m_strConnection);
        //                SqlDataAdapter da = null;

        //                DataTable dt = new DataTable();
        //                dt.Columns.Add("Client_ID");
        //                dt.Columns.Add("ClientName");
        //                for (int i = 0; i < m_gvClient.SelectedRows.Count; i++)
        //                {
        //                    DataRow dr = ((DataRowView)((DataGridViewRow)m_gvClient.SelectedRows[i]).DataBoundItem).Row;
        //                    DataRow drXML = dt.NewRow();
        //                    drXML.ItemArray = dr.ItemArray;
        //                    dt.Rows.Add(drXML);
        //                }

        //                strSQL = string.Format(@"
        //DECLARE @xml XML
        //SET @xml = '{0}'
        //SELECT  
        //       Tbl.Col.value('Client_ID[1]', 'varchar(20)') Client_ID,  
        //       Tbl.Col.value('ClientName[1]', 'varchar(200)') English_Name
        //INTO #Client
        //FROM   @xml.nodes('//row') Tbl(Col)  
        //
        //
        //--DECLARE @U VARCHAR(100)
        //--DECLARE @P VARCHAR(100)
        //--DECLARE @S VARCHAR(100)
        //--DECLARE @E VARCHAR(100)
        //
        //--SET @U = 'pariwatk'
        //--SET @P = 'Fank7813'
        //--SET @S = '20110601'
        //--SET @E = '20110601'
        //
        //SELECT CONVERT(DATETIME,MIN(Spots_Match.SP_Show_Date)) MinMatch,CONVERT(DATETIME,MAX(Spots_Match.SP_Show_Date)) MaxMatch
        //FROM Spot_Plan   with(nolock)
        //INNER JOIN Spots_Match   with(nolock)
        //	ON (Spots_Match.Buying_Brief_ID = Spot_Plan.Buying_Brief_ID) 
        //		AND (Spots_Match.Market_ID = Spot_Plan.Market_ID) 
        //		AND (Spots_Match.[Version] = Spot_Plan.[Version]) 
        //		AND (Spots_Match.Item = Spot_Plan.Item) 
        //		AND (Spots_Match.SP_Show_Date = Spot_Plan.Show_Date) 
        //		AND (Spots_Match.ID = Spot_Plan.ID) 
        //		AND (Spots_Match.[Status] = Spot_Plan.[Status])
        //	INNER JOIN Buying_Brief		
        //		ON Spots_Match.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
        //	INNER JOIN #Client UserPermission
        //		ON UserPermission.Client_ID = Buying_Brief.Client_ID 
        //WHERE Spots_Match.Actual_Rating IS NOT NULL		
        //AND Buying_Brief.Media_Sub_Type IN ('TV')
        //AND Spots_Match.Show_Date BETWEEN @S AND @E
        //
        //            ", GRM.UTL.XMLUtil.ConvertDataTableToXML(dt));
        //                da = new SqlDataAdapter(strSQL, conn);
        //                da.SelectCommand.Parameters.Add("@U", SqlDbType.VarChar).Value = UserName.Replace(".", "");
        //                da.SelectCommand.Parameters.Add("@P", SqlDbType.VarChar).Value = Password;
        //                da.SelectCommand.Parameters.Add("@S", SqlDbType.VarChar).Value = m_strDateFrom;
        //                da.SelectCommand.Parameters.Add("@E", SqlDbType.VarChar).Value = m_strDateTo;
        //                da.SelectCommand.CommandTimeout = 0;
        //                da.Fill(ds);
        //                string strMinMatch = string.Empty;
        //                string strMaxMatch = string.Empty;
        //                if (ds.Tables[0].Rows[0]["MinMatch"] != DBNull.Value)
        //                    strMinMatch = ((DateTime)ds.Tables[0].Rows[0]["MinMatch"]).ToString("dd MMM yyyy");
        //                if (ds.Tables[0].Rows[0]["MaxMatch"] != DBNull.Value)
        //                    strMaxMatch = ((DateTime)ds.Tables[0].Rows[0]["MaxMatch"]).ToString("dd MMM yyyy");

        //            }
        //            catch (Exception ex)
        //            {
        //                GMessage.MessageError(ex);
        //            }



        //        }



        private void btnTarget_Click(object sender, EventArgs e)
        {
            if (btnTarget.Text == m_strTarget)
            {
                btnTarget.Text = "Del TargetName From Row";
                btnTarget.Image = global::GroupM.Minder.Properties.Resources.delete;
                DataLoading();
            }
            else
            {
                btnTarget.Text = m_strTarget;
                btnTarget.Image = global::GroupM.Minder.Properties.Resources.add;
                DataLoading();
            }
        }

        private void gvDesc_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == 0 && e.ColumnIndex == 0)
            {
                MPA002_SearchCondition frmSearch = new MPA002_SearchCondition();

                DateTime dtFrom;
                DateTime dtTo;
                if (gvDesc.Rows[0].Cells[1].Value.ToString() == "-")
                {
                    dtFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    dtTo = dtFrom.AddMonths(1).AddDays(-1);
                    m_gvClient = null;
                    m_gvProduct = null;
                    m_dgvBuyingBrief = null;
                }
                else
                {
                    dtFrom = Convert.ToDateTime(gvDesc.Rows[0].Cells[1].Value);
                    dtTo = Convert.ToDateTime(gvDesc.Rows[0].Cells[2].Value);
                }
                frmSearch.DateFrom = dtFrom;
                frmSearch.DateTo = dtTo;
                frmSearch.MediaSubType = m_strMedaiSubType;
                frmSearch.DataGridClient = m_gvClient;
                frmSearch.DataGridProduct = m_gvProduct;
                frmSearch.DataGridBuyingBrief = m_dgvBuyingBrief;
                frmSearch.mainTabControl.SelectedIndex = m_iSelectedTabActive;
                if (frmSearch.ShowDialog() == DialogResult.OK)
                {
                    try
                    {


                        m_strDateFrom = frmSearch.DateFrom.ToString("yyyyMMdd");
                        m_strDateTo = frmSearch.DateTo.ToString("yyyyMMdd");
                        m_strMedaiSubType = frmSearch.MediaSubType;
                        m_gvClient = frmSearch.DataGridClient;
                        m_gvProduct = frmSearch.DataGridProduct;
                        m_dgvBuyingBrief = frmSearch.DataGridBuyingBrief;


                        string strClients = "";

                        if (frmSearch.mainTabControl.SelectedIndex == 0)
                        { //Client
                            m_iSelectedTabActive = 0;
                            gvDesc.Columns[4].HeaderText = "Selected Client";
                            for (int i = 0; i < m_gvClient.SelectedRows.Count; i++)
                            {
                                DataGridViewRow dvr = m_gvClient.SelectedRows[i];
                                DataRow dr = ((DataRowView)dvr.DataBoundItem).Row;
                                if (strClients != "")
                                    strClients += ", ";
                                strClients += dr[1].ToString();
                            }
                        }
                        else if (frmSearch.mainTabControl.SelectedIndex == 1)
                        { //Product
                            m_iSelectedTabActive = 1;
                            gvDesc.Columns[4].HeaderText = "Selected Product";
                            for (int i = 0; i < m_gvProduct.SelectedRows.Count; i++)
                            {
                                DataGridViewRow dvr = m_gvProduct.SelectedRows[i];
                                DataRow dr = ((DataRowView)dvr.DataBoundItem).Row;
                                if (strClients != "")
                                    strClients += ", ";
                                strClients += dr[1].ToString();
                            }
                        }
                        else
                        { //BB
                            m_iSelectedTabActive = 2;
                            gvDesc.Columns[4].HeaderText = "Selected Buying Brief";
                            for (int i = 0; i < m_dgvBuyingBrief.SelectedRows.Count; i++)
                            {
                                DataGridViewRow dvr = m_dgvBuyingBrief.SelectedRows[i];
                                DataRow dr = ((DataRowView)dvr.DataBoundItem).Row;
                                if (strClients != "")
                                    strClients += ", ";
                                strClients += dr[0].ToString();
                            }
                        }
                        gvDesc.Rows[0].Cells[4].Value = strClients;
                        gvDesc.Rows[0].Cells[1].Value = frmSearch.DateFrom.ToString("dd MMM yyyy");
                        gvDesc.Rows[0].Cells[2].Value = frmSearch.DateTo.ToString("dd MMM yyyy");
                        DataLoading();
                        SetSpotMatch();
                    }
                    finally
                    {
                        Cursor = Cursors.Default;
                    }

                }
            }
        }

        private void btnRefreshData_Click(object sender, EventArgs e)
        {
            if (gvDesc.DataSource == null)
            {
                return;
            }
            else
            {
                DataLoading();
            }
        }

        private void MPA004_PlanCPRPByBuyingBrief_Load(object sender, EventArgs e)
        {

            gvDesc.Rows.Add(1);
            gvDesc.Rows[0].Cells[0].Value = "Modify";
            gvDesc.Rows[0].Cells[1].Value = "-";
            gvDesc.Rows[0].Cells[2].Value = "-";
            gvDesc.Rows[0].Cells[3].Value = "NONE";

            if (lbCondition.Text == "NONE")
            {
                gvDesc_CellMouseClick(null, new DataGridViewCellMouseEventArgs(0, 0, 0, 0, new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0)));
            }
            DevExpress.Export.ExportSettings.DefaultExportType = DevExpress.Export.ExportType.WYSIWYG;
        }
    }
}
