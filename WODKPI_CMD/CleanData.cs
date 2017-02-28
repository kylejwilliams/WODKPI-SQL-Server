
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
