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

namespace GroupM.App.Setting.GroupManagement
{
    public partial class frmGroupManagementInput : frmBaseInput
    {

        #region ### Constructor ###
        public frmGroupManagementInput(DataRow dr)
        {
            InitializeComponent();

            this.OnBinddingData(dr);
        }
        #endregion

        #region ### Enum ###
        enum eColUserGroup
        {
            UserGroupID,
            UserGroupName,
            Description,
            IsActive,
            CreateBy,
            CreateDate,
        }
        enum eColUserProfile
        {
            Selected,
            UserGroupMappingID,
            UserProfileID,
            UserName,
            DisplayName,
            FirstName,
            LastName,
            Domain,
        }
        enum eColUserGroupMenu
        {
            Selected,
            UserGroupMenuMappingID,
            UserMenuID,
            UserGroupID,
        }
        enum ecolUserGroupScreen
        {
            Selected,
            UserGroupScreenMappingID,
            UserScreenID,
            UserGroupID,
        }
        #endregion

        #region ### Member ###
        int _userGroupId = 0;
        public DataRow drData { get; set; }
        DBManager db = new DBManager();
        #endregion

        #region ### Method ###
        // Tab User
        private void OnBinddingData(DataRow dr)
        {
            //DataView dv = dr.Table.Copy().DefaultView;
            //dv.RowFilter = " UserGroupID = '" + dr[eColUserProfile.UserGroupID.ToString()].ToString() + "'";

            //if (dv.Count == 0) drData = dr; else drData = dv[0].Row;

            //this.txtUserGroupName.DataBindings.Add("Text", dv, "UserGroupName", false, DataSourceUpdateMode.OnPropertyChanged);
            //this.txtDescription.DataBindings.Add("Text", dv, "Description", false, DataSourceUpdateMode.OnPropertyChanged);

            //this.txtUserGroupName.Focus();
            try
            {
                if (dr[eColUserGroup.UserGroupID.ToString()] != null & dr[eColUserGroup.UserGroupID.ToString()].ToString() != string.Empty)
                {
                    _userGroupId = (int)dr[eColUserGroup.UserGroupID.ToString()];
                }
                else
                {
                    _userGroupId = 0;
                }
                this.txtUserGroupName.Text = dr[eColUserGroup.UserGroupName.ToString()].ToString();
                this.txtDescription.Text = dr[eColUserGroup.Description.ToString()].ToString();

                if (dr[eColUserGroup.IsActive.ToString()] != null & dr[eColUserGroup.IsActive.ToString()].ToString() != string.Empty)
                {
                    this.chkActive.Checked = (bool)dr[eColUserGroup.IsActive.ToString()];
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
            if (string.IsNullOrEmpty(this.txtUserGroupName.Text.Trim()))
            {
                GMessage.MessageWarning("Please enter User Group Name.");
                this.txtUserGroupName.Focus();
                return false;
            }
            return true;
        }
        private bool OnCommandUpdate()
        {
            try
            {
                this.gvUserProfile.EndEdit();
                this.gvMenuName.EndEdit();
                this.gvScreenName.EndEdit();

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
                    strSQL = " SELECT TOP 1 1 FROM UserGroup WHERE UserGroupID = '" + _userGroupId.ToString().Trim() + "' ";
                    if (db.SelectNoParameter(strSQL).Rows.Count == 0)
                    {
                        GMessage.MessageWarning("Not found user gorup " + this.txtUserGroupName.Text.Trim() + ".");
                        return false;
                    }
                    #endregion

                    #region # Update UserGroup #
                    strSQL = @" UPDATE  UserGroup
                                SET     UserGroupName = @UserGroupName,
                                        Description = @Description,
                                        IsActive = @IsActive,
                                        UpdateBy = @UpdateBy
                                WHERE   UserGroupID = @UserGroupID ";

                    com.CommandText = strSQL;
                    com.CommandType = CommandType.Text;
                    com.Parameters.Add(new SqlParameter("@UserGroupName", SqlDbType.NVarChar)).Value = this.txtUserGroupName.Text.Trim();
                    com.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar)).Value = this.txtDescription.Text.Trim();
                    com.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit)).Value = this.chkActive.Checked;
                    com.Parameters.Add(new SqlParameter("@UpdateBy", SqlDbType.NVarChar)).Value = GlobalVariable.UserName;
                    com.Parameters.Add(new SqlParameter("@UserGroupID", SqlDbType.Int)).Value = _userGroupId;
                    com.ExecuteNonQuery();
                    #endregion

