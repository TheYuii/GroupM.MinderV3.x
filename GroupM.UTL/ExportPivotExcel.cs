using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Globalization;
using GroupM.UTL;


namespace GroupM.UTL
{
    public class ExportPivotExcel
    {

        static DataControl DataAccess = new DataControl();
        static frmLoadingScreen ShowLoadingScreen = new frmLoadingScreen();
        static System.Threading.Thread StartThread;
        static System.Threading.Thread StopThread;
        static object useDefault = Type.Missing;
        static string targetName = "";
        static string targetNameDetail = "";

        enum eMonth
        {
            January,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December
        }

        enum AddBorderColumn
        {
            B = 1, C = 2, D = 3, E = 4, F = 5, G = 6, H = 7, I = 8, J = 9, K = 10, L = 11, M = 12
        }

        private static void fnStartThread()
        {
            ShowLoadingScreen.ShowDialog();
        }

        private static void fnStopThread()
        {
            ShowLoadingScreen.Close();
        }

        public static void ExportToExcelFile(DataTable dataTableFile, string pivotFiledName)
        {

            StartThread = new System.Threading.Thread(fnStartThread);
            StartThread.Start();

            try
            {

                Excel.Application oApp;
                Excel.Worksheet oSheet;
                Excel.Workbook oBook;

                oApp = new Excel.Application();
                oBook = oApp.Workbooks.Add();
                oSheet = (Excel.Worksheet)oBook.Worksheets.get_Item(1);

                int r = 1;


                var GetSummary = dataTableFile;

                for (int col = 1; col < GetSummary.Columns.Count; col++)
                    oSheet.Cells[r, col] = GetSummary.Columns[col - 1].ColumnName;
                r++;

                for (int row = 0; row < GetSummary.Rows.Count; row++)
                {
                    for (int col = 1; col < GetSummary.Columns.Count; col++)
                        oSheet.Cells[r, col] = GetSummary.Rows[row][col - 1].ToString();
                    r++;
                }

                Excel.Range oRange = oSheet.Range[oSheet.Cells[1, 1], oSheet.Cells[GetSummary.Rows.Count + 1, GetSummary.Columns.Count - 1]];

                if (oApp.Application.Sheets.Count < 2)
                {
                    oSheet.Name = "Data";
                    oSheet = (Excel.Worksheet)oBook.Worksheets.Add();
                }
                else
                {
                    oSheet = (Excel.Worksheet)oApp.Worksheets[2];
                }
                oSheet.Name = "Pivot";

                Excel.Range oRange2 = (Excel.Range)oSheet.Cells[1, 1];

                Excel.PivotCache oPivotCache = (Excel.PivotCache)oBook.PivotCaches().Add(Excel.XlPivotTableSourceType.xlDatabase, oRange);

                Excel.Worksheet sheet = (Excel.Worksheet)oBook.ActiveSheet;
                Excel.PivotTables pivotTables = (Excel.PivotTables)sheet.PivotTables();
                Excel.PivotTable pivotTable = pivotTables.Add(oPivotCache, oRange2);

                pivotTable.SmallGrid = false;
                pivotTable.ShowTableStyleRowStripes = true;
                pivotTable.TableStyle2 = "PivotStyleLight1";
                pivotTable.DisplayImmediateItems = true;

                Excel.PivotField rowField = (Excel.PivotField)pivotTable.PivotFields(pivotFiledName);
                rowField.Orientation = Excel.XlPivotFieldOrientation.xlRowField;

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
                //this.Show();
            }
        }


