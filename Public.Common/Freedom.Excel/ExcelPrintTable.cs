

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Public.Log;

namespace Public.Common
{
    //public class ExcelPrintTable
    //{
    //    private bool isDisposed = false;
    //    object missing = System.Reflection.Missing.Value;
    //    Microsoft.Office.Interop.Excel.Application app = null;
    //    Microsoft.Office.Interop.Excel.Workbook workBook = null;
    //    Microsoft.Office.Interop.Excel.Worksheet workSheet = null;

    //    public ExcelPrintTable()
    //    {
    //        app = new Microsoft.Office.Interop.Excel.Application();
    //        app.Visible = false;
    //        app.DisplayAlerts = false;
    //    }


    //    /// <summary>
    //    /// 打开Excel文件
    //    /// </summary>
    //    /// <param name="templateFile"></param>
    //    /// <returns></returns>
    //    public ResponseModel Open(string templateFile)
    //    {
    //        ResponseModel ret = ResponseModel.CreateSuccess();
    //        try
    //        {
    //            workBook = app.Workbooks.Open(templateFile, missing, missing, missing, missing, missing,
    //                  missing, missing, missing, missing, missing, missing, missing, missing, missing);
    //            workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Sheets.get_Item(1);
    //        }
    //        catch (Exception ex)
    //        {
    //            ret = ResponseModel.CreateError(ex.Message);
    //        }
    //        return ret;
    //    }

    //    /// <summary>
    //    /// 设置单元格文字
    //    /// </summary>
    //    /// <param name="cellName"></param>
    //    /// <param name="cellValue"></param>
    //    /// <returns></returns>
    //    public ResponseModel SetCellValue(string cellName, string cellValue)
    //    {
    //        if (workSheet == null)
    //            return ResponseModel.CreateError("未打开文件");

    //        if (string.IsNullOrEmpty(cellName))
    //            return ResponseModel.CreateError("参数错误，cellName参数不能为空");

    //        try
    //        {
    //            if (cellName.IndexOf("textBox") > 0)
    //            {
    //                Logger.Debug("--cellname---" + cellName);
    //                Logger.Debug("--cellvalue---" + cellValue);
    //                ((Microsoft.Office.Interop.Excel.TextBox)(workSheet.TextBoxes(cellName.Replace("_textBox", "")))).Text = cellValue;
    //            }
    //            else
    //            {
    //                Microsoft.Office.Interop.Excel.Range range = workSheet.get_Range(cellName, missing);
    //                if (range == null)
    //                    return ResponseModel.CreateError("未找到单元格:" + cellName);

    //                try
    //                {
    //                    if (cellValue.Replace("*", "") != "")
    //                    {
    //                        range.Value2 = cellValue;
    //                    }
    //                }
    //                catch (Exception ex)
    //                {
    //                    Logger.Debug(string.Format("设置单元格({0})的值为({1})异常:{2}", cellName, cellValue, ex.Message));
    //                    return ResponseModel.CreateError(ex.Message);
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            return ResponseModel.CreateError("未找到单元格:" + cellName);
    //        }

    //        return ResponseModel.CreateSuccess();
    //    }

    //    /// <summary>
    //    /// 设置单元格是否被选中
    //    /// </summary>
    //    /// <param name="cellName"></param>
    //    /// <returns></returns>
    //    public ResponseModel SetCellChecked(string cellName, bool isFill = false)
    //    {
    //        if (workSheet == null)
    //            return ResponseModel.CreateError("未打开文件");

    //        if (string.IsNullOrEmpty(cellName))
    //            return ResponseModel.CreateError("参数错误，cellName参数不能为空");

    //        try
    //        {
    //            Microsoft.Office.Interop.Excel.Range range = workSheet.get_Range(cellName, missing);
    //            if (range == null)
    //                return ResponseModel.CreateError("未找到单元格:" + cellName);

    //            try
    //            {
    //                string zifu = "☑";
    //                if (isFill == true)
    //                    zifu = "■";
    //                string cellValue = range.Value2.ToString().Replace("□", zifu);
    //                range.Value2 = cellValue;
    //            }
    //            catch (Exception ex)
    //            {
    //                Logger.Debug(string.Format("设置单元格({0})异常:{1}", cellName, ex.Message));
    //                return ResponseModel.CreateError(ex.Message);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Logger.Debug("未找到单元格:" + cellName);
    //        }

    //        return ResponseModel.CreateSuccess();
    //    }

