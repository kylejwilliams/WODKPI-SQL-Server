using System;
using System.Windows.Forms;
using System.Data;
using GenericParsing;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;


namespace WODKPI_SQL_Server
{
    class SqlDB
    {
        #region QUERIES & CONSTS

        const string dataSource = @"C:\Users\y712969\Desktop\Ranker Text Files\CLEAN\R81_Clean.txt";
        const string connectionString = @"Data Source=ABCSTLT6602;Initial Catalog=WODKPI_Refusals;Integrated Security=True";
        int[] columnWidths = new int[17] { 6, 14, 11, 26, 17, 6, 20, 8, 11, 12, 6, 15, 24, 8, 8, 9, 21 };

        const string createTempRefusalsQuery =
            @"IF OBJECT_ID('TempRefusals') IS NOT NULL
	            DROP TABLE TempRefusals

            CREATE TABLE TempRefusals
            (
	            rep#		VARCHAR(6),
	            repName		VARCHAR(14),
	            cust#		VARCHAR(11),
	            custName	VARCHAR(26),
	            inv#		VARCHAR(17),
	            item#		VARCHAR(6),
	            itemDesc	VARCHAR(20),
	            itemQty		VARCHAR(8),
	            item$		VARCHAR(11),
	            orderDate	VARCHAR(12),
	            driver#		VARCHAR(6),
	            driverName	VARCHAR(15),
	            reason		VARCHAR(24),
	            load#		VARCHAR(8),
	            CE			VARCHAR(8),
	            PDCN		VARCHAR(9),
	            branch		VARCHAR(21)
            )";

        // http://stackoverflow.com/questions/650098/how-to-execute-an-sql-script-file-using-c-sharp

        const string createSalesRepsQuery =
            @"IF OBJECT_ID('SalesRepresentatives') IS NOT NULL
                DROP TABLE SalesRepresentatives

            CREATE TABLE SalesRepresentatives
            (
                salesRepNum     VARCHAR(6),
                salesRepName    VARCHAR(14),
            )
            CREATE UNIQUE INDEX salesRepPK
                ON SalesRepresentatives(salesRepNum)";

        string[] columnNames = new string[] 
        {
            "rep#",
            "repName",
            "cust#",
            "custName",
            "inv#",
            "item#",
            "itemDesc",
            "itemQty",
            "item$",
            "orderDate",
            "driver#",
            "driverName",
            "reason",
            "load#",
            "CE",
            "PDCN",
            "branch"
        };

        #endregion

        public void importR81ToTemp()
        {
            DataTable tempRefusalsDT;
            try
            {
                // lets make our data table so we can import it into SQL
                tempRefusalsDT = new DataTable();
                for (int i = 0; i < columnNames.Length; i++)
                    tempRefusalsDT.Columns.Add(columnNames[i]);

                using (GenericParser parser = new GenericParser())
                {
                    parser.SetDataSource(dataSource);
                    parser.ColumnWidths = columnWidths;
                    parser.MaxBufferSize = 4096;
                    parser.TrimResults = true;

                    while (parser.Read())
                    {
                        DataRow dr = tempRefusalsDT.NewRow();
                        for (int i = 0; i < parser.ColumnCount; i++)
                            dr[columnNames[i]] = parser[i];
                        tempRefusalsDT.Rows.Add(dr);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR! " + e.ToString());
                return;
            }


            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();

                using (SqlCommand createTempTableCmd = new SqlCommand(createTempRefusalsQuery, sourceConnection))
                {
                    // Attempt to create the table in case it doesn't already exist in the db
                    try
                    {
                        createTempTableCmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        System.Windows.Forms.MessageBox.Show("ERROR!: " + e.ToString());
                        return;
                    }
                }

                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(sourceConnection))
                {
                    sqlBulkCopy.DestinationTableName = "TempRefusals";

                    try
                    {
                        sqlBulkCopy.WriteToServer(tempRefusalsDT);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("ERROR! " + e.ToString());
                    }
                }
            }
        }

        public void executeSQLScript(string connString, string script)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
            }
            
        }
    }
}
