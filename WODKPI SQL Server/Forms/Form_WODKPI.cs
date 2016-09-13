using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WODKPI_SQL_Server
{
    public partial class Form_WODKPI : Form
    {
        updateDataForm udf;
        SqlDB sqlDB;

        public Form_WODKPI()
        {
            InitializeComponent();
            sqlDB = new SqlDB();

            connectToWODKPIBackgroundWorker = new BackgroundWorker();
            connectToWODKPIBackgroundWorker.WorkerReportsProgress = false;
            connectToWODKPIBackgroundWorker.WorkerSupportsCancellation = true;
            connectToWODKPIBackgroundWorker.DoWork += connectToWODKPIBackgroundWorker_DoWork;
            connectToWODKPIBackgroundWorker.RunWorkerCompleted += connectToWODKPIBackgroundWorker_RunWorkerCompleted;

            cancelWODKPIBackgroundWorker = new BackgroundWorker();
            cancelWODKPIBackgroundWorker.WorkerReportsProgress = false;
            cancelWODKPIBackgroundWorker.WorkerSupportsCancellation = true;
            cancelWODKPIBackgroundWorker.DoWork += cancelWODKPIBackgroundWorker_DoWork;
            cancelWODKPIBackgroundWorker.RunWorkerCompleted += cancelWODKPIBackgroundWorker_RunWorkerCompleted;
        }

        private void Connect_To_WODKPI_Click(object sender, EventArgs e)
        {
            connectToWODKPIBackgroundWorker.RunWorkerAsync();
            
        }

        private void Clean_Data_Click(object sender, EventArgs e)
        {
            Hide();
            udf = new updateDataForm();
            udf.Show(this);
            udf = null;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            cancelWODKPIBackgroundWorker.RunWorkerAsync();
        }

        private void connectToWODKPIBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            connectToWODKPIOperation();
        }

        private void connectToWODKPIBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Job Completed!");
        }

        private void cancelWODKPIBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (connectToWODKPIBackgroundWorker.WorkerSupportsCancellation == true)
                connectToWODKPIBackgroundWorker.CancelAsync();
            Close();
        }

        private void cancelWODKPIBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void connectToWODKPIOperation()
        {
            try
            {
                sqlDB.importR81ToTemp();
                MessageBox.Show("Command executed successfully!");
                //cnn.Open();
                //MessageBox.Show("Connection open!");
                //cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not execute command! Reason: {0}", ex.ToString());
                return;
            }
        }
    }
}
