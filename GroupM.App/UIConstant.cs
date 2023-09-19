using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using GroupM.UTL;

namespace GroupM.App
{
    public class UIConstant
    {
        private static string cText = "GroupMThailand555#";

        public static string ConnectionStringPath { get { 
                //return ConfigurationManager.AppSettings["ConnectionStringPath"];

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
        public static string ConnectionStringMPA { 
            get { 
                
                //return ConfigurationManager.ConnectionStrings["MPA"].ToString();


                string connString = ConfigurationManager.ConnectionStrings["MPA"].ToString();

                System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(connString);
                string useren = builder.UserID;
                string userde = Crypto.DecryptStringAES(builder.UserID, cText);
                connString = connString.Replace(useren, userde);

                string passen = builder.Password;
                string passde = Crypto.DecryptStringAES(builder.Password, cText);
                connString = connString.Replace(passen, passde);

                return connString;
                //return ConfigurationManager.ConnectionStrings["OMIMInter"].ToString(); 


            }
        }

        public static string TEMPALTE_WEEKLY_TRACKING_REPORT = ConfigurationManager.AppSettings["TemplateWeeklyTrackingReport"].ToString();
        public static string TEMPALTE_MONTHLY_TV_SCHEDULE_SUPPLIER_REPORT = ConfigurationManager.AppSettings["TemplateMonthlyTVScheduleSupplierReport"].ToString();
        public static string USERID = "";
        public static string PASSWORD = "";
        public static bool DevelopmentMode = false;

    }
}