                    if (gvUserProfile.Rows.Count > 0)
                    {
                        #region # Delete UserGroupMapping #
                        string userProfile = string.Empty;
                        foreach (DataRow item in ((DataTable)this.gvUserProfile.DataSource).Rows)
                        {
                            if (item[eColUserProfile.UserGroupMappingID.ToString()] != DBNull.Value)
                            {
                                userProfile += item[eColUserProfile.UserProfileID.ToString()].ToString() + ",";
                            }
                        }
                        if (userProfile.Length > 0)
                        {
                            userProfile = userProfile.Substring(0, userProfile.Length - 1);

                            strSQL = " DELETE FROM UserGroupMapping WHERE UserGroupID = " + _userGroupId + " AND userProfileID IN (" + userProfile + ") ";
                            db.ExecuteQuery(strSQL);
                        }
                        #endregion

                        #region # Insert UserGroupMapping #
                        foreach (DataRow item in ((DataTable)this.gvUserProfile.DataSource).Rows)
                        {
                            if (Convert.ToBoolean(item[eColUserProfile.Selected.ToString()]) == true)
                            {
                                strSQL = @" INSERT INTO UserGroupMapping (UserProfileID,
                                                                          UserGroupID,
                                                                          CreateBy)
                                            VALUES                       (@UserProfileID,
                                                                          @UserGroupID,
                                                                          @CreateBy)";
                                com = new SqlCommand(strSQL, m_sqlconn);
                                com.Transaction = tran;
                                com.Parameters.Add(new SqlParameter("@UserProfileID", SqlDbType.Int)).Value = Convert.ToInt32(item[eColUserProfile.UserProfileID.ToString()]); ;
                                com.Parameters.Add(new SqlParameter("@UserGroupID", SqlDbType.Int)).Value = _userGroupId;
                                com.Parameters.Add(new SqlParameter("@CreateBy", SqlDbType.NVarChar)).Value = GlobalVariable.UserName;
                                com.ExecuteNonQuery();
                            }
                        }
                        #endregion
                    }

