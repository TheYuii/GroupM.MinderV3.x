using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GroupM.UTL
{
    public class ControlManipulate
    {

        public static void SetEnable(bool bEnable, params object[] args)
        {
            foreach (object oControl in args)
            {
                ((Control)oControl).Enabled = bEnable;
            }
        }
        public static void ClearControl(params object[] args)
        {
            foreach (object oControl in args)
            {
                ((Control)oControl).Text = string.Empty;
            }
        }
    }
}
