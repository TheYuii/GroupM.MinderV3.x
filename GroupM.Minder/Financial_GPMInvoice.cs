using GroupM.DBAccess;
using GroupM.UTL;
using Excel = Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Configuration;

namespace GroupM.Minder
{
    public partial class Financial_GPMInvoice : Form
    {
        private string reportPath = ConfigurationManager.AppSettings["ReportMinder"];
        DBManager m_db;

        public Financial_GPMInvoice()
        {
            InitializeComponent();
            m_db = new DBManager();
        }

        private void InitControl()
        {
            // variable
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int lastDay = DateTime.DaysInMonth(year, month);
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = new DateTime(year, month, lastDay);
            // set control value
            dtStartDate.Value = startDate;
            dtEndDate.Value = endDate;
            txtAgencyCode.Text = "GPM";
            txtAgency.Text = "GroupM Proprietary Media Co.,Ltd";
        }

        private void Financial_GPMInvoice_Load(object sender, EventArgs e)
        {
            try
            {
                InitControl();
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex.Message);
            }
        }

        private void txtAgencyCode_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Agency_ID", "Short_Name", "Agency", false, "", "");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtAgencyCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
                txtAgency.Text = frm.SelectedGridRow["MasterName"].ToString();
            }
        }

        private void txtAgencyCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtAgencyCode_TextChanged(object sender, EventArgs e)
        {
            if (txtAgencyCode.Text == "")
            {
                txtAgency.Text = "";
            }
        }

        private void txtClientCode_Click(object sender, EventArgs e)
        {
            Utility_SearchPopup_CodeName frm = new Utility_SearchPopup_CodeName("Client_ID", "Short_Name", "Client", false, "", "");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtClientCode.Text = frm.SelectedGridRow["MasterCode"].ToString();
                txtClient.Text = frm.SelectedGridRow["MasterName"].ToString();
            }
        }

        private void txtClientCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void txtClientCode_TextChanged(object sender, EventArgs e)
        {
            if (txtClientCode.Text == "")
            {
                txtClient.Text = "";
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dtStartDate.Value > dtEndDate.Value)
            {
                GMessage.MessageWarning("End date must more than or equal start date.");
            }
            else
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    // declare variable
                    Excel.Application excel = new Excel.Application();
                    excel.Visible = false;
                    Excel.Workbook workbook = (Excel.Workbook)(excel.Workbooks.Open(reportPath + "GPM Invoice Report.xltx"));
                    Excel.Worksheet worksheet = workbook.Worksheets[1];
                    // template head detail
                    DataTable table = m_db.SelectGPMInvoiceReport(dtStartDate.Value.ToString("yyyyMM"), dtEndDate.Value.ToString("yyyyMM"), txtAgencyCode.Text, txtClientCode.Text);
                    worksheet.Cells[1, 2] = txtAgency.Text;
                    worksheet.Cells[2, 2] = "Actual";
                    worksheet.Cells[3, 2] = dtStartDate.Value.ToString("yyyy/MM/dd") + "~" + dtEndDate.Value.ToString("yyyy/MM/dd");
                    worksheet.Cells[4, 2] = txtClient.Text == "" ? "All" : txtClient.Text;
                    // template data table
                    int i = 0;
                    string[,] RawData = new string[table.Rows.Count, 10];
                    double[,] RawData1 = new double[table.Rows.Count, 1];
                    // set data to array
                    for (i = 0; i < table.Rows.Count; i++)
                    {
                        RawData[i, 0] = table.Rows[i]["Original_BB"].ToString();
                        RawData[i, 1] = table.Rows[i]["Buying_Brief_ID"].ToString();
                        RawData[i, 2] = table.Rows[i]["Product_Code"].ToString();
                        RawData[i, 3] = table.Rows[i]["Program_Name"].ToString();
                        RawData[i, 4] = table.Rows[i]["Media_Type"].ToString();
                        RawData[i, 5] = table.Rows[i]["Media_Sub_Type"].ToString();
                        RawData[i, 6] = table.Rows[i]["Client_Code"].ToString();
                        RawData[i, 7] = table.Rows[i]["Client_Name"].ToString();
                        RawData[i, 8] = table.Rows[i]["Product_Name"].ToString();
                        RawData[i, 9] = table.Rows[i]["Package_Name"].ToString();
                        RawData1[i, 0] = Convert.ToDouble(table.Rows[i]["Net_Cost"].ToString());
                        if (i < table.Rows.Count - 2)
                        {
                            worksheet.Rows[i + 9].Insert();
                        }
                        GC.Collect();
                    }
                    if (table.Rows.Count > 0)
                    {
                        Excel.Range range;
                        Excel.Range range1;
                        range = worksheet.get_Range("A8", Missing.Value);
                        range = range.get_Resize(table.Rows.Count, 10);
                        range1 = worksheet.get_Range("K8", Missing.Value);
                        range1 = range1.get_Resize(table.Rows.Count, 1);
                        // put data to excel
                        range.set_Value(Missing.Value, RawData);
                        range1.set_Value(Missing.Value, RawData1);
                    }
                    excel.Visible = true;
                }
                catch (Exception ex)
                {
                    GMessage.MessageError(ex.Message);
                }
                this.Cursor = Cursors.Default;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
