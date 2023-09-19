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
using GroupM.DBAccess;

namespace  GroupM.Minder
{
    public partial class MPA003_MediaSpending2 : Form
    {
        
        

        string m_strScreenName = "MediaSpending";
        string m_strConnection = Connection.ConnectionStringMinder;
        
        string m_strDateFrom = "yyyyMMdd";
        string m_strDateTo = "yyyyMMdd";
        string m_strMediaType = "";
        string m_strMediaSubType = "";
        string m_strRows = "LateBrief,TargetName,DayPartName";
        string m_strFilter = "ClientName,Buying_Brief_ID,CampaignStartDate,CampaignEndDate,MonthWeek,ShowDate,OwnDayPart,DayOfWeek,Day,Month,Year,StartTime,Length,Media,ProductName,BookingProgramName,MatchedProgramName,ProgramType,CopyLineName,SpotType,MaterialName,CampaignName,MediaSubType,VendorName,MasterVendorName,PIB,Adex_Cost,Spots,Rating30sec";
        string m_strColumns = "Version";
        string m_strData = "Cost,Rating,CPRP";
        bool m_highSpeed = true;
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

        // ========== Add by atiwat.t [2015-07-16] ==========
        int m_intSearchCondition;
        string m_strMonthFrom = string.Empty;
        string m_strMonthTo = string.Empty;
        string m_strMonth = string.Empty;

        string m_strYearFrom = string.Empty;
        string m_strYearTo = string.Empty;
        string m_strYear = string.Empty;
        // ==================================================

        enum eCol
        {
            LateBrief
            ,
            ClientName
                ,
            TargetName
                ,
            DayPartName
                ,
            Version
                ,
            Buying_Brief_ID
                ,
            CampaignStartDate
                ,
            CampaignEndDate
                ,
            MonthWeek
                ,
            ShowDate
                ,
            OwnDayPart
                ,
            DayOfWeek
                ,
            Day
                ,
            Month
                ,
            Year
                ,
            StartTime
                ,
            Length
                ,
            Media
                ,
            ProductName
                ,
            BookingProgramName
                ,
            MatchedProgramName
                ,
            ProgramType
                ,
            CopyLineName
                ,
            SpotType
                ,
            MaterialName
                ,
            CampaignName
                ,
            MediaSubType
                ,
            VendorName
                ,
            MasterVendorName
                ,
            PIB
                ,
            PIB_No
                ,
            TSB
                ,
            Adex_Cost
                ,
            Cost
                ,
            Spots
                ,
            Rating
                ,
            Rating30sec
                ,
            CPRP
                ,
            RateCardCost
        }
        string[] m_strAllColumn = {"LateBrief"
            ,"ClientName"
            ,"TargetName"
            ,"DayPartName"
            ,"Version"
            ,"Buying_Brief_ID"
            ,"CampaignStartDate"
            ,"CampaignEndDate"
            ,"MonthWeek"
            ,"ShowDate"
            ,"OwnDayPart"
            ,"DayOfWeek"
            ,"Day"
            ,"Month"
            ,"Year"
            ,"StartTime"
            ,"Length"
            ,"Media"
            ,"ProductName"
            ,"BookingProgramName"
            ,"MatchedProgramName"
            ,"ProgramType"
            ,"CopyLineName"
            ,"SpotType"
            ,"MaterialName"
            ,"CampaignName"
            ,"MediaSubType"
            ,"VendorName"
            ,"MasterVendorName"
            ,"PIB"
            ,"PIB_No"
            ,"TSB"
            ,"Adex_Cost"
            ,"Cost"
            ,"Spots"
            ,"Rating"
            ,"Rating30sec"
            ,"CPRP"
            ,"RateCardCost"};

        public MPA003_MediaSpending2(string strUserName, string strPassword)
        {
            this.UserName = strUserName;
            this.Password = strPassword;
            InitializeComponent();
        }

