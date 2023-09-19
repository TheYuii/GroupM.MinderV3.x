using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GroupM.UTL;

namespace GroupM.App.ExportPivotTable
{
    public partial class ExportPivot : Form
    {
        public ExportPivot()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DataControl DataAccess = new DataControl();
            ExportPivotExcel.ExportToExcelFile(DataAccess.GetSummaryCD("", ""), "Vendor Name");
        }

        private void btnExportTVBuyingPattern_Click(object sender, EventArgs e)
        {
            if (this.txtPathFileHeader.Text.Trim() == string.Empty || this.txtPathFileDetial.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please select all file.");
                this.btnExportTVBuyingPattern.Focus();
                return;
            }
            this.Hide();
            ExportPivotExcel.ExportTVBuyingPattern(this.txtPathFileHeader.Text, this.txtPathFileDetial.Text);
            this.Show();
        }

        private void btnBrowseHeader_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files|*.txt";
                    if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        this.txtPathFileHeader.Text = ofd.FileName;
                    }
                }
            }
            catch (Exception ex)
            {
                #region exception
                if (ex.InnerException != null)
                {
                    MessageBox.Show("System Error :" + ex.InnerException.Message.ToString() + "\r\n" + ex.InnerException.StackTrace.ToString());
                }
                else
                {
                    MessageBox.Show("System Error :" + ex.Message.ToString() + "\r\n" + ex.StackTrace.ToString());
                }
                #endregion
            }
        }

        private void btnBrowseDetail_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files|*.txt";
                    if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        this.txtPathFileDetial.Text = ofd.FileName;
                    }
                }
            }
            catch (Exception ex)
            {
                #region exception
                if (ex.InnerException != null)
                {
                    MessageBox.Show("System Error :" + ex.InnerException.Message.ToString() + "\r\n" + ex.InnerException.StackTrace.ToString());
                }
                else
                {
                    MessageBox.Show("System Error :" + ex.Message.ToString() + "\r\n" + ex.StackTrace.ToString());
                }
                #endregion
            }
        }
    }
}
