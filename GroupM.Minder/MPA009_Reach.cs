using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GroupM.UTL;
using System.Data.SqlClient;

namespace  GroupM.Minder
{
    public partial class MPA009_Reach : Form
    {
        public MPA009_Reach()
        {
            InitializeComponent();
            m_dtResult = CreateTableResult();
        }
        int intDiffDate = 35;
        string m_strPanel = "THCABLE";
        string m_strConnection = Connection.ConnectionStringMinder;


        private void MPA009_Reach_Load(object sender, EventArgs e)
        {
            gvCondition.DataSource = CreateTableCondition();
            DataLoading();
            
        }
        MPA010_Reach_SearchCondition frm = null;
        private void DataLoading()
        {
            string strBBSelectedBefore = gvCondition.Rows[0].Cells[4].Value.ToString();
            if(frm == null)
                frm = new MPA010_Reach_SearchCondition(Connection.USERID, Connection.PASSWORD,"SingleRow");
            frm.BuyingBriefBefore = strBBSelectedBefore;
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    DataRow dr = ((DataRowView)((DataGridViewRow)frm.gvClient.SelectedRows[0]).DataBoundItem).Row;
                    string strBB = dr["BuyingBriefID"].ToString();
                    string strTarget = dr["TargetID"].ToString();
                    string dtStartDate = frm.dtStartDate.Value.ToString("yyyyMMdd");
                    string dtEndDate = frm.dtEndDate.Value.ToString("yyyyMMdd");
                    intDiffDate = Convert.ToInt32(frm.txtDiffDate.Text);

                    DataTable dt = (DataTable)gvCondition.DataSource;
                    dt.Rows.Clear();
                    DataRow drCondition = dt.NewRow();
                    drCondition["StartDate"] = frm.dtStartDate.Value.ToString("dd/MM/yyyy");
                    drCondition["EndDate"] = frm.dtEndDate.Value.ToString("dd/MM/yyyy");
                    drCondition["BaseDate"] = frm.dtEndDate.Value.AddDays(-1 * intDiffDate).ToString("dd/MM/yyyy");
                    drCondition["BuyingBriefID"] = strBB;
                    dt.Rows.Add(drCondition);

                    InsertReach(strBB, strTarget, dtStartDate, dtEndDate);

                    for (int i = 0; i < gvDetail.Columns.Count; i++)
                    {
                        DataGridViewColumn column = gvDetail.Columns[i];
                        DataGridViewCell cell = new DataGridViewTextBoxCell();
                        if (i <= 16)
                            cell.Style.BackColor = Color.Wheat;
                        else if (i == 17 || i == 35)
                            cell.Style.BackColor = Color.White;
                        else if (i <= 34)
                            cell.Style.BackColor = Color.LightBlue;
                        else if (i >= 36)
                            cell.Style.BackColor = Color.LightPink;

                        column.CellTemplate = cell;
                        column.ReadOnly = true;
                    }
                    CheckNoEtamCode(strBB, dtStartDate, dtEndDate);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }
        DataTable m_dtResult = null;

        private DataTable CreateTableCondition()
        {
            DataTable dtResult = new DataTable();

            dtResult.Columns.Add("BaseDate");
            dtResult.Columns.Add("StartDate");
            dtResult.Columns.Add("EndDate");
            dtResult.Columns.Add("BuyingBriefID");

            DataRow dr = dtResult.NewRow();
            dtResult.Rows.Add(dr);
            return dtResult;

        }
        private DataTable CreateTableResult()
        {
            DataTable dtResult = new DataTable();
            
            dtResult.Columns.Add("Approve_Spot");
            dtResult.Columns.Add("Approve_Date");
            dtResult.Columns.Add("Approve_Day");
            dtResult.Columns.Add("Approve_Time");
            dtResult.Columns.Add("Approve_Start_Time");
            dtResult.Columns.Add("Approve_End_Time");
            dtResult.Columns.Add("Approve_Station");
            dtResult.Columns.Add("Approve_Program");
            dtResult.Columns.Add("Approve_Cost");
            dtResult.Columns.Add("Approve_GRP");
            dtResult.Columns.Add("Approve_CummeGRP");
            dtResult.Columns.Add("Approve_IncrementalReach");
            dtResult.Columns.Add("Approve_EffectiveReach1");
            dtResult.Columns.Add("Approve_EffectiveReach2");
            dtResult.Columns.Add("Approve_EffectiveReach3");
            dtResult.Columns.Add("Approve_EffectiveReach4");
            dtResult.Columns.Add("Approve_EffectiveReach5");

            dtResult.Columns.Add("Execute_Spot");
            dtResult.Columns.Add("Execute_Date");
            dtResult.Columns.Add("Execute_Day");
            dtResult.Columns.Add("Execute_Time");
            dtResult.Columns.Add("Execute_Start_Time");
            dtResult.Columns.Add("Execute_End_Time");
            dtResult.Columns.Add("Execute_Station");
            dtResult.Columns.Add("Execute_Program");
            dtResult.Columns.Add("Execute_Cost");
            dtResult.Columns.Add("Execute_GRP");
            dtResult.Columns.Add("Execute_CummeGRP");
            dtResult.Columns.Add("Execute_IncrementalReach");
            dtResult.Columns.Add("Execute_EffectiveReach1");
            dtResult.Columns.Add("Execute_EffectiveReach2");
            dtResult.Columns.Add("Execute_EffectiveReach3");
            dtResult.Columns.Add("Execute_EffectiveReach4");
            dtResult.Columns.Add("Execute_EffectiveReach5");

            dtResult.Columns.Add("Actual_Spot");
            dtResult.Columns.Add("Actual_Date");
            dtResult.Columns.Add("Actual_Day");
            dtResult.Columns.Add("Actual_Time");
            dtResult.Columns.Add("Actual_Start_Time");
            dtResult.Columns.Add("Actual_End_Time");
            dtResult.Columns.Add("Actual_Station");
            dtResult.Columns.Add("Actual_Program");
            dtResult.Columns.Add("Actual_Cost");
            dtResult.Columns.Add("Actual_GRP");
            dtResult.Columns.Add("Actual_CummeGRP");
            dtResult.Columns.Add("Actual_IncrementalReach");
            dtResult.Columns.Add("Actual_EffectiveReach1");
            dtResult.Columns.Add("Actual_EffectiveReach2");
            dtResult.Columns.Add("Actual_EffectiveReach3");
            dtResult.Columns.Add("Actual_EffectiveReach4");
            dtResult.Columns.Add("Actual_EffectiveReach5");
            return dtResult;
        
        }
        private void InsertReach(string strBuyingBrief, string strTarget_eTam, string strStartDate, string strEndDate)
        {
            m_dtResult = CreateTableResult();
            gvDetail.AutoGenerateColumns = false;
            gvDetail.DataSource = m_dtResult;

            if (!InsertReach_Approve(strBuyingBrief, strTarget_eTam, strStartDate, strEndDate))
                return;
            if (!InsertReach_Executing(strBuyingBrief, strTarget_eTam, strStartDate, strEndDate))
                return;
            if (!InsertReach_Actual(strBuyingBrief, strTarget_eTam, strStartDate, strEndDate))
                return;


            //InsertReach_Test();


        }
        private void CheckNoEtamCode(string strBuyingBrief, string strStartDate, string strEndDate)
        {
            string strSQL = @"
 Select DISTINCT m.Short_Name+'('+s.Media_ID+')'
  From Spot_Plan s 
  Inner join Media m on s.Media_ID = m.Media_ID 
  Where s.Buying_Brief_ID = @Buying_Brief_ID
  And s.Show_Date >= @ShowDate 
  And s.Show_Date <= @EndDate
  and s.Market_ID = 'THAILAND'  
  and s.Version = (select MAX(Version) AS [Version] from spot_plan_version where Buying_Brief_ID = @Buying_Brief_ID and len(Version) = 1)
  and s.Media_ID NOT IN (SELECT Media_ID FROM Media_Mapping_ETAM)	
";
            SqlConnection conn = new SqlConnection(m_strConnection);
            conn.Open();
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, conn);
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBuyingBrief;
            sda.SelectCommand.Parameters.Add("@ShowDate", SqlDbType.VarChar).Value = strStartDate;
            sda.SelectCommand.Parameters.Add("@EndDate", SqlDbType.VarChar).Value = strEndDate;
            DataTable dt = new DataTable();
            sda.Fill(dt);

