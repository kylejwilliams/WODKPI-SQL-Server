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
        BackgroundWorker bw;
        CleanData cd;
        AccessDB aDB;

        public updateDataForm()
        {
            InitializeComponent();

            copyDataBackgroundWorker = new BackgroundWorker();
            copyDataBackgroundWorker.WorkerReportsProgress = true;
            copyDataBackgroundWorker.WorkerSupportsCancellation = true;
            copyDataBackgroundWorker.DoWork += copyDataBackgroundWorker_DoWork;
            copyDataBackgroundWorker.ProgressChanged += copyDataBackgroundWorker_ProgressChanged;
            copyDataBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(copyDataBackgroundWorker_RunWorkerCompleted);

            convertDataBackgroundWorker = new BackgroundWorker();
            convertDataBackgroundWorker.WorkerReportsProgress = true;
            convertDataBackgroundWorker.WorkerSupportsCancellation = true;
            convertDataBackgroundWorker.DoWork += convertDataBackgroundWorker_DoWork;
            convertDataBackgroundWorker.ProgressChanged += convertDataBackgroundWorker_ProgressChanged;
            convertDataBackgroundWorker.RunWorkerCompleted += convertDataBackgroundWorker_RunWorkerCompleted;

            cleanDataBackgroundWorker = new BackgroundWorker();
            cleanDataBackgroundWorker.WorkerReportsProgress = true;
            cleanDataBackgroundWorker.WorkerSupportsCancellation = true;
            cleanDataBackgroundWorker.DoWork += cleanDataBackgroundWorker_DoWork;
            cleanDataBackgroundWorker.ProgressChanged += cleanDataBackgroundWorker_ProgressChanged;
            cleanDataBackgroundWorker.RunWorkerCompleted += cleanDataBackgroundWorker_RunWorkerCompleted;

            updateDataWithoutOverwriteBackgroundWorker = new BackgroundWorker();
            updateDataWithoutOverwriteBackgroundWorker.WorkerReportsProgress = true;
            updateDataWithoutOverwriteBackgroundWorker.WorkerSupportsCancellation = true;
            updateDataWithoutOverwriteBackgroundWorker.DoWork += updateDataWithoutOverwriteBackgroundWorker_DoWork;
            updateDataWithoutOverwriteBackgroundWorker.ProgressChanged += updateDataWithoutOverwriteBackgroundWorker_ProgressChanged;
            updateDataWithoutOverwriteBackgroundWorker.RunWorkerCompleted += updateDataWithoutOverwriteBackgroundWorker_RunWorkerCompleted;

            uploadToNetworkDriveBackgroundWorker = new BackgroundWorker();
            uploadToNetworkDriveBackgroundWorker.WorkerReportsProgress = true;
            uploadToNetworkDriveBackgroundWorker.WorkerSupportsCancellation = true;
            uploadToNetworkDriveBackgroundWorker.DoWork += uploadToNetworkDriveBackgroundWorker_DoWork;
            uploadToNetworkDriveBackgroundWorker.ProgressChanged += uploadToNetworkDriveBackgroundWorker_ProgressChanged;
            uploadToNetworkDriveBackgroundWorker.RunWorkerCompleted += uploadToNetworkDriveBackgroundWorker_RunWorkerCompleted;

            uploadToAccessBackgroundWorker = new BackgroundWorker();
            uploadToAccessBackgroundWorker.WorkerReportsProgress = true;
            uploadToAccessBackgroundWorker.WorkerSupportsCancellation = true;
            uploadToAccessBackgroundWorker.DoWork += uploadToAccessBackgroundWorker_DoWork;
            uploadToAccessBackgroundWorker.ProgressChanged += uploadToAccessBackgroundWorker_ProgressChanged;
            uploadToAccessBackgroundWorker.RunWorkerCompleted += uploadToAccessBackgroundWorker_RunWorkerCompleted;

            cd = new CleanData();
            aDB = new AccessDB();
            bw = new BackgroundWorker(); // will hold a reference to the background worker that called each method to pass events
        }

        private void buttonConfirmCleanData_Click(object sender, EventArgs e)
        {
            cleanDataBackgroundWorker.RunWorkerAsync();
        }

        private void buttonCancelCleanData_Click(object sender, EventArgs e)
        {
            if (updateDataWithoutOverwriteBackgroundWorker.WorkerSupportsCancellation == true)
                updateDataWithoutOverwriteBackgroundWorker.CancelAsync();
            if (copyDataBackgroundWorker.WorkerSupportsCancellation == true)
                copyDataBackgroundWorker.CancelAsync();
            if (cleanDataBackgroundWorker.WorkerSupportsCancellation == true)
                cleanDataBackgroundWorker.CancelAsync();
            if (convertDataBackgroundWorker.WorkerSupportsCancellation == true)
                convertDataBackgroundWorker.CancelAsync();

            mainMessageLabel.Text = "Operation Aborted.";
        }

        private void convertDataButton_Click(object sender, EventArgs e)
        {
            convertDataBackgroundWorker.RunWorkerAsync();
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
            copyDataBackgroundWorker.RunWorkerAsync();
        }

        private void copyDataBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int totalOperations = 1;
            int currentOperation = 0;
            int overallPercentage = (int)((currentOperation * 100) / totalOperations);
            CopyDataOperation(sender, e, overallPercentage);
            currentOperation++;
            overallPercentage = (int)((currentOperation * 100) / totalOperations);
            copyDataBackgroundWorker.ReportProgress(overallPercentage);
        }

        private void copyDataBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                if (e.UserState != null)
                {
                    currentProgress.Value = e.ProgressPercentage;
                    overallProgress.Value = (int)e.UserState;
                }
                else // this takes care of the final overall percentage (can't be called from inside the operation after it's done and it needs to update one more time)
                {
                    overallProgress.Value = e.ProgressPercentage;
                }

                currentProgress.Refresh();
                overallProgress.Refresh();
            });
        }

        private void copyDataBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled == true)
            {
                MessageBox.Show("ERROR: You must select at least one data source to copy!");
                Invoke((MethodInvoker)delegate
                {
                    mainMessageLabel.Text = "File copy cancelled.";
                });
            }
            else
            {
                Invoke((MethodInvoker)delegate
                {
                    mainMessageLabel.Text = "File transfer complete!";
                });
            }
        }

        private void cleanDataBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int totalOperations = 1;
            int currentOperation = 0;
            int overallPercentage = (int)((currentOperation * 100) / totalOperations);
            CleanDataOperation(sender, e, overallPercentage);
            currentOperation++;
            overallPercentage = (int)((currentOperation * 100) / totalOperations);
            cleanDataBackgroundWorker.ReportProgress(overallPercentage);
        }

        private void cleanDataBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                if (e.UserState != null)
                {
                    currentProgress.Value = e.ProgressPercentage;
                    overallProgress.Value = (int)e.UserState;
                }
                else // this takes care of the final overall percentage (can't be called from inside the operation after it's done and it needs to update one more time)
                {
                    overallProgress.Value = e.ProgressPercentage;
                }

                currentProgress.Refresh();
                overallProgress.Refresh();
            });
        }

        private void cleanDataBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                mainMessageLabel.Text = "Error while cleaning.";
            }
            else if (e.Cancelled == true)
            {
                MessageBox.Show("ERROR: You must select at least one data source to clean!");
                mainMessageLabel.Text = "Cleaning cancelled.";
            }
            else
            {
                mainMessageLabel.Text = "Cleaning complete!";
            }
        }

        private void convertDataBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int totalOperations = 1;
            int currentOperation = 0;

            int overallPercentage = (int)((currentOperation * 100) / totalOperations);
            ConvertDataOperation(sender, e, overallPercentage);
            currentOperation++;
            overallPercentage = (int)((currentOperation * 100) / totalOperations);
            convertDataBackgroundWorker.ReportProgress(overallPercentage); // this lets us write out that overall percentage is at 100%
        }

        private void convertDataBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                if (e.UserState != null)
                {
                    currentProgress.Value = e.ProgressPercentage;
                    overallProgress.Value = (int)e.UserState;
                }
                else // this takes care of the final overall percentage (can't be called from inside the operation after it's done and it needs to update one more time)
                {
                    overallProgress.Value = e.ProgressPercentage;
                }

                currentProgress.Refresh();
                overallProgress.Refresh();
            });
        }

        private void convertDataBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                mainMessageLabel.Text = "Error during conversion.";
            }
            else if (e.Cancelled == true)
            {
                MessageBox.Show("ERROR: you must select at least one data source to convert!");
                mainMessageLabel.Text = "Conversion cancelled.";
            }
            else
            {
                mainMessageLabel.Text = "Conversion complete.";
            }
        }

        private void updateDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (copyDataBackgroundWorker.WorkerSupportsCancellation == true)
                copyDataBackgroundWorker.CancelAsync();
            if (cleanDataBackgroundWorker.WorkerSupportsCancellation == true)
                cleanDataBackgroundWorker.CancelAsync();
            if (convertDataBackgroundWorker.WorkerSupportsCancellation == true)
                convertDataBackgroundWorker.CancelAsync();

            Owner.Show();
        }

        private void updateNewEntriesButton_Click(object sender, EventArgs e)
        {
            updateDataWithoutOverwriteBackgroundWorker.RunWorkerAsync();
        }

        private void CopyDataOperation(object sender, DoWorkEventArgs e, object overallPercentage)
        {
            string srcPath = @"N:\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\Corporate KPI Database\Ranker Text Files\";
            string dstPath = @"C:\Users\y712969\Desktop\Ranker Text Files\";
            bool overwrite = false;
            string ranker;
            List<string> foldersToCopy = new List<string>();
            List<string> nonRankersToCopy = new List<string>();
            string[] invalidRankers = { "Credit360", "Dayforce", "Roadnet" };

            // we're starting at 1 because we don't want to include the "check all" box
            for (int i = 1; i < rankerSelectorCheckedListBox.Items.Count; i++)
            {
                if (rankerSelectorCheckedListBox.GetItemCheckState(i) == CheckState.Checked)
                {
                    string tmp = rankerSelectorCheckedListBox.Items[i].ToString();

                    if (invalidRankers.Any(tmp.Contains))
                    {
                        if (tmp == "Credit360" || tmp == "Dayforce")
                            nonRankersToCopy.Add(@"N:\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\Corporate KPI Database\Imports\");
                        else if (tmp == "Roadnet")
                        {
                            nonRankersToCopy.Add(@"N:\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\Corporate KPI Database\Roadnet Reports Pulled by Omnitracs\");
                        }
                    }
                    else
                    {
                        // Example: "1CE - Volume"; split it at the dash and take the first half of the string to get the ranker name
                        string rankerName = tmp.Split('-')[0].Trim();
                        foldersToCopy.Add(rankerName);
                    }
                }
            }

            if (!foldersToCopy.Any() && !nonRankersToCopy.Any())
            {
                e.Cancel = true;
                return;
            }

            Invoke((MethodInvoker)delegate
            {
                mainMessageLabel.Text = "Transferring files...";
                overallProgress.Minimum = 0;
                overallProgress.Maximum = 100;
            });

            int fileCount = 0; // used to total up the number of files processed for the progress bar
            int totalFiles = nonRankersToCopy.Count + foldersToCopy.Count;
            for (int i = 0; i < nonRankersToCopy.Count; i++)
            {
                fileCount++;
                string filename = "";
                ranker = nonRankersToCopy[i];
                if (ranker.Contains("Imports"))
                    filename = "Credit360 and Dayforce";
                else if (ranker.Contains("Roadnet"))
                    filename = "Roadnet";

                Invoke((MethodInvoker)delegate
                {
                    mainMessageLabel.Text = "copying " + filename + "...";
                    mainMessageLabel.Refresh();
                });
                cd.CopyDirectory(ranker, @"C:\Users\y712969\Desktop\Ranker Text Files\CLEAN\" + filename, overwrite);
                bw = (BackgroundWorker)sender;
                bw.ReportProgress(fileCount * 100 / totalFiles, overallPercentage);
            }
            for (int i = 0; i < foldersToCopy.Count; i++)
            {
                fileCount++;
                ranker = foldersToCopy[i];
                Invoke((MethodInvoker)delegate
                {
                    mainMessageLabel.Text = "copying " + ranker + "...";
                    mainMessageLabel.Refresh();
                });
                cd.CopyDirectory(srcPath + ranker, dstPath + ranker, overwrite);
                BackgroundWorker bw = (BackgroundWorker)sender;
                bw.ReportProgress(fileCount * 100 / totalFiles, overallPercentage);
            }
        }

        private void CleanDataOperation(object sender, DoWorkEventArgs e, int overallPercentage)
        {
            string ranker;
            List<string> rankersToClean = new List<string>();
            string[] invalidRankers = { "Credit360", "Dayforce", "Roadnet" };

            // we're starting at 1 because we don't want to include the "check all" box
            for (int i = 1; i < rankerSelectorCheckedListBox.Items.Count; i++)
            {
                if (invalidRankers.Any(rankerSelectorCheckedListBox.Items[i].ToString().Contains))
                    continue;

                if (rankerSelectorCheckedListBox.GetItemCheckState(i) == CheckState.Checked)
                {
                    // Example: "1CE - Volume"; split it at the dash and take the first half of the string to get the ranker name
                    string rankerName = rankerSelectorCheckedListBox.Items[i].ToString().Split('-')[0].Trim();
                    rankersToClean.Add(rankerName);
                }
            }

            if (!rankersToClean.Any())
            {
                e.Cancel = true;
                return;
            }

            Invoke((MethodInvoker)delegate
            {
                overallProgress.Minimum = 0;
                overallProgress.Maximum = 100;
            });

            for (int i = 0; i < rankersToClean.Count; i++)
            {
                ranker = rankersToClean[i];
                Invoke((MethodInvoker)delegate
                {
                    mainMessageLabel.Text = "Cleaning " + ranker + "...";
                    mainMessageLabel.Refresh();
                });
                cd.InitClean(ranker);
                bw = (BackgroundWorker)sender;
                bw.ReportProgress((i + 1) * 100 / rankersToClean.Count, overallPercentage);
            }
        }

        private void ConvertDataOperation(object sender, DoWorkEventArgs e, int overallPercentage)
        {
            string dstPath;
            string srcPath = @"C:\Users\y712969\Desktop\Ranker Text Files\CLEAN\";
            List<string> srcPaths = new List<string>();
            string[] invalidRankers = { "Credit360", "Roadnet", "Dayforce" };
            overallProgress.Minimum = 0;
            overallProgress.Maximum = 100;

            // we're starting at 1 because we don't want to include the "check all" box
            for (int i = 1; i < rankerSelectorCheckedListBox.Items.Count; i++)
            {
                if (invalidRankers.Any(rankerSelectorCheckedListBox.Items[i].ToString().Contains))
                    continue;

                if (rankerSelectorCheckedListBox.GetItemCheckState(i) == CheckState.Checked)
                {
                    // Example: "1CE - Volume"; split it at the dash and take the first half of the string to get the ranker name
                    string rankerName = rankerSelectorCheckedListBox.Items[i].ToString().Split('-')[0].Trim() + "_clean.txt";
                    srcPaths.Add(srcPath + rankerName);
                }
            }

            if (!srcPaths.Any())
            {
                e.Cancel = true;
                return;
            }

            for (int i = 0; i < srcPaths.Count; i++)
            {
                srcPath = srcPaths[i];
                dstPath = srcPath.Replace(".txt", ".csv");
                string[] sections = srcPath.Split('\\');

                Invoke((MethodInvoker)delegate
                {
                    mainMessageLabel.Text = "Converting " + sections[sections.Length - 1] + " to csv...";
                });

                cd.ConvertToCSV(srcPath, dstPath);
                bw = (BackgroundWorker)sender;
                bw.ReportProgress((i + 1) * 100 / srcPaths.Count, overallPercentage);
            }
        }

        private void UploadToNetworkDriveOperation(object sender, DoWorkEventArgs e, int overallPercentage)
        {
            string srcPath = @"C:\Users\y712969\Desktop\Ranker Text Files\CLEAN";
            string dstPath = @"N:\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\WODKPI\Clean CSVs";
            string fileName;
            string[] csvFiles = { };
            System.IO.FileInfo oldFileInfo;
            System.IO.FileInfo newFileInfo;

            if (!System.IO.Directory.Exists(srcPath))
            {
                e.Cancel = true;
                return;
            }

            csvFiles = System.IO.Directory.GetFiles(srcPath, "*.csv");
            for (int i = 0; i < csvFiles.Length; i++)
            {
                fileName = System.IO.Path.GetFileName(csvFiles[i]);
                string dstFile = System.IO.Path.Combine(dstPath, fileName);

                oldFileInfo = new System.IO.FileInfo(dstFile);
                newFileInfo = new System.IO.FileInfo(csvFiles[i]);

                if (oldFileInfo.Exists)
                {
                    if (newFileInfo.LastWriteTime > oldFileInfo.LastWriteTime)
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            mainMessageLabel.Text = "Copying " + fileName + " to the network drive...";
                            mainMessageLabel.Refresh();
                        });
                        System.IO.File.Copy(csvFiles[i], dstFile, true);
                    }
                }
                else
                {
                    Invoke((MethodInvoker)delegate
                    {
                        mainMessageLabel.Text = "Copying " + fileName + " to the network drive...";
                        mainMessageLabel.Refresh();
                    });
                    System.IO.File.Copy(csvFiles[i], dstFile, true);
                }
                bw = (BackgroundWorker)sender;
                bw.ReportProgress((i + 1) * 100 / csvFiles.Length, overallPercentage);
            }
        }

        private void updateDataWithoutOverwriteBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int totalOperations = 3;
            int currentOperation;
            int overallPercentage;

            currentOperation = 0;
            overallPercentage = (int)((currentOperation * 100) / totalOperations);
            CopyDataOperation(sender, e, overallPercentage);

            currentOperation++;
            overallPercentage = (int)((currentOperation * 100) / totalOperations);
            CleanDataOperation(sender, e, overallPercentage);

            currentOperation++;
            overallPercentage = (int)((currentOperation * 100) / totalOperations);
            ConvertDataOperation(sender, e, overallPercentage);

            currentOperation++;
            overallPercentage = (int)((currentOperation * 100) / totalOperations);
            updateDataWithoutOverwriteBackgroundWorker.ReportProgress(overallPercentage);
        }

        private void updateDataWithoutOverwriteBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                if (e.UserState != null)
                {
                    currentProgress.Value = e.ProgressPercentage;
                    overallProgress.Value = (int)e.UserState;
                }
                else // this takes care of the final overall percentage (can't be called from inside the operation after it's done and it needs to update one more time)
                {
                    overallProgress.Value = e.ProgressPercentage;
                }

                currentProgress.Refresh();
                overallProgress.Refresh();
            });
        }

        private void updateDataWithoutOverwriteBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                mainMessageLabel.Text = "Error during update! Error message: " + e.Error.ToString();
            }
            else if (e.Cancelled == true)
            {
                MessageBox.Show("ERROR: you must select at least one data source to update!");
                mainMessageLabel.Text = "Update cancelled.";
            }
            else
            {
                mainMessageLabel.Text = "Update complete.";
                mainMessageLabel.Refresh();
            }
        }

        private void uploadToNetworkDriveBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int totalOperations = 1;
            int currentOperation = 0;

            int overallPercentage = (int)((currentOperation * 100) / totalOperations);
            UploadToNetworkDriveOperation(sender, e, overallPercentage);
            currentOperation++;
            overallPercentage = (int)((currentOperation * 100) / totalOperations);
            uploadToNetworkDriveBackgroundWorker.ReportProgress(overallPercentage); // this lets us write out that overall percentage is at 100%
        }

        private void uploadToNetworkDriveBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                if (e.UserState != null)
                {
                    currentProgress.Value = e.ProgressPercentage;
                    overallProgress.Value = (int)e.UserState;
                }
                else // this takes care of the final overall percentage (can't be called from inside the operation after it's done and it needs to update one more time)
                {
                    overallProgress.Value = e.ProgressPercentage;
                }

                currentProgress.Refresh();
                overallProgress.Refresh();
            });
        }

        private void uploadToNetworkDriveBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                
            }
            else if (e.Cancelled == true)
            {
                mainMessageLabel.Text = "Upload Failed.";
                mainMessageLabel.Refresh();
                MessageBox.Show("ERROR: Source directory does not exist");
            }
            else
            {
                mainMessageLabel.Text = "Upload Complete!";
                mainMessageLabel.Refresh();
            }
        }

        private void uploadToNetworkDriveButton_Click(object sender, EventArgs e)
        {
            uploadToNetworkDriveBackgroundWorker.RunWorkerAsync();
        }

        private void uploadToAccessBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //aDB.accessTestConnection(sender, e);
            aDB.CsvFileToDataTable(sender, e);
        }

        private void uploadToAccessBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void uploadToAccessBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void uploadToAccessButton_Click(object sender, EventArgs e)
        {
            uploadToAccessBackgroundWorker.RunWorkerAsync();
        }

        private void closeWindowButton_Click(object sender, EventArgs e)
        {
            if (updateDataWithoutOverwriteBackgroundWorker.WorkerSupportsCancellation == true)
                updateDataWithoutOverwriteBackgroundWorker.CancelAsync();
            if (copyDataBackgroundWorker.WorkerSupportsCancellation == true)
                copyDataBackgroundWorker.CancelAsync();
            if (cleanDataBackgroundWorker.WorkerSupportsCancellation == true)
                cleanDataBackgroundWorker.CancelAsync();
            if (convertDataBackgroundWorker.WorkerSupportsCancellation == true)
                convertDataBackgroundWorker.CancelAsync();
            Close();
        }
    }
}
 