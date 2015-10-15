using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace StoreManagement.Data.GeneralHelper
{
    public class ExcelHelper
    {


        //public static void DataTableToExcel(DataTable dt, String fileName)
        //{

        //    //Make a new npoi workbook
        //    HSSFWorkbook hssfworkbook = new HSSFWorkbook();
        //    //Here I am making sure that I am giving the file name the right
        //    //extension:
        //    string filename = "";
        //    if (fileName.EndsWith(".xls"))
        //        filename = fileName;
        //    else
        //        filename = fileName + ".xls";
        //    //This starts the dialogue box that allows the user to download the file
        //    System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
        //    //Response.ContentType = “application/vnd.ms-excel”;
        //    //Response.AddHeader(“Content-Disposition”, string.Format(“attachment;filename={0}”, filename));
        //    Response.Clear();
        //    //make a new sheet – name it any excel-compliant string you want
        //    var sheet1 = hssfworkbook.CreateSheet("Sheet 1");
        //    //make a header row
        //    var row1 = sheet1.CreateRow(0);
        //    //Puts in headers (these are table row headers, omit if you
        //    //just need a straight data dump
        //    for (int j = 0; j < dt.Columns.Count; j++)
        //    {
        //        var cell = row1.CreateCell(j);
        //        String columnName = dt.Columns[j].ToString();
        //        cell.SetCellValue(columnName);
        //    }

        //    //loops through data

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        var row = sheet1.CreateRow(i + 1);
        //        for (int j = 0; j < dt.Columns.Count; j++)
        //        {
        //            var cell = row.CreateCell(j);
        //            String columnName = dt.Columns[j].ToString();
        //            cell.SetCellValue(dt.Rows[i][columnName].ToString());
        //        }
        //    }

        //    //writing the data to binary from memory

        //    Response.BinaryWrite(WriteToStream(hssfworkbook).GetBuffer());

        //    Response.End();



        //}



        static MemoryStream WriteToStream(HSSFWorkbook hssfworkbook)
        {

            //Write the stream data of workbook to the root directory
            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file);
            file.Seek(0, 0);
            return file;

        }

        public static HSSFWorkbook CreateWorkBook(List<DataTable> dtList)
        {
            var workbook = new HSSFWorkbook();

            var headerStyle = GetCellStyle(workbook);
            var headerStyle1 = GetCellStyle2(workbook);
            var headerStyle3 = GetCellStyle3(workbook);
            int i = 1;
            foreach (DataTable dt in dtList)
            {
                String name = String.Format("workbook-{0}", i);
                if (!String.IsNullOrEmpty(dt.TableName))
                {
                    name = dt.TableName;
                }
                else
                {
                    i++;
                }
                var sheet = workbook.CreateSheet(name);
                ExportDataTableToSheet(dt, sheet, headerStyle, headerStyle1, headerStyle3);
            }
            return workbook;
        }
        private static ICellStyle GetCellStyle3(HSSFWorkbook workbook)
        {
            ICellStyle headerStyle = workbook.CreateCellStyle();

            //headerStyle.FillForegroundColor = IndexedColors.White.Index;
            //headerStyle.FillBackgroundColor = IndexedColors.Red.Index;
            //headerStyle.FillPattern = FillPattern.SolidForeground;

            HSSFFont font = (HSSFFont)workbook.CreateFont();
            font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            font.Color = HSSFColor.Black.Index;
            font.FontHeightInPoints = 11;
            headerStyle.SetFont(font);
            return headerStyle;
        }
        private static ICellStyle GetCellStyle2(HSSFWorkbook workbook)
        {
            ICellStyle headerStyle = workbook.CreateCellStyle();

            //headerStyle.FillForegroundColor = IndexedColors.White.Index;
            //headerStyle.FillBackgroundColor = IndexedColors.Red.Index;
            //headerStyle.FillPattern = FillPattern.SolidForeground;

            HSSFFont font = (HSSFFont)workbook.CreateFont();
            // font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            font.Color = HSSFColor.Red.Index;
            font.FontHeightInPoints = 11;
            headerStyle.SetFont(font);
            return headerStyle;
        }
        private static ICellStyle GetCellStyle(HSSFWorkbook workbook)
        {
            ICellStyle headerStyle = workbook.CreateCellStyle();
            //headerStyle.FillForegroundColor = HSSFColor.White.Index;
            //headerStyle.FillBackgroundColor = HSSFColor.DarkRed.Index;
            //headerStyle.FillPattern = FillPattern.SolidForeground;

            var font = workbook.CreateFont();
            font.Boldweight = 10;
            font.Color = HSSFColor.DarkBlue.Index;
            font.FontHeightInPoints = 11;
            headerStyle.SetFont(font);
            return headerStyle;
        }

        private static void ExportDataTableToSheet(DataTable dt, ISheet sheet, ICellStyle headerStyle, ICellStyle headerStyle2, ICellStyle headerStyle3)
        {


            var row1 = sheet.CreateRow(0);
            //Puts in headers (these are table row headers, omit if you
            //just need a straight data dump
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                var cell = row1.CreateCell(j);
                String columnName = dt.Columns[j].ToString();
                cell.SetCellValue(columnName);
                cell.CellStyle = headerStyle3;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    var cell = row.CreateCell(j);
                    cell.CellStyle = headerStyle;
                    //if (i%2 == 0)
                    //{
                    //    cell.CellStyle = headerStyle;
                    //}
                    //else
                    //{
                    //    cell.CellStyle = headerStyle2;
                    //}
                    String columnName = dt.Columns[j].ToString();
                    cell.SetCellValue(dt.Rows[i][columnName].ToString());
                }
            }
        }
        public static byte[] GetExcelByteArrayFromDataTable(DataTable dt)
        {
            var dtList = new List<DataTable>();
            dtList.Add(dt);
            var ms = GetExcelByteArrayFromDataTable(dtList);
            return ms.ToArray();
        }
        public static byte[] GetExcelByteArrayFromDataTable(List<DataTable> dtList)
        {

            var ms = GetExcelMemoryFromDataTable(dtList);
            return ms.ToArray();
        }
        public static MemoryStream GetExcelMemoryFromDataTable(List<DataTable> dtList)
        {
            var workbook = CreateWorkBook(dtList);
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Seek(0, 0);

            return ms;
        }

      
        public static HSSFWorkbook GetExcel1()
        {
            var workbook = new HSSFWorkbook();
            var ExampleSheet = workbook.CreateSheet("Example Sheet");
            var rowIndex = 0;
            var row = ExampleSheet.CreateRow(rowIndex);
            row.CreateCell(0).SetCellValue("Example in first cell(0,0)");
            rowIndex++;
            return workbook;
        }
        public static MemoryStream GetExcelMemory()
        {
            MemoryStream ms = new MemoryStream();
            var templateWorkbook = GetExcel();
            templateWorkbook.Write(ms);

            return ms;
        }
        public static HSSFWorkbook GetExcel()
        {
            HSSFWorkbook workbook = new HSSFWorkbook();

            ISheet sheet = workbook.CreateSheet("Sheet1");
            //increase the width of Column A
            sheet.SetColumnWidth(0, 5000);
            //create the format instance
            IDataFormat format = workbook.CreateDataFormat();

            // Create a row and put some cells in it. Rows are 0 based.
            ICell cell = sheet.CreateRow(0).CreateCell(0);
            //number format with 2 digits after the decimal point - "1.20"
            SetValueAndFormat(workbook, cell, 1.2, HSSFDataFormat.GetBuiltinFormat("0.00"));

            //RMB currency format with comma    -   "¥20,000"
            ICell cell2 = sheet.CreateRow(1).CreateCell(0);
            SetValueAndFormat(workbook, cell2, 20000, format.GetFormat("¥#,##0"));

            //scentific number format   -   "3.15E+00"
            ICell cell3 = sheet.CreateRow(2).CreateCell(0);
            SetValueAndFormat(workbook, cell3, 3.151234, format.GetFormat("0.00E+00"));

            //percent format, 2 digits after the decimal point    -  "99.33%"
            ICell cell4 = sheet.CreateRow(3).CreateCell(0);
            SetValueAndFormat(workbook, cell4, 0.99333, format.GetFormat("0.00%"));

            //phone number format - "021-65881234"
            ICell cell5 = sheet.CreateRow(4).CreateCell(0);
            SetValueAndFormat(workbook, cell5, 02165881234, format.GetFormat("000-00000000"));

            //Chinese capitalized character number - 壹贰叁 元
            ICell cell6 = sheet.CreateRow(5).CreateCell(0);
            SetValueAndFormat(workbook, cell6, 123, format.GetFormat("[DbNum2][$-804]0 元"));

            //Chinese date string
            ICell cell7 = sheet.CreateRow(6).CreateCell(0);
            SetValueAndFormat(workbook, cell7, new DateTime(2004, 5, 6), format.GetFormat("yyyy年m月d日"));
            cell7.SetCellValue(new DateTime(2004, 5, 6));

            //Chinese date string
            ICell cell8 = sheet.CreateRow(7).CreateCell(0);
            SetValueAndFormat(workbook, cell8, new DateTime(2005, 11, 6), format.GetFormat("yyyy年m月d日"));

            //formula value with datetime style 
            ICell cell9 = sheet.CreateRow(8).CreateCell(0);
            cell9.CellFormula = "DateValue(\"2005-11-11\")+TIMEVALUE(\"11:11:11\")";
            ICellStyle cellStyle9 = workbook.CreateCellStyle();
            cellStyle9.DataFormat = HSSFDataFormat.GetBuiltinFormat("m/d/yy h:mm");
            cell9.CellStyle = cellStyle9;

            //display current time
            ICell cell10 = sheet.CreateRow(9).CreateCell(0);
            SetValueAndFormat(workbook, cell10, DateTime.Now, format.GetFormat("[$-409]h:mm:ss AM/PM;@"));



            ISheet sheet2 = workbook.CreateSheet("Sheet2");
            //increase the width of Column A
            sheet2.SetColumnWidth(0, 5000);

            // Create a row and put some cells in it. Rows are 0 based.
            ICell cell21 = sheet2.CreateRow(0).CreateCell(0);
            //number format with 2 digits after the decimal point - "1.20"
            SetValueAndFormat(workbook, cell21, 1.2, HSSFDataFormat.GetBuiltinFormat("0.00"));


            return workbook;
        }
        static void SetValueAndFormat(IWorkbook workbook, ICell cell, int value, short formatId)
        {
            cell.SetCellValue(value);
            ICellStyle cellStyle = workbook.CreateCellStyle();
            cellStyle.DataFormat = formatId;
            cell.CellStyle = cellStyle;
        }

        static void SetValueAndFormat(IWorkbook workbook, ICell cell, double value, short formatId)
        {
            cell.SetCellValue(value);
            ICellStyle cellStyle = workbook.CreateCellStyle();
            cellStyle.DataFormat = formatId;
            cell.CellStyle = cellStyle;
        }
        static void SetValueAndFormat(IWorkbook workbook, ICell cell, DateTime value, short formatId)
        {
            //set value for the cell
            if (value != null)
                cell.SetCellValue(value);

            ICellStyle cellStyle = workbook.CreateCellStyle();
            cellStyle.DataFormat = formatId;
            cell.CellStyle = cellStyle;
        }

        public static DataTable PostValues(HttpPostedFileBase file)
        {
            ISheet sheet;
          //  string filename = Path.GetFileName(Server.MapPath(file.FileName));
            var fileExt = Path.GetExtension(file.FileName);
            if (fileExt == ".xls")
            {
                HSSFWorkbook hssfwb = new HSSFWorkbook(file.InputStream);
                sheet = hssfwb.GetSheetAt(0);
            }
            else
            {
                XSSFWorkbook hssfwb = new XSSFWorkbook(file.InputStream);
                sheet = hssfwb.GetSheetAt(0);
            }

            DataTable table = new DataTable();
            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }
            int rowCount = sheet.LastRowNum;
            for (int i = (sheet.FirstRowNum); i < sheet.LastRowNum + 1; i++)
            {
                try
                {
                    IRow row = sheet.GetRow(i);
                    if (row != null)
                    {
                        DataRow dataRow = table.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                            {
                                dataRow[j] = row.GetCell(j).ToString();
                            }
                        }
                        table.Rows.Add(dataRow);
                        
                    }

                }
                catch (Exception)
                {
                   
                }
              

            }
            return table;
        }
    }
}
