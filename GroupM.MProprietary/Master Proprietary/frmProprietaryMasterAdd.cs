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
    public partial class frmProprietaryMasterAdd : Form
    {
        public frmProprietaryMasterAdd()
        {
            InitializeComponent();
        }

        private DBAccess connect = new DBAccess();

        private void SelectVendor()
        {
            frmVendorList frm = new frmVendorList();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtVendorID.Text = frm.gvDetail.SelectedRows[0].Cells[0].Value.ToString();
                txtVendorName.Text = frm.gvDetail.SelectedRows[0].Cells[1].Value.ToString();
            }
        }

        private void SelectMedia()
        {
            frmMediaList frm = new frmMediaList();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtMediaID.Text = frm.gvDetail.SelectedRows[0].Cells[0].Value.ToString();
                txtMediaName.Text = frm.gvDetail.SelectedRows[0].Cells[1].Value.ToString();
            }
        }

        private void btnAddVendor_Click(object sender, EventArgs e)
        {
            SelectVendor();
        }

        private void btnAddMedia_Click(object sender, EventArgs e)
        {
            SelectMedia();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtVendorID.Text == "" || txtMediaID.Text == "")
            {
                if (txtVendorID.Text == "" && txtMediaID.Text == "")
                {
                    MessageBox.Show("Please choose media vendor and media!", "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnAddVendor.Focus();
                }
                else
                {
                    if (txtVendorID.Text == "")
                    {
                        MessageBox.Show("Please choose media vendor!", "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        btnAddVendor.Focus();
                    }
                    if (txtMediaID.Text == "")
                    {
                        MessageBox.Show("Please choose media!", "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        btnAddMedia.Focus();
                    }
                }
            }
            else
            {
                DataTable dt = connect.SelectProprietaryMaster(txtVendorID.Text, txtMediaID.Text);
                if (dt.Rows.Count == 0)
                {
                    connect.InsertProprietaryMaster(txtVendorID.Text, txtMediaID.Text);
                    MessageBox.Show("Done.", "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Dispose();
                }
                else
                {
                    MessageBox.Show("Cannot save, Because this proprietary is already exist!", "MProprietary", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void txtVendorID_Click(object sender, EventArgs e)
        {
            SelectVendor();
        }

        private void txtMediaID_Click(object sender, EventArgs e)
        {
            SelectMedia();
        }
    }
}