    //    /// <summary>
    //    /// 设置单元格中的一部分为选中
    //    /// </summary>
    //    /// <param name="cellName"></param>
    //    /// <param name="selectedText"></param>
    //    /// <returns></returns>
    //    public ResponseModel SetCellChecked(string cellName, string selectedText, bool isFill = false)
    //    {
    //        if (workSheet == null)
    //            return ResponseModel.CreateError("未打开文件");

    //        if (string.IsNullOrEmpty(cellName))
    //            return ResponseModel.CreateError("参数错误，cellName参数不能为空");

    //        try
    //        {
    //            Microsoft.Office.Interop.Excel.Range range = workSheet.get_Range(cellName, missing);
    //            if (range == null)
    //                return ResponseModel.CreateError("未找到单元格:" + cellName);

    //            try
    //            {
    //                string cellValue = range.Value2.ToString();
    //                string zifu = "☑";
    //                if (isFill == true)
    //                    zifu = "■";
    //                if (selectedText != "" && selectedText != null)
    //                {
    //                    cellValue = cellValue.Replace("□" + selectedText, zifu + selectedText);
    //                    range.Value2 = cellValue;
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                Logger.Debug(string.Format("设置单元格({0})异常:{1}", cellName, ex.Message));
    //                return ResponseModel.CreateError(ex.Message);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Logger.Debug("未找到单元格:" + cellName);
    //        }

    //        return ResponseModel.CreateSuccess();
    //    }


    //    /// <summary>
    //    /// 获取单元格的值
    //    /// </summary>
    //    /// <param name="cellName"></param>
    //    /// <returns></returns>
    //    public string GetCellValue(string cellName)
    //    {
    //        if (workSheet == null)
    //        {
    //            Logger.Debug("未打开文件");
    //            return string.Empty;
    //        }

    //        if (string.IsNullOrEmpty(cellName))
    //        {
    //            Logger.Debug("参数错误，cellName参数不能为空");
    //            return string.Empty;
    //        }

    //        Microsoft.Office.Interop.Excel.Range range = workSheet.get_Range(cellName, missing);
    //        if (range == null)
    //        {
    //            Logger.Debug("未找到单元格:" + cellName);
    //            return string.Empty;
    //        }
    //        return range.Value2.ToString();
    //    }

    //    /// <summary>
    //    /// 插入图片
    //    /// </summary>
    //    /// <param name="picFilePath"></param>
    //    /// <param name="cellName"></param>
    //    /// <param name="x"></param>
    //    /// <param name="y"></param>
    //    /// <param name="w"></param>
    //    /// <param name="h"></param>
    //    /// <returns></returns>
    //    public ResponseModel InsertPicture(string picFilePath, string cellName)
    //    {
    //        if (workSheet == null)
    //            return ResponseModel.CreateError("未打开文件");

    //        if (string.IsNullOrEmpty(cellName))
    //            return ResponseModel.CreateError("参数错误，cellName参数不能为空");


    //        Microsoft.Office.Interop.Excel.Range rag = workSheet.get_Range(cellName, missing);
    //        if (rag == null)
    //            return ResponseModel.CreateError("未找到单元格:" + cellName);

    //        try
    //        {
    //            Logger.Debug("插入图片开始");
    //            rag.Select();

    //            float PicLeft, PicTop, PicWidth, PicHeight;
    //            PicLeft = Convert.ToSingle(rag.Left); //30f;
    //            PicTop = Convert.ToSingle(rag.Top); //3f;
    //            PicWidth = Convert.ToSingle(rag.Width); //320f;
    //            PicHeight = Convert.ToSingle(rag.Height); //64f;

    //            Microsoft.Office.Interop.Excel.Pictures pics = (Microsoft.Office.Interop.Excel.Pictures)workSheet.Pictures(missing);
    //            Microsoft.Office.Interop.Excel.Picture p = pics.Insert(picFilePath, missing);

    //            p.Locked = false;
    //            p.Left = PicLeft + 2;
    //            p.Top = PicTop + 2;
    //            //p.Width = PicWidth;
    //            //p.Height = PicHeight;
    //            rag.Select();
    //            Logger.Debug("插入图片结束");
    //        }
    //        catch (Exception ex)
    //        {
    //            Logger.Debug(string.Format("插入图片异常({0})异常:{1}", picFilePath, ex.Message));
    //            return ResponseModel.CreateError(ex.Message);
    //        }

    //        return ResponseModel.CreateSuccess();
    //    }



    //    /// <summary>
    //    /// 保存到文件
    //    /// </summary>
    //    /// <param name="filePath"></param>
    //    /// <returns></returns>
    //    public ResponseModel SaveToFile(string filePath)
    //    {
    //        if (workBook == null)
    //            return ResponseModel.CreateError("文件未打开");
    //        if (string.IsNullOrEmpty(filePath))
    //            return ResponseModel.CreateError("文件名不能为空");

