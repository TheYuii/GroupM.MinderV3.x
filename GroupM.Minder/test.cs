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

namespace GRM.MPA
{
    public partial class test : System.Windows.Forms.Form
    {
        string m_strConnection = "Data Source=BKK-SQL01;database=Minder_Thai;User id=sa;Password=Groupm#01;";
        string m_strDateFrom = "yyyyMMdd";
        string m_strDateTo = "yyyyMMdd";
        string m_strMedaiSubType = "TV";

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
            ,MonthWeek
            ,Length
            ,Media
            ,ProductName
            ,BookingProgramName
           ,
            SpotType
           ,
            MaterialName
            ,
            CampaignName
            ,MediaSubType
            ,VendorName
            ,Cost
            ,Spots
            ,Rating
            ,Rating30sec
            ,CPRP
        }
        public test(string strUserName,string strPassword)
        {
            this.UserName = strUserName;
            this.Password = strPassword;
            InitializeComponent();
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

            strSQL = @"
SELECT * FROM 
(
SELECT 
	CASE Buying_Brief.Late_Brief WHEN 0 THEN 'NO' ELSE 'YES' END LateBrief
	, Client.English_Name ClientName
	, Target.Short_Name TargetName
	, dbo.GetDayPart(Spot_Plan.Start_Time,Spot_Plan.End_Time,Spot_Plan.Show_Date) DayPartName
	, Spot_Plan.Buying_Brief_ID
	, CASE LEN(Spot_Plan.Version) WHEN 2 THEN '1-APPROVE (PLANNED)' ELSE '2-LATEST (EXECUTING)' END [Version]
	, Spot_Plan.Length
	, RIGHT('0'+CONVERT(VARCHAR(2),MONTH(Spot_Plan.Show_Date)),2)
		+ RIGHT('0'+CONVERT(VARCHAR(2),dbo.fn_ISOWK(Spot_Plan.Show_Date)),2) MonthWeek
	, Spot_Plan.Media_ID Media
	, Product.English_Name ProductName
	, Spot_Plan.Program BookingProgramName
	, Spot_Plan.Package SpotType
	, Material.Thai_Name MaterialName
	, Buying_Brief.Description CampaignName
	, Buying_Brief.Media_Sub_Type MediaSubType
	, Media_Vendor.English_Name VendorName
	, SUM(Spot_Plan.Spots) Spots
	, SUM(Spot_Plan.Net_Cost) Cost
	, SUM(Spot_Plan.Rating) Rating
	, SUM((Spot_Plan.Rating * Spot_Plan.Length)/30 ) Rating30sec
	, CASE SUM((Spot_Plan.Rating * Spot_Plan.Length)/30 )  WHEN 0 THEN 0 ELSE
	SUM(Spot_Plan.Net_Cost) / SUM((Spot_Plan.Rating * Spot_Plan.Length)/30 ) END CPRP 
FROM Spot_Plan with(nolock)
	INNER JOIN Buying_Brief		
		ON Spot_Plan.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
	INNER JOIN fn_User_CheckPermission(@U,@P,NULL) UserPermission
		ON UserPermission.Client_ID = Buying_Brief.Client_ID 
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
WHERE 
	Buying_Brief.Media_Sub_Type IN ('" + m_strMedaiSubType.Replace(",", "','") + @"')
	AND Spot_Plan.Show_Date BETWEEN @S AND @E
	AND Spot_Plan.Status > 0
	--AND (Spot_Plan.Rating * Spot_Plan.Length)/30  <> 0
	--AND Buying_Brief.Client_ID ='20LORMOB'
GROUP BY 
	 Target.Short_Name
	, Spot_Plan.Media_ID
	, Product.English_Name 
	, dbo.GetDayPart(Spot_Plan.Start_Time,Spot_Plan.End_Time,Spot_Plan.Show_Date) 
	, Client.English_Name
	, Spot_Plan.Program 
	, Spot_Plan.Package 
	, Material.Thai_Name 
	, Buying_Brief.Description
	, Buying_Brief.Media_Sub_Type
	, Media_Vendor.English_Name 
	, Buying_Brief.Late_Brief
	, Spot_Plan.Buying_Brief_ID
	, Spot_Plan.Version
	, Spot_Plan.Length
	, RIGHT('0'+CONVERT(VARCHAR(2),MONTH(Spot_Plan.Show_Date)),2)  
		+ RIGHT('0'+CONVERT(VARCHAR(2),dbo.fn_ISOWK(Spot_Plan.Show_Date)),2) 

UNION

SELECT 
	CASE Buying_Brief.Late_Brief WHEN 0 THEN 'NO' ELSE 'YES' END LateBrief
	, Client.English_Name ClientName
	, Target.Short_Name TargetName
	, dbo.GetDayPart(SpotsMatch.Start_Time,SpotsMatch.End_Time,SpotsMatch.Show_Date) DayPartName
	, SpotsMatch.Buying_Brief_ID
	, '3-ACTUAL (POST)' [Version]
	, SpotsMatch.Length
	, RIGHT('0'+CONVERT(VARCHAR(2),MONTH(SpotsMatch.Show_Date)),2)
		+ RIGHT('0'+CONVERT(VARCHAR(2),dbo.fn_ISOWK(SpotsMatch.Show_Date)),2) MonthWeek
	, SpotsMatch.Media_ID Media
	, Product.English_Name ProductName
	, SpotsMatch.BookingProgramName
	, SpotsMatch.SpotType
	, SpotsMatch.MaterialName
	, Buying_Brief.Description CampaignName
	, Buying_Brief.Media_Sub_Type MediaSubType
	, SpotsMatch.VendorName
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
			,Spot_Plan.Start_Time
			,Spot_Plan.End_Time
			,Spots_Match.Actual_Rating
			,Spot_Plan.Spots
			,Spot_Plan.Program BookingProgramName
			,Spot_Plan.Package SpotType
			,Material.Thai_Name MaterialName
			,Media_Vendor.English_Name VendorName
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
		WHERE Spots_Match.Actual_Rating IS NOT NULL		
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
			,'Unknown Bonus' SpotType
			,PDNO MaterialName
			,Media_Vendor.English_Name VendorName
		FROM Spots_Match  with(nolock)
			INNER JOIN Media_Vendor
				ON Media_Vendor.Media_Vendor_ID = Spots_Match.Media_Vendor
		WHERE Spots_Match.Flag_Bonus = 1 AND Spots_Match.Actual_Rating IS NOT NULL
		
		) SpotsMatch 
	INNER JOIN Buying_Brief		
		ON SpotsMatch.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
	INNER JOIN fn_User_CheckPermission(@U,@P,NULL) UserPermission
		ON UserPermission.Client_ID = Buying_Brief.Client_ID 
	INNER JOIN Target 
		ON Buying_Brief.Primary_Target = Target.Target_ID
	INNER JOIN Client
		ON Buying_Brief.Client_ID = Client.Client_ID
	INNER JOIN Product
		ON Buying_Brief.Product_ID = Product.Product_ID
WHERE 
	Buying_Brief.Media_Sub_Type IN ('" + m_strMedaiSubType.Replace(",", "','") + @"')
	AND SpotsMatch.Show_Date BETWEEN @S AND @E
	AND SpotsMatch.Actual_Rating IS NOT NULL
	--AND (SpotsMatch.Actual_Rating * SpotsMatch.Length)/30   <> 0
	--AND Buying_Brief.Client_ID ='20LORMOB'
	--AND SpotsMatch.Buying_Brief_ID = '2011050387'
GROUP BY 
	 Target.Short_Name
	, SpotsMatch.Media_ID
	, Product.English_Name 
	, dbo.GetDayPart(SpotsMatch.Start_Time,SpotsMatch.End_Time,SpotsMatch.Show_Date)
	, Client.English_Name
	, SpotsMatch.BookingProgramName
	, SpotsMatch.SpotType
	, SpotsMatch.MaterialName
	, Buying_Brief.Description
	, Buying_Brief.Media_Sub_Type
	, SpotsMatch.VendorName
	, Buying_Brief.Late_Brief
	, SpotsMatch.Buying_Brief_ID
	, SpotsMatch.Length
	, RIGHT('0'+CONVERT(VARCHAR(2),MONTH(SpotsMatch.Show_Date)),2)
		+ RIGHT('0'+CONVERT(VARCHAR(2),dbo.fn_ISOWK(SpotsMatch.Show_Date)),2) 
) rec


