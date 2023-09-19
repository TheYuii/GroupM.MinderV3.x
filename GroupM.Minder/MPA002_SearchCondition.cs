using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace  GroupM.Minder
{
    public partial class MPA002_SearchCondition : Form
    {
        bool bIsRefreshClient = false;
        public MPA002_SearchCondition()
        {
            DateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTo = DateFrom.AddMonths(1).AddDays(-1);
            DataGridClient = null;
            DataGridProduct = null;
            //MediaSubType = "TV";
            InitializeComponent();
        }

        private DateTime m_dtFrom;
        public DateTime DateFrom
        {
            get { return m_dtFrom; }
            set { m_dtFrom = value; }
        }

        private DateTime m_dtTo;
        public DateTime DateTo
        {
            get { return m_dtTo; }
            set { m_dtTo = value; }
        }




        private DataTable m_dataTemp;
        public DataTable DataTemp
        {
            get { return m_dataTemp; }
            set { m_dataTemp = value; }
        }






        // ========== Add by atiwat.t [2015-07-16] ========== 

        private int m_conditionOption;
        public int ConditionOption
        {
            get { return m_conditionOption; }
            set { m_conditionOption = value; }
        }

        private string m_monthFrom;
        public string MonthFrom
        {
            get { return m_monthFrom; }
            set { m_monthFrom = value; }
        }

        private string m_monthTo;
        public string MonthTo
        {
            get { return m_monthTo; }
            set { m_monthTo = value; }
        }

        private string m_yearFrom;
        public string YearFrom
        {
            get { return m_yearFrom; }
            set { m_yearFrom = value; }
        }

        private string m_yearTo;
        public string YearTo
        {
            get { return m_yearTo; }
            set { m_yearTo = value; }
        }

        private string m_month;
        public string Month
        {
            get { return m_month; }
            set { m_month = value; }
        }

        private string m_year;
        public string Year
        {
            get { return m_year; }
            set { m_year = value; }
        }

        private bool m_highSpeed;
        public bool HighSpeed
        {
            get { return m_highSpeed; }
            set { m_highSpeed = value; }
        }



        // ================================================== 

        private DataGridView m_dgvBuyingBrief;
        public DataGridView DataGridBuyingBrief
        {
            get { return m_dgvBuyingBrief; }
            set { m_dgvBuyingBrief = value; }
        }

        private DataGridView m_gvProduct;
        public DataGridView DataGridProduct
        {
            get { return m_gvProduct; }
            set { m_gvProduct = value; }
        }

        private DataGridView m_gvClient;
        public DataGridView DataGridClient
        {
            get { return m_gvClient; }
            set { m_gvClient = value; }
        }

        private string m_strMediaSubType;
        public string MediaSubType
        {
            get { return m_strMediaSubType; }
            set { m_strMediaSubType = value; }
        }

        private string m_strMediaType;
        public string MediaType
        {
            get { return m_strMediaType; }
            set { m_strMediaType = value; }
        }

        private void RefreshBuyingBriefList()
        {
            if (!bIsRefreshClient)
                return;
            if (this.rdoMonthYearRange.Checked == true)
            {
                if (string.IsNullOrEmpty(this.cbbMonthFrom.Text) ||
                    string.IsNullOrEmpty(this.cbbMonthTo.Text) ||
                    string.IsNullOrEmpty(this.cbbYearFrom.Text) ||
                    string.IsNullOrEmpty(this.cbbYearTo.Text))
                {
                    return;
                }
            }
            else if (this.rdoMonthYear.Checked == true)
            {
                if (string.IsNullOrEmpty(this.cbbMonth.Text) ||
                    string.IsNullOrEmpty(this.cbbYear.Text))
                {
                    return;
                }
            }

            string StartDate = dtpFrom.Value.ToShortDateString().Replace("/", "");
            string EndDate = dtpTo.Value.ToShortDateString().Replace("/", "");


            SqlConnection conn = new SqlConnection(Connection.ConnectionStringMinder);
            //            string strSQL = @"  select
            //	                                Buying_Brief_ID, 
            //	                                [Description]
            //	                                --,
            //	                                --Media_Sub_Type,
            //	                                --Client.Client_ID
            //                                from Buying_Brief
            //                                INNER JOIN dbo.[fn_User_CheckPermission](@UserName,NULL,NULL) Client
            //	                                ON Client.Client_ID = Buying_Brief.Client_ID
            //                                where Buying_Brief_ID like @BuyingBriefId + '%'
            //                                and Media_Sub_Type = @MediaSubType 
            //                                and Campaign_Start_Date BETWEEN @StartDate AND @EndDate
            //                                and	Campaign_End_Date BETWEEN @StartDate AND @EndDate 
            //                            ";
            string sqlGetBuyingBrief = @"  select
	                                                Buying_Brief_ID, 
	                                                [Description]
	                                                --,
	                                                --Media_Sub_Type,
	                                                --Client.Client_ID
                                                from Buying_Brief
                                                INNER JOIN dbo.[fn_User_CheckPermission](@UserName,NULL,NULL) Client
	                                                ON Client.Client_ID = Buying_Brief.Client_ID
                                                where Buying_Brief_ID like @BuyingBriefId + '%'
                                                and charindex(','+Media_Sub_Type+',' , ','+@MediaSubType + ',') > 0
                                                --and Media_Sub_Type = @MediaSubType 
                                                and Campaign_Start_Date BETWEEN @StartDate AND @EndDate
                                                and	Campaign_End_Date BETWEEN @StartDate AND @EndDate 
                                            ";
            SqlDataAdapter da = new SqlDataAdapter(sqlGetBuyingBrief, conn);
            da.SelectCommand.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = Connection.USERID.Replace(".", "");
            da.SelectCommand.Parameters.Add("@MediaSubType", SqlDbType.NVarChar).Value = cbxMediaSubType.Text;

            // cboMediaSubType.Text;

            // ========== Modify by Atiwat.t [2015-07-16] ==========

            //da.SelectCommand.Parameters.Add("@StartDate", SqlDbType.NVarChar).Value = dtpFrom.Value.ToString("yyyyMMdd");
            //da.SelectCommand.Parameters.Add("@EndDate", SqlDbType.NVarChar).Value = dtpTo.Value.ToString("yyyyMMdd");

            string strStart = string.Empty;
            string strEnd = string.Empty;

            if (this.rdoDate.Checked == true)
            {
                strStart = this.dtpFrom.Value.ToString("yyyyMMdd");
                strEnd = this.dtpTo.Value.ToString("yyyyMMdd");
            }
            else if (this.rdoMonthYearRange.Checked == true)
            {
                strStart = this.cbbYearFrom.Text + this.cbbMonthFrom.Text + "01";
                strEnd = this.cbbYearTo.Text + this.cbbMonthTo.Text + DateTime.DaysInMonth(Convert.ToInt32(this.cbbYearTo.Text), Convert.ToInt32(this.cbbMonthTo.Text)).ToString("00");
            }
            else if (this.rdoMonthYear.Checked == true)
            {
                strStart = this.cbbYear.Text + this.cbbMonth.Text + "01";
                strEnd = this.cbbYear.Text + this.cbbMonth.Text + DateTime.DaysInMonth(Convert.ToInt32(this.cbbYear.Text), Convert.ToInt32(this.cbbMonth.Text)).ToString("00");
            }

            da.SelectCommand.Parameters.Add("@StartDate", SqlDbType.NVarChar).Value = strStart;
            da.SelectCommand.Parameters.Add("@EndDate", SqlDbType.NVarChar).Value = strEnd;

            // =====================================================

            da.SelectCommand.Parameters.Add("@BuyingBriefId", SqlDbType.NVarChar).Value = txtBuyingBriefNo.Text;
            da.SelectCommand.CommandTimeout = 0;
            DataSet ds = new DataSet();
            da.Fill(ds);

            DataTemp = ds.Tables[0];








            dgvBuyingBrief.AutoGenerateColumns = false;
            dgvBuyingBrief.DataSource = ds.Tables[0];
            //dgvBuyingBrief.Columns["Buying_Brief_ID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            //dgvBuyingBrief.ReadOnly = true;
            if (ds.Tables[0].Rows.Count > 0)
            {
                dgvBuyingBrief.SelectAll();
            }
        }

        private void RefreshProductList()
        {
            if (!bIsRefreshClient)
                return;

            if (this.rdoMonthYearRange.Checked == true)
            {
                if (string.IsNullOrEmpty(this.cbbMonthFrom.Text) ||
                    string.IsNullOrEmpty(this.cbbMonthTo.Text) ||
                    string.IsNullOrEmpty(this.cbbYearFrom.Text) ||
                    string.IsNullOrEmpty(this.cbbYearTo.Text))
                {
                    return;
                }
            }
            else if (this.rdoMonthYear.Checked == true)
            {
                if (string.IsNullOrEmpty(this.cbbMonth.Text) ||
                    string.IsNullOrEmpty(this.cbbYear.Text))
                {
                    return;
                }
            }

            SqlConnection conn = new SqlConnection(Connection.ConnectionStringMinder);
            //string strSQL = "dbo.[MPA_get_Client]";
            string strSQL = "dbo.[MPA_get_Product]";
            SqlDataAdapter da = new SqlDataAdapter(strSQL, conn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = Connection.USERID.Replace(".", "");
            da.SelectCommand.Parameters.Add("@MediaSubType", SqlDbType.VarChar).Value = cbxMediaSubType.Text;

            // ========== Modify by Atiwat.t [2015-07-16] ==========

            //da.SelectCommand.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = dtpFrom.Value;
            //da.SelectCommand.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = dtpTo.Value;

            string strStart = string.Empty;
            string strEnd = string.Empty;

            if (this.rdoDate.Checked == true)
            {
                strStart = this.dtpFrom.Value.ToString("yyyyMMdd");
                strEnd = this.dtpTo.Value.ToString("yyyyMMdd");
            }
            else if (this.rdoMonthYearRange.Checked == true)
            {
                strStart = this.cbbYearFrom.Text + this.cbbMonthFrom.Text + "01";
                strEnd = this.cbbYearTo.Text + this.cbbMonthTo.Text + DateTime.DaysInMonth(Convert.ToInt32(this.cbbYearTo.Text), Convert.ToInt32(this.cbbMonthTo.Text)).ToString("00");
            }
            else if (this.rdoMonthYear.Checked == true)
            {
                strStart = this.cbbYear.Text + this.cbbMonth.Text + "01";
                strEnd = this.cbbYear.Text + this.cbbMonth.Text + DateTime.DaysInMonth(Convert.ToInt32(this.cbbYear.Text), Convert.ToInt32(this.cbbMonth.Text)).ToString("00");
            }

            da.SelectCommand.Parameters.Add("@StartDate", SqlDbType.NVarChar).Value = strStart;
            da.SelectCommand.Parameters.Add("@EndDate", SqlDbType.NVarChar).Value = strEnd;

            // =====================================================

            da.SelectCommand.CommandTimeout = 0;
            DataSet ds = new DataSet();
            da.Fill(ds);
            gvProduct.AutoGenerateColumns = false;
            gvProduct.DataSource = ds.Tables[0];
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvProduct.SelectAll();
            }
        }

        private void RefreshClientList()
        {
            if (!bIsRefreshClient)
                return;

            if (this.rdoMonthYearRange.Checked == true)
            {
                if (string.IsNullOrEmpty(this.cbbMonthFrom.Text) ||
                    string.IsNullOrEmpty(this.cbbMonthTo.Text) ||
                    string.IsNullOrEmpty(this.cbbYearFrom.Text) ||
                    string.IsNullOrEmpty(this.cbbYearTo.Text))
                {
                    return;
                }
            }
            else if (this.rdoMonthYear.Checked == true)
            {
                if (string.IsNullOrEmpty(this.cbbMonth.Text) ||
                    string.IsNullOrEmpty(this.cbbYear.Text))
                {
                    return;
                }
            }

            SqlConnection conn = new SqlConnection(Connection.ConnectionStringMinder);
            string strSQL = "dbo.[MPA_get_Client]";
            SqlDataAdapter da = new SqlDataAdapter(strSQL, conn);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = Connection.USERID.Replace(".", "");
            da.SelectCommand.Parameters.Add("@MediaSubType", SqlDbType.VarChar).Value = cbxMediaSubType.Text;

            // ========== Modify by Atiwat.t [2015-07-16] ==========

            //da.SelectCommand.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = dtpFrom.Value;
            //da.SelectCommand.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = dtpTo.Value;

            string strStart = string.Empty;
            string strEnd = string.Empty;

            if (this.rdoDate.Checked == true)
            {
                strStart = this.dtpFrom.Value.ToString("yyyyMMdd");
                strEnd = this.dtpTo.Value.ToString("yyyyMMdd");
            }
            else if (this.rdoMonthYearRange.Checked == true)
            {
                strStart = this.cbbYearFrom.Text + this.cbbMonthFrom.Text + "01";
                strEnd = this.cbbYearTo.Text + this.cbbMonthTo.Text + DateTime.DaysInMonth(Convert.ToInt32(this.cbbYearTo.Text), Convert.ToInt32(this.cbbMonthTo.Text)).ToString("00");
            }
            else if (this.rdoMonthYear.Checked == true)
            {
                strStart = this.cbbYear.Text + this.cbbMonth.Text + "01";
                strEnd = this.cbbYear.Text + this.cbbMonth.Text + DateTime.DaysInMonth(Convert.ToInt32(this.cbbYear.Text), Convert.ToInt32(this.cbbMonth.Text)).ToString("00");
            }

            da.SelectCommand.Parameters.Add("@StartDate", SqlDbType.NVarChar).Value = strStart;
            da.SelectCommand.Parameters.Add("@EndDate", SqlDbType.NVarChar).Value = strEnd;

            // =====================================================


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

        private void InitialSearchCondition()
        {
            // Add month
            for (int i = 1; i <= 12; i++)
            {
                this.cbbMonthFrom.Items.Add(i.ToString("00"));
                this.cbbMonthTo.Items.Add(i.ToString("00"));
                this.cbbMonth.Items.Add(i.ToString("00"));
            }

            // Add year
            for (int i = 2001; i <= 2065; i++)
            {
                this.cbbYearFrom.Items.Add(i.ToString("00"));
                this.cbbYearTo.Items.Add(i.ToString("00"));
                this.cbbYear.Items.Add(i.ToString("00"));
            }

            if (m_highSpeed) this.rdoHighSpeed.Checked = true; else this.rdoRealTime.Checked = true;


            if (m_conditionOption == 1)
            {
                this.rdoDate.Checked = true;
            }
            else if (m_conditionOption == 2)
            {
                this.rdoMonthYearRange.Checked = true;
            }
            else if (m_conditionOption == 3)
            {
                this.rdoMonthYear.Checked = true;
            }
            //this.rdoDate.Checked
            //this.rdoMonthYearRange.Checked
            //this.rdoMonthYear.Checked

            //rdoDate
            //rdoMonthYearRange
            //rdoMonthYear


            //Add Date Latest by Puwarun.P
            this.rdoHighSpeed.Text = "High Speed(Updated:" + GetDateLatest() + ")";



            

        }

        private string GetDateLatest()
        {
            string LatestDate = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(Connection.ConnectionStringMPA))
                using (SqlCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandTimeout = 0;

                    string strCommand = @"select MAX(DateAdded) DateLatest from dbo.Data_MediaSpending";
                    command.CommandText = strCommand;
                    command.CommandType = CommandType.Text;

                    DataSet ds = new DataSet();
                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        da.Fill(ds);
                        LatestDate = ds.Tables[0].Rows[0].ItemArray[0].ToString();
                        return LatestDate;
                    }
                }
            }
            catch (Exception)
            {
                return "Data has empty";
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DateFrom = dtpFrom.Value;
            DateTo = dtpTo.Value;
            DataGridClient = gvClient;
            DataGridProduct = gvProduct;
            DataGridBuyingBrief = dgvBuyingBrief;
            MediaType = this.cbxMediaType.Text;
            MediaSubType = this.cbxMediaSubType.Text;

            // Add by atiwat.t [2015-07-16]
            MonthFrom = this.cbbMonthFrom.Text;
            MonthTo = this.cbbMonthTo.Text;
            YearFrom = this.cbbYearFrom.Text;
            YearTo = this.cbbYearTo.Text;
            Month = this.cbbMonth.Text;
            Year = this.cbbYear.Text;
            HighSpeed = this.rdoHighSpeed.Checked;

            if (this.rdoDate.Checked) ConditionOption = 1;
            else if (this.rdoMonthYearRange.Checked) ConditionOption = 2;
            else if (this.rdoMonthYear.Checked) ConditionOption = 3;

            this.DialogResult = DialogResult.OK;
        }

        private void MPA002_SearchCondition_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            dtpFrom.Value = DateFrom;
            dtpTo.Value = DateTo;

            // Add by puwarun.p [2016-04-27]
            this.InitialMediaType();
            this.InitialMediaSubType();



            //cboMediaSubType.SelectedIndex = cboMediaSubType.Items.IndexOf(MediaSubType);

            bIsRefreshClient = true;
            if (DataGridClient == null)
            {
                RefreshClientList();
            }
            else
            {
                gvClient.AutoGenerateColumns = false;
                gvClient.DataSource = DataGridClient.DataSource;
                gvClient.ClearSelection();
                for (int i = 0; i < DataGridClient.SelectedRows.Count; i++)
                {
                    for (int j = 0; j < gvClient.Rows.Count; j++)
                    {
                        if (gvClient.Rows[j].Cells[0].Value.ToString() == DataGridClient.SelectedRows[i].Cells[0].Value.ToString())
                        {
                            gvClient.Rows[j].Selected = true;
                            break;
                        }
                    }
                }
            }
            if (DataGridProduct == null)
            {
                RefreshProductList();
            }
            else
            {
                gvProduct.AutoGenerateColumns = false;
                gvProduct.DataSource = DataGridProduct.DataSource;
                gvProduct.ClearSelection();
                for (int i = 0; i < DataGridProduct.SelectedRows.Count; i++)
                {
                    for (int j = 0; j < gvProduct.Rows.Count; j++)
                    {
                        if (gvProduct.Rows[j].Cells[0].Value.ToString() == DataGridProduct.SelectedRows[i].Cells[0].Value.ToString())
                        {
                            gvProduct.Rows[j].Selected = true;
                            break;
                        }
                    }
                }
            }
            if (DataGridBuyingBrief == null)
            {
                RefreshBuyingBriefList();
            }
            else
            {

                RefreshBuyingBriefList();
                if (DataGridBuyingBrief.DataSource != null && ((DataTable)DataGridBuyingBrief.DataSource).Rows.Count != 0)
                {
                    txtBuyingBriefNo.Text = ((DataTable)DataGridBuyingBrief.DataSource).Rows[0].ItemArray[0].ToString();
                }
                    




                dgvBuyingBrief.AutoGenerateColumns = false;
                dgvBuyingBrief.DataSource = DataGridBuyingBrief.DataSource;
                dgvBuyingBrief.ClearSelection();
                for (int i = 0; i < DataGridBuyingBrief.SelectedRows.Count; i++)
                {
                    for (int j = 0; j < dgvBuyingBrief.Rows.Count; j++)
                    {
                        if (dgvBuyingBrief.Rows[j].Cells[0].Value.ToString() == DataGridBuyingBrief.SelectedRows[i].Cells[0].Value.ToString())
                        {
                            dgvBuyingBrief.Rows[j].Selected = true;
                            break;
                        }
                    }
                }
            }

            // Add by atiwat.t [2015-07-16]
            this.InitialSearchCondition();



            this.Cursor = Cursors.Default;
        }


        private void InitialMediaType()
        {
            //if (MediaType != "" && MediaType != null)
            //{
            //    this.cbxMediaType.SelectedText = "";
            //    this.cbxMediaType.SelectedText = MediaType;
            //    this.cbxMediaType.
            //}
            //else
            //{
                using (SqlConnection conn = new SqlConnection(Connection.ConnectionStringMinder))
                {
                    conn.Open();

                    using (SqlCommand com = new SqlCommand("SELECT * FROM Media_Type WHERE Media_Type in ('TV', 'TS')", conn))
                    {

                        DataTable dt = new DataTable();
                        SqlDataReader dr;
                        dr = com.ExecuteReader();
                        dt.Load(dr);

                        this.cbxMediaType.BindingData(dt, "Media_Type", "Media_Type", true);
                    }
                }
            //if (MediaType != "" && MediaType != null)
            //{
            //    this.cbxMediaType.SelectedText = "";
            //    this.cbxMediaType.SelectedText = MediaType;
            //    //this.cbxMediaType.DataBindings.Control
            //}
            // }

        }

        private void InitialMediaSubType()
        {
            //if (MediaSubType != "" && MediaSubType != null)
            //{
            //    this.cbxMediaSubType.SelectedText = "";
            //    this.cbxMediaSubType.SelectedText = MediaSubType;
            //}
            //else
            //{
                string strMediaType = cbxMediaType.GetNodeValue;
                using (SqlConnection conn = new SqlConnection(Connection.ConnectionStringMinder))
                {
                    conn.Open();

                    string strSql = @"SELECT * FROM Media_Sub_Type WHERE Media_Type in ('" + strMediaType.Replace(",", "','") + "')";

                    using (SqlCommand com = new SqlCommand(strSql, conn))
                    {

                        DataTable dt = new DataTable();
                        SqlDataReader dr;
                        dr = com.ExecuteReader();
                        dt.Load(dr);

                        this.cbxMediaSubType.BindingData(dt, "Media_Sub_Type", "Media_Sub_Type", true);
                    }
                }
            //if (MediaSubType != "" && MediaSubType != null)
            //{
            //    this.cbxMediaSubType.SelectedText = "";
            //    this.cbxMediaSubType.SelectedText = MediaSubType;
            //}

            //}



        }


        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            RefreshClientList();
            RefreshProductList();
            RefreshBuyingBriefList();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            RefreshClientList();
            RefreshProductList();
            RefreshBuyingBriefList();
        }

        private void cboMediaSubType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshClientList();
            RefreshProductList();
            RefreshBuyingBriefList();
        }

        private void llSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (this.mainTabControl.SelectedIndex == 0)
            {
                gvClient.SelectAll();
                if (gvProduct.Rows.Count > 0) { gvProduct.ClearSelection(); gvProduct.Rows[0].Selected = true; }
                if (dgvBuyingBrief.Rows.Count > 0) { dgvBuyingBrief.ClearSelection(); dgvBuyingBrief.Rows[0].Selected = true; }
            }
            else if (this.mainTabControl.SelectedIndex == 1)
            {
                gvProduct.SelectAll();
                if (gvClient.Rows.Count > 0) { gvClient.ClearSelection(); gvClient.Rows[0].Selected = true; }
                if (dgvBuyingBrief.Rows.Count > 0) { dgvBuyingBrief.ClearSelection(); dgvBuyingBrief.Rows[0].Selected = true; }
            }
            else if (this.mainTabControl.SelectedIndex == 2)
            {
                dgvBuyingBrief.SelectAll();
                if (gvClient.Rows.Count > 0) { gvClient.ClearSelection(); gvClient.Rows[0].Selected = true; }
                if (gvProduct.Rows.Count > 0) { gvProduct.ClearSelection(); gvProduct.Rows[0].Selected = true; }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DateFrom = dtpFrom.Value;
            DateTo = dtpTo.Value;
            DataGridClient = gvClient;
            DataGridProduct = gvProduct;
            DataGridBuyingBrief = dgvBuyingBrief;
            MediaType = this.cbxMediaType.Text;
            MediaSubType = this.cbxMediaSubType.Text;


            // Add by atiwat.t [2015-07-16]
            MonthFrom = this.cbbMonthFrom.Text;
            MonthTo = this.cbbMonthTo.Text;
            YearFrom = this.cbbYearFrom.Text;
            YearTo = this.cbbYearTo.Text;
            Month = this.cbbMonth.Text;
            Year = this.cbbYear.Text;
            HighSpeed = this.rdoHighSpeed.Checked;

            if (this.rdoDate.Checked) ConditionOption = 1;
            else if (this.rdoMonthYearRange.Checked) ConditionOption = 2;
            else if (this.rdoMonthYear.Checked) ConditionOption = 3;


            this.DialogResult = DialogResult.OK;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void GetBuyingBrief()
        {
            if (txtBuyingBriefNo.Text == "")
                return;
            string StartDate = dtpFrom.Value.ToShortDateString().Replace("/", "");
            string EndDate = dtpTo.Value.ToShortDateString().Replace("/", "");


            SqlConnection conn = new SqlConnection(Connection.ConnectionStringMinder);
            //            string strSQL = @"  select
            //	                                Buying_Brief_ID, 
            //	                                [Description]
            //	                                --,
            //	                                --Media_Sub_Type,
            //	                                --Client.Client_ID
            //                                from Buying_Brief
            //                                INNER JOIN dbo.[fn_User_CheckPermission](@UserName,NULL,NULL) Client
            //	                                ON Client.Client_ID = Buying_Brief.Client_ID
            //                                where Buying_Brief_ID like @BuyingBriefId + '%'
            //                                and Media_Sub_Type = @MediaSubType 
            //                                and Campaign_Start_Date BETWEEN @StartDate AND @EndDate
            //                                and	Campaign_End_Date BETWEEN @StartDate AND @EndDate 
            //                            ";
            string sqlGetBuyingBrief = @"  select
	                                                Buying_Brief_ID, 
	                                                [Description]
	                                                --,
	                                                --Media_Sub_Type,
	                                                --Client.Client_ID
                                                from Buying_Brief
                                                INNER JOIN dbo.[fn_User_CheckPermission](@UserName,NULL,NULL) Client
	                                                ON Client.Client_ID = Buying_Brief.Client_ID
                                                where Buying_Brief_ID like @BuyingBriefId + '%'
                                                and  CHARINDEX(Media_Sub_Type , @MediaSubType) > 0
                                                --and Media_Sub_Type = @MediaSubType 
                                                and Campaign_Start_Date BETWEEN @StartDate AND @EndDate
                                                and	Campaign_End_Date BETWEEN @StartDate AND @EndDate 
                                            ";
            SqlDataAdapter da = new SqlDataAdapter(sqlGetBuyingBrief, conn);
            da.SelectCommand.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = Connection.USERID.Replace(".", "");
            da.SelectCommand.Parameters.Add("@MediaSubType", SqlDbType.NVarChar).Value = ""; //cboMediaSubType.Text;
            da.SelectCommand.Parameters.Add("@StartDate", SqlDbType.NVarChar).Value = dtpFrom.Value.ToString("yyyyMMdd");
            da.SelectCommand.Parameters.Add("@EndDate", SqlDbType.NVarChar).Value = dtpTo.Value.ToString("yyyyMMdd");
            da.SelectCommand.Parameters.Add("@BuyingBriefId", SqlDbType.NVarChar).Value = txtBuyingBriefNo.Text;
            da.SelectCommand.CommandTimeout = 0;
            DataSet ds = new DataSet();
            da.Fill(ds);
            dgvBuyingBrief.AutoGenerateColumns = false;
            dgvBuyingBrief.DataSource = ds.Tables[0];
            dgvBuyingBrief.ReadOnly = true;
            if (ds.Tables[0].Rows.Count > 0)
            {
                dgvBuyingBrief.SelectAll();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DateFrom = dtpFrom.Value;
            DateTo = dtpTo.Value;
            DataGridClient = gvClient;
            DataGridProduct = gvProduct;
            DataGridBuyingBrief = dgvBuyingBrief;
            MediaType = this.cbxMediaType.Text;
            MediaSubType = this.cbxMediaSubType.Text;

            // Add by atiwat.t [2015-07-16]
            MonthFrom = this.cbbMonthFrom.Text;
            MonthTo = this.cbbMonthTo.Text;
            YearFrom = this.cbbYearFrom.Text;
            YearTo = this.cbbYearTo.Text;
            Month = this.cbbMonth.Text;
            Year = this.cbbYear.Text;
            HighSpeed = this.rdoHighSpeed.Checked;

            if (this.rdoDate.Checked) ConditionOption = 1;
            else if (this.rdoMonthYearRange.Checked) ConditionOption = 2;
            else if (this.rdoMonthYear.Checked) ConditionOption = 3;

            this.DialogResult = DialogResult.OK;
        }


        private void GetBuyingBriefByFilter()
        {
            DataTable _newDT = new DataTable();

            //Buying_Brief_ID, 
            //[Description]

            _newDT.Columns.Add("Buying_Brief_ID");
            _newDT.Columns.Add("Description");



            var strExpr = "Buying_Brief_ID like '%" + txtBuyingBriefNo.Text + "%'";
            var strSort = "Buying_Brief_ID";

            foreach (DataRow row in m_dataTemp.Select(strExpr, strSort))
            {
                _newDT.ImportRow(row);
            }

            //DataTemp = DataTemp.Select(strExpr, strSort);


            dgvBuyingBrief.AutoGenerateColumns = false;
            dgvBuyingBrief.DataSource = _newDT;
            dgvBuyingBrief.ReadOnly = true;
            if (DataTemp.Select(strExpr, strSort).Count() > 0)
            {
                dgvBuyingBrief.SelectAll();
            }
        }

        private void txtBuyingBriefNo_KeyUp(object sender, KeyEventArgs e)
        {
            //if(e.KeyCode == Keys.Enter)



            GetBuyingBriefByFilter();

            //foreach (DataRow row in DataTemp.Select(strExpr, strSort))
            //{
            //    DataTemp.ImportRow(row);
            //}



            //GetBuyingBrief();
        }

        private void mainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.mainTabControl.SelectedIndex == 0)
            {
                this.llSelectAll.Text = "Select All Client";
            }
            else if (this.mainTabControl.SelectedIndex == 1)
            {
                this.llSelectAll.Text = "Select All Product";
            }
            else if (this.mainTabControl.SelectedIndex == 2)
            {
                this.llSelectAll.Text = "Select All BB No.";





                // ========== Add by puwarun.p [2015-10-15] ===========


                //GetBuyingBriefByPeriod(null, string.Empty, string.Empty, string.Empty, string.Empty);


                // ==================================================== 


            }
        }


        private void RefreshFunction()
        {

            //if (this.mainTabControl.SelectedIndex == 0)
            //{
            //    this.RefreshClientList();
            //}
            //else if (this.mainTabControl.SelectedIndex == 1)
            //{
            //    this.RefreshProductList();
            //}
            //else if (this.mainTabControl.SelectedIndex == 2)
            //{
            //    this.RefreshBuyingBriefList();
            //}

            this.RefreshClientList();
            this.RefreshProductList();
            this.RefreshBuyingBriefList();
        }


        private void rdo_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var currentMonth = DateTime.Now.Month.ToString("00");
                var currentYear = DateTime.Now.Year.ToString("0000");
                if (rdoDate.Checked == true)
                {
                    this.pnlDate.Enabled = true;
                    this.pnlMonthYearRange.Enabled = false;
                    this.pnlMonthYear.Enabled = false;

                    this.cbbMonthFrom.Text = "";
                    this.cbbMonthTo.Text = "";
                    this.cbbMonth.Text = "";

                    this.cbbYearFrom.Text = "";
                    this.cbbYearTo.Text = "";
                    this.cbbYear.Text = "";



                    RefreshFunction();



                    //this.RefreshBuyingBriefList();
                    //this.RefreshProductList();
                    //this.RefreshClientList();

                }
                else if (rdoMonthYearRange.Checked == true)
                {
                    this.pnlDate.Enabled = false;
                    this.pnlMonthYearRange.Enabled = true;
                    this.pnlMonthYear.Enabled = false;

                    this.cbbMonthFrom.Text = "01";
                    this.cbbMonthTo.Text = currentMonth;
                    this.cbbMonth.Text = "";

                    this.cbbYearFrom.Text = currentYear;
                    this.cbbYearTo.Text = currentYear;
                    this.cbbYear.Text = "";



                    RefreshFunction();



                    //this.RefreshBuyingBriefList();
                    //this.RefreshProductList();
                    //this.RefreshClientList();
                }
                else if (rdoMonthYear.Checked == true)
                {
                    this.pnlDate.Enabled = false;
                    this.pnlMonthYearRange.Enabled = false;
                    this.pnlMonthYear.Enabled = true;

                    this.cbbMonthFrom.Text = "";
                    this.cbbMonthTo.Text = "";
                    this.cbbMonth.Text = currentMonth;

                    this.cbbYearFrom.Text = "";
                    this.cbbYearTo.Text = "";
                    this.cbbYear.Text = currentYear;



                    RefreshFunction();



                    //this.RefreshBuyingBriefList();
                    //this.RefreshProductList();
                    //this.RefreshClientList();
                }
            }
            catch (Exception ex)
            {
                GroupM.UTL.GMessage.MessageError(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void cbbMonthFrom_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //try







            //{
            //    Cursor.Current = Cursors.WaitCursor;
            //    this.RefreshBuyingBriefList();
            //    this.RefreshProductList();
            //    this.RefreshClientList();
            //}
            //catch (Exception ex)
            //{
            //    GroupM.UTL.GMessage.MessageError(ex);
            //}
            //finally
            //{
            //    Cursor.Current = Cursors.Default;
            //}
        }

        private void cbbMonthFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.RefreshBuyingBriefList();
                this.RefreshProductList();
                this.RefreshClientList();
            }
            catch (Exception ex)
            {
                GroupM.UTL.GMessage.MessageError(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }




        private void GetBuyingBriefByPeriod(int? userName, string buyingBriefId, string mediaSubType, string dateFrom, string dateTo)
        {
            if (txtBuyingBriefNo.Text == "")
                return;
            string StartDate = dtpFrom.Value.ToShortDateString().Replace("/", "");
            string EndDate = dtpTo.Value.ToShortDateString().Replace("/", "");

            SqlConnection conn = new SqlConnection(Connection.ConnectionStringMinder);
            string sqlGetBuyingBrief = @"  select
	                                                Buying_Brief_ID, 
	                                                [Description]
	                                                --,
	                                                --Media_Sub_Type,
	                                                --Client.Client_ID
                                                from Buying_Brief
                                                INNER JOIN dbo.[fn_User_CheckPermission](@UserName,NULL,NULL) Client
	                                                ON Client.Client_ID = Buying_Brief.Client_ID
                                                where Buying_Brief_ID like @BuyingBriefId + '%'
                                                and  CHARINDEX(Media_Sub_Type , @MediaSubType) > 0
                                                --and Media_Sub_Type = @MediaSubType 
                                                and Campaign_Start_Date BETWEEN @StartDate AND @EndDate
                                                and	Campaign_End_Date BETWEEN @StartDate AND @EndDate 
                                            ";
            SqlDataAdapter da = new SqlDataAdapter(sqlGetBuyingBrief, conn);
            da.SelectCommand.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = Connection.USERID.Replace(".", "");
            da.SelectCommand.Parameters.Add("@MediaSubType", SqlDbType.NVarChar).Value = ""; //cboMediaSubType.Text;
            da.SelectCommand.Parameters.Add("@StartDate", SqlDbType.NVarChar).Value = dtpFrom.Value.ToString("yyyyMMdd");
            da.SelectCommand.Parameters.Add("@EndDate", SqlDbType.NVarChar).Value = dtpTo.Value.ToString("yyyyMMdd");
            da.SelectCommand.Parameters.Add("@BuyingBriefId", SqlDbType.NVarChar).Value = txtBuyingBriefNo.Text;
            da.SelectCommand.CommandTimeout = 0;
            DataSet ds = new DataSet();
            da.Fill(ds);
            dgvBuyingBrief.AutoGenerateColumns = false;
            dgvBuyingBrief.DataSource = ds.Tables[0];
            dgvBuyingBrief.ReadOnly = true;
            if (ds.Tables[0].Rows.Count > 0)
            {
                dgvBuyingBrief.SelectAll();
            }
        }

        private void rdoHighSpeed_CheckedChanged(object sender, EventArgs e)
        {
            this.RefreshClientList();
            this.RefreshProductList();
            this.RefreshBuyingBriefList();
        }

        private void rdoRealTime_CheckedChanged(object sender, EventArgs e)
        {
            this.RefreshClientList();
            this.RefreshProductList();
            this.RefreshBuyingBriefList();
        }

        private void cbbMonth_KeyUp(object sender, KeyEventArgs e)
        {
            if (Convert.ToInt32(cbbMonth.Text) > 0 && Convert.ToInt32(cbbMonth.Text) < 13)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    this.RefreshBuyingBriefList();
                    this.RefreshProductList();
                    this.RefreshClientList();
                }
                catch (Exception ex)
                {
                    GroupM.UTL.GMessage.MessageError(ex);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void cbbYear_KeyUp(object sender, KeyEventArgs e)
        {
            if (cbbYear.Text.Length == 4)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    this.RefreshBuyingBriefList();
                    this.RefreshProductList();
                    this.RefreshClientList();
                }
                catch (Exception ex)
                {
                    GroupM.UTL.GMessage.MessageError(ex);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        private void cbxMediaType_Validated(object sender, EventArgs e)
        {
            InitialMediaSubType();
        }

        private void cbxMediaSubType_e_Deactivated()
        {
            RefreshFunction();
        }

        private void cbxMediaType_e_Deactivated()
        {
            InitialMediaSubType();
            RefreshFunction();
        }
    }
}