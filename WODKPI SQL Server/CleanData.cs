
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace WODKPI_SQL_Server
{
    class CleanData
    {
        ProgressBar pb;
        Form mainForm;

        public CleanData(ProgressBar pb, Form mainForm)
        {
            this.pb = pb;
            this.mainForm = mainForm;
        }

        private void CleanFiles(string srcPath, string dstPath)
        {
            HashSet<string> hs = new HashSet<string>();

            List<string> fileNames = Directory.GetFiles(srcPath).ToList();
            List<string> lines = new List<string>();

            mainForm.Invoke((MethodInvoker)delegate
            {
                // setup our progress bar
                pb.Minimum = 1;
                pb.Maximum = fileNames.Count();
                pb.Value = 1;
                pb.Step = 1;
            });

            foreach (string fileName in fileNames)
            {
                mainForm.Invoke((MethodInvoker)delegate
                {
                    pb.PerformStep();
                });
                
                lines = File.ReadAllLines(fileName).ToList<string>();
                foreach (string line in lines)
                {
                    string cleanLine = line.Trim();
                    if (CheckLine(cleanLine))
                        hs.Add(cleanLine);
                }
            }

            File.WriteAllLines(dstPath, hs);
        }

        private List<string> GetNewFiles(string oldFilesPath, List<string> evalFiles)
        {
            List<string> alreadyReadFiles = new List<string>();
            if (File.Exists(oldFilesPath))
                alreadyReadFiles = File.ReadAllLines(oldFilesPath).ToList();

            HashSet<string> newFiles = new HashSet<string>();
            HashSet<string> oldFiles = new HashSet<string>();
            oldFiles.UnionWith(alreadyReadFiles);

            foreach (string evalFile in evalFiles)
                if (!oldFiles.Contains(evalFile))
                    newFiles.Add(evalFile);

            return newFiles.ToList();
        }

        // TODO fix process for skipping the next line
        private bool CheckLine(string line)
        {
            bool skipNextLine = false;
            double d;

            // common keywords shared among headers (that we don't want included)
            string[] badLines =
            {
                    "ANHEUSER",
                    "COPYRIGHT",
                    "AUGUST",
                    "AB ",
                    "EUGENE",
                    "PAGE",
                    "RFM.#",
            };

            if (line.StartsWith("*") ||
                line.StartsWith("FINAL"))
            {
                skipNextLine = true;
            }

            return !(badLines.Any(line.StartsWith)) ||                                     // multiple
                    (line == "") ||                                                        // multiple
                    (line.EndsWith("-")) ||                                                // 6PU
                    (System.Text.RegularExpressions.Regex.IsMatch(line, @"^\d+$")) ||      // 6PU
                    (line.StartsWith("28")) ||                                             // 6PU
                    (line == "C14") ||                                                     // OUT
                    (skipNextLine) ||                                                      // 1CE (multiple?)
                    (line.StartsWith("=")) ||                                              // OUT
                    (System.Double.TryParse(line.Split(' ')[0], out d)) ||                 // OUT
                    (line.Split(' ').Length == 2);                                         // R81
        }

        private string GetFileName(string path)
        {
            string[] pathPieces = path.Split('\\');
            return pathPieces[pathPieces.Length - 1];
        }

        public void InitClean(string ranker, Form myForm)
        {
            string srcPath = @"N:\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\Corporate KPI Database\Ranker Text Files\" + ranker;
            string dstPath = @"C:\Users\y712969\Desktop\Data\" + ranker + "_clean.txt";

            CleanFiles(srcPath, dstPath);
        }

        public void ConvertToCSV(string srcPath, string dstPath, List<int> beginIndexes, List<int> lengths)
        {
            List<string> rankerFile = File.ReadAllLines(srcPath).ToList();
            List<string> fields = new List<string>();
            List<string> csvStrings = new List<string>();

            for (int i = 0; i < rankerFile.Count(); i++)
            {
                if (rankerFile[i].StartsWith("WH")) // the header isn't the same length as the other lines
                    rankerFile[i] += "      "; // add some spaces to get the line to match up with the rest
                else if (rankerFile[i].Length != lengths.Sum() - 1) // -1 is for 0-based indexing purposes; checks if string is valid
                    continue;                                       // (skips all rows not containing data)

                for (int j = 0; j < beginIndexes.Count; j++)
                    fields.Add(rankerFile[i].Substring(beginIndexes[j], lengths[j]).Trim());

                csvStrings.Add(System.String.Join(",", fields));
                fields.Clear();
            }

            File.WriteAllLines(dstPath, csvStrings);
        }

        private static void CopyDirectory(string srcPath, string dstPath, bool overwriteFiles, int copiedFiles = 0)
        {
            // Get the subdirectories the the specified directory
            DirectoryInfo dir = new DirectoryInfo(srcPath);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: " +
                    srcPath);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // if the destination directory doesn't exist, create it
            if (!Directory.Exists(dstPath))
                Directory.CreateDirectory(dstPath);

            // Get the files in the directory and copy them to the new location
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                FileInfo dstFile = new FileInfo(Path.Combine(dstPath, file.Name));
                if (dstFile.Exists)
                {
                    if (file.LastWriteTime > dstFile.LastWriteTime || overwriteFiles == true)
                    {
                        file.CopyTo(dstFile.FullName, overwriteFiles);
                        copiedFiles++;
                    }
                }
                else
                {
                    file.CopyTo(dstFile.FullName, overwriteFiles);
                    copiedFiles++;
                }
            }

            foreach (DirectoryInfo subDir in dirs)
            {
                string tmpPath = Path.Combine(dstPath, subDir.Name);
                CopyDirectory(subDir.FullName, tmpPath, overwriteFiles, copiedFiles);
            }

            System.Console.WriteLine("{0} files were copied from {1}", copiedFiles, dir);
        }
    }
}
