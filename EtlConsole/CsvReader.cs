using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace ConsoleApp
{
    public class CsvReader
    {
        static void Main(string[] args)
        {
            //const string filePath = @"C:\Users\gchan\Documents\DBIO2\Performance\dumpfile.csv";

            // TODO: check if dataset name already exists before processing

            if (args == null || args.Length == 0)
            {
                Console.WriteLine("The filename and client must be supplied");
            }
            else if (args.Length == 1)
            {
                Console.WriteLine("The client must be supplied");
            }
            else if (!IsCsvFile(args[0]))
            {
                Console.WriteLine($"The file supplied '{args[0]}' is not a valid CSV");
            }
            else
            {
                Console.WriteLine($"Starting reading file '{args[0]}' for Mine Site '{args[1]}'...");

                using (var csv = new LumenWorks.Framework.IO.Csv.CsvReader(new StreamReader(args[0]), true))
                {
                    //string[] headers = csv.GetFieldHeaders();
                    string[] headers = { "EventName", "Type", "EventId", "Version", "Channel", "Level" };

                    using (var table = new DataTable())
                    {
                        table.Columns.Add(headers[0], typeof(string));
                        table.Columns.Add(headers[1], typeof(string));
                        table.Columns.Add(headers[2], typeof(int));
                        // todo: add remaining here...
                        table.Columns.Add("MineSite", typeof(string));

                        while (csv.ReadNextRecord())
                        {
                            table.Rows.Add(csv[0], csv[1], csv[2], args[1]);
                        }

                        BulkInsertDataTable("etw.DispatchEvent", table);

                        Console.WriteLine($"Finished importing {table.Rows.Count} rows for file '{args[0]}'.");
                    }
                }
            }

        }

        private static bool IsCsvFile(string filename)
        {

            // todo: do we want more robust validation or just check filename extension good enough?
            string ext = Path.GetExtension(filename);

            if (ext == null)
                return false;

            return ext.ToLower().Contains("csv");

            //using (var parser = new TextFieldParser(filename))
            //{
            //    parser.TextFieldType = FieldType.Delimited;
            //    parser.SetDelimiters(",");

            //    string[] line;
            //    while (!parser.EndOfData)
            //    {
            //        try
            //        {
            //            line = parser.ReadFields();
            //        }
            //        catch (MalformedLineException ex)
            //        {
            //            // log ex.Message
            //        }
            //    }
            //}
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

                        // todo: specify batch size??
                        bulkCopy.ColumnMappings.Add("EventName", "EventName");
                        bulkCopy.ColumnMappings.Add("Type", "Type");
                        bulkCopy.ColumnMappings.Add("EventId", "EventId");
                        bulkCopy.ColumnMappings.Add("MineSite", "MineSite");

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
