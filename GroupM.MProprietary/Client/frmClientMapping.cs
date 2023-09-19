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
    public partial class frmClientMapping : Form
    {
        public frmClientMapping(string ClientID, string ClientName)
        {
            InitializeComponent();
            clientID = ClientID;
            clientName = ClientName;
        }
        
        private DBAccess connect = new DBAccess();
        private string clientID = "";
        private string clientName = "";
        private DataTable dtGroup;
        private DataTable dtProprietary;
        private DataTable dtOriginal;
        private DataTable dtDelete;
        private int indexDelete = 0;
        private bool multipleDelete = false;
        private bool cancelDelete = false;
        private bool editData = false;
        private bool saveData = false;
        private bool close = false;

        private void DataLoading()
        {
            //Header
            txtClientID.Text = clientID;
            txtClientName.Text = clientName;
            //Detail (Proprietary group)
            dtGroup = connect.SelectProprietaryGroupMapping(clientID);
            gvDetailGroup.AutoGenerateColumns = false;
            gvDetailGroup.DataSource = dtGroup;
            //Detail (Proprietary)
            dtProprietary = connect.SelectProprietaryDetailMapping(clientID);
            gvDetailProprietary.AutoGenerateColumns = false;
            gvDetailProprietary.DataSource = dtProprietary;
            // copy data
            dtOriginal = dtGroup.Copy();
            dtDelete = dtGroup.Clone();
        }

        private void RefreshProprietary()
        {
            if (dtGroup.Rows.Count > 0)
            {
                string filter = ",";
                for (int i = 0; i < dtGroup.Rows.Count; i++)
                {
                    if (dtGroup.Rows[i].RowState == DataRowState.Deleted)
                    {
                        continue;
                    }
                    filter += dtGroup.Rows[i]["GroupProprietaryId"].ToString() + ",";
                }
                dtProprietary.Clear();
                dtProprietary = connect.SelectProprietaryDetailMappingRefresh(filter);
                gvDetailProprietary.DataSource = dtProprietary;
            }
        }

        private void RefreshData()
        {
            //Detail (Proprietary group)
            dtGroup.Clear();
            dtGroup = connect.SelectProprietaryGroupMapping(clientID);
            gvDetailGroup.DataSource = dtGroup;
            //Detail (Proprietary)
            dtProprietary.Clear();
            dtProprietary = connect.SelectProprietaryDetailMapping(clientID);
            gvDetailProprietary.DataSource = dtProprietary;
            // copy data
            dtOriginal = dtGroup.Copy();
        }

        private void OnCommandSave()
        {
            try
            {
                // proprietary group has no data
                if (gvDetailGroup.Rows.Count == 0)
                {
                    MessageBox.Show("Client must contain at least one proprietary group!", "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tabData.SelectedTab = tabGroup;
                    btnAdd.Focus();
                }
                else // proprietary group have data
                {
                    // save group mapping
                    int dlg = 0;
                    for (int i = 0; i < dtGroup.Rows.Count; i++)
                    {
                        // check row insert or delete
                        if (dtGroup.Rows[i].RowState != DataRowState.Unchanged)
                        {
                            // check row add
                            if (dtGroup.Rows[i].RowState == DataRowState.Added)
                            {
                                // if this row is not found on grid view (before add row), add this row
                                string strCondition = string.Format("GroupProprietaryId = '{0}'", dtGroup.Rows[i]["GroupProprietaryId"].ToString());
                                DataRow[] rowsFind = dtOriginal.Select(strCondition);
                                if (rowsFind.Count() == 0)
                                {
                                    connect.InsertClientMapping((int)dtGroup.Rows[i]["GroupProprietaryId"], clientID);
                                }
                            }
                            else
                            {
                                // check row delete
                                if (dtGroup.Rows[i].RowState == DataRowState.Deleted)
                                {
                                    // if this row is found on grid view (before delete row), delete this row
                                    string strCondition = string.Format("GroupProprietaryId = '{0}'", dtDelete.Rows[dlg]["GroupProprietaryId"].ToString());
                                    DataRow[] rowsFind = dtOriginal.Select(strCondition);
                                    if (rowsFind.Count() > 0)
                                    {
                                        connect.DeleteClientMapping((int)dtDelete.Rows[dlg]["GroupProprietaryId"], clientID);
                                    }
                                    dlg++;
                                }
                            }
                        }
                    }
                    // reset variable for next save
                    RefreshData();
                    saveData = true;
                    MessageBox.Show("Done.", "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Exit()
        {
            // not click button save before close
            if (saveData == false)
            {
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

        private void frmClientMapping_Load(object sender, EventArgs e)
        {
            DataLoading();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string filter = ",";
            for (int i = 0; i < dtGroup.Rows.Count; i++)
            {
                if (dtGroup.Rows[i].RowState == DataRowState.Deleted)
                {
                    continue;
                }
                filter += dtGroup.Rows[i]["GroupProprietaryId"].ToString() + ",";
            }
            frmProprietaryGroupList frm = new frmProprietaryGroupList();
            frm.filter = filter;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < frm.SelectedGridRow.Count; i++)
                {
                    DataRow drInput = (DataRow)frm.SelectedGridRow[i];
                    DataRow dr = dtGroup.NewRow();
                    dr.ItemArray = drInput.ItemArray;
                    dtGroup.Rows.Add(dr);
                }
                RefreshProprietary();
                editData = true;
            }
        }

        private void gvDetailGroup_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            // select 1 row
            if (gvDetailGroup.SelectedRows.Count == 1)
            {
                // delete 1 row
                if (multipleDelete == false)
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to delete Proprietary Group? ", "MProprietary", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        DataRow dr = ((DataRowView)e.Row.DataBoundItem).Row;
                        dtDelete.Rows.Add(dr.ItemArray);
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                else // delete multiple row (last row)
                {
                    DataRow dr = ((DataRowView)e.Row.DataBoundItem).Row;
                    dtDelete.Rows.Add(dr.ItemArray);
                    indexDelete = 0;
                    multipleDelete = false;
                }
            }
            else // select multiple row
            {
                // delete multiple row (first row)
                if (indexDelete == 0)
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to delete Proprietary Group? ", "MProprietary", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        DataRow dr = ((DataRowView)e.Row.DataBoundItem).Row;
                        dtDelete.Rows.Add(dr.ItemArray);
                        multipleDelete = true;
                    }
                    else
                    {
                        e.Cancel = true;
                        cancelDelete = true;
                    }
                }
                else // delete multiple row (not first row)
                {
                    if (cancelDelete == true)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        DataRow dr = ((DataRowView)e.Row.DataBoundItem).Row;
                        dtDelete.Rows.Add(dr.ItemArray);
                    }
                }
                indexDelete++;
                // reset variable for cancel delete (last row)
                if (cancelDelete == true)
                {
                    if (indexDelete == gvDetailGroup.SelectedRows.Count)
                    {
                        indexDelete = 0;
                        cancelDelete = false;
                    }
                }
            }
        }

        private void gvDetailGroup_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            RefreshProprietary();
            editData = true;
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

        private void frmClientMapping_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (close == false)
            {
                Exit();
            }
        }
    }
}
