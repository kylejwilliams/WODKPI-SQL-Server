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
        public Form_WODKPI()
        {
            InitializeComponent();
        }

        private void Connect_To_WODKPI_Click(object sender, EventArgs e)
        {
            string connectionString = null;
            SqlConnection cnn;

            connectionString = "Data Source=ABCSTLT6602;Initial Catalog=WODKPI;Integrated Security=True";
            cnn = new SqlConnection(connectionString);

            try
            {
                cnn.Open();
                MessageBox.Show("Connection open!");
                cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection! Reason: {0}", ex.ToString());
            }
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
            Close();
        }
    }
}
