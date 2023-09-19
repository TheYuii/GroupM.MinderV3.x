using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MProprietary
{
    public partial class frmProprietaryMapping : Form
    {
        public frmProprietaryMapping(DataRow dr)
        {
            InitializeComponent();
            drGroupProp = dr;
            groupID = (int)dr["GroupProprietaryId"];
        }
        
        private DBAccess connect = new DBAccess();
        private DataRow drGroupProp;
        private int groupID = 0;
        private string groupName;
        private DataTable dtProprietary;
        private DataTable dtClient;
        private DataTable dtOriginalProprietary;
        private DataTable dtOriginalClient;
        private DataTable dtDeleteProprietary;
        private DataTable dtDeleteClient;
        private int indexDeleteProprietary = 0;
        private int indexDeleteClient = 0;
        private bool multipleDeleteProprietary = false;
        private bool multipleDeleteClient = false;
        private bool cancelDeleteProprietary = false;
        private bool cancelDeleteClient = false;
        private bool editData = false;
        private bool saveData = false;
        private bool close = false;
        
        private void DataLoading()
        {
            //Header
            txtGroupName.Text = drGroupProp["GroupProprietaryName"].ToString();
            txtDescription.Text = drGroupProp["GroupProprietaryDescription"].ToString();
            cboContractType.Text = drGroupProp["ContractType"].ToString();
            //Detail (Proprietary)
            dtProprietary = connect.SelectProprietaryMapping(groupID);
            gvDetailProprietary.AutoGenerateColumns = false;
            gvDetailProprietary.DataSource = dtProprietary;
            //Detail (Client)
            dtClient = connect.SelectClientMapping(groupID);
            gvDetailClient.AutoGenerateColumns = false;
            gvDetailClient.DataSource = dtClient;
            // copy data
            groupName = drGroupProp["GroupProprietaryName"].ToString();
            dtOriginalProprietary = dtProprietary.Copy();
            dtDeleteProprietary = dtProprietary.Clone();
            dtOriginalClient = dtClient.Copy();
            dtDeleteClient = dtClient.Clone();
        }

        private void RefreshData()
        {
            //Detail (Proprietary)
            dtProprietary.Clear();
            dtProprietary = connect.SelectProprietaryMapping(groupID);
            gvDetailProprietary.DataSource = dtProprietary;
            //Detail (Client)
            dtClient.Clear();
            dtClient = connect.SelectClientMapping(groupID);
            gvDetailClient.DataSource = dtClient;
            // copy data
            dtOriginalProprietary = dtProprietary.Copy();
            dtOriginalClient = dtClient.Copy();
        }

        private void OnCommandSave()
        {
            try
            {
                if (cboContractType.Text.Trim() == "")
                {
                    MessageBox.Show("Please select Contract Type", "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // group name is blank
                if (txtGroupName.Text.Trim() == "")
                {
                    MessageBox.Show("Proprietary Group Name is require!", "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else // group name is not blank
                {
                    // proprietary has no data
                    if (gvDetailProprietary.Rows.Count == 0)
                    {
                        // group is not exist
                        if (groupID == 0)
                        {
                            MessageBox.Show("Proprietary group must contain at least one permission!", "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            tabData.SelectedTab = tabProprietary;
                            btnAddProprietary.Focus();
                        }
                        else // group is exist
                        {
                            DialogResult dialogResult = MessageBox.Show("Do you want to delete this Proprietary Group? ", "MProprietary", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialogResult == DialogResult.Yes)
                            {
                                // delete group
                                connect.DeleteProprietaryGroup(groupID);
                                saveData = true;
                                MessageBox.Show("Done.", "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Dispose();
                            }
                            else
                            {
                                MessageBox.Show("Proprietary group must contain at least one permission!", "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                tabData.SelectedTab = tabProprietary;
                                btnAddProprietary.Focus();
                            }
                        }
                    }
                    else // proprietary have data
                    {
                        // check duplicate group name
                        DataTable dtCheck = connect.SelectProprietaryGroup(groupName, txtGroupName.Text);
                        // not duplicate group name
                        if (dtCheck.Rows.Count == 0)
                        {
                            // set group ID and group name, save group information
                            if (groupID == 0)
                            {
                                // create group ID
                                connect.InsertProprietaryGroup(txtGroupName.Text, txtDescription.Text,cboContractType.Text);
                                DataTable dtFind = connect.SelectProprietaryGroup(txtGroupName.Text);
                                if (dtFind.Rows.Count > 0)
                                {
                                    // set group ID
                                    groupID = (int)dtFind.Rows[0]["GroupProprietaryId"];
                                }
                            }
                            else
                            {
                                // update group information
                                connect.UpdateProprietaryGroup(groupID, groupName, txtGroupName.Text, txtDescription.Text,cboContractType.Text);
                            }
                            groupName = txtGroupName.Text;
                            // save proprietary mapping
                            int dlp = 0;
                            for (int i = 0; i < dtProprietary.Rows.Count; i++)
                            {
                                // check row insert or delete
                                if (dtProprietary.Rows[i].RowState != DataRowState.Unchanged)
                                {
                                    // check row add
                                    if (dtProprietary.Rows[i].RowState == DataRowState.Added)
                                    {
                                        // if this row is not found on grid view (before add row), add this row
                                        string strCondition = string.Format("GroupProprietaryVendorId = '{0}'", dtProprietary.Rows[i]["GroupProprietaryVendorId"].ToString());
                                        DataRow[] rowsFind = dtOriginalProprietary.Select(strCondition);
                                        if (rowsFind.Count() == 0)
                                        {
                                            connect.InsertProprietaryMapping(groupID, (int)dtProprietary.Rows[i]["GroupProprietaryVendorId"]);
                                        }
                                    }
                                    else
                                    {
                                        // check row delete
                                        if (dtProprietary.Rows[i].RowState == DataRowState.Deleted)
                                        {
                                            // if this row is found on grid view (before delete row), delete this row
                                            string strCondition = string.Format("GroupProprietaryVendorId = '{0}'", dtDeleteProprietary.Rows[dlp]["GroupProprietaryVendorId"].ToString());
                                            DataRow[] rowsFind = dtOriginalProprietary.Select(strCondition);
                                            if (rowsFind.Count() > 0)
                                            {
                                                connect.DeleteProprietaryMapping(groupID, (int)dtDeleteProprietary.Rows[dlp]["GroupProprietaryVendorId"]);
                                            }
                                            dlp++;
                                        }
                                    }
                                }
                            }
                            // save client mapping
                            int dlc = 0;
                            for (int i = 0; i < dtClient.Rows.Count; i++)
                            {
                                // check row insert or delete
                                if (dtClient.Rows[i].RowState != DataRowState.Unchanged)
                                {
                                    // check row add
                                    if (dtClient.Rows[i].RowState == DataRowState.Added)
                                    {
                                        // if this row is not found on grid view (before add row), add this row
                                        string strCondition = string.Format("Client_Id = '{0}'", dtClient.Rows[i]["Client_Id"].ToString());
                                        DataRow[] rowsFind = dtOriginalClient.Select(strCondition);
                                        if (rowsFind.Count() == 0)
                                        {
                                            connect.InsertClientMapping(groupID, dtClient.Rows[i]["Client_Id"].ToString());
                                        }
                                    }
                                    else
                                    {
                                        // check row delete
                                        if (dtClient.Rows[i].RowState == DataRowState.Deleted)
                                        {
                                            // if this row is found on grid view (before delete row), delete this row
                                            string strCondition = string.Format("Client_Id = '{0}'", dtDeleteClient.Rows[dlc]["Client_Id"].ToString());
                                            DataRow[] rowsFind = dtOriginalClient.Select(strCondition);
                                            if (rowsFind.Count() > 0)
                                            {
                                                connect.DeleteClientMapping(groupID, dtDeleteClient.Rows[dlc]["Client_Id"].ToString());
                                            }
                                            dlc++;
                                        }
                                    }
                                }
                            }
                            // reset variable for next save
                            RefreshData();
                            saveData = true;
                            MessageBox.Show("Done.", "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            // duplicate group name
                            MessageBox.Show("This group name is exist, Please use another group name!", "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Exit()
        {
            // not click button save before close
            if (saveData == false)
            {
                if (txtGroupName.Text != drGroupProp["GroupProprietaryName"].ToString() || txtDescription.Text != drGroupProp["GroupProprietaryDescription"].ToString())
                {
                    editData = true;
                }
                if (editData == true)
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to save? ", "MProprietary", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        OnCommandSave();
                    }
                }
            }
        }

        private void frmProprietaryMapping_Load(object sender, EventArgs e)
        {
            DataLoading();
            txtGroupName.Focus();
        }

        private void btnAddProprietary_Click(object sender, EventArgs e)
        {
            // create string filter
            string filter = ",";
            for (int i = 0; i < dtProprietary.Rows.Count; i++)
            {
                if (dtProprietary.Rows[i].RowState == DataRowState.Deleted)
                {
                    continue;
                }
                filter += dtProprietary.Rows[i]["GroupProprietaryVendorId"].ToString() + ",";
            }
            // show pop up list proprietary
            frmProprietaryList frm = new frmProprietaryList();
            frm.filter = filter;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < frm.SelectedGridRow.Count; i++)
                {
                    // insert new proprietary from list
                    DataRow drInput = (DataRow)frm.SelectedGridRow[i];
                    DataRow dr = dtProprietary.NewRow();
                    dr.ItemArray = drInput.ItemArray;
                    dtProprietary.Rows.Add(dr);
                }
                editData = true;
            }
        }

        private void gvDetailProprietary_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            // select 1 row
            if (gvDetailProprietary.SelectedRows.Count == 1)
            {
                // delete 1 row
                if (multipleDeleteProprietary == false)
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to delete Proprietary? ", "MProprietary", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        DataRow dr = ((DataRowView)e.Row.DataBoundItem).Row;
                        dtDeleteProprietary.Rows.Add(dr.ItemArray);
                        editData = true;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                else // delete multiple row (last row)
                {
                    DataRow dr = ((DataRowView)e.Row.DataBoundItem).Row;
                    dtDeleteProprietary.Rows.Add(dr.ItemArray);
                    editData = true;
                    indexDeleteProprietary = 0;
                    multipleDeleteProprietary = false;
                }
            }
            else // select multiple row
            {
                // delete multiple row (first row)
                if (indexDeleteProprietary == 0)
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to delete Proprietary? ", "MProprietary", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        DataRow dr = ((DataRowView)e.Row.DataBoundItem).Row;
                        dtDeleteProprietary.Rows.Add(dr.ItemArray);
                        multipleDeleteProprietary = true;
                    }
                    else
                    {
                        e.Cancel = true;
                        cancelDeleteProprietary = true;
                    }
                }
                else // delete multiple row (not first row)
                {
                    if (cancelDeleteProprietary == true)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        DataRow dr = ((DataRowView)e.Row.DataBoundItem).Row;
                        dtDeleteProprietary.Rows.Add(dr.ItemArray);
                    }
                }
                indexDeleteProprietary++;
                // reset variable for cancel delete (last row)
                if (cancelDeleteProprietary == true)
                {
                    if (indexDeleteProprietary == gvDetailProprietary.SelectedRows.Count)
                    {
                        indexDeleteProprietary = 0;
                        cancelDeleteProprietary = false;
                    }
                }
            }
        }

        private void btnAddClient_Click(object sender, EventArgs e)
        {
            // create string filter
            string filter = ",";
            for (int i = 0; i < dtClient.Rows.Count; i++)
            {
                if (dtClient.Rows[i].RowState == DataRowState.Deleted)
                {
                    continue;
                }
                filter += dtClient.Rows[i]["Client_Id"].ToString() + ",";
            }
            // show pop up list client
            frmClientList frm = new frmClientList();
            frm.filter = filter;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < frm.SelectedGridRow.Count; i++)
                {
                    // insert new client from list
                    DataRow drInput = (DataRow)frm.SelectedGridRow[i];
                    DataRow dr = dtClient.NewRow();
                    dr["Client_Id"] = drInput["Client_Id"];
                    dr["Short_Name"] = drInput["Short_Name"];
                    dtClient.Rows.Add(dr);
                }
                editData = true;
            }
        }

        private void gvDetailClient_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            // select 1 row
            if (gvDetailClient.SelectedRows.Count == 1)
            {
                // delete 1 row
                if (multipleDeleteClient == false)
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to delete Client? ", "MProprietary", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        DataRow dr = ((DataRowView)e.Row.DataBoundItem).Row;
                        dtDeleteClient.Rows.Add(dr.ItemArray);
                        editData = true;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                else // delete multiple row (last row)
                {
                    DataRow dr = ((DataRowView)e.Row.DataBoundItem).Row;
                    dtDeleteClient.Rows.Add(dr.ItemArray);
                    editData = true;
                    indexDeleteClient = 0;
                    multipleDeleteClient = false;
                }
            }
            else // select multiple row
            {
                // delete multiple row (first row)
                if (indexDeleteClient == 0)
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to delete Client? ", "MProprietary", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        DataRow dr = ((DataRowView)e.Row.DataBoundItem).Row;
                        dtDeleteClient.Rows.Add(dr.ItemArray);
                        multipleDeleteClient = true;
                    }
                    else
                    {
                        e.Cancel = true;
                        cancelDeleteClient = true;
                    }
                }
                else // delete multiple row (not first row)
                {
                    if (cancelDeleteClient == true)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        DataRow dr = ((DataRowView)e.Row.DataBoundItem).Row;
                        dtDeleteClient.Rows.Add(dr.ItemArray);
                    }
                }
                indexDeleteClient++;
                // reset variable for cancel delete (last row)
                if (cancelDeleteClient == true)
                {
                    if (indexDeleteClient == gvDetailClient.SelectedRows.Count)
                    {
                        indexDeleteClient = 0;
                        cancelDeleteClient = false;
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            OnCommandSave();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Exit();
            Dispose();
            close = true;
        }

        private void frmProprietaryMapping_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (close == false)
            {
                Exit();
            }
        }
    }
}
