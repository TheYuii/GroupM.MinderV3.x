using GroupM.UTL;
using GroupM.DBAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupM.App.Setting.UserManagement
{
    public partial class frmUserManagementInput : frmBaseInput
    {

        #region ### Constructor ###
        public frmUserManagementInput(DataRow dr)
        {
            InitializeComponent();

            this.OnBinddingData(dr);
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
            Password,
            Domain,
            IsActive,
            CreateBy,
            CreateDate,
        }
        enum eColUserGroup
        {
            Selected,
            UserGroupMappingID,
            UserGroupID,
            UserGroupName,
        }
        #endregion

        #region ### Member ###
        int _userProfileId = 0;
        DBManager db = new DBManager();
        #endregion

        #region ### Method ###
        // Tab User
        private void OnBinddingData(DataRow dr)
        {
            try
            {
                if (dr[eColUserProfile.UserProfileID.ToString()] != null & dr[eColUserProfile.UserProfileID.ToString()].ToString() != string.Empty)
                {
                    _userProfileId = (int)dr[eColUserProfile.UserProfileID.ToString()];
                }
                else
                {
                    _userProfileId = 0;
                }
                this.txtUserName.Text = dr[eColUserProfile.UserName.ToString()].ToString();
                this.txtDisplayName.Text = dr[eColUserProfile.DisplayName.ToString()].ToString();
                this.txtFirstName.Text = dr[eColUserProfile.FirstName.ToString()].ToString();
                this.txtLastName.Text = dr[eColUserProfile.LastName.ToString()].ToString();
                this.txtPassword.Text = dr[eColUserProfile.Password.ToString()].ToString();
                this.txtDomain.Text = dr[eColUserProfile.Domain.ToString()].ToString();

                if (dr[eColUserProfile.IsActive.ToString()] != null & dr[eColUserProfile.IsActive.ToString()].ToString() != string.Empty)
                {
                    this.chkActive.Checked = (bool)dr[eColUserProfile.IsActive.ToString()];
                }
                else
                {
                    this.chkActive.Checked = false;
                }

                SendKeys.Send("{TAB}");
            }
            catch (Exception)
            {
                throw;
            }
        }
        private bool OnValidateData()
        {
            StringBuilder sbMessage = new StringBuilder();
            if (string.IsNullOrEmpty(this.txtUserName.Text.Trim()))
            {
                GMessage.MessageWarning("Please enter username.");
                this.txtUserName.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtDomain.Text.Trim()))
            {
                GMessage.MessageWarning("Please enter domain.");
                this.txtUserName.Focus();
                return false;
            }
            return true;
        }
        private bool OnCommandUpdate()
        {
            try
            {
                this.gvUserGroup.EndEdit();

                SqlConnection m_sqlconn = new SqlConnection(DAContants.ConnectionString);
                m_sqlconn.Open();

                if (m_sqlconn.State != ConnectionState.Open) return false;
                SqlTransaction tran = m_sqlconn.BeginTransaction();
                SqlCommand com = new SqlCommand("", m_sqlconn);
                com.Transaction = tran;
                string strSQL = string.Empty;

                try
                {

                    #region # Update UserProfile #
                    strSQL = @" UPDATE  UserProfile 
                                SET     UserName = @UserName,
                                        FirstName = @FirstName,
                                        LastName = @LastName,
                                        Password = @Password,
                                        Domain = @Domain,
                                        IsActive = @IsActive,
                                        UpdateBy = @UpdateBy
                                WHERE   UserProfileID = @UserProfileID";

                    com.CommandText = strSQL;
                    com.CommandType = CommandType.Text;
                    com.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar)).Value = this.txtUserName.Text.Trim();
                    com.Parameters.Add(new SqlParameter("@DisplayName", SqlDbType.NVarChar)).Value = this.txtDisplayName.Text.Trim();
                    com.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar)).Value = this.txtFirstName.Text.Trim();
                    com.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar)).Value = this.txtLastName.Text.Trim();
                    com.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar)).Value = this.txtPassword.Text.Trim();
                    com.Parameters.Add(new SqlParameter("@Domain", SqlDbType.NVarChar)).Value = this.txtDomain.Text.Trim();
                    com.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit)).Value = this.chkActive.Checked;
                    com.Parameters.Add(new SqlParameter("@UpdateBy", SqlDbType.NVarChar)).Value = GlobalVariable.UserName;
                    com.Parameters.Add(new SqlParameter("@UserProfileID", SqlDbType.Int)).Value = _userProfileId;
                    com.ExecuteNonQuery();
                    #endregion

                    if (this.gvUserGroup.Rows.Count > 0)
                    {
                        #region # Delete UserGroupMapping #
                        string userGroupID = string.Empty;
                        foreach (DataRow item in ((DataTable)this.gvUserGroup.DataSource).Rows)
                        {
                            if (item[eColUserGroup.UserGroupMappingID.ToString()] != DBNull.Value)
                            {
                                userGroupID += item[eColUserGroup.UserGroupID.ToString()].ToString() + ",";
                            }
                        }
                        if (userGroupID.Length > 0)
                        {
                            userGroupID = userGroupID.Substring(0, userGroupID.Length - 1);

                            strSQL = " DELETE FROM UserGroupMapping WHERE UserProfileID = " + _userProfileId + " AND UserGroupID IN (" + userGroupID + ") ";
                            db.ExecuteQuery(strSQL);
                        }
                        #endregion

                        #region # Insert UserGroupMapping #
                        foreach (DataRow item in ((DataTable)this.gvUserGroup.DataSource).Rows)
                        {
                            if (Convert.ToBoolean(item[eColUserGroup.Selected.ToString()]) == true)
                            {
                                strSQL = @" INSERT INTO UserGroupMapping (UserProfileID,
                                                                      UserGroupID,
                                                                      CreateBy,
                                                                      CreateDate)
                                        VALUES                       (@UserProfileID,
                                                                      @UserGroupID,
                                                                      @CreateBy,
                                                                      @CreateDate)";
                                com = new SqlCommand(strSQL, m_sqlconn);
                                com.Transaction = tran;
                                com.Parameters.Add(new SqlParameter("@UserProfileID", SqlDbType.Int)).Value = _userProfileId;
                                com.Parameters.Add(new SqlParameter("@UserGroupID", SqlDbType.Int)).Value = Convert.ToInt32(item[eColUserGroup.UserGroupID.ToString()]);
                                com.Parameters.Add(new SqlParameter("@CreateBy", SqlDbType.NVarChar)).Value = GlobalVariable.UserName;
                                com.Parameters.Add(new SqlParameter("@CreateDate", SqlDbType.DateTime)).Value = DateTime.Now;
                                com.ExecuteNonQuery();
                            }
                        }
                        #endregion
                    }
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }

                tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageInfo(ex.Message.ToString());
                return false;
            }
        }        
        private bool OnCommandInsert()
        {
            try
            {
                this.gvUserGroup.EndEdit();

                DataTable dt = new DataTable();

                SqlConnection m_sqlconn = new SqlConnection(DAContants.ConnectionString);
                m_sqlconn.Open();

                if (m_sqlconn.State != ConnectionState.Open) return false;
                SqlTransaction tran = m_sqlconn.BeginTransaction();
                SqlCommand com = new SqlCommand("", m_sqlconn);
                com.Transaction = tran;

                string strSQL = string.Empty;

                try
                {

                    #region # Validate existing #
                    strSQL = " SELECT TOP 1 1 FROM UserProfile WHERE Username = '" + this.txtUserName.Text.Trim() + "' ";
                    if (db.SelectNoParameter(strSQL).Rows.Count > 0)
                    {
                        GMessage.MessageWarning("Username: " + this.txtUserName.Text.Trim() + " has already.");
                        return false;
                    }
                    #endregion

                    #region # Insert UserProfile #
                    strSQL = @" INSERT INTO UserProfile (UserName
                                                    ,DisplayName
                                                    ,FirstName
                                                    ,LastName
                                                    ,Password
                                                    ,Domain
                                                    ,IsActive
                                                    ,CreateBy
                                                    ,CreateDate)
                            VALUES                 (@UserName
                                                    ,@DisplayName
                                                    ,@FirstName
                                                    ,@LastName
                                                    ,@Password
                                                    ,@Domain
                                                    ,@IsActive
                                                    ,@CreateBy
                                                    ,@CreateDate)
                            SELECT SCOPE_IDENTITY() AS INT ";

                    com.CommandText = strSQL;
                    com.CommandType = CommandType.Text;
                    com.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar)).Value = this.txtUserName.Text.Trim();
                    com.Parameters.Add(new SqlParameter("@DisplayName", SqlDbType.NVarChar)).Value = this.txtDisplayName.Text.Trim();
                    com.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar)).Value = this.txtFirstName.Text.Trim();
                    com.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar)).Value = this.txtLastName.Text.Trim();
                    com.Parameters.Add(new SqlParameter("@Password", SqlDbType.NVarChar)).Value = this.txtPassword.Text.Trim();
                    com.Parameters.Add(new SqlParameter("@Domain", SqlDbType.NVarChar)).Value = this.txtDomain.Text.Trim();
                    com.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit)).Value = this.chkActive.Checked;
                    com.Parameters.Add(new SqlParameter("@CreateBy", SqlDbType.NVarChar)).Value = GlobalVariable.UserName;
                    com.Parameters.Add(new SqlParameter("@CreateDate", SqlDbType.DateTime)).Value = DateTime.Now;
                    _userProfileId = Convert.ToInt32(com.ExecuteScalar());
                    #endregion
                    
                    if (this.gvUserGroup.Rows.Count > 0)
                    {

                        #region # Delete UserGroupMapping #
                        string userGroupID = string.Empty;
                        foreach (DataRow item in ((DataTable)this.gvUserGroup.DataSource).Rows)
                        {
                            if (item[eColUserGroup.UserGroupMappingID.ToString()] != DBNull.Value)
                            {
                                userGroupID += item[eColUserGroup.UserGroupID.ToString()].ToString() + ",";
                            }
                        }
                        if (userGroupID.Length > 0)
                        {
                            userGroupID = userGroupID.Substring(0, userGroupID.Length - 1);

                            strSQL = " DELETE FROM UserGroupMapping WHERE UserProfileID = " + _userProfileId + " AND UserGroupID IN (" + userGroupID + ") ";
                            db.ExecuteQuery(strSQL);
                        }
                        #endregion

                        #region # Insert UserGroupMapping #
                        foreach (DataRow item in ((DataTable)this.gvUserGroup.DataSource).Rows)
                        {
                            if (Convert.ToBoolean(item[eColUserGroup.Selected.ToString()]) == true)
                            {
                                strSQL = @" INSERT INTO UserGroupMapping (UserProfileID,
                                                                      UserGroupID,
                                                                      CreateBy,
                                                                      CreateDate)
                                        VALUES                       (@UserProfileID,
                                                                      @UserGroupID,
                                                                      @CreateBy,
                                                                      @CreateDate)";
                                com = new SqlCommand(strSQL, m_sqlconn);
                                com.Transaction = tran;
                                com.Parameters.Add(new SqlParameter("@UserProfileID", SqlDbType.Int)).Value = _userProfileId;
                                com.Parameters.Add(new SqlParameter("@UserGroupID", SqlDbType.Int)).Value = Convert.ToInt32(item[eColUserGroup.UserGroupID.ToString()]);
                                com.Parameters.Add(new SqlParameter("@CreateBy", SqlDbType.NVarChar)).Value = GlobalVariable.UserName;
                                com.Parameters.Add(new SqlParameter("@CreateDate", SqlDbType.DateTime)).Value = DateTime.Now;
                                com.ExecuteNonQuery();
                            }
                        }
                        #endregion

                    }
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }

                tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageInfo(ex.Message.ToString());
                return false;
            }
        }
        private void OnCommandClearData()
        {
            //this.tabUserProfile.SelectedIndex = 0;

            //// Tab User
            //this.txtUserName.Clear();
            //this.txtDisplayName.Clear();
            //this.txtFirstName.Clear();
            //this.txtLastName.Clear();
            //this.txtDomain.Clear();
            //this.chkActive.Checked = false;

            //// Tab Member
            //this.txtGroupName.Clear();
            //this.txtDescription.Clear();
            //this.gvUserGroup.DataSource = null;

            //this.txtUserName.Focus();
        }

        // Tab Member
        private void OnDataLoading_UserGroup()
        {
            string strCommand = string.Empty;
            strCommand = @" SELECT  CASE WHEN (UserGroupMapping.UserGroupMappingID IS NULL) THEN 0 ELSE 1 END AS Selected
                                    , UserGroupMapping.UserGroupMappingID
                                    , UserGroup.UserGroupID
                                    , UserGroup.UserGroupName
                            FROM UserGroup
                            LEFT JOIN (SELECT * FROM UserGroupMapping WHERE UserProfileID = " + _userProfileId + @") AS UserGroupmapping
                            ON UserGroup.UserGroupID = UserGroupmapping.UserGroupID
                            WHERE 0=0";

            if (!string.IsNullOrEmpty(this.txtGroupName.Text.Trim()))
            {
                strCommand += "AND UserGroup.UserGroupName LIKE '%" + this.txtGroupName.Text.Trim() + "%'";
            }

            if (!string.IsNullOrEmpty(this.txtDescription.Text.Trim()))
            {
                strCommand += "AND UserGroup.Description LIKE '%" + this.txtDescription.Text.Trim() + "%'";
            }

            DataTable dt = db.SelectNoParameter(strCommand);
            dt.Columns[eColUserGroup.Selected.ToString()].ReadOnly = false;
            this.gvUserGroup.AutoGenerateColumns = false;
            this.gvUserGroup.DataSource = dt;
        }
        #endregion

        #region ### Event ###
        private void frmUserManagementInput_Load(object sender, EventArgs e)
        {
            OnDataLoading_UserGroup();
        }
        private void tsbSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.OnValidateData() == false) return;

                if (_userProfileId.ToString() == "0")
                {
                    this.OnCommandInsert();
                }
                else
                {
                    this.OnCommandUpdate();
                }

                GMessage.MessageInfo("Save successfully.");
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }
        }
        private void btnSearchUserGroup_Click(object sender, EventArgs e)
        {
            this.OnDataLoading_UserGroup();
        }
        #endregion

    }
}