    //        try
    //        {
    //            workBook.SaveAs(filePath, missing, missing, missing, missing, missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing, missing);
    //            workBook.Close();
    //        }
    //        catch (Exception ex)
    //        {
    //            Logger.Debug("保存文件异常:" + ex.Message);
    //            return ResponseModel.CreateError(ex.Message);
    //        }

    //        return ResponseModel.CreateSuccess();
    //    }

    //    public void Close()
    //    {
    //        try
    //        {
    //            if (workBook != null)
    //                workBook.Close(null, null, null);

    //            if (app != null)
    //            {
    //                app.Workbooks.Close();
    //                app.Application.Quit();
    //                app.Quit();
    //            }

    //            System.Runtime.InteropServices.Marshal.ReleaseComObject(workSheet);
    //            System.Runtime.InteropServices.Marshal.ReleaseComObject(workBook);
    //            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);

    //            workSheet = null;
    //            workBook = null;
    //            app = null;
    //        }
    //        catch (Exception ex)
    //        {
    //            Logger.Debug("Workbooks.Close：" + ex.Message);
    //        }

    //        GC.Collect();
    //        Logger.Debug("GC.Collect");
    //        Logger.Debug("Excel_Close");
    //    }

    //    public bool Disposed
    //    {
    //        get { return isDisposed; }
    //        private set
    //        {
    //            isDisposed = value;
    //        }
    //    }

    //    public void Dispose()
    //    {
    //        if (!Disposed)
    //        {
    //            Close();
    //        }
    //        Disposed = true;
    //    }

    //    public ResponseModel Print(string fileName)
    //    {
    //        ResponseModel ret = ResponseModel.CreateSuccess();
    //        if (string.IsNullOrEmpty(fileName))
    //            return ResponseModel.CreateError("文件名不能为空");
    //        ExcelPrintTable excel = new ExcelPrintTable();
    //        try
    //        {
    //            excel.Open(fileName);
    //            //获取当前默认打印机
    //            string defaultPrinter = GetDefaultPrinter();
    //            //将指定的打印机设为默认打印机
    //            SetDefaultPrinter(defaultPrinter);
    //            excel.Print(1);
    //            excel.Close();
    //        }
    //        catch (Exception ex)
    //        {
    //            Logger.Debug("OnlyPrint:" + ex.Message);
    //        }
    //        return ret;
    //    }

    //    public ResponseModel Print()
    //    {
    //        //return Print(QJTConfig.QJTModel.PrintCount);
    //        return Print(1);
    //    }


    //    public ResponseModel Print(int printCount)
    //    {
    //        try
    //        {
    //            object printCopys = printCount;
    //            workBook.PrintOut(missing, missing, printCopys, missing, missing, missing, missing, missing);
    //        }
    //        catch (Exception ex)
    //        {
    //            Logger.Debug("打印异常:" + ex.Message);
    //            return ResponseModel.CreateError(ex.Message);
    //        }

    //        return ResponseModel.CreateSuccess();
    //    }

    //    [DllImport("Winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
    //    private static extern bool SetDefaultPrinter(string printerName);

    //    [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]

    //    private static extern bool GetDefaultPrinter(StringBuilder pszBuffer, ref int pcchBuffer);
    //    /// <summary>
    //    /// 获取默认的打印机
    //    /// </summary>
    //    /// <returns></returns>
    //    static string GetDefaultPrinter()
    //    {
    //        const int ERROR_FILE_NOT_FOUND = 2;

    //        const int ERROR_INSUFFICIENT_BUFFER = 122;

    //        int pcchBuffer = 0;

    //        if (GetDefaultPrinter(null, ref pcchBuffer))
    //        {
    //            return null;
    //        }

    //        int lastWin32Error = Marshal.GetLastWin32Error();

    //        if (lastWin32Error == ERROR_INSUFFICIENT_BUFFER)
    //        {
    //            StringBuilder pszBuffer = new StringBuilder(pcchBuffer);
    //            if (GetDefaultPrinter(pszBuffer, ref pcchBuffer))
    //            {
    //                return pszBuffer.ToString();
    //            }

    //            lastWin32Error = Marshal.GetLastWin32Error();
    //        }
    //        if (lastWin32Error == ERROR_FILE_NOT_FOUND)
    //        {
    //            return null;
    //        }

    //        throw new Win32Exception(Marshal.GetLastWin32Error());


    //    }
    //}
}
