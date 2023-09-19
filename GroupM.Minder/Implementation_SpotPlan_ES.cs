using GroupM.DBAccess;
using GroupM.UTL;
using System;
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
    public partial class Implementation_SpotPlan_ES : Form
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

        string m_strStartDate = null;
        string m_strEndDate = null;
        string m_strEditDate = null;

        string m_Edit_Date = "";
        string m_Edit_Time = "";

        string m_strScheduleExportPath = ConfigurationManager.AppSettings["ReportMinder"] + "Cue_ES.xlt";
        string m_strScheduleDirectPayExportPath = ConfigurationManager.AppSettings["ReportMinder"] + "Cue_ES_Pay2Vendor.xlt";
        string m_strScheduleLumpSumExportPath = ConfigurationManager.AppSettings["ReportMinder"] + "Cue_ES_LumpSum.xlt";
        string m_strPOExportPath = ConfigurationManager.AppSettings["ReportMinder"] + "Cue_ES_Booking_Order.xlt";
        string m_strAdviceNoteExportPath = ConfigurationManager.AppSettings["ReportMinder"] + "Var_ES.xlt";
        string m_strAdviceNoteDirectPayExportPath = ConfigurationManager.AppSettings["ReportMinder"] + "Var_ES_Pay2Vendor.xlt";

        enum eScreenMode { Add, Edit, View }
        eScreenMode m_screenMode = eScreenMode.Edit;
        enum eCol { MediaTpe, MediaSubTpe, Media, Vendor, Desc, StartDate, EndDate, Material, Impression, GRP, CPRP, TotalCost, Disc, NetCost, AgencyFeePercent, AgencyFee }

        public Implementation_SpotPlan_ES(string Username, string ScreenPermission, string BB, string v, int s, string mt)
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

            m_db = new DBManager();
            username = Username;
            screenmode = ScreenPermission;
        }
        public Implementation_SpotPlan_ES(string Username, string ScreenPermission, string BB, string v, int s, string mt, string mmt)
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
            txtAdviceNoteRemark.Text = dr["CommentAdviceNote"].ToString();//join buying brife martket

            DataTable dtMaterial = m_db.SelectMaterial(m_strBuyingBriefID);
            gvMaterial.AutoGenerateColumns = false;
            gvMaterial.DataSource = dtMaterial;

            DataTable dt = m_db.SelectSpotPlan(m_strBuyingBriefID, m_strVersion);
            gvDetail.AutoGenerateColumns = false;
            gvDetail.DataSource = dt;

            RefreshBudget();

            DataTable dtSpotPlanEdit = m_db.SelectSpotPlanEdit(m_strBuyingBriefID, m_strVersion);
            gvSpotPlanEdit.AutoGenerateColumns = false;
            gvSpotPlanEdit.DataSource = dtSpotPlanEdit;

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
            }
            catch (Exception e)
            {
                GMessage.MessageError(e);
            }
        }

        private void SpotPlan_Load(object sender, EventArgs e)
        {
            InitailControl();
            DataLoading();
            /*if (m_screenMode == eScreenMode.Edit)
            {
                m_db.InsertSpotPlanSavingStatus(txtBuyingBriefNo.Text, m_strVersion, username, DateTime.Now.ToString("yyyyMMddhhmmss"), "Spot_Plan");
            }*/
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (GMessage.MessageComfirm("Do you want to save?") != DialogResult.Yes)
                    return;

                if (CheckDataBeforeSave())
                {
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
            if (!m_db.UpdateSpotPlanRemark(m_strBuyingBriefID, txtScheduleRemark.Text, txtPORemark.Text, txtAdviceNoteRemark.Text))
                return false;

            DataTable dt = (DataTable)gvDetail.DataSource;
            if (m_db.DeleteSpotPlan(m_strBuyingBriefID, m_strVersion))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                        m_db.InsertSpotPlanDetail(dr);
                }
            }
            else
                return false;

            if (m_db.DeleteSpotPlanVersion(m_strBuyingBriefID, m_strVersion))
            {
                m_db.InsertSpotPlanVersion(m_strBuyingBriefID, m_strVersion, m_iStatus, dt.Rows.Count, username, Convert.ToDouble(txtActualBudget.Text));
            }
            else
                return false;

            //Modified by Chaiwat.i 25/08/2021
            /*DataTable mts = m_db.SelectSpotPlanMultipleSearch(m_strBuyingBriefID, "Media_Type");
            DataTable msts = m_db.SelectSpotPlanMultipleSearch(m_strBuyingBriefID, "Media_Sub_Type");
            string MediaTypeSearch = mts.Rows[0][0].ToString();
            string MediaSubTypeSearch = msts.Rows[0][0].ToString();*/
            if (!m_db.UpdateBuyingBrief(m_strBuyingBriefID, m_iStatus, Convert.ToDouble(txtActualBudget.Text)))
                return false;

            if (!m_db.UpdateBuyingBriefMarket(m_strBuyingBriefID, m_iStatus, Convert.ToDouble(txtActualBudget.Text), dt.Rows.Count))
                return false;

            DataTable dtSpotplanEdit = (DataTable)gvSpotPlanEdit.DataSource;
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
            Implementation_SpotPlan_ES_Amendment frm;
            if (m_MasterMediaType == "")
                frm = new Implementation_SpotPlan_ES_Amendment(screenMode, m_strMediaType);
            else
                frm = new Implementation_SpotPlan_ES_Amendment(screenMode, m_strMediaType, m_MasterMediaType);

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
                int item = GetNextItem();
                dr["Item"] = item;
                dr["Adept_Code"] = item;

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

                dr["Package"] = frm.txtSpotType.Text;

                dr["Material_ID"] = frm.m_strMaterialID;
                dr["Material_Key"] = frm.txtMaterialCode.Text;
                dr["MaterialName"] = frm.txtMaterial.Text;

                dr["Show_Date"] = frm.dtShowDate.Value.ToString("yyyyMMdd");
                dr["ShowDate"] = frm.dtShowDate.Value.ToString("dd/MM/yyyy");
                dr["Start_Time"] = "";
                dr["End_Time"] = "";
                dr["Spots"] = frm.txtQuantity.Value;
                dr["Rate"] = frm.txtTotalCost.Value;
                dr["Discount"] = frm.txtDiscount.Value;
                dr["Net_Cost"] = frm.txtNetCost.Value;
                dr["Spending"] = frm.txtNetCost.Value * frm.txtQuantity.Value;
                dr["Agency_Fee"] = frm.txtAgencyFeePercent.Value / 100;
                dr["AgencyFeePercent"] = frm.txtAgencyFeePercent.Value;
                dr["AgencyFee"] = (frm.txtNetCost.Value * frm.txtQuantity.Value) * (frm.txtAgencyFeePercent.Value / 100);
                if (m_bCreateAdviceNote)
                {
                    if (!dr["Media_ID"].Equals(drCloneOriginal["Media_ID"])
                        || !dr["Media_Vendor_ID"].Equals(drCloneOriginal["Media_Vendor_ID"])
                        || !dr["Program"].Equals(drCloneOriginal["Program"])
                        || !dr["Package"].Equals(drCloneOriginal["Package"])
                        || !dr["Deadline_Terminate"].Equals(drCloneOriginal["Deadline_Terminate"])
                        || !dr["Show_Date"].Equals(drCloneOriginal["Show_Date"])
                        || !dr["Material_Key"].Equals(drCloneOriginal["Material_Key"])
                        || !dr["Spots"].Equals(drCloneOriginal["Spots"])
                        || !dr["Rate"].Equals(drCloneOriginal["Rate"])
                        || !dr["Discount"].Equals(drCloneOriginal["Discount"])
                        || !dr["Net_Cost"].Equals(drCloneOriginal["Net_Cost"])
                        || !dr["AgencyFeePercent"].Equals(drCloneOriginal["AgencyFeePercent"])
                        )
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

                        PackingSpotPlanEdit(drSpotPlanEdit_Del, drCloneOriginal, iTeam, "Del");
                        PackingSpotPlanEdit(drSpotPlanEdit_Add, dr, iTeam, "Add");

                        bool bChangeExist = false;
                        for (int i = 0; i < dtSpotPlanEdit.Rows.Count; i++)
                        {
                            DataRow drTemp = dtSpotPlanEdit.Rows[i];
                            if (drTemp["Spot_Plan_Id"].ToString() == dr["Spot_Plan_Id"].ToString()
                                && drTemp["Kind"].ToString() == "Add")
                            {
                                //iTeam = Convert.ToInt32(drTemp["Team"]);
                                bChangeExist = true;
                                drSpotPlanEdit_Add["Item"] = Convert.ToInt32(drTemp["Item"]);
                                drSpotPlanEdit_Add["Team"] = Convert.ToInt32(drTemp["Team"]);
                                dr["Item"] = Convert.ToInt32(drTemp["Item"]);
                                dtSpotPlanEdit.Rows.Remove(drTemp);
                                break;
                            }
                        }

                        if (bChangeExist == false)
                        {
                            ((DataTable)gvSpotPlanEdit.DataSource).Rows.Add(drSpotPlanEdit_Del);
                        }
                        ((DataTable)gvSpotPlanEdit.DataSource).Rows.Add(drSpotPlanEdit_Add);
                    }
                }
                RefreshBudget();
            }
        }

        private void PackingSpotPlanEdit(DataRow drTo, DataRow drFrom, int iTeam, string strKind)
        {
            drTo["Buying_Brief_ID"] = drFrom["Buying_Brief_ID"];
            drTo["Market_ID"] = "THAILAND";
            drTo["Version"] = drFrom["Version"];
            drTo["Item"] = drFrom["Item"];
            drTo["Show_Date"] = drFrom["Show_Date"];
            drTo["ShowDate"] = drFrom["ShowDate"];
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

                if (m_bCreateAdviceNote)
                {
                    DataTable dtSpotPlanEdit = ((DataTable)gvSpotPlanEdit.DataSource);
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

                    DataRow drSpotPlanEdit_Del = ((DataTable)gvSpotPlanEdit.DataSource).NewRow();
                    DataRow dr = ((DataRowView)gvDetail.Rows[e.RowIndex].DataBoundItem).Row;

                    if (dr.RowState == DataRowState.Added
                        || dr.RowState == DataRowState.Modified)
                    {
                        for (int i = 0; i < dtSpotPlanEdit.Rows.Count; i++)
                        {
                            DataRow drSPEdit = dtSpotPlanEdit.Rows[i];
                            if (drSPEdit["Item"].Equals(dr["Item"]))
                                dtSpotPlanEdit.Rows.Remove(drSPEdit);
                        }
                    }
                    else //Unchange
                    {
                        for (int i = 0; i < dtSpotPlanEdit.Rows.Count; i++)
                        {
                            DataRow drSPEdit = dtSpotPlanEdit.Rows[i];
                            if (drSPEdit["Item"].Equals(dr["Item"]) && drSPEdit["Kind"].ToString() == "Add")
                                iTeam = Convert.ToInt32(drSPEdit["Team"].ToString());
                        }
                        PackingSpotPlanEdit(drSpotPlanEdit_Del, dr, iTeam, "Del");
                        dtSpotPlanEdit.Rows.Add(drSpotPlanEdit_Del);
                    }
                }
                gvDetail.Rows.Remove(gvDetail.Rows[e.RowIndex]);
                RefreshBudget();
            }
        }

        private void btnAddDetail_Click(object sender, EventArgs e)
        {
            Implementation_SpotPlan_ES_Amendment frm;
            if (m_MasterMediaType == "")
                frm = new Implementation_SpotPlan_ES_Amendment("Add", m_strMediaType);
            else
                frm = new Implementation_SpotPlan_ES_Amendment("Add", m_strMediaType, m_MasterMediaType);

            frm.BuyingBriefID = txtBuyingBriefNo.Text;
            frm.Product_ID = txtProductCode.Text;
            frm.Client_ID = txtClientCode.Text;
            frm.txtAgencyFeePercent.Value = m_dAgencyFee * 100;
            frm.CampaignStartDate = m_strStartDate;
            frm.CampaignEndDate = m_strEndDate;
            DataRow drAddDetail = ((DataTable)gvDetail.DataSource).NewRow();
            drAddDetail["Buying_Brief_ID"] = txtBuyingBriefNo.Text;
            frm.drInput = drAddDetail;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                int item = GetNextItem();
                drAddDetail["Item"] = item;
                drAddDetail["AdeptMediaTypeName"] = frm.AdeptMediaType;
                drAddDetail["MediaTypeName"] = frm.txtMediaType.Text;
                drAddDetail["Media_Type"] = frm.txtMediaTypeCode.Text;
                drAddDetail["MediaSubTypeName"] = frm.txtMediaSubType.Text;
                drAddDetail["Media_Sub_Type"] = frm.txtMediaSubTypeCode.Text;
                drAddDetail["MediaName"] = frm.txtMedia.Text;
                drAddDetail["Media_ID"] = frm.txtMediaCode.Text;
                drAddDetail["MediaVendorName"] = frm.txtVendor.Text;
                drAddDetail["Media_Vendor_ID"] = frm.txtVendorCode.Text;
                drAddDetail["Program"] = frm.txtDescription.Text.Replace(",", ";").Replace("\"", " ").Replace("'", "`");

                drAddDetail["BuyTypeID"] = frm.txtBuyTypeCode.Text == "" ? "AG" : frm.txtBuyTypeCode.Text;
                drAddDetail["BuyTypeName"] = frm.m_strBuyTypeName == "" ? "By Agency" : frm.m_strBuyTypeName;
                drAddDetail["Deadline_Terminate"] = frm.txtBuyType.Text == "" ? "By Agency" : frm.txtBuyType.Text;

                drAddDetail["Package"] = frm.txtSpotType.Text;

                drAddDetail["Material_ID"] = frm.m_strMaterialID;
                drAddDetail["Material_Key"] = frm.txtMaterialCode.Text;
                drAddDetail["MaterialName"] = frm.txtMaterial.Text;

                drAddDetail["Show_Date"] = frm.dtShowDate.Value.ToString("yyyyMMdd");
                drAddDetail["ShowDate"] = frm.dtShowDate.Value.ToString("dd/MM/yyyy");

                drAddDetail["Spots"] = frm.txtQuantity.Value;

                drAddDetail["Rate"] = frm.txtTotalCost.Value;
                drAddDetail["Discount"] = frm.txtDiscount.Value;
                drAddDetail["Net_Cost"] = frm.txtNetCost.Value;
                drAddDetail["Spending"] = frm.txtNetCost.Value * frm.txtQuantity.Value;
                drAddDetail["Agency_Fee"] = frm.txtAgencyFeePercent.Value / 100;
                drAddDetail["AgencyFeePercent"] = frm.txtAgencyFeePercent.Value;
                drAddDetail["AgencyFee"] = (frm.txtNetCost.Value * frm.txtQuantity.Value) * (frm.txtAgencyFeePercent.Value / 100);

                //Defaul for NULL Value 
                drAddDetail["Start_Time"] = "";
                drAddDetail["End_Time"] = "";
                drAddDetail["SizeHW"] = "";
                drAddDetail["Length"] = 0;
                drAddDetail["Unit"] = 1;
                drAddDetail["Spending_Type"] = frm.m_iSpendingType;
                drAddDetail["Market_ID"] = "THAILAND";
                drAddDetail["ID"] = 1;
                drAddDetail["Version"] = m_strVersion;
                drAddDetail["Status"] = m_iStatus;
                drAddDetail["Weight"] = 100;
                drAddDetail["Include_Media_Cost"] = true; //1
                drAddDetail["Market_Price"] = 1;
                drAddDetail["Adept_Code"] = item;
                drAddDetail["Adept_Export"] = 0;
                drAddDetail["Adept_Hide"] = 0;
                drAddDetail["Appear"] = 0;
                drAddDetail["Finish"] = 0;
                drAddDetail["Pkg"] = 0;
                drAddDetail["Program_Code"] = 0;
                drAddDetail["Surcharge"] = 0;
                drAddDetail["Bonus_Percent"] = 0;
                drAddDetail["Bonus_Cost"] = 0;
                drAddDetail["Province"] = "THAILAND";
                drAddDetail["CD_Percent"] = 0;
                drAddDetail["OMI"] = false;
                drAddDetail["Verified"] = false;
                drAddDetail["CH7_export"] = false;
                drAddDetail["Spot_verified"] = false;
                drAddDetail["Spot_Plan_ID"] = 0;

                ((DataTable)gvDetail.DataSource).Rows.Add(drAddDetail);
                if (m_bCreateAdviceNote)
                {
                    DataTable dtSpotPlanEdit = ((DataTable)gvSpotPlanEdit.DataSource);
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
                    PackingSpotPlanEdit(drSpotPlanEdit_Add, drAddDetail, iTeam, "Add");
                    dtSpotPlanEdit.Rows.Add(drSpotPlanEdit_Add);
                }
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
                DataSet ds = m_db.SelectSpotPlanEditForAdviceNoteES(m_strBuyingBriefID, m_strVersion, dtStamp, showX);
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
                ExcelUtil.ExcelSetValueString(workSheet, "B", 6, dt.Rows[0]["Agency_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 7, dt.Rows[0]["Buying_Brief_ID"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 8, dt.Rows[0]["Campaign_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 9, dt.Rows[0]["Campaign_Period"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 10, dt.Rows[0]["Product_Name"].ToString());
                ExcelUtil.ExcelSetValueString(workSheet, "B", 11, dt.Rows[0]["Material_Key"].ToString());

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
                dtAdviceNote.Columns.Add("OriSche");
                dtAdviceNote.Columns.Add("OriM");
                dtAdviceNote.Columns.Add("OriQuan");
                dtAdviceNote.Columns.Add("OriBuy");
                dtAdviceNote.Columns.Add("OriNet");
                dtAdviceNote.Columns.Add("OriFeePercent");
                dtAdviceNote.Columns.Add("OriFee");

                dtAdviceNote.Columns.Add("NewMedia");
                dtAdviceNote.Columns.Add("NewDesc");
                dtAdviceNote.Columns.Add("NewSche");
                dtAdviceNote.Columns.Add("NewM");
                dtAdviceNote.Columns.Add("NewQuan");
                dtAdviceNote.Columns.Add("NewBuy");
                dtAdviceNote.Columns.Add("NewNet");
                dtAdviceNote.Columns.Add("NewFeePercent");
                dtAdviceNote.Columns.Add("NewFee");
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
                            dr["OriDesc"] = drOriFill[i]["Program"];
                            dr["OriSche"] = drOriFill[i]["ShowDate"];
                            dr["OriM"] = drOriFill[i]["Material_Key"];
                            dr["OriQuan"] = drOriFill[i]["Spots"];
                            dr["OriBuy"] = drOriFill[i]["BuyTypeName"];
                            dr["OriNet"] = drOriFill[i]["Net_Cost"];
                            dr["OriFeePercent"] = drOriFill[i]["AgencyFeePercent"];
                            dr["OriFee"] = drOriFill[i]["AgencyFee"];
                            dr["Remark"] = drOriFill[i]["Remark"];
                        }
                        if (i < drNewFill.Length)
                        {
                            dr["NewMedia"] = drNewFill[i]["MediaName"];
                            dr["NewDesc"] = drNewFill[i]["Program"];
                            dr["NewSche"] = drNewFill[i]["ShowDate"];
                            dr["NewM"] = drNewFill[i]["Material_Key"];
                            dr["NewQuan"] = drNewFill[i]["Spots"];
                            dr["NewBuy"] = drNewFill[i]["BuyTypeName"];
                            dr["NewNet"] = drNewFill[i]["Net_Cost"];
                            dr["NewFeePercent"] = drNewFill[i]["AgencyFeePercent"];
                            dr["NewFee"] = drNewFill[i]["AgencyFee"];
                            dr["Remark"] = drNewFill[i]["Remark"];
                        }
                        dtAdviceNote.Rows.Add(dr);
                    }
                }

                if (dtAdviceNote.Rows.Count > 3)
                {
                    workSheet.Rows[15].Insert(Excel.XlInsertShiftDirection.xlShiftDown, workSheet.Rows[15].Copy(Type.Missing));
                }
                ExcelUtil.ExcelSetValueStringFullTableAdviceNote(workSheet, 15, 1, 4, dtAdviceNote);

                //=======================================
                // Client Direct Pay
                //=======================================
                if (bDirectPay)
                {
                    dt = ds.Tables[2];
                    DataTable dtDirectPay = new DataTable();
                    dtDirectPay.Columns.Add("OriMedia");
                    dtDirectPay.Columns.Add("OriDesc");
                    dtDirectPay.Columns.Add("OriSche");
                    dtDirectPay.Columns.Add("OriM");
                    dtDirectPay.Columns.Add("OriQuan");
                    dtDirectPay.Columns.Add("OriBuy");
                    dtDirectPay.Columns.Add("OriNet");
                    dtDirectPay.Columns.Add("OriVendor");
                    dtDirectPay.Columns.Add("OriSpace");

                    dtDirectPay.Columns.Add("NewMedia");
                    dtDirectPay.Columns.Add("NewDesc");
                    dtDirectPay.Columns.Add("NewSche");
                    dtDirectPay.Columns.Add("NewM");
                    dtDirectPay.Columns.Add("NewQuan");
                    dtDirectPay.Columns.Add("NewBuy");
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
                                dr["OriDesc"] = drOriFill[i]["Program"];
                                dr["OriSche"] = drOriFill[i]["ShowDate"];
                                dr["OriM"] = drOriFill[i]["Material_Key"];
                                dr["OriQuan"] = drOriFill[i]["Spots"];
                                dr["OriBuy"] = drOriFill[i]["BuyTypeName"];
                                dr["OriNet"] = drOriFill[i]["Net_Cost"];
                                dr["OriVendor"] = drOriFill[i]["MediaVendorName"];
                                dr["OriSpace"] = "";
                                dr["Remark"] = drOriFill[i]["Remark"];
                            }
                            if (i < drNewFill.Length)
                            {
                                dr["NewMedia"] = drNewFill[i]["MediaName"];
                                dr["NewDesc"] = drNewFill[i]["Program"];
                                dr["NewSche"] = drNewFill[i]["ShowDate"];
                                dr["NewM"] = drNewFill[i]["Material_Key"];
                                dr["NewQuan"] = drNewFill[i]["Spots"];
                                dr["NewBuy"] = drNewFill[i]["BuyTypeName"];
                                dr["NewNet"] = drNewFill[i]["Net_Cost"];
                                dr["NewVendor"] = drNewFill[i]["MediaVendorName"];
                                dr["NewSpace"] = "";
                                dr["Remark"] = drNewFill[i]["Remark"];
                            }
                            dtDirectPay.Rows.Add(dr);
                        }
                    }
                    int rowDP = 33;
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

        private void Implementation_SpotPlan_ES_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Modified by Chaiwat.i 25/08/2021
            //m_db.DeleteSpotPlanSavingStatus(txtBuyingBriefNo.Text, m_strVersion, username, "Spot_Plan");
            m_db.DeleteSpotPlanSavingStatus(txtBuyingBriefNo.Text, m_strVersion, username, "Spot Plan");
        }
    }
}
