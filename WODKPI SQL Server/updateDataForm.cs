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

        // TODO Implement error handling for escaping clean on close
        // http://stackoverflow.com/questions/324831/breaking-out-of-a-nested-loop
        private void CleanDataLoop()
        {
            try
            {
                CleanData cd = new CleanData(this.progressCurrentRanker, this);
                List<string> rankersToClean = new List<string>();

                // we're starting at 1 because we don't want to include the "check all" box
                for (int i = 1; i < this.rankerSelectorCheckedListBox.Items.Count; i++)
                {
                    if (rankerSelectorCheckedListBox.GetItemCheckState(i) == CheckState.Checked)
                    {
                        string rankerName = rankerSelectorCheckedListBox.Items[i].ToString().Split('-')[0].Trim();
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
            catch (ThreadInterruptedException e)
            {
                MessageBox.Show("ERROR: Data update was ended prematurely!");
                return;
            }
        }

        private void CopyDataLoop()
        {
            CleanData cd = new CleanData();

            string srcPath = @"N:\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\Corporate KPI Database\Ranker Text Files";
            string dstPath = @"C:\Users\y712969\Desktop\Ranker Text Files";
            bool overwrite = false;

            this.Invoke((MethodInvoker) delegate
            {
                this.mainMessageLabel.Text = "Transferring files...";
            });

            int numCopiedFiles = cd.CopyDirectory(srcPath, dstPath, overwrite);

            this.Invoke((MethodInvoker) delegate {
                this.mainMessageLabel.Text = "File transfer complete! " + numCopiedFiles + " files transferred.";
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

        private void ConvertDataLoop()
        {
            CleanData cd = new CleanData();
            //cd.ConvertToCSV(@"C:\Users\y712969\")
            
        }

        private void convertDataButton_Click(object sender, EventArgs e)
        {

        }

        private void rankerSelectorCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.Index == 0)
            {
                for (int i = 0; i < rankerSelectorCheckedListBox.Items.Count; i++)
                {
                    // can't change the state for the caller of this event (prevents StackOverFlowException)
                    if (i != e.Index)
                        rankerSelectorCheckedListBox.SetItemCheckState(i, CheckState.Checked);
                }
            }
        }

        private void branchSelectorCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.Index == 0)
            {
                for (int i = 0; i < branchSelectorCheckedListBox.Items.Count; i++)
                {
                    // can't change the state for the caller of this event (prevents StackOverFlowException)
                    if (i != e.Index)
                        branchSelectorCheckedListBox.SetItemCheckState(i, CheckState.Checked);
                }
            }
        }

        private void copyDataButton_Click(object sender, EventArgs e)
        {
            thread = new Thread(new ThreadStart(CopyDataLoop));
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
 