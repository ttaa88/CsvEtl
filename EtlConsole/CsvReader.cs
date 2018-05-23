using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LumenWorks.Framework.IO.Csv;

namespace ConsoleApp
{
    public class CsvReader
    {
        static void Main(string[] args)
        {
            const string filePath = @"C:\Users\gchan\Documents\DBIO2\Performance\dumpfile.csv";

            using (LumenWorks.Framework.IO.Csv.CsvReader csv = new LumenWorks.Framework.IO.Csv.CsvReader(new StreamReader(filePath), true))
            {
                //string[] headers = csv.GetFieldHeaders();
                string[] headers = {"EventName", "Type", "EventId", "Version", "Channel", "Level"};
                

                // TODO: create db table from headers or append data if one already exists?

                using (var table = new DataTable())
                {
                    table.Columns.Add(headers[0], typeof(string));
                    table.Columns.Add(headers[1], typeof(string));
                    table.Columns.Add(headers[2], typeof(int));

                    while (csv.ReadNextRecord())
                    {
                        //for (int i = 0; i < fieldCount; i++)
                        //{
                        //    Console.Write($"{headers[i]} = {csv[i]};");
                        //}

                        //Console.WriteLine();

                        table.Rows.Add(csv[0], csv[1], csv[2]);
                    }

                    BulkInsertDataTable("etw.DispatchEvent", table);
                }
            }
        }

        public static void BulkInsertDataTable(string tableName, DataTable dataTable)
        {
            try
            {
                const string connectionStr = "Data Source=VANAT018; Integrated Security=true; Initial Catalog=ITCOperational;";
                using (var conn = new SqlConnection(connectionStr))
                {
                    conn.Open();

                    using (var bulkCopy = new SqlBulkCopy(conn))
                    {
                        bulkCopy.DestinationTableName = tableName;

                        // todo: batch size??
                        bulkCopy.ColumnMappings.Add("EventName", "EventName");
                        bulkCopy.ColumnMappings.Add("Type", "Type");
                        bulkCopy.ColumnMappings.Add("EventId", "EventId");

                        bulkCopy.WriteToServer(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
