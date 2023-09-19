using GroupM.DBAccess;
using GroupM.UTL;
using MProprietary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupM.Minder
{
    public partial class Master_Client : Form
    {
        enum eScreenMode { Add, Edit, View }
        public DataRow drClient { get; set; }

        private string username = "";

        private decimal oldAgencyFee = 0;

        private string masterClient = "";

        private Regex regex = new Regex(@"[^a-zA-Z\d\s]");

        eScreenMode m_screemMode = eScreenMode.Edit;

        DBManager m_db;

        public Master_Client(string screenMode, string Username)
        {
            username = Username;
            m_db = new DBManager();

            InitializeComponent();

            if (screenMode == "Add")
                SetScreenMode(eScreenMode.Add);
            else if (screenMode == "Edit")
                SetScreenMode(eScreenMode.Edit);
            else
                SetScreenMode(eScreenMode.View);
        }

        private void Dataloading()
        {
            DataTable dt = m_db.SelectClient(drClient["Client_ID"].ToString());
            DataRow dr = dt.Rows[0];

            txtClientCode.Text = dr["Client_ID"].ToString();
            txtDisplayName.Text = dr["Short_Name"].ToString();
            txtThaiName.Text = dr["Thai_Name"].ToString();
            txtEnglishName.Text = dr["English_Name"].ToString();
            chkMasterClient.Checked = dr["Master_Client"].ToString() == "1" ? true : false;
            if (dr["Master_Client"].ToString() == "1")
                txtMasterClient.Enabled = false;
            else
                txtMasterClient.Enabled = true;
            txtMasterClient.Text = dr["MasterClientName"].ToString();
            txtMasterClientCode.Text = dr["Group"].ToString();
            txtAgency.Text = dr["AgencyName"].ToString();
            txtAgencyCode.Text = dr["Agency_ID"].ToString();
            txtReportToAgency.Text = dr["ReportToAgencyName"].ToString();
            txtReportToAgencyCode.Text = dr["Report_to_Agency"].ToString();
            txtOffice.Text = dr["OfficeName"].ToString();
            txtOfficeCode.Text = dr["Office_ID"].ToString();
            txtCreativeAgency.Text = dr["CreativeAgencyName"].ToString();
            txtCreativeAgencyCode.Text = dr["Creative_Agency_ID"].ToString();
            txtAgencyComm.Value = Convert.ToDecimal(dr["Agency_Commission"]) * 100;
            txtSharedComm.Value = Convert.ToDecimal(dr["Creative_Agency_Commission"]) * 100;
            txtGPMClient.Text = dr["GPMName"].ToString();
            txtGPMClientCode.Text = dr["GPM_CLIENT_CODE"].ToString();
            txtSymphonyID.Text = dr["Sym_ClientUniqueId"].ToString();
            if (dr["InactiveClient"].ToString() != "1")
            {
                rdActive.Checked = true;
                rdInactive.Checked = false;
                DateTimeConvertUtil.DateTimeSetEmpty(dtInactiveDate);
            }
            else
            {
                rdActive.Checked = false;
                rdInactive.Checked = true;

                dtInactiveDate.Enabled = true;
                DateTimeConvertUtil.DateTimeRestoreFormat(dtInactiveDate);
                if (dr["InactiveDate"] == DBNull.Value)
                    DateTimeConvertUtil.DateTimeSetEmpty(dtInactiveDate);
                else
                    dtInactiveDate.Value = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(dr["InactiveDate"].ToString());
            }
            if (dr["RatingEngineId"] == DBNull.Value || dr["RatingEngineId"].ToString() == "1")
                cboRatingEngine.Text = "eTam Rating Engine";
            else
                cboRatingEngine.Text = "Pinergy Rating Engine";

            chkDirectClient.Checked = Convert.ToBoolean(dr["Direct_Client"]);
            chkSpecial.Checked = Convert.ToBoolean(dr["Special"]);
            chkMargin.Checked = Convert.ToBoolean(dr["Margin_Cost"]);
            chkRedClient.Checked = Convert.ToBoolean(dr["RED_Status"]);

            if (Convert.ToBoolean(dr["Opt_in_Signed"]))
            {
                chkOptInSigned.Checked = true;
                if (dr["Opt_in_StartDate"] == DBNull.Value || dr["Opt_in_EndDate"] == DBNull.Value)
                {
                    DateTimeConvertUtil.DateTimeRestoreFormat(dtOptInStartDate);
                    DateTimeConvertUtil.DateTimeRestoreFormat(dtOptInEndDate);
                }
                else
                {
                    dtOptInStartDate.Value = Convert.ToDateTime(dr["Opt_in_StartDate"]);
                    dtOptInEndDate.Value = Convert.ToDateTime(dr["Opt_in_EndDate"]);
                }
                btnAddProprietaryGroup.Enabled = true;
                btnOptInAdvanceSetting.Enabled = true;
            }
            else
            {
                chkOptInSigned.Checked = false;
                DateTimeConvertUtil.DateTimeSetEmpty(dtOptInStartDate);
                DateTimeConvertUtil.DateTimeSetEmpty(dtOptInEndDate);
                btnAddProprietaryGroup.Enabled = false;
                btnOptInAdvanceSetting.Enabled = false;
            }
            DataTable dtOptIn = m_db.SelectProprietaryByClient(dr["Client_ID"].ToString());
            gvOptIn.DataSource = dtOptIn;

            txtOptInNote.Text = dr["Opt_in_Note"].ToString();
            if (dr["Region"] != DBNull.Value && dr["Region"].ToString() != "")
                cboRegion.Text = dr["Region"].ToString();

            txtContractPerson.Text = dr["Contact"].ToString();
            txtOfficeTelNo.Text = dr["Tel"].ToString();
            txtFax.Text = dr["Fax"].ToString();

            cboBookingHeader.Text = dr["Booking_order_Header"].ToString();
            txtThaiAddress1.Text = dr["Address"].ToString();
            txtThaiAddress2.Text = dr["Thai_Address2"].ToString();
            txtThaiAddress3.Text = dr["Thai_Address3"].ToString();
            txtThaiAddress4.Text = dr["Thai_Address4"].ToString();

            txtEnglishAddress1.Text = dr["English_Address1"].ToString();
            txtEnglishAddress2.Text = dr["English_Address2"].ToString();
            txtEnglishAddress3.Text = dr["English_Address3"].ToString();

            DataTable dtBrand = m_db.SelectBrandByClient(dr["Client_ID"].ToString());
            gvBrand.DataSource = dtBrand;

            DataTable dtCategory = m_db.SelectCategoryByClient(dr["Client_ID"].ToString());
            gvCategory.DataSource = dtCategory;

            DataTable dtAgencyFee = m_db.SelectAgencyFeeByClient(dr["Client_ID"].ToString());
            gvAgencyFee.AutoGenerateColumns = false;
            gvAgencyFee.DataSource = dtAgencyFee;

            if (!chkMasterClient.Checked)
            {
                btnAddPeriodAudit.Enabled = false;
                gvAudit.Enabled = false;
                btnAddPeriodEPD.Enabled = false;
                gvEPD.Enabled = false;
                btnAddPeriodMediaCredit.Enabled = false;
                gvMediaCredit.Enabled = false;
                btnAddPeriodRebate.Enabled = false;
                gvRebate.Enabled = false;
                btnAddPeriodSAC.Enabled = false;
                gvSAC.Enabled = false;
            }

            DataTable dtAudit = m_db.SelectClientRedGreenPeriod(dr["Client_ID"].ToString(), "Client_Audit_Right");
            gvAudit.AutoGenerateColumns = false;
            gvAudit.DataSource = dtAudit;
            gvAudit.Columns[0].DefaultCellStyle.Format = @"dd/MM/yyyy";
            gvAudit.Columns[1].DefaultCellStyle.Format = @"dd/MM/yyyy";

            DataTable dtEPD = m_db.SelectClientRedGreenPeriod(dr["Client_ID"].ToString(), "Client_EPD");
            gvEPD.AutoGenerateColumns = false;
            gvEPD.DataSource = dtEPD;
            gvEPD.Columns[0].DefaultCellStyle.Format = @"dd/MM/yyyy";
            gvEPD.Columns[1].DefaultCellStyle.Format = @"dd/MM/yyyy";

            DataTable dtMediaCredit = m_db.SelectClientRedGreenPeriod(dr["Client_ID"].ToString(), "Client_Media_Credit");
            gvMediaCredit.AutoGenerateColumns = false;
            gvMediaCredit.DataSource = dtMediaCredit;
            gvMediaCredit.Columns[0].DefaultCellStyle.Format = @"dd/MM/yyyy";
            gvMediaCredit.Columns[1].DefaultCellStyle.Format = @"dd/MM/yyyy";

            DataTable dtRebate = m_db.SelectClientRedGreenPeriod(dr["Client_ID"].ToString(), "Client_Rebate");
            gvRebate.AutoGenerateColumns = false;
            gvRebate.DataSource = dtRebate;
            gvRebate.Columns[0].DefaultCellStyle.Format = @"dd/MM/yyyy";
            gvRebate.Columns[1].DefaultCellStyle.Format = @"dd/MM/yyyy";

            DataTable dtSAC = m_db.SelectClientRedGreenPeriod(dr["Client_ID"].ToString(), "Client_SAC");
            gvSAC.AutoGenerateColumns = false;
            gvSAC.DataSource = dtSAC;
            gvSAC.Columns[0].DefaultCellStyle.Format = @"dd/MM/yyyy";
            gvSAC.Columns[1].DefaultCellStyle.Format = @"dd/MM/yyyy";

            // store old agency fee
            oldAgencyFee = Convert.ToDecimal(dr["Agency_Commission"]) * 100;

            // store old master client
            masterClient = txtMasterClientCode.Text;

            if (m_screemMode == eScreenMode.View)
            {
                dtInactiveDate.Enabled = false;
                dtOptInStartDate.Enabled = false;
                dtOptInEndDate.Enabled = false;
                btnAddProprietaryGroup.Enabled = false;
            }
        }

        private void SetScreenMode(eScreenMode mode)
        {
            switch (mode)
            {
                case eScreenMode.Add:
                    btnAddProprietaryGroup.Enabled = false;
                    btnOptInAdvanceSetting.Visible = false;
                    btnAddPeriodAudit.Enabled = false;
                    btnAddPeriodEPD.Enabled = false;
                    btnAddPeriodMediaCredit.Enabled = false;
                    btnAddPeriodRebate.Enabled = false;
                    btnAddPeriodSAC.Enabled = false;
                    btnDelete.Visible = false;
                    break;
                case eScreenMode.Edit:
                    txtClientCode.ReadOnly = true;
                    btnOptInAdvanceSetting.Visible = false;
                    break;
                case eScreenMode.View:
                    txtClientCode.ReadOnly = true;
                    txtDisplayName.ReadOnly = true;
                    txtThaiName.ReadOnly = true;
                    txtEnglishName.ReadOnly = true;
                    chkMasterClient.Enabled = false;
                    txtMasterClient.ReadOnly = true;
                    txtAgency.ReadOnly = true;
                    txtReportToAgency.ReadOnly = true;
                    txtOffice.ReadOnly = true;
                    txtCreativeAgency.ReadOnly = true;
                    txtAgencyComm.Enabled = false;
                    txtSharedComm.Enabled = false;
                    txtGPMClient.ReadOnly = true;
                    txtSymphonyID.ReadOnly = true;
                    rdActive.Enabled = false;
                    rdInactive.Enabled = false;
                    cboRatingEngine.Enabled = false;
                    chkDirectClient.Enabled = false;
                    chkMargin.Enabled = false;
                    chkRedClient.Enabled = false;
                    chkSpecial.Enabled = false;
                    chkOptInSigned.Enabled = false;
                    gvOptIn.Enabled = false;
                    txtOptInNote.ReadOnly = true;
                    cboRegion.Enabled = false;
                    btnOptInAdvanceSetting.Visible = false;
                    txtContractPerson.ReadOnly = true;
                    cboBookingHeader.Enabled = false;
                    txtThaiAddress1.ReadOnly = true;
                    txtThaiAddress2.ReadOnly = true;
                    txtThaiAddress3.ReadOnly = true;
                    txtThaiAddress4.ReadOnly = true;
                    txtOfficeTelNo.ReadOnly = true;
                    txtFax.ReadOnly = true;
                    txtEnglishAddress1.ReadOnly = true;
                    txtEnglishAddress2.ReadOnly = true;
                    txtEnglishAddress3.ReadOnly = true;
                    btnBrand.Enabled = false;
                    gvBrand.Enabled = false;
                    btnCategory.Enabled = false;
                    gvCategory.Enabled = false;
                    btnAddPeriodAudit.Enabled = false;
                    gvAudit.Enabled = false;
                    btnAddPeriodEPD.Enabled = false;
                    gvEPD.Enabled = false;
                    btnAddPeriodMediaCredit.Enabled = false;
                    gvMediaCredit.Enabled = false;
                    btnAddPeriodRebate.Enabled = false;
                    gvRebate.Enabled = false;
                    btnAddPeriodSAC.Enabled = false;
                    gvSAC.Enabled = false;
                    saveToolStripMenuItem.Enabled = false;
                    btnDelete.Enabled = false;
                    break;
            }
            SetScreenPermission();
            m_screemMode = mode;
        }

        private void SetScreenPermission()
        {

        }

        private void Master_Client_Load(object sender, EventArgs e)
        {
            InitControl();
            if (m_screemMode == eScreenMode.Add)
            {
                DataTable dtAgencyFee = m_db.SelectAgencyFeeByClient("dummyclient");
                string AgencyFeeName = "Activation|Digital Biddable|Digital Non Biddable|eCommerce|Fee|Offline|OOH";
                string[] aAgencyFeeName = AgencyFeeName.Split('|');
                foreach (string FeeName in aAgencyFeeName)
                {

                    DataRow dr = dtAgencyFee.NewRow();
                    dr["Priority"] = "1";
                    dr["Agency_Fee"] = "0";
                    dr["Editable"] = false ;
                    dr["Agency_Fee_Set_Up_Name"] = "Media Type Group";
                    dr["Media_Type_Group"] = FeeName;
                    dr["Description"] = FeeName;
                    dtAgencyFee.Rows.Add(dr);
                }
                gvAgencyFee.AutoGenerateColumns = false;
                gvAgencyFee.DataSource = dtAgencyFee;


            }
            if (m_screemMode == eScreenMode.Edit || m_screemMode == eScreenMode.View)
                Dataloading();
        }

        private void InitControl()
        {
            DataTable dtBookingHeader = m_db.SelectBookingHeader();
            foreach (DataRow dr in dtBookingHeader.Rows)
            {
                cboBookingHeader.Items.Add(dr["Booking_order_Header"]);
            }

            DateTimeConvertUtil.DateTimeSetEmpty(dtInactiveDate);
            cboRatingEngine.Text = "eTam Rating Engine";
            DateTimeConvertUtil.DateTimeSetEmpty(dtOptInStartDate);
            DateTimeConvertUtil.DateTimeSetEmpty(dtOptInEndDate);
            DataTable dtOptIn = m_db.SelectProprietaryByClient("");
            gvOptIn.AutoGenerateColumns = false;
            gvOptIn.DataSource = dtOptIn;

            DataTable dtBrand = m_db.SelectBrandByClient("");
            gvBrand.AutoGenerateColumns = false;
            gvBrand.DataSource = dtBrand;

            DataTable dtCategory = m_db.SelectCategoryByClient("");
            gvCategory.AutoGenerateColumns = false;
            gvCategory.DataSource = dtCategory;

            DataTable dtAudit = m_db.SelectClientRedGreenPeriod("", "Client_Audit_Right");
            gvAudit.AutoGenerateColumns = false;
            gvAudit.DataSource = dtAudit;
            gvAudit.Columns[0].DefaultCellStyle.Format = @"dd/MM/yyyy";
            gvAudit.Columns[1].DefaultCellStyle.Format = @"dd/MM/yyyy";

            DataTable dtEPD = m_db.SelectClientRedGreenPeriod("", "Client_EPD");
            gvEPD.AutoGenerateColumns = false;
            gvEPD.DataSource = dtEPD;
            gvEPD.Columns[0].DefaultCellStyle.Format = @"dd/MM/yyyy";
            gvEPD.Columns[1].DefaultCellStyle.Format = @"dd/MM/yyyy";

            DataTable dtMediaCredit = m_db.SelectClientRedGreenPeriod("", "Client_Media_Credit");
            gvMediaCredit.AutoGenerateColumns = false;
            gvMediaCredit.DataSource = dtMediaCredit;
            gvMediaCredit.Columns[0].DefaultCellStyle.Format = @"dd/MM/yyyy";
            gvMediaCredit.Columns[1].DefaultCellStyle.Format = @"dd/MM/yyyy";

            DataTable dtRebate = m_db.SelectClientRedGreenPeriod("", "Client_Rebate");
            gvRebate.AutoGenerateColumns = false;
            gvRebate.DataSource = dtRebate;
            gvRebate.Columns[0].DefaultCellStyle.Format = @"dd/MM/yyyy";
            gvRebate.Columns[1].DefaultCellStyle.Format = @"dd/MM/yyyy";

            DataTable dtSAC = m_db.SelectClientRedGreenPeriod("", "Client_SAC");
            gvSAC.AutoGenerateColumns = false;
            gvSAC.DataSource = dtSAC;
            gvSAC.Columns[0].DefaultCellStyle.Format = @"dd/MM/yyyy";
            gvSAC.Columns[1].DefaultCellStyle.Format = @"dd/MM/yyyy";
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OnCommandSave())
                DialogResult = DialogResult.Yes;
        }

        private bool OnCommandSave()
        {
            if (!CheckDataBeforeSave())
                return false;
            try
            {
                string log = "";
                //For End Edit Checkbox
                if (gvAgencyFee.SelectedRows.Count > 0)
                {
                    btnAddSpecificFee.Focus();
                    gvAgencyFee.Rows[gvAgencyFee.SelectedRows[0].Index].Cells[0].Selected = true;
                }
                DataTable dtAgencyFee = (DataTable)gvAgencyFee.DataSource;
                if (m_screemMode == eScreenMode.Add)
                {
                    if (m_db.InsertClient(Packing()) == -1)
                        return false;
                    else
                        log = "Add Client ID:" + txtClientCode.Text + ",Name :" + txtDisplayName.Text + ",Agency:" + txtAgencyCode.Text + "(" + txtAgency.Text + "),Office:" + txtOfficeCode.Text + "(" + txtOffice.Text + "),CA:" + txtCreativeAgencyCode.Text + "(" + txtCreativeAgency.Text + "),AComm:" + (txtAgencyComm.Value / 100);

                    m_db.InsertLogMinder(username.Replace(".", ""), log, "Master File", username.Replace(".", ""));

                    foreach (DataRow dr in dtAgencyFee.Rows)
                    {
                        m_db.InsertClientAgencyFee(dr, txtClientCode.Text);
                        log = $"Add Client:{txtClientCode.Text}({txtDisplayName.Text}),Add Agency Fee Detail:{dr["Agency_Fee_Set_Up_Name"].ToString()}({dr["description"].ToString()}) Fee({dr["Agency_Fee"].ToString()}) Edit({dr["Editable"].ToString()})";
                        m_db.InsertLogMinder(username.Replace(".", ""), log, "Master File", username.Replace(".", ""), "Agency_Fee", "", dr["Agency_Fee"].ToString());
                    }
                }
                else
                {
                    //if (m_db.UpdateClient(Packing()) == -1)
                    //{
                    //    return false;
                    //}
                    //else
                    //{
                    //    if (txtAgencyComm.Value == oldAgencyFee)
                    //        log = "Modify Client ID:" + txtClientCode.Text + ",Name :" + txtDisplayName.Text + ",Agency:" + txtAgencyCode.Text + "(" + txtAgency.Text + "),Office:" + txtOfficeCode.Text + "(" + txtOffice.Text + "),CA:" + txtCreativeAgencyCode.Text + "(" + txtCreativeAgency.Text + "),AComm:" + (txtAgencyComm.Value / 100);
                    //    else
                    //        log = "Modify Client ID:" + txtClientCode.Text + ",Name :" + txtDisplayName.Text + ",Agency:" + txtAgencyCode.Text + "(" + txtAgency.Text + "),Office:" + txtOfficeCode.Text + "(" + txtOffice.Text + "),CA:" + txtCreativeAgencyCode.Text + "(" + txtCreativeAgency.Text + "),AComm: From " + oldAgencyFee.ToString("N4") + " To " + txtAgencyComm.Value.ToString("N4");
                    //}

                    m_db.UpdateClient(Packing());
                    log = "Modify Client ID:" + txtClientCode.Text + ",Name :" + txtDisplayName.Text + ",Agency:" + txtAgencyCode.Text + "(" + txtAgency.Text + "),Office:" + txtOfficeCode.Text + "(" + txtOffice.Text + "),CA:" + txtCreativeAgencyCode.Text + "(" + txtCreativeAgency.Text + ")";
                    m_db.InsertLogMinder(username.Replace(".", ""), log, "Master File", username.Replace(".", ""));

                    DataTable delRows = new DataView(dtAgencyFee, null, null, DataViewRowState.Deleted).ToTable();
                    foreach (DataRow dr in delRows.Rows)
                    {
                        m_db.DeleteClientAgencyFee(Convert.ToInt32(dr["Client_Agency_Fee_ID"]));
                        log = $"Modify Client:{txtClientCode.Text}({txtDisplayName.Text}),Delete Agency Fee Detail:{dr["Agency_Fee_Set_Up_Name"].ToString()}({dr["description"].ToString()}) Fee({dr["Agency_Fee"].ToString()}) Edit({dr["Editable"].ToString()})";
                        m_db.InsertLogMinder(username.Replace(".", ""), log, "Master File", username.Replace(".", ""), "Agency_Fee", dr["Agency_Fee"].ToString(), "");
                    }
                    foreach (DataRow dr in dtAgencyFee.Rows)
                    {
                        if (dr.RowState == DataRowState.Added)
                        {
                            m_db.InsertClientAgencyFee(dr, txtClientCode.Text);

                            log = $"Modify Client:{txtClientCode.Text}({txtDisplayName.Text}),Add Agency Fee Detail:{dr["Agency_Fee_Set_Up_Name"].ToString()}({dr["description"].ToString()}) Fee({dr["Agency_Fee"].ToString()}) Edit({dr["Editable"].ToString()})";
                            m_db.InsertLogMinder(username.Replace(".", ""), log, "Master File", username.Replace(".", ""), "Agency_Fee", "", dr["Agency_Fee"].ToString());
                        }
                        else if (dr.RowState == DataRowState.Modified)
                        {
                            m_db.UpdateClientAgencyFee(dr);
                            log = $"Modify Client:{txtClientCode.Text}({txtDisplayName.Text}),Update Agency Fee Detail:{dr["Agency_Fee_Set_Up_Name"].ToString()}({dr["description"].ToString()}) Fee({ dr["Agency_Fee", DataRowVersion.Original].ToString()}>{dr["Agency_Fee"].ToString()}) Edit({dr["Editable", DataRowVersion.Original].ToString()}>{dr["Editable"].ToString()})";
                            m_db.InsertLogMinder(username.Replace(".", ""), log, "Master File", username.Replace(".", ""), "Agency_Fee", dr["Agency_Fee", DataRowVersion.Original].ToString(), dr["Agency_Fee"].ToString());
                        }
                    }

                }
                m_db.UpdatePropreitroyClient((DataTable)gvOptIn.DataSource, txtClientCode.Text);
                m_db.UpdateBrandClient((DataTable)gvBrand.DataSource, txtClientCode.Text);
                m_db.UpdateCategoryClient((DataTable)gvCategory.DataSource, txtClientCode.Text, username);
                



                if (chkMasterClient.Checked)
                {
                    DateTime now = DateTime.Now;
                    m_db.UpdateClientRedGreenPeriod((DataTable)gvAudit.DataSource, "Client_Audit_Right", txtClientCode.Text, now, username);
                    m_db.UpdateClientRedGreenPeriod((DataTable)gvEPD.DataSource, "Client_EPD", txtClientCode.Text, now, username);
                    m_db.UpdateClientRedGreenPeriod((DataTable)gvMediaCredit.DataSource, "Client_Media_Credit", txtClientCode.Text, now, username);
                    m_db.UpdateClientRedGreenPeriod((DataTable)gvRebate.DataSource, "Client_Rebate", txtClientCode.Text, now, username);
                    m_db.UpdateClientRedGreenPeriod((DataTable)gvSAC.DataSource, "Client_SAC", txtClientCode.Text, now, username);
                    m_db.UpdateClientRGPeriodFromMaster(txtClientCode.Text, username);
                }
                else
                {
                    if (m_screemMode == eScreenMode.Add)
                        m_db.CopyClientRGPeriodFromMaster(txtClientCode.Text, txtMasterClientCode.Text, username);
                    else if (masterClient != txtMasterClientCode.Text)
                        m_db.CopyClientRGPeriodFromNewMaster(txtClientCode.Text, txtMasterClientCode.Text, username);
                }
                
                GMessage.MessageInfo("Save Completed");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private DataRow Packing()
        {
            DataTable dt = m_db.SelectClient("dummycolumn");
            DataRow dr = dt.NewRow();

            dr["Client_ID"] = txtClientCode.Text;
            dr["Thai_Name"] = txtThaiName.Text;
            dr["English_Name"] = txtEnglishName.Text;
            dr["Short_Name"] = txtDisplayName.Text;
            dr["Agency_ID"] = txtAgencyCode.Text;
            dr["Office_ID"] = txtOfficeCode.Text;
            dr["Creative_Agency_ID"] = txtCreativeAgencyCode.Text;
            dr["Group"] = txtMasterClientCode.Text;
            dr["Master_Client"] = chkMasterClient.Checked ? 1 : 0;
            dr["Contact"] = txtContractPerson.Text;
            dr["Tel"] = txtOfficeTelNo.Text;
            dr["Fax"] = txtFax.Text;
            dr["Booking_order_Header"] = cboBookingHeader.Text;
            dr["Address"] = txtThaiAddress1.Text;
            dr["Thai_Address2"] = txtThaiAddress2.Text;
            dr["Thai_Address3"] = txtThaiAddress3.Text;
            dr["Thai_Address4"] = txtThaiAddress4.Text;
            dr["English_Address1"] = txtEnglishAddress1.Text;
            dr["English_Address2"] = txtEnglishAddress2.Text;
            dr["English_Address3"] = txtEnglishAddress3.Text;
            dr["Agency_Commission"] = txtAgencyComm.Value / 100; // real
            dr["Special"] = chkSpecial.Checked ? 1 : 0; // bit
            dr["User_ID"] = username.Replace(".", "");
            dr["Modify_Date"] = DBNull.Value;
            dr["AC_Struct"] = 0;
            dr["Media_Fee"] = 0; // real
            dr["Client_Referrence_ID"] = txtClientCode.Text;
            dr["CA_Struct"] = 0;
            dr["Creative_Agency_Commission"] = txtSharedComm.Value / 100; // real
            dr["CA_Media_Fee"] = 0; // real
            dr["Client_Classification_ID"] = DBNull.Value;
            dr["IsNewClient"] = DBNull.Value;
            dr["CreateDate"] = DateTime.Now.ToString("yyyyMMdd");
            dr["InactiveClient"] = rdInactive.Checked ? 1 : 0;
            if (rdInactive.Checked)
            {
                dr["InactiveDate"] = dtInactiveDate.Value.ToString("yyyyMMdd");
            }
            else
            {
                dr["InactiveDate"] = DBNull.Value;
            }
            dr["Special_Unit"] = 0;
            dr["Business_Type"] = 0;
            dr["MOB"] = 0;
            dr["Mgmt_Category"] = 0;
            dr["Contract_Expiry"] = DBNull.Value;
            dr["Managing_Partner"] = DBNull.Value;
            dr["Planing_Director"] = DBNull.Value;
            dr["Mgmt_Team"] = 0;
            dr["Added_Calculation_Type"] = 0;
            dr["Margin_Cost"] = chkMargin.Checked ? 1 : 0; // bit
            dr["GPM"] = 0; // bit
            dr["Show_CComm"] = 1;
            dr["GPM_CLIENT_CODE"] = txtGPMClientCode.Text;
            dr["GPM_CLIENT_CODE_TMP"] = txtGPMClientCode.Text;
            dr["Opt_in"] = DBNull.Value;
            dr["Mapping_Symp"] = txtSymphonyID.Text;
            dr["Direct_Client"] = chkDirectClient.Checked ? 1 : 0; // bit
            dr["RatingEngineId"] = cboRatingEngine.SelectedIndex + 1; // int
            dr["RED_Status"] = chkRedClient.Checked ? 1 : 0; // bit
            dr["Report_to_Agency"] = txtReportToAgencyCode.Text;
            dr["Sym_ClientUniqueId"] = DBNull.Value;
            dr["Opt_in_Signed"] = chkOptInSigned.Checked ? 1 : 0; // bit
            if (chkOptInSigned.Checked)
            {
                dr["Opt_in_StartDate"] = dtOptInStartDate.Value; // date
                dr["Opt_in_EndDate"] = dtOptInEndDate.Value; // date
            }
            else
            {
                dr["Opt_in_StartDate"] = DBNull.Value; // date
                dr["Opt_in_EndDate"] = DBNull.Value; // date
            }
            dr["Opt_in_Note"] = txtOptInNote.Text;
            dr["Region"] = cboRegion.Text;

            return dr;
        }

        private bool CheckDataBeforeSave()
        {
            if (m_screemMode == eScreenMode.Add)
            {
                if (txtClientCode.Text == "")
                {
                    GMessage.MessageWarning("Please input Client Code.");
                    txtClientCode.Focus();
                    return false;
                }
                if (m_db.SelectClient(txtClientCode.Text).Rows.Count != 0)
                {
                    GMessage.MessageWarning(string.Format("Please change Client Code. The {0} already exist.", txtClientCode.Text));
                    txtClientCode.Focus();
                    return false;
                }
            }
            if (txtDisplayName.Text == "")
            {
                GMessage.MessageWarning("Please input Display Name.");
                txtDisplayName.Focus();
                return false;
            }
            if (txtEnglishName.Text == "")
            {
                GMessage.MessageWarning("Please input English Name.");
                txtEnglishName.Focus();
                return false;
            }
            if (txtMasterClientCode.Text == "")
            {
                GMessage.MessageWarning("Please input Master Client Name.");
                txtMasterClient.Focus();
                return false;
            }
            if (txtAgencyCode.Text == "")
            {
                GMessage.MessageWarning("Please input Agency Name.");
                txtAgency.Focus();
                return false;
            }
            if (txtReportToAgencyCode.Text == "")
            {
                GMessage.MessageWarning("Please input Report to Agency Name.");
                txtReportToAgency.Focus();
                return false;
            }
            if (txtOfficeCode.Text == "")
            {
                GMessage.MessageWarning("Please input Office Name.");
                txtOffice.Focus();
                return false;
            }
            if (txtCreativeAgencyCode.Text == "")
            {
                GMessage.MessageWarning("Please input Creative Agency Name.");
                txtCreativeAgency.Focus();
                return false;
            }
            if (chkOptInSigned.Checked)
            {
                if (dtOptInStartDate.Value > dtOptInEndDate.Value)
                {
                    GMessage.MessageWarning("Opt-In end date must more than or equal start date.");
                    return false;
                }
                if (gvOptIn.Rows.Count == 0)
                {
                    GMessage.MessageWarning("Please select proprietary group.");
                    return false;
                }
            }
            ArrayList al = new ArrayList();
            foreach (DataRow dr in ((DataTable)gvBrand.DataSource).Rows)
            {
                if (al.Contains(dr["MasterCode"]))
                {

                    GMessage.MessageWarning("Duplicate brand code, please check brand code \"" + dr["MasterCode"].ToString() + "\".");
                    tabContact.SelectedIndex = 1;
                    return false;
                }
                else
                {
                    al.Add(dr["MasterCode"]);
                }
            }
            return true;
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to exit?", "Client", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Dispose();
            }
        }

        private void txtClientCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)3 && e.KeyChar != (char)22 && e.KeyChar != (char)24 && regex.IsMatch(e.KeyChar.ToString()))
            {
                e.Handled = true;
            }
        }

        private void txtClientCode_TextChanged(object sender, EventArgs e)
        {
            if (m_screemMode == eScreenMode.Add && chkMasterClient.Checked == true)
            {
                txtMasterClientCode.Text = txtClientCode.Text;
            }
        }

        private void txtDisplayName_TextChanged(object sender, EventArgs e)
        {
            if (m_screemMode == eScreenMode.Add)
            {
                txtEnglishName.Text = txtDisplayName.Text;
                txtThaiName.Text = txtDisplayName.Text;
            }
            if (chkMasterClient.Checked == true)
            {
                txtMasterClient.Text = txtDisplayName.Text;
            }
        }

        private void chkMasterClient_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMasterClient.Checked == true)
            {
                txtMasterClient.Enabled = false;
                txtMasterClient.Text = txtDisplayName.Text;
                txtMasterClientCode.Text = txtClientCode.Text;

                btnAddPeriodAudit.Enabled = true;
                gvAudit.Enabled = true;
                btnAddPeriodEPD.Enabled = true;
                gvEPD.Enabled = true;
                btnAddPeriodMediaCredit.Enabled = true;
                gvMediaCredit.Enabled = true;
                btnAddPeriodRebate.Enabled = true;
                gvRebate.Enabled = true;
                btnAddPeriodSAC.Enabled = true;
                gvSAC.Enabled = true;
            }
            else
            {
                if (m_db.SelectMasterClientIsUsing(txtClientCode.Text))
                {
                    string listClient = "";
                    DataTable dt = m_db.SelectClientInMasterClient(txtClientCode.Text);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (listClient == "")
                            listClient += dt.Rows[i]["Client_ID"].ToString() + " - " + dt.Rows[i]["Short_Name"].ToString();
                        else
                            listClient += ",\n" + dt.Rows[i]["Client_ID"].ToString() + " - " + dt.Rows[i]["Short_Name"].ToString();
                    }
                    string str = "Cannot remove Master Client Flag because some client(s) has referred {Client} as Master Client.\n" +
                    "Please change a new Master Client to those client first.\n" +
                    listClient;
                    GMessage.MessageWarning(str);
                    chkMasterClient.Checked = true;
                }
                else
                {
                    txtMasterClient.Enabled = true;

                    btnAddPeriodAudit.Enabled = false;
                    gvAudit.Enabled = false;
                    btnAddPeriodEPD.Enabled = false;
                    gvEPD.Enabled = false;
                    btnAddPeriodMediaCredit.Enabled = false;
                    gvMediaCredit.Enabled = false;
                    btnAddPeriodRebate.Enabled = false;
                    gvRebate.Enabled = false;
                    btnAddPeriodSAC.Enabled = false;
                    gvSAC.Enabled = false;
                }
            }
        }

        private void txtMasterClient_Click(object sender, EventArgs e)
        {
            if (m_screemMode == eScreenMode.Add || m_screemMode == eScreenMode.Edit)
            {
                Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Client_ID", "Short_Name", "Master_Client", false, "", "");
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    txtMasterClient.Text = frm.SelectedGridRow["MasterName"].ToString();
                    txtMasterClientCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
                }
            }
        }

        private void txtMasterClient_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtMasterClient_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Clipboard.Clear();
            }
        }

        private void txtMasterClient_TextChanged(object sender, EventArgs e)
        {
            if (txtMasterClient.Text == "" && chkMasterClient.Checked == false)
            {
                txtMasterClientCode.Text = "";
            }
        }

        private void txtAgency_Click(object sender, EventArgs e)
        {
            if (m_screemMode == eScreenMode.Add || m_screemMode == eScreenMode.Edit)
            {
                Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Agency_ID", "Short_Name", "Agency", false, "", "");
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    txtAgency.Text = frm.SelectedGridRow["MasterName"].ToString();
                    txtAgencyCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
                }
            }
        }

        private void txtAgency_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtAgency_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Clipboard.Clear();
            }
        }

        private void txtAgency_TextChanged(object sender, EventArgs e)
        {
            if (txtAgency.Text == "")
            {
                txtAgencyCode.Text = "";
            }
        }

        private void txtReportToAgency_Click(object sender, EventArgs e)
        {
            if (m_screemMode == eScreenMode.Add || m_screemMode == eScreenMode.Edit)
            {
                Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Agency_ID", "Short_Name", "Agency", false, "", "");
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    txtReportToAgency.Text = frm.SelectedGridRow["MasterName"].ToString();
                    txtReportToAgencyCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
                }
            }
        }

        private void txtReportToAgency_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtReportToAgency_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Clipboard.Clear();
            }
        }

        private void txtReportToAgency_TextChanged(object sender, EventArgs e)
        {
            if (txtReportToAgency.Text == "")
            {
                txtReportToAgencyCode.Text = "";
            }
        }

        private void txtOffice_Click(object sender, EventArgs e)
        {
            if (m_screemMode == eScreenMode.Add || m_screemMode == eScreenMode.Edit)
            {
                Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Office_ID", "Short_Name", "Office", false, "", txtAgencyCode.Text);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    txtOffice.Text = frm.SelectedGridRow["MasterName"].ToString();
                    txtOfficeCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
                }
            }
        }

        private void txtOffice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtOffice_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Clipboard.Clear();
            }
        }

        private void txtOffice_TextChanged(object sender, EventArgs e)
        {
            if (txtOffice.Text == "")
            {
                txtOfficeCode.Text = "";
            }
        }

        private void txtCreativeAgency_Click(object sender, EventArgs e)
        {
            if (m_screemMode == eScreenMode.Add || m_screemMode == eScreenMode.Edit)
            {
                Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Creative_Agency_ID", "Short_Name", "Creative_Agency", false, "", "");
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    txtCreativeAgency.Text = frm.SelectedGridRow["MasterName"].ToString();
                    txtCreativeAgencyCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
                }
            }
        }

        private void txtCreativeAgency_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtCreativeAgency_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Clipboard.Clear();
            }
        }

        private void txtCreativeAgency_TextChanged(object sender, EventArgs e)
        {
            if (txtCreativeAgency.Text == "")
            {
                txtCreativeAgencyCode.Text = "";
            }
        }

        private void txtGPMClient_Click(object sender, EventArgs e)
        {
            if (m_screemMode == eScreenMode.Add || m_screemMode == eScreenMode.Edit)
            {
                Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Client_ID", "Short_Name", "GPM_Client", false, "", "");
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    txtGPMClient.Text = frm.SelectedGridRow["MasterName"].ToString();
                    txtGPMClientCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
                }
            }
        }

        private void txtGPMClient_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtGPMClient_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Clipboard.Clear();
            }
        }

        private void txtGPMClient_TextChanged(object sender, EventArgs e)
        {
            if (txtGPMClient.Text == "")
            {
                txtGPMClientCode.Text = "";
            }
        }

        private void rdActive_CheckedChanged(object sender, EventArgs e)
        {
            DateTimeConvertUtil.DateTimeSetEmpty(dtInactiveDate);
        }

        private void rdInactive_CheckedChanged(object sender, EventArgs e)
        {
            DateTimeConvertUtil.DateTimeRestoreFormat(dtInactiveDate);
        }

        private void chkOptInSigned_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOptInSigned.Checked == true)
            {
                DateTimeConvertUtil.DateTimeRestoreFormat(dtOptInStartDate);
                DateTimeConvertUtil.DateTimeRestoreFormat(dtOptInEndDate);
                btnAddProprietaryGroup.Enabled = true;
                btnOptInAdvanceSetting.Enabled = true;
            }
            else
            {
                DateTimeConvertUtil.DateTimeSetEmpty(dtOptInStartDate);
                DateTimeConvertUtil.DateTimeSetEmpty(dtOptInEndDate);
                btnAddProprietaryGroup.Enabled = false;
                btnOptInAdvanceSetting.Enabled = false;
                DataTable dtOptIn = m_db.SelectProprietaryByClient("");
                gvOptIn.DataSource = dtOptIn;
            }
        }

        private void cboBookingHeader_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = m_db.SelectAddressByBookingHeader(cboBookingHeader.Text);
            if (dt.Rows.Count > 0)
            {
                txtThaiAddress1.Text = dt.Rows[0]["Address"].ToString();
                txtThaiAddress2.Text = dt.Rows[0]["Thai_Address2"].ToString();
                txtThaiAddress3.Text = dt.Rows[0]["Thai_Address3"].ToString();
                txtThaiAddress4.Text = dt.Rows[0]["Thai_Address4"].ToString();

                txtEnglishAddress1.Text = dt.Rows[0]["English_Address1"].ToString();
                txtEnglishAddress2.Text = dt.Rows[0]["English_Address2"].ToString();
                txtEnglishAddress3.Text = dt.Rows[0]["English_Address3"].ToString();
            }
        }

        private void btnBrand_Click(object sender, EventArgs e)
        {
            Master_ClientBrand frm = new Master_ClientBrand();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (gvBrand.DataSource == null)
                    gvBrand.DataSource = m_db.SelectMasterCommon("Brand_ID", "Brand_Name", "Brand", false, "", "").Clone();
                DataRow dr = ((DataTable)gvBrand.DataSource).NewRow();
                dr["MasterCode"] = frm.txtBrandCode.Text;
                dr["MasterName"] = frm.txtBrandName.Text;
                ((DataTable)gvBrand.DataSource).Rows.Add(dr);
            }
        }

        private void gvBrand_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvBrand.Rows.Count == 0)
                return;
            if (e.ColumnIndex == 2) //Del
            {
                if (m_db.SelectBrandIsUsing(gvBrand.Rows[e.RowIndex].Cells[0].Value.ToString(), txtClientCode.Text))
                {
                    string str = $"Cannot delete Brand Code: {gvBrand.Rows[e.RowIndex].Cells[0].Value}  because some Buying Brief(s) referred to this Brand.";
                    GMessage.MessageWarning(str);
                    return;
                }
                else
                {
                    DataRow dr = ((DataRowView)gvBrand.Rows[e.RowIndex].DataBoundItem).Row;
                    ((DataTable)gvBrand.DataSource).Rows.Remove(dr);
                }
            }
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            if (gvCategory.DataSource == null)
                gvCategory.DataSource = m_db.SelectMasterCommon("Category_ID", "Category_Name", "Category", false, "", "").Clone();

            DataTable dtCategory = (DataTable)gvCategory.DataSource;
            Utility_MultipleSelect_CodeName frm = new Utility_MultipleSelect_CodeName("Category_ID", "Category_Name", "Category", dtCategory, "");
            frm.ShowDialog();
        }

        private void gvCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvCategory.Rows.Count == 0)
                return;
            if (e.ColumnIndex == 2) //Del
            {
                if (m_db.SelectCategoryIsUsing(gvCategory.Rows[e.RowIndex].Cells[0].Value.ToString(), txtClientCode.Text))
                {
                    string str = $"Cannot delete Category Code: {gvCategory.Rows[e.RowIndex].Cells[0].Value}  because some Buying Brief(s) referred to this Category.";
                    GMessage.MessageWarning(str);
                    return;
                }
                else
                {
                    DataRow dr = ((DataRowView)gvCategory.Rows[e.RowIndex].DataBoundItem).Row;
                    ((DataTable)gvCategory.DataSource).Rows.Remove(dr);
                }
            }
        }
        
        private void btnAddProprietaryGroup_Click(object sender, EventArgs e)
        {
            string filter = ",";
            DataTable dt = (DataTable)gvOptIn.DataSource;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                filter += dr["GroupProprietaryId"].ToString() + ",";
            }
            filter += ",";
            frmProprietaryGroupList frm = new frmProprietaryGroupList();
            frm.filter = filter;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < frm.SelectedGridRow.Count; i++)
                {
                    DataRow dr = ((DataTable)gvOptIn.DataSource).NewRow();
                    dr["GroupProprietaryId"] = ((DataRow)frm.SelectedGridRow[i])["GroupProprietaryId"];
                    dr["GroupProprietaryName"] = ((DataRow)frm.SelectedGridRow[i])["GroupProprietaryName"];
                    dr["ContractType"] = ((DataRow)frm.SelectedGridRow[i])["ContractType"];
                    ((DataTable)gvOptIn.DataSource).Rows.Add(dr);
                }
            }
        }

        private void gvOptIn_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvOptIn.Rows.Count == 0)
                return;
            if (e.ColumnIndex == 2) //Del
            {
                DataRow dr = ((DataRowView)gvOptIn.Rows[e.RowIndex].DataBoundItem).Row;
                ((DataTable)gvOptIn.DataSource).Rows.Remove(dr);
            }
        }

        private void btnOptInSetting_Click(object sender, EventArgs e)
        {
            Program.Username = username;
            frmClientMapping frm = new frmClientMapping(txtClientCode.Text, txtDisplayName.Text);
            frm.ShowDialog();
            gvOptIn.DataSource = null;
            DataTable dtOptIn = m_db.SelectProprietaryByClient(txtClientCode.Text);
            gvOptIn.DataSource = dtOptIn;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (m_db.SelectClientIsUsing(txtClientCode.Text))
            {
                string str = $"Cannot delete Client Code: {txtClientCode.Text} - {txtDisplayName.Text}  because some Buying Brief(s) referred to this Client.";
                GMessage.MessageWarning(str);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure to delete Client: " + txtClientCode.Text + " - " + txtDisplayName.Text + "?", "Client", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                m_db.DeleteClient(txtClientCode.Text);
                m_db.InsertLogMinder(username.Replace(".", ""), "Delete Client ID:" + txtClientCode.Text + ",Name :" + txtDisplayName.Text, "Master File", username.Replace(".", ""));
                GMessage.MessageInfo("Delete Completed");
                DialogResult = DialogResult.Yes;
            }
        }

        private void btnAddPeriodAudit_Click(object sender, EventArgs e)
        {
            Master_Client_VendorPeriod frm = new Master_Client_VendorPeriod();
            frm.type = "Client";
            frm.header = "Audit Right";
            if (frm.ShowDialog() == DialogResult.OK)
            {
                DataRow dr = ((DataTable)gvAudit.DataSource).NewRow();
                if (frm.rdRed.Checked)
                    dr["Type"] = "Red";
                else
                    dr["Type"] = "Green";
                dr["Start_Date"] = frm.dtStartDate.Value.Date;
                dr["End_Date"] = frm.dtEndDate.Value.Date;
                ((DataTable)gvAudit.DataSource).Rows.Add(dr);
            }
        }

        private void gvAudit_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvAudit.Rows.Count == 0)
                return;
            if (e.ColumnIndex == 3)
            {
                DialogResult result = MessageBox.Show("Are you sure to delete the period?", "Client", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DataRow dr = ((DataRowView)gvAudit.Rows[e.RowIndex].DataBoundItem).Row;
                    ((DataTable)gvAudit.DataSource).Rows.Remove(dr);
                }
            }
        }

        private void btnAddPeriodEPD_Click(object sender, EventArgs e)
        {
            Master_Client_VendorPeriod frm = new Master_Client_VendorPeriod();
            frm.type = "Client";
            frm.header = "EPD (Pass Back)";
            if (frm.ShowDialog() == DialogResult.OK)
            {
                DataRow dr = ((DataTable)gvEPD.DataSource).NewRow();
                if (frm.rdRed.Checked)
                    dr["Type"] = "Red";
                else
                    dr["Type"] = "Green";
                dr["Start_Date"] = frm.dtStartDate.Value.Date;
                dr["End_Date"] = frm.dtEndDate.Value.Date;
                ((DataTable)gvEPD.DataSource).Rows.Add(dr);
            }
        }

        private void gvEPD_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvEPD.Rows.Count == 0)
                return;
            if (e.ColumnIndex == 3)
            {
                DialogResult result = MessageBox.Show("Are you sure to delete the period?", "Client", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DataRow dr = ((DataRowView)gvEPD.Rows[e.RowIndex].DataBoundItem).Row;
                    ((DataTable)gvEPD.DataSource).Rows.Remove(dr);
                }
            }
        }

        private void btnAddPeriodMediaCredit_Click(object sender, EventArgs e)
        {
            Master_Client_VendorPeriod frm = new Master_Client_VendorPeriod();
            frm.type = "Client";
            frm.header = "Media Credit (Pass Back)";
            if (frm.ShowDialog() == DialogResult.OK)
            {
                DataRow dr = ((DataTable)gvMediaCredit.DataSource).NewRow();
                if (frm.rdRed.Checked)
                    dr["Type"] = "Red";
                else
                    dr["Type"] = "Green";
                dr["Start_Date"] = frm.dtStartDate.Value.Date;
                dr["End_Date"] = frm.dtEndDate.Value.Date;
                ((DataTable)gvMediaCredit.DataSource).Rows.Add(dr);
            }
        }

        private void gvMediaCredit_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvMediaCredit.Rows.Count == 0)
                return;
            if (e.ColumnIndex == 3)
            {
                DialogResult result = MessageBox.Show("Are you sure to delete the period?", "Client", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DataRow dr = ((DataRowView)gvMediaCredit.Rows[e.RowIndex].DataBoundItem).Row;
                    ((DataTable)gvMediaCredit.DataSource).Rows.Remove(dr);
                }
            }
        }

        private void btnAddPeriodRebate_Click(object sender, EventArgs e)
        {
            Master_Client_VendorPeriod frm = new Master_Client_VendorPeriod();
            frm.type = "Client";
            frm.header = "Rebate (Pass Back)";
            if (frm.ShowDialog() == DialogResult.OK)
            {
                DataRow dr = ((DataTable)gvRebate.DataSource).NewRow();
                if (frm.rdRed.Checked)
                    dr["Type"] = "Red";
                else
                    dr["Type"] = "Green";
                dr["Start_Date"] = frm.dtStartDate.Value.Date;
                dr["End_Date"] = frm.dtEndDate.Value.Date;
                ((DataTable)gvRebate.DataSource).Rows.Add(dr);
            }
        }

        private void gvRebate_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvRebate.Rows.Count == 0)
                return;
            if (e.ColumnIndex == 3)
            {
                DialogResult result = MessageBox.Show("Are you sure to delete the period?", "Client", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DataRow dr = ((DataRowView)gvRebate.Rows[e.RowIndex].DataBoundItem).Row;
                    ((DataTable)gvRebate.DataSource).Rows.Remove(dr);
                }
            }
        }

        private void btnAddPeriodSAC_Click(object sender, EventArgs e)
        {
            Master_Client_VendorPeriod frm = new Master_Client_VendorPeriod();
            frm.type = "Client";
            frm.header = "SAC (Pass Back)";
            if (frm.ShowDialog() == DialogResult.OK)
            {
                DataRow dr = ((DataTable)gvSAC.DataSource).NewRow();
                if (frm.rdRed.Checked)
                    dr["Type"] = "Red";
                else
                    dr["Type"] = "Green";
                dr["Start_Date"] = frm.dtStartDate.Value.Date;
                dr["End_Date"] = frm.dtEndDate.Value.Date;
                ((DataTable)gvSAC.DataSource).Rows.Add(dr);
            }
        }

        private void gvSAC_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvSAC.Rows.Count == 0)
                return;
            if (e.ColumnIndex == 3)
            {
                DialogResult result = MessageBox.Show("Are you sure to delete the period?", "Client", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DataRow dr = ((DataRowView)gvSAC.Rows[e.RowIndex].DataBoundItem).Row;
                    ((DataTable)gvSAC.DataSource).Rows.Remove(dr);
                }
            }
        }

        private void gvAgencyFee_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvAgencyFee.Rows.Count == 0)
                return;
            if (e.ColumnIndex == 4)
            {
                DataRow dr = ((DataRowView)gvAgencyFee.Rows[e.RowIndex].DataBoundItem).Row;
                if (dr["Priority"].ToString() == "1")
                {
                    GMessage.MessageWarning("Can't delete mandatory agency fee.");
                    return;
                }
                DialogResult result = MessageBox.Show("Are you sure to delete the Agency Fee?", "Client", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    dr.Delete();
                    //((DataTable)gvAgencyFee.DataSource).Rows.Remove(dr);
                }
            }
        }

        private void btnAddSpecificFee_Click(object sender, EventArgs e)
        {
            Master_ClientAgencyFee frm = new Master_ClientAgencyFee();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                DataRow dr = ((DataTable)gvAgencyFee.DataSource).NewRow();
                dr["Client_ID"] = "";
                if (frm.rdMediaType.Checked)
                {
                    dr["Priority"] = 2;
                    dr["Agency_Fee"] = "0.0000";
                    dr["Editable"] = false ;
                    dr["Agency_Fee_Set_Up_Name"] = "Media Type";
                    dr["Agency_Fee_Set_Up_Column"] = "[{\"Column\":\"Media_Type\",\"Value\":\"" + frm.cboMediaType.SelectedValue + "\"}]";
                    dr["Media_Type"] = frm.cboMediaType.SelectedValue;
                    dr["description"] = frm.cboMediaType.Text;
                }
                else if (frm.rdMediaSubType.Checked)
                {
                    dr["Priority"] = 3;
                    dr["Agency_Fee"] = "0.0000";
                    dr["Editable"] = false;
                    dr["Agency_Fee_Set_Up_Name"] = "Media Sub Type";
                    dr["Agency_Fee_Set_Up_Column"] = "[{\"Column\":\"Media_Sub_Type\",\"Value\":\"" + frm.cboMediaSubType.SelectedValue + "\"}]";
                    dr["Media_Sub_Type"] = frm.cboMediaSubType.SelectedValue;
                    dr["description"] = frm.cboMediaSubType.Text;
                }
                else if (frm.rdOutdoorCostType.Checked)
                {
                    dr["Priority"] = 4;
                    dr["Agency_Fee"] = "0.0000";
                    dr["Editable"] = false;
                    dr["Agency_Fee_Set_Up_Name"] = "Outdoor Cost Type";
                    dr["Agency_Fee_Set_Up_Column"] = "[{\"Column\":\"Media_Type\",\"Value\":\"OD\"},{\"Column\":\"Cost_Type\",\"Value\":\""+frm.cboOutdoorCostType.Text+"\"}]";
                    dr["Other_Value"] = frm.cboOutdoorCostType.Text;
                    dr["description"] = frm.cboOutdoorCostType.Text;
                }
                else if (frm.rdXaxis.Checked)
                {
                    dr["Priority"] = 5;
                    dr["Agency_Fee"] = "0.0000";
                    dr["Editable"] = false;
                    dr["Agency_Fee_Set_Up_Name"] = "Xaxis";
                    dr["Agency_Fee_Set_Up_Column"] = "[{\"Column\":\"Media_Type\",\"Value\":\"PM\"},{\"Column\":\"Media_Vendor_ID\",\"Value\":\"XAXISMI\"}]";
                    dr["Other_Value"] = "Xaxis";
                    dr["description"] = "Xaxis";
                }
                else if (frm.rdINCA.Checked)
                {
                    dr["Priority"] = 6;
                    dr["Agency_Fee"] = "0.0000";
                    dr["Editable"] = false;
                    dr["Agency_Fee_Set_Up_Name"] = "INCA";
                    dr["Agency_Fee_Set_Up_Column"] = "[{\"Column\":\"Media_Sub_Type\",\"Value\":\"KLPK\"},{\"Column\":\"Media_Vendor_ID\",\"Value\":\"MMINCA\"}]";
                    dr["Other_Value"] = "INCA";
                    dr["description"] = "INCA";
                }
                if (((DataTable)gvAgencyFee.DataSource).Select($"Agency_Fee_Set_Up_Name='{dr["Agency_Fee_Set_Up_Name"].ToString()}' AND description='{dr["description"].ToString()}'").Length > 0)
                {
                    GMessage.MessageWarning($"{dr["Agency_Fee_Set_Up_Name"].ToString()}-{dr["description"].ToString()} is exists");
                    return;
                }
                ((DataTable)gvAgencyFee.DataSource).Rows.Add(dr);
            }
        }

        private void gvAgencyFee_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (gvAgencyFee.Columns[gvAgencyFee.CurrentCell.ColumnIndex].Name == "ColAgencyFee") //Total
            {
                e.Control.KeyPress -= new KeyPressEventHandler(ColNumber_KeyPress);

                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(ColNumber_KeyPress);
                }
            }

        }
        private void ColNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar))
            {
                if(e.KeyChar == 46 && !(((TextBox)sender).Text.IndexOf(".") > -1))
                        return;
                e.Handled = true;
            }
      }
    }
}
