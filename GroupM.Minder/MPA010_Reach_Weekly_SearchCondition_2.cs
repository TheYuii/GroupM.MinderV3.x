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
    public partial class MPA010_Reach_Weekly_SearchCondition_2 : Form
    {
        string UserName = "";
        string Password = "";
        string ModeSelect = "";
        public string BuyingBriefBefore { get; set; }
        public MPA010_Reach_Weekly_SearchCondition_2(string strUserName, string strPassword,string strModeSelect)
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

        }
        public DataTable m_dtSelected = null;
        private void DataLoading()
        {
            gvClient.AutoGenerateColumns = false;
            gvClient.DataSource = m_dtSelected;
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

        private void ResetCondition_First()
        {
            
            //=============================================
            // Week 1
            //=============================================
            DataTable dt = ((DataTable)gvClient.DataSource);
            string strStartDate = dt.Rows[0]["StartDate"].ToString();
            int iYear = Convert.ToInt32(strStartDate.Substring(0, 4));
            int iMonth = Convert.ToInt32(strStartDate.Substring(4, 2));
            dtStartDate1.Value = new DateTime(iYear, iMonth, 1);
            dtEndDate1.Value = dtStartDate1.Value.AddDays(6);

            DateTime dtTemp = new DateTime();
            dtTemp = dtBaseDate0.Value;
            if (dtTemp.DayOfWeek == dtEndDate1.Value.DayOfWeek)
            {
                dtTemp = dtTemp.AddDays(1);
            }
            while (dtTemp.DayOfWeek != dtEndDate1.Value.DayOfWeek)
            {
                dtTemp = dtTemp.AddDays(-1);
            }

            int BackBaseDate = Convert.ToInt32(-1 * ((TimeSpan)(Convert.ToDateTime(dtEndDate1.Value) - Convert.ToDateTime(dtTemp))).TotalDays);



            dtBaseDate1.Value = dtEndDate1.Value.AddDays(BackBaseDate - 1);
            txtDOWEndDate1.Text = dtEndDate1.Value.DayOfWeek.ToString();
            txtDOWBaseDate1.Text = dtBaseDate1.Value.DayOfWeek.ToString();

            //=============================================
            // Week 2
            //=============================================
            dtStartDate2.Value = dtEndDate1.Value.AddDays(1);
            dtEndDate2.Value = dtEndDate1.Value.AddDays(7);
            dtBaseDate2.Value = dtBaseDate1.Value.AddDays(7);
            txtDOWEndDate2.Text = dtEndDate2.Value.DayOfWeek.ToString();
            txtDOWBaseDate2.Text = dtBaseDate2.Value.DayOfWeek.ToString();

            //=============================================
            // Week 3
            //=============================================
            dtStartDate3.Value = dtEndDate2.Value.AddDays(1);
            dtEndDate3.Value = dtEndDate2.Value.AddDays(7);
            dtBaseDate3.Value = dtBaseDate2.Value.AddDays(7);
            txtDOWEndDate3.Text = dtEndDate3.Value.DayOfWeek.ToString();
            txtDOWBaseDate3.Text = dtBaseDate3.Value.DayOfWeek.ToString();

            //=============================================
            // Week 4
            //=============================================
            dtStartDate4.Value = dtEndDate3.Value.AddDays(1);
            dtEndDate4.Value = dtEndDate3.Value.AddDays(7);
            dtBaseDate4.Value = dtBaseDate3.Value.AddDays(7);
            txtDOWEndDate4.Text = dtEndDate4.Value.DayOfWeek.ToString();
            txtDOWBaseDate4.Text = dtBaseDate4.Value.DayOfWeek.ToString();

            //=============================================
            // Week 5
            //=============================================
            dtStartDate5.Value = dtEndDate4.Value.AddDays(1);
            dtEndDate5.Value = dtEndDate4.Value.AddDays(7);
            if (dtStartDate5.Value.Month != dtStartDate4.Value.Month)
            {
                chkWeek5.Checked = false;
                dtStartDate5.Value = DateTime.Now;
                dtEndDate5.Value = DateTime.Now;
            }
            else if (dtStartDate5.Value.Month != dtEndDate5.Value.Month)
            {
                dtEndDate5.Value = new DateTime(dtStartDate5.Value.Year, dtStartDate5.Value.Month, 1);
                dtEndDate5.Value = dtEndDate5.Value.AddMonths(1);
                dtEndDate5.Value = dtEndDate5.Value.AddDays(-1);
            }
            dtBaseDate5.Value = dtEndDate5.Value.AddDays(BackBaseDate - (dtEndDate5.Value - dtStartDate5.Value).Days +1);
            txtDOWEndDate5.Text = dtEndDate5.Value.DayOfWeek.ToString();
            txtDOWBaseDate5.Text = dtBaseDate5.Value.DayOfWeek.ToString();

            //=============================================
            // Week 6
            //=============================================
            dtStartDate6.Value = dtEndDate5.Value.AddDays(1);
            dtEndDate6.Value = dtEndDate5.Value.AddDays(7);
            if (dtStartDate6.Value.Month != dtStartDate5.Value.Month)
            {
                chkWeek6.Checked = false;
                dtStartDate6.Value = DateTime.Now;
                dtEndDate6.Value = DateTime.Now;
            }
            else if (dtStartDate6.Value.Month != dtEndDate6.Value.Month)
            {
                dtEndDate6.Value = new DateTime(dtStartDate6.Value.Year, dtStartDate6.Value.Month, 1);
                dtEndDate6.Value = dtEndDate6.Value.AddMonths(1);
                dtEndDate6.Value = dtEndDate6.Value.AddDays(-1);
            }
            dtBaseDate6.Value = dtBaseDate6.Value.AddDays(BackBaseDate);
            txtDOWEndDate6.Text = dtEndDate6.Value.DayOfWeek.ToString();
            txtDOWBaseDate6.Text = dtBaseDate6.Value.DayOfWeek.ToString();
        }

        private void ResetCondition_Day()
        {
            DayOfWeek dow = DayOfWeek.Monday;
            if (rdMon.Checked)
                dow = DayOfWeek.Sunday;
            else if (rdTue.Checked)
                dow = DayOfWeek.Monday;
            else if (rdWed.Checked)
                dow = DayOfWeek.Tuesday;
            else if (rdThu.Checked)
                dow = DayOfWeek.Wednesday;
            else if (rdFri.Checked)
                dow = DayOfWeek.Thursday;
            else if (rdSat.Checked)
                dow = DayOfWeek.Friday;
            else if (rdSun.Checked)
                dow = DayOfWeek.Saturday;

            
            //=============================================
            // Week 1
            //=============================================
            DataTable dt = ((DataTable)gvClient.DataSource);
            string strStartDate = dt.Rows[0]["StartDate"].ToString();
            int iYear = Convert.ToInt32(strStartDate.Substring(0, 4));
            int iMonth = Convert.ToInt32(strStartDate.Substring(4, 2));


            dtStartDate1.Value = new DateTime(iYear, iMonth, 1);
            dtEndDate1.Value = dtStartDate1.Value;
            while(true)
            {
                if (dow == dtEndDate1.Value.DayOfWeek)
                    break;
                dtEndDate1.Value = dtEndDate1.Value.AddDays(1);
            }
            DateTime dtTemp = new DateTime();
            dtTemp = dtBaseDate0.Value;
            while (true) 
            {
                if (dtTemp.DayOfWeek == dtEndDate1.Value.DayOfWeek)
                    break;
                dtTemp = dtTemp.AddDays(-1);
            }

            int BackBaseDate = Convert.ToInt32(-1 * ((TimeSpan)(Convert.ToDateTime(dtEndDate1.Value) - Convert.ToDateTime(dtTemp))).TotalDays);

            dtBaseDate1.Value = dtEndDate1.Value.AddDays(BackBaseDate-1);
            txtDOWEndDate1.Text = dtEndDate1.Value.DayOfWeek.ToString();
            txtDOWBaseDate1.Text = dtBaseDate1.Value.DayOfWeek.ToString();

            //=============================================
            // Week 2
            //=============================================
            dtStartDate2.Value = dtEndDate1.Value.AddDays(1);
            dtEndDate2.Value = dtEndDate1.Value.AddDays(7);
            dtBaseDate2.Value = dtBaseDate1.Value.AddDays(7);
            txtDOWEndDate2.Text = dtEndDate2.Value.DayOfWeek.ToString();
            txtDOWBaseDate2.Text = dtBaseDate2.Value.DayOfWeek.ToString();

            //=============================================
            // Week 3
            //=============================================
            dtStartDate3.Value = dtEndDate2.Value.AddDays(1);
            dtEndDate3.Value = dtEndDate2.Value.AddDays(7);
            dtBaseDate3.Value = dtBaseDate2.Value.AddDays(7);
            txtDOWEndDate3.Text = dtEndDate3.Value.DayOfWeek.ToString();
            txtDOWBaseDate3.Text = dtBaseDate3.Value.DayOfWeek.ToString();

            //=============================================
            // Week 4
            //=============================================
            dtStartDate4.Value = dtEndDate3.Value.AddDays(1);
            dtEndDate4.Value = dtEndDate3.Value.AddDays(7);
            dtBaseDate4.Value = dtBaseDate3.Value.AddDays(7);
            txtDOWEndDate4.Text = dtEndDate4.Value.DayOfWeek.ToString();
            txtDOWBaseDate4.Text = dtBaseDate4.Value.DayOfWeek.ToString();

            //=============================================
            // Week 5
            //=============================================
            dtStartDate5.Value = dtEndDate4.Value.AddDays(1);
            dtEndDate5.Value = dtEndDate4.Value.AddDays(7);
            if (dtStartDate5.Value.Month != dtStartDate4.Value.Month)
            {
                chkWeek5.Checked = false;
                dtStartDate5.Value = DateTime.Now;
                dtEndDate5.Value = DateTime.Now;
            }
            else if (dtStartDate5.Value.Month != dtEndDate5.Value.Month)
            {
                dtEndDate5.Value = new DateTime(dtStartDate5.Value.Year, dtStartDate5.Value.Month, 1);
                dtEndDate5.Value = dtEndDate5.Value.AddMonths(1);
                dtEndDate5.Value = dtEndDate5.Value.AddDays(-1);
            }
            int lastBackBaseDate = Convert.ToInt32(-1 * ((TimeSpan)(Convert.ToDateTime(dtEndDate5.Value) - Convert.ToDateTime(dtStartDate5.Value))).TotalDays);
            dtBaseDate5.Value = dtBaseDate4.Value.AddDays(lastBackBaseDate);
            txtDOWEndDate5.Text = dtEndDate5.Value.DayOfWeek.ToString();
            txtDOWBaseDate5.Text = dtBaseDate5.Value.DayOfWeek.ToString();

            //=============================================
            // Week 6
            //=============================================
            dtStartDate6.Value = dtEndDate5.Value.AddDays(1);
            dtEndDate6.Value = dtEndDate5.Value.AddDays(7);
            if (dtStartDate6.Value.Month != dtStartDate5.Value.Month)
            {
                chkWeek6.Checked = false;
                dtStartDate6.Value = DateTime.Now;
                dtEndDate6.Value = DateTime.Now;
            }
            else if (dtStartDate5.Value.Month != dtEndDate5.Value.Month)
            {
                dtEndDate6.Value = new DateTime(dtStartDate6.Value.Year, dtStartDate6.Value.Month, 1);
                dtEndDate6.Value = dtEndDate6.Value.AddMonths(1);
                dtEndDate6.Value = dtEndDate6.Value.AddDays(-1);
            }
            dtBaseDate6.Value = dtBaseDate6.Value.AddDays(BackBaseDate);
            txtDOWEndDate6.Text = dtEndDate6.Value.DayOfWeek.ToString();
            txtDOWBaseDate6.Text = dtBaseDate6.Value.DayOfWeek.ToString();
        }
        private void MPA010_Reach_Weekly_SearchCondition_2_Load(object sender, EventArgs e)
        {

            dtAvailable = DateTime.Now.AddDays(-10);
            lbAvailable.Text = "*ETam Latest available date : " + dtAvailable.ToString("dd/MM/yyyy");

            DataLoading();
            if("20130920" == dtBaseDate0.Value.ToString("yyyyMMdd"))
            {
            int defualtBackBaseDate = -63;
            DataTable dt = ((DataTable)gvClient.DataSource);
            string strStartDate = dt.Rows[0]["StartDate"].ToString();
            int iYear = Convert.ToInt32(strStartDate.Substring(0, 4));
            int iMonth = Convert.ToInt32(strStartDate.Substring(4, 2));
            dtBaseDate0.Value = new DateTime(iYear, iMonth, 1).AddDays(6).AddDays(defualtBackBaseDate);
            }
            ResetCondition_First();
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
            //======================================
            // Week 1 
            //======================================
            if (chkWeek1.Checked)
            {
                if (txtDOWEndDate1.Text != txtDOWBaseDate1.Text)
                {
                    GMessage.MessageWarning("Week 1 : Day Of Week miss matched.");
                    return;
                }
                if (dtEndDate1.Value < dtStartDate1.Value)
                {
                    GMessage.MessageWarning("Week 1 : End Date must more then Start Day");
                    return;
                }
                if (dtAvailable < dtBaseDate1.Value)
                {
                    GMessage.MessageWarning("Week 1 : Base Date must smaller the Available Day");
                    return;
                }
            }

            //======================================
            // Week 2 
            //======================================
            if (chkWeek2.Checked)
            {
                if (txtDOWEndDate2.Text != txtDOWBaseDate2.Text)
                {
                    GMessage.MessageWarning("Week 2 : Day Of Week miss matched.");
                    return;
                }
                if (dtEndDate2.Value < dtStartDate2.Value)
                {
                    GMessage.MessageWarning("Week 2 : End Date must more then Start Day");
                    return;
                }
                if (dtAvailable < dtBaseDate2.Value)
                {
                    GMessage.MessageWarning("Week 2 : Base Date must smaller the Available Day");
                    return;
                }
            }

            //======================================
            // Week 3 
            //======================================
            if (chkWeek3.Checked)
            {
                if (txtDOWEndDate3.Text != txtDOWBaseDate3.Text)
                {
                    GMessage.MessageWarning("Week 3 : Day Of Week miss matched.");
                    return;
                }
                if (dtEndDate3.Value < dtStartDate3.Value)
                {
                    GMessage.MessageWarning("Week 3 : End Date must more then Start Day");
                    return;
                }
                if (dtAvailable < dtBaseDate3.Value)
                {
                    GMessage.MessageWarning("Week 3 : Base Date must smaller the Available Day");
                    return;
                }
            }

            //======================================
            // Week 4 
            //======================================
            if (chkWeek4.Checked)
            {
                if (txtDOWEndDate4.Text != txtDOWBaseDate4.Text)
                {
                    GMessage.MessageWarning("Week 4 : Day Of Week miss matched.");
                    return;
                }
                if (dtEndDate4.Value < dtStartDate4.Value)
                {
                    GMessage.MessageWarning("Week 4 : End Date must more then Start Day");
                    return;
                }
                if (dtAvailable < dtBaseDate4.Value)
                {
                    GMessage.MessageWarning("Week 4 : Base Date must smaller the Available Day");
                    return;
                }
            }

            //======================================
            // Week 5 
            //======================================
            if (chkWeek5.Checked)
            {
                if (txtDOWEndDate5.Text != txtDOWBaseDate5.Text)
                {
                    GMessage.MessageWarning("Week 5 : Day Of Week miss matched.");
                    return;
                }
                if (dtEndDate5.Value < dtStartDate5.Value)
                {
                    GMessage.MessageWarning("Week 5 : End Date must more then Start Day");
                    return;
                }
                if (dtAvailable < dtBaseDate5.Value)
                {
                    GMessage.MessageWarning("Week 5 : Base Date must smaller the Available Day");
                    return;
                }
            }

            //======================================
            // Week 6 
            //======================================
            if (chkWeek6.Checked)
            {
                if (txtDOWEndDate6.Text != txtDOWBaseDate6.Text)
                {
                    GMessage.MessageWarning("Week 6 : Day Of Week miss matched.");
                    return;
                }
                if (dtEndDate6.Value < dtStartDate6.Value)
                {
                    GMessage.MessageWarning("Week 6 : End Date must more then Start Day");
                    return;
                }
                if (dtAvailable < dtBaseDate6.Value)
                {
                    GMessage.MessageWarning("Week 6 : Base Date must smaller the Available Day");
                    return;
                }
            }         


            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void dtEndDate_ValueChanged(object sender, EventArgs e)
        {
            txtDOWEndDate5.Text = dtEndDate5.Value.DayOfWeek.ToString();

        }

        private void dtBaseDate_ValueChanged(object sender, EventArgs e)
        {
            txtDOWBaseDate5.Text = dtBaseDate5.Value.DayOfWeek.ToString();
        }

        private void btnRefreshBB_Click(object sender, EventArgs e)
        {
            DataLoading();
        }

        private void dtEndDate1_ValueChanged(object sender, EventArgs e)
        {
            txtDOWEndDate1.Text = dtEndDate1.Value.DayOfWeek.ToString();
        }
        private void dtEndDate2_ValueChanged(object sender, EventArgs e)
        {
            txtDOWEndDate2.Text = dtEndDate2.Value.DayOfWeek.ToString();
        }

        private void dtEndDate3_ValueChanged(object sender, EventArgs e)
        {
            txtDOWEndDate3.Text = dtEndDate3.Value.DayOfWeek.ToString();
        }
        private void dtEndDate4_ValueChanged(object sender, EventArgs e)
        {
            txtDOWEndDate4.Text = dtEndDate4.Value.DayOfWeek.ToString();
        }
        private void dtEndDate5_ValueChanged(object sender, EventArgs e)
        {
            txtDOWEndDate5.Text = dtEndDate5.Value.DayOfWeek.ToString();
        }
        private void dtEndDate6_ValueChanged(object sender, EventArgs e)
        {
            txtDOWEndDate6.Text = dtEndDate6.Value.DayOfWeek.ToString();
        }

        private void dtBaseDate1_ValueChanged(object sender, EventArgs e)
        {

            txtDOWBaseDate1.Text = dtBaseDate1.Value.DayOfWeek.ToString();
        }
        private void dtBaseDate2_ValueChanged(object sender, EventArgs e)
        {

            txtDOWBaseDate2.Text = dtBaseDate2.Value.DayOfWeek.ToString();
        }
        private void dtBaseDate3_ValueChanged(object sender, EventArgs e)
        {

            txtDOWBaseDate3.Text = dtBaseDate3.Value.DayOfWeek.ToString();
        }
        private void dtBaseDate4_ValueChanged(object sender, EventArgs e)
        {

            txtDOWBaseDate4.Text = dtBaseDate4.Value.DayOfWeek.ToString();
        }
        private void dtBaseDate5_ValueChanged(object sender, EventArgs e)
        {

            txtDOWBaseDate5.Text = dtBaseDate5.Value.DayOfWeek.ToString();
        }
        private void dtBaseDate6_ValueChanged(object sender, EventArgs e)
        {

            txtDOWBaseDate6.Text = dtBaseDate6.Value.DayOfWeek.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.No;
            
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (rdFirst.Checked)
                ResetCondition_First();
            else
                ResetCondition_Day();
        }

    
     
    }
}