        private DataSet GetSource()
        {
            //if (Connection.USERID == "Thapanee.T")
            //{
            try
            {
                string Value = "";

                //BuyingBrief
                if (m_iSelectedTabActive == 2)
                {
                    for (int i = 0; i < m_dgvBuyingBrief.SelectedRows.Count; i++)
                    {
                        DataRow dr = ((DataRowView)((DataGridViewRow)m_dgvBuyingBrief.SelectedRows[i]).DataBoundItem).Row;
                        Value = Value + dr.ItemArray.GetValue(0).ToString() + ",";
                    }
                }
                //Product
                else if (m_iSelectedTabActive == 1)
                {
                    for (int i = 0; i < m_gvProduct.SelectedRows.Count; i++)
                    {
                        DataRow dr = ((DataRowView)((DataGridViewRow)m_gvProduct.SelectedRows[i]).DataBoundItem).Row;
                        if (this.rdoHighSpeed.Checked == true)
                        {
                            Value = Value + dr.ItemArray.GetValue(1).ToString() + ",";
                        }
                        else if (this.rdoRealTime.Checked == true)
                        {
                            Value = Value + dr.ItemArray.GetValue(0).ToString() + ",";
                        }
                       // Value = Value + dr.ItemArray.GetValue(1).ToString() + ",";
                    }
                }
                //Client
                else
                {
                    for (int i = 0; i < m_gvClient.SelectedRows.Count; i++)
                    {
                        DataRow dr = ((DataRowView)((DataGridViewRow)m_gvClient.SelectedRows[i]).DataBoundItem).Row;
                        if (this.rdoHighSpeed.Checked == true)
                        {
                            Value = Value + dr.ItemArray.GetValue(1).ToString() + ",";
                        }
                        else if(this.rdoRealTime.Checked == true)
                        {
                            Value = Value + dr.ItemArray.GetValue(0).ToString() + ",";
                        }
                    }
                }
                if (Value != null && Value != "")
                {
                    if (m_intSearchCondition == 2)
                    {
                        m_strDateFrom = m_strYearFrom + m_strMonthFrom + "01";
                        m_strDateTo = m_strYearTo + m_strMonthTo + DateTime.DaysInMonth(Convert.ToInt32(m_strYearTo), Convert.ToInt32(m_strMonthTo)).ToString("00");
                    }
                    else if (m_intSearchCondition == 3)
                    {
                        m_strDateFrom = m_strYear + m_strMonth + "01";
                        m_strDateTo = m_strYear + m_strMonth + DateTime.DaysInMonth(Convert.ToInt32(m_strYear), Convert.ToInt32(m_strMonth)).ToString("00");
                    }


                    string tabString = "";
                    DBManager db = new DBManager();
                    if (this.rdoRealTime.Checked == true)
                    {
                        using (SqlConnection connection = new SqlConnection(Connection.ConnectionStringMinder))
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            connection.Open();
                            command.CommandTimeout = 0;

                            //command.CommandType = CommandType.StoredProcedure;
                            //command.CommandText = "dbo.MPA_MediaSpendingWithMarketRate";
                            //command.Parameters.Add("@UserID", SqlDbType.NVarChar, 50).Value = UserName;
                            ////command.Parameters.Add("@Function", SqlDbType.NVarChar, 10).Value = m_iSelectedTabActive;
                            //command.Parameters.Add("@MediaSubType", SqlDbType.NVarChar, 10).Value = m_strMedaiSubType;
                            //command.Parameters.Add("@ClientName", SqlDbType.NVarChar, 500).Value = Value.Substring(0, Value.Length - 1);
                            //command.Parameters.Add("@StartDate", SqlDbType.NVarChar, 8).Value = m_strDateFrom;
                            //command.Parameters.Add("@EndDate", SqlDbType.NVarChar, 8).Value = m_strDateTo;


                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = "dbo.MPA_MediaSpendingWithMarketRate_New";
                            command.Parameters.Add("@UserID", SqlDbType.NVarChar, 50).Value = UserName;
                            command.Parameters.Add("@Function", SqlDbType.NVarChar, 10).Value = m_iSelectedTabActive;
                            command.Parameters.Add("@MediaSubType", SqlDbType.NVarChar, 10).Value = m_strMediaSubType;
                            command.Parameters.Add("@Value", SqlDbType.NVarChar).Value = Value.Substring(0, Value.Length - 1);
                            command.Parameters.Add("@StartDate", SqlDbType.NVarChar, 8).Value = m_strDateFrom;
                            command.Parameters.Add("@EndDate", SqlDbType.NVarChar, 8).Value = m_strDateTo;


                            tabString = "";
                            if (m_iSelectedTabActive == 2)
                            {
                                tabString = "Buying Brief";
                            }
                            else if (m_iSelectedTabActive == 1)
                            {
                                tabString = "Product";
                            }
                            else if (m_iSelectedTabActive == 0)
                            {
                                tabString = "Client";
                            }

                            db = new DBManager();
                            db.InsertLog(UserName, SystemInformation.ComputerName, this.Name, "RealTime search", "From " + m_strDateFrom + " To " + m_strDateTo + ", By MediaSubType : " + m_strMediaSubType + " From : " + tabString);

                            DataSet ds = new DataSet();
                            using (SqlDataAdapter da = new SqlDataAdapter(command))
                            {
                                da.Fill(ds);
                                return ds;
                            }

                            
                            
                        }
                    }
                    else if (this.rdoHighSpeed.Checked == true)
                    {
                        using (SqlConnection connection = new SqlConnection(Connection.ConnectionStringMPA))
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            connection.Open();
                            command.CommandTimeout = 0;

                            string strCommand = "";
                            if (m_iSelectedTabActive == 0)
                            {
                                strCommand = @" SELECT  * FROM Data_MediaSpending
                                                WHERE   CHARINDEX(',' + MediaSubType + ',' , ',' + '" + m_strMediaSubType + @"' + ',') > 0
                                                AND     CHARINDEX(',' + ClientName + ',' , ',' + '" + Value.Substring(0, Value.Length - 1) + @"' + ',') > 0
                                                AND     ShowDate BETWEEN '" + m_strDateFrom + @"' AND '" + m_strDateTo + @"'
                                             ";
                            }
                            else if (m_iSelectedTabActive == 1)
                            {
                                strCommand = @" SELECT  * FROM Data_MediaSpending
                                                WHERE   CHARINDEX(',' + MediaSubType + ',' , ',' + '" + m_strMediaSubType + @"' + ',') > 0
                                                AND     CHARINDEX(',' + ProductName + ',' , ',' + '" + Value.Substring(0, Value.Length - 1) + @"' + ',') > 0
                                                AND     ShowDate BETWEEN '" + m_strDateFrom + @"' AND '" + m_strDateTo + @"'
                                             ";
                            }
                            else if (m_iSelectedTabActive == 2)
                            {
                                strCommand = @" SELECT  * FROM Data_MediaSpending
                                                WHERE   CHARINDEX(',' + MediaSubType + ',' , ',' + '" + m_strMediaSubType + @"' + ',') > 0
                                                AND     CHARINDEX(',' + Buying_Brief_ID + ',' , ',' + '" + Value.Substring(0, Value.Length - 1) + @"' + ',') > 0
                                                AND     ShowDate BETWEEN '" + m_strDateFrom + @"' AND '" + m_strDateTo + @"'
                                             ";
                            }
                            

//                            string strCommand = @"  SELECT  * FROM Data_MediaSpending
//                                                    WHERE   CHARINDEX(',' + MediaSubType + ',' , ',' + 'TV,CS' + ',') > 0
//                                                    AND     CHARINDEX(',' + ProductName + ',' , ',' + '" + Value.Substring(0, Value.Length - 1) + @"' + ',') > 0
//                                                    AND     ShowDate BETWEEN '" + m_strDateFrom + @"' AND '" + m_strDateTo + @"'
//                                                ";



                            command.CommandText = strCommand;
                            command.CommandType = CommandType.Text;

                            tabString = "";
                            if (m_iSelectedTabActive == 2)
                            {
                                tabString = "Buying Brief";
                            }
                            else if (m_iSelectedTabActive == 1)
                            {
                                tabString = "Product";
                            }
                            else if (m_iSelectedTabActive == 0)
                            {
                                tabString = "Client";
                            }

                            db = new DBManager();
                            db.InsertLog(UserName, SystemInformation.ComputerName, this.Name, "HighSpeed search", "From " + m_strDateFrom + " To " + m_strDateTo + ", By MediaSubType : " + m_strMediaSubType + " From : " + tabString);

                            DataSet ds = new DataSet();
                            using (SqlDataAdapter da = new SqlDataAdapter(command))
                            {
                                da.Fill(ds);
                                return ds;
                            }
                        }
                    }

                    
                    




                    //                    //Test
                    //                    m_strConnection = @"Data Source=BKKSQLP01101\SQLINS01_2008R2;database=MPA;User id=bkkit;Password=Groupm#03;";

                    //                    using (SqlConnection connection = new SqlConnection(m_strConnection))
                    //                    using (SqlCommand command = connection.CreateCommand())
                    //                    {
                    //                        connection.Open();
                    //                        command.CommandTimeout = 0;

                    //                        if (this.rdoRealTime.Checked == true)
                    //                        {
                    //                            command.CommandType = CommandType.StoredProcedure;
                    //                            command.CommandText = "dbo.MPA_GetMeidaSpending";
                    //                            command.Parameters.Add("@Function", SqlDbType.NVarChar, 10).Value = m_iSelectedTabActive;
                    //                            command.Parameters.Add("@MediaSubType", SqlDbType.NVarChar, 10).Value = m_strMedaiSubType;
                    //                            command.Parameters.Add("@Value", SqlDbType.NVarChar, 500).Value = Value.Substring(0, Value.Length - 1);
                    //                            command.Parameters.Add("@StartDate", SqlDbType.NVarChar, 8).Value = m_strDateFrom;
                    //                            command.Parameters.Add("@EndDate", SqlDbType.NVarChar, 8).Value = m_strDateTo;

                    //                        }
                    //                        else if (this.rdoHighSpeed.Checked == true)
                    //                        {
                    //                            string strCommand = @"  SELECT  * FROM Data_MediaSpending
                    //                                                    WHERE   CHARINDEX(',' + MediaSubType + ',' , ',' + '" + m_strMedaiSubType + @"' + ',') > 0
                    //                                                    AND     CHARINDEX(',' + ClientName + ',' , ',' + '" + Value.Substring(0, Value.Length - 1) + @"' + ',') > 0
                    //                                                    AND     ShowDate BETWEEN '" + m_strDateFrom + @"' AND '" + m_strDateTo  + @"'
                    //                                                ";
                    //                            command.CommandText = strCommand;
                    //                            command.CommandType = CommandType.Text;
                    //                        }

                    //                        DataSet ds = new DataSet();
                    //                        using (SqlDataAdapter da = new SqlDataAdapter(command))
                    //                        {
                    //                            da.Fill(ds);
                    //                            return ds;
                    //                        }
                    //                    }

                }
                return null;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return null;
            }

