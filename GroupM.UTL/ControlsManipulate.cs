using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GRM.UTL
{
    public class ControlsManipulate
    {

        public static void SetEnable(bool bEnable, params object[] args)
        {
            foreach (object oControl in args)
            {
                ((Control)oControl).Enabled = bEnable;
            }
        }
    }
}