            if (dt.Rows.Count == 0)
                return;
            string strMsg = "";            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strMsg += Environment.NewLine+dt.Rows[i][0].ToString();
            }
            GMessage.MessageInfo("Could not found reach for each channel following list below:"+strMsg);
        }
        private bool InsertReach_Approve(string strBuyingBrief, string strTarget_eTam, string strStartDate, string strEndDate)
        {
            AQETAMRFENGINECOMLib.RfEngine mRF = new AQETAMRFENGINECOMLib.RfEngine();
            if (mRF.Initialise() != 0)
            {
                GMessage.MessageWarning("Can't Intitiallis Reach Engine : " + mRF.GetLastError());
                mRF.Terminate();
                return false;
            }
            if (mRF.SetPanel(m_strPanel, "Thailand", 1) != 0)
            {
                GMessage.MessageWarning("Can not find Panel in eTam : " + mRF.GetLastError());
                mRF.Terminate();
                return false;
            }
            if (mRF.AddDemographic(strTarget_eTam) != 0)
            {
                GMessage.MessageWarning("Can not find Target in eTam : " + mRF.GetLastError());
                mRF.Terminate();
                return false;
            }
            if (mRF.SetRestrictSpotsToCommercialMinutes(Convert.ToInt32(true), 2) != 0)
            {
                GMessage.MessageWarning("Can not set restrict spots to commercial minutes in eTam : " + mRF.GetLastError());
                mRF.Terminate();
                return false;
            }
            SqlConnection conn = new SqlConnection(m_strConnection);
            conn.Open();
            string strSQL = @"Select 
s.Show_Date, s.Start_Time, s.End_Time
,RIGHT('00'+CONVERT(varchar(25),(

CONVERT(int,
(CONVERT(int,LEFT(s.Start_Time,2)*60) + CONVERT(int,RIGHT(s.Start_Time,2)))+
((CONVERT(int,LEFT(s.End_Time,2)*60) + CONVERT(int,RIGHT(s.End_Time,2))) - (CONVERT(int,LEFT(s.Start_Time,2)*60) + CONVERT(int,RIGHT(s.Start_Time,2))))/2.00
)

)/60),2)
+RIGHT('00'+CONVERT(varchar(25),(

CONVERT(int,
(CONVERT(int,LEFT(s.Start_Time,2)*60) + CONVERT(int,RIGHT(s.Start_Time,2)))+
((CONVERT(int,LEFT(s.End_Time,2)*60) + CONVERT(int,RIGHT(s.End_Time,2))) - (CONVERT(int,LEFT(s.Start_Time,2)*60) + CONVERT(int,RIGHT(s.Start_Time,2))))/2.00
)

)%60),2) AS Mid_Time
,mm.ETAM_Name,s.Program
,convert( decimal(18,3), s.Rating) AS Rating
,s.Net_Cost,s.Media_ID 
  From Spot_Plan s 
  Inner Join Media_Mapping_ETAM mm On s.Media_ID = mm.Media_ID 
  Inner join Media m on s.Media_ID = m.Media_ID 
  Where s.Buying_Brief_ID = @Buying_Brief_ID
  And s.Show_Date >= @ShowDate 
  And s.Show_Date <= @EndDate
  and s.Market_ID = 'THAILAND'  
  and s.Version = (select MAX(Version) AS [Version] from spot_plan_version where Buying_Brief_ID = @Buying_Brief_ID and len(Version) = 2)
  Order by s.Show_Date,Mid_Time , s.Media_ID ";

            SqlDataAdapter sda = new SqlDataAdapter(strSQL, conn);
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBuyingBrief;
            sda.SelectCommand.Parameters.Add("@ShowDate", SqlDbType.VarChar).Value = strStartDate;
            sda.SelectCommand.Parameters.Add("@EndDate", SqlDbType.VarChar).Value = strEndDate;
            DataTable dt = new DataTable();
            sda.Fill(dt);


            if (dt.Rows.Count == 0)
                return false;
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                string Start_Time = dr["Start_Time"].ToString();
                string End_Time = dr["End_Time"].ToString();
                string Show_Date = dr["Show_Date"].ToString();
                string ETAM_Name = dr["ETAM_Name"].ToString();

                DateTime dtShowDate = new DateTime(Convert.ToInt32(Show_Date.Substring(0, 4)), Convert.ToInt32(Show_Date.Substring(4, 2)), Convert.ToInt32(Show_Date.Substring(6, 2)));

                dtShowDate = dtShowDate.AddDays(-1 * intDiffDate);


                int intSTime = (Convert.ToInt32(Start_Time.Substring(0, 2)) * 60) + Convert.ToInt32(Start_Time.Substring(2, 2));
                int intETime = (Convert.ToInt32(End_Time.Substring(0, 2)) * 60) + Convert.ToInt32(End_Time.Substring(2, 2));
                int intTotalMin = intSTime + ((intETime - intSTime) / 2);
                string strMidTime = string.Format("{0}:{1}", (intTotalMin / 60).ToString("00"), (intTotalMin % 60).ToString("00"));
                string VDate = dtShowDate.ToString("yyyyMMdd");
                string SETime = string.Format("{0}{1}", (intTotalMin / 60).ToString("00"), (intTotalMin % 60).ToString("00"));
                mRF.AddSpot(VDate, SETime, ETAM_Name, "", 0, 0);

            }
            if (mRF.ProcessRequest() != 0)
            {
                GMessage.MessageWarning("An error occurred while Process Request in eTam : " + mRF.GetLastError());
                mRF.Terminate();
                return false;
            }

            int iResultRowCount = mRF.GetDetailRowCount();

            if (iResultRowCount < 0)
            {
                GMessage.MessageWarning("An error occurred while Process Request in eTam : " + mRF.GetLastError());
                mRF.Terminate();
                return false;
            }

            double apdOfficialCummulativeSpotBySpotReach = 0;
            double apdOfficialCummulativeSpotBySpotReachPercent = 0;
            double apdOfficialFrequency = 0;
            double apdCumeTarps = 0;
            double apdTarp = 0;
            double apdThresholdTarp = 0;
            double apdProjection = 0;
            double apdThresholdProjection = 0;
            double adIncrementalReach = 0;
            object apEffectiveReachArray, apExclusiveReachArray, apEffectiveReachUnitsArray, apExclusiveReachUnitsArray;
            System.DateTime apdDate = default(System.DateTime);
            string apsChannel = null;
            string apsProgram = null;
            double apdCost = 0;
            int apnFromMinuteOfDay = 0;
            int apnToMinuteOfDay = 0;
            int apnSpotDuration = 0;


            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("Spot");
            dtResult.Columns.Add("apdOfficialCummulativeSpotBySpotReach");
            dtResult.Columns.Add("apdOfficialCummulativeSpotBySpotReachPercent");
            dtResult.Columns.Add("apdOfficialFrequency");
            dtResult.Columns.Add("apdCumeTarps");
            dtResult.Columns.Add("apdTarp");
            dtResult.Columns.Add("apdThresholdTarp");
            dtResult.Columns.Add("apdProjection");
            dtResult.Columns.Add("apdThresholdProjection");
            dtResult.Columns.Add("adIncrementalReach");
            dtResult.Columns.Add("apEffectiveReachArray1");
            dtResult.Columns.Add("apEffectiveReachArray2");
            dtResult.Columns.Add("apEffectiveReachArray3");
            dtResult.Columns.Add("apEffectiveReachArray4");
            dtResult.Columns.Add("apEffectiveReachArray5");
            dtResult.Columns.Add("apEffectiveReachArray6");
            dtResult.Columns.Add("apEffectiveReachArray7");
            dtResult.Columns.Add("apEffectiveReachArray8");
            dtResult.Columns.Add("apEffectiveReachArray9");
            dtResult.Columns.Add("apEffectiveReachArray10");
            dtResult.Columns.Add("apExclusiveReachArray1");
            dtResult.Columns.Add("apExclusiveReachArray2");
            dtResult.Columns.Add("apExclusiveReachArray3");
            dtResult.Columns.Add("apExclusiveReachArray4");
            dtResult.Columns.Add("apExclusiveReachArray5");
            dtResult.Columns.Add("apExclusiveReachArray6");
            dtResult.Columns.Add("apExclusiveReachArray7");
            dtResult.Columns.Add("apExclusiveReachArray8");
            dtResult.Columns.Add("apExclusiveReachArray9");
            dtResult.Columns.Add("apExclusiveReachArray10");
            dtResult.Columns.Add("apEffectiveReachUnitsArray1");
            dtResult.Columns.Add("apEffectiveReachUnitsArray2");
            dtResult.Columns.Add("apEffectiveReachUnitsArray3");
            dtResult.Columns.Add("apEffectiveReachUnitsArray4");
            dtResult.Columns.Add("apEffectiveReachUnitsArray5");
            dtResult.Columns.Add("apEffectiveReachUnitsArray6");
            dtResult.Columns.Add("apEffectiveReachUnitsArray7");
            dtResult.Columns.Add("apEffectiveReachUnitsArray8");
            dtResult.Columns.Add("apEffectiveReachUnitsArray9");
            dtResult.Columns.Add("apEffectiveReachUnitsArray10");
            dtResult.Columns.Add("apExclusiveReachUnitsArray1");
            dtResult.Columns.Add("apExclusiveReachUnitsArray2");
            dtResult.Columns.Add("apExclusiveReachUnitsArray3");
            dtResult.Columns.Add("apExclusiveReachUnitsArray4");
            dtResult.Columns.Add("apExclusiveReachUnitsArray5");
            dtResult.Columns.Add("apExclusiveReachUnitsArray6");
            dtResult.Columns.Add("apExclusiveReachUnitsArray7");
            dtResult.Columns.Add("apExclusiveReachUnitsArray8");
            dtResult.Columns.Add("apExclusiveReachUnitsArray9");
            dtResult.Columns.Add("apExclusiveReachUnitsArray10");
            dtResult.Columns.Add("Date");
            dtResult.Columns.Add("apdDate");
            dtResult.Columns.Add("DayOfWeek");
            dtResult.Columns.Add("Time");
            dtResult.Columns.Add("apsChannel");
            dtResult.Columns.Add("apsProgram");
            dtResult.Columns.Add("apdCost");
            dtResult.Columns.Add("apnFromMinuteOfDay");
            dtResult.Columns.Add("apnToMinuteOfDay");
            dtResult.Columns.Add("apnSpotDuration");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                mRF.GetDetailRow(i
                   , 0
                   , out apdOfficialCummulativeSpotBySpotReach
                   , out apdOfficialCummulativeSpotBySpotReachPercent
                   , out apdOfficialFrequency
                   , out apdCumeTarps
                   , out apdTarp
                   , out apdThresholdTarp
                   , out apdProjection
                   , out apdThresholdProjection
                   , out adIncrementalReach
                   , out apEffectiveReachArray
                   , out apExclusiveReachArray
                   , out apEffectiveReachUnitsArray
                   , out apExclusiveReachUnitsArray
                   , out apdDate
                   , out apsChannel
                   , out apsProgram
                   , out apdCost
                   , out apnFromMinuteOfDay
                   , out apnToMinuteOfDay
                   , out apnSpotDuration);

                DataRow drInput = dt.Rows[i];

                DataRow dr = dtResult.NewRow();
                dr["Spot"] = i + 1;
                dr["apdOfficialCummulativeSpotBySpotReach"] = apdOfficialCummulativeSpotBySpotReach;
                dr["apdOfficialCummulativeSpotBySpotReachPercent"] = apdOfficialCummulativeSpotBySpotReachPercent.ToString("#,##0.000");
                dr["apdOfficialFrequency"] = apdOfficialFrequency;

                //dr["apdTarp"] = apdTarp.ToString("#,##0.000");
                dr["apdTarp"] = Convert.ToDouble(drInput["Rating"]).ToString("#,##0.000");

                //dr["apdCumeTarps"] = apdCumeTarps.ToString("#,##0.000");
                if (i == 0)
                    dr["apdCumeTarps"] = Convert.ToDecimal(drInput["Rating"]).ToString("#,##0.000");
                else
                    dr["apdCumeTarps"] = (Convert.ToDecimal(drInput["Rating"]) + Convert.ToDecimal(dtResult.Rows[i - 1]["apdCumeTarps"])).ToString("#,##0.000");

                dr["apdThresholdTarp"] = apdThresholdTarp;
                dr["apdProjection"] = apdProjection;
                dr["apdThresholdProjection"] = apdThresholdProjection;
                dr["adIncrementalReach"] = adIncrementalReach;

                double[] tmpEffectiveReachArray = (double[])apEffectiveReachArray;
                dr["apEffectiveReachArray1"] = tmpEffectiveReachArray[0].ToString("#,##0.000");
                dr["apEffectiveReachArray2"] = tmpEffectiveReachArray[1].ToString("#,##0.000");
                dr["apEffectiveReachArray3"] = tmpEffectiveReachArray[2].ToString("#,##0.000");
                dr["apEffectiveReachArray4"] = tmpEffectiveReachArray[3].ToString("#,##0.000");
                dr["apEffectiveReachArray5"] = tmpEffectiveReachArray[4].ToString("#,##0.000");
                dr["apEffectiveReachArray6"] = tmpEffectiveReachArray[5].ToString("#,##0.000");
                dr["apEffectiveReachArray7"] = tmpEffectiveReachArray[6].ToString("#,##0.000");
                dr["apEffectiveReachArray8"] = tmpEffectiveReachArray[7].ToString("#,##0.000");
                dr["apEffectiveReachArray9"] = tmpEffectiveReachArray[8].ToString("#,##0.000");
                dr["apEffectiveReachArray10"] = tmpEffectiveReachArray[9].ToString("#,##0.000");

                double[] tmpExclusiveReachArray = (double[])apExclusiveReachArray;
                dr["apExclusiveReachArray1"] = tmpExclusiveReachArray[0];
                dr["apExclusiveReachArray2"] = tmpExclusiveReachArray[1];
                dr["apExclusiveReachArray3"] = tmpExclusiveReachArray[2];
                dr["apExclusiveReachArray4"] = tmpExclusiveReachArray[3];
                dr["apExclusiveReachArray5"] = tmpExclusiveReachArray[4];
                dr["apExclusiveReachArray6"] = tmpExclusiveReachArray[5];
                dr["apExclusiveReachArray7"] = tmpExclusiveReachArray[6];
                dr["apExclusiveReachArray8"] = tmpExclusiveReachArray[7];
                dr["apExclusiveReachArray9"] = tmpExclusiveReachArray[8];
                dr["apExclusiveReachArray10"] = tmpExclusiveReachArray[9];

                double[] tmpEffectiveReachUnitsArray = (double[])apEffectiveReachUnitsArray;
                dr["apEffectiveReachUnitsArray1"] = tmpEffectiveReachUnitsArray[0];
                dr["apEffectiveReachUnitsArray2"] = tmpEffectiveReachUnitsArray[1];
                dr["apEffectiveReachUnitsArray3"] = tmpEffectiveReachUnitsArray[2];
                dr["apEffectiveReachUnitsArray4"] = tmpEffectiveReachUnitsArray[3];
                dr["apEffectiveReachUnitsArray5"] = tmpEffectiveReachUnitsArray[4];
                dr["apEffectiveReachUnitsArray6"] = tmpEffectiveReachUnitsArray[5];
                dr["apEffectiveReachUnitsArray7"] = tmpEffectiveReachUnitsArray[6];
                dr["apEffectiveReachUnitsArray8"] = tmpEffectiveReachUnitsArray[7];
                dr["apEffectiveReachUnitsArray9"] = tmpEffectiveReachUnitsArray[8];
                dr["apEffectiveReachUnitsArray10"] = tmpEffectiveReachUnitsArray[9];

                double[] tmpExclusiveReachUnitsArray = (double[])apExclusiveReachUnitsArray;
                dr["apExclusiveReachUnitsArray1"] = tmpExclusiveReachUnitsArray[0];
                dr["apExclusiveReachUnitsArray2"] = tmpExclusiveReachUnitsArray[1];
                dr["apExclusiveReachUnitsArray3"] = tmpExclusiveReachUnitsArray[2];
                dr["apExclusiveReachUnitsArray4"] = tmpExclusiveReachUnitsArray[3];
                dr["apExclusiveReachUnitsArray5"] = tmpExclusiveReachUnitsArray[4];
                dr["apExclusiveReachUnitsArray6"] = tmpExclusiveReachUnitsArray[5];
                dr["apExclusiveReachUnitsArray7"] = tmpExclusiveReachUnitsArray[6];
                dr["apExclusiveReachUnitsArray8"] = tmpExclusiveReachUnitsArray[7];
                dr["apExclusiveReachUnitsArray9"] = tmpExclusiveReachUnitsArray[8];
                dr["apExclusiveReachUnitsArray10"] = tmpExclusiveReachUnitsArray[9];

                dr["Date"] = Convert.ToDateTime(apdDate).AddDays(intDiffDate).ToString("dd/MM/yyyy");
                dr["apdDate"] = Convert.ToDateTime(apdDate).ToString("dd/MM/yyyy");
                dr["DayOfWeek"] = Convert.ToDateTime(apdDate).DayOfWeek;
                int iHr = (int)(Convert.ToDouble(apnFromMinuteOfDay) / 60.00);
                int iMin = Convert.ToInt32(apnFromMinuteOfDay % 60.00);
                dr["Time"] = iHr.ToString("00") + ":" + iMin.ToString("00");
                //dr["apsChannel"] = apsChannel;
                //dr["apsProgram"] = apsProgram;
                dr["apsChannel"] = drInput["Media_ID"];
                dr["apsProgram"] = drInput["Program"];
                dr["apdCost"] = Convert.ToDouble(drInput["Net_Cost"]).ToString("#,##0.00");
                dr["apnFromMinuteOfDay"] = apnFromMinuteOfDay;
                dr["apnToMinuteOfDay"] = apnToMinuteOfDay;
                dr["apnSpotDuration"] = apnSpotDuration;
                dtResult.Rows.Add(dr);

                DataRow drNewResult = m_dtResult.NewRow();

                drNewResult["Approve_Spot"] = dr["Spot"];
                drNewResult["Approve_Date"] = dr["Date"];
                drNewResult["Approve_Day"] = dr["DayOfWeek"];
                drNewResult["Approve_Time"] = dr["Time"];
                drNewResult["Approve_Start_Time"] = drInput["Start_Time"];
                drNewResult["Approve_End_Time"] = drInput["End_Time"];
                drNewResult["Approve_Station"] = dr["apsChannel"];
                drNewResult["Approve_Program"] = dr["apsProgram"];
                drNewResult["Approve_Cost"] = dr["apdCost"];
                drNewResult["Approve_GRP"] = dr["apdTarp"];
                drNewResult["Approve_CummeGRP"] = dr["apdCumeTarps"];
                drNewResult["Approve_IncrementalReach"] = dr["apdOfficialCummulativeSpotBySpotReachPercent"];

                if (Convert.ToDouble(dr["apEffectiveReachArray1"]) != 0)
                    drNewResult["Approve_EffectiveReach1"] = dr["apEffectiveReachArray1"];
                if (Convert.ToDouble(dr["apEffectiveReachArray2"]) != 0)
                    drNewResult["Approve_EffectiveReach2"] = dr["apEffectiveReachArray2"];
                if (Convert.ToDouble(dr["apEffectiveReachArray3"]) != 0)
                    drNewResult["Approve_EffectiveReach3"] = dr["apEffectiveReachArray3"];
                if (Convert.ToDouble(dr["apEffectiveReachArray4"]) != 0)
                    drNewResult["Approve_EffectiveReach4"] = dr["apEffectiveReachArray4"];
                if (Convert.ToDouble(dr["apEffectiveReachArray5"]) != 0)
                drNewResult["Approve_EffectiveReach5"] = dr["apEffectiveReachArray5"];
                m_dtResult.Rows.Add(drNewResult);

            }

            conn.Close();
            return true;


        }
        private bool InsertReach_Executing(string strBuyingBrief, string strTarget_eTam, string strStartDate, string strEndDate)
        {
            int iExecuteSpotLastIndex = 0;
            AQETAMRFENGINECOMLib.RfEngine mRF = new AQETAMRFENGINECOMLib.RfEngine();
            if (mRF.Initialise() != 0)
            {
                GMessage.MessageWarning("Can't Intitiallis Reach Engine : " + mRF.GetLastError());
                mRF.Terminate();
                return false;
            }
            if (mRF.SetPanel(m_strPanel, "Thailand", 1) != 0)
            {
                GMessage.MessageWarning("Can not find Panel in eTam : " + mRF.GetLastError());
                mRF.Terminate();
                return false;
            }
            if (mRF.AddDemographic(strTarget_eTam) != 0)
            {
                GMessage.MessageWarning("Can not find Target in eTam : " + mRF.GetLastError());
                mRF.Terminate();
                return false;
            }
            if (mRF.SetRestrictSpotsToCommercialMinutes(Convert.ToInt32(true), 2) != 0)
            {
                GMessage.MessageWarning("Can not set restrict spots to commercial minutes in eTam : " + mRF.GetLastError());
                mRF.Terminate();
                return false;
            }
            SqlConnection conn = new SqlConnection(m_strConnection);
            conn.Open();
            string strSQL = @"Select s.Show_Date, s.Start_Time, s.End_Time,mm.ETAM_Name,s.Program
,s.Rating
,s.Net_Cost,s.Media_ID 
  From Spot_Plan s 
  Inner Join Media_Mapping_ETAM mm On s.Media_ID = mm.Media_ID 
  Inner join Media m on s.Media_ID = m.Media_ID 
  Where s.Buying_Brief_ID = @Buying_Brief_ID
  And s.Show_Date >= @ShowDate 
  And s.Show_Date <= @EndDate
  and s.Market_ID = 'THAILAND'  
  and s.Version = (select MAX(Version) AS [Version] from spot_plan_version where Buying_Brief_ID = @Buying_Brief_ID and len(Version) = 1)
  Order by s.Show_Date,(s.Start_Time+s.End_Time)/2 , s.Media_ID,s.Row_ID ";

            strSQL = @"
Select 
s.Show_Date, s.Start_Time, s.End_Time
,RIGHT('00'+CONVERT(varchar(25),(

CONVERT(int,
(CONVERT(int,LEFT(s.Start_Time,2)*60) + CONVERT(int,RIGHT(s.Start_Time,2)))+
((CONVERT(int,LEFT(s.End_Time,2)*60) + CONVERT(int,RIGHT(s.End_Time,2))) - (CONVERT(int,LEFT(s.Start_Time,2)*60) + CONVERT(int,RIGHT(s.Start_Time,2))))/2.00
)

)/60),2)
+RIGHT('00'+CONVERT(varchar(25),(

CONVERT(int,
(CONVERT(int,LEFT(s.Start_Time,2)*60) + CONVERT(int,RIGHT(s.Start_Time,2)))+
((CONVERT(int,LEFT(s.End_Time,2)*60) + CONVERT(int,RIGHT(s.End_Time,2))) - (CONVERT(int,LEFT(s.Start_Time,2)*60) + CONVERT(int,RIGHT(s.Start_Time,2))))/2.00
)

)%60),2) AS Mid_Time
,s.Length Duration
,mm.ETAM_Name,s.Program
,s.Rating
,s.Net_Cost,s.Media_ID 
  From Spot_Plan s 
  Inner Join Media_Mapping_ETAM mm On s.Media_ID = mm.Media_ID 
  Inner join Media m on s.Media_ID = m.Media_ID 
  Where s.Buying_Brief_ID = @Buying_Brief_ID
  And s.Show_Date >= @ShowDate 
  And s.Show_Date <= @EndDate
  and s.Market_ID = 'THAILAND'  
  and s.Version = (select MAX(Version) AS [Version] from spot_plan_version where Buying_Brief_ID = @Buying_Brief_ID and len(Version) = 1)
  Order by s.Show_Date,Mid_Time , s.Media_ID,s.Row_ID 
";

            SqlDataAdapter sda = new SqlDataAdapter(strSQL, conn);
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBuyingBrief;
            sda.SelectCommand.Parameters.Add("@ShowDate", SqlDbType.VarChar).Value = strStartDate;
            sda.SelectCommand.Parameters.Add("@EndDate", SqlDbType.VarChar).Value = strEndDate;
            DataTable dt = new DataTable();
            sda.Fill(dt);


            if (dt.Rows.Count == 0)
                return true;

            int intDiffDate = 35;
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                string Start_Time = dr["Start_Time"].ToString();
                string End_Time = dr["End_Time"].ToString();
                string Show_Date = dr["Show_Date"].ToString();
                string ETAM_Name = dr["ETAM_Name"].ToString();
                int Duration = Convert.ToInt32(dr["Duration"]);

                DateTime dtShowDate = new DateTime(Convert.ToInt32(Show_Date.Substring(0, 4)), Convert.ToInt32(Show_Date.Substring(4, 2)), Convert.ToInt32(Show_Date.Substring(6, 2)));

                dtShowDate = dtShowDate.AddDays(-1 * intDiffDate);


                int intSTime = (Convert.ToInt32(Start_Time.Substring(0, 2)) * 60) + Convert.ToInt32(Start_Time.Substring(2, 2));
                int intETime = (Convert.ToInt32(End_Time.Substring(0, 2)) * 60) + Convert.ToInt32(End_Time.Substring(2, 2));
                int intTotalMin = intSTime + ((intETime - intSTime) / 2);
                string strMidTime = string.Format("{0}:{1}", (intTotalMin / 60).ToString("00"), (intTotalMin % 60).ToString("00"));
                string VDate = dtShowDate.ToString("yyyyMMdd");
                string SETime = string.Format("{0}{1}", (intTotalMin / 60).ToString("00"), (intTotalMin % 60).ToString("00"));
                mRF.AddSpot(VDate, SETime, ETAM_Name, "", 0, Duration);

            }
            //=========================================================
            // Test
            //=========================================================
           //mRF.AddSpot("20130102", "121743", "3", "", 0, 15);
           //mRF.AddSpot("20130102", "221736", "1", "", 0, 15);
           //mRF.AddSpot("20130102", "134001", "1", "", 0, 15);
           //mRF.AddSpot("20130101", "134751", "2", "", 0, 15);
           //mRF.AddSpot("20130102", "235254", "1", "", 0, 15);
           //mRF.AddSpot("20130102", "093519", "3", "", 0, 15);
           //mRF.AddSpot("20130101", "191202", "3", "", 0, 15);
           //mRF.AddSpot("20130101", "195001", "4", "", 0, 15);
           //mRF.AddSpot("20130101", "151242", "1", "", 0, 15);
           //mRF.AddSpot("20130102", "100037", "1", "", 0, 15);
           //mRF.AddSpot("20130101", "081934", "3", "", 0, 15);
           //mRF.AddSpot("20130101", "114323", "3", "", 0, 15);

            //=========================================================
            // End Test
            //=========================================================

            if (mRF.ProcessRequest() != 0)
            {
                GMessage.MessageWarning("An error occurred while Process Request in eTam : " + mRF.GetLastError());
                mRF.Terminate();
                return false;
            }

            int iResultRowCount = mRF.GetDetailRowCount();

            if (iResultRowCount < 0)
            {
                GMessage.MessageWarning("An error occurred while Process Request in eTam : " + mRF.GetLastError());
                mRF.Terminate();
                return false;
            }

            double apdOfficialCummulativeSpotBySpotReach = 0;
            double apdOfficialCummulativeSpotBySpotReachPercent = 0;
            double apdOfficialFrequency = 0;
            double apdCumeTarps = 0;
            double apdTarp = 0;
            double apdThresholdTarp = 0;
            double apdProjection = 0;
            double apdThresholdProjection = 0;
            double adIncrementalReach = 0;
            object apEffectiveReachArray, apExclusiveReachArray, apEffectiveReachUnitsArray, apExclusiveReachUnitsArray;
            System.DateTime apdDate = default(System.DateTime);
            string apsChannel = null;
            string apsProgram = null;
            double apdCost = 0;
            int apnFromMinuteOfDay = 0;
            int apnToMinuteOfDay = 0;
            int apnSpotDuration = 0;


            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("Spot");
            dtResult.Columns.Add("apdOfficialCummulativeSpotBySpotReach");
            dtResult.Columns.Add("apdOfficialCummulativeSpotBySpotReachPercent");
            dtResult.Columns.Add("apdOfficialFrequency");
            dtResult.Columns.Add("apdCumeTarps");
            dtResult.Columns.Add("apdTarp");
            dtResult.Columns.Add("apdThresholdTarp");
            dtResult.Columns.Add("apdProjection");
            dtResult.Columns.Add("apdThresholdProjection");
            dtResult.Columns.Add("adIncrementalReach");
            dtResult.Columns.Add("apEffectiveReachArray1");
            dtResult.Columns.Add("apEffectiveReachArray2");
            dtResult.Columns.Add("apEffectiveReachArray3");
            dtResult.Columns.Add("apEffectiveReachArray4");
            dtResult.Columns.Add("apEffectiveReachArray5");
            dtResult.Columns.Add("apEffectiveReachArray6");
            dtResult.Columns.Add("apEffectiveReachArray7");
            dtResult.Columns.Add("apEffectiveReachArray8");
            dtResult.Columns.Add("apEffectiveReachArray9");
            dtResult.Columns.Add("apEffectiveReachArray10");
            dtResult.Columns.Add("apExclusiveReachArray1");
            dtResult.Columns.Add("apExclusiveReachArray2");
            dtResult.Columns.Add("apExclusiveReachArray3");
            dtResult.Columns.Add("apExclusiveReachArray4");
            dtResult.Columns.Add("apExclusiveReachArray5");
            dtResult.Columns.Add("apExclusiveReachArray6");
            dtResult.Columns.Add("apExclusiveReachArray7");
            dtResult.Columns.Add("apExclusiveReachArray8");
            dtResult.Columns.Add("apExclusiveReachArray9");
            dtResult.Columns.Add("apExclusiveReachArray10");
            dtResult.Columns.Add("apEffectiveReachUnitsArray1");
            dtResult.Columns.Add("apEffectiveReachUnitsArray2");
            dtResult.Columns.Add("apEffectiveReachUnitsArray3");
            dtResult.Columns.Add("apEffectiveReachUnitsArray4");
            dtResult.Columns.Add("apEffectiveReachUnitsArray5");
            dtResult.Columns.Add("apEffectiveReachUnitsArray6");
            dtResult.Columns.Add("apEffectiveReachUnitsArray7");
            dtResult.Columns.Add("apEffectiveReachUnitsArray8");
            dtResult.Columns.Add("apEffectiveReachUnitsArray9");
            dtResult.Columns.Add("apEffectiveReachUnitsArray10");
            dtResult.Columns.Add("apExclusiveReachUnitsArray1");
            dtResult.Columns.Add("apExclusiveReachUnitsArray2");
            dtResult.Columns.Add("apExclusiveReachUnitsArray3");
            dtResult.Columns.Add("apExclusiveReachUnitsArray4");
            dtResult.Columns.Add("apExclusiveReachUnitsArray5");
            dtResult.Columns.Add("apExclusiveReachUnitsArray6");
            dtResult.Columns.Add("apExclusiveReachUnitsArray7");
            dtResult.Columns.Add("apExclusiveReachUnitsArray8");
            dtResult.Columns.Add("apExclusiveReachUnitsArray9");
            dtResult.Columns.Add("apExclusiveReachUnitsArray10");
            dtResult.Columns.Add("Date");
            dtResult.Columns.Add("apdDate");
            dtResult.Columns.Add("DayOfWeek");
            dtResult.Columns.Add("Time");
            dtResult.Columns.Add("apsChannel");
            dtResult.Columns.Add("apsProgram");
            dtResult.Columns.Add("apdCost");
            dtResult.Columns.Add("apnFromMinuteOfDay");
            dtResult.Columns.Add("apnToMinuteOfDay");
            dtResult.Columns.Add("apnSpotDuration");

            int iApproveRowIndex = 0;
            //for (int i = 0; i < 12; i++)
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                mRF.GetDetailRow(i
                   , 0
                   , out apdOfficialCummulativeSpotBySpotReach
                   , out apdOfficialCummulativeSpotBySpotReachPercent
                   , out apdOfficialFrequency
                   , out apdCumeTarps
                   , out apdTarp
                   , out apdThresholdTarp
                   , out apdProjection
                   , out apdThresholdProjection
                   , out adIncrementalReach
                   , out apEffectiveReachArray
                   , out apExclusiveReachArray
                   , out apEffectiveReachUnitsArray
                   , out apExclusiveReachUnitsArray
                   , out apdDate
                   , out apsChannel
                   , out apsProgram
                   , out apdCost
                   , out apnFromMinuteOfDay
                   , out apnToMinuteOfDay
                   , out apnSpotDuration);

                DataRow drInput = dt.Rows[i];

                DataRow dr = dtResult.NewRow();
                dr["Spot"] = i + 1;
                dr["apdOfficialCummulativeSpotBySpotReach"] = apdOfficialCummulativeSpotBySpotReach;
                dr["apdOfficialCummulativeSpotBySpotReachPercent"] = apdOfficialCummulativeSpotBySpotReachPercent.ToString("#,##0.000");
                dr["apdOfficialFrequency"] = apdOfficialFrequency;

                //dr["apdTarp"] = apdTarp.ToString("#,##0.000");
                dr["apdTarp"] = Convert.ToDouble(drInput["Rating"]).ToString("#,##0.000");
                

                //dr["apdCumeTarps"] = apdCumeTarps.ToString("#,##0.000");
                if (i == 0)
                    dr["apdCumeTarps"] = Convert.ToDecimal(drInput["Rating"]).ToString("#,##0.000");
                else
                    dr["apdCumeTarps"] = (Convert.ToDecimal(drInput["Rating"]) + Convert.ToDecimal(dtResult.Rows[i - 1]["apdCumeTarps"])).ToString("#,##0.000");

                dr["apdThresholdTarp"] = apdThresholdTarp;
                dr["apdProjection"] = apdProjection;
                dr["apdThresholdProjection"] = apdThresholdProjection;
                dr["adIncrementalReach"] = adIncrementalReach;

                double[] tmpEffectiveReachArray = (double[])apEffectiveReachArray;
                dr["apEffectiveReachArray1"] = tmpEffectiveReachArray[0].ToString("#,##0.000");
                dr["apEffectiveReachArray2"] = tmpEffectiveReachArray[1].ToString("#,##0.000");
                dr["apEffectiveReachArray3"] = tmpEffectiveReachArray[2].ToString("#,##0.000");
                dr["apEffectiveReachArray4"] = tmpEffectiveReachArray[3].ToString("#,##0.000");
                dr["apEffectiveReachArray5"] = tmpEffectiveReachArray[4].ToString("#,##0.000");
                dr["apEffectiveReachArray6"] = tmpEffectiveReachArray[5].ToString("#,##0.000");
                dr["apEffectiveReachArray7"] = tmpEffectiveReachArray[6].ToString("#,##0.000");
                dr["apEffectiveReachArray8"] = tmpEffectiveReachArray[7].ToString("#,##0.000");
                dr["apEffectiveReachArray9"] = tmpEffectiveReachArray[8].ToString("#,##0.000");
                dr["apEffectiveReachArray10"] = tmpEffectiveReachArray[9].ToString("#,##0.000");

                double[] tmpExclusiveReachArray = (double[])apExclusiveReachArray;
                dr["apExclusiveReachArray1"] = tmpExclusiveReachArray[0];
                dr["apExclusiveReachArray2"] = tmpExclusiveReachArray[1];
                dr["apExclusiveReachArray3"] = tmpExclusiveReachArray[2];
                dr["apExclusiveReachArray4"] = tmpExclusiveReachArray[3];
                dr["apExclusiveReachArray5"] = tmpExclusiveReachArray[4];
                dr["apExclusiveReachArray6"] = tmpExclusiveReachArray[5];
                dr["apExclusiveReachArray7"] = tmpExclusiveReachArray[6];
                dr["apExclusiveReachArray8"] = tmpExclusiveReachArray[7];
                dr["apExclusiveReachArray9"] = tmpExclusiveReachArray[8];
                dr["apExclusiveReachArray10"] = tmpExclusiveReachArray[9];

                double[] tmpEffectiveReachUnitsArray = (double[])apEffectiveReachUnitsArray;
                dr["apEffectiveReachUnitsArray1"] = tmpEffectiveReachUnitsArray[0];
                dr["apEffectiveReachUnitsArray2"] = tmpEffectiveReachUnitsArray[1];
                dr["apEffectiveReachUnitsArray3"] = tmpEffectiveReachUnitsArray[2];
                dr["apEffectiveReachUnitsArray4"] = tmpEffectiveReachUnitsArray[3];
                dr["apEffectiveReachUnitsArray5"] = tmpEffectiveReachUnitsArray[4];
                dr["apEffectiveReachUnitsArray6"] = tmpEffectiveReachUnitsArray[5];
                dr["apEffectiveReachUnitsArray7"] = tmpEffectiveReachUnitsArray[6];
                dr["apEffectiveReachUnitsArray8"] = tmpEffectiveReachUnitsArray[7];
                dr["apEffectiveReachUnitsArray9"] = tmpEffectiveReachUnitsArray[8];
                dr["apEffectiveReachUnitsArray10"] = tmpEffectiveReachUnitsArray[9];

                double[] tmpExclusiveReachUnitsArray = (double[])apExclusiveReachUnitsArray;
                dr["apExclusiveReachUnitsArray1"] = tmpExclusiveReachUnitsArray[0];
                dr["apExclusiveReachUnitsArray2"] = tmpExclusiveReachUnitsArray[1];
                dr["apExclusiveReachUnitsArray3"] = tmpExclusiveReachUnitsArray[2];
                dr["apExclusiveReachUnitsArray4"] = tmpExclusiveReachUnitsArray[3];
                dr["apExclusiveReachUnitsArray5"] = tmpExclusiveReachUnitsArray[4];
                dr["apExclusiveReachUnitsArray6"] = tmpExclusiveReachUnitsArray[5];
                dr["apExclusiveReachUnitsArray7"] = tmpExclusiveReachUnitsArray[6];
                dr["apExclusiveReachUnitsArray8"] = tmpExclusiveReachUnitsArray[7];
                dr["apExclusiveReachUnitsArray9"] = tmpExclusiveReachUnitsArray[8];
                dr["apExclusiveReachUnitsArray10"] = tmpExclusiveReachUnitsArray[9];

                dr["Date"] = Convert.ToDateTime(apdDate).AddDays(intDiffDate).ToString("dd/MM/yyyy");
                dr["apdDate"] = Convert.ToDateTime(apdDate).ToString("dd/MM/yyyy");
                dr["DayOfWeek"] = Convert.ToDateTime(apdDate).DayOfWeek;
                int iHr = (int)(Convert.ToDouble(apnFromMinuteOfDay) / 60.00);
                int iMin = Convert.ToInt32(apnFromMinuteOfDay % 60.00);
                dr["Time"] = iHr.ToString("00") + ":" + iMin.ToString("00");
                //dr["apsChannel"] = apsChannel;
                //dr["apsProgram"] = apsProgram;
                dr["apsChannel"] = drInput["Media_ID"];
                dr["apsProgram"] = drInput["Program"];
                dr["apdCost"] = Convert.ToDouble(drInput["Net_Cost"]).ToString("#,##0.00");
                dr["apnFromMinuteOfDay"] = apnFromMinuteOfDay;
                dr["apnToMinuteOfDay"] = apnToMinuteOfDay;
                dr["apnSpotDuration"] = apnSpotDuration;
                dtResult.Rows.Add(dr);



                DataRow[] drArray = m_dtResult.Select(string.Format("Approve_Date='{0}' AND Approve_Station = '{1}' AND Approve_Start_Time = '{2}' AND  Approve_End_Time = '{3}'"
                    , dr["Date"].ToString()
                    , dr["apsChannel"].ToString()
                    , drInput["Start_Time"].ToString()
                    , drInput["End_Time"].ToString()));
                DataRow drNewResult = null;
                bool bInsert = false;
                if (drArray.Length <= 0)
                {
                    drNewResult = m_dtResult.NewRow();
                }
                else
                {
                    for (int iShiftRow = 0; iShiftRow < drArray.Length; iShiftRow++)
                    {
                        string strExecute_Spot = drArray[iShiftRow]["Execute_Spot"].ToString() ;
                        if (strExecute_Spot == "")
                        {
                            drNewResult = drArray[iShiftRow];
                            iApproveRowIndex++;
                            break;
                        }
                    }
                    if (drNewResult == null)
                    {
                        bInsert = true;
                        drNewResult = m_dtResult.NewRow();
                    }
                }


                drNewResult["Execute_Start_Time"] = drInput["Start_Time"];
                drNewResult["Execute_End_Time"] = drInput["End_Time"];

                drNewResult["Execute_Spot"] = dr["Spot"];
                drNewResult["Execute_Date"] = dr["Date"];
                drNewResult["Execute_Day"] = dr["DayOfWeek"];
                drNewResult["Execute_Time"] = dr["Time"];
                drNewResult["Execute_Station"] = dr["apsChannel"];
                drNewResult["Execute_Program"] = dr["apsProgram"];
                drNewResult["Execute_Cost"] = dr["apdCost"];
                drNewResult["Execute_GRP"] = dr["apdTarp"];
                drNewResult["Execute_CummeGRP"] = dr["apdCumeTarps"];
                drNewResult["Execute_IncrementalReach"] = dr["apdOfficialCummulativeSpotBySpotReachPercent"];
                if (Convert.ToDouble(dr["apEffectiveReachArray1"]) != 0)
                    drNewResult["Execute_EffectiveReach1"] = dr["apEffectiveReachArray1"];
                if (Convert.ToDouble(dr["apEffectiveReachArray2"]) != 0)
                    drNewResult["Execute_EffectiveReach2"] = dr["apEffectiveReachArray2"];
                if (Convert.ToDouble(dr["apEffectiveReachArray3"]) != 0)
                    drNewResult["Execute_EffectiveReach3"] = dr["apEffectiveReachArray3"];
                if (Convert.ToDouble(dr["apEffectiveReachArray4"]) != 0)
                    drNewResult["Execute_EffectiveReach4"] = dr["apEffectiveReachArray4"];
                if (Convert.ToDouble(dr["apEffectiveReachArray5"]) != 0)
                    drNewResult["Execute_EffectiveReach5"] = dr["apEffectiveReachArray5"];

                if (drArray.Length <= 0 || bInsert)
                {

                    string strTmp = "0";
                    string strTmpNew = ConvertToyyyyMMdd(drNewResult["Execute_Date"].ToString()) + drNewResult["Execute_Time"].ToString().Replace(":","");
                    while (Convert.ToInt64(strTmp) < Convert.ToInt64(strTmpNew) && iApproveRowIndex < m_dtResult.Rows.Count)
                    {
                        if (m_dtResult.Rows[iApproveRowIndex]["Approve_Date"].ToString() != "")
                        {
                            strTmp = ConvertToyyyyMMdd(m_dtResult.Rows[iApproveRowIndex]["Approve_Date"].ToString()) + m_dtResult.Rows[iApproveRowIndex]["Approve_Time"].ToString().Replace(":", "");
                        }
                        iApproveRowIndex++;
                    }
                    iApproveRowIndex--;
                    if (iApproveRowIndex == m_dtResult.Rows.Count - 1)
                    {
                        m_dtResult.Rows.Add(drNewResult);
                    }
                    else
                    {
                        
                        m_dtResult.Rows.InsertAt(drNewResult, iApproveRowIndex);
                    }
                }
                iExecuteSpotLastIndex = GetExecuteLastIndex();
                iApproveRowIndex = iExecuteSpotLastIndex;
            }

            conn.Close();
            return true;

        }
        private bool InsertReach_Test()
        {
            try
            {
                int iExecuteSpotLastIndex = 0;
                AQETAMRFENGINECOMLib.RfEngine mRF = new AQETAMRFENGINECOMLib.RfEngine();
                mRF.Initialise();
                mRF.SetPanel("THCABLE", "Thailand", 1);
                mRF.AddDemographic("T176");
                mRF.SetRestrictSpotsToCommercialMinutes(Convert.ToInt32(true), 2);
                mRF.AddSpot("20130102", "121743", "3", "", 0, 15);
                mRF.AddSpot("20130102", "221736", "1", "", 0, 15);
                mRF.AddSpot("20130102", "134001", "1", "", 0, 15);
                mRF.AddSpot("20130101", "134751", "2", "", 0, 15);
                mRF.AddSpot("20130102", "235254", "1", "", 0, 15);
                mRF.AddSpot("20130102", "093519", "3", "", 0, 15);
                mRF.AddSpot("20130101", "191202", "3", "", 0, 15);
                mRF.AddSpot("20130101", "195001", "4", "", 0, 15);
                mRF.AddSpot("20130101", "151242", "1", "", 0, 15);
                mRF.AddSpot("20130102", "100037", "1", "", 0, 15);
                mRF.AddSpot("20130101", "081934", "3", "", 0, 15);
                mRF.AddSpot("20130101", "114323", "3", "", 0, 15);
                mRF.ProcessRequest();
                int iResultRowCount = mRF.GetDetailRowCount();

                double apdOfficialCummulativeSpotBySpotReach = 0;
                double apdOfficialCummulativeSpotBySpotReachPercent = 0;
                double apdOfficialFrequency = 0;
                double apdCumeTarps = 0;
                double apdTarp = 0;
                double apdThresholdTarp = 0;
                double apdProjection = 0;
                double apdThresholdProjection = 0;
                double adIncrementalReach = 0;
                object apEffectiveReachArray, apExclusiveReachArray, apEffectiveReachUnitsArray, apExclusiveReachUnitsArray;
                System.DateTime apdDate = default(System.DateTime);
                string apsChannel = null;
                string apsProgram = null;
                double apdCost = 0;
                int apnFromMinuteOfDay = 0;
                int apnToMinuteOfDay = 0;
                int apnSpotDuration = 0;


                DataTable dtResult = new DataTable();
                dtResult.Columns.Add("Spot");
                dtResult.Columns.Add("apdOfficialCummulativeSpotBySpotReach");
                dtResult.Columns.Add("apdOfficialCummulativeSpotBySpotReachPercent");
                dtResult.Columns.Add("apdOfficialFrequency");
                dtResult.Columns.Add("apdCumeTarps");
                dtResult.Columns.Add("apdTarp");
                dtResult.Columns.Add("apdThresholdTarp");
                dtResult.Columns.Add("apdProjection");
                dtResult.Columns.Add("apdThresholdProjection");
                dtResult.Columns.Add("adIncrementalReach");
                dtResult.Columns.Add("apEffectiveReachArray1");
                dtResult.Columns.Add("apEffectiveReachArray2");
                dtResult.Columns.Add("apEffectiveReachArray3");
                dtResult.Columns.Add("apEffectiveReachArray4");
                dtResult.Columns.Add("apEffectiveReachArray5");
                dtResult.Columns.Add("apEffectiveReachArray6");
                dtResult.Columns.Add("apEffectiveReachArray7");
                dtResult.Columns.Add("apEffectiveReachArray8");
                dtResult.Columns.Add("apEffectiveReachArray9");
                dtResult.Columns.Add("apEffectiveReachArray10");
                dtResult.Columns.Add("apExclusiveReachArray1");
                dtResult.Columns.Add("apExclusiveReachArray2");
                dtResult.Columns.Add("apExclusiveReachArray3");
                dtResult.Columns.Add("apExclusiveReachArray4");
                dtResult.Columns.Add("apExclusiveReachArray5");
                dtResult.Columns.Add("apExclusiveReachArray6");
                dtResult.Columns.Add("apExclusiveReachArray7");
                dtResult.Columns.Add("apExclusiveReachArray8");
                dtResult.Columns.Add("apExclusiveReachArray9");
                dtResult.Columns.Add("apExclusiveReachArray10");
                dtResult.Columns.Add("apEffectiveReachUnitsArray1");
                dtResult.Columns.Add("apEffectiveReachUnitsArray2");
                dtResult.Columns.Add("apEffectiveReachUnitsArray3");
                dtResult.Columns.Add("apEffectiveReachUnitsArray4");
                dtResult.Columns.Add("apEffectiveReachUnitsArray5");
                dtResult.Columns.Add("apEffectiveReachUnitsArray6");
                dtResult.Columns.Add("apEffectiveReachUnitsArray7");
                dtResult.Columns.Add("apEffectiveReachUnitsArray8");
                dtResult.Columns.Add("apEffectiveReachUnitsArray9");
                dtResult.Columns.Add("apEffectiveReachUnitsArray10");
                dtResult.Columns.Add("apExclusiveReachUnitsArray1");
                dtResult.Columns.Add("apExclusiveReachUnitsArray2");
                dtResult.Columns.Add("apExclusiveReachUnitsArray3");
                dtResult.Columns.Add("apExclusiveReachUnitsArray4");
                dtResult.Columns.Add("apExclusiveReachUnitsArray5");
                dtResult.Columns.Add("apExclusiveReachUnitsArray6");
                dtResult.Columns.Add("apExclusiveReachUnitsArray7");
                dtResult.Columns.Add("apExclusiveReachUnitsArray8");
                dtResult.Columns.Add("apExclusiveReachUnitsArray9");
                dtResult.Columns.Add("apExclusiveReachUnitsArray10");
                dtResult.Columns.Add("Date");
                dtResult.Columns.Add("apdDate");
                dtResult.Columns.Add("DayOfWeek");
                dtResult.Columns.Add("Time");
                dtResult.Columns.Add("apsChannel");
                dtResult.Columns.Add("apsProgram");
                dtResult.Columns.Add("apdCost");
                dtResult.Columns.Add("apnFromMinuteOfDay");
                dtResult.Columns.Add("apnToMinuteOfDay");
                dtResult.Columns.Add("apnSpotDuration");

                int iApproveRowIndex = 0;
                for (int i = 0; i < 12; i++)
                {
                    mRF.GetDetailRow(i
                       , 0
                       , out apdOfficialCummulativeSpotBySpotReach
                       , out apdOfficialCummulativeSpotBySpotReachPercent
                       , out apdOfficialFrequency
                       , out apdCumeTarps
                       , out apdTarp
                       , out apdThresholdTarp
                       , out apdProjection
                       , out apdThresholdProjection
                       , out adIncrementalReach
                       , out apEffectiveReachArray
                       , out apExclusiveReachArray
                       , out apEffectiveReachUnitsArray
                       , out apExclusiveReachUnitsArray
                       , out apdDate
                       , out apsChannel
                       , out apsProgram
                       , out apdCost
                       , out apnFromMinuteOfDay
                       , out apnToMinuteOfDay
                       , out apnSpotDuration);

                    //DataRow drInput = dt.Rows[i];

                    DataRow dr = dtResult.NewRow();
                    dr["Spot"] = i + 1;
                    dr["apdOfficialCummulativeSpotBySpotReach"] = apdOfficialCummulativeSpotBySpotReach;
                    dr["apdOfficialCummulativeSpotBySpotReachPercent"] = apdOfficialCummulativeSpotBySpotReachPercent.ToString("#,##0.000");
                    dr["apdOfficialFrequency"] = apdOfficialFrequency;

                    //dr["apdTarp"] = apdTarp.ToString("#,##0.000");
                    //dr["apdTarp"] = Convert.ToDouble(drInput["Rating"]).ToString("#,##0.000");


                    //dr["apdCumeTarps"] = apdCumeTarps.ToString("#,##0.000");
                    //if (i == 0)
                    //    dr["apdCumeTarps"] = Convert.ToDecimal(drInput["Rating"]).ToString("#,##0.000");
                    //else
                    //    dr["apdCumeTarps"] = (Convert.ToDecimal(drInput["Rating"]) + Convert.ToDecimal(dtResult.Rows[i - 1]["apdCumeTarps"])).ToString("#,##0.000");

                    dr["apdThresholdTarp"] = apdThresholdTarp;
                    dr["apdProjection"] = apdProjection;
                    dr["apdThresholdProjection"] = apdThresholdProjection;
                    dr["adIncrementalReach"] = adIncrementalReach;

                    double[] tmpEffectiveReachArray = (double[])apEffectiveReachArray;
                    dr["apEffectiveReachArray1"] = tmpEffectiveReachArray[0].ToString("#,##0.000");
                    dr["apEffectiveReachArray2"] = tmpEffectiveReachArray[1].ToString("#,##0.000");
                    dr["apEffectiveReachArray3"] = tmpEffectiveReachArray[2].ToString("#,##0.000");
                    dr["apEffectiveReachArray4"] = tmpEffectiveReachArray[3].ToString("#,##0.000");
                    dr["apEffectiveReachArray5"] = tmpEffectiveReachArray[4].ToString("#,##0.000");
                    dr["apEffectiveReachArray6"] = tmpEffectiveReachArray[5].ToString("#,##0.000");
                    dr["apEffectiveReachArray7"] = tmpEffectiveReachArray[6].ToString("#,##0.000");
                    dr["apEffectiveReachArray8"] = tmpEffectiveReachArray[7].ToString("#,##0.000");
                    dr["apEffectiveReachArray9"] = tmpEffectiveReachArray[8].ToString("#,##0.000");
                    dr["apEffectiveReachArray10"] = tmpEffectiveReachArray[9].ToString("#,##0.000");

                    double[] tmpExclusiveReachArray = (double[])apExclusiveReachArray;
                    dr["apExclusiveReachArray1"] = tmpExclusiveReachArray[0];
                    dr["apExclusiveReachArray2"] = tmpExclusiveReachArray[1];
                    dr["apExclusiveReachArray3"] = tmpExclusiveReachArray[2];
                    dr["apExclusiveReachArray4"] = tmpExclusiveReachArray[3];
                    dr["apExclusiveReachArray5"] = tmpExclusiveReachArray[4];
                    dr["apExclusiveReachArray6"] = tmpExclusiveReachArray[5];
                    dr["apExclusiveReachArray7"] = tmpExclusiveReachArray[6];
                    dr["apExclusiveReachArray8"] = tmpExclusiveReachArray[7];
                    dr["apExclusiveReachArray9"] = tmpExclusiveReachArray[8];
                    dr["apExclusiveReachArray10"] = tmpExclusiveReachArray[9];

                    double[] tmpEffectiveReachUnitsArray = (double[])apEffectiveReachUnitsArray;
                    dr["apEffectiveReachUnitsArray1"] = tmpEffectiveReachUnitsArray[0];
                    dr["apEffectiveReachUnitsArray2"] = tmpEffectiveReachUnitsArray[1];
                    dr["apEffectiveReachUnitsArray3"] = tmpEffectiveReachUnitsArray[2];
                    dr["apEffectiveReachUnitsArray4"] = tmpEffectiveReachUnitsArray[3];
                    dr["apEffectiveReachUnitsArray5"] = tmpEffectiveReachUnitsArray[4];
                    dr["apEffectiveReachUnitsArray6"] = tmpEffectiveReachUnitsArray[5];
                    dr["apEffectiveReachUnitsArray7"] = tmpEffectiveReachUnitsArray[6];
                    dr["apEffectiveReachUnitsArray8"] = tmpEffectiveReachUnitsArray[7];
                    dr["apEffectiveReachUnitsArray9"] = tmpEffectiveReachUnitsArray[8];
                    dr["apEffectiveReachUnitsArray10"] = tmpEffectiveReachUnitsArray[9];

                    double[] tmpExclusiveReachUnitsArray = (double[])apExclusiveReachUnitsArray;
                    dr["apExclusiveReachUnitsArray1"] = tmpExclusiveReachUnitsArray[0];
                    dr["apExclusiveReachUnitsArray2"] = tmpExclusiveReachUnitsArray[1];
                    dr["apExclusiveReachUnitsArray3"] = tmpExclusiveReachUnitsArray[2];
                    dr["apExclusiveReachUnitsArray4"] = tmpExclusiveReachUnitsArray[3];
                    dr["apExclusiveReachUnitsArray5"] = tmpExclusiveReachUnitsArray[4];
                    dr["apExclusiveReachUnitsArray6"] = tmpExclusiveReachUnitsArray[5];
                    dr["apExclusiveReachUnitsArray7"] = tmpExclusiveReachUnitsArray[6];
                    dr["apExclusiveReachUnitsArray8"] = tmpExclusiveReachUnitsArray[7];
                    dr["apExclusiveReachUnitsArray9"] = tmpExclusiveReachUnitsArray[8];
                    dr["apExclusiveReachUnitsArray10"] = tmpExclusiveReachUnitsArray[9];

                    dr["Date"] = Convert.ToDateTime(apdDate).AddDays(intDiffDate).ToString("dd/MM/yyyy");
                    dr["apdDate"] = Convert.ToDateTime(apdDate).ToString("dd/MM/yyyy");
                    dr["DayOfWeek"] = Convert.ToDateTime(apdDate).DayOfWeek;
                    int iHr = (int)(Convert.ToDouble(apnFromMinuteOfDay) / 60.00);
                    int iMin = Convert.ToInt32(apnFromMinuteOfDay % 60.00);
                    dr["Time"] = iHr.ToString("00") + ":" + iMin.ToString("00");
                    //dr["apsChannel"] = apsChannel;
                    //dr["apsProgram"] = apsProgram;
                    //dr["apsChannel"] = drInput["Media_ID"];
                    //dr["apsProgram"] = drInput["Program"];
                    //dr["apdCost"] = Convert.ToDouble(drInput["Net_Cost"]).ToString("#,##0.00");
                    dr["apnFromMinuteOfDay"] = apnFromMinuteOfDay;
                    dr["apnToMinuteOfDay"] = apnToMinuteOfDay;
                    dr["apnSpotDuration"] = apnSpotDuration;
                    dtResult.Rows.Add(dr);



                    DataRow[] drArray = m_dtResult.Select(string.Format("Approve_Date='{0}' AND Approve_Station = '{1}' AND Approve_Start_Time = '{2}' AND  Approve_End_Time = '{3}'"
                        , dr["Date"].ToString()
                        , dr["apsChannel"].ToString()
                        , ""//drInput["Start_Time"].ToString()
                        , ""//drInput["End_Time"].ToString()
                        ));
                    DataRow drNewResult = null;
                    bool bInsert = false;
                    if (drArray.Length <= 0)
                    {
                        drNewResult = m_dtResult.NewRow();
                    }
                    else
                    {
                        for (int iShiftRow = 0; iShiftRow < drArray.Length; iShiftRow++)
                        {
                            string strExecute_Spot = drArray[iShiftRow]["Execute_Spot"].ToString();
                            if (strExecute_Spot == "")
                            {
                                drNewResult = drArray[iShiftRow];
                                iApproveRowIndex++;
                                break;
                            }
                        }
                        if (drNewResult == null)
                        {
                            bInsert = true;
                            drNewResult = m_dtResult.NewRow();
                        }
                    }


                    //drNewResult["Execute_Start_Time"] = drInput["Start_Time"];
                    //drNewResult["Execute_End_Time"] = drInput["End_Time"];

                    drNewResult["Execute_Spot"] = dr["Spot"];
                    drNewResult["Execute_Date"] = dr["Date"];
                    drNewResult["Execute_Day"] = dr["DayOfWeek"];
                    drNewResult["Execute_Time"] = dr["Time"];
                    drNewResult["Execute_Station"] = dr["apsChannel"];
                    drNewResult["Execute_Program"] = dr["apsProgram"];
                    drNewResult["Execute_Cost"] = dr["apdCost"];
                    drNewResult["Execute_GRP"] = dr["apdTarp"];
                    drNewResult["Execute_CummeGRP"] = dr["apdCumeTarps"];
                    drNewResult["Execute_IncrementalReach"] = dr["apdOfficialCummulativeSpotBySpotReachPercent"];
                    if (Convert.ToDouble(dr["apEffectiveReachArray1"]) != 0)
                        drNewResult["Execute_EffectiveReach1"] = dr["apEffectiveReachArray1"];
                    if (Convert.ToDouble(dr["apEffectiveReachArray2"]) != 0)
                        drNewResult["Execute_EffectiveReach2"] = dr["apEffectiveReachArray2"];
                    if (Convert.ToDouble(dr["apEffectiveReachArray3"]) != 0)
                        drNewResult["Execute_EffectiveReach3"] = dr["apEffectiveReachArray3"];
                    if (Convert.ToDouble(dr["apEffectiveReachArray4"]) != 0)
                        drNewResult["Execute_EffectiveReach4"] = dr["apEffectiveReachArray4"];
                    if (Convert.ToDouble(dr["apEffectiveReachArray5"]) != 0)
                        drNewResult["Execute_EffectiveReach5"] = dr["apEffectiveReachArray5"];

                    if (drArray.Length <= 0 || bInsert)
                    {

                        string strTmp = "0";
                        string strTmpNew = ConvertToyyyyMMdd(drNewResult["Execute_Date"].ToString()) + drNewResult["Execute_Time"].ToString().Replace(":", "");
                        while (Convert.ToInt64(strTmp) < Convert.ToInt64(strTmpNew) && iApproveRowIndex < m_dtResult.Rows.Count)
                        {
                            if (m_dtResult.Rows[iApproveRowIndex]["Approve_Date"].ToString() != "")
                            {
                                strTmp = ConvertToyyyyMMdd(m_dtResult.Rows[iApproveRowIndex]["Approve_Date"].ToString()) + m_dtResult.Rows[iApproveRowIndex]["Approve_Time"].ToString().Replace(":", "");
                            }
                            iApproveRowIndex++;
                        }
                        iApproveRowIndex--;
                        if (iApproveRowIndex == m_dtResult.Rows.Count - 1)
                        {
                            m_dtResult.Rows.Add(drNewResult);
                        }
                        else
                        {

                            m_dtResult.Rows.InsertAt(drNewResult, iApproveRowIndex);
                        }
                    }
                    iExecuteSpotLastIndex = GetExecuteLastIndex();
                    iApproveRowIndex = iExecuteSpotLastIndex;
                }
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                throw;
            }


            return true;

        }
        private int GetExecuteLastIndex()
        {
            int iMaxIndex = 0;
            int iMaxValue = 0;
            for (int i = 0; i < m_dtResult.Rows.Count; i++)
            {
                if (m_dtResult.Rows[i]["Execute_Spot"].ToString() == "")
                    continue;
                if (Convert.ToInt32(m_dtResult.Rows[i]["Execute_Spot"].ToString()) > iMaxValue)
                {
                    iMaxValue = Convert.ToInt32(m_dtResult.Rows[i]["Execute_Spot"].ToString());
                    iMaxIndex = i;
                }
            }
            return iMaxIndex;
        }
        private string ConvertToyyyyMMdd(string strddMMyyyy)
        {
            string[] str = strddMMyyyy.Split('/');
            return str[2] + str[1] + str[0];
        }
        private bool InsertReach_Actual(string strBuyingBrief, string strTarget_eTam, string strStartDate, string strEndDate)
        {
            AQETAMRFENGINECOMLib.RfEngine mRF = new AQETAMRFENGINECOMLib.RfEngine();
            if (mRF.Initialise() != 0)
            {
                GMessage.MessageWarning("Can't Intitiallis Reach Engine : " + mRF.GetLastError());
                mRF.Terminate();
                return false;
            }
            if (mRF.SetPanel(m_strPanel, "Thailand", 1) != 0)
            {
                GMessage.MessageWarning("Can not find Panel in eTam : " + mRF.GetLastError());
                mRF.Terminate();
                return false;
            }
            if (mRF.AddDemographic(strTarget_eTam) != 0)
            {
                GMessage.MessageWarning("Can not find Target in eTam : " + mRF.GetLastError());
                mRF.Terminate();
                return false;
            }
            if (mRF.SetRestrictSpotsToCommercialMinutes(Convert.ToInt32(true), 2) != 0)
            {
                GMessage.MessageWarning("Can not set restrict spots to commercial minutes in eTam : " + mRF.GetLastError());
                mRF.Terminate();
                return false;
            }
            SqlConnection conn = new SqlConnection(m_strConnection);
            conn.Open();
            string strSQL = @"
SELECT * 
,RIGHT('00'+CONVERT(varchar(25),(

CONVERT(int,
(CONVERT(int,LEFT(Start_Time,2)*60) + CONVERT(int,RIGHT(Start_Time,2)))+
((CONVERT(int,LEFT(End_Time,2)*60) + CONVERT(int,RIGHT(End_Time,2))) - (CONVERT(int,LEFT(Start_Time,2)*60) + CONVERT(int,RIGHT(Start_Time,2))))/2.00
)

)/60),2)
+RIGHT('00'+CONVERT(varchar(25),(

CONVERT(int,
(CONVERT(int,LEFT(Start_Time,2)*60) + CONVERT(int,RIGHT(Start_Time,2)))+
((CONVERT(int,LEFT(End_Time,2)*60) + CONVERT(int,RIGHT(End_Time,2))) - (CONVERT(int,LEFT(Start_Time,2)*60) + CONVERT(int,RIGHT(Start_Time,2))))/2.00
)

)%60),2) AS Mid_Time

FROM (
SELECT 
	Spots_Match.Buying_Brief_ID
	,Spot_Plan.Net_Cost
	,Spots_Match.Length
	,Spot_Plan.Media_ID
	,mm.ETAM_Name
	,Spots_Match.SP_Show_Date Show_Date
	,Spot_Plan.Start_Time 
	,Spot_Plan.End_Time
	,Spots_Match.Actual_Time
	,Spots_Match.Actual_Rating AS Rating
	,Spot_Plan.Spots
	,Spot_Plan.Program AS Program
    ,isnull(Program_Type.Short_Name,'UNKNOWN') ProgramType
	,Spots_Match.PDNO CopyLineName
	,Spot_Plan.Package SpotType
	,Material.Thai_Name MaterialName
	,Media_Vendor.English_Name VendorName
	,MasterVendor.English_Name MasterVendorName
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
LEFT JOIN Program_Type
    ON Program_Type.Program_Type = Spot_Plan.Program_Type
INNER Join Media_Mapping_ETAM mm On Spots_Match.Media_ID = mm.Media_ID 
WHERE Spots_Match.Actual_Rating IS NOT NULL		
AND Buying_Brief.Buying_Brief_ID = @Buying_Brief_ID
AND Spots_Match.SP_Show_Date BETWEEN @ShowDate AND @EndDate

UNION ALL		

SELECT 
	Spots_Match.Buying_Brief_ID
	, 0 Net_Cost
	,Spots_Match.Length
	,Spots_Match.Media_ID
	,mm.ETAM_Name
	,Spots_Match.Show_Date
	,Spots_Match.Actual_Time Start_Time
	,Spots_Match.Actual_Time End_Time
	,Spots_Match.Actual_Time 
	,Spots_Match.Actual_Rating 
	, 1 Spots
	,Spots_Match.[Program_Name] BookingProgramName
    ,'BONUS' ProgramType
	,Spots_Match.PDNO CopyLineName
	,'Unknown Bonus' SpotType
	,PDNO MaterialName
	,Media_Vendor.English_Name VendorName
	,MasterVendor.English_Name MasterVendorName
FROM Spots_Match  with(nolock)
	INNER JOIN Media_Vendor
		ON Media_Vendor.Media_Vendor_ID = Spots_Match.Media_Vendor
	INNER JOIN Media_Vendor MasterVendor
		ON Media_Vendor.Master_Group = MasterVendor.Media_Vendor_ID 	
	INNER JOIN Buying_Brief		
		ON Spots_Match.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
	INNER Join Media_Mapping_ETAM mm On Spots_Match.Media_ID = mm.Media_ID 
WHERE Spots_Match.Flag_Bonus = 1 AND Spots_Match.Actual_Rating IS NOT NULL
AND Buying_Brief.Buying_Brief_ID = @Buying_Brief_ID
AND Spots_Match.Show_Date BETWEEN @ShowDate AND @EndDate
) Rec
ORDER BY Show_Date,Mid_Time,Media_ID


 ";

            SqlDataAdapter sda = new SqlDataAdapter(strSQL, conn);
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBuyingBrief;
            sda.SelectCommand.Parameters.Add("@ShowDate", SqlDbType.VarChar).Value = strStartDate;
            sda.SelectCommand.Parameters.Add("@EndDate", SqlDbType.VarChar).Value = strEndDate;
            DataTable dt = new DataTable();
            sda.Fill(dt);

            if (dt.Rows.Count == 0)
                return true;
            int intDiffDate = 35;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                string Start_Time = dr["Start_Time"].ToString();
                string End_Time = dr["End_Time"].ToString();
                string Show_Date = dr["Show_Date"].ToString();
                string ETAM_Name = dr["ETAM_Name"].ToString();

                DateTime dtShowDate = new DateTime(Convert.ToInt32(Show_Date.Substring(0, 4)), Convert.ToInt32(Show_Date.Substring(4, 2)), Convert.ToInt32(Show_Date.Substring(6, 2)));

                dtShowDate = dtShowDate.AddDays(-1 * intDiffDate);


                int intSTime = (Convert.ToInt32(Start_Time.Substring(0, 2)) * 60) + Convert.ToInt32(Start_Time.Substring(2, 2));
                int intETime = (Convert.ToInt32(End_Time.Substring(0, 2)) * 60) + Convert.ToInt32(End_Time.Substring(2, 2));
                int intTotalMin = intSTime + ((intETime - intSTime) / 2);
                string strMidTime = string.Format("{0}:{1}", (intTotalMin / 60).ToString("00"), (intTotalMin % 60).ToString("00"));
                string VDate = dtShowDate.ToString("yyyyMMdd");
                string SETime = string.Format("{0}{1}", (intTotalMin / 60).ToString("00"), (intTotalMin % 60).ToString("00"));
                mRF.AddSpot(VDate, SETime, ETAM_Name, "", 0, 0);

            }
            if (mRF.ProcessRequest() != 0)
            {
                GMessage.MessageWarning("An error occurred while Process Request in eTam : " + mRF.GetLastError());
                mRF.Terminate();
                return false;
            }

            int iResultRowCount = mRF.GetDetailRowCount();

            if (iResultRowCount < 0)
            {
                GMessage.MessageWarning("An error occurred while Process Request in eTam : " + mRF.GetLastError());
                mRF.Terminate();
                return false;
            }

            double apdOfficialCummulativeSpotBySpotReach = 0;
            double apdOfficialCummulativeSpotBySpotReachPercent = 0;
            double apdOfficialFrequency = 0;
            double apdCumeTarps = 0;
            double apdTarp = 0;
            double apdThresholdTarp = 0;
            double apdProjection = 0;
            double apdThresholdProjection = 0;
            double adIncrementalReach = 0;
            object apEffectiveReachArray, apExclusiveReachArray, apEffectiveReachUnitsArray, apExclusiveReachUnitsArray;
            System.DateTime apdDate = default(System.DateTime);
            string apsChannel = null;
            string apsProgram = null;
            double apdCost = 0;
            int apnFromMinuteOfDay = 0;
            int apnToMinuteOfDay = 0;
            int apnSpotDuration = 0;


            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("Spot");
            dtResult.Columns.Add("apdOfficialCummulativeSpotBySpotReach");
            dtResult.Columns.Add("apdOfficialCummulativeSpotBySpotReachPercent");
            dtResult.Columns.Add("apdOfficialFrequency");
            dtResult.Columns.Add("apdCumeTarps");
            dtResult.Columns.Add("apdTarp");
            dtResult.Columns.Add("apdThresholdTarp");
            dtResult.Columns.Add("apdProjection");
            dtResult.Columns.Add("apdThresholdProjection");
            dtResult.Columns.Add("adIncrementalReach");
            dtResult.Columns.Add("apEffectiveReachArray1");
            dtResult.Columns.Add("apEffectiveReachArray2");
            dtResult.Columns.Add("apEffectiveReachArray3");
            dtResult.Columns.Add("apEffectiveReachArray4");
            dtResult.Columns.Add("apEffectiveReachArray5");
            dtResult.Columns.Add("apEffectiveReachArray6");
            dtResult.Columns.Add("apEffectiveReachArray7");
            dtResult.Columns.Add("apEffectiveReachArray8");
            dtResult.Columns.Add("apEffectiveReachArray9");
            dtResult.Columns.Add("apEffectiveReachArray10");
            dtResult.Columns.Add("apExclusiveReachArray1");
            dtResult.Columns.Add("apExclusiveReachArray2");
            dtResult.Columns.Add("apExclusiveReachArray3");
            dtResult.Columns.Add("apExclusiveReachArray4");
            dtResult.Columns.Add("apExclusiveReachArray5");
            dtResult.Columns.Add("apExclusiveReachArray6");
            dtResult.Columns.Add("apExclusiveReachArray7");
            dtResult.Columns.Add("apExclusiveReachArray8");
            dtResult.Columns.Add("apExclusiveReachArray9");
            dtResult.Columns.Add("apExclusiveReachArray10");
            dtResult.Columns.Add("apEffectiveReachUnitsArray1");
            dtResult.Columns.Add("apEffectiveReachUnitsArray2");
            dtResult.Columns.Add("apEffectiveReachUnitsArray3");
            dtResult.Columns.Add("apEffectiveReachUnitsArray4");
            dtResult.Columns.Add("apEffectiveReachUnitsArray5");
            dtResult.Columns.Add("apEffectiveReachUnitsArray6");
            dtResult.Columns.Add("apEffectiveReachUnitsArray7");
            dtResult.Columns.Add("apEffectiveReachUnitsArray8");
            dtResult.Columns.Add("apEffectiveReachUnitsArray9");
            dtResult.Columns.Add("apEffectiveReachUnitsArray10");
            dtResult.Columns.Add("apExclusiveReachUnitsArray1");
            dtResult.Columns.Add("apExclusiveReachUnitsArray2");
            dtResult.Columns.Add("apExclusiveReachUnitsArray3");
            dtResult.Columns.Add("apExclusiveReachUnitsArray4");
            dtResult.Columns.Add("apExclusiveReachUnitsArray5");
            dtResult.Columns.Add("apExclusiveReachUnitsArray6");
            dtResult.Columns.Add("apExclusiveReachUnitsArray7");
            dtResult.Columns.Add("apExclusiveReachUnitsArray8");
            dtResult.Columns.Add("apExclusiveReachUnitsArray9");
            dtResult.Columns.Add("apExclusiveReachUnitsArray10");
            dtResult.Columns.Add("Date");
            dtResult.Columns.Add("apdDate");
            dtResult.Columns.Add("DayOfWeek");
            dtResult.Columns.Add("Time");
            dtResult.Columns.Add("apsChannel");
            dtResult.Columns.Add("apsProgram");
            dtResult.Columns.Add("apdCost");
            dtResult.Columns.Add("apnFromMinuteOfDay");
            dtResult.Columns.Add("apnToMinuteOfDay");
            dtResult.Columns.Add("apnSpotDuration");

            int iApproveRowIndex = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                mRF.GetDetailRow(i
                   , 0
                   , out apdOfficialCummulativeSpotBySpotReach
                   , out apdOfficialCummulativeSpotBySpotReachPercent
                   , out apdOfficialFrequency
                   , out apdCumeTarps
                   , out apdTarp
                   , out apdThresholdTarp
                   , out apdProjection
                   , out apdThresholdProjection
                   , out adIncrementalReach
                   , out apEffectiveReachArray
                   , out apExclusiveReachArray
                   , out apEffectiveReachUnitsArray
                   , out apExclusiveReachUnitsArray
                   , out apdDate
                   , out apsChannel
                   , out apsProgram
                   , out apdCost
                   , out apnFromMinuteOfDay
                   , out apnToMinuteOfDay
                   , out apnSpotDuration);

                DataRow drInput = dt.Rows[i];

                DataRow dr = dtResult.NewRow();
                dr["Spot"] = i + 1;
                dr["apdOfficialCummulativeSpotBySpotReach"] = apdOfficialCummulativeSpotBySpotReach;
                dr["apdOfficialCummulativeSpotBySpotReachPercent"] = apdOfficialCummulativeSpotBySpotReachPercent.ToString("#,##0.000");
                dr["apdOfficialFrequency"] = apdOfficialFrequency;

                //dr["apdTarp"] = apdTarp.ToString("#,##0.000");
                dr["apdTarp"] = Convert.ToDouble(drInput["Rating"]).ToString("#,##0.000");

                //dr["apdCumeTarps"] = apdCumeTarps.ToString("#,##0.000");
                if (i == 0)
                    dr["apdCumeTarps"] = Convert.ToDecimal(drInput["Rating"]).ToString("#,##0.000");
                else
                    dr["apdCumeTarps"] = (Convert.ToDecimal(drInput["Rating"]) + Convert.ToDecimal(dtResult.Rows[i - 1]["apdCumeTarps"])).ToString("#,##0.000");

                dr["apdThresholdTarp"] = apdThresholdTarp;
                dr["apdProjection"] = apdProjection;
                dr["apdThresholdProjection"] = apdThresholdProjection;
                dr["adIncrementalReach"] = adIncrementalReach;

                double[] tmpEffectiveReachArray = (double[])apEffectiveReachArray;
                dr["apEffectiveReachArray1"] = tmpEffectiveReachArray[0].ToString("#,##0.000");
                dr["apEffectiveReachArray2"] = tmpEffectiveReachArray[1].ToString("#,##0.000");
                dr["apEffectiveReachArray3"] = tmpEffectiveReachArray[2].ToString("#,##0.000");
                dr["apEffectiveReachArray4"] = tmpEffectiveReachArray[3].ToString("#,##0.000");
                dr["apEffectiveReachArray5"] = tmpEffectiveReachArray[4].ToString("#,##0.000");
                dr["apEffectiveReachArray6"] = tmpEffectiveReachArray[5].ToString("#,##0.000");
                dr["apEffectiveReachArray7"] = tmpEffectiveReachArray[6].ToString("#,##0.000");
                dr["apEffectiveReachArray8"] = tmpEffectiveReachArray[7].ToString("#,##0.000");
                dr["apEffectiveReachArray9"] = tmpEffectiveReachArray[8].ToString("#,##0.000");
                dr["apEffectiveReachArray10"] = tmpEffectiveReachArray[9].ToString("#,##0.000");

                double[] tmpExclusiveReachArray = (double[])apExclusiveReachArray;
                dr["apExclusiveReachArray1"] = tmpExclusiveReachArray[0];
                dr["apExclusiveReachArray2"] = tmpExclusiveReachArray[1];
                dr["apExclusiveReachArray3"] = tmpExclusiveReachArray[2];
                dr["apExclusiveReachArray4"] = tmpExclusiveReachArray[3];
                dr["apExclusiveReachArray5"] = tmpExclusiveReachArray[4];
                dr["apExclusiveReachArray6"] = tmpExclusiveReachArray[5];
                dr["apExclusiveReachArray7"] = tmpExclusiveReachArray[6];
                dr["apExclusiveReachArray8"] = tmpExclusiveReachArray[7];
                dr["apExclusiveReachArray9"] = tmpExclusiveReachArray[8];
                dr["apExclusiveReachArray10"] = tmpExclusiveReachArray[9];

                double[] tmpEffectiveReachUnitsArray = (double[])apEffectiveReachUnitsArray;
                dr["apEffectiveReachUnitsArray1"] = tmpEffectiveReachUnitsArray[0];
                dr["apEffectiveReachUnitsArray2"] = tmpEffectiveReachUnitsArray[1];
                dr["apEffectiveReachUnitsArray3"] = tmpEffectiveReachUnitsArray[2];
                dr["apEffectiveReachUnitsArray4"] = tmpEffectiveReachUnitsArray[3];
                dr["apEffectiveReachUnitsArray5"] = tmpEffectiveReachUnitsArray[4];
                dr["apEffectiveReachUnitsArray6"] = tmpEffectiveReachUnitsArray[5];
                dr["apEffectiveReachUnitsArray7"] = tmpEffectiveReachUnitsArray[6];
                dr["apEffectiveReachUnitsArray8"] = tmpEffectiveReachUnitsArray[7];
                dr["apEffectiveReachUnitsArray9"] = tmpEffectiveReachUnitsArray[8];
                dr["apEffectiveReachUnitsArray10"] = tmpEffectiveReachUnitsArray[9];

                double[] tmpExclusiveReachUnitsArray = (double[])apExclusiveReachUnitsArray;
                dr["apExclusiveReachUnitsArray1"] = tmpExclusiveReachUnitsArray[0];
                dr["apExclusiveReachUnitsArray2"] = tmpExclusiveReachUnitsArray[1];
                dr["apExclusiveReachUnitsArray3"] = tmpExclusiveReachUnitsArray[2];
                dr["apExclusiveReachUnitsArray4"] = tmpExclusiveReachUnitsArray[3];
                dr["apExclusiveReachUnitsArray5"] = tmpExclusiveReachUnitsArray[4];
                dr["apExclusiveReachUnitsArray6"] = tmpExclusiveReachUnitsArray[5];
                dr["apExclusiveReachUnitsArray7"] = tmpExclusiveReachUnitsArray[6];
                dr["apExclusiveReachUnitsArray8"] = tmpExclusiveReachUnitsArray[7];
                dr["apExclusiveReachUnitsArray9"] = tmpExclusiveReachUnitsArray[8];
                dr["apExclusiveReachUnitsArray10"] = tmpExclusiveReachUnitsArray[9];

                dr["Date"] = Convert.ToDateTime(apdDate).AddDays(intDiffDate).ToString("dd/MM/yyyy");
                dr["apdDate"] = Convert.ToDateTime(apdDate).ToString("dd/MM/yyyy");
                dr["DayOfWeek"] = Convert.ToDateTime(apdDate).DayOfWeek;
                int iHr = (int)(Convert.ToDouble(apnFromMinuteOfDay) / 60.00);
                int iMin = Convert.ToInt32(apnFromMinuteOfDay % 60.00);
                dr["Time"] = iHr.ToString("00") + ":" + iMin.ToString("00");
                //dr["apsChannel"] = apsChannel;
                //dr["apsProgram"] = apsProgram;
                dr["apsChannel"] = drInput["Media_ID"];
                dr["apsProgram"] = drInput["Program"];
                dr["apdCost"] = Convert.ToDouble(drInput["Net_Cost"]).ToString("#,##0.00");
                dr["apnFromMinuteOfDay"] = apnFromMinuteOfDay;
                dr["apnToMinuteOfDay"] = apnToMinuteOfDay;
                dr["apnSpotDuration"] = apnSpotDuration;
                dtResult.Rows.Add(dr);



                DataRow[] drArray = m_dtResult.Select(string.Format("Execute_Date='{0}' AND Execute_Station = '{1}' AND Execute_Start_Time = '{2}' AND  Execute_End_Time = '{3}'"
                    , dr["Date"].ToString()
                    , dr["apsChannel"].ToString()
                    , drInput["Start_Time"].ToString()
                    , drInput["End_Time"].ToString()));
                DataRow drNewResult = null;
                if (drArray.Length <= 0)
                {
                    drNewResult = m_dtResult.NewRow();
                }
                else
                {
                    for (int iShiftRow = 0; iShiftRow < drArray.Length; iShiftRow++)
                    {
                        string strActual_Spot = drArray[iShiftRow]["Actual_Spot"].ToString();
                        if (strActual_Spot == "")
                        {
                            drNewResult = drArray[iShiftRow];
                            break;
                        }
                    }
                }


                drNewResult["Actual_Start_Time"] = drInput["Start_Time"];
                drNewResult["Actual_End_Time"] = drInput["End_Time"];

                drNewResult["Actual_Spot"] = dr["Spot"];
                drNewResult["Actual_Date"] = dr["Date"];
                drNewResult["Actual_Day"] = dr["DayOfWeek"];
                drNewResult["Actual_Time"] = dr["Time"];
                drNewResult["Actual_Station"] = dr["apsChannel"];
                drNewResult["Actual_Program"] = dr["apsProgram"];
                drNewResult["Actual_Cost"] = dr["apdCost"];
                drNewResult["Actual_GRP"] = dr["apdTarp"];
                drNewResult["Actual_CummeGRP"] = dr["apdCumeTarps"];
                drNewResult["Actual_IncrementalReach"] = dr["apdOfficialCummulativeSpotBySpotReachPercent"];
                if (Convert.ToDouble(dr["apEffectiveReachArray1"]) != 0)
                    drNewResult["Actual_EffectiveReach1"] = dr["apEffectiveReachArray1"];
                if (Convert.ToDouble(dr["apEffectiveReachArray2"]) != 0)
                    drNewResult["Actual_EffectiveReach2"] = dr["apEffectiveReachArray2"];
                if (Convert.ToDouble(dr["apEffectiveReachArray3"]) != 0)
                    drNewResult["Actual_EffectiveReach3"] = dr["apEffectiveReachArray3"];
                if (Convert.ToDouble(dr["apEffectiveReachArray4"]) != 0)
                    drNewResult["Actual_EffectiveReach4"] = dr["apEffectiveReachArray4"];
                if (Convert.ToDouble(dr["apEffectiveReachArray5"]) != 0)
                    drNewResult["Actual_EffectiveReach5"] = dr["apEffectiveReachArray5"];

                if (drArray.Length <= 0)
                {
                    m_dtResult.Rows.InsertAt(drNewResult, iApproveRowIndex);
                }
                iApproveRowIndex++;
            }

            conn.Close();

            return true;


        }

        private void gvCondition_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
                DataLoading();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            GroupM.UTL.ExcelUtil.ExportXlsx(gvDetail, 1, true);
        }

    }
}