            //}








            //try
            //{
            //    string ClientName = "";
            //    string ProductId = "";
            //    string BuyingBrief = "";


            //    //BuyingBrief
            //    if (m_iSelectedTabActive == 2)
            //    {
            //        for (int i = 0; i < m_dgvBuyingBrief.SelectedRows.Count; i++)
            //        {
            //            DataRow dr = ((DataRowView)((DataGridViewRow)m_dgvBuyingBrief.SelectedRows[i]).DataBoundItem).Row;
            //            BuyingBrief = BuyingBrief + dr.ItemArray.GetValue(0).ToString() + ",";
            //            //BuyingBrief = dr.ItemArray.GetValue(0).ToString();
            //        }
            //        using (SqlConnection connection = new SqlConnection(m_strConnection))
            //        using (SqlCommand command = connection.CreateCommand())
            //        {
            //            command.CommandText = "dbo.MPA_MediaSpending_By_BuyingBriefId_tmp";
            //            command.CommandTimeout = 0;
            //            command.CommandType = CommandType.StoredProcedure;
            //            command.Parameters.Add("@UserID", SqlDbType.NVarChar, 50).Value = UserName;
            //            command.Parameters.Add("@MediaSubType", SqlDbType.NVarChar, 10).Value = m_strMedaiSubType;
            //            //command.Parameters.Add("@ClientName", SqlDbType.NVarChar, 500).Value = ClientName;
            //            command.Parameters.Add("@BuyingBrief", SqlDbType.NVarChar, 500).Value = BuyingBrief.Substring(0,BuyingBrief.Length - 1);
            //            command.Parameters.Add("@StartDate", SqlDbType.NVarChar, 8).Value = m_strDateFrom;
            //            command.Parameters.Add("@EndDate", SqlDbType.NVarChar, 8).Value = m_strDateTo;
            //            DataSet ds = new DataSet();
            //            using (SqlDataAdapter da = new SqlDataAdapter(command))
            //            {
            //                da.Fill(ds);
            //                return ds;
            //            }
            //        }
            //    }
            //    else if (m_iSelectedTabActive == 1)
            //    {
            //        for (int i = 0; i < m_gvProduct.SelectedRows.Count; i++)
            //        {
            //            DataRow dr = ((DataRowView)((DataGridViewRow)m_gvProduct.SelectedRows[i]).DataBoundItem).Row;
            //            ProductId = ProductId + dr.ItemArray.GetValue(0).ToString() + ",";
            //            //ProductId = dr.ItemArray.GetValue(0).ToString();
            //        }
            //        using (SqlConnection connection = new SqlConnection(m_strConnection))
            //        using (SqlCommand command = connection.CreateCommand())
            //        {
            //            command.CommandText = "dbo.MPA_MediaSpending_By_Product";
            //            command.CommandTimeout = 0;
            //            command.CommandType = CommandType.StoredProcedure;
            //            command.Parameters.Add("@UserID", SqlDbType.NVarChar, 50).Value = UserName;
            //            command.Parameters.Add("@MediaSubType", SqlDbType.NVarChar, 10).Value = m_strMedaiSubType;
            //            //command.Parameters.Add("@ClientName", SqlDbType.NVarChar, 500).Value = ClientName;
            //            command.Parameters.Add("@ProductId", SqlDbType.NVarChar, 500).Value = ProductId.Substring(0, ProductId.Length - 1);
            //            command.Parameters.Add("@StartDate", SqlDbType.NVarChar, 8).Value = m_strDateFrom;
            //            command.Parameters.Add("@EndDate", SqlDbType.NVarChar, 8).Value = m_strDateTo;
            //            DataSet ds = new DataSet();
            //            using (SqlDataAdapter da = new SqlDataAdapter(command))
            //            {
            //                da.Fill(ds);
            //                return ds;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        for (int i = 0; i < m_gvClient.SelectedRows.Count; i++)
            //        {
            //            DataRow dr = ((DataRowView)((DataGridViewRow)m_gvClient.SelectedRows[i]).DataBoundItem).Row;
            //            ClientName = ClientName + dr.ItemArray.GetValue(0).ToString() + ",";
            //        }

            //        using (SqlConnection connection = new SqlConnection(m_strConnection))
            //        using (SqlCommand command = connection.CreateCommand())
            //        {

            //            connection.Open();
            //            command.CommandTimeout = 0;
            //            command.CommandText = "dbo.MPA_MediaSpending";
            //            command.CommandType = CommandType.StoredProcedure;
            //            command.Parameters.Add("@UserID", SqlDbType.NVarChar, 50).Value = UserName;
            //            command.Parameters.Add("@MediaSubType", SqlDbType.NVarChar, 10).Value = m_strMedaiSubType;
            //            command.Parameters.Add("@ClientName", SqlDbType.NVarChar, 500).Value = ClientName.Substring(0, ClientName.Length - 1);
            //            command.Parameters.Add("@StartDate", SqlDbType.NVarChar, 8).Value = m_strDateFrom;
            //            command.Parameters.Add("@EndDate", SqlDbType.NVarChar, 8).Value = m_strDateTo;

            //            //command.ExecuteNonQuery();

            //            DataSet ds = new DataSet();
            //            using (SqlDataAdapter da = new SqlDataAdapter(command))
            //            {
            //                da.Fill(ds);
            //                return ds;
            //            }

            //        }
            //    }

            //}
            //catch (Exception)
            //{
            //    return null;
            //}



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
        //                DECLARE @xml XML
        //                SET @xml = '{0}'
        //                SELECT  
        //                       Tbl.Col.value('Client_ID[1]', 'varchar(20)') Client_ID,  
        //                       Tbl.Col.value('ClientName[1]', 'varchar(200)') English_Name
        //                INTO #Client
        //                FROM   @xml.nodes('//row') Tbl(Col)  
        //                
        //                
        //--DECLARE @CreateBy VARCHAR(100)
        //--DECLARE @U VARCHAR(100)
        //--DECLARE @P VARCHAR(100)
        //--DECLARE @S VARCHAR(100)
        //--DECLARE @E VARCHAR(100)

        //--SET @CreateBy = 'pariwatk'
        //--SET @U = 'pariwatk'
        //--SET @P = 'Fank7813'
        //--SET @S = '20110601'
        //--SET @E = '20110601'

