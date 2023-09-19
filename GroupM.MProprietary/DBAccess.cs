using GroupM.UTL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MProprietary
{
    public class DBAccess
    {
        private static string cText = "GroupMThailand555#";
        private SqlConnection con;

        public DBAccess()
        {
            con = new SqlConnection(ConnectionStringMinder);
            OpenConnection();
        }

        private string DecryptConnection(string connection, string encrypt)
        {
            string decrypt = Crypto.DecryptStringAES(encrypt, cText);
            connection = connection.Replace(encrypt, decrypt);
            return connection;
        }

        public string ConnectionStringMinder
        {
            get
            {
                string conStr = ConfigurationManager.ConnectionStrings["Minder"].ToString();
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(conStr);
                conStr = DecryptConnection(conStr, builder.UserID);
                conStr = DecryptConnection(conStr, builder.Password);
                return conStr;
            }
        }

        private void OpenConnection()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertLog(string description, string action)
        {
            try
            {
                string sql = @"insert into Log (User_ID, Action_Time, Description, System_Description, Action_Name, Ad_Login, PC_name) 
                values (@User_ID, @Action_Time, @Description, @System_Description, @Action_Name, @Ad_Login, @PC_name)";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add("@User_ID", SqlDbType.NVarChar).Value = Program.Username;
                cmd.Parameters.Add("@Action_Time", SqlDbType.NVarChar).Value = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = description;
                cmd.Parameters.Add("@System_Description", SqlDbType.NVarChar).Value = "MProprietary";
                cmd.Parameters.Add("@Action_Name", SqlDbType.NVarChar).Value = action;
                cmd.Parameters.Add("@Ad_Login", SqlDbType.NVarChar).Value = Environment.UserName;
                cmd.Parameters.Add("@PC_name", SqlDbType.NVarChar).Value = SystemInformation.ComputerName;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region ProprietaryGroup
        public DataTable SelectProprietaryGroup()
        {
            try
            {
                string sql = @"select * from GroupProprietary order by GroupProprietaryName";
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        public DataTable SelectProprietaryGroup(int GroupPropId)
        {
            try
            {
                string sql = @"select * 
                from GroupProprietary 
                where GroupProprietaryId = @GroupProprietaryId";
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.Parameters.Add("@GroupProprietaryId", SqlDbType.Int).Value = GroupPropId;
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        public DataTable SelectProprietaryGroup(string GroupPropName)
        {
            try
            {
                string sql = @"select * 
                from GroupProprietary 
                where GroupProprietaryName = @GroupProprietaryName";
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.Parameters.Add("@GroupProprietaryName", SqlDbType.NVarChar).Value = GroupPropName;
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        public DataTable SelectProprietaryGroup(string OldName, string NewName)
        {
            try
            {
                string sql = @"select * from (
	                select GroupProprietaryName 
	                from GroupProprietary 
	                where GroupProprietaryName <> @OldName
                ) as TB 
                where TB.GroupProprietaryName = @NewName";
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.Parameters.Add("@OldName", SqlDbType.NVarChar).Value = OldName;
                sda.SelectCommand.Parameters.Add("@NewName", SqlDbType.NVarChar).Value = NewName;
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        public DataTable SelectProprietaryGroupInClient(int GroupPropId)
        {
            try
            {
                string sql = @"select * 
                from GroupProprietaryClientMapping 
                where GroupProprietaryId = @GroupProprietaryId";
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.Parameters.Add("@GroupProprietaryId", SqlDbType.Int).Value = GroupPropId;
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        public void InsertProprietaryGroup(string GroupPropName, string GroupPropropDesc, string ContractType)
        {
            try
            {
                string sql = @"insert into GroupProprietary (GroupProprietaryName, GroupProprietaryDescription, ContractType) 
                values (@GroupProprietaryName, @GroupProprietaryDescription, @ContractType)";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add("@GroupProprietaryName", SqlDbType.NVarChar).Value = GroupPropName;
                cmd.Parameters.Add("@GroupProprietaryDescription", SqlDbType.NVarChar).Value = GroupPropropDesc;
                cmd.Parameters.Add("@ContractType", SqlDbType.NVarChar).Value = ContractType;
                cmd.ExecuteNonQuery();
                // Log
                InsertLog("Create new proprietary group (" + GroupPropName + ")", "Create proprietary group");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void UpdateProprietaryGroup(int GroupPropId, string oldGroupName, string GroupPropName, string GroupPropropDesc, string ContractType)
        {
            try
            {
                string sql = @"update GroupProprietary 
                set GroupProprietaryName = @GroupProprietaryName, GroupProprietaryDescription = @GroupProprietaryDescription , ContractType = @ContractType 
                where GroupProprietaryId = @GroupProprietaryId";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add("@GroupProprietaryId", SqlDbType.Int).Value = GroupPropId;
                cmd.Parameters.Add("@GroupProprietaryName", SqlDbType.NVarChar).Value = GroupPropName;
                cmd.Parameters.Add("@GroupProprietaryDescription", SqlDbType.NVarChar).Value = GroupPropropDesc;
                cmd.Parameters.Add("@ContractType", SqlDbType.NVarChar).Value = ContractType;
                cmd.ExecuteNonQuery();
                // Log
                if (oldGroupName != GroupPropName)
                {
                    InsertLog("Update proprietary group name change from (" + oldGroupName + ") to (" + GroupPropName + ")", "Update proprietary group");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void DeleteProprietaryGroup(int GroupPropId)
        {
            try
            {
                DataTable tb = SelectProprietaryGroup(GroupPropId);
                string GroupPropName = tb.Rows[0]["GroupProprietaryName"].ToString();
                string[] sql = new string[2];
                SqlCommand cmd;
                sql[0] = @"delete from GroupProprietary 
                where GroupProprietaryId = @GroupProprietaryId";
                sql[1] = @"delete from GroupProprietaryVendorMapping 
                where GroupProprietaryId = @GroupProprietaryId";
                for (int i = 0; i < 2; i++)
                {
                    cmd = new SqlCommand(sql[i], con);
                    cmd.Parameters.Add("@GroupProprietaryId", SqlDbType.Int).Value = GroupPropId;
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                // Log
                InsertLog("delete proprietary group (" + GroupPropName + ")", "Delete proprietary group");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public DataTable SelectProprietaryMapping(int GroupPropId)
        {
            try
            {
                string sql = @"select gpv.*, 
                mv.Short_Name as Media_Vendor_Name, 
                m.Short_Name as Media_Name 
                from GroupProprietaryVendorMapping as gpvm 
                inner join GroupProprietaryVendor as gpv 
                on gpvm.GroupProprietaryVendorId = gpv.GroupProprietaryVendorId 
                inner join Media_Vendor as mv 
                on gpv.Media_Vendor_ID = mv.Media_Vendor_ID 
                inner join Media as m 
                on gpv.Media_ID = m.Media_ID 
                where gpvm.GroupProprietaryId = @GroupProprietaryId 
                order by mv.Short_Name, m.Short_Name";
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.Parameters.Add("@GroupProprietaryId", SqlDbType.Int).Value = GroupPropId;
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        public DataTable SelectProprietary(int GroupProprietaryVendorId)
        {
            try
            {
                string sql = @"select * 
                from GroupProprietaryVendor 
                where GroupProprietaryVendorId = @GroupProprietaryVendorId";
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.Parameters.Add("@GroupProprietaryVendorId", SqlDbType.Int).Value = GroupProprietaryVendorId;
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        public void InsertProprietaryMapping(int GroupPropId, int GroupProprietaryVendorId)
        {
            try
            {
                string sql = @"insert into GroupProprietaryVendorMapping (GroupProprietaryId, GroupProprietaryVendorId) 
                values (@GroupProprietaryId, @GroupProprietaryVendorId)";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add("@GroupProprietaryId", SqlDbType.Int).Value = GroupPropId;
                cmd.Parameters.Add("@GroupProprietaryVendorId", SqlDbType.Int).Value = GroupProprietaryVendorId;
                cmd.ExecuteNonQuery();
                // Log
                DataTable tb1 = SelectProprietaryGroup(GroupPropId);
                DataTable tb2 = SelectProprietary(GroupProprietaryVendorId);
                string GroupPropName = tb1.Rows[0]["GroupProprietaryName"].ToString();
                string VendorID = tb2.Rows[0]["Media_Vendor_ID"].ToString();
                string MediaID = tb2.Rows[0]["Media_ID"].ToString();
                InsertLog("Add proprietary permission : media vendor id (" + VendorID + "), media id (" + MediaID + ") to proprietary group (" + GroupPropName + ")", "Add proprietary permission to group");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void DeleteProprietaryMapping(int GroupPropId, int GroupProprietaryVendorId)
        {
            try
            {
                string sql = @"delete from GroupProprietaryVendorMapping
                where GroupProprietaryId = @GroupProprietaryId 
                and GroupProprietaryVendorId = @GroupProprietaryVendorId";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add("@GroupProprietaryId", SqlDbType.Int).Value = GroupPropId;
                cmd.Parameters.Add("@GroupProprietaryVendorId", SqlDbType.Int).Value = GroupProprietaryVendorId;
                cmd.ExecuteNonQuery();
                // Log
                DataTable tb1 = SelectProprietaryGroup(GroupPropId);
                DataTable tb2 = SelectProprietary(GroupProprietaryVendorId);
                string GroupPropName = tb1.Rows[0]["GroupProprietaryName"].ToString();
                string VendorID = tb2.Rows[0]["Media_Vendor_ID"].ToString();
                string MediaID = tb2.Rows[0]["Media_ID"].ToString();
                InsertLog("Delete proprietary permission : media vendor id (" + VendorID + "), media id (" + MediaID + ") from proprietary group (" + GroupPropName + ")", "Delete proprietary permission from group");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public DataTable SelectClientMapping(int GroupPropId)
        {
            try
            {
                string sql = @"select gpcm.Client_Id, c.Short_Name 
                from GroupProprietaryClientMapping gpcm 
                inner join Client c 
                on gpcm.Client_Id = c.Client_ID 
                where c.Opt_in_Signed = 'True' 
                and gpcm.GroupProprietaryId = @GroupProprietaryId 
                order by c.Short_Name";
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.Parameters.Add("@GroupProprietaryId", SqlDbType.Int).Value = GroupPropId;
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        public bool SelectClientOptIn(string ClientId)
        {
            try
            {
                bool optIn = false;
                string sql = @"select isnull(Opt_in_Signed, 'False') as Opt_in_Signed 
                from Client 
                where Client_Id = @Client_Id";
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.Parameters.Add("@Client_Id", SqlDbType.NVarChar).Value = ClientId;
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    optIn = false;
                }
                else
                {
                    bool flagOptIn = Convert.ToBoolean(dt.Rows[0]["Opt_in_Signed"].ToString());
                    optIn = flagOptIn;
                }
                return optIn;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public DataTable SelectProprietaryGroupInClient(string ClientId)
        {
            try
            {
                string sql = @"select * 
                from GroupProprietaryClientMapping 
                where Client_Id = @Client_Id";
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.Parameters.Add("@Client_Id", SqlDbType.NVarChar).Value = ClientId;
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        public void InsertClientMapping(int GroupPropId, string ClientId)
        {
            try
            {
                string sql = @"insert into GroupProprietaryClientMapping (GroupProprietaryId, Client_Id) 
                values (@GroupProprietaryId, @Client_Id)";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add("@GroupProprietaryId", SqlDbType.Int).Value = GroupPropId;
                cmd.Parameters.Add("@Client_Id", SqlDbType.NVarChar).Value = ClientId;
                cmd.ExecuteNonQuery();
                // update check box minder on screen minder
                bool checkOptIn = SelectClientOptIn(ClientId);
                if (checkOptIn == false)
                {
                    sql = @"update Client 
                    set Opt_in_Signed = 'True' 
                    where Client_ID = @Client_Id";
                    cmd = new SqlCommand(sql, con);
                    cmd.Parameters.Add("@Client_Id", SqlDbType.NVarChar).Value = ClientId;
                    cmd.ExecuteNonQuery();
                }
                // Log
                DataTable tb = SelectProprietaryGroup(GroupPropId);
                string GroupPropName = tb.Rows[0]["GroupProprietaryName"].ToString();
                InsertLog("Add proprietary group (" + GroupPropName + ") to client id (" + ClientId + ")", "Add proprietary group to client");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void DeleteClientMapping(int GroupPropId, string ClientId)
        {
            try
            {
                string sql = @"delete from GroupProprietaryClientMapping
                where GroupProprietaryId = @GroupProprietaryId 
                and Client_Id = @Client_Id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add("@GroupProprietaryId", SqlDbType.Int).Value = GroupPropId;
                cmd.Parameters.Add("@Client_Id", SqlDbType.NVarChar).Value = ClientId;
                cmd.ExecuteNonQuery();
                // update check box minder on screen minder
                DataTable listProprietaryGroup = SelectProprietaryGroupInClient(ClientId);
                if (listProprietaryGroup.Rows.Count == 0)
                {
                    sql = @"update Client 
                    set Opt_in_Signed = 'False' 
                    where Client_ID = @Client_Id";
                    cmd = new SqlCommand(sql, con);
                    cmd.Parameters.Add("@Client_Id", SqlDbType.NVarChar).Value = ClientId;
                    cmd.ExecuteNonQuery();
                }
                // Log
                DataTable tb = SelectProprietaryGroup(GroupPropId);
                string GroupPropName = tb.Rows[0]["GroupProprietaryName"].ToString();
                InsertLog("Delete proprietary group (" + GroupPropName + ") from client id (" + ClientId + ")", "Delete proprietary group from client");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public DataTable SelectProprietaryList(string strfileter)
        {
            try
            {
                string sql = @"select gpv.*, 
                mv.Short_Name as Media_Vendor_Name, 
                m.Short_Name as Media_Name 
                from GroupProprietaryVendor as gpv 
                inner join Media_Vendor as mv 
                on gpv.Media_Vendor_ID = mv.Media_Vendor_ID 
                inner join Media as m 
                on gpv.Media_ID = m.Media_ID 
                where charindex(',' + convert(varchar(10), gpv.GroupProprietaryVendorId) + ',', @GroupProprietaryVendorId) = 0 
                order by mv.Short_Name, m.Short_Name";
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.Parameters.Add("@GroupProprietaryVendorId", SqlDbType.VarChar).Value = strfileter;
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        public DataTable SelectClientList(string strfileter)
        {
            try
            {
                string sql = @"select Agency_ID, Office_ID, Client_ID, Short_Name 
                from Client 
                where InactiveClient = '0' 
                and charindex(',' + convert(varchar(10), Client_ID) + ',', @Client_ID) = 0 
                order by Short_Name";
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.Parameters.Add("@Client_ID", SqlDbType.VarChar).Value = strfileter;
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }
        #endregion

        #region Client
        public DataTable SelectClient()
        {
            try
            {
                string sql = @"select * from Client where InactiveClient = '0'";
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        public DataTable SelectProprietaryGroupMapping(string ClientId)
        {
            try
            {
                string sql = @"select gp.* 
                from GroupProprietary gp 
                inner join GroupProprietaryClientMapping gpcm 
                on gp.GroupProprietaryId = gpcm.GroupProprietaryId 
                where gpcm.Client_Id = @Client_Id 
                order by gp.GroupProprietaryName";
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.Parameters.Add("@Client_Id", SqlDbType.NVarChar).Value = ClientId;
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        public DataTable SelectProprietaryDetailMapping(string ClientId)
        {
            try
            {
                string sql = @"select MainTB.Media_Vendor_ID, MainTB.Media_Vendor_Name, MainTB.Media_ID, MainTB.Media_Name, MainTB.TotalGroup as GroupProprietaryName 
                from (
	                select TB.*, stuff((
		                select ', ' + SubTB.GroupProprietaryName 
		                from (
			                select tbgp.GroupProprietaryName, tbgpv.GroupProprietaryVendorId, tbgpcm.Client_Id 
			                from GroupProprietary as tbgp 
			                inner join GroupProprietaryClientMapping as tbgpcm 
			                on tbgp.GroupProprietaryId = tbgpcm.GroupProprietaryId 
			                inner join GroupProprietaryVendorMapping as tbgpvm 
			                on tbgpcm.GroupProprietaryId = tbgpvm.GroupProprietaryId 
			                inner join GroupProprietaryVendor as tbgpv 
			                on tbgpvm.GroupProprietaryVendorId = tbgpv.GroupProprietaryVendorId
		                ) as SubTB 
		                where SubTB.GroupProprietaryVendorId = TB.GroupProprietaryVendorId 
		                and SubTB.Client_Id = TB.Client_Id 
                        order by SubTB.GroupProprietaryName 
		                for xml path('')), 1, 2, ''
	                ) as TotalGroup 
	                from (
		                select gp.GroupProprietaryName, gpcm.Client_Id, gpv.*, mv.Short_Name as Media_Vendor_Name, m.Short_Name as Media_Name 
		                from GroupProprietary as gp 
		                inner join GroupProprietaryClientMapping as gpcm 
		                on gp.GroupProprietaryId = gpcm.GroupProprietaryId 
		                inner join GroupProprietaryVendorMapping as gpvm 
		                on gpcm.GroupProprietaryId = gpvm.GroupProprietaryId 
		                inner join GroupProprietaryVendor as gpv 
		                on gpvm.GroupProprietaryVendorId = gpv.GroupProprietaryVendorId 
		                inner join Media_Vendor as mv 
		                on gpv.Media_Vendor_ID = mv.Media_Vendor_ID 
                        inner join Media as m 
		                on gpv.Media_ID = m.Media_ID
	                ) as TB
                ) as MainTB 
                where MainTB.Client_Id = @Client_Id 
                group by MainTB.Media_Vendor_ID, MainTB.Media_Vendor_Name, MainTB.Media_ID, MainTB.Media_Name, MainTB.TotalGroup 
                order by MainTB.Media_Vendor_Name, MainTB.Media_Name";
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.Parameters.Add("@Client_Id", SqlDbType.NVarChar).Value = ClientId;
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        public DataTable SelectProprietaryDetailMappingRefresh(string strfileter)
        {
            try
            {
                string sql = @"select MainTB.Media_Vendor_ID, MainTB.Media_Vendor_Name, MainTB.Media_ID, MainTB.Media_Name, MainTB.TotalGroup as GroupProprietaryName 
                from (
	                select TB.*, stuff((
			            select ', ' + SubTB.GroupProprietaryName 
			            from (
				            select tbgp.GroupProprietaryId, tbgp.GroupProprietaryName, tbgpv.GroupProprietaryVendorId 
				            from GroupProprietary as tbgp 
				            inner join GroupProprietaryVendorMapping as tbgpvm 
				            on tbgp.GroupProprietaryId = tbgpvm.GroupProprietaryId 
				            inner join GroupProprietaryVendor as tbgpv 
				            on tbgpvm.GroupProprietaryVendorId = tbgpv.GroupProprietaryVendorId
			            ) as SubTB 
			            where SubTB.GroupProprietaryVendorId = TB.GroupProprietaryVendorId 
			            and charindex(',' + convert(varchar(10), SubTB.GroupProprietaryId) + ',', @GroupProprietaryId) > 0 
                        order by SubTB.GroupProprietaryName 
			            for xml path('')), 1, 2, ''
		            ) as TotalGroup
	                from (
		                select gp.GroupProprietaryId, gp.GroupProprietaryName, gpv.*, mv.Short_Name as Media_Vendor_Name, m.Short_Name as Media_Name 
		                from GroupProprietary as gp 
		                inner join GroupProprietaryVendorMapping as gpvm 
		                on gp.GroupProprietaryId = gpvm.GroupProprietaryId 
		                inner join GroupProprietaryVendor as gpv 
		                on gpvm.GroupProprietaryVendorId = gpv.GroupProprietaryVendorId 
		                inner join Media_Vendor as mv 
		                on gpv.Media_Vendor_ID = mv.Media_Vendor_ID 
                        inner join Media as m 
		                on gpv.Media_ID = m.Media_ID
	                ) as TB
                ) as MainTB
                where charindex(',' + convert(varchar(10), MainTB.GroupProprietaryId) + ',', @GroupProprietaryId) > 0 
                group by MainTB.Media_Vendor_ID, MainTB.Media_Vendor_Name, MainTB.Media_ID, MainTB.Media_Name, MainTB.TotalGroup 
                order by MainTB.Media_Vendor_Name, MainTB.Media_Name";
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.Parameters.Add("@GroupProprietaryId", SqlDbType.NVarChar).Value = strfileter;
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        public DataTable SelectProprietaryGroupList(string strfileter)
        {
            try
            {
                string sql = @"select * 
                from GroupProprietary 
                where charindex(',' + convert(varchar(10), GroupProprietaryId) + ',', @GroupProprietaryId) = 0 
                order by GroupProprietaryName";
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.Parameters.Add("@GroupProprietaryId", SqlDbType.VarChar).Value = strfileter;
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }
        #endregion

        #region Proprietary
        public DataTable SelectProprietaryMaster()
        {
            try
            {
                string sql = @"select gpv.*, 
                mv.Short_Name as Media_Vendor_Name, 
                m.Short_Name as Media_Name 
                from GroupProprietaryVendor as gpv 
                inner join Media_Vendor as mv 
                on gpv.Media_Vendor_ID = mv.Media_Vendor_ID 
                inner join Media as m 
                on gpv.Media_ID = m.Media_ID 
                order by mv.Short_Name, m.Short_Name";
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        public DataTable SelectProprietaryMaster(string VendorID, string MediaID)
        {
            try
            {
                string sql = @"select * 
                from GroupProprietaryVendor 
                where Media_Vendor_ID = @Media_Vendor_ID 
                and Media_ID = @Media_ID";
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.Parameters.Add("@Media_Vendor_ID", SqlDbType.NVarChar).Value = VendorID;
                sda.SelectCommand.Parameters.Add("@Media_ID", SqlDbType.NVarChar).Value = MediaID;
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        public DataTable SelectProprietaryInGroup(int GroupProprietaryVendorId)
        {
            try
            {
                string sql = @"select * 
                from GroupProprietaryVendorMapping 
                where GroupProprietaryVendorId = @GroupProprietaryVendorId";
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                sda.SelectCommand.Parameters.Add("@GroupProprietaryVendorId", SqlDbType.Int).Value = GroupProprietaryVendorId;
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        public void InsertProprietaryMaster(string VendorID, string MediaID)
        {
            try
            {
                string sql = @"insert into GroupProprietaryVendor (Media_Vendor_ID, Media_ID) 
                values (@Media_Vendor_ID, @Media_ID)";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add("@Media_Vendor_ID", SqlDbType.NVarChar).Value = VendorID;
                cmd.Parameters.Add("@Media_ID", SqlDbType.NVarChar).Value = MediaID;
                cmd.ExecuteNonQuery();
                // Log
                InsertLog("Add new proprietary permission : media vendor id (" + VendorID + ") and media id (" + MediaID + ")", "Create proprietary");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void DeleteProprietaryMaster(int GroupProprietaryVendorId)
        {
            try
            {
                DataTable tb1 = SelectProprietary(GroupProprietaryVendorId);
                string VendorID = tb1.Rows[0]["Media_Vendor_ID"].ToString();
                string MediaID = tb1.Rows[0]["Media_ID"].ToString();
                string sql = @"delete from GroupProprietaryVendor
                where GroupProprietaryVendorId = @GroupProprietaryVendorId";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.Add("@GroupProprietaryVendorId", SqlDbType.Int).Value = GroupProprietaryVendorId;
                cmd.ExecuteNonQuery();
                // Log
                InsertLog("Delete proprietary permission : media vendor id (" + VendorID + ") and media id (" + MediaID + ")", "Delete proprietary");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public DataTable SelectVendorList()
        {
            try
            {
                string sql = @"select * 
                from Media_Vendor 
                where Inactive = 0 
                and GPM_Vendor = 1 
                order by Short_Name";
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }

        public DataTable SelectMediaList()
        {
            try
            {
                string sql = @"select Media.Media_ID, 
                Media.Short_Name as Media_Name, 
                Media_Sub_Type.Media_Sub_Type as Media_Sub_Type_ID, 
                Media_Sub_Type.Short_Name as Media_Sub_Type_Name, 
                Media_Type.Media_Type as Media_Type_ID, 
                Media_Type.Short_Name as Media_Type_Name, 
                case 
                when UseMedia.Media_ID is null 
                then 'New' 
                else '' 
                end as Status 
                from Media 
                inner join Media_Sub_Type 
                on Media.Media_Sub_Type = Media_Sub_Type.Media_Sub_Type 
                inner join Media_Type 
                on Media_Sub_Type.Media_Type = Media_Type.Media_Type 
                left outer join (
	                select distinct Media_ID 
	                from GroupProprietaryVendor
                ) as UseMedia 
                on Media.Media_ID = UseMedia.Media_ID 
                where Media.Valid = 1 
                and Media.isOptin = 1 
                order by Media_Type.Short_Name, 
                Media_Sub_Type.Short_Name, 
                Media.Short_Name";
                SqlDataAdapter sda = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataTable();
            }
        }
        #endregion

    }
}
