using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MProprietary
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void proprietaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProprietary frm = new frmProprietary();
            frm.Show();
        }

        private void clientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmClient frm = new frmClient();
            frm.Show();
        }

        private void masterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProprietaryMaster frm = new frmProprietaryMaster();
            frm.Show();
        }
    }
}
