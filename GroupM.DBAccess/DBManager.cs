using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GroupM.UTL;
using Newtonsoft.Json;

namespace GroupM.DBAccess
{
    public class DBManager
    {
        private SqlConnection m_conn = null;
        private SqlConnection m_connMinder = null;
        public SqlConnection conn
        {
            get
            {
                OpenConnection();
                return m_conn;
            }
        }

        public DBManager()
        {
            m_conn = new SqlConnection(DAContants.ConnectionString);
            m_connMinder = new SqlConnection(DAContants.ConnectionStringMinder);
            OpenConnection();

        }

        public void OpenConnection()
        {
            if (m_conn == null)
                m_conn = new SqlConnection(DAContants.ConnectionString);
            if (m_conn.State != System.Data.ConnectionState.Open)
                m_conn.Open();

            if (m_connMinder == null)
                m_connMinder = new SqlConnection(DAContants.ConnectionStringMinder);
            if (m_connMinder.State != System.Data.ConnectionState.Open)
                m_connMinder.Open();
        }

        public DataTable SelectNoParameter(string strSQL)
        {
            SqlDataAdapter oda = new SqlDataAdapter(strSQL, conn);
            DataSet ds = new DataSet();
            oda.Fill(ds);
            return ds.Tables[0];
        }

        public bool ExecuteQuery(string strSQL)
        {
            try
            {
                SqlCommand com = new SqlCommand(strSQL, conn);
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // ### User Profile ###
        public DataTable SelectUserProfile(string username, string userStatus)
        {
            string strSQL = string.Empty;

            strSQL = @" SELECT * FROM UserProfile 
                            WHERE (0=0) ";

            if (!string.IsNullOrEmpty(username))
            {
                strSQL += " AND Username LIKE '%" + username + "%'";
            }
            if (!string.IsNullOrEmpty(userStatus) && userStatus.ToLower() == "active")
            {
                strSQL += " AND IsActive ='1'";
            }
            else if (!string.IsNullOrEmpty(userStatus) && userStatus.ToLower() == "inactive")
            {
                strSQL += " AND IsActive ='0'";
            }

            return SelectNoParameter(strSQL);
        }
        public DataTable CheckUserProfileExisting(string username)
        {
            m_conn = new SqlConnection(DAContants.ConnectionStringMPAServerConfig);
            string strSQL = string.Empty;
            //strSQL = " SELECT TOP 1 * FROM UserProfile WHERE Username = '" + username + "'";
            strSQL = " SELECT TOP 1 * FROM Users WHERE User_ID = '" + username.Replace(".", "") + "'";
            return SelectNoParameter(strSQL);
        }
        public DataTable CheckUserProfileExistingMainApp(string username)
        {
            m_conn = new SqlConnection(DAContants.ConnectionString);
            string strSQL = string.Empty;
            strSQL = " SELECT TOP 1 * FROM UserProfile WHERE Username = '" + username + "'";
            return SelectNoParameter(strSQL);
        }
        public int GetUserProfileIDByUsername(string username)
        {
            string strSQL = string.Empty;
            strSQL = " SELECT TOP 1 UserProfileID FROM UserProfile WHERE Username = '" + username + "'";
            DataTable result = SelectNoParameter(strSQL);
            if (result.Rows.Count > 0)
            {
                return Convert.ToInt32(result.Rows[0]["UserProfileID"]);
            }
            else
            {
                return 0;
            }
        }

        public bool InsertUserProfile(string username, string displayName, string firstName, string lastName, string domain, bool isActive, string loginName)
        {
            try
            {
                if (CheckUserProfileExisting(username).Rows.Count > 0)
                {
                    throw new Exception("Username: " + username + " has already.");
                }

                string strSQL = string.Empty;
                strSQL = @" INSERT INTO UserProfile (UserName
                                                    ,DisplayName
                                                    ,FirstName
                                                    ,LastName
                                                    ,Domain
                                                    ,IsActive
                                                    ,CreateBy)
                            VALUES                 (@UserName
                                                    ,@DisplayName
                                                    ,@FirstName
                                                    ,@LastName
                                                    ,@Domain
                                                    ,@IsActive
                                                    ,@CreateBy)";

                SqlCommand com = new SqlCommand(strSQL, conn);
                com.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar)).Value = username;
                com.Parameters.Add(new SqlParameter("@DisplayName", SqlDbType.NVarChar)).Value = displayName;
                com.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar)).Value = firstName;
                com.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar)).Value = lastName;
                com.Parameters.Add(new SqlParameter("@Domain", SqlDbType.NVarChar)).Value = domain;
                com.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit)).Value = isActive;
                com.Parameters.Add(new SqlParameter("@CreateBy", SqlDbType.NVarChar)).Value = loginName;
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool UpdateUserProfile(int userProfileID, string username, string displayName, string firstName, string lastName, string domain, bool isActive, string loginName)
        {
            try
            {
                if (CheckUserProfileExisting(username).Rows.Count == 0)
                {
                    throw new Exception("Username: " + username + " does not exist.");
                }

                string strSQL = string.Empty;
                strSQL = @" UPDATE UserProfile (UserName
                                               ,DisplayName
                                               ,FirstName
                                               ,LastName
                                               ,Domain
                                               ,IsActive
                                               ,UpdateBy)
                            VALUES             (@UserName
                                               ,@DisplayName
                                               ,@FirstName
                                               ,@LastName
                                               ,@Domain
                                               ,@IsActive
                                               ,@UpdateBy)
                            WHERE  UserProfileID = @UserProfileID ";

                SqlCommand com = new SqlCommand(strSQL, conn);
                com.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar)).Value = username;
                com.Parameters.Add(new SqlParameter("@DisplayName", SqlDbType.NVarChar)).Value = displayName;
                com.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar)).Value = firstName;
                com.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar)).Value = lastName;
                com.Parameters.Add(new SqlParameter("@Domain", SqlDbType.NVarChar)).Value = domain;
                com.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit)).Value = isActive;
                com.Parameters.Add(new SqlParameter("@UpdateBy", SqlDbType.NVarChar)).Value = loginName;
                com.Parameters.Add(new SqlParameter("@UserProfileID", SqlDbType.Int)).Value = userProfileID;
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool DeleteUserProfile(int userProfileID)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = @" DELETE FROM UserProfile WHERE  UserProfileID = @UserProfileID ";

                SqlCommand com = new SqlCommand(strSQL, conn);
                com.Parameters.Add(new SqlParameter("@UserProfileID", SqlDbType.Int)).Value = userProfileID;
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }



        // ### User Group ###
        public DataTable SelectUserGroup(string userGorupName, string userGroupStatus)
        {
            string strSQL = string.Empty;

            strSQL = @" SELECT * FROM UserGroup 
                            WHERE (0=0) ";

            if (!string.IsNullOrEmpty(userGorupName))
            {
                strSQL += " AND UserGroupName LIKE '%" + userGorupName + "%'";
            }
            if (!string.IsNullOrEmpty(userGroupStatus) && userGroupStatus.ToLower() != "active")
            {
                strSQL += " AND IsActive ='1'";
            }
            else if (!string.IsNullOrEmpty(userGroupStatus) && userGroupStatus.ToLower() != "inactive")
            {
                strSQL += " AND IsActive ='0'";
            }

            return SelectNoParameter(strSQL);
        }
        public DataTable CheckUserGroupExisting(string userGroupName)
        {
            string strSQL = string.Empty;
            strSQL = " SELECT TOP 1 * FROM UserGroup WHERE UserGroupName = '" + userGroupName + "'";
            return SelectNoParameter(strSQL);
        }
        public bool UserGroupIsMapping(int userGroupID)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = @" SELECT TOP 1 1 FROM UserGroup
                            INNER JOIN UserGroupMapping
                            ON UserGroup.UserGroupID = UserGroupMapping.UserGroupID
                            WHERE UserGroup.UserGroupID = '" + userGroupID + "'";
                if (SelectNoParameter(strSQL).Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool InsertUserGroup(string userGroupName, string description, bool isActive, string loginName)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = @" INSERT INTO UserProfile (UserGroupName
                                                    ,Description
                                                    ,IsActive
                                                    ,CreateBy)
                            VALUES                 (@UserGroupName
                                                    ,@Description
                                                    ,@IsActive
                                                    ,@CreateBy)";

                SqlCommand com = new SqlCommand(strSQL, conn);
                com.Parameters.Add(new SqlParameter("@UserGroupName", SqlDbType.NVarChar)).Value = userGroupName;
                com.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar)).Value = description;
                com.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit)).Value = isActive;
                com.Parameters.Add(new SqlParameter("@CreateBy", SqlDbType.NVarChar)).Value = loginName;
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool UpdateUserGroup(int userGroupID, string userGroupName, string description, bool isActive, string loginName)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = @" UPDATE UserProfile (UserGroupName
                                               ,Description
                                               ,IsActive
                                               ,UpdateBy)
                            VALUES             (@UserGroupName
                                               ,@Description
                                               ,@IsActive
                                               ,@UpdateBy)
                            WHERE  UserGroupID = @UserGroupID ";

                SqlCommand com = new SqlCommand(strSQL, conn);
                com.Parameters.Add(new SqlParameter("@UserGroupName", SqlDbType.NVarChar)).Value = userGroupName;
                com.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar)).Value = description;
                com.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit)).Value = isActive;
                com.Parameters.Add(new SqlParameter("@UpdateBy", SqlDbType.NVarChar)).Value = loginName;
                com.Parameters.Add(new SqlParameter("@UserGroupID", SqlDbType.Int)).Value = userGroupID;
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }

        }
        public bool DeleteUserGroup(int userGroupID)
        {
            try
            {
                if (UserGroupIsMapping(userGroupID) == true)
                {
                    throw new Exception("This group has been use in anothor users.");
                }

                string strSQL = string.Empty;
                strSQL = @" DELETE FROM UserGroup WHERE  UserGroupID = @UserGroupID ";

                SqlCommand com = new SqlCommand(strSQL, conn);
                com.Parameters.Add(new SqlParameter("@UserGroupID", SqlDbType.Int)).Value = userGroupID;
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }

        // ### SetScreenPermission ###
        public bool SetScreenPermission(string permissionName, string userName)
        {
            //m_conn = new SqlConnection(DAContants.ConnectionStringMPAServerConfig);
            try
            {
                string strSQL = string.Empty;
                strSQL = @" SELECT TOP 1 1 FROM UserGroupScreenMapping 
                            WHERE UserScreenID = (SELECT TOP 1 USERSCREENID FROM UserScreen WHERE UserScreenName = @UserScreenName)
                            AND UserGroupID IN (SELECT UserGroupID FROM UserGroupMapping 
                                                WHERE UserProfileID = (SELECT TOP 1 UserProfileID FROM UserProfile WHERE IsActive = 1 AND UserName = @UserName))";
                SqlDataAdapter da = new SqlDataAdapter(strSQL, conn);
                da.SelectCommand.Parameters.Add("@UserScreenName", SqlDbType.VarChar).Value = permissionName;
                da.SelectCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userName;

                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }
        }

        public int InsertLog(string strLoginName, string strComputerName, string strScreenName, string strAction, string strDesc)
        {
            m_conn = new SqlConnection(DAContants.ConnectionString);
            string strSQL = @"INSERT INTO SystemLog(LoginName, ComputerName, ScreenName, Action, Description) VALUES (@LoginName, @ComputerName, @ScreenName, @Action, @Description)";
            SqlCommand comm = new SqlCommand(strSQL, conn);
            comm.Parameters.Add("@LoginName", SqlDbType.VarChar).Value = strLoginName;
            comm.Parameters.Add("@ComputerName", SqlDbType.VarChar).Value = strComputerName;
            comm.Parameters.Add("@ScreenName", SqlDbType.VarChar).Value = strScreenName;
            comm.Parameters.Add("@Action", SqlDbType.VarChar).Value = strAction;
            comm.Parameters.Add("@Description", SqlDbType.VarChar).Value = strDesc;
            return comm.ExecuteNonQuery();
        }

        public int InsertLogMinder(string strUserID, string strDescription, string strActionName, string strADLogin)
        {
            m_conn = new SqlConnection(DAContants.ConnectionString);
            string strSQL = @"INSERT INTO Log(User_ID, Action_Time, Description, System_Description, Action_Name, Ad_Login, PC_name) VALUES (@User_ID, @Action_Time, @Description, @System_Description, @Action_Name, @Ad_Login, @PC_name)";
            SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
            comm.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = strUserID;
            comm.Parameters.Add("@Action_Time", SqlDbType.VarChar).Value = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            comm.Parameters.Add("@Description", SqlDbType.VarChar).Value = strDescription;
            comm.Parameters.Add("@System_Description", SqlDbType.VarChar).Value = "Minder";
            comm.Parameters.Add("@Action_Name", SqlDbType.VarChar).Value = strActionName;
            comm.Parameters.Add("@Ad_Login", SqlDbType.VarChar).Value = strADLogin;
            comm.Parameters.Add("@PC_name", SqlDbType.VarChar).Value = DBNull.Value;
            return comm.ExecuteNonQuery();
        }

        public int InsertLogMinder(
          string strUserID,
          string strDescription,
          string strActionName,
          string strADLogin,
          string strFieldchange,
          string strFieldChangeBefore,
          string strFieldChangeAfter)
        {
            this.m_conn = new SqlConnection(DAContants.ConnectionString);
            SqlCommand sqlCommand = new SqlCommand("INSERT INTO Log(User_ID, Action_Time, Description, System_Description, Action_Name, Ad_Login, PC_name,Field_Change,Field_Change_Before,Field_Change_After) VALUES (@User_ID, @Action_Time, @Description, @System_Description, @Action_Name, @Ad_Login, @PC_name,@Field_Change,@Field_Change_Before,@Field_Change_After)", this.m_connMinder);
            sqlCommand.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = (object)strUserID;
            sqlCommand.Parameters.Add("@Action_Time", SqlDbType.VarChar).Value = (object)DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            sqlCommand.Parameters.Add("@Description", SqlDbType.VarChar).Value = (object)strDescription;
            sqlCommand.Parameters.Add("@System_Description", SqlDbType.VarChar).Value = (object)"Minder";
            sqlCommand.Parameters.Add("@Action_Name", SqlDbType.VarChar).Value = (object)strActionName;
            sqlCommand.Parameters.Add("@Ad_Login", SqlDbType.VarChar).Value = (object)strADLogin;
            sqlCommand.Parameters.Add("@PC_name", SqlDbType.VarChar).Value = (object)DBNull.Value;
            sqlCommand.Parameters.Add("@Field_Change", SqlDbType.VarChar).Value = (object)strFieldchange;
            sqlCommand.Parameters.Add("@Field_Change_Before", SqlDbType.VarChar).Value = (object)strFieldChangeBefore;
            sqlCommand.Parameters.Add("@Field_Change_After", SqlDbType.VarChar).Value = (object)strFieldChangeAfter;
            return sqlCommand.ExecuteNonQuery();
        }


        public bool ValidatePermissionBB(string strBBID, string strUser)
        {
            DataTable dt = new DataTable();
            string strSQL = @"ValidateBuyingBriefPermission";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBBID;
            sda.SelectCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = strUser;
            sda.SelectCommand.Parameters.Add("@Permission", SqlDbType.Bit).Direction = ParameterDirection.Output;
            sda.Fill(dt);
            bool valid = Convert.ToBoolean(sda.SelectCommand.Parameters["@Permission"].Value);
            return valid;
        }

        public bool ValidatePermissionScreenMaster(string strScreenName, string strScreenMode, string strUser)
        {
            DataTable dt = new DataTable();
            string strSQL = @"ValidateScreenPermission";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sda.SelectCommand.Parameters.Add("@Screen", SqlDbType.VarChar).Value = strScreenName;
            sda.SelectCommand.Parameters.Add("@Mode", SqlDbType.VarChar).Value = strScreenMode;
            sda.SelectCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = strUser;
            sda.SelectCommand.Parameters.Add("@Permission", SqlDbType.Bit).Direction = ParameterDirection.Output;
            sda.Fill(dt);
            bool valid = Convert.ToBoolean(sda.SelectCommand.Parameters["@Permission"].Value);
            return valid;
        }

        public bool ValidatePermissionScreenSpotPlan(string strBBID, string strVersion, string permission)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select * 
            from Spot_Plan_Version 
            where Buying_Brief_ID = @BB_ID 
            and Version = @Version";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@BB_ID", SqlDbType.VarChar).Value = strBBID;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                DataTable dtBB = new DataTable();
                strSQL = @"select * 
                from Buying_Brief 
                where Buying_Brief_ID = @BB_ID";
                sda = new SqlDataAdapter(strSQL, m_connMinder);
                sda.SelectCommand.Parameters.Add("@BB_ID", SqlDbType.VarChar).Value = strBBID;
                sda.Fill(dtBB);
                if (dtBB.Rows.Count == 0)
                    return false;
                else
                {
                    if ((dtBB.Rows[0]["Version_Approve"].ToString() == "0" || dtBB.Rows[0]["Version_Approve"].ToString() == "") && permission == "Edit")
                        return true;
                    else
                        return false;
                }
            }
            else
            {
                if ((dt.Rows[0]["Approve"].ToString() == "0" || dt.Rows[0]["Approve"].ToString() == "5") && permission == "Edit")
                    return true;
                else if ((dt.Rows[0]["Approve"].ToString() == "0" || dt.Rows[0]["Approve"].ToString() == "4" || dt.Rows[0]["Approve"].ToString() == "5" || dt.Rows[0]["Approve"].ToString() == "8") && permission == "View")
                    return true;
                else
                    return false;
            }
        }

        public int InsertOffice(DataRow dr)
        {
            try
            {
                string strSQL = @"INSERT INTO [dbo].[Office]
                ([Office_ID]
                ,[Thai_Name]
                ,[English_Name]
                ,[Short_Name]
                ,[Agency_ID]
                ,[Address]
                ,[Tel]
                ,[Fax]
                ,[User_ID]
                ,[Modify_Date]
                ,[Creative_Agency_ID]
                ,[IsActive]
                ,[InactiveDate]
                ,[To_Email]
                ,[CC_Email]
                ,[Remark])
                VALUES
                (@Office_ID
                ,@Thai_Name
                ,@English_Name
                ,@Short_Name
                ,@Agency_ID
                ,@Address
                ,@Tel
                ,@Fax
                ,@User_ID
                ,@Modify_Date
                ,@Creative_Agency_ID
                ,@IsActive
                ,@InactiveDate
                ,@To_Email
                ,@CC_Email
                ,@Remark)";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Office_ID", SqlDbType.VarChar).Value = dr["Office_ID"];
                comm.Parameters.Add("@Thai_Name", SqlDbType.VarChar).Value = dr["Thai_Name"];
                comm.Parameters.Add("@English_Name", SqlDbType.VarChar).Value = dr["English_Name"];
                comm.Parameters.Add("@Short_Name", SqlDbType.VarChar).Value = dr["Short_Name"];
                comm.Parameters.Add("@Agency_ID", SqlDbType.VarChar).Value = dr["Agency_ID"];
                comm.Parameters.Add("@Address", SqlDbType.VarChar).Value = dr["Address"];
                comm.Parameters.Add("@Tel", SqlDbType.VarChar).Value = dr["Tel"];
                comm.Parameters.Add("@Fax", SqlDbType.VarChar).Value = dr["Fax"];
                comm.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = dr["User_ID"];
                comm.Parameters.Add("@Modify_Date", SqlDbType.VarChar).Value = dr["Modify_Date"];
                comm.Parameters.Add("@Creative_Agency_ID", SqlDbType.VarChar).Value = dr["Creative_Agency_ID"];
                comm.Parameters.Add("@IsActive", SqlDbType.Int).Value = dr["IsActive"];
                comm.Parameters.Add("@InactiveDate", SqlDbType.VarChar).Value = dr["InactiveDate"];
                comm.Parameters.Add("@To_Email", SqlDbType.VarChar).Value = dr["To_Email"];
                comm.Parameters.Add("@CC_Email", SqlDbType.VarChar).Value = dr["CC_Email"];
                comm.Parameters.Add("@Remark", SqlDbType.VarChar).Value = dr["Remark"];
                return comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return -1;
            }
        }
        public int InsertVendor(DataRow dr)
        {
            try
            {
                string strSQL = @"INSERT INTO [dbo].[Media_Vendor]
           ([Media_Vendor_ID]
           ,[Thai_Name]
           ,[English_Name]
           ,[Short_Name]
           ,[Media_Type]
           ,[Address]
           ,[Contact]
           ,[Tel]
           ,[Fax]
           ,[Payment_Term]
           ,[Comment]
           ,[User_ID]
           ,[Modify_Date]
           ,[Supplier]
           ,[Master_Vendor]
           ,[Master_Group]
           ,[Broker]
           ,[InActive]
           ,[Agency_ID]
           ,[isPreferred]
           ,[GPM_Vendor]
           ,[POBreakByMediaFlag]
           ,[CDPercentageFlag]
           ,[CDPercentage]
           ,[Expire_Date]
           ,[Sym_VendorUID]
           ,[Agency_Vendor])
     VALUES
           (@Media_Vendor_ID
			,@Thai_Name
			,@English_Name
			,@Short_Name
			,@Media_Type
			,@Address
			,@Contact
			,@Tel
			,@Fax
			,@Payment_Term
			,@Comment
			,@User_ID
			,@Modify_Date
			,@Supplier
			,@Master_Vendor
			,@Master_Group
			,@Broker
			,@InActive
			,@Agency_ID
			,@isPreferred
			,@GPM_Vendor
			,@POBreakByMediaFlag
			,@CDPercentageFlag
			,@CDPercentage
			,@Expire_Date
			,@Sym_VendorUID
			,@Agency_Vendor)";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Media_Vendor_ID", SqlDbType.VarChar).Value = dr["Media_Vendor_ID"];
                comm.Parameters.Add("@Thai_Name", SqlDbType.VarChar).Value = dr["Thai_Name"];
                comm.Parameters.Add("@English_Name", SqlDbType.VarChar).Value = dr["English_Name"];
                comm.Parameters.Add("@Short_Name", SqlDbType.VarChar).Value = dr["Short_Name"];
                comm.Parameters.Add("@Media_Type", SqlDbType.VarChar).Value = dr["Media_Type"];
                comm.Parameters.Add("@Address", SqlDbType.VarChar).Value = dr["Address"];
                comm.Parameters.Add("@Contact", SqlDbType.VarChar).Value = dr["Contact"];
                comm.Parameters.Add("@Tel", SqlDbType.VarChar).Value = dr["Tel"];
                comm.Parameters.Add("@Fax", SqlDbType.VarChar).Value = dr["Fax"];
                comm.Parameters.Add("@Payment_Term", SqlDbType.VarChar).Value = dr["Payment_Term"];
                comm.Parameters.Add("@Comment", SqlDbType.VarChar).Value = dr["Comment"];
                comm.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = dr["User_ID"];
                comm.Parameters.Add("@Modify_Date", SqlDbType.VarChar).Value = dr["Modify_Date"];
                comm.Parameters.Add("@Supplier", SqlDbType.VarChar).Value = dr["Supplier"];
                comm.Parameters.Add("@Master_Vendor", SqlDbType.VarChar).Value = dr["Master_Vendor"];
                comm.Parameters.Add("@Master_Group", SqlDbType.VarChar).Value = dr["Master_Group"];
                comm.Parameters.Add("@Broker", SqlDbType.Bit).Value = dr["Broker"];
                comm.Parameters.Add("@InActive", SqlDbType.Bit).Value = dr["InActive"];
                comm.Parameters.Add("@Agency_ID", SqlDbType.VarChar).Value = dr["Agency_ID"];
                comm.Parameters.Add("@isPreferred", SqlDbType.Bit).Value = dr["isPreferred"];
                comm.Parameters.Add("@GPM_Vendor", SqlDbType.Bit).Value = dr["GPM_Vendor"];
                comm.Parameters.Add("@POBreakByMediaFlag", SqlDbType.Bit).Value = dr["POBreakByMediaFlag"];
                comm.Parameters.Add("@CDPercentageFlag", SqlDbType.Bit).Value = dr["CDPercentageFlag"];
                comm.Parameters.Add("@CDPercentage", SqlDbType.Decimal).Value = dr["CDPercentage"];
                comm.Parameters.Add("@Expire_Date", SqlDbType.VarChar).Value = dr["Expire_Date"];
                comm.Parameters.Add("@Sym_VendorUID", SqlDbType.VarChar).Value = dr["Sym_VendorUID"];
                comm.Parameters.Add("@Agency_Vendor", SqlDbType.Bit).Value = dr["Agency_Vendor"];
                return comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return -1;
            }
        }
        public int UpdateOffice(DataRow dr)
        {
            try
            {
                string strSQL = @"UPDATE [dbo].[Office]
                SET [Thai_Name] = @Thai_Name
                ,[English_Name] = @English_Name
                ,[Short_Name] = @Short_Name
                ,[Agency_ID] = @Agency_ID
                ,[Address] = @Address
                ,[Tel] = @Tel
                ,[Fax] = @Fax
                ,[User_ID] = @User_ID
                ,[Modify_Date] = @Modify_Date
                ,[Creative_Agency_ID] = @Creative_Agency_ID
                ,[IsActive] = @IsActive
                ,[InactiveDate] = @InactiveDate
                ,[To_Email] = @To_Email
                ,[CC_Email] = @CC_Email
                ,[Remark] = @Remark
                WHERE [Office_ID] = @Office_ID";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Office_ID", SqlDbType.VarChar).Value = dr["Office_ID"];
                comm.Parameters.Add("@Thai_Name", SqlDbType.VarChar).Value = dr["Thai_Name"];
                comm.Parameters.Add("@English_Name", SqlDbType.VarChar).Value = dr["English_Name"];
                comm.Parameters.Add("@Short_Name", SqlDbType.VarChar).Value = dr["Short_Name"];
                comm.Parameters.Add("@Agency_ID", SqlDbType.VarChar).Value = dr["Agency_ID"];
                comm.Parameters.Add("@Address", SqlDbType.VarChar).Value = dr["Address"];
                comm.Parameters.Add("@Tel", SqlDbType.VarChar).Value = dr["Tel"];
                comm.Parameters.Add("@Fax", SqlDbType.VarChar).Value = dr["Fax"];
                comm.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = dr["User_ID"];
                comm.Parameters.Add("@Modify_Date", SqlDbType.VarChar).Value = DateTime.Now.ToString("yyyyMMdd");
                comm.Parameters.Add("@Creative_Agency_ID", SqlDbType.VarChar).Value = dr["Creative_Agency_ID"];
                comm.Parameters.Add("@IsActive", SqlDbType.Int).Value = dr["IsActive"];
                comm.Parameters.Add("@InactiveDate", SqlDbType.VarChar).Value = dr["InactiveDate"];
                comm.Parameters.Add("@To_Email", SqlDbType.VarChar).Value = dr["To_Email"];
                comm.Parameters.Add("@CC_Email", SqlDbType.VarChar).Value = dr["CC_Email"];
                comm.Parameters.Add("@Remark", SqlDbType.VarChar).Value = dr["Remark"];
                return comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return -1;
            }
        }
        public int UpdateVendor(DataRow dr)
        {
            try
            {
                string strSQL = @"UPDATE [dbo].[Media_Vendor]
                SET
                    Thai_Name=@Thai_Name
                    ,English_Name=@English_Name
                    ,Short_Name=@Short_Name
                    ,Media_Type=@Media_Type
                    ,Address=@Address
                    ,Contact=@Contact
                    ,Tel=@Tel
                    ,Fax=@Fax
                    ,Payment_Term=@Payment_Term
                    ,Comment=@Comment
                   ,User_ID=@User_ID
                    ,Modify_Date=@Modify_Date
                    ,Supplier=@Supplier
                    ,Master_Vendor=@Master_Vendor
                    ,Master_Group=@Master_Group
                    ,Broker=@Broker
                    ,InActive=@InActive
                     ,Agency_ID=@Agency_ID
                   ,isPreferred=@isPreferred
                     ,GPM_Vendor=@GPM_Vendor
                    ,POBreakByMediaFlag=@POBreakByMediaFlag
                    ,CDPercentageFlag=@CDPercentageFlag
                    ,CDPercentage=@CDPercentage
                    ,Expire_Date=@Expire_Date
                    ,Sym_VendorUID=@Sym_VendorUID
                    ,Agency_Vendor=@Agency_Vendor
                WHERE  Media_Vendor_ID=@Media_Vendor_ID";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Media_Vendor_ID", SqlDbType.VarChar).Value = dr["Media_Vendor_ID"];
                comm.Parameters.Add("@Thai_Name", SqlDbType.VarChar).Value = dr["Thai_Name"];
                comm.Parameters.Add("@English_Name", SqlDbType.VarChar).Value = dr["English_Name"];
                comm.Parameters.Add("@Short_Name", SqlDbType.VarChar).Value = dr["Short_Name"];
                comm.Parameters.Add("@Media_Type", SqlDbType.VarChar).Value = dr["Media_Type"];
                comm.Parameters.Add("@Address", SqlDbType.VarChar).Value = dr["Address"];
                comm.Parameters.Add("@Contact", SqlDbType.VarChar).Value = dr["Contact"];
                comm.Parameters.Add("@Tel", SqlDbType.VarChar).Value = dr["Tel"];
                comm.Parameters.Add("@Fax", SqlDbType.VarChar).Value = dr["Fax"];
                comm.Parameters.Add("@Payment_Term", SqlDbType.VarChar).Value = dr["Payment_Term"];
                comm.Parameters.Add("@Comment", SqlDbType.VarChar).Value = dr["Comment"];
                comm.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = dr["User_ID"];
                comm.Parameters.Add("@Modify_Date", SqlDbType.VarChar).Value = dr["Modify_Date"];
                comm.Parameters.Add("@Supplier", SqlDbType.VarChar).Value = dr["Supplier"];
                comm.Parameters.Add("@Master_Vendor", SqlDbType.VarChar).Value = dr["Master_Vendor"];
                comm.Parameters.Add("@Master_Group", SqlDbType.VarChar).Value = dr["Master_Group"];
                comm.Parameters.Add("@Broker", SqlDbType.Bit).Value = dr["Broker"];
                comm.Parameters.Add("@InActive", SqlDbType.Bit).Value = dr["InActive"];
                comm.Parameters.Add("@Agency_ID", SqlDbType.VarChar).Value = dr["Agency_ID"];
                comm.Parameters.Add("@isPreferred", SqlDbType.Bit).Value = dr["isPreferred"];
                comm.Parameters.Add("@GPM_Vendor", SqlDbType.Bit).Value = dr["GPM_Vendor"];
                comm.Parameters.Add("@POBreakByMediaFlag", SqlDbType.Bit).Value = dr["POBreakByMediaFlag"];
                comm.Parameters.Add("@CDPercentageFlag", SqlDbType.Bit).Value = dr["CDPercentageFlag"];
                comm.Parameters.Add("@CDPercentage", SqlDbType.Decimal).Value = dr["CDPercentage"];
                comm.Parameters.Add("@Expire_Date", SqlDbType.VarChar).Value = dr["Expire_Date"];
                comm.Parameters.Add("@Sym_VendorUID", SqlDbType.VarChar).Value = dr["Sym_VendorUID"];
                comm.Parameters.Add("@Agency_Vendor", SqlDbType.Bit).Value = dr["Agency_Vendor"];
                return comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return -1;
            }
        }
        public bool DeleteOffice(string strOfficeCode)
        {
            try
            {
                string strSQL = @"DELETE FROM [dbo].[Office] WHERE Office_ID = @Office_ID";
                SqlCommand com = new SqlCommand(strSQL, m_connMinder);
                com.Parameters.Add(new SqlParameter("@Office_ID", SqlDbType.VarChar)).Value = strOfficeCode;
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteSpotPlan(string strBB, string strVersion)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = @" DELETE FROM Spot_Plan WHERE  Buying_Brief_ID = @strBB and [Version] = @Version";

                SqlCommand com = new SqlCommand(strSQL, m_connMinder);
                com.Parameters.Add(new SqlParameter("@strBB", SqlDbType.VarChar)).Value = strBB;
                com.Parameters.Add(new SqlParameter("@Version", SqlDbType.VarChar)).Value = strVersion;
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }
        }

        public bool DeleteSpotPlanVersion(string strBB, string strVersion)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = @" DELETE FROM Spot_Plan_version WHERE  Buying_Brief_ID = @strBB and [Version] = @Version";

                SqlCommand com = new SqlCommand(strSQL, m_connMinder);
                com.Parameters.Add(new SqlParameter("@strBB", SqlDbType.VarChar)).Value = strBB;
                com.Parameters.Add(new SqlParameter("@Version", SqlDbType.VarChar)).Value = strVersion;
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }
        }

        //public bool DeleteSpotPlanEdit(string strBB, string strVersion)
        //{
        //    try
        //    {
        //        string strSQL = string.Empty;
        //        strSQL = @" DELETE FROM Spot_Plan_Edit WHERE Buying_Brief_ID = @strBB and [Version] = @Version";

        //        SqlCommand com = new SqlCommand(strSQL, m_connMinder);
        //        com.Parameters.Add(new SqlParameter("@strBB", SqlDbType.VarChar)).Value = strBB;
        //        com.Parameters.Add(new SqlParameter("@Version", SqlDbType.VarChar)).Value = strVersion;
        //        com.ExecuteNonQuery();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        GMessage.MessageError(ex);
        //        return false;
        //    }

        //}

        public bool DeleteVendor(string strCode)
        {
            try
            {
                string strSQL = @"DELETE FROM [dbo].[Media_Vendor] WHERE Media_Vendor_ID = @Media_Vendor_ID
                DELETE FROM [dbo].[Media_Detail] WHERE Media_Vendor_ID = @Media_Vendor_ID";
                SqlCommand com = new SqlCommand(strSQL, m_connMinder);
                com.Parameters.Add(new SqlParameter("@Media_Vendor_ID", SqlDbType.VarChar)).Value = strCode;
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int InsertClient(DataRow dr)
        {
            try
            {
                string strSQL = @"INSERT INTO [dbo].[Client]
                ([Client_ID]
                ,[Thai_Name]
                ,[English_Name]
                ,[Short_Name]
                ,[Agency_ID]
                ,[Office_ID]
                ,[Creative_Agency_ID]
                ,[Group]
                ,[Master_Client]
                ,[Contact]
                ,[Tel]
                ,[Fax]
                ,[Booking_order_Header]
                ,[Address]
                ,[Thai_Address2]
                ,[Thai_Address3]
                ,[Thai_Address4]
                ,[English_Address1]
                ,[English_Address2]
                ,[English_Address3]
                ,[Agency_Commission]
                ,[Special]
                ,[User_ID]
                ,[Modify_Date]
                ,[AC_Struct]
                ,[Media_Fee]
                ,[Client_Referrence_ID]
                ,[CA_Struct]
                ,[Creative_Agency_Commission]
                ,[CA_Media_Fee]
                ,[Client_Classification_ID]
                ,[IsNewClient]
                ,[CreateDate]
                ,[InactiveClient]
                ,[InactiveDate]
                ,[Special_Unit]
                ,[Business_Type]
                ,[MOB]
                ,[Mgmt_Category]
                ,[Contract_Expiry]
                ,[Managing_Partner]
                ,[Planing_Director]
                ,[Mgmt_Team]
                ,[Added_Calculation_Type]
                ,[Margin_Cost]
                ,[GPM]
                ,[Show_CComm]
                ,[GPM_CLIENT_CODE]
                ,[GPM_CLIENT_CODE_TMP]
                ,[Opt_in]
                ,[Mapping_Symp]
                ,[Direct_Client]
                ,[RatingEngineId]
                ,[RED_Status]
                ,[Report_to_Agency]
                ,[Sym_ClientUniqueId]
                ,[Opt_in_Signed]
                ,[Opt_in_StartDate]
                ,[Opt_in_EndDate]
                ,[Opt_in_Note]
                ,[Region])
                VALUES
                (@Client_ID
                ,@Thai_Name
                ,@English_Name
                ,@Short_Name
                ,@Agency_ID
                ,@Office_ID
                ,@Creative_Agency_ID
                ,@Group
                ,@Master_Client
                ,@Contact
                ,@Tel
                ,@Fax
                ,@Booking_order_Header
                ,@Address
                ,@Thai_Address2
                ,@Thai_Address3
                ,@Thai_Address4
                ,@English_Address1
                ,@English_Address2
                ,@English_Address3
                ,@Agency_Commission
                ,@Special
                ,@User_ID
                ,@Modify_Date
                ,@AC_Struct
                ,@Media_Fee
                ,@Client_Referrence_ID
                ,@CA_Struct
                ,@Creative_Agency_Commission
                ,@CA_Media_Fee
                ,@Client_Classification_ID
                ,@IsNewClient
                ,@CreateDate
                ,@InactiveClient
                ,@InactiveDate
                ,@Special_Unit
                ,@Business_Type
                ,@MOB
                ,@Mgmt_Category
                ,@Contract_Expiry
                ,@Managing_Partner
                ,@Planing_Director
                ,@Mgmt_Team
                ,@Added_Calculation_Type
                ,@Margin_Cost
                ,@GPM
                ,@Show_CComm
                ,@GPM_CLIENT_CODE
                ,@GPM_CLIENT_CODE_TMP
                ,@Opt_in
                ,@Mapping_Symp
                ,@Direct_Client
                ,@RatingEngineId
                ,@RED_Status
                ,@Report_to_Agency
                ,@Sym_ClientUniqueId
                ,@Opt_in_Signed
                ,@Opt_in_StartDate
                ,@Opt_in_EndDate
                ,@Opt_in_Note
                ,@Region)";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = dr["Client_ID"];
                comm.Parameters.Add("@Thai_Name", SqlDbType.VarChar).Value = dr["Thai_Name"];
                comm.Parameters.Add("@English_Name", SqlDbType.VarChar).Value = dr["English_Name"];
                comm.Parameters.Add("@Short_Name", SqlDbType.VarChar).Value = dr["Short_Name"];
                comm.Parameters.Add("@Agency_ID", SqlDbType.VarChar).Value = dr["Agency_ID"];
                comm.Parameters.Add("@Office_ID", SqlDbType.VarChar).Value = dr["Office_ID"];
                comm.Parameters.Add("@Creative_Agency_ID", SqlDbType.VarChar).Value = dr["Creative_Agency_ID"];
                comm.Parameters.Add("@Group", SqlDbType.VarChar).Value = dr["Group"];
                comm.Parameters.Add("@Master_Client", SqlDbType.VarChar).Value = dr["Master_Client"];
                comm.Parameters.Add("@Contact", SqlDbType.VarChar).Value = dr["Contact"];
                comm.Parameters.Add("@Tel", SqlDbType.VarChar).Value = dr["Tel"];
                comm.Parameters.Add("@Fax", SqlDbType.VarChar).Value = dr["Fax"];
                comm.Parameters.Add("@Booking_order_Header", SqlDbType.VarChar).Value = dr["Booking_order_Header"];
                comm.Parameters.Add("@Address", SqlDbType.VarChar).Value = dr["Address"];
                comm.Parameters.Add("@Thai_Address2", SqlDbType.VarChar).Value = dr["Thai_Address2"];
                comm.Parameters.Add("@Thai_Address3", SqlDbType.VarChar).Value = dr["Thai_Address3"];
                comm.Parameters.Add("@Thai_Address4", SqlDbType.VarChar).Value = dr["Thai_Address4"];
                comm.Parameters.Add("@English_Address1", SqlDbType.VarChar).Value = dr["English_Address1"];
                comm.Parameters.Add("@English_Address2", SqlDbType.VarChar).Value = dr["English_Address2"];
                comm.Parameters.Add("@English_Address3", SqlDbType.VarChar).Value = dr["English_Address3"];
                comm.Parameters.Add("@Agency_Commission", SqlDbType.Real).Value = dr["Agency_Commission"];
                comm.Parameters.Add("@Special", SqlDbType.Bit).Value = dr["Special"];
                comm.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = dr["User_ID"];
                comm.Parameters.Add("@Modify_Date", SqlDbType.VarChar).Value = dr["Modify_Date"];
                comm.Parameters.Add("@AC_Struct", SqlDbType.VarChar).Value = dr["AC_Struct"];
                comm.Parameters.Add("@Media_Fee", SqlDbType.Real).Value = dr["Media_Fee"];
                comm.Parameters.Add("@Client_Referrence_ID", SqlDbType.VarChar).Value = dr["Client_Referrence_ID"];
                comm.Parameters.Add("@CA_Struct", SqlDbType.VarChar).Value = dr["CA_Struct"];
                comm.Parameters.Add("@Creative_Agency_Commission", SqlDbType.Real).Value = dr["Creative_Agency_Commission"];
                comm.Parameters.Add("@CA_Media_Fee", SqlDbType.Real).Value = dr["CA_Media_Fee"];
                comm.Parameters.Add("@Client_Classification_ID", SqlDbType.VarChar).Value = dr["Client_Classification_ID"];
                comm.Parameters.Add("@IsNewClient", SqlDbType.VarChar).Value = dr["IsNewClient"];
                comm.Parameters.Add("@CreateDate", SqlDbType.VarChar).Value = dr["CreateDate"];
                comm.Parameters.Add("@InactiveClient", SqlDbType.VarChar).Value = dr["InactiveClient"];
                comm.Parameters.Add("@InactiveDate", SqlDbType.VarChar).Value = dr["InactiveDate"];
                comm.Parameters.Add("@Special_Unit", SqlDbType.VarChar).Value = dr["Special_Unit"];
                comm.Parameters.Add("@Business_Type", SqlDbType.VarChar).Value = dr["Business_Type"];
                comm.Parameters.Add("@MOB", SqlDbType.VarChar).Value = dr["MOB"];
                comm.Parameters.Add("@Mgmt_Category", SqlDbType.VarChar).Value = dr["Mgmt_Category"];
                comm.Parameters.Add("@Contract_Expiry", SqlDbType.VarChar).Value = dr["Contract_Expiry"];
                comm.Parameters.Add("@Managing_Partner", SqlDbType.VarChar).Value = dr["Managing_Partner"];
                comm.Parameters.Add("@Planing_Director", SqlDbType.VarChar).Value = dr["Planing_Director"];
                comm.Parameters.Add("@Mgmt_Team", SqlDbType.VarChar).Value = dr["Mgmt_Team"];
                comm.Parameters.Add("@Added_Calculation_Type", SqlDbType.VarChar).Value = dr["Added_Calculation_Type"];
                comm.Parameters.Add("@Margin_Cost", SqlDbType.Bit).Value = dr["Margin_Cost"];
                comm.Parameters.Add("@GPM", SqlDbType.Bit).Value = dr["GPM"];
                comm.Parameters.Add("@Show_CComm", SqlDbType.VarChar).Value = dr["Show_CComm"];
                comm.Parameters.Add("@GPM_CLIENT_CODE", SqlDbType.VarChar).Value = dr["GPM_CLIENT_CODE"];
                comm.Parameters.Add("@GPM_CLIENT_CODE_TMP", SqlDbType.VarChar).Value = dr["GPM_CLIENT_CODE_TMP"];
                comm.Parameters.Add("@Opt_in", SqlDbType.VarChar).Value = dr["Opt_in"];
                comm.Parameters.Add("@Mapping_Symp", SqlDbType.VarChar).Value = dr["Mapping_Symp"];
                comm.Parameters.Add("@Direct_Client", SqlDbType.Bit).Value = dr["Direct_Client"];
                comm.Parameters.Add("@RatingEngineId", SqlDbType.Int).Value = dr["RatingEngineId"];
                comm.Parameters.Add("@RED_Status", SqlDbType.Bit).Value = dr["RED_Status"];
                comm.Parameters.Add("@Report_to_Agency", SqlDbType.VarChar).Value = dr["Report_to_Agency"];
                comm.Parameters.Add("@Sym_ClientUniqueId", SqlDbType.VarChar).Value = dr["Sym_ClientUniqueId"];
                comm.Parameters.Add("@Opt_in_Signed", SqlDbType.Bit).Value = dr["Opt_in_Signed"];
                comm.Parameters.Add("@Opt_in_StartDate", SqlDbType.Date).Value = dr["Opt_in_StartDate"];
                comm.Parameters.Add("@Opt_in_EndDate", SqlDbType.Date).Value = dr["Opt_in_EndDate"];
                comm.Parameters.Add("@Opt_in_Note", SqlDbType.VarChar).Value = dr["Opt_in_Note"];
                comm.Parameters.Add("@Region", SqlDbType.VarChar).Value = dr["Region"];
                return comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return -1;
            }
        }

        public int UpdateClient(DataRow dr)
        {
            try
            {
                int update = 0;
                string strSQL = @"Update [dbo].[Client] 
                set [Thai_Name]=@Thai_Name
                ,[English_Name]=@English_Name
                ,[Short_Name]=@Short_Name
                ,[Agency_ID]=@Agency_ID
                ,[Office_ID]=@Office_ID
                ,[Creative_Agency_ID]=@Creative_Agency_ID
                ,[Group]=@Group
                ,[Master_Client]=@Master_Client
                ,[Contact]=@Contact
                ,[Tel]=@Tel
                ,[Fax]=@Fax
                ,[Booking_order_Header]=@Booking_order_Header
                ,[Address]=@Address
                ,[Thai_Address2]=@Thai_Address2
                ,[Thai_Address3]=@Thai_Address3
                ,[Thai_Address4]=@Thai_Address4
                ,[English_Address1]=@English_Address1
                ,[English_Address2]=@English_Address2
                ,[English_Address3]=@English_Address3
                ,[Agency_Commission]=@Agency_Commission
                ,[Special]=@Special
                ,[User_ID]=@User_ID
                ,[Modify_Date]=@Modify_Date
                ,[AC_Struct]=@AC_Struct
                ,[Media_Fee]=@Media_Fee
                ,[Client_Referrence_ID]=@Client_Referrence_ID
                ,[CA_Struct]=@CA_Struct
                ,[Creative_Agency_Commission]=@Creative_Agency_Commission
                ,[CA_Media_Fee]=@CA_Media_Fee
                ,[Client_Classification_ID]=@Client_Classification_ID
                ,[IsNewClient]=@IsNewClient
                ,[CreateDate]=@CreateDate
                ,[InactiveClient]=@InactiveClient
                ,[InactiveDate]=@InactiveDate
                ,[Special_Unit]=@Special_Unit
                ,[Business_Type]=@Business_Type
                ,[MOB]=@MOB
                ,[Mgmt_Category]=@Mgmt_Category
                ,[Contract_Expiry]=@Contract_Expiry
                ,[Managing_Partner]=@Managing_Partner
                ,[Planing_Director]=@Planing_Director
                ,[Mgmt_Team]=@Mgmt_Team
                ,[Added_Calculation_Type]=@Added_Calculation_Type
                ,[Margin_Cost]=@Margin_Cost
                ,[GPM]=@GPM
                ,[Show_CComm]=@Show_CComm
                ,[GPM_CLIENT_CODE]=@GPM_CLIENT_CODE
                ,[GPM_CLIENT_CODE_TMP]=@GPM_CLIENT_CODE_TMP
                ,[Opt_in]=@Opt_in
                ,[Mapping_Symp]=@Mapping_Symp
                ,[Direct_Client]=@Direct_Client
                ,[RatingEngineId]=@RatingEngineId
                ,[RED_Status]=@RED_Status
                ,[Report_to_Agency]=@Report_to_Agency
                ,[Sym_ClientUniqueId]=@Sym_ClientUniqueId
                ,[Opt_in_Signed]=@Opt_in_Signed
                ,[Opt_in_StartDate]=@Opt_in_StartDate
                ,[Opt_in_EndDate]=@Opt_in_EndDate
                ,[Opt_in_Note]=@Opt_in_Note
                ,[Region]=@Region
                WHERE [Client_ID]=@Client_ID";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = dr["Client_ID"];
                comm.Parameters.Add("@Thai_Name", SqlDbType.VarChar).Value = dr["Thai_Name"];
                comm.Parameters.Add("@English_Name", SqlDbType.VarChar).Value = dr["English_Name"];
                comm.Parameters.Add("@Short_Name", SqlDbType.VarChar).Value = dr["Short_Name"];
                comm.Parameters.Add("@Agency_ID", SqlDbType.VarChar).Value = dr["Agency_ID"];
                comm.Parameters.Add("@Office_ID", SqlDbType.VarChar).Value = dr["Office_ID"];
                comm.Parameters.Add("@Creative_Agency_ID", SqlDbType.VarChar).Value = dr["Creative_Agency_ID"];
                comm.Parameters.Add("@Group", SqlDbType.VarChar).Value = dr["Group"];
                comm.Parameters.Add("@Master_Client", SqlDbType.VarChar).Value = dr["Master_Client"];
                comm.Parameters.Add("@Contact", SqlDbType.VarChar).Value = dr["Contact"];
                comm.Parameters.Add("@Tel", SqlDbType.VarChar).Value = dr["Tel"];
                comm.Parameters.Add("@Fax", SqlDbType.VarChar).Value = dr["Fax"];
                comm.Parameters.Add("@Booking_order_Header", SqlDbType.VarChar).Value = dr["Booking_order_Header"];
                comm.Parameters.Add("@Address", SqlDbType.VarChar).Value = dr["Address"];
                comm.Parameters.Add("@Thai_Address2", SqlDbType.VarChar).Value = dr["Thai_Address2"];
                comm.Parameters.Add("@Thai_Address3", SqlDbType.VarChar).Value = dr["Thai_Address3"];
                comm.Parameters.Add("@Thai_Address4", SqlDbType.VarChar).Value = dr["Thai_Address4"];
                comm.Parameters.Add("@English_Address1", SqlDbType.VarChar).Value = dr["English_Address1"];
                comm.Parameters.Add("@English_Address2", SqlDbType.VarChar).Value = dr["English_Address2"];
                comm.Parameters.Add("@English_Address3", SqlDbType.VarChar).Value = dr["English_Address3"];
                comm.Parameters.Add("@Agency_Commission", SqlDbType.Real).Value = dr["Agency_Commission"];
                comm.Parameters.Add("@Special", SqlDbType.Bit).Value = dr["Special"];
                comm.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = dr["User_ID"];
                comm.Parameters.Add("@Modify_Date", SqlDbType.VarChar).Value = DateTime.Now.ToString("yyyyMMdd");
                comm.Parameters.Add("@AC_Struct", SqlDbType.VarChar).Value = dr["AC_Struct"];
                comm.Parameters.Add("@Media_Fee", SqlDbType.Real).Value = dr["Media_Fee"];
                comm.Parameters.Add("@Client_Referrence_ID", SqlDbType.VarChar).Value = dr["Client_Referrence_ID"];
                comm.Parameters.Add("@CA_Struct", SqlDbType.VarChar).Value = dr["CA_Struct"];
                comm.Parameters.Add("@Creative_Agency_Commission", SqlDbType.Real).Value = dr["Creative_Agency_Commission"];
                comm.Parameters.Add("@CA_Media_Fee", SqlDbType.Real).Value = dr["CA_Media_Fee"];
                comm.Parameters.Add("@Client_Classification_ID", SqlDbType.VarChar).Value = dr["Client_Classification_ID"];
                comm.Parameters.Add("@IsNewClient", SqlDbType.VarChar).Value = dr["IsNewClient"];
                comm.Parameters.Add("@CreateDate", SqlDbType.VarChar).Value = dr["CreateDate"];
                comm.Parameters.Add("@InactiveClient", SqlDbType.VarChar).Value = dr["InactiveClient"];
                comm.Parameters.Add("@InactiveDate", SqlDbType.VarChar).Value = dr["InactiveDate"];
                comm.Parameters.Add("@Special_Unit", SqlDbType.VarChar).Value = dr["Special_Unit"];
                comm.Parameters.Add("@Business_Type", SqlDbType.VarChar).Value = dr["Business_Type"];
                comm.Parameters.Add("@MOB", SqlDbType.VarChar).Value = dr["MOB"];
                comm.Parameters.Add("@Mgmt_Category", SqlDbType.VarChar).Value = dr["Mgmt_Category"];
                comm.Parameters.Add("@Contract_Expiry", SqlDbType.VarChar).Value = dr["Contract_Expiry"];
                comm.Parameters.Add("@Managing_Partner", SqlDbType.VarChar).Value = dr["Managing_Partner"];
                comm.Parameters.Add("@Planing_Director", SqlDbType.VarChar).Value = dr["Planing_Director"];
                comm.Parameters.Add("@Mgmt_Team", SqlDbType.VarChar).Value = dr["Mgmt_Team"];
                comm.Parameters.Add("@Added_Calculation_Type", SqlDbType.VarChar).Value = dr["Added_Calculation_Type"];
                comm.Parameters.Add("@Margin_Cost", SqlDbType.Bit).Value = dr["Margin_Cost"];
                comm.Parameters.Add("@GPM", SqlDbType.Bit).Value = dr["GPM"];
                comm.Parameters.Add("@Show_CComm", SqlDbType.VarChar).Value = dr["Show_CComm"];
                comm.Parameters.Add("@GPM_CLIENT_CODE", SqlDbType.VarChar).Value = dr["GPM_CLIENT_CODE"];
                comm.Parameters.Add("@GPM_CLIENT_CODE_TMP", SqlDbType.VarChar).Value = dr["GPM_CLIENT_CODE_TMP"];
                comm.Parameters.Add("@Opt_in", SqlDbType.VarChar).Value = dr["Opt_in"];
                comm.Parameters.Add("@Mapping_Symp", SqlDbType.VarChar).Value = dr["Mapping_Symp"];
                comm.Parameters.Add("@Direct_Client", SqlDbType.Bit).Value = dr["Direct_Client"];
                comm.Parameters.Add("@RatingEngineId", SqlDbType.Int).Value = dr["RatingEngineId"];
                comm.Parameters.Add("@RED_Status", SqlDbType.Bit).Value = dr["RED_Status"];
                comm.Parameters.Add("@Report_to_Agency", SqlDbType.VarChar).Value = dr["Report_to_Agency"];
                comm.Parameters.Add("@Sym_ClientUniqueId", SqlDbType.VarChar).Value = dr["Sym_ClientUniqueId"];
                comm.Parameters.Add("@Opt_in_Signed", SqlDbType.Bit).Value = dr["Opt_in_Signed"];
                comm.Parameters.Add("@Opt_in_StartDate", SqlDbType.Date).Value = dr["Opt_in_StartDate"];
                comm.Parameters.Add("@Opt_in_EndDate", SqlDbType.Date).Value = dr["Opt_in_EndDate"];
                comm.Parameters.Add("@Opt_in_Note", SqlDbType.VarChar).Value = dr["Opt_in_Note"];
                comm.Parameters.Add("@Region", SqlDbType.VarChar).Value = dr["Region"];
                update = comm.ExecuteNonQuery();

                strSQL = @"update Product 
                set Office_ID = c.Office_ID_C 
                from Product p 
                inner join (
	                select client_id, office_id as Office_ID_C 
	                from client
                ) as c 
                on p.Client_ID = c.Client_ID
                where p.Office_ID <> c.Office_ID_C 
                and p.Client_ID = @Client_ID";
                comm.Parameters.Clear();
                comm.CommandText = strSQL;
                comm.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = dr["Client_ID"];
                comm.ExecuteNonQuery();
                return update;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return -1;
            }
        }

        public int UpdatePreferVendor(DataTable dt, string strMedia_Vendor_ID)
        {
            try
            {
                string strSQL = @"DELETE Media_Vendor_PreferDetail WHERE Media_Vendor_ID = @Media_Vendor_ID";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Media_Vendor_ID", SqlDbType.VarChar).Value = strMedia_Vendor_ID;
                comm.ExecuteNonQuery();

                strSQL = @"INSERT INTO [dbo].[Media_Vendor_PreferDetail]
                ([Media_Vendor_ID]
                ,[start_date]
                ,[end_date])
                VALUES
                (@Media_Vendor_ID
                ,@start_date
                ,@end_date)";
                comm.CommandText = strSQL;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Media_Vendor_ID", SqlDbType.VarChar).Value = strMedia_Vendor_ID;
                    comm.Parameters.Add("@start_date", SqlDbType.VarChar).Value = ((DateTime)dr["start_date"]).ToString("yyyyMMdd");
                    comm.Parameters.Add("@end_date", SqlDbType.VarChar).Value = ((DateTime)dr["end_date"]).ToString("yyyyMMdd");
                    comm.ExecuteNonQuery();
                }
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return -1;
            }
        }

        public int UpdateVendorRedGreenPeriod(DataTable dt, string table, string strMedia_Vendor_ID, DateTime now, string username)
        {
            try
            {
                string strSQL = @"delete 
                from " + table + @" 
                where Media_Vendor_ID = @Media_Vendor_ID";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Media_Vendor_ID", SqlDbType.VarChar).Value = strMedia_Vendor_ID;
                comm.ExecuteNonQuery();

                strSQL = @"insert 
                into " + table + @" 
                (
                    Media_Vendor_ID, 
                    Type, 
                    Start_Date, 
                    End_Date, 
                    Flag, 
                    Modify_Date, 
                    Modify_By
                )
                values 
                (
                    @Media_Vendor_ID, 
                    @Type, 
                    @Start_Date, 
                    @End_Date, 
                    @Flag, 
                    @Modify_Date, 
                    @Modify_By
                )";
                comm.CommandText = strSQL;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Media_Vendor_ID", SqlDbType.VarChar).Value = strMedia_Vendor_ID;
                    comm.Parameters.Add("@Type", SqlDbType.VarChar).Value = dr["Type"].ToString();
                    comm.Parameters.Add("@Start_Date", SqlDbType.VarChar).Value = ((DateTime)dr["Start_Date"]).ToString("yyyyMMdd");
                    comm.Parameters.Add("@End_Date", SqlDbType.VarChar).Value = ((DateTime)dr["End_Date"]).ToString("yyyyMMdd");
                    comm.Parameters.Add("@Flag", SqlDbType.VarChar).Value = dr["Flag"].ToString();
                    comm.Parameters.Add("@Modify_Date", SqlDbType.DateTime).Value = now;
                    comm.Parameters.Add("@Modify_By", SqlDbType.VarChar).Value = username;
                    comm.ExecuteNonQuery();
                }
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return -1;
            }
        }

        public bool UpdateVendorPeriodFromMaster(string strMedia_Vendor_ID, string username)
        {
            try
            {
                string strSQL = "SaveVendorPeriodFromMasterToVendor";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@VendorID", SqlDbType.VarChar).Value = strMedia_Vendor_ID;
                comm.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }
        }

        public bool UpdateVendorPeriodClearFlag(string strMedia_Vendor_ID)
        {
            try
            {
                string strSQL = "SaveVendorPeriodClearFlag";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@VendorID", SqlDbType.VarChar).Value = strMedia_Vendor_ID;
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }
        }

        public bool CopyVendorRGPeriodFromMaster(string strMedia_Vendor_ID, string strMaster_Vendor_ID, string username)
        {
            try
            {
                string strSQL = "SaveVendorPeriodFromMaster";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@VendorID", SqlDbType.VarChar).Value = strMedia_Vendor_ID;
                comm.Parameters.Add("@MasterVendorID", SqlDbType.VarChar).Value = strMaster_Vendor_ID;
                comm.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }
        }

        public bool CopyVendorRGPeriodFromNewMaster(string strMedia_Vendor_ID, string strMaster_Vendor_ID, string username)
        {
            try
            {
                string strSQL = "SaveVendorPeriodFromNewMaster";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@VendorID", SqlDbType.VarChar).Value = strMedia_Vendor_ID;
                comm.Parameters.Add("@MasterVendorID", SqlDbType.VarChar).Value = strMaster_Vendor_ID;
                comm.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }
        }

        public int UpdatePropreitroyClient(DataTable dt, string strClient_ID)
        {
            try
            {
                string strSQL = @"DELETE GroupProprietaryClientMapping WHERE Client_ID = @Client_ID";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = strClient_ID;
                comm.ExecuteNonQuery();

                strSQL = @"INSERT INTO [dbo].[GroupProprietaryClientMapping]
                ([GroupProprietaryId]
                ,[Client_Id])
                VALUES
                (@GroupProprietaryId
                ,@Client_ID)";
                comm.CommandText = strSQL;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@GroupProprietaryId", SqlDbType.Int).Value = dr["GroupProprietaryId"];
                    comm.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = strClient_ID;
                    comm.ExecuteNonQuery();
                }
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return -1;
            }
        }
        public int UpdateClientAgencyFee(DataTable dt, string strClient_ID)
        {
            try
            {
                string strSQL = @"DELETE client_agency_fee WHERE Client_ID = @Client_ID";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = strClient_ID;
                comm.ExecuteNonQuery();

                strSQL = @"INSERT INTO client_agency_fee
                (Client_ID
,Priority
,Agency_Fee
,Editable
,Agency_Fee_Set_Up_Name
,Agency_Fee_Set_Up_Column
,Media_Type_Group
,Media_Type
,Media_Sub_Type
,Other_Value
,[description])
VALUES
(@Client_ID
,@Priority
,@Agency_Fee
,@Editable
,@Agency_Fee_Set_Up_Name
,@Agency_Fee_Set_Up_Column
,@Media_Type_Group
,@Media_Type
,@Media_Sub_Type
,@Other_Value
,@description)";
                comm.CommandText = strSQL;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = strClient_ID;
                    comm.Parameters.Add("@Priority", SqlDbType.Int).Value = dr["Priority"];
                    comm.Parameters.Add("@Agency_Fee", SqlDbType.Decimal).Value = Convert.ToDouble(dr["Agency_Fee"])/100.00;
                    comm.Parameters.Add("@Editable", SqlDbType.Bit).Value = dr["Editable"];
                    comm.Parameters.Add("@Agency_Fee_Set_Up_Name", SqlDbType.VarChar).Value = dr["Agency_Fee_Set_Up_Name"];
                    comm.Parameters.Add("@Agency_Fee_Set_Up_Column", SqlDbType.VarChar).Value = dr["Agency_Fee_Set_Up_Column"];
                    comm.Parameters.Add("@Media_Type_Group", SqlDbType.VarChar).Value = dr["Media_Type_Group"];
                    comm.Parameters.Add("@Media_Type", SqlDbType.VarChar).Value = dr["Media_Type"];
                    comm.Parameters.Add("@Media_Sub_Type", SqlDbType.VarChar).Value = dr["Media_Sub_Type"];
                    comm.Parameters.Add("@Other_Value", SqlDbType.VarChar).Value = dr["Other_Value"];
                    comm.Parameters.Add("@description", SqlDbType.VarChar).Value = dr["description"];

                    comm.ExecuteNonQuery();
                }
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return -1;
            }
        }
        public int UpdateBrandClient(DataTable dt, string strClient_ID)
        {
            try
            {
                string strSQL = @"DELETE Brand WHERE Client_ID = @Client_ID";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = strClient_ID;
                comm.ExecuteNonQuery();

                strSQL = @"INSERT INTO Brand
                (Client_ID
                ,Brand_ID
                ,Brand_Name)
                VALUES
                (@Client_ID
                ,@Brand_ID
                ,@Brand_Name)";
                comm.CommandText = strSQL;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = strClient_ID;
                    comm.Parameters.Add("@Brand_ID", SqlDbType.VarChar).Value = dr["MasterCode"];
                    comm.Parameters.Add("@Brand_Name", SqlDbType.VarChar).Value = dr["MasterName"];
                    comm.ExecuteNonQuery();
                }
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return -1;
            }
        }

        public void DeleteClientAgencyFee(int client_agency_fee_id)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand("DELETE Client_agency_fee WHERE client_agency_fee_id = @client_agency_fee_id", this.m_connMinder);
                sqlCommand.Parameters.Add("@client_agency_fee_id", SqlDbType.VarChar).Value = (object)client_agency_fee_id;
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }
        }
        public void InsertClientAgencyFee(DataRow dr, string Client_ID)
        {
            try
            {
                string cmdText = @"INSERT INTO Client_agency_fee
                    (Client_ID
    , Priority
    , Agency_Fee
    , Editable
    , Agency_Fee_Set_Up_Name
    , Agency_Fee_Set_Up_Column
    , Media_Type_Group
    , Media_Type
    , Media_Sub_Type
    , Other_Value
    ,[description])
  VALUES
  (@Client_ID
  , @Priority
  , @Agency_Fee
  , @Editable
  , @Agency_Fee_Set_Up_Name
  , @Agency_Fee_Set_Up_Column
  , @Media_Type_Group
  , @Media_Type
  , @Media_Sub_Type
  , @Other_Value
  , @description)";
  
            SqlCommand sqlCommand = new SqlCommand(cmdText, this.m_connMinder);
                sqlCommand.CommandText = cmdText;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = (object)Client_ID;
                sqlCommand.Parameters.Add("@Priority", SqlDbType.Int).Value = dr["Priority"];
                sqlCommand.Parameters.Add("@Agency_Fee", SqlDbType.Decimal).Value = (object)(Convert.ToDouble(dr["Agency_Fee"]) / 100.0);
                sqlCommand.Parameters.Add("@Editable", SqlDbType.Bit).Value = dr["Editable"];
                sqlCommand.Parameters.Add("@Agency_Fee_Set_Up_Name", SqlDbType.VarChar).Value = dr["Agency_Fee_Set_Up_Name"];
                sqlCommand.Parameters.Add("@Agency_Fee_Set_Up_Column", SqlDbType.VarChar).Value = dr["Agency_Fee_Set_Up_Column"];
                sqlCommand.Parameters.Add("@Media_Type_Group", SqlDbType.VarChar).Value = dr["Media_Type_Group"];
                sqlCommand.Parameters.Add("@Media_Type", SqlDbType.VarChar).Value = dr["Media_Type"];
                sqlCommand.Parameters.Add("@Media_Sub_Type", SqlDbType.VarChar).Value = dr["Media_Sub_Type"];
                sqlCommand.Parameters.Add("@Other_Value", SqlDbType.VarChar).Value = dr["Other_Value"];
                sqlCommand.Parameters.Add("@description", SqlDbType.VarChar).Value = dr["description"];
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }
        }
        public void UpdateClientAgencyFee(DataRow dr)
        {
            try
            {
                string cmdText = @"update client_agency_fee
  set
  Priority = @Priority
  , Agency_Fee = @Agency_Fee
  , Editable = @Editable
  , Agency_Fee_Set_Up_Name = @Agency_Fee_Set_Up_Name
  , Agency_Fee_Set_Up_Column = @Agency_Fee_Set_Up_Column
  , Media_Type_Group = @Media_Type_Group
  , Media_Type = @Media_Type
  , Media_Sub_Type = @Media_Sub_Type
  , Other_Value = @Other_Value
  ,[description] = @description
  WHERE client_Agency_Fee_ID = @Client_Agency_Fee_ID";
  
            SqlCommand sqlCommand = new SqlCommand(cmdText, this.m_connMinder);
                sqlCommand.CommandText = cmdText;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("@Client_Agency_Fee_ID", SqlDbType.VarChar).Value = dr["Client_Agency_Fee_ID"];
                sqlCommand.Parameters.Add("@Priority", SqlDbType.Int).Value = dr["Priority"];
                sqlCommand.Parameters.Add("@Agency_Fee", SqlDbType.Decimal).Value = (object)(Convert.ToDouble(dr["Agency_Fee"]) / 100.0);
                sqlCommand.Parameters.Add("@Editable", SqlDbType.Bit).Value = dr["Editable"];
                sqlCommand.Parameters.Add("@Agency_Fee_Set_Up_Name", SqlDbType.VarChar).Value = dr["Agency_Fee_Set_Up_Name"];
                sqlCommand.Parameters.Add("@Agency_Fee_Set_Up_Column", SqlDbType.VarChar).Value = dr["Agency_Fee_Set_Up_Column"];
                sqlCommand.Parameters.Add("@Media_Type_Group", SqlDbType.VarChar).Value = dr["Media_Type_Group"];
                sqlCommand.Parameters.Add("@Media_Type", SqlDbType.VarChar).Value = dr["Media_Type"];
                sqlCommand.Parameters.Add("@Media_Sub_Type", SqlDbType.VarChar).Value = dr["Media_Sub_Type"];
                sqlCommand.Parameters.Add("@Other_Value", SqlDbType.VarChar).Value = dr["Other_Value"];
                sqlCommand.Parameters.Add("@description", SqlDbType.VarChar).Value = dr["description"];
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }
        }
        public int UpdateCategoryClient(DataTable dt, string strClient_ID, string username)
        {
            try
            {
                string strSQL = @"DELETE Client_Category WHERE Client_ID = @Client_ID";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = strClient_ID;
                comm.ExecuteNonQuery();

                strSQL = @"INSERT INTO Client_Category
                (Client_ID
                ,Category_ID
                ,Modify_user_ID)
                VALUES
                (@Client_ID
                ,@Category_ID
                ,@Modify_user_ID)";
                comm.CommandText = strSQL;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = strClient_ID;
                    comm.Parameters.Add("@Category_ID", SqlDbType.VarChar).Value = dr["MasterCode"];
                    comm.Parameters.Add("@Modify_user_ID", SqlDbType.VarChar).Value = username;
                    comm.ExecuteNonQuery();
                }
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return -1;
            }
        }

        public int UpdateClientRedGreenPeriod(DataTable dt, string table, string strClient_ID, DateTime now, string username)
        {
            try
            {
                string strSQL = @"delete 
                from " + table + @" 
                where Client_ID = @Client_ID";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = strClient_ID;
                comm.ExecuteNonQuery();

                strSQL = @"insert 
                into " + table + @" 
                (
                    Client_ID, 
                    Type, 
                    Start_Date, 
                    End_Date, 
                    Modify_Date, 
                    Modify_By
                )
                values 
                (
                    @Client_ID, 
                    @Type, 
                    @Start_Date, 
                    @End_Date, 
                    @Modify_Date, 
                    @Modify_By
                )";
                comm.CommandText = strSQL;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    comm.Parameters.Clear();
                    comm.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = strClient_ID;
                    comm.Parameters.Add("@Type", SqlDbType.VarChar).Value = dr["Type"].ToString();
                    comm.Parameters.Add("@Start_Date", SqlDbType.VarChar).Value = ((DateTime)dr["Start_Date"]).ToString("yyyyMMdd");
                    comm.Parameters.Add("@End_Date", SqlDbType.VarChar).Value = ((DateTime)dr["End_Date"]).ToString("yyyyMMdd");
                    comm.Parameters.Add("@Modify_Date", SqlDbType.DateTime).Value = now;
                    comm.Parameters.Add("@Modify_By", SqlDbType.VarChar).Value = username;
                    comm.ExecuteNonQuery();
                }
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return -1;
            }
        }

        public bool UpdateClientRGPeriodFromMaster(string strClient_ID, string username)
        {
            try
            {
                string strSQL = "SaveClientPeriodFromMasterToClient";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@ClientID", SqlDbType.VarChar).Value = strClient_ID;
                comm.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }
        }

        public bool CopyClientRGPeriodFromMaster(string strClient_ID, string strMaster_Client_ID, string username)
        {
            try
            {
                string strSQL = "SaveClientPeriodFromMaster";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@ClientID", SqlDbType.VarChar).Value = strClient_ID;
                comm.Parameters.Add("@MasterClientID", SqlDbType.VarChar).Value = strMaster_Client_ID;
                comm.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }
        }

        public bool CopyClientRGPeriodFromNewMaster(string strClient_ID, string strMaster_Client_ID, string username)
        {
            try
            {
                string strSQL = "SaveClientPeriodFromNewMaster";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@ClientID", SqlDbType.VarChar).Value = strClient_ID;
                comm.Parameters.Add("@MasterClientID", SqlDbType.VarChar).Value = strMaster_Client_ID;
                comm.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }
        }

        public bool DeleteClient(string strClientCode)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = @"DELETE FROM GroupProprietaryClientMapping WHERE Client_ID = @Client_ID
                DELETE FROM Brand WHERE Client_ID = @Client_ID
                DELETE FROM Client_Category WHERE Client_ID = @Client_ID
                DELETE FROM Client_Audit_Right WHERE Client_ID = @Client_ID
                DELETE FROM Client_Media_Credit WHERE Client_ID = @Client_ID
                DELETE FROM Client_Rebate WHERE Client_ID = @Client_ID
                DELETE FROM Client_EPD WHERE Client_ID = @Client_ID
                DELETE FROM Client_SAC WHERE Client_ID = @Client_ID
                DELETE FROM Client_Agency_Fee WHERE Client_ID = @Client_ID
                DELETE FROM Client WHERE Client_ID = @Client_ID";
                SqlCommand com = new SqlCommand(strSQL, m_connMinder);
                com.Parameters.Add(new SqlParameter("@Client_ID", SqlDbType.VarChar)).Value = strClientCode;
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int InsertMediaSubType(DataRow dr)
        {
            try
            {
                string strSQL = @"INSERT INTO [dbo].[Media_Sub_Type]
                ([Media_Sub_Type]
                ,[Media_Type]
                ,[Short_Name]
                ,[User_ID]
                ,[Modify_Date]
                ,[Adept_Export_Mapping]
                ,[Show_BB]
                ,[Optin_Checked]
                ,[Adept_MergewithMedia]
                ,[AdeptExport_Prefix]
                ,[BillingType_Revenue]
                ,[OptIn_OptOut_Footnotes]
                ,[isActive]
                ,[InactiveDate]
                ,[FeeLocked_SP]
                ,[Forecast_Input]
                ,[Service_Group]
                ,[Billing_Group]
                ,[Media_Sub_Type_Mapping_CoreM_New]
                ,[Master_Media_Sub_Type]
                ,[Media_Sub_Type_Group]
                ,[BusinessDefinition])
                VALUES
                (@Media_Sub_Type
                ,@Media_Type
                ,@Short_Name
                ,@User_ID
                ,@Modify_Date
                ,@Adept_Export_Mapping
                ,@Show_BB
                ,@Optin_Checked
                ,@Adept_MergewithMedia
                ,@AdeptExport_Prefix
                ,@BillingType_Revenue
                ,@OptIn_OptOut_Footnotes
                ,@isActive
                ,@InactiveDate
                ,@FeeLocked_SP
                ,@Forecast_Input
                ,@Service_Group
                ,@Billing_Group
                ,@Media_Sub_Type_Mapping_CoreM_New
                ,@Master_Media_Sub_Type
                ,@Media_Sub_Type_Group
                ,@BusinessDefinition)";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Media_Sub_Type", SqlDbType.VarChar).Value = dr["Media_Sub_Type"];
                comm.Parameters.Add("@Media_Type", SqlDbType.VarChar).Value = dr["Media_Type"];
                comm.Parameters.Add("@Short_Name", SqlDbType.VarChar).Value = dr["Short_Name"];
                comm.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = dr["User_ID"];
                comm.Parameters.Add("@Modify_Date", SqlDbType.VarChar).Value = dr["Modify_Date"];
                comm.Parameters.Add("@Adept_Export_Mapping", SqlDbType.VarChar).Value = dr["Adept_Export_Mapping"];
                comm.Parameters.Add("@Show_BB", SqlDbType.VarChar).Value = dr["Show_BB"];
                comm.Parameters.Add("@Optin_Checked", SqlDbType.Bit).Value = dr["Optin_Checked"];
                comm.Parameters.Add("@Adept_MergewithMedia", SqlDbType.Bit).Value = dr["Adept_MergewithMedia"];
                comm.Parameters.Add("@AdeptExport_Prefix", SqlDbType.VarChar).Value = dr["AdeptExport_Prefix"];
                comm.Parameters.Add("@BillingType_Revenue", SqlDbType.Bit).Value = dr["BillingType_Revenue"];
                comm.Parameters.Add("@OptIn_OptOut_Footnotes", SqlDbType.VarChar).Value = dr["OptIn_OptOut_Footnotes"];
                comm.Parameters.Add("@isActive", SqlDbType.Bit).Value = dr["isActive"];
                comm.Parameters.Add("@InactiveDate", SqlDbType.VarChar).Value = dr["InactiveDate"];
                comm.Parameters.Add("@FeeLocked_SP", SqlDbType.Bit).Value = dr["FeeLocked_SP"];
                comm.Parameters.Add("@Forecast_Input", SqlDbType.Bit).Value = dr["Forecast_Input"];
                comm.Parameters.Add("@Service_Group", SqlDbType.VarChar).Value = dr["Service_Group"];
                comm.Parameters.Add("@Billing_Group", SqlDbType.VarChar).Value = dr["Billing_Group"];
                comm.Parameters.Add("@Media_Sub_Type_Mapping_CoreM_New", SqlDbType.VarChar).Value = dr["Media_Sub_Type_Mapping_CoreM_New"];
                comm.Parameters.Add("@Master_Media_Sub_Type", SqlDbType.VarChar).Value = dr["Media_Type"];
                comm.Parameters.Add("@Media_Sub_Type_Group", SqlDbType.VarChar).Value = dr["Media_Type"];
                comm.Parameters.Add("@BusinessDefinition", SqlDbType.VarChar).Value = dr["BusinessDefinition"];
                return comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return -1;
            }
        }

        public int InsertMediaType(DataRow dr)
        {
            try
            {
                string strSQL = @"INSERT INTO [dbo].[Media_Type]
                   ([Media_Type]
               ,[Short_Name]
               ,[Description]
               ,[User_ID]
               ,[Modify_Date]
               ,[Master_Media_Type]
               ,[IsMaster]
               ,[Media_Type_Group]
               ,[IsActive]
               ,[InactiveDate])
                VALUES
                (@Media_Type
               ,@Short_Name
               ,@Description
               ,@User_ID
               ,@Modify_Date
               ,@Master_Media_Type
               ,@IsMaster
               ,@Media_Type_Group
               ,@IsActive
               ,@InactiveDate)";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Media_Type", SqlDbType.VarChar).Value = dr["Media_Type"];
                comm.Parameters.Add("@Short_Name", SqlDbType.VarChar).Value = dr["Short_Name"];
                comm.Parameters.Add("@Description", SqlDbType.VarChar).Value = dr["Description"];
                comm.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = dr["User_ID"];
                comm.Parameters.Add("@Modify_Date", SqlDbType.VarChar).Value = dr["Modify_Date"];
                comm.Parameters.Add("@Master_Media_Type", SqlDbType.VarChar).Value = dr["Master_Media_Type"];
                comm.Parameters.Add("@IsMaster", SqlDbType.VarChar).Value = dr["IsMaster"];
                comm.Parameters.Add("@Media_Type_Group", SqlDbType.VarChar).Value = dr["Media_Type_Group"];
                comm.Parameters.Add("@isActive", SqlDbType.Bit).Value = dr["isActive"];
                comm.Parameters.Add("@InactiveDate", SqlDbType.VarChar).Value = dr["InactiveDate"];
                return comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return -1;
            }
        }

        public int InsertAdeptMediaType(DataRow dr)
        {
            try
            {
                string strSQL = @"INSERT INTO [dbo].[Adept_Media_Type]
                   ([Adept_Media_Type]
               ,[Adept_Media_Type_Name]
               ,[Billing_Type_Revenue]
               ,[IsActive]
               ,[Inactive_Date]
               ,[Description]
               ,[Modify_Date]
               ,[Modify_By])
                VALUES
                (@Adept_Media_Type
               ,@Adept_Media_Type_Name
               ,@Billing_Type_Revenue
               ,@IsActive
               ,@Inactive_Date
               ,@Description
               ,@Modify_Date
               ,@Modify_By)";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Adept_Media_Type", SqlDbType.VarChar).Value = dr["Adept_Media_Type"];
                comm.Parameters.Add("@Adept_Media_Type_Name", SqlDbType.VarChar).Value = dr["Adept_Media_Type_Name"];
                comm.Parameters.Add("@Billing_Type_Revenue", SqlDbType.Bit).Value = dr["Billing_Type_Revenue"];
                comm.Parameters.Add("@IsActive", SqlDbType.Bit).Value = dr["IsActive"];
                comm.Parameters.Add("@Inactive_Date", SqlDbType.DateTime).Value = dr["Inactive_Date"];
                comm.Parameters.Add("@Description", SqlDbType.VarChar).Value = dr["Description"];
                comm.Parameters.Add("@Modify_Date", SqlDbType.DateTime).Value = dr["Modify_Date"];
                comm.Parameters.Add("@Modify_By", SqlDbType.VarChar).Value = dr["Modify_By"];
                return comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return -1;
            }
        }

        public int UpdateMediaSubType(DataRow dr)
        {
            try
            {
                string strSQL = @"UPDATE [dbo].[Media_Sub_Type]
                SET [Media_Type] = @Media_Type
                ,[Short_Name] = @Short_Name
                ,[User_ID] = @User_ID
                ,[Modify_Date] = @Modify_Date
                ,[Adept_Export_Mapping] = @Adept_Export_Mapping
                ,[Show_BB] = @Show_BB
                ,[Optin_Checked] = @Optin_Checked
                ,[Adept_MergewithMedia] = @Adept_MergewithMedia
                ,[AdeptExport_Prefix] = @AdeptExport_Prefix
                ,[BillingType_Revenue] = @BillingType_Revenue
                ,[OptIn_OptOut_Footnotes] = @OptIn_OptOut_Footnotes
                ,[isActive] = @isActive
                ,[InactiveDate] = @InactiveDate
                ,[FeeLocked_SP] = @FeeLocked_SP
                ,[Forecast_Input] = @Forecast_Input
                ,[Service_Group] = @Service_Group
                ,[Billing_Group] = @Billing_Group
                ,[Media_Sub_Type_Mapping_CoreM_New] = @Media_Sub_Type_Mapping_CoreM_New
                ,[Master_Media_Sub_Type] = @Master_Media_Sub_Type
                ,[Media_Sub_Type_Group] = @Media_Sub_Type_Group
                ,[BusinessDefinition] = @BusinessDefinition
                WHERE [Media_Sub_Type] = @Media_Sub_Type";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Media_Sub_Type", SqlDbType.VarChar).Value = dr["Media_Sub_Type"];
                comm.Parameters.Add("@Media_Type", SqlDbType.VarChar).Value = dr["Media_Type"];
                comm.Parameters.Add("@Short_Name", SqlDbType.VarChar).Value = dr["Short_Name"];
                comm.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = dr["User_ID"];
                comm.Parameters.Add("@Modify_Date", SqlDbType.VarChar).Value = DateTime.Now.ToString("yyyyMMdd");
                comm.Parameters.Add("@Adept_Export_Mapping", SqlDbType.VarChar).Value = dr["Adept_Export_Mapping"];
                comm.Parameters.Add("@Show_BB", SqlDbType.VarChar).Value = dr["Show_BB"];
                comm.Parameters.Add("@Optin_Checked", SqlDbType.Bit).Value = dr["Optin_Checked"];
                comm.Parameters.Add("@Adept_MergewithMedia", SqlDbType.Bit).Value = dr["Adept_MergewithMedia"];
                comm.Parameters.Add("@AdeptExport_Prefix", SqlDbType.VarChar).Value = dr["AdeptExport_Prefix"];
                comm.Parameters.Add("@BillingType_Revenue", SqlDbType.Bit).Value = dr["BillingType_Revenue"];
                comm.Parameters.Add("@OptIn_OptOut_Footnotes", SqlDbType.VarChar).Value = dr["OptIn_OptOut_Footnotes"];
                comm.Parameters.Add("@isActive", SqlDbType.Bit).Value = dr["isActive"];
                comm.Parameters.Add("@InactiveDate", SqlDbType.VarChar).Value = dr["InactiveDate"];
                comm.Parameters.Add("@FeeLocked_SP", SqlDbType.Bit).Value = dr["FeeLocked_SP"];
                comm.Parameters.Add("@Forecast_Input", SqlDbType.Bit).Value = dr["Forecast_Input"];
                comm.Parameters.Add("@Service_Group", SqlDbType.VarChar).Value = dr["Service_Group"];
                comm.Parameters.Add("@Billing_Group", SqlDbType.VarChar).Value = dr["Billing_Group"];
                comm.Parameters.Add("@Media_Sub_Type_Mapping_CoreM_New", SqlDbType.VarChar).Value = dr["Media_Sub_Type_Mapping_CoreM_New"];
                comm.Parameters.Add("@Master_Media_Sub_Type", SqlDbType.VarChar).Value = dr["Media_Type"];
                comm.Parameters.Add("@Media_Sub_Type_Group", SqlDbType.VarChar).Value = dr["Media_Type"];
                comm.Parameters.Add("@BusinessDefinition", SqlDbType.VarChar).Value = dr["BusinessDefinition"];
                return comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return -1;
            }
        }

        public int UpdateMediaType(DataRow dr)
        {
            try
            {
                string strSQL = @"UPDATE [dbo].[Media_Type]
                SET 
                    [Short_Name] = @Short_Name
                   ,[Description] = @Description
                   ,[User_ID] = @User_ID
                   ,[Modify_Date] = @Modify_Date
                   ,[Master_Media_Type] = @Master_Media_Type
                   ,[IsMaster] = @IsMaster
                   ,[Media_Type_Group] = @Media_Type_Group
                   ,[IsActive] = @IsActive
                   ,[InactiveDate] = @InactiveDate
                WHERE [Media_Type] = @Media_Type";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Media_Type", SqlDbType.VarChar).Value = dr["Media_Type"];
                comm.Parameters.Add("@Short_Name", SqlDbType.VarChar).Value = dr["Short_Name"];
                comm.Parameters.Add("@Description", SqlDbType.VarChar).Value = dr["Description"];
                comm.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = dr["User_ID"];
                comm.Parameters.Add("@Modify_Date", SqlDbType.VarChar).Value = dr["Modify_Date"];
                comm.Parameters.Add("@Master_Media_Type", SqlDbType.VarChar).Value = dr["Master_Media_Type"];
                comm.Parameters.Add("@IsMaster", SqlDbType.VarChar).Value = dr["IsMaster"];
                comm.Parameters.Add("@Media_Type_Group", SqlDbType.VarChar).Value = dr["Media_Type_Group"];
                comm.Parameters.Add("@isActive", SqlDbType.Bit).Value = dr["isActive"];
                comm.Parameters.Add("@InactiveDate", SqlDbType.VarChar).Value = dr["InactiveDate"];
                return comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return -1;
            }
        }

        public int UpdateAdeptMediaType(DataRow dr)
        {
            try
            {
                string strSQL = @"UPDATE [dbo].[Adept_Media_Type]
                SET 
                    [Adept_Media_Type_Name] = @Adept_Media_Type_Name
                   ,[Billing_Type_Revenue] = @Billing_Type_Revenue
                   ,[IsActive] = @IsActive
                   ,[Inactive_Date] = @Inactive_Date
                   ,[Description] = @Description
                   ,[Modify_Date] = @Modify_Date
                   ,[Modify_By] = @Modify_By
                WHERE [Adept_Media_Type] = @Adept_Media_Type";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Adept_Media_Type", SqlDbType.VarChar).Value = dr["Adept_Media_Type"];
                comm.Parameters.Add("@Adept_Media_Type_Name", SqlDbType.VarChar).Value = dr["Adept_Media_Type_Name"];
                comm.Parameters.Add("@Billing_Type_Revenue", SqlDbType.Bit).Value = dr["Billing_Type_Revenue"];
                comm.Parameters.Add("@IsActive", SqlDbType.Bit).Value = dr["IsActive"];
                comm.Parameters.Add("@Inactive_Date", SqlDbType.DateTime).Value = dr["Inactive_Date"];
                comm.Parameters.Add("@Description", SqlDbType.VarChar).Value = dr["Description"];
                comm.Parameters.Add("@Modify_Date", SqlDbType.DateTime).Value = dr["Modify_Date"];
                comm.Parameters.Add("@Modify_By", SqlDbType.VarChar).Value = dr["Modify_By"];
                return comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return -1;
            }
        }

        public bool DeleteMediaSubType(string strMediaSubTypeCode)
        {
            try
            {
                string strSQL = @"DELETE FROM [dbo].[Media_Sub_Type] WHERE Media_Sub_Type = @Media_Sub_Type";
                SqlCommand com = new SqlCommand(strSQL, m_connMinder);
                com.Parameters.Add(new SqlParameter("@Media_Sub_Type", SqlDbType.VarChar)).Value = strMediaSubTypeCode;
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteMediaType(string strMediaTypeCode)
        {
            try
            {
                string strSQL = @"DELETE FROM [dbo].[Media_Type] WHERE Media_Type = @Media_Type";
                SqlCommand com = new SqlCommand(strSQL, m_connMinder);
                com.Parameters.Add(new SqlParameter("@Media_Type", SqlDbType.VarChar)).Value = strMediaTypeCode;
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteAdeptMediaType(string strAdeptMediaTypeCode)
        {
            try
            {
                string strSQL = @"DELETE FROM [dbo].[Adept_Media_Type] WHERE Adept_Media_Type = @Adept_Media_Type";
                SqlCommand com = new SqlCommand(strSQL, m_connMinder);
                com.Parameters.Add(new SqlParameter("@Adept_Media_Type", SqlDbType.VarChar)).Value = strAdeptMediaTypeCode;
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int InsertIncidentTracking(DataRow dr)
        {
            try
            {
                string strSQL = @"insert 
                into Buying_Brief_Incident_Tracking 
                (
                    Incident_ID, 
                    Buying_Brief_ID, 
                    Status, 
                    Title, 
                    Description, 
                    Incident_Date, 
                    Create_Date, 
                    User_ID, 
                    Remark
                ) 
                values (
                    @Incident_ID, 
                    @Buying_Brief_ID, 
                    @Status, 
                    @Title, 
                    @Description, 
                    @Incident_Date, 
                    @Create_Date, 
                    @User_ID, 
                    @Remark
                );
                insert 
                into Buying_Brief_Incident_Track_History 
                (
                    Incident_ID, 
                    Status, 
                    Description, 
                    Incident_Date, 
                    Create_Date, 
                    User_ID
                ) 
                values (
                    @Incident_ID, 
                    @Status, 
                    @Description, 
                    @Incident_Date, 
                    @Create_Date, 
                    @User_ID
                )";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Incident_ID", SqlDbType.VarChar).Value = dr["Incident_ID"];
                comm.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = dr["Buying_Brief_ID"];
                comm.Parameters.Add("@Status", SqlDbType.VarChar).Value = dr["Status"];
                comm.Parameters.Add("@Title", SqlDbType.VarChar).Value = dr["Title"];
                comm.Parameters.Add("@Description", SqlDbType.VarChar).Value = dr["Description"];
                comm.Parameters.Add("@Incident_Date", SqlDbType.DateTime).Value = dr["Incident_Date"];
                comm.Parameters.Add("@Create_Date", SqlDbType.DateTime).Value = dr["Create_Date"];
                comm.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = dr["User_ID"];
                comm.Parameters.Add("@Remark", SqlDbType.VarChar).Value = dr["Remark"];
                return comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return -1;
            }
        }

        public int UpdateIncidentTracking(DataRow dr)
        {
            try
            {
                string strSQL = @"update Buying_Brief_Incident_Tracking 
                set Status = @Status, 
                Title = @Title, 
                Description = @Description, 
                Incident_Date = @Incident_Date, 
                Modify_Date = @Modify_Date, 
                User_ID = @User_ID, 
                Remark = @Remark 
                where Incident_ID = @Incident_ID;
                
                select * 
                from Buying_Brief_Incident_Track_History 
                where Incident_ID = @Incident_ID 
                and Status = @Status 
                and Description = @Description 
                and Incident_Date = @Incident_Date 
                and User_ID = @User_ID 
                and ID = (
	                select max(ID) 
	                from Buying_Brief_Incident_Track_History 
	                where Incident_ID = @Incident_ID
                );
                
                if (@@rowcount = 0)
                    begin
                        insert 
                        into Buying_Brief_Incident_Track_History 
                        (
                            Incident_ID, 
                            Status, 
                            Description, 
                            Incident_Date, 
                            Create_Date, 
                            Modify_Date, 
                            User_ID
                        ) 
                        values (
                            @Incident_ID, 
                            @Status, 
                            @Description, 
                            @Incident_Date, 
                            @Create_Date, 
                            @Modify_Date, 
                            @User_ID
                        )
                    end
                ";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Incident_ID", SqlDbType.VarChar).Value = dr["Incident_ID"];
                comm.Parameters.Add("@Status", SqlDbType.VarChar).Value = dr["Status"];
                comm.Parameters.Add("@Title", SqlDbType.VarChar).Value = dr["Title"];
                comm.Parameters.Add("@Description", SqlDbType.VarChar).Value = dr["Description"];
                comm.Parameters.Add("@Incident_Date", SqlDbType.DateTime).Value = dr["Incident_Date"];
                comm.Parameters.Add("@Create_Date", SqlDbType.DateTime).Value = dr["Create_Date"];
                comm.Parameters.Add("@Modify_Date", SqlDbType.DateTime).Value = dr["Modify_Date"];
                comm.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = dr["User_ID"];
                comm.Parameters.Add("@Remark", SqlDbType.VarChar).Value = dr["Remark"];
                return comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return -1;
            }
        }

        public DataTable SelectMasterCommon(string strCode, string strName, string strTable, bool includeInactive, string strUsername, string strFilter)
        {
            string strQuery = "select {0} as MasterCode, {1} as MasterName,* from {2}";
            if (strTable == "Agency")
            {
                if (strUsername != "")
                {
                    strQuery += @" inner join Office 
                    on Agency.Agency_ID = Office.Agency_ID 
                    inner join Client 
                    on Agency.Agency_ID = Client.Agency_ID 
                    where (
                    Agency.Agency_ID in (
                        select Agency_ID 
                        from User_Client 
                        where User_ID = @Username
                    ) 
                    or Office.Office_ID in (
                        select Office_ID 
                        from User_Client 
                        where User_ID = @Username
                    ) 
                    or Client.Client_ID in (
                        select Client_ID 
                        from User_Client 
                        where User_ID = @Username
                    )
                    ) 
                    group by Agency.Agency_ID, Agency.Short_Name";
                }
            }
            if (strTable == "Office")
            {
                if (strUsername == "")
                {
                    if (strFilter == "")
                    {
                        if (includeInactive == false)
                        {
                            strQuery += @" where IsActive = 1";
                        }
                    }
                    else
                    {
                        if (includeInactive == true)
                        {
                            strQuery += @" where Agency_ID = @Filter";
                        }
                        else
                        {
                            strQuery += @" where IsActive = 1 
                            and Agency_ID = @Filter";
                        }
                    }
                }
                else
                {
                    if (includeInactive == true)
                    {
                        strQuery += @" inner join Client 
                        on Office.Office_ID = Client.Office_ID 
                        where Office.Agency_ID in (
                            select Agency_ID 
                            from User_Client 
                            where User_ID = @Username
                        ) 
                        or Office.Office_ID in (
                            select Office_ID 
                            from User_Client 
                            where User_ID = @Username
                        ) 
                        or Client.Client_ID in (
                            select Client_ID 
                            from User_Client 
                            where User_ID = @Username
                        ) 
                        group by Office.Office_ID, Office.Short_Name";
                    }
                    else
                    {
                        strQuery += @" inner join Client 
                        on Office.Office_ID = Client.Office_ID 
                        where Office.IsActive = 1 
                        and (
                        Office.Agency_ID in (
                            select Agency_ID 
                            from User_Client 
                            where User_ID = @Username
                        ) 
                        or Office.Office_ID in (
                            select Office_ID 
                            from User_Client 
                            where User_ID = @Username
                        ) 
                        or Client.Client_ID in (
                            select Client_ID 
                            from User_Client 
                            where User_ID = @Username
                        )
                        ) 
                        group by Office.Office_ID, Office.Short_Name";
                    }
                }
            }
            if (strTable == "Client")
            {
                if (strUsername == "")
                {
                    if (strFilter == "")
                    {
                        if (includeInactive == false)
                        {
                            strQuery += " where InactiveClient = '0'";
                        }
                    }
                    else
                    {
                        if (includeInactive == true)
                        {
                            strQuery += @" where Office_ID = @Filter 
                            or Agency_ID = @Filter";
                        }
                        else
                        {
                            strQuery += @" where InactiveClient = '0' 
                            and (Office_ID = @Filter 
                            or Agency_ID = @Filter)";
                        }
                    }
                }
                else
                {
                    if (includeInactive == true)
                    {
                        strQuery += @" where Agency_ID in (
                            select Agency_ID 
                            from User_Client 
                            where User_ID = @Username
                        ) 
                        or Office_ID in (
                            select Office_ID 
                            from User_Client 
                            where User_ID = @Username
                        ) 
                        or Client_ID in (
                            select Client_ID 
                            from User_Client 
                            where User_ID = @Username
                        )";
                    }
                    else
                    {
                        strQuery += @" where InactiveClient = '0' 
                        and (
                        Agency_ID in (
                            select Agency_ID 
                            from User_Client 
                            where User_ID = @Username
                        ) 
                        or Office_ID in (
                            select Office_ID 
                            from User_Client 
                            where User_ID = @Username
                        ) 
                        or Client_ID in (
                            select Client_ID 
                            from User_Client 
                            where User_ID = @Username
                        )
                        )";
                    }

                }
            }
            if (strTable == "Product")
            {
                if (strUsername == "")
                {
                    if (strFilter == "")
                    {
                        if (includeInactive == false)
                        {
                            strQuery += " where Valid = 'True'";
                        }
                    }
                    else
                    {
                        if (includeInactive == true)
                        {
                            strQuery += @" where Client_ID = @Filter";
                        }
                        else
                        {
                            strQuery += @" where where Valid = 'True' 
                            and Client_ID = @Filter";
                        }
                    }
                }
                else
                {
                    if (includeInactive == true)
                    {
                        strQuery += @" inner join Client 
                        on Product.Client_ID = Client.Client_ID 
                        where Client.Agency_ID in (
                            select Agency_ID 
                            from User_Client 
                            where User_ID = @Username
                        ) 
                        or Client.Office_ID in (
                            select Office_ID 
                            from User_Client 
                            where User_ID = @Username
                        ) 
                        or Client.Client_ID in (
                            select Client_ID 
                            from User_Client 
                            where User_ID = @Username
                        ) 
                        group by Product.Product_ID, Product.Short_Name";
                    }
                    else
                    {
                        strQuery += @" inner join Client 
                        on Product.Client_ID = Client.Client_ID 
                        where Product.Valid = 'True' 
                        and (
                        Client.Agency_ID in (
                            select Agency_ID 
                            from User_Client 
                            where User_ID = @Username
                        ) 
                        or Client.Office_ID in (
                            select Office_ID 
                            from User_Client 
                            where User_ID = @Username
                        ) 
                        or Client.Client_ID in (
                            select Client_ID 
                            from User_Client 
                            where User_ID = @Username
                        )
                        ) 
                        group by Product.Product_ID, Product.Short_Name";
                    }
                }
            }
            if (strTable == "Master_Client")
            {
                if (strUsername == "")
                {
                    if (strFilter == "")
                    {
                        if (includeInactive == true)
                        {
                            strQuery += @" where Master_Client = '1'";
                        }
                        else
                        {
                            strQuery += @" where InactiveClient = '0' 
                            and Master_Client = '1'";
                        }
                    }
                    else
                    {
                        if (includeInactive == true)
                        {
                            strQuery += @" where Master_Client = '1' 
                            and Office_ID = @Filter";
                        }
                        else
                        {
                            strQuery += @" where InactiveClient = '0' 
                            and Master_Client = '1' 
                            and Office_ID = @Filter";
                        }
                    }
                }
                else
                {
                    if (includeInactive == true)
                    {
                        strQuery += @" where Master_Client = '1' 
                        and (
                        Agency_ID in (
                            select Agency_ID 
                            from User_Client 
                            where User_ID = @Username
                        ) 
                        or Office_ID in (
                            select Office_ID 
                            from User_Client 
                            where User_ID = @Username
                        ) 
                        or Client_ID in (
                            select Client_ID 
                            from User_Client 
                            where User_ID = @Username
                        )
                        )";
                    }
                    else
                    {
                        strQuery += @" where InactiveClient = '0' 
                        and Master_Client = '1' 
                        and (
                        Agency_ID in (
                            select Agency_ID 
                            from User_Client 
                            where User_ID = @Username
                        ) 
                        or Office_ID in (
                            select Office_ID 
                            from User_Client 
                            where User_ID = @Username
                        ) 
                        or Client_ID in (
                            select Client_ID 
                            from User_Client 
                            where User_ID = @Username
                        )
                        )";
                    }
                }
            }
            if (strTable == "GPM_Client")
            {
                if (strUsername == "")
                {
                    strQuery += @" where InactiveClient = '0' 
                    and Agency_ID = 'GPM'";
                }
                else
                {
                    strQuery += @" where InactiveClient = '0' 
                    and Agency_ID = 'GPM' 
                    and (
                    Agency_ID in (
                        select Agency_ID 
                        from User_Client 
                        where User_ID = @Username
                    ) 
                    or Office_ID in (
                        select Office_ID 
                        from User_Client 
                        where User_ID = @Username
                    ) 
                    or Client_ID in (
                        select Client_ID 
                        from User_Client 
                        where User_ID = @Username
                    )
                    )";
                }
            }
            if (strTable == "Media")
            {
                strQuery += " where Valid = 1";
            }
            if (strTable == "Media_Vendor")
            {
                if (includeInactive == false)
                {
                    strQuery += " where InActive = 0";
                }
            }
            if (strTable == "Master_Vendor")
            {
                if (includeInactive == true)
                {
                    strQuery += " where Master_Vendor = '1'";
                }
                else
                {
                    strQuery += @" where InActive = 0 
                    and Master_Vendor = '1'";
                }
            }

            if (strTable == "Master_Vendor_TV")
            {
                strTable = "Master_Vendor";
                if (includeInactive == true)
                {
                    strQuery += " where Master_Vendor = '1'";
                }
                else
                {
                    strQuery += @" where InActive = 0 
                    and Master_Vendor = '1'";
                }
            }
            if (strTable == "Media_Sub_Type")
            {
                if (strFilter == "")
                {
                    if (includeInactive == false)
                    {
                        strQuery += @" where isActive = 1";
                    }
                }
                else
                {
                    if (includeInactive == true)
                    {
                        strQuery += @" where Media_Type = @Filter";
                    }
                    else
                    {
                        strQuery += @" where isActive = 1 
                        and Media_Type = @Filter";
                    }
                }
            }
            if (strTable == "Master_Media_Type")
            {
                if (strFilter == "")
                {
                    if (includeInactive == false)
                    {
                        strQuery += @" where IsMaster = 1 and IsActive = 1";
                    }
                    else
                    {
                        strQuery += @" where IsMaster = 1";
                    }
                }
                else
                {
                    if (includeInactive == true)
                    {
                        strQuery += @" where IsMaster = 1 and Media_Type = @Filter";
                    }
                    else
                    {
                        strQuery += @" where IsMaster = 1 and isActive = 1 
                        and Media_Type = @Filter";
                    }
                }
            }
            if (strTable == "Product_Category")
            {
                strTable = @" Client_Category  cc
inner join Category c
	on cc.Category_ID = c.Category_ID
";
                if (strFilter != "")
                {
                    strQuery += " where cc.Client_ID = @Filter";
                }
            }
            if (strTable == "Brand" && strFilter != "")
                strQuery += " where Client_ID = @Filter";
            string strSQL = string.Format(strQuery, strCode, strName, strTable.Replace("Master_Client", "Client").Replace("GPM_Client", "Client").Replace("Master_Vendor", "Media_Vendor").Replace("Master_Media_Type", "Media_Type"));
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            if (strUsername != "")
            {
                sda.SelectCommand.Parameters.Add("@Username", SqlDbType.VarChar).Value = strUsername;
            }
            if (strFilter != "")
            {
                sda.SelectCommand.Parameters.Add("@Filter", SqlDbType.VarChar).Value = strFilter;
            }
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectMasterCommon3Col(string strCode, string strName, string strCol, string strTable, bool includeInactive, string strUsername, string strFilter)
        {
            string strQuery = "select {0} as MasterCode, {1} as MasterName, {2} as Master, TB.* from {3} as TB";
            if (strTable == "Media_Sub_Type")
            {
                strQuery += @" inner join Media_Type as MT 
                on TB.Media_Type = MT.Media_Type";
                if (strFilter == "")
                {
                    if (includeInactive == false)
                    {
                        strQuery += @" where TB.isActive = 1";
                    }
                }
                else
                {
                    if (includeInactive == true)
                    {
                        strQuery += @" where TB.Media_Type = @Filter";
                    }
                    else
                    {
                        strQuery += @" where TB.isActive = 1 
                        and TB.Media_Type = @Filter";
                    }
                }
            }
            string strSQL = string.Format(strQuery, strCode, strName, strCol, strTable.Replace("Master_Client", "Client").Replace("GPM_Client", "Client").Replace("Master_Vendor", "Media_Vendor").Replace("Master_Media_Type", "Media_Type"));
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            if (strUsername != "")
            {
                sda.SelectCommand.Parameters.Add("@Username", SqlDbType.VarChar).Value = strUsername;
            }
            if (strFilter != "")
            {
                sda.SelectCommand.Parameters.Add("@Filter", SqlDbType.VarChar).Value = strFilter;
            }
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectMediaByFilter(string strFilterBy, string strValue)
        {
            string strSQL = @"select m.Media_ID, 
            m.Short_Name, 
            m.Spending_Type, 
            mst.Media_Sub_Type, 
            mst.Short_Name as Media_Sub_Type_Name, 
            mt.Media_Type, 
            mt.Short_Name as Media_Type_Name 
            from Media as m 
            inner join Media_Sub_Type as mst 
            on m.Media_Sub_Type = mst.Media_Sub_Type 
            inner join Media_Type as mt 
            on mst.Media_Type = mt.Media_Type 
            where m.Valid = 1";
            if (strFilterBy == "MasterMediaType")
                strSQL += " and mt.Master_Media_Type = @Master_Media_Type";
            else if (strFilterBy == "MediaType")
                strSQL += " and mt.Media_Type = @Media_Type";
            else if (strFilterBy == "MediaSubType")
                strSQL += " and mst.Media_Sub_Type = @Media_Sub_Type";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            if (strFilterBy == "MasterMediaType")
                sda.SelectCommand.Parameters.Add("@Master_Media_Type", SqlDbType.VarChar).Value = strValue;
            else if (strFilterBy == "MediaType")
                sda.SelectCommand.Parameters.Add("@Media_Type", SqlDbType.VarChar).Value = strValue;
            else if (strFilterBy == "MediaSubType")
                sda.SelectCommand.Parameters.Add("@Media_Sub_Type", SqlDbType.VarChar).Value = strValue;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectMediaByMediaTypePeriod(string strMediaType, string strStartDate, string strEndDate)
        {
            string strSQL = @"select m.Media_ID, 
            m.Short_Name, 
            m.Spending_Type, 
            m.EffectiveDate, 
            m.InactiveDate, 
            mst.Media_Sub_Type, 
            mst.Short_Name as Media_Sub_Type_Name, 
            mt.Media_Type, 
            mt.Short_Name as Media_Type_Name 
            from Media as m 
            inner join Media_Sub_Type as mst 
            on m.Media_Sub_Type = mst.Media_Sub_Type 
            inner join Media_Type as mt 
            on mst.Media_Type = mt.Media_Type 
            where mt.Media_Type = @Media_Type 
            and (
                (
                    m.EffectiveDate <= @Start_Date 
                    and m.InactiveDate >= @Start_Date
                ) 
                or (
                    m.EffectiveDate <= @End_Date 
                    and m.InactiveDate >= @End_Date
                ) 
                or (
                    m.EffectiveDate >= @Start_Date 
                    and m.InactiveDate <= @End_Date
                )
            )";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Media_Type", SqlDbType.VarChar).Value = strMediaType;
            sda.SelectCommand.Parameters.Add("@Start_Date", SqlDbType.VarChar).Value = strStartDate;
            sda.SelectCommand.Parameters.Add("@End_Date", SqlDbType.VarChar).Value = strEndDate;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        public DataTable SelectMediaByMasterMediaTypePeriod(string strMasterMediaType, string strStartDate, string strEndDate)
        {
            string strSQL = @"select m.Media_ID, 
            m.Short_Name, 
            m.Spending_Type, 
            m.EffectiveDate, 
            m.InactiveDate, 
            mst.Media_Sub_Type, 
            mst.Short_Name as Media_Sub_Type_Name, 
            mt.Media_Type, 
            mt.Short_Name as Media_Type_Name 
            from Media as m 
            inner join Media_Sub_Type as mst 
            on m.Media_Sub_Type = mst.Media_Sub_Type 
            inner join Media_Type as mt 
            on mst.Media_Type = mt.Media_Type 
            where mt.Master_Media_Type = @Master_Media_Type 
                and mt.IsActive = 1
            and (
                (
                    m.EffectiveDate <= @Start_Date 
                    and m.InactiveDate >= @Start_Date
                ) 
                or (
                    m.EffectiveDate <= @End_Date 
                    and m.InactiveDate >= @End_Date
                ) 
                or (
                    m.EffectiveDate >= @Start_Date 
                    and m.InactiveDate <= @End_Date
                )
            )";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Master_Media_Type", SqlDbType.VarChar).Value = strMasterMediaType;
            sda.SelectCommand.Parameters.Add("@Start_Date", SqlDbType.VarChar).Value = strStartDate;
            sda.SelectCommand.Parameters.Add("@End_Date", SqlDbType.VarChar).Value = strEndDate;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectRateCardByFilter(string strFilterBy, string strValue, string strStartDate, string strEndDate)
        {
            string strSQL = @"
select
m.Media_ID
,m.Short_Name Media_Name
,m.Spending_Type
,m.EffectiveDate
,m.InactiveDate
,mt.Media_Type
,mt.Short_Name Media_Type_Name
,mst.Media_Sub_Type
,mst.Short_Name Media_Sub_Type_Name
,v.Media_Vendor_ID
,v.Short_Name Media_Vendor_Name
,bt.BuyTypeID
,bt.BuyTypeName
,bt.BuyTypeDisplay
,r.Position
,r.Rate
,isnull(r.Discount, 0) Discount
,r.Start_Date
,r.End_Date
from Print_RateCard r
inner join media m
	on m.Media_ID = r.Media_ID
inner join Media_Type mt
	on mt.Media_Type = m.Media_Type
inner join Media_Sub_Type mst
	on mst.Media_Sub_Type = m.Media_Sub_Type
inner join Media_Vendor v
	on v.Media_Vendor_ID = r.Media_Vendor_ID
inner join v_BuyType bt
	on bt.BuyTypeDisplay = 'By Agency - ' + r.Buy_type
where (
    (
        r.Start_Date <= @Start_Date 
        and r.End_Date >= @Start_Date
    ) 
    or (
        r.Start_Date <= @End_Date 
        and r.End_Date >= @End_Date
    ) 
    or (
        r.Start_Date >= @Start_Date 
        and r.End_Date <= @End_Date
    )
)
and (
    (
        m.EffectiveDate <= @Start_Date 
        and m.InactiveDate >= @Start_Date
    ) 
    or (
        m.EffectiveDate <= @End_Date 
        and m.InactiveDate >= @End_Date
    ) 
    or (
        m.EffectiveDate >= @Start_Date 
        and m.InactiveDate <= @End_Date
    )
)
";
            if (strFilterBy == "MasterMediaType")
                strSQL += " and mt.Master_Media_Type = @Master_Media_Type";
            else if (strFilterBy == "MediaType")
                strSQL += " and mt.Media_Type = @Media_Type";
            else if (strFilterBy == "MediaSubType")
                strSQL += " and mst.Media_Sub_Type = @Media_Sub_Type";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            if (strFilterBy == "MasterMediaType")
                sda.SelectCommand.Parameters.Add("@Master_Media_Type", SqlDbType.VarChar).Value = strValue;
            else if (strFilterBy == "MediaType")
                sda.SelectCommand.Parameters.Add("@Media_Type", SqlDbType.VarChar).Value = strValue;
            else if (strFilterBy == "MediaSubType")
                sda.SelectCommand.Parameters.Add("@Media_Sub_Type", SqlDbType.VarChar).Value = strValue;

            sda.SelectCommand.Parameters.Add("@Start_Date", SqlDbType.VarChar).Value = strStartDate;
            sda.SelectCommand.Parameters.Add("@End_Date", SqlDbType.VarChar).Value = strEndDate;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public bool ValidateMasterMediaType(string strMasterMediaType, string strMediaType)
        {
            string strSQL = @"select * 
            from Media_Type 
            where Master_Media_Type = @Master_Media_Type 
            and Media_Type = @Media_Type";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Master_Media_Type", SqlDbType.VarChar).Value = strMasterMediaType;
            sda.SelectCommand.Parameters.Add("@Media_Type", SqlDbType.VarChar).Value = strMediaType;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            bool valid = dt.Rows.Count == 0 ? false : true;
            return valid;
        }

        public DataTable SelectBuyingBrief()
        {
            string strSQL = @"
select bb.*
,  case Version_Approve 
		when 0 then 'Draft'
		when 4 then 'Approved'
		when 5 then 'Executing'
		when 8 then 'Actual'
	end CampaignStatus
, c.Short_Name ClientName
, p.Short_Name ProductName
from Buying_Brief bb
inner join client c
	on c.Client_ID = bb.Client_ID	
inner join Product p
	on p.Product_ID = bb.Product_ID
where left(Buying_Brief_id,6) = '201909'
and Media_Type = 'IT'	
";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        public DataTable SelectBuyingBrief(string strBB)
        {
            string strSQL = @"
select bb.*
,  case Version_Approve 
		when 0 then 'Draft'
		when 4 then 'Approved'
		when 5 then 'Executing'
		when 8 then 'Actual'
	end CampaignStatus
, c.Short_Name ClientName
, p.Short_Name ProductName
, bbm.Comment_Client CommentClient
, bbm.Comment_Vendor CommentVendor
, bbm.Comment_Advice_Note CommentAdviceNote
, case when Opt_in_Signed = 1 and  convert(date,getdate())  between Opt_in_StartDate and Opt_in_EndDate then 'Opt-in Signed' else ''  end Opt_In
from Buying_Brief bb
inner join client c
	on c.Client_ID = bb.Client_ID	
inner join Product p
	on p.Product_ID = bb.Product_ID
inner join Buying_Brief_Market bbm
	on bb.Buying_Brief_ID = bbm.Buying_Brief_ID
where bb.Buying_Brief_id =  @Buying_Brief_ID
";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.CommandTimeout = 0;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataTable CheckOpenSpotPlan(string strBB, string strVersion, string strUser)
        {
            string strSQL = @"CheckLockSpotPlan";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.SelectCommand.Parameters.Add("@User", SqlDbType.VarChar).Value = strUser;
            sda.SelectCommand.Parameters.Add("@SaveTime", SqlDbType.VarChar).Value = DateTime.Now.ToString("yyyyMMddHHmmss");
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        public DataTable SelectOnAirCheckingGPM(DateTime dtStartDate,DateTime dtEndDate,string strProgarmList, string strFilterBy,string strFilterValue)
        {
            string strSQL = "";
            if (strFilterBy == "Media")
            {
                strSQL = $@"
select 
ISNULL(GPM.Program_Group,'NAME_NOT_MATCH') Program_Group
,a.MasterAgency
,SP.Media_Vendor_ID
,MV.Short_Name as Vendor_Name
,M.Short_Name as Media_Name
,SP.Program as Program_Name
,SP.Start_time
,br.Brand_Name as Brand_Name
,P.Product_ID
,P.Short_Name as Product_Name
,P.Thai_Name as Product_Thai
,SP.Buying_brief_id as Buying_Brief_No
,CASE SP.Status	
	WHEN 0 THEN 'Draft'
	WHEN 5 THEN 'Executing'
	WHEN 8 THEN 'Actual'
End Status
,BB.Description as Campaign
,MT.Short_Name as Material
,sum(SP.Length*SP.Spots) AS Length
--,SPV.Approve
,SP.Net_Cost
, SP.Vendor_Net_Cost
,SP.Show_Date 
,sum(SP.Spots) Spots
,SP.Package
,SP.Weight
,C.Special
,SP.Program_Code
,SP.Start_time
,SP.End_Time
,BB.office_id
,O.Short_Name As Office_Name
,Case When (PG.Thai_Name is null) Then '' When (PG.Thai_Name is not null) Then PG.Thai_Name End As Thai_Name
,SP.Color
,BB.Media_Type
,BB.Client_ID as Client_ID
,C.Short_Name as Client_Name
, T.Short_Name AS Target
,CA.Short_name AS Creative_Agency_Name
, A.Short_name as Report_to_Agency 
from spot_plan SP  with(nolock) 
left join Buying_brief BB on SP.Buying_brief_id = BB.Buying_brief_id 
left join Creative_Agency CA on BB.Creative_Agency_id = CA.Creative_Agency_id 
left join Product P on BB.Product_ID = P.Product_ID left join Media M on SP.Media_ID = M.Media_ID 
left join Media_Vendor MV on SP.Media_Vendor_ID = MV.Media_Vendor_ID 
left join Material MT on SP.Material_ID  = MT.Material_ID 
left join Client C on C.Client_ID  = BB.Client_ID 
left join Brand br on BB.[Client_ID] = br.[Client_ID] and P.Brand_ID = br.Brand_ID
left join Office O on O.Office_ID = BB.Office_ID 
inner join Agency A on C.Report_to_Agency = A.Agency_ID 
left join Target T on T.Target_ID  = BB.Primary_Target 
inner join Spot_plan_Version SPV on (SP.Buying_brief_id = SPV.Buying_brief_id AND SP.Version = SPV.Version) 
left join Program PG on SP.Program_Code = PG.Program_Code And SP.Media_Vendor_ID = PG.Media_Vendor_ID And SP.Media_ID = PG.Media_ID And SP.Program_Type = PG.Program_Type 
left join GPM_Program_Mapping gpm on GPM.Program = SP.Program
where BB.Media_Sub_Type <> 'GM'
AND LEN(SP.Version) = 1 --SP.Status >= 5 --0 draf 4 Approve 5 Execute 8--Actual
AND SP.Show_Date between @StartDate and @EndDate
AND SP.Program In ({strProgarmList}) 
--AND SP.Program_Code In (select Program_Code from spot_plan where program in ({strProgarmList}) and Show_Date between @StartDate and @EndDate)
AND SP.Media_ID IN ({strFilterValue})
GROUP BY
ISNULL(GPM.Program_Group,'NAME_NOT_MATCH')
,a.MasterAgency
,SP.Media_Vendor_ID
,MV.Short_Name 
,M.Short_Name 
,SP.Program 
,SP.Start_time
,br.Brand_Name 
,P.Product_ID
,P.Short_Name 
,P.Thai_Name 
,SP.Buying_brief_id 
,CASE SP.Status	
	WHEN 0 THEN 'Draft'
	WHEN 5 THEN 'Executing'
	WHEN 8 THEN 'Actual'
End 
,BB.Description 
,MT.Short_Name 
--,SP.Length
--,SPV.Approve
,SP.Net_Cost
, SP.Vendor_Net_Cost
,SP.Show_Date 
--,SP.Spots
,SP.Package
,SP.Weight
,C.Special
,SP.Program_Code
,SP.Start_time
,SP.End_Time
,BB.office_id
,O.Short_Name 
,Case When (PG.Thai_Name is null) Then '' When (PG.Thai_Name is not null) Then PG.Thai_Name End
,SP.Color
,BB.Media_Type
,BB.Client_ID 
,C.Short_Name 
, T.Short_Name
,CA.Short_name 
, A.Short_name 
order by 
ISNULL(GPM.Program_Group,'NAME_NOT_MATCH') --Show 0
,case a.MasterAgency 
when 'MEC' then 1
when 'MS' then 2
when 'MC' then 3
when 'GPM' then 4
else 99
end --Show 0
,MV.Short_Name--Show 1
,SP.Program--Show 2
,SP.Start_time--Show 3
,SP.End_Time--Show 4
,br.Brand_Name--Show 5
,P.Short_Name--Show 6
,SP.Buying_brief_id--Show 7
,CASE SP.Status	
	WHEN 0 THEN 'Draft'
	WHEN 5 THEN 'Executing'
	WHEN 8 THEN 'Actual'
End --Show 8
,C.Short_Name--Show 8
,BB.Description--Show 9
,MT.Short_Name----Show 10
,sum(SP.Length)/sum(SP.Spots)--Show 11
,SP.Net_Cost --Show 12
,SP.Package --Show 13
,CA.Short_name--Show 14
,O.Short_Name--Show 15
, A.Short_name--Show 16

";
            }
            else
            {
                strSQL = $@"
select 
ISNULL(GPM.Program_Group,'NAME_NOT_MATCH') Program_Group
,a.MasterAgency
,SP.Media_Vendor_ID
,MV.Short_Name as Vendor_Name
,M.Short_Name as Media_Name
,SP.Program as Program_Name
,SP.Start_time
,br.Brand_Name as Brand_Name
,P.Product_ID
,P.Short_Name as Product_Name
,P.Thai_Name as Product_Thai
,SP.Buying_brief_id as Buying_Brief_No
,CASE SP.Status	
	WHEN 0 THEN 'Draft'
	WHEN 5 THEN 'Executing'
	WHEN 8 THEN 'Actual'
End Status
,BB.Description as Campaign
,MT.Short_Name as Material
,sum(SP.Length*SP.Spots) AS Length
--,SPV.Approve
,SP.Net_Cost
, SP.Vendor_Net_Cost
,SP.Show_Date 
,sum(SP.Spots) Spots
,SP.Package
,SP.Weight
,C.Special
,SP.Program_Code
,SP.Start_time
,SP.End_Time
,BB.office_id
,O.Short_Name As Office_Name
,Case When (PG.Thai_Name is null) Then '' When (PG.Thai_Name is not null) Then PG.Thai_Name End As Thai_Name
,SP.Color
,BB.Media_Type
,BB.Client_ID as Client_ID
,C.Short_Name as Client_Name
, T.Short_Name AS Target
,CA.Short_name AS Creative_Agency_Name
, A.Short_name as Report_to_Agency 
from spot_plan SP with(nolock) 
left join Buying_brief BB on SP.Buying_brief_id = BB.Buying_brief_id 
left join Creative_Agency CA on BB.Creative_Agency_id = CA.Creative_Agency_id 
left join Product P on BB.Product_ID = P.Product_ID left join Media M on SP.Media_ID = M.Media_ID 
left join Media_Vendor MV on SP.Media_Vendor_ID = MV.Media_Vendor_ID 
left join Material MT on SP.Material_ID  = MT.Material_ID 
left join Client C on C.Client_ID  = BB.Client_ID 
left join Brand br on BB.[Client_ID] = br.[Client_ID] and P.Brand_ID = br.Brand_ID
left join Office O on O.Office_ID = BB.Office_ID 
inner join Agency A on C.Report_to_Agency = A.Agency_ID 
left join Target T on T.Target_ID  = BB.Primary_Target 
inner join Spot_plan_Version SPV on (SP.Buying_brief_id = SPV.Buying_brief_id AND SP.Version = SPV.Version) 
left join Program PG on SP.Program_Code = PG.Program_Code And SP.Media_Vendor_ID = PG.Media_Vendor_ID And SP.Media_ID = PG.Media_ID And SP.Program_Type = PG.Program_Type 
left join GPM_Program_Mapping gpm on GPM.Program = SP.Program
where BB.Media_Sub_Type <> 'GM'
AND LEN(SP.Version) = 1 --SP.Status >= 5 --0 draf 4 Approve 5 Execute 8--Actual
AND SP.Show_Date between @StartDate and @EndDate
AND SP.Media_Vendor_ID = 'MGPME'
AND SP.Program In ({strProgarmList})
--AND SP.Program_Code In (select Program_Code from spot_plan where program in ({strProgarmList}) and Show_Date between @StartDate and @EndDate)
AND br.Brand_ID IN ({strFilterValue})
GROUP BY
ISNULL(GPM.Program_Group,'NAME_NOT_MATCH')
,a.MasterAgency
,SP.Media_Vendor_ID
,MV.Short_Name 
,M.Short_Name 
,SP.Program 
,SP.Start_time
,br.Brand_Name 
,P.Product_ID
,P.Short_Name 
,P.Thai_Name 
,SP.Buying_brief_id 
,CASE SP.Status	
	WHEN 0 THEN 'Draft'
	WHEN 5 THEN 'Executing'
	WHEN 8 THEN 'Actual'
End 
,BB.Description 
,MT.Short_Name 
--,SP.Length
--,SPV.Approve
,SP.Net_Cost
, SP.Vendor_Net_Cost
,SP.Show_Date 
--,SP.Spots
,SP.Package
,SP.Weight
,C.Special
,SP.Program_Code
,SP.Start_time
,SP.End_Time
,BB.office_id
,O.Short_Name 
,Case When (PG.Thai_Name is null) Then '' When (PG.Thai_Name is not null) Then PG.Thai_Name End
,SP.Color
,BB.Media_Type
,BB.Client_ID 
,C.Short_Name 
, T.Short_Name
,CA.Short_name 
, A.Short_name 
order by 
ISNULL(GPM.Program_Group,'NAME_NOT_MATCH') --Show 0
,case a.MasterAgency 
when 'MEC' then 1
when 'MS' then 2
when 'MC' then 3
when 'GPM' then 4
else 99
end --Show 0
,MV.Short_Name--Show 1
,SP.Program--Show 2
,SP.Start_time--Show 3
,SP.End_Time--Show 4
,br.Brand_Name--Show 5
,P.Short_Name--Show 6
,SP.Buying_brief_id--Show 7
,CASE SP.Status	
	WHEN 0 THEN 'Draft'
	WHEN 5 THEN 'Executing'
	WHEN 8 THEN 'Actual'
End --Show 8
,C.Short_Name--Show 8
,BB.Description--Show 9
,MT.Short_Name----Show 10
,sum(SP.Length)/sum(SP.Spots)--Show 11
,SP.Net_Cost --Show 12
,SP.Package --Show 13
,CA.Short_name--Show 14
,O.Short_Name--Show 15
, A.Short_name--Show 16

";
            }

            //strSQL = $@"";

            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 0;
            sda.SelectCommand.Parameters.Add("@StartDate", SqlDbType.NVarChar).Value = dtStartDate.ToString("yyyyMMdd");
            sda.SelectCommand.Parameters.Add("@EndDate", SqlDbType.NVarChar).Value = dtEndDate.ToString("yyyyMMdd");
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectSpotPlanTV(string strBB, string strVersion)
        {
            string strSQL = @"

select 
sp.Media_ID
,m.Short_Name MediaName
,sp.Media_Vendor_ID
,mv.Short_Name MediaVendorName
,sp.Start_Time
,sp.End_Time
,SP.Program
,Remark 
,sp.WeekdayLimit
,sp.Package AS Spot_Type
,Pkg
--,[Group]
,sp.Material_Key
,sp.Length
,sp.Rate
,sp.Discount
,sp.Net_Cost
,sp.Program_Type
,sp.Rating
,null [CPRP]
--,sp.*
, sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 1 then spots else 0 end) AS [1
W], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 2 then spots else 0 end) AS [2
H], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 3 then spots else 0 end) AS [3
F], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 4 then spots else 0 end) AS [4
A], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 5 then spots else 0 end) AS [5
S], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 6 then spots else 0 end) AS [6
M], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 7 then spots else 0 end) AS [7
T], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 8 then spots else 0 end) AS [8
W], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 9 then spots else 0 end) AS [9
H], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 10 then spots else 0 end) AS [10
F], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 11 then spots else 0 end) AS [11
A], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 12 then spots else 0 end) AS [12
S], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 13 then spots else 0 end) AS [13
M], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 14 then spots else 0 end) AS [14
T], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 15 then spots else 0 end) AS [15
W], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 16 then spots else 0 end) AS [16
H], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 17 then spots else 0 end) AS [17
F], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 18 then spots else 0 end) AS [18
A], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 19 then spots else 0 end) AS [19
S], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 20 then spots else 0 end) AS [20
M], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 21 then spots else 0 end) AS [21
T], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 22 then spots else 0 end) AS [22
W], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 23 then spots else 0 end) AS [23
H], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 24 then spots else 0 end) AS [24
F], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 25 then spots else 0 end) AS [25
A], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 26 then spots else 0 end) AS [26
S], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 27 then spots else 0 end) AS [27
M], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 28 then spots else 0 end) AS [28
T], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 29 then spots else 0 end) AS [29
W], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 30 then spots else 0 end) AS [30
H], sum(case when DAY(CONVERT(datetime, sp.Show_Date)) = 31 then spots else 0 end) AS [31
F]
from spot_plan sp
inner join media m
	on m.Media_ID =sp.Media_ID 
inner join Media_Vendor mv
	on mv.Media_Vendor_ID = sp.Media_Vendor_ID
inner join Media_Sub_Type mst
	on mst.Media_Sub_Type = sp.Media_Sub_Type
left join Media_Type mt
	on mt.Media_Type = sp.Media_Type
where  Buying_Brief_ID = '2023030525' AND [Version] = 'A'
group by 

sp.Media_ID
,m.Short_Name 
,sp.Media_Vendor_ID
,mv.Short_Name 
,sp.Start_Time
,sp.End_Time
,SP.Program
,Remark 
,sp.WeekdayLimit
,sp.Package 
,Pkg
--,[Group]
,sp.Material_Key
,sp.Length
,sp.Rate
,sp.Discount
,sp.Net_Cost
,sp.Program_Type
,sp.Rating";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectSpotPlanOD(string strBB, string strVersion,DateTime CampaignStartDate, DateTime CampaignEndDate)
        {
            int iMonthly = 0;
            string sqlCalendar = "";
            for (DateTime startDate = CampaignStartDate;
            startDate <= CampaignEndDate;
            startDate = new DateTime(startDate.AddMonths(1).Year, startDate.AddMonths(1).Month, 1))
            {
                sqlCalendar = sqlCalendar+$@"
,NULLIF(sum(case when show_date = {startDate.ToString("yyyyMM")}01 then Net_Cost else 0 end),0) [{startDate.ToString("yyyyMM")}]
,NULLIF(sum(case when show_date = {startDate.ToString("yyyyMM")}01 then Net_Cost_BeforePrintAdviseNote else 0 end),0) [{startDate.ToString("yyyyMM")}_Original]
,NULLIF(sum(case when show_date = {startDate.ToString("yyyyMM")}01 then Agency_Fee_Cost else 0 end),0) [{startDate.ToString("yyyyMM")}_Agency_Fee_Cost]
,max(case when show_date = {startDate.ToString("yyyyMM")}01 then IDKey else 0 end) [{startDate.ToString("yyyyMM")}_IDKey]
,max(case when show_date = {startDate.ToString("yyyyMM")}01 then Verified else 0 end) [{startDate.ToString("yyyyMM")}_Verfied]";
                iMonthly++;
            }

            string strSQL = @"select sp.*
,amt.Adept_Media_Type_Name AdeptMediaTypeName
,mt.Short_Name MediaTypeName
,mst.Short_Name MediaSubTypeName
,m.Short_Name MediaName 
,mv.Short_Name MediaVendorName
,Agency_Fee*100.00 AgencyFeePercent
,FORMAT (Convert(date,Start_Date), 'dd/MM/yyyy') as StartDate
,FORMAT (Convert(date,End_Date), 'dd/MM/yyyy') as EndDate
,Net_Cost*Agency_Fee AgencyFee
,ma.Short_Name MaterialName
,DATEDIFF(d,Convert(date,Start_Date),Convert(date,End_Date)) DifDays
,spPayment.*
from spot_plan sp
left join media m
	on m.Media_ID =sp.Media_ID 
left join Media_Vendor mv
	on mv.Media_Vendor_ID = sp.Media_Vendor_ID
left join Media_Sub_Type mst
	on mst.Media_Sub_Type = sp.Media_Sub_Type
left join Media_Type mt
	on mt.Media_Type = sp.Media_Type
left join Material ma
	on ma.Material_ID = sp.Material_ID
left join Adept_Media_Type amt
	on amt.Adept_Media_Type = m.Adept_Media_Type
left join (
            SELECT buying_brief_id
            ,item
            ,[version]
            " + sqlCalendar + @"
              FROM [dbo].[spot_plan_payment]
              WHERE Buying_Brief_ID = @Buying_Brief_ID
              AND  version = @Version
            group by buying_brief_id,item,[version]
) spPayment
	on spPayment.buying_brief_id = sp.Buying_Brief_ID
    and spPayment.item = sp.item
	and spPayment.version = sp.Version

where  sp.Buying_Brief_ID = @Buying_Brief_ID AND sp.[Version] = @Version
ORDER BY sp.Start_Date,sp.End_Date";

            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.SelectCommand.CommandTimeout = 0;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        
        public DataTable SelectSpotPlan(string strBB, string strVersion)
        {
            string strSQL = @"select sp.*
,amt.Adept_Media_Type_Name AdeptMediaTypeName
,mt.Short_Name MediaTypeName
,mst.Short_Name MediaSubTypeName
,m.Short_Name MediaName 
,mv.Short_Name MediaVendorName
,FORMAT (Convert(date,Show_Date), 'dd/MM/yyyy') as ShowDate
,FORMAT (Convert(date,Start_Date), 'dd/MM/yyyy') as StartDate
,FORMAT (Convert(date,End_Date), 'dd/MM/yyyy') as EndDate
,Net_Cost*Spots Spending
,Agency_Fee*100.00 AgencyFeePercent
,(Net_Cost*Spots)*Agency_Fee AgencyFee
,ma.Short_Name MaterialName
from spot_plan sp
left join media m
	on m.Media_ID =sp.Media_ID 
left join Media_Vendor mv
	on mv.Media_Vendor_ID = sp.Media_Vendor_ID
left join Media_Sub_Type mst
	on mst.Media_Sub_Type = sp.Media_Sub_Type
left join Media_Type mt
	on mt.Media_Type = sp.Media_Type
left join Material ma
	on ma.Material_ID = sp.Material_ID
left join Adept_Media_Type amt
	on amt.Adept_Media_Type = m.Adept_Media_Type
where  sp.Buying_Brief_ID = @Buying_Brief_ID AND sp.[Version] = @Version";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectSpotPlanVersion(string strBB, string strVersion)
        {
            string strSQL = @"select *
from spot_plan_version
where  Buying_Brief_ID = @Buying_Brief_ID AND [Version] = @Version";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectMediaSC(string strBB, string strVersion)
        {
            string strSQL = @"select sp.Media_ID, 
            m.Short_Name as Media_Name 
            from Spot_Plan as sp 
            left outer join media as m 
            on sp.Media_ID = m.Media_ID 
            where sp.Buying_Brief_ID = @Buying_Brief_ID 
            and sp.Version = @Version 
            group by sp.Media_ID, 
            m.Short_Name";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataSet SelectSpotPlanForSchedule(string strBB, string strVersion, string startDate, string endDate, string strMedia)
        {
            string strSQL = @"
select 
ca.Short_Name Creative_Agency_Name
,a.Icon_Path
,c.Short_Name Client_Name
,p.Short_Name Product_Name
,bb.Description Campaign_Name
,bb.Buying_Brief_ID
,bbm.Comment_Client Remark
,FORMAT (Convert(date,bb.Campaign_Start_Date), 'dd/MM/yyyy') +' - '+FORMAT (Convert(date,bb.Campaign_End_Date), 'dd/MM/yyyy') as Campaign_Period
,bb.Commission_Type
from Buying_Brief bb
inner join Buying_Brief_Market bbm
	on bbm.Buying_Brief_ID = bb.Buying_Brief_ID
inner join client c
	on c.Client_ID = bb.Client_ID
inner join product p
	on p.Product_ID = bb.Product_ID 
inner join Creative_Agency ca
	on ca.Creative_Agency_ID = c.Creative_Agency_ID
inner join agency a
	on a.Agency_ID = c.Agency_ID
where bb.Buying_Brief_ID = @Buying_Brief_ID 

SELECT bbmt.Buying_Brief_id,
Material = Stuff(
(select distinct ','+ Material_Key +': '+ Short_Name
from Material m inner join Buying_Brief_Market_Material bmt
on m.Material_ID = bmt.Material_ID
where bmt.Buying_Brief_ID = bbmt.Buying_Brief_ID
for xml path('')),1,1,'')
FROM Buying_Brief_Market_Material bbmt
where bbmt.Buying_Brief_ID = @Buying_Brief_ID
GROUP BY bbmt.Buying_Brief_id

select amt.Adept_Media_Type_Name
,sum(sp.Cost_BuyType*sp.Spots) Media_Cost
,sum(sp.Net_Cost*sp.Agency_Fee) Agency_Commission
from (
    select s.*
    ,case when s.BuyTypeName = 'By Client' then 0 else s.Net_Cost end as Cost_BuyType
    from Spot_Plan as s
    where s.Buying_Brief_ID = @Buying_Brief_ID
    and s.Version = @Version
    and s.Media_ID in (" + strMedia + @")
    and s.Start_Date <= @ED
    and s.End_Date >= @SD
) as sp
inner join Media_Sub_Type mst
on sp.Media_Sub_Type = mst.Media_Sub_Type
inner join Media m
on sp.Media_ID = m.Media_ID
inner join Adept_Media_Type amt
on m.Adept_Media_Type = amt.Adept_Media_Type
group by m.Adept_Media_Type
,amt.Adept_Media_Type_Name
,sp.Agency_Fee
having sum(sp.Cost_BuyType*sp.Spots) + sum(sp.Net_Cost*sp.Agency_Fee) <> 0
order by amt.Adept_Media_Type_Name
";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.SelectCommand.Parameters.Add("@SD", SqlDbType.VarChar).Value = startDate;
            sda.SelectCommand.Parameters.Add("@ED", SqlDbType.VarChar).Value = endDate;
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }

        public DataTable SelectSpotPlanForSchedule_Details(string strBB, string strVersion, string strMedia, string startDate, string endDate)
        {
            string strSQL = @"select amt.Adept_Media_Type_Name + ' | ' + mst.Short_Name as Media_Sub_Type, 
            case when isnull(sp.Program, '') = '' then m.Short_Name else m.Short_Name + ' - ' + sp.Program end as Description, 
            format(convert(date, sp.Start_Date), 'dd/MM/yyyy') + ' - ' + format(convert(date, sp.End_Date), 'dd/MM/yyyy') as Schedule, 
            sp.Material_Key as Materail, 
            sp.Market_Price as Impression, 
            sp.Rating as GRP, 
            sp.CPRP_Cost as CPRP, 
            sp.Rate as Total_Cost, 
            sp.Discount as Disc, 
            sp.Net_Cost as Net_Cost, 
            sp.Agency_Fee * 100 as Agency_Commission_Percentage, 
            sp.Net_Cost * sp.Agency_Fee as Agency_Commission 
            from Spot_Plan as sp 
            inner join Media as m 
            on sp.Media_ID = m.Media_ID 
            inner join Media_Vendor as mv 
            on sp.Media_Vendor_ID = mv.Media_Vendor_ID 
            inner join Media_Sub_Type as mst 
            on sp.Media_Sub_Type = mst.Media_Sub_Type 
            inner join Adept_Media_Type as amt 
            on m.Adept_Media_Type = amt.Adept_Media_Type 
            left join Material as mt 
            on sp.Material_ID = mt.Material_ID 
            where sp.Buying_Brief_ID = @Buying_Brief_ID 
            and sp.Version = @Version 
            and sp.Media_ID in (" + strMedia + @") 
            and sp.Start_Date <= @ED 
            and sp.End_Date >= @SD 
            order by amt.Adept_Media_Type_Name, 
            mst.Short_Name, 
            sp.Show_Date, 
            sp.Media_ID, 
            m.Short_Name, 
            sp.Start_Time, 
            sp.Program, 
            sp.Material_Key";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.SelectCommand.Parameters.Add("@SD", SqlDbType.VarChar).Value = startDate;
            sda.SelectCommand.Parameters.Add("@ED", SqlDbType.VarChar).Value = endDate;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataSet SelectSpotPlanForScheduleIT(string strBB, string strVersion, string startDate, string endDate, string strMedia)
        {
            string strSQL = @"
select 
ca.Short_Name Creative_Agency_Name
,a.Icon_Path
,c.Short_Name Client_Name
,mt.Short_Name Media_Type_Name
,p.Short_Name Product_Name
,bb.Description Campaign_Name
,bb.Buying_Brief_ID
,bbm.Comment_Client Remark
,FORMAT (Convert(date,bb.Campaign_Start_Date), 'dd/MM/yyyy') +' - '+FORMAT (Convert(date,bb.Campaign_End_Date), 'dd/MM/yyyy') as Campaign_Period
,bb.Commission_Type
--,spv.Revised_No
from Buying_Brief bb
inner join Buying_Brief_Market bbm
	on bbm.Buying_Brief_ID = bb.Buying_Brief_ID
inner join client c
	on c.Client_ID = bb.Client_ID
inner join product p
	on p.Product_ID = bb.Product_ID 
inner join Creative_Agency ca
	on ca.Creative_Agency_ID = c.Creative_Agency_ID
inner join Agency a
	on a.Agency_ID = c.Agency_ID
inner join Media_Type mt
	on bb.Media_Type = mt.Media_Type
--inner join spot_plan_version spv
    --on spv.Buying_Brief_ID = bb.Buying_Brief_ID and spv.version = '" + strVersion + @"'
where  bb.Buying_Brief_ID = @Buying_Brief_ID 

SELECT bbmt.Buying_Brief_id,
Material = Stuff(
(select distinct ','+ Material_Key +': '+ Short_Name
from Material m inner join Buying_Brief_Market_Material bmt
on m.Material_ID = bmt.Material_ID
where bmt.Buying_Brief_ID = bbmt.Buying_Brief_ID
for xml path('')),1,1,'')
FROM Buying_Brief_Market_Material bbmt
where bbmt.Buying_Brief_ID = @Buying_Brief_ID
GROUP BY bbmt.Buying_Brief_id

select amt.Adept_Media_Type_Name, 
--sum(sp.Cost_BuyType * sp.Spots) as Media_Cost, 
--sum(sp.Net_Cost * sp.Agency_Fee) as Agency_Commission 
round(sum(sp.Cost_BuyType * sp.Spots),2) as Media_Cost, 
round(sum(round(convert(decimal(18,4),sp.Net_Cost) * convert(decimal(18,4),sp.Agency_Fee),2)),2) as Agency_Commission 
from (
    select s.*, 
    case when s.BuyTypeName = 'By Client' then 0 else s.Net_Cost end as Cost_BuyType 
    from Spot_Plan as s 
    where s.Buying_Brief_ID = @Buying_Brief_ID 
    and s.Version = @Version 
    and s.Media_ID in (" + strMedia + @") 
    and s.Start_Date <= @ED 
    and s.End_Date >= @SD
) as sp 
inner join Media_Sub_Type as mst 
on sp.Media_Sub_Type = mst.Media_Sub_Type 
inner join Media as m 
on sp.Media_ID = m.Media_ID 
inner join Adept_Media_Type as amt 
on m.Adept_Media_Type = amt.Adept_Media_Type 
group by m.Adept_Media_Type, 
amt.Adept_Media_Type_Name, 
sp.Agency_Fee 
having sum(sp.Cost_BuyType * sp.Spots) + sum(sp.Net_Cost * sp.Agency_Fee) <> 0 
order by amt.Adept_Media_Type_Name";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.SelectCommand.Parameters.Add("@SD", SqlDbType.VarChar).Value = startDate;
            sda.SelectCommand.Parameters.Add("@ED", SqlDbType.VarChar).Value = endDate;
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }

        public DataTable SelectSpotPlanForScheduleIT_Details(string strBB, string strVersion, string strMedia, string startDate, string endDate, string buyType)
        {
            string strSQL = "";
            if (buyType == "AG")
                strSQL = @"select amt.Adept_Media_Type_Name + ' | ' + mst.Short_Name as Media_Sub_Type, 
                case when isnull(sp.Program, '') = '' then m.Short_Name else m.Short_Name + ' - ' + sp.Program end as Description, 
                format(convert(date, sp.Start_Date), 'dd/MM/yyyy') + ' - ' + format(convert(date, sp.End_Date), 'dd/MM/yyyy') as Schedule, 
                sp.Deadline_Terminate as Buy_Type, 
                sp.Unit, 
                --sp.Rate as Total_Cost,
                round(sp.Rate,2) as Total_Cost, 
                sp.Discount as Disc, 
                '' as Col_H, 
                '' as Col_I, 
                --sp.Net_Cost as Net_Cost, 
                round(sp.Net_Cost,2) as Net_Cost, 
                sp.Agency_Fee * 100.00 as Agency_Commission_Percentage, 
                --sp.Net_Cost * sp.Agency_Fee as Agency_Commission 
                round(convert(decimal(18,4),sp.Net_Cost) * convert(decimal(18,4),sp.Agency_Fee),4) as Agency_Commission 
                from Spot_Plan as sp 
                inner join Media as m 
                on sp.Media_ID = m.Media_ID 
                inner join Media_Vendor as mv 
                on sp.Media_Vendor_ID = mv.Media_Vendor_ID 
                inner join Media_Sub_Type as mst 
                on sp.Media_Sub_Type = mst.Media_Sub_Type 
                inner join Adept_Media_Type as amt 
                on m.Adept_Media_Type = amt.Adept_Media_Type 
                left join Material as mt 
                on sp.Material_ID = mt.Material_ID 
                where sp.Buying_Brief_ID = @Buying_Brief_ID 
                and sp.Version = @Version 
                and sp.Media_ID in (" + strMedia + @") 
                and sp.Start_Date <= @ED 
                and sp.End_Date >= @SD 
                order by amt.Adept_Media_Type_Name, 
                mst.Short_Name, 
                sp.Show_Date, 
                sp.Media_ID, 
                m.Short_Name, 
                sp.Start_Time, 
                sp.Program, 
                sp.Material_Key";
            else
                strSQL = @"select case when isnull(sp.Program, '') = '' then m.Short_Name else m.Short_Name + ' - ' + sp.Program end as Description, 
                mv.Short_Name as Vendor_Name, 
                format(convert(date, sp.Start_Date), 'dd/MM/yyyy') + ' - ' + format(convert(date, sp.End_Date), 'dd/MM/yyyy') as Schedule, 
                sp.Deadline_Terminate as Buy_Type, 
                sp.Unit, 
                --sp.Rate as Total_Cost,
                round(sp.Rate,2) as Total_Cost, 
                sp.Discount as Disc, 
                '' as Col_H, 
                '' as Col_I, 
                --sp.Net_Cost as Net_Cost
                round(sp.Net_Cost,2) as Net_Cost 
                from Spot_Plan as sp 
                inner join Media as m 
                on sp.Media_ID = m.Media_ID 
                inner join Media_Vendor as mv 
                on sp.Media_Vendor_ID = mv.Media_Vendor_ID 
                inner join Media_Sub_Type as mst 
                on sp.Media_Sub_Type = mst.Media_Sub_Type 
                inner join Adept_Media_Type as amt 
                on m.Adept_Media_Type = amt.Adept_Media_Type 
                left join Material as mt 
                on sp.Material_ID = mt.Material_ID 
                where sp.Buying_Brief_ID = @Buying_Brief_ID 
                and sp.Version = @Version 
                and sp.Media_ID in (" + strMedia + @") 
                and sp.Start_Date <= @ED 
                and sp.End_Date >= @SD 
                and sp.BuyTypeID = @BuyType 
                order by amt.Adept_Media_Type_Name, 
                mst.Short_Name, 
                sp.Show_Date, 
                sp.Media_ID, 
                m.Short_Name, 
                sp.Start_Time, 
                sp.Program, 
                sp.Material_Key";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.SelectCommand.Parameters.Add("@SD", SqlDbType.VarChar).Value = startDate;
            sda.SelectCommand.Parameters.Add("@ED", SqlDbType.VarChar).Value = endDate;
            sda.SelectCommand.Parameters.Add("@BuyType", SqlDbType.VarChar).Value = buyType;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        public DataTable SelectSpotPlanForScheduleOD_Details(string strBB, string strVersion, string strMedia, string startDate, string endDate, string buyType)
        {
            string strSQL = "";
            if (buyType == "AG")
                strSQL = @"
select amt.Adept_Media_Type_Name + ' | ' + mst.Short_Name as Media_Sub_Type, 
            m.short_name as Media_Name,
            Province,
            '' as Col_D,
            isnull(sp.Program, '') as Description, 
            '' as Col_F, 
            '' as Col_G, 
            sp.State as Cost_Type, 
            '' as Col_I, 
            sp.Deadline_Terminate as Buy_Type, 
            sp.SizeHW, 
            sp.Unit,
            format(convert(date, Start_Date), 'dd/MM/yyyy') + ' - ' + format(convert(date, End_Date), 'dd/MM/yyyy') as Schedule,
            '' as Col_M,
            --DATEDIFF ( d , convert(date, Start_Date) , convert(date, End_Date) ) as Total_Days,
            round(sp.Net_Cost,2) as Net_Cost, 
            sp.Agency_Fee * 100.00 as Agency_Commission_Percentage, 
            --round(convert(decimal(18,4),sp.Net_Cost) * convert(decimal(18,4),sp.Agency_Fee),4) as Agency_Commission ,
            spp.AgencyFeeCost as Agency_Commission ,
            sp.Remark
                from Spot_Plan as sp 
                inner join Media as m 
                on sp.Media_ID = m.Media_ID 
                inner join Media_Vendor as mv 
                on sp.Media_Vendor_ID = mv.Media_Vendor_ID 
                inner join Media_Sub_Type as mst 
                on sp.Media_Sub_Type = mst.Media_Sub_Type 
                inner join Adept_Media_Type as amt 
                on m.Adept_Media_Type = amt.Adept_Media_Type 
                left join Material as mt 
                on sp.Material_ID = mt.Material_ID 
                inner join (
SELECT buying_brief_id
            ,item
            ,[version]
,sum(Agency_Fee_Cost) AgencyFeeCost 
FROM [dbo].[spot_plan_payment]
WHERE Buying_Brief_ID = @Buying_Brief_ID 
AND  version = @Version 
group by buying_brief_id,item,[version]
) spp
                    on spp.Buying_Brief_ID = sp.Buying_Brief_ID
                        and spp.version = sp.version
                        and spp.item = sp.item
                where sp.Buying_Brief_ID = @Buying_Brief_ID 
                and sp.Version = @Version 
                and sp.Media_ID in (" + strMedia + @") 
                and sp.Start_Date <= @ED 
                and sp.End_Date >= @SD 
                order by amt.Adept_Media_Type_Name, 
                mst.Short_Name, 
                sp.Show_Date, 
                sp.Media_ID, 
                m.Short_Name, 
                sp.Start_Time, 
                sp.Program, 
                sp.Material_Key";
            else //Not Use Yet
                strSQL = @"select case when isnull(sp.Program, '') = '' then m.Short_Name else m.Short_Name + ' - ' + sp.Program end as Description, 
                mv.Short_Name as Vendor_Name, 
                format(convert(date, Start_Date), 'dd/MM/yyyy') + ' - ' + format(convert(date, End_Date), 'dd/MM/yyyy') as Schedule, 
                sp.Deadline_Terminate as Buy_Type, 
                sp.Unit, 
                --sp.Rate as Total_Cost,
                round(sp.Rate,2) as Total_Cost, 
                sp.Discount as Disc, 
                '' as Col_H, 
                '' as Col_I, 
                --sp.Net_Cost as Net_Cost
                round(sp.Net_Cost,2) as Net_Cost 
                from Spot_Plan as sp 
                inner join Media as m 
                on sp.Media_ID = m.Media_ID 
                inner join Media_Vendor as mv 
                on sp.Media_Vendor_ID = mv.Media_Vendor_ID 
                inner join Media_Sub_Type as mst 
                on sp.Media_Sub_Type = mst.Media_Sub_Type 
                inner join Adept_Media_Type as amt 
                on m.Adept_Media_Type = amt.Adept_Media_Type 
                left join Material as mt 
                on sp.Material_ID = mt.Material_ID 
                where sp.Buying_Brief_ID = @Buying_Brief_ID 
                and sp.Version = @Version 
                and sp.Media_ID in (" + strMedia + @") 
                and sp.Start_Date <= @ED 
                and sp.End_Date >= @SD 
                and sp.BuyTypeID = @BuyType 
                order by amt.Adept_Media_Type_Name, 
                mst.Short_Name, 
                sp.Show_Date, 
                sp.Media_ID, 
                m.Short_Name, 
                sp.Start_Time, 
                sp.Program, 
                sp.Material_Key";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.SelectCommand.Parameters.Add("@SD", SqlDbType.VarChar).Value = startDate;
            sda.SelectCommand.Parameters.Add("@ED", SqlDbType.VarChar).Value = endDate;
            sda.SelectCommand.Parameters.Add("@BuyType", SqlDbType.VarChar).Value = buyType;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        public DataTable SelectSpotPlanForScheduleOD_Summary(string strBB, string strVersion, string strMedia, string startDate, string endDate, string buyType,string strYear)
        {
            string strSQL = "";
            if (buyType == "AG")
                strSQL = $@"
select amt.Adept_Media_Type_Name
,spp.[Detail]
,sum(spp.[JAN]) [JAN]
,sum(spp.[FEB]) [FEB]
,sum(spp.[MAR]) [MAR]
,sum(spp.[APR]) [APR]
,sum(spp.[MAY]) [MAY]
,sum(spp.[JUN]) [JUN]
,sum(spp.[JUL]) [JUL]
,sum(spp.[AUG]) [AUG]
,sum(spp.[SEP]) [SEP]
,sum(spp.[OCT]) [OCT]
,sum(spp.[NOV]) [NOV]
,sum(spp.[DEC]) [DEC]

from 
(

SELECT 
1 as orderRow
,buying_brief_id
            ,item
            ,[version]
,'Media Cost' as [Detail]
,sum(case when show_date = {strYear}0101 then Net_Cost else 0 end) [JAN]
,sum(case when show_date = {strYear}0201 then Net_Cost else 0 end) [FEB]
,sum(case when show_date = {strYear}0301 then Net_Cost else 0 end) [MAR]
,sum(case when show_date = {strYear}0401 then Net_Cost else 0 end) [APR]
,sum(case when show_date = {strYear}0501 then Net_Cost else 0 end) [MAY]
,sum(case when show_date = {strYear}0601 then Net_Cost else 0 end) [JUN]
,sum(case when show_date = {strYear}0701 then Net_Cost else 0 end) [JUL]
,sum(case when show_date = {strYear}0801 then Net_Cost else 0 end) [AUG]
,sum(case when show_date = {strYear}0901 then Net_Cost else 0 end) [SEP]
,sum(case when show_date = {strYear}1001 then Net_Cost else 0 end) [OCT]
,sum(case when show_date = {strYear}1101 then Net_Cost else 0 end) [NOV]
,sum(case when show_date = {strYear}1201 then Net_Cost else 0 end) [DEC]
              FROM [dbo].[spot_plan_payment]
              WHERE Buying_Brief_ID = @Buying_Brief_ID
              AND  version = @Version
			  and left(show_date,4) = @Year
              and show_date between @SD and @ED
            group by buying_brief_id,item,[version]

UNION
SELECT 
2 as orderRow
,buying_brief_id
,item
,[version]
,'Agency Commission' as [Detail]
,sum(case when show_date = {strYear}0101 then Agency_Fee_Cost else 0 end) [JAN]
,sum(case when show_date = {strYear}0201 then Agency_Fee_Cost else 0 end) [FEB]
,sum(case when show_date = {strYear}0301 then Agency_Fee_Cost else 0 end) [MAR]
,sum(case when show_date = {strYear}0401 then Agency_Fee_Cost else 0 end) [APR]
,sum(case when show_date = {strYear}0501 then Agency_Fee_Cost else 0 end) [MAY]
,sum(case when show_date = {strYear}0601 then Agency_Fee_Cost else 0 end) [JUN]
,sum(case when show_date = {strYear}0701 then Agency_Fee_Cost else 0 end) [JUL]
,sum(case when show_date = {strYear}0801 then Agency_Fee_Cost else 0 end) [AUG]
,sum(case when show_date = {strYear}0901 then Agency_Fee_Cost else 0 end) [SEP]
,sum(case when show_date = {strYear}1001 then Agency_Fee_Cost else 0 end) [OCT]
,sum(case when show_date = {strYear}1101 then Agency_Fee_Cost else 0 end) [NOV]
,sum(case when show_date = {strYear}1201 then Agency_Fee_Cost else 0 end) [DEC]
              FROM [dbo].[spot_plan_payment]
              WHERE Buying_Brief_ID = @Buying_Brief_ID
              AND  version = @Version
			  and left(show_date,4) = @Year
            group by buying_brief_id,item,[version]
UNION
SELECT 
3 as orderRow
,buying_brief_id
,item
,[version]
,'Sub Total' as [Detail]
,sum(case when show_date = {strYear}0101 then Payment_Term else 0 end) [JAN]
,sum(case when show_date = {strYear}0201 then Payment_Term else 0 end) [FEB]
,sum(case when show_date = {strYear}0301 then Payment_Term else 0 end) [MAR]
,sum(case when show_date = {strYear}0401 then Payment_Term else 0 end) [APR]
,sum(case when show_date = {strYear}0501 then Payment_Term else 0 end) [MAY]
,sum(case when show_date = {strYear}0601 then Payment_Term else 0 end) [JUN]
,sum(case when show_date = {strYear}0701 then Payment_Term else 0 end) [JUL]
,sum(case when show_date = {strYear}0801 then Payment_Term else 0 end) [AUG]
,sum(case when show_date = {strYear}0901 then Payment_Term else 0 end) [SEP]
,sum(case when show_date = {strYear}1001 then Payment_Term else 0 end) [OCT]
,sum(case when show_date = {strYear}1101 then Payment_Term else 0 end) [NOV]
,sum(case when show_date = {strYear}1201 then Payment_Term else 0 end) [DEC]
              FROM [dbo].[spot_plan_payment]
              WHERE Buying_Brief_ID = @Buying_Brief_ID
              AND  version = @Version
			  and left(show_date,4) = @Year
            group by buying_brief_id,item,[version]
) spp
inner join spot_plan sp
	on sp.Buying_Brief_ID = spp.buying_brief_id
	and sp.Version = spp.version
	and sp.item = spp.item
inner join media m
	on m.Media_ID = sp.Media_ID
inner join Adept_Media_Type as amt 
            on m.Adept_Media_Type = amt.Adept_Media_Type 
WHERE sp.Buying_Brief_ID = @Buying_Brief_ID 
and sp.Version = @Version 
and sp.Media_ID in (" + strMedia + @") 
and sp.Start_Date <= @ED 
and sp.End_Date >= @SD 
group by 
amt.Adept_Media_Type_Name
,spp.orderRow
,spp.[Detail] ";
            else
                strSQL = @"select case when isnull(sp.Program, '') = '' then m.Short_Name else m.Short_Name + ' - ' + sp.Program end as Description, 
                mv.Short_Name as Vendor_Name, 
                format(convert(date, Start_Date), 'dd/MM/yyyy') + ' - ' + format(convert(date, End_Date), 'dd/MM/yyyy') as Schedule, 
                sp.Deadline_Terminate as Buy_Type, 
                sp.Unit, 
                --sp.Rate as Total_Cost,
                round(sp.Rate,2) as Total_Cost, 
                sp.Discount as Disc, 
                '' as Col_H, 
                '' as Col_I, 
                --sp.Net_Cost as Net_Cost
                round(sp.Net_Cost,2) as Net_Cost 
                from Spot_Plan as sp 
                inner join Media as m 
                on sp.Media_ID = m.Media_ID 
                inner join Media_Vendor as mv 
                on sp.Media_Vendor_ID = mv.Media_Vendor_ID 
                inner join Media_Sub_Type as mst 
                on sp.Media_Sub_Type = mst.Media_Sub_Type 
                inner join Adept_Media_Type as amt 
                on m.Adept_Media_Type = amt.Adept_Media_Type 
                left join Material as mt 
                on sp.Material_ID = mt.Material_ID 
                where sp.Buying_Brief_ID = @Buying_Brief_ID 
                and sp.Version = @Version 
                and sp.Media_ID in (" + strMedia + @") 
                and sp.Start_Date <= @ED 
                and sp.End_Date >= @SD 
                and sp.BuyTypeID = @BuyType 
                order by amt.Adept_Media_Type_Name, 
                mst.Short_Name, 
                sp.Show_Date, 
                sp.Media_ID, 
                m.Short_Name, 
                sp.Start_Time, 
                sp.Program, 
                sp.Material_Key";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.SelectCommand.Parameters.Add("@SD", SqlDbType.VarChar).Value = startDate;
            sda.SelectCommand.Parameters.Add("@ED", SqlDbType.VarChar).Value = endDate;
            sda.SelectCommand.Parameters.Add("@BuyType", SqlDbType.VarChar).Value = buyType;
            sda.SelectCommand.Parameters.Add("@Year", SqlDbType.VarChar).Value = strYear;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        public DataTable SelectSpotPlanForScheduleLSIT_Details(string strBB, string strVersion, string strMedia, string startDate, string endDate)
        {
            string strSQL = @"select amt.Adept_Media_Type_Name + ' | ' + mst.Short_Name as Media_Sub_Type, 
            case when isnull(sp.Program, '') = '' then m.Short_Name else m.Short_Name + ' - ' + sp.Program end as Description, 
            format(convert(date, sp.Start_Date), 'dd/MM/yyyy') + ' - ' + format(convert(date, sp.End_Date), 'dd/MM/yyyy') as Schedule, 
            '' as Col_D, 
            sp.Deadline_Terminate as Buy_Type, 
            '' as Col_F, 
            '' as Col_G, 
            '' as Col_H, 
            sp.Net_Cost as Net_Cost, 
            sp.Unit 
            from Spot_Plan as sp 
            inner join Media as m 
            on sp.Media_ID = m.Media_ID 
            inner join Media_Vendor as mv 
            on sp.Media_Vendor_ID = mv.Media_Vendor_ID 
            inner join Media_Sub_Type as mst 
            on sp.Media_Sub_Type = mst.Media_Sub_Type 
            inner join Adept_Media_Type as amt 
            on m.Adept_Media_Type = amt.Adept_Media_Type 
            left join Material as mt 
            on sp.Material_ID = mt.Material_ID 
            where sp.Buying_Brief_ID = @Buying_Brief_ID 
            and sp.Version = @Version 
            and sp.Media_ID in (" + strMedia + @") 
            and sp.Start_Date <= @ED 
            and sp.End_Date >= @SD 
            order by amt.Adept_Media_Type_Name, 
            mst.Short_Name, 
            sp.Show_Date, 
            sp.Media_ID, 
            m.Short_Name, 
            sp.Start_Time, 
            sp.Program, 
            sp.Material_Key";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.SelectCommand.Parameters.Add("@SD", SqlDbType.VarChar).Value = startDate;
            sda.SelectCommand.Parameters.Add("@ED", SqlDbType.VarChar).Value = endDate;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataSet SelectSpotPlanForScheduleES(string strBB, string strVersion, string startDate, string endDate, string strMedia)
        {
            string strSQL = @"
select 
ca.Short_Name Creative_Agency_Name
,a.Icon_Path
,c.Short_Name Client_Name
,mt.Short_Name Media_Type_Name
,p.Short_Name Product_Name
,bb.Description Campaign_Name
,bb.Buying_Brief_ID
,bbm.Comment_Client Remark
,FORMAT (Convert(date,bb.Campaign_Start_Date), 'dd/MM/yyyy') +' - '+FORMAT (Convert(date,bb.Campaign_End_Date), 'dd/MM/yyyy') as Campaign_Period
,bb.Commission_Type
--,spv.Revised_No
from Buying_Brief bb
inner join Buying_Brief_Market bbm
	on bbm.Buying_Brief_ID = bb.Buying_Brief_ID
inner join client c
	on c.Client_ID = bb.Client_ID
inner join product p
	on p.Product_ID = bb.Product_ID 
inner join Creative_Agency ca
	on ca.Creative_Agency_ID = c.Creative_Agency_ID
inner join Agency a
	on a.Agency_ID = c.Agency_ID
inner join Media_Type mt
	on bb.Media_Type = mt.Media_Type
--inner join spot_plan_version spv
    --on spv.Buying_Brief_ID = bb.Buying_Brief_ID and spv.version = '" + strVersion + @"'
where  bb.Buying_Brief_ID = @Buying_Brief_ID 

SELECT bbmt.Buying_Brief_id,
Material = Stuff(
(select distinct ','+ Material_Key +': '+ Short_Name
from Material m inner join Buying_Brief_Market_Material bmt
on m.Material_ID = bmt.Material_ID
where bmt.Buying_Brief_ID = bbmt.Buying_Brief_ID
for xml path('')),1,1,'')
FROM Buying_Brief_Market_Material bbmt
where bbmt.Buying_Brief_ID = @Buying_Brief_ID
GROUP BY bbmt.Buying_Brief_id

select amt.Adept_Media_Type_Name, 
--sum(sp.Cost_BuyType * sp.Spots) as Media_Cost, 
--sum(sp.Net_Cost * sp.Agency_Fee) as Agency_Commission 
round(sum(sp.Cost_BuyType * sp.Spots),2) as Media_Cost, 
round(sum(round(convert(decimal(18,4),sp.Net_Cost) * convert(decimal(18,4),sp.Agency_Fee),2)),2) as Agency_Commission 
from (
    select s.*, 
    case when s.BuyTypeName = 'By Client' then 0 else s.Net_Cost end as Cost_BuyType 
    from Spot_Plan as s 
    where s.Buying_Brief_ID = @Buying_Brief_ID 
    and s.Version = @Version 
    and s.Media_ID in (" + strMedia + @") 
    and s.Show_Date <= @ED 
    and s.Show_Date >= @SD
) as sp 
inner join Media_Sub_Type as mst 
on sp.Media_Sub_Type = mst.Media_Sub_Type 
inner join Media as m 
on sp.Media_ID = m.Media_ID 
inner join Adept_Media_Type as amt 
on m.Adept_Media_Type = amt.Adept_Media_Type 
group by m.Adept_Media_Type, 
amt.Adept_Media_Type_Name, 
sp.Agency_Fee 
having sum(sp.Cost_BuyType * sp.Spots) + sum(sp.Net_Cost * sp.Agency_Fee) <> 0 
order by amt.Adept_Media_Type_Name";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.SelectCommand.Parameters.Add("@SD", SqlDbType.VarChar).Value = startDate;
            sda.SelectCommand.Parameters.Add("@ED", SqlDbType.VarChar).Value = endDate;
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }

        public DataTable SelectSpotPlanForScheduleES_Details(string strBB, string strVersion, string strMedia, string startDate, string endDate, string buyType)
        {
            string strSQL = "";
            if (buyType == "AG")
                strSQL = @"select amt.Adept_Media_Type_Name + ' | ' + mst.Short_Name as Media_Sub_Type, 
                case when isnull(sp.Program, '') = '' then m.Short_Name else m.Short_Name + ' - ' + sp.Program end as Description, 
                format(convert(date, sp.Show_Date), 'dd/MM/yyyy') as Schedule, 
                sp.BuyTypeName as Buy_Type, 
                sp.Spots as Quantity, 
                --sp.Rate as Total_Cost,
                round(sp.Rate,2) as Total_Cost, 
                sp.Discount as Disc, 
                '' as Col_H, 
                '' as Col_I, 
                --sp.Net_Cost as Net_Cost, 
                round(sp.Net_Cost,2) as Net_Cost, 
                sp.Agency_Fee * 100.00 as Agency_Commission_Percentage, 
                --sp.Net_Cost * sp.Agency_Fee as Agency_Commission 
                round(convert(decimal(18,4),sp.Net_Cost) * convert(decimal(18,4),sp.Agency_Fee),4) as Agency_Commission 
                from Spot_Plan as sp 
                inner join Media as m 
                on sp.Media_ID = m.Media_ID 
                inner join Media_Vendor as mv 
                on sp.Media_Vendor_ID = mv.Media_Vendor_ID 
                inner join Media_Sub_Type as mst 
                on sp.Media_Sub_Type = mst.Media_Sub_Type 
                inner join Adept_Media_Type as amt 
                on m.Adept_Media_Type = amt.Adept_Media_Type 
                left join Material as mt 
                on sp.Material_ID = mt.Material_ID 
                where sp.Buying_Brief_ID = @Buying_Brief_ID 
                and sp.Version = @Version 
                and sp.Media_ID in (" + strMedia + @") 
                and sp.Show_Date <= @ED 
                and sp.Show_Date >= @SD 
                order by amt.Adept_Media_Type_Name, 
                mst.Short_Name, 
                sp.Show_Date, 
                sp.Media_ID, 
                m.Short_Name, 
                sp.Start_Time, 
                sp.Program, 
                sp.Material_Key";
            else
                strSQL = @"select case when isnull(sp.Program, '') = '' then m.Short_Name else m.Short_Name + ' - ' + sp.Program end as Description, 
                mv.Short_Name as Vendor_Name, 
                format(convert(date, sp.Show_Date), 'dd/MM/yyyy') as Schedule, 
                sp.BuyTypeName as Buy_Type, 
                sp.Spots as Quantity, 
                --sp.Rate as Total_Cost,
                round(sp.Rate,2) as Total_Cost, 
                sp.Discount as Disc, 
                '' as Col_H, 
                '' as Col_I, 
                --sp.Net_Cost as Net_Cost
                round(sp.Net_Cost,2) as Net_Cost 
                from Spot_Plan as sp 
                inner join Media as m 
                on sp.Media_ID = m.Media_ID 
                inner join Media_Vendor as mv 
                on sp.Media_Vendor_ID = mv.Media_Vendor_ID 
                inner join Media_Sub_Type as mst 
                on sp.Media_Sub_Type = mst.Media_Sub_Type 
                inner join Adept_Media_Type as amt 
                on m.Adept_Media_Type = amt.Adept_Media_Type 
                left join Material as mt 
                on sp.Material_ID = mt.Material_ID 
                where sp.Buying_Brief_ID = @Buying_Brief_ID 
                and sp.Version = @Version 
                and sp.Media_ID in (" + strMedia + @") 
                and sp.Show_Date <= @ED 
                and sp.Show_Date >= @SD 
                and sp.BuyTypeID = @BuyType 
                order by amt.Adept_Media_Type_Name, 
                mst.Short_Name, 
                sp.Show_Date, 
                sp.Media_ID, 
                m.Short_Name, 
                sp.Start_Time, 
                sp.Program, 
                sp.Material_Key";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.SelectCommand.Parameters.Add("@SD", SqlDbType.VarChar).Value = startDate;
            sda.SelectCommand.Parameters.Add("@ED", SqlDbType.VarChar).Value = endDate;
            sda.SelectCommand.Parameters.Add("@BuyType", SqlDbType.VarChar).Value = buyType;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectSpotPlanForScheduleLSES_Details(string strBB, string strVersion, string strMedia, string startDate, string endDate)
        {
            string strSQL = @"select amt.Adept_Media_Type_Name + ' | ' + mst.Short_Name as Media_Sub_Type, 
            case when isnull(sp.Program, '') = '' then m.Short_Name else m.Short_Name + ' - ' + sp.Program end as Description, 
            format(convert(date, sp.Show_Date), 'dd/MM/yyyy') as Schedule, 
            '' as Col_D, 
            sp.BuyTypeName as Buy_Type, 
            '' as Col_F, 
            '' as Col_G, 
            '' as Col_H, 
            sp.Net_Cost as Net_Cost, 
            sp.Spots as Quantity 
            from Spot_Plan as sp 
            inner join Media as m 
            on sp.Media_ID = m.Media_ID 
            inner join Media_Vendor as mv 
            on sp.Media_Vendor_ID = mv.Media_Vendor_ID 
            inner join Media_Sub_Type as mst 
            on sp.Media_Sub_Type = mst.Media_Sub_Type 
            inner join Adept_Media_Type as amt 
            on m.Adept_Media_Type = amt.Adept_Media_Type 
            left join Material as mt 
            on sp.Material_ID = mt.Material_ID 
            where sp.Buying_Brief_ID = @Buying_Brief_ID 
            and sp.Version = @Version 
            and sp.Media_ID in (" + strMedia + @") 
            and sp.Show_Date <= @ED 
            and sp.Show_Date >= @SD 
            order by amt.Adept_Media_Type_Name, 
            mst.Short_Name, 
            sp.Show_Date, 
            sp.Media_ID, 
            m.Short_Name, 
            sp.Start_Time, 
            sp.Program, 
            sp.Material_Key";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.SelectCommand.Parameters.Add("@SD", SqlDbType.VarChar).Value = startDate;
            sda.SelectCommand.Parameters.Add("@ED", SqlDbType.VarChar).Value = endDate;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectVendorPO(string strBB, string strVersion)
        {
            string strSQL = @"select sp.Media_Vendor_ID as Vendor_ID, 
            mv.Short_Name as Vendor_Name, 
            isnull(mv.POBreakByMediaFlag, 'False') as PO_Media 
            from Spot_Plan as sp 
            left outer join Media_Vendor as mv 
            on sp.Media_Vendor_ID = mv.Media_Vendor_ID 
            where sp.Buying_Brief_ID = @Buying_Brief_ID 
            and sp.Version = @Version 
            and (
                sp.BuyTypeID is null 
                or sp.BuyTypeID = 'AG'
            ) 
            group by sp.Media_Vendor_ID, 
            mv.Short_Name, 
            mv.POBreakByMediaFlag";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataSet SelectSpotPlanForPOHeader(string strBB, string strVersion)
        {
            string strSQL = @"
select distinct
a.Icon_Path
,c.Short_Name Client_Name
,p.Short_Name Product_Name
,bb.Description Campaign_Name
,bb.Buying_Brief_ID
,bbm.Comment_Vendor Remark
,format(convert(date, bb.Campaign_Start_Date), 'dd/MM/yyyy') + ' - ' + format(convert(date, bb.Campaign_End_Date), 'dd/MM/yyyy') as Campaign_Period
from Buying_Brief bb
inner join Buying_Brief_Market bbm
	on bbm.Buying_Brief_ID = bb.Buying_Brief_ID
inner join client c
	on c.Client_ID = bb.Client_ID
inner join product p
	on p.Product_ID = bb.Product_ID 
inner join agency a
	on a.Agency_ID = c.Agency_ID
inner join spot_plan sp
	on bb.Buying_Brief_ID = sp.Buying_Brief_ID
where  bb.Buying_Brief_ID = @Buying_Brief_ID AND sp.[Version] = @Version

SELECT bbmt.Buying_Brief_id,
Material_name = Stuff(
(select distinct ', '+ Material_Key +': '+ Short_Name
from Material m inner join Buying_Brief_Market_Material bmt
on m.Material_ID = bmt.Material_ID
where bmt.Buying_Brief_ID = bbmt.Buying_Brief_ID
for xml path('')),1,2,'')
FROM Buying_Brief_Market_Material bbmt
where bbmt.Buying_Brief_ID = @Buying_Brief_ID
GROUP BY bbmt.Buying_Brief_id

select TOP 1 format(getdate(),'yyyyMM')+format(Order_No,'00000') OrderId from Booking_Order where Order_Date = format(getdate(),'yyyyMM')
update Booking_Order set Order_No = Order_No + 1 where Order_Date = format(getdate(),'yyyyMM')
";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }

        public DataTable SelectPOMediaCategory(string strBB, string strVendor)
        {
            string strSQL = @"select case Media.Media_Category when '' then 'NonCat' else Media.Media_Category end as Media_Category_ID, 
            isnull(Media_Category.Short_Name, '') as Media_Category_Name, 
            Media.Media_ID 
            from Spot_Plan 
            inner join Media 
            on Spot_Plan.Media_ID = Media.Media_ID 
            left outer join Media_Category 
            on Media.Media_Category = Media_Category.Media_Category 
            where Spot_Plan.Buying_Brief_ID = @Buying_Brief_ID 
            and Spot_Plan.Media_Vendor_ID = @Vendor 
            group by Media.Media_Category, 
            Media_Category.Short_Name, 
            Media.Media_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Vendor", SqlDbType.VarChar).Value = strVendor;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectPOITMediaCategory(string strBB, string strVendor)
        {
            string strSQL = @"select case Media.Media_Category when '' then 'NonCat' else Media.Media_Category end as Media_Category_ID, 
            isnull(Media_Category.Short_Name, '') as Media_Category_Name 
            from Spot_Plan 
            inner join Media 
            on Spot_Plan.Media_ID = Media.Media_ID 
            left outer join Media_Category 
            on Media.Media_Category = Media_Category.Media_Category 
            where Spot_Plan.Buying_Brief_ID = @Buying_Brief_ID 
            and Spot_Plan.Media_Vendor_ID = @Vendor 
            group by Media.Media_Category, 
            Media_Category.Short_Name";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Vendor", SqlDbType.VarChar).Value = strVendor;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectSpotPlanForPOIDByVendor(string strBB, string strVersion, string strVendor)
        {
            string strSQL = @"select Booking_Order_ID 
            from spot_plan
            where Buying_Brief_ID = @Buying_Brief_ID 
            and Version = @Version 
            and Media_Vendor_ID = @Vendor";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.SelectCommand.Parameters.Add("@Vendor", SqlDbType.VarChar).Value = strVendor;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectSpotPlanForPOIDByMediaCategory(string strBB, string strVersion, string strVendor, string strMedia)
        {
            string strSQL = @"select Booking_Order_ID 
            from spot_plan
            where Buying_Brief_ID = @Buying_Brief_ID 
            and Version = @Version 
            and Media_Vendor_ID = @Vendor 
            and Media_ID = @Media";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.SelectCommand.Parameters.Add("@Vendor", SqlDbType.VarChar).Value = strVendor;
            sda.SelectCommand.Parameters.Add("@Media", SqlDbType.VarChar).Value = strMedia;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectSpotPlanForPODetailsByVendor(string strBB, string strVersion, string strVendor, string startDate, string endDate)
        {
            string strSQL = @"select amt.Adept_Media_Type_Name + ' | ' + mst.Short_Name as Media_Sub_Type, 
            case when isnull(sp.Program, '') = '' then m.Short_Name else m.Short_Name + ' - ' + sp.Program end as Description, 
            sp.Material_Key as Material, 
            format(convert(date, sp.Start_Date), 'dd/MM/yyyy') + ' - ' + format(convert(date, sp.End_Date), 'dd/MM/yyyy') as Schedule, 
            sp.Rating GRP, 
            sp.CPRP_Cost CPRP, 
            sp.Net_Cost 
            from Spot_Plan as sp 
            inner join Media as m 
            on m.Media_ID = sp.Media_ID 
            inner join Media_Sub_Type as mst 
            on mst.Media_Sub_Type = sp.Media_Sub_Type 
            inner join Media_Vendor as mv 
            on sp.Media_Vendor_ID = mv.Media_Vendor_ID 
            inner join Adept_Media_Type as amt 
            on m.Adept_Media_Type = amt.Adept_Media_Type 
            where sp.Buying_Brief_ID = @Buying_Brief_ID 
            and sp.Version = @Version 
            and sp.Media_Vendor_ID = @Vendor 
            and sp.Start_Date <= @ED 
            and sp.End_Date >= @SD";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.SelectCommand.Parameters.Add("@Vendor", SqlDbType.VarChar).Value = strVendor;
            sda.SelectCommand.Parameters.Add("@SD", SqlDbType.VarChar).Value = startDate;
            sda.SelectCommand.Parameters.Add("@ED", SqlDbType.VarChar).Value = endDate;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectSpotPlanForPODetailsByMediaCategory(string strBB, string strVersion, string strVendor, string strMC, string startDate, string endDate)
        {
            string strSQL = @"select amt.Adept_Media_Type_Name + ' | ' + mst.Short_Name as Media_Sub_Type, 
            case when isnull(sp.Program, '') = '' then m.Short_Name else m.Short_Name + ' - ' + sp.Program end as Description, 
            sp.Material_Key as Material, 
            format(convert(date, sp.Start_Date), 'dd/MM/yyyy') + ' - ' + format(convert(date, sp.End_Date), 'dd/MM/yyyy') as Schedule, 
            sp.Rating GRP, 
            sp.CPRP_Cost CPRP, 
            sp.Net_Cost 
            from Spot_Plan as sp 
            inner join Media as m 
            on m.Media_ID = sp.Media_ID 
            inner join Media_Sub_Type as mst 
            on mst.Media_Sub_Type = sp.Media_Sub_Type 
            inner join Media_Vendor as mv 
            on sp.Media_Vendor_ID = mv.Media_Vendor_ID 
            inner join Adept_Media_Type as amt 
            on m.Adept_Media_Type = amt.Adept_Media_Type 
            where sp.Buying_Brief_ID = @Buying_Brief_ID 
            and sp.Version = @Version 
            and sp.Media_Vendor_ID = @Vendor 
            and m.Media_Category = @MediaCategory 
            and sp.Start_Date <= @ED 
            and sp.End_Date >= @SD";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.SelectCommand.Parameters.Add("@Vendor", SqlDbType.VarChar).Value = strVendor;
            sda.SelectCommand.Parameters.Add("@MediaCategory", SqlDbType.VarChar).Value = strMC.Replace("NonCat", "");
            sda.SelectCommand.Parameters.Add("@SD", SqlDbType.VarChar).Value = startDate;
            sda.SelectCommand.Parameters.Add("@ED", SqlDbType.VarChar).Value = endDate;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectSpotPlanForPOITDetailsByVendor(string strBB, string strVersion, string strVendor, string startDate, string endDate)
        {
            string strSQL = @"select amt.Adept_Media_Type_Name + ' | ' + mst.Short_Name as Media_Sub_Type, 
            case when isnull(sp.Program, '') = '' then m.Short_Name else m.Short_Name + ' - ' + sp.Program end as Description, 
            sp.Material_Key as Material, 
            format(convert(date, sp.Start_Date), 'dd/MM/yyyy') + ' - ' + format(convert(date, sp.End_Date), 'dd/MM/yyyy') as Schedule, 
            sp.Deadline_Terminate as Buy_Type, 
            sp.Unit, 
            sp.Net_Cost 
            from Spot_Plan as sp 
            inner join Media as m 
            on m.Media_ID = sp.Media_ID 
            inner join Media_Sub_Type as mst 
            on mst.Media_Sub_Type = sp.Media_Sub_Type 
            inner join Media_Vendor as mv 
            on sp.Media_Vendor_ID = mv.Media_Vendor_ID 
            inner join Adept_Media_Type as amt 
            on m.Adept_Media_Type = amt.Adept_Media_Type 
            where sp.Buying_Brief_ID = @Buying_Brief_ID 
            and sp.Version = @Version 
            and sp.Media_Vendor_ID = @Vendor 
            and sp.Start_Date <= @ED 
            and sp.End_Date >= @SD 
            and sp.BuyTypeID <> 'CL'";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.SelectCommand.Parameters.Add("@Vendor", SqlDbType.VarChar).Value = strVendor;
            sda.SelectCommand.Parameters.Add("@SD", SqlDbType.VarChar).Value = startDate;
            sda.SelectCommand.Parameters.Add("@ED", SqlDbType.VarChar).Value = endDate;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectSpotPlanForPOODDetailsByVendor(string strBB, string strVersion, string strVendor, string startDate, string endDate)
        {
            string strSQL = @"select amt.Adept_Media_Type_Name + ' | ' + mst.Short_Name as Media_Sub_Type, 
            sp.province,
            case when isnull(sp.Program, '') = '' then m.Short_Name else m.Short_Name + ' - ' + sp.Program end as Description,
            sp.State,
            sp.SizeHW,
            sp.Unit, 
            format(convert(date, sp.Start_Date), 'dd/MM/yyyy') + ' - ' + format(convert(date, sp.End_Date), 'dd/MM/yyyy') as Schedule, 
            --DATEDIFF(d,Convert(date,sp.Start_Date),Convert(date,sp.End_Date)) DifDays,
            sp.Net_Cost,
            sp.Remark
            from Spot_Plan as sp 
            inner join Media as m 
            on m.Media_ID = sp.Media_ID 
            inner join Media_Sub_Type as mst 
            on mst.Media_Sub_Type = sp.Media_Sub_Type 
            inner join Media_Vendor as mv 
            on sp.Media_Vendor_ID = mv.Media_Vendor_ID 
            inner join Adept_Media_Type as amt 
            on m.Adept_Media_Type = amt.Adept_Media_Type 
            where sp.Buying_Brief_ID = @Buying_Brief_ID 
            and sp.Version = @Version 
            and sp.Media_Vendor_ID = @Vendor 
            and sp.Start_Date <= @ED 
            and sp.End_Date >= @SD 
            and sp.BuyTypeID <> 'CL'";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.SelectCommand.Parameters.Add("@Vendor", SqlDbType.VarChar).Value = strVendor;
            sda.SelectCommand.Parameters.Add("@SD", SqlDbType.VarChar).Value = startDate;
            sda.SelectCommand.Parameters.Add("@ED", SqlDbType.VarChar).Value = endDate;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectSpotPlanForPOITDetailsByMediaCategory(string strBB, string strVersion, string strVendor, string strMC, string startDate, string endDate)
        {
            string strSQL = @"select amt.Adept_Media_Type_Name + ' | ' + mst.Short_Name as Media_Sub_Type, 
            case when isnull(sp.Program, '') = '' then m.Short_Name else m.Short_Name + ' - ' + sp.Program end as Description, 
            sp.Material_Key as Material, 
            format(convert(date, sp.Start_Date), 'dd/MM/yyyy') + ' - ' + format(convert(date, sp.End_Date), 'dd/MM/yyyy') as Schedule, 
            sp.Deadline_Terminate as Buy_Type, 
            sp.Unit, 
            sp.Net_Cost 
            from Spot_Plan as sp 
            inner join Media as m 
            on m.Media_ID = sp.Media_ID 
            inner join Media_Sub_Type as mst 
            on mst.Media_Sub_Type = sp.Media_Sub_Type 
            inner join Media_Vendor as mv 
            on sp.Media_Vendor_ID = mv.Media_Vendor_ID 
            inner join Adept_Media_Type as amt 
            on m.Adept_Media_Type = amt.Adept_Media_Type 
            where sp.Buying_Brief_ID = @Buying_Brief_ID 
            and sp.Version = @Version 
            and sp.Media_Vendor_ID = @Vendor 
            and isnull(m.Media_Category, '') = @MediaCategory 
            and sp.Start_Date <= @ED 
            and sp.End_Date >= @SD 
            and sp.BuyTypeID <> 'CL'";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.SelectCommand.Parameters.Add("@Vendor", SqlDbType.VarChar).Value = strVendor;
            sda.SelectCommand.Parameters.Add("@MediaCategory", SqlDbType.VarChar).Value = strMC.Replace("NonCat", "");
            sda.SelectCommand.Parameters.Add("@SD", SqlDbType.VarChar).Value = startDate;
            sda.SelectCommand.Parameters.Add("@ED", SqlDbType.VarChar).Value = endDate;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectSpotPlanForPOESDetailsByVendor(string strBB, string strVersion, string strVendor, string startDate, string endDate)
        {
            string strSQL = @"select amt.Adept_Media_Type_Name + ' | ' + mst.Short_Name as Media_Sub_Type, 
            case when isnull(sp.Program, '') = '' then m.Short_Name else m.Short_Name + ' - ' + sp.Program end as Description, 
            sp.Material_Key as Material, 
            format(convert(date, sp.Show_Date), 'dd/MM/yyyy') as Schedule, 
            sp.BuyTypeName as Buy_Type, 
            sp.Spots as Quantity, 
            sp.Net_Cost 
            from Spot_Plan as sp 
            inner join Media as m 
            on m.Media_ID = sp.Media_ID 
            inner join Media_Sub_Type as mst 
            on mst.Media_Sub_Type = sp.Media_Sub_Type 
            inner join Media_Vendor as mv 
            on sp.Media_Vendor_ID = mv.Media_Vendor_ID 
            inner join Adept_Media_Type as amt 
            on m.Adept_Media_Type = amt.Adept_Media_Type 
            where sp.Buying_Brief_ID = @Buying_Brief_ID 
            and sp.Version = @Version 
            and sp.Media_Vendor_ID = @Vendor 
            and sp.Show_Date <= @ED 
            and sp.Show_Date >= @SD 
            and sp.BuyTypeID <> 'CL'";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.SelectCommand.Parameters.Add("@Vendor", SqlDbType.VarChar).Value = strVendor;
            sda.SelectCommand.Parameters.Add("@SD", SqlDbType.VarChar).Value = startDate;
            sda.SelectCommand.Parameters.Add("@ED", SqlDbType.VarChar).Value = endDate;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectSpotPlanForPOESDetailsByMediaCategory(string strBB, string strVersion, string strVendor, string strMC, string startDate, string endDate)
        {
            string strSQL = @"select amt.Adept_Media_Type_Name + ' | ' + mst.Short_Name as Media_Sub_Type, 
            case when isnull(sp.Program, '') = '' then m.Short_Name else m.Short_Name + ' - ' + sp.Program end as Description, 
            sp.Material_Key as Material, 
            format(convert(date, sp.Show_Date), 'dd/MM/yyyy') as Schedule, 
            sp.BuyTypeName as Buy_Type, 
            sp.Spots as Quantity, 
            sp.Net_Cost 
            from Spot_Plan as sp 
            inner join Media as m 
            on m.Media_ID = sp.Media_ID 
            inner join Media_Sub_Type as mst 
            on mst.Media_Sub_Type = sp.Media_Sub_Type 
            inner join Media_Vendor as mv 
            on sp.Media_Vendor_ID = mv.Media_Vendor_ID 
            inner join Adept_Media_Type as amt 
            on m.Adept_Media_Type = amt.Adept_Media_Type 
            where sp.Buying_Brief_ID = @Buying_Brief_ID 
            and sp.Version = @Version 
            and sp.Media_Vendor_ID = @Vendor 
            and isnull(m.Media_Category, '') = @MediaCategory 
            and sp.Show_Date <= @ED 
            and sp.Show_Date >= @SD 
            and sp.BuyTypeID <> 'CL'";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.SelectCommand.Parameters.Add("@Vendor", SqlDbType.VarChar).Value = strVendor;
            sda.SelectCommand.Parameters.Add("@MediaCategory", SqlDbType.VarChar).Value = strMC.Replace("NonCat", "");
            sda.SelectCommand.Parameters.Add("@SD", SqlDbType.VarChar).Value = startDate;
            sda.SelectCommand.Parameters.Add("@ED", SqlDbType.VarChar).Value = endDate;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectSpotPlanEdit(string strBB, string strVersion)
        {
            string strSQL = @"select sp.*
,sp.Team Old_Team
,mt.Short_Name MediaTypeName
,mst.Short_Name MediaSubTypeName
,m.Short_Name MediaName 
,mv.Short_Name MediaVendorName
,FORMAT (Convert(date,Show_Date), 'dd/MM/yyyy') as ShowDate
,FORMAT (Convert(date,Start_Date), 'dd/MM/yyyy') as StartDate
,FORMAT (Convert(date,End_Date), 'dd/MM/yyyy') as EndDate
,Agency_Fee*100.00 AgencyFeePercent
,FORMAT (Convert(date,Edit_Date), 'dd/MM/yyyy') as EditDate
from spot_plan_edit sp
inner join media m
	on m.Media_ID =sp.Media_ID 
inner join Media_Vendor mv
	on mv.Media_Vendor_ID = sp.Media_Vendor_ID
inner join Media_Sub_Type mst
	on mst.Media_Sub_Type = sp.Media_Sub_Type
left join Media_Type mt
	on mt.Media_Type = sp.Media_Type
where  Buying_Brief_ID = @Buying_Brief_ID AND [Version] = @Version
ORDER BY mv.Short_Name,m.Short_Name,sp.Team,sp.Kind desc";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }
        public DataSet SelectSpotPlanEditForAdviceNote(string strBB, string strVersion, DateTime Print_DateTime, bool ShowX)
        {
            string strSQL = @"

select 
ca.Short_Name Creative_Agency_Name
,a.Icon_Path
,c.Short_Name Client_Name
,p.Short_Name Product_Name
,bb.Description Campaign_Name
,bb.Buying_Brief_ID
,FORMAT (Convert(date,bb.Campaign_Start_Date), 'dd/MM/yyyy') +' - '+FORMAT (Convert(date,bb.Campaign_End_Date), 'dd/MM/yyyy') as Campaign_Period
from Buying_Brief bb
inner join client c
	on c.Client_ID = bb.Client_ID
inner join product p
	on p.Product_ID = bb.Product_ID 
inner join Creative_Agency ca
	on ca.Creative_Agency_ID = c.Creative_Agency_ID
inner join agency a
	on a.Agency_ID = c.Agency_ID
where  bb.Buying_Brief_ID = @Buying_Brief_ID 

select sp.*
,m.Short_Name MediaName
,format(convert(date,Start_Date), 'dd/MM/yyyy') as StartDate
,format(convert(date,End_Date), 'dd/MM/yyyy') as EndDate
,Agency_Fee*100 AgencyFeePercent
,Agency_Fee*Net_Cost AgencyFee
from spot_plan_edit sp
inner join media m
on m.Media_ID =sp.Media_ID
where sp.Buying_Brief_ID = @Buying_Brief_ID
and Version = @Version
and sp.Print_DateTime = @Print_DateTime
and (@Show_X = 'True' or (@Show_X = 'False' and Team <> 'x'))
order by sp.Edit_Date
,sp.Edit_Time";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.SelectCommand.Parameters.Add("@Print_DateTime", SqlDbType.DateTime).Value = Print_DateTime;
            sda.SelectCommand.Parameters.Add("@Show_X", SqlDbType.VarChar).Value = ShowX;
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }

        public DataSet SelectSpotPlanEditForAdviceNoteIT(string strBB, string strVersion, DateTime Print_DateTime, bool ShowX)
        {
            string strSQL = @"

select 
ca.Short_Name Creative_Agency_Name
,a.Icon_Path
,c.Short_Name Client_Name
,p.Short_Name Product_Name
,bb.Description Campaign_Name
,bb.Buying_Brief_ID
,FORMAT (Convert(date,bb.Campaign_Start_Date), 'dd/MM/yyyy') +' - '+FORMAT (Convert(date,bb.Campaign_End_Date), 'dd/MM/yyyy') as Campaign_Period
from Buying_Brief bb
inner join client c
	on c.Client_ID = bb.Client_ID
inner join product p
	on p.Product_ID = bb.Product_ID 
inner join Creative_Agency ca
	on ca.Creative_Agency_ID = c.Creative_Agency_ID
inner join agency a
	on a.Agency_ID = c.Agency_ID
where  bb.Buying_Brief_ID = @Buying_Brief_ID 

select sp.*
,m.Short_Name MediaName
,format(convert(date,Start_Date), 'dd/MM/yyyy') as StartDate
,format(convert(date,End_Date), 'dd/MM/yyyy') as EndDate
,Agency_Fee*100 AgencyFeePercent
--,Agency_Fee*Net_Cost AgencyFee
,round(convert(decimal(18,4),Net_Cost) * convert(decimal(18,4),Agency_Fee),2) as AgencyFee 
from spot_plan_edit sp
inner join media m
on m.Media_ID =sp.Media_ID
where sp.Buying_Brief_ID = @Buying_Brief_ID
and Version = @Version
and sp.Print_DateTime = @Print_DateTime
and (@Show_X = 'True' or (@Show_X = 'False' and Team <> 'x'))
order by sp.Edit_Date
,sp.Edit_Time

select sp.*
,m.Short_Name MediaName
,format(convert(date,Start_Date), 'dd/MM/yyyy') as StartDate
,format(convert(date,End_Date), 'dd/MM/yyyy') as EndDate
,mv.Short_Name MediaVendorName
from spot_plan_edit sp
inner join media m
on m.Media_ID =sp.Media_ID
inner join Media_Vendor mv
on mv.Media_Vendor_ID = sp.Media_Vendor_ID
where sp.Buying_Brief_ID = @Buying_Brief_ID
and Version = @Version
and sp.Print_DateTime = @Print_DateTime
and (@Show_X = 'True' or (@Show_X = 'False' and Team <> 'x'))
and sp.BuyTypeID = 'CL'
order by sp.Edit_Date
,sp.Edit_Time";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.SelectCommand.Parameters.Add("@Print_DateTime", SqlDbType.DateTime).Value = Print_DateTime;
            sda.SelectCommand.Parameters.Add("@Show_X", SqlDbType.VarChar).Value = ShowX;
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }

        public DataSet SelectSpotPlanEditForAdviceNoteES(string strBB, string strVersion, DateTime Print_DateTime, bool ShowX)
        {
            string strSQL = @"

select 
ca.Short_Name Creative_Agency_Name
,a.Icon_Path
,a.Short_Name Agency_Name
,c.Short_Name Client_Name
,p.Short_Name Product_Name
,bb.Description Campaign_Name
,bb.Buying_Brief_ID
,FORMAT (Convert(date,bb.Campaign_Start_Date), 'dd/MM/yyyy') +' - '+FORMAT (Convert(date,bb.Campaign_End_Date), 'dd/MM/yyyy') as Campaign_Period
,bm.Material_Key
from Buying_Brief bb
left join buying_brief_market_material bm
	on bm.Buying_Brief_ID = bb.Buying_Brief_ID
left join material m
	on m.Material_ID = bm.Material_ID
inner join client c
	on c.Client_ID = bb.Client_ID
inner join product p
	on p.Product_ID = bb.Product_ID 
inner join Creative_Agency ca
	on ca.Creative_Agency_ID = c.Creative_Agency_ID
inner join agency a
	on a.Agency_ID = c.Agency_ID
where  bb.Buying_Brief_ID = @Buying_Brief_ID 

select sp.*
,m.Short_Name MediaName
,format(convert(date,Show_Date), 'dd/MM/yyyy') as ShowDate
,Agency_Fee*100 AgencyFeePercent
,round((convert(decimal(18,4),Net_Cost) * Spots) * convert(decimal(18,4),Agency_Fee),2) as AgencyFee 
from spot_plan_edit sp
inner join media m
on m.Media_ID =sp.Media_ID
where sp.Buying_Brief_ID = @Buying_Brief_ID
and Version = @Version
and sp.Print_DateTime = @Print_DateTime
and (@Show_X = 'True' or (@Show_X = 'False' and Team <> 'x'))
order by sp.Edit_Date
,sp.Edit_Time

select sp.*
,m.Short_Name MediaName
,format(convert(date,Show_Date), 'dd/MM/yyyy') as ShowDate
,mv.Short_Name MediaVendorName
from spot_plan_edit sp
inner join media m
on m.Media_ID =sp.Media_ID
inner join Media_Vendor mv
on mv.Media_Vendor_ID = sp.Media_Vendor_ID
where sp.Buying_Brief_ID = @Buying_Brief_ID
and Version = @Version
and sp.Print_DateTime = @Print_DateTime
and (@Show_X = 'True' or (@Show_X = 'False' and Team <> 'x'))
and sp.BuyTypeID = 'CL'
order by sp.Edit_Date
,sp.Edit_Time";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.SelectCommand.Parameters.Add("@Print_DateTime", SqlDbType.DateTime).Value = Print_DateTime;
            sda.SelectCommand.Parameters.Add("@Show_X", SqlDbType.VarChar).Value = ShowX;
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }
        public DataSet SelectSpotPlanEditForAdviceNoteOD(string strBB, string strVersion, DateTime Print_DateTime, bool ShowX)
        {
            string strSQL = @"

select 
ca.Short_Name Creative_Agency_Name
,a.Icon_Path
,c.Short_Name Client_Name
,p.Short_Name Product_Name
,bb.Description Campaign_Name
,bb.Buying_Brief_ID
,FORMAT (Convert(date,bb.Campaign_Start_Date), 'dd/MM/yyyy') +' - '+FORMAT (Convert(date,bb.Campaign_End_Date), 'dd/MM/yyyy') as Campaign_Period
from Buying_Brief bb
inner join client c
	on c.Client_ID = bb.Client_ID
inner join product p
	on p.Product_ID = bb.Product_ID 
inner join Creative_Agency ca
	on ca.Creative_Agency_ID = c.Creative_Agency_ID
inner join agency a
	on a.Agency_ID = c.Agency_ID
where  bb.Buying_Brief_ID = @Buying_Brief_ID 

select sp.*
,m.Short_Name MediaName
,sp.State CostType
,sp.BuyTypeName
,sp.SizeHW Size
,format(convert(date,Start_Date), 'dd/MM/yyyy') as StartDate
,format(convert(date,End_Date), 'dd/MM/yyyy') as EndDate
,Agency_Fee*100 AgencyFeePercent
--,Agency_Fee*Net_Cost AgencyFee
,round(convert(decimal(18,4),Net_Cost) * convert(decimal(18,4),Agency_Fee),2) as AgencyFee 
from spot_plan_edit sp
inner join media m
on m.Media_ID =sp.Media_ID
where sp.Buying_Brief_ID = @Buying_Brief_ID
and Version = @Version
and sp.Print_DateTime = @Print_DateTime
and (@Show_X = 'True' or (@Show_X = 'False' and Team <> 'x'))
order by sp.Edit_Date
,sp.Edit_Time

select sp.*
,m.Short_Name MediaName
,sp.State CostType
,sp.BuyTypeName
,sp.SizeHW Size
,format(convert(date,Start_Date), 'dd/MM/yyyy') as StartDate
,format(convert(date,End_Date), 'dd/MM/yyyy') as EndDate
,mv.Short_Name MediaVendorName
from spot_plan_edit sp
inner join media m
on m.Media_ID =sp.Media_ID
inner join Media_Vendor mv
on mv.Media_Vendor_ID = sp.Media_Vendor_ID
where sp.Buying_Brief_ID = @Buying_Brief_ID
and Version = @Version
and sp.Print_DateTime = @Print_DateTime
and (@Show_X = 'True' or (@Show_X = 'False' and Team <> 'x'))
and sp.BuyTypeID = 'CL'
order by sp.Edit_Date
,sp.Edit_Time";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 300;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
            sda.SelectCommand.Parameters.Add("@Print_DateTime", SqlDbType.DateTime).Value = Print_DateTime;
            sda.SelectCommand.Parameters.Add("@Show_X", SqlDbType.VarChar).Value = ShowX;
            DataSet ds = new DataSet();
            sda.Fill(ds);
            return ds;
        }

        public DataTable SelectSpotPlanMultipleSearch(string strBB, string type)
        {
            string strSQL = @"select stuff(
	            (
		            select ',' + " + type + @" 
		            from (
			            select distinct " + type + @" 
			            from Spot_Plan 
			            where Buying_Brief_ID = @Buying_Brief_ID
		            ) as SP 
		            order by " + type + @" 
		            for xml path('')
	            ), 1, 1, ','
            ) + ',' as Search";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public bool CheckPrintPOFooter(string pathParameter, string strVendorID)
        {
            bool print = false;
            string strCon = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathParameter + ";Persist Security Info=False";
            string strSQL = @"select * from SystemParameter where Parameter = 'PO_Footers_ExceptVendor' and Instr(1, PValue, @Vendor_ID) > 0";
            OleDbConnection con = new OleDbConnection(strCon);
            OleDbDataAdapter sda = new OleDbDataAdapter(strSQL, con);
            sda.SelectCommand.Parameters.AddWithValue("@Vendor_ID", strVendorID).DbType = DbType.String;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count == 0)
            {
                print = true;
            }
            else
            {
                print = false;
            }
            return print;
        }

        public DataTable SelectPOFooter(string pathParameter)
        {
            string strCon = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathParameter + ";Persist Security Info=False";
            string strSQL = @"select * from SystemParameter where Parameter = 'PO_Footers'";
            OleDbConnection con = new OleDbConnection(strCon);
            OleDbDataAdapter sda = new OleDbDataAdapter(strSQL, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            return dt;
        }

        public DataTable GetPONo(string strBBID)
        {
            string strSQL = @"Select * from Booking_Order where Order_Date = left(@BB,6)";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@BB", SqlDbType.VarChar).Value = strBBID;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public bool InsertPONo()
        {
            try
            {
                string strSQL = @"INSERT INTO Booking_Order (Order_No, Order_Date) VALUES (@Order_No, @Order_Date)";
                SqlCommand cmd = new SqlCommand(strSQL, m_connMinder);
                cmd.Parameters.Add("@Order_No", SqlDbType.VarChar).Value = 1;
                cmd.Parameters.Add("@Order_Date", SqlDbType.VarChar).Value = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2");
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                GMessage.MessageError(e.Message);
                return false;
            }
        }
        public bool UpdatePONo(string strBB)
        {
            try
            {
                string strSQL = @"Update Booking_Order set Order_No = Order_No + 1 where Order_Date = left(@BBID, 6)";
                SqlCommand cmd = new SqlCommand(strSQL, m_connMinder);
                cmd.Parameters.Add("@BBID", SqlDbType.VarChar).Value = strBB;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                GMessage.MessageError(e.Message);
                return false;
            }
        }

        public bool UpdatePONoInSpotplanByVendor(string strBB, string strVersion, string strVendor, string strOrderNo)
        {
            try
            {
                string strSQL = @"update Spot_Plan 
                set Booking_Order_ID = @OrderNo 
                where Buying_Brief_ID = @BBID 
                and Version = @Version 
                and Media_Vendor_ID = @Vendor 
                and Spots <> 0";
                SqlCommand cmd = new SqlCommand(strSQL, m_connMinder);
                cmd.Parameters.Add("@BBID", SqlDbType.VarChar).Value = strBB;
                cmd.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
                cmd.Parameters.Add("@Vendor", SqlDbType.VarChar).Value = strVendor;
                cmd.Parameters.Add("@OrderNo", SqlDbType.VarChar).Value = strOrderNo;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                GMessage.MessageError(e.Message);
                return false;
            }
        }

        public bool UpdatePONoInSpotplanByMediaCategory(string strBB, string strVersion, string strVendor, string strMedia, string strOrderNo)
        {
            try
            {
                string strSQL = @"update Spot_Plan 
                set Booking_Order_ID = @OrderNo 
                where Buying_Brief_ID = @BBID 
                and Version = @Version 
                and Media_Vendor_ID = @Vendor 
                and Media_ID = @Media 
                and Spots <> 0";
                SqlCommand cmd = new SqlCommand(strSQL, m_connMinder);
                cmd.Parameters.Add("@BBID", SqlDbType.VarChar).Value = strBB;
                cmd.Parameters.Add("@Version", SqlDbType.VarChar).Value = strVersion;
                cmd.Parameters.Add("@Vendor", SqlDbType.VarChar).Value = strVendor;
                cmd.Parameters.Add("@Media", SqlDbType.VarChar).Value = strMedia;
                cmd.Parameters.Add("@OrderNo", SqlDbType.VarChar).Value = strOrderNo;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                GMessage.MessageError(e.Message);
                return false;
            }
        }

        public DataTable GetPOHeaderSpecial(string strBBID, string strVendor)
        {
            string strSQL = @"Select b.Buying_Brief_ID,mv.Master_Group,c.* from buying_brief b
            inner join client c on b.Client_ID = c.Client_ID
            inner join Agency a on c.Agency_ID = a.Agency_ID
            inner join Spot_Plan s on b.Buying_Brief_ID = s.Buying_Brief_ID 
            inner join Media_Vendor mv on s.Media_Vendor_ID = mv.Media_Vendor_ID
            where b.Buying_brief_id = @BB and LEN(s.version) = 1 and s.Media_Vendor_ID = @Vendor";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@BB", SqlDbType.VarChar).Value = strBBID;
            sda.SelectCommand.Parameters.Add("@Vendor", SqlDbType.VarChar).Value = strVendor;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataTable GetPOHeader(string strAgency, string strCA)
        {
            string strSQL = @"Select top 1 * from Client where Agency_id = @Agency
            and booking_order_header like '%'+@CA+'%' and inactiveclient = 0";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Agency", SqlDbType.VarChar).Value = strAgency;
            sda.SelectCommand.Parameters.Add("@CA", SqlDbType.VarChar).Value = strCA;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataTable CheckFCRevenue(string strMedia, string strVendor, string strBBID)
        {
            string strSQL = @"FinecastRevenueValidation";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sda.SelectCommand.Parameters.Add("@Media_ID", SqlDbType.VarChar).Value = strMedia;
            sda.SelectCommand.Parameters.Add("@Media_Vendor_ID", SqlDbType.VarChar).Value = strVendor;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBBID;
            DataTable dt = new DataTable();
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectOffice(DataRow dr)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select *, 
            case when IsActive = 1 then 'Active' else 'Inactive' end as status_show 
            from Office 
            where 1 = 1";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            if (dr["OfficeCode"] != DBNull.Value)
            {
                strSQL += @" and Office_ID like '%'+@OfficeCode+'%'";
                sda.SelectCommand.Parameters.Add("@OfficeCode", SqlDbType.VarChar).Value = dr["OfficeCode"];
            }
            if (dr["Office_ID"] != DBNull.Value)
            {
                strSQL += @" and Office_ID = @Office_ID";
                sda.SelectCommand.Parameters.Add("@Office_ID", SqlDbType.VarChar).Value = dr["Office_ID"];
            }
            if (dr["Short_Name"] != DBNull.Value)
            {
                strSQL += @" and Short_Name like '%'+@Short_Name+'%'";
                sda.SelectCommand.Parameters.Add("@Short_Name", SqlDbType.VarChar).Value = dr["Short_Name"];
            }
            if (dr["Agency_ID"] != DBNull.Value)
            {
                strSQL += @" and Agency_ID = @Agency_ID";
                sda.SelectCommand.Parameters.Add("@Agency_ID", SqlDbType.VarChar).Value = dr["Agency_ID"];
            }
            if (dr["IsActive"] != DBNull.Value)
            {
                strSQL += @" and IsActive = @IsActive";
                sda.SelectCommand.Parameters.Add("@IsActive", SqlDbType.VarChar).Value = dr["IsActive"];
            }
            strSQL += @" order by Agency_ID, Short_Name";
            sda.SelectCommand.CommandText = strSQL;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectOffice(string strOffice_ID)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select O.*, 
            A.Short_Name as AgencyName 
            from Office as O 
            left outer join Agency as A 
            on O.Agency_ID = A.Agency_ID 
            where O.Office_ID = @Office_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Office_ID", SqlDbType.VarChar).Value = strOffice_ID;
            sda.Fill(dt);
            return dt;
        }

        public bool SelectEmailTail(string emailTail)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select * from EmailTail where EmailTail = @EmailTail";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@EmailTail", SqlDbType.VarChar).Value = emailTail;
            sda.Fill(dt);
            return dt.Rows.Count > 0 ? true : false;
        }

        public bool SelectOfficeIsUsing(string strOffice_ID)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select top 1 1 from Buying_Brief where Office_ID = @Office_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 0;
            sda.SelectCommand.Parameters.Add("@Office_ID", SqlDbType.VarChar).Value = strOffice_ID;
            sda.Fill(dt);
            return dt.Rows.Count > 0 ? true : false;
        }

        public DataTable SelectOfficeMasterFileReport()
        {
            DataTable dt = new DataTable();
            string strSQL = @"dbo.MasterFileOffice_Rpt";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sda.SelectCommand.CommandTimeout = 0;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectOptInGroup()
        {
            DataTable dt = new DataTable();
            string strSQL = @"select * 
            from (
                select * 
                from GroupProprietary 
                union 
                select 0, 'All', 'All', 'All'
            ) as rec
            order by GroupProprietaryId";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandType = CommandType.Text;
            sda.SelectCommand.CommandTimeout = 0;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectClient(DataRow dr)
        {
            DataTable dt = new DataTable();
            string strSQL = "";
            string strSelect1 = @"select * 
            from (
                select TBMain.Client_ID, 
                TBMain.Thai_Name, 
                TBMain.[Group], 
                TBMain.Agency_ID, 
                TBMain.Report_to_Agency, 
                TBMain.Office_ID, 
                case 
	            when TBMain.Opt_in_Signed = 'True' 
	            then 'Signed' 
	            else '' 
	            end as Opt_in_Show, 
                TBMain.Opt_in_Group, 
                case 
	            when TBMain.InactiveClient = '1' 
	            then 'Inactive' 
	            else 'Active' 
	            end as Status_Show, 
                TBMain.Audit_Right, 
                TBMain.Audit_Right_Period, 
                TBMain.EPD, 
                TBMain.EPD_Period, 
                TBMain.Media_Credit, 
                TBMain.Media_Credit_Period, 
                TBMain.Rebate, 
                TBMain.Rebate_Period, 
                TBMain.SAC, 
                TBMain.SAC_Period, 
                TBMain.Short_Name, 
                TBMain.Opt_in_Signed, 
                TBMain.InactiveClient 
                from (
                    select distinct c.*, 
                    stuff(
	                    (
		                    select ', ' + TBProprietary.GroupProprietaryName 
		                    from (
			                    select c.Client_ID, p.GroupProprietaryName 
			                    from Client as c 
			                    left join GroupProprietaryClientMapping as gp 
			                    on c.Client_ID = gp.Client_Id 
			                    left join GroupProprietary as p 
			                    on gp.GroupProprietaryId = p.GroupProprietaryId
		                    ) as TBProprietary 
		                    where TBProprietary.Client_Id = c.Client_ID 
		                    order by TBProprietary.GroupProprietaryName 
		                    for xml path('')
	                    ), 1, 2, ''
                    ) as Opt_in_Group, 
                    stuff(
	                    (
		                    select ', ' + TBAuditRight.Type 
		                    from (
			                    select c.Client_ID, ad.Type, ad.Start_Date, ad.End_Date 
			                    from Client as c 
			                    left join Client_Audit_Right as ad 
			                    on c.Client_ID = ad.Client_ID 
                                where 1 = 1";
            string strSelect2 = @") as TBAuditRight 
		            where TBAuditRight.Client_ID = c.Client_ID 
                    order by TBAuditRight.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as Audit_Right, 
            stuff(
	            (
		            select ', ' + 
		            convert(varchar(50), TBAuditRight.Start_Date, 6) + 
		            ' to ' + 
		            convert(varchar(50), TBAuditRight.End_Date, 6) 
		            from (
			            select c.Client_ID, ad.Type, ad.Start_Date, ad.End_Date 
			            from Client as c 
			            left join Client_Audit_Right as ad 
			            on c.Client_ID = ad.Client_ID 
                        where 1 = 1";
            string strSelect3 = @") as TBAuditRight 
		            where TBAuditRight.Client_ID = c.Client_ID 
                    order by TBAuditRight.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as Audit_Right_Period, 
            stuff(
	            (
		            select ', ' + TBMediaCredit.Type 
		            from (
			            select c.Client_ID, mc.Type, mc.Start_Date, mc.End_Date 
			            from Client as c 
			            left join Client_Media_Credit as mc 
			            on c.Client_ID = mc.Client_ID 
                        where 1 = 1";
            string strSelect4 = @") as TBMediaCredit 
		            where TBMediaCredit.Client_ID = c.Client_ID 
                    order by TBMediaCredit.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as Media_Credit, 
            stuff(
	            (
		            select ', ' + 
		            convert(varchar(50), TBMediaCredit.Start_Date, 6) + 
		            ' to ' + 
		            convert(varchar(50), TBMediaCredit.End_Date, 6) 
		            from (
			            select c.Client_ID, mc.Type, mc.Start_Date, mc.End_Date 
			            from Client as c 
			            left join Client_Media_Credit as mc 
			            on c.Client_ID = mc.Client_ID 
                        where 1 = 1";
            string strSelect5 = @") as TBMediaCredit 
		            where TBMediaCredit.Client_ID = c.Client_ID 
                    order by TBMediaCredit.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as Media_Credit_Period, 
            stuff(
	            (
		            select ', ' + TBRebate.Type 
		            from (
			            select c.Client_ID, rb.Type, rb.Start_Date, rb.End_Date 
			            from Client as c 
			            left join Client_Rebate as rb 
			            on c.Client_ID = rb.Client_ID 
                        where 1 = 1";
            string strSelect6 = @") as TBRebate 
		            where TBRebate.Client_ID = c.Client_ID 
                    order by TBRebate.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as Rebate, 
            stuff(
	            (
		            select ', ' + 
		            convert(varchar(50), TBRebate.Start_Date, 6) + 
		            ' to ' + 
		            convert(varchar(50), TBRebate.End_Date, 6) 
		            from (
			            select c.Client_ID, rb.Type, rb.Start_Date, rb.End_Date 
			            from Client as c 
			            left join Client_Rebate as rb 
			            on c.Client_ID = rb.Client_ID 
                        where 1 = 1";
            string strSelect7 = @") as TBRebate 
		            where TBRebate.Client_ID = c.Client_ID 
                    order by TBRebate.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as Rebate_Period, 
            stuff(
	            (
		            select ', ' + TBEPD.Type 
		            from (
			            select c.Client_ID, epd.Type, epd.Start_Date, epd.End_Date 
			            from Client as c 
			            left join Client_EPD as epd 
			            on c.Client_ID = epd.Client_ID 
                        where 1 = 1";
            string strSelect8 = @") as TBEPD 
		            where TBEPD.Client_ID = c.Client_ID 
                    order by TBEPD.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as EPD, 
            stuff(
	            (
		            select ', ' + 
		            convert(varchar(50), TBEPD.Start_Date, 6) + 
		            ' to ' + 
		            convert(varchar(50), TBEPD.End_Date, 6) 
		            from (
			            select c.Client_ID, epd.Type, epd.Start_Date, epd.End_Date 
			            from Client as c 
			            left join Client_EPD as epd 
			            on c.Client_ID = epd.Client_ID 
                        where 1 = 1";
            string strSelect9 = @") as TBEPD 
		            where TBEPD.Client_ID = c.Client_ID 
                    order by TBEPD.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as EPD_Period, 
            stuff(
	            (
		            select ', ' + TBSAC.Type 
		            from (
			            select c.Client_ID, sac.Type, sac.Start_Date, sac.End_Date 
			            from Client as c 
			            left join Client_SAC as sac 
			            on c.Client_ID = sac.Client_ID 
                        where 1 = 1";
            string strSelect10 = @") as TBSAC 
		            where TBSAC.Client_ID = c.Client_ID 
                    order by TBSAC.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as SAC, 
            stuff(
	            (
		            select ', ' + 
		            convert(varchar(50), TBSAC.Start_Date, 6) + 
		            ' to ' + 
		            convert(varchar(50), TBSAC.End_Date, 6) 
		            from (
			            select c.Client_ID, sac.Type, sac.Start_Date, sac.End_Date 
			            from Client as c 
			            left join Client_SAC as sac 
			            on c.Client_ID = sac.Client_ID 
                        where 1 = 1";
            string strSelect11 = @") as TBSAC 
		            where TBSAC.Client_ID = c.Client_ID 
                    order by TBSAC.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as SAC_Period 
            from Client as c 
            left join GroupProprietaryClientMapping as gp 
            on c.Client_ID = gp.Client_Id 
            left join GroupProprietary as p 
            on gp.GroupProprietaryId = p.GroupProprietaryId 
            left join Client_Audit_Right as ad 
            on c.Client_ID = ad.Client_ID 
            left join Client_Media_Credit as mc 
            on c.Client_ID = mc.Client_ID 
            left join Client_Rebate as rb 
            on c.Client_ID = rb.Client_ID 
            left join Client_EPD as epd 
            on c.Client_ID = epd.Client_ID 
            left join Client_SAC as sac 
            on c.Client_ID = sac.Client_ID 
            where 1 = 1";
            string strSelect12 = @") as TBMain
            ) as TB 
            where 1 = 1";
            string strWhere1 = "";
            string strWhere2 = "";
            string strWhere3 = "";
            string strWhere4 = "";
            string strWhere5 = "";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            if (dr["ClientCode"] != DBNull.Value)
            {
                strSelect12 += " and Client_ID like '%' + @ClientCode + '%'";
                sda.SelectCommand.Parameters.Add("@ClientCode", SqlDbType.VarChar).Value = dr["ClientCode"];
            }
            if (dr["Client_ID"] != DBNull.Value)
            {
                strSelect12 += " and Client_ID = @Client_ID";
                sda.SelectCommand.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = dr["Client_ID"];
            }
            if (dr["Short_Name"] != DBNull.Value)
            {
                strSelect12 += " and Short_Name like '%' + @Short_Name + '%'";
                sda.SelectCommand.Parameters.Add("@Short_Name", SqlDbType.VarChar).Value = dr["Short_Name"];
            }
            if (dr["MasterClient_ID"] != DBNull.Value)
            {
                strSelect12 += " and [Group] = @MasterClient_ID";
                sda.SelectCommand.Parameters.Add("@MasterClient_ID", SqlDbType.VarChar).Value = dr["MasterClient_ID"];
            }
            if (dr["Agency_ID"] != DBNull.Value)
            {
                strSelect12 += " and Agency_ID = @Agency_ID";
                sda.SelectCommand.Parameters.Add("@Agency_ID", SqlDbType.VarChar).Value = dr["Agency_ID"];
            }
            if (dr["Report_to_Agency"] != DBNull.Value)
            {
                strSelect12 += " and Report_to_Agency = @Report_to_Agency";
                sda.SelectCommand.Parameters.Add("@Report_to_Agency", SqlDbType.VarChar).Value = dr["Report_to_Agency"];
            }
            if (dr["Office_ID"] != DBNull.Value)
            {
                strSelect12 += " and Office_ID = @Office_ID";
                sda.SelectCommand.Parameters.Add("@Office_ID", SqlDbType.VarChar).Value = dr["Office_ID"];
            }
            if (dr["Opt_in_Signed"] != DBNull.Value)
            {
                strSelect12 += " and Opt_in_Signed = @Opt_in_Signed";
                sda.SelectCommand.Parameters.Add("@Opt_in_Signed", SqlDbType.VarChar).Value = dr["Opt_in_Signed"];
            }
            if (dr["InactiveClient"] != DBNull.Value)
            {
                strSelect12 += " and InactiveClient = @InactiveClient";
                sda.SelectCommand.Parameters.Add("@InactiveClient", SqlDbType.VarChar).Value = dr["InactiveClient"];
            }
            if (dr["GroupProprietaryId"] != DBNull.Value)
            {
                strSelect11 += " and p.GroupProprietaryId = @GroupProprietaryId";
                sda.SelectCommand.Parameters.Add("@GroupProprietaryId", SqlDbType.VarChar).Value = dr["GroupProprietaryId"];
            }
            if (dr["Condition"] == DBNull.Value)
            {
                strWhere1 = @" and ad.End_Date = (
	                select max(End_Date) 
	                from Client_Audit_Right 
	                where Client_ID = ad.Client_ID
                )";
                strWhere2 = @" and mc.End_Date = (
	                select max(End_Date) 
	                from Client_Media_Credit 
	                where Client_ID = mc.Client_ID
                )";
                strWhere3 = @" and rb.End_Date = (
	                select max(End_Date) 
	                from Client_Rebate 
	                where Client_ID = rb.Client_ID
                )";
                strWhere4 = @" and epd.End_Date = (
	                select max(End_Date) 
	                from Client_EPD 
	                where Client_ID = epd.Client_ID
                )";
                strWhere5 = @" and sac.End_Date = (
	                select max(End_Date) 
	                from Client_SAC 
	                where Client_ID = sac.Client_ID
                )";
            }
            else
            {
                strWhere1 = @" and (
                    (
                        ad.Start_Date >= @ADSD 
                        and ad.Start_Date < @ADED
                    ) 
                    or (
                        ad.End_Date > @ADSD 
                        and ad.End_Date <= @ADED
                    )
                )";
                strWhere2 = @" and (
                    (
                        mc.Start_Date >= @MCSD 
                        and mc.Start_Date < @MCED
                    ) 
                    or (
                        mc.End_Date > @MCSD 
                        and mc.End_Date <= @MCED
                    )
                )";
                strWhere3 = @" and (
                    (
                        rb.Start_Date >= @RBSD 
                        and rb.Start_Date < @RBED
                    ) 
                    or (
                        rb.End_Date > @RBSD 
                        and rb.End_Date <= @RBED
                    )
                )";
                strWhere4 = @" and (
                    (
                        epd.Start_Date >= @EPDSD 
                        and epd.Start_Date < @EPDED
                    ) 
                    or (
                        epd.End_Date > @EPDSD 
                        and epd.End_Date <= @EPDED
                    )
                )";
                strWhere5 = @" and (
                    (
                        sac.Start_Date >= @SACSD 
                        and sac.Start_Date < @SACED
                    ) 
                    or (
                        sac.End_Date > @SACSD 
                        and sac.End_Date <= @SACED
                    )
                )";
                sda.SelectCommand.Parameters.Add("@ADSD", SqlDbType.DateTime).Value = dr["StartDate"];
                sda.SelectCommand.Parameters.Add("@ADED", SqlDbType.DateTime).Value = dr["EndDate"];
                sda.SelectCommand.Parameters.Add("@MCSD", SqlDbType.DateTime).Value = dr["StartDate"];
                sda.SelectCommand.Parameters.Add("@MCED", SqlDbType.DateTime).Value = dr["EndDate"];
                sda.SelectCommand.Parameters.Add("@RBSD", SqlDbType.DateTime).Value = dr["StartDate"];
                sda.SelectCommand.Parameters.Add("@RBED", SqlDbType.DateTime).Value = dr["EndDate"];
                sda.SelectCommand.Parameters.Add("@EPDSD", SqlDbType.DateTime).Value = dr["StartDate"];
                sda.SelectCommand.Parameters.Add("@EPDED", SqlDbType.DateTime).Value = dr["EndDate"];
                sda.SelectCommand.Parameters.Add("@SACSD", SqlDbType.DateTime).Value = dr["StartDate"];
                sda.SelectCommand.Parameters.Add("@SACED", SqlDbType.DateTime).Value = dr["EndDate"];
                if (dr["Audit_Right"] != DBNull.Value)
                {
                    string strAnd = " and ad.Type = @Audit_Right";
                    strWhere1 += strAnd;
                    strSelect11 += strWhere1;
                    sda.SelectCommand.Parameters.Add("@Audit_Right", SqlDbType.VarChar).Value = dr["Audit_Right"];
                }
                if (dr["Media_Credit"] != DBNull.Value)
                {
                    string strAnd = " and mc.Type = @Media_Credit";
                    strWhere2 += strAnd;
                    strSelect11 += strWhere2;
                    sda.SelectCommand.Parameters.Add("@Media_Credit", SqlDbType.VarChar).Value = dr["Media_Credit"];
                }
                if (dr["Rebate"] != DBNull.Value)
                {
                    string strAnd = " and rb.Type = @Rebate";
                    strWhere3 += strAnd;
                    strSelect11 += strWhere3;
                    sda.SelectCommand.Parameters.Add("@Rebate", SqlDbType.VarChar).Value = dr["Rebate"];
                }
                if (dr["EPD"] != DBNull.Value)
                {
                    string strAnd = " and epd.Type = @EPD";
                    strWhere4 += strAnd;
                    strSelect11 += strWhere4;
                    sda.SelectCommand.Parameters.Add("@EPD", SqlDbType.VarChar).Value = dr["EPD"];
                }
                if (dr["SAC"] != DBNull.Value)
                {
                    string strAnd = " and sac.Type = @SAC";
                    strWhere5 += strAnd;
                    strSelect11 += strWhere5;
                    sda.SelectCommand.Parameters.Add("@SAC", SqlDbType.VarChar).Value = dr["SAC"];
                }
            }
            strSQL = strSelect1 + strWhere1 +
            strSelect2 + strWhere1 +
            strSelect3 + strWhere2 +
            strSelect4 + strWhere2 +
            strSelect5 + strWhere3 +
            strSelect6 + strWhere3 +
            strSelect7 + strWhere4 +
            strSelect8 + strWhere4 +
            strSelect9 + strWhere5 +
            strSelect10 + strWhere5 +
            strSelect11 + strSelect12;
            strSQL += @" order by Short_Name, 
            Client_ID";
            sda.SelectCommand.CommandText = strSQL;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectClient(string strClient_ID)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select C.*, 
            A.Short_Name as AgencyName, 
            RA.Short_Name as ReportToAgencyName, 
            CA.Short_Name as CreativeAgencyName, 
            O.Short_Name as OfficeName, 
            GPM.Short_Name as GPMName, 
            MC.Short_Name as MasterClientName 
            from Client as C 
            left outer join Agency as A 
            on C.Agency_ID = A.Agency_ID 
            left outer join Agency as RA 
            on C.Report_to_Agency = RA.Agency_ID 
            left outer join Creative_Agency as CA 
            on C.Creative_Agency_ID = CA.Creative_Agency_ID 
            left outer join Office as O 
            on C.Office_ID = O.Office_ID 
            left outer join Client as GPM 
            on C.GPM_CLIENT_CODE = GPM.Client_ID 
            left outer join Client as MC 
            on C.[Group] = MC.Client_ID 
            where C.Client_ID = @Client_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = strClient_ID;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectClientInMasterClient(string strClient_ID)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select Client_ID, 
            Short_Name 
            from Client 
            where [Group] = @Client_ID 
            and Client_ID <> @Client_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 0;
            sda.SelectCommand.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = strClient_ID;
            sda.Fill(dt);
            return dt;
        }
        public DataTable SelectMediaTypeInMasterMediaType(string strMediaType)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select Media_Type, 
            Short_Name 
            from Media_Type 
            where Master_Media_Type = @Media_Type 
            and Media_Type <> @Media_Type";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 0;
            sda.SelectCommand.Parameters.Add("@Media_Type", SqlDbType.VarChar).Value = strMediaType;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectBookingHeader()
        {
            DataTable dt = new DataTable();
            string strSQL = @"select distinct Booking_order_Header 
            from Client 
            order by Booking_order_Header";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectAddressByBookingHeader(string strBooking_Header)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select distinct Booking_order_Header, 
            Address, 
            Thai_Address2, 
            Thai_Address3, 
            Thai_Address4, 
            English_Address1, 
            English_Address2, 
            English_Address3 
            from Client 
            where Booking_order_Header = @Booking_order_Header";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Booking_order_Header", SqlDbType.VarChar).Value = strBooking_Header;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectProprietaryByClient(string strClient_ID)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select P.GroupProprietaryId, 
            P.GroupProprietaryName, 
            P.ContractType 
            from GroupProprietaryClientMapping C 
            inner join GroupProprietary P 
            on P.GroupProprietaryId = C.GroupProprietaryId 
            where C.Client_ID = @Client_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = strClient_ID;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectBrandByClient(string strClient_ID)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select Brand_ID as MasterCode, 
            Brand_Name as MasterName 
            from Brand 
            where Client_ID = @Client_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = strClient_ID;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectCategoryByClient(string strClient_ID)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select C.Category_ID as MasterCode, 
            C.Category_Name as MasterName
            ,c.*
            from Client_Category as CC 
            inner join Category as C 
            on CC.Category_ID = C.Category_ID 
            where CC.Client_ID = @Client_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = strClient_ID;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectClientRedGreenPeriod(string strClient_ID, string table)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select Client_ID, 
            Type, 
            convert(date, Start_Date) as Start_Date, 
            convert(date, End_Date) as End_Date 
            from " + table + @" 
            where Client_ID = @Client_ID 
            order by Start_Date";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = strClient_ID;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectProduct(string strProduct_ID)
        {
            DataTable dataTable = new DataTable();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(@"select p.*
  , a.Short_Name Agency_Name
  , c.Short_Name Client_Name
  , o.Short_Name Office_Name
  , b.Brand_Name
  , ca.Category_Name
  , c.GPM_CLIENT_CODE
  , c2.Short_Name GPM_Client_Name
  from Product as p
  left join Agency as a 
    on a.Agency_ID = p.Agency_ID
  left join Client as c
    on c.Client_ID = p.Client_ID
  left join Office o
    on o.Office_ID = p.Office_ID
  left join Brand b
    on b.Brand_ID = p.Brand_ID
    and b.Client_ID = p.Client_ID
  left join Category ca
    on ca.Category_ID = p.Category_ID
  left join client c2
    on c2.Client_ID = c.GPM_CLIENT_CODE
  where p.Product_ID = @Product_ID", this.m_connMinder);
  
            sqlDataAdapter.SelectCommand.Parameters.Add("@Product_ID", SqlDbType.VarChar).Value = (object)strProduct_ID;
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }
        public DataTable SelectProductMasterFileReport()
        {
            DataTable dataTable = new DataTable();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("select * from product", this.m_connMinder);
            sqlDataAdapter.SelectCommand.CommandTimeout = 0;
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }

        public bool SelectProductIsUsing(string strProduct_ID)
        {
            DataTable dataTable = new DataTable();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("select top 1 1 from Buying_Brief where Product_ID = @Product_ID", this.m_connMinder);
            sqlDataAdapter.SelectCommand.CommandTimeout = 0;
            sqlDataAdapter.SelectCommand.Parameters.Add("@Product_ID", SqlDbType.VarChar).Value = (object)strProduct_ID;
            sqlDataAdapter.Fill(dataTable);
            return dataTable.Rows.Count > 0;
        }
        public DataTable SelectAgencyFeeByProduct(string strProduct_ID)
        {
            DataTable dataTable = new DataTable();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(@"select
Product_Agency_Fee_ID
, Product_ID
,[Priority]
, Agency_Fee * 100.00 Agency_Fee
, Editable
, Agency_Fee_Set_Up_Name
, Agency_Fee_Set_Up_Column
, Media_Type_Group
, Media_Type
, Media_Sub_Type
, Other_Value
,[description]
  
  from product_agency_fee
  where Product_ID = @Product_ID
  order by [Priority], Media_Type_Group", this.m_connMinder);
  
            sqlDataAdapter.SelectCommand.Parameters.Add("@Product_ID", SqlDbType.VarChar).Value = (object)strProduct_ID;
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }
        public DataTable SelectAgencyFeeBySchedule(string strProduct_ID,string strMediaTypeCode, string strMediaSubTypeCode, string VendorCode, string CostType, string BuyingBriefID)
        {

            DataTable dt = new DataTable();
            string strSQL = @"dbo.CheckAgencyFeeSchedule";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sda.SelectCommand.CommandTimeout = 0;
            sda.SelectCommand.Parameters.Add("@Product_ID", SqlDbType.VarChar).Value = strProduct_ID;
            sda.SelectCommand.Parameters.Add("@Media_Type", SqlDbType.VarChar).Value = strMediaTypeCode;
            sda.SelectCommand.Parameters.Add("@Media_Sub_Type", SqlDbType.VarChar).Value = strMediaSubTypeCode;
            sda.SelectCommand.Parameters.Add("@Media_Vendor_ID", SqlDbType.VarChar).Value = VendorCode;
            sda.SelectCommand.Parameters.Add("@Cost_Type", SqlDbType.VarChar).Value = CostType;
            sda.SelectCommand.Parameters.Add("@BuyingBrief", SqlDbType.VarChar).Value = BuyingBriefID;

            sda.Fill(dt);
            return dt;

        }
        public int UpdateProduct(DataRow dr)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand(@"UPDATE [dbo].[Product]
    
       SET [Thai_Name] = @Thai_Name
          ,[English_Name] = @English_Name
          ,[Short_Name] = @Short_Name
          ,[Category_ID] = @Category_ID
          ,[Brand_ID] = @Brand_ID
          ,[Valid] = @Valid
          ,[Comment] = @Comment
          ,[User_ID] = @User_ID
          ,[Modify_Date] = @Modify_Date
    
       WHERE Product_ID = @Product_ID", this.m_connMinder);
    
                sqlCommand.Parameters.Add("@Product_ID", SqlDbType.VarChar).Value = dr["Product_ID"];
                sqlCommand.Parameters.Add("@Thai_Name", SqlDbType.VarChar).Value = dr["Thai_Name"];
                sqlCommand.Parameters.Add("@English_Name", SqlDbType.VarChar).Value = dr["English_Name"];
                sqlCommand.Parameters.Add("@Short_Name", SqlDbType.VarChar).Value = dr["Short_Name"];
                sqlCommand.Parameters.Add("@Category_ID", SqlDbType.VarChar).Value = dr["Category_ID"];
                sqlCommand.Parameters.Add("@Brand_ID", SqlDbType.VarChar).Value = dr["Brand_ID"];
                sqlCommand.Parameters.Add("@Valid", SqlDbType.VarChar).Value = dr["Valid"];
                sqlCommand.Parameters.Add("@Comment", SqlDbType.VarChar).Value = dr["Comment"];
                sqlCommand.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = dr["User_ID"];
                sqlCommand.Parameters.Add("@Modify_Date", SqlDbType.VarChar).Value = dr["Modify_Date"];
                return sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return -1;
            }
        }
        public int InsertProduct(DataRow dr)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO [dbo].[Product]
                    ([Agency_ID]
               ,[Office_ID]
               ,[Client_ID]
               ,[Product_ID]
               ,[Thai_Name]
               ,[English_Name]
               ,[Short_Name]
               ,[Category_ID]
               ,[Brand_ID]
               ,[Valid]
               ,[Comment]
               ,[User_ID]
               ,[Modify_Date]
               ,[GPM_PRODUCT_CODE]
               ,[GPM_PRODUCT_CODE_TMP]
               ,[Product_Referrence_ID])
                  VALUES
                (
@Agency_ID
, @Office_ID
, @Client_ID
, @Product_ID
, @Thai_Name
, @English_Name
, @Short_Name
, @Category_ID
, @Brand_ID
, @Valid
, @Comment
, @User_ID
, @Modify_Date
, @GPM_PRODUCT_CODE
, @GPM_PRODUCT_CODE_TMP
, @Product_Referrence_ID)", this.m_connMinder);
  
            sqlCommand.Parameters.Add("@Agency_ID", SqlDbType.VarChar).Value = dr["Agency_ID"];
                sqlCommand.Parameters.Add("@Office_ID", SqlDbType.VarChar).Value = dr["Office_ID"];
                sqlCommand.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = dr["Client_ID"];
                sqlCommand.Parameters.Add("@Product_ID", SqlDbType.VarChar).Value = dr["Product_ID"];
                sqlCommand.Parameters.Add("@Thai_Name", SqlDbType.VarChar).Value = dr["Thai_Name"];
                sqlCommand.Parameters.Add("@English_Name", SqlDbType.VarChar).Value = dr["English_Name"];
                sqlCommand.Parameters.Add("@Short_Name", SqlDbType.VarChar).Value = dr["Short_Name"];
                sqlCommand.Parameters.Add("@Category_ID", SqlDbType.VarChar).Value = dr["Category_ID"];
                sqlCommand.Parameters.Add("@Brand_ID", SqlDbType.VarChar).Value = dr["Brand_ID"];
                sqlCommand.Parameters.Add("@Valid", SqlDbType.VarChar).Value = dr["Valid"];
                sqlCommand.Parameters.Add("@Comment", SqlDbType.VarChar).Value = dr["Comment"];
                sqlCommand.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = dr["User_ID"];
                sqlCommand.Parameters.Add("@Modify_Date", SqlDbType.VarChar).Value = dr["Modify_Date"];
                sqlCommand.Parameters.Add("@GPM_PRODUCT_CODE", SqlDbType.VarChar).Value = dr["GPM_PRODUCT_CODE"];
                sqlCommand.Parameters.Add("@GPM_PRODUCT_CODE_TMP", SqlDbType.VarChar).Value = dr["GPM_PRODUCT_CODE_TMP"];
                sqlCommand.Parameters.Add("@Product_Referrence_ID", SqlDbType.VarChar).Value = dr["Product_Referrence_ID"];
                return sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return -1;
            }
        }
        public DataTable SelectProduct(DataRow dr)
        {
            DataTable dataTable = new DataTable();
            string selectCommandText = @"select top 1000 p.*
  , a.Short_Name Agency_Name
   , c.Short_Name Client_Name
    , o.Short_Name Office_Name
     , b.Brand_Name
,ca.Category_Name
,c.GPM_CLIENT_CODE
,c2.Short_Name GPM_Client_Name
,case when p.Valid = 1 then 'Active' else 'Inactive' end as status_show
from Product as p
left join Agency as a
    on a.Agency_ID = p.Agency_ID
left join Client as c
    on c.Client_ID = p.Client_ID
left join Office o
    on o.Office_ID = p.Office_ID
left join Brand b
    on b.Brand_ID = p.Brand_ID
    and b.Client_ID = p.Client_ID
left join Category ca
    on ca.Category_ID = p.Category_ID
left join client c2
    on c2.Client_ID = c.GPM_CLIENT_CODE
where 1 = 1
";

          SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommandText, this.m_connMinder);
            if (dr["Product_ID"] != DBNull.Value)
            {
                selectCommandText += " and p.Product_ID like '%'+@Office_ID+'%'";
                sqlDataAdapter.SelectCommand.Parameters.Add("@Office_ID", SqlDbType.VarChar).Value = dr["Product_ID"];
            }
            if (dr["Product_Name"] != DBNull.Value)
            {
                selectCommandText += " and p.Short_Name like '%'+@Product_Name+'%'";
                sqlDataAdapter.SelectCommand.Parameters.Add("@Product_Name", SqlDbType.VarChar).Value = dr["Product_Name"];
            }
            if (dr["Agency_ID"] != DBNull.Value)
            {
                selectCommandText += " and p.Agency_ID = @Agency_ID";
                sqlDataAdapter.SelectCommand.Parameters.Add("@Agency_ID", SqlDbType.VarChar).Value = dr["Agency_ID"];
            }
            if (dr["Client_ID"] != DBNull.Value)
            {
                selectCommandText += " and p.Client_ID = @Client_ID";
                sqlDataAdapter.SelectCommand.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = dr["Client_ID"];
            }
            if (dr["Office_ID"] != DBNull.Value)
            {
                selectCommandText += " and p.Office_ID = @Office_ID";
                sqlDataAdapter.SelectCommand.Parameters.Add("@Office_ID", SqlDbType.VarChar).Value = dr["Office_ID"];
            }
            if (dr["IsActive"] != DBNull.Value)
            {
                selectCommandText += " and p.Valid = @IsActive";
                sqlDataAdapter.SelectCommand.Parameters.Add("@IsActive", SqlDbType.VarChar).Value = dr["IsActive"];
            }
            string str = selectCommandText + " order by p.Product_ID, p.Short_Name";
            sqlDataAdapter.SelectCommand.CommandText = str;
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }
        public void UpdateProductAgencyFee(DataRow dr)
        {
            try
            {
                string cmdText = @"update product_agency_fee
  set
  Priority = @Priority
  , Agency_Fee = @Agency_Fee
  , Editable = @Editable
  , Agency_Fee_Set_Up_Name = @Agency_Fee_Set_Up_Name
  , Agency_Fee_Set_Up_Column = @Agency_Fee_Set_Up_Column
  , Media_Type_Group = @Media_Type_Group
  , Media_Type = @Media_Type
  , Media_Sub_Type = @Media_Sub_Type
  , Other_Value = @Other_Value
  ,[description] = @description
  WHERE Product_Agency_Fee_ID = @Product_Agency_Fee_ID";
  
            SqlCommand sqlCommand = new SqlCommand(cmdText, this.m_connMinder);
                sqlCommand.CommandText = cmdText;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("@Product_Agency_Fee_ID", SqlDbType.VarChar).Value = dr["Product_Agency_Fee_ID"];
                sqlCommand.Parameters.Add("@Priority", SqlDbType.Int).Value = dr["Priority"];
                sqlCommand.Parameters.Add("@Agency_Fee", SqlDbType.Decimal).Value = (object)(Convert.ToDouble(dr["Agency_Fee"]) / 100.0);
                sqlCommand.Parameters.Add("@Editable", SqlDbType.Bit).Value = dr["Editable"];
                sqlCommand.Parameters.Add("@Agency_Fee_Set_Up_Name", SqlDbType.VarChar).Value = dr["Agency_Fee_Set_Up_Name"];
                sqlCommand.Parameters.Add("@Agency_Fee_Set_Up_Column", SqlDbType.VarChar).Value = dr["Agency_Fee_Set_Up_Column"];
                sqlCommand.Parameters.Add("@Media_Type_Group", SqlDbType.VarChar).Value = dr["Media_Type_Group"];
                sqlCommand.Parameters.Add("@Media_Type", SqlDbType.VarChar).Value = dr["Media_Type"];
                sqlCommand.Parameters.Add("@Media_Sub_Type", SqlDbType.VarChar).Value = dr["Media_Sub_Type"];
                sqlCommand.Parameters.Add("@Other_Value", SqlDbType.VarChar).Value = dr["Other_Value"];
                sqlCommand.Parameters.Add("@description", SqlDbType.VarChar).Value = dr["description"];
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }
        }

        public bool DeleteProduct(string strProductCode)
        {
            try
            {
                string strSQL = @"DELETE FROM Product_Agency_Fee WHERE Product_ID = @Product_ID
                DELETE FROM Product WHERE Product_ID = @Product_ID";
                SqlCommand com = new SqlCommand(strSQL, m_connMinder);
                com.Parameters.Add(new SqlParameter("@Product_ID", SqlDbType.VarChar)).Value = strProductCode;
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteProductAgencyFee(int product_agency_fee_id)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand("DELETE product_agency_fee WHERE product_agency_fee_id = @product_agency_fee_id", this.m_connMinder);
                sqlCommand.Parameters.Add("@product_agency_fee_id", SqlDbType.VarChar).Value = (object)product_agency_fee_id;
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }
        }
        public void InsertProductAgencyFee(DataRow dr, string Product_ID)
        {
            try
            {
                string cmdText = @"INSERT INTO product_agency_fee
                    (Product_ID
    , Priority
    , Agency_Fee
    , Editable
    , Agency_Fee_Set_Up_Name
    , Agency_Fee_Set_Up_Column
    , Media_Type_Group
    , Media_Type
    , Media_Sub_Type
    , Other_Value
    ,[description])
  VALUES
  (@Product_ID
  , @Priority
  , @Agency_Fee
  , @Editable
  , @Agency_Fee_Set_Up_Name
  , @Agency_Fee_Set_Up_Column
  , @Media_Type_Group
  , @Media_Type
  , @Media_Sub_Type
  , @Other_Value
  , @description)";
  
            SqlCommand sqlCommand = new SqlCommand(cmdText, this.m_connMinder);
                sqlCommand.CommandText = cmdText;
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.Add("@Product_ID", SqlDbType.VarChar).Value = (object)Product_ID;
                sqlCommand.Parameters.Add("@Priority", SqlDbType.Int).Value = dr["Priority"];
                sqlCommand.Parameters.Add("@Agency_Fee", SqlDbType.Decimal).Value = (object)(Convert.ToDouble(dr["Agency_Fee"]) / 100.0);
                sqlCommand.Parameters.Add("@Editable", SqlDbType.Bit).Value = dr["Editable"];
                sqlCommand.Parameters.Add("@Agency_Fee_Set_Up_Name", SqlDbType.VarChar).Value = dr["Agency_Fee_Set_Up_Name"];
                sqlCommand.Parameters.Add("@Agency_Fee_Set_Up_Column", SqlDbType.VarChar).Value = dr["Agency_Fee_Set_Up_Column"];
                sqlCommand.Parameters.Add("@Media_Type_Group", SqlDbType.VarChar).Value = dr["Media_Type_Group"];
                sqlCommand.Parameters.Add("@Media_Type", SqlDbType.VarChar).Value = dr["Media_Type"];
                sqlCommand.Parameters.Add("@Media_Sub_Type", SqlDbType.VarChar).Value = dr["Media_Sub_Type"];
                sqlCommand.Parameters.Add("@Other_Value", SqlDbType.VarChar).Value = dr["Other_Value"];
                sqlCommand.Parameters.Add("@description", SqlDbType.VarChar).Value = dr["description"];
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }
        }

        public DataTable SelectAgencyFeeByClient(string strClient_ID)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select
	Client_Agency_Fee_ID
	,Client_ID
	,[Priority]
	,Agency_Fee*100.00 Agency_Fee
	,Editable
	,Agency_Fee_Set_Up_Name
	,Agency_Fee_Set_Up_Column
	,Media_Type_Group
	,Media_Type
	,Media_Sub_Type
	,Other_Value
	,[description] 

from client_agency_fee 
where Client_ID = @Client_ID 
order by [Priority],Media_Type_Group";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = strClient_ID;
            sda.Fill(dt);
            return dt;
        }

        public bool SelectClientIsUsing(string strClient_ID)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select top 1 1 from Buying_Brief where Client_ID = @Client_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 0;
            sda.SelectCommand.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = strClient_ID;
            sda.Fill(dt);
            return dt.Rows.Count > 0 ? true : false;
        }

        public bool SelectMasterClientIsUsing(string strClient_ID)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select * 
            from Client 
            where [Group] = @Client_ID 
            and Client_ID <> @Client_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 0;
            sda.SelectCommand.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = strClient_ID;
            sda.Fill(dt);
            return dt.Rows.Count > 0 ? true : false;
        }
        public bool SelectMasterMediaTypeIsUsing(string strMediaTypeCode)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select 1 
            from Media_Type 
            where Master_Media_Type = @Media_Type 
            and Media_Type <> @Media_Type";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 0;
            sda.SelectCommand.Parameters.Add("@Media_Type", SqlDbType.VarChar).Value = strMediaTypeCode;
            sda.Fill(dt);
            return dt.Rows.Count > 0 ? true : false;
        }
        public bool SelectBrandIsUsing(string strBrand_ID, string strClient_ID)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select Brand.Client_ID, 
            Brand.Brand_ID, 
            Product.Product_ID, 
            Buying_Brief.Buying_Brief_ID 
            from Buying_Brief 
            left join Product 
            on Buying_Brief.Product_ID = Product.Product_ID 
            left join Brand 
            on Product.Brand_ID = Brand.Brand_ID 
            and Product.Client_ID = Brand.Client_ID 
            where Brand.Brand_ID = @Brand_ID 
            and Brand.Client_ID = @Client_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 0;
            sda.SelectCommand.Parameters.Add("@Brand_ID", SqlDbType.VarChar).Value = strBrand_ID;
            sda.SelectCommand.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = strClient_ID;
            sda.Fill(dt);
            return dt.Rows.Count > 0 ? true : false;
        }

        public bool SelectCategoryIsUsing(string strCategory_ID, string strClient_ID)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select Client_Category.Client_ID, 
            Category.Category_ID, 
            Product.Product_ID, 
            Buying_Brief.Buying_Brief_ID 
            from Buying_Brief 
            left join Product 
            on Buying_Brief.Product_ID = Product.Product_ID 
            left join Category 
            on Product.Category_ID = Category.Category_ID 
            left join Client_Category 
            on Category.Category_ID = Client_Category.Category_ID 
            and Product.Client_ID = Client_Category.Client_ID 
            where Category.Category_ID = @Category_ID 
            and Client_Category.Client_ID = @Client_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 0;
            sda.SelectCommand.Parameters.Add("@Category_ID", SqlDbType.VarChar).Value = strCategory_ID;
            sda.SelectCommand.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = strClient_ID;
            sda.Fill(dt);
            return dt.Rows.Count > 0 ? true : false;
        }

        public DataTable SelectClientMasterFileReport(string username)
        {
            DataTable dt = new DataTable();
            string strSQL = @"dbo.MasterFileClient_Rpt";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sda.SelectCommand.CommandTimeout = 0;
            sda.SelectCommand.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = username;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectClientMasterFileReport()
        {
            DataTable dt = new DataTable();
            string strSQL = @"dbo.MasterFileClient_Rpt_Reporting";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sda.SelectCommand.CommandTimeout = 0;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectClientScreenReport(DataRow dr)
        {
            DataTable dt = new DataTable();
            string strSQL = "";
            string strSelect1 = @"select Client_ID as [Client Code], 
			Thai_Name as [Client Name], 
			[Group] as [Master Client], 
			Agency_ID as Agency, 
			Report_to_Agency as [Report to Agency], 
			Office_ID as Office, 
            Opt_in_Show as [Opt-In], 
            Opt_in_Group as [Opt-In Group], 
            Status_Show as Status, ";
            string strSelect2 = @"Audit_Right as [Audit Right], 
            EPD as [EPD (Pass Back)], 
            Media_Credit as [Media Credit (Pass Back)], 
            Rebate as [Rebate (Pass Back)], 
            SAC as [SAC (Pass Back)] ";
            string strSelect3 = @"Audit_Right as [Audit Right], 
            Audit_Right_Period as [Audit Right Period], 
            EPD as [EPD (Pass Back)], 
            EPD_Period as [EPD (Pass Back) Period], 
            Media_Credit as [Media Credit (Pass Back)], 
            Media_Credit_Period as [Media Credit (Pass Back) Period], 
            Rebate as [Rebate (Pass Back)], 
            Rebate_Period as [Rebate (Pass Back) Period], 
            SAC as [SAC (Pass Back)], 
            SAC_Period as [SAC (Pass Back) Period] ";
            string strSelect4 = @"from (
            select TBMain.Client_ID, 
            TBMain.Thai_Name, 
            TBMain.[Group], 
            TBMain.Agency_ID, 
            TBMain.Report_to_Agency, 
            TBMain.Office_ID, 
            case 
	        when TBMain.Opt_in_Signed = 'True' 
	        then 'Signed' 
	        else '' 
	        end as Opt_in_Show, 
            TBMain.Opt_in_Group, 
            case 
	        when TBMain.InactiveClient = '1' 
	        then 'Inactive' 
	        else 'Active' 
	        end as Status_Show, 
            TBMain.Audit_Right, 
            TBMain.Audit_Right_Period, 
            TBMain.EPD, 
            TBMain.EPD_Period, 
            TBMain.Media_Credit, 
            TBMain.Media_Credit_Period, 
            TBMain.Rebate, 
            TBMain.Rebate_Period, 
            TBMain.SAC, 
            TBMain.SAC_Period, 
            TBMain.Short_Name, 
            TBMain.Opt_in_Signed, 
            TBMain.InactiveClient 
            from (
                select distinct c.*, 
                stuff(
	                (
		                select ', ' + TBProprietary.GroupProprietaryName 
		                from (
			                select c.Client_ID, p.GroupProprietaryName 
			                from Client as c 
			                left join GroupProprietaryClientMapping as gp 
			                on c.Client_ID = gp.Client_Id 
			                left join GroupProprietary as p 
			                on gp.GroupProprietaryId = p.GroupProprietaryId
		                ) as TBProprietary 
		                where TBProprietary.Client_Id = c.Client_ID 
		                order by TBProprietary.GroupProprietaryName 
		                for xml path('')
	                ), 1, 2, ''
                ) as Opt_in_Group, 
                stuff(
	                (
		                select ', ' + TBAuditRight.Type 
		                from (
			                select c.Client_ID, ad.Type, ad.Start_Date, ad.End_Date 
			                from Client as c 
			                left join Client_Audit_Right as ad 
			                on c.Client_ID = ad.Client_ID 
                            where 1 = 1";
            string strSelect5 = @") as TBAuditRight 
		            where TBAuditRight.Client_ID = c.Client_ID 
                    order by TBAuditRight.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as Audit_Right, 
            stuff(
	            (
		            select ', ' + 
		            convert(varchar(50), TBAuditRight.Start_Date, 6) + 
		            ' to ' + 
		            convert(varchar(50), TBAuditRight.End_Date, 6) 
		            from (
			            select c.Client_ID, ad.Type, ad.Start_Date, ad.End_Date 
			            from Client as c 
			            left join Client_Audit_Right as ad 
			            on c.Client_ID = ad.Client_ID 
                        where 1 = 1";
            string strSelect6 = @") as TBAuditRight 
		            where TBAuditRight.Client_ID = c.Client_ID 
                    order by TBAuditRight.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as Audit_Right_Period, 
            stuff(
	            (
		            select ', ' + TBMediaCredit.Type 
		            from (
			            select c.Client_ID, mc.Type, mc.Start_Date, mc.End_Date 
			            from Client as c 
			            left join Client_Media_Credit as mc 
			            on c.Client_ID = mc.Client_ID 
                        where 1 = 1";
            string strSelect7 = @") as TBMediaCredit 
		            where TBMediaCredit.Client_ID = c.Client_ID 
                    order by TBMediaCredit.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as Media_Credit, 
            stuff(
	            (
		            select ', ' + 
		            convert(varchar(50), TBMediaCredit.Start_Date, 6) + 
		            ' to ' + 
		            convert(varchar(50), TBMediaCredit.End_Date, 6) 
		            from (
			            select c.Client_ID, mc.Type, mc.Start_Date, mc.End_Date 
			            from Client as c 
			            left join Client_Media_Credit as mc 
			            on c.Client_ID = mc.Client_ID 
                        where 1 = 1";
            string strSelect8 = @") as TBMediaCredit 
		            where TBMediaCredit.Client_ID = c.Client_ID 
                    order by TBMediaCredit.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as Media_Credit_Period, 
            stuff(
	            (
		            select ', ' + TBRebate.Type 
		            from (
			            select c.Client_ID, rb.Type, rb.Start_Date, rb.End_Date 
			            from Client as c 
			            left join Client_Rebate as rb 
			            on c.Client_ID = rb.Client_ID 
                        where 1 = 1";
            string strSelect9 = @") as TBRebate 
		            where TBRebate.Client_ID = c.Client_ID 
                    order by TBRebate.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as Rebate, 
            stuff(
	            (
		            select ', ' + 
		            convert(varchar(50), TBRebate.Start_Date, 6) + 
		            ' to ' + 
		            convert(varchar(50), TBRebate.End_Date, 6) 
		            from (
			            select c.Client_ID, rb.Type, rb.Start_Date, rb.End_Date 
			            from Client as c 
			            left join Client_Rebate as rb 
			            on c.Client_ID = rb.Client_ID 
                        where 1 = 1";
            string strSelect10 = @") as TBRebate 
		            where TBRebate.Client_ID = c.Client_ID 
                    order by TBRebate.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as Rebate_Period, 
            stuff(
	            (
		            select ', ' + TBEPD.Type 
		            from (
			            select c.Client_ID, epd.Type, epd.Start_Date, epd.End_Date 
			            from Client as c 
			            left join Client_EPD as epd 
			            on c.Client_ID = epd.Client_ID 
                        where 1 = 1";
            string strSelect11 = @") as TBEPD 
		            where TBEPD.Client_ID = c.Client_ID 
                    order by TBEPD.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as EPD, 
            stuff(
	            (
		            select ', ' + 
		            convert(varchar(50), TBEPD.Start_Date, 6) + 
		            ' to ' + 
		            convert(varchar(50), TBEPD.End_Date, 6) 
		            from (
			            select c.Client_ID, epd.Type, epd.Start_Date, epd.End_Date 
			            from Client as c 
			            left join Client_EPD as epd 
			            on c.Client_ID = epd.Client_ID 
                        where 1 = 1";
            string strSelect12 = @") as TBEPD 
		            where TBEPD.Client_ID = c.Client_ID 
                    order by TBEPD.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as EPD_Period, 
            stuff(
	            (
		            select ', ' + TBSAC.Type 
		            from (
			            select c.Client_ID, sac.Type, sac.Start_Date, sac.End_Date 
			            from Client as c 
			            left join Client_SAC as sac 
			            on c.Client_ID = sac.Client_ID 
                        where 1 = 1";
            string strSelect13 = @") as TBSAC 
		            where TBSAC.Client_ID = c.Client_ID 
                    order by TBSAC.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as SAC, 
            stuff(
	            (
		            select ', ' + 
		            convert(varchar(50), TBSAC.Start_Date, 6) + 
		            ' to ' + 
		            convert(varchar(50), TBSAC.End_Date, 6) 
		            from (
			            select c.Client_ID, sac.Type, sac.Start_Date, sac.End_Date 
			            from Client as c 
			            left join Client_SAC as sac 
			            on c.Client_ID = sac.Client_ID 
                        where 1 = 1";
            string strSelect14 = @") as TBSAC 
		            where TBSAC.Client_ID = c.Client_ID 
                    order by TBSAC.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as SAC_Period 
            from Client as c 
            left join GroupProprietaryClientMapping as gp 
            on c.Client_ID = gp.Client_Id 
            left join GroupProprietary as p 
            on gp.GroupProprietaryId = p.GroupProprietaryId 
            left join Client_Audit_Right as ad 
            on c.Client_ID = ad.Client_ID 
            left join Client_Media_Credit as mc 
            on c.Client_ID = mc.Client_ID 
            left join Client_Rebate as rb 
            on c.Client_ID = rb.Client_ID 
            left join Client_EPD as epd 
            on c.Client_ID = epd.Client_ID 
            left join Client_SAC as sac 
            on c.Client_ID = sac.Client_ID 
            where 1 = 1";
            string strSelect15 = @") as TBMain
            ) as TB 
            where 1 = 1";
            string strWhere1 = "";
            string strWhere2 = "";
            string strWhere3 = "";
            string strWhere4 = "";
            string strWhere5 = "";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            if (dr["ClientCode"] != DBNull.Value)
            {
                strSelect15 += " and Client_ID like '%' + @ClientCode + '%'";
                sda.SelectCommand.Parameters.Add("@ClientCode", SqlDbType.VarChar).Value = dr["ClientCode"];
            }
            if (dr["Client_ID"] != DBNull.Value)
            {
                strSelect15 += " and Client_ID = @Client_ID";
                sda.SelectCommand.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = dr["Client_ID"];
            }
            if (dr["Short_Name"] != DBNull.Value)
            {
                strSelect15 += " and Short_Name like '%' + @Short_Name + '%'";
                sda.SelectCommand.Parameters.Add("@Short_Name", SqlDbType.VarChar).Value = dr["Short_Name"];
            }
            if (dr["MasterClient_ID"] != DBNull.Value)
            {
                strSelect15 += " and [Group] = @MasterClient_ID";
                sda.SelectCommand.Parameters.Add("@MasterClient_ID", SqlDbType.VarChar).Value = dr["MasterClient_ID"];
            }
            if (dr["Agency_ID"] != DBNull.Value)
            {
                strSelect15 += " and Agency_ID = @Agency_ID";
                sda.SelectCommand.Parameters.Add("@Agency_ID", SqlDbType.VarChar).Value = dr["Agency_ID"];
            }
            if (dr["Report_to_Agency"] != DBNull.Value)
            {
                strSelect15 += " and Report_to_Agency = @Report_to_Agency";
                sda.SelectCommand.Parameters.Add("@Report_to_Agency", SqlDbType.VarChar).Value = dr["Report_to_Agency"];
            }
            if (dr["Office_ID"] != DBNull.Value)
            {
                strSelect15 += " and Office_ID = @Office_ID";
                sda.SelectCommand.Parameters.Add("@Office_ID", SqlDbType.VarChar).Value = dr["Office_ID"];
            }
            if (dr["Opt_in_Signed"] != DBNull.Value)
            {
                strSelect15 += " and Opt_in_Signed = @Opt_in_Signed";
                sda.SelectCommand.Parameters.Add("@Opt_in_Signed", SqlDbType.VarChar).Value = dr["Opt_in_Signed"];
            }
            if (dr["InactiveClient"] != DBNull.Value)
            {
                strSelect15 += " and InactiveClient = @InactiveClient";
                sda.SelectCommand.Parameters.Add("@InactiveClient", SqlDbType.VarChar).Value = dr["InactiveClient"];
            }
            if (dr["GroupProprietaryId"] != DBNull.Value)
            {
                strSelect14 += " and p.GroupProprietaryId = @GroupProprietaryId";
                sda.SelectCommand.Parameters.Add("@GroupProprietaryId", SqlDbType.VarChar).Value = dr["GroupProprietaryId"];
            }
            if (dr["Condition"] == DBNull.Value)
            {
                strWhere1 = @" and ad.End_Date = (
	                select max(End_Date) 
	                from Client_Audit_Right 
	                where Client_ID = ad.Client_ID
                )";
                strWhere2 = @" and mc.End_Date = (
	                select max(End_Date) 
	                from Client_Media_Credit 
	                where Client_ID = mc.Client_ID
                )";
                strWhere3 = @" and rb.End_Date = (
	                select max(End_Date) 
	                from Client_Rebate 
	                where Client_ID = rb.Client_ID
                )";
                strWhere4 = @" and epd.End_Date = (
	                select max(End_Date) 
	                from Client_EPD 
	                where Client_ID = epd.Client_ID
                )";
                strWhere5 = @" and sac.End_Date = (
	                select max(End_Date) 
	                from Client_SAC 
	                where Client_ID = sac.Client_ID
                )";
            }
            else
            {
                strWhere1 = @" and (
                    (
                        ad.Start_Date >= @ADSD 
                        and ad.Start_Date < @ADED
                    ) 
                    or (
                        ad.End_Date > @ADSD 
                        and ad.End_Date <= @ADED
                    )
                )";
                strWhere2 = @" and (
                    (
                        mc.Start_Date >= @MCSD 
                        and mc.Start_Date < @MCED
                    ) 
                    or (
                        mc.End_Date > @MCSD 
                        and mc.End_Date <= @MCED
                    )
                )";
                strWhere3 = @" and (
                    (
                        rb.Start_Date >= @RBSD 
                        and rb.Start_Date < @RBED
                    ) 
                    or (
                        rb.End_Date > @RBSD 
                        and rb.End_Date <= @RBED
                    )
                )";
                strWhere4 = @" and (
                    (
                        epd.Start_Date >= @EPDSD 
                        and epd.Start_Date < @EPDED
                    ) 
                    or (
                        epd.End_Date > @EPDSD 
                        and epd.End_Date <= @EPDED
                    )
                )";
                strWhere5 = @" and (
                    (
                        sac.Start_Date >= @SACSD 
                        and sac.Start_Date < @SACED
                    ) 
                    or (
                        sac.End_Date > @SACSD 
                        and sac.End_Date <= @SACED
                    )
                )";
                sda.SelectCommand.Parameters.Add("@ADSD", SqlDbType.DateTime).Value = dr["StartDate"];
                sda.SelectCommand.Parameters.Add("@ADED", SqlDbType.DateTime).Value = dr["EndDate"];
                sda.SelectCommand.Parameters.Add("@MCSD", SqlDbType.DateTime).Value = dr["StartDate"];
                sda.SelectCommand.Parameters.Add("@MCED", SqlDbType.DateTime).Value = dr["EndDate"];
                sda.SelectCommand.Parameters.Add("@RBSD", SqlDbType.DateTime).Value = dr["StartDate"];
                sda.SelectCommand.Parameters.Add("@RBED", SqlDbType.DateTime).Value = dr["EndDate"];
                sda.SelectCommand.Parameters.Add("@EPDSD", SqlDbType.DateTime).Value = dr["StartDate"];
                sda.SelectCommand.Parameters.Add("@EPDED", SqlDbType.DateTime).Value = dr["EndDate"];
                sda.SelectCommand.Parameters.Add("@SACSD", SqlDbType.DateTime).Value = dr["StartDate"];
                sda.SelectCommand.Parameters.Add("@SACED", SqlDbType.DateTime).Value = dr["EndDate"];
                if (dr["Audit_Right"] != DBNull.Value)
                {
                    string strAnd = " and ad.Type = @Audit_Right";
                    strWhere1 += strAnd;
                    strSelect14 += strWhere1;
                    sda.SelectCommand.Parameters.Add("@Audit_Right", SqlDbType.VarChar).Value = dr["Audit_Right"];
                }
                if (dr["Media_Credit"] != DBNull.Value)
                {
                    string strAnd = " and mc.Type = @Media_Credit";
                    strWhere2 += strAnd;
                    strSelect14 += strWhere2;
                    sda.SelectCommand.Parameters.Add("@Media_Credit", SqlDbType.VarChar).Value = dr["Media_Credit"];
                }
                if (dr["Rebate"] != DBNull.Value)
                {
                    string strAnd = " and rb.Type = @Rebate";
                    strWhere3 += strAnd;
                    strSelect14 += strWhere3;
                    sda.SelectCommand.Parameters.Add("@Rebate", SqlDbType.VarChar).Value = dr["Rebate"];
                }
                if (dr["EPD"] != DBNull.Value)
                {
                    string strAnd = " and epd.Type = @EPD";
                    strWhere4 += strAnd;
                    strSelect14 += strWhere4;
                    sda.SelectCommand.Parameters.Add("@EPD", SqlDbType.VarChar).Value = dr["EPD"];
                }
                if (dr["SAC"] != DBNull.Value)
                {
                    string strAnd = " and sac.Type = @SAC";
                    strWhere5 += strAnd;
                    strSelect14 += strWhere5;
                    sda.SelectCommand.Parameters.Add("@SAC", SqlDbType.VarChar).Value = dr["SAC"];
                }
            }
            if (dr["Display"] == DBNull.Value)
                strSQL = strSelect1 + strSelect2 +
                         strSelect4 + strWhere1 +
                         strSelect5 + strWhere1 +
                         strSelect6 + strWhere2 +
                         strSelect7 + strWhere2 +
                         strSelect8 + strWhere3 +
                         strSelect9 + strWhere3 +
                         strSelect10 + strWhere4 +
                         strSelect11 + strWhere4 +
                         strSelect12 + strWhere5 +
                         strSelect13 + strWhere5 +
                         strSelect14 + strSelect15;
            else
                strSQL = strSelect1 + strSelect3 +
                         strSelect4 + strWhere1 +
                         strSelect5 + strWhere1 +
                         strSelect6 + strWhere2 +
                         strSelect7 + strWhere2 +
                         strSelect8 + strWhere3 +
                         strSelect9 + strWhere3 +
                         strSelect10 + strWhere4 +
                         strSelect11 + strWhere4 +
                         strSelect12 + strWhere5 +
                         strSelect13 + strWhere5 +
                         strSelect14 + strSelect15;
            strSQL += @" order by Thai_Name, 
            Client_ID";
            sda.SelectCommand.CommandText = strSQL;
            sda.Fill(dt);
            return dt;
        }

        public DataSet SelectClientScreenReportDetail(DataRow dr)
        {
            DataSet ds = new DataSet();
            string strSQL = "";
            string strSQL1 = "";
            string strSQL2 = "";
            string strSQL3 = "";
            string strSQL4 = "";
            string strSQL5 = "";
            string strSelect1 = @"select distinct c.Client_ID as [Client Code], 
            c.Thai_Name as [Client Name], 
            c.[Group] as [Master Client], 
            c.Agency_ID as Agency, 
            c.Report_to_Agency as [Report to Agency], 
            c.Office_ID as Office, 
            case 
            when c.Opt_in_Signed = 'True' 
            then 'Signed' 
            else '' 
            end as [Opt-In], 
            stuff(
	            (
		            select ', ' + TBProprietary.GroupProprietaryName 
		            from (
			            select c.Client_ID, p.GroupProprietaryName 
			            from Client as c 
			            left join GroupProprietaryClientMapping as gp 
			            on c.Client_ID = gp.Client_Id 
			            left join GroupProprietary as p 
			            on gp.GroupProprietaryId = p.GroupProprietaryId
		            ) as TBProprietary 
		            where TBProprietary.Client_Id = c.Client_ID 
		            order by TBProprietary.GroupProprietaryName 
		            for xml path('')
	            ), 1, 2, ''
            ) as [Opt-In Group], 
            case 
            when c.InactiveClient = '1' 
            then 'Inactive' 
            else 'Active' 
            end as Status, 
            cp.Type as [";
            string strSelect2 = @"], 
            cp.Start_Date, 
            cp.End_Date 
            from Client as c 
            left outer join (
	            select * 
	            from ";
            string strSelect3 = @") as cp 
            on c.Client_ID = cp.Client_ID 
            left join GroupProprietaryClientMapping as gp 
            on c.Client_ID = gp.Client_Id 
            left join GroupProprietary as p 
            on gp.GroupProprietaryId = p.GroupProprietaryId ";
            string strField1 = "Audit Right";
            string strField2 = "EPD (Pass Back)";
            string strField3 = "Media Credit (Pass Back)";
            string strField4 = "Rebate (Pass Back)";
            string strField5 = "SAC (Pass Back)";
            string strTable1 = "Client_Audit_Right";
            string strTable2 = "Client_EPD";
            string strTable3 = "Client_Media_Credit";
            string strTable4 = "Client_Rebate";
            string strTable5 = "Client_SAC";
            string strWhere1 = @" where 1 = 1";
            string strWhere2 = @" and (
                (
                    Start_Date >= @SD 
                    and Start_Date < @ED
                ) 
                or (
                    End_Date > @SD 
                    and End_Date <= @ED
                )
            )";
            string strWhere3 = " and Type = @AD";
            string strWhere4 = " and Type = @EPD";
            string strWhere5 = " and Type = @MC";
            string strWhere6 = " and Type = @RB";
            string strWhere7 = " and Type = @SAC";
            string strWhere8 = @" and End_Date = (
	        select max(mx.End_Date) 
	        from ";
            string strWhere9 = @" as mx 
            where mx.Client_ID = ";
            string strWhere10 = @".Client_ID
            )";
            string strWhere = strWhere1;
            string strWhereRG1 = "";
            string strWhereRG2 = "";
            string strWhereRG3 = "";
            string strWhereRG4 = "";
            string strWhereRG5 = "";
            string strSubWhere1 = strWhere1;
            string strSubWhere2 = strWhere1;
            string strSubWhere3 = strWhere1;
            string strSubWhere4 = strWhere1;
            string strSubWhere5 = strWhere1;
            string strOrder = @" order by c.Thai_Name, 
            c.Client_ID, 
            cp.Start_Date";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            if (dr["ClientCode"] != DBNull.Value)
            {
                strWhere += " and c.Client_ID like '%' + @ClientCode + '%'";
                sda.SelectCommand.Parameters.Add("@ClientCode", SqlDbType.VarChar).Value = dr["ClientCode"];
            }
            if (dr["Client_ID"] != DBNull.Value)
            {
                strWhere += " and c.Client_ID = @Client_ID";
                sda.SelectCommand.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = dr["Client_ID"];
            }
            if (dr["Short_Name"] != DBNull.Value)
            {
                strWhere += " and c.Short_Name like '%' + @Short_Name + '%'";
                sda.SelectCommand.Parameters.Add("@Short_Name", SqlDbType.VarChar).Value = dr["Short_Name"];
            }
            if (dr["MasterClient_ID"] != DBNull.Value)
            {
                strWhere += " and c.[Group] = @MasterClient_ID";
                sda.SelectCommand.Parameters.Add("@MasterClient_ID", SqlDbType.VarChar).Value = dr["MasterClient_ID"];
            }
            if (dr["Agency_ID"] != DBNull.Value)
            {
                strWhere += " and c.Agency_ID = @Agency_ID";
                sda.SelectCommand.Parameters.Add("@Agency_ID", SqlDbType.VarChar).Value = dr["Agency_ID"];
            }
            if (dr["Report_to_Agency"] != DBNull.Value)
            {
                strWhere += " and c.Report_to_Agency = @Report_to_Agency";
                sda.SelectCommand.Parameters.Add("@Report_to_Agency", SqlDbType.VarChar).Value = dr["Report_to_Agency"];
            }
            if (dr["Office_ID"] != DBNull.Value)
            {
                strWhere += " and c.Office_ID = @Office_ID";
                sda.SelectCommand.Parameters.Add("@Office_ID", SqlDbType.VarChar).Value = dr["Office_ID"];
            }
            if (dr["Opt_in_Signed"] != DBNull.Value)
            {
                strWhere += " and c.Opt_in_Signed = @Opt_in_Signed";
                sda.SelectCommand.Parameters.Add("@Opt_in_Signed", SqlDbType.VarChar).Value = dr["Opt_in_Signed"];
            }
            if (dr["InactiveClient"] != DBNull.Value)
            {
                strWhere += " and c.InactiveClient = @InactiveClient";
                sda.SelectCommand.Parameters.Add("@InactiveClient", SqlDbType.VarChar).Value = dr["InactiveClient"];
            }
            if (dr["GroupProprietaryId"] != DBNull.Value)
            {
                strWhere += " and p.GroupProprietaryId = @GroupProprietaryId";
                sda.SelectCommand.Parameters.Add("@GroupProprietaryId", SqlDbType.VarChar).Value = dr["GroupProprietaryId"];
            }
            if (dr["Condition"] == DBNull.Value)
            {
                strSubWhere1 += strWhere8 + strTable1 + strWhere9 + strTable1 + strWhere10;
                strSubWhere2 += strWhere8 + strTable2 + strWhere9 + strTable2 + strWhere10;
                strSubWhere3 += strWhere8 + strTable3 + strWhere9 + strTable3 + strWhere10;
                strSubWhere4 += strWhere8 + strTable4 + strWhere9 + strTable4 + strWhere10;
                strSubWhere5 += strWhere8 + strTable5 + strWhere9 + strTable5 + strWhere10;
            }
            else
            {
                sda.SelectCommand.Parameters.Add("@SD", SqlDbType.DateTime).Value = dr["StartDate"];
                sda.SelectCommand.Parameters.Add("@ED", SqlDbType.DateTime).Value = dr["EndDate"];
                strSubWhere1 += strWhere2;
                strSubWhere2 += strWhere2;
                strSubWhere3 += strWhere2;
                strSubWhere4 += strWhere2;
                strSubWhere5 += strWhere2;
                if (dr["Audit_Right"] != DBNull.Value ||
                    dr["Media_Credit"] != DBNull.Value ||
                    dr["Rebate"] != DBNull.Value ||
                    dr["EPD"] != DBNull.Value ||
                    dr["SAC"] != DBNull.Value)
                {
                    string strSubSelect1 = @"select distinct c.Client_ID 
                    from Client as c";
                    string strSubSelect2 = @" left outer join (
	                select * 
	                from ";
                    string strSubSelect3 = @") as ad 
                    on c.Client_ID = ad.Client_ID";
                    string strSubSelect4 = @") as epd 
                    on c.Client_ID = epd.Client_ID";
                    string strSubSelect5 = @") as mc 
                    on c.Client_ID = mc.Client_ID";
                    string strSubSelect6 = @") as rb 
                    on c.Client_ID = rb.Client_ID";
                    string strSubSelect7 = @") as sac 
                    on c.Client_ID = sac.Client_ID 
                    left join GroupProprietaryClientMapping as gp 
                    on c.Client_ID = gp.Client_Id 
                    left join GroupProprietary as p 
                    on gp.GroupProprietaryId = p.GroupProprietaryId ";
                    string strSubSubWhere1 = strSubWhere1;
                    string strSubSubWhere2 = strSubWhere2;
                    string strSubSubWhere3 = strSubWhere3;
                    string strSubSubWhere4 = strSubWhere4;
                    string strSubSubWhere5 = strSubWhere5;
                    string strSubWhere = strWhere;
                    if (dr["Audit_Right"] != DBNull.Value)
                    {
                        strSubSubWhere1 += strWhere3;
                        strSubWhere += " and ad.Type = @AD";
                        strSubWhere1 += strWhere3;
                        strWhereRG1 += " and cp.Type = @AD";
                        sda.SelectCommand.Parameters.Add("@AD", SqlDbType.VarChar).Value = dr["Audit_Right"];
                    }
                    if (dr["EPD"] != DBNull.Value)
                    {
                        strSubSubWhere2 += strWhere4;
                        strSubWhere += " and epd.Type = @EPD";
                        strSubWhere2 += strWhere4;
                        strWhereRG2 += " and cp.Type = @EPD";
                        sda.SelectCommand.Parameters.Add("@EPD", SqlDbType.VarChar).Value = dr["EPD"];
                    }
                    if (dr["Media_Credit"] != DBNull.Value)
                    {
                        strSubSubWhere3 += strWhere5;
                        strSubWhere += " and mc.Type = @MC";
                        strSubWhere3 += strWhere5;
                        strWhereRG3 += " and cp.Type = @MC";
                        sda.SelectCommand.Parameters.Add("@MC", SqlDbType.VarChar).Value = dr["Media_Credit"];
                    }
                    if (dr["Rebate"] != DBNull.Value)
                    {
                        strSubSubWhere4 += strWhere6;
                        strSubWhere += " and rb.Type = @RB";
                        strSubWhere4 += strWhere6;
                        strWhereRG4 += " and cp.Type = @RB";
                        sda.SelectCommand.Parameters.Add("@RB", SqlDbType.VarChar).Value = dr["Rebate"];
                    }
                    if (dr["SAC"] != DBNull.Value)
                    {
                        strSubSubWhere5 += strWhere7;
                        strSubWhere += " and sac.Type = @SAC";
                        strSubWhere5 += strWhere7;
                        strWhereRG5 += " and cp.Type = @SAC";
                        sda.SelectCommand.Parameters.Add("@SAC", SqlDbType.VarChar).Value = dr["SAC"];
                    }
                    string strSubSQL = strSubSelect1 +
                                       strSubSelect2 + strTable1 + strSubSubWhere1 + strSubSelect3 +
                                       strSubSelect2 + strTable2 + strSubSubWhere2 + strSubSelect4 +
                                       strSubSelect2 + strTable3 + strSubSubWhere3 + strSubSelect5 +
                                       strSubSelect2 + strTable4 + strSubSubWhere4 + strSubSelect6 +
                                       strSubSelect2 + strTable5 + strSubSubWhere5 + strSubSelect7 +
                                       strSubWhere;
                    string strWhereSubQuery = " and c.Client_ID in (" + strSubSQL + ")";
                    strWhereRG1 += strWhereSubQuery;
                    strWhereRG2 += strWhereSubQuery;
                    strWhereRG3 += strWhereSubQuery;
                    strWhereRG4 += strWhereSubQuery;
                    strWhereRG5 += strWhereSubQuery;
                }
            }
            strSQL1 = strSelect1 +
                      strField1 +
                      strSelect2 +
                      strTable1 +
                      strSubWhere1 +
                      strSelect3 +
                      strWhere +
                      strWhereRG1 +
                      strOrder;
            strSQL2 = strSelect1 +
                      strField2 +
                      strSelect2 +
                      strTable2 +
                      strSubWhere2 +
                      strSelect3 +
                      strWhere +
                      strWhereRG2 +
                      strOrder;
            strSQL3 = strSelect1 +
                      strField3 +
                      strSelect2 +
                      strTable3 +
                      strSubWhere3 +
                      strSelect3 +
                      strWhere +
                      strWhereRG3 +
                      strOrder;
            strSQL4 = strSelect1 +
                      strField4 +
                      strSelect2 +
                      strTable4 +
                      strSubWhere4 +
                      strSelect3 +
                      strWhere +
                      strWhereRG4 +
                      strOrder;
            strSQL5 = strSelect1 +
                      strField5 +
                      strSelect2 +
                      strTable5 +
                      strSubWhere5 +
                      strSelect3 +
                      strWhere +
                      strWhereRG5 +
                      strOrder;
            strSQL = strSQL1 + " " + strSQL2 + " " + strSQL3 + " " + strSQL4 + " " + strSQL5;
            sda.SelectCommand.CommandText = strSQL;
            sda.Fill(ds);
            return ds;
        }

        public DataTable SelectVendor(DataRow dr)
        {
            DataTable dt = new DataTable();
            string strSQL = "";
            string strSelect1 = @"select * 
            from (
                select TBMain.Media_Vendor_ID, 
                TBMain.Short_Name, 
                case 
			    when TBMain.InActive = 0 
			    then 'Active' 
			    else 'Inactive' 
			    end as Status_Show, 
                TBMain.User_ID, 
                TBMain.Modify_Date, 
                TBMain.Vendor_Contract, 
                TBMain.Vendor_Contract_Period, 
                TBMain.EPD, 
                TBMain.EPD_Period, 
                TBMain.Media_Credit, 
                TBMain.Media_Credit_Period, 
                TBMain.Rebate, 
                TBMain.Rebate_Period, 
                TBMain.SAC, 
                TBMain.SAC_Period, 
                TBMain.Master_Group, 
                TBMain.InActive 
                from (
                    select distinct v.*, 
			        stuff(
	                    (
		                    select ', ' + TBVendorContract.Type 
		                    from (
			                    select v.Media_Vendor_ID, vc.Type, vc.Start_Date, vc.End_Date 
			                    from Media_Vendor as v 
			                    left join Media_Vendor_Contract as vc 
			                    on v.Media_Vendor_ID = vc.Media_Vendor_ID 
                                where 1 = 1";
            string strSelect2 = @") as TBVendorContract 
		            where TBVendorContract.Media_Vendor_ID = v.Media_Vendor_ID 
                    order by TBVendorContract.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as Vendor_Contract, 
            stuff(
	            (
		            select ', ' + 
		            convert(varchar(50), TBVendorContract.Start_Date, 6) + 
		            ' to ' + 
		            convert(varchar(50), TBVendorContract.End_Date, 6) 
		            from (
			            select v.Media_Vendor_ID, vc.Type, vc.Start_Date, vc.End_Date 
			            from Media_Vendor as v 
			            left join Media_Vendor_Contract as vc 
			            on v.Media_Vendor_ID = vc.Media_Vendor_ID 
                        where 1 = 1";
            string strSelect3 = @") as TBVendorContract 
		            where TBVendorContract.Media_Vendor_ID = v.Media_Vendor_ID 
                    order by TBVendorContract.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as Vendor_Contract_Period, 
            stuff(
	            (
		            select ', ' + TBMediaCredit.Type 
		            from (
			            select v.Media_Vendor_ID, mc.Type, mc.Start_Date, mc.End_Date 
			            from Media_Vendor as v 
			            left join Media_Vendor_Media_Credit as mc 
			            on v.Media_Vendor_ID = mc.Media_Vendor_ID 
                        where 1 = 1";
            string strSelect4 = @") as TBMediaCredit 
		            where TBMediaCredit.Media_Vendor_ID = v.Media_Vendor_ID 
                    order by TBMediaCredit.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as Media_Credit, 
            stuff(
	            (
		            select ', ' + 
		            convert(varchar(50), TBMediaCredit.Start_Date, 6) + 
		            ' to ' + 
		            convert(varchar(50), TBMediaCredit.End_Date, 6) 
		            from (
			            select v.Media_Vendor_ID, mc.Type, mc.Start_Date, mc.End_Date 
			            from Media_Vendor as v 
			            left join Media_Vendor_Media_Credit as mc 
			            on v.Media_Vendor_ID = mc.Media_Vendor_ID 
                        where 1 = 1";
            string strSelect5 = @") as TBMediaCredit 
		            where TBMediaCredit.Media_Vendor_ID = v.Media_Vendor_ID 
                    order by TBMediaCredit.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as Media_Credit_Period, 
			stuff(
	            (
		            select ', ' + TBRebate.Type 
		            from (
			            select v.Media_Vendor_ID, rb.Type, rb.Start_Date, rb.End_Date 
			            from Media_Vendor as v 
			            left join Media_Vendor_Rebate as rb 
			            on v.Media_Vendor_ID = rb.Media_Vendor_ID 
                        where 1 = 1";
            string strSelect6 = @") as TBRebate 
		            where TBRebate.Media_Vendor_ID = v.Media_Vendor_ID 
                    order by TBRebate.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as Rebate, 
			stuff(
	            (
		            select ', ' + 
		            convert(varchar(50), TBRebate.Start_Date, 6) + 
		            ' to ' + 
		            convert(varchar(50), TBRebate.End_Date, 6) 
		            from (
			            select v.Media_Vendor_ID, rb.Type, rb.Start_Date, rb.End_Date 
			            from Media_Vendor as v 
			            left join Media_Vendor_Rebate as rb 
			            on v.Media_Vendor_ID = rb.Media_Vendor_ID 
                        where 1 = 1";
            string strSelect7 = @") as TBRebate 
		            where TBRebate.Media_Vendor_ID = v.Media_Vendor_ID 
                    order by TBRebate.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as Rebate_Period, 
			stuff(
	            (
		            select ', ' + TBEPD.Type 
		            from (
			            select v.Media_Vendor_ID, epd.Type, epd.Start_Date, epd.End_Date 
			            from Media_Vendor as v 
			            left join Media_Vendor_EPD as epd 
			            on v.Media_Vendor_ID = epd.Media_Vendor_ID 
                        where 1 = 1";
            string strSelect8 = @") as TBEPD 
		            where TBEPD.Media_Vendor_ID = v.Media_Vendor_ID 
                    order by TBEPD.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as EPD, 
			stuff(
	            (
		            select ', ' + 
		            convert(varchar(50), TBEPD.Start_Date, 6) + 
		            ' to ' + 
		            convert(varchar(50), TBEPD.End_Date, 6) 
		            from (
			            select v.Media_Vendor_ID, epd.Type, epd.Start_Date, epd.End_Date 
			            from Media_Vendor as v 
			            left join Media_Vendor_EPD as epd 
			            on v.Media_Vendor_ID = epd.Media_Vendor_ID 
                        where 1 = 1";
            string strSelect9 = @") as TBEPD 
		            where TBEPD.Media_Vendor_ID = v.Media_Vendor_ID 
                    order by TBEPD.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as EPD_Period, 
			stuff(
	            (
		            select ', ' + TBSAC.Type 
		            from (
			            select v.Media_Vendor_ID, sac.Type, sac.Start_Date, sac.End_Date 
			            from Media_Vendor as v 
			            left join Media_Vendor_SAC as sac 
			            on v.Media_Vendor_ID = sac.Media_Vendor_ID 
                        where 1 = 1";
            string strSelect10 = @") as TBSAC 
		            where TBSAC.Media_Vendor_ID = v.Media_Vendor_ID 
                    order by TBSAC.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as SAC, 
			stuff(
	            (
		            select ', ' + 
		            convert(varchar(50), TBSAC.Start_Date, 6) + 
		            ' to ' + 
		            convert(varchar(50), TBSAC.End_Date, 6) 
		            from (
			            select v.Media_Vendor_ID, sac.Type, sac.Start_Date, sac.End_Date 
			            from Media_Vendor as v 
			            left join Media_Vendor_SAC as sac 
			            on v.Media_Vendor_ID = sac.Media_Vendor_ID 
                        where 1 = 1";
            string strSelect11 = @") as TBSAC 
		            where TBSAC.Media_Vendor_ID = v.Media_Vendor_ID 
                    order by TBSAC.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as SAC_Period 
			from Media_Vendor as v 
            left join Media_Vendor_Contract as vc 
            on v.Media_Vendor_ID = vc.Media_Vendor_ID 
            left join Media_Vendor_Media_Credit as mc 
            on v.Media_Vendor_ID = mc.Media_Vendor_ID 
            left join Media_Vendor_Rebate as rb 
            on v.Media_Vendor_ID = rb.Media_Vendor_ID 
            left join Media_Vendor_EPD as epd 
            on v.Media_Vendor_ID = epd.Media_Vendor_ID 
            left join Media_Vendor_SAC as sac 
            on v.Media_Vendor_ID = sac.Media_Vendor_ID 
            where 1 = 1";
            string strSelect12 = @") as TBMain
            ) as TB  
			where 1 = 1";
            string strWhere1 = "";
            string strWhere2 = "";
            string strWhere3 = "";
            string strWhere4 = "";
            string strWhere5 = "";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            if (dr["Media_Vendor_Code"] != DBNull.Value)
            {
                strSelect12 += " and Media_Vendor_ID like '%' + @Media_Vendor_Code + '%'";
                sda.SelectCommand.Parameters.Add("@Media_Vendor_Code", SqlDbType.VarChar).Value = dr["Media_Vendor_Code"];
            }
            if (dr["Media_Vendor_ID"] != DBNull.Value)
            {
                strSelect12 += " and Media_Vendor_ID = @Media_Vendor_ID";
                sda.SelectCommand.Parameters.Add("@Media_Vendor_ID", SqlDbType.VarChar).Value = dr["Media_Vendor_ID"];
            }
            if (dr["Master_Group"] != DBNull.Value)
            {
                strSelect12 += " and Master_Group = @Master_Group";
                sda.SelectCommand.Parameters.Add("@Master_Group", SqlDbType.VarChar).Value = dr["Master_Group"];
            }
            if (dr["Short_Name"] != DBNull.Value)
            {
                strSelect12 += " and Short_Name like '%' + @Short_Name + '%'";
                sda.SelectCommand.Parameters.Add("@Short_Name", SqlDbType.VarChar).Value = dr["Short_Name"];
            }
            if (dr["InActive"] != DBNull.Value)
            {
                strSelect12 += " and InActive = @InActive";
                sda.SelectCommand.Parameters.Add("@InActive", SqlDbType.VarChar).Value = dr["InActive"];
            }
            if (dr["Condition"] == DBNull.Value)
            {
                strWhere1 = @" and vc.End_Date = (
	                select max(End_Date) 
	                from Media_Vendor_Contract 
	                where Media_Vendor_ID = vc.Media_Vendor_ID
                )";
                strWhere2 = @" and mc.End_Date = (
	                select max(End_Date) 
	                from Media_Vendor_Media_Credit 
	                where Media_Vendor_ID = mc.Media_Vendor_ID
                )";
                strWhere3 = @" and rb.End_Date = (
	                select max(End_Date) 
	                from Media_Vendor_Rebate 
	                where Media_Vendor_ID = rb.Media_Vendor_ID
                )";
                strWhere4 = @" and epd.End_Date = (
	                select max(End_Date) 
	                from Media_Vendor_EPD 
	                where Media_Vendor_ID = epd.Media_Vendor_ID
                )";
                strWhere5 = @" and sac.End_Date = (
	                select max(End_Date) 
	                from Media_Vendor_SAC 
	                where Media_Vendor_ID = sac.Media_Vendor_ID
                )";
            }
            else
            {
                strWhere1 = @" and (
                    (
                        vc.Start_Date >= @VCSD 
                        and vc.Start_Date < @VCED
                    ) 
                    or (
                        vc.End_Date > @VCSD 
                        and vc.End_Date <= @VCED
                    )
                )";
                strWhere2 = @" and (
                    (
                        mc.Start_Date >= @MCSD 
                        and mc.Start_Date < @MCED
                    ) 
                    or (
                        mc.End_Date > @MCSD 
                        and mc.End_Date <= @MCED
                    )
                )";
                strWhere3 = @" and (
                    (
                        rb.Start_Date >= @RBSD 
                        and rb.Start_Date < @RBED
                    ) 
                    or (
                        rb.End_Date > @RBSD 
                        and rb.End_Date <= @RBED
                    )
                )";
                strWhere4 = @" and (
                    (
                        epd.Start_Date >= @EPDSD 
                        and epd.Start_Date < @EPDED
                    ) 
                    or (
                        epd.End_Date > @EPDSD 
                        and epd.End_Date <= @EPDED
                    )
                )";
                strWhere5 = @" and (
                    (
                        sac.Start_Date >= @SACSD 
                        and sac.Start_Date < @SACED
                    ) 
                    or (
                        sac.End_Date > @SACSD 
                        and sac.End_Date <= @SACED
                    )
                )";
                sda.SelectCommand.Parameters.Add("@VCSD", SqlDbType.DateTime).Value = dr["StartDate"];
                sda.SelectCommand.Parameters.Add("@VCED", SqlDbType.DateTime).Value = dr["EndDate"];
                sda.SelectCommand.Parameters.Add("@MCSD", SqlDbType.DateTime).Value = dr["StartDate"];
                sda.SelectCommand.Parameters.Add("@MCED", SqlDbType.DateTime).Value = dr["EndDate"];
                sda.SelectCommand.Parameters.Add("@RBSD", SqlDbType.DateTime).Value = dr["StartDate"];
                sda.SelectCommand.Parameters.Add("@RBED", SqlDbType.DateTime).Value = dr["EndDate"];
                sda.SelectCommand.Parameters.Add("@EPDSD", SqlDbType.DateTime).Value = dr["StartDate"];
                sda.SelectCommand.Parameters.Add("@EPDED", SqlDbType.DateTime).Value = dr["EndDate"];
                sda.SelectCommand.Parameters.Add("@SACSD", SqlDbType.DateTime).Value = dr["StartDate"];
                sda.SelectCommand.Parameters.Add("@SACED", SqlDbType.DateTime).Value = dr["EndDate"];
                if (dr["Vendor_Contract"] != DBNull.Value)
                {
                    string strAnd = " and vc.Type = @Vendor_Contract";
                    strWhere1 += strAnd;
                    strSelect11 += strWhere1;
                    sda.SelectCommand.Parameters.Add("@Vendor_Contract", SqlDbType.VarChar).Value = dr["Vendor_Contract"];
                }
                if (dr["Media_Credit"] != DBNull.Value)
                {
                    string strAnd = " and mc.Type = @Media_Credit";
                    strWhere2 += strAnd;
                    strSelect11 += strWhere2;
                    sda.SelectCommand.Parameters.Add("@Media_Credit", SqlDbType.VarChar).Value = dr["Media_Credit"];
                }
                if (dr["Rebate"] != DBNull.Value)
                {
                    string strAnd = " and rb.Type = @Rebate";
                    strWhere3 += strAnd;
                    strSelect11 += strWhere3;
                    sda.SelectCommand.Parameters.Add("@Rebate", SqlDbType.VarChar).Value = dr["Rebate"];
                }
                if (dr["EPD"] != DBNull.Value)
                {
                    string strAnd = " and epd.Type = @EPD";
                    strWhere4 += strAnd;
                    strSelect11 += strWhere4;
                    sda.SelectCommand.Parameters.Add("@EPD", SqlDbType.VarChar).Value = dr["EPD"];
                }
                if (dr["SAC"] != DBNull.Value)
                {
                    string strAnd = " and sac.Type = @SAC";
                    strWhere5 += strAnd;
                    strSelect11 += strWhere5;
                    sda.SelectCommand.Parameters.Add("@SAC", SqlDbType.VarChar).Value = dr["SAC"];
                }
            }
            strSQL = strSelect1 + strWhere1 +
            strSelect2 + strWhere1 +
            strSelect3 + strWhere2 +
            strSelect4 + strWhere2 +
            strSelect5 + strWhere3 +
            strSelect6 + strWhere3 +
            strSelect7 + strWhere4 +
            strSelect8 + strWhere4 +
            strSelect9 + strWhere5 +
            strSelect10 + strWhere5 +
            strSelect11 + strSelect12;
            strSQL += @" order by Short_Name, 
            Media_Vendor_ID";
            sda.SelectCommand.CommandText = strSQL;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectVendor(string strVendor_ID)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select v.*
            ,isnull(mv.Short_Name,'') MasterVendorName
            ,isnull(a.Short_Name,'') AgencyName
            from Media_Vendor v 
            left join Media_Vendor mv
	        on v.Master_Group = mv.Media_Vendor_ID
            left join Agency a
	        on a.Agency_ID = v.Agency_ID
            where v.Media_Vendor_ID = @Media_Vendor_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Media_Vendor_ID", SqlDbType.VarChar).Value = strVendor_ID;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectMediaPeriod(string strValue)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select EffectiveDate, 
            InactiveDate 
            from Media 
            where Media_ID = @Media_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Media_ID", SqlDbType.VarChar).Value = strValue;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectVendorByMedia(string strValue)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select v.Media_Vendor_ID, v.Short_Name 
            from Media_Vendor v 
            inner join Media_Detail m 
	        on m.Media_Vendor_ID = v.Media_Vendor_ID 
            where v.InActive = 0 
            and m.Media_ID = @Media_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Media_ID", SqlDbType.VarChar).Value = strValue;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectAdeptMediaTypeByMedia(string strValue)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select m.Adept_Media_Type, amt.Adept_Media_Type_Name 
            from Media m 
            inner join Adept_Media_Type amt 
	        on m.Adept_Media_Type = amt.Adept_Media_Type 
            where m.Media_ID = @Media_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Media_ID", SqlDbType.VarChar).Value = strValue;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectPerferVendorPeriod(string strVendor_ID)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select Media_Vendor_ID,
            CONVERT(date,start_date) start_date
            ,Convert(date,end_date) end_date
             from Media_Vendor_PreferDetail
            where Media_Vendor_ID = @Media_Vendor_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Media_Vendor_ID", SqlDbType.VarChar).Value = strVendor_ID;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectVendorRedGreenPeriod(string strVendor_ID, string table)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select Media_Vendor_ID, 
            Type, 
            convert(date, Start_Date) as Start_Date, 
            convert(date, End_Date) as End_Date, 
            'Old' as Flag 
            from " + table + @" 
            where Media_Vendor_ID = @Media_Vendor_ID 
            order by Start_Date";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Media_Vendor_ID", SqlDbType.VarChar).Value = strVendor_ID;
            sda.Fill(dt);
            return dt;
        }

        public bool SelectVendorIsUsing(string strMedia_Vendor_ID)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select top 1 1 from spot_plan where Media_Vendor_ID = @Media_Vendor_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 0;
            sda.SelectCommand.Parameters.Add("@Media_Vendor_ID", SqlDbType.VarChar).Value = strMedia_Vendor_ID;
            sda.Fill(dt);
            return dt.Rows.Count > 0 ? true : false;
        }

        public DataTable SelectVendorMasterFileReport()
        {
            DataTable dt = new DataTable();
            string strSQL = @"dbo.MasterFileVendor_Rpt";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sda.SelectCommand.CommandTimeout = 0;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectPreferVendorReport()
        {
            DataTable dt = new DataTable();
            string strSQL = @"SELECT v.Media_Vendor_ID AS Vendor_ID, 
            v.Short_Name AS Vendor_Name, 
            CAST(vd.start_date AS Date) AS Start_Date, 
            CAST(vd.end_date AS Date) AS End_Date, 
            CASE WHEN v.InActive = 0 THEN 'Yes' ELSE 'No' END AS Active 
            FROM Media_Vendor AS v 
            LEFT JOIN Media_Vendor_PreferDetail AS vd 
            ON v.Media_Vendor_ID = vd.Media_Vendor_ID 
            WHERE isPreferred = 1 
            ORDER BY v.Media_Vendor_ID asc, 
            vd.start_date desc";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 0;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectVendorScreenReport(DataRow dr)
        {
            DataTable dt = new DataTable();
            string strSQL = "";
            string strSelect1 = @"select Media_Vendor_ID as [Vendor Code], 
			Short_Name as [Vendor Name], 
            Master_Group as [Master Vendor], 
			Status_Show as Status, 
			User_ID as [Modify By], 
			Modify_Date as [Modify Date], ";
            string strSelect2 = @"Vendor_Contract as [Sign Vendor Contract], 
            EPD as EPD, 
            --Media_Credit as [Media Credit], 
            Rebate as Rebate, 
            SAC as SAC ";
            string strSelect3 = @"Vendor_Contract as [Sign Vendor Contract], 
            Vendor_Contract_Period as [Sign Vendor Contract Period], 
            EPD, 
            EPD_Period as [EPD Period], 
            /*Media_Credit as [Media Credit], 
            Media_Credit_Period as [Media Credit Period], */
            Rebate, 
            Rebate_Period as [Rebate Period], 
            SAC, 
            SAC_Period as [SAC Period] ";
            string strSelect4 = @"from (
            select TBMain.Media_Vendor_ID, 
            TBMain.Short_Name, 
            case 
			when TBMain.InActive = 0 
			then 'Active' 
			else 'Inactive' 
			end as Status_Show, 
            TBMain.User_ID, 
            TBMain.Modify_Date, 
            TBMain.Vendor_Contract, 
            TBMain.Vendor_Contract_Period, 
            TBMain.EPD, 
            TBMain.EPD_Period, 
            TBMain.Media_Credit, 
            TBMain.Media_Credit_Period, 
            TBMain.Rebate, 
            TBMain.Rebate_Period, 
            TBMain.SAC, 
            TBMain.SAC_Period, 
            TBMain.Master_Group, 
            TBMain.InActive 
            from (
                select distinct v.*, 
			    stuff(
	                (
		                select ', ' + TBVendorContract.Type 
		                from (
			                select v.Media_Vendor_ID, vc.Type, vc.Start_Date, vc.End_Date 
			                from Media_Vendor as v 
			                left join Media_Vendor_Contract as vc 
			                on v.Media_Vendor_ID = vc.Media_Vendor_ID 
                            where 1 = 1";
            string strSelect5 = @") as TBVendorContract 
		            where TBVendorContract.Media_Vendor_ID = v.Media_Vendor_ID 
                    order by TBVendorContract.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as Vendor_Contract, 
            stuff(
	            (
		            select ', ' + 
		            convert(varchar(50), TBVendorContract.Start_Date, 6) + 
		            ' to ' + 
		            convert(varchar(50), TBVendorContract.End_Date, 6) 
		            from (
			            select v.Media_Vendor_ID, vc.Type, vc.Start_Date, vc.End_Date 
			            from Media_Vendor as v 
			            left join Media_Vendor_Contract as vc 
			            on v.Media_Vendor_ID = vc.Media_Vendor_ID 
                        where 1 = 1";
            string strSelect6 = @") as TBVendorContract 
		            where TBVendorContract.Media_Vendor_ID = v.Media_Vendor_ID 
                    order by TBVendorContract.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as Vendor_Contract_Period, 
            stuff(
	            (
		            select ', ' + TBMediaCredit.Type 
		            from (
			            select v.Media_Vendor_ID, mc.Type, mc.Start_Date, mc.End_Date 
			            from Media_Vendor as v 
			            left join Media_Vendor_Media_Credit as mc 
			            on v.Media_Vendor_ID = mc.Media_Vendor_ID 
                        where 1 = 1";
            string strSelect7 = @") as TBMediaCredit 
		            where TBMediaCredit.Media_Vendor_ID = v.Media_Vendor_ID 
                    order by TBMediaCredit.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as Media_Credit, 
            stuff(
	            (
		            select ', ' + 
		            convert(varchar(50), TBMediaCredit.Start_Date, 6) + 
		            ' to ' + 
		            convert(varchar(50), TBMediaCredit.End_Date, 6) 
		            from (
			            select v.Media_Vendor_ID, mc.Type, mc.Start_Date, mc.End_Date 
			            from Media_Vendor as v 
			            left join Media_Vendor_Media_Credit as mc 
			            on v.Media_Vendor_ID = mc.Media_Vendor_ID 
                        where 1 = 1";
            string strSelect8 = @") as TBMediaCredit 
		            where TBMediaCredit.Media_Vendor_ID = v.Media_Vendor_ID 
                    order by TBMediaCredit.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as Media_Credit_Period, 
			stuff(
	            (
		            select ', ' + TBRebate.Type 
		            from (
			            select v.Media_Vendor_ID, rb.Type, rb.Start_Date, rb.End_Date 
			            from Media_Vendor as v 
			            left join Media_Vendor_Rebate as rb 
			            on v.Media_Vendor_ID = rb.Media_Vendor_ID 
                        where 1 = 1";
            string strSelect9 = @") as TBRebate 
		            where TBRebate.Media_Vendor_ID = v.Media_Vendor_ID 
                    order by TBRebate.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as Rebate, 
			stuff(
	            (
		            select ', ' + 
		            convert(varchar(50), TBRebate.Start_Date, 6) + 
		            ' to ' + 
		            convert(varchar(50), TBRebate.End_Date, 6) 
		            from (
			            select v.Media_Vendor_ID, rb.Type, rb.Start_Date, rb.End_Date 
			            from Media_Vendor as v 
			            left join Media_Vendor_Rebate as rb 
			            on v.Media_Vendor_ID = rb.Media_Vendor_ID 
                        where 1 = 1";
            string strSelect10 = @") as TBRebate 
		            where TBRebate.Media_Vendor_ID = v.Media_Vendor_ID 
                    order by TBRebate.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as Rebate_Period, 
			stuff(
	            (
		            select ', ' + TBEPD.Type 
		            from (
			            select v.Media_Vendor_ID, epd.Type, epd.Start_Date, epd.End_Date 
			            from Media_Vendor as v 
			            left join Media_Vendor_EPD as epd 
			            on v.Media_Vendor_ID = epd.Media_Vendor_ID 
                        where 1 = 1";
            string strSelect11 = @") as TBEPD 
		            where TBEPD.Media_Vendor_ID = v.Media_Vendor_ID 
                    order by TBEPD.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as EPD, 
			stuff(
	            (
		            select ', ' + 
		            convert(varchar(50), TBEPD.Start_Date, 6) + 
		            ' to ' + 
		            convert(varchar(50), TBEPD.End_Date, 6) 
		            from (
			            select v.Media_Vendor_ID, epd.Type, epd.Start_Date, epd.End_Date 
			            from Media_Vendor as v 
			            left join Media_Vendor_EPD as epd 
			            on v.Media_Vendor_ID = epd.Media_Vendor_ID 
                        where 1 = 1";
            string strSelect12 = @") as TBEPD 
		            where TBEPD.Media_Vendor_ID = v.Media_Vendor_ID 
                    order by TBEPD.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as EPD_Period, 
			stuff(
	            (
		            select ', ' + TBSAC.Type 
		            from (
			            select v.Media_Vendor_ID, sac.Type, sac.Start_Date, sac.End_Date 
			            from Media_Vendor as v 
			            left join Media_Vendor_SAC as sac 
			            on v.Media_Vendor_ID = sac.Media_Vendor_ID 
                        where 1 = 1";
            string strSelect13 = @") as TBSAC 
		            where TBSAC.Media_Vendor_ID = v.Media_Vendor_ID 
                    order by TBSAC.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as SAC, 
			stuff(
	            (
		            select ', ' + 
		            convert(varchar(50), TBSAC.Start_Date, 6) + 
		            ' to ' + 
		            convert(varchar(50), TBSAC.End_Date, 6) 
		            from (
			            select v.Media_Vendor_ID, sac.Type, sac.Start_Date, sac.End_Date 
			            from Media_Vendor as v 
			            left join Media_Vendor_SAC as sac 
			            on v.Media_Vendor_ID = sac.Media_Vendor_ID 
                        where 1 = 1";
            string strSelect14 = @") as TBSAC 
		            where TBSAC.Media_Vendor_ID = v.Media_Vendor_ID 
                    order by TBSAC.Start_Date 
		            for xml path('')
	            ), 1, 2, ''
            ) as SAC_Period 
			from Media_Vendor as v 
            left join Media_Vendor_Contract as vc 
            on v.Media_Vendor_ID = vc.Media_Vendor_ID 
            left join Media_Vendor_Media_Credit as mc 
            on v.Media_Vendor_ID = mc.Media_Vendor_ID 
            left join Media_Vendor_Rebate as rb 
            on v.Media_Vendor_ID = rb.Media_Vendor_ID 
            left join Media_Vendor_EPD as epd 
            on v.Media_Vendor_ID = epd.Media_Vendor_ID 
            left join Media_Vendor_SAC as sac 
            on v.Media_Vendor_ID = sac.Media_Vendor_ID 
            where 1 = 1";
            string strSelect15 = @") as TBMain
            ) as TB  
			where 1 = 1";
            string strWhere1 = "";
            string strWhere2 = "";
            string strWhere3 = "";
            string strWhere4 = "";
            string strWhere5 = "";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            if (dr["Media_Vendor_Code"] != DBNull.Value)
            {
                strSelect15 += " and Media_Vendor_ID like '%' + @Media_Vendor_Code + '%'";
                sda.SelectCommand.Parameters.Add("@Media_Vendor_Code", SqlDbType.VarChar).Value = dr["Media_Vendor_Code"];
            }
            if (dr["Media_Vendor_ID"] != DBNull.Value)
            {
                strSelect15 += " and Media_Vendor_ID = @Media_Vendor_ID";
                sda.SelectCommand.Parameters.Add("@Media_Vendor_ID", SqlDbType.VarChar).Value = dr["Media_Vendor_ID"];
            }
            if (dr["Master_Group"] != DBNull.Value)
            {
                strSelect15 += " and Master_Group = @Master_Group";
                sda.SelectCommand.Parameters.Add("@Master_Group", SqlDbType.VarChar).Value = dr["Master_Group"];
            }
            if (dr["Short_Name"] != DBNull.Value)
            {
                strSelect15 += " and Short_Name like '%' + @Short_Name + '%'";
                sda.SelectCommand.Parameters.Add("@Short_Name", SqlDbType.VarChar).Value = dr["Short_Name"];
            }
            if (dr["InActive"] != DBNull.Value)
            {
                strSelect15 += " and InActive = @InActive";
                sda.SelectCommand.Parameters.Add("@InActive", SqlDbType.VarChar).Value = dr["InActive"];
            }
            if (dr["Condition"] == DBNull.Value)
            {
                strWhere1 = @" and vc.End_Date = (
	                select max(End_Date) 
	                from Media_Vendor_Contract 
	                where Media_Vendor_ID = vc.Media_Vendor_ID
                )";
                strWhere2 = @" and mc.End_Date = (
	                select max(End_Date) 
	                from Media_Vendor_Media_Credit 
	                where Media_Vendor_ID = mc.Media_Vendor_ID
                )";
                strWhere3 = @" and rb.End_Date = (
	                select max(End_Date) 
	                from Media_Vendor_Rebate 
	                where Media_Vendor_ID = rb.Media_Vendor_ID
                )";
                strWhere4 = @" and epd.End_Date = (
	                select max(End_Date) 
	                from Media_Vendor_EPD 
	                where Media_Vendor_ID = epd.Media_Vendor_ID
                )";
                strWhere5 = @" and sac.End_Date = (
	                select max(End_Date) 
	                from Media_Vendor_SAC 
	                where Media_Vendor_ID = sac.Media_Vendor_ID
                )";
            }
            else
            {
                strWhere1 = @" and (
                    (
                        vc.Start_Date >= @VCSD 
                        and vc.Start_Date < @VCED
                    ) 
                    or (
                        vc.End_Date > @VCSD 
                        and vc.End_Date <= @VCED
                    )
                )";
                strWhere2 = @" and (
                    (
                        mc.Start_Date >= @MCSD 
                        and mc.Start_Date < @MCED
                    ) 
                    or (
                        mc.End_Date > @MCSD 
                        and mc.End_Date <= @MCED
                    )
                )";
                strWhere3 = @" and (
                    (
                        rb.Start_Date >= @RBSD 
                        and rb.Start_Date < @RBED
                    ) 
                    or (
                        rb.End_Date > @RBSD 
                        and rb.End_Date <= @RBED
                    )
                )";
                strWhere4 = @" and (
                    (
                        epd.Start_Date >= @EPDSD 
                        and epd.Start_Date < @EPDED
                    ) 
                    or (
                        epd.End_Date > @EPDSD 
                        and epd.End_Date <= @EPDED
                    )
                )";
                strWhere5 = @" and (
                    (
                        sac.Start_Date >= @SACSD 
                        and sac.Start_Date < @SACED
                    ) 
                    or (
                        sac.End_Date > @SACSD 
                        and sac.End_Date <= @SACED
                    )
                )";
                sda.SelectCommand.Parameters.Add("@VCSD", SqlDbType.DateTime).Value = dr["StartDate"];
                sda.SelectCommand.Parameters.Add("@VCED", SqlDbType.DateTime).Value = dr["EndDate"];
                sda.SelectCommand.Parameters.Add("@MCSD", SqlDbType.DateTime).Value = dr["StartDate"];
                sda.SelectCommand.Parameters.Add("@MCED", SqlDbType.DateTime).Value = dr["EndDate"];
                sda.SelectCommand.Parameters.Add("@RBSD", SqlDbType.DateTime).Value = dr["StartDate"];
                sda.SelectCommand.Parameters.Add("@RBED", SqlDbType.DateTime).Value = dr["EndDate"];
                sda.SelectCommand.Parameters.Add("@EPDSD", SqlDbType.DateTime).Value = dr["StartDate"];
                sda.SelectCommand.Parameters.Add("@EPDED", SqlDbType.DateTime).Value = dr["EndDate"];
                sda.SelectCommand.Parameters.Add("@SACSD", SqlDbType.DateTime).Value = dr["StartDate"];
                sda.SelectCommand.Parameters.Add("@SACED", SqlDbType.DateTime).Value = dr["EndDate"];
                if (dr["Vendor_Contract"] != DBNull.Value)
                {
                    string strAnd = " and vc.Type = @Vendor_Contract";
                    strWhere1 += strAnd;
                    strSelect14 += strWhere1;
                    sda.SelectCommand.Parameters.Add("@Vendor_Contract", SqlDbType.VarChar).Value = dr["Vendor_Contract"];
                }
                if (dr["Media_Credit"] != DBNull.Value)
                {
                    string strAnd = " and mc.Type = @Media_Credit";
                    strWhere2 += strAnd;
                    strSelect14 += strWhere2;
                    sda.SelectCommand.Parameters.Add("@Media_Credit", SqlDbType.VarChar).Value = dr["Media_Credit"];
                }
                if (dr["Rebate"] != DBNull.Value)
                {
                    string strAnd = " and rb.Type = @Rebate";
                    strWhere3 += strAnd;
                    strSelect14 += strWhere3;
                    sda.SelectCommand.Parameters.Add("@Rebate", SqlDbType.VarChar).Value = dr["Rebate"];
                }
                if (dr["EPD"] != DBNull.Value)
                {
                    string strAnd = " and epd.Type = @EPD";
                    strWhere4 += strAnd;
                    strSelect14 += strWhere4;
                    sda.SelectCommand.Parameters.Add("@EPD", SqlDbType.VarChar).Value = dr["EPD"];
                }
                if (dr["SAC"] != DBNull.Value)
                {
                    string strAnd = " and sac.Type = @SAC";
                    strWhere5 += strAnd;
                    strSelect14 += strWhere5;
                    sda.SelectCommand.Parameters.Add("@SAC", SqlDbType.VarChar).Value = dr["SAC"];
                }
            }
            if (dr["Display"] == DBNull.Value)
                strSQL = strSelect1 + strSelect2 +
                         strSelect4 + strWhere1 +
                         strSelect5 + strWhere1 +
                         strSelect6 + strWhere2 +
                         strSelect7 + strWhere2 +
                         strSelect8 + strWhere3 +
                         strSelect9 + strWhere3 +
                         strSelect10 + strWhere4 +
                         strSelect11 + strWhere4 +
                         strSelect12 + strWhere5 +
                         strSelect13 + strWhere5 +
                         strSelect14 + strSelect15;
            else
                strSQL = strSelect1 + strSelect3 +
                         strSelect4 + strWhere1 +
                         strSelect5 + strWhere1 +
                         strSelect6 + strWhere2 +
                         strSelect7 + strWhere2 +
                         strSelect8 + strWhere3 +
                         strSelect9 + strWhere3 +
                         strSelect10 + strWhere4 +
                         strSelect11 + strWhere4 +
                         strSelect12 + strWhere5 +
                         strSelect13 + strWhere5 +
                         strSelect14 + strSelect15;
            strSQL += @" order by Short_Name, 
            Media_Vendor_ID";
            sda.SelectCommand.CommandText = strSQL;
            sda.Fill(dt);
            return dt;
        }

        public DataSet SelectVendorScreenReportDetail(DataRow dr)
        {
            DataSet ds = new DataSet();
            string strSQL = "";
            string strSQL1 = "";
            string strSQL2 = "";
            string strSQL3 = "";
            string strSQL4 = "";
            string strSQL5 = "";
            string strSelect1 = @"select v.Media_Vendor_ID as [Vendor Code], 
			v.Short_Name as [Vendor Name], 
            v.Master_Group as [Master Vendor], 
			case 
			when v.InActive = 0 
			then 'Active' 
			else 'Inactive' 
			end as Status, 
			v.User_ID as [Modify By], 
			v.Modify_Date as [Modify Date], 
            vp.Type as ";
            string strSelect2 = @", 
            vp.Start_Date, 
            vp.End_Date 
            from Media_Vendor as v 
            left outer join (
	            select * 
	            from ";
            string strSelect3 = @") as vp 
            on v.Media_Vendor_ID = vp.Media_Vendor_ID ";
            string strField1 = "Sign Vendor Contract";
            string strField2 = "EPD";
            string strField3 = "Media Credit";
            string strField4 = "Rebate";
            string strField5 = "SAC";
            string strTable1 = "Media_Vendor_Contract";
            string strTable2 = "Media_Vendor_EPD";
            string strTable3 = "Media_Vendor_Media_Credit";
            string strTable4 = "Media_Vendor_Rebate";
            string strTable5 = "Media_Vendor_SAC";
            string strWhere1 = @" where 1 = 1";
            string strWhere2 = @" and (
                (
                    Start_Date >= @SD 
                    and Start_Date < @ED
                ) 
                or (
                    End_Date > @SD 
                    and End_Date <= @ED
                )
            )";
            string strWhere3 = " and Type = @VC";
            string strWhere4 = " and Type = @EPD";
            string strWhere5 = " and Type = @MC";
            string strWhere6 = " and Type = @RB";
            string strWhere7 = " and Type = @SAC";
            string strWhere8 = @" and End_Date = (
	        select max(mx.End_Date) 
	        from ";
            string strWhere9 = @" as mx 
            where mx.Media_Vendor_ID = ";
            string strWhere10 = @".Media_Vendor_ID
            )";
            string strWhere = strWhere1;
            string strWhereRG1 = "";
            string strWhereRG2 = "";
            string strWhereRG3 = "";
            string strWhereRG4 = "";
            string strWhereRG5 = "";
            string strSubWhere1 = strWhere1;
            string strSubWhere2 = strWhere1;
            string strSubWhere3 = strWhere1;
            string strSubWhere4 = strWhere1;
            string strSubWhere5 = strWhere1;
            string strOrder = @" order by v.Short_Name, 
            v.Media_Vendor_ID, 
            vp.Start_Date";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            if (dr["Media_Vendor_Code"] != DBNull.Value)
            {
                strWhere += " and v.Media_Vendor_ID like '%' + @Media_Vendor_Code + '%'";
                sda.SelectCommand.Parameters.Add("@Media_Vendor_Code", SqlDbType.VarChar).Value = dr["Media_Vendor_Code"];
            }
            if (dr["Media_Vendor_ID"] != DBNull.Value)
            {
                strWhere += " and v.Media_Vendor_ID = @Media_Vendor_ID";
                sda.SelectCommand.Parameters.Add("@Media_Vendor_ID", SqlDbType.VarChar).Value = dr["Media_Vendor_ID"];
            }
            if (dr["Master_Group"] != DBNull.Value)
            {
                strWhere += " and v.Master_Group = @Master_Group";
                sda.SelectCommand.Parameters.Add("@Master_Group", SqlDbType.VarChar).Value = dr["Master_Group"];
            }
            if (dr["Short_Name"] != DBNull.Value)
            {
                strWhere += " and v.Short_Name like '%' + @Short_Name + '%'";
                sda.SelectCommand.Parameters.Add("@Short_Name", SqlDbType.VarChar).Value = dr["Short_Name"];
            }
            if (dr["InActive"] != DBNull.Value)
            {
                strWhere += " and v.InActive = @InActive";
                sda.SelectCommand.Parameters.Add("@InActive", SqlDbType.VarChar).Value = dr["InActive"];
            }
            if (dr["Condition"] == DBNull.Value)
            {
                strSubWhere1 += strWhere8 + strTable1 + strWhere9 + strTable1 + strWhere10;
                strSubWhere2 += strWhere8 + strTable2 + strWhere9 + strTable2 + strWhere10;
                strSubWhere3 += strWhere8 + strTable3 + strWhere9 + strTable3 + strWhere10;
                strSubWhere4 += strWhere8 + strTable4 + strWhere9 + strTable4 + strWhere10;
                strSubWhere5 += strWhere8 + strTable5 + strWhere9 + strTable5 + strWhere10;
            }
            else
            {
                sda.SelectCommand.Parameters.Add("@SD", SqlDbType.DateTime).Value = dr["StartDate"];
                sda.SelectCommand.Parameters.Add("@ED", SqlDbType.DateTime).Value = dr["EndDate"];
                strSubWhere1 += strWhere2;
                strSubWhere2 += strWhere2;
                strSubWhere3 += strWhere2;
                strSubWhere4 += strWhere2;
                strSubWhere5 += strWhere2;
                if (dr["Vendor_Contract"] != DBNull.Value ||
                    dr["Media_Credit"] != DBNull.Value ||
                    dr["Rebate"] != DBNull.Value ||
                    dr["EPD"] != DBNull.Value ||
                    dr["SAC"] != DBNull.Value)
                {
                    string strSubSelect1 = @"select distinct v.Media_Vendor_ID 
                    from Media_Vendor as v";
                    string strSubSelect2 = @" left outer join (
	                select * 
	                from ";
                    string strSubSelect3 = @") as vc 
                    on v.Media_Vendor_ID = vc.Media_Vendor_ID";
                    string strSubSelect4 = @") as epd 
                    on v.Media_Vendor_ID = epd.Media_Vendor_ID";
                    string strSubSelect5 = @") as mc 
                    on v.Media_Vendor_ID = mc.Media_Vendor_ID";
                    string strSubSelect6 = @") as rb 
                    on v.Media_Vendor_ID = rb.Media_Vendor_ID";
                    string strSubSelect7 = @") as sac 
                    on v.Media_Vendor_ID = sac.Media_Vendor_ID ";
                    string strSubSubWhere1 = strSubWhere1;
                    string strSubSubWhere2 = strSubWhere2;
                    string strSubSubWhere3 = strSubWhere3;
                    string strSubSubWhere4 = strSubWhere4;
                    string strSubSubWhere5 = strSubWhere5;
                    string strSubWhere = strWhere;
                    if (dr["Vendor_Contract"] != DBNull.Value)
                    {
                        strSubSubWhere1 += strWhere3;
                        strSubWhere += " and vc.Type = @VC";
                        strSubWhere1 += strWhere3;
                        strWhereRG1 += " and vp.Type = @VC";
                        sda.SelectCommand.Parameters.Add("@VC", SqlDbType.VarChar).Value = dr["Vendor_Contract"];
                    }
                    if (dr["EPD"] != DBNull.Value)
                    {
                        strSubSubWhere2 += strWhere4;
                        strSubWhere += " and epd.Type = @EPD";
                        strSubWhere2 += strWhere4;
                        strWhereRG2 += " and vp.Type = @EPD";
                        sda.SelectCommand.Parameters.Add("@EPD", SqlDbType.VarChar).Value = dr["EPD"];
                    }
                    if (dr["Media_Credit"] != DBNull.Value)
                    {
                        strSubSubWhere3 += strWhere5;
                        strSubWhere += " and mc.Type = @MC";
                        strSubWhere3 += strWhere5;
                        strWhereRG3 += " and vp.Type = @MC";
                        sda.SelectCommand.Parameters.Add("@MC", SqlDbType.VarChar).Value = dr["Media_Credit"];
                    }
                    if (dr["Rebate"] != DBNull.Value)
                    {
                        strSubSubWhere4 += strWhere6;
                        strSubWhere += " and rb.Type = @RB";
                        strSubWhere4 += strWhere6;
                        strWhereRG4 += " and vp.Type = @RB";
                        sda.SelectCommand.Parameters.Add("@RB", SqlDbType.VarChar).Value = dr["Rebate"];
                    }
                    if (dr["SAC"] != DBNull.Value)
                    {
                        strSubSubWhere5 += strWhere7;
                        strSubWhere += " and sac.Type = @SAC";
                        strSubWhere5 += strWhere7;
                        strWhereRG5 += " and vp.Type = @SAC";
                        sda.SelectCommand.Parameters.Add("@SAC", SqlDbType.VarChar).Value = dr["SAC"];
                    }
                    string strSubSQL = strSubSelect1 +
                                       strSubSelect2 + strTable1 + strSubSubWhere1 + strSubSelect3 +
                                       strSubSelect2 + strTable2 + strSubSubWhere2 + strSubSelect4 +
                                       strSubSelect2 + strTable3 + strSubSubWhere3 + strSubSelect5 +
                                       strSubSelect2 + strTable4 + strSubSubWhere4 + strSubSelect6 +
                                       strSubSelect2 + strTable5 + strSubSubWhere5 + strSubSelect7 +
                                       strSubWhere;
                    string strWhereSubQuery = " and v.Media_Vendor_ID in (" + strSubSQL + ")";
                    strWhereRG1 += strWhereSubQuery;
                    strWhereRG2 += strWhereSubQuery;
                    strWhereRG3 += strWhereSubQuery;
                    strWhereRG4 += strWhereSubQuery;
                    strWhereRG5 += strWhereSubQuery;
                }
            }
            strSQL1 = strSelect1 +
                      "[" + strField1 + "]" +
                      strSelect2 +
                      strTable1 +
                      strSubWhere1 +
                      strSelect3 +
                      strWhere +
                      strWhereRG1 +
                      strOrder;
            strSQL2 = strSelect1 +
                      strField2 +
                      strSelect2 +
                      strTable2 +
                      strSubWhere2 +
                      strSelect3 +
                      strWhere +
                      strWhereRG2 +
                      strOrder;
            strSQL3 = strSelect1 +
                      "[" + strField3 + "]" +
                      strSelect2 +
                      strTable3 +
                      strSubWhere3 +
                      strSelect3 +
                      strWhere +
                      strWhereRG3 +
                      strOrder;
            strSQL4 = strSelect1 +
                      strField4 +
                      strSelect2 +
                      strTable4 +
                      strSubWhere4 +
                      strSelect3 +
                      strWhere +
                      strWhereRG4 +
                      strOrder;
            strSQL5 = strSelect1 +
                      strField5 +
                      strSelect2 +
                      strTable5 +
                      strSubWhere5 +
                      strSelect3 +
                      strWhere +
                      strWhereRG5 +
                      strOrder;
            //strSQL = strSQL1 + " " + strSQL2 + " " + strSQL3 + " " + strSQL4 + " " + strSQL5;
            strSQL = strSQL1 + " " + strSQL2 + " " + strSQL4 + " " + strSQL5;
            sda.SelectCommand.CommandText = strSQL;
            sda.Fill(ds);
            return ds;
        }

        /*public DataTable SelectVendorScreenReport(DataRow dr)
        {
            DataTable dt = new DataTable();
            string strSQL = "";
            string strSelect1 = "select distinct 'x' as X_Col, ";
            string strSelect2 = "select distinct TBVendor.Row_No, ";
            string strSelect3 = @"TBVendor.Media_Vendor_ID as [Vendor Code], 
			TBVendor.Short_Name as [Vendor Name], 
            TBVendor.Master_Group as [Master Vendor], 
			TBVendor.Status_Show as Status, 
			TBVendor.User_ID as [Modify By], 
			TBVendor.Modify_Date as [Modify Date], ";
            string strSelect4 = @"TBVendor.Vendor_Contract as [Sign Vendor Contract], 
            TBVendor.EPD, 
            TBVendor.Media_Credit as [Media Credit], 
            TBVendor.Rebate, 
            TBVendor.SAC ";
            string strSelect5 = @"TBVendor.Vendor_Contract as [Sign Vendor Contract], 
            case TBVendor.Row_Vendor_Contract 
			when 1 
			then TBVendor.Vendor_Contract_Period 
			else '' 
			end as [Sign Vendor Contract Period], 
            TBVendor.EPD, 
            case TBVendor.Row_EPD 
			when 1 
			then TBVendor.EPD_Period 
			else '' 
			end as [EPD Period], 
            TBVendor.Media_Credit as [Media Credit], 
            case TBVendor.Row_Media_Credit 
			when 1 
			then TBVendor.Media_Credit_Period 
			else '' 
			end as [Media Credit Period], 
            TBVendor.Rebate, 
            case TBVendor.Row_Rebate 
			when 1 
			then TBVendor.Rebate_Period 
			else '' 
			end as [Rebate Period], 
            TBVendor.SAC, 
            case TBVendor.Row_SAC 
			when 1 
			then TBVendor.SAC_Period 
			else '' 
			end as [SAC Period] ";
            string strSelect6 = @"from (
            select row_number() 
			over(
				order by Short_Name, 
			    Media_Vendor_ID, 
			    Vendor_Contract_Period, 
			    Row_Vendor_Contract, 
			    EPD_Period, 
			    Row_EPD, 
			    Media_Credit_Period, 
			    Row_Media_Credit, 
			    Rebate_Period, 
			    Row_Rebate, 
			    SAC_Period, 
			    Row_SAC
			) as Row_No, 
			* 
			from (
			    select v.Media_Vendor_ID, 
			    v.Short_Name, 
                v.Master_Group, 
			    v.User_ID, 
			    v.Modify_Date, 
			    v.InActive, 
			    vc.Start_Date as Vendor_Contract_SD, 
			    vc.End_Date as Vendor_Contract_ED, 
                mc.Start_Date as Media_Credit_SD, 
			    mc.End_Date as Media_Credit_ED, 
			    rb.Start_Date as Rebate_SD, 
			    rb.End_Date as Rebate_ED, 
			    epd.Start_Date as EPD_SD, 
			    epd.End_Date as EPD_ED, 
			    sac.Start_Date as SAC_SD, 
			    sac.End_Date as SAC_ED, 
			    case 
			    when v.InActive = 0 
			    then 'Active' 
			    else 'Inactive' 
			    end as Status_Show, 
			    case 
	            when isnull(vc.Media_Vendor_ID, '') = '' 
	            then '' 
	            else 'Yes' 
	            end as Vendor_Contract, 
			    isnull(
			    convert(varchar(50), vc.Start_Date, 6) + 
		        ' to ' + 
		        convert(varchar(50), vc.End_Date, 6), 
			    '') as Vendor_Contract_Period, 
			    row_number() 
			    over(
				    partition by v.Media_Vendor_ID, 
				    vc.Start_Date, 
				    vc.End_Date 
				    order by v.Media_Vendor_ID, 
				    vc.Start_Date, 
				    vc.End_Date, 
				    epd.Start_Date, 
				    epd.End_Date, 
				    mc.Start_Date, 
				    mc.End_Date, 
				    rb.Start_Date, 
				    rb.End_Date, 
				    sac.Start_Date, 
				    sac.End_Date
			    ) as Row_Vendor_Contract, 
			    case 
			    when vc.Start_Date is null 
			    then 1 
			    else (
				    select 1 
				    from Media_Vendor_Contract 
				    where Media_Vendor_ID = vc.Media_Vendor_ID 
				    having max(End_Date) = vc.End_Date
			    ) end as Last_Vendor_Contract, 
                case 
	            when isnull(mc.Media_Vendor_ID, '') = '' 
	            then '' 
	            else 'Yes' 
	            end as Media_Credit, 
			    isnull(
			    convert(varchar(50), mc.Start_Date, 6) + 
		        ' to ' + 
		        convert(varchar(50), mc.End_Date, 6), 
			    '') as Media_Credit_Period, 
                row_number() 
                over(
	                partition by v.Media_Vendor_ID, 
	                vc.Start_Date, 
	                vc.End_Date, 
	                epd.Start_Date, 
	                epd.End_Date, 
	                mc.Start_Date, 
	                mc.End_Date 
	                order by v.Media_Vendor_ID, 
	                vc.Start_Date, 
	                vc.End_Date, 
	                epd.Start_Date, 
	                epd.End_Date, 
	                mc.Start_Date, 
	                mc.End_Date, 
	                rb.Start_Date, 
	                rb.End_Date, 
	                sac.Start_Date, 
	                sac.End_Date
                ) as Row_Media_Credit, 
			    case 
			    when mc.Start_Date is null 
			    then 1 
			    else (
				    select 1 
				    from Media_Vendor_Media_Credit 
				    where Media_Vendor_ID = mc.Media_Vendor_ID 
				    having max(End_Date) = mc.End_Date
			    ) end as Last_Media_Credit, 
			    case 
	            when isnull(rb.Media_Vendor_ID, '') = '' 
	            then '' 
	            else 'Yes' 
	            end as Rebate, 
			    isnull(
			    convert(varchar(50), rb.Start_Date, 6) + 
		        ' to ' + 
		        convert(varchar(50), rb.End_Date, 6), 
			    '') as Rebate_Period, 
                row_number() 
                over(
	                partition by v.Media_Vendor_ID, 
	                vc.Start_Date, 
	                vc.End_Date, 
	                epd.Start_Date, 
	                epd.End_Date, 
	                mc.Start_Date, 
	                mc.End_Date, 
	                rb.Start_Date, 
	                rb.End_Date 
	                order by v.Media_Vendor_ID, 
	                vc.Start_Date, 
	                vc.End_Date, 
	                epd.Start_Date, 
	                epd.End_Date, 
	                mc.Start_Date, 
	                mc.End_Date, 
	                rb.Start_Date, 
	                rb.End_Date, 
	                sac.Start_Date, 
	                sac.End_Date
                ) as Row_Rebate, 
			    case 
			    when rb.Start_Date is null 
			    then 1 
			    else (
				    select 1 
				    from Media_Vendor_Rebate 
				    where Media_Vendor_ID = rb.Media_Vendor_ID 
				    having max(End_Date) = rb.End_Date
			    ) end as Last_Rebate, 
	            case 
	            when isnull(epd.Media_Vendor_ID, '') = '' 
	            then '' 
	            else 'Yes' 
	            end as EPD, 
			    isnull(
			    convert(varchar(50), epd.Start_Date, 6) + 
		        ' to ' + 
		        convert(varchar(50), epd.End_Date, 6), 
			    '') as EPD_Period, 
                row_number() 
                over(
				    partition by v.Media_Vendor_ID, 
				    vc.Start_Date, 
				    vc.End_Date, 
				    epd.Start_Date, 
				    epd.End_Date 
				    order by v.Media_Vendor_ID, 
				    vc.Start_Date, 
				    vc.End_Date, 
				    epd.Start_Date, 
				    epd.End_Date, 
				    mc.Start_Date, 
				    mc.End_Date, 
				    rb.Start_Date, 
				    rb.End_Date, 
				    sac.Start_Date, 
				    sac.End_Date
			    ) as Row_EPD, 
			    case 
			    when epd.Start_Date is null 
			    then 1 
			    else (
				    select 1 
				    from Media_Vendor_EPD 
				    where Media_Vendor_ID = epd.Media_Vendor_ID 
				    having max(End_Date) = epd.End_Date
			    ) end as Last_EPD, 
	            case 
	            when isnull(sac.Media_Vendor_ID, '') = '' 
	            then '' 
	            else 'Yes' 
	            end as SAC, 
			    isnull(
			    convert(varchar(50), sac.Start_Date, 6) + 
		        ' to ' + 
		        convert(varchar(50), sac.End_Date, 6), 
			    '') as SAC_Period, 
                row_number() 
                over(
				    partition by v.Media_Vendor_ID, 
				    vc.Start_Date, 
				    vc.End_Date, 
				    epd.Start_Date, 
				    epd.End_Date, 
				    mc.Start_Date, 
				    mc.End_Date, 
				    rb.Start_Date, 
				    rb.End_Date, 
				    sac.Start_Date, 
				    sac.End_Date 
				    order by v.Media_Vendor_ID, 
				    vc.Start_Date, 
				    vc.End_Date, 
				    epd.Start_Date, 
				    epd.End_Date, 
				    mc.Start_Date, 
				    mc.End_Date, 
				    rb.Start_Date, 
				    rb.End_Date, 
				    sac.Start_Date, 
				    sac.End_Date
			    ) as Row_SAC, 
			    case 
			    when sac.Start_Date is null 
			    then 1 
			    else (
				    select 1 
				    from Media_Vendor_SAC 
				    where Media_Vendor_ID = sac.Media_Vendor_ID 
				    having max(End_Date) = sac.End_Date
			    ) end as Last_SAC 
			    from Media_Vendor as v 
			    left join (
                    select * 
                    from Media_Vendor_Contract 
                    where 1 = 1";
            string strSelect7 = @") as vc 
			on v.Media_Vendor_ID = vc.Media_Vendor_ID 
			left join (
                select * 
                from Media_Vendor_Media_Credit 
                where 1 = 1";
            string strSelect8 = @") as mc 
			on v.Media_Vendor_ID = mc.Media_Vendor_ID 
			left join (
                select * 
                from Media_Vendor_Rebate 
                where 1 = 1";
            string strSelect9 = @") as rb 
	        on v.Media_Vendor_ID = rb.Media_Vendor_ID 
	        left join (
                select * 
                from Media_Vendor_EPD 
                where 1 = 1";
            string strSelect10 = @") as epd 
	        on v.Media_Vendor_ID = epd.Media_Vendor_ID 
	        left join (
                select * 
                from Media_Vendor_SAC 
                where 1 = 1";
            string strSelect11 = @") as sac 
	                on v.Media_Vendor_ID = sac.Media_Vendor_ID
                ) as TBV
			) as TBVendor 
			where 1 = 1";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            if (dr["Media_Vendor_Code"] != DBNull.Value)
            {
                strSelect11 += " and TBVendor.Media_Vendor_ID like '%' + @Media_Vendor_Code + '%'";
                sda.SelectCommand.Parameters.Add("@Media_Vendor_Code", SqlDbType.VarChar).Value = dr["Media_Vendor_Code"];
            }
            if (dr["Media_Vendor_ID"] != DBNull.Value)
            {
                strSelect11 += " and TBVendor.Media_Vendor_ID = @Media_Vendor_ID";
                sda.SelectCommand.Parameters.Add("@Media_Vendor_ID", SqlDbType.VarChar).Value = dr["Media_Vendor_ID"];
            }
            if (dr["Master_Group"] != DBNull.Value)
            {
                strSelect11 += " and TBVendor.Master_Group = @Master_Group";
                sda.SelectCommand.Parameters.Add("@Master_Group", SqlDbType.VarChar).Value = dr["Master_Group"];
            }
            if (dr["Short_Name"] != DBNull.Value)
            {
                strSelect11 += " and TBVendor.Short_Name like '%' + @Short_Name + '%'";
                sda.SelectCommand.Parameters.Add("@Short_Name", SqlDbType.VarChar).Value = dr["Short_Name"];
            }
            if (dr["InActive"] != DBNull.Value)
            {
                strSelect11 += " and TBVendor.InActive = @InActive";
                sda.SelectCommand.Parameters.Add("@InActive", SqlDbType.VarChar).Value = dr["InActive"];
            }
            if (dr["Condition"] == DBNull.Value)
            {
                strSelect11 += @" and TBVendor.Last_Vendor_Contract = 1 
                and TBVendor.Last_Media_Credit = 1 
                and TBVendor.Last_Rebate = 1 
                and TBVendor.Last_EPD = 1 
                and TBVendor.Last_SAC = 1";
            }
            else
            {
                if (dr["Vendor_Contract"] == DBNull.Value)
                {
                    strSelect6 += @" and (
                        (
                            Start_Date >= @VCSD 
                            and Start_Date < @VCED
                        ) 
                        or (
                            End_Date > @VCSD 
                            and End_Date <= @VCED
                        )
                    )";
                    sda.SelectCommand.Parameters.Add("@VCSD", SqlDbType.DateTime).Value = dr["StartDate"];
                    sda.SelectCommand.Parameters.Add("@VCED", SqlDbType.DateTime).Value = dr["EndDate"];
                }
                else
                {
                    if (dr["Vendor_Contract"].ToString() == "Yes")
                    {
                        strSelect11 += @" and TBVendor.Vendor_Contract = @Vendor_Contract 
                        and (
                            (
                                TBVendor.Vendor_Contract_SD >= @VCSD 
                                and TBVendor.Vendor_Contract_SD < @VCED
                            ) 
                            or (
                                TBVendor.Vendor_Contract_ED > @VCSD 
                                and TBVendor.Vendor_Contract_ED <= @VCED
                            )
                        )";
                        sda.SelectCommand.Parameters.Add("@Vendor_Contract", SqlDbType.VarChar).Value = dr["Vendor_Contract"];
                        sda.SelectCommand.Parameters.Add("@VCSD", SqlDbType.DateTime).Value = dr["StartDate"];
                        sda.SelectCommand.Parameters.Add("@VCED", SqlDbType.DateTime).Value = dr["EndDate"];
                    }
                    else
                    {
                        strSelect11 += " and TBVendor.Vendor_Contract = ''";
                    }
                }
                if (dr["Media_Credit"] == DBNull.Value)
                {
                    strSelect7 += @" and (
                        (
                            Start_Date >= @MCSD 
                            and Start_Date < @MCED
                        ) 
                        or (
                            End_Date > @MCSD 
                            and End_Date <= @MCED
                        )
                    )";
                    sda.SelectCommand.Parameters.Add("@MCSD", SqlDbType.DateTime).Value = dr["StartDate"];
                    sda.SelectCommand.Parameters.Add("@MCED", SqlDbType.DateTime).Value = dr["EndDate"];
                }
                else
                {
                    if (dr["Media_Credit"].ToString() == "Yes")
                    {
                        strSelect11 += @" and TBVendor.Media_Credit = @Media_Credit 
                        and (
                            (
                                TBVendor.Media_Credit_SD >= @MCSD 
                                and TBVendor.Media_Credit_SD < @MCED
                            ) 
                            or (
                                TBVendor.Media_Credit_ED > @MCSD 
                                and TBVendor.Media_Credit_ED <= @MCED
                            )
                        )";
                        sda.SelectCommand.Parameters.Add("@Media_Credit", SqlDbType.VarChar).Value = dr["Media_Credit"];
                        sda.SelectCommand.Parameters.Add("@MCSD", SqlDbType.DateTime).Value = dr["StartDate"];
                        sda.SelectCommand.Parameters.Add("@MCED", SqlDbType.DateTime).Value = dr["EndDate"];
                    }
                    else
                    {
                        strSelect11 += " and TBVendor.Media_Credit = ''";
                    }
                }
                if (dr["Rebate"] == DBNull.Value)
                {
                    strSelect8 += @" and (
                        (
                            Start_Date >= @RBSD 
                            and Start_Date < @RBED
                        ) 
                        or (
                            End_Date > @RBSD 
                            and End_Date <= @RBED
                        )
                    )";
                    sda.SelectCommand.Parameters.Add("@RBSD", SqlDbType.DateTime).Value = dr["StartDate"];
                    sda.SelectCommand.Parameters.Add("@RBED", SqlDbType.DateTime).Value = dr["EndDate"];
                }
                else
                {
                    if (dr["Rebate"].ToString() == "Yes")
                    {
                        strSelect11 += @" and TBVendor.Rebate = @Rebate 
                        and (
                            (
                                TBVendor.Rebate_SD >= @RBSD 
                                and TBVendor.Rebate_SD < @RBED
                            ) 
                            or (
                                TBVendor.Rebate_ED > @RBSD 
                                and TBVendor.Rebate_ED <= @RBED
                            )
                        )";
                        sda.SelectCommand.Parameters.Add("@Rebate", SqlDbType.VarChar).Value = dr["Rebate"];
                        sda.SelectCommand.Parameters.Add("@RBSD", SqlDbType.DateTime).Value = dr["StartDate"];
                        sda.SelectCommand.Parameters.Add("@RBED", SqlDbType.DateTime).Value = dr["EndDate"];
                    }
                    else
                    {
                        strSelect11 += " and TBVendor.Rebate = ''";
                    }
                }
                if (dr["EPD"] == DBNull.Value)
                {
                    strSelect9 += @" and (
                        (
                            Start_Date >= @EPDSD 
                            and Start_Date < @EPDED
                        ) 
                        or (
                            End_Date > @EPDSD 
                            and End_Date <= @EPDED
                        )
                    )";
                    sda.SelectCommand.Parameters.Add("@EPDSD", SqlDbType.DateTime).Value = dr["StartDate"];
                    sda.SelectCommand.Parameters.Add("@EPDED", SqlDbType.DateTime).Value = dr["EndDate"];
                }
                else
                {
                    if (dr["EPD"].ToString() == "Yes")
                    {
                        strSelect11 += @" and TBVendor.EPD = @EPD 
                        and (
                            (
                                TBVendor.EPD_SD >= @EPDSD 
                                and TBVendor.EPD_SD < @EPDED
                            ) 
                            or (
                                TBVendor.EPD_ED > @EPDSD 
                                and TBVendor.EPD_ED <= @EPDED
                            )
                        )";
                        sda.SelectCommand.Parameters.Add("@EPD", SqlDbType.VarChar).Value = dr["EPD"];
                        sda.SelectCommand.Parameters.Add("@EPDSD", SqlDbType.DateTime).Value = dr["StartDate"];
                        sda.SelectCommand.Parameters.Add("@EPDED", SqlDbType.DateTime).Value = dr["EndDate"];
                    }
                    else
                    {
                        strSelect11 += " and TBVendor.EPD = ''";
                    }
                }
                if (dr["SAC"] == DBNull.Value)
                {
                    strSelect10 += @" and (
                        (
                            Start_Date >= @SACSD 
                            and Start_Date < @SACED
                        ) 
                        or (
                            End_Date > @SACSD 
                            and End_Date <= @SACED
                        )
                    )";
                    sda.SelectCommand.Parameters.Add("@SACSD", SqlDbType.DateTime).Value = dr["StartDate"];
                    sda.SelectCommand.Parameters.Add("@SACED", SqlDbType.DateTime).Value = dr["EndDate"];
                }
                else
                {
                    if (dr["SAC"].ToString() == "Yes")
                    {
                        strSelect11 += @" and TBVendor.SAC = @SAC 
                        and (
                            (
                                TBVendor.SAC_SD >= @SACSD 
                                and TBVendor.SAC_SD < @SACED
                            ) 
                            or (
                                TBVendor.SAC_ED > @SACSD 
                                and TBVendor.SAC_ED <= @SACED
                            )
                        )";
                        sda.SelectCommand.Parameters.Add("@SAC", SqlDbType.VarChar).Value = dr["SAC"];
                        sda.SelectCommand.Parameters.Add("@SACSD", SqlDbType.DateTime).Value = dr["StartDate"];
                        sda.SelectCommand.Parameters.Add("@SACED", SqlDbType.DateTime).Value = dr["EndDate"];
                    }
                    else
                    {
                        strSelect11 += " and TBVendor.SAC = ''";
                    }
                }
            }
            if (dr["Display"] == DBNull.Value)
            {
                strSQL = strSelect1 +
                         strSelect3 +
                         strSelect4 +
                         strSelect6 +
                         strSelect7 +
                         strSelect8 +
                         strSelect9 +
                         strSelect10 +
                         strSelect11;
                strSQL += @" order by TBVendor.Short_Name, 
                TBVendor.Media_Vendor_ID";
            }
            else
            {
                strSQL = strSelect2 +
                         strSelect3 +
                         strSelect5 +
                         strSelect6 +
                         strSelect7 +
                         strSelect8 +
                         strSelect9 +
                         strSelect10 +
                         strSelect11;
            }
            sda.SelectCommand.CommandText = strSQL;
            sda.Fill(dt);
            return dt;
        }*/

        public DataTable SelectMediaSubType(DataRow dr)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select *, 
            case when IsActive = 1 then 'Active' else 'Inactive' end as status_show 
            from Media_Sub_Type 
            where 1 = 1";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            if (dr["MediaSubTypeCode"] != DBNull.Value)
            {
                strSQL += @" and Media_Sub_Type like '%'+@MediaSubTypeCode+'%'";
                sda.SelectCommand.Parameters.Add("@MediaSubTypeCode", SqlDbType.VarChar).Value = dr["MediaSubTypeCode"];
            }
            if (dr["Media_Sub_Type"] != DBNull.Value)
            {
                strSQL += @" and Media_Sub_Type = @Media_Sub_Type";
                sda.SelectCommand.Parameters.Add("@Media_Sub_Type", SqlDbType.VarChar).Value = dr["Media_Sub_Type"];
            }
            if (dr["Short_Name"] != DBNull.Value)
            {
                strSQL += @" and Short_Name like '%'+@Short_Name+'%'";
                sda.SelectCommand.Parameters.Add("@Short_Name", SqlDbType.VarChar).Value = dr["Short_Name"];
            }
            if (dr["Media_Type"] != DBNull.Value)
            {
                strSQL += @" and Media_Type = @Media_Type";
                sda.SelectCommand.Parameters.Add("@Media_Type", SqlDbType.VarChar).Value = dr["Media_Type"];
            }
            if (dr["isActive"] != DBNull.Value)
            {
                strSQL += @" and isActive = @isActive";
                sda.SelectCommand.Parameters.Add("@isActive", SqlDbType.VarChar).Value = dr["isActive"];
            }
            strSQL += @" order by Media_Type, Short_Name";
            sda.SelectCommand.CommandText = strSQL;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectMediaType(DataRow dr)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select *, 
            case when IsActive = 1 then 'Active' else 'Inactive' end as status_show 
            from Media_Type 
            where 1 = 1";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            if (dr["MediaTypeCode"] != DBNull.Value)
            {
                strSQL += @" and Media_Type like '%'+@MediaTypeCode+'%'";
                sda.SelectCommand.Parameters.Add("@MediaTypeCode", SqlDbType.VarChar).Value = dr["MediaTypeCode"];
            }
            if (dr["MediaType_ID"] != DBNull.Value)
            {
                strSQL += @" and Media_Type = @Media_Type";
                sda.SelectCommand.Parameters.Add("@Media_Type", SqlDbType.VarChar).Value = dr["MediaType_ID"];
            }
            if (dr["Master_Media_Type"] != DBNull.Value)
            {
                strSQL += @" and Master_Media_Type = @Master_Media_Type";
                sda.SelectCommand.Parameters.Add("@Master_Media_Type", SqlDbType.VarChar).Value = dr["Master_Media_Type"];
            }
            if (dr["Short_Name"] != DBNull.Value)
            {
                strSQL += @" and Short_Name like '%'+@Short_Name+'%'";
                sda.SelectCommand.Parameters.Add("@Short_Name", SqlDbType.VarChar).Value = dr["Short_Name"];
            }
            if (dr["isActive"] != DBNull.Value)
            {
                strSQL += @" and isActive = @isActive";
                sda.SelectCommand.Parameters.Add("@isActive", SqlDbType.VarChar).Value = dr["isActive"];
            }
            strSQL += @" order by Media_Type, Short_Name";
            sda.SelectCommand.CommandText = strSQL;
            sda.Fill(dt);
            return dt;
        }
        public DataTable SelectMediaType(bool bActiveOnly)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select *
            from Media_Type  
            where 1 = 1
            ";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);

            if (bActiveOnly == true)
            {
                strSQL += @" and isActive = @isActive";
                sda.SelectCommand.Parameters.Add("@isActive", SqlDbType.Bit).Value = bActiveOnly;
            }
            strSQL += @" order by Media_Type, Short_Name";

            sda.SelectCommand.CommandText = strSQL;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectMediaSubType(bool bActiveOnly)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select *
            from Media_Sub_Type  
            where 1 = 1
            ";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);

            if (bActiveOnly == true)
            {
                strSQL += @" and isActive = @isActive";
                sda.SelectCommand.Parameters.Add("@isActive", SqlDbType.Bit).Value = bActiveOnly;
            }
            strSQL += @" order by Media_Type, Short_Name";

            sda.SelectCommand.CommandText = strSQL;
            sda.Fill(dt);
            return dt;
        }
        public DataTable SelectMediaSubType_DisplayMediaType(bool bActiveOnly)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select 
mst.Media_Type
,mst.Media_Sub_Type
,isnull(mt.Short_Name + ' : ','') + mst.Short_Name Short_Name
,mst.User_ID
,mst.Modify_Date
,mst.Adept_Export_Mapping
,mst.Show_BB
,mst.Optin_Checked
,mst.Adept_MergewithMedia
,mst.AdeptExport_Prefix
,mst.BillingType_Revenue
,mst.Forecast_Input
,mst.FeeLocked_SP
,mst.OptIn_OptOut_Footnotes
,mst.isActive
,mst.InactiveDate
,mst.Service_Group
,mst.BusinessDefinition
,mst.ConvertToOnline
,mst.Master_Media_Sub_Type
,mst.Media_Sub_Type_Group
,mst.Billing_Group
,mst.Media_Sub_Type_Mapping_CoreM_New
from Media_Sub_Type  mst
left join media_type mt
	on mt.Media_Type = mst.Media_Type
where 1 = 1
            ";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);

            if (bActiveOnly == true)
            {
                strSQL += @" and mst.isActive = @isActive";
                sda.SelectCommand.Parameters.Add("@isActive", SqlDbType.Bit).Value = bActiveOnly;
            }
            strSQL += @" order by mt.Short_Name, mst.Short_Name";

            sda.SelectCommand.CommandText = strSQL;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectAdeptMediaType(DataRow dr)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select *, 
            case when Billing_Type_Revenue = 'True' then 'Yes' else 'No' end as Type_Revenue, 
            case when IsActive = 1 then 'Active' else 'Inactive' end as status_show 
            from Adept_Media_Type 
            where 1 = 1";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            if (dr["AdeptMediaTypeCode"] != DBNull.Value)
            {
                strSQL += @" and Adept_Media_Type like '%'+@AdeptMediaTypeCode+'%'";
                sda.SelectCommand.Parameters.Add("@AdeptMediaTypeCode", SqlDbType.VarChar).Value = dr["AdeptMediaTypeCode"];
            }
            if (dr["Adept_Media_Type"] != DBNull.Value)
            {
                strSQL += @" and Adept_Media_Type = @Adept_Media_Type";
                sda.SelectCommand.Parameters.Add("@Adept_Media_Type", SqlDbType.VarChar).Value = dr["Adept_Media_Type"];
            }
            if (dr["Adept_Media_Type_Name"] != DBNull.Value)
            {
                strSQL += @" and Adept_Media_Type_Name like '%'+@Adept_Media_Type_Name+'%'";
                sda.SelectCommand.Parameters.Add("@Adept_Media_Type_Name", SqlDbType.VarChar).Value = dr["Adept_Media_Type_Name"];
            }
            if (dr["IsActive"] != DBNull.Value)
            {
                strSQL += @" and IsActive = @IsActive";
                sda.SelectCommand.Parameters.Add("@IsActive", SqlDbType.VarChar).Value = dr["IsActive"];
            }
            strSQL += @" order by Adept_Media_Type, Adept_Media_Type_Name";
            sda.SelectCommand.CommandText = strSQL;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectMediaSubType(string strMedia_Sub_Type)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select MST.*, 
            MT.Short_Name as MediaTypeName, 
            MST_CoreM.Short_Name as MST_CoreM_Name 
            from Media_Sub_Type as MST 
            inner join Media_Type as MT 
            on MST.Media_Type = MT.Media_Type
            inner join Media_Sub_Type as MST_CoreM 
            on MST.Media_Sub_Type_Mapping_CoreM_New = MST_CoreM.Media_Sub_Type
            where MST.Media_Sub_Type = @Media_Sub_Type";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Media_Sub_Type", SqlDbType.VarChar).Value = strMedia_Sub_Type;
            sda.Fill(dt);
            return dt;
        }
        public DataTable SelectMediaType(string strMedia_Type)
        {
            DataTable dt = new DataTable();
            string strSQL = @"
select mt.*,mmt.Short_Name Master_Media_Type_Name 
from Media_Type as mt 
left join Media_Type as mmt 
on mmt.Media_Type = mt.Master_Media_Type
            where mt.Media_Type = @Media_Type";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Media_Type", SqlDbType.VarChar).Value = strMedia_Type;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectAdeptMediaType(string strAdeptMedia_Type)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select * 
            from Adept_Media_Type 
            where Adept_Media_Type = @Adept_Media_Type";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Adept_Media_Type", SqlDbType.VarChar).Value = strAdeptMedia_Type;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectServiceGroup()
        {
            DataTable dt = new DataTable();
            string strSQL = @"select * from ServiceGroup";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectBillingGroup()
        {
            DataTable dt = new DataTable();
            string strSQL = @"select distinct Billing_Group from Media_Sub_Type";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectMediaTypeGroup()
        {
            DataTable dt = new DataTable();
            string strSQL = @"select distinct Media_Type_Group from Media_Type";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.Fill(dt);
            return dt;
        }

        public bool SelectMediaSubTypeIsUsing(string strMedia_Sub_Type)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select top 1 1 
            from Buying_Brief 
            inner join Spot_Plan 
            on Buying_Brief.Buying_Brief_ID = Spot_Plan.Buying_Brief_ID 
            where Buying_Brief.Media_Sub_Type = @Media_Sub_Type 
            or Spot_Plan.Media_Sub_Type = @Media_Sub_Type";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 0;
            sda.SelectCommand.Parameters.Add("@Media_Sub_Type", SqlDbType.VarChar).Value = strMedia_Sub_Type;
            sda.Fill(dt);
            return dt.Rows.Count > 0 ? true : false;
        }

        public bool SelectMediaTypeIsUsing(string strMedia_Type)
        {
            DataSet ds = new DataSet();
            string strSQL = @"select top 1 1 
            from Buying_Brief with(nolock) 
            where Media_Type = @Media_Type

            select top 1 1 
            from Spot_Plan with(nolock) 
            where Media_Type = @Media_Type";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 0;
            sda.SelectCommand.Parameters.Add("@Media_Type", SqlDbType.VarChar).Value = strMedia_Type;
            sda.Fill(ds);
            return ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0 ? true : false;
        }

        public bool SelectAdeptMediaTypeIsUsing(string strAdeptMedia_Type)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select top 1 1 
            from Media 
            where Adept_Media_Type = @Adept_Media_Type";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 0;
            sda.SelectCommand.Parameters.Add("@Adept_Media_Type", SqlDbType.VarChar).Value = strAdeptMedia_Type;
            sda.Fill(dt);
            return dt.Rows.Count > 0 ? true : false;
        }

        public DataTable SelectBuyType(string strMediaType)
        {
            DataTable dt = new DataTable();
            string strSQL = "";
            if (strMediaType == "IT")
                strSQL = "GetBuyType";
            else
                strSQL = @"select * 
                from v_BuyType 
                where BuyTypeDisplay in ('By Agency - Monthly', 'By Client')";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            if (strMediaType == "IT")
                sda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectSpotType()
        {
            DataTable dt = new DataTable();
            string strSQL = "GetSpotType";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectMaterial(string strValue)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select bm.Material_Key,m.Material_ID,m.Short_Name Material_Name from buying_brief_market_material bm
            inner join material m
	        on m.Material_ID = bm.Material_ID 
            where bm.buying_brief_id = @buying_brieft_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@buying_brieft_ID", SqlDbType.VarChar).Value = strValue;
            sda.Fill(dt);
            return dt;
        }
        public DataTable SelectChannelFromSpotPlan(DateTime dtStartDate,DateTime dtEndDate )
        {
            DataTable dt = new DataTable();
            string strSQL = @"
SELECT distinct
	  1 chk
      ,m.Media_ID
	  ,m.Short_Name
  FROM spot_plan s with(nolock)
  INNER JOIN Media_Sub_Type mst
	on mst.Media_Sub_Type = s.Media_Sub_Type
  INNER join Media m on m.[Media_ID] = s.[Media_ID]
  where mst.Media_Type = 'TV'
	and len(version) = 1
    and s.Status >= 5 --0 draf 4 Approve 5 Execute 8--Actual
	--and @StartDate <= s.[Show_Date] 
	--and s.[Show_Date] <= @EndDate 
    and s.Show_Date between @StartDate and @EndDate
  order by m.Media_ID
";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@StartDate", SqlDbType.NVarChar).Value = dtStartDate.ToString("yyyyMMdd");
            sda.SelectCommand.Parameters.Add("@EndDate", SqlDbType.NVarChar).Value = dtEndDate.ToString("yyyyMMdd");
            sda.Fill(dt);
            return dt;
        }
        public DataTable SelectBrandFromSpotPlan(DateTime dtStartDate, DateTime dtEndDate)
        {
            DataTable dt = new DataTable();
            string strSQL = @"
 SELECT distinct
	  0 chk
      ,p.Brand_ID
	  ,br.Brand_Name
  FROM [dbo].[Buying_Brief] b
  left join [dbo].[Product] p on b.Product_ID = p.Product_ID and b.[Client_ID] = p.[Client_ID] 
  INNER join Brand br on b.[Client_ID] = br.[Client_ID] and p.Brand_ID = br.Brand_ID
  left join spot_plan s with(nolock) on b.Buying_Brief_ID = s.Buying_Brief_ID
  where b.Media_Type = 'TV'
	and len(version) = 1
	and Media_Vendor_ID = 'MGPME'
	--and @StartDate <= s.[Show_Date] 
	--and s.[Show_Date] <= @EndDate 
    and s.Status >= 5 --0 draf 4 Approve 5 Execute 8--Actual
    and s.Show_Date between @StartDate and @EndDate
  order by br.Brand_Name

";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = dtStartDate;
            sda.SelectCommand.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = dtEndDate;
            sda.Fill(dt);
            return dt;
        }
        public DataTable SelectProgramFromSpotPlan(DateTime dtStartDate, DateTime dtEndDate,string strList, string strFilterByMediaOrBrand)
        {
            DataTable dt = new DataTable();
            string strSQL = "";
            if (strFilterByMediaOrBrand == "Brand")
            {
                strSQL = $@"
 SELECT distinct
	  1 chk
      --,s.Program_Code
	  ,s.Program
	  ,LEFT(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
	  isnull(gpm.Program_Group,s.Program)
	  ,':',''),'\',''),'/',''),'?',''),'*',''),'[',''),']',''),31) Program_Group
	  ,s.Start_Time
  FROM [dbo].[Buying_Brief] b
  left join [dbo].[Product] p on b.Product_ID = p.Product_ID and b.[Client_ID] = p.[Client_ID] 
  INNER join Brand br on b.[Client_ID] = br.[Client_ID] and p.Brand_ID = br.Brand_ID
  left join spot_plan s with(nolock) on b.Buying_Brief_ID = s.Buying_Brief_ID
  LEFT JOIN GPM_Program_Mapping gpm on gpm.Program = s.Program
  where 
  	b.Media_Type IN ('TV','TS')
	and len(version) = 1
	and Media_Vendor_ID = 'MGPME'
	--and @StartDate <= s.[Show_Date] 
	--and s.[Show_Date] <= @EndDate 
    and s.Status >= 5 --0 draf 4 Approve 5 Execute 8--Actual
    and s.Show_Date between @StartDate and @EndDate
    and br.Brand_ID in ({strList})
  order by s.Program
";
            }
            else
            {
                strSQL = $@"
SELECT distinct
	1 chk
    --,s.Program_Code
	,s.Program
	,LEFT(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
	isnull(gpm.Program_Group,s.Program)
	,':',''),'\',''),'/',''),'?',''),'*',''),'[',''),']',''),31) Program_Group
	,s.Start_Time
FROM [dbo].[Buying_Brief] b
    left join spot_plan s with(nolock) on b.Buying_Brief_ID = s.Buying_Brief_ID
    LEFT JOIN GPM_Program_Mapping gpm on gpm.Program = s.Program
where 
    b.Media_Type IN ('TV','TS')
    and len(version) = 1
    --and @StartDate <= s.[Show_Date] 
    --and s.[Show_Date] <= @EndDate 
    and s.Status >= 5 --0 draf 4 Approve 5 Execute 8--Actual
    and s.Show_Date between @StartDate and @EndDate
    and Media_ID IN ({strList})
order by s.Program
";
            }

            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = dtStartDate;
            sda.SelectCommand.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = dtEndDate;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectMediaSubTypeBusinessDefinition(string strMedia_Type)
        {
            DataTable dt = new DataTable();
            string strSQL = @"dbo.MediaSubType_Business_Definition";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sda.SelectCommand.CommandTimeout = 0;
            sda.SelectCommand.Parameters.Add("@Media_Type", SqlDbType.VarChar).Value = strMedia_Type;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectIncedentTracking(string bbid)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select * 
            from Buying_Brief_Incident_Tracking 
            where Buying_Brief_ID = @Buying_Brief_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = bbid;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectMaxIncedentTracking(string bbid)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select replace(isnull(max(Incident_ID), '0'), @Buying_Brief_ID + '-', '') as Max_No 
            from Buying_Brief_Incident_Tracking 
            where Buying_Brief_ID = @Buying_Brief_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = bbid;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectIncedentTrackingHeader(string trackingID)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select * 
            from Buying_Brief_Incident_Tracking 
            where Incident_ID = @Incident_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Incident_ID", SqlDbType.VarChar).Value = trackingID;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectIncedentTrackingHistory(string trackingID)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select * 
            from Buying_Brief_Incident_Track_History 
            where Incident_ID = @Incident_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Incident_ID", SqlDbType.VarChar).Value = trackingID;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectNonParameter(string strSQL)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectAgency(string strAgency_ID)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select * from Agency where Agency_ID = @Agency_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Agency_ID", SqlDbType.VarChar).Value = strAgency_ID;
            sda.Fill(dt);
            return dt;
        }

        public DataTable CheckOptIn(string strVendor, string strMedia)
        {
            DataTable dt = new DataTable();
            string strSQL = @"dbo.CheckOptIn_VendorMedia";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sda.SelectCommand.CommandTimeout = 0;
            sda.SelectCommand.Parameters.Add("@Media_Vendor_ID", SqlDbType.VarChar).Value = strVendor;
            sda.SelectCommand.Parameters.Add("@Media_ID", SqlDbType.VarChar).Value = strMedia;
            sda.Fill(dt);
            return dt;
        }
        public bool CheckOptInSchedule(string strBB, string version, string strMedia)
        {
            DataTable dt = new DataTable();
            string strSQL = @"
select top 1 1
from spot_plan s
inner join  (
	SELECT v.Media_Vendor_ID,v.Media_ID
	FROM [dbo].[GroupProprietary] g
	INNER JOIN [dbo].[GroupProprietaryVendorMapping] vm on g.GroupProprietaryId = vm.GroupProprietaryId
	INNER JOIN [dbo].[GroupProprietaryVendor] v on vm.GroupProprietaryVendorId = v.GroupProprietaryVendorId
	WHERE g.ContractType in ('Opt-in Signed', 'Letter Signed')
) pro
on pro.Media_Vendor_ID = s.Media_Vendor_ID 
and pro.Media_ID = s.Media_ID 
and s.Media_ID in (" + strMedia + @") 
where s.Buying_Brief_ID = @Buying_Brief_ID and s.version = @Version
";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandType = CommandType.Text;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = version;
            sda.Fill(dt);
            return dt.Rows.Count > 0;
        }
        public DataTable CheckOptIn(string strVendor, string strMedia, string strClient)
        {
            DataTable dt = new DataTable();
            string strSQL = @"dbo.CheckOptIn_VendorMediaClient";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sda.SelectCommand.CommandTimeout = 0;
            sda.SelectCommand.Parameters.Add("@Media_Vendor_ID", SqlDbType.VarChar).Value = strVendor;
            sda.SelectCommand.Parameters.Add("@Media_ID", SqlDbType.VarChar).Value = strMedia;
            sda.SelectCommand.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = strClient;
            sda.Fill(dt);
            return dt;
        }
        public DataTable CheckOptIn_ContractType_Spotplan(string strVendor, string strMedia)
        {
            DataTable dt = new DataTable();
            string strSQL = @"dbo.CheckOptIn_ContractType_Spotplan";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sda.SelectCommand.CommandTimeout = 0;
            sda.SelectCommand.Parameters.Add("@Media_Vendor_ID", SqlDbType.VarChar).Value = strVendor;
            sda.SelectCommand.Parameters.Add("@Media_ID", SqlDbType.VarChar).Value = strMedia;
            sda.Fill(dt);
            return dt;
        }
        public DataTable CheckGPMVendor(string strVendor)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select media_vendor_id,short_name,gpm_vendor from media_vendor where gpm_vendor = 1 and media_vendor_id = @Vendor_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.Parameters.Add("@Vendor_ID", SqlDbType.VarChar).Value = strVendor;
            sda.Fill(dt);
            return dt;
        }
        public DataTable SelectOptInReport(DateTime startDate, DateTime endDate, int proprietaryGroup, string agency, string region, string status)
        {
            DataTable dt = new DataTable();
            string strSQL = @"dbo.Opt_In_Report";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sda.SelectCommand.CommandTimeout = 0;
            sda.SelectCommand.Parameters.Add("@PeriodFrom", SqlDbType.Date).Value = startDate;
            sda.SelectCommand.Parameters.Add("@PeriodTo", SqlDbType.Date).Value = endDate;
            sda.SelectCommand.Parameters.Add("@OptInGroup", SqlDbType.Int).Value = proprietaryGroup;
            sda.SelectCommand.Parameters.Add("@Agency", SqlDbType.VarChar).Value = agency;
            sda.SelectCommand.Parameters.Add("@Region", SqlDbType.VarChar).Value = region;
            sda.SelectCommand.Parameters.Add("@Status", SqlDbType.VarChar).Value = status;
            sda.Fill(dt);
            return dt;
        }

        public bool CheckDirectPaySchedule(string strBB, string version, string strMedia)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select top 1 1 
            from Spot_Plan as s 
            where s.Buying_Brief_ID = @Buying_Brief_ID 
            and s.version = @Version 
            and s.Media_ID in (" + strMedia + @") 
            and s.BuyTypeID = 'CL'";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandType = CommandType.Text;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = version;
            sda.Fill(dt);
            return dt.Rows.Count > 0;
        }

        //Modified by Chaiwat.i 10/03/2023 TFS 158558 [T2] : LINE's remark has been disappear when print online schedule on C#
        public bool CheckLINEVendor(string strBB, string version)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select top 1 1 
            from Spot_Plan as s 
            where s.Buying_Brief_ID = @Buying_Brief_ID 
            and s.version = @Version
            and s.Media_Vendor_ID in ('ML010','MLIPU')";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandType = CommandType.Text;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = version;
            sda.Fill(dt);
            return dt.Rows.Count > 0;
        }

        public bool CheckDirectPayAdviceNote(string strBB, string version, string strMedia)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select top 1 1 
            from Spot_Plan_Edit as se 
            where se.Buying_Brief_ID = @Buying_Brief_ID 
            and se.version = @Version 
            and se.Media_ID in (" + strMedia + @") 
            and se.BuyTypeID = 'CL'";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandType = CommandType.Text;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = strBB;
            sda.SelectCommand.Parameters.Add("@Version", SqlDbType.VarChar).Value = version;
            sda.Fill(dt);
            return dt.Rows.Count > 0;
        }

        public DataTable SelectBuyingBriefMDF(string startPeriod, string endPeriod, string status, string exportFee, string username, string buyingBrief, string mediaType, string client, string agency, string office, string creativeAgency, string product)
        {
            DataTable dt = new DataTable();
            string strSQL = @"select * 
            from (
                select distinct B.Buying_Brief_ID, 
                B.Description as Campaign, 
                @Status as Status, 
                case B.Version_Approve when '4' then 'Approved' when '5' then 'Executing' when '8' then 'Actual' else 'Draft' end as Buying_Brief_Status, 
                @Export_Fee as AgencyCommission, 
                B.Agency_ID, 
                A.Short_Name as Agency, 
                B.Office_ID, 
                O.Short_Name as Office, 
                B.Client_ID, 
                C.Short_Name as Client, 
                B.Product_ID, 
                P.Short_Name as Product, 
                B.Creative_Agency_ID, 
                CA.Short_Name as CreativeAgency, 
                B.Media_Type, 
                M.Short_Name as MediaType 
                from Buying_Brief B 
                inner join Agency A 
                on B.Agency_ID = A.Agency_ID 
                inner join Office O 
                on B.Office_ID = O.Office_ID 
                inner join Client C 
                on B.Client_ID = C.Client_ID 
                inner join Product P 
                on B.Product_ID = P.Product_ID 
                inner join Creative_Agency CA 
                on B.Creative_Agency_ID = CA.Creative_Agency_ID 
                inner join Media_Type M 
                on B.Media_Type = M.Media_Type 
	            inner join Spot_Plan S 
	            on B.Buying_Brief_ID = S.Buying_Brief_ID 
	            where B.Media_Type <> 'OD' 
	            and S.Show_Date >= @Start_Date 
	            and S.Show_Date <= @End_Date 
	            union all 
	            select distinct B.Buying_Brief_ID, 
                B.Description as Campaign, 
                @Status as Status, 
                case B.Version_Approve when '4' then 'Approved' when '5' then 'Executing' when '8' then 'Actual' else 'Draft' end as Buying_Brief_Status, 
                @Export_Fee as AgencyCommission, 
                B.Agency_ID, 
                A.Short_Name as Agency, 
                B.Office_ID, 
                O.Short_Name as Office, 
                B.Client_ID, 
                C.Short_Name as Client, 
                B.Product_ID, 
                P.Short_Name as Product, 
                B.Creative_Agency_ID, 
                CA.Short_Name as CreativeAgency, 
                B.Media_Type, 
                M.Short_Name as MediaType 
                from Buying_Brief B 
                inner join Agency A 
                on B.Agency_ID = A.Agency_ID 
                inner join Office O 
                on B.Office_ID = O.Office_ID 
                inner join Client C 
                on B.Client_ID = C.Client_ID 
                inner join Product P 
                on B.Product_ID = P.Product_ID 
                inner join Creative_Agency CA 
                on B.Creative_Agency_ID = CA.Creative_Agency_ID 
                inner join Media_Type M 
                on B.Media_Type = M.Media_Type 
	            inner join spot_plan_payment SP 
	            on B.Buying_Brief_ID = SP.buying_brief_id 
	            where B.Media_Type = 'OD' 
	            and SP.show_date >= @Start_Date 
	            and SP.show_date <= @End_Date
            ) as TB 
            where case @BB_Status when 'Approved' then (case Buying_Brief_Status when 'Executing' then 'Approved' when 'Actual' then 'Approved' else '' end) else (case when Buying_Brief_Status in ('Executing', 'Actual') then Buying_Brief_Status else '' end) end = @BB_Status 
            and (
            Agency_ID in (
	            select Agency_ID 
	            from User_Client 
	            where User_ID = @Username
            ) 
            or Office_ID in (
	            select Office_ID 
	            from User_Client 
	            where User_ID = @Username
            ) 
            or Client_ID in (
	            select Client_ID 
	            from User_Client 
	            where User_ID = @Username
            )
            )";
            if (buyingBrief != "")
            {
                strSQL += " and Buying_Brief_ID like '%' + @Buying_Brief_ID + '%'";
            }
            if (mediaType != "")
            {
                strSQL += " and Media_Type = @Media_Type";
            }
            if (client != "")
            {
                strSQL += " and Client_ID = @Client_ID";
            }
            if (agency != "")
            {
                strSQL += " and Agency_ID = @Agency_ID";
            }
            if (office != "")
            {
                strSQL += " and Office_ID = @Office_ID";
            }
            if (creativeAgency != "")
            {
                strSQL += " and Creative_Agency_ID = @Creative_Agency_ID";
            }
            if (product != "")
            {
                strSQL += " and Product_ID = @Product_ID";
            }
            strSQL += @" order by Buying_Brief_ID";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandTimeout = 0;
            sda.SelectCommand.Parameters.Add("@Start_Date", SqlDbType.VarChar).Value = startPeriod;
            sda.SelectCommand.Parameters.Add("@End_Date", SqlDbType.VarChar).Value = endPeriod;
            sda.SelectCommand.Parameters.Add("@BB_Status", SqlDbType.VarChar).Value = status.Replace(" without advice note", "");
            sda.SelectCommand.Parameters.Add("@Status", SqlDbType.VarChar).Value = status.Replace("without advice note", "w/o AN");
            sda.SelectCommand.Parameters.Add("@Export_Fee", SqlDbType.VarChar).Value = exportFee;
            sda.SelectCommand.Parameters.Add("@Username", SqlDbType.VarChar).Value = username;
            if (buyingBrief != "")
            {
                sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = buyingBrief;
            }
            if (mediaType != "")
            {
                sda.SelectCommand.Parameters.Add("@Media_Type", SqlDbType.VarChar).Value = mediaType;
            }
            if (client != "")
            {
                sda.SelectCommand.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = client;
            }
            if (agency != "")
            {
                sda.SelectCommand.Parameters.Add("@Agency_ID", SqlDbType.VarChar).Value = agency;
            }
            if (office != "")
            {
                sda.SelectCommand.Parameters.Add("@Office_ID", SqlDbType.VarChar).Value = office;
            }
            if (creativeAgency != "")
            {
                sda.SelectCommand.Parameters.Add("@Creative_Agency_ID", SqlDbType.VarChar).Value = creativeAgency;
            }
            if (product != "")
            {
                sda.SelectCommand.Parameters.Add("@Product_ID", SqlDbType.VarChar).Value = product;
            }
            sda.Fill(dt);
            return dt;
        }

        public DataTable getCostMDF(string buyingBrief, string spotPlanStatus, DateTime dStartDate, DateTime dEndDate)
        {
            DataTable dt = new DataTable();
            string strSQL = @"dbo.MDF_GetCost";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sda.SelectCommand.CommandTimeout = 0;
            sda.SelectCommand.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = buyingBrief;
            sda.SelectCommand.Parameters.Add("@ApproveStatus", SqlDbType.VarChar).Value = spotPlanStatus;
            sda.SelectCommand.Parameters.Add("@StartDate", SqlDbType.Date).Value = dStartDate;
            sda.SelectCommand.Parameters.Add("@EndDate", SqlDbType.Date).Value = dEndDate;
            sda.Fill(dt);
            return dt;
        }

        public DataTable SelectMDFDetail(string BBXML, DateTime dStartDate, DateTime dEndDate, string spotPlanStatus, int exportFee)
        {
            DataTable dt = new DataTable();
            string strSQL = @"dbo.MDF_Export";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sda.SelectCommand.CommandTimeout = 0;
            sda.SelectCommand.Parameters.Add("@BuyingBrief", SqlDbType.Xml).Value = BBXML;
            sda.SelectCommand.Parameters.Add("@StartDate", SqlDbType.Date).Value = dStartDate;
            sda.SelectCommand.Parameters.Add("@EndDate", SqlDbType.Date).Value = dEndDate;
            sda.SelectCommand.Parameters.Add("@ApproveStatus", SqlDbType.VarChar).Value = spotPlanStatus;
            sda.SelectCommand.Parameters.Add("@ExportFee", SqlDbType.Int).Value = exportFee;
            sda.Fill(dt);
            return dt;
        }

        public void UpdateAdeptCodeBeforeExport(string buyingBrief)
        {
            try
            {
                DataTable dt = new DataTable();
                string strSQL = @"select sp.adept_code as AdeptCode_AN, 
                sp2.AdeptCode_SP, 
                sp.* 
                from Spot_Plan_Edit sp 
                inner join (
	                select Buying_Brief_ID as BB2, 
	                media_id as Media2, 
	                media_vendor_id as vendor2, 
	                show_date as show_date2, 
	                Program as program2, 
	                net_cost as cost2, 
	                Adept_Code as AdeptCode_SP, 
	                Id as Id2, 
	                Item as Item2, 
	                start_time as start_time2, 
	                end_time As end_time2 
	                from spot_plan 
	                where Buying_Brief_ID = @BuyingBrief 
	                and status = 4
                ) as SP2 
                on sp.Buying_Brief_ID = SP2.BB2 
                and sp.media_id = sp2.media2 
                and sp.media_vendor_id = sp2.vendor2 
                and sp.show_date = sp2.show_date2 
                and replace(sp.Program, ' ', '') = replace(sp2.program2, ' ', '') 
                and sp.net_cost = sp2.cost2 
                and sp.ID = sp2.ID2 
                and sp.Item = sp2.Item2 
                and sp.start_time = sp2.start_time2 
                and sp.end_time = sp2.end_time2 
                where sp.Adept_Code <> sp2.AdeptCode_SP 
                and sp.Kind = 'Del' 
                order by sp.adept_code desc";
                SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
                sda.SelectCommand.CommandTimeout = 0;
                sda.SelectCommand.Parameters.Add("@BuyingBrief", SqlDbType.VarChar).Value = buyingBrief;
                sda.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    strSQL = @"update Spot_Plan_Edit 
                    set Adept_Code = @Adept_Code 
                    where Buying_Brief_ID = @Buying_Brief_ID 
                    and len(Version) = 1 
                    and status <> 4 
                    and Kind = 'Del' 
                    and Media_Vendor_ID = @Media_Vendor_ID 
                    and Program = @Program 
                    and Net_Cost = @Net_Cost 
                    and Show_Date = @Show_Date 
                    and ID = @ID 
                    and Start_Time = @Start_Time 
                    and End_Time = @End_Time";
                    SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                    comm.CommandTimeout = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        comm.Parameters.Add("@Adept_Code", SqlDbType.VarChar).Value = dt.Rows[i]["AdeptCode_SP"].ToString();
                        comm.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = buyingBrief;
                        comm.Parameters.Add("@Media_Vendor_ID", SqlDbType.VarChar).Value = dt.Rows[i]["Media_Vendor_ID"].ToString();
                        comm.Parameters.Add("@Program", SqlDbType.VarChar).Value = dt.Rows[i]["Program"].ToString();
                        comm.Parameters.Add("@Net_Cost", SqlDbType.VarChar).Value = dt.Rows[i]["Net_Cost"].ToString();
                        comm.Parameters.Add("@Show_Date", SqlDbType.VarChar).Value = dt.Rows[i]["Show_Date"].ToString();
                        comm.Parameters.Add("@ID", SqlDbType.VarChar).Value = dt.Rows[i]["ID"].ToString();
                        comm.Parameters.Add("@Start_Time", SqlDbType.VarChar).Value = dt.Rows[i]["Start_Time"].ToString();
                        comm.Parameters.Add("@End_Time", SqlDbType.VarChar).Value = dt.Rows[i]["End_Time"].ToString();
                        comm.ExecuteNonQuery();
                        comm.Parameters.Clear();
                        InsertLogMinder("System", "Update Adept Code(" + dt.Rows[i]["Adept_Code"].ToString() + ") to (" + dt.Rows[i]["AdeptCode_SP"].ToString() + ") Buying Brief:" + buyingBrief, "Export to Adept", "System");
                    }
                }
            }
            catch (Exception e)
            {
                GMessage.MessageError(e);
            }
        }

        public bool UpdateFlagExportToAdept(string buyingBrief, string status)
        {
            try
            {
                string trueStatus = status == "9" ? "8" : status;
                string strSQL = @"update Spot_Plan 
                set Adept_Export = 'True', 
                Adept_Export_Date = @DateTime 
                where Buying_Brief_ID = @BuyingBrief 
                and Status = @Status";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.CommandTimeout = 0;
                comm.Parameters.Add("@DateTime", SqlDbType.VarChar).Value = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                comm.Parameters.Add("@BuyingBrief", SqlDbType.VarChar).Value = buyingBrief;
                comm.Parameters.Add("@Status", SqlDbType.VarChar).Value = trueStatus;
                comm.ExecuteNonQuery();
                if (status == "5" || status == "8")
                {
                    strSQL = @"update Spot_Plan_Edit 
                    set Adept_Export = 'True', 
                    Adept_Export_Date = @DateTime 
                    where Buying_Brief_ID = @BuyingBrief 
                    and Status = @Status";
                    comm.CommandText = strSQL;
                    comm.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception e)
            {
                GMessage.MessageError(e);
                return false;
            }
        }
        public bool UpdateSpotPlanEditStamPrintDateTime(string buyingBrief, string version, int item, DateTime print_datetime)
        {
            try
            {
                string strSQL = @"update spot_plan_edit
                set Print_DateTime = @Print_DateTime
                ,Printed_Flag = 1
                where Buying_Brief_ID = @Buying_Brief_ID
                and [Version] = @Version
                and [Item] = @Item";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.CommandTimeout = 0;
                comm.Parameters.Add("@Buying_Brief_ID", SqlDbType.VarChar).Value = buyingBrief;
                comm.Parameters.Add("@Version", SqlDbType.VarChar).Value = version;
                comm.Parameters.Add("@Item", SqlDbType.Int).Value = item;
                comm.Parameters.Add("@Print_DateTime", SqlDbType.DateTime).Value = print_datetime;
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                GMessage.MessageError(e);
                return false;
            }
        }

        public DataTable SelectGPMInvoiceReport(string startDate, string endDate, string agency, string client)
        {
            DataTable dt = new DataTable();
            string strSQL = @"dbo.GPM_ExportInvoice";
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, m_connMinder);
            sda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sda.SelectCommand.CommandTimeout = 0;
            sda.SelectCommand.Parameters.Add("@PeriodFrom", SqlDbType.VarChar).Value = startDate;
            sda.SelectCommand.Parameters.Add("@PeriodTo", SqlDbType.VarChar).Value = endDate;
            sda.SelectCommand.Parameters.Add("@AgencyID", SqlDbType.VarChar).Value = agency == "" ? "All" : agency;
            sda.SelectCommand.Parameters.Add("@ClientID", SqlDbType.VarChar).Value = client == "" ? "All" : client;
            sda.Fill(dt);
            return dt;
        }

        public bool DeleteSpotPlanSavingStatus(string strBB, string strVersion, string strUserID, string strScreenLock)
        {
            try
            {
                string strSQL = @"
DELETE FROM [dbo].[Spot_plan_SavingStatus]
WHERE Buying_Brief_ID = @Buying_Brief_ID
AND Version = @Version
AND User_ID = @User_ID
AND Screen_Locked = @Screen_Locked
        ";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Buying_Brief_ID", SqlDbType.NVarChar).Value = strBB;
                comm.Parameters.Add("@Version", SqlDbType.NVarChar).Value = strVersion;
                comm.Parameters.Add("@User_ID", SqlDbType.NVarChar).Value = strUserID;
                comm.Parameters.Add("@Screen_Locked", SqlDbType.NVarChar).Value = strScreenLock;

                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }
        }
        public bool InsertSpotPlanSavingStatus(string strBB, string strVersion, string strUserID, string strSavingTime, string strScreenLock)
        {
            try
            {
                string strSQL = @"INSERT INTO [dbo].[Spot_plan_SavingStatus]
           ([Buying_Brief_ID]
           ,[Market_ID]
           ,[Version]
           ,[User_ID]
           ,[Status]
           ,[Saving_Time]
           ,[Screen_Locked])
     VALUES
           (@Buying_Brief_ID
           ,'THAILAND'
           ,@Version
           ,@User_ID
           ,1
           ,@Saving_Time
           ,@Screen_Locked)
        ";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Buying_Brief_ID", SqlDbType.NVarChar).Value = strBB;
                comm.Parameters.Add("@Version", SqlDbType.NVarChar).Value = strVersion;
                comm.Parameters.Add("@User_ID", SqlDbType.NVarChar).Value = strUserID;
                comm.Parameters.Add("@Saving_Time", SqlDbType.NVarChar).Value = strSavingTime;
                comm.Parameters.Add("@Screen_Locked", SqlDbType.NVarChar).Value = strScreenLock;

                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }
        }
        public bool DeleteSpotPlanEditOD(DataRow dr)
        {
            try
            {
                string strSQL = string.Empty;
                strSQL = @" DELETE FROM Spot_Plan_Edit 
WHERE  Spot_Plan_Id = @Spot_Plan_Id 
and [Item] = @Item
and [Kind] = @Kind 
and Round([Net_Cost],2) = Round(@Net_Cost,2)
and [Buying_Brief_ID] = @Buying_Brief_ID
and [Version] = @Version";

                SqlCommand com = new SqlCommand(strSQL, m_connMinder);
                com.Parameters.Add(new SqlParameter("@Spot_Plan_Id", SqlDbType.Int)).Value = dr["Spot_Plan_Id"];
                com.Parameters.Add(new SqlParameter("@Item", SqlDbType.Int)).Value = dr["Item"];
                com.Parameters.Add(new SqlParameter("@Kind", SqlDbType.VarChar)).Value = dr["Kind"];
                com.Parameters.Add(new SqlParameter("@Net_Cost", SqlDbType.Decimal)).Value = dr["Net_Cost"];
                com.Parameters.Add(new SqlParameter("@Buying_Brief_ID", SqlDbType.VarChar)).Value = dr["Buying_Brief_ID"];
                com.Parameters.Add(new SqlParameter("@Version", SqlDbType.VarChar)).Value = dr["Version"];
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }
        }
        public bool InsertSpotPlanEdit(DataRow dr)
        {
            try
            {
                string strSQL = @"
INSERT INTO [dbo].[Spot_Plan_Edit]
           ([Buying_Brief_ID]
           ,[Market_ID]
           ,[Version]
           ,[Item]
           ,[Show_Date]
           ,[ID]
           ,[Status]
           ,[Kind]
           ,[Team]
           ,[Edit_Time]
           ,[Edit_Date]
           ,[Media_ID]
           ,[Media_Vendor_ID]
           ,[Start_Time]
           ,[End_Time]
           ,[Program]
           ,[WeekdayLimit]
           ,[Package]
           ,[SizeHW]
           ,[Unit]
           ,[Material_Key]
           ,[Material_ID]
           ,[Length]
           ,[Start_date]
           ,[end_date]
           ,[Deadline_Material]
           ,[Deadline_Terminate]
           ,[State]
           ,[Market_Price]
           ,[Rate]
           ,[Discount]
           ,[Weight]
           ,[Net_Cost]
           ,[Program_Type]
           ,[Prebuy_Start_Time]
           ,[Prebuy_End_Time]
           ,[Rating]
           ,[CPRP_Cost]
           ,[Include_Media_Cost]
           ,[CPM]
           ,[Remark]
           ,[Spots]
           ,[Adept_Code]
           ,[Adept_Export]
           ,[Adept_Hide]
           ,[Adept_Export_Date]
           ,[Appear]
           ,[Finish]
           ,[Row_ID]
           ,[Pkg]
           ,[Booking_Order_ID]
           ,[Color]
           ,[Program_Code]
           ,[Surcharge]
           ,[Bonus_Percent]
           ,[Bonus_Cost]
           ,[Invoice_Number]
           ,[Disctrict]
           ,[Sub_Disctrict]
           ,[Site]
           ,[Pane]
           ,[TOTAL]
           ,[Province]
           ,[Total_Gross]
           ,[Ad_Tax]
           ,[VAT]
           ,[Cash_Discount]
           ,[Position_In_Break]
           ,[Printed_Flag]
           ,[Vendor_Discount]
           ,[Vendor_Net_Cost]
           ,[SP_Movement]
           ,[Cost_Item_ID]
           ,[Billings_Year]
           ,[Billings_Month]
           ,[Media_Sub_Type]
           ,[Agency_Fee]
           ,[BuyTypeName]
           ,[BuyTypeID]
           ,[Print_DateTime]
           ,[Spot_Plan_Id]
           ,[Media_Type])
     VALUES(@Buying_Brief_ID
        ,@Market_ID
        ,@Version
        ,@Item
        ,@Show_Date
        ,@ID
        ,@Status
        ,@Kind
        ,@Team
        ,@Edit_Time
        ,@Edit_Date
        ,@Media_ID
        ,@Media_Vendor_ID
        ,@Start_Time
        ,@End_Time
        ,@Program
        ,@WeekdayLimit
        ,@Package
        ,@SizeHW
        ,@Unit
        ,@Material_Key
        ,@Material_ID
        ,@Length
        ,@Start_date
        ,@end_date
        ,@Deadline_Material
        ,@Deadline_Terminate
        ,@State
        ,@Market_Price
        ,@Rate
        ,@Discount
        ,@Weight
        ,@Net_Cost
        ,@Program_Type
        ,@Prebuy_Start_Time
        ,@Prebuy_End_Time
        ,@Rating
        ,@CPRP_Cost
        ,@Include_Media_Cost
        ,@CPM
        ,@Remark
        ,@Spots
        ,@Adept_Code
        ,@Adept_Export
        ,@Adept_Hide
        ,@Adept_Export_Date
        ,@Appear
        ,@Finish
        ,@Row_ID
        ,@Pkg
        ,@Booking_Order_ID
        ,@Color
        ,@Program_Code
        ,@Surcharge
        ,@Bonus_Percent
        ,@Bonus_Cost
        ,@Invoice_Number
        ,@Disctrict
        ,@Sub_Disctrict
        ,@Site
        ,@Pane
        ,@TOTAL
        ,@Province
        ,@Total_Gross
        ,@Ad_Tax
        ,@VAT
        ,@Cash_Discount
        ,@Position_In_Break
        ,@Printed_Flag
        ,@Vendor_Discount
        ,@Vendor_Net_Cost
        ,@SP_Movement
        ,@Cost_Item_ID
        ,@Billings_Year
        ,@Billings_Month
        ,@Media_Sub_Type
        ,@Agency_Fee
        ,@BuyTypeName
        ,@BuyTypeID
        ,@Print_DateTime
        ,@Spot_Plan_Id
        ,@Media_Type)";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Buying_Brief_ID", SqlDbType.NVarChar).Value = dr["Buying_Brief_ID"];
                comm.Parameters.Add("@Market_ID", SqlDbType.NVarChar).Value = dr["Market_ID"];
                comm.Parameters.Add("@Version", SqlDbType.NVarChar).Value = dr["Version"];
                comm.Parameters.Add("@Item", SqlDbType.Int).Value = dr["Item"];
                comm.Parameters.Add("@Show_Date", SqlDbType.NVarChar).Value = dr["Show_Date"];
                comm.Parameters.Add("@ID", SqlDbType.Int).Value = dr["ID"];
                comm.Parameters.Add("@Status", SqlDbType.Int).Value = dr["Status"];
                comm.Parameters.Add("@Kind", SqlDbType.NVarChar).Value = dr["Kind"];
                comm.Parameters.Add("@Team", SqlDbType.NVarChar).Value = dr["Team"];
                comm.Parameters.Add("@Edit_Time", SqlDbType.NVarChar).Value = dr["Edit_Time"];
                comm.Parameters.Add("@Edit_Date", SqlDbType.NVarChar).Value = dr["Edit_Date"];
                comm.Parameters.Add("@Media_ID", SqlDbType.NVarChar).Value = dr["Media_ID"];
                comm.Parameters.Add("@Media_Vendor_ID", SqlDbType.NVarChar).Value = dr["Media_Vendor_ID"];
                comm.Parameters.Add("@Start_Time", SqlDbType.NVarChar).Value = dr["Start_Time"];
                comm.Parameters.Add("@End_Time", SqlDbType.NVarChar).Value = dr["End_Time"];
                comm.Parameters.Add("@Program", SqlDbType.NVarChar).Value = dr["Program"];
                comm.Parameters.Add("@WeekdayLimit", SqlDbType.NVarChar).Value = dr["WeekdayLimit"];
                comm.Parameters.Add("@Package", SqlDbType.NVarChar).Value = dr["Package"];
                comm.Parameters.Add("@SizeHW", SqlDbType.NVarChar).Value = dr["SizeHW"];
                comm.Parameters.Add("@Unit", SqlDbType.NVarChar).Value = dr["Unit"];
                comm.Parameters.Add("@Material_Key", SqlDbType.NVarChar).Value = dr["Material_Key"];
                comm.Parameters.Add("@Material_ID", SqlDbType.NVarChar).Value = dr["Material_ID"];
                comm.Parameters.Add("@Length", SqlDbType.Float).Value = dr["Length"];
                comm.Parameters.Add("@Start_date", SqlDbType.NVarChar).Value = dr["Start_date"];
                comm.Parameters.Add("@end_date", SqlDbType.NVarChar).Value = dr["end_date"];
                comm.Parameters.Add("@Deadline_Material", SqlDbType.NVarChar).Value = dr["Deadline_Material"];
                comm.Parameters.Add("@Deadline_Terminate", SqlDbType.NVarChar).Value = dr["Deadline_Terminate"];
                comm.Parameters.Add("@State", SqlDbType.NVarChar).Value = dr["State"];
                comm.Parameters.Add("@Market_Price", SqlDbType.Float).Value = dr["Market_Price"];
                comm.Parameters.Add("@Rate", SqlDbType.Float).Value = dr["Rate"];
                comm.Parameters.Add("@Discount", SqlDbType.Float).Value = dr["Discount"];
                comm.Parameters.Add("@Weight", SqlDbType.Float).Value = dr["Weight"];
                comm.Parameters.Add("@Net_Cost", SqlDbType.Float).Value = dr["Net_Cost"];
                comm.Parameters.Add("@Program_Type", SqlDbType.NVarChar).Value = dr["Program_Type"];
                comm.Parameters.Add("@Prebuy_Start_Time", SqlDbType.NVarChar).Value = dr["Prebuy_Start_Time"];
                comm.Parameters.Add("@Prebuy_End_Time", SqlDbType.NVarChar).Value = dr["Prebuy_End_Time"];
                comm.Parameters.Add("@Rating", SqlDbType.Float).Value = dr["Rating"];
                comm.Parameters.Add("@CPRP_Cost", SqlDbType.Float).Value = dr["CPRP_Cost"];
                comm.Parameters.Add("@Include_Media_Cost", SqlDbType.Bit).Value = dr["Include_Media_Cost"];
                comm.Parameters.Add("@CPM", SqlDbType.Float).Value = dr["CPM"];
                comm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = dr["Remark"];
                comm.Parameters.Add("@Spots", SqlDbType.Int).Value = dr["Spots"];
                comm.Parameters.Add("@Adept_Code", SqlDbType.Int).Value = dr["Adept_Code"];
                comm.Parameters.Add("@Adept_Export", SqlDbType.Bit).Value = dr["Adept_Export"];
                comm.Parameters.Add("@Adept_Hide", SqlDbType.Bit).Value = dr["Adept_Hide"];
                comm.Parameters.Add("@Adept_Export_Date", SqlDbType.NVarChar).Value = dr["Adept_Export_Date"];
                comm.Parameters.Add("@Appear", SqlDbType.Bit).Value = dr["Appear"];
                comm.Parameters.Add("@Finish", SqlDbType.Bit).Value = dr["Finish"];
                comm.Parameters.Add("@Row_ID", SqlDbType.Int).Value = Convert.ToInt32(dr["Item"]) + 1;
                comm.Parameters.Add("@Pkg", SqlDbType.Int).Value = dr["Pkg"];
                comm.Parameters.Add("@Booking_Order_ID", SqlDbType.NVarChar).Value = dr["Booking_Order_ID"];
                comm.Parameters.Add("@Color", SqlDbType.NVarChar).Value = dr["Color"];
                comm.Parameters.Add("@Program_Code", SqlDbType.Decimal).Value = dr["Program_Code"];
                comm.Parameters.Add("@Surcharge", SqlDbType.Real).Value = dr["Surcharge"];
                comm.Parameters.Add("@Bonus_Percent", SqlDbType.Real).Value = dr["Bonus_Percent"];
                comm.Parameters.Add("@Bonus_Cost", SqlDbType.Real).Value = dr["Bonus_Cost"];
                comm.Parameters.Add("@Invoice_Number", SqlDbType.NVarChar).Value = dr["Invoice_Number"];
                comm.Parameters.Add("@Disctrict", SqlDbType.NVarChar).Value = dr["Disctrict"];
                comm.Parameters.Add("@Sub_Disctrict", SqlDbType.NVarChar).Value = dr["Sub_Disctrict"];
                comm.Parameters.Add("@Site", SqlDbType.NVarChar).Value = dr["Site"];
                comm.Parameters.Add("@Pane", SqlDbType.NVarChar).Value = dr["Pane"];
                comm.Parameters.Add("@TOTAL", SqlDbType.Float).Value = dr["TOTAL"];
                comm.Parameters.Add("@Province", SqlDbType.NVarChar).Value = dr["Province"];
                comm.Parameters.Add("@Total_Gross", SqlDbType.Real).Value = dr["Total_Gross"];
                comm.Parameters.Add("@Ad_Tax", SqlDbType.Real).Value = dr["Ad_Tax"];
                comm.Parameters.Add("@VAT", SqlDbType.Bit).Value = dr["VAT"];
                comm.Parameters.Add("@Cash_Discount", SqlDbType.Real).Value = dr["Cash_Discount"];
                comm.Parameters.Add("@Position_In_Break", SqlDbType.NVarChar).Value = dr["Position_In_Break"];
                comm.Parameters.Add("@Printed_Flag", SqlDbType.Bit).Value = dr["Printed_Flag"];
                comm.Parameters.Add("@Vendor_Discount", SqlDbType.Real).Value = dr["Vendor_Discount"];
                comm.Parameters.Add("@Vendor_Net_Cost", SqlDbType.Real).Value = dr["Vendor_Net_Cost"];
                comm.Parameters.Add("@SP_Movement", SqlDbType.Bit).Value = dr["SP_Movement"];
                comm.Parameters.Add("@Cost_Item_ID", SqlDbType.NVarChar).Value = dr["Cost_Item_ID"];
                comm.Parameters.Add("@Billings_Year", SqlDbType.NVarChar).Value = dr["Billings_Year"];
                comm.Parameters.Add("@Billings_Month", SqlDbType.NVarChar).Value = dr["Billings_Month"];
                comm.Parameters.Add("@Media_Sub_Type", SqlDbType.NVarChar).Value = dr["Media_Sub_Type"];
                comm.Parameters.Add("@Agency_Fee", SqlDbType.Real).Value = dr["Agency_Fee"];
                comm.Parameters.Add("@BuyTypeName", SqlDbType.NVarChar).Value = dr["BuyTypeName"];
                comm.Parameters.Add("@BuyTypeID", SqlDbType.NVarChar).Value = dr["BuyTypeID"];
                comm.Parameters.Add("@Print_DateTime", SqlDbType.DateTime).Value = dr["Print_DateTime"];
                comm.Parameters.Add("@Spot_Plan_Id", SqlDbType.BigInt).Value = dr["Spot_Plan_Id"];
                comm.Parameters.Add("@Media_Type", SqlDbType.NVarChar).Value = dr["Media_Type"];
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }
        }

        public bool InsertGPMProgramMapping(DataTable dt)
        {
            try
            {
                string strSQLDelete = $@"DELETE FROM GPM_Program_Mapping WHERE Program = @Program";
                SqlCommand commDelete = new SqlCommand(strSQLDelete, m_connMinder);

                string strSQLInsert = $@"INSERT INTO [dbo].[GPM_Program_Mapping] ([Program],[Program_Group]) VALUES (@Program

,RTRIM(LTRIM(LEFT(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(
	@Program_Group
	,':',''),'\',''),'/',''),'?',''),'*',''),'[',''),']',''),31)))

)";
                SqlCommand comm = new SqlCommand(strSQLInsert, m_connMinder);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    if (dr["chk"].ToString() == "1")
                    {
                        commDelete.Parameters.Clear();
                        commDelete.Parameters.Add("@Program", SqlDbType.NVarChar).Value = dr["Program"];
                        commDelete.ExecuteNonQuery();

                        comm.Parameters.Clear();
                        comm.Parameters.Add("@Program", SqlDbType.NVarChar).Value = dr["Program"];
                        comm.Parameters.Add("@Program_Group", SqlDbType.NVarChar).Value = dr["Program_Group"];
                        comm.ExecuteNonQuery();
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }

        }

        public bool UpdateSpotPlanDetail(DataRow dr)
        {
            try
            {
                string strSQL = @"UPDATE [dbo].[Spot_Plan]
   SET [Market_ID] = @Market_ID 
,[Show_Date] = @Show_Date 
,[Status] = @Status 
,[Media_ID] = @Media_ID 
,[Media_Vendor_ID] = @Media_Vendor_ID 
,[Start_Time] = @Start_Time 
,[End_Time] = @End_Time 
,[Program] = @Program 
,[WeekdayLimit] = @WeekdayLimit 
,[Package] = @Package 
,[SizeHW] = @SizeHW 
,[Unit] = @Unit 
,[Material_Key] = @Material_Key 
,[Material_ID] = @Material_ID 
,[Length] = @Length 
,[Start_date] = @Start_date 
,[end_date] = @end_date 
,[Deadline_Material] = @Deadline_Material 
,[Deadline_Terminate] = @Deadline_Terminate 
,[State] = @State 
,[Market_Price] = @Market_Price 
,[Rate] = @Rate 
,[Discount] = @Discount 
,[Weight] = @Weight 
,[Net_Cost] = @Net_Cost 
,[Program_Type] = @Program_Type 
,[Prebuy_Start_Time] = @Prebuy_Start_Time 
,[Prebuy_End_Time] = @Prebuy_End_Time 
,[Rating] = @Rating 
,[CPRP_Cost] = @CPRP_Cost 
,[Include_Media_Cost] = @Include_Media_Cost 
,[CPM] = @CPM 
,[Remark] = @Remark 
,[Spots] = @Spots 
,[Adept_Code] = @Adept_Code 
,[Adept_Export] = @Adept_Export 
,[Adept_Hide] = @Adept_Hide 
,[Adept_Export_Date] = @Adept_Export_Date 
,[Appear] = @Appear 
,[Finish] = @Finish 
,[Row_ID] = @Row_ID 
,[Pkg] = @Pkg 
,[Booking_Order_ID] = @Booking_Order_ID 
,[District] = @District 
,[Sub_District] = @Sub_District 
,[Site] = @Site 
,[Pane] = @Pane 
,[Color] = @Color 
,[Program_Code] = @Program_Code 
,[Surcharge] = @Surcharge 
,[Bonus_Percent] = @Bonus_Percent 
,[Bonus_Cost] = @Bonus_Cost 
,[Invoice_Number] = @Invoice_Number 
,[Program_Referrence_Code] = @Program_Referrence_Code 
,[Reschedule_Status] = @Reschedule_Status 
,[Resch_BO] = @Resch_BO 
,[TOTAL] = @TOTAL 
,[Province] = @Province 
,[Bonus_Status] = @Bonus_Status 
,[GroupRating] = @GroupRating 
,[GroupCPRP] = @GroupCPRP 
,[Total_Gross] = @Total_Gross 
,[Ad_Tax] = @Ad_Tax 
,[VAT] = @VAT 
,[CD_Percent] = @CD_Percent 
,[OMI] = @OMI 
,[Format_Type_ID] = @Format_Type_ID 
,[Export_Booking_Order_Flag] = @Export_Booking_Order_Flag 
,[Spending_Type] = @Spending_Type 
,[Original_Rating] = @Original_Rating 
,[Buffer_TVR_State] = @Buffer_TVR_State 
,[Cash_Discount] = @Cash_Discount 
,[Position_In_Break] = @Position_In_Break 
,[Ratecard] = @Ratecard 
,[Discount_Ratecard] = @Discount_Ratecard 
,[MarketRate] = @MarketRate 
,[Discount_MarketRate] = @Discount_MarketRate 
,[GroupMRate] = @GroupMRate 
,[Discount_GroupMRate] = @Discount_GroupMRate 
,[Added_Name] = @Added_Name 
,[Sub_Package] = @Sub_Package 
,[C_Start_Date] = @C_Start_Date 
,[D_End_Date] = @D_End_Date 
,[C_End_Date] = @C_End_Date 
,[Frequency] = @Frequency 
,[Reach] = @Reach 
,[Frequency_PostBuy] = @Frequency_PostBuy 
,[Reach_PostBuy] = @Reach_PostBuy 
,[Master_Package] = @Master_Package 
,[Verified] = @Verified 
,[Usr] = @Usr 
,[KeyID] = @KeyID 
,[Vendor_Discount] = @Vendor_Discount 
,[Vendor_Net_Cost] = @Vendor_Net_Cost 
,[CH7_export] = @CH7_export 
,[DH_Export] = @DH_Export 
,[Spot_verified] = @Spot_verified 
,[Cost_Item_ID] = @Cost_Item_ID 
,[Billings_Year] = @Billings_Year 
,[Billings_Month] = @Billings_Month 
,[AdjustedAfterActual] = @AdjustedAfterActual 
,[Verified_Date] = @Verified_Date 
,[Media_Sub_Type] = @Media_Sub_Type 
,[Agency_Fee] = @Agency_Fee 
,[Show_Date_Original] = @Show_Date_Original 
,[BuyTypeName] = @BuyTypeName 
,[BuyTypeID] = @BuyTypeID 
,[Media_Type] = @Media_Type 
 WHERE [Buying_Brief_ID] = @Buying_Brief_ID 
 AND [Version] = @Version 
 AND [Item] = @Item 
 AND [ID] = @ID 

";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Buying_Brief_ID", SqlDbType.NVarChar).Value = dr["Buying_Brief_ID"];
                comm.Parameters.Add("@Market_ID", SqlDbType.NVarChar).Value = dr["Market_ID"];
                comm.Parameters.Add("@Version", SqlDbType.NVarChar).Value = dr["Version"];
                comm.Parameters.Add("@Item", SqlDbType.Int).Value = dr["Item"];
                comm.Parameters.Add("@Show_Date", SqlDbType.NVarChar).Value = dr["Show_Date"];
                comm.Parameters.Add("@ID", SqlDbType.Int).Value = dr["ID"];
                comm.Parameters.Add("@Status", SqlDbType.Int).Value = dr["Status"];
                comm.Parameters.Add("@Media_ID", SqlDbType.NVarChar).Value = dr["Media_ID"];
                comm.Parameters.Add("@Media_Vendor_ID", SqlDbType.NVarChar).Value = dr["Media_Vendor_ID"];
                comm.Parameters.Add("@Start_Time", SqlDbType.NVarChar).Value = dr["Start_Time"];
                comm.Parameters.Add("@End_Time", SqlDbType.NVarChar).Value = dr["End_Time"];
                comm.Parameters.Add("@Program", SqlDbType.NVarChar).Value = dr["Program"];
                comm.Parameters.Add("@WeekdayLimit", SqlDbType.NVarChar).Value = dr["WeekdayLimit"];
                comm.Parameters.Add("@Package", SqlDbType.NVarChar).Value = dr["Package"];
                comm.Parameters.Add("@SizeHW", SqlDbType.NVarChar).Value = dr["SizeHW"];
                comm.Parameters.Add("@Unit", SqlDbType.NVarChar).Value = dr["Unit"];
                comm.Parameters.Add("@Material_Key", SqlDbType.NVarChar).Value = dr["Material_Key"];
                comm.Parameters.Add("@Material_ID", SqlDbType.NVarChar).Value = dr["Material_ID"];
                comm.Parameters.Add("@Length", SqlDbType.Float).Value = dr["Length"];
                comm.Parameters.Add("@Start_date", SqlDbType.NVarChar).Value = dr["Start_date"];
                comm.Parameters.Add("@end_date", SqlDbType.NVarChar).Value = dr["end_date"];
                comm.Parameters.Add("@Deadline_Material", SqlDbType.NVarChar).Value = dr["Deadline_Material"];
                comm.Parameters.Add("@Deadline_Terminate", SqlDbType.NVarChar).Value = dr["Deadline_Terminate"];
                comm.Parameters.Add("@State", SqlDbType.NVarChar).Value = dr["State"];
                comm.Parameters.Add("@Market_Price", SqlDbType.Float).Value = dr["Market_Price"];
                comm.Parameters.Add("@Rate", SqlDbType.Float).Value = dr["Rate"];
                comm.Parameters.Add("@Discount", SqlDbType.Float).Value = dr["Discount"];
                comm.Parameters.Add("@Weight", SqlDbType.Float).Value = dr["Weight"];
                comm.Parameters.Add("@Net_Cost", SqlDbType.Float).Value = dr["Net_Cost"];
                comm.Parameters.Add("@Program_Type", SqlDbType.NVarChar).Value = dr["Program_Type"];
                comm.Parameters.Add("@Prebuy_Start_Time", SqlDbType.NVarChar).Value = dr["Prebuy_Start_Time"];
                comm.Parameters.Add("@Prebuy_End_Time", SqlDbType.NVarChar).Value = dr["Prebuy_End_Time"];
                comm.Parameters.Add("@Rating", SqlDbType.Float).Value = dr["Rating"];
                comm.Parameters.Add("@CPRP_Cost", SqlDbType.Float).Value = dr["CPRP_Cost"];
                comm.Parameters.Add("@Include_Media_Cost", SqlDbType.Bit).Value = dr["Include_Media_Cost"];
                comm.Parameters.Add("@CPM", SqlDbType.Float).Value = dr["CPM"];
                comm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = dr["Remark"];
                comm.Parameters.Add("@Spots", SqlDbType.Int).Value = dr["Spots"];
                comm.Parameters.Add("@Adept_Code", SqlDbType.Int).Value = dr["Adept_Code"];
                comm.Parameters.Add("@Adept_Export", SqlDbType.Bit).Value = dr["Adept_Export"];
                comm.Parameters.Add("@Adept_Hide", SqlDbType.Bit).Value = dr["Adept_Hide"];
                comm.Parameters.Add("@Adept_Export_Date", SqlDbType.NVarChar).Value = dr["Adept_Export_Date"];
                comm.Parameters.Add("@Appear", SqlDbType.Bit).Value = dr["Appear"];
                comm.Parameters.Add("@Finish", SqlDbType.Bit).Value = dr["Finish"];
                comm.Parameters.Add("@Row_ID", SqlDbType.Int).Value = Convert.ToInt32(dr["Item"]) + 1;  //SqlDbType.Int).Value = dr["Row_ID"];
                comm.Parameters.Add("@Pkg", SqlDbType.Int).Value = dr["Pkg"];
                comm.Parameters.Add("@Booking_Order_ID", SqlDbType.NVarChar).Value = dr["Booking_Order_ID"];
                comm.Parameters.Add("@District", SqlDbType.NVarChar).Value = dr["District"];
                comm.Parameters.Add("@Sub_District", SqlDbType.NVarChar).Value = dr["Sub_District"];
                comm.Parameters.Add("@Site", SqlDbType.NVarChar).Value = dr["Site"];
                comm.Parameters.Add("@Pane", SqlDbType.NVarChar).Value = dr["Pane"];
                comm.Parameters.Add("@Color", SqlDbType.NVarChar).Value = dr["Color"];
                comm.Parameters.Add("@Program_Code", SqlDbType.Decimal).Value = dr["Program_Code"];
                comm.Parameters.Add("@Surcharge", SqlDbType.Real).Value = dr["Surcharge"];
                comm.Parameters.Add("@Bonus_Percent", SqlDbType.Real).Value = dr["Bonus_Percent"];
                comm.Parameters.Add("@Bonus_Cost", SqlDbType.Real).Value = dr["Bonus_Cost"];
                comm.Parameters.Add("@Invoice_Number", SqlDbType.NVarChar).Value = dr["Invoice_Number"];
                comm.Parameters.Add("@Program_Referrence_Code", SqlDbType.NVarChar).Value = dr["Program_Referrence_Code"];
                comm.Parameters.Add("@Reschedule_Status", SqlDbType.NVarChar).Value = dr["Reschedule_Status"];
                comm.Parameters.Add("@Resch_BO", SqlDbType.NVarChar).Value = dr["Resch_BO"];
                comm.Parameters.Add("@TOTAL", SqlDbType.Float).Value = dr["TOTAL"];
                comm.Parameters.Add("@Province", SqlDbType.NVarChar).Value = dr["Province"];
                comm.Parameters.Add("@Bonus_Status", SqlDbType.Int).Value = dr["Bonus_Status"];
                comm.Parameters.Add("@GroupRating", SqlDbType.Float).Value = dr["GroupRating"];
                comm.Parameters.Add("@GroupCPRP", SqlDbType.Float).Value = dr["GroupCPRP"];
                comm.Parameters.Add("@Total_Gross", SqlDbType.Real).Value = dr["Total_Gross"];
                comm.Parameters.Add("@Ad_Tax", SqlDbType.Real).Value = dr["Ad_Tax"];
                comm.Parameters.Add("@VAT", SqlDbType.Bit).Value = dr["VAT"];
                comm.Parameters.Add("@CD_Percent", SqlDbType.Real).Value = dr["CD_Percent"];
                comm.Parameters.Add("@OMI", SqlDbType.Bit).Value = dr["OMI"];
                comm.Parameters.Add("@Format_Type_ID", SqlDbType.NVarChar).Value = dr["Format_Type_ID"];
                comm.Parameters.Add("@Export_Booking_Order_Flag", SqlDbType.Int).Value = dr["Export_Booking_Order_Flag"];
                comm.Parameters.Add("@Spending_Type", SqlDbType.Int).Value = dr["Spending_Type"];
                comm.Parameters.Add("@Original_Rating", SqlDbType.Float).Value = dr["Original_Rating"];
                comm.Parameters.Add("@Buffer_TVR_State", SqlDbType.Bit).Value = dr["Buffer_TVR_State"];
                comm.Parameters.Add("@Cash_Discount", SqlDbType.Real).Value = dr["Cash_Discount"];
                comm.Parameters.Add("@Position_In_Break", SqlDbType.NVarChar).Value = dr["Position_In_Break"];
                comm.Parameters.Add("@Ratecard", SqlDbType.Float).Value = dr["Ratecard"];
                comm.Parameters.Add("@Discount_Ratecard", SqlDbType.Float).Value = dr["Discount_Ratecard"];
                comm.Parameters.Add("@MarketRate", SqlDbType.Float).Value = dr["MarketRate"];
                comm.Parameters.Add("@Discount_MarketRate", SqlDbType.Float).Value = dr["Discount_MarketRate"];
                comm.Parameters.Add("@GroupMRate", SqlDbType.Float).Value = dr["GroupMRate"];
                comm.Parameters.Add("@Discount_GroupMRate", SqlDbType.Float).Value = dr["Discount_GroupMRate"];
                comm.Parameters.Add("@Added_Name", SqlDbType.NVarChar).Value = dr["Added_Name"];
                comm.Parameters.Add("@Sub_Package", SqlDbType.NVarChar).Value = dr["Sub_Package"];
                comm.Parameters.Add("@C_Start_Date", SqlDbType.NVarChar).Value = dr["C_Start_Date"];
                comm.Parameters.Add("@D_End_Date", SqlDbType.NVarChar).Value = dr["D_End_Date"];
                comm.Parameters.Add("@C_End_Date", SqlDbType.NVarChar).Value = dr["C_End_Date"];
                comm.Parameters.Add("@Frequency", SqlDbType.NVarChar).Value = dr["Frequency"];
                comm.Parameters.Add("@Reach", SqlDbType.Float).Value = dr["Reach"];
                comm.Parameters.Add("@Frequency_PostBuy", SqlDbType.NVarChar).Value = dr["Frequency_PostBuy"];
                comm.Parameters.Add("@Reach_PostBuy", SqlDbType.Float).Value = dr["Reach_PostBuy"];
                comm.Parameters.Add("@Master_Package", SqlDbType.NVarChar).Value = dr["Master_Package"];
                comm.Parameters.Add("@Verified", SqlDbType.Bit).Value = dr["Verified"];
                comm.Parameters.Add("@Usr", SqlDbType.NVarChar).Value = dr["Usr"];
                comm.Parameters.Add("@KeyID", SqlDbType.NVarChar).Value = dr["KeyID"];
                comm.Parameters.Add("@Vendor_Discount", SqlDbType.Float).Value = dr["Vendor_Discount"];
                comm.Parameters.Add("@Vendor_Net_Cost", SqlDbType.Float).Value = dr["Vendor_Net_Cost"];
                comm.Parameters.Add("@CH7_export", SqlDbType.Bit).Value = dr["CH7_export"];
                comm.Parameters.Add("@DH_Export", SqlDbType.Bit).Value = dr["DH_Export"];
                comm.Parameters.Add("@Spot_verified", SqlDbType.Bit).Value = dr["Spot_verified"];
                comm.Parameters.Add("@Cost_Item_ID", SqlDbType.NVarChar).Value = dr["Cost_Item_ID"];
                comm.Parameters.Add("@Billings_Year", SqlDbType.NVarChar).Value = dr["Billings_Year"];
                comm.Parameters.Add("@Billings_Month", SqlDbType.NVarChar).Value = dr["Billings_Month"];
                comm.Parameters.Add("@AdjustedAfterActual", SqlDbType.Bit).Value = dr["AdjustedAfterActual"];
                comm.Parameters.Add("@Verified_Date", SqlDbType.NVarChar).Value = dr["Verified_Date"];
                comm.Parameters.Add("@Media_Sub_Type", SqlDbType.NVarChar).Value = dr["Media_Sub_Type"];
                comm.Parameters.Add("@Agency_Fee", SqlDbType.Real).Value = dr["Agency_Fee"];
                comm.Parameters.Add("@Show_Date_Original", SqlDbType.NVarChar).Value = dr["Show_Date_Original"];
                comm.Parameters.Add("@BuyTypeName", SqlDbType.NVarChar).Value = dr["BuyTypeName"];
                comm.Parameters.Add("@BuyTypeID", SqlDbType.NVarChar).Value = dr["BuyTypeID"];
                comm.Parameters.Add("@Media_Type", SqlDbType.NVarChar).Value = dr["Media_Type"];

                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }

        }
        public bool DeleteSpotPlanDetail(DataRow dr)
        {
            try
            {
                string strSQL = @"DELETE FROM [dbo].[Spot_Plan]
WHERE [Buying_Brief_ID] = @Buying_Brief_ID
and [Version] = @Version
--and Media_ID = @Media_ID
--and Media_Vendor_ID = @Media_Vendor_ID
--and Item = @Item
and Spot_Plan_Id = @Spot_Plan_Id
";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Buying_Brief_ID", SqlDbType.NVarChar).Value = dr["Buying_Brief_ID",DataRowVersion.Original];
                comm.Parameters.Add("@Version", SqlDbType.NVarChar).Value = dr["Version", DataRowVersion.Original];
                //comm.Parameters.Add("@Media_ID", SqlDbType.NVarChar).Value = dr["Media_ID",DataRowVersion.Original];
                //comm.Parameters.Add("@Media_Vendor_ID", SqlDbType.NVarChar).Value = dr["Media_Vendor_ID",DataRowVersion.Original];
                //comm.Parameters.Add("@Item", SqlDbType.Int).Value = dr["Item",DataRowVersion.Original];
                comm.Parameters.Add("@Spot_Plan_Id", SqlDbType.Int).Value = dr["Spot_Plan_Id", DataRowVersion.Original];

                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }

        }
        public bool InsertSpotPlanDetail(DataRow dr)
        {
            try
            {
                string strSQL = @"
INSERT INTO [dbo].[Spot_Plan]
           ([Buying_Brief_ID]
           ,[Market_ID]
           ,[Version]
           ,[Item]
           ,[Show_Date]
           ,[ID]
           ,[Status]
           ,[Media_ID]
           ,[Media_Vendor_ID]
           ,[Start_Time]
           ,[End_Time]
           ,[Program]
           ,[WeekdayLimit]
           ,[Package]
           ,[SizeHW]
           ,[Unit]
           ,[Material_Key]
           ,[Material_ID]
           ,[Length]
           ,[Start_date]
           ,[end_date]
           ,[Deadline_Material]
           ,[Deadline_Terminate]
           ,[State]
           ,[Market_Price]
           ,[Rate]
           ,[Discount]
           ,[Weight]
           ,[Net_Cost]
           ,[Program_Type]
           ,[Prebuy_Start_Time]
           ,[Prebuy_End_Time]
           ,[Rating]
           ,[CPRP_Cost]
           ,[Include_Media_Cost]
           ,[CPM]
           ,[Remark]
           ,[Spots]
           ,[Adept_Code]
           ,[Adept_Export]
           ,[Adept_Hide]
           ,[Adept_Export_Date]
           ,[Appear]
           ,[Finish]
           ,[Row_ID]
           ,[Pkg]
           ,[Booking_Order_ID]
           ,[District]
           ,[Sub_District]
           ,[Site]
           ,[Pane]
           ,[Color]
           ,[Program_Code]
           ,[Surcharge]
           ,[Bonus_Percent]
           ,[Bonus_Cost]
           ,[Invoice_Number]
           ,[Program_Referrence_Code]
           ,[Reschedule_Status]
           ,[Resch_BO]
           ,[TOTAL]
           ,[Province]
           ,[Bonus_Status]
           ,[GroupRating]
           ,[GroupCPRP]
           ,[Total_Gross]
           ,[Ad_Tax]
           ,[VAT]
           ,[CD_Percent]
           ,[OMI]
           ,[Format_Type_ID]
           ,[Export_Booking_Order_Flag]
           ,[Spending_Type]
           ,[Original_Rating]
           ,[Buffer_TVR_State]
           ,[Cash_Discount]
           ,[Position_In_Break]
           ,[Ratecard]
           ,[Discount_Ratecard]
           ,[MarketRate]
           ,[Discount_MarketRate]
           ,[GroupMRate]
           ,[Discount_GroupMRate]
           ,[Added_Name]
           ,[Sub_Package]
           ,[C_Start_Date]
           ,[D_End_Date]
           ,[C_End_Date]
           ,[Frequency]
           ,[Reach]
           ,[Frequency_PostBuy]
           ,[Reach_PostBuy]
           ,[Master_Package]
           ,[Verified]
           ,[Usr]
           ,[KeyID]
           ,[Vendor_Discount]
           ,[Vendor_Net_Cost]
           ,[CH7_export]
           ,[DH_Export]
           ,[Spot_verified]
           ,[Cost_Item_ID]
           ,[Billings_Year]
           ,[Billings_Month]
           ,[AdjustedAfterActual]
           ,[Verified_Date]
           ,[Media_Sub_Type]
           ,[Agency_Fee]
           ,[Show_Date_Original]
           ,[BuyTypeName]
           ,[BuyTypeID]
           ,[Media_Type])
     VALUES
           (@Buying_Brief_ID
    ,@Market_ID
    ,@Version
    ,@Item
    ,@Show_Date
    ,@ID
    ,@Status
    ,@Media_ID
    ,@Media_Vendor_ID
    ,@Start_Time
    ,@End_Time
    ,@Program
    ,@WeekdayLimit
    ,@Package
    ,@SizeHW
    ,@Unit
    ,@Material_Key
    ,@Material_ID
    ,@Length
    ,@Start_date
    ,@end_date
    ,@Deadline_Material
    ,@Deadline_Terminate
    ,@State
    ,@Market_Price
    ,@Rate
    ,@Discount
    ,@Weight
    ,@Net_Cost
    ,@Program_Type
    ,@Prebuy_Start_Time
    ,@Prebuy_End_Time
    ,@Rating
    ,@CPRP_Cost
    ,@Include_Media_Cost
    ,@CPM
    ,@Remark
    ,@Spots
    ,@Adept_Code
    ,@Adept_Export
    ,@Adept_Hide
    ,@Adept_Export_Date
    ,@Appear
    ,@Finish
    ,@Row_ID
    ,@Pkg
    ,@Booking_Order_ID
    ,@District
    ,@Sub_District
    ,@Site
    ,@Pane
    ,@Color
    ,@Program_Code
    ,@Surcharge
    ,@Bonus_Percent
    ,@Bonus_Cost
    ,@Invoice_Number
    ,@Program_Referrence_Code
    ,@Reschedule_Status
    ,@Resch_BO
    ,@TOTAL
    ,@Province
    ,@Bonus_Status
    ,@GroupRating
    ,@GroupCPRP
    ,@Total_Gross
    ,@Ad_Tax
    ,@VAT
    ,@CD_Percent
    ,@OMI
    ,@Format_Type_ID
    ,@Export_Booking_Order_Flag
    ,@Spending_Type
    ,@Original_Rating
    ,@Buffer_TVR_State
    ,@Cash_Discount
    ,@Position_In_Break
    ,@Ratecard
    ,@Discount_Ratecard
    ,@MarketRate
    ,@Discount_MarketRate
    ,@GroupMRate
    ,@Discount_GroupMRate
    ,@Added_Name
    ,@Sub_Package
    ,@C_Start_Date
    ,@D_End_Date
    ,@C_End_Date
    ,@Frequency
    ,@Reach
    ,@Frequency_PostBuy
    ,@Reach_PostBuy
    ,@Master_Package
    ,@Verified
    ,@Usr
    ,@KeyID
    ,@Vendor_Discount
    ,@Vendor_Net_Cost
    ,@CH7_export
    ,@DH_Export
    ,@Spot_verified
    ,@Cost_Item_ID
    ,@Billings_Year
    ,@Billings_Month
    ,@AdjustedAfterActual
    ,@Verified_Date
    ,@Media_Sub_Type
    ,@Agency_Fee
    ,@Show_Date_Original
    ,@BuyTypeName
    ,@BuyTypeID
    ,@Media_Type)
";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Buying_Brief_ID", SqlDbType.NVarChar).Value = dr["Buying_Brief_ID"];
                comm.Parameters.Add("@Market_ID", SqlDbType.NVarChar).Value = dr["Market_ID"];
                comm.Parameters.Add("@Version", SqlDbType.NVarChar).Value = dr["Version"];
                comm.Parameters.Add("@Item", SqlDbType.Int).Value = dr["Item"];
                comm.Parameters.Add("@Show_Date", SqlDbType.NVarChar).Value = dr["Show_Date"];
                comm.Parameters.Add("@ID", SqlDbType.Int).Value = dr["ID"];
                comm.Parameters.Add("@Status", SqlDbType.Int).Value = dr["Status"];
                comm.Parameters.Add("@Media_ID", SqlDbType.NVarChar).Value = dr["Media_ID"];
                comm.Parameters.Add("@Media_Vendor_ID", SqlDbType.NVarChar).Value = dr["Media_Vendor_ID"];
                comm.Parameters.Add("@Start_Time", SqlDbType.NVarChar).Value = dr["Start_Time"];
                comm.Parameters.Add("@End_Time", SqlDbType.NVarChar).Value = dr["End_Time"];
                comm.Parameters.Add("@Program", SqlDbType.NVarChar).Value = dr["Program"];
                comm.Parameters.Add("@WeekdayLimit", SqlDbType.NVarChar).Value = dr["WeekdayLimit"];
                comm.Parameters.Add("@Package", SqlDbType.NVarChar).Value = dr["Package"];
                comm.Parameters.Add("@SizeHW", SqlDbType.NVarChar).Value = dr["SizeHW"];
                comm.Parameters.Add("@Unit", SqlDbType.NVarChar).Value = dr["Unit"];
                comm.Parameters.Add("@Material_Key", SqlDbType.NVarChar).Value = dr["Material_Key"];
                comm.Parameters.Add("@Material_ID", SqlDbType.NVarChar).Value = dr["Material_ID"];
                comm.Parameters.Add("@Length", SqlDbType.Float).Value = dr["Length"];
                comm.Parameters.Add("@Start_date", SqlDbType.NVarChar).Value = dr["Start_date"];
                comm.Parameters.Add("@end_date", SqlDbType.NVarChar).Value = dr["end_date"];
                comm.Parameters.Add("@Deadline_Material", SqlDbType.NVarChar).Value = dr["Deadline_Material"];
                comm.Parameters.Add("@Deadline_Terminate", SqlDbType.NVarChar).Value = dr["Deadline_Terminate"];
                comm.Parameters.Add("@State", SqlDbType.NVarChar).Value = dr["State"];
                comm.Parameters.Add("@Market_Price", SqlDbType.Float).Value = dr["Market_Price"];
                comm.Parameters.Add("@Rate", SqlDbType.Float).Value = dr["Rate"];
                comm.Parameters.Add("@Discount", SqlDbType.Float).Value = dr["Discount"];
                comm.Parameters.Add("@Weight", SqlDbType.Float).Value = dr["Weight"];
                comm.Parameters.Add("@Net_Cost", SqlDbType.Float).Value = dr["Net_Cost"];
                comm.Parameters.Add("@Program_Type", SqlDbType.NVarChar).Value = dr["Program_Type"];
                comm.Parameters.Add("@Prebuy_Start_Time", SqlDbType.NVarChar).Value = dr["Prebuy_Start_Time"];
                comm.Parameters.Add("@Prebuy_End_Time", SqlDbType.NVarChar).Value = dr["Prebuy_End_Time"];
                comm.Parameters.Add("@Rating", SqlDbType.Float).Value = dr["Rating"];
                comm.Parameters.Add("@CPRP_Cost", SqlDbType.Float).Value = dr["CPRP_Cost"];
                comm.Parameters.Add("@Include_Media_Cost", SqlDbType.Bit).Value = dr["Include_Media_Cost"];
                comm.Parameters.Add("@CPM", SqlDbType.Float).Value = dr["CPM"];
                comm.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = dr["Remark"];
                comm.Parameters.Add("@Spots", SqlDbType.Int).Value = dr["Spots"];
                comm.Parameters.Add("@Adept_Code", SqlDbType.Int).Value = dr["Adept_Code"];
                comm.Parameters.Add("@Adept_Export", SqlDbType.Bit).Value = dr["Adept_Export"];
                comm.Parameters.Add("@Adept_Hide", SqlDbType.Bit).Value = dr["Adept_Hide"];
                comm.Parameters.Add("@Adept_Export_Date", SqlDbType.NVarChar).Value = dr["Adept_Export_Date"];
                comm.Parameters.Add("@Appear", SqlDbType.Bit).Value = dr["Appear"];
                comm.Parameters.Add("@Finish", SqlDbType.Bit).Value = dr["Finish"];
                comm.Parameters.Add("@Row_ID", SqlDbType.Int).Value = Convert.ToInt32(dr["Item"]) + 1;  //SqlDbType.Int).Value = dr["Row_ID"];
                comm.Parameters.Add("@Pkg", SqlDbType.Int).Value = dr["Pkg"];
                comm.Parameters.Add("@Booking_Order_ID", SqlDbType.NVarChar).Value = dr["Booking_Order_ID"];
                comm.Parameters.Add("@District", SqlDbType.NVarChar).Value = dr["District"];
                comm.Parameters.Add("@Sub_District", SqlDbType.NVarChar).Value = dr["Sub_District"];
                comm.Parameters.Add("@Site", SqlDbType.NVarChar).Value = dr["Site"];
                comm.Parameters.Add("@Pane", SqlDbType.NVarChar).Value = dr["Pane"];
                comm.Parameters.Add("@Color", SqlDbType.NVarChar).Value = dr["Color"];
                comm.Parameters.Add("@Program_Code", SqlDbType.Decimal).Value = dr["Program_Code"];
                comm.Parameters.Add("@Surcharge", SqlDbType.Real).Value = dr["Surcharge"];
                comm.Parameters.Add("@Bonus_Percent", SqlDbType.Real).Value = dr["Bonus_Percent"];
                comm.Parameters.Add("@Bonus_Cost", SqlDbType.Real).Value = dr["Bonus_Cost"];
                comm.Parameters.Add("@Invoice_Number", SqlDbType.NVarChar).Value = dr["Invoice_Number"];
                comm.Parameters.Add("@Program_Referrence_Code", SqlDbType.NVarChar).Value = dr["Program_Referrence_Code"];
                comm.Parameters.Add("@Reschedule_Status", SqlDbType.NVarChar).Value = dr["Reschedule_Status"];
                comm.Parameters.Add("@Resch_BO", SqlDbType.NVarChar).Value = dr["Resch_BO"];
                comm.Parameters.Add("@TOTAL", SqlDbType.Float).Value = dr["TOTAL"];
                comm.Parameters.Add("@Province", SqlDbType.NVarChar).Value = dr["Province"];
                comm.Parameters.Add("@Bonus_Status", SqlDbType.Int).Value = dr["Bonus_Status"];
                comm.Parameters.Add("@GroupRating", SqlDbType.Float).Value = dr["GroupRating"];
                comm.Parameters.Add("@GroupCPRP", SqlDbType.Float).Value = dr["GroupCPRP"];
                comm.Parameters.Add("@Total_Gross", SqlDbType.Real).Value = dr["Total_Gross"];
                comm.Parameters.Add("@Ad_Tax", SqlDbType.Real).Value = dr["Ad_Tax"];
                comm.Parameters.Add("@VAT", SqlDbType.Bit).Value = dr["VAT"];
                comm.Parameters.Add("@CD_Percent", SqlDbType.Real).Value = dr["CD_Percent"];
                comm.Parameters.Add("@OMI", SqlDbType.Bit).Value = dr["OMI"];
                comm.Parameters.Add("@Format_Type_ID", SqlDbType.NVarChar).Value = dr["Format_Type_ID"];
                comm.Parameters.Add("@Export_Booking_Order_Flag", SqlDbType.Int).Value = dr["Export_Booking_Order_Flag"];
                comm.Parameters.Add("@Spending_Type", SqlDbType.Int).Value = dr["Spending_Type"];
                comm.Parameters.Add("@Original_Rating", SqlDbType.Float).Value = dr["Original_Rating"];
                comm.Parameters.Add("@Buffer_TVR_State", SqlDbType.Bit).Value = dr["Buffer_TVR_State"];
                comm.Parameters.Add("@Cash_Discount", SqlDbType.Real).Value = dr["Cash_Discount"];
                comm.Parameters.Add("@Position_In_Break", SqlDbType.NVarChar).Value = dr["Position_In_Break"];
                comm.Parameters.Add("@Ratecard", SqlDbType.Float).Value = dr["Ratecard"];
                comm.Parameters.Add("@Discount_Ratecard", SqlDbType.Float).Value = dr["Discount_Ratecard"];
                comm.Parameters.Add("@MarketRate", SqlDbType.Float).Value = dr["MarketRate"];
                comm.Parameters.Add("@Discount_MarketRate", SqlDbType.Float).Value = dr["Discount_MarketRate"];
                comm.Parameters.Add("@GroupMRate", SqlDbType.Float).Value = dr["GroupMRate"];
                comm.Parameters.Add("@Discount_GroupMRate", SqlDbType.Float).Value = dr["Discount_GroupMRate"];
                comm.Parameters.Add("@Added_Name", SqlDbType.NVarChar).Value = dr["Added_Name"];
                comm.Parameters.Add("@Sub_Package", SqlDbType.NVarChar).Value = dr["Sub_Package"];
                comm.Parameters.Add("@C_Start_Date", SqlDbType.NVarChar).Value = dr["C_Start_Date"];
                comm.Parameters.Add("@D_End_Date", SqlDbType.NVarChar).Value = dr["D_End_Date"];
                comm.Parameters.Add("@C_End_Date", SqlDbType.NVarChar).Value = dr["C_End_Date"];
                comm.Parameters.Add("@Frequency", SqlDbType.NVarChar).Value = dr["Frequency"];
                comm.Parameters.Add("@Reach", SqlDbType.Float).Value = dr["Reach"];
                comm.Parameters.Add("@Frequency_PostBuy", SqlDbType.NVarChar).Value = dr["Frequency_PostBuy"];
                comm.Parameters.Add("@Reach_PostBuy", SqlDbType.Float).Value = dr["Reach_PostBuy"];
                comm.Parameters.Add("@Master_Package", SqlDbType.NVarChar).Value = dr["Master_Package"];
                comm.Parameters.Add("@Verified", SqlDbType.Bit).Value = dr["Verified"];
                comm.Parameters.Add("@Usr", SqlDbType.NVarChar).Value = dr["Usr"];
                comm.Parameters.Add("@KeyID", SqlDbType.NVarChar).Value = dr["KeyID"];
                comm.Parameters.Add("@Vendor_Discount", SqlDbType.Float).Value = dr["Vendor_Discount"];
                comm.Parameters.Add("@Vendor_Net_Cost", SqlDbType.Float).Value = dr["Vendor_Net_Cost"];
                comm.Parameters.Add("@CH7_export", SqlDbType.Bit).Value = dr["CH7_export"];
                comm.Parameters.Add("@DH_Export", SqlDbType.Bit).Value = dr["DH_Export"];
                comm.Parameters.Add("@Spot_verified", SqlDbType.Bit).Value = dr["Spot_verified"];
                comm.Parameters.Add("@Cost_Item_ID", SqlDbType.NVarChar).Value = dr["Cost_Item_ID"];
                comm.Parameters.Add("@Billings_Year", SqlDbType.NVarChar).Value = dr["Billings_Year"];
                comm.Parameters.Add("@Billings_Month", SqlDbType.NVarChar).Value = dr["Billings_Month"];
                comm.Parameters.Add("@AdjustedAfterActual", SqlDbType.Bit).Value = dr["AdjustedAfterActual"];
                comm.Parameters.Add("@Verified_Date", SqlDbType.NVarChar).Value = dr["Verified_Date"];
                comm.Parameters.Add("@Media_Sub_Type", SqlDbType.NVarChar).Value = dr["Media_Sub_Type"];
                comm.Parameters.Add("@Agency_Fee", SqlDbType.Real).Value = dr["Agency_Fee"];
                comm.Parameters.Add("@Show_Date_Original", SqlDbType.NVarChar).Value = dr["Show_Date_Original"];
                comm.Parameters.Add("@BuyTypeName", SqlDbType.NVarChar).Value = dr["BuyTypeName"];
                comm.Parameters.Add("@BuyTypeID", SqlDbType.NVarChar).Value = dr["BuyTypeID"];
                comm.Parameters.Add("@Media_Type", SqlDbType.NVarChar).Value = dr["Media_Type"];

                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }

        }
        public int InsertSpotPlanPaymentDetail(DataRow dr,double Net_Cost,double Agency_Fee_Cost,double Payment_Term,double Net_Cost_BeforePrintAdviseNote)
        {
            try
            {
                string strSQL = @"INSERT INTO [dbo].[spot_plan_payment]
           ([buying_brief_id]
           ,[market_id]
           ,[version]
           ,[item]
           ,[show_date]
           ,[id]
           ,[status]
           ,[Payment_Term]
           ,[Province]
           ,[Verified]
           ,[Usr]
           ,[KeyID]
           ,[Spot_verified]
           ,[Verified_Date]
           ,[Net_Cost]
           ,[Agency_Fee_Cost]
           ,[Net_Cost_BeforePrintAdviseNote])
     VALUES
           (@Buying_Brief_ID
           ,@Market_ID
           ,@Version
           ,@Item
           ,@Show_Date
           ,@ID
           ,@Status
           ,@Payment_Term
           ,@Province
           ,@Verified
           ,@Usr
           ,@KeyID
           ,@Spot_verified
           ,@Verified_Date
           ,@Net_Cost
           ,@Agency_Fee_Cost
           ,@Net_Cost_BeforePrintAdviseNote)

SELECT SCOPE_IDENTITY();
";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Buying_Brief_ID", SqlDbType.NVarChar).Value = dr["Buying_Brief_ID"];
                comm.Parameters.Add("@Market_ID", SqlDbType.NVarChar).Value = dr["Market_ID"];
                comm.Parameters.Add("@Version", SqlDbType.NVarChar).Value = dr["Version"];
                comm.Parameters.Add("@Item", SqlDbType.Int).Value = dr["Item"];
                comm.Parameters.Add("@Show_Date", SqlDbType.NVarChar).Value = dr["Show_Date"]; //SqlDbType.NVarChar).Value = dr["Show_Date"];
                comm.Parameters.Add("@ID", SqlDbType.Int).Value = dr["ID"];
                comm.Parameters.Add("@Status", SqlDbType.Int).Value = dr["Status"];
                comm.Parameters.Add("@Payment_Term", SqlDbType.Float).Value = Payment_Term;
                comm.Parameters.Add("@Province", SqlDbType.NVarChar).Value = dr["Province"];
                comm.Parameters.Add("@Verified", SqlDbType.Bit).Value = dr["Verified"];
                comm.Parameters.Add("@Usr", SqlDbType.NVarChar).Value = dr["Usr"];
                comm.Parameters.Add("@KeyID", SqlDbType.NVarChar).Value = dr["KeyID"];
                comm.Parameters.Add("@Verified_Date", SqlDbType.NVarChar).Value = dr["Verified_Date"];
                comm.Parameters.Add("@Spot_verified", SqlDbType.Bit).Value = dr["Spot_verified"];
                comm.Parameters.Add("@Net_Cost", SqlDbType.Float).Value = Net_Cost;
                comm.Parameters.Add("@Agency_Fee_Cost", SqlDbType.Float).Value = Agency_Fee_Cost;
                comm.Parameters.Add("@Net_Cost_BeforePrintAdviseNote", SqlDbType.Float).Value = Net_Cost_BeforePrintAdviseNote;

                

                int iIDKey = Convert.ToInt32(comm.ExecuteScalar());
                return iIDKey;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return 0;
            }

        }
        public bool DeleteSpotPlanPaymentDetail(int iIDKey)
        {
            try
            {
                string strSQL = @"DELETE FROM [dbo].[spot_plan_payment] WHERE IDKey = @IDKey";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@IDKey", SqlDbType.Int).Value = iIDKey;
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }

        }
        public bool DeleteSpotPlanPaymentDetailIFNetCostIsZero(int iIDKey)
        {
            try
            {
                string strSQL = @"DELETE FROM [dbo].[spot_plan_payment] WHERE IDKey = @IDKey and isnull(Net_Cost,0) = 0";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@IDKey", SqlDbType.Int).Value = iIDKey;
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }

        }
        public bool UpdateSpotPlanPaymentDetail(int iIDKey,double Net_Cost, double Agency_Fee_Cost, double Payment_Term)
        {
            try
            {
                string strSQL = @"
UPDATE [dbo].[spot_plan_payment]
SET [Net_Cost] =@Net_Cost
,[Agency_Fee_Cost]=@Agency_Fee_Cost
,[Payment_Term]=@Payment_Term
WHERE [IDKey] = @IDKey
";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Net_Cost", SqlDbType.Float).Value = Net_Cost;
                comm.Parameters.Add("@Agency_Fee_Cost", SqlDbType.Float).Value = Agency_Fee_Cost;
                comm.Parameters.Add("@Payment_Term", SqlDbType.Float).Value = Payment_Term;
                comm.Parameters.Add("@IDKey", SqlDbType.BigInt).Value = iIDKey;

                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }

        }
        public bool UpdateSpotPlanPaymentNet_Cost_BeforePrintAdviseNote(int iIDKey)
        {
            try
            {
                string strSQL = @"
UPDATE [dbo].[spot_plan_payment]
SET [Net_Cost_BeforePrintAdviseNote] =[Net_Cost]
WHERE [IDKey] = @IDKey
";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@IDKey", SqlDbType.BigInt).Value = iIDKey;

                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }

        }

        public bool InsertSpotPlanVersion(string BBID, string Version, int Status, int Spots, string User, double Cost)
        {
            try
            {
                string strSQL = @"
INSERT INTO [dbo].[Spot_Plan_Version]
               ([Buying_Brief_ID]
               ,[Market_ID]
               ,[Version]
               ,[GRP]
               ,[Revised_No]
               ,[Spots]
               ,[Approve]
               ,[Buyer_ID]
               ,[Population]
               ,[Cost]
               ,[CPRP]
               ,[GRP_Prime]
               ,[Normal_CPRP]
               ,[Highlight])
         VALUES
               (@Buying_Brief_ID,
                'THAILAND',
                @Version,
                0,
                1,
                @Spots,
                @Approve,
                @Buyer_ID,
                0,
                @Cost,
                0,
                0,
                0,
                'False')
";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@Buying_Brief_ID", SqlDbType.NVarChar).Value = BBID;
                comm.Parameters.Add("@Version", SqlDbType.NVarChar).Value = Version;
                comm.Parameters.Add("@Approve", SqlDbType.NVarChar).Value = Status;
                comm.Parameters.Add("@Spots", SqlDbType.Int).Value = Spots;
                comm.Parameters.Add("@Cost", SqlDbType.Float).Value = Cost;
                comm.Parameters.Add("@Buyer_ID", SqlDbType.NVarChar).Value = User;

                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }
        }

        public bool UpdateSpotPlanRemark(string BBID, string MSRemark, string PORemark, string ANRemark)
        {
            try
            {
                string strSQL = @"update Buying_Brief_Market 
                set Comment_Vendor = @PORemark, 
                Comment_Client = @MSRemark, 
                Comment_Advice_Note = @ANRemark 
                where Buying_Brief_ID = @BBID";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@BBID", SqlDbType.VarChar).Value = BBID;
                comm.Parameters.Add("@MSRemark", SqlDbType.VarChar).Value = MSRemark;
                comm.Parameters.Add("@PORemark", SqlDbType.VarChar).Value = PORemark;
                comm.Parameters.Add("@ANRemark", SqlDbType.VarChar).Value = ANRemark;
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }
        }

        public bool UpdateSpotPlanEdit(DataRow dr)
        {
            try
            {
                string strSQL = @"update Spot_Plan_Edit 
                set Team = @Team 
                where Buying_Brief_ID = @BBID 
                and Item = @Item 
                and Kind = @Kind 
                and Team = @Old_Team";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@BBID", SqlDbType.VarChar).Value = dr["Buying_Brief_ID"];
                comm.Parameters.Add("@Team", SqlDbType.VarChar).Value = dr["Team"];
                comm.Parameters.Add("@Item", SqlDbType.VarChar).Value = dr["Item"];
                comm.Parameters.Add("@Kind", SqlDbType.VarChar).Value = dr["Kind"];
                comm.Parameters.Add("@Old_Team", SqlDbType.VarChar).Value = dr["Old_Team"];
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }
        }

        public bool UpdateMediaSubTypeAndAgencyFee(string strBB)
        {
            try
            {
                string strSQL = @"Minder_Thai_UpdateMediaSubTypeAndAgencyFee";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("@BB", SqlDbType.VarChar).Value = strBB;
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }
        }

        //Modified by Chaiwat.i 25/08/2021 : Update status,actual cost to Buying brief / Update status,actual cost,spot(s) to Buying brief Market 
        public bool UpdateBuyingBrief(string BBID, int Status, double Cost)
        {
            try
            {
                string strSQL = @"update Buying_Brief 
                set Version_Approve = @Version_Approve, 
                Actual_Cost = @Actual_Cost 
                where Buying_Brief_ID = @BBID";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@BBID", SqlDbType.VarChar).Value = BBID;
                comm.Parameters.Add("@Version_Approve", SqlDbType.VarChar).Value = Status;
                comm.Parameters.Add("@Actual_Cost", SqlDbType.VarChar).Value = Cost;
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }
        }

        public bool UpdateBuyingBriefMarket(string BBID, int Status, double Cost, int Spots)
        {
            try
            {
                string strSQL = @"update Buying_Brief_Market
                set Approve_Version = @Approve_Version, 
                Actual_Cost = @Actual_Cost,
                Spots = @Spots
                where Buying_Brief_ID = @BBID";
                SqlCommand comm = new SqlCommand(strSQL, m_connMinder);
                comm.Parameters.Add("@BBID", SqlDbType.VarChar).Value = BBID;
                comm.Parameters.Add("@Approve_Version", SqlDbType.VarChar).Value = Status;
                comm.Parameters.Add("@Actual_Cost", SqlDbType.VarChar).Value = Cost;
                comm.Parameters.Add("@Spots", SqlDbType.VarChar).Value = Spots;
                comm.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return false;
            }
        }
    }
}
