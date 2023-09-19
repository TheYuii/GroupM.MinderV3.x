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
    public partial class Master_VendorContractPeriod : Form
    {

        public Master_VendorContractPeriod()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (dtStartDate.Value > dtEndDate.Value)
            {
                GMessage.MessageWarning("Period Date To should be greater than Date From.");
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void Master_VendorContractPeriod_Load(object sender, EventArgs e)
        {
            rdYes.Checked = true;
            dtStartDate.Value = new DateTime(DateTime.Now.Year, 1, 1);
            dtEndDate.Value = new DateTime(DateTime.Now.Year, 1, 1).AddYears(1).AddDays(-1);
        }
    }
}
