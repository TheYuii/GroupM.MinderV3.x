using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace  GroupM.Minder
{
    public partial class MPA008_TVCalendar_Material : Form
    {
        public MPA008_TVCalendar_Material()
        {
            InitializeComponent();
            ar.Add("A");
            ar.Add("B");
            ar.Add("C");
            ar.Add("D");
            ar.Add("E");
            ar.Add("F");
            ar.Add("G");
            ar.Add("H");
            ar.Add("I");
            ar.Add("J");
            ar.Add("K");
            ar.Add("L");
            ar.Add("M");
            ar.Add("N");
            ar.Add("O");
            ar.Add("P");
            ar.Add("Q");
            ar.Add("R");
            ar.Add("S");
            ar.Add("U");
            ar.Add("V");
            ar.Add("W");
            ar.Add("X");
            ar.Add("Y");
            ar.Add("Z");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Yes;
            Close();
        }

        private void chkCheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gvDetail.Rows.Count; i++)
            {
                gvDetail.Rows[i].Cells[0].Value = true;
            }
        }

        private void btnUncheck_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gvDetail.Rows.Count; i++)
            {
                gvDetail.Rows[i].Cells[0].Value = false;
            }
        }

        private void MPA008_TVCalendar_Material_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < gvDetail.Rows.Count; i++)
            {
                gvDetail.Rows[i].Cells[0].Value = true;
            }
        }
        ArrayList ar = new ArrayList();
        
        private void btnReRunMaterail_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gvDetail.Rows.Count; i++)
            {
                gvDetail.Rows[i].Cells[3].Value = ar[i];
            }
        }
    }
}
