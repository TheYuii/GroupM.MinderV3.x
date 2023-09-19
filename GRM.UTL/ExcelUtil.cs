using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Windows.Forms;

namespace GRM.UTL
{
    public class ExcelUtil
    {
        public static Excel.Workbook ExportXlsx(DataTable dt, int iStartRow,bool bVisible/*, string strSavePath*/)
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
                    sheetSource.Cells[iStartRow, i+1] = dt.Columns[i].ColumnName;
                }
                iStartRow++;
                for (int i = 0; i < dt.Rows.Count; i++,iStartRow++)
                {
                    DataRow dr = dt.Rows[i];
                    int iCol = 0;
                    foreach (object obj in dr.ItemArray)
                    {

                        DateTime tmp = new DateTime();
                        if (DateTime.TryParse(obj.ToString(), out tmp))
                        {
                            sheetSource.Cells[iStartRow, ++iCol] = tmp.ToString("dd/MM/yyyy hh:mm:ss");
                        }
                        else
                            sheetSource.Cells[iStartRow, ++iCol] = obj; 
                        
                    }
                }
                //((Excel.Worksheet)theWorkbook.Application.ActiveWorkbook.Sheets[1]).Delete();
                //theWorkbook.Application.DisplayAlerts = false;
                //sheetSource.Delete();
                //theWorkbook.Application.DisplayAlerts = true;
                Excel.Range selectedRange = sheetSource.Range[sheetSource.Cells[1, 1], sheetSource.Cells[iStartRow, dt.Columns.Count+1]];
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
        public static Excel.Workbook ExportXlsx(DataGridView gv, int iStartRow, bool bVisible/*, string strSavePath*/)
        {
            return ExportXlsx(null, gv, iStartRow, bVisible);
        }
        public static Excel.Workbook ExportXlsx(DataTable dtHeader,DataGridView gv, int iStartRow, bool bVisible/*, string strSavePath*/)
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
                        rng.Interior.Color =  System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Orange);
                        rng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        iColHeader++;
                    }
                }
                iStartRow++;
                for (int i = 0; i < gv.Rows.Count; i++, iStartRow++)
                {
                    DataGridViewRow dr = gv.Rows[i];
                    int iCol = 0;
                    for (int j = 0; j < gv.Columns.Count ;j++ )
                    {
                        if (gv.Columns[j].Visible == true && gv.Columns[j].GetType() != typeof(DataGridViewImageColumn))
                        {
                            DateTime tmp = new DateTime();
                            if (dr.Cells[j].Value == null)
                            {
                                sheetSource.Cells[iStartRow, iCol + 1] = "";
                            }
                            else if (DateTime.TryParse(dr.Cells[j].Value.ToString(), out tmp))
                            {
                                //((Excel.Range)sheetSource.Cells[iStartRow, iCol + 1]).EntireColumn.NumberFormat = "dd/mm/yyyy h:mm";
                                sheetSource.Cells[iStartRow, iCol + 1] = dr.Cells[j].Value;
                            }
                            else
                            {
                                sheetSource.Cells[iStartRow, iCol + 1] = dr.Cells[j].Value.ToString().Replace("\r\nสาขา1\r\nเบิก", "").Replace("\r\nสาขา2\r\nเบิก", "").Replace("\r\nเสีย\r\nเบิก", "").Replace("\r\nเข้า", "");
                            }
                            Excel.Range rng = ((Excel.Range)sheetSource.Cells[iStartRow, iCol + 1]);
                            rng.Cells.Interior.Color = gv.Rows[i].Cells[j].Style.BackColor;
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
                    rng.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Orange);
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
    }
}
