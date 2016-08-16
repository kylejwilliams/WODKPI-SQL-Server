using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WODKPI_SQL_Server
{
    public partial class updateDataForm : Form
    {
        Thread thread = null;
        public updateDataForm()
        {
            InitializeComponent();
        }

        private void buttonConfirmCleanData_Click(object sender, EventArgs e)
        {
            thread = new Thread(new ThreadStart(CleanDataLoop));
            thread.IsBackground = true;
            thread.Start();
        }

        private void CleanDataLoop()
        {
            CleanData cd = new CleanData(this.progressCurrentRanker, this);
            List<string> rankersToClean = new List<string>();

            foreach (Control c in this.Controls)
            {
                if (c is CheckBox && ((CheckBox)c).Checked == true)
                {
                    string rankerName = c.Text.Split('-')[0].Trim();
                    rankersToClean.Add(rankerName);
                }
            }
            if (!rankersToClean.Any())
            {
                MessageBox.Show("WARNING: You must select at least one ranker to clean!");
                return;
            }

            this.Invoke((MethodInvoker)delegate
            {
                this.progressOverall.Minimum = 0;
                this.progressOverall.Maximum = rankersToClean.Count;
                this.progressOverall.Value = 0;
                this.progressOverall.Step = 1;
            });

            foreach (string ranker in rankersToClean)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    mainMessageLabel.Text = "Cleaning " + ranker + "...";
                    mainMessageLabel.Refresh();
                });

                cd.InitClean(ranker, this);

                this.Invoke((MethodInvoker)delegate
                {
                    this.progressOverall.PerformStep(); ;
                });
            }

            this.Invoke((MethodInvoker)delegate
            {
                mainMessageLabel.Text = "Cleaning complete!";
            });
        }

        private void buttonCancelCleanData_Click(object sender, EventArgs e)
        {
            if (thread != null && thread.IsAlive)
            {
                thread.Interrupt();
            }
            this.Owner.Show();
            this.Close();
            
        }
    }
}
