using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GroupM.DBAccess;
using GroupM.UTL;
using System.Text.RegularExpressions;

namespace GroupM.Minder
{
    public partial class Implementation_SpotPlan_OD_Amendment : Form
    {
        public DataRow drInput = null;
        enum eScreenMode { Add, Edit, View }
        eScreenMode m_screenMode;
        public string Client_ID = "";
        public int m_iSpendingType = 0;
        public string MediaTypeCode = "";
        public string MasterMediaType = "";
        public string m_strBuyTypeName = "";
        public string m_strMaterialID = "";
        public string CampaignStartDate = "";
        public string CampaignEndDate = "";
        public string AdeptMediaType = "";
        private DateTime CSD;
        private DateTime CED;
        private DateTime MSD;
        private DateTime MED;
        private DateTime SD;
        private DateTime ED;
        DBManager m_db = null;
        private bool m_bLoading = true;

        //ForCalAgencyFee
        public string Product_ID = "";
        public string BuyingBriefID = "";

        public Implementation_SpotPlan_OD_Amendment(string screenMode, string strMt)
        {
            InitializeComponent();
            MediaTypeCode = strMt;
            if (screenMode == "Add")
            {
                m_screenMode = eScreenMode.Add;
                m_bLoading = false ;
            }
            if (screenMode == "Edit")
                m_screenMode = eScreenMode.Edit;
            if (screenMode == "View")
                m_screenMode = eScreenMode.View;
            SetScreenMode(m_screenMode);
            m_db = new DBManager();
        }
        public Implementation_SpotPlan_OD_Amendment(string screenMode, string strMt,string strMasterMediaType)
        {
            InitializeComponent();
            MasterMediaType = strMasterMediaType;
            MediaTypeCode = strMt;
            if (screenMode == "Add")
            {
                m_screenMode = eScreenMode.Add;
                m_bLoading = false;
            }
            if (screenMode == "Edit")
                m_screenMode = eScreenMode.Edit;
            if (screenMode == "View")
                m_screenMode = eScreenMode.View;
            SetScreenMode(m_screenMode);
            m_db = new DBManager();
        }

        private void SetScreenMode(eScreenMode mode)
        {
            switch (mode)
            {
                case eScreenMode.View:
                    txtMedia.Enabled = false;
                    txtVendor.Enabled = false;
                    txtDescription.ReadOnly = true;
                    btnRateCard.Enabled = false;
                    txtBuyType.Enabled = false;
                    dtStartDate.Enabled = false;
                    dtEndDate.Enabled = false;
                    txtNetCost.ReadOnly = true;
                    txtAgencyFeePercent.ReadOnly = true;
                    btnOK.Visible = false;
                    btnCancel.Visible = false;
                    break; 
            }
            m_screenMode = mode;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!CheckDataBeforeClose())
                return;
            DialogResult = DialogResult.OK;
        }

