using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupM.Minder
{
    public partial class Implementation_SpotPlan_Control_Numberic : Form
    {
        public Implementation_SpotPlan_Control_Numberic()
        {
            InitializeComponent();
        }

        private void txtValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}
