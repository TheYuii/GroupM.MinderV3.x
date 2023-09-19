using GroupM.DBAccess;
using GroupM.UTL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupM.Minder
{
    public partial class Master_Product : Form
    {
        enum eScreenMode { Add, Edit, View }
        public DataRow drProduct { get; set; }

        private string username = "";

        private Regex regex = new Regex(@"[^a-zA-Z\d\s]");

        eScreenMode m_screemMode;

        DBManager m_db;

        private void SetScreenMode(eScreenMode mode)
        {
            switch (mode)
            {
                case eScreenMode.Add:
                    btnDelete.Visible = false;
                    break;
                case eScreenMode.Edit:
                    txtClient.ReadOnly = true;
                    txtClient.BackColor = SystemColors.Control;
                    txtProductCode.ReadOnly = true;
                    break;
                case eScreenMode.View:
                    txtProductCode.ReadOnly = true;
                    txtDisplayName.ReadOnly = true;
                    txtThaiName.ReadOnly = true;
                    txtEnglishName.ReadOnly = true;
                    txtClient.ReadOnly = true;

                    txtRemark.ReadOnly = true;
                    rdActive.Enabled = false;
                    rdInactive.Enabled = false;
                   
                    saveToolStripMenuItem.Enabled = false;
                    btnDelete.Enabled = false;
                    break;
            }
            m_screemMode = mode;
        }

        public Master_Product(string screenMode, string Username)
        {
            InitializeComponent();
            username = Username;
            m_db = new DBManager();
            if (screenMode == "Add")
                SetScreenMode(eScreenMode.Add);
            else if (screenMode == "Edit")
                SetScreenMode(eScreenMode.Edit);
            else
                SetScreenMode(eScreenMode.View);
        }

        private DataTable TableEmail(string column)
        {
            DataTable table = new DataTable();
            table.Columns.Add(column, typeof(string));
            return table;
        }

        private void InitControl()
        {
   
        }

        private DataTable TextToTable(string column, string listEmail)
        {
            DataTable table = TableEmail(column);
            string[] emails = listEmail.Replace(" ", "").Split(';');
            for (int i = 0; i < emails.Length; i++)
                if (emails[i] != "")
                    table.Rows.Add(emails[i]);
            return table;
        }

        private void Dataloading()
        {
            DataTable dt = m_db.SelectProduct(drProduct["Product_ID"].ToString());
            DataRow dr = dt.Rows[0];


            txtClient.Text = dr["Client_Name"].ToString();
            txtClientCode.Text = dr["Client_ID"].ToString();
            txtAgency.Text = dr["Agency_Name"].ToString();
            txtAgencyCode.Text = dr["Agency_ID"].ToString();
            txtOffice.Text = dr["Office_Name"].ToString();
            txtOfficeCode.Text = dr["Office_ID"].ToString();
            txtGPMMapping.Text = dr["GPM_Client_Name"].ToString();
            txtGPMMappingCode.Text = dr["GPM_CLIENT_CODE"].ToString();


            txtBrand.Text = dr["Brand_Name"].ToString();
            txtBrandCode.Text = dr["Brand_ID"].ToString();

            txtCategory.Text = dr["Category_Name"].ToString();
            txtCategoryCode.Text = dr["Category_ID"].ToString();




            txtProductCode.Text = dr["Product_ID"].ToString();
            txtDisplayName.Text = dr["Short_Name"].ToString();
            txtThaiName.Text = dr["Thai_Name"].ToString();
            txtEnglishName.Text = dr["English_Name"].ToString();
            
            txtRemark.Text = dr["Comment"].ToString();
            if( Convert.ToBoolean(dr["Valid"]))
            {
                rdActive.Checked = true;
                rdInactive.Checked = false;
               
            }
            else
            {
                rdActive.Checked = false;
                rdInactive.Checked = true;
             
            }

            DataTable dtAgencyFee = m_db.SelectAgencyFeeByProduct(drProduct["Product_ID"].ToString());
            gvAgencyFee.AutoGenerateColumns = false;
            gvAgencyFee.DataSource = dtAgencyFee;

        }

        private void Master_Product_Load(object sender, EventArgs e)
        {
            InitControl();
            if (m_screemMode == eScreenMode.Edit || m_screemMode == eScreenMode.View)
                Dataloading();
        }

        private void TxtOfficeCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)3 && e.KeyChar != (char)22 && e.KeyChar != (char)24 && regex.IsMatch(e.KeyChar.ToString()))
            {
                e.Handled = true;
            }
        }

        private void TxtAgency_Click(object sender, EventArgs e)
        {
            
        }

        private void TxtAgency_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void TxtAgency_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Clipboard.Clear();
            }
        }

        private void TxtAgency_TextChanged(object sender, EventArgs e)
        {
            if (txtClient.Text == "")
            {
                txtClientCode.Text = "";
            }
        }

        private void RdActive_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void RdInactive_CheckedChanged(object sender, EventArgs e)
        {
          
        }

        private bool CheckDataBeforeSave()
        {
            if (m_screemMode == eScreenMode.Add)
            {
                if (txtProductCode.Text == "")
                {
                    GMessage.MessageWarning("Please input Product Code.");
                    txtProductCode.Focus();
                    return false;
                }
                if (txtBrandCode.Text == "")
                {
                    GMessage.MessageWarning("Please input Brand.");
                    txtProductCode.Focus();
                    return false;
                }
                if (txtCategoryCode.Text == "")
                {
                    GMessage.MessageWarning("Please input Category.");
                    txtProductCode.Focus();
                    return false;
                }
                if (m_db.SelectProduct(txtProductCode.Text).Rows.Count != 0)
                {
                    GMessage.MessageWarning(string.Format("Please change Product Code. The {0} already exist.", txtProductCode.Text));
                    txtProductCode.Focus();
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
            if (txtClientCode.Text == "")
            {
                GMessage.MessageWarning("Please input Agency Name.");
                txtClient.Focus();
                return false;
            }
          
            return true;
        }

        private DataRow Packing()
        {
            DataTable dt = m_db.SelectProduct("dummycolumn");
            DataRow dr = dt.NewRow();
            dr["Agency_ID"] = txtAgencyCode.Text;
            dr["Office_ID"] = txtOfficeCode.Text;
            dr["Client_ID"] = txtClientCode.Text;
            dr["Product_ID"] = txtProductCode.Text;
            dr["Thai_Name"] = txtThaiName.Text;
            dr["English_Name"] = txtEnglishName.Text;
            dr["Short_Name"] = txtDisplayName.Text;
            dr["Category_ID"] = txtCategoryCode.Text;
            dr["Brand_ID"] = txtBrandCode.Text;
            dr["Valid"] = rdActive.Checked?1:0;
            dr["Comment"] = txtRemark.Text;
            dr["User_ID"] = username;
            dr["Modify_Date"] = DateTime.Now.ToString("yyyyMMdd"); 
            dr["GPM_PRODUCT_CODE"] = txtGPMMappingCode.Text;

            dr["Product_Referrence_ID"] = txtProductCode.Text;
            dr["GPM_Product_Code_Tmp"] = txtGPMMappingCode.Text;

            return dr;
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
                    if (m_db.InsertProduct(Packing()) == -1)
                        return false;
                    else
                        log = $"Add Product:{txtProductCode.Text}({txtDisplayName.Text}),Client:{txtClientCode.Text}({txtClient.Text}),Agency:{txtAgencyCode.Text}({txtAgencyCode.Text})";
                    m_db.InsertLogMinder(username.Replace(".", ""), log, "Master File", username.Replace(".", ""));

                    foreach (DataRow dr in dtAgencyFee.Rows)
                    {
                        if (dr.RowState == DataRowState.Deleted)
                            continue;
                        m_db.InsertProductAgencyFee(dr, txtProductCode.Text);
                        log = $"Add Product:{txtProductCode.Text}({txtDisplayName.Text}),Add Agency Fee Detail:{dr["Agency_Fee_Set_Up_Name"].ToString()}({dr["description"].ToString()}) Fee({dr["Agency_Fee"].ToString()}) Edit({dr["Editable"].ToString()})";
                        m_db.InsertLogMinder(username.Replace(".", ""), log, "Master File", username.Replace(".", ""), "Agency_Fee", "", dr["Agency_Fee"].ToString());
                    }
                }
                else
                {
                    m_db.UpdateProduct(Packing());
                    log = $"Modify Product:{txtProductCode.Text}({txtDisplayName.Text}),Client:{txtClientCode.Text}({txtClient.Text}),Agency:{txtAgencyCode.Text}({txtAgencyCode.Text})";
                    m_db.InsertLogMinder(username.Replace(".", ""), log, "Master File", username.Replace(".", ""));

                    
                    DataTable delRows = new DataView(dtAgencyFee, null, null, DataViewRowState.Deleted).ToTable();
                    foreach (DataRow dr in delRows.Rows)
                    {
                        m_db.DeleteProductAgencyFee(Convert.ToInt32(dr["Product_Agency_Fee_ID"]));
                        log = $"Modify Product:{txtProductCode.Text}({txtDisplayName.Text}),Delete Agency Fee Detail:{dr["Agency_Fee_Set_Up_Name"].ToString()}({dr["description"].ToString()}) Fee({dr["Agency_Fee"].ToString()}) Edit({dr["Editable"].ToString()})";
                        m_db.InsertLogMinder(username.Replace(".", ""), log, "Master File", username.Replace(".", ""), "Agency_Fee", dr["Agency_Fee"].ToString(),"");
                    }
                    foreach (DataRow dr in dtAgencyFee.Rows)
                    {
                        if (dr.RowState == DataRowState.Added)
                        {
                            m_db.InsertProductAgencyFee(dr, txtProductCode.Text);

                            log = $"Modify Product:{txtProductCode.Text}({txtDisplayName.Text}),Add Agency Fee Detail:{dr["Agency_Fee_Set_Up_Name"].ToString()}({dr["description"].ToString()}) Fee({dr["Agency_Fee"].ToString()}) Edit({dr["Editable"].ToString()})";
                            m_db.InsertLogMinder(username.Replace(".", ""), log, "Master File", username.Replace(".", ""), "Agency_Fee", "", dr["Agency_Fee"].ToString());
                        }
                        else if (dr.RowState == DataRowState.Modified)
                        {
                            m_db.UpdateProductAgencyFee(dr);
                            log = $"Modify Product:{txtProductCode.Text}({txtDisplayName.Text}),Update Agency Fee Detail:{dr["Agency_Fee_Set_Up_Name"].ToString()}({dr["description"].ToString()}) Fee({ dr["Agency_Fee", DataRowVersion.Original].ToString()}>{dr["Agency_Fee"].ToString()}) Edit({dr["Editable", DataRowVersion.Original].ToString()}>{dr["Editable"].ToString()})";
                            m_db.InsertLogMinder(username.Replace(".", ""), log, "Master File", username.Replace(".", ""), "Agency_Fee", dr["Agency_Fee", DataRowVersion.Original].ToString(), dr["Agency_Fee"].ToString());
                        }
                    }
                }
                
                GMessage.MessageInfo("Save Completed");
                return true;
            }
            catch (Exception e)
            {
                GMessage.MessageError(e);
                return false;
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OnCommandSave())
                DialogResult = DialogResult.Yes;
        }

        private void CancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to exit?", "Product", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Dispose();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (m_db.SelectProductIsUsing(txtProductCode.Text))
            {
                string str = $"Cannot delete Product Code: {txtProductCode.Text} - {txtDisplayName.Text}  because some Buying Brief(s) referred to this Product.";
                GMessage.MessageWarning(str);
                return;
            }
            DialogResult result = MessageBox.Show("Are you sure to delete Product: " + txtProductCode.Text + " - " + txtDisplayName.Text + "?", "Product", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                m_db.DeleteProduct(txtProductCode.Text);
                m_db.InsertLogMinder(username.Replace(".", ""), "Delete Product ID:" + txtProductCode.Text + ",Name :" + txtDisplayName.Text, "Master File", username.Replace(".", ""));
                GMessage.MessageInfo("Delete Completed");
                DialogResult = DialogResult.Yes;
            }
        }

        private void txtDisplayName_TextChanged(object sender, EventArgs e)
        {
            if (m_screemMode == eScreenMode.Add)
            {
                txtEnglishName.Text = txtDisplayName.Text;
                txtThaiName.Text = txtDisplayName.Text;
            }
        }

        private void btnAddSpecificFee_Click(object sender, EventArgs e)
        {
            if (txtProductCode.Text == "")
            {
                GMessage.MessageWarning("Please input product first.");
                return;
            }
            Master_ClientAgencyFee frm = new Master_ClientAgencyFee();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                
                DataRow dr = ((DataTable)gvAgencyFee.DataSource).NewRow();
                dr["Product_ID"] = "";
                if (frm.rdMediaType.Checked)
                {
                    dr["Priority"] = 2;
                    dr["Agency_Fee"] = "0.0000";
                    dr["Editable"] = false;
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
                    dr["Agency_Fee_Set_Up_Column"] = "[{\"Column\":\"Media_Type\",\"Value\":\"OD\"},{\"Column\":\"Cost_Type\",\"Value\":\"" + frm.cboOutdoorCostType.Text + "\"}]";
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

        private void txtBrand_Click(object sender, EventArgs e)
        {
            if (m_screemMode == eScreenMode.Add || m_screemMode == eScreenMode.Edit)
            {
                if (txtClientCode.Text == "")
                {
                    GMessage.MessageWarning("Please select client first.");
                    return;
                }
                Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Brand_ID", "Brand_Name", "Brand", false, "", txtClientCode.Text);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    txtBrand.Text = frm.SelectedGridRow["MasterName"].ToString();
                    txtBrandCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
                }
            }
        }

        private void txtCategory_Click(object sender, EventArgs e)
        {
            if (m_screemMode == eScreenMode.Add || m_screemMode == eScreenMode.Edit)
            {
                if (txtClientCode.Text == "")
                {
                    GMessage.MessageWarning("Please select client first.");
                    return;
                }
                Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("c.Category_ID", "c.Category_Name", "Product_Category", false, "", txtClientCode.Text);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    txtCategory.Text = frm.SelectedGridRow["MasterName"].ToString();
                    txtCategoryCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
                }
            }
        }

        private void txtClient_Click(object sender, EventArgs e)
        {
            if (m_screemMode == eScreenMode.Add)
            {
                Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Client_ID", "Short_Name", "Client", false, "", "");
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    txtClient.Text = frm.SelectedGridRow["MasterName"].ToString();
                    txtClientCode.Text = frm.SelectedGridRow["MasterCode"].ToString();

                    txtCategory.Text = "";
                    txtCategoryCode.Text = "";

                    DataTable dtAgency = m_db.SelectAgency(frm.SelectedGridRow["Agency_ID"].ToString());
                    txtAgency.Text = dtAgency.Rows[0]["Short_Name"].ToString();
                    txtAgencyCode.Text = frm.SelectedGridRow["Agency_ID"].ToString();

                    DataTable dtOffice = m_db.SelectOffice(frm.SelectedGridRow["Office_ID"].ToString());
                    txtOffice.Text = dtOffice.Rows[0]["Short_Name"].ToString();
                    txtOfficeCode.Text = frm.SelectedGridRow["Office_ID"].ToString();

                    if (frm.SelectedGridRow["GPM_CLIENT_CODE"] != DBNull.Value
                        && frm.SelectedGridRow["GPM_CLIENT_CODE"].ToString() != "")
                    {
                        DataTable dtGPMMapping = m_db.SelectClient(frm.SelectedGridRow["GPM_CLIENT_CODE"].ToString());
                        txtGPMMapping.Text = dtGPMMapping.Rows[0]["Short_Name"].ToString();
                        txtGPMMappingCode.Text = frm.SelectedGridRow["GPM_CLIENT_CODE"].ToString();
                    }

                    DataTable dtAgencyFee = m_db.SelectAgencyFeeByClient(txtClientCode.Text);
                    dtAgencyFee.Columns["Client_ID"].ColumnName = "Product_ID";
                    gvAgencyFee.AutoGenerateColumns = false;
                    gvAgencyFee.DataSource = dtAgencyFee;



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
                DialogResult result = MessageBox.Show("Are you sure to delete the Agency Fee?", "Product", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    dr.Delete();
                    //((DataTable)gvAgencyFee.DataSource).Rows.Remove(dr);
                }
            }
        }

        private void txtBrand_TextChanged(object sender, EventArgs e)
        {
            if (txtBrand.Text == "" )
            {
                txtBrandCode.Text = "";
            }
        }

        private void txtCategory_TextChanged(object sender, EventArgs e)
        {
            if (txtCategory.Text == "")
            {
                txtCategoryCode.Text = "";
            }
        }

        private void gvAgencyFee_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            GMessage.MessageWarning("Please enter a valid Agency Commission (%).");
        }

        private void gvAgencyFee_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 2)
                {
                    gvAgencyFee.EndEdit();
                    if (gvAgencyFee.CurrentRow.Cells[2].Value.ToString() == "")
                    {
                        gvAgencyFee.CurrentRow.Cells[2].Value = "0.0000";
                    }
                    else
                    {
                        double fee = Convert.ToDouble(gvAgencyFee.CurrentRow.Cells[2].Value);
                        if (fee < 0 || fee > 100)
                        {
                            gvAgencyFee.CurrentRow.Cells[2].Value = "0.0000";
                            GMessage.MessageWarning("Please enter an Agency Commission (%) from 0 to 100.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
