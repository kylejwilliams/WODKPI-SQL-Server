using System;
using System.Data;
using GenericParsing;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace WODKPI_CMD
{
    class SQL
    {
        public void importRankerToTemp(
            string tempTableFilePath,
            string queryPath, 
            string dataSource, 
            string connStr, 
            string destTableName,
            int[] columnWidths, 
            string[] columnNames,
            string dropTableStr)
        {
            DataTable tempRankerDT;

            try
            {
                // lets make our data table so we can import it into SQL
                tempRankerDT = new DataTable();
                for (int i = 0; i < columnNames.Length; i++)
                    tempRankerDT.Columns.Add(columnNames[i]);

                using (GenericParser parser = new GenericParser())
                {
                    parser.SetDataSource(dataSource);
                    parser.ColumnWidths = columnWidths;
                    parser.MaxBufferSize = 4096;
                    parser.TrimResults = true;

                    while (parser.Read())
                    {
                        DataRow dr = tempRankerDT.NewRow();
                        for (int i = 0; i < parser.ColumnCount; i++)
                            dr[columnNames[i]] = parser[i];
                        tempRankerDT.Rows.Add(dr);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR! " + e.ToString());
                return;
            }

            using (SqlConnection sourceConnection = new SqlConnection(connStr))
            {
                sourceConnection.Open();

                executeSQLScript(dropTableStr, connStr);
                // make sure our TempRefusals table exists in the server
                executeSQLScript(tempTableFilePath, connStr);

                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(sourceConnection))
                {
                    sqlBulkCopy.DestinationTableName = destTableName;
                    sqlBulkCopy.BulkCopyTimeout = 600;
                    try
                    {
                        sqlBulkCopy.WriteToServer(tempRankerDT);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("ERROR! " + e.ToString());
                    }
                }
            }

            List<string> filenames = Directory.GetFiles(queryPath).ToList();

            // create all of the "real" tables
            foreach (string file in filenames)
                executeSQLScript(file, connStr);

        }

        public void executeSQLScript(string file, string connString)
        {
            string script = File.ReadAllText(file);
            IEnumerable<string> cmdStrings = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                foreach (string cmdString in cmdStrings)
                {
                    if (cmdString.Trim() != "")
                    {
                        using (SqlCommand uploadTempData = new SqlCommand(cmdString, conn))
                        {
                            try
                            {
                                uploadTempData.CommandTimeout = 600;
                                uploadTempData.ExecuteNonQuery();
                                Console.WriteLine("COMMAND SUCCESSFUL");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("ERROR! " + e.ToString());
                                Console.ReadKey();
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}
