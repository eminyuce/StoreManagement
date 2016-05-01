using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
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


        public static void AddColumn<T>(DataTable dataTable, string columnName, bool nullable)
        {
            var col = dataTable.Columns.Add(columnName, typeof(T));
            col.AllowDBNull = nullable;
        }

        public static void AddColumn<T>(DataTable dataTable, string columnName)
        {
              AddColumn<T>(dataTable,columnName, true);
        }

        public static void AddRows<T>(DataTable dataTable, List<T> items, Func<T, object[]> mapper)
        {
            items.ForEach(x => dataTable.Rows.Add(mapper(x)));
        }
        /// <summary>
        /// Converts to.
        /// </summary>
        /// <returns>The to.</returns>
        /// <param name="list">List.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static DataTable ConvertTo<T>(IList<T> paramList)
        {
            DataTable table = CreateTable<T>();
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (T item in paramList)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }

                table.Rows.Add(row);
            }

            return table;
        }

        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <returns>The table.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static DataTable CreateTable<T>()
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            return table;
        }
        public static string[] GetFieldAsArray(DataTable dt, string fieldName)
        {
            List<string> data = new List<string>();

            int index = dt.Columns.IndexOf(fieldName);

            if (index < 0)
            {
                return new string[] { };
            }

            foreach (DataRow row in dt.Rows)
            {
                data.Add(row[index].ToString());
            }

            return data.ToArray();
        }
        /// <summary>
        /// DataTable extension for deleteing rows
        /// </summary>
        /// <param name="table">DataTable</param>
        /// <param name="filter">Filter expression</param>
        /// <returns>Altered DataTable</returns>
        public static DataTable Delete(DataTable table, string filter)
        {
            Delete(table.Select(filter));
            return table;
        }

        /// <summary>
        /// Delete function for DataTable
        /// </summary>
        /// <param name="rows">DataRows in DataTable</param>
        public static void Delete(IEnumerable<DataRow> rows)
        {
            foreach (var row in rows)
                row.Delete();
        }
        public static T[] ToArray<R, T>(DataTable table, Func<R, T> converter) where R : DataRow
        {
            if (table.Rows.Count == 0)
            {
                return new T[] { };
            }
            //Verify [DataContract] or [Serializable] on T
            Debug.Assert(IsDataContract(typeof(T)) || typeof(T).IsSerializable);

            //Verify table contains correct rows 
            Debug.Assert(MatchingTableRow<R>(table));

            return table.Rows.Cast<R>().Select(converter).ToArray();
        }
        static bool IsDataContract(Type type)
        {
            object[] attributes = type.GetCustomAttributes(typeof(DataContractAttribute), false);
            return attributes.Length == 1;
        }
        static bool MatchingTableRow<R>(DataTable table)
        {
            if (table.Rows.Count == 0)
            {
                return true;
            }
            return table.Rows[0] is R;
        }
    }
}