        //SELECT * FROM 
        //(
        //SELECT 
        //    CASE Buying_Brief.Late_Brief WHEN 0 THEN 'NO' ELSE 'YES' END LateBrief
        //    , Client.English_Name ClientName
        //    , Target.Short_Name TargetName
        //    , dbo.GetDayPart(Spot_Plan.Start_Time,Spot_Plan.End_Time,Spot_Plan.Show_Date) DayPartName
        //    , Spot_Plan.Buying_Brief_ID
        //    , Buying_Brief.Campaign_Start_Date AS CampaignStartDate
        //    , Buying_Brief.Campaign_End_Date AS CampaignEndDate
        //    , CASE LEN(Spot_Plan.Version) WHEN 2 THEN '1-APPROVE (PLANNED)' ELSE '2-LATEST (EXECUTING)' END [Version]
        //    , Spot_Plan.Length
        //    , RIGHT('0'+CONVERT(VARCHAR(2),MONTH(Spot_Plan.Show_Date)),2)
        //        + RIGHT('0'+CONVERT(VARCHAR(2),dbo.fn_ISOWK(Spot_Plan.Show_Date)),2) MonthWeek
        //    , Spot_Plan.Show_Date [ShowDate]
        //    , dbo.GetOwnDayPart(Spot_Plan.Start_Time,Spot_Plan.End_Time,Spot_Plan.Show_Date,@CreateBy) OwnDayPart
        //    , dbo.fn_DayofWeek(Spot_Plan.Show_Date) [DayOfWeek]
        //    , DAY(CONVERT(DATETIME,Spot_Plan.Show_Date)) [Day]
        //    , MONTH(CONVERT(DATETIME,Spot_Plan.Show_Date)) [Month]
        //    , YEAR(CONVERT(DATETIME,Spot_Plan.Show_Date)) [Year]
        //    , LEFT(Spot_Plan.Start_Time + '00',2)+':'
        //    + RIGHT(LEFT(Spot_Plan.Start_Time + '00',4),2)+':' 
        //    + RIGHT(LEFT(Spot_Plan.Start_Time + '00',6),2) StartTime
        //    , Media.English_Name Media
        //    , Product.English_Name ProductName
        //    , Spot_Plan.Program BookingProgramName
        //    , '' MatchedProgramName
        //    , isnull(Program_Type.Short_Name,'UNKNOWN') ProgramType
        //    , '' CopyLineName
        //    , Spot_Plan.Package SpotType
        //    , Material.Thai_Name MaterialName
        //    , Buying_Brief.Description CampaignName
        //    , Buying_Brief.Media_Sub_Type MediaSubType
        //    , Media_Vendor.English_Name VendorName
        //    , MasterVendor.English_Name MasterVendorName
        //    , NULL AS PIB
        //    , 0 AS Adex_Cost
        //    , SUM(Spot_Plan.Spots) Spots
        //    , SUM(Spot_Plan.Net_Cost) Cost
        //    , CONVERT(decimal(18,4),SUM(Spot_Plan.Rating)) Rating
        //    , CONVERT(decimal(18,4),SUM((Spot_Plan.Rating * Spot_Plan.Length)/30 )) Rating30sec
        //    , CASE SUM((Spot_Plan.Rating * Spot_Plan.Length)/30 )  WHEN 0 THEN 0 ELSE
        //    SUM(Spot_Plan.Net_Cost) / SUM((Spot_Plan.Rating * Spot_Plan.Length)/30 ) END CPRP 
        //FROM Spot_Plan with(nolock)
        //    INNER JOIN Buying_Brief		
        //        ON Spot_Plan.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
        //    INNER JOIN #Client UserPermission
        //        ON UserPermission.Client_ID = Buying_Brief.Client_ID  collate SQL_Latin1_General_CP1_CI_AS
        //    INNER JOIN Target 
        //        ON Buying_Brief.Primary_Target = Target.Target_ID
        //    INNER JOIN Client
        //        ON Buying_Brief.Client_ID = Client.Client_ID
        //    INNER JOIN Product
        //        ON Buying_Brief.Product_ID = Product.Product_ID
        //    INNER JOIN Material
        //        ON Material.Material_ID = Spot_Plan.Material_ID
        //    INNER JOIN Media_Vendor
        //        ON Media_Vendor.Media_Vendor_ID = Spot_Plan.Media_Vendor_ID
        //    LEFT JOIN Media_Vendor MasterVendor
        //        ON Media_Vendor.Master_Group = MasterVendor.Media_Vendor_ID 
        //    INNER JOIN Media
        //        ON Media.Media_ID = Spot_Plan.Media_ID
        //    LEFT JOIN Program_Type
        //        ON Program_Type.Program_Type = Spot_Plan.Program_Type
        //WHERE 
        //    Buying_Brief.Media_Sub_Type IN ('" + m_strMedaiSubType.Replace(",", "','") + @"')
        //    AND Spot_Plan.Show_Date BETWEEN @S AND @E
        //    AND Spot_Plan.Status > 0
        //    --AND (Spot_Plan.Rating * Spot_Plan.Length)/30  <> 0
        //    --AND Buying_Brief.Client_ID ='20LORMOB'
        //GROUP BY 
        //     Target.Short_Name
        //    , Media.English_Name
        //    , Product.English_Name 
        //    , dbo.GetOwnDayPart(Spot_Plan.Start_Time,Spot_Plan.End_Time,Spot_Plan.Show_Date,@CreateBy) 
        //    , dbo.GetDayPart(Spot_Plan.Start_Time,Spot_Plan.End_Time,Spot_Plan.Show_Date) 
        //    , Client.English_Name
        //    , Spot_Plan.Program 
        //    , Program_Type.Short_Name
        //    , Spot_Plan.Package 
        //    , Material.Thai_Name 
        //    , Buying_Brief.Description
        //    , Buying_Brief.Media_Sub_Type
        //    , Media_Vendor.English_Name 
        //    , MasterVendor.English_Name 
        //    , Buying_Brief.Late_Brief
        //    , Spot_Plan.Buying_Brief_ID
        //    , Buying_Brief.Campaign_Start_Date
        //    , Buying_Brief.Campaign_End_Date
        //    , Spot_Plan.Version
        //    , Spot_Plan.Length
        //    , RIGHT('0'+CONVERT(VARCHAR(2),MONTH(Spot_Plan.Show_Date)),2)  
        //        + RIGHT('0'+CONVERT(VARCHAR(2),dbo.fn_ISOWK(Spot_Plan.Show_Date)),2) 
        //    , Spot_Plan.Show_Date  
        //    , dbo.fn_DayofWeek(Spot_Plan.Show_Date)  
        //    , DAY(CONVERT(DATETIME,Spot_Plan.Show_Date))  
        //    , MONTH(CONVERT(DATETIME,Spot_Plan.Show_Date))  
        //    , YEAR(CONVERT(DATETIME,Spot_Plan.Show_Date))  
        //    , Spot_Plan.Start_Time  

        //UNION

