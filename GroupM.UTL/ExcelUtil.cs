using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Drawing;

namespace GroupM.UTL
{
    public class ExcelUtil
    {
        // ## Load excel file ##
        public static DataTable GetSheetName(string strPath)
        {
            DataTable result = new DataTable();
            try
            {
                var connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strPath + ";Extended Properties=\"Excel 12.0;IMEX=1;HDR=NO;TypeGuessRows=0;ImportMixedTypes=Text\""; ;
                using (var conn = new OleDbConnection(connectionString))
                {
                    conn.Open();

                    var sheets = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    result = sheets;
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    MessageBox.Show(ex.InnerException.Message.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(ex.Message.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            return result;
        }
        public static DataTable LoadExcelFile(string strPath)
        {
            DataTable result = new DataTable();
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                DataTable dtSheet = GetSheetName(strPath);

                string myConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strPath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES\"";

                OleDbConnection conn = new OleDbConnection(myConnection);
                string strSQL = "SELECT * FROM [" + dtSheet.Rows[0]["TABLE_NAME"] + "] ";

                OleDbCommand cmd = new OleDbCommand(strSQL, conn);

                DataSet dataset = new DataSet();
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                adapter.Fill(dataset);

                result = dataset.Tables[0];
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            return result;
        }
        public static DataTable LoadCSVFile(string strPath)
        {

            string FileName = strPath;
            OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OleDb.4.0; Data Source = " + Path.GetDirectoryName(FileName) + "; Extended Properties = \"Text;HDR=YES;FMT=Delimited\"");
            conn.Open();
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [" + Path.GetFileName(FileName) + "]", conn);
            DataSet ds = new DataSet("Temp");
            adapter.Fill(ds);
            conn.Close();
            return ds.Tables[0];
        }
        public static Excel.Workbook ExportXlsx(DataTable dt, int iStartRow, bool bVisible/*, string strSavePath*/)
        {
            return ExportXlsx("",dt,iStartRow,bVisible);
        }

            public static Excel.Workbook ExportXlsx(string filename,DataTable dt, int iStartRow, bool bVisible/*, string strSavePath*/)
        {
            try
            {
                //if (File.Exists(strSavePath))
                //    File.Delete(strSavePath);
                //======================================
                // Create Result Excel
                //======================================
                Excel.Application ExcelObjDesc = new Excel.Application();
                //Excel.Workbook theWorkbook = ExcelObjDesc.Workbooks.Open(strSavePath, Missing.Value, Missing.Value,
                //    Missing.Value, Missing.Value,
                //    Missing.Value, Missing.Value,
                //    Missing.Value, Missing.Value,
                //    Missing.Value, Missing.Value,
                //    Missing.Value, Missing.Value,
                //    Missing.Value, Missing.Value);
                Excel.Workbook theWorkbook = ExcelObjDesc.Workbooks.Add(Type.Missing);
                Excel.Sheets sheets = theWorkbook.Worksheets;
                Excel.Worksheet sheetSource = (Excel.Worksheet)sheets.get_Item(1);//Template
                //Header
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sheetSource.Cells[iStartRow, i + 1] = dt.Columns[i].ColumnName;
                    Excel.Range rng = ((Excel.Range)sheetSource.Cells[iStartRow, i + 1]);
                    rng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    rng.Interior.Color = ColorTranslator.ToOle(Color.LightBlue);
                    rng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                }
                iStartRow++;
                for (int i = 0; i < dt.Rows.Count; i++, iStartRow++)
                {
                    DataRow dr = dt.Rows[i];
                    int iCol = 0;
                    foreach (object obj in dr.ItemArray)
                    {

                        DateTime tmp = new DateTime();
                        if (DateTime.TryParse(obj.ToString(), out tmp))
                        {
                            sheetSource.Cells[iStartRow, ++iCol] = tmp.ToString("dd/MM/yyyy");
                        }
                        else
                            sheetSource.Cells[iStartRow, ++iCol] = obj;

                    }
                }
                //((Excel.Worksheet)theWorkbook.Application.ActiveWorkbook.Sheets[1]).Delete();
                //theWorkbook.Application.DisplayAlerts = false;
                //sheetSource.Delete();
                //theWorkbook.Application.DisplayAlerts = true;
                Excel.Range selectedRange = sheetSource.Range[sheetSource.Cells[1, 1], sheetSource.Cells[iStartRow, dt.Columns.Count + 1]];
                selectedRange.Columns.ColumnWidth = 25;
                theWorkbook.SaveAs(filename + DateTime.Now.ToString("yyyyMMddHHmmss"));
                if (bVisible)
                    ExcelObjDesc.Visible = true;
                else
                    ExcelObjDesc.Quit();

                return theWorkbook;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return null;
            }
        }

        public static Excel.Workbook ExportFileXlsx(DataTable dt, int iStartRow, bool bVisible, string strFileName)
        {
            try
            {
                //======================================
                // Create Result Excel
                //======================================
                Excel.Application ExcelObjDesc = new Excel.Application();
                Excel.Workbook theWorkbook = ExcelObjDesc.Workbooks.Add(Type.Missing);
                Excel.Sheets sheets = theWorkbook.Worksheets;
                Excel.Worksheet sheetSource = (Excel.Worksheet)sheets.get_Item(1);//Template
                //Header
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sheetSource.Cells[iStartRow, i + 1] = dt.Columns[i].ColumnName;
                    Excel.Range rng = ((Excel.Range)sheetSource.Cells[iStartRow, i + 1]);
                    rng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    rng.Interior.Color = ColorTranslator.ToOle(Color.LightBlue);
                    rng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                }
                iStartRow++;
                for (int i = 0; i < dt.Rows.Count; i++, iStartRow++)
                {
                    DataRow dr = dt.Rows[i];
                    int iCol = 0;
                    foreach (object obj in dr.ItemArray)
                    {

                        DateTime tmp = new DateTime();
                        if (DateTime.TryParse(obj.ToString(), out tmp))
                        {
                            sheetSource.Cells[iStartRow, ++iCol] = tmp.ToString("dd/MM/yyyy");
                        }
                        else
                            sheetSource.Cells[iStartRow, ++iCol] = obj;

                    }
                }
                Excel.Range selectedRange = sheetSource.Range[sheetSource.Cells[1, 1], sheetSource.Cells[iStartRow, dt.Columns.Count + 1]];
                selectedRange.Columns.ColumnWidth = 25;
                theWorkbook.SaveAs(strFileName);
                if (bVisible)
                    ExcelObjDesc.Visible = true;
                else
                    ExcelObjDesc.Quit();

                return theWorkbook;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return null;
            }
        }

        public static Excel.Workbook ExportClientVendorScreen(DataTable dt, int iStartRow, bool bPeriod, string type, DataSet ds)
        {
            try
            {
                //======================================
                // Create Result Excel
                //======================================
                Excel.Application ExcelObjDesc = new Excel.Application();
                Excel.Workbook theWorkbook = ExcelObjDesc.Workbooks.Add(Type.Missing);
                Excel.Sheets sheets = theWorkbook.Worksheets;
                Excel.Worksheet sheetSource = (Excel.Worksheet)theWorkbook.ActiveSheet;
                sheetSource.Name = type;
                //Header
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sheetSource.Cells[iStartRow, i + 1] = dt.Columns[i].ColumnName;
                    Excel.Range rng = ((Excel.Range)sheetSource.Cells[iStartRow, i + 1]);
                    rng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    rng.Interior.Color = ColorTranslator.ToOle(Color.LightBlue);
                    rng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                }
                iStartRow++;
                //Detail
                for (int i = 0; i < dt.Rows.Count; i++, iStartRow++)
                {
                    DataRow dr = dt.Rows[i];
                    int iCol = 0;
                    for (int j = 0; j < dr.ItemArray.Length; j++)
                    {
                        iCol += 1;
                        DateTime date = new DateTime();
                        if (bPeriod)
                        {
                            sheetSource.Cells[iStartRow, iCol].VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
                            if (j >= dr.ItemArray.Length - 10)
                                sheetSource.Cells[iStartRow, iCol] = dr[j].ToString().Replace(", ", "\r\n");
                            else
                                sheetSource.Cells[iStartRow, iCol] = dr[j].ToString();
                        }
                        else
                        {
                            sheetSource.Cells[iStartRow, iCol] = dr[j].ToString();
                        }
                    }
                }
                Excel.Range useRange = sheetSource.UsedRange;
                useRange.Columns.AutoFit();
                useRange.Rows.AutoFit();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    iStartRow = 1;
                    Excel.Worksheet worksheet = (Excel.Worksheet)theWorkbook.Worksheets.Add();
                    worksheet.Move(After: theWorkbook.Sheets[theWorkbook.Sheets.Count]);
                    worksheet = theWorkbook.Worksheets[theWorkbook.Sheets.Count];
                    if (type == "Client")
                    {
                        if (i == 0)
                            worksheet.Name = "Audit Right";
                        if (i == 1)
                            worksheet.Name = "EPD";
                        if (i == 2)
                            worksheet.Name = "Media Credit";
                        if (i == 3)
                            worksheet.Name = "Rebate";
                        if (i == 4)
                            worksheet.Name = "SAC";
                    }
                    else
                    {
                        if (i == 0)
                            worksheet.Name = "Sign Vendor Contract";
                        if (i == 1)
                            worksheet.Name = "EPD";
                        if (i == 2)
                            worksheet.Name = "Rebate";
                        if (i == 3)
                            worksheet.Name = "SAC";
                    }
                    /*if (i == 0)
                    {
                        if (type == "Client")
                            worksheet.Name = "Audit Right";
                        else
                            worksheet.Name = "Sign Vendor Contract";
                    }
                    if (i == 1)
                        worksheet.Name = "EPD";
                    if (i == 2)
                        worksheet.Name = "Media Credit";
                    if (i == 3)
                        worksheet.Name = "Rebate";
                    if (i == 4)
                        worksheet.Name = "SAC";*/
                    //Header
                    for (int j = 0; j < ds.Tables[i].Columns.Count; j++)
                    {
                        worksheet.Cells[iStartRow, j + 1] = ds.Tables[i].Columns[j].ColumnName;
                        Excel.Range rng = ((Excel.Range)worksheet.Cells[iStartRow, j + 1]);
                        rng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        rng.Interior.Color = ColorTranslator.ToOle(Color.LightBlue);
                        rng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    }
                    iStartRow++;
                    //Detail
                    for (int j = 0; j < ds.Tables[i].Rows.Count; j++, iStartRow++)
                    {
                        DataRow dr = ds.Tables[i].Rows[j];
                        int iCol = 0;
                        for (int k = 0; k < dr.ItemArray.Length; k++)
                        {
                            iCol += 1;
                            DateTime date = new DateTime();
                            if (DateTime.TryParse(dr[k].ToString(), out date))
                                worksheet.Cells[iStartRow, iCol] = date.ToString("yyyy/MM/dd");
                            else
                                worksheet.Cells[iStartRow, iCol] = dr[k].ToString();
                        }
                    }
                    Excel.Range range = worksheet.UsedRange;
                    range.Columns.AutoFit();
                }
                sheetSource.Activate();
                theWorkbook.SaveAs(type + " - " + DateTime.Now.ToString("yyyyMMddHHmmss"));
                ExcelObjDesc.Visible = true;
                return theWorkbook;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return null;
            }
        }

