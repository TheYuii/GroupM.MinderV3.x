using GroupM.DBAccess;
using GroupM.UTL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace GroupM.Minder
{
    public partial class Implementation_SpotPlan_OD : Form
    {

        DBManager m_db = null;
        private string username;
        private string screenmode;
        private string m_MasterMediaType = "";

        private decimal m_dAgencyFee = 0;
        private string m_strBuyingBriefID;
        private string m_strVersion;
        private string m_strMediaType;
        private int m_iStatus;
        private ArrayList m_alCalendar;

        string m_strStartDate = null;
        string m_strEndDate = null;
        string m_strEditDate = null;

        string m_Edit_Date = "";
        string m_Edit_Time = "";

        string m_strScheduleExportPath = ConfigurationManager.AppSettings["ReportMinder"] + "Cue_OD.xlt";
        string m_strScheduleDirectPayExportPath = ConfigurationManager.AppSettings["ReportMinder"] + "Cue_IT_Pay2Vendor.xlt";
        string m_strScheduleLumpSumExportPath = ConfigurationManager.AppSettings["ReportMinder"] + "Cue_IT_LumpSum.xlt";
        string m_strPOExportPath = ConfigurationManager.AppSettings["ReportMinder"] + "Cue_OD_Booking_Order.xlt";
        string m_strAdviceNoteExportPath = ConfigurationManager.AppSettings["ReportMinder"] + "Var_OD.xlt";
        string m_strAdviceNoteDirectPayExportPath = ConfigurationManager.AppSettings["ReportMinder"] + "Var_IT_Pay2Vendor.xlt";

        int m_iMonthly = 0;

        enum eScreenMode { Add, Edit, View }
        eScreenMode m_screenMode = eScreenMode.Edit;
        enum eCol { MediaTpe, MediaSubTpe, Media, Vendor, Desc, StartDate, EndDate, Material, Impression, GRP, CPRP, TotalCost, Disc, NetCost, AgencyFeePercent, AgencyFee }

        public Implementation_SpotPlan_OD(string Username, string ScreenPermission, string BB, string v, int s, string mt)
        {
            InitializeComponent();
            username = Username;
            if (ScreenPermission == "Add")
                m_screenMode = eScreenMode.Add;
            else if (ScreenPermission == "Edit")
                m_screenMode = eScreenMode.Edit;
            else
                m_screenMode = eScreenMode.View;

            m_strBuyingBriefID = BB;
            m_strVersion = v;
            m_iStatus = s;
            m_strMediaType = mt;
            m_MasterMediaType = mt;

            m_db = new DBManager();
            username = Username;
            screenmode = ScreenPermission;
        }
        public Implementation_SpotPlan_OD(string Username, string ScreenPermission, string BB, string v, int s, string mt, string mmt)
        {
            m_MasterMediaType = mmt;
            InitializeComponent();
            username = Username;
            if (ScreenPermission == "Add")
                m_screenMode = eScreenMode.Add;
            else if (ScreenPermission == "Edit")
                m_screenMode = eScreenMode.Edit;
            else
                m_screenMode = eScreenMode.View;

            m_strBuyingBriefID = BB;
            m_strVersion = v;
            m_iStatus = s;
            m_strMediaType = mt;

            m_db = new DBManager();
            username = Username;
            screenmode = ScreenPermission;
        }
        bool m_bCreateAdviceNote = false;
        private void SetStatus(int iStatus)
        {

            if (m_iStatus == 0)
            {
                txtHearder.Text = $" Version {m_strVersion} - Draft";
                purchaseOrderToolStripMenuItem.Enabled = false;
                adviceNoteToolStripMenuItem.Enabled = false;
                m_bCreateAdviceNote = false;
                gpAdviceNote.Visible = false;
                splitContainer2.Panel2Collapsed = true;
            }
            else if (m_iStatus == 4)
            {
                txtHearder.Text = $" Version {m_strVersion} - Approved";
                adviceNoteToolStripMenuItem.Enabled = false;
                purchaseOrderToolStripMenuItem.Enabled = false;
                m_bCreateAdviceNote = false;
                gpAdviceNote.Visible = false;
                splitContainer2.Panel2Collapsed = true;
            }
            else if (m_iStatus == 5)
            {
                txtHearder.Text = $" Version {m_strVersion} - Executing";
                adviceNoteToolStripMenuItem.Enabled = true;
                m_bCreateAdviceNote = true;
            }
            else if (m_iStatus == 8)
            {
                txtHearder.Text = $" Version {m_strVersion} - Actual";
                adviceNoteToolStripMenuItem.Enabled = true;
                m_bCreateAdviceNote = false;
            }
            m_iStatus = iStatus;
        }
        private void SetScreenMode(eScreenMode mode)
        {
            switch (mode)
            {
                case eScreenMode.View:
                    btnAddDetail.Visible = false;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    txtPORemark.ReadOnly = true;
                    txtScheduleRemark.ReadOnly = true;
                    gvSpotPlanEdit.Enabled = false;
                    break;
            }
            m_screenMode = mode;
        }
        private void InitailControl()
        {
            ControlManipulate.ClearControl(txtHearder);
            m_Edit_Date = DateTime.Now.ToString("yyyyMMdd");
            m_Edit_Time = DateTime.Now.ToString("HHmmss");
            m_strEditDate = DateTime.Now.ToString("dd/MM/yyyy");

            this.Text = $"Spot Plan - {Environment.CurrentDirectory}, {DAContants.ConnectionStringMinder.Split(';')[1]}";

            SetScreenMode(m_screenMode);
            SetStatus(m_iStatus);
        }

        private void DataLoading()
        {

            DataTable dtBB = m_db.SelectBuyingBrief(m_strBuyingBriefID);
            DataRow dr = dtBB.Rows[0];
            lbOptIn.Text = dr["Opt_In"].ToString();
            m_dAgencyFee = Convert.ToDecimal(dr["Agency_Commission"]);
            txtClientCode.Text = dr["Client_ID"].ToString();
            txtClientName.Text = dr["ClientName"].ToString();
            txtProductCode.Text = dr["Product_ID"].ToString();
            txtProductName.Text = dr["ProductName"].ToString();
            txtCampaignName.Text = dr["Description"].ToString();
            txtBuyingBriefNo.Text = dr["Buying_Brief_ID"].ToString();
            txtPeriod.Text = dr["Campaign_Start_Date"].ToString() + "-" + dr["Campaign_End_Date"].ToString();
            txtPlanBudget.Text = Convert.ToDouble(dr["Total_Budget"]).ToString("#,##0.00");
            m_strStartDate = dr["Campaign_Start_Date"].ToString();
            m_strEndDate = dr["Campaign_End_Date"].ToString();

            txtScheduleRemark.Text = dr["CommentClient"].ToString(); ;//join buying brife martket
            txtPORemark.Text = dr["CommentVendor"].ToString();//join buying brife martket

            DataTable dtMaterial = m_db.SelectMaterial(m_strBuyingBriefID);
            gvMaterial.AutoGenerateColumns = false;
            gvMaterial.DataSource = dtMaterial;

            DateTime CampaignStartDate = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strStartDate);
            DateTime CampaignEndDate = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strEndDate);

            DataTable dt = m_db.SelectSpotPlanOD(m_strBuyingBriefID, m_strVersion, CampaignStartDate, CampaignEndDate);

            gvDetail.AutoGenerateColumns = false;
            gvDetail.DataSource = dt;


            gvCalendar.AutoGenerateColumns = false;
            gvCalendar.DataSource = dt;

            DataTable dtSpotPlanEdit = m_db.SelectSpotPlanEdit(m_strBuyingBriefID, m_strVersion);
            gvSpotPlanEdit.AutoGenerateColumns = false;
            gvSpotPlanEdit.DataSource = dtSpotPlanEdit;
            //gvSpotPlanEdit.Sort(this.gvSpotPlanEdit.Columns[0], ListSortDirection.Ascending);

            RefreshBudget();
        }

        private void RefreshBudget()
        {
            double dActualBudget = 0;
            try
            {
                DataTable dt = (DataTable)gvDetail.DataSource;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                        dActualBudget += Convert.ToDouble(dr["Net_Cost"]);
                }

                txtActualBudget.Text = dActualBudget.ToString("#,##0.00");
                txtVarianceBudget.Text = (Convert.ToDouble(txtPlanBudget.Text) - dActualBudget).ToString("#,##0.00");


                for (int i = 0; i < gvCalendar.Rows.Count; i++)
                {
                    double totalCurrentRow = 0;
                    DataRow dr = ((DataRowView)gvCalendar.Rows[i].DataBoundItem).Row;
                    foreach (string colName in m_alCalendar)
                    {
                        double currentValue = 0;
                        if (dr[colName] != null && dr[colName] != DBNull.Value && dr[colName].ToString() != "")
                            currentValue = Convert.ToDouble(dr[colName]);
                        totalCurrentRow = totalCurrentRow + currentValue;
                    }
                    gvCalendar.Rows[i].HeaderCell.Value = totalCurrentRow.ToString("#,##0.00");
                }
            }
            catch (Exception e)
            {
                GMessage.MessageError(e);
            }
        }

        private void SpotPlan_Load(object sender, EventArgs e)
        {

            //this.gvSpotPlanEdit.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.gvSpotPlanEdit_RowPrePaint);

            InitailControl();
            DataLoading();
            //=====================
            // Create Carlendar
            //=====================
            m_alCalendar = new ArrayList();

            DateTime CampaignStartDate = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strStartDate);
            DateTime CampaignEndDate = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strEndDate);
            m_iMonthly = 0;
            for (DateTime startDate = CampaignStartDate;
            startDate <= CampaignEndDate;
            startDate = new DateTime(startDate.AddMonths(1).Year, startDate.AddMonths(1).Month, 1))
            {

                string colName = "Col" + startDate.ToString("yyyyMM");
                string colHeaderName = startDate.ToString("yyyy-MM");
                string dataProperties = startDate.ToString("yyyyMM");
                m_alCalendar.Add(dataProperties);
                gvCalendar.Columns.Add(colName, colHeaderName);
                gvCalendar.Columns[m_iMonthly].DataPropertyName = dataProperties;
                gvCalendar.Columns[m_iMonthly].Width = 80;
                gvCalendar.Columns[m_iMonthly].DefaultCellStyle = new DataGridViewCellStyle() { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight };
                m_iMonthly++;
            }
            this.gvCalendar.RowHeadersDefaultCellStyle.Padding = new Padding(this.gvCalendar.RowHeadersWidth);
            gvCalendar.RowPostPaint += new DataGridViewRowPostPaintEventHandler(gvCalendar_RowPostPaint);
            gvCalendar.TopLeftHeaderCell.Value = "Total";
            RefreshBudget();
            /*if (m_screenMode == eScreenMode.Edit)
            {
                m_db.InsertSpotPlanSavingStatus(txtBuyingBriefNo.Text, m_strVersion, username, DateTime.Now.ToString("yyyyMMddhhmmss"), "Spot_Plan");
            }*/

        }
        void gvCalendar_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            object o = gvCalendar.Rows[e.RowIndex].HeaderCell.Value;

            e.Graphics.DrawString(
                o != null ? o.ToString() : "",
                gvCalendar.Font,
                Brushes.Black,
                new PointF((float)e.RowBounds.Left + 2, (float)e.RowBounds.Top + 4));
        }
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (CheckDataBeforeSave())
                {
                    if (GMessage.MessageComfirm("Do you want to save?") != DialogResult.Yes)
                        return;
                    if (OnCommandSave())
                    {
                        GMessage.MessageInfo("Save Completed.");
                        DataLoading();
                    }
                    else
                    {
                        GMessage.MessageError("Can't Save, Some thing wrong.");
                    }
                }
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
        private bool OnCommandSave()
        {
            if (!m_db.UpdateSpotPlanRemark(m_strBuyingBriefID, txtScheduleRemark.Text, txtPORemark.Text, ""))
                return false;

            DataTable dt = (DataTable)gvDetail.DataSource;
            //if (m_db.DeleteSpotPlan(m_strBuyingBriefID, m_strVersion))
            //{
            //    int iItem = 0;
            //    if (m_screenMode != eScreenMode.Add)
            //        iItem = Convert.ToInt32(dt.AsEnumerable().Max(row => row["Item"]));
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        if (dr.RowState != DataRowState.Deleted)
            //        {
            //            dr["Item"] = ++iItem;
            //            m_db.InsertSpotPlanDetail(dr);

            //            foreach(string strYearMonth in m_alCalendar)   
            //            { 
            //                if (dr[strYearMonth] != null
            //                     && dr[strYearMonth] != DBNull.Value
            //                     && dr[strYearMonth].ToString() != "")
            //                {
            //                    dr["Show_Date"] = strYearMonth+"01";
            //                    double dNetCost = Convert.ToDouble(dr[strYearMonth]);
            //                    double dAgencyFeeCost = Convert.ToDouble(dr[strYearMonth]) * Convert.ToDouble(dr["Agency_Fee"]);
            //                    double dPaymentTerm = dNetCost + dAgencyFeeCost;
            //                    m_db.InsertSpotPlanPaymentDetail(dr,dNetCost,dAgencyFeeCost,dPaymentTerm);
            //                }
            //            }

            //        }
            //    }
            //}
            //else
            //    return false;

            foreach (DataRow dr in dt.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    foreach (string strYearMonth in m_alCalendar)
                    {
                        m_db.DeleteSpotPlanPaymentDetail(Convert.ToInt32(dr[strYearMonth + "_IDKey", DataRowVersion.Original]));
                    }
                    m_db.DeleteSpotPlanDetail(dr);
                }
            }
            foreach (DataRow dr in dt.Rows)
            {
                if (dr.RowState == DataRowState.Added)
                {
                    m_db.InsertSpotPlanDetail(dr);
                    foreach (string strYearMonth in m_alCalendar)
                    {
                        if (dr[strYearMonth] != null
                             && dr[strYearMonth] != DBNull.Value
                             && dr[strYearMonth].ToString() != "")
                        {
                            dr["Show_Date"] = strYearMonth + "01";
                            double dNetCost = Convert.ToDouble(dr[strYearMonth]);
                            double dAgencyFeeCost = Convert.ToDouble(dr[strYearMonth+"_Agency_Fee_Cost"]);
                            double dPaymentTerm = dNetCost + dAgencyFeeCost;
                            int iIDKey = m_db.InsertSpotPlanPaymentDetail(dr, dNetCost, dAgencyFeeCost, dPaymentTerm,0);

                            //update IDKey to Spot Plan ID when Add.
                            DataTable dtSpotplanEdit2 = (DataTable)gvSpotPlanEdit.DataSource;
                            for (int i = 0; i < dtSpotplanEdit2.Rows.Count; i++)
                            {
                                DataRow dr2 = dtSpotplanEdit2.Rows[i];
                                if (dr2.RowState == DataRowState.Added
                                    && dr2["Kind"].ToString() == "Add"
                                    && dr2["Spot_Plan_Id"].ToString() == "0"
                                    && dr2["Show_Date"].ToString() == dr["Show_Date"].ToString()
                                    && dr2["Item"].ToString() == dr["item"].ToString())
                                {
                                    dr2["Spot_Plan_Id"] = iIDKey;
                                    break;
                                }
                            }
                        }
                    }
                }
                else if (dr.RowState == DataRowState.Modified)
                {
                    m_db.UpdateSpotPlanDetail(dr);
                    foreach (string strYearMonth in m_alCalendar)
                    {
                        double currentValue = 0;
                        if (dr[strYearMonth] != null && dr[strYearMonth] != DBNull.Value && dr[strYearMonth].ToString() != "")
                            currentValue = Convert.ToDouble(dr[strYearMonth]);

                        dr["Show_Date"] = strYearMonth + "01";
                        double dNetCost = 0;
                        double dAgencyFeeCost = 0;
                        double dPaymentTerm = 0;

                        if (dr[strYearMonth + "_IDKey"] != null && dr[strYearMonth + "_IDKey"] != DBNull.Value && Convert.ToInt32(dr[strYearMonth + "_IDKey"]) != 0)
                        {//delete or update
                            //if (currentValue == 0)
                            //    m_db.DeleteSpotPlanPaymentDetail(Convert.ToInt32(dr[strYearMonth + "_IDKey"]));
                            //else //if (originalValue != currentValue)
                            //{
                            //    dNetCost = Convert.ToDouble(dr[strYearMonth]);
                            //    dAgencyFeeCost = Convert.ToDouble(dr[strYearMonth + "_Agency_Fee_Cost"]);
                            //    dPaymentTerm = dNetCost + dAgencyFeeCost;
                            //    m_db.UpdateSpotPlanPaymentDetail(Convert.ToInt32(dr[strYearMonth + "_IDKey"]), dNetCost, dAgencyFeeCost, dPaymentTerm);
                            //}
                            dNetCost = currentValue;
                            if (currentValue == 0)
                                dAgencyFeeCost = 0;
                            else
                                dAgencyFeeCost = Convert.ToDouble(dr[strYearMonth + "_Agency_Fee_Cost"]);
                            dPaymentTerm = dNetCost + dAgencyFeeCost;
                            m_db.UpdateSpotPlanPaymentDetail(Convert.ToInt32(dr[strYearMonth + "_IDKey"]), dNetCost, dAgencyFeeCost, dPaymentTerm);
                        }
                        else if (currentValue != 0)
                        {
                            dNetCost = Convert.ToDouble(dr[strYearMonth]);
                            dAgencyFeeCost = Convert.ToDouble(dr[strYearMonth + "_Agency_Fee_Cost"]);
                            dPaymentTerm = dNetCost + dAgencyFeeCost;
                            int iIDKey = m_db.InsertSpotPlanPaymentDetail(dr, dNetCost, dAgencyFeeCost, dPaymentTerm, dNetCost);
                            //update IDKey to Spot Plan ID when Add.
                            DataTable dtSpotplanEdit2 = (DataTable)gvSpotPlanEdit.DataSource;
                            for (int i = 0; i < dtSpotplanEdit2.Rows.Count; i++)
                            {
                                DataRow dr2 = dtSpotplanEdit2.Rows[i];
                                if (dr2.RowState == DataRowState.Added
                                    && dr2["Kind"].ToString() == "Add"
                                    && dr2["Spot_Plan_Id"].ToString() == "0"
                                    && dr2["Show_Date"].ToString() == dr["Show_Date"].ToString()
                                    && dr2["Item"].ToString() == dr["item"].ToString())
                                {
                                    dr2["Spot_Plan_Id"] = iIDKey;
                                    break;
                                }
                            }
                        }


                    }
                }
            }


            if (m_db.DeleteSpotPlanVersion(m_strBuyingBriefID, m_strVersion))
            {
                m_db.InsertSpotPlanVersion(m_strBuyingBriefID, m_strVersion, m_iStatus, dt.Rows.Count, username, Convert.ToDouble(txtActualBudget.Text));
            }
            else
                return false;

            if (!m_db.UpdateBuyingBrief(m_strBuyingBriefID, m_iStatus, Convert.ToDouble(txtActualBudget.Text)))
                return false;

            if (!m_db.UpdateBuyingBriefMarket(m_strBuyingBriefID, m_iStatus, Convert.ToDouble(txtActualBudget.Text), dt.Rows.Count))
                return false;

            DataTable dtSpotplanEdit = (DataTable)gvSpotPlanEdit.DataSource;
            foreach (DataRow dr in lsSpotPlanEditDelete)
            {
                m_db.DeleteSpotPlanEditOD(dr);
            }
            for (int i = 0; i < dtSpotplanEdit.Rows.Count; i++)
            {
                DataRow dr = dtSpotplanEdit.Rows[i];
                if (dr.RowState == DataRowState.Added)
                    m_db.InsertSpotPlanEdit(dr);
                else if (dr.RowState == DataRowState.Modified)
                    m_db.UpdateSpotPlanEdit(dr);
            }

            if (!m_db.UpdateMediaSubTypeAndAgencyFee(m_strBuyingBriefID))
                return false;

            return true;
        }

        private bool CheckDataBeforeSave()
        {

            for (int i = 0; i < gvCalendar.Rows.Count; i++)
            {
                DataRow dr = ((DataRowView)gvCalendar.Rows[i].DataBoundItem).Row;
                double totalCurrentRow = Convert.ToDouble(gvCalendar.Rows[i].HeaderCell.Value);
                double dNetCost = Convert.ToDouble(dr["Net_Cost"]);
                if (totalCurrentRow != dNetCost)
                {
                    gvCalendar.Rows[i].Selected = true;
                    GMessage.MessageWarning("Net Cost not equal total");
                    return false;
                }
            }
            bool valid = true;
            bool check = false;
            DataTable dtversion = m_db.SelectSpotPlanVersion(m_strBuyingBriefID, m_strVersion);
            if (dtversion.Rows.Count == 0)
            {
                check = true;
            }
            else
            {
                int status = Convert.ToInt32(dtversion.Rows[0]["Approve"].ToString());
                if (status == m_iStatus)
                {
                    check = true;
                }
                else
                {
                    GMessage.MessageWarning($"This buying brief : {m_strBuyingBriefID} has change status by another user.");
                    SetStatus(status);
                    check = false;
                    valid = false;
                }
            }
            if (check)
            {
                DataTable dtSP = (DataTable)gvDetail.DataSource;
                for (int i = 0; i < dtSP.Rows.Count; i++)
                {
                    if (valid)
                    {
                        if (dtSP.Rows[i].RowState != DataRowState.Deleted)
                        {
                            string vendor = dtSP.Rows[i]["Media_Vendor_ID"].ToString();
                            string media = dtSP.Rows[i]["Media_ID"].ToString();
                            if (m_db.CheckOptIn(vendor, media).Rows.Count > 0)
                            {
                                DataTable dt = m_db.CheckOptIn(vendor, media, txtClientCode.Text);
                                if (dt.Rows.Count == 0)
                                {
                                    GMessage.MessageWarning($"This client can not buy media({media}) with vendor({vendor}). Please contact commercial team to check set up.");
                                    valid = false;
                                }
                                else
                                {
                                    DataRow dr = dt.Rows[0];
                                    DateTime dtOptInStartDate = Convert.ToDateTime(dr["Opt_in_StartDate"]);
                                    DateTime dtOptInEndDate = Convert.ToDateTime(dr["Opt_in_EndDate"]);
                                    DateTime dtStartDate = Convert.ToDateTime(dtSP.Rows[i]["StartDate"].ToString());
                                    DateTime dtEndDate = Convert.ToDateTime(dtSP.Rows[i]["EndDate"].ToString());
                                    if (!(dtOptInStartDate <= dtStartDate && dtEndDate <= dtOptInEndDate))
                                    {
                                        GMessage.MessageWarning($"This client can not buy media({media}) with vendor({vendor}). The client have been set up period Opt-in between {dtOptInStartDate.ToString("dd/MM/yyyy")} and {dtOptInEndDate.ToString("dd/MM/yyyy")}. Please contact commercial team to check set up.");
                                        valid = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return valid;
        }

        private int GetNextItem()
        {
            int iMax = 0;
            foreach (DataGridViewRow drv in gvDetail.Rows)
            {
                DataRow dr = ((DataRowView)drv.DataBoundItem).Row;
                if (Convert.ToInt32(dr["Item"]) > iMax)
                    iMax = Convert.ToInt32(dr["Item"]);
            }
            foreach (DataGridViewRow drv in gvSpotPlanEdit.Rows)
            {
                DataRow dr = ((DataRowView)drv.DataBoundItem).Row;
                if (Convert.ToInt32(dr["Item"]) > iMax)
                    iMax = Convert.ToInt32(dr["Item"]);
            }
            iMax++;
            return iMax;
        }
        private void SpotPlanEditClearMonth(DateTime startDate,DateTime conditionDate,DataRow dr)
        {
            for (;
                          startDate <= conditionDate;// DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strEndDate);
                          startDate = new DateTime(startDate.AddMonths(1).Year, startDate.AddMonths(1).Month, 1))
            {
                string colName = startDate.ToString("yyyyMM");
                dr[colName] = DBNull.Value;
                dr[colName + "_Agency_Fee_Cost"] = DBNull.Value;
                if (m_bCreateAdviceNote)
                {
                    double originalValue = 0;
                    if (dr[colName + "_Original"] != null && dr[colName + "_Original"] != DBNull.Value && dr[colName + "_Original"].ToString() != "")
                        originalValue = Convert.ToDouble(dr[colName + "_Original"]);
                    InsertSpotPlanEdit(0, originalValue, colName, dr, true);
                }
            }
        }
        private void GvDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            string screenMode = "";
            if (m_screenMode == eScreenMode.Add)
                screenMode = "Add";
            if (m_screenMode == eScreenMode.Edit)
                screenMode = "Edit";
            if (m_screenMode == eScreenMode.View)
                screenMode = "View";
            DataRow dr = ((DataRowView)gvDetail.Rows[e.RowIndex].DataBoundItem).Row;
            Implementation_SpotPlan_OD_Amendment frm;
            if (m_MasterMediaType == "")
                frm = new Implementation_SpotPlan_OD_Amendment(screenMode, m_strMediaType);
            else
                frm = new Implementation_SpotPlan_OD_Amendment(screenMode, m_strMediaType, m_MasterMediaType);

            frm.Client_ID = txtClientCode.Text;
            frm.Product_ID = txtProductCode.Text;
            frm.CampaignStartDate = m_strStartDate;
            frm.CampaignEndDate = m_strEndDate;
            frm.AdeptMediaType = dr["AdeptMediaTypeName"].ToString();
            frm.drInput = dr;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                DataRow drCloneOriginal = ((DataTable)gvDetail.DataSource).NewRow();
                drCloneOriginal.ItemArray = dr.ItemArray;

                //Update
                //int item = GetNextItem();
                //dr["Item"] = item;
                //dr["Adept_Code"] = item;

                dr["AdeptMediaTypeName"] = frm.AdeptMediaType;
                dr["MediaTypeName"] = frm.txtMediaType.Text;
                dr["Media_Type"] = frm.txtMediaTypeCode.Text;
                dr["MediaSubTypeName"] = frm.txtMediaSubType.Text;
                dr["Media_Sub_Type"] = frm.txtMediaSubTypeCode.Text;
                dr["MediaName"] = frm.txtMedia.Text;
                dr["Media_ID"] = frm.txtMediaCode.Text;
                dr["MediaVendorName"] = frm.txtVendor.Text;
                dr["Media_Vendor_ID"] = frm.txtVendorCode.Text;
                dr["Program"] = frm.txtDescription.Text.Replace(",", ";").Replace("\"", " ").Replace("'", "`");

                dr["BuyTypeID"] = frm.txtBuyTypeCode.Text;
                dr["BuyTypeName"] = frm.m_strBuyTypeName;
                dr["Deadline_Terminate"] = frm.txtBuyType.Text;

                dr["Material_ID"] = frm.m_strMaterialID;
                dr["SizeHW"] = frm.txtSize.Text;
                dr["Unit"] = frm.txtUnit.Text;
                dr["State"] = frm.cboCostType.Text;

                dr["Remark"] = frm.txtRemark.Text;
                dr["Invoice_Number"] = frm.txtQuotation.Text;

                dr["Show_Date"] = frm.dtStartDate.Value.ToString("yyyyMMdd");
                dr["Start_Date"] = frm.dtStartDate.Value.ToString("yyyyMMdd");
                dr["End_Date"] = frm.dtEndDate.Value.ToString("yyyyMMdd");
                dr["StartDate"] = frm.dtStartDate.Value.ToString("dd/MM/yyyy");
                dr["EndDate"] = frm.dtEndDate.Value.ToString("dd/MM/yyyy");
                dr["Start_Time"] = "0600";
                dr["End_Time"] = "2400";
                dr["DifDays"] = Convert.ToInt32(frm.txtTotalDays.Text.ToString());
                dr["Original_Rating"] = Convert.ToInt32(frm.txtTotalDays.Text.ToString());
                //dr["Market_Price"] = frm.txtImpression.Value;
                dr["Rate"] = frm.txtNetCost.Value;
                dr["Discount"] = 0;//frm.txtDiscount.Value;

                double dNetCost = Convert.ToDouble(frm.txtNetCost.Value);
                dr["Net_Cost"] = dNetCost;//frm.txtNetCost.Value;

                double dTotalPrice = Convert.ToDouble(frm.txtTotalPrice.Value);
                dr["CPRP_Cost"] = dTotalPrice;

                dr["Agency_Fee"] = frm.txtAgencyFeePercent.Value / 100;
                dr["AgencyFeePercent"] = frm.txtAgencyFeePercent.Value;
                double dAgencyFee = Convert.ToDouble(Math.Round(frm.txtNetCost.Value * (frm.txtAgencyFeePercent.Value / 100), 2));
                dr["AgencyFee"] = dAgencyFee;//frm.txtNetCost.Value * (frm.txtAgencyFeePercent.Value / 100);

                if (frm.rdNone.Checked)
                {
                    dr["CPM"] = 1;
                    string colName = "yyyyMM";
                    //Clear Start Other Month
                    SpotPlanEditClearMonth(DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strStartDate), frm.dtStartDate.Value, dr);
                    //DateTime startDate2 = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strStartDate);
                    //for (;
                    //   startDate2 < frm.dtStartDate.Value;
                    //   startDate2 = new DateTime(startDate2.AddMonths(1).Year, startDate2.AddMonths(1).Month, 1))
                    //{
                    //    colName = startDate2.ToString("yyyyMM");
                    //    dr[colName] = DBNull.Value;
                    //    dr[colName + "_Agency_Fee_Cost"] = DBNull.Value;
                    //    //dr[colName + "_Original"] = DBNull.Value;
                    //    if (m_bCreateAdviceNote)
                    //    {
                    //        double originalValue = 0;
                    //        if (dr[colName + "_Original"] != null && dr[colName + "_Original"] != DBNull.Value && dr[colName + "_Original"].ToString() != "")
                    //            originalValue = Convert.ToDouble(dr[colName + "_Original"]);
                    //        InsertSpotPlanEdit(0, originalValue, colName, dr, true);
                    //    }
                    //}


                    colName = frm.dtStartDate.Value.ToString("yyyyMM");
                    dr[colName] = dNetCost;
                    dr[colName + "_Agency_Fee_Cost"] = dAgencyFee;
                    //dr[frm.dtStartDate.Value.ToString("yyyyMM") + "_Original"] = dNetCost;
                    DateTime startDate = frm.dtStartDate.Value;

                    if (m_bCreateAdviceNote)
                    {
                        double originalValue = 0;
                        if (dr[colName + "_Original"] != null && dr[colName + "_Original"] != DBNull.Value && dr[colName + "_Original"].ToString() != "")
                            originalValue = Convert.ToDouble(dr[colName + "_Original"]);
                        InsertSpotPlanEdit(dNetCost, originalValue, colName, dr, false);
                    }

                    startDate = new DateTime(startDate.AddMonths(1).Year, startDate.AddMonths(1).Month, 1);
                    //Clear Other Month
                    SpotPlanEditClearMonth(startDate, DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strEndDate), dr);
                    //for (;
                    //  startDate <= DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strEndDate);
                    //  startDate = new DateTime(startDate.AddMonths(1).Year, startDate.AddMonths(1).Month, 1))
                    //{
                    //    colName = startDate.ToString("yyyyMM");
                    //    dr[colName] = DBNull.Value;
                    //    dr[colName + "_Agency_Fee_Cost"] = DBNull.Value;
                    //    if (m_bCreateAdviceNote)
                    //    {
                    //        double originalValue = 0;
                    //        if (dr[colName + "_Original"] != null && dr[colName + "_Original"] != DBNull.Value && dr[colName + "_Original"].ToString() != "")
                    //            originalValue = Convert.ToDouble(dr[colName + "_Original"]);
                    //        InsertSpotPlanEdit(0, originalValue, colName, dr, true);
                    //    }
                    //}
                }
                else if (frm.rdDaily.Checked) //Daily
                {
                    dr["CPM"] = 5;

                    //Clear Start Other Month
                    SpotPlanEditClearMonth(DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strStartDate), frm.dtStartDate.Value, dr);
                    //DateTime startDate2 = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strStartDate);
                    //for (;
                    //   startDate2 < frm.dtStartDate.Value;
                    //   startDate2 = new DateTime(startDate2.AddMonths(1).Year, startDate2.AddMonths(1).Month, 1))
                    //{
                    //    string colName = startDate2.ToString("yyyyMM");
                    //    dr[colName] = DBNull.Value;
                    //    dr[colName + "_Agency_Fee_Cost"] = DBNull.Value;
                    //    //dr[colName + "_Original"] = DBNull.Value;
                    //    if (m_bCreateAdviceNote)
                    //    {
                    //        double originalValue = 0;
                    //        if (dr[colName + "_Original"] != null && dr[colName + "_Original"] != DBNull.Value && dr[colName + "_Original"].ToString() != "")
                    //            originalValue = Convert.ToDouble(dr[colName + "_Original"]);
                    //        InsertSpotPlanEdit(0, originalValue, colName, dr, true);
                    //    }
                    //}

                    double dTotalDays = (frm.dtEndDate.Value - frm.dtStartDate.Value).Days;
                    double dPricePerDay = dNetCost / dTotalDays;
                    double dAgencyFeePerDay = dAgencyFee / dTotalDays;

                    double dPriceBeforeLastMonth = 0;
                    double dAgencyFeeBeforeLastMonth = 0;
                    DateTime startDate = frm.dtStartDate.Value;
                    for (;
                       startDate <= frm.dtEndDate.Value;
                       startDate = new DateTime(startDate.AddMonths(1).Year, startDate.AddMonths(1).Month, 1))
                    {
                        DateTime endDate = new DateTime(startDate.Year, startDate.Month, 1).AddMonths(1).AddDays(-1);
                        if (endDate > frm.dtEndDate.Value)
                            endDate = frm.dtEndDate.Value;

                        double dDiffDay = (endDate - startDate).Days;
                        double dPrice = Math.Round(dPricePerDay * dDiffDay, 2);
                        double dAgencyCost = Math.Round(dAgencyFeePerDay * dDiffDay, 2);


                        //Last Month
                        if (new DateTime(startDate.AddMonths(1).Year, startDate.AddMonths(1).Month, 1) > frm.dtEndDate.Value)
                        {
                            dPrice = dNetCost - dPriceBeforeLastMonth;
                            dAgencyCost = dAgencyFee - dAgencyFeeBeforeLastMonth;
                        }

                        dPriceBeforeLastMonth = dPriceBeforeLastMonth + dPrice;
                        dAgencyFeeBeforeLastMonth = dAgencyFeeBeforeLastMonth + dAgencyCost;
                        string colName = startDate.ToString("yyyyMM");
                        dr[colName] = dPrice;
                        dr[colName + "_Agency_Fee_Cost"] = dAgencyCost;

                        if (m_bCreateAdviceNote)
                        {
                            double originalValue = 0;
                            if (dr[colName + "_Original"] != null && dr[colName + "_Original"] != DBNull.Value && dr[colName + "_Original"].ToString() != "")
                                originalValue = Convert.ToDouble(dr[colName + "_Original"]);
                            InsertSpotPlanEdit(dPrice, originalValue, colName, dr, false);
                        }
                    }
                    //Clear End Other Month
                    SpotPlanEditClearMonth(startDate, DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strEndDate), dr);
                    //for (;
                    //  startDate <= DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strEndDate);
                    //  startDate = new DateTime(startDate.AddMonths(1).Year, startDate.AddMonths(1).Month, 1))
                    //{
                    //    string colName = startDate.ToString("yyyyMM");
                    //    dr[colName] = DBNull.Value;
                    //    dr[colName + "_Agency_Fee_Cost"] = DBNull.Value;
                    //    if (m_bCreateAdviceNote)
                    //    {
                    //        double originalValue = 0;
                    //        if (dr[colName + "_Original"] != null && dr[colName + "_Original"] != DBNull.Value && dr[colName + "_Original"].ToString() != "")
                    //            originalValue = Convert.ToDouble(dr[colName + "_Original"]);
                    //        InsertSpotPlanEdit(0, originalValue, colName, dr, true);
                    //    }
                    //}
                }
                //=======================
                // Monthly
                //=======================
                else
                {
                    dr["CPM"] = 3;
                    double iMonthly = 0;
                    //Count Month
                    DateTime startDate = frm.dtStartDate.Value;
                    for (;
                       startDate <= frm.dtEndDate.Value;
                       startDate = new DateTime(startDate.AddMonths(1).Year, startDate.AddMonths(1).Month, 1))
                    {
                        //iMonthly++;
                        DateTime endDate = new DateTime(startDate.AddMonths(1).Year, startDate.AddMonths(1).Month, 1).AddDays(-1);
                        if (endDate > frm.dtEndDate.Value)
                            endDate = frm.dtEndDate.Value;
                        if (startDate.Day == 16 || endDate.Day == 15)
                            iMonthly = iMonthly + 0.5;
                        else
                            iMonthly = iMonthly + 1;
                    }


                    //Clear Start Other Month
                    SpotPlanEditClearMonth(DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strStartDate), frm.dtStartDate.Value, dr);
                    //DateTime startDate2 = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strStartDate);
                    //for (;
                    //   startDate2 < frm.dtStartDate.Value;
                    //   startDate2 = new DateTime(startDate2.AddMonths(1).Year, startDate2.AddMonths(1).Month, 1))
                    //{
                    //    string colName = startDate2.ToString("yyyyMM");
                    //    dr[colName] = DBNull.Value;
                    //    dr[colName + "_Agency_Fee_Cost"] = DBNull.Value;
                    //    //dr[colName + "_Original"] = DBNull.Value;
                    //    if (m_bCreateAdviceNote)
                    //    {
                    //        double originalValue = 0;
                    //        if (dr[colName + "_Original"] != null && dr[colName + "_Original"] != DBNull.Value && dr[colName + "_Original"].ToString() != "")
                    //            originalValue = Convert.ToDouble(dr[colName + "_Original"]);
                    //        InsertSpotPlanEdit(0, originalValue, colName, dr, true);
                    //    }
                    //}

                    double dPricePerMonth = Math.Round(dNetCost / iMonthly, 2);
                    double dAgencyCostPerMonth = Math.Round(dAgencyFee / iMonthly, 2);

                    startDate = frm.dtStartDate.Value;
                    for (;
                       startDate <= frm.dtEndDate.Value;
                       startDate = new DateTime(startDate.AddMonths(1).Year, startDate.AddMonths(1).Month, 1))
                    {
                        DateTime endDate = new DateTime(startDate.AddMonths(1).Year, startDate.AddMonths(1).Month, 1).AddDays(-1);
                        if (endDate > frm.dtEndDate.Value)
                            endDate = frm.dtEndDate.Value;

                        //Last Month
                        if (new DateTime(startDate.AddMonths(1).Year, startDate.AddMonths(1).Month, 1) > frm.dtEndDate.Value)
                        {
                            if ((dPricePerMonth * iMonthly) != dNetCost)
                            {
                                double dBeforeLastMonth = dPricePerMonth * (iMonthly - 1);
                                dPricePerMonth = dNetCost - dBeforeLastMonth;
                            }
                            if ((dAgencyCostPerMonth * iMonthly) != dAgencyFee)
                            {
                                double dBeforeLastMonth = dAgencyCostPerMonth * (iMonthly - 1);
                                dAgencyCostPerMonth = dAgencyFee - dBeforeLastMonth;
                            }

                        }
                        string colName = startDate.ToString("yyyyMM");
                        if (startDate.Day == 16 || endDate.Day == 15)
                        {
                            dr[colName] = Math.Round(dPricePerMonth / 2, 2);
                            dr[colName + "_Agency_Fee_Cost"] = Math.Round(dAgencyCostPerMonth / 2, 2);

                        }
                        else
                        {
                            dr[colName] = dPricePerMonth;
                            dr[colName + "_Agency_Fee_Cost"] = dAgencyCostPerMonth;
                        }
                        //dr[colName + "_Original"] = dPricePerMonth;
                        if (m_bCreateAdviceNote)
                        {
                            double originalValue = 0;
                            if (dr[colName + "_Original"] != null && dr[colName + "_Original"] != DBNull.Value && dr[colName + "_Original"].ToString() != "")
                                originalValue = Convert.ToDouble(dr[colName + "_Original"]);
                            InsertSpotPlanEdit(dPricePerMonth, originalValue, colName, dr, false);
                        }
                    }
                    //Clear End Other Month
                    SpotPlanEditClearMonth(startDate, DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strEndDate), dr);
                    //for (;
                    //   startDate <= DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strEndDate);
                    //   startDate = new DateTime(startDate.AddMonths(1).Year, startDate.AddMonths(1).Month, 1))
                    //{
                    //    string colName = startDate.ToString("yyyyMM");
                    //    dr[colName] = DBNull.Value;
                    //    dr[colName + "_Agency_Fee_Cost"] = DBNull.Value;
                    //    //dr[colName + "_Original"] = DBNull.Value;
                    //    double originalValue = 0;
                    //    if (dr[colName + "_Original"] != null && dr[colName + "_Original"] != DBNull.Value && dr[colName + "_Original"].ToString() != "")
                    //        originalValue = Convert.ToDouble(dr[colName + "_Original"]);
                    //    InsertSpotPlanEdit(0, originalValue, colName, dr, true);
                    //}
                }

                RefreshBudget();
            }
        }
        //private void gvSpotPlanEdit_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        //{
        //    DataRow dr = ((DataRowView)gvSpotPlanEdit.Rows[e.RowIndex].DataBoundItem).Row;
        //    DateTime startDate = Convert.ToDateTime(dr["StartDate"]);
        //    switch (startDate.Month%2)
        //    {
        //        case 0:
        //            gvSpotPlanEdit.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.PaleTurquoise;
        //            break;
        //        //case 1:
        //        //    gvSpotPlanEdit.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Lavender;
        //        //    break;
        //        default:
        //            break;
        //    }
        //    //switch (dr["Kind"].ToString())
        //    //{
        //    //    case "Del":
        //    //        gvSpotPlanEdit.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.DarkGray;
        //    //        break;
        //    //    //case "Add":
        //    //    //    gvSpotPlanEdit.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.DarkGray;
        //    //    //    break;
        //    //    default:
        //    //        break;
        //    //}
        //}
        List<DataRow> lsSpotPlanEditDelete = new List<DataRow>();
        private void InsertSpotPlanEdit(double currentValue, double originalValue, string colName, DataRow dr, bool IsClearOtherMonth)
        {
            if (currentValue != originalValue)
            {
                DataTable dtSpotPlanEdit = ((DataTable)gvSpotPlanEdit.DataSource);
                DataRow drSpotPlanEdit_Del = dtSpotPlanEdit.NewRow();
                DataRow drSpotPlanEdit_Add = dtSpotPlanEdit.NewRow();
                int iTeam = 0;
                for (int i = 0; i < dtSpotPlanEdit.Rows.Count; i++)
                {
                    DataRow drTemp = dtSpotPlanEdit.Rows[i];
                    if (drTemp["Team"] == DBNull.Value)
                        continue;
                    if (drTemp["Team"].ToString() == iTeam.ToString())
                    {
                        iTeam++;
                        i = -1;
                    }
                }

                DateTime startDate = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(colName + "01");
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);


                DateTime rowStartDate = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(dr["Start_Date"].ToString());
                DateTime rowEndDate = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(dr["End_Date"].ToString());
                if (!IsClearOtherMonth)
                {
                    if (startDate < rowStartDate)
                        startDate = rowStartDate;

                    if (endDate > rowEndDate)
                        endDate = rowEndDate;
                }

                DataRow drClone = ((DataTable)gvCalendar.DataSource).NewRow();

                drClone.ItemArray = dr.ItemArray;
                if (drClone["CPM"].ToString() != "1")//None Option 1 Unit
                {
                    drClone["Show_Date"] = startDate.ToString("yyyyMMdd");
                    drClone["Start_Date"] = startDate.ToString("yyyyMMdd");
                    drClone["End_Date"] = endDate.ToString("yyyyMMdd");
                    drClone["StartDate"] = startDate.ToString("dd/MM/yyyy");
                    drClone["EndDate"] = endDate.ToString("dd/MM/yyyy");
                }
                drClone["Spot_Plan_Id"] = drClone[colName + "_IDKey"];
                if (drClone["Spot_Plan_Id"] == null
                    || drClone["Spot_Plan_Id"] == DBNull.Value
                    || drClone["Spot_Plan_Id"].ToString() == "")
                    drClone["Spot_Plan_Id"] = 0;

                drClone["Net_Cost"] = originalValue;
                PackingSpotPlanEdit(drSpotPlanEdit_Del, drClone, iTeam, "Del");
                drClone["Net_Cost"] = currentValue;
                PackingSpotPlanEdit(drSpotPlanEdit_Add, drClone, iTeam, "Add");

                bool bChangeAddExist = false;
                for (int i = 0; i < dtSpotPlanEdit.Rows.Count; i++)
                {
                    DataRow drTemp = dtSpotPlanEdit.Rows[i];
                    if (drTemp["Spot_Plan_Id"].ToString() == drClone["Spot_Plan_Id"].ToString()
                        && drTemp["Start_Date"].ToString() == drClone["Start_Date"].ToString()
                        && drTemp["End_Date"].ToString() == drClone["End_Date"].ToString()
                        && drTemp["Print_DateTime"] == DBNull.Value
                        && drTemp["Kind"].ToString() == "Add")
                    {
                        bChangeAddExist = true;
                        drSpotPlanEdit_Add["Item"] = Convert.ToInt32(drTemp["Item"]);
                        drSpotPlanEdit_Add["Team"] = Convert.ToInt32(drTemp["Team"]);
                        drClone["Item"] = Convert.ToInt32(drTemp["Item"]);


                        DataRow drDelete = drTemp.Table.NewRow();
                        drDelete.ItemArray = drTemp.ItemArray;
                        lsSpotPlanEditDelete.Add(drDelete);

                        dtSpotPlanEdit.Rows.Remove(drTemp);
                        break;
                    }
                }
                if (bChangeAddExist == false)
                {
                    bool bChangeDelExist = false;
                    for (int i = 0; i < dtSpotPlanEdit.Rows.Count; i++)
                    {
                        DataRow drTemp = dtSpotPlanEdit.Rows[i];
                        if (drTemp["Spot_Plan_Id"].ToString() == drClone["Spot_Plan_Id"].ToString()
                            && drTemp["Start_Date"].ToString() == drClone["Start_Date"].ToString()
                            && drTemp["End_Date"].ToString() == drClone["End_Date"].ToString()
                            && drTemp["Print_DateTime"] == DBNull.Value
                            && drTemp["Kind"].ToString() == "Del")
                        {
                            bChangeDelExist = true;
                            drSpotPlanEdit_Add["Item"] = Convert.ToInt32(drTemp["Item"]);
                            drSpotPlanEdit_Add["Team"] = Convert.ToInt32(drTemp["Team"]);
                            drClone["Item"] = Convert.ToInt32(drTemp["Item"]);
                            break;
                        }
                    }
                    if (bChangeDelExist == false)
                    {
                        if (originalValue != 0)
                            ((DataTable)gvSpotPlanEdit.DataSource).Rows.Add(drSpotPlanEdit_Del);
                    }
                }
                //DELETE
                if (currentValue != 0)
                    ((DataTable)gvSpotPlanEdit.DataSource).Rows.Add(drSpotPlanEdit_Add);


            }
            else// original value = current
            {
                DataTable dtSpotPlanEdit = ((DataTable)gvSpotPlanEdit.DataSource);

                DateTime startDate = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(colName + "01");
                DateTime rowStartDate = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(dr["Start_Date"].ToString());
                if (startDate < rowStartDate)
                    startDate = rowStartDate;

                for (int i = 0; i < dtSpotPlanEdit.Rows.Count; i++)
                {
                    DataRow drTemp = dtSpotPlanEdit.Rows[i];
                    if (drTemp["Spot_Plan_Id"].ToString() == dr[colName + "_IDKey"].ToString()
                        && drTemp["Start_Date"].ToString() == startDate.ToString("yyyyMMdd")
                        && drTemp["Print_DateTime"] == DBNull.Value)
                    {
                        DataRow drDelete = drTemp.Table.NewRow();
                        drDelete.ItemArray = drTemp.ItemArray;
                        lsSpotPlanEditDelete.Add(drDelete);

                        dtSpotPlanEdit.Rows.Remove(drTemp);
                        i--;
                    }
                }
            }
        }

        private void PackingSpotPlanEdit(DataRow drTo, DataRow drFrom, int iTeam, string strKind)
        {
            drTo["Buying_Brief_ID"] = drFrom["Buying_Brief_ID"];
            drTo["Market_ID"] = "THAILAND";
            drTo["Version"] = drFrom["Version"];
            drTo["Item"] = drFrom["Item"];
            drTo["Show_Date"] = drFrom["Show_Date"];
            drTo["ID"] = drFrom["ID"];
            drTo["Status"] = drFrom["Status"];
            drTo["Team"] = iTeam;
            drTo["Kind"] = strKind;
            drTo["Edit_Date"] = m_Edit_Date;
            drTo["Edit_Time"] = m_Edit_Time;
            drTo["EditDate"] = m_strEditDate;
            drTo["Media_ID"] = drFrom["Media_ID"];
            drTo["MediaName"] = drFrom["MediaName"];
            drTo["Media_Vendor_ID"] = drFrom["Media_Vendor_ID"];
            drTo["MediaVendorName"] = drFrom["MediaVendorName"];
            drTo["Start_Time"] = drFrom["Start_Time"];
            drTo["End_Time"] = drFrom["End_Time"];
            drTo["Program"] = drFrom["Program"];
            drTo["WeekdayLimit"] = drFrom["WeekdayLimit"];
            drTo["Package"] = drFrom["Package"];
            drTo["SizeHW"] = drFrom["SizeHW"];
            drTo["Unit"] = drFrom["Unit"];
            drTo["Material_Key"] = drFrom["Material_Key"];
            drTo["Material_ID"] = drFrom["Material_ID"];
            drTo["Length"] = drFrom["Length"];
            drTo["Start_Date"] = drFrom["Start_Date"];
            drTo["End_Date"] = drFrom["End_Date"];
            drTo["StartDate"] = drFrom["StartDate"];
            drTo["EndDate"] = drFrom["EndDate"];
            drTo["Deadline_Material"] = drFrom["Deadline_Material"];
            drTo["Deadline_Terminate"] = drFrom["Deadline_Terminate"];
            drTo["State"] = drFrom["State"];
            drTo["Market_Price"] = drFrom["Market_Price"];
            drTo["Rate"] = drFrom["Rate"];
            drTo["Discount"] = drFrom["Discount"];
            drTo["Weight"] = 100;
            drTo["Net_Cost"] = drFrom["Net_Cost"];
            drTo["Program_Type"] = drFrom["Program_Type"];
            drTo["Prebuy_Start_Time"] = drFrom["Prebuy_Start_Time"];
            drTo["Prebuy_End_Time"] = drFrom["Prebuy_End_Time"];
            drTo["Rating"] = drFrom["Rating"];
            drTo["CPRP_Cost"] = drFrom["CPRP_Cost"];
            drTo["Include_Media_Cost"] = true;
            drTo["Remark"] = drFrom["Remark"];
            drTo["Spots"] = drFrom["Spots"];
            drTo["Adept_Code"] = drFrom["Adept_Code"];
            drTo["Adept_Export"] = drFrom["Adept_Export"];
            drTo["Adept_Export_Date"] = drFrom["Adept_Export_Date"];
            drTo["Row_ID"] = drFrom["Row_ID"];
            drTo["Pkg"] = drFrom["Pkg"];
            drTo["Booking_Order_ID"] = drFrom["Booking_Order_ID"];
            drTo["Program_Code"] = drFrom["Program_Code"];
            drTo["Invoice_Number"] = drFrom["Invoice_Number"];
            drTo["Total_Gross"] = drFrom["Total_Gross"];
            drTo["Vendor_Discount"] = drFrom["Vendor_Discount"];
            drTo["Vendor_Net_Cost"] = drFrom["Vendor_Net_Cost"];
            drTo["Billings_Year"] = drFrom["Billings_Year"];
            drTo["Billings_Month"] = drFrom["Billings_Month"];
            drTo["MediaSubTypeName"] = drFrom["MediaSubTypeName"];
            drTo["Media_Sub_Type"] = drFrom["Media_Sub_Type"];
            drTo["BuyTypeName"] = drFrom["BuyTypeName"];
            drTo["BuyTypeID"] = drFrom["BuyTypeID"];
            drTo["Agency_Fee"] = drFrom["Agency_Fee"];
            drTo["AgencyFeePercent"] = drFrom["AgencyFeePercent"];
            drTo["Spot_Plan_Id"] = drFrom["Spot_Plan_ID"];
            drTo["MediaTypeName"] = drFrom["MediaTypeName"];
            drTo["Media_Type"] = drFrom["Media_Type"];
        }

        private void MediaScheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Implementation_SpotPlan_Print_Schedule frm = new Implementation_SpotPlan_Print_Schedule(m_strBuyingBriefID, m_strVersion, m_strStartDate, m_strEndDate, m_strScheduleExportPath, m_strScheduleDirectPayExportPath, m_strScheduleLumpSumExportPath, m_strMediaType, "", m_MasterMediaType);
            frm.username = username; //Modified by Chaiwat.i 15/03/2023 TFS159033 [T2] : Set protect excel sheet when print report (C#)
            frm.chkLumpSum.Visible = false;
            frm.Show();
        }

        private void PurchaseOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Implementation_SpotPlan_Print_PO frm = new Implementation_SpotPlan_Print_PO(m_strBuyingBriefID, m_strVersion, m_strStartDate, m_strEndDate, m_strPOExportPath, m_strMediaType, "", m_MasterMediaType);
            frm.username = username; //Modified by Chaiwat.i 15/03/2023 TFS159033 [T2] : Set protect excel sheet when print report (C#)
            frm.Show();
        }

        private void gvDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || m_screenMode == eScreenMode.View)
                return;
            if (e.ColumnIndex == 0) //DELETE
            {
                if (GMessage.MessageComfirm("Do you want to delete ?") != DialogResult.Yes)
                    return;

                DataRow dr = ((DataRowView)gvDetail.Rows[e.RowIndex].DataBoundItem).Row;
                if (m_bCreateAdviceNote)
                {
                    // Clear Start Other Month
                    SpotPlanEditClearMonth(DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strStartDate)
                        , DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strEndDate)
                        , dr);

                    //DataTable dtSpotPlanEdit = ((DataTable)gvSpotPlanEdit.DataSource);
                    //int iTeam = 0;
                    //for (int i = 0; i < dtSpotPlanEdit.Rows.Count; i++)
                    //{
                    //    DataRow drTemp = dtSpotPlanEdit.Rows[i];
                    //    if (drTemp["Team"] == DBNull.Value)
                    //        continue;
                    //    if (drTemp["Team"].ToString() == iTeam.ToString())
                    //    {
                    //        iTeam++;
                    //        i = -1;
                    //    }
                    //}

                    //DataRow drSpotPlanEdit_Del = ((DataTable)gvSpotPlanEdit.DataSource).NewRow();
                    //DataRow dr = ((DataRowView)gvDetail.Rows[e.RowIndex].DataBoundItem).Row;

                    //if (dr.RowState == DataRowState.Added
                    //    || dr.RowState == DataRowState.Modified)
                    //{
                    //    for (int i = 0; i < dtSpotPlanEdit.Rows.Count; i++)
                    //    {
                    //        DataRow drSPEdit = dtSpotPlanEdit.Rows[i];
                    //        if (drSPEdit["Item"].Equals(dr["Item"]))
                    //        {
                    //            DataRow drDelete = drSPEdit.Table.NewRow();
                    //            drDelete.ItemArray = drSPEdit.ItemArray;
                    //            lsSpotPlanEditDelete.Add(drDelete);

                    //            dtSpotPlanEdit.Rows.Remove(drSPEdit);
                    //        }
                    //    }
                    //}
                    //else //Unchange
                    //{
                    //    for (int i = 0; i < dtSpotPlanEdit.Rows.Count; i++)
                    //    {
                    //        DataRow drSPEdit = dtSpotPlanEdit.Rows[i];
                    //        if (drSPEdit["Item"].Equals(dr["Item"]) && drSPEdit["Kind"].ToString() == "Add")
                    //            iTeam = Convert.ToInt32(drSPEdit["Team"].ToString());
                    //    }
                    //    PackingSpotPlanEdit(drSpotPlanEdit_Del, dr, iTeam, "Del");
                    //    dtSpotPlanEdit.Rows.Add(drSpotPlanEdit_Del);
                    //}
                }
                gvDetail.Rows.Remove(gvDetail.Rows[e.RowIndex]);
                RefreshBudget();
            }
        }

        private void btnAddDetail_Click(object sender, EventArgs e)
        {
            Implementation_SpotPlan_OD_Amendment frm = new Implementation_SpotPlan_OD_Amendment("Add", m_strMediaType);
            if (m_MasterMediaType == "")
                frm = new Implementation_SpotPlan_OD_Amendment("Add", m_strMediaType);
            else
                frm = new Implementation_SpotPlan_OD_Amendment("Add", m_strMediaType, m_MasterMediaType);

            frm.BuyingBriefID = txtBuyingBriefNo.Text;
            frm.Product_ID = txtProductCode.Text;
            frm.Client_ID = txtClientCode.Text;
            frm.txtAgencyFeePercent.Value = m_dAgencyFee * 100;
            frm.CampaignStartDate = m_strStartDate;
            frm.CampaignEndDate = m_strEndDate;
            DataRow drDetail = ((DataTable)gvDetail.DataSource).NewRow();
            drDetail["Buying_Brief_ID"] = txtBuyingBriefNo.Text;
            frm.drInput = drDetail;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                DataRow dr = ((DataTable)gvDetail.DataSource).NewRow();
                int item = GetNextItem();
                //*****//
                //dr["Pane"] = "A";//Add
                //*****//

                dr["Buying_Brief_ID"] = txtBuyingBriefNo.Text;
                dr["Item"] = item;
                dr["AdeptMediaTypeName"] = frm.AdeptMediaType;
                dr["MediaTypeName"] = frm.txtMediaType.Text;
                dr["Media_Type"] = frm.txtMediaTypeCode.Text;
                dr["MediaSubTypeName"] = frm.txtMediaSubType.Text;
                dr["Media_Sub_Type"] = frm.txtMediaSubTypeCode.Text;
                dr["MediaName"] = frm.txtMedia.Text;
                dr["Media_ID"] = frm.txtMediaCode.Text;
                dr["MediaVendorName"] = frm.txtVendor.Text;
                dr["Media_Vendor_ID"] = frm.txtVendorCode.Text;
                dr["Program"] = frm.txtDescription.Text.Replace(",", ";").Replace("\"", " ").Replace("'", "`");

                dr["BuyTypeID"] = frm.txtBuyTypeCode.Text == "" ? "AG" : frm.txtBuyTypeCode.Text;
                dr["BuyTypeName"] = frm.m_strBuyTypeName == "" ? "By Agency" : frm.m_strBuyTypeName;
                dr["Deadline_Terminate"] = frm.txtBuyType.Text == "" ? "By Agency" : frm.txtBuyType.Text;

                dr["Material_ID"] = frm.m_strMaterialID;
                //dr["Material_Key"] = frm.txtMaterialCode.Text;
                //dr["MaterialName"] = frm.txtMaterial.Text;

                
                //Defaul for NULL Value 
                dr["Package"] = "";
                dr["SizeHW"] = frm.txtSize.Text;
                dr["Length"] = 0;
                dr["Unit"] = frm.txtUnit.Text;
                dr["State"] = frm.cboCostType.Text;
                dr["Spending_Type"] = frm.m_iSpendingType;
                dr["Market_ID"] = "THAILAND";
                dr["ID"] = 1;
                dr["Version"] = m_strVersion;
                dr["Status"] = m_iStatus;
                dr["Weight"] = 100;
                dr["Include_Media_Cost"] = true; //1
                dr["Spots"] = 1;
                dr["Adept_Code"] = item;
                dr["Adept_Export"] = 0;
                dr["Adept_Hide"] = 0;
                dr["Appear"] = 0;
                dr["Finish"] = 0;
                dr["Pkg"] = 0;
                dr["Program_Code"] = 0;
                dr["Surcharge"] = 0;
                dr["Bonus_Percent"] = 0;
                dr["Bonus_Cost"] = 0;
                dr["Province"] = frm.txtProvince.Text;
                dr["CD_Percent"] = 0;
                dr["OMI"] = false;
                dr["Verified"] = false;
                dr["CH7_export"] = false;
                dr["Spot_verified"] = false;
                dr["Spot_Plan_ID"] = 0;

                dr["Remark"] = frm.txtRemark.Text;
                dr["Invoice_Number"] = frm.txtQuotation.Text;

                dr["Show_Date"] = frm.dtStartDate.Value.ToString("yyyyMMdd");
                dr["Start_Date"] = frm.dtStartDate.Value.ToString("yyyyMMdd");
                dr["End_Date"] = frm.dtEndDate.Value.ToString("yyyyMMdd");
                dr["StartDate"] = frm.dtStartDate.Value.ToString("dd/MM/yyyy");
                dr["EndDate"] = frm.dtEndDate.Value.ToString("dd/MM/yyyy");
                dr["Start_Time"] = "0600";
                dr["End_Time"] = "2400";
                dr["DifDays"] = Convert.ToInt32(frm.txtTotalDays.Text.ToString());
                dr["Original_Rating"] = Convert.ToInt32(frm.txtTotalDays.Text.ToString());
                //dr["Market_Price"] = frm.txtImpression.Value;
                dr["Rate"] = frm.txtNetCost.Value;
                dr["Discount"] = 0;
                double dNetCost = Convert.ToDouble(frm.txtNetCost.Value);
                dr["Net_Cost"] = dNetCost;

                double dTotalPrice = Convert.ToDouble(frm.txtTotalPrice.Value);
                dr["CPRP_Cost"] = dTotalPrice;
                dr["Total_Gross"] = dTotalPrice;

                dr["Agency_Fee"] = frm.txtAgencyFeePercent.Value / 100;
                dr["AgencyFeePercent"] = frm.txtAgencyFeePercent.Value;
                double dAgencyFee = Convert.ToDouble(Math.Round(frm.txtNetCost.Value * (frm.txtAgencyFeePercent.Value / 100), 2));
                dr["AgencyFee"] = dAgencyFee;//frm.txtNetCost.Value * (frm.txtAgencyFeePercent.Value / 100);

                dr["Market_Price"] = 0;
                dr["Deadline_Material"] = m_iStatus;
                dr["Program_Type"] = "Percent %";
                dr["Rating"] = frm.txtAgencyFeePercent.Value;

                if (frm.rdNone.Checked)
                {
                    dr["CPM"] = 1;
                    string colName = "yyyyMM";
                    //Clear Start Other Month
                    //SpotPlanEditClearMonth(DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strStartDate), frm.dtStartDate.Value, dr);
                    
                    colName = frm.dtStartDate.Value.ToString("yyyyMM");
                    dr[colName] = dNetCost;
                    dr[colName + "_Original"] = 0;
                    dr[colName + "_Agency_Fee_Cost"] = dAgencyFee;
                    dr[colName + "_IDKey"] = 0;

                    DateTime startDate = frm.dtStartDate.Value;

                    if (m_bCreateAdviceNote)
                    {
                        double originalValue = 0;
                        if (dr[colName + "_Original"] != null && dr[colName + "_Original"] != DBNull.Value && dr[colName + "_Original"].ToString() != "")
                            originalValue = Convert.ToDouble(dr[colName + "_Original"]);
                        InsertSpotPlanEdit(dNetCost, originalValue, colName, dr, false);
                    }

                    //startDate = new DateTime(startDate.AddMonths(1).Year, startDate.AddMonths(1).Month, 1);
                    //Clear Other Month
                    //SpotPlanEditClearMonth(startDate, DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strEndDate), dr);
                }
                else if (frm.rdDaily.Checked) //Daily
                {
                    dr["CPM"] = 5;

                    //Clear Start Other Month
                    //SpotPlanEditClearMonth(DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strStartDate), frm.dtStartDate.Value, dr);
                    

                    double dTotalDays = (frm.dtEndDate.Value - frm.dtStartDate.Value).Days;
                    double dPricePerDay = dNetCost / dTotalDays;
                    double dAgencyFeePerDay = dAgencyFee / dTotalDays;

                    double dPriceBeforeLastMonth = 0;
                    double dAgencyFeeBeforeLastMonth = 0;
                    DateTime startDate = frm.dtStartDate.Value;
                    for (;
                       startDate <= frm.dtEndDate.Value;
                       startDate = new DateTime(startDate.AddMonths(1).Year, startDate.AddMonths(1).Month, 1))
                    {
                        DateTime endDate = new DateTime(startDate.Year, startDate.Month, 1).AddMonths(1).AddDays(-1);
                        if (endDate > frm.dtEndDate.Value)
                            endDate = frm.dtEndDate.Value;

                        double dDiffDay = (endDate - startDate).Days;
                        double dPrice = Math.Round(dPricePerDay * dDiffDay, 2);
                        double dAgencyCost = Math.Round(dAgencyFeePerDay * dDiffDay, 2);

                        //Last Month
                        if (new DateTime(startDate.AddMonths(1).Year, startDate.AddMonths(1).Month, 1) > frm.dtEndDate.Value)
                        {
                            dPrice = dNetCost - dPriceBeforeLastMonth;
                            dAgencyCost = dAgencyFee - dAgencyFeeBeforeLastMonth;
                        }

                        dPriceBeforeLastMonth = dPriceBeforeLastMonth + dPrice;
                        dAgencyFeeBeforeLastMonth = dAgencyFeeBeforeLastMonth + dAgencyCost;
                        string colName = startDate.ToString("yyyyMM");
                        dr[colName] = dPrice;
                        dr[colName + "_Original"] = 0;
                        dr[colName + "_Agency_Fee_Cost"] = dAgencyCost;
                        dr[colName + "_IDKey"] = 0;
                        if (m_bCreateAdviceNote)
                        {
                            double originalValue = 0;
                            if (dr[colName + "_Original"] != null && dr[colName + "_Original"] != DBNull.Value && dr[colName + "_Original"].ToString() != "")
                                originalValue = Convert.ToDouble(dr[colName + "_Original"]);
                            InsertSpotPlanEdit(dPrice, originalValue, colName, dr, false);
                        }
                    }
                    //Clear End Other Month
                    //SpotPlanEditClearMonth(startDate, DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strEndDate), dr);
                }
                else//Monthly
                {
                    dr["CPM"] = 3;
                    double iMonthly = 0;
                    //Count Month
                    DateTime startDate = frm.dtStartDate.Value;
                    for (;
                       startDate <= frm.dtEndDate.Value;
                       startDate = new DateTime(startDate.AddMonths(1).Year, startDate.AddMonths(1).Month, 1))
                    {
                        //iMonthly++;
                        DateTime endDate = new DateTime(startDate.AddMonths(1).Year, startDate.AddMonths(1).Month, 1).AddDays(-1);
                        if (endDate > frm.dtEndDate.Value)
                            endDate = frm.dtEndDate.Value;
                        if (startDate.Day == 16 || endDate.Day == 15)
                            iMonthly = iMonthly + 0.5;
                        else
                            iMonthly = iMonthly + 1;
                    }


                    //Clear Start Other Month
                    //SpotPlanEditClearMonth(DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strStartDate), frm.dtStartDate.Value, dr);
                    
                    double dPricePerMonth = Math.Round(dNetCost / iMonthly, 2);
                    double dAgencyCostPerMonth = Math.Round(dAgencyFee / iMonthly, 2);

                    startDate = frm.dtStartDate.Value;
                    for (;
                       startDate <= frm.dtEndDate.Value;
                       startDate = new DateTime(startDate.AddMonths(1).Year, startDate.AddMonths(1).Month, 1))
                    {
                        DateTime endDate = new DateTime(startDate.AddMonths(1).Year, startDate.AddMonths(1).Month, 1).AddDays(-1);
                        if (endDate > frm.dtEndDate.Value)
                            endDate = frm.dtEndDate.Value;

                        //Last Month
                        if (new DateTime(startDate.AddMonths(1).Year, startDate.AddMonths(1).Month, 1) > frm.dtEndDate.Value)
                        {
                            if ((dPricePerMonth * iMonthly) != dNetCost)
                            {
                                double dBeforeLastMonth = dPricePerMonth * (iMonthly - 1);
                                dPricePerMonth = dNetCost - dBeforeLastMonth;
                            }
                            if ((dAgencyCostPerMonth * iMonthly) != dAgencyFee)
                            {
                                double dBeforeLastMonth = dAgencyCostPerMonth * (iMonthly - 1);
                                dAgencyCostPerMonth = dAgencyFee - dBeforeLastMonth;
                            }

                        }
                        string colName = startDate.ToString("yyyyMM");
                        if (startDate.Day == 16 || endDate.Day == 15)
                        {
                            dr[colName] = Math.Round(dPricePerMonth / 2, 2);
                            dr[colName + "_Agency_Fee_Cost"] = Math.Round(dAgencyCostPerMonth / 2, 2);

                        }
                        else
                        {
                            dr[colName] = dPricePerMonth;
                            dr[colName + "_Agency_Fee_Cost"] = dAgencyCostPerMonth;
                        }
                        //if (dPricePerMonth == 0)
                        //{
                        //    dr[colName] = DBNull.Value;
                        //    dr[colName + "_Agency_Fee_Cost"] = DBNull.Value;
                        //}
                        dr[colName + "_IDKey"] = 0;
                        if (m_bCreateAdviceNote)
                        {
                            double originalValue = 0;
                            if (dr[colName + "_Original"] != null && dr[colName + "_Original"] != DBNull.Value && dr[colName + "_Original"].ToString() != "")
                                originalValue = Convert.ToDouble(dr[colName + "_Original"]);
                            InsertSpotPlanEdit(dPricePerMonth, originalValue, colName, dr, false);
                        }
                    }
                    //Clear End Other Month
                    //SpotPlanEditClearMonth(startDate, DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(m_strEndDate), dr);
                }
                ((DataTable)gvDetail.DataSource).Rows.Add(dr);
                RefreshBudget();
            }

        }

        private void adviceNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                string selectMedia = "";
                DataTable dtMedia = new DataTable();
                dtMedia.Columns.Add("Media");

                Implementation_SpotPlan_Print_AdviceNote frm = new Implementation_SpotPlan_Print_AdviceNote();
                frm.username = username; //Modified by Chaiwat.i 15/03/2023 TFS159033 [T2] : Set protect excel sheet when print report (C#)
                frm.dtSpontEdit = ((DataTable)gvSpotPlanEdit.DataSource).Copy();
                if (frm.ShowDialog() != DialogResult.Yes)
                    return;

                Cursor = Cursors.WaitCursor;
                OnCommandSave();
                DateTime dtStamp = DateTime.Now;
                foreach (DataGridViewRow item in frm.gvSpotPlanEdit.Rows)
                {
                    if (Convert.ToBoolean(item.Cells[0].Value))
                    {
                        DataRow dr = dtMedia.NewRow();
                        dr["Media"] = item.Cells[4].Value;
                        dtMedia.Rows.Add(dr);

                        DataRow drSpotPlanEditData = ((DataRowView)item.DataBoundItem).Row;
                        m_db.UpdateSpotPlanEditStamPrintDateTime(m_strBuyingBriefID, m_strVersion, Convert.ToInt32(drSpotPlanEditData["item"]), dtStamp);

                        if (drSpotPlanEditData["Kind"].ToString() == "Add")
                            m_db.UpdateSpotPlanPaymentNet_Cost_BeforePrintAdviseNote(Convert.ToInt32(drSpotPlanEditData["Spot_Plan_Id"]));
                        if (drSpotPlanEditData["Kind"].ToString() == "Del")
                            m_db.DeleteSpotPlanPaymentDetailIFNetCostIsZero(Convert.ToInt32(drSpotPlanEditData["Spot_Plan_Id"]));
                    }
                }

                for (int i = 0; i < dtMedia.Rows.Count; i++)
                {
                    if (i == dtMedia.Rows.Count - 1)
                        selectMedia += "'" + dtMedia.Rows[i]["Media"].ToString() + "'";
                    else
                        selectMedia += "'" + dtMedia.Rows[i]["Media"].ToString() + "', ";
                }

                bool showX = frm.chkShowX.Checked;

                bool bDirectPay = m_db.CheckDirectPayAdviceNote(m_strBuyingBriefID, m_strVersion, selectMedia);
                DataSet ds = m_db.SelectSpotPlanEditForAdviceNoteOD(m_strBuyingBriefID, m_strVersion, dtStamp, showX);
                //======================================
                // Create Result Excel
                //======================================
                string Template = m_strAdviceNoteExportPath;
                if (bDirectPay)
                    Template = m_strAdviceNoteDirectPayExportPath;
                Excel.Application ExcelObjDesc = new Excel.Application();
                Excel.Workbook theWorkbook = (Excel.Workbook)(ExcelObjDesc.Workbooks.Open(Template));
                Excel.Sheets sheets = theWorkbook.Worksheets;
                Excel.Worksheet workSheet = (Excel.Worksheet)sheets.get_Item(1);//Template

                //=======================================
                // Buying Brief
                //=======================================
                DataTable dt = ds.Tables[0];
                ExcelUtil.ExcelSetValueString(workSheet, "B", 5, dt.Rows[0]["Client_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 8, dt.Rows[0]["Product_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 9, dt.Rows[0]["Campaign_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 10, dt.Rows[0]["Buying_Brief_ID"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 11, dt.Rows[0]["Campaign_Period"].ToString());

                //=======================================
                // Spot Plan Edit (Advise Note) Header Logo
                //=======================================
                workSheet.PageSetup.FirstPage.RightHeader.Picture.Filename = dt.Rows[0]["Icon_Path"].ToString();
                workSheet.PageSetup.FirstPage.RightHeader.Text = "&G";
                workSheet.PageSetup.RightHeaderPicture.Filename = dt.Rows[0]["Icon_Path"].ToString();
                workSheet.PageSetup.RightHeader = "&G";

                //=======================================
                // Spot Plan Edit (Advise Note) Footer - BARCODE
                //=======================================
                workSheet.PageSetup.FirstPage.LeftFooter.Text = "&\"Free 3 of 9 Extended\"&45& *" + m_strBuyingBriefID + "AN" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*\n" +
                "" + "&\"Arial\"&12&                                              *" + m_strBuyingBriefID + "AN" + DateTime.Now.ToString("yyyyMMddHHmmss") + "*";

                //=======================================
                // Spot Plan Edit
                //=======================================
                dt = ds.Tables[1];
                DataTable dtAdviceNote = new DataTable();
                dtAdviceNote.Columns.Add("OriMedia");
                dtAdviceNote.Columns.Add("OriDesc");
                dtAdviceNote.Columns.Add("OriCostType");
                dtAdviceNote.Columns.Add("OriBuyType");
                dtAdviceNote.Columns.Add("OriSize");
                dtAdviceNote.Columns.Add("OriSche");
                dtAdviceNote.Columns.Add("OriFee");
                dtAdviceNote.Columns.Add("OriNet");

                dtAdviceNote.Columns.Add("NewMedia");
                dtAdviceNote.Columns.Add("NewDesc");
                dtAdviceNote.Columns.Add("NewCostType");
                dtAdviceNote.Columns.Add("NewBuyType");
                dtAdviceNote.Columns.Add("NewSize");
                dtAdviceNote.Columns.Add("NewSche");
                dtAdviceNote.Columns.Add("NewFee");
                dtAdviceNote.Columns.Add("NewNet");
                dtAdviceNote.Columns.Add("Remark");

                var gpTeam = dt.AsEnumerable()
                               .GroupBy(row => new
                               {
                                   Team = row.Field<string>("Team")
                               }).Select(g => new { g.Key.Team, RowCount = g.Count() });
                foreach (var gp in gpTeam.ToList())
                {
                    DataRow[] drOriFill = dt.Select(string.Format("Kind = 'Del' and Team = '{0}'", gp.Team));
                    DataRow[] drNewFill = dt.Select(string.Format("Kind = 'Add' and Team = '{0}'", gp.Team));
                    int iMaxRow = drOriFill.Length > drNewFill.Length ? drOriFill.Length : drNewFill.Length;
                    for (int i = 0; i < iMaxRow; i++)
                    {
                        DataRow dr = dtAdviceNote.NewRow();
                        if (i < drOriFill.Length)
                        {
                            dr["OriMedia"] = drOriFill[i]["MediaName"];
                            dr["OriDesc"] = bDirectPay == true ? drOriFill[i]["Deadline_Terminate"] : drOriFill[i]["Program"];
                            dr["OriCostType"] = drOriFill[i]["CostType"];
                            dr["OriBuyType"] = drOriFill[i]["BuyTypeName"];
                            dr["OriSize"] = drOriFill[i]["Size"];
                            dr["OriSche"] = drOriFill[i]["StartDate"] + " - " + drOriFill[i]["EndDate"];
                            dr["OriFee"] = drOriFill[i]["AgencyFee"];
                            dr["OriNet"] = drOriFill[i]["Net_Cost"];
                            dr["Remark"] = drOriFill[i]["Remark"];
                        }
                        if (i < drNewFill.Length)
                        {
                            dr["NewMedia"] = drNewFill[i]["MediaName"];
                            dr["NewDesc"] = bDirectPay == true ? drNewFill[i]["Deadline_Terminate"] : drNewFill[i]["Program"];
                            dr["NewCostType"] = drOriFill[i]["CostType"];
                            dr["NewBuyType"] = drOriFill[i]["BuyTypeName"];
                            dr["NewSize"] = drOriFill[i]["Size"];
                            dr["NewSche"] = drNewFill[i]["StartDate"] + " - " + drNewFill[i]["EndDate"];
                            dr["NewFee"] = drNewFill[i]["AgencyFee"];
                            dr["NewNet"] = drNewFill[i]["Net_Cost"];
                            dr["Remark"] = drNewFill[i]["Remark"];
                        }
                        dtAdviceNote.Rows.Add(dr);
                    }
                }

                if (dtAdviceNote.Rows.Count > 3)
                {
                    workSheet.Rows[15].Insert(Excel.XlInsertShiftDirection.xlShiftDown, workSheet.Rows[15].Copy(Type.Missing));
                }
                ExcelUtil.ExcelSetValueStringFullTableAdviceNote(workSheet, 14, 1, 4, dtAdviceNote);

                //=======================================
                // Client Direct Pay
                //=======================================
                if (bDirectPay)
                {
                    dt = ds.Tables[2];
                    DataTable dtDirectPay = new DataTable();
                    dtDirectPay.Columns.Add("OriMedia");
                    dtDirectPay.Columns.Add("OriBuyType");
                    dtDirectPay.Columns.Add("OriSche");
                    dtDirectPay.Columns.Add("OriNet");
                    dtDirectPay.Columns.Add("OriVendor");
                    dtDirectPay.Columns.Add("OriSpace");

                    dtDirectPay.Columns.Add("NewMedia");
                    dtDirectPay.Columns.Add("NewBuyType");
                    dtDirectPay.Columns.Add("NewSche");
                    dtDirectPay.Columns.Add("NewNet");
                    dtDirectPay.Columns.Add("NewVendor");
                    dtDirectPay.Columns.Add("NewSpace");
                    dtDirectPay.Columns.Add("Remark");

                    gpTeam = dt.AsEnumerable()
                                   .GroupBy(row => new
                                   {
                                       Team = row.Field<string>("Team")
                                   }).Select(g => new { g.Key.Team, RowCount = g.Count() });
                    foreach (var gp in gpTeam.ToList())
                    {
                        DataRow[] drOriFill = dt.Select(string.Format("Kind = 'Del' and Team = '{0}'", gp.Team));
                        DataRow[] drNewFill = dt.Select(string.Format("Kind = 'Add' and Team = '{0}'", gp.Team));
                        int iMaxRow = drOriFill.Length > drNewFill.Length ? drOriFill.Length : drNewFill.Length;
                        for (int i = 0; i < iMaxRow; i++)
                        {
                            DataRow dr = dtDirectPay.NewRow();
                            if (i < drOriFill.Length)
                            {
                                dr["OriMedia"] = drOriFill[i]["MediaName"];
                                dr["OriBuyType"] = drOriFill[i]["Deadline_Terminate"];
                                dr["OriSche"] = drOriFill[i]["StartDate"] + " - " + drOriFill[i]["EndDate"];
                                dr["OriNet"] = drOriFill[i]["Net_Cost"];
                                dr["OriVendor"] = drOriFill[i]["MediaVendorName"];
                                dr["OriSpace"] = "";
                                dr["Remark"] = drOriFill[i]["Remark"];
                            }
                            if (i < drNewFill.Length)
                            {
                                dr["NewMedia"] = drNewFill[i]["MediaName"];
                                dr["NewBuyType"] = drNewFill[i]["Deadline_Terminate"];
                                dr["NewSche"] = drNewFill[i]["StartDate"] + " - " + drNewFill[i]["EndDate"];
                                dr["NewNet"] = drNewFill[i]["Net_Cost"];
                                dr["NewVendor"] = drNewFill[i]["MediaVendorName"];
                                dr["NewSpace"] = "";
                                dr["Remark"] = drNewFill[i]["Remark"];
                            }
                            dtDirectPay.Rows.Add(dr);
                        }
                    }
                    int rowDP = 32;
                    if (dtAdviceNote.Rows.Count > 3)
                    {
                        rowDP += dtAdviceNote.Rows.Count - 3;
                    }
                    if (dtDirectPay.Rows.Count > 2)
                    {
                        workSheet.Rows[rowDP + 1].Insert(Excel.XlInsertShiftDirection.xlShiftDown, workSheet.Rows[rowDP + 1].Copy(Type.Missing));
                    }
                    ExcelUtil.ExcelSetValueStringFullTableAdviceNote(workSheet, rowDP, 1, 3, dtDirectPay);
                }

                //Modified by Chaiwat.i 15/03/2023 TFS159033 [T2] : Set protect excel sheet when print report (C#)
                workSheet.Protect(username, true);
                ExcelObjDesc.Visible = true;
                DataLoading();
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }

        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to exit Media Schedule?", "Media Schedule", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                this.DialogResult = DialogResult.Cancel;
        }

        private void Implementation_SpotPlan_OD_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Modified by Chaiwat.i 25/08/2021
            //m_db.DeleteSpotPlanSavingStatus(txtBuyingBriefNo.Text, m_strVersion, username, "Spot_Plan");
            m_db.DeleteSpotPlanSavingStatus(txtBuyingBriefNo.Text, m_strVersion, username, "Spot Plan");
        }

        private void gvCalendar_Scroll(object sender, ScrollEventArgs e)
        {
            if (gvDetail.Rows.Count == 0)
                return;
            gvDetail.FirstDisplayedScrollingRowIndex = gvCalendar.FirstDisplayedScrollingRowIndex;
        }

        private void gvDetail_Scroll(object sender, ScrollEventArgs e)
        {
            if (gvCalendar.Rows.Count == 0)
                return;
            gvCalendar.FirstDisplayedScrollingRowIndex = gvDetail.FirstDisplayedScrollingRowIndex;
        }

        private void gvCalendar_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            gvCalendar.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void gvCalendar_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {


            string colName = gvCalendar.Columns[e.ColumnIndex].HeaderText.Replace("-", "");
            DataRow dr = ((DataRowView)gvCalendar.Rows[e.RowIndex].DataBoundItem).Row;

            //Check Before and after change
            string vAfterChangeCarlendar = "";
            if (dr[colName] != null
                     && dr[colName] != DBNull.Value
                     && dr[colName].ToString() != "")
            {
                vAfterChangeCarlendar = dr[colName].ToString();
                if (Convert.ToInt32(dr[colName]) == 0)
                {
                    vAfterChangeCarlendar = "";
                    dr[colName] = DBNull.Value;
                    dr[colName + "_Agency_Fee_Cost"] = DBNull.Value;
                }
            }

            if (vBeforeChangeCarlendar == vAfterChangeCarlendar)
            {
                return;
            }


            DateTime startDate = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(colName + "01");
            DateTime rowEndDate = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(dr["End_Date"].ToString());
            if (startDate.Month > rowEndDate.Month)
            {
                dr[colName] = DBNull.Value;
                dr[colName + "_Agency_Fee_Cost"] = DBNull.Value;

                GMessage.MessageWarning($"Period must be in between {dr["StartDate"].ToString()} and {dr["EndDate"].ToString()}");
            }

            double dNewNet_Cost = 0;
            foreach (string strYearMonth in m_alCalendar)
            {
                if (dr[strYearMonth] != null
                     && dr[strYearMonth] != DBNull.Value
                     && dr[strYearMonth].ToString() != "")
                {
                    dr[strYearMonth] = Math.Round(Convert.ToDouble(dr[strYearMonth]), 2);
                    dNewNet_Cost = dNewNet_Cost + Convert.ToDouble(dr[strYearMonth]);
                }
            }

            if (m_bCreateAdviceNote)
            {

                double currentValue = 0;
                double originalValue = 0;

                if (dr[colName] != null && dr[colName] != DBNull.Value && dr[colName].ToString() != "")
                {
                    currentValue = Convert.ToDouble(dr[colName]);
                    dr[colName + "_Agency_Fee_Cost"] = Math.Round(currentValue * Convert.ToDouble(dr["Agency_Fee"]), 2);
                }
                if (currentValue == 0)
                {
                    dr[colName] = DBNull.Value;
                    dr[colName + "_Agency_Fee_Cost"] = DBNull.Value;
                }


                if (dr[colName + "_Original"] != null
                    && dr[colName + "_Original"] != DBNull.Value
                    && dr[colName + "_Original"].ToString() != "")
                    originalValue = Convert.ToDouble(dr[colName + "_Original"]);

                InsertSpotPlanEdit(currentValue, originalValue, colName, dr, false);
            }
            RefreshBudget();
        }


        private void gvCalendar_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (gvDetail.Columns[e.ColumnIndex].Name == "ColIssueTotal") //Total
            {
                if (!double.TryParse(gvDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString(), out var n))//checknumber
                {
                    GMessage.MessageWarning("Input valid format.");
                    e.Cancel = true;
                }
            }
            if (gvDetail.Columns[e.ColumnIndex].Name == "ColWHTaxPercent") //Percent
            {
                string val = gvDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString();
                val = val.Replace("%", "");
                if (!double.TryParse(val, out var n) && val != "")//checknumber
                {
                    GMessage.MessageWarning("Input valid format.");
                    e.Cancel = true;
                }

            }
        }

        private void gvCalendar_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            DataRow dr = ((DataRowView)gvDetail.Rows[e.RowIndex].DataBoundItem).Row;

            //else if (gvDetail.Columns[e.ColumnIndex].Name == "ColWHTaxPercent")
            //{
            //    DataRow dr = ((DataRowView)gvDetail.Rows[e.RowIndex].DataBoundItem).Row;

            //    if (dr["WHTPercent"] == DBNull.Value)
            //        return;
            //    string val = dr["WHTPercent"].ToString();
            //    if (val == "")
            //    {
            //        dr["WHTPercent"] = DBNull.Value;
            //        return;
            //    }
            //    val = val.Replace("%", "");
            //    if (val.IndexOf(".") > -1)
            //    {
            //        if (Convert.ToDouble(val) == 0.00)
            //            dr["WHTPercent"] = DBNull.Value;
            //        else
            //            dr["WHTPercent"] = Convert.ToDouble(val).ToString() + "%";
            //    }
            //    else
            //    {
            //        if (Convert.ToInt32(val) == 0)
            //            dr["WHTPercent"] = DBNull.Value;
            //        else
            //            dr["WHTPercent"] = Convert.ToInt32(val).ToString() + "%";
            //    }
            //}
        }

        private void gvCalendar_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(ColNumber_KeyPress);

            TextBox tb = e.Control as TextBox;
            if (tb != null)
            {
                tb.KeyPress += new KeyPressEventHandler(ColNumber_KeyPress);
            }
        }
        private void ColNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar))
            {
                if (e.KeyChar == 46 && !(((TextBox)sender).Text.IndexOf(".") > -1))
                    return;
                e.Handled = true;
            }
        }
        private void ColPercent_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != 46
                && e.KeyChar != 37)
            {
                e.Handled = true;
            }
            else
            {
                if (e.KeyChar == 46 && ((TextBox)sender).Text.IndexOf(".") > -1)
                    e.Handled = true;
                else if (e.KeyChar == 37 && ((TextBox)sender).Text.IndexOf("%") > -1)
                    e.Handled = true;
            }
        }
        private void gvCalendar_MouseClick(object sender, MouseEventArgs e)
        {
            //if (gvDetail.Rows.Count == 0)
            //    return;
            //if (e.Button == MouseButtons.Right)
            //{
            //    if (chkNonVat.Checked)
            //    {
            //        convertNonVatToVatAmountLessThan10ToolStripMenuItem.Visible = true;
            //        convertVatCheckedInvoiceToNonVatToolStripMenuItem.Visible = false;
            //    }
            //    else
            //    {
            //        convertNonVatToVatAmountLessThan10ToolStripMenuItem.Visible = false;
            //        convertVatCheckedInvoiceToNonVatToolStripMenuItem.Visible = true;
            //    }
            //    contextMenuStrip1.Show(gvDetail, new Point(e.X, e.Y));
            //}
        }

        private void gvDetail_Sorted(object sender, EventArgs e)
        {
            RefreshBudget();
        }

        private void gvCalendar_Sorted(object sender, EventArgs e)
        {
            RefreshBudget();
        }
        string vBeforeChangeCarlendar = "";
        private void gvCalendar_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void gvCalendar_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            string colName = gvCalendar.Columns[e.ColumnIndex].HeaderText.Replace("-", "");
            DataRow dr = ((DataRowView)gvCalendar.Rows[e.RowIndex].DataBoundItem).Row;
            vBeforeChangeCarlendar = dr[colName].ToString();
            if (dr["CPM"].ToString() == "1" && Convert.ToInt32(colName) != Convert.ToInt32(dr["Start_Date"].ToString().Substring(0, 6)))
            {
                GMessage.MessageWarning("Period out of range");
                e.Cancel = true;
                return;
            }
            if (Convert.ToInt32(colName) < Convert.ToInt32(dr["Start_Date"].ToString().Substring(0, 6))
                || Convert.ToInt32(colName) > Convert.ToInt32(dr["End_Date"].ToString().Substring(0, 6)))
            {
                GMessage.MessageWarning("Period out of range");
                e.Cancel = true;
            }
        }
    }
}
