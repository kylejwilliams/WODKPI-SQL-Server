
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace WODKPI_CMD
{
    class CleanData
    {
        #region GLOBALS

        List<int> indexes_1CE = new List<int> { 0,  6, 13, 28, 50, 76, 91, 106                                              };
        List<int> indexes_6PU = new List<int> { 0,  8, 15, 22, 43, 72, 83                                                   };
        List<int> indexes_AN6 = new List<int> { 0,  9, 15, 21, 43, 55, 70, 84,  94                                          };
        List<int> indexes_OUT = new List<int> { 0,  6, 32, 38, 59, 74, 84, 96, 113, 120, 125, 131, 146, 156                 };
        List<int> indexes_R81 = new List<int> { 0,  6, 26, 32, 67, 74, 80, 99, 108, 119, 131, 137, 152, 176, 181, 192, 197  };
        List<int> indexes_Z90 = new List<int> { 0,  6, 12, 34, 43, 46, 49, 60,  68,  75,  92, 103, 110, 121                 };
        List<int> indexes_ZIA = new List<int> { 0,  6, 12, 33, 56, 63, 68, 76,  90, 106, 120, 125                           };

        List<int> lengths_1CE = new List<int> { 6,  7, 15, 22, 26, 15, 15, 17                                               };
        List<int> lengths_6PU = new List<int> { 8,  7,  7, 21, 29, 11, 17                                                   };
        List<int> lengths_AN6 = new List<int> { 8,  6,  6, 22, 12, 15, 15, 10, 17                                           };
        List<int> lengths_OUT = new List<int> { 6, 26,  6, 21, 15, 10, 12, 17,  7,    5,   6,  15,  10,  17                 };
        List<int> lengths_R81 = new List<int> { 6, 20,  6, 35,  7,  6, 19,  9, 11,   12,   6,  15,  24,   5,  11,   5,  17  };
        List<int> lengths_Z90 = new List<int> { 6,  6, 22,  9,  3,  3, 11,  8,  7,   17,  11,   7,  11,  17                 };
        List<int> lengths_ZIA = new List<int> { 6,  6, 21, 23,  7,  5,  8, 14, 16,   14,   5,  17                           };

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
                    if (line.StartsWith("    "))
                        continue;

                    string cleanLine = line.Trim();
                    
                    if (!IsBadLine(cleanLine))
                    {
                        if (!cleanLine.StartsWith("1") || (cleanLine.StartsWith("1/")))     // need to handle the case that lines with months 1-9 are not formatted similarly to months 10-12
                            cleanLine = "0" + cleanLine;
                        // gets the filename of the file and separates it from the date info in from of filename "[...] Branch.txt", removes the filename, and appends the branch name to the end of the file.
                        fileName = System.IO.Path.GetFileNameWithoutExtension(fileName).Split(new string[] { "] " }, System.StringSplitOptions.None).Last();

                         // need to make sure we're appending the branch to the end of the actual line if it's not the proper length
                        string dirName = System.IO.Path.GetFileName(srcPath);
                        int numPadding = 0;
                        switch (dirName)
                        {
                            case "1CE":
                                //if (lengths_1CE.Sum() - cleanLine.Length - lengths_1CE.Last() < lengths_1CE.Sum() - lengths_1CE.Last())
                                    numPadding = lengths_1CE.Sum() - cleanLine.Length - lengths_1CE.Last();
                                break;
                            case "6PU":
                                //if (cleanLine.Length < lengths_6PU.Sum() - lengths_6PU.Last())
                                    numPadding = lengths_6PU.Sum() - cleanLine.Length - lengths_6PU.Last();
                                break;
                            case "AN6":
                                //if (cleanLine.Length < lengths_AN6.Sum() - lengths_AN6.Last())
                                    numPadding = lengths_AN6.Sum() - cleanLine.Length - lengths_AN6.Last();
                                break;
                            case "OUT":
                                //if (cleanLine.Length < lengths_OUT.Sum() - lengths_OUT.Last())
                                    numPadding = lengths_OUT.Sum() - cleanLine.Length - lengths_OUT.Last();
                                break;
                            case "R81":
                                //if (cleanLine.Length < lengths_R81.Sum() - lengths_R81.Last())
                                    numPadding = lengths_R81.Sum() - cleanLine.Length - lengths_R81.Last();
                                break;
                            case "Z90":
                                //if (cleanLine.Length < lengths_Z90.Sum() - lengths_Z90.Last())
                                    numPadding = lengths_Z90.Sum() - cleanLine.Length - lengths_Z90.Last();
                                break;
                            case "ZIA":
                               // if (cleanLine.Length < lengths_ZIA.Sum() - lengths_ZIA.Last())
                                    numPadding = lengths_ZIA.Sum() - cleanLine.Length - lengths_ZIA.Last();
                                break;
                            default:
                                MessageBox.Show("ERROR: dirName " + dirName + " didn't catch to check length!");
                                break;
                        }
                        cleanLine += new string(' ', numPadding + 4) + fileName;
                        hs.Add(cleanLine);
                    }
                        
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
                "    ",
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
                "EXT",
                "WH ",      // 1CE header
                "DAIDAT ",  // 6PU header
                "INV ",     // AN6 header
                "ACCT# ",   // OUT header
                "FILLED ",  // OUT header #2
                "SALES ",   // R81 header
                "WH# ",     // Z90 header
                "WHS#"      // ZIA header
            };

            return  (badLines.Any(line.StartsWith)) ||                                     // multiple
                    (line == "") ||                                                        // multiple
                    (line.EndsWith("-")) ||                                                // 6PU
                    (System.Text.RegularExpressions.Regex.IsMatch(line, @"^\d+$")) ||      // 6PU
                    (line == "C14") ||                                                     // OUT
                    (System.Double.TryParse(line.Split(' ')[0], out d)) ||                 // OUT
                    (line.Split(' ').Length < 10);                                         // R81
        }

        private string GetFileName(string path)
        {
            string[] pathPieces = path.Split('\\');
            return pathPieces[pathPieces.Length - 1];
        }

        public void InitClean(string ranker)
        {
            if (ranker.ToLower() == "all")
            {
                string[] rankers =
                {
                    "1CE",
                    "6PU",
                    "AN6",
                    "OUT",
                    "R81",
                    "Z90",
                    "ZIA"
                };

                foreach (string r in rankers)
                {
                    string srcPath = @"C:\Users\y712969\Desktop\WODKPI Data\Ranker Text Files\" + r;
                    string dstPath = @"C:\Users\y712969\Desktop\WODKPI Data\Ranker Text Files\CLEAN\" + r + "_clean.txt";
                    CleanFiles(srcPath, dstPath);
                }
            }
            else
            {
                string srcPath = @"C:\Users\y712969\Desktop\Ranker Text Files\" + ranker;
                string dstPath = @"C:\Users\y712969\Desktop\Ranker Text Files\CLEAN\" + ranker + "_clean.txt";
                CleanFiles(srcPath, dstPath);
            }
            
        }

        // not a "real" csv, because it's delimited by pipes (" || "), but you get the idea
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
                        fields.Add(tmp.Trim());
                    }
                    catch (System.ArgumentOutOfRangeException e)
                    {
                        MessageBox.Show("ERROR: Indexing for the columns out of place when converting from *.txt to *.csv.\nAlert Kyle Williams at kyle.williams2@anheuser-busch.com");
                        return;
                    }
                }

                rankerFile.RemoveAt(i); // memory purposes; too many lists getting hella big
                                        // zero indexed because we each time we remove one, the next line moves up to position 0
                csvStrings.Add(System.String.Join("|", fields));
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

        public void pullDataByDate(string srcPath, string dstPath, string date, int startCol, int colLength)
        {
            string[] lines = File.ReadAllLines(srcPath);
            List<string> validLines = new List<string>();
            string tmpLine;
            foreach (string line in lines)
            {
                tmpLine = line.Substring(startCol, colLength);
                if (tmpLine.Contains(date))
                    validLines.Add(tmpLine);
            }

            File.WriteAllLines(dstPath, validLines.ToArray());
        }

        public void ProcessDirectory(string srcPath, string dstPath)
        {
            DirectoryInfo dir = new DirectoryInfo(srcPath);

            if (!dir.Exists)
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + srcPath);

            DirectoryInfo[] dirs = dir.GetDirectories();
            if (!Directory.Exists(dstPath))
                Directory.CreateDirectory(dstPath);

            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                string tmpPath = Path.Combine(dstPath);
                ProcessFile(file, tmpPath);
            }
                
            foreach (DirectoryInfo subDir in dirs)
            {
                string tmpPath = Path.Combine(dstPath, subDir.Name);
                ProcessDirectory(subDir.FullName, tmpPath);
            }
        }

        public void ProcessFile(FileInfo file, string dstPath, bool overwriteFiles = false)
        {
            FileInfo dstFile = new FileInfo(Path.Combine(dstPath, file.Name));
            if (!dstFile.Exists || (file.LastWriteTime > dstFile.LastWriteTime && overwriteFiles == true))
                file.CopyTo(dstFile.FullName, true);
        }
    }
}
