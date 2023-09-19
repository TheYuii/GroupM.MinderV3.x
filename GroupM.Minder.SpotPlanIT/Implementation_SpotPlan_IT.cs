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

namespace GroupM.Minder.SpotPlanIT
{
    public partial class Implementation_SpotPlan_IT : Form
    {

        DBManager m_db = null;
        private string username;
        private string screenmode;

        private decimal m_dAgencyFee = 0;
        private string m_strBuyingBriefID;
        private string m_strVersion;
        private string m_strMediaType;
        private int m_iStatus;

        string m_strStartDate = null;
        string m_strEndDate = null;

        string m_Edit_Date = "";
        string m_Edit_Time = "";


        string m_strScheduleExportPath = ConfigurationManager.AppSettings["ReportMinder"] + "Schedule_IT.xlt";
        string m_strScheduleDirectPayExportPath = ConfigurationManager.AppSettings["ReportMinder"] + "Schedule_IT_Pay2Vendor.xlt";
        string m_strPOExportPath = ConfigurationManager.AppSettings["ReportMinder"] + "PO_IT.xltx";
        string m_strAdviceNoteExportPath = ConfigurationManager.AppSettings["ReportMinder"] + "AdviseNote_IT.xlt";
        string m_strAdviceNoteDirectPayExportPath = ConfigurationManager.AppSettings["ReportMinder"] + "AdviseNote_IT_Pay2Vendor.xlt";

        enum eScreenMode { Add, Edit, View }
        eScreenMode m_screenMode = eScreenMode.Edit;
        enum eCol { MediaTpe, MediaSubTpe, Media, Vendor, Desc, StartDate, EndDate, Material, Impression, GRP, CPRP, TotalCost, Disc, NetCost, AgencyFeePercent, AgencyFee }