            ";
            da = new SqlDataAdapter(strSQL, conn);
            da.SelectCommand.Parameters.Add("@U", SqlDbType.VarChar).Value = UserName;
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
       
        private void MPA003_MediaSpending_Load(object sender, System.EventArgs e)
        {
            if(lbCondition.Text == "NONE")
            {
                MPA002_SearchCondition frmSearch = new MPA002_SearchCondition();
                if (frmSearch.ShowDialog() == DialogResult.OK)
                {
                    lbCondition.Text = frmSearch.DateFrom.ToString("dd MMM yyyy") + " - " + frmSearch.DateTo.ToString("dd MMM yyyy");

                    m_strDateFrom = frmSearch.DateFrom.ToString("yyyyMMdd");
                    m_strDateTo = frmSearch.DateTo.ToString("yyyyMMdd");
                    m_strMedaiSubType = frmSearch.MediaSubType;

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
        }		
        private void lbCondition_Click(object sender, EventArgs e)
        {
            MPA002_SearchCondition frmSearch = new MPA002_SearchCondition();
            
            DateTime dtFrom;
            DateTime dtTo;
            if (lbCondition.Text == "NONE")
            {
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;                
            }
            else
            {
                dtFrom = Convert.ToDateTime(lbCondition.Text.Split('-')[0]);
                dtTo = Convert.ToDateTime(lbCondition.Text.Split('-')[1]);
            }
            frmSearch.DateFrom = dtFrom;
            frmSearch.DateTo = dtTo;
            frmSearch.MediaSubType = m_strMedaiSubType;
            if (frmSearch.ShowDialog() == DialogResult.OK)
            {
                lbCondition.Text = frmSearch.DateFrom.ToString("dd MMM yyyy") + " - " + frmSearch.DateTo.ToString("dd MMM yyyy");
                m_strDateFrom = frmSearch.DateFrom.ToString("yyyyMMdd");
                m_strDateTo = frmSearch.DateTo.ToString("yyyyMMdd");
                m_strMedaiSubType = frmSearch.MediaSubType;
         
            }
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
        }

        private void axDetail_FetchAttributes(object sender, AxDynamiCubeLib.IDCubeEvents_FetchAttributesEvent e)
        {
            
        }
        private void axDetail_FetchData(object sender, EventArgs e)
        {
        }
	}
}
