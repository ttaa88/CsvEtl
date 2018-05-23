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
            var csvReader = new CsvReader();

            using (LumenWorks.Framework.IO.Csv.CsvReader csv = new LumenWorks.Framework.IO.Csv.CsvReader(new StreamReader(filePath), true))
            {
                int fieldCount = csv.FieldCount;

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

                    csvReader.BulkInsertDataTable("etw.DispatchEvent", table);
                }
            }
        }

        public bool BulkInsertDataTable(string tableName, DataTable dataTable)
        {
            bool isSuccess;
            try
            {
                var sqlConnectionObj = GetSqlConnection();
                var bulkCopy = new SqlBulkCopy(sqlConnectionObj, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null);
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.WriteToServer(dataTable);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        private static SqlConnection GetSqlConnection()
        {
            const string connectionStr = "Data Source=VANAT018; Integrated Security=true; Initial Catalog=ITCOperational;";

            var conn = new SqlConnection(connectionStr);
            conn.Open();

            return conn;
        }

    }
}
