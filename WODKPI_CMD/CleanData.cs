
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace WODKPI_CMD
{
    class CleanData
    {
        List<string> headers_SU6 = new List<string>
        {
            "copyright",
            "COPYRIGHT",
            "rfm.",
            "RFM.",
            "cfm.",
            "CFM.",
            "anheuser-",
            "ANHEUSER",
            "wh inv",
            "WH INV",
            "<!doctype"
        };
        List<string> headers_S90 = new List<string>
        {
            "COPYRIGHT",
            "RFM.",
            "ANHEUSER",
            "WH#",
            "EUGENE",
            "CFM.",
            "RFM.",
            "AUGUST",
            "RANKER"
        };

        public CleanData()
        {

        }

        //public void CleanAllFiles(string srcPath, string dstPath)
        //{
        //    DirectoryInfo dir = new DirectoryInfo(srcPath);
        //    foreach (FileInfo fi in dir.EnumerateFiles())
        //    {
        //        if (fi.Name != "SU6_clean.txt")
        //            CleanFile(fi.FullName, dstPath);
        //    }
        //}

        //private void cleanfile(string srcpath, string dstpath)
        //{
        //    hashset<string> hs = new hashset<string>();
        //    list<string> headerlines = new list<string>();
        //    headerlines.add("copyright");
        //    headerlines.add("rfm.");
        //    headerlines.add("anheuser-");
        //    headerlines.add("wh inv");

        //    string filename;
        //    int numpadding;
        //    string line;


        //    //read in any existing data in the dstpath if it exists
        //    if (file.exists(dstpath))
        //    {
        //        using (streamreader sr = file.opentext(dstpath))
        //        {
        //            while (!sr.endofstream)
        //            {
        //                line = sr.readline();
        //                hs.add(line);
        //            }
        //        }
        //    }

        //    // read in any new data
        //    using (streamreader sr = file.opentext(srcpath))
        //    {
        //        while (!sr.endofstream)
        //        {
        //            line = sr.readline();
        //            filename = system.io.path.getfilenamewithoutextension(srcpath).split(new string[] { "] " }, system.stringsplitoptions.none).last();
        //            numpadding = lengths_su6.sum() - line.length - lengths_su6.last();
        //            if (numpadding < 0) numpadding = 0; // prevent negative padding
        //            line += new string(' ', numpadding + 4) + filename;
        //            hs.add(line);
        //        }
        //    }
        //    // get rid of any lurking headers
        //    hs.removewhere(s => headerlines.any(s.contains));
        //    // overwrite the dst file and create the new based on our read data up to now
        //    file.writealllines(dstpath, hs);
        //}

        //using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(superConnStr))
        //{
        //    conn.Open();
        //    for (int i = 0; i < 55; i++)
        //    {
        //        Console.WriteLine("Beginning write for rows " + i + " million...");
        //        sql.executeSQLScript(file, conn);
        //        Console.WriteLine("End");
        //    }
        //}

        //List<string> lines = File.ReadLines(cleanDstPath).Skip(52295579).ToList();
        //File.WriteAllLines(@"C:\Users\y712969\Desktop\SU6 Rankers\temp.txt", lines);

        //List<string> files = File.ReadAllLines(@"C:\Users\y712969\Desktop\SU6 Rankers\temp.txt").ToList();
        //List<string> newFiles = new List<string>();
        //foreach (string file in files)
        //{
        //    if (file.Length > 223) newFiles.Add(file.Substring(0, 223));
        //    else newFiles.Add(file);
        //}
        //File.WriteAllLines(@"C:\Users\y712969\Desktop\SU6 Rankers\temp.txt", newFiles);


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
                //"28",
                "=",
                "EXT",
                "WH ",      // 1CE header
                "DAIDAT ",  // 6PU header
                "INV ",     // AN6 header
                "ACCT# ",   // OUT header
                "FILLED ",  // OUT header #2
                "SALES ",   // R81 header
                "WH# ",     // S90 header
                "WHS#"      // ZIA header
            };

            return  (badLines.Any(line.StartsWith)) ||                                     // multiple
                    (line == "") ||                                                        // multiple
                    (line.EndsWith("-")) ||                                                // 6PU
                    (System.Text.RegularExpressions.Regex.IsMatch(line, @"^\d+$")) ||      // 6PU
                    (line == "C14") ||                                                     // OUT
                    //(System.Double.TryParse(line.Split(' ')[0], out d)) ||                 // OUT
                    (line.Split(' ').Length < 10);                                         // R81
        }

        private string GetFileName(string path)
        {
            string[] pathPieces = path.Split('\\');
            return pathPieces[pathPieces.Length - 1];
        }

        public void InitClean(string srcPath, string dstPath)
        {
            //if (ranker.ToLower() == "all")
            //{
            //    string[] rankers =
            //    {
            //        "AN6",
            //        "OUT",
            //        "R81",
            //        "S90",
            //    };

            //    foreach (string r in rankers)
            //    {
            //        string srcPath = @"C:\Users\y712969\Desktop\WODKPI Data\Ranker Text Files\" + r;
            //        string dstPath = @"C:\Users\y712969\Desktop\WODKPI Data\Ranker Text Files\CLEAN\" + r + "_clean.txt";
            //        CleanFiles(srcPath, dstPath);
            //    }
            //}
            //else
            //{
            //    string srcPath = @"C:\Users\y712969\Desktop\WODKPI Data\Ranker Text Files\" + ranker;
            //    string dstPath = @"C:\Users\y712969\Desktop\WODKPI Data\Ranker Text Files\CLEAN\" + ranker + "_clean.txt";
            //    CleanFiles(srcPath, dstPath);
            //}
            //CleanFiles(srcPath, dstPath);
            
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

        //public void ProcessDirectory(string srcPath, string dstPath)
        //{
        //    DirectoryInfo dir = new DirectoryInfo(srcPath);

        //    if (!dir.Exists)
        //        throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + srcPath);

        //    DirectoryInfo[] dirs = dir.GetDirectories();
        //    if (!Directory.Exists(dstPath))
        //        Directory.CreateDirectory(dstPath);

        //    FileInfo[] files = dir.GetFiles();

        //    foreach (FileInfo file in files)
        //    {
        //        string tmpPath = Path.Combine(dstPath);
        //        //ProcessFile(file, tmpPath);
        //    }
                
        //    foreach (DirectoryInfo subDir in dirs)
        //    {
        //        string tmpPath = Path.Combine(dstPath, subDir.Name);
        //        ProcessDirectory(subDir.FullName, tmpPath);
        //    }
        //}

        //public void ProcessFile(FileInfo file, string rankerPath, string pendingPath, string processedPath, bool overwriteFiles = false)
        //{
        //    FileInfo dstFile = new FileInfo(Path.Combine(dstPath, file.Name));
        //    if (!dstFile.Exists || (file.LastWriteTime > dstFile.LastWriteTime && overwriteFiles == true))
        //        file.CopyTo(dstFile.FullName, true);
        //}

        public void CopyFiles(string rankerPath, string processedPath, string pendingPath)
        {
            DirectoryInfo rankerDir = new DirectoryInfo(rankerPath);
            DirectoryInfo processedDir = new DirectoryInfo(processedPath);
            if (!rankerDir.Exists || !processedDir.Exists)
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found");

            HashSet<string> files = new HashSet<string>();
            foreach (FileInfo file in rankerDir.GetFiles())
            {
                FileInfo processedFile = new FileInfo(Path.Combine(processedPath, file.Name));
                FileInfo pendingFile = new FileInfo(Path.Combine(pendingPath, file.Name));
                if ((!processedFile.Exists && !pendingFile.Exists) || ((file.LastWriteTime > processedFile.LastWriteTime) && (file.LastWriteTime > pendingFile.LastWriteTime)))
                    file.CopyTo(Path.Combine(pendingPath, file.Name), true);
            }
        }

        public void CleanFiles(string srcPath, string dstPath, string filePath, string ranker, List<int> colLengths)
        {
            HashSet<string> hs = new HashSet<string>();
            DirectoryInfo srcDir = new DirectoryInfo(srcPath);
            DirectoryInfo dstDir = new DirectoryInfo(dstPath);
            FileInfo cleanFile = new FileInfo(filePath);
            int numFiles = 0;
            File.Create(filePath).Close(); // clear any contents / create file if it doesn't exist
            if (!srcDir.Exists || !dstDir.Exists)
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found");

            while ((numFiles = srcDir.GetFiles().Count()) > 0) // only run if we actually have new files to process
            {
                FileInfo file = srcDir.GetFiles().First();
                switch (ranker)
                {
                    case "SU6":
                        hs = CleanFile(file.FullName, headers_SU6, ranker, colLengths);
                        break;
                    case "S90":
                        hs = CleanFile(file.FullName, headers_S90, ranker, colLengths);
                        break;
                }
                File.AppendAllLines(cleanFile.FullName, hs);
                System.IO.File.Copy(file.FullName, dstPath + file.Name, true);
                System.IO.File.Delete(file.FullName);
            }
        }

        private HashSet<string> CleanFile(string filePath, List<string> headers, string ranker, List<int> colLengths)
        {
            HashSet < string > hs = new HashSet<string>();
            
            string filename = "";
            int numPadding;
            string line;

            FileInfo file = new FileInfo(filePath);
            using (StreamReader sr = file.OpenText())
            {
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    // example: "[10-02-15 to 10-03-15] Boston.txt" --> "Boston"
                    // splits the file name at the ']' and '.' characters to isolate the filename and trims off any additional whitespace
                    filename = file.Name
                        .Split(new string[] { "]" }, System.StringSplitOptions.None).Last()
                        .Split(new string[] { "." }, System.StringSplitOptions.None).First() 
                        .Split(new string[] { "-" }, System.StringSplitOptions.None).First() // handle the case when one ranker file is split across many text files
                        .Trim();
                    numPadding = colLengths.Sum() - line.Length - colLengths.Last();
                    if (numPadding < 0) numPadding = 0;
                    if (!filename.Contains(ranker)) line += new string(' ', numPadding + 4) + filename; // append branch
                    if (!(line.Trim() == filename)) hs.Add(line);
                }
            }

            // get rid of any lurking headers
            hs.RemoveWhere(s => s.Length <= 0);
            hs.RemoveWhere(s => headers.Any(s.Contains));
            if (ranker == "S90") { hs.RemoveWhere(s => s.Substring(0, 6).Contains(@"/")); } // weirdly formatted header containing date in WH# column
            // overwrite the dst file and create the new based on our read data up to now
            return hs;
        }
    }
}
