using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace WODKPI_SQL_Server
{
    class AccessDB
    {
        public void AccessTestConnection(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            string myConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                @"Data Source=C:\Users\y712969\Desktop\WODKPI SQL Server\WODKPI SQL Server\DBs\RefusalsTest.accdb;" +
                @"User ID=admin;" + 
                @"Jet OLEDB:Database Password=admin";

            OleDbConnection myConnection = new OleDbConnection();

            try
            {
                
                myConnection.ConnectionString = myConnectionString;
                myConnection.Open();
                MessageBox.Show("Connected to DB");
            }
            catch (Exception ex)
            {
                MessageBox.Show("OLEDB Connection FAILED: " + ex.Message);
                return;
            }

            try
            {
                // Execute Queries...
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = myConnection;
                cmd.CommandText = File.ReadAllText(@"..\..\SQL Queries\createR81.sql");
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                MessageBox.Show("createR81.sql executed successfully!");
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("ERROR: " + ex.Message);
            }



        }

        public void CsvFileToDataTable(
            object sender,
            System.ComponentModel.DoWorkEventArgs e,
            string path = @"C:\Users\y712969\Desktop\Ranker Text Files\CLEAN\R81_clean.csv")
        {
            string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                @"Data Source=C:\Users\y712969\Desktop\WODKPI SQL Server\WODKPI SQL Server\DBs\RefusalsTest.accdb;" +
                @"User ID=admin;" +
                @"Jet OLEDB:Database Password=admin";
            OleDbConnection con = new OleDbConnection(connString);
            OleDbCommand cmd = new OleDbCommand();
            int effectedRow = 0;
            int lineCounter = 0;

            con.Open();

            if ((con.State.ToString() == "Open"))
            {

                StreamReader stReader = new StreamReader(path);
                string[] strRowData = null;

                while (stReader.Peek() >= 0)
                {
                    lineCounter = lineCounter + 1;
                    strRowData = stReader.ReadLine().Split(',');

                    try
                    {
                        string query = "INSERT INTO [R81] VALUES ('";
                        for (int i = 0; i < strRowData.Length; i++)
                        {
                            if (i == 0)
                                query += strRowData[i];
                            else
                                query += "','" + strRowData[i];
                        }
                        query += "')";
                        cmd.CommandText = query;
                        cmd.Connection = con;
                        effectedRow = cmd.ExecuteNonQuery();
                        query = "";
                        if ((effectedRow == -1))
                            MessageBox.Show("Line: " + lineCounter + " Error");
                        //else
                            //MessageBox.Show("Line: " + lineCounter + " Executed Successfully");
                    }
                    catch (OleDbException er)
                    {
                        MessageBox.Show("Line: " + lineCounter + " Error: " + er.Message);
                    }
                }
                stReader.Close();
                con.Close();
            }
            else
            {
                MessageBox.Show("Not Connected To Database");
            }
        }
    }
}