        private bool SetDescriptionProprietaryMedia()
        {
            bool set = true;
            DataTable dt = m_db.CheckOptIn_ContractType_Spotplan(txtVendorCode.Text, txtMediaCode.Text);
            if (dt.Rows.Count > 0)
            {
                //Checking media name including "Proprietary Media" or not ?
                string txtDesc = txtDescription.Text.Trim().ToUpper();
                bool endsWithSearchResult1 = txtDesc.EndsWith("PROPRIETARY MEDIA", System.StringComparison.CurrentCultureIgnoreCase);
                bool endsWithSearchResult2 = txtDesc.EndsWith("MEDIA INVENTORY", System.StringComparison.CurrentCultureIgnoreCase);
                string txtM = txtMedia.Text.Trim().ToUpper();
                bool endsWithSearchResult_M1 = txtM.ToUpper().Contains("PROPRIETARY MEDIA");
                bool endsWithSearchResult_M2 = txtM.ToUpper().Contains("PROPRIETARYMEDIA");
                if (txtDesc.Contains("MEDIA INVENTORY"))
                {
                    txtDescription.Text = Regex.Replace(txtDescription.Text, "media inventory", ":PROPRIETARY MEDIA", RegexOptions.IgnoreCase);
                }
                if (txtDesc.Contains("INVENTORY MEDIA"))
                {
                    txtDescription.Text = Regex.Replace(txtDescription.Text, "inventory media", ":PROPRIETARY MEDIA", RegexOptions.IgnoreCase);
                }
                if (endsWithSearchResult_M1 == false && endsWithSearchResult_M2 == false && endsWithSearchResult1 == false)
                {
                    txtDescription.Text += ":PROPRIETARY MEDIA";
                }
                if (txtDescription.Text != "" &&
                    (endsWithSearchResult1 == false || txtDesc.Substring(txtDesc.Length - 17, 17) != "PROPRIETARY MEDIA") &&
                    (endsWithSearchResult2 == false || txtDesc.Substring(txtDesc.Length - 15, 15) != "MEDIA INVENTORY") &&
                    txtMedia.Text.IndexOf("PROPRIETARY MEDIA", StringComparison.OrdinalIgnoreCase) == 0 &&
                    txtMedia.Text.IndexOf("PROPRIETARYMEDIA", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    GMessage.MessageWarning($"Please correct the description with \":PROPRIETARY MEDIA\" at the end.\r\n(such as : {txtDescription.Text}:PROPRIETARY MEDIA)");
                    txtDescription.Text = "";
                    set = false;
                }
                if (txtDescription.TextLength > 47)
                {
                    GMessage.MessageWarning($"Please correct lenght of description isn't over than 47 characters.\r\n(Current lenght = {txtDescription.TextLength})");
                    set = false;
                }
            }
            return set;
        }

        private bool ValidatePeriod(bool sd, bool ed, bool chk)
        {
            bool valid = true;
            bool validSD1 = true;
            bool validSD2 = true;
            bool validED1 = true;
            bool validED2 = true;
            if (sd)
            {
                validSD1 = true;
                validSD2 = true;
                validED1 = false;
                validED2 = false;
            }
            else if (ed)
            {
                validSD1 = false;
                validSD2 = false;
                validED1 = true;
                validED2 = true;
            }
            else if (chk)
            {
                validSD1 = true;
                validSD2 = true;
                validED1 = false;
                validED2 = true;
            }
            if (validSD1)
            {
                if (dtStartDate.Value > dtEndDate.Value)
                {
                    GMessage.MessageWarning("Start date can't greater than End date");
                    dtStartDate.Value = SD;
                    valid = false;
                }
            }
            if (validSD2)
            {
                if (dtStartDate.Value < CSD)
                {
                    GMessage.MessageWarning("Start date can't less than Campaign Start date");
                    dtStartDate.Value = SD;
                    valid = false;
                }
                if (dtStartDate.Value < MSD)
                {
                    string msg = "Media " + txtMediaCode.Text + " : " + txtMedia.Text + " is effective from " + MSD.ToString("dd/MM/yyyy") + " to " + MED.ToString("dd/MM/yyyy") +
                    "\n\nSystem will automatically change the Media Start Date to the validity period.";
                    GMessage.MessageWarning(msg);
                    dtStartDate.Value = SD;
                    valid = false;
                }
            }
            if (validED1)
            {
                if (dtEndDate.Value < dtStartDate.Value)
                {
                    GMessage.MessageWarning("End date can't less than Start date");
                    dtEndDate.Value = ED;
                    valid = false;
                }
            }
            if (validED2)
            {
                if (dtEndDate.Value > CED)
                {
                    GMessage.MessageWarning("End date can't greater than Campaign End date");
                    dtEndDate.Value = ED;
                    valid = false;
                }
                if (dtEndDate.Value > MED)
                {
                    string msg = "Media " + txtMediaCode.Text + " : " + txtMedia.Text + " is effective from " + MSD.ToString("dd/MM/yyyy") + " to " + MED.ToString("dd/MM/yyyy") +
                    "\n\nSystem will automatically change the Media End Date to the validity period.";
                    GMessage.MessageWarning(msg);
                    dtEndDate.Value = ED;
                    valid = false;
                }
            }
            txtTotalDays.Text = ((dtEndDate.Value - dtStartDate.Value).Days+1).ToString();
            return valid;
        }

        private bool CheckDataBeforeClose()
        {
            if (txtProvince.Text == "")
            {
                GMessage.MessageWarning("Please select Province.");
                return false;
            }
            if (txtMediaCode.Text == "")
            {
                GMessage.MessageWarning("Please select Media.");
                return false;
            }
            if (txtVendorCode.Text == "")
            {
                GMessage.MessageWarning("Please select Vendor.");
                return false;
            }
            if (txtH.Text == "" && txtW.Text != "")
            {
                GMessage.MessageWarning("Please input Size H.");
                return false;
            }
            if (txtH.Text != "" && txtW.Text == "")
            { 
                GMessage.MessageWarning("Please input Size W.");
                return false;
            }
            if (m_db.CheckOptIn(txtVendorCode.Text, txtMediaCode.Text).Rows.Count > 0)
            {
                DataTable dt = m_db.CheckOptIn(txtVendorCode.Text, txtMediaCode.Text, Client_ID);
                if (dt.Rows.Count == 0)
                {
                    GMessage.MessageWarning($"This client can not buy media({txtMedia.Text}) with vendor({txtVendor.Text}). Please contact commercial team to check set up.");
                    return false;
                }
                else
                {
                    DataRow dr = dt.Rows[0];
                    DateTime dtOptInStartDate = Convert.ToDateTime(dr["Opt_in_StartDate"]);
                    DateTime dtOptInEndDate = Convert.ToDateTime(dr["Opt_in_EndDate"]);

                    if (!(dtOptInStartDate <= dtStartDate.Value && dtEndDate.Value <= dtOptInEndDate))
                    {
                        GMessage.MessageWarning($"This client can not buy media({txtMedia.Text}) with vendor({txtVendor.Text}). The client have been set up period Opt-in between {dtOptInStartDate.ToString("dd/MM/yyyy")} and {dtOptInEndDate.ToString("dd/MM/yyyy")}. Please contact commercial team to check set up.");
                        return false;
                    }
                }
                bool validDescriptionProprietaryMedia = SetDescriptionProprietaryMedia();
                if (!validDescriptionProprietaryMedia)
                {
                    return false;
                }
            }
            bool validPeriod = ValidatePeriod(false, false, true);
            if (!validPeriod)
            {
                return false;
            }

            if (cboCostType.Text == "")
            {
                GMessage.MessageWarning("Please select Cost Type.");
                return false;
            }
            if (txtNetCost.Value == 0)
            {
                GMessage.MessageWarning("Price must not equal 0.");
                return false;
            }

            return true;
        }

        private void txtVendor_Click(object sender, EventArgs e)
        {
            if (txtMediaCode.Text == "")
            {
                txtMedia_MouseClick(null, null);
            }
            if (txtMediaCode.Text == "")
            {
                return;
            }

            Implementation_SpotPlan_Popup_Vendor frm = new Implementation_SpotPlan_Popup_Vendor(txtMediaCode.Text);
            frm.IncludeInactive = true;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                DataTable dtCheck = m_db.CheckFCRevenue(txtMediaCode.Text, frm.SelectedGridRow["Media_Vendor_ID"].ToString(), drInput["Buying_Brief_ID"].ToString());
                if (dtCheck.Rows[0]["Buy"].ToString() == "True")
                {
                    txtVendor.Text = frm.SelectedGridRow["Short_Name"].ToString();
                    txtVendorCode.Text = frm.SelectedGridRow["Media_Vendor_ID"].ToString();
                }
                else
                {
                    GMessage.MessageWarning(dtCheck.Rows[0]["Message"].ToString());
                }
            }
        }