                    if (gvMenuName.Rows.Count > 0)
                    {
                        #region # Delete UserGroupMenuMapping #
                        string userMenu = string.Empty;
                        foreach (DataRow item in ((DataTable)this.gvMenuName.DataSource).Rows)
                        {
                            if (item[eColUserGroupMenu.UserGroupMenuMappingID.ToString()] != DBNull.Value)
                            {
                                userMenu += item[eColUserGroupMenu.UserMenuID.ToString()].ToString() + ",";
                            }
                        }
                        if (userMenu.Length > 0)
                        {
                            userMenu = userMenu.Substring(0, userMenu.Length - 1);

                            strSQL = " DELETE FROM UserGroupMenuMapping WHERE UserGroupID = " + _userGroupId + " AND UserMenuID IN (" + userMenu + ") ";
                            db.ExecuteQuery(strSQL);
                        }
                        #endregion

                        #region # Insert UserGroupMenuMapping #
                        foreach (DataRow item in ((DataTable)this.gvMenuName.DataSource).Rows)
                        {
                            if (Convert.ToBoolean(item[eColUserGroupMenu.Selected.ToString()]) == true)
                            {
                                strSQL = @" INSERT INTO UserGroupMenuMapping (UserMenuID,
                                                                              UserGroupID,
                                                                              CreateBy)
                                            VALUES                           (@UserMenuID,
                                                                              @UserGroupID,
                                                                              @CreateBy)";
                                com = new SqlCommand(strSQL, m_sqlconn);
                                com.Transaction = tran;
                                com.Parameters.Add(new SqlParameter("@UserMenuID", SqlDbType.Int)).Value = Convert.ToInt32(item[eColUserGroupMenu.UserMenuID.ToString()]); ;
                                com.Parameters.Add(new SqlParameter("@UserGroupID", SqlDbType.Int)).Value = _userGroupId;
                                com.Parameters.Add(new SqlParameter("@CreateBy", SqlDbType.NVarChar)).Value = GlobalVariable.UserName;
                                com.ExecuteNonQuery();
                            }
                        }
                        #endregion
                    }

                    if (gvScreenName.Rows.Count > 0)
                    {
                        #region # Delete UserGroupScreenMapping #
                        string userScreen = string.Empty;
                        foreach (DataRow item in ((DataTable)this.gvScreenName.DataSource).Rows)
                        {
                            if (item[ecolUserGroupScreen.UserGroupScreenMappingID.ToString()] != DBNull.Value)
                            {
                                userScreen += item[ecolUserGroupScreen.UserScreenID.ToString()].ToString() + ",";
                            }
                        }
                        if (userScreen.Length > 0)
                        {
                            userScreen = userScreen.Substring(0, userScreen.Length - 1);

                            strSQL = " DELETE FROM UserGroupScreenMapping WHERE UserGroupID = " + _userGroupId + " AND UserScreenID IN (" + userScreen + ") ";
                            db.ExecuteQuery(strSQL);
                        }
                        #endregion

                        #region # Insert UserGroupScreenMapping #
                        foreach (DataRow item in ((DataTable)this.gvScreenName.DataSource).Rows)
                        {
                            if (Convert.ToBoolean(item[eColUserGroupMenu.Selected.ToString()]) == true)
                            {
                                strSQL = @" INSERT INTO UserGroupScreenMapping (UserScreenID,
                                                                                UserGroupID,
                                                                                CreateBy)
                                            VALUES                             (@UserScreenID,
                                                                                @UserGroupID,
                                                                                @CreateBy)";
                                com = new SqlCommand(strSQL, m_sqlconn);
                                com.Transaction = tran;
                                com.Parameters.Add(new SqlParameter("@UserScreenID", SqlDbType.Int)).Value = Convert.ToInt32(item[ecolUserGroupScreen.UserScreenID.ToString()]); ;
                                com.Parameters.Add(new SqlParameter("@UserGroupID", SqlDbType.Int)).Value = _userGroupId;
                                com.Parameters.Add(new SqlParameter("@CreateBy", SqlDbType.NVarChar)).Value = GlobalVariable.UserName;
                                com.ExecuteNonQuery();
                            }
                        }
                        #endregion
                    }
                }
                catch (Exception)
                {

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
                this.gvUserProfile.EndEdit();
                this.gvMenuName.EndEdit();
                this.gvScreenName.EndEdit();

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
                    strSQL = " SELECT TOP 1 1 FROM UserGroup WHERE UserGroupName = '" + this.txtUserGroupName.Text.Trim() + "' ";
                    if (db.SelectNoParameter(strSQL).Rows.Count > 0)
                    {
                        GMessage.MessageWarning("User Group: " + this.txtUserGroupName.Text.Trim() + " has already.");
                        return false;
                    }
                    #endregion

                    #region # Insert UserGroup #
                    strSQL = @" INSERT INTO UserGroup (UserGroupName
                                                      , Description
                                                      , IsActive
                                                      , CreateBy)
                                VALUES               (@UserGroupName
                                                      ,@Description
                                                      ,@IsActive
                                                      ,@CreateBy)
                                SELECT SCOPE_IDENTITY() AS INT ";

                    com.CommandText = strSQL;
                    com.CommandType = CommandType.Text;
                    com.Parameters.Add(new SqlParameter("@UserGroupName", SqlDbType.NVarChar)).Value = this.txtUserGroupName.Text.Trim();
                    com.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar)).Value = this.txtDescription.Text.Trim();
                    com.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit)).Value = this.chkActive.Checked;
                    com.Parameters.Add(new SqlParameter("@CreateBy", SqlDbType.NVarChar)).Value = GlobalVariable.UserName;
                    _userGroupId = Convert.ToInt32(com.ExecuteScalar());
                    #endregion

                    if (gvUserProfile.Rows.Count > 0)
                    {
                        #region # Delete UserGroupMapping #
                        string userProfile = string.Empty;
                        foreach (DataRow item in ((DataTable)this.gvUserProfile.DataSource).Rows)
                        {
                            if (item[eColUserProfile.UserGroupMappingID.ToString()] != DBNull.Value)
                            {
                                userProfile += item[eColUserProfile.UserProfileID.ToString()].ToString() + ",";
                            }
                        }
                        if (userProfile.Length > 0)
                        {
                            userProfile = userProfile.Substring(0, userProfile.Length - 1);

                            strSQL = " DELETE FROM UserGroupMapping WHERE UserGroupID = " + _userGroupId + " AND userProfileID IN (" + userProfile + ") ";
                            db.ExecuteQuery(strSQL);
                        }
                        #endregion

                        #region # Insert UserGroupMapping #
                        foreach (DataRow item in ((DataTable)this.gvUserProfile.DataSource).Rows)
                        {
                            if (Convert.ToBoolean(item[eColUserProfile.Selected.ToString()]) == true)
                            {
                                strSQL = @" INSERT INTO UserGroupMapping (UserProfileID,
                                                                          UserGroupID,
                                                                          CreateBy)
                                            VALUES                       (@UserProfileID,
                                                                          @UserGroupID,
                                                                          @CreateBy)";
                                com = new SqlCommand(strSQL, m_sqlconn);
                                com.Transaction = tran;
                                com.Parameters.Add(new SqlParameter("@UserProfileID", SqlDbType.Int)).Value = Convert.ToInt32(item[eColUserProfile.UserProfileID.ToString()]); ;
                                com.Parameters.Add(new SqlParameter("@UserGroupID", SqlDbType.Int)).Value = _userGroupId;
                                com.Parameters.Add(new SqlParameter("@CreateBy", SqlDbType.NVarChar)).Value = GlobalVariable.UserName;
                                com.ExecuteNonQuery();
                            }
                        }
                        #endregion
                    }

                    if (gvMenuName.Rows.Count > 0)
                    {
                        #region # Delete UserGroupMenuMapping #
                        string userMenu = string.Empty;
                        foreach (DataRow item in ((DataTable)this.gvMenuName.DataSource).Rows)
                        {
                            if (item[eColUserGroupMenu.UserGroupMenuMappingID.ToString()] != DBNull.Value)
                            {
                                userMenu += item[eColUserGroupMenu.UserMenuID.ToString()].ToString() + ",";
                            }
                        }
                        if (userMenu.Length > 0)
                        {
                            userMenu = userMenu.Substring(0, userMenu.Length - 1);

                            strSQL = " DELETE FROM UserGroupMenuMapping WHERE UserGroupID = " + _userGroupId + " AND UserMenuID IN (" + userMenu + ") ";
                            db.ExecuteQuery(strSQL);
                        }
                        #endregion

                        #region # Insert UserGroupMenuMapping #
                        foreach (DataRow item in ((DataTable)this.gvMenuName.DataSource).Rows)
                        {
                            if (Convert.ToBoolean(item[eColUserGroupMenu.Selected.ToString()]) == true)
                            {
                                strSQL = @" INSERT INTO UserGroupMenuMapping (UserMenuID,
                                                                              UserGroupID,
                                                                              CreateBy)
                                            VALUES                           (@UserMenuID,
                                                                              @UserGroupID,
                                                                              @CreateBy)";
                                com = new SqlCommand(strSQL, m_sqlconn);
                                com.Transaction = tran;
                                com.Parameters.Add(new SqlParameter("@UserMenuID", SqlDbType.Int)).Value = Convert.ToInt32(item[eColUserGroupMenu.UserMenuID.ToString()]); ;
                                com.Parameters.Add(new SqlParameter("@UserGroupID", SqlDbType.Int)).Value = _userGroupId;
                                com.Parameters.Add(new SqlParameter("@CreateBy", SqlDbType.NVarChar)).Value = GlobalVariable.UserName;
                                com.ExecuteNonQuery();
                            }
                        }
                        #endregion
                    }

                    if (gvScreenName.Rows.Count > 0)
                    {
                        #region # Delete UserGroupScreenMapping #
                        string userScreen = string.Empty;
                        foreach (DataRow item in ((DataTable)this.gvScreenName.DataSource).Rows)
                        {
                            if (item[eColUserGroupMenu.UserGroupID.ToString()] != DBNull.Value)
                            {
                                userScreen += item[eColUserProfile.UserProfileID.ToString()].ToString() + ",";
                            }
                        }
                        if (userScreen.Length > 0)
                        {
                            userScreen = userScreen.Substring(0, userScreen.Length - 1);

                            strSQL = " DELETE FROM UserGroupScreenMapping WHERE UserGroupID = " + _userGroupId + " AND UserScreenID IN (" + userScreen + ") ";
                            db.ExecuteQuery(strSQL);
                        }
                        #endregion

                        #region # Insert UserGroupScreenMapping #
                        foreach (DataRow item in ((DataTable)this.gvScreenName.DataSource).Rows)
                        {
                            if (Convert.ToBoolean(item[eColUserGroupMenu.Selected.ToString()]) == true)
                            {
                                strSQL = @" INSERT INTO UserGroupScreenMapping (UserScreenID,
                                                                                UserGroupID,
                                                                                CreateBy)
                                            VALUES                             (@UserScreenID,
                                                                                @UserGroupID,
                                                                                @CreateBy)";
                                com = new SqlCommand(strSQL, m_sqlconn);
                                com.Transaction = tran;
                                com.Parameters.Add(new SqlParameter("@UserScreenID", SqlDbType.Int)).Value = Convert.ToInt32(item[ecolUserGroupScreen.UserScreenID.ToString()]); ;
                                com.Parameters.Add(new SqlParameter("@UserGroupID", SqlDbType.Int)).Value = _userGroupId;
                                com.Parameters.Add(new SqlParameter("@CreateBy", SqlDbType.NVarChar)).Value = GlobalVariable.UserName;
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

        // Tab Member
        private void OnDataLoading_UserGroup()
        {
            string strCommand = string.Empty;
            strCommand = @" SELECT	CASE WHEN (UserGroupMapping.UserGroupMappingID IS NULL) THEN 0 ELSE 1 END AS Selected
                                    , UserGroupMapping.UserGroupMappingID
		                            , UserProfile.UserProfileID
		                            , UserProfile.UserName
		                            , UserProfile.Domain
		                            , UserProfile.IsActive
                            FROM UserProfile
                            LEFT JOIN (SELECT * FROM UserGroupMapping WHERE UserGroupID = '" + _userGroupId + @"') AS UserGroupmapping
                            ON UserProfile.UserProfileID = UserGroupMapping.UserProfileID 
                            WHERE (0=0)";

            if (!string.IsNullOrEmpty(this.txtUserName.Text.Trim()))
            {
                strCommand += "AND UserProfile.UserName LIKE '%" + this.txtUserName.Text.Trim() + "%'";
            }

            if (this.cmbStatus.SelectedIndex == 1)
            {
                strCommand += "AND UserProfile.IsActive = '1'";
            }
            else if (this.cmbStatus.SelectedIndex == 2)
            {
                strCommand += "AND UserProfile.IsActive = '0'";
            }

            DataTable dt = db.SelectNoParameter(strCommand);
            this.gvUserProfile.AutoGenerateColumns = false;
            this.gvUserProfile.DataSource = dt;
        }

        // Tab Menu
        private void OnDataLoading_MenuName()
        {
            string strCommand = string.Empty;
            strCommand = @"  SELECT	CASE WHEN (UserGroupMenuMapping.UserGroupMenuMappingID IS NULL) THEN 0 ELSE 1 END AS Selected
                                     , UserGroupMenuMapping.UserGroupMenuMappingID
                                     , UserMenu.UserMenuID
                                     , UserMenu.UserMenuName
                             FROM UserMenu
                             LEFT JOIN (SELECT * FROM UserGroupMenuMapping WHERE UserGroupID = '" + _userGroupId + @"') AS UserGroupMenuMapping
                             ON UserMenu.UserMenuID = UserGroupMenuMapping.UserMenuID  
                             WHERE (0=0)";

            if (!string.IsNullOrEmpty(this.txtMenuName.Text.Trim()))
            {
                strCommand += "AND UserMenu.UserMenuName LIKE '%" + this.txtMenuName.Text.Trim() + "%'";
            }

            DataTable dt = db.SelectNoParameter(strCommand);
            this.gvMenuName.AutoGenerateColumns = false;
            this.gvMenuName.DataSource = dt;
        }

        // Tab Screen
        private void OnDataLoading_ScreenName()
        {
            string strCommand = string.Empty;
            strCommand = @" SELECT	CASE WHEN (UserGroupScreenMapping.UserGroupScreenMappingID IS NULL) THEN 0 ELSE 1 END AS Selected 
                                    , UserGroupScreenMapping.UserGroupScreenMappingID
                                    , UserScreen.UserScreenID
                                    , UserScreen.UserScreenName
                            FROM UserScreen
                            LEFT JOIN (SELECT * FROM UserGroupScreenMapping WHERE UserGroupID = '" + _userGroupId + @"') AS UserGroupScreenMapping
                            ON UserScreen.UserScreenID = UserGroupScreenMapping.UserScreenID  
                            WHERE (0=0)";

            if (!string.IsNullOrEmpty(this.txtScreenName.Text.Trim()))
            {
                strCommand += "AND UserScreen.UserScreenName LIKE '%" + this.txtScreenName.Text.Trim() + "%'";
            }

            DataTable dt = db.SelectNoParameter(strCommand);
            this.gvScreenName.AutoGenerateColumns = false;
            this.gvScreenName.DataSource = dt;
        }

        #endregion

        #region ### Event ###
        private void frmGroupManagementInput_Load(object sender, EventArgs e)
        {
            this.cmbStatus.SelectedIndex = 0;
            this.OnDataLoading_UserGroup();
            this.OnDataLoading_MenuName();
            this.OnDataLoading_ScreenName();
        }
        private void tsbSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.OnValidateData() == false) return;

                if (_userGroupId.ToString() == "0")
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
        private void btnSearchMenuName_Click(object sender, EventArgs e)
        {
            this.OnDataLoading_MenuName();
        }
        private void btnSearchScreenName_Click(object sender, EventArgs e)
        {
            this.OnDataLoading_ScreenName();
        }
        #endregion

    }
}
