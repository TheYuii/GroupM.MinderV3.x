using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupM.UTL
{
    public class DateTimeConvertUtil
    {
        public static void DateTimeSetEmpty(System.Windows.Forms.DateTimePicker dtp)
        {
            dtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            dtp.CustomFormat = " ";
            dtp.Value = DateTime.FromOADate(0);
            dtp.Enabled = false;
        }


        public static void DateTimeRestoreFormat(System.Windows.Forms.DateTimePicker dtp)
        {
            dtp.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtp.Value = DateTime.Now;
            dtp.Enabled = true;
        }

        public static DateTime DateTimeConvertyyyyMMddToDateTime(string strYyyyMMdd)
        {
            int y = Convert.ToInt32(strYyyyMMdd.Substring(0, 4));
            int m = Convert.ToInt32(strYyyyMMdd.Substring(4, 2));
            int d = Convert.ToInt32(strYyyyMMdd.Substring(6, 2));
            return new DateTime(y,m,d);
        }
        public static DateTime DateTimeConvertdd_MM_yyyyWithToDateTime(string strYyyyMMdd)
        {
            int y = Convert.ToInt32(strYyyyMMdd.Substring(6, 4));
            int m = Convert.ToInt32(strYyyyMMdd.Substring(3, 2));
            int d = Convert.ToInt32(strYyyyMMdd.Substring(0, 2));
            return new DateTime(y, m, d);
        }
    }
}
