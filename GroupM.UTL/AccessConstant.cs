using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace GroupM.UTL
{
    class AccessConstant
    {
        public static string MPA { get { return ConfigurationManager.ConnectionStrings["MPA"].ToString(); } }
    }
}
