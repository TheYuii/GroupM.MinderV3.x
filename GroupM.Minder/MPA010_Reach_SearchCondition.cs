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
    public partial class MPA010_Reach_SearchCondition : Form
    {
        string UserName = "";
        string Password = "";
        string ModeSelect = "";
        public string BuyingBriefBefore { get; set; }
        public MPA010_Reach_SearchCondition(string strUserName, string strPassword,string strModeSelect)
        {
            this.UserName = strUserName;
            this.Password = strPassword;
            this.ModeSelect = strModeSelect;
            InitializeComponent();


            if (ModeSelect == "SingleRow")
            {
                chkCheckAll.Visible = false;
                chkUnCheckAll.Visible = false;
                gvClient.Columns[0].Visible = false;
                for (int i = 0; i < gvClient.Rows.Count; i++)
                {
                    if (gvClient.Rows[i].Cells[1].Value.ToString() == BuyingBriefBefore)
                    {
                        gvClient.Rows[i].Cells[1].Selected = true;
                        break;
                    }
                }
            }
            else
            {
                chkCheckAll.Visible = true;
                chkUnCheckAll.Visible = true;
                gvClient.Columns[0].Visible = true;
            }



            dtStartDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtEndDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtEndDate.Value = dtEndDate.Value.AddMonths(1);
            dtEndDate.Value = dtEndDate.Value.AddDays(-1);
            dtBaseDate.Value = dtEndDate.Value.AddDays(-63);
            dtAvailable = DateTime.Now.AddDays(-20);
            txtDOWEndDate.Text = dtEndDate.Value.DayOfWeek.ToString();
            txtDOWBaseDate.Text = dtBaseDate.Value.DayOfWeek.ToString();
            lbAvailable.Text = "*Selected Date by " + dtAvailable.ToString("dd/MM/yyyy");
            txtDiffDate.Text = (dtEndDate.Value - dtBaseDate.Value).Days.ToString();
        }

        private void DataLoading()
        {
            //if (cboMonth.Text == "" || cboYear.Text == "")
            //    return;
            SqlConnection conn = new SqlConnection(Connection.ConnectionStringMinder);
            string strSQL = "";
                strSQL = string.Format(@"
    SELECT DISTINCT NULL chk, 
    Buying_Brief.Buying_Brief_ID BuyingBriefID
    ,spotversion.Version
    ,Buying_Brief.Media_Sub_Type MediaSubType
    ,Target.Short_Name Target
    ,Target.Target_eTam_Name TargetID
    ,Buying_Brief.Product_ID ProductID
    ,Product.Short_Name ProductName
    ,Client.English_Name ClientName
    ,Buying_Brief.Campaign_Start_Date StartDate
    ,Buying_Brief.Campaign_End_Date EndDate
    FROM Buying_Brief with(nolock)
		INNER JOIN (select Distinct Buying_Brief_ID,Version from Spot_Plan_Version where Approve <> 0 and len(version) = 1) spotversion
			ON spotversion.Buying_Brief_ID = Buying_Brief.Buying_Brief_ID
	    INNER JOIN Client ON Client.Client_ID = Buying_Brief.Client_ID
	    INNER JOIN Product ON Product.Product_ID = Buying_Brief.Product_ID
	    INNER JOIN Target ON Target.Target_ID = Buying_Brief.Primary_Target
	    INNER JOIN dbo.[fn_User_CheckPermission](@UserName,NULL,NULL) ClientPermission
	     ON ClientPermission.Client_ID = Buying_Brief.Client_ID
    	
    WHERE Buying_Brief.Media_Sub_Type IN ({0})
	    AND (
            (CONVERT(DATETIME,Buying_Brief.Campaign_Start_Date) >= @StartDate AND CONVERT(DATETIME,Buying_Brief.Campaign_Start_Date) <= @EndDate)
			    OR (CONVERT(DATETIME,Buying_Brief.Campaign_End_Date) >= @StartDate AND CONVERT(DATETIME,Buying_Brief.Campaign_End_Date) <= @EndDate)
			    OR (CONVERT(DATETIME,Buying_Brief.Campaign_Start_Date) >= @StartDate AND CONVERT(DATETIME,Buying_Brief.Campaign_End_Date) <= @EndDate)
			    )", "'"+cboMediaSubType.Text.Replace(",","','")+"'");
              

            SqlDataAdapter da = new SqlDataAdapter(strSQL, conn);
            da.SelectCommand.CommandType = CommandType.Text;
            da.SelectCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = Connection.USERID.Replace(".", "");
            da.SelectCommand.Parameters.Add("@StartDate", SqlDbType.VarChar).Value = dtStartDate.Value.ToString("yyyyMMdd");
            da.SelectCommand.Parameters.Add("@EndDate", SqlDbType.VarChar).Value = dtEndDate.Value.ToString("yyyyMMdd");
            da.SelectCommand.CommandTimeout = 0;
            DataSet ds = new DataSet();
            da.Fill(ds);
            gvClient.AutoGenerateColumns = false;
            gvClient.DataSource = ds.Tables[0];
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvClient.SelectAll();
            }
            SelectBBBefore();
        }
        private void SelectBBBefore()
        {
           
            if (ModeSelect == "SingleRow")
            {
                for (int i = 0; i < gvClient.Rows.Count; i++)
                {
                    if (gvClient.Rows[i].Cells[1].Value.ToString() == BuyingBriefBefore)
                    {
                        gvClient.Rows[i].Cells[0].Value = 1;
                        gvClient.CurrentCell = gvClient.Rows[i].Cells[1];
                        gvClient.Rows[i].Selected = true;
                        gvClient.Columns[0].Visible = false;
                        break;
                    }
                }
            }
            else
            {
                string[] strBBs = BuyingBriefBefore.Split(',');
                foreach (var BB in strBBs)
                {
                    for (int i = 0; i < gvClient.Rows.Count; i++)
                    {
                        if (gvClient.Rows[i].Cells[1].Value.ToString() == BB)
                        {
                            gvClient.Rows[i].Cells[0].Value = 1;
                            break;
                        }
                    }
                }

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

           
        private void MPA010_Reach_SearchCondition_Load(object sender, EventArgs e)
        {
            //Initial Control
            cboMediaSubType.SelectedIndex = 0;
            DataLoading();
            DevExpress.Export.ExportSettings.DefaultExportType = DevExpress.Export.ExportType.WYSIWYG;
        }
        DateTime dtAvailable = DateTime.Now;
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

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void gvClient_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ModeSelect == "SingleRow")
            {
                btnProcess.PerformClick();
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            if (txtDOWEndDate.Text != txtDOWBaseDate.Text)
            {
                GMessage.MessageWarning("Day Of Week miss matched.");
                return;
            }
            if (dtEndDate.Value < dtStartDate.Value)
            {
                GMessage.MessageWarning("End Date must more then Start Day");
                return;
            }
            if (dtAvailable < dtBaseDate.Value)
            {
                GMessage.MessageWarning("Base Date must smaller the Available Day");
                return;
            }
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void dtEndDate_ValueChanged(object sender, EventArgs e)
        {
            txtDOWEndDate.Text = dtEndDate.Value.DayOfWeek.ToString();
            txtDiffDate.Text = (dtEndDate.Value - dtBaseDate.Value).Days.ToString();
            btnRefreshBB.PerformClick();
        }

        private void dtBaseDate_ValueChanged(object sender, EventArgs e)
        {
            txtDOWBaseDate.Text = dtBaseDate.Value.DayOfWeek.ToString();
            txtDiffDate.Text = (dtEndDate.Value - dtBaseDate.Value).Days.ToString();
        }

        private void btnRefreshBB_Click(object sender, EventArgs e)
        {
            DataLoading();
        }

        private void dtStartDate_ValueChanged(object sender, EventArgs e)
        {
            btnRefreshBB.PerformClick();
        }

        private void cboMediaSubType_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRefreshBB.PerformClick();
        }
    }
}