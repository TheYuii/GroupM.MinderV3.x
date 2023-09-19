using GroupM.UTL;
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
    public partial class Master_ClientBrand : Form
    {
        public Master_ClientBrand()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtBrandCode.Text == ""
                || txtBrandName.Text == "")
            {
                GMessage.MessageWarning("Please input all data");
                return;
            }
            DialogResult = DialogResult.OK;
        }
    }
}
