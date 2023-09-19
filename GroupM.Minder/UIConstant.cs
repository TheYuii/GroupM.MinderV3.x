using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace GRM.MPA
{
    public class UIConstant
    {

        public static string ConnectionStringPath { get { return ConfigurationManager.AppSettings["ConnectionStringPath"]; } }
        public static string ConnectionStringMinder { get { return ConfigurationManager.ConnectionStrings["MPAServerConfig"].ToString(); } }
        public static string ConnectionStringMPA { get { return ConfigurationManager.ConnectionStrings["MPA"].ToString(); } }

        public static string TEMPALTE_WEEKLY_TRACKING_REPORT = ConfigurationManager.AppSettings["TemplateWeeklyTrackingReport"].ToString();
        public static string TEMPALTE_MONTHLY_TV_SCHEDULE_SUPPLIER_REPORT = ConfigurationManager.AppSettings["TemplateMonthlyTVScheduleSupplierReport"].ToString();
        public static string USERID = "";
        public static string PASSWORD = "";

    }
}
