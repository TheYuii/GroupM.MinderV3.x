using GroupM.DBAccess;
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

namespace GroupM.App
{
    public partial class frmReport : Form
    {

        public frmReport()
        {
            InitializeComponent();
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void tsbSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                string strCommand = " SELECT * FROM UserProfile ";
                DBManager db = new DBManager();

                DataTable dt = db.SelectNoParameter(strCommand);
                this.gvDetail.DataSource = dt;



            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Exception Message", MessageBoxButtons.OK);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void tsbExport_Click(object sender, EventArgs e)
        {
            try
            {
                //this.Cursor = Cursors.WaitCursor;

                string strCommand = " SELECT * FROM UserProfile ";
                DBManager db = new DBManager();

                DataTable dt = db.SelectNoParameter(strCommand);

                ExportPivotExcel.ExportToExcelFile(dt, "UserName");


            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Exception Message", MessageBoxButtons.OK);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

    }
}
