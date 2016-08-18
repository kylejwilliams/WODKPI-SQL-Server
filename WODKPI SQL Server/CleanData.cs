
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;

namespace WODKPI_SQL_Server
{
    class CleanData
    {
        #region GLOBALS

        List<int> indexes_1CE = new List<int> { 0,  6, 13, 28, 49, 76, 91                                             };
        List<int> indexes_6PU = new List<int> { 0,  7, 14, 21, 42, 70                                                 };
        List<int> indexes_AN6 = new List<int> { 0,  8, 14, 20, 42, 54, 69, 84                                         };
        List<int> indexes_OUT = new List<int> { 0,  6, 32, 38, 59, 74, 84, 94, 103, 119, 125, 131, 146                };
        List<int> indexes_R81 = new List<int> { 0,  6, 26, 32, 67, 74, 80, 99, 108, 119, 131, 137, 152, 176, 181, 192 };
        List<int> indexes_Z90 = new List<int> { 0,  6, 12, 34, 43, 46, 49, 60,  68,  75,  92, 103, 110                };
        List<int> indexes_ZIA = new List<int> { 0,  6, 12, 33, 56, 63, 68, 76,  90, 106, 120                          };

        List<int> lengths_1CE = new List<int> { 6,  7, 15, 21, 27, 15, 15                                             };
        List<int> lengths_6PU = new List<int> { 7,  7,  7, 21, 28, 11                                                 };
        List<int> lengths_AN6 = new List<int> { 8,  6,  6, 22, 12, 15, 15,  7                                         };
        List<int> lengths_OUT = new List<int> { 6, 26,  6, 21, 15, 10, 10,  9,  16,   6,    6,  15,   5               };
        List<int> lengths_R81 = new List<int> { 6, 20,  6, 35,  7,  6, 19,  9,  11,  12,    6,  15,  24,  5,  11,   5 };
        List<int> lengths_Z90 = new List<int> { 6,  6, 22,  9,  3,  3, 11,  8,   7,  17,   11,   7,  11               };
        List<int> lengths_ZIA = new List<int> { 6,  6, 21, 23,  7,  5,  8, 14,  16,  14,    5                         };

        #endregion

        public CleanData()
        {

        }

        private void CleanFiles(string srcPath, string dstPath)
        {
            HashSet<string> hs = new HashSet<string>();

            List<string> fileNames = Directory.GetFiles(srcPath).ToList();
            List<string> lines = new List<string>();
            
            for (int i = 0; i < fileNames.Count; i++)
            {
                string fileName = fileNames[i];
                
                lines = File.ReadAllLines(fileName).ToList<string>();
                foreach (string line in lines)
                {
                    string cleanLine = line.Trim();
                    if (!IsBadLine(cleanLine))
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

        private bool IsBadLine(string line)
        {
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
                    "*",
                    "FINAL",
                    "28",
                    "=",
                    "EXT"
            };

            return  (badLines.Any(line.StartsWith)) ||                                     // multiple
                    (line == "") ||                                                        // multiple
                    (line.EndsWith("-")) ||                                                // 6PU
                    (System.Text.RegularExpressions.Regex.IsMatch(line, @"^\d+$")) ||      // 6PU
                    (line == "C14") ||                                                     // OUT
                    (System.Double.TryParse(line.Split(' ')[0], out d)) ||                 // OUT
                    (line.Split(' ').Length == 2);                                         // R81
        }

        private string GetFileName(string path)
        {
            string[] pathPieces = path.Split('\\');
            return pathPieces[pathPieces.Length - 1];
        }

        public void InitClean(string ranker)
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
                    break;
                default:
                    throw new System.Exception("ERROR: srcPath is not a valid ranker!");
            }

            // have to go backwards so we can concurrently remove elements from list
            for (int i = (rankerFile.Count - 1); i >= 0; i--)
            //for (int i = 0; i < rankerFile.Count; i++)
            {
                // need to ensure every line is long enough
                if (rankerFile[i].Length < lengths.Sum())
                {
                    numPadding = lengths.Sum() - rankerFile[i].Length;
                    rankerFile[i] = rankerFile[i] + new string(' ', numPadding);
                }

                for (int j = 0; j < indexes.Count; j++)
                {
                    try
                    {
                        string tmp = rankerFile[i].Substring(indexes[j], lengths[j]).Trim();
                        tmp = tmp.Replace(',', '^'); // need to avoid putting extra commas in our csv; '^' was chosen arbitrarily (can be any unused character)
                        tmp = tmp.Replace('\'', '%'); // same as above
                        fields.Add(tmp);
                    }
                    catch (System.ArgumentOutOfRangeException e)
                    {
                        MessageBox.Show("ERROR: Indexing for the columns out of place when converting from *.txt to *.csv.\nAlert Kyle Williams at kyle.williams2@anheuser-busch.com");
                        break;
                    }
                }

                rankerFile.RemoveAt(i); // memory purposes; too many lists getting hella big
                                        // zero indexed because we each time we remove one, the next line moves up to position 0
                csvStrings.Add(System.String.Join(",", fields));
                fields.Clear();
            }
            
            try
            {
                csvStrings.Reverse(); // need to reverse the data since we read it in backwards
                File.WriteAllLines(dstPath, csvStrings);
            }
            catch (System.IO.IOException e)
            {
                MessageBox.Show("ERROR: File is open in another program. Please close this file and try again.");
            }
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
