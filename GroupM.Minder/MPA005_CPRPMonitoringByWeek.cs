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
    public partial class MPA005_CPRPMonitoringByWeek : System.Windows.Forms.Form
    {
        //string m_strConnection = "Data Source=BKK-SQL01;database=Minder_Thai;User id=sa;Password=Groupm#01;";
        string m_strConnection = Connection.ConnectionStringMinder;
        string m_strDateFrom = "yyyyMMdd";
        string m_strDateTo = "yyyyMMdd";
        string m_strMedaiSubType = "TV";
        int m_iSelectedTabActive = 0;
        DataGridView m_gvClient = new DataGridView();
        DataGridView m_gvProduct = new DataGridView();
        DataGridView m_dgvBuyingBrief = new DataGridView();
        DataTable m_dtResult = new DataTable();


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
            LateBrief,
            ClientName,
            TargetName,
            DayPartName,
            MonthWeek,
            Buying_Brief_ID,
            Length,
            Media,
            ProductName,
            BookingProgramName,
            SpotType,
            MaterialName,
            CampaignName,
            MediaSubType,
            VendorName,
            Cost,
            Spots,
            Rating,
            Rating30sec,
            CPRP
        }
        public MPA005_CPRPMonitoringByWeek(string strUserName, string strPassword)
        {
            this.UserName = strUserName;
            this.Password = strPassword;
            InitializeComponent();

            

        }

        private DataSet GetSource()
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
                    command.CommandText = "dbo.MPA_CPRPMonitoringByWeek";
                    command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.Add("@UserID", SqlDbType.NVarChar, 50).Value = UserName;
                    command.Parameters.Add("@MediaSubType", SqlDbType.NVarChar, 10).Value = m_strMedaiSubType.Replace(",", "','");
                    command.Parameters.Add("@ClientName", SqlDbType.NVarChar).Value = ClientName;
                    command.Parameters.Add("@StartDate", SqlDbType.NVarChar, 8).Value = m_strDateFrom;
                    command.Parameters.Add("@EndDate", SqlDbType.NVarChar, 8).Value = m_strDateTo;
                    DataSet ds = new DataSet();
                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        da.Fill(ds);
                        return ds;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        //        private DataSet GetSource()
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
        //--DECLARE @U VARCHAR(100)
        //--DECLARE @P VARCHAR(100)
        //--DECLARE @S VARCHAR(100)
        //--DECLARE @E VARCHAR(100)
        //
        //--SET @U = 'ubonvanc'
        //--SET @P = 'Uc27805865'
        //--SET @S = '20110801'
        //--SET @E = '20110831'
        //
        //
        //SELECT 
        //	CASE Spots.Late_Brief WHEN 0 THEN 'NO' ELSE 'YES' END LateBrief
        //	, Client.English_Name ClientName
        //	, Target.Short_Name TargetName
        //	, dbo.GetDayPart(Spots.Start_Time,Spots.End_Time,Spots.Show_Date) DayPartName
        //	, RIGHT('0'+CONVERT(VARCHAR(2),MONTH(Spots.Show_Date)),2)
        //		+ RIGHT('0'+CONVERT(VARCHAR(2),dbo.fn_ISOWK(Spots.Show_Date)),2) MonthWeek
        //	, Spots.Buying_Brief_ID
        //	, Spots.Length
        //	, Media.English_Name Media
        //	, Product.English_Name ProductName
        //	, Spots.BookingProgramName
        //	, Spots.SpotType
        //	, Spots.MaterialName
        //	, Spots.CampaignName
        //	, Spots.MediaSubType
        //	, Spots.VendorName
        //	, SUM(Spots.Spots) Spots
        //	, SUM(Spots.Net_Cost) Cost
        //	, SUM(Spots.Rating) Rating
        //	, SUM((Spots.Rating * Spots.Length)/30 ) Rating30sec
        //	, 
        //	CASE SUM((Spots.Rating * Spots.Length)/30 ) WHEN 0 THEN 0 ELSE
        //	 SUM(Spots.Net_Cost) / SUM((Spots.Rating * Spots.Length)/30 ) END CPRP 
        //FROM (
        //		SELECT 
        //			isnull(Spots_Match.Buying_Brief_ID,Spot_Plan.Buying_Brief_ID) Buying_Brief_ID
        //			,Buying_Brief.Late_Brief
        //			,Buying_Brief.Primary_Target 
        //			,Buying_Brief.Client_ID
        //			,Buying_Brief.Product_ID
        //			,Buying_Brief.Description CampaignName
        //			,Buying_Brief.Media_Sub_Type MediaSubType
        //			,Spot_Plan.Net_Cost
        //			,isnull(Spots_Match.Length, Spot_Plan.Length) [Length]
        //			,Spot_Plan.Media_ID
        //			,isnull(Spots_Match.SP_Show_Date,Spot_Plan.Show_Date) Show_Date
        //			,Spot_Plan.Start_Time
        //			,Spot_Plan.End_Time
        //			,isnull(Spots_Match.Actual_Rating,Spot_Plan.Rating) Rating
        //			,Spot_Plan.Spots
        //			,Spot_Plan.Program BookingProgramName
        //			,Spot_Plan.Package SpotType
        //			,Material.Thai_Name MaterialName
        //			,Media_Vendor.English_Name VendorName
        //		FROM Spot_Plan   with(nolock)
        //		LEFT JOIN Spots_Match   with(nolock)
        //			ON (Spots_Match.Buying_Brief_ID = Spot_Plan.Buying_Brief_ID) 
        //				AND (Spots_Match.Market_ID = Spot_Plan.Market_ID) 
        //				AND (Spots_Match.[Version] = Spot_Plan.[Version]) 
        //				AND (Spots_Match.Item = Spot_Plan.Item) 
        //				AND (Spots_Match.SP_Show_Date = Spot_Plan.Show_Date) 
        //				AND (Spots_Match.ID = Spot_Plan.ID) 
        //				AND (Spots_Match.[Status] = Spot_Plan.[Status])
        //		INNER JOIN Material
        //			ON Material.Material_ID = Spot_Plan.Material_ID
        //		INNER JOIN Media_Vendor
        //			ON Media_Vendor.Media_Vendor_ID = Spot_Plan.Media_Vendor_ID
        //		INNER JOIN Buying_Brief		
        //			ON Spot_Plan.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
        //		INNER JOIN #Client UserPermission
        //			ON UserPermission.Client_ID = Buying_Brief.Client_ID 
        //		WHERE isnull(Spots_Match.SP_Show_Date,Spot_Plan.Show_Date) BETWEEN @S AND @E
        //			AND LEN(Spot_Plan.Version) = 1
        //			AND Buying_Brief.Media_Sub_Type IN ('" + m_strMedaiSubType.Replace(",", "','") + @"')	
        //----------------------------------
        //-- Unknow Bonus
        //----------------------------------
        //		UNION ALL		
        //		SELECT 
        //			Spots_Match.Buying_Brief_ID
        //			,Buying_Brief.Late_Brief
        //			,Buying_Brief.Primary_Target 
        //			,Buying_Brief.Client_ID
        //			,Buying_Brief.Product_ID
        //			,Buying_Brief.Description CampaignName
        //			,Buying_Brief.Media_Sub_Type MediaSubType
        //			, 0 Net_Cost
        //			,Spots_Match.Length
        //			,Spots_Match.Media_ID
        //			,Spots_Match.Show_Date
        //			,Spots_Match.Actual_Time Start_Time
        //			,Spots_Match.Actual_Time End_Time
        //			,Spots_Match.Actual_Rating 
        //			, 1 Spots
        //			,Spots_Match.[Program_Name] BookingProgramName
        //			,'Unknown Bonus' SpotType
        //			,PDNO MaterialName
        //			,Media_Vendor.English_Name VendorName
        //		FROM Spots_Match  with(nolock)
        //			INNER JOIN Media_Vendor
        //				ON Media_Vendor.Media_Vendor_ID = Spots_Match.Media_Vendor
        //			INNER JOIN Buying_Brief		
        //				ON Spots_Match.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
        //			INNER JOIN #Client UserPermission
        //				ON UserPermission.Client_ID = Buying_Brief.Client_ID 
        //		WHERE Spots_Match.Flag_Bonus = 1 AND Spots_Match.Actual_Rating IS NOT NULL
        //			AND Spots_Match.Show_Date BETWEEN @S AND @E	
        //			AND Buying_Brief.Media_Sub_Type IN ('" + m_strMedaiSubType.Replace(",", "','") + @"')			
        //			
        //		) Spots 
        //	INNER JOIN Target 
        //		ON Spots.Primary_Target = Target.Target_ID
        //	INNER JOIN Client
        //		ON Spots.Client_ID = Client.Client_ID
        //	INNER JOIN Product
        //		ON Spots.Product_ID = Product.Product_ID
        //	INNER JOIN Media
        //		ON Media.Media_ID = Spots.Media_ID
        //WHERE Spots.Rating IS NOT NULL
        //GROUP BY 
        //	 Target.Short_Name
        //	, Media.English_Name
        //	, Product.English_Name 
        //	, dbo.GetDayPart(Spots.Start_Time,Spots.End_Time,Spots.Show_Date)
        //	, Client.English_Name
        //	, Spots.BookingProgramName
        //	, Spots.SpotType
        //	, Spots.MaterialName
        //	, Spots.CampaignName
        //	, Spots.MediaSubType
        //	, Spots.VendorName
        //	, Spots.Late_Brief
        //	, Spots.Buying_Brief_ID
        //	, Spots.Length
        //	, RIGHT('0'+CONVERT(VARCHAR(2),MONTH(Spots.Show_Date)),2)
        //		+ RIGHT('0'+CONVERT(VARCHAR(2),dbo.fn_ISOWK(Spots.Show_Date)),2)
        //
        //            ", GRM.UTL.XMLUtil.ConvertDataTableToXML(dt));
        //                da = new SqlDataAdapter(strSQL, conn);
        //                da.SelectCommand.Parameters.Add("@U", SqlDbType.VarChar).Value = UserName.Replace(".", "");
        //                da.SelectCommand.Parameters.Add("@P", SqlDbType.VarChar).Value = Password;
        //                da.SelectCommand.Parameters.Add("@S", SqlDbType.VarChar).Value = m_strDateFrom;
        //                da.SelectCommand.Parameters.Add("@E", SqlDbType.VarChar).Value = m_strDateTo;
        //                da.SelectCommand.CommandTimeout = 0;
        //                m_dtExecuteTime = DateTime.Now;
        //                da.Fill(ds);
        //                return ds;
        //            }
        //            catch (Exception ex)
        //            {
        //                GMessage.MessageError(ex);
        //                return null;
        //            }


        //        }
        DateTime m_dtExecuteTime = new DateTime();
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
            gvDetail.Fields.Add("LateBrief", PivotArea.RowArea);
            gvDetail.Fields.Add("ClientName", PivotArea.RowArea);
            gvDetail.Fields.Add("TargetName", PivotArea.RowArea);
            gvDetail.Fields.Add("DayPartName", PivotArea.RowArea);


            // row fields

            //=====================================================================================================
            // DC Page
            //=====================================================================================================

            gvDetail.Fields.Add("Buying_Brief_ID", PivotArea.FilterArea);
            gvDetail.Fields.Add("Length", PivotArea.FilterArea);
            gvDetail.Fields.Add("Media", PivotArea.FilterArea);
            gvDetail.Fields.Add("ProductName", PivotArea.FilterArea);
            gvDetail.Fields.Add("BookingProgramName", PivotArea.FilterArea);
            gvDetail.Fields.Add("SpotType", PivotArea.FilterArea);
            gvDetail.Fields.Add("MaterialName", PivotArea.FilterArea);
            gvDetail.Fields.Add("CampaignName", PivotArea.FilterArea);
            gvDetail.Fields.Add("MediaSubType", PivotArea.FilterArea);
            gvDetail.Fields.Add("VendorName", PivotArea.FilterArea);

            //=====================================================================================================
            // DC Column
            //=====================================================================================================
            gvDetail.Fields.Add("MonthWeek", PivotArea.ColumnArea);

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


        }
        private void SetFiledFormat()
        {
            SetFieldsN0(gvDetail.Fields["Cost"]);
            SetFieldsN0(gvDetail.Fields["Spots"]);
            if (m_strMedaiSubType.IndexOf("CS") != -1)
            {
                SetFieldsN4(gvDetail.Fields["Rating"]);
                SetFieldsN4(gvDetail.Fields["Rating30sec"]);
                //SetFieldsN4(gvDetail.Fields["CPRP"]);
            }
            else
            {
                SetFieldsN2(gvDetail.Fields["Rating"]);
                SetFieldsN2(gvDetail.Fields["Rating30sec"]);
                //SetFieldsN2(gvDetail.Fields["CPRP"]);
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
                    command.CommandText = "dbo.MPA_CPRPMonitoringByWeek_SetSpotMatch";
                    command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.Add("@UserID", SqlDbType.NVarChar, 50).Value = UserName;
                    command.Parameters.Add("@MediaSubType", SqlDbType.NVarChar, 10).Value = m_strMedaiSubType.Replace(",", "','");
                    command.Parameters.Add("@ClientName", SqlDbType.NVarChar).Value = ClientName;
                    command.Parameters.Add("@StartDate", SqlDbType.NVarChar, 8).Value = m_strDateFrom;
                    command.Parameters.Add("@EndDate", SqlDbType.NVarChar, 8).Value = m_strDateTo;
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


                        if (strMaxMatch != string.Empty)
                        {
                            lbSportMatch.Text = strMinMatch + " - " + strMaxMatch;
                            gvDesc.Rows[0].Cells[3].Value = strMinMatch + " to " + strMinMatch;
                        }
                        else
                        {
                            lbSportMatch.Text = "NONE";
                            gvDesc.Rows[0].Cells[3].Value = "NONE";
                        }
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


        //                if (strMaxMatch != string.Empty)
        //                {
        //                    lbSportMatch.Text = strMinMatch + " - " + strMaxMatch;
        //                    gvDesc.Rows[0].Cells[3].Value = strMinMatch + " to " + strMinMatch;
        //                }
        //                else
        //                {
        //                    lbSportMatch.Text = "NONE";
        //                    gvDesc.Rows[0].Cells[3].Value = "NONE";
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                GMessage.MessageError(ex);
        //            }



        //        }
        private void MPA003_MediaSpending_Load(object sender, System.EventArgs e)
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

        private void btnTotal_Click(object sender, EventArgs e)
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
                //SetSpotMatch();
            }
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                //axDetail.ExportToExcel(saveFileDialog.FileName);
                GMessage.MessageInfo("Exporting Completed.");
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
                        Cursor = Cursors.WaitCursor;
                        m_strDateFrom = frmSearch.DateFrom.ToString("yyyyMMdd");
                        m_strDateTo = frmSearch.DateTo.ToString("yyyyMMdd");
                        m_strMedaiSubType = frmSearch.MediaSubType;
                        m_gvClient = frmSearch.DataGridClient;
                        m_gvProduct = frmSearch.DataGridProduct;
                        m_dgvBuyingBrief = frmSearch.DataGridBuyingBrief;
                        
                        //DataLoading();
                        //SetSpotMatch();
                        //gvDesc.Rows[0].Cells[1].Value = frmSearch.DateFrom.ToString("dd MMM yyyy");
                        //gvDesc.Rows[0].Cells[2].Value = frmSearch.DateTo.ToString("dd MMM yyyy");
                        //string strClients = "";
                        //for (int i = 0; i < m_gvClient.SelectedRows.Count; i++)
                        //{
                        //    DataGridViewRow dvr = m_gvClient.SelectedRows[i];
                        //    DataRow dr = ((DataRowView)dvr.DataBoundItem).Row;
                        //    if (strClients != "")
                        //        strClients += ", ";
                        //    strClients += dr[1].ToString();
                        //}
                        //gvDesc.Rows[0].Cells[4].Value = strClients;

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

        private void gvDesc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void gvDesc_CellMouseClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {

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

        private void btnExport_Click_1(object sender, EventArgs e)
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
                var test = excelWorkbook.Sheets.get_Item(1);
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
        }

        private void chkHideRowTotal_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHideRowTotal.Checked)
            {
                gvDetail.OptionsView.ShowRowTotals = false;
                gvDetail.OptionsView.ShowRowGrandTotals = false;
            }
            else
            {
                gvDetail.OptionsView.ShowRowTotals = true;
                gvDetail.OptionsView.ShowRowGrandTotals = true;
            }
        }

        private void chkHideColTotal_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHideColTotal.Checked)
            {
                gvDetail.OptionsView.ShowColumnTotals = false;
                gvDetail.OptionsView.ShowColumnGrandTotals = false;
            }
            else
            {
                gvDetail.OptionsView.ShowColumnTotals = true;
                gvDetail.OptionsView.ShowColumnGrandTotals = true;
            }
        }
    }
}
