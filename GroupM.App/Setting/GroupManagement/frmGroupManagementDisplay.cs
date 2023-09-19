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

namespace GroupM.App.Setting.GroupManagement
{
    public partial class frmGroupManagementDisplay : frmBaseDisplay
    {

        #region ### Constructor ###
        public frmGroupManagementDisplay()
        {
            InitializeComponent();
        }
        #endregion

        #region ### Enum ###
        enum eColUserGroup
        {
            UserGroupID,
            UserGroupName,
            Description,
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
            string strCommand = string.Empty;
            strCommand = @" SELECT * FROM UserGroup WHERE 0=0";

            if (!string.IsNullOrEmpty(this.txtUsername.Text.Trim()))
            {
                strCommand += "AND UserGroupName LIKE '%" + this.txtUsername.Text.Trim() + "%'";
            }

            if (!string.IsNullOrEmpty(this.txtDescription.Text.Trim()))
            {
                strCommand += "AND Description LIKE '%" + this.txtDescription.Text.Trim() + "%'";
            }

            DataTable dt = db.SelectNoParameter(strCommand);

            this.bsData.DataSource = dt;

            this.gvDetail.AutoGenerateColumns = false;
            this.gvDetail.DataSource = bsData;
        }
        private void OnCommandDelete()
        {
            try
            {
                int userGroupID = Convert.ToInt32(((DataRowView)this.gvDetail.SelectedRows[0].DataBoundItem).Row[eColUserGroup.UserGroupID.ToString()].ToString());
                db.DeleteUserGroup(userGroupID);               
                GMessage.MessageInfo("Delete successfully.");
            }
            catch (Exception ex)
            {
                GMessage.MessageWarning(ex.Message.ToString());
            }
        }
        #endregion

        #region ### Event ###
        // ## Form ##
        private void frmGroupManagementDisplay_Load(object sender, EventArgs e)
        {
            this.OnDataLoading();
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
                dr[eColUserGroup.UserGroupID.ToString()] = 0;
                using (frmGroupManagementInput frm = new frmGroupManagementInput(dr))
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
                using (frmGroupManagementInput frm = new frmGroupManagementInput(dr))
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