        private void txtVendor_TextChanged(object sender, EventArgs e)
        {
            if (txtVendor.Text == "")
            {
                txtVendorCode.Text = "";
            }
        }

        private void txtVendor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtMedia_MouseClick(object sender, MouseEventArgs e)
        {
            Implementation_SpotPlan_Popup_Media frm;
            if(MasterMediaType == "")
                frm = new Implementation_SpotPlan_Popup_Media("MediaTypePeriod", MediaTypeCode);
            else
                frm = new Implementation_SpotPlan_Popup_Media("MasterMediaTypePeriod", MasterMediaType);
            frm.m_SD = CSD.ToString("yyyyMMdd");
            frm.m_ED = CED.ToString("yyyyMMdd");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtMediaCode.Text = frm.SelectedGridRow["Media_ID"].ToString();
                txtMedia.Text = frm.SelectedGridRow["Short_Name"].ToString();
                m_iSpendingType = Convert.ToInt32(frm.SelectedGridRow["Spending_Type"]);
                MSD = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(frm.SelectedGridRow["EffectiveDate"].ToString());
                MED = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(frm.SelectedGridRow["InactiveDate"].ToString());

                txtMediaTypeCode.Text = frm.SelectedGridRow["Media_Type"].ToString();
                txtMediaType.Text = frm.SelectedGridRow["Media_Type_Name"].ToString();
                txtMediaSubTypeCode.Text = frm.SelectedGridRow["Media_Sub_Type"].ToString();
                txtMediaSubType.Text = frm.SelectedGridRow["Media_Sub_Type_Name"].ToString();

                DataTable dt = m_db.SelectVendorByMedia(txtMediaCode.Text);
                if (dt.Rows.Count == 1)
                {
                    txtVendorCode.Text = dt.Rows[0]["Media_Vendor_ID"].ToString();
                    txtVendor.Text = dt.Rows[0]["Short_Name"].ToString();
                }
                else if (txtVendorCode.Text != "")
                {
                    DataRow[] rowsToCopy = dt.Select(string.Format("Media_Vendor_ID = '{0}'", txtVendorCode.Text));
                    if (rowsToCopy.Length == 0)
                    {
                        txtVendorCode.Text = "";
                        txtVendor.Text = "";
                    }
                }

                SD = CSD < MSD ? MSD : CSD;
                ED = CED > MED ? MED : CED;
                //dtStartDate.Value = SD;
                //dtEndDate.Value = ED;

                dt = m_db.SelectAdeptMediaTypeByMedia(txtMediaCode.Text);
                if (dt.Rows.Count == 1)
                {
                    AdeptMediaType = dt.Rows[0]["Adept_Media_Type_Name"].ToString();
                }
            }
        }

        private void txtMedia_TextChanged(object sender, EventArgs e)
        {
            if (txtMedia.Text == "")
            {
                txtMediaCode.Text = "";
            }
        }

        private void txtMedia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtBuyType_Click(object sender, EventArgs e)
        {
            Implementation_SpotPlan_Popup_BuyType frm = new Implementation_SpotPlan_Popup_BuyType();
            frm.MediaType = "OD";
            frm.RemoveMonthly = true;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                m_strBuyTypeName = frm.SelectedGridRow["BuyTypeName"].ToString();
                txtBuyTypeCode.Text = frm.SelectedGridRow["BuyTypeID"].ToString();
                txtBuyType.Text = frm.SelectedGridRow["BuyTypeDisplay"].ToString();
            }
        }

        private void txtBuyType_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtBuyType_TextChanged(object sender, EventArgs e)
        {
            if (txtBuyType.Text == "")
            {
                txtBuyTypeCode.Text = "";
            }
        }

        private void Implementation_SpotPlan_OD_Amendment_Load(object sender, EventArgs e)
        {
            //Initial Data
            txtBuyType.Text = "By Agency";
            txtBuyTypeCode.Text = "AG";
            m_strBuyTypeName = "By Agency";

            CSD = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(CampaignStartDate);
            CED = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(CampaignEndDate);
            if (m_screenMode == eScreenMode.Add)
            {
                SD = CSD;
                ED = CED;
                UpdateAgencyFeePercent();
            }
            if (m_screenMode == eScreenMode.Edit || m_screenMode == eScreenMode.View)
            {
                txtProvince.Text = drInput["Province"].ToString();
                txtProvinceCode.Text = drInput["Province"].ToString();

                txtMediaType.Text = drInput["MediaTypeName"].ToString();
                txtMediaTypeCode.Text = drInput["Media_Type"].ToString();
                txtMediaSubType.Text = drInput["MediaSubTypeName"].ToString();
                txtMediaSubTypeCode.Text = drInput["Media_Sub_Type"].ToString();
                txtMedia.Text = drInput["MediaName"].ToString();
                txtMediaCode.Text = drInput["Media_ID"].ToString();
                txtVendor.Text = drInput["MediaVendorName"].ToString();
                txtVendorCode.Text = drInput["Media_Vendor_ID"].ToString();
                txtDescription.Text = drInput["Program"].ToString();

                txtSize.Text = drInput["SizeHW"].ToString();
                if (txtSize.Text.Split('x').Length == 2)
                {
                    txtH.Text = txtSize.Text.Split('x')[0];
                    txtW.Text = txtSize.Text.Split('x')[1];
                }
                txtUnit.Text = drInput["Unit"].ToString();

                cboCostType.Text = drInput["State"].ToString();

                if (drInput["Start_Date"] == DBNull.Value)
                {
                    drInput["Start_Date"] = DateTime.Now.ToString("yyyyMMdd");
                    drInput["End_Date"] = DateTime.Now.ToString("yyyyMMdd");
                }
                SD = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(drInput["Start_Date"].ToString());
                ED = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(drInput["End_Date"].ToString());

                DataTable dt = m_db.SelectMediaPeriod(txtMediaCode.Text);
                if (dt.Rows.Count > 0)
                {
                    MSD = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(dt.Rows[0]["EffectiveDate"].ToString());
                    MED = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(dt.Rows[0]["InactiveDate"].ToString());
                }
                if(drInput["BuyTypeID"].ToString() != "")
                    {
                    txtBuyTypeCode.Text = drInput["BuyTypeID"].ToString();
                    txtBuyType.Text = drInput["Deadline_Terminate"].ToString();
                    m_strBuyTypeName = drInput["BuyTypeName"].ToString();
                }

                
                m_strMaterialID = drInput["Material_ID"].ToString();
                
                txtNetCost.Value = ConvertObjectToDecimal(drInput["Net_Cost"]);
                txtAgencyFeePercent.Value = ConvertObjectToDecimal(drInput["AgencyFeePercent"]);

                DataTable dtAgencyFee = m_db.SelectAgencyFeeBySchedule(Product_ID, txtMediaTypeCode.Text, txtMediaSubTypeCode.Text, txtVendorCode.Text, cboCostType.Text, "");
                if (dtAgencyFee.Rows.Count > 0)
                {
                    txtAgencyFeePercent.Enabled = Convert.ToBoolean(dtAgencyFee.Rows[0]["Editable"]);
                }
                txtRemark.Text = drInput["Remark"].ToString();
                txtQuotation.Text = drInput["Invoice_Number"].ToString();

                if (drInput["CPM"] != null && drInput["CPM"] != DBNull.Value)
                {
                    int CalType = Convert.ToInt32(drInput["CPM"]);
                    if (CalType == 1)
                        rdNone.Checked = true; 
                    else if (CalType == 3)
                        rdMonthly.Checked = true;
                    else if (CalType == 5)
                        rdDaily.Checked = true;
                    else
                        rdMonthly.Checked = true;
                }
                else
                    rdMonthly.Checked = true;
                RefreshTotalPrice();
                m_bLoading = false;
            }
            dtStartDate.Value = SD;
            dtEndDate.Value = ED;

            txtTotalDays.Text = ((dtEndDate.Value - dtStartDate.Value).Days+1).ToString();
        }
        private void UpdateAgencyFeePercent()
        {
            if (m_bLoading)
                return;
            DataTable dt = m_db.SelectAgencyFeeBySchedule(Product_ID, txtMediaTypeCode.Text, txtMediaSubTypeCode.Text, txtVendorCode.Text, cboCostType.Text, "");
            if (dt.Rows.Count > 0)
            {
                if (!m_bLoading)
                    txtAgencyFeePercent.Value = ConvertObjectToDecimal(dt.Rows[0]["Agency_Fee"]) * 100.00m;
                txtAgencyFeePercent.Enabled = Convert.ToBoolean(dt.Rows[0]["Editable"]);
            }
            
        }
        private decimal ConvertObjectToDecimal(object obj)
        {
            if (obj == DBNull.Value || obj == null)
                return 0;
            return Convert.ToDecimal(obj.ToString());
        }

        private void RefreshTotalPrice()
        {
            //txtNetCost.Value = txtTotalCost.Value - (txtTotalCost.Value * txtDiscount.Value / 100.00m);
            txtTotalPrice.Value = txtNetCost.Value * (1m+ (txtAgencyFeePercent.Value / 100.00m));

        }

        private void txtTotalCost_ValueChanged(object sender, EventArgs e)
        {
            RefreshTotalPrice();
        }

        private void txtDiscount_ValueChanged(object sender, EventArgs e)
        {
            RefreshTotalPrice();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void txtTotalCost_Enter(object sender, EventArgs e)
        {
            txtNetCost.Select(0, txtNetCost.ToString().Length);
        }

        private void txtTotalCost_Click(object sender, EventArgs e)
        {
            txtNetCost.Select(0, txtNetCost.ToString().Length);
        }

        private void txtAgencyFeePercent_Click(object sender, EventArgs e)
        {
            txtAgencyFeePercent.Select(0, txtAgencyFeePercent.ToString().Length);
        }

        private void txtAgencyFeePercent_Enter(object sender, EventArgs e)
        {
            txtAgencyFeePercent.Select(0, txtAgencyFeePercent.ToString().Length);
        }

        private void dtStartDate_CloseUp(object sender, EventArgs e)
        {
            ValidatePeriod(true, false, false);
        }

        private void dtStartDate_Leave(object sender, EventArgs e)
        {
            ValidatePeriod(true, false, false);
        }

        private void dtEndDate_CloseUp(object sender, EventArgs e)
        {
            ValidatePeriod(false, true, false);
        }

        private void dtEndDate_Leave(object sender, EventArgs e)
        {
            ValidatePeriod(false, true, false);
        }

        private void txtDescription_Leave(object sender, EventArgs e)
        {
            SetDescriptionProprietaryMedia();
        }

        private void btnRateCard_Click(object sender, EventArgs e)
        {
            Implementation_SpotPlan_Popup_RateCard frm = new Implementation_SpotPlan_Popup_RateCard("MediaType", MediaTypeCode, CampaignStartDate, CampaignEndDate);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                DataTable dtCheck = m_db.CheckFCRevenue(frm.SelectedGridRow["Media_ID"].ToString(), frm.SelectedGridRow["Media_Vendor_ID"].ToString(), drInput["Buying_Brief_ID"].ToString());
                if (dtCheck.Rows[0]["Buy"].ToString() == "True")
                {

                    txtMediaCode.Text = frm.SelectedGridRow["Media_ID"].ToString();
                    txtMedia.Text = frm.SelectedGridRow["Media_Name"].ToString();
                    m_iSpendingType = Convert.ToInt32(frm.SelectedGridRow["Spending_Type"]);

                    txtMediaTypeCode.Text = frm.SelectedGridRow["Media_Type"].ToString();
                    txtMediaType.Text = frm.SelectedGridRow["Media_Type_Name"].ToString();
                    txtMediaSubTypeCode.Text = frm.SelectedGridRow["Media_Sub_Type"].ToString();
                    txtMediaSubType.Text = frm.SelectedGridRow["Media_Sub_Type_Name"].ToString();

                    txtDescription.Text = frm.SelectedGridRow["Position"].ToString();

                    txtVendorCode.Text = frm.SelectedGridRow["Media_Vendor_ID"].ToString();
                    txtVendor.Text = frm.SelectedGridRow["Media_Vendor_Name"].ToString();

                    m_strBuyTypeName = frm.SelectedGridRow["BuyTypeName"].ToString();
                    txtBuyTypeCode.Text = frm.SelectedGridRow["BuyTypeID"].ToString();
                    txtBuyType.Text = frm.SelectedGridRow["BuyTypeDisplay"].ToString();

                    MSD = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(frm.SelectedGridRow["EffectiveDate"].ToString());
                    MED = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(frm.SelectedGridRow["InactiveDate"].ToString());
                    SD = CSD < MSD ? MSD : CSD;
                    ED = CED > MED ? MED : CED;
                    dtStartDate.Value = SD;
                    dtEndDate.Value = ED;

                    DataTable dt = m_db.SelectAdeptMediaTypeByMedia(txtMediaCode.Text);
                    if (dt.Rows.Count == 1)
                    {
                        AdeptMediaType = dt.Rows[0]["Adept_Media_Type_Name"].ToString();
                    }

                    txtNetCost.Value = Convert.ToDecimal(frm.SelectedGridRow["Net_Cost"]);
                    txtTotalPrice.Value = Convert.ToDecimal(frm.SelectedGridRow["Net_Cost"]);
                }
                else
                {
                    GMessage.MessageWarning(dtCheck.Rows[0]["Message"].ToString());
                }
            }
        }

        private void txtVendorCode_TextChanged(object sender, EventArgs e)
        {
            UpdateAgencyFeePercent();
        }

        private void txtMediaSubTypeCode_TextChanged(object sender, EventArgs e)
        {
            UpdateAgencyFeePercent();
        }

        private void txtProvince_MouseClick(object sender, MouseEventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Market_ID", "Market_Name", "Market", false, "", "");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtProvince.Text = frm.SelectedGridRow["MasterName"].ToString();
                txtProvinceCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
            }
        }

        private void txtProvince_TextChanged(object sender, EventArgs e)
        {
            if (txtProvince.Text == "")
            {
                txtProvinceCode.Text = "";
            }
        }

        private void txtProvince_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void cboCostType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateAgencyFeePercent();
        }

        private void txtH_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtW_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtH_KeyUp(object sender, KeyEventArgs e)
        {
            txtSize.Text = $"{txtH.Text}x{txtW.Text}";
            if (txtH.Text == "" && txtW.Text == "")
                txtSize.Text = drInput["SizeHW"].ToString();
        }

        private void txtW_KeyUp(object sender, KeyEventArgs e)
        {
            txtSize.Text = $"{txtH.Text}x{txtW.Text}";
            if (txtH.Text == "" && txtW.Text == "")
                txtSize.Text = drInput["SizeHW"].ToString();
        }
    }
}