        public Implementation_SpotPlan_IT(string Username, string ScreenPermission, string BB, string v, int s, string mt)
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
                case eScreenMode.Add:
                    break;
                case eScreenMode.Edit:
                    break;
                case eScreenMode.View:
                    btnAddDetail.Visible = false;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    txtPORemark.ReadOnly = true;
                    txtScheduleRemark.ReadOnly = true;
                    break;
            }
            m_screenMode = mode;
        }

        private void InitailControl()
        {
            ControlManipulate.ClearControl(txtHearder);
            m_Edit_Date = DateTime.Now.ToString("yyyyMMdd");
            m_Edit_Time = DateTime.Now.ToString("HHmmss");

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

            DataTable dt = m_db.SelectSpotPlan(m_strBuyingBriefID, m_strVersion);
            DataTable dtversion = m_db.SelectSpotPlanVersion(m_strBuyingBriefID, m_strVersion);
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

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F8 || keyData == Keys.Enter)
            {
                DataLoading();
                return true;
            }
            if (keyData == (Keys.Control | Keys.R))
            {
                InitailControl();
                DataLoading();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SpotPlan_Load(object sender, EventArgs e)
        {
            InitailControl();
            DataLoading();
            //if (m_screenMode == eScreenMode.Edit)
            //{
            //    m_db.InsertSpotPlanSavingStatus(txtBuyingBriefNo.Text, m_strVersion, username, DateTime.Now.ToString("yyyyMMddhhmmss"), "Spot_Plan");
            //}
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
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
            if (!m_db.UpdateSpotPlanRemark(m_strBuyingBriefID, txtScheduleRemark.Text, txtPORemark.Text))
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
            DataTable mts = m_db.SelectSpotPlanMultipleSearch(m_strBuyingBriefID, "Media_Type");
            DataTable msts = m_db.SelectSpotPlanMultipleSearch(m_strBuyingBriefID, "Media_Sub_Type");
            string MediaTypeSearch = mts.Rows[0][0].ToString();
            string MediaSubTypeSearch = msts.Rows[0][0].ToString();
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
            return true;

        }

        private bool CheckDataBeforeSave()
        {
            bool valid = true;
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
            return valid;
        }

        private void EditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gvDetail.Rows.Count == 0)
                return;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataRow dr = ((DataRowView)gvDetail.Rows[gvDetail.SelectedRows[0].Index].DataBoundItem).Row;
                Master_Vendor frm = new Master_Vendor(screenmode, username);
                frm.drData = dr;
                if (frm.ShowDialog() == DialogResult.Yes)
                {
                    DataLoading();
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
            if (e.RowIndex < 0 || m_iStatus > 5)
            {
                return;
            }
            DataRow dr = ((DataRowView)gvDetail.Rows[e.RowIndex].DataBoundItem).Row;
            Implementation_SpotPlan_IT_Amendment frm = new Implementation_SpotPlan_IT_Amendment("Edit", m_strMediaType);
            frm.Client_ID = txtClientCode.Text;
            frm.CampaignStartDate = m_strStartDate;
            frm.CampaignEndDate = m_strEndDate;
            frm.drInput = dr;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                DataRow drCloneOriginal = ((DataTable)gvDetail.DataSource).NewRow();
                drCloneOriginal.ItemArray = dr.ItemArray;

                //Update
                int item = GetNextItem();
                dr["Item"] = item;
                dr["Adept_Code"] = item;

                dr["MediaTypeName"] = frm.txtMediaType.Text;
                dr["Media_Type"] = frm.txtMediaTypeCode.Text;
                dr["MediaSubTypeName"] = frm.txtMediaSubType.Text;
                dr["Media_Sub_Type"] = frm.txtMediaSubTypeCode.Text;
                dr["MediaName"] = frm.txtMedia.Text;
                dr["Media_ID"] = frm.txtMediaCode.Text;
                dr["MediaVendorName"] = frm.txtVendor.Text;
                dr["Media_Vendor_ID"] = frm.txtVendorCode.Text;
                dr["Program"] = frm.txtDescription.Text;

                dr["BuyTypeID"] = frm.txtBuyTypeCode.Text;
                dr["BuyTypeName"] = frm.m_strBuyTypeName;
                dr["Deadline_Terminate"] = frm.txtBuyType.Text;

                dr["Material_ID"] = frm.m_strMaterialID;
                dr["Material_Key"] = frm.txtMaterialCode.Text;
                dr["MaterialName"] = frm.txtMaterial.Text;

                dr["Start_Date"] = frm.dtStartDate.Value.ToString("yyyyMMdd");
                dr["End_Date"] = frm.dtEndDate.Value.ToString("yyyyMMdd");
                dr["StartDate"] = frm.dtStartDate.Value.ToString("dd/MM/yyyy");
                dr["EndDate"] = frm.dtEndDate.Value.ToString("dd/MM/yyyy");
                dr["Start_Time"] = "0600";
                dr["End_Time"] = "2400";
                dr["Market_Price"] = frm.txtImpression.Value;
                //dr["Rating"] = frm.txtGRP.Value;
                //dr["CPRP_Cost"] = frm.txtCPRP.Value;
                dr["Rate"] = frm.txtTotalCost.Value;
                dr["Discount"] = frm.txtDiscount.Value;
                dr["Net_Cost"] = frm.txtNetCost.Value;
                dr["Agency_Fee"] = frm.txtAgencyFeePercent.Value / 100;
                dr["AgencyFeePercent"] = frm.txtAgencyFeePercent.Value;
                dr["AgencyFee"] = frm.txtNetCost.Value * (frm.txtAgencyFeePercent.Value / 100);
                if (m_bCreateAdviceNote)
                {
                    if (!dr["Media_ID"].Equals(drCloneOriginal["Media_ID"])
                        || !dr["Media_Vendor_ID"].Equals(drCloneOriginal["Media_Vendor_ID"])
                        || !dr["Program"].Equals(drCloneOriginal["Program"])
                        || !dr["Start_Date"].Equals(drCloneOriginal["Start_Date"])
                        || !dr["End_Date"].Equals(drCloneOriginal["End_Date"])
                        || !dr["Deadline_Terminate"].Equals(drCloneOriginal["Deadline_Terminate"])
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
            Implementation_SpotPlan_Print_Schedule frm = new Implementation_SpotPlan_Print_Schedule(m_strBuyingBriefID, m_strVersion, m_strStartDate, m_strEndDate, m_strScheduleExportPath, m_strScheduleDirectPayExportPath, m_strMediaType, "");
            //Implementation_SpotPlan_IT_PrintSC frm = new Implementation_SpotPlan_IT_PrintSC(m_strBuyingBriefID, m_strVersion, m_strStartDate, m_strEndDate, m_strScheduleExportPath);
            frm.Show();
        }

        private void PurchaseOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Implementation_SpotPlan_Print_PO frm = new Implementation_SpotPlan_Print_PO(m_strBuyingBriefID, m_strVersion, m_strStartDate, m_strEndDate, m_strPOExportPath, m_strMediaType, "");
            frm.Show();
        }


        private void gvDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (!(e.RowIndex != -1
                && m_iStatus != 4
                && m_iStatus != 8))
            {
                return;
            }
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
            }
        }

        private void btnAddDetail_Click(object sender, EventArgs e)
        {
            Implementation_SpotPlan_IT_Amendment frm = new Implementation_SpotPlan_IT_Amendment("Add", m_strMediaType);
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
                drAddDetail["MediaTypeName"] = frm.txtMediaType.Text;
                drAddDetail["Media_Type"] = frm.txtMediaTypeCode.Text;
                drAddDetail["MediaSubTypeName"] = frm.txtMediaSubType.Text;
                drAddDetail["Media_Sub_Type"] = frm.txtMediaSubTypeCode.Text;
                drAddDetail["MediaName"] = frm.txtMedia.Text;
                drAddDetail["Media_ID"] = frm.txtMediaCode.Text;
                drAddDetail["MediaVendorName"] = frm.txtVendor.Text;
                drAddDetail["Media_Vendor_ID"] = frm.txtVendorCode.Text;
                drAddDetail["Program"] = frm.txtDescription.Text;

                drAddDetail["BuyTypeID"] = frm.txtBuyTypeCode.Text;
                drAddDetail["BuyTypeName"] = frm.m_strBuyTypeName;
                drAddDetail["Deadline_Terminate"] = frm.txtBuyType.Text;

                drAddDetail["Material_ID"] = frm.m_strMaterialID;
                drAddDetail["Material_Key"] = frm.txtMaterialCode.Text;
                drAddDetail["MaterialName"] = frm.txtMaterial.Text;

                drAddDetail["Show_Date"] = frm.dtStartDate.Value.ToString("yyyyMMdd");
                drAddDetail["Start_Date"] = frm.dtStartDate.Value.ToString("yyyyMMdd");
                drAddDetail["End_Date"] = frm.dtEndDate.Value.ToString("yyyyMMdd");
                drAddDetail["StartDate"] = frm.dtStartDate.Value.ToString("dd/MM/yyyy");
                drAddDetail["EndDate"] = frm.dtEndDate.Value.ToString("dd/MM/yyyy");
                drAddDetail["Start_Time"] = "0600";
                drAddDetail["End_Time"] = "2400";

                drAddDetail["Market_Price"] = frm.txtImpression.Value;
                //drAddDetail["Rating"] = frm.txtGRP.Value;
                //drAddDetail["CPRP_Cost"] = frm.txtCPRP.Value;

                drAddDetail["Rate"] = frm.txtTotalCost.Value;
                drAddDetail["Discount"] = frm.txtDiscount.Value;
                drAddDetail["Net_Cost"] = frm.txtNetCost.Value;
                drAddDetail["Agency_Fee"] = frm.txtAgencyFeePercent.Value / 100;
                drAddDetail["AgencyFeePercent"] = frm.txtAgencyFeePercent.Value;
                drAddDetail["AgencyFee"] = frm.txtNetCost.Value * (frm.txtAgencyFeePercent.Value / 100);

                //Defaul for NULL Value 
                drAddDetail["Package"] = "";
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
                drAddDetail["Spots"] = 1;
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
                drAddDetail["BuyTypeName"] = "By Agency";
                drAddDetail["BuyTypeID"] = "AG";
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

                bool bDirectPay = m_db.CheckDirectPayAdviceNote(m_strBuyingBriefID, m_strVersion, selectMedia);
                DataSet ds = m_db.SelectSpotPlanEditForAdviceNoteIT(m_strBuyingBriefID, m_strVersion, dtStamp, true);
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
                // Spot Plan Edit
                //=======================================
                dt = ds.Tables[1];
                DataTable dtAdviceNote = new DataTable();
                dtAdviceNote.Columns.Add("OriMedia");
                dtAdviceNote.Columns.Add("OriBuyType");
                dtAdviceNote.Columns.Add("OriSche");
                dtAdviceNote.Columns.Add("OriNet");
                dtAdviceNote.Columns.Add("OriFeePercent");
                dtAdviceNote.Columns.Add("OriFee");

                dtAdviceNote.Columns.Add("NewMedia");
                dtAdviceNote.Columns.Add("NewBuyType");
                dtAdviceNote.Columns.Add("NewSche");
                dtAdviceNote.Columns.Add("NewNet");
                dtAdviceNote.Columns.Add("NewFeePercent");
                dtAdviceNote.Columns.Add("NewFee");
                dtAdviceNote.Columns.Add("Remark");

                var gpTeam = dt.AsEnumerable()
                               .GroupBy(row => new
                               {
                                   Team = row.Field<string>("Team"),
                                   Edit_Date = row.Field<string>("Edit_Date")
                               }).Select(g => new { g.Key.Team, g.Key.Edit_Date, RowCount = g.Count() });
                foreach (var gp in gpTeam.ToList())
                {
                    DataRow[] drOriFill = dt.Select(string.Format("Kind = 'Del' and Team = '{0}' and Edit_Date = '{1}'", gp.Team, gp.Edit_Date));
                    DataRow[] drNewFill = dt.Select(string.Format("Kind = 'Add' and Team = '{0}' and Edit_Date = '{1}'", gp.Team, gp.Edit_Date));
                    int iMaxRow = drOriFill.Length > drNewFill.Length ? drOriFill.Length : drNewFill.Length;
                    for (int i = 0; i < iMaxRow; i++)
                    {
                        DataRow dr = dtAdviceNote.NewRow();
                        if (i < drOriFill.Length)
                        {
                            dr["OriMedia"] = drOriFill[i]["MediaName"];
                            dr["OriBuyType"] = drOriFill[i]["Deadline_Terminate"];
                            dr["OriSche"] = drOriFill[i]["StartDate"] + " - " + drOriFill[i]["EndDate"];
                            dr["OriNet"] = drOriFill[i]["Net_Cost"];
                            dr["OriFeePercent"] = drOriFill[i]["AgencyFeePercent"];
                            dr["OriFee"] = drOriFill[i]["AgencyFee"];
                            dr["Remark"] = drOriFill[i]["Remark"];
                        }
                        if (i < drNewFill.Length)
                        {
                            dr["NewMedia"] = drNewFill[i]["MediaName"];
                            dr["NewBuyType"] = drNewFill[i]["Deadline_Terminate"];
                            dr["NewSche"] = drNewFill[i]["StartDate"] + " - " + drNewFill[i]["EndDate"];
                            dr["NewNet"] = drNewFill[i]["Net_Cost"];
                            dr["NewFeePercent"] = drNewFill[i]["AgencyFeePercent"];
                            dr["NewFee"] = drNewFill[i]["AgencyFee"];
                            dr["Remark"] = drNewFill[i]["Remark"];
                        }
                        dtAdviceNote.Rows.Add(dr);
                    }
                }

                ExcelUtil.ExcelSetValueStringFullTableAdviceNote(workSheet, 14, 1, 4, dtAdviceNote);

                //=======================================
                // Client Direct Pay
                //=======================================
                if (bDirectPay)
                {
                    dt = ds.Tables[2];
                    dtAdviceNote = new DataTable();
                    dtAdviceNote.Columns.Add("OriMedia");
                    dtAdviceNote.Columns.Add("OriBuyType");
                    dtAdviceNote.Columns.Add("OriSche");
                    dtAdviceNote.Columns.Add("OriNet");
                    dtAdviceNote.Columns.Add("OriVendor");
                    dtAdviceNote.Columns.Add("OriSpace");

                    dtAdviceNote.Columns.Add("NewMedia");
                    dtAdviceNote.Columns.Add("NewBuyType");
                    dtAdviceNote.Columns.Add("NewSche");
                    dtAdviceNote.Columns.Add("NewNet");
                    dtAdviceNote.Columns.Add("NewVendor");
                    dtAdviceNote.Columns.Add("NewSpace");
                    dtAdviceNote.Columns.Add("Remark");

                    gpTeam = dt.AsEnumerable()
                                   .GroupBy(row => new
                                   {
                                       Team = row.Field<string>("Team"),
                                       Edit_Date = row.Field<string>("Edit_Date")
                                   }).Select(g => new { g.Key.Team, g.Key.Edit_Date, RowCount = g.Count() });
                    foreach (var gp in gpTeam.ToList())
                    {
                        DataRow[] drOriFill = dt.Select(string.Format("Kind = 'Del' and Team = '{0}' and Edit_Date = '{1}'", gp.Team, gp.Edit_Date));
                        DataRow[] drNewFill = dt.Select(string.Format("Kind = 'Add' and Team = '{0}' and Edit_Date = '{1}'", gp.Team, gp.Edit_Date));
                        int iMaxRow = drOriFill.Length > drNewFill.Length ? drOriFill.Length : drNewFill.Length;
                        for (int i = 0; i < iMaxRow; i++)
                        {
                            DataRow dr = dtAdviceNote.NewRow();
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
                            dtAdviceNote.Rows.Add(dr);
                        }
                    }
                    int rowDP = 32;
                    if (ds.Tables[1].Rows.Count > 4)
                    {
                        rowDP += ds.Tables[1].Rows.Count - 4;
                    }
                    ExcelUtil.ExcelSetValueStringFullTableAdviceNote(workSheet, rowDP, 1, 4, dtAdviceNote);
                }

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

        private void Implementation_SpotPlan_IT_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Modified by Chaiwat.i 25/08/2021
            //m_db.DeleteSpotPlanSavingStatus(txtBuyingBriefNo.Text, m_strVersion, username, "Spot_Plan");
            m_db.DeleteSpotPlanSavingStatus(txtBuyingBriefNo.Text, m_strVersion, username, "Spot Plan");
        }
    }
}
