using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GroupM.App
{
    public partial class frmBaseDisplay : Form
    {
        public frmBaseDisplay()
        {
            InitializeComponent();
        }

        private void frmBaseDisplay_Load(object sender, EventArgs e)
        {

        }
        
        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {
            Control ctl = (Panel)sender;
            Pen p = new Pen(SystemBrushes.ActiveCaption);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            e.Graphics.DrawLine(p, 0, ctl.Height - 1, ctl.Width, ctl.Height - 1);
        }
    }
}