        /*public static Excel.Workbook ExportClientVendorScreen(DataTable dt, int iStartRow)
        {
            try
            {
                //======================================
                // Create Result Excel
                //======================================
                Excel.Application ExcelObjDesc = new Excel.Application();
                Excel.Workbook theWorkbook = ExcelObjDesc.Workbooks.Add(Type.Missing);
                Excel.Sheets sheets = theWorkbook.Worksheets;
                Excel.Worksheet sheetSource = (Excel.Worksheet)sheets.get_Item(1);//Template
                //Header
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i > 0)
                    {
                        sheetSource.Cells[iStartRow, i] = dt.Columns[i].ColumnName;
                        Excel.Range rng = ((Excel.Range)sheetSource.Cells[iStartRow, i]);
                        rng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        rng.Interior.Color = ColorTranslator.ToOle(Color.LightBlue);
                        rng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    }
                }
                iStartRow++;
                for (int i = 0; i < dt.Rows.Count; i++, iStartRow++)
                {
                    DataRow dr = dt.Rows[i];
                    int iCol = 0;
                    for (int j = 0; j < dr.ItemArray.Length; j++)
                    {
                        if (j > 0)
                        {
                            iCol += 1;
                            DateTime date = new DateTime();
                            if (DateTime.TryParse(dr[j].ToString(), out date))
                                sheetSource.Cells[iStartRow, iCol] = date.ToString("dd/MM/yyyy");
                            else
                                sheetSource.Cells[iStartRow, iCol] = dr[j];
                        }
                    }
                }
                Excel.Range selectedRange = sheetSource.Range[sheetSource.Cells[1, 1], sheetSource.Cells[iStartRow, dt.Columns.Count + 1]];
                selectedRange.Columns.ColumnWidth = 25;
                theWorkbook.SaveAs(DateTime.Now.ToString("yyyyMMddHHmmss"));
                ExcelObjDesc.Visible = true;
                return theWorkbook;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return null;
            }
        }*/

