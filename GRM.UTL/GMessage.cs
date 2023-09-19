using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GRM.UTL
{
    public class GMessage 
    {
        public static DialogResult MessageComfirm(string strText) {
            return MessageBox.Show(strText, "Comfirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        }
        public static DialogResult MessageWarning(string strText)
        {
            return MessageBox.Show(strText, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public static DialogResult MessageInfo(string strText)
        {
            return MessageBox.Show(strText, "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static void MessageError(Exception ex)
        {
            MessageBox.Show(ex.Message, "Exception Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void MessageError(string strText)
        {
            MessageBox.Show(strText, "Exception Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    
}
