
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace WODKPI_SQL_Server
{
    class CleanData
    {
        #region GLOBALS

        List<int> indexes_1CE = new List<int> { 0, 6,  13, 28, 48, 75, 91                                              };
        List<int> indexes_6PU = new List<int> { 0, 8,  15, 22, 43, 71                                                  };
        List<int> indexes_AN6 = new List<int> { 0, 9,  15, 21, 42, 55, 70                                              };
        List<int> indexes_OUT = new List<int> { 0, 6,  32, 38, 59, 84, 96, 103, 119, 124, 130, 144                     };
        List<int> indexes_R81 = new List<int> { 0, 6,  26, 32, 67, 74, 80,  97, 108, 119, 130, 134, 151, 177, 181, 192 };
        List<int> indexes_Z90 = new List<int> { 0, 6,  12, 34, 43, 46, 49,  60,  68,  75,  91, 102, 109, 121           };
        List<int> indexes_ZIA = new List<int> { 0, 12, 31, 57, 63, 68, 73,  91, 106, 114, 120                          };

        List<int> lengths_1CE = new List<int> { 6,  7, 15, 20, 28, 15, 15                                };
        List<int> lengths_6PU = new List<int> { 8,  7,  7, 21, 28, 12                                    };
        List<int> lengths_AN6 = new List<int> { 9,  6,  6, 22, 12, 15, 15                                };
        List<int> lengths_OUT = new List<int> { 6, 26, 27, 37, 17,  7,  5,  6, 15,  5                    };
        List<int> lengths_R81 = new List<int> { 6, 20,  6, 35,  7,  6, 17, 11, 11,  7, 15, 24,  4, 12, 5 };
        List<int> lengths_Z90 = new List<int> { 6,  6, 22,  9,  3,  3, 11,  8,  7, 17, 12,  7, 11        };
        List<int> lengths_ZIA = new List<int> { 6,  6, 19, 26,  6,  5,  5, 18, 29,  4                    };

        ProgressBar pb;
        Form mainForm;

        #endregion

        public CleanData()
        {

        }

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
            string srcPath = @"C:\Users\y712969\Desktop\Ranker Text Files\" + ranker;
            string dstPath = @"C:\Users\y712969\Desktop\Ranker Text Files\CLEAN\" + ranker + "_clean.txt";

            CleanFiles(srcPath, dstPath);
        }

        public void ConvertToCSV(string srcPath, string dstPath)
        {
            List<string> rankerFile = File.ReadAllLines(srcPath).ToList();
            List<string> fields = new List<string>();
            List<string> csvStrings = new List<string>();
            List<int> indexes;
            List<int> lengths;
            int numPadding = 0;
            string firstHeaderField = "";

            string ranker = GetFileName(srcPath);
            switch(ranker)
            {
                case "1CE_clean.txt":
                    indexes = indexes_1CE;
                    lengths = lengths_1CE;

                    break;
                case "6PU_clean.txt":
                    indexes = indexes_6PU;
                    lengths = lengths_6PU;
                    break;
                case "AN6_clean.txt":
                    indexes = indexes_AN6;
                    lengths = lengths_AN6;
                    break;
                case "OUT_clean.txt":
                    indexes = indexes_OUT;
                    lengths = lengths_OUT;
                    break;
                case "R81_clean.txt":
                    indexes = indexes_R81;
                    lengths = lengths_R81;
                    break;
                case "Z90_clean.txt":
                    indexes = indexes_Z90;
                    lengths = lengths_Z90;
                    break;
                case "ZIA_clean.txt":
                    indexes = indexes_ZIA;
                    lengths = lengths_ZIA;
                    firstHeaderField = "WHS#";
                    break;
                default:
                    throw new System.Exception("ERROR: srcPath is not a valid ranker!");
            }

            for (int i = 0; i < rankerFile.Count(); i++)
            {
                //if (rankerFile[i].StartsWith(firstHeaderField)) // the header isn't the same length as the other lines
                //    rankerFile[i] = new string(' ', numPadding); // add some spaces to get the line to match up with the rest
                //else if (rankerFile[i].Length != lengths.Sum() - 1) // -1 is for 0-based indexing purposes; checks if string is valid
                //    continue;                                       // (skips all rows not containing data)

                for (int j = 0; j < indexes.Count; j++)
                    fields.Add(rankerFile[i].Substring(indexes[j], lengths[j]).Trim());

                csvStrings.Add(System.String.Join(",", fields));
                fields.Clear();
            }

            File.WriteAllLines(dstPath, csvStrings);
        }

        public int CopyDirectory(string srcPath, string dstPath, bool overwriteFiles, int copiedFiles = 0)
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
                        file.CopyTo(dstFile.FullName, true);
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

            return copiedFiles;
        }
    }
}
