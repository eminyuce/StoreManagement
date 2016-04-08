using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.GeneralHelper
{
    public class DataTableHelper
    {
        public static DataTable AddPercentColums(DataTable dt, String columnName, String percentageColumnName)
        {
            DataColumnCollection columns = dt.Columns;

            if (!columns.Contains(columnName))
            {
                return dt;
            }

            List<int> list = dt.AsEnumerable().Select(r => r.Field<int>(columnName)).ToList();
            double totalValue = list.Select(r => r.ToDouble()).Sum();
            dt.Columns.Add(new DataColumn(percentageColumnName, typeof(String)));
            int i = 0;
            foreach (var s in list)
            {
                dt.Rows[i++][percentageColumnName] = (s.ToDouble() / totalValue).ToString("P2", new NumberFormatInfo { PercentPositivePattern = 1, PercentNegativePattern = 1 });
            }
            return dt;
        }
        public static void ToPrintConsole(DataTable dataTable)
        {
            // Print top line
            Console.WriteLine(new string('-', 75));

            // Print col headers
            var colHeaders = dataTable.Columns.Cast<DataColumn>().Select(arg => arg.ColumnName);
            foreach (String s in colHeaders)
            {
                Console.Write("| {0,-20}", s);
            }
            Console.WriteLine();

            // Print line below col headers
            Console.WriteLine(new string('-', 75));

            // Print rows
            foreach (DataRow row in dataTable.Rows)
            {
                foreach (Object o in row.ItemArray)
                {
                    Console.Write("| {0,-20}", o.ToString());
                }
                Console.WriteLine();
            }

            // Print bottom line
            Console.WriteLine(new string('-', 75));
        }
    }
}