        //SELECT 
        //    CASE Buying_Brief.Late_Brief WHEN 0 THEN 'NO' ELSE 'YES' END LateBrief
        //    , Client.English_Name ClientName
        //    , Target.Short_Name TargetName
        //    , dbo.GetDayPart(SpotsMatch.Start_Time,SpotsMatch.End_Time,SpotsMatch.Show_Date) DayPartName
        //    , SpotsMatch.Buying_Brief_ID
        //    , Buying_Brief.Campaign_Start_Date AS CampaignStartDate
        //    , Buying_Brief.Campaign_End_Date AS CampaignEndDate
        //    , '3-ACTUAL (POST)' [Version]
        //    , SpotsMatch.Length
        //    , RIGHT('0'+CONVERT(VARCHAR(2),MONTH(SpotsMatch.Show_Date)),2)
        //        + RIGHT('0'+CONVERT(VARCHAR(2),dbo.fn_ISOWK(SpotsMatch.Show_Date)),2) MonthWeek
        //    , SpotsMatch.Show_Date [ShowDate]
        //    , dbo.GetOwnDayPart(SpotsMatch.Start_Time,SpotsMatch.End_Time,SpotsMatch.Show_Date,@CreateBy) OwnDayPart
        //    , dbo.fn_DayofWeek(SpotsMatch.Show_Date) [DayOfWeek]
        //    , DAY(CONVERT(DATETIME,SpotsMatch.Show_Date)) [Day]
        //    , MONTH(CONVERT(DATETIME,SpotsMatch.Show_Date)) [Month]
        //    , YEAR(CONVERT(DATETIME,SpotsMatch.Show_Date)) [Year]
        //    , LEFT(SpotsMatch.Start_Time + '00',2)+':'
        //    + RIGHT(LEFT(SpotsMatch.Start_Time + '00',4),2)+':' 
        //    + RIGHT(LEFT(SpotsMatch.Start_Time + '00',6),2)
        //    StartTime
        //    , Media.English_Name Media
        //    , Product.English_Name ProductName
        //    , SpotsMatch.BookingProgramName
        //    , SpotsMatch.MatchedProgramName
        //    , SpotsMatch.ProgramType
        //    , SpotsMatch.CopyLineName
        //    , SpotsMatch.SpotType
        //    , SpotsMatch.MaterialName
        //    , Buying_Brief.Description CampaignName
        //    , Buying_Brief.Media_Sub_Type MediaSubType
        //    , SpotsMatch.VendorName
        //    , SpotsMatch.MasterVendorName
        //    , PIB
        //    , SUM(Adex_Cost) AdexCost
        //    , SUM(SpotsMatch.Spots) Spots
        //    , SUM(SpotsMatch.Net_Cost) Cost
        //    , CONVERT(decimal(18,4),SUM(SpotsMatch.Actual_Rating)) Rating
        //    , CONVERT(decimal(18,4),SUM((SpotsMatch.Actual_Rating * SpotsMatch.Length)/30 )) Rating30sec
        //    , 
        //    CASE SUM((SpotsMatch.Actual_Rating * SpotsMatch.Length)/30 ) WHEN 0 THEN 0 ELSE
        //     SUM(SpotsMatch.Net_Cost) / SUM((SpotsMatch.Actual_Rating * SpotsMatch.Length)/30 ) END CPRP  
        //FROM (
        //        SELECT 
        //            Spots_Match.Buying_Brief_ID
        //            ,Spot_Plan.Net_Cost
        //            ,Spots_Match.Length
        //            ,Spot_Plan.Media_ID
        //            ,Spots_Match.SP_Show_Date Show_Date
        //            ,Spots_Match.Actual_Time Start_Time
        //            ,Spots_Match.Actual_Time End_Time
        //            ,Spots_Match.Actual_Rating
        //            ,Spot_Plan.Spots
        //            ,Spot_Plan.Program BookingProgramName
        //            ,Spots_Match.[Program_Name] MatchedProgramName
        //            ,isnull(Program_Type.Short_Name,'UNKNOWN') ProgramType
        //            ,Spots_Match.PDNO CopyLineName
        //            ,Spot_Plan.Package SpotType
        //            ,Material.Thai_Name MaterialName
        //            ,Media_Vendor.English_Name VendorName
        //            ,MasterVendor.English_Name MasterVendorName
        //            ,CASE WHEN Spots_Match.PIB = 1 
        //                THEN 'First' 
        //                ELSE CASE WHEN Spots_Match.TSB - Spots_Match.PIB = 0
        //                    THEN 'Last'
        //                    ELSE CASE WHEN Spots_Match.PIB = 2
        //                        THEN 'Second'
        //                        ELSE CASE WHEN Spots_Match.PIB = 3
        //                            THEN 'Third'
        //                            ELSE CASE WHEN Spots_Match.TSB - Spots_Match.PIB = 1
        //                                THEN 'Second to Last'
        //                                ELSE CASE WHEN Spots_Match.TSB - Spots_Match.PIB = 2
        //                                    THEN 'Third to Last'
        //                                    ELSE 'Middle'
        //                                    END
        //                                END						
        //                            END				 
        //                        END 			
        //                    END
        //                END AS  [PIB]
        //            ,Adex_Cost
        //        FROM Spot_Plan   with(nolock)
        //        INNER JOIN Spots_Match   with(nolock)
        //            ON (Spots_Match.Buying_Brief_ID = Spot_Plan.Buying_Brief_ID) 
        //                AND (Spots_Match.Market_ID = Spot_Plan.Market_ID) 
        //                AND (Spots_Match.[Version] = Spot_Plan.[Version]) 
        //                AND (Spots_Match.Item = Spot_Plan.Item) 
        //                AND (Spots_Match.SP_Show_Date = Spot_Plan.Show_Date) 
        //                AND (Spots_Match.ID = Spot_Plan.ID) 
        //                AND (Spots_Match.[Status] = Spot_Plan.[Status])
        //        INNER JOIN Material
        //            ON Material.Material_ID = Spot_Plan.Material_ID
        //        INNER JOIN Media_Vendor
        //            ON Media_Vendor.Media_Vendor_ID = Spot_Plan.Media_Vendor_ID
        //        INNER JOIN Media_Vendor MasterVendor
        //            ON Media_Vendor.Master_Group = MasterVendor.Media_Vendor_ID 
        //        INNER JOIN Buying_Brief		
        //            ON Spots_Match.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
        //        INNER JOIN #Client UserPermission
        //            ON UserPermission.Client_ID = Buying_Brief.Client_ID  collate SQL_Latin1_General_CP1_CI_AS
        //        LEFT JOIN Program_Type
        //            ON Program_Type.Program_Type = Spot_Plan.Program_Type
        //        WHERE Spots_Match.Actual_Rating IS NOT NULL AND  Spots_Match.Flag_Bonus = 0
        //        UNION ALL		
        //        SELECT 
        //            Spots_Match.Buying_Brief_ID
        //            , 0 Net_Cost
        //            ,Spots_Match.Length
        //            ,Spots_Match.Media_ID
        //            ,Spots_Match.Show_Date
        //            ,Spots_Match.Actual_Time Start_Time
        //            ,Spots_Match.Actual_Time End_Time
        //            ,Spots_Match.Actual_Rating 
        //            , 1 Spots
        //            ,Spots_Match.[Program_Name] BookingProgramName
        //            ,Spots_Match.[Program_Name] MatchedProgramName
        //            ,'BONUS' ProgramType
        //            ,Spots_Match.PDNO CopyLineName
        //            ,'Unknown Bonus' SpotType
        //            ,PDNO MaterialName
        //            ,Media_Vendor.English_Name VendorName
        //            ,MasterVendor.English_Name MasterVendorName
        //            ,CASE WHEN Spots_Match.PIB = 1 
        //                THEN 'First' 
        //                ELSE CASE WHEN Spots_Match.TSB - Spots_Match.PIB = 0
        //                    THEN 'Last'
        //                    ELSE CASE WHEN Spots_Match.PIB = 2
        //                        THEN 'Second'
        //                        ELSE CASE WHEN Spots_Match.PIB = 3
        //                            THEN 'Third'
        //                            ELSE CASE WHEN Spots_Match.TSB - Spots_Match.PIB = 1
        //                                THEN 'Second to Last'
        //                                ELSE CASE WHEN Spots_Match.TSB - Spots_Match.PIB = 2
        //                                    THEN 'Third to Last'
        //                                    ELSE 'Middle'
        //                                    END
        //                                END						
        //                            END				 
        //                        END 			
        //                    END
        //                END AS  [PIB]
        //            ,Adex_Cost
        //        FROM Spots_Match  with(nolock)
        //            INNER JOIN Media_Vendor
        //                ON Media_Vendor.Media_Vendor_ID = Spots_Match.Media_Vendor
        //            INNER JOIN Media_Vendor MasterVendor
        //                ON Media_Vendor.Master_Group = MasterVendor.Media_Vendor_ID 	
        //            INNER JOIN Buying_Brief		
        //                ON Spots_Match.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
        //            INNER JOIN #Client UserPermission
        //                ON UserPermission.Client_ID = Buying_Brief.Client_ID  collate SQL_Latin1_General_CP1_CI_AS
        //        WHERE Spots_Match.Flag_Bonus = 1 AND Spots_Match.Actual_Rating IS NOT NULL

