using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GroupM.UTL;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Principal;

namespace  GroupM.Minder
{
    public partial class MPA001_Login : Form
    {
        public MPA001_Login()
        {
            InitializeComponent();
        }

        private bool m_bPermission;
        public bool Permission
        {
            get { return m_bPermission; }
        }
        private string m_strUser;
        public string UserName
        {
            get { return m_strUser; }
            set { m_strUser = value; }
        }
        private string m_strPass;
        public string Password
        {
            get { return m_strPass; }
            set { m_strPass = value; }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            m_bPermission = false;
            this.Close();
        }
        public void CheckPermisstion()
        {
            try
            {

                //txtUserName.Text = "siriya.v";
                //txtPassword.Text = "fank";

                //if (txtPassword.Text == "")
                //{
                    if (!UserPermisstion.AuthenticateUserDomain(txtUserName.Text, txtPassword.Text))
                    {
                        GMessage.MessageWarning("Username and Password are not match.\nPlease use username\\password for accessing your computer.\nExample: Firstname.L");
                        txtPassword.Focus();
                    }
                    else
                    {
                        CreateConstants();
                    }
                //}
                //else
                //{
                //    CreateConstants();
                //}
            }
            catch
            {
                throw;
            }

        }
        private void CreateConstants()
        {
            UserName = txtUserName.Text;
            Password = txtPassword.Text;
            Connection.USERID = UserName;
            Connection.PASSWORD = Password;
            m_bPermission = true;
            //Connection.ConnectionStringMinder = XMLUtil.GetConneectionString("Minder", Connection.ConnectionStringPath, @"databases/bkksqlp01102");
            Connection.ConnectionStringMinder = "Data Source=BKKSQLP01102\\SQLINS02_2008R2;database=Minder_Thai;User id=bkkit;Password=Groupm#03;";
            SqlConnection conn = new SqlConnection(Connection.ConnectionStringMinder);
            SqlCommand comm = new SqlCommand(@"
                    INSERT INTO Log ([User_ID],[Action_Time],[Description],[System_Description],[Action_Name],[Ad_Login],[PC_name])
                    SELECT UPPER('" + UserName + "'),convert(varchar(10),getdate(),111)+'" + DateTime.Now.ToString(" HH:mm") + @"','Log In','MPA','Login','" + UserName + "','" + System.Environment.MachineName + "'"
                                    , conn);
            conn.Open();
            comm.ExecuteNonQuery();
            conn.Close();



            if (Connection.ConnectionStringMPA == null)
            {
                Connection.ConnectionStringMPA = ConfigurationManager.ConnectionStrings["MPA"].ToString();
            }
            SqlConnection conn2 = new SqlConnection(Connection.ConnectionStringMPA);
            SqlCommand comm2 = new SqlCommand(@"
                    INSERT INTO Log ([User_ID],[Action_Time],[Description],[System_Description],[Action_Name],[Ad_Login],[PC_name])
                    SELECT UPPER('" + UserName + "'),convert(varchar(10),getdate(),111)+'" + DateTime.Now.ToString(" HH:mm") + @"','Log In','MPA','Login','" + UserName + "','" + System.Environment.MachineName + "'"
                                    , conn2);
            conn2.Open();
            comm2.ExecuteNonQuery();
            conn2.Close();

            this.Close();



        }
        private void btnLogIn_Click(object sender, EventArgs e)
        {
            CheckPermisstion();
        }
        private void txtUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLogIn.PerformClick();
            }
        }
        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLogIn.PerformClick();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MPA001_Login_Load(object sender, EventArgs e)
        {
            m_bPermission = false;

            this.Text = "Minder Post-Buy Analysis - Version : " + Application.ProductVersion;

            string tmpUserName = WindowsIdentity.GetCurrent().Name;
            string[] arUserName = tmpUserName.Split('\\');
            if (arUserName.Length > 1)
                txtUserName.Text = arUserName[1];
            else
                txtUserName.Text = arUserName[0];
            txtPassword.Focus();

            //txtUserName.Text = "kornchaya.c";
            //txtPassword.Text = "fank";
            //btnLogIn.PerformClick();

        }
    }
}