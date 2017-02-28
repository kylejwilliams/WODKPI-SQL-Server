using System;
using System.Data;
using GenericParsing;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Data.OleDb;

namespace WODKPI_CMD
{
    class SQL
    {
        public void importRankerToTemp(
            string tempTableFilePath,
            string queryPath,
            string tempTableName,
            string dataSource,
            string connStr,
            List<int> columnWidths,
            List<string> columnNames)
        {
            DataTable tempRankerDT;

            // check to see if we have any data we need to upload
            if (new FileInfo(dataSource).Length == 0) return;

            try
            {
                using (SqlConnection sourceConnection = new SqlConnection(connStr))
                {
                    sourceConnection.Open();
                    // make sure our temp table exists in the server
                    executeSQLScript(tempTableFilePath, sourceConnection);
                }

                // lets make our data table so we can import it into SQL
                tempRankerDT = new DataTable();
                for (int i = 0; i < columnNames.Count; i++)
                    tempRankerDT.Columns.Add(columnNames[i]);

                using (GenericParser parser = new GenericParser())
                {
                    parser.SetDataSource(dataSource);
                    parser.ColumnWidths = columnWidths.ToArray();
                    parser.TextFieldType = FieldType.FixedWidth;
                    parser.MaxBufferSize = 4096;
                    parser.TrimResults = true;

                    while (parser.Read())
                    {
                        DataRow dr = tempRankerDT.NewRow();
                        for (int i = 0; i < parser.ColumnCount; i++) dr[columnNames[i]] = parser[i];
                        if (!dr[columnNames.Count - 1].ToString().Any(char.IsDigit))
                            tempRankerDT.Rows.Add(dr);
                        if (tempRankerDT.Rows.Count > 100000) // insert data into sql in chunks of 100000 lines
                        {
                            //for (int i = 0; i < tempRankerDT.Columns.Count; i++) { Console.Write(tempRankerDT.Rows[0]); }
                            using (SqlConnection sourceConnection = new SqlConnection(connStr))
                            {
                                sourceConnection.Open();

                                // import our data into SQL
                                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(sourceConnection))
                                {
                                    sqlBulkCopy.DestinationTableName = tempTableName;
                                    sqlBulkCopy.BulkCopyTimeout = 0;
                                    try { sqlBulkCopy.WriteToServer(tempRankerDT); }
                                    catch (Exception e) { Console.WriteLine(e.ToString()); }
                                }
                            }
                            tempRankerDT.Clear(); // clear the table so we can start over
                        }
                    }
                    using (SqlConnection sourceConnection = new SqlConnection(connStr)) // push any data that's left over (<100000 lines)
                    {
                        sourceConnection.Open();

                        //import our data into SQL
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(sourceConnection))
                        {
                            sqlBulkCopy.DestinationTableName = tempTableName;
                            sqlBulkCopy.BulkCopyTimeout = 0;
                            try { sqlBulkCopy.WriteToServer(tempRankerDT); }
                            catch (Exception e) { Console.WriteLine(e.ToString()); }
                        }

                        // insert new data into the other tables
                        executeSQLScript(queryPath, sourceConnection);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return;
            }


        }

        public void executeSQLScript(string file, SqlConnection conn)
        {
            string script = File.ReadAllText(file);
            IEnumerable<string> cmdStrings = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            foreach (string cmdString in cmdStrings)
            {
                if (cmdString.Trim() != "")
                {
                    using (SqlCommand uploadTempData = new SqlCommand(cmdString, conn))
                    {
                        try
                        {
                            uploadTempData.CommandTimeout = 0;
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

        public void connectToAccess(string dbPath, string branch, string query)
        {
            query = File.ReadAllText(query);
            query = query.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ").Replace("BRANCHNAME", branch).Replace("&", "\"");
            string connStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+dbPath+@";Persist Security Info=False;";
            
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.CommandTimeout = 0;
                        cmd.ExecuteNonQuery();
                    }
                    catch (OleDbException e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            }
        }
    }
}
