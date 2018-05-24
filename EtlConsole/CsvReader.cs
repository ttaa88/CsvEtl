using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace ConsoleApp
{
    public class CsvReader
    {
        private const string ConnectionStr = "Data Source=VANAT018; Integrated Security=true; Initial Catalog=ITCOperational;";

        static void Main(string[] args)
        {
            //const string filePath = @"C:\Users\gchan\Documents\DBIO2\Performance\dumpfile.csv";

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
                var csvFilename = args[0];
                var datasetName = Path.GetFileNameWithoutExtension(csvFilename);
                var mineSite = args[1];

                if (DatasetExists(datasetName))
                {
                    Console.WriteLine($"The dataset '{datasetName}' has already been imported!");
                }
                else
                {
                    Console.WriteLine($"Starting reading file '{csvFilename}' for Mine Site '{mineSite}'...");

                    using (var csv = new LumenWorks.Framework.IO.Csv.CsvReader(new StreamReader(csvFilename), true))
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
                            table.Columns.Add("DatasetName", typeof(string));

                            while (csv.ReadNextRecord())
                            {
                                table.Rows.Add(csv[0], csv[1], csv[2], mineSite, datasetName);
                            }

                            BulkInsertDataTable("etw.DispatchEvent", table);

                            Console.WriteLine($"Finished importing {table.Rows.Count} rows for file '{csvFilename}'.");
                        }
                    }
                }
            }

        }

        private static bool DatasetExists(string datasetName)
        {
            var queryString = "SELECT * FROM [etw].[DispatchEvent] WHERE DatasetName = @DatasetName";

            using (var connection = new SqlConnection(ConnectionStr))
            {
                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@DatasetName", datasetName);
                connection.Open();
                var reader = command.ExecuteReader();
                try
                {
                    return reader.HasRows;
                }
                finally
                {
                    // Always call Close when done reading.
                    reader.Close();
                }
            }
        }

        private static bool IsCsvFile(string filename)
        {

            // todo: do we want more robust validation or just check filename extension?
            string ext = Path.GetExtension(filename);

            if (ext == null)
                return false;

            return ext.ToLower().Contains("csv");
        }

        public static void BulkInsertDataTable(string tableName, DataTable dataTable)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionStr))
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
                        bulkCopy.ColumnMappings.Add("DatasetName", "DatasetName");

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
