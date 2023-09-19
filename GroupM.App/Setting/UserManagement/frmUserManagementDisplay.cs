using GroupM.UTL;
using GroupM.DBAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupM.App.Setting.UserManagement
{
    public partial class frmUserManagementDisplay : frmBaseDisplay
    {

        #region ### Constructor ###
        public frmUserManagementDisplay()
        {
            InitializeComponent();
        }
        #endregion

        #region ### Enum ###
        enum eColUserProfile
        {
            UserProfileID,
            UserName,
            DisplayName,
            FirstName,
            LastName,
            Domain,
            IsActive,
            CreateBy,
            CreateDate,
        }
        #endregion

        #region ### Member ###
        DBManager db = new DBManager();
        #endregion

        #region ### Method ###
        private void OnDataLoading()
        {
            this.bsData.DataSource = db.SelectUserProfile(this.txtUsername.Text.Trim(), this.cmbUserStatus.Text.Trim());

            this.gvDetail.AutoGenerateColumns = false;
            this.gvDetail.DataSource = this.bsData;
        }
        private void OnCommandDelete()
        {
            try
            {
                int UserProfileID = Convert.ToInt32((((DataRowView)this.gvDetail.SelectedRows[0].DataBoundItem).Row[eColUserProfile.UserProfileID.ToString()]).ToString());
                db.DeleteUserProfile(UserProfileID);
                GMessage.MessageInfo("Deleted successfully.");
            }
            catch (Exception ex)
            {
                GMessage.MessageInfo(ex.Message.ToString());
            }
        }
        #endregion

        #region ### Event ###
        // ## Form_Load ##
        private void frmUserManagementDisplay_Load(object sender, EventArgs e)
        {
            this.cmbUserStatus.SelectedIndex = 0;
            this.OnDataLoading();
        }
        private void frmUserManagementDisplay_KeyDown(object sender, KeyEventArgs e)
        {
            //if (0 == 1)
            //{
            //    if (e.KeyCode == Keys.F5)
            //    {
            //        this.btnSearch.PerformClick();
            //    }
            //    else if (e.Control && e.KeyCode == Keys.N)
            //    {
            //        this.btnNew.PerformClick();
            //    }
            //    else if (e.Control && e.KeyCode == Keys.E)
            //    {
            //        this.btnEdit.PerformClick();
            //    }
            //    else if (e.Control && e.KeyCode == Keys.D)
            //    {
            //        this.btnDelete.PerformClick();
            //    }
            //}
        }

        // ## Button ##
        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.OnDataLoading();
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow dr = ((DataTable)this.bsData.DataSource).NewRow();
                dr[eColUserProfile.UserProfileID.ToString()] = 0;
                using (frmUserManagementInput frm = new frmUserManagementInput(dr))
                {
                    frm.ShowDialog();
                    this.OnDataLoading();
                }
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow dr = ((DataRowView)bsData.Current).Row;
                using (frmUserManagementInput frm = new frmUserManagementInput(dr))
                {
                    frm.ShowDialog();
                    this.OnDataLoading();
                }
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                this.OnCommandDelete();
                this.OnDataLoading();
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }
        }

        // ## DataGridView ##
        private void gvDetail_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            base.btnEdit.PerformClick();
        }
        #endregion

    }
}