        //        ) SpotsMatch 
        //    INNER JOIN Buying_Brief		
        //        ON SpotsMatch.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
        //    INNER JOIN #Client UserPermission
        //        ON UserPermission.Client_ID = Buying_Brief.Client_ID   collate SQL_Latin1_General_CP1_CI_AS
        //    INNER JOIN Target 
        //        ON Buying_Brief.Primary_Target = Target.Target_ID
        //    INNER JOIN Client
        //        ON Buying_Brief.Client_ID = Client.Client_ID
        //    INNER JOIN Product
        //        ON Buying_Brief.Product_ID = Product.Product_ID
        //    INNER JOIN Media
        //        ON Media.Media_ID = SpotsMatch.Media_ID
        //WHERE 
        //    Buying_Brief.Media_Sub_Type IN ('" + m_strMedaiSubType.Replace(",", "','") + @"')
        //    AND SpotsMatch.Show_Date BETWEEN @S AND @E
        //    AND SpotsMatch.Actual_Rating IS NOT NULL
        //    --AND (SpotsMatch.Actual_Rating * SpotsMatch.Length)/30   <> 0
        //    --AND Buying_Brief.Client_ID ='20LORMOB'
        //    --AND SpotsMatch.Buying_Brief_ID = '2011050387'
        //GROUP BY 
        //     Target.Short_Name
        //    , Media.English_Name
        //    , Product.English_Name 
        //    , dbo.GetOwnDayPart(SpotsMatch.Start_Time,SpotsMatch.End_Time,SpotsMatch.Show_Date,@CreateBy)
        //    , dbo.GetDayPart(SpotsMatch.Start_Time,SpotsMatch.End_Time,SpotsMatch.Show_Date)
        //    , Client.English_Name
        //    , SpotsMatch.BookingProgramName
        //    , SpotsMatch.MatchedProgramName
        //    , SpotsMatch.ProgramType
        //    , SpotsMatch.CopyLineName
        //    , SpotsMatch.SpotType
        //    , SpotsMatch.MaterialName
        //    , Buying_Brief.Description
        //    , Buying_Brief.Media_Sub_Type
        //    , SpotsMatch.VendorName
        //    , SpotsMatch.MasterVendorName
        //    , Buying_Brief.Late_Brief
        //    , SpotsMatch.Buying_Brief_ID
        //    , Buying_Brief.Campaign_Start_Date
        //    , Buying_Brief.Campaign_End_Date
        //    , SpotsMatch.Length
        //    , RIGHT('0'+CONVERT(VARCHAR(2),MONTH(SpotsMatch.Show_Date)),2)
        //        + RIGHT('0'+CONVERT(VARCHAR(2),dbo.fn_ISOWK(SpotsMatch.Show_Date)),2) 
        //    , SpotsMatch.Show_Date  
        //    , dbo.fn_DayofWeek(SpotsMatch.Show_Date)  
        //    , DAY(CONVERT(DATETIME,SpotsMatch.Show_Date)) 
        //    , MONTH(CONVERT(DATETIME,SpotsMatch.Show_Date)) 
        //    , YEAR(CONVERT(DATETIME,SpotsMatch.Show_Date))  
        //    , SpotsMatch.Start_Time 
        //    , PIB
        //) rec
        //                --ORDER BY Buying_Brief_ID
        //                --where Buying_Brief_ID = '2012070829'
        //                
        //                
        //                            ", GRM.UTL.XMLUtil.ConvertDataTableToXML(dt));
        //                da = new SqlDataAdapter(strSQL, conn);
        //                da.SelectCommand.Parameters.Add("@CreateBy", SqlDbType.VarChar).Value = UserName;
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

        private System.Threading.Thread StartThread;
        private System.Threading.Thread StopThread;
        private void fnStartThread()
        {
            //ShowLoadingScreen.ShowDialog();
        }

        private void fnStopThread()
        {
            //ShowLoadingScreen.Close();
        }