        public static void ExportTVBuyingPattern(string pathHeader, string pathDetail)
        {
            try
            {
                #region Add Header
                var readerHeader = new StreamReader(File.OpenRead(pathHeader));

                //"Target", "Brand", "Month", "Variables\Year", "2015"
                List<string> listTarget = new List<string>();
                List<string> listBrand = new List<string>();
                List<string> listMonth = new List<string>();
                List<string> listVariables = new List<string>();
                List<string> list2015 = new List<string>();

                string checkTarget = ""; ;

                int countChk = 0;
                int retChk = 0;
                while (!readerHeader.EndOfStream)
                {
                    var line = readerHeader.ReadLine();

                    var values = line.Split(',');

                    if (values[0].Replace("\"", "").Equals("Target") || checkTarget == "Target")
                    {
                        if (checkTarget == "")
                            checkTarget = "Target";
                        countChk++;
                        retChk = countChk;

                        if (targetName == "" && checkTarget != "" && retChk == 1)
                        {
                            targetName = values[0].Replace("\"", "");
                            //this.txtTargetName.Text = targetName;
                        }

                        if (targetName != "" && checkTarget != "" && retChk > 0)
                        {
                            string Target = values[0].Replace("\"", "");
                            listTarget.Add(Target.Replace(",", ""));

                            string Brand = values[1].Replace("\"", "");
                            listBrand.Add(Brand.Replace(",", ""));

                            string Month = values[2].Replace("\"", "");
                            listMonth.Add(Month.Replace(",", ""));

                            string Variables = values[3].Replace("\"", "");
                            listVariables.Add(Variables.Replace(",", ""));

                            string list2015s = values[4].Replace("\"", "");
                            list2015.Add(list2015s.Replace(",", ""));

                            Application.DoEvents();
                        }

                    }
                }
                #endregion

                #region Add Detail
                var readerDetail = new StreamReader(File.OpenRead(pathDetail));

                //"Target", "Brand", "Channel", "Duration", "Wk day/Wk end", "Position", "Day part", "Prog Type", "Month", "Variables\Year", "2015"
                List<string> listTargetDetail = new List<string>();
                List<string> listBrandDetail = new List<string>();
                List<string> listChannel = new List<string>();
                List<string> listDuration = new List<string>();
                List<string> listDayWk = new List<string>();
                List<string> listPosition = new List<string>();
                List<string> listDayPart = new List<string>();
                List<string> listProgType = new List<string>();
                List<string> listMonthDetail = new List<string>();
                List<string> listVariablesDetail = new List<string>();
                List<double> list2015Detail = new List<double>();

                string checkTargetDetail = ""; ;

                int countChkDtl = 0;
                int retChkDtl = 0;

                DataTable dt = new DataTable("NestleReport");
                string[] columns = null;

                var lines = File.ReadAllLines(pathDetail);

                // assuming the first row contains the columns information
                if (lines.Count() > 0)
                {
                    columns = lines[11].Split(new char[] { ',' });

                    foreach (var column in columns)
                    {
                        if (column != "")
                        {
                            dt.Columns.Add(column.Replace("\"", ""));
                        }
                    }
                }

                // reading rest of the data
                for (int i = 1; i < lines.Count() - 11; i++)
                {
                    DataRow dr = dt.NewRow();
                    string[] values = lines[i + 11].Split(new char[] { ',' });

                    for (int j = 0; j < values.Count() && j < columns.Count(); j++)
                    {
                        if (values[j] != "")
                        {
                            dr[j] = values[j].Replace("\"", "");
                        }
                    }
                    dt.Rows.Add(dr);
                }

                while (!readerDetail.EndOfStream)
                {
                    var line = readerDetail.ReadLine();
                    var values = line.Split(',');

                    if (values[0].Replace("\"", "").Equals("Target") || checkTargetDetail == "Target")
                    {
                        if (checkTargetDetail == "")
                            checkTargetDetail = "Target";
                        countChkDtl++;
                        retChkDtl = countChkDtl;

                        if (targetNameDetail == "" && checkTargetDetail != "" && retChkDtl == 1)
                        {
                            targetNameDetail = values[0].Replace("\"", "");
                        }

                        if (targetNameDetail != "" && checkTargetDetail != "" && retChkDtl > 0)
                        {
                            string Target = values[0].Replace("\"", "");
                            listTargetDetail.Add(Target.Replace(",", ""));

                            string BrandDetail = values[1].Replace("\"", "");
                            listBrandDetail.Add(BrandDetail.Replace(",", ""));

                            string Channel = values[2].Replace("\"", "");
                            listChannel.Add(Channel.Replace(",", ""));

                            string Duration = values[3].Replace("\"", "");
                            listDuration.Add(Duration.Replace(",", ""));

                            string DayWk = values[4].Replace("\"", "");
                            listDayWk.Add(DayWk.Replace(",", ""));

                            string Position = values[5].Replace("\"", "");
                            listPosition.Add(Position.Replace(",", ""));

                            string DayPart = values[6].Replace("\"", "");
                            listDayPart.Add(DayPart.Replace(",", ""));

                            string ProgType = values[7].Replace("\"", "");
                            listProgType.Add(ProgType.Replace(",", ""));

                            string MonthDetail = values[8].Replace("\"", "");
                            listMonthDetail.Add(MonthDetail.Replace(",", ""));

                            string VariablesDetail = values[9].Replace("\"", "");
                            listVariablesDetail.Add(VariablesDetail.Replace(",", ""));

                            string list2015Details = values[10].Replace("\"", "");
                            list2015Detail.Add(Convert.ToDouble(list2015Details.Replace(",", "")));

                            Application.DoEvents();
                        }
                    }
                }
                #endregion

                listBrand.RemoveAt(0);
                var BrandHeaderDis = fnDistinct(listBrand);

                var BrandDetailDis = fnDistinct(listBrandDetail);

                listVariables.RemoveAt(0);
                var VariablesHeader = fnDistinct(listVariables);

                listMonth.RemoveAt(0);
                var MonthHeader = fnDistinct(listMonth);

                list2015.RemoveAt(0);

                listDayWk.RemoveAt(0);
                listDuration.RemoveAt(0);
                listChannel.RemoveAt(0);
                var tmplistChannel = listChannel;

                listPosition.RemoveAt(0);
                listDayPart.RemoveAt(0);
                listProgType.RemoveAt(0);

                listBrandDetail.RemoveAt(0);
                list2015Detail.RemoveAt(0);

                listTarget.RemoveAt(0);
                listTarget = fnDistinct(listTarget);

                #region Add Data
                try
                {
                    StartThread = new System.Threading.Thread(fnStartThread);
                    StartThread.Start();

                    Microsoft.Office.Interop.Excel.Application oApp;
                    Microsoft.Office.Interop.Excel.Worksheet oSheet;
                    Microsoft.Office.Interop.Excel.Workbook oBook;

                    oApp = new Microsoft.Office.Interop.Excel.Application();
                    oBook = oApp.Workbooks.Add();
                    oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oBook.Worksheets.get_Item(1);

                    for (int iSheet = 0; iSheet < BrandHeaderDis.Count; iSheet++)
                    {

                        oSheet = oApp.Worksheets[1];
                        oSheet.Tab.Color = RandomColor.RandomColorName();
                        oSheet.Name = (BrandHeaderDis[iSheet].Length > 20 ? (iSheet + 1) + ". " + BrandHeaderDis[iSheet].Substring(0, 20) : (iSheet + 1) + ". " + BrandHeaderDis[iSheet].ToString());
                        Microsoft.Office.Interop.Excel.Range oRange;

                        //row 1
                        oSheet.Cells[1, 1] = "TV Buying Pattern";
                        oRange = oSheet.get_Range("A1", Type.Missing);
                        oRange.DisplayFormat.Font.Size = 14.0;
                        oRange.DisplayFormat.Font.Bold = true;
                        oRange.DisplayFormat.Font.Underline = true;

                        //row 2
                        oSheet.Cells[2, 1] = "Brand:";
                        oSheet.Cells[2, 2] = BrandHeaderDis[iSheet].ToString();

                        //row 3
                        oSheet.Cells[3, 1] = "Target:";
                        oSheet.Cells[3, 2] = listTarget[0].ToString();

                        //row 4
                        oSheet.Cells[5, 1] = "TV Buying Strategy";

                        MonthHeader = MonthHeader
                            .Select(x => new { Name = x, Sort = DateTime.ParseExact(x, "MMMM", CultureInfo.InvariantCulture) })
                            .OrderBy(x => x.Sort.Month)
                            .Select(x => x.Name)
                            .ToList<string>();

                        for (int i = 0; i < MonthHeader.Count; i++)
                        {
                            oSheet.Cells[5, i + 2] = MonthHeader[i].ToString();
                            oSheet.Cells[5, i + 2].Font.Bold = true;
                        }

                        //row 6 - 10
                        for (int i = 0; i < VariablesHeader.Count; i++)
                        {
                            oSheet.Cells[i + 6, 1] = listVariables[i].ToString();
                        }

                        var selectBrand = listBrand.Where(q => q.Equals(BrandHeaderDis[iSheet].ToString()));

                        int countSelectBrand = selectBrand.ToList().Count;

                        int jan = 0, feb = 0, mar = 0, apr = 0, may = 0, jun = 0, jul = 0, aug = 0, sep = 0, oct = 0, nov = 0, dec = 0;
                        for (int i = 0; i < selectBrand.ToList().Count; i++)
                        {
                            if (eMonth.January.ToString().Equals(listMonth[i].ToString()))
                            {
                                oSheet.Cells[6 + jan, 2] = list2015[i].ToString().Replace("'", "");

                                jan++;
                            }
                            else if (eMonth.February.ToString().Equals(listMonth[i].ToString()))
                            {
                                oSheet.Cells[6 + feb, 3] = list2015[i].ToString().Replace("'", "");
                                feb++;
                            }
                            else if (eMonth.March.ToString().Equals(listMonth[i].ToString()))
                            {
                                oSheet.Cells[6 + mar, 4] = list2015[i].ToString().Replace("'", "");
                                mar++;
                            }
                            else if (eMonth.April.ToString().Equals(listMonth[i].ToString()))
                            {
                                oSheet.Cells[6 + apr, 5] = list2015[i].ToString().Replace("'", "");
                                apr++;
                            }
                            else if (eMonth.May.ToString().Equals(listMonth[i].ToString()))
                            {
                                oSheet.Cells[6 + may, 6] = list2015[i].ToString().Replace("'", "");
                                may++;
                            }
                            else if (eMonth.June.ToString().Equals(listMonth[i].ToString()))
                            {
                                oSheet.Cells[6 + jun, 7] = list2015[i].ToString().Replace("'", "");
                                jun++;
                            }
                            else if (eMonth.July.ToString().Equals(listMonth[i].ToString()))
                            {
                                oSheet.Cells[6 + jul, 8] = list2015[i].ToString().Replace("'", "");
                                jul++;
                            }
                            else if (eMonth.August.ToString().Equals(listMonth[i].ToString()))
                            {
                                oSheet.Cells[6 + aug, 9] = list2015[i].ToString().Replace("'", "");
                                aug++;
                            }
                            else if (eMonth.September.ToString().Equals(listMonth[i].ToString()))
                            {
                                oSheet.Cells[6 + sep, 10] = list2015[i].ToString().Replace("'", "");
                                sep++;
                            }
                            else if (eMonth.October.ToString().Equals(listMonth[i].ToString()))
                            {
                                oSheet.Cells[6 + oct, 11] = list2015[i].ToString().Replace("'", "");
                                oct++;
                            }
                            else if (eMonth.November.ToString().Equals(listMonth[i].ToString()))
                            {
                                oSheet.Cells[6 + nov, 12] = list2015[i].ToString().Replace("'", "");
                                nov++;
                            }
                            else if (eMonth.December.ToString().Equals(listMonth[i].ToString()))
                            {
                                oSheet.Cells[6 + dec, 13] = list2015[i].ToString().Replace("'", "");
                                dec++;
                            }
                        }

                        listMonth.RemoveRange(0, countSelectBrand);
                        list2015.RemoveRange(0, countSelectBrand);

                        //row 11
                        oSheet.Cells[11, 1] = "Duration";
                        oSheet.Cells[11, 1].Font.Bold = true;
                        int durationRow = 11;
                        var listDurationDis = fnDistinct(listDuration).Select(c => new { Durations = c }).OrderBy(c => c.Durations).Select(c => c.Durations).ToList<string>();
                        var NextRowCount = 12;
                        for (int i = 0; i < listDurationDis.Count; i++)
                        {
                            oSheet.Cells[(12 + i), 1] = listDurationDis[i].ToString();

                            var ArrayOfDuration = (from r in dt.AsEnumerable()
                                                   where r.ItemArray[3].ToString().Equals(listDurationDis[i].ToString()) &&
                                                          r.ItemArray[1].ToString().Equals(BrandDetailDis[iSheet + 1].ToString())
                                                   group r by new
                                                   {
                                                       Month = r.ItemArray[8]//Month
                                                   } into g
                                                   select new
                                                   {
                                                       Month = g.Key.Month,
                                                       SumOfDuration = g.Sum(x => Convert.ToDecimal(x.ItemArray[10]))
                                                   }).ToArray();

                            foreach (var item in ArrayOfDuration)
                            {
                                if (item.Month.Equals(eMonth.December.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 13] = item.SumOfDuration;
                                }
                                else if (item.Month.Equals(eMonth.November.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 12] = item.SumOfDuration;
                                }
                                else if (item.Month.Equals(eMonth.October.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 11] = item.SumOfDuration;
                                }
                                else if (item.Month.Equals(eMonth.September.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 10] = item.SumOfDuration;
                                }
                                else if (item.Month.Equals(eMonth.August.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 9] = item.SumOfDuration;
                                }
                                else if (item.Month.Equals(eMonth.July.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 8] = item.SumOfDuration;
                                }
                                else if (item.Month.Equals(eMonth.June.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 7] = item.SumOfDuration;
                                }
                                else if (item.Month.Equals(eMonth.May.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 6] = item.SumOfDuration;
                                }
                                else if (item.Month.Equals(eMonth.April.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 5] = item.SumOfDuration;
                                }
                                else if (item.Month.Equals(eMonth.March.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 4] = item.SumOfDuration;
                                }
                                else if (item.Month.Equals(eMonth.February.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 3] = item.SumOfDuration;
                                }
                                else if (item.Month.Equals(eMonth.January.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 2] = item.SumOfDuration;
                                }
                            }
                        }
                        NextRowCount = 12 + listDurationDis.Count;

                        //row 1
                        oSheet.Cells[(int)NextRowCount, 1] = "Day";
                        oSheet.Cells[(int)NextRowCount, 1].Font.Bold = true;
                        int dayRow = (int)NextRowCount;
                        listDayWk = fnDistinct(listDayWk).Select(c => new { Durations = c }).OrderBy(c => c.Durations).Select(c => c.Durations).ToList<string>();
                        NextRowCount++;
                        for (int i = 0; i < listDayWk.Count; i++)
                        {
                            oSheet.Cells[((int)NextRowCount + i), 1] = listDayWk[i].ToString();

                            var ArrayOfDay = (from r in dt.AsEnumerable()
                                              where r.ItemArray[4].ToString().Equals(listDayWk[i].ToString()) &&
                                                          r.ItemArray[1].ToString().Equals(BrandDetailDis[iSheet + 1].ToString())
                                              group r by new
                                              {
                                                  Month = r.ItemArray[8]//Month
                                              } into g
                                              select new
                                              {
                                                  Month = g.Key.Month,
                                                  SumOfDay = g.Sum(x => Convert.ToDecimal(x.ItemArray[10]))
                                              }).ToArray();

                            foreach (var item in ArrayOfDay)
                            {
                                if (item.Month.Equals(eMonth.December.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 13] = item.SumOfDay;
                                }
                                else if (item.Month.Equals(eMonth.November.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 12] = item.SumOfDay;
                                }
                                else if (item.Month.Equals(eMonth.October.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 11] = item.SumOfDay;
                                }
                                else if (item.Month.Equals(eMonth.September.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 10] = item.SumOfDay;
                                }
                                else if (item.Month.Equals(eMonth.August.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 9] = item.SumOfDay;
                                }
                                else if (item.Month.Equals(eMonth.July.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 8] = item.SumOfDay;
                                }
                                else if (item.Month.Equals(eMonth.June.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 7] = item.SumOfDay;
                                }
                                else if (item.Month.Equals(eMonth.May.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 6] = item.SumOfDay;
                                }
                                else if (item.Month.Equals(eMonth.April.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 5] = item.SumOfDay;
                                }
                                else if (item.Month.Equals(eMonth.March.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 4] = item.SumOfDay;
                                }
                                else if (item.Month.Equals(eMonth.February.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 3] = item.SumOfDay;
                                }
                                else if (item.Month.Equals(eMonth.January.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 2] = item.SumOfDay;
                                }
                            }
                        }
                        NextRowCount += listDayWk.Count;

                        oSheet.Cells[(int)NextRowCount, 1] = "Channel";

                        oSheet.Cells[(int)NextRowCount, 1].Font.Bold = true;
                        int channelRow = (int)NextRowCount;
                        listChannel = fnDistinct(listChannel).Select(c => new { Channel = c }).OrderBy(c => c.Channel).Select(c => c.Channel).ToList<string>();
                        NextRowCount++;
                        for (int i = 0; i < listChannel.Count; i++)
                        {
                            oSheet.Cells[((int)NextRowCount + i), 1] = listChannel[i].ToString();

                            var ArrayOfChannel = (from r in dt.AsEnumerable()
                                                  where r.ItemArray[2].ToString().Equals(listChannel[i].ToString()) &&
                                                          r.ItemArray[1].ToString().Equals(BrandDetailDis[iSheet + 1].ToString())
                                                  group r by new
                                                  {
                                                      Month = r.ItemArray[8]//Month
                                                  } into g
                                                  select new
                                                  {
                                                      Month = g.Key.Month,
                                                      SumOfChannel = g.Sum(x => Convert.ToDecimal(x.ItemArray[10]))
                                                  }).ToArray();

                            foreach (var item in ArrayOfChannel)
                            {
                                if (item.Month.Equals(eMonth.December.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 13] = item.SumOfChannel;
                                }
                                else if (item.Month.Equals(eMonth.November.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 12] = item.SumOfChannel;
                                }
                                else if (item.Month.Equals(eMonth.October.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 11] = item.SumOfChannel;
                                }
                                else if (item.Month.Equals(eMonth.September.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 10] = item.SumOfChannel;
                                }
                                else if (item.Month.Equals(eMonth.August.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 9] = item.SumOfChannel;
                                }
                                else if (item.Month.Equals(eMonth.July.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 8] = item.SumOfChannel;
                                }
                                else if (item.Month.Equals(eMonth.June.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 7] = item.SumOfChannel;
                                }
                                else if (item.Month.Equals(eMonth.May.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 6] = item.SumOfChannel;
                                }
                                else if (item.Month.Equals(eMonth.April.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 5] = item.SumOfChannel;
                                }
                                else if (item.Month.Equals(eMonth.March.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 4] = item.SumOfChannel;
                                }
                                else if (item.Month.Equals(eMonth.February.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 3] = item.SumOfChannel;
                                }
                                else if (item.Month.Equals(eMonth.January.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 2] = item.SumOfChannel;
                                }
                            }
                        }
                        NextRowCount += listChannel.Count;

                        oSheet.Cells[(int)NextRowCount, 1] = "PIB";
                        oSheet.Cells[(int)NextRowCount, 1].Font.Bold = true;
                        int pibRow = (int)NextRowCount;

                        listPosition = fnDistinct(listPosition).Select(c => new { Position = c }).OrderBy(c => c.Position).Select(c => c.Position).ToList<string>();
                        NextRowCount++;
                        for (int i = 0; i < listPosition.Count; i++)
                        {
                            oSheet.Cells[((int)NextRowCount + i), 1] = listPosition[i].ToString();

                            var ArrayOfPIB = (from r in dt.AsEnumerable()
                                              where r.ItemArray[5].ToString().Equals(listPosition[i].ToString()) &&
                                                          r.ItemArray[1].ToString().Equals(BrandDetailDis[iSheet + 1].ToString())
                                              group r by new
                                              {
                                                  Month = r.ItemArray[8]//Month
                                              } into g
                                              select new
                                              {
                                                  Month = g.Key.Month,
                                                  SumOfPIB = g.Sum(x => Convert.ToDecimal(x.ItemArray[10]))//x.Field<int>("CurrentHours"))
                                              }).ToArray();

                            foreach (var item in ArrayOfPIB)
                            {
                                if (item.Month.Equals(eMonth.December.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 13] = item.SumOfPIB;
                                }
                                else if (item.Month.Equals(eMonth.November.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 12] = item.SumOfPIB;
                                }
                                else if (item.Month.Equals(eMonth.October.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 11] = item.SumOfPIB;
                                }
                                else if (item.Month.Equals(eMonth.September.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 10] = item.SumOfPIB;
                                }
                                else if (item.Month.Equals(eMonth.August.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 9] = item.SumOfPIB;
                                }
                                else if (item.Month.Equals(eMonth.July.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 8] = item.SumOfPIB;
                                }
                                else if (item.Month.Equals(eMonth.June.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 7] = item.SumOfPIB;
                                }
                                else if (item.Month.Equals(eMonth.May.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 6] = item.SumOfPIB;
                                }
                                else if (item.Month.Equals(eMonth.April.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 5] = item.SumOfPIB;
                                }
                                else if (item.Month.Equals(eMonth.March.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 4] = item.SumOfPIB;
                                }
                                else if (item.Month.Equals(eMonth.February.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 3] = item.SumOfPIB;
                                }
                                else if (item.Month.Equals(eMonth.January.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 2] = item.SumOfPIB;
                                }
                            }

                        }
                        NextRowCount += listPosition.Count;

                        oSheet.Cells[(int)NextRowCount, 1] = "Daypart";
                        oSheet.Cells[(int)NextRowCount, 1].Font.Bold = true;
                        int dayPartRow = (int)NextRowCount;
                        listDayPart = fnDistinct(listDayPart).Select(c => new { Position = c }).OrderBy(c => c.Position).Select(c => c.Position).ToList<string>();
                        NextRowCount++;
                        for (int i = 0; i < listDayPart.Count; i++)
                        {
                            oSheet.Cells[((int)NextRowCount + i), 1] = listDayPart[i].ToString();

                            var ArrayOfDayPart = (from r in dt.AsEnumerable()
                                                  where r.ItemArray[6].ToString().Equals(listDayPart[i].ToString()) &&
                                                            r.ItemArray[1].ToString().Equals(BrandDetailDis[iSheet + 1].ToString())
                                                  group r by new
                                                  {
                                                      Month = r.ItemArray[8]//Month
                                                  } into g
                                                  select new
                                                  {
                                                      Month = g.Key.Month,
                                                      SumOfDayPart = g.Sum(x => Convert.ToDecimal(x.ItemArray[10]))//x.Field<int>("CurrentHours"))
                                                  }).ToArray();

                            foreach (var item in ArrayOfDayPart)
                            {
                                if (item.Month.Equals(eMonth.December.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 13] = item.SumOfDayPart;
                                }
                                else if (item.Month.Equals(eMonth.November.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 12] = item.SumOfDayPart;
                                }
                                else if (item.Month.Equals(eMonth.October.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 11] = item.SumOfDayPart;
                                }
                                else if (item.Month.Equals(eMonth.September.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 10] = item.SumOfDayPart;
                                }
                                else if (item.Month.Equals(eMonth.August.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 9] = item.SumOfDayPart;
                                }
                                else if (item.Month.Equals(eMonth.July.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 8] = item.SumOfDayPart;
                                }
                                else if (item.Month.Equals(eMonth.June.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 7] = item.SumOfDayPart;
                                }
                                else if (item.Month.Equals(eMonth.May.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 6] = item.SumOfDayPart;
                                }
                                else if (item.Month.Equals(eMonth.April.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 5] = item.SumOfDayPart;
                                }
                                else if (item.Month.Equals(eMonth.March.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 4] = item.SumOfDayPart;
                                }
                                else if (item.Month.Equals(eMonth.February.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 3] = item.SumOfDayPart;
                                }
                                else if (item.Month.Equals(eMonth.January.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 2] = item.SumOfDayPart;
                                }
                            }
                        }

                        NextRowCount += listDayPart.Count;
                        oSheet.Cells[(int)NextRowCount, 1] = "Program Type";
                        oSheet.Cells[(int)NextRowCount, 1].Font.Bold = true;
                        int progTypeRow = (int)NextRowCount;
                        listProgType = fnDistinct(listProgType).Select(c => new { Position = c }).OrderBy(c => c.Position).Select(c => c.Position).ToList<string>();
                        NextRowCount++;
                        for (int i = 0; i < listProgType.Count; i++)
                        {
                            oSheet.Cells[((int)NextRowCount + i), 1] = listProgType[i].ToString();

                            var ArrayOfProgramType = (from r in dt.AsEnumerable()
                                                      where r.ItemArray[7].ToString().Equals(listProgType[i].ToString()) &&
                                                            r.ItemArray[1].ToString().Equals(BrandDetailDis[iSheet + 1].ToString())
                                                      group r by new
                                                      {
                                                          ProgramType = r.ItemArray[8]//Month
                                                      } into g
                                                      select new
                                                      {
                                                          ProgramType = g.Key.ProgramType,
                                                          SumOfProgramType = g.Sum(x => Convert.ToDecimal(x.ItemArray[10]))//x.Field<int>("CurrentHours"))
                                                      }).ToArray();

                            foreach (var item in ArrayOfProgramType)
                            {
                                if (item.ProgramType.Equals(eMonth.December.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 13] = item.SumOfProgramType;
                                }
                                else if (item.ProgramType.Equals(eMonth.November.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 12] = item.SumOfProgramType;
                                }
                                else if (item.ProgramType.Equals(eMonth.October.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 11] = item.SumOfProgramType;
                                }
                                else if (item.ProgramType.Equals(eMonth.September.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 10] = item.SumOfProgramType;
                                }
                                else if (item.ProgramType.Equals(eMonth.August.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 9] = item.SumOfProgramType;
                                }
                                else if (item.ProgramType.Equals(eMonth.July.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 8] = item.SumOfProgramType;
                                }
                                else if (item.ProgramType.Equals(eMonth.June.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 7] = item.SumOfProgramType;
                                }
                                else if (item.ProgramType.Equals(eMonth.May.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 6] = item.SumOfProgramType;
                                }
                                else if (item.ProgramType.Equals(eMonth.April.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 5] = item.SumOfProgramType;
                                }
                                else if (item.ProgramType.Equals(eMonth.March.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 4] = item.SumOfProgramType;
                                }
                                else if (item.ProgramType.Equals(eMonth.February.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 3] = item.SumOfProgramType;
                                }
                                else if (item.ProgramType.Equals(eMonth.January.ToString()))
                                {
                                    oSheet.Cells[(int)NextRowCount + i, 2] = item.SumOfProgramType;
                                }
                            }
                        }

                        oSheet.Columns["A:B"].AutoFit();
                        oSheet.Columns["B:C"].AutoFit();
                        oSheet.Columns["C:D"].AutoFit();
                        oSheet.Columns["D:E"].AutoFit();
                        oSheet.Columns["E:F"].AutoFit();
                        oSheet.Columns["F:G"].AutoFit();
                        oSheet.Columns["G:H"].AutoFit();
                        oSheet.Columns["H:I"].AutoFit();
                        oSheet.Columns["I:J"].AutoFit();
                        oSheet.Columns["J:K"].AutoFit();
                        oSheet.Columns["K:L"].AutoFit();
                        oSheet.Columns["L:M"].AutoFit();
                        oSheet.Columns["M:N"].AutoFit();

                        oSheet.get_Range("A5", "N5").Font.Bold = true;

                        //======================================================================================================================

                        //Set RightRow TV Duration
                        oSheet.get_Range("A5", "A" + ((int)NextRowCount + listProgType.Count - 1)).Cells.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).Color =
                            System.Drawing.ColorTranslator.ToOle(Color.Black);
                        oSheet.get_Range("B5", "B" + ((int)NextRowCount + listProgType.Count - 1)).Cells.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).Color =
                            System.Drawing.ColorTranslator.ToOle(Color.Black);
                        oSheet.get_Range("C5", "C" + ((int)NextRowCount + listProgType.Count - 1)).Cells.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).Color =
                            System.Drawing.ColorTranslator.ToOle(Color.Black);
                        oSheet.get_Range("D5", "D" + ((int)NextRowCount + listProgType.Count - 1)).Cells.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).Color =
                            System.Drawing.ColorTranslator.ToOle(Color.Black);
                        oSheet.get_Range("E5", "E" + ((int)NextRowCount + listProgType.Count - 1)).Cells.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).Color =
                            System.Drawing.ColorTranslator.ToOle(Color.Black);
                        oSheet.get_Range("F5", "F" + ((int)NextRowCount + listProgType.Count - 1)).Cells.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).Color =
                            System.Drawing.ColorTranslator.ToOle(Color.Black);
                        oSheet.get_Range("G5", "G" + ((int)NextRowCount + listProgType.Count - 1)).Cells.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).Color =
                            System.Drawing.ColorTranslator.ToOle(Color.Black);
                        oSheet.get_Range("H5", "H" + ((int)NextRowCount + listProgType.Count - 1)).Cells.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).Color =
                            System.Drawing.ColorTranslator.ToOle(Color.Black);
                        oSheet.get_Range("I5", "I" + ((int)NextRowCount + listProgType.Count - 1)).Cells.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).Color =
                            System.Drawing.ColorTranslator.ToOle(Color.Black);
                        oSheet.get_Range("J5", "J" + ((int)NextRowCount + listProgType.Count - 1)).Cells.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).Color =
                            System.Drawing.ColorTranslator.ToOle(Color.Black);
                        oSheet.get_Range("K5", "K" + ((int)NextRowCount + listProgType.Count - 1)).Cells.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).Color =
                            System.Drawing.ColorTranslator.ToOle(Color.Black);
                        oSheet.get_Range("L5", "L" + ((int)NextRowCount + listProgType.Count - 1)).Cells.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).Color =
                            System.Drawing.ColorTranslator.ToOle(Color.Black);
                        oSheet.get_Range("M5", "M" + ((int)NextRowCount + listProgType.Count - 1)).Cells.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).Color =
                            System.Drawing.ColorTranslator.ToOle(Color.Black);

                        oSheet.get_Range("A" + ((int)NextRowCount + listProgType.Count - 1), "M" + ((int)NextRowCount + listProgType.Count - 1)).Cells.Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).Color =
                            System.Drawing.ColorTranslator.ToOle(Color.Black);

                        oSheet.get_Range("B" + 6, "M" + ((int)NextRowCount + listProgType.Count - 1)).NumberFormat = "#,##0.00";

                        oSheet.get_Range("B2", "B3").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                        oSheet.get_Range("A12", "A" + (listDurationDis.Count + 12)).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

                        oSheet.get_Range("A5", "M5").Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        oSheet.get_Range("A5", "M5").Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                        oSheet.get_Range("A5", "M5").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DodgerBlue);

                        oSheet.get_Range("A" + durationRow, "M" + durationRow).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        oSheet.get_Range("A" + durationRow, "M" + durationRow).Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                        oSheet.get_Range("A" + dayRow, "M" + dayRow).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        oSheet.get_Range("A" + dayRow, "M" + dayRow).Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                        oSheet.get_Range("A" + channelRow, "M" + channelRow).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        oSheet.get_Range("A" + channelRow, "M" + channelRow).Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                        oSheet.get_Range("A" + pibRow, "M" + pibRow).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        oSheet.get_Range("A" + pibRow, "M" + pibRow).Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                        oSheet.get_Range("A" + dayPartRow, "M" + dayPartRow).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        oSheet.get_Range("A" + dayPartRow, "M" + dayPartRow).Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
                        oSheet.get_Range("A" + progTypeRow, "M" + progTypeRow).Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        oSheet.get_Range("A" + progTypeRow, "M" + progTypeRow).Cells.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);

                        oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oBook.Worksheets.Add();
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
                #endregion



                listBrandDetail = fnDistinct(listBrandDetail);
            }
            catch (Exception)
            {

                MessageBox.Show("Input wrong file.", "Error input file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            

        }

        #region Function Distinct Data
        private static List<string> fnDistinct(List<string> data)
        {
            Dictionary<string, bool> Distinct = new Dictionary<string, bool>();
            foreach (string value in data)
            {
                Distinct[value] = true;
            }
            var retData = new List<string>();
            retData.AddRange(Distinct.Keys);

            return retData;
        }
        #endregion

        public DataTable ToDataTable<T>(IEnumerable<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                tb.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

    }
}
