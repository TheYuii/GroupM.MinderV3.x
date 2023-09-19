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

namespace GRM.UTL
{
    public class UserPermisstion
    {
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
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SourceConnectionString"].ConnectionString);
            string strSQL = string.Format("SELECT TOP 1 1 FROM Users WHERE User_ID=@User_ID AND Password_TXT=@Password");
            SqlDataAdapter sda = new SqlDataAdapter(strSQL, conn);
            sda.SelectCommand.Parameters.Add("@User_ID", SqlDbType.VarChar).Value = strUserName;
            sda.SelectCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = strPassword;
            DataSet ds = new DataSet();
            sda.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
                return false;
            return true;
        }
        
        public static bool AuthenticateUserDomain(string usr, string pwd)
        {

            using (LdapConnection con = GetLdapConnection())
            {

                try
                {
                    con.Bind(new NetworkCredential(usr, pwd, ConfigurationManager.AppSettings["Domain"].ToString()));
                }
                catch (Exception ex)
                {
                    return false;
                }

            }

            return true;

        }
    }
}
