using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace  GroupM.Minder
{
    public partial class Import_AGB_File : Form
    {
        string FileName;
        DataAccess SQLDataAccess = new DataAccess();
        LoadingScreen ShowLoadingScreen = new LoadingScreen();
        private System.Threading.Thread StartThread;
        private System.Threading.Thread StopThread;
        string targetName = "";

        public Import_AGB_File()
        {
            InitializeComponent();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            string txtPathFile = "";
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files|*.txt;";
                    if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        txtPathFile = ofd.FileName;
                        FileName = ofd.SafeFileName;
                    }
                }
                if (txtPathFile != "")
                {
                    this.btnImport.Text = "Importing . . .";
                    this.btnImport.BackColor = Color.OrangeRed;
                    this.btnImport.ForeColor = Color.White;

                    var reader = new StreamReader(File.OpenRead(txtPathFile));

                    //"Channel","Day part","Prog Type","Programme","Day Of Week\Variables","000s per Spot"
                    List<string> listChannel = new List<string>();
                    List<string> listDayPart = new List<string>();
                    List<string> listProgType = new List<string>();
                    List<string> listProgramme = new List<string>();
                    List<string> listDayOfWeekVariables = new List<string>();
                    List<string> listPerSpot = new List<string>();
                    string checkTarget = "";;
                    
                    int countChk = 0;
                    int retChk = 0;
                    while (!reader.EndOfStream)
                    {
                        //var line = reader.ReadLine();
                        //var values = line.Split(',');
                        var line = reader.ReadLine();
                        

                        string[] strOperation = new string[] { "\"," };
                        var values = line.Split(strOperation, StringSplitOptions.None);

                        if (values[0].Replace("\"", "").Equals("Target") || checkTarget == "Target")
                        {
                            if (checkTarget == "")
                                checkTarget = "Target";
                                countChk++;

                            if (targetName == "" && checkTarget != "" && retChk == 1)
                            {
                                targetName = values[0].Replace("\"", "");
                                this.txtTargetName.Text = targetName;
                            }
                                

                            retChk = countChk;

                            if (targetName != "" && checkTarget != "" && retChk > 3)
                            {
                                string channel = values[0].Replace("\"", "");
                                listChannel.Add(channel.Replace(",", ""));

                                string DayPart = values[1].Replace("\"", "");
                                listDayPart.Add(DayPart.Replace(",", ""));

                                string ProgType = values[2].Replace("\"", "");
                                listProgType.Add(ProgType.Replace(",", ""));

                                string Programme = values[3].Replace("\"", "");
                                listProgramme.Add(Programme.Replace(",", ""));

                                string DayOfWeekVariables = values[4].Replace("\"", "");
                                listDayOfWeekVariables.Add(DayOfWeekVariables.Replace(",", ""));

                                string PerSpot = values[5].Replace("\"", "");
                                listPerSpot.Add(PerSpot.Replace(",", ""));

                                Application.DoEvents();
                            }
                            
                        }
                    }
                    if (listChannel.Count() == 0)
                    {
                        MessageBox.Show("Import fail . . ."
                                    + Environment.NewLine
                                    + "Please input data in this format"
                                    + Environment.NewLine
                                    + "\"Channel\""
                                    + Environment.NewLine
                                    + "\"Day part\""
                                    + Environment.NewLine
                                    + "\"Prog Type\""
                                    + Environment.NewLine
                                    + "\"Programme\""
                                    + Environment.NewLine
                                    + "\"Day Of Week\\Variables\""
                                    + Environment.NewLine
                                    + "\"000s per Spot\"",
                                    "Import CSV",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                        this.btnImport.Text = "Import";
                        this.btnImport.BackColor = SystemColors.Control;
                        this.btnImport.ForeColor = Color.Black;
                        return;
                    }
                    int retCheck = SQLDataAccess.ImportCSVConnString(listChannel, listDayPart, listProgType, 
                                                                    listProgramme, listDayOfWeekVariables, listPerSpot, FileName);
                    if (retCheck == 1)
                    {
                        MessageBox.Show("Import Success...", "Import CSV", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.btnImport.Text = "Import Complete...";
                        this.btnImport.BackColor = Color.GreenYellow;
                        this.btnImport.ForeColor = Color.Black;
                        this.txtTargetName.Enabled = true;
                        this.btnExportCSV.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Import fail . . ."
                                    + Environment.NewLine
                                    + "Please input data in this format"
                                    + Environment.NewLine
                                    + "\"Channel\""
                                    + Environment.NewLine
                                    + "\"Day part\""
                                    + Environment.NewLine
                                    + "\"Prog Type\""
                                    + Environment.NewLine
                                    + "\"Programme\""
                                    + Environment.NewLine
                                    + "\"Day Of Week\\Variables\""
                                    + Environment.NewLine
                                    + "\"000s per Spot\"",
                                    "Import CSV",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                        this.btnImport.Text = "Import";
                        this.btnImport.BackColor = SystemColors.Control;
                        this.btnImport.ForeColor = Color.Black;
                    }
                }
                else
                {
                    MessageBox.Show("Please choose file to import.", "Import CSV", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Import fail . . ."
                                    + Environment.NewLine
                                    + "Please input data in this format"
                                    + Environment.NewLine
                                    + "\"Channel\""
                                    + Environment.NewLine
                                    + "\"Day part\""
                                    + Environment.NewLine
                                    + "\"Prog Type\""
                                    + Environment.NewLine
                                    + "\"Programme\""
                                    + Environment.NewLine
                                    + "\"Day Of Week\\Variables\""
                                    + Environment.NewLine
                                    + "\"000s per Spot\"",
                                    "Import CSV",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                this.btnImport.Text = "Import";
                this.btnImport.BackColor = SystemColors.Control;
                this.btnImport.ForeColor = Color.Black;
            }
        }

        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            this.Hide();
            StartThread = new System.Threading.Thread(fnStartThread);
            StartThread.Start();

            try
            {

                Microsoft.Office.Interop.Excel.Application oApp;
                Microsoft.Office.Interop.Excel.Worksheet oSheet;
                Microsoft.Office.Interop.Excel.Workbook oBook;

                oApp = new Microsoft.Office.Interop.Excel.Application();
                oBook = oApp.Workbooks.Add();
                oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oBook.Worksheets.get_Item(1);

                DataTable CSVData = SQLDataAccess.GetCSV();

                for (int col = 0; col < CSVData.Rows.Count - 1; col++)
                {
                    string sheetName = CSVData.Rows[col].ItemArray[0].ToString();
                    if (!CSVData.Rows[col + 1].ItemArray[0].ToString().Equals(sheetName))
                    {

                        int r = 1;
                        oSheet = oApp.Worksheets[1];
                        oSheet.Tab.Color = RandomColorName();
                        oSheet.Name = CSVData.Rows[col + 1].ItemArray[0].ToString();
                        Microsoft.Office.Interop.Excel.Range oRange;

                        DataTable GetItemByChannel = SQLDataAccess.GetCSVByChannel(CSVData.Rows[col + 1].ItemArray[0].ToString());

                        //row 1
                        oSheet.Cells[1, 1] = "Target";
                        oSheet.Cells[1, 2] = CSVData.Columns[0].ColumnName;
                        oRange = oSheet.get_Range("A1", "B1");

                        oRange.Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                        oRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                        //row 2
                        oSheet.Cells[2, 1] = (this.txtTargetName.Text != "" ? this.txtTargetName.Text : targetName);
                        oSheet.Cells[2, 2] = CSVData.Rows[col + 1].ItemArray[0].ToString();
                        oRange = oSheet.get_Range("A2", "B2");

                        oRange.Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);

                        //row 3
                        oSheet.Cells[3, 1] = "Day Part";
                        oSheet.Cells[3, 2] = "Programme\\Variables";
                        oSheet.Cells[3, 3] = "000s per Spot";
                        oRange = oSheet.get_Range("A3", "B3");

                        oRange.Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                        oRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                        oRange = oSheet.get_Range("C3");

                        oRange.Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                        oRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                        int i = 4;
                        int ii = 1;
                        string tmpDayPart = "";
                        for (i = 4; i < GetItemByChannel.Rows.Count + 3; i++)
                        {

                            if (!GetItemByChannel.Rows[ii].ItemArray[0].ToString().Equals(tmpDayPart))
                            {
                                tmpDayPart = GetItemByChannel.Rows[ii].ItemArray[0].ToString();
                                oSheet.Cells[i, 1] = GetItemByChannel.Rows[ii].ItemArray[0].ToString();
                                oRange = oSheet.get_Range("A" + i);
                                oRange.Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                                oRange.Cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                                oRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                            }

                            oSheet.Cells[i, 2] = GetItemByChannel.Rows[ii].ItemArray[1].ToString();
                            oSheet.Cells[i, 3] = GetItemByChannel.Rows[ii].ItemArray[2].ToString();

                            oRange = oSheet.get_Range("A" + i);
                            oRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                            oRange = oSheet.get_Range("B" + i);
                            oRange.Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                            oRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                            oRange = oSheet.get_Range("C" + i);
                            oRange.NumberFormat = "#,##0";
                            oRange.Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);

                            ii++;
                        }

                        oRange = oSheet.get_Range("A" + (i - 1));
                        oRange.Cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

                        oRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                        oSheet.Columns["A:B"].AutoFit();
                        oSheet.Columns["B:C"].AutoFit();
                        oSheet.Columns["C:D"].AutoFit();

                        oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oBook.Worksheets.Add();
                        r++;

                    }
                    else
                    {
                        if (oApp.Application.Sheets.Count < 2)
                        {
                            int r = 1;
                            oSheet = oApp.Worksheets[1];
                            oSheet.Tab.Color = RandomColorName();
                            oSheet.Name = CSVData.Rows[col].ItemArray[0].ToString();
                            Microsoft.Office.Interop.Excel.Range oRange;

                            DataTable GetItemByChannel = SQLDataAccess.GetCSVByChannel(CSVData.Rows[col + 1].ItemArray[0].ToString());

                            //row 1
                            oSheet.Cells[1, 1] = "Target";
                            oSheet.Cells[1, 2] = CSVData.Columns[col].ColumnName;
                            oRange = oSheet.get_Range("A1", "B1");
                            
                            oRange.Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                            oRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                            //row 2
                            oSheet.Cells[2, 1] = (this.txtTargetName.Text != "" ? this.txtTargetName.Text : targetName);
                            oSheet.Cells[2, 2] = CSVData.Rows[col + 1].ItemArray[0].ToString();
                            oRange = oSheet.get_Range("A2", "B2");
                            
                            oRange.Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);

                            //row 3
                            oSheet.Cells[3, 1] = "Day Part";
                            oSheet.Cells[3, 2] = "Programme\\Variables";
                            oSheet.Cells[3, 3] = "000s per Spot";
                            oRange = oSheet.get_Range("A3", "B3");
                            
                            oRange.Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                            oRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                            oRange = oSheet.get_Range("C3");
                            
                            oRange.Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                            oRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                            int i = 4;
                            int ii = 1;
                            string tmpDayPart = "";
                            for (i = 4; i < GetItemByChannel.Rows.Count + 3; i++)
                            {

                                if (!GetItemByChannel.Rows[ii].ItemArray[0].ToString().Equals(tmpDayPart))
                                {
                                    tmpDayPart = GetItemByChannel.Rows[ii].ItemArray[0].ToString();
                                    oSheet.Cells[i, 1] = GetItemByChannel.Rows[ii].ItemArray[0].ToString();
                                    oRange = oSheet.get_Range("A" + i);
                                    oRange.Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                                    oRange.Cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
                                    oRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                                }

                                oSheet.Cells[i, 2] = GetItemByChannel.Rows[ii].ItemArray[1].ToString();
                                oSheet.Cells[i, 3] = GetItemByChannel.Rows[ii].ItemArray[2].ToString();

                                oRange = oSheet.get_Range("A" + i);
                                oRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                                oRange = oSheet.get_Range("B" + i);
                                oRange.Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                                oRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                                oRange = oSheet.get_Range("C" + i);
                                oRange.NumberFormat = "#,##0";
                                oRange.Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);

                                ii++;
                            }

                            oRange = oSheet.get_Range("A" + (i - 1));
                            oRange.Cells.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                            
                            oRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                            oSheet.Columns["A:B"].AutoFit();
                            oSheet.Columns["B:C"].AutoFit();
                            oSheet.Columns["C:D"].AutoFit();

                            oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oBook.Worksheets.Add();
                            r++;
                        }
                    }
                }

                oSheet.Delete();
                oApp.Visible = true;

                StopThread = new System.Threading.Thread(fnStopThread);
                StartThread.Abort();
                StopThread.Start();
                StopThread.Abort();

            }
            catch (Exception exHandle)
            {
                StopThread = new System.Threading.Thread(fnStopThread);
                StartThread.Abort();
                StopThread.Start();
                StopThread.Abort();
                MessageBox.Show("Exception: " + exHandle.Message, "Close Excel File", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                this.Show();
                //foreach (Process process in Process.GetProcessesByName("Excel"))
                //    process.Kill();
            }
        }

        private void fnStartThread()
        {
            ShowLoadingScreen.ShowDialog();
        }

        private void fnStopThread()
        {
            ShowLoadingScreen.Close();
        }

        private Color RandomColorName()
        {
            Random randomGen = new Random();
            KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            KnownColor randomColorName = names[randomGen.Next(names.Length)];
            Color randomColor = Color.FromKnownColor(randomColorName);
            return randomColor;
        }

        //private void txtTargetName_TextChanged(object sender, EventArgs e)
        //{
        //    if (this.txtTargetName.Text != "")
        //    {
        //        this.btnExportCSV.Enabled = true;
        //    }
        //    else
        //    {
        //        this.btnExportCSV.Enabled = false;
        //    }
        //}
    }
}