        DateTime m_dtExecuteTime = new DateTime();
        DateTime startTime;
        DateTime stopTime;
        private void DataLoading()
        {

            //=====================================================================================================
            // Pivot
            //=====================================================================================================
            // gvDetail.Fields.Clear();
            this.textBox1.Text = "   Pre and Post Buy Report,  (Loading.....)";

            bgWorker.RunWorkerAsync();


        }
        private void SetFiledFormat()
        {
            //SetFieldsN0(gvDetail.Fields["Cost"]);
            SetFieldsN0(gvDetail.Fields["Spots"]);
            if (m_strMediaSubType.IndexOf("CS") != -1)
            {
                SetFieldsN4(gvDetail.Fields["Rating"]);
                SetFieldsN4(gvDetail.Fields["Rating30sec"]);
                SetFieldsN2(gvDetail.Fields["Cost"]);
                SetFieldsN2(gvDetail.Fields["RateCardCost"]);
            }
            else
            {
                SetFieldsN2(gvDetail.Fields["Cost"]);
                SetFieldsN2(gvDetail.Fields["RateCardCost"]);
                SetFieldsN2(gvDetail.Fields["Rating"]);
                SetFieldsN2(gvDetail.Fields["Rating30sec"]);
            }
            SetFieldsN0(fieldCPRP);
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

            ", GroupM.UTL.XMLUtil.ConvertDataTableToXML(dt));
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
                {
                    lbSportMatch.Text = strMinMatch + " - " + strMaxMatch;
                    gvDesc.Rows[0].Cells[3].Value = strMinMatch + " to " + strMaxMatch;
                }
                else
                {
                    lbSportMatch.Text = "NONE";
                    gvDesc.Rows[0].Cells[3].Value = "NONE";
                }
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }



        }
        private void MPA003_MediaSpending2_Load(object sender, System.EventArgs e)
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
            frmSearch.MediaSubType = m_strMediaSubType;
            frmSearch.DataGridClient = m_gvClient;
            if (frmSearch.ShowDialog() == DialogResult.OK)
            {
                lbCondition.Text = frmSearch.DateFrom.ToString("dd MMM yyyy") + " - " + frmSearch.DateTo.ToString("dd MMM yyyy");

                m_strDateFrom = frmSearch.DateFrom.ToString("yyyyMMdd");
                m_strDateTo = frmSearch.DateTo.ToString("yyyyMMdd");
                m_strMediaSubType = frmSearch.MediaSubType;
                m_gvClient = frmSearch.DataGridClient;

                DataLoading();
                SetSpotMatch();
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                //axDetail.ExportToExcel(saveFileDialog.FileName);
                //GMessage.MessageInfo("Exporting Completed.");

                gvDetail.ExportToXlsx(saveFileDialog.FileName);
            }
        }
        private void gvDetail_CustomSummary(object sender, PivotGridCustomSummaryEventArgs e)
        {
            if (e.DataField == fieldCPRP)
            {
                double dTotalGrp = 0;
                double dTotalBudget = 0;

                PivotDrillDownDataSource ds = e.CreateDrillDownDataSource();
                for (int i = 0; i < ds.RowCount; i++)
                {
                    PivotDrillDownDataRow row = ds[i];

                    //dTotalGrp += Convert.ToDouble(row["GRPs"]) * Convert.ToDouble(row["Duration"]) / 30.00;
                    dTotalGrp += Convert.ToDouble(row["Rating30sec"]);
                    //dTotalBudget += Convert.ToDouble(row["Budget"]);
                    dTotalBudget += Convert.ToDouble(row["Cost"]);


                }
                if (dTotalGrp != 0)
                    e.CustomValue = dTotalBudget / dTotalGrp;
                else
                    e.CustomValue = 0;
            }
        }
        private string ConvertyyyyMMddToDash(string sDate)
        {
            return sDate.Substring(0, 4) + "-" + sDate.Substring(4, 2) + "-" + sDate.Substring(6, 2);
        }
        //DataRow m_drDebug ;

        MPA002_SearchCondition frmSearch = null;
        private void gvDesc_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == 0 && e.ColumnIndex == 0)
            {
                frmSearch = new MPA002_SearchCondition();

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
                frmSearch.MediaType = m_strMediaType;
                frmSearch.MediaSubType = m_strMediaSubType;
                frmSearch.DataGridClient = m_gvClient;
                frmSearch.DataGridProduct = m_gvProduct;
                frmSearch.DataGridBuyingBrief = m_dgvBuyingBrief;
                frmSearch.mainTabControl.SelectedIndex = m_iSelectedTabActive;
                frmSearch.HighSpeed = m_highSpeed;
                frmSearch.ConditionOption = m_intSearchCondition;

                if (frmSearch.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        m_strDateFrom = frmSearch.DateFrom.ToString("yyyyMMdd");
                        m_strDateTo = frmSearch.DateTo.ToString("yyyyMMdd");
                        m_strMediaType = frmSearch.MediaType;
                        m_strMediaSubType = frmSearch.MediaSubType;
                        m_gvClient = frmSearch.DataGridClient;
                        m_gvProduct = frmSearch.DataGridProduct;
                        m_dgvBuyingBrief = frmSearch.DataGridBuyingBrief;

                        // ========== Add by atiwat.t [2015-07-16] ==========
                        m_intSearchCondition = frmSearch.ConditionOption;
                        m_strMonthFrom = frmSearch.MonthFrom;
                        m_strMonthTo = frmSearch.MonthTo;
                        m_strMonth = frmSearch.Month;

                        m_strYearFrom = frmSearch.YearFrom;
                        m_strYearTo = frmSearch.YearTo;
                        m_strYear = frmSearch.Year;
                        m_highSpeed = frmSearch.HighSpeed;
                        if (m_highSpeed) this.rdoHighSpeed.Checked = true; else this.rdoRealTime.Checked = true;
                        // ==================================================

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

        private void btnSave_Click(object sender, EventArgs e)
        {

            MPA012_SaveTemplate frm = new MPA012_SaveTemplate(Connection.USERID, "MediaSpending");

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string strRows = "''";
                string strColumns = "''";
                string strData = "''";
                string strFilter = "''";
                for (int i = 0; i < gvDetail.Fields.Count; i++)
                {
                    switch (gvDetail.Fields[i].Area.ToString())
                    {
                        case "RowArea":
                            strRows += "," + gvDetail.Fields[i];
                            break;
                        case "FilterArea":
                            strFilter += "," + gvDetail.Fields[i];
                            break;
                        case "ColumnArea":
                            strColumns += "," + gvDetail.Fields[i];
                            break;
                        case "DataArea":
                            strData += "," + gvDetail.Fields[i];
                            break;
                    }
                }

                strRows = strRows.Replace("'',", "");
                strColumns = strColumns.Replace("'',", "");
                strData = strData.Replace("'',", "");
                strFilter = strFilter.Replace("'',", "");

                SqlConnection conn = new SqlConnection(Connection.ConnectionStringMPA);
                SqlCommand comm = new SqlCommand(string.Format("DELETE FROM Template WHERE TemplateName = '{0}' AND CreateBy = '{1}'", frm.TemplateName, Connection.USERID), conn);
                conn.Open();
                comm.ExecuteNonQuery();
                string strSelect = string.Format(@"
INSERT INTO Template 
(
      [TemplateScreenName]
      ,[TemplateName]
      ,[TemplateRow]
      ,[TemplateColumn]
      ,[TemplateData]
      ,[TemplateFilter]
      ,[CreateBy]
)
VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}')
", m_strScreenName
 , frm.TemplateName
 , strRows
 , strColumns
 , strData
 , strFilter
 , Connection.USERID);
                comm = new SqlCommand(strSelect, conn);
                comm.ExecuteNonQuery();
                conn.Close();
            }
            txtLoadTemplate.Text = frm.TemplateName;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                MPA013_LoadTemplate frm = new MPA013_LoadTemplate();
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                    LoadTemplate(frm.gvDetail.SelectedRows[0].Cells[1].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }
        }
        private void LoadTemplate()
        {
            LoadTemplate(m_strRows.Split(','), m_strColumns.Split(','), m_strData.Split(','), m_strFilter.Split(','));
        }
        private void LoadTemplate(string strTemplateName)
        {
            SqlConnection conn = new SqlConnection(Connection.ConnectionStringMPA);
            string strSelect = string.Format(@"SELECT * FROM Template WHERE TemplateScreenName = 'MediaSpending' AND TemplateName = @TemplateName AND CreateBy = @CreateBy");
            SqlDataAdapter sda = new SqlDataAdapter(strSelect, conn);
            sda.SelectCommand.Parameters.Add("@TemplateName", SqlDbType.VarChar).Value = strTemplateName;
            sda.SelectCommand.Parameters.Add("@CreateBy", SqlDbType.VarChar).Value = Connection.USERID;
            DataSet ds = new DataSet();
            sda.Fill(ds);

            txtLoadTemplate.Text = strTemplateName;
            DataRow dr = ds.Tables[0].Rows[0];
            m_strRows = dr["TemplateRow"].ToString();
            m_strColumns = dr["TemplateColumn"].ToString();
            m_strData = dr["TemplateData"].ToString();
            m_strFilter = dr["TemplateFilter"].ToString();
            LoadTemplate(m_strRows.Split(','), m_strColumns.Split(','), m_strData.Split(','), m_strFilter.Split(','));
        }
        private void LoadTemplate(string[] strRows, string[] strColumns, string[] strData, string[] strFilter)
        {

            gvDetail.Fields.Clear();
            foreach (string tmp in strRows)
            {
                if (tmp == "CPRP")
                {
                    fieldCPRP.Caption = "CPRP";
                    fieldCPRP.Name = "colCPRP";
                    fieldCPRP.Area = PivotArea.RowArea;
                    fieldCPRP.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Custom;
                    gvDetail.Fields.Add(fieldCPRP);
                }
                else
                {
                    gvDetail.Fields.Add(tmp, PivotArea.RowArea).Name = "col" + tmp;
                }
            }


            foreach (string tmp in strColumns)
            {
                if (tmp == "CPRP")
                {
                    fieldCPRP.Caption = "CPRP";
                    fieldCPRP.Name = "colCPRP";
                    fieldCPRP.Area = PivotArea.ColumnArea;
                    fieldCPRP.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Custom;
                    gvDetail.Fields.Add(fieldCPRP);
                }
                else
                {
                    gvDetail.Fields.Add(tmp, PivotArea.ColumnArea).Name = "col" + tmp;
                }
            }


            foreach (string tmp in strData)
            {
                if (tmp == "CPRP")
                {
                    fieldCPRP.Caption = "CPRP";
                    fieldCPRP.Name = "colCPRP";
                    fieldCPRP.Area = PivotArea.DataArea;
                    fieldCPRP.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Custom;
                    gvDetail.Fields.Add(fieldCPRP);
                }
                else
                {

                    gvDetail.Fields.Add(tmp, PivotArea.DataArea).Name = "col" + tmp;
                }
            }

            foreach (string tmp in strFilter)
            {
                if (tmp == "CPRP")
                {
                    fieldCPRP.Caption = "CPRP";
                    fieldCPRP.Name = "colCPRP";
                    fieldCPRP.Area = PivotArea.FilterArea;
                    fieldCPRP.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Custom;
                    gvDetail.Fields.Add(fieldCPRP);
                }
                else
                {
                    gvDetail.Fields.Add(tmp, PivotArea.FilterArea).Name = "col" + tmp;
                }
            }
            foreach (string tmp in m_strAllColumn)
            {
                if (gvDetail.Fields.GetFieldByName("col" + tmp) == null)
                {
                    if (tmp == "CPRP")
                    {
                        fieldCPRP.Caption = "CPRP";
                        fieldCPRP.Name = "colCPRP";
                        fieldCPRP.Area = PivotArea.FilterArea;
                        fieldCPRP.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Custom;
                        gvDetail.Fields.Add(fieldCPRP);
                    }
                    else
                    {
                        gvDetail.Fields.Add(tmp, PivotArea.FilterArea).Name = "col" + tmp;
                    }
                }
            }
            SetFiledFormat();
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

        private void btnRefreshData_Click(object sender, EventArgs e)
        {
            if (gvDetail.DataSource == null)
            {
                GMessage.MessageWarning("Please select criteria before refresh data!");
                return;
            }
            else
            {
                DataLoading();
            }
        }

        private void btnCreateOwnDayPart_Click(object sender, EventArgs e)
        {
            MPA007_CreateOwnDayPart frm = new MPA007_CreateOwnDayPart();
            frm.ShowDialog();
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

        private void gvDesc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void chkDecimal_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkDecimal.Checked == true)
            {
                SetFieldsN2(gvDetail.Fields["Cost"]);
            }
            else
            {
                SetFieldsN0(gvDetail.Fields["Cost"]);
            }
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            startTime = DateTime.Now;
            var result = GetSource();
            e.Result = (result == null ? null : result.Tables[0]);
            m_dtResult = (DataTable)e.Result;
            stopTime = DateTime.Now;
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            this.textBox1.Text = "   Pre and Post Buy Report, Time Overall: " + (stopTime - startTime).TotalSeconds.ToString() + " sec";
            gvDetail.DataSource = m_dtResult;

            if (txtLoadTemplate.Text == "")
            {
                if (gvDetail.Fields.Count == 0)
                {
                    //=================================
                    // ROW AREA
                    //=================================

                    gvDetail.Fields.Add("LateBrief", PivotArea.RowArea);
                    gvDetail.Fields.Add("ClientName", PivotArea.RowArea);
                    gvDetail.Fields.Add("TargetName", PivotArea.RowArea);
                    gvDetail.Fields.Add("DayPartName", PivotArea.RowArea);

                    //=================================
                    // FILTER AREA
                    //=================================
                    gvDetail.Fields.Add("Adex_Cost", PivotArea.FilterArea);
                    SetFieldsN0(gvDetail.Fields["Adex_Cost"]);

                    gvDetail.Fields.Add("BookingProgramName", PivotArea.FilterArea);
                    gvDetail.Fields.Add("Buying_Brief_ID", PivotArea.FilterArea);
                    gvDetail.Fields.Add("CampaignName", PivotArea.FilterArea);
                    gvDetail.Fields.Add("CampaignStartDate", PivotArea.FilterArea);
                    gvDetail.Fields.Add("CampaignEndDate", PivotArea.FilterArea);
                    gvDetail.Fields.Add("CopyLineName", PivotArea.FilterArea);
                    gvDetail.Fields.Add("Day", PivotArea.FilterArea);
                    gvDetail.Fields.Add("DayOfWeek", PivotArea.FilterArea);
                    gvDetail.Fields.Add("Length", PivotArea.FilterArea);

                    gvDetail.Fields.Add("MarketDiscount", PivotArea.FilterArea);
                    SetFieldsN2(gvDetail.Fields["MarketDiscount"]);

                    gvDetail.Fields.Add("MarketNet", PivotArea.FilterArea);
                    SetFieldsN0(gvDetail.Fields["MarketNet"]);

                    gvDetail.Fields.Add("MarketRate", PivotArea.FilterArea);
                    SetFieldsN0(gvDetail.Fields["MarketRate"]);

                    gvDetail.Fields.Add("MasterVendorName", PivotArea.FilterArea);
                    gvDetail.Fields.Add("MatchedProgramName", PivotArea.FilterArea);
                    gvDetail.Fields.Add("MaterialName", PivotArea.FilterArea);
                    gvDetail.Fields.Add("Media", PivotArea.FilterArea);
                    gvDetail.Fields.Add("MediaSubType", PivotArea.FilterArea);
                    gvDetail.Fields.Add("Month", PivotArea.FilterArea);
                    gvDetail.Fields.Add("MonthWeek", PivotArea.FilterArea);
                    gvDetail.Fields.Add("OwnDayPart", PivotArea.FilterArea);
                    gvDetail.Fields.Add("PIB", PivotArea.FilterArea);
                    gvDetail.Fields.Add("PIB_No", PivotArea.FilterArea);
                    gvDetail.Fields.Add("TSB", PivotArea.FilterArea);
                    gvDetail.Fields.Add("ProductName", PivotArea.FilterArea);
                    gvDetail.Fields.Add("ProgramType", PivotArea.FilterArea);
                    gvDetail.Fields.Add("RateCardCost", PivotArea.FilterArea);

                    gvDetail.Fields.Add("Saving", PivotArea.FilterArea);
                    SetFieldsN0(gvDetail.Fields["Saving"]);

                    gvDetail.Fields.Add("ShowDate", PivotArea.FilterArea);
                    gvDetail.Fields.Add("SpotType", PivotArea.FilterArea);
                    gvDetail.Fields.Add("StartTime", PivotArea.FilterArea);
                    gvDetail.Fields.Add("VendorName", PivotArea.FilterArea);
                    gvDetail.Fields.Add("Year", PivotArea.FilterArea); 


                    //=================================
                    // COLUMN AREA
                    //=================================
                    gvDetail.Fields.Add("Version", PivotArea.ColumnArea);
                    //gvDetail.Fields["ShowDay"].Width = 25;


                    //=================================
                    // DATA AREA
                    //=================================

                    gvDetail.Fields.Add("Cost", PivotArea.DataArea);
                    gvDetail.Fields.Add("Spots", PivotArea.DataArea);
                    gvDetail.Fields.Add("Rating", PivotArea.DataArea);
                    gvDetail.Fields.Add("Rating30sec", PivotArea.DataArea);

                    fieldCPRP.Caption = "CPRP";
                    fieldCPRP.Area = PivotArea.DataArea;
                    fieldCPRP.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Custom;
                    gvDetail.Fields.Add(fieldCPRP);
                    SetFiledFormat();
                }

                gvDetail.OptionsView.ShowRowTotals = false;
                gvDetail.OptionsView.ShowRowGrandTotals = false;


                gvDetail.BestFit();
            }
            else
            {
                LoadTemplate();
            }
            //SetFiledFormat();

        }
    }
}
