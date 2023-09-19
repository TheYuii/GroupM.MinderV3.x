using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupM.Minder.SpotPlanIT
{
    #region DatabaseTypes
    public enum DatabaseTypes
    {
        MicrosoftSqlServer,
        Oracle,
        MicrosoftAccess
    }
    #endregion


    public class GlobalVariable
    {
        //Application Config
        //public static Icon AppIcon { get { return global::GroupM.App.Properties.Resources.App; } }
        // User
        public static int UserProfileID { get; set; }
        public static string UserName { get; set; }

        public static bool DevlopmentMode = false;

        // Application Version
        public static string Version
        {
            get
            {
                string strVersion = "";
                if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                {
                    System.Deployment.Application.ApplicationDeployment ad = System.Deployment.Application.ApplicationDeployment.CurrentDeployment;
                    strVersion = ad.CurrentVersion.ToString();
                }
                else
                {
                    strVersion = Application.ProductVersion;
                };
                return strVersion;
            }
        }


        public static string DatabaseName
        {
            get 
            {
                string[] strTmp = ConfigurationManager.ConnectionStrings["Minder"].ToString().Split('\\');
                return  strTmp[strTmp.Length - 1].Split('=')[1].Split(';')[0];
            }
        }
        public static string DatabaseServer
        {
            get
            {
                string[] strTmp = ConfigurationManager.ConnectionStrings["Minder"].ToString().Split('\\');
                return strTmp[0].Split('=')[1] + "\\" + strTmp[strTmp.Length - 1].Split('=')[0].Split(';')[0];

            }
        }
        public static string SupportEmail
        {
            get { return ConfigurationManager.AppSettings["SupportEmail"].ToString(); }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class AppConfig
    {

        private const int CONNECTION_TIMEOUT = 60;//second(s)        

        #region Private member
        private static String m_sApplicationPath = String.Empty;

        private AppConfig() { }


        private static string ConfigFileName
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + "config.ini";
            }
        }
        #endregion

        #region ConfigName
        private class ConfigName
        {
            #region Database
            public class Database
            {
                //	Section Name
                public const string SECTION = "Database";
                //	Entry Name [Oracle]
                public const string HOST_STRING = "Host String";
                //	Entry Name [SQL]
                public const string SERVER_NAME = "Database Server";
                public const string DATABASE_NAME = "Database Name";
                //	Entry Both
                public const string LOGIN_NAME = "Login Name";
                public const string PASSWORD = "Password";
            }
            #endregion

            #region Application
            public class Application
            {
                public const string SECTION = "Application";
                public const string LOCATION = "Location";
                public const string WINDOW_STATE = "Window State";
                public const string SIZE = "Size";
                public const string DATABASE_TYPE = "DatabaseType";
                public const string REPORT_PATH = "Report Path";
                public const string MIGRATION_PATH = "Migration Path";
                public const string ICON_PATH = "Icon Path";
            }
            #endregion
        }
        #endregion

        #region Properties
        public static string ApplicationPath
        {
            get
            {
                return m_sApplicationPath;
            }
            set
            {
                m_sApplicationPath = value;
            }
        }

        public static string IconPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + @"Icon\";
            }
        }

        public static string ServerName
        {
            get { return ""; }
            //set{	Instance.SetValue(ConfigName.Database.SECTION, ConfigName.Database.SERVER_NAME, value);	}
        }
        public static DatabaseTypes DatabaseType
        {
            get
            {
                return DatabaseTypes.MicrosoftSqlServer;

            }
        }
        public static Point Location
        {
            get
            {
                return new Point(0, 0);
            }
        }
        public static System.Drawing.Size Size
        {
            get
            {
                return SystemInformation.WorkingArea.Size;
            }
        }
        public static FormWindowState WindowState
        {
            get
            {
                return FormWindowState.Maximized;

            }
        }
        #endregion

    }


    public static class Connection
    {
        public static string ConnectionStringPath { get; set; }
        public static string ConnectionStringMinder { get; set; }
        public static string ConnectionStringMPA { get; set; }

        public static string TEMPALTE_WEEKLY_TRACKING_REPORT { get; set; }
        public static string TEMPALTE_MONTHLY_TV_SCHEDULE_SUPPLIER_REPORT { get; set; }


        public static string USERID = "";
        public static string PASSWORD = "";

    }
}
