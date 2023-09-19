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
    public partial class Implementation_SpotPlan_TV_FC_Amendment : Form
    {
        public DataRow drInput = null;
        enum eScreenMode { Add, Edit, View }
        eScreenMode m_screenMode;
        public string Client_ID = "";
        public int m_iSpendingType = 0;
        public string MediaSubTypeCode = "";
        public string m_strMaterialID = "";
        public string CampaignStartDate = "";
        public string CampaignEndDate = "";
        private DateTime CSD;
        private DateTime CED;
        private DateTime SD;
        private DateTime ED;
        DBManager m_db = null;

        public Implementation_SpotPlan_TV_FC_Amendment(string screenMode, string strMst)
        {
            InitializeComponent();
            MediaSubTypeCode = strMst;
            if (screenMode == "Add")
                m_screenMode = eScreenMode.Add;
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
                    dtStartDate.Enabled = false;
                    dtEndDate.Enabled = false;
                    txtMaterial.Enabled = false;
                    txtImpression.ReadOnly = true;
                    txtGRP.ReadOnly = true;
                    txtCPRP.ReadOnly = true;
                    txtTotalCost.ReadOnly = true;
                    txtDiscount.ReadOnly = true;
                    btnOK.Visible = false;
                    btnCancel.Visible = false;
                    break;
            }
            m_screenMode = mode;
        }

        private bool SetDescriptionProprietaryMedia()
        {
            //Modified by Chaiwat.i 16/11/2021 TFS 139631 : T2: Media Inventory for all Media
            bool set = true;
            DataTable dt = m_db.CheckOptIn_ContractType_Spotplan(txtVendorCode.Text, txtMediaCode.Text);
            if (dt.Rows.Count > 0)
            {
                //Checking media name including "Proprietary Media" or not ?
                string txtDesc = txtDescription.Text.Trim().ToUpper();
                bool endsWithSearchResult1 = txtDesc.EndsWith("PROPRIETARY MEDIA", System.StringComparison.CurrentCultureIgnoreCase);
                bool endsWithSearchResult2 = txtDesc.EndsWith("MEDIA INVENTORY", System.StringComparison.CurrentCultureIgnoreCase);
                string txtM = txtMedia.Text.Trim().ToUpper();
                bool endsWithSearchResult_M = txtM.EndsWith("PROPRIETARY MEDIA", System.StringComparison.CurrentCultureIgnoreCase);
                if (txtDesc.Contains("MEDIA INVENTORY"))
                {
                    txtDescription.Text = Regex.Replace(txtDescription.Text, "media inventory", ":PROPRIETARY MEDIA", RegexOptions.IgnoreCase);
                }
                if (txtDesc.Contains("INVENTORY MEDIA"))
                {
                    txtDescription.Text = Regex.Replace(txtDescription.Text, "inventory media", ":PROPRIETARY MEDIA", RegexOptions.IgnoreCase);
                }
                if (endsWithSearchResult_M == false && endsWithSearchResult1 == false)
                {
                    txtDescription.Text += ":PROPRIETARY MEDIA";
                }
                if (txtDescription.Text != "" &&
                    (endsWithSearchResult1 == false || txtDesc.Substring(txtDesc.Length - 17, 17) != "PROPRIETARY MEDIA") &&
                    (endsWithSearchResult2 == false || txtDesc.Substring(txtDesc.Length - 15, 15) != "MEDIA INVENTORY") &&
                    txtMedia.Text.IndexOf("PROPRIETARY MEDIA", StringComparison.OrdinalIgnoreCase) == 0)
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
            }
            return valid;
        }

        private decimal ConvertObjectToDecimal(object obj)
        {
            if (obj == DBNull.Value || obj == null)
                return 0;
            return Convert.ToDecimal(obj.ToString());
        }

        private void RefreshNetCost()
        {
            txtNetCost.Value = txtTotalCost.Value - (txtTotalCost.Value * txtDiscount.Value / 100.00m);
        }

        private bool CheckDataBeforeClose()
        {
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
            if (m_db.CheckOptIn(txtVendorCode.Text, txtMediaCode.Text).Rows.Count > 0)
            {
                DataTable dt = m_db.CheckOptIn(txtVendorCode.Text, txtMediaCode.Text, Client_ID);
                if (dt.Rows.Count == 0)
                {
                    GMessage.MessageWarning($"This client can not buy media sub type({txtMediaSubType.Text}) with vendor({txtVendor.Text}). Please contact commercial team to check set up.");
                    return false;
                }
                else
                {
                    DataRow dr = dt.Rows[0];
                    DateTime dtOptInStartDate = Convert.ToDateTime(dr["Opt_in_StartDate"]);
                    DateTime dtOptInEndDate = Convert.ToDateTime(dr["Opt_in_EndDate"]);

                    if (!(dtOptInStartDate <= dtStartDate.Value && dtEndDate.Value <= dtOptInEndDate))
                    {
                        GMessage.MessageWarning($"This client can not buy media sub type({txtMediaSubType.Text}) with vendor({txtVendor.Text}). The client have been set up period Opt-in between {dtOptInStartDate.ToString("dd/MM/yyyy")} and {dtOptInEndDate.ToString("dd/MM/yyyy")}. Please contact commercial team to check set up.");
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
            return true;
        }

        private void Implementation_SpotPlan_TV_FC_Amendment_Load(object sender, EventArgs e)
        {
            CSD = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(CampaignStartDate);
            CED = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(CampaignEndDate);
            if (m_screenMode == eScreenMode.Add)
            {
                SD = CSD;
                ED = CED;
            }
            if (m_screenMode == eScreenMode.Edit || m_screenMode == eScreenMode.View)
            {
                txtMediaSubType.Text = drInput["MediaSubTypeName"].ToString();
                txtMediaSubTypeCode.Text = drInput["Media_Sub_Type"].ToString();
                txtMedia.Text = drInput["MediaName"].ToString();
                txtMediaCode.Text = drInput["Media_ID"].ToString();
                txtVendor.Text = drInput["MediaVendorName"].ToString();
                txtVendorCode.Text = drInput["Media_Vendor_ID"].ToString();
                txtDescription.Text = drInput["Program"].ToString();

                if (drInput["Start_Date"] == DBNull.Value)
                {
                    drInput["Start_Date"] = DateTime.Now.ToString("yyyyMMdd");
                    drInput["End_Date"] = DateTime.Now.ToString("yyyyMMdd");
                }
                SD = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(drInput["Start_Date"].ToString());
                ED = DateTimeConvertUtil.DateTimeConvertyyyyMMddToDateTime(drInput["End_Date"].ToString());

                m_strMaterialID = drInput["Material_ID"].ToString();
                txtMaterial.Text = drInput["MaterialName"].ToString();
                txtMaterialCode.Text = drInput["Material_Key"].ToString();

                txtImpression.Value = ConvertObjectToDecimal(drInput["Market_Price"]);
                txtGRP.Value = ConvertObjectToDecimal(drInput["Rating"]);
                txtCPRP.Value = ConvertObjectToDecimal(drInput["CPRP_Cost"]);
                txtTotalCost.Value = ConvertObjectToDecimal(drInput["Rate"]);
                txtDiscount.Value = ConvertObjectToDecimal(drInput["Discount"]);
                txtNetCost.Value = ConvertObjectToDecimal(drInput["Net_Cost"]);
                txtAgencyFeePercent.Value = ConvertObjectToDecimal(drInput["AgencyFeePercent"]);
            }
            dtStartDate.Value = SD;
            dtEndDate.Value = ED;
        }

        private void txtMedia_MouseClick(object sender, MouseEventArgs e)
        {
            Implementation_SpotPlan_Popup_Media frm = new Implementation_SpotPlan_Popup_Media("MediaSubType", MediaSubTypeCode);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtMediaCode.Text = frm.SelectedGridRow["Media_ID"].ToString();
                txtMedia.Text = frm.SelectedGridRow["Short_Name"].ToString();
                m_iSpendingType = Convert.ToInt32(frm.SelectedGridRow["Spending_Type"]);

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
            }
        }

        private void txtMedia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtMedia_TextChanged(object sender, EventArgs e)
        {
            if (txtMedia.Text == "")
            {
                txtMediaCode.Text = "";
            }
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

        private void txtVendor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtVendor_TextChanged(object sender, EventArgs e)
        {
            if (txtVendor.Text == "")
            {
                txtVendorCode.Text = "";
            }
        }

        private void txtDescription_Leave(object sender, EventArgs e)
        {
            SetDescriptionProprietaryMedia();
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

        private void txtMaterial_Click(object sender, EventArgs e)
        {
            Implementation_SpotPlan_Popup_Material frm = new Implementation_SpotPlan_Popup_Material(drInput["Buying_Brief_ID"].ToString());
            frm.IncludeInactive = true;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                m_strMaterialID = frm.SelectedGridRow["Material_ID"].ToString();
                txtMaterialCode.Text = frm.SelectedGridRow["Material_Key"].ToString();
                txtMaterial.Text = frm.SelectedGridRow["Material_Name"].ToString();
            }
        }

        private void txtMaterial_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtMaterial_TextChanged(object sender, EventArgs e)
        {
            if (txtMaterial.Text == "")
            {
                txtMaterialCode.Text = "";
            }
        }

        private void txtImpression_Click(object sender, EventArgs e)
        {
            txtImpression.Select(0, txtImpression.ToString().Length);
        }

        private void txtImpression_Enter(object sender, EventArgs e)
        {
            txtImpression.Select(0, txtImpression.ToString().Length);
        }

        private void txtGRP_Click(object sender, EventArgs e)
        {
            txtGRP.Select(0, txtGRP.ToString().Length);
        }

        private void txtGRP_Enter(object sender, EventArgs e)
        {
            txtGRP.Select(0, txtGRP.ToString().Length);
        }

        private void txtCPRP_Click(object sender, EventArgs e)
        {
            txtCPRP.Select(0, txtCPRP.ToString().Length);
        }

        private void txtCPRP_Enter(object sender, EventArgs e)
        {
            txtCPRP.Select(0, txtCPRP.ToString().Length);
        }

        private void txtTotalCost_Click(object sender, EventArgs e)
        {
            txtTotalCost.Select(0, txtTotalCost.ToString().Length);
        }

        private void txtTotalCost_ValueChanged(object sender, EventArgs e)
        {
            RefreshNetCost();
        }

        private void txtTotalCost_Enter(object sender, EventArgs e)
        {
            txtTotalCost.Select(0, txtTotalCost.ToString().Length);
        }

        private void txtDiscount_Click(object sender, EventArgs e)
        {
            txtDiscount.Select(0, txtDiscount.ToString().Length);
        }

        private void txtDiscount_ValueChanged(object sender, EventArgs e)
        {
            RefreshNetCost();
        }

        private void txtDiscount_Enter(object sender, EventArgs e)
        {
            txtDiscount.Select(0, txtDiscount.ToString().Length);
        }

        private void txtAgencyFeePercent_Click(object sender, EventArgs e)
        {
            txtAgencyFeePercent.Select(0, txtAgencyFeePercent.ToString().Length);
        }

        private void txtAgencyFeePercent_Enter(object sender, EventArgs e)
        {
            txtAgencyFeePercent.Select(0, txtAgencyFeePercent.ToString().Length);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!CheckDataBeforeClose())
                return;
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
