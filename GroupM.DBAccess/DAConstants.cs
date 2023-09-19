using GroupM.UTL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupM.DBAccess
{
    public class DAContants
    {
        private static string cText = "GroupMThailand555#";
        public static string ConnectionString { get { 
                //return ConfigurationManager.ConnectionStrings["MainApp"].ToString();
                string connString = ConfigurationManager.ConnectionStrings["MainApp"].ToString();

                System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(connString);
                string useren = builder.UserID;
                string userde = Crypto.DecryptStringAES(builder.UserID, cText);
                connString = connString.Replace(useren, userde);

                string passen = builder.Password;
                string passde = Crypto.DecryptStringAES(builder.Password, cText);
                connString = connString.Replace(passen, passde);

                return connString;
            } }
        public static string ConnectionStringMPAServerConfig { get { 
                //return ConfigurationManager.ConnectionStrings["MPAServerConfig"].ToString();

                string connString = ConfigurationManager.ConnectionStrings["MPAServerConfig"].ToString();

                System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(connString);
                string useren = builder.UserID;
                string userde = Crypto.DecryptStringAES(builder.UserID, cText);
                connString = connString.Replace(useren, userde);

                string passen = builder.Password;
                string passde = Crypto.DecryptStringAES(builder.Password, cText);
                connString = connString.Replace(passen, passde);

                return connString;
            } }
        public static string ConnectionStringMinder { get { 
                
                //return ConfigurationManager.ConnectionStrings["Minder"].ToString();
                string connString = ConfigurationManager.ConnectionStrings["Minder"].ToString();

                System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(connString);
                string useren = builder.UserID;
                string userde = Crypto.DecryptStringAES(builder.UserID, cText);
                connString = connString.Replace(useren, userde);

                string passen = builder.Password;
                string passde = Crypto.DecryptStringAES(builder.Password, cText);
                connString = connString.Replace(passen, passde);

                return connString;

            } }
    }
}