        public static Excel.Workbook ExportMSTBusinessDefinition(DataTable dt, int iStartRow)
        {
            try
            {
                //======================================
                // Create Result Excel
                //======================================
                Excel.Application ExcelObjDesc = new Excel.Application();
                Excel.Workbook theWorkbook = ExcelObjDesc.Workbooks.Add(Type.Missing);
                Excel.Sheets sheets = theWorkbook.Worksheets;
                Excel.Worksheet sheetSource = (Excel.Worksheet)sheets.get_Item(1);//Template
                sheetSource.Columns[1].ColumnWidth = 20;
                sheetSource.Columns[2].ColumnWidth = 15;
                sheetSource.Columns[3].ColumnWidth = 40;
                sheetSource.Columns[4].ColumnWidth = 110;
                sheetSource.Columns[5].ColumnWidth = 50;
                //Header
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i > 1)
                    {
                        int iCol = i - 1;
                        sheetSource.Cells[iStartRow, iCol] = dt.Columns[i].ColumnName.Replace("_", " ");
                        Excel.Range rng = ((Excel.Range)sheetSource.Cells[iStartRow, iCol]);
                        rng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        rng.Interior.Color = ColorTranslator.ToOle(Color.LightBlue);
                        rng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    }
                }
                iStartRow++;
                for (int i = 0; i < dt.Rows.Count; i++, iStartRow++)
                {
                    DataRow dr = dt.Rows[i];
                    int iCol = 0;
                    for (int j = 0; j < dr.ItemArray.Length; j++)
                    {
                        if (j > 1)
                        {
                            iCol += 1;
                            sheetSource.Cells[iStartRow, iCol] = dr[j];
                            sheetSource.Cells[iStartRow, iCol].Style.VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
                            int mergeRow = Convert.ToInt32(dr["CountRow"]);
                            if (mergeRow > 0 && j < dr.ItemArray.Length - 1)
                            {
                                if (j == dr.ItemArray.Length - 2)
                                {
                                    sheetSource.Cells[iStartRow, iCol].Style.WrapText = true;
                                }
                                Excel.Range rng = sheetSource.Range[sheetSource.Cells[iStartRow, iCol], sheetSource.Cells[iStartRow + mergeRow - 1, iCol]];
                                rng.Merge();
                            }
                        }
                    }
                }
                Excel.Range useRange = sheetSource.UsedRange;
                useRange.Columns.AutoFit();
                useRange.Rows.AutoFit();
                ExcelObjDesc.ActiveWindow.Zoom = 75;
                theWorkbook.SaveAs(DateTime.Now.ToString("yyyyMMddHHmmss"));
                ExcelObjDesc.Visible = true;
                return theWorkbook;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return null;
            }
        }

        public static Excel.Workbook ExportXlsx(DataGridView gv, int iStartRow, bool bVisible/*, string strSavePath*/)
        {
            return ExportXlsx(null, gv, iStartRow, bVisible);
        }
        public static Excel.Workbook ExportXlsx(DataTable dtHeader, DataGridView gv, int iStartRow, bool bVisible/*, string strSavePath*/)
        {
            try
            {
                //======================================
                // Create Result Excel
                //======================================
                Excel.Application ExcelObjDesc = new Excel.Application();
                Excel.Workbook theWorkbook = ExcelObjDesc.Workbooks.Add(Type.Missing);
                Excel.Sheets sheets = theWorkbook.Worksheets;
                Excel.Worksheet sheetSource = (Excel.Worksheet)sheets.get_Item(1);//Template
                //=======================================
                // Table Header
                //=======================================
                if (dtHeader != null)
                {
                    for (int i = 0; i < dtHeader.Rows.Count; i++)
                    {
                        DataRow dr = dtHeader.Rows[i];
                        for (int j = 0; j < dtHeader.Columns.Count; j++)
                        {
                            sheetSource.Cells[i + 1, j + 1] = dr[j];
                        }
                    }
                }
                //=======================================
                //Row Header
                //=======================================
                int iColHeader = 1;
                for (int i = 0; i < gv.Columns.Count; i++)
                {
                    if (gv.Columns[i].Visible == true && gv.Columns[i].GetType() != typeof(DataGridViewImageColumn))
                    {
                        sheetSource.Cells[iStartRow, iColHeader] = gv.Columns[i].HeaderText;
                        Excel.Range rng = ((Excel.Range)sheetSource.Cells[iStartRow, iColHeader]);
                        rng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        rng.Interior.Color = ColorTranslator.ToOle(Color.LightBlue);
                        rng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        iColHeader++;
                    }
                }
                iStartRow++;
                for (int i = 0; i < gv.Rows.Count; i++, iStartRow++)
                {
                    DataGridViewRow dr = gv.Rows[i];
                    int iCol = 0;
                    for (int j = 0; j < gv.Columns.Count; j++)
                    {
                        if (gv.Columns[j].Visible == true && gv.Columns[j].GetType() != typeof(DataGridViewImageColumn))
                        {
                            DateTime tmp = new DateTime();
                            if (DateTime.TryParse(dr.Cells[j].Value.ToString(), out tmp))
                            {
                                ((Excel.Range)sheetSource.Cells[iStartRow, iCol + 1]).EntireColumn.NumberFormat = "dd/mm/yyyy h:mm";
                                sheetSource.Cells[iStartRow, iCol + 1] = tmp;
                            }
                            else
                            {
                                sheetSource.Cells[iStartRow, iCol + 1] = dr.Cells[j].Value.ToString().Replace("\r\nสาขา1\r\nเบิก", "").Replace("\r\nสาขา2\r\nเบิก", "").Replace("\r\nเสีย\r\nเบิก", "").Replace("\r\nเข้า", "");
                            }
                            Excel.Range rng = ((Excel.Range)sheetSource.Cells[iStartRow, iCol + 1]);
                            rng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                            iCol++;
                        }
                    }
                }
                //((Excel.Worksheet)theWorkbook.Application.ActiveWorkbook.Sheets[1]).Delete();
                //theWorkbook.Application.DisplayAlerts = false;
                //sheetSource.Delete();
                //theWorkbook.Application.DisplayAlerts = true;
                Excel.Range selectedRange = sheetSource.Range[sheetSource.Cells[1, 1], sheetSource.Cells[iStartRow, gv.Columns.Count + 1]];
                selectedRange.Columns.AutoFit();
                theWorkbook.SaveAs(DateTime.Now.ToString("yyyyMMddHHmmss"));
                if (bVisible)
                    ExcelObjDesc.Visible = true;
                else
                    ExcelObjDesc.Quit();

                return theWorkbook;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return null;
            }
        }

        public static Excel.Workbook ExportXlsx(DataTable dtHeader, DataTable dt, int iStartRow, bool bVisible/*, string strSavePath*/)
        {
            try
            {
                //======================================
                // Create Result Excel
                //======================================
                Excel.Application ExcelObjDesc = new Excel.Application();
                Excel.Workbook theWorkbook = ExcelObjDesc.Workbooks.Add(Type.Missing);
                Excel.Sheets sheets = theWorkbook.Worksheets;
                Excel.Worksheet sheetSource = (Excel.Worksheet)sheets.get_Item(1);//Template
                //=======================================
                // Table Header
                //=======================================
                if (dtHeader != null)
                {
                    for (int i = 0; i < dtHeader.Rows.Count; i++)
                    {
                        DataRow dr = dtHeader.Rows[i];
                        for (int j = 0; j < dtHeader.Columns.Count; j++)
                        {
                            sheetSource.Cells[i + 1, j + 1] = dr[j];
                        }
                    }
                }
                //=======================================
                //Row Header
                //=======================================
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sheetSource.Cells[iStartRow, i + 1] = dt.Columns[i].ColumnName;
                    Excel.Range rng = ((Excel.Range)sheetSource.Cells[iStartRow, i + 1]);
                    rng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    rng.Interior.Color = ColorTranslator.ToOle(Color.Orange);
                    rng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                }
                iStartRow++;
                for (int i = 0; i < dt.Rows.Count; i++, iStartRow++)
                {
                    DataRow dr = dt.Rows[i];
                    int iCol = 0;
                    foreach (object obj in dr.ItemArray)
                    {

                        DateTime tmp = new DateTime();
                        if (DateTime.TryParse(obj.ToString(), out tmp))
                        {
                            if (tmp.ToString("hh:mm:ss") == "12:00:00")
                            {
                                Excel.Range rg = (Excel.Range)sheetSource.Cells[iStartRow, iCol + 1];
                                rg.EntireColumn.NumberFormat = "dd/MM/yyyy";

                                sheetSource.Cells[iStartRow, iCol + 1] = tmp;//.ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                Excel.Range rg = (Excel.Range)sheetSource.Cells[iStartRow, iCol + 1];
                                rg.EntireColumn.NumberFormat = "dd/MM/yyyy hh:mm:ss";
                                sheetSource.Cells[iStartRow, iCol + 1] = tmp;//.ToString("dd/MM/yyyy hh:mm:ss");
                            }
                        }
                        else
                            sheetSource.Cells[iStartRow, iCol + 1] = obj;
                        Excel.Range rng = ((Excel.Range)sheetSource.Cells[iStartRow, iCol + 1]);
                        rng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        iCol++;
                    }
                }
                //((Excel.Worksheet)theWorkbook.Application.ActiveWorkbook.Sheets[1]).Delete();
                //theWorkbook.Application.DisplayAlerts = false;
                //sheetSource.Delete();
                //theWorkbook.Application.DisplayAlerts = true;
                Excel.Range selectedRange = sheetSource.Range[sheetSource.Cells[1, 1], sheetSource.Cells[iStartRow, dt.Columns.Count + 1]];
                selectedRange.Columns.AutoFit();
                theWorkbook.SaveAs(DateTime.Now.ToString("yyyyMMddHHmmss"));
                if (bVisible)
                    ExcelObjDesc.Visible = true;
                else
                    ExcelObjDesc.Quit();

                return theWorkbook;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return null;
            }
        }

        public static Excel.Workbook ExportTemplateXlsx(DataTable dt, int iStartRow, bool bVisible, string strTemplatePath, string startColumn)
        {
            try
            {
                //======================================
                // Create Result Excel
                //======================================
                Excel.Application ExcelObjDesc = new Excel.Application();
                Excel.Workbook theWorkbook = (Excel.Workbook)(ExcelObjDesc.Workbooks.Open(strTemplatePath));
                Excel.Sheets sheets = theWorkbook.Worksheets;
                Excel.Worksheet sheetSource = (Excel.Worksheet)sheets.get_Item(1);//Template
                //=======================================
                // Convert Data
                //=======================================
                string[,] RawData = new string[dt.Rows.Count, dt.Columns.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        RawData[i, j] = dt.Rows[i][j].ToString();
                    }
                    if (i < dt.Rows.Count - 2)
                    {
                        sheetSource.Rows[i + iStartRow].Insert(Excel.XlInsertShiftDirection.xlShiftDown, sheetSource.Rows[i + iStartRow].Copy(System.Type.Missing));
                    }
                }
                //=======================================
                // Write Data
                //=======================================
                Excel.Range range;
                range = sheetSource.get_Range(startColumn + iStartRow.ToString(), Missing.Value);
                range = range.get_Resize(dt.Rows.Count, dt.Columns.Count);
                range.set_Value(Missing.Value, RawData);
                theWorkbook.SaveAs(DateTime.Now.ToString("yyyyMMddHHmmss"));
                if (bVisible)
                    ExcelObjDesc.Visible = true;
                else
                    ExcelObjDesc.Quit();

                return theWorkbook;
            }
            catch (Exception ex)
            {
                GMessage.MessageError(ex);
                return null;
            }
        }

        public static void ExcelSetValueRangeString(Excel.Worksheet sheet, string strFirstCell, int lastRow, DataTable dt, string strColName)
        {
            string[,] strValue = new string[lastRow, 1];
            for (int i = 0; i < lastRow; i++)
                strValue[i, 0] = dt.Rows[i][strColName].ToString();

            Excel.Range rang;
            rang = sheet.get_Range(strFirstCell, Missing.Value);
            rang = rang.get_Resize(lastRow, 1);
            rang.set_Value(Missing.Value, strValue);
        }

        public static void ExcelSetValueRangeString(Excel.Worksheet sheet, string strFirstCell, int lastRow, DataTable dt, int startColumnIndex)
        {
            string[,] strValue = new string[lastRow, dt.Columns.Count - startColumnIndex];

            for (int i = 0; i < lastRow; i++)//Row
            {
                for (int j = 0; j < dt.Columns.Count - startColumnIndex; j++)//Column
                {
                    strValue[i, j] = dt.Rows[i][j + startColumnIndex].ToString();
                }

            }

            Excel.Range rang;
            rang = sheet.get_Range(strFirstCell, Missing.Value);
            rang = rang.get_Resize(lastRow, dt.Columns.Count - startColumnIndex);
            //rang = rang.get_Resize(lastRow, 1);
            rang.set_Value(Missing.Value, strValue);
        }
        public static void ExcelSetValueRangeDouble(Excel.Worksheet sheet, string strFirstCell, int lastRow, DataTable dt, string strColName)
        {
            double[,] dobValue = new double[lastRow, 1];
            for (int i = 0; i < lastRow; i++)
                dobValue[i, 0] = dt.Rows[i][strColName].ToString() == "" ? 0 : Convert.ToDouble(dt.Rows[i][strColName]);

            Excel.Range rang;
            rang = sheet.get_Range(strFirstCell, Missing.Value);
            rang = rang.get_Resize(lastRow, 1);
            rang.set_Value(Missing.Value, dobValue);
        }

        public static void ExcelSetValueRangeDouble(Excel.Worksheet sheet, string strFirstCell, int lastRow, DataTable dt, int startColumnIndex)
        {
            double[,] dobValue = new double[lastRow, dt.Columns.Count - startColumnIndex];

            for (int i = 0; i < lastRow; i++)//Row
            {
                for (int j = 0; j < dt.Columns.Count - startColumnIndex; j++)//Column
                {
                    dobValue[i, j] = dt.Rows[i][j + startColumnIndex].ToString() == "" ? 0 : Convert.ToDouble(dt.Rows[i][j + startColumnIndex]);
                }

            }

            Excel.Range rang;
            rang = sheet.get_Range(strFirstCell, Missing.Value);
            rang = rang.get_Resize(lastRow, dt.Columns.Count - startColumnIndex);
            //rang = rang.get_Resize(lastRow, 1);
            rang.set_Value(Missing.Value, dobValue);
        }

        public static void ExcelSetValueString(Excel.Worksheet sheet, string strCol, int iRow, string strValue)
        {
            Excel.Range rang = sheet.get_Range($"{strCol}{iRow}:{strCol}{iRow}");
            rang.Value = strValue.ToString();
        }

        public static void ExcelSetValueString(Excel.Worksheet sheet, int iRow, int iCol, string strValue)
        {
            object rangeObject = sheet.Cells[iRow, iCol];
            Excel.Range range = (Excel.Range)rangeObject;
            range.Value = strValue.ToString();
        }

        public static void ExcelSetFormular(Excel.Worksheet sheet, string strCol, int iRow, string strValue)
        {
            Excel.Range rang = sheet.get_Range($"{strCol}{iRow}:{strCol}{iRow}");
            rang.Formula = strValue;
        }

        public static void ExcelSetValueStringIncremental(Excel.Worksheet sheet, int iRow, int iCol, string strValue)
        {
            object rangeObject = sheet.Cells[iRow, iCol];
            Excel.Range range = (Excel.Range)rangeObject;
            string strV = Convert.ToString(range.Value);
            if (strV != null && strV.Trim() != "")
            {
                strV = (Convert.ToDouble(strV) + Convert.ToDouble(strValue)).ToString();
                range.Value = strV == "0" ? "" : strV.ToString();
            }
            else
                range.Value = strValue == "0" ? "" : strValue.ToString();
        }

        public static void ExcelSetValueStringFullTable(Excel.Worksheet sheet, int iStartRow, int iStartCol, int iNewLine, DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ExcelSetValueString(sheet, iStartRow + i, iStartCol + j, dt.Rows[i][j].ToString());
                }
                if (i < dt.Rows.Count - iNewLine)
                {
                    sheet.Rows[i + iStartRow + 1].Insert(Excel.XlInsertShiftDirection.xlShiftDown, sheet.Rows[i + iStartRow + 1].Copy(Type.Missing));
                }
            }
            Clipboard.Clear();
        }

        public static void ExcelSetValueStringFullTableAdviceNote(Excel.Worksheet sheet, int iStartRow, int iStartCol, int iNewLine, DataTable dt)
        {
            Excel.Range range;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ExcelSetValueString(sheet, iStartRow + i, iStartCol + j, dt.Rows[i][j].ToString());
                }
                range = sheet.get_Range($"{iStartRow + i}:{iStartRow + i}");
                range.Rows.AutoFit();
                if (i < dt.Rows.Count - iNewLine)
                {
                    sheet.Rows[i + iStartRow + 1].Insert(Excel.XlInsertShiftDirection.xlShiftDown, sheet.Rows[i + iStartRow + 1].Copy(Type.Missing));
                }
            }
            Clipboard.Clear();
        }
    }
}
