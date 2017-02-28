using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Data.OleDb;

namespace WODKPI_CMD
{
    public class ExcelEngine
    {
        /// <summary>
        /// To combine multiple workbooks into a file
        /// </summary>
        /// <remarks>
        /// The following file name convention will be used while combining the child files
        /// exportFileKey_[Description] where the description will be the tab name
        ///
        /// The ordering of the worksheets will be using the file creation time
        ///
        /// The above convention can be enhanced when necessary but need to make sure the backward compatibility for the existing codes
        ///
        /// Note:
        /// - the index starts from 1 in the excel automation array
        /// - be careful when making changes, especially moving things around in the method, e.g. prompts might come up unexpectedly
        /// - to avoid "zombie" excel instances in the task manager when referencing the COM object, please refer to the http://support.microsoft.com/default.aspx/kb/317109
        ///
        /// </remarks>
        /// <param name="exportFilePath">the destination file name choosen by the user</param>
        /// <param name="exportFileKey">the unique key file name choosen by the user, this is to avoid merging files with similar names</param>
        /// <param name="rawFilesDirectory">the folder where the files are being generated, this can be temp folder or any folder basically</param>
        /// <param name="deleteRawFiles">delete the raw files after completed?</param>
        ///
        /// <returns></returns>
        public static bool CombineWorkBooks(string exportFilePath, string exportFileKey, string rawFilesDirectory, bool deleteRawFiles)
        {
            Application xlApp = null;
            Workbooks newBooks = null;
            Workbook newBook = null;
            Sheets newBookWorksheets = null;
            Worksheet defaultWorksheet = null;
            IEnumerable<string> filesToMerge = null;
            bool areRowsTruncated = false;

            try
            {
                Console.WriteLine(" Method: CombineWorkBooks - Starting excel ");
                xlApp = new Application();

                if (xlApp == null)
                {
                    Console.WriteLine(" EXCEL could not be started.Check that your office installation and project references are correct.");
                    return false;
                }

                Console.WriteLine(" Method: CombineWorkBooks - Disabling the display alerts to prevent any prompts during workbooks close");
                // NOT AN ELEGANT SOLUTION? HOWEVER HAS TO DO THIS ELSE WILL PROMPT FOR SAVE ON EXIT, EVEN SET THE SAVED PROPERTY DIDN'T HELP
                xlApp.DisplayAlerts = false;

                Console.WriteLine(" Method: CombineWorkBooks - Set Visible to false as a background process, else it will be displayed in the task bar ");
                xlApp.Visible = false;

                Console.WriteLine(" Method: CombineWorkBooks - Create a new workbook, comes with an empty default worksheet");
                newBooks = xlApp.Workbooks;
                newBook = newBooks.Add(XlWBATemplate.xlWBATWorksheet);
                newBookWorksheets = newBook.Worksheets;

                // GET THE REFERENCE FOR THE EMPTY DEFAULT WORKSHEET
                if (newBookWorksheets.Count > 0)
                {
                    defaultWorksheet = newBookWorksheets[1] as Worksheet;
                }

                Console.WriteLine(" Method: CombineWorkBooks - Get the files sorted by creation date");
                var dirInfo = new DirectoryInfo(rawFilesDirectory);
                filesToMerge = from f in dirInfo.GetFiles()
                               orderby f.CreationTimeUtc
                               select f.FullName;
                //filesToMerge = from f in dirInfo.GetFiles(exportFileKey + " *", SearchOption.TopDirectoryOnly)
                //               orderby f.CreationTimeUtc
                //               select f.FullName;

                foreach (var filePath in filesToMerge)
                {
                    Workbook childBook = null;
                    Sheets childSheets = null;
                    try
                    {
                        Console.WriteLine(" Method: CombineWorkBooks - Processing {0}", filePath);
                        childBook = newBooks.Open(filePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, 
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                        childSheets = childBook.Worksheets;
                        if (childSheets != null)
                        {
                            for (int iChildSheet = 1; iChildSheet <= childSheets.Count; iChildSheet++)
                            {
                                Worksheet sheetToCopy = null;
                                try
                                {
                                    sheetToCopy = childSheets[iChildSheet] as Worksheet;
                                    if (sheetToCopy != null)
                                    {
                                        Console.WriteLine(" Method: CombineWorkBooks - Assigning the worksheet name ");
                                        sheetToCopy.Name = Truncate(GetReportDescription(Path.GetFileNameWithoutExtension(filePath), sheetToCopy.Name), 31); // ONLY 31 CHAR MAX

                                        //sheetToCopy.
                                        Console.WriteLine(" Method: CombineWorkBooks - Copy the worksheet before the default sheet");
                                        sheetToCopy.Copy(defaultWorksheet, Type.Missing);
                                    }
                                }
                                finally
                                {
                                    DisposeCOMObject(sheetToCopy);
                                }
                            }

                            Console.WriteLine(" Method: CombineWorkBooks - Close the childbook");
                            // FOR SOME REASON, CALLING CLOSE BELOW MAY CAUSE AN EXCEPTION -> SYSTEM.RUNTIME.INTEROPSERVICES.COMEXCEPTION (0X80010108): THE OBJECT INVOKED HAS DISCONNECTED FROM ITS CLIENTS.
                            childBook.Close(false, Type.Missing, Type.Missing);
                        }
                    }
                    finally
                    {
                        DisposeCOMObject(childSheets);
                        DisposeCOMObject(childBook);
                    }
                }

                Console.WriteLine(" Method: CombineWorkBooks - Delete the empty default worksheet");
                if (defaultWorksheet != null) defaultWorksheet.Delete();

                Console.WriteLine(" Method: CombineWorkBooks - Save the new book into the export file path: {0}", exportFilePath);
                newBook.SaveAs(exportFilePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, 
                    XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                newBooks.Close();
                xlApp.DisplayAlerts = true;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Method: CombineWorkBooks - Exception: {0}", ex.ToString());
                return false;
            }
            finally
            {
                DisposeCOMObject(defaultWorksheet);
                DisposeCOMObject(newBookWorksheets);
                DisposeCOMObject(newBooks);
                DisposeCOMObject(newBook);

                Console.WriteLine(" Method: CombineWorkBooks - Closing the excel app ");
                if (xlApp != null)
                {
                    xlApp.Quit();
                    DisposeCOMObject(xlApp);
                }

                if (deleteRawFiles)
                {
                    Console.WriteLine(" Method: CombineWorkBooks - Deleting the temporary files ");
                    DeleteTemporaryFiles(filesToMerge);
                }
            }
        }

        private static void DisposeCOMObject(object o)
        {
            Console.WriteLine(" Method: DisposeCOMObject - Disposing ");
            if (o == null)
            {
                return;
            }
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Method: DisposeCOMObject - Exception: {0}", ex.ToString());
            }
        }

        private static void DeleteTemporaryFiles(IEnumerable<string> tempFilenames)
        {
            foreach (var tempFile in tempFilenames)
            {
                try
                {
                    File.Delete(tempFile);
                }
                catch
                    (Exception)
                {
                    Console.WriteLine("Could not delete temporary file '{0}'", tempFilenames);
                }
            }
        }

        /// <summary>
        /// the first array item will be the key
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="defaultName"></param>
        /// <returns></returns>
        protected static string GetReportDescription(string fileName, string defaultName)
        {
            var splits = fileName.Split('_');
            return splits.Length > 1 ? string.Join(" -", splits, 1, splits.Length - 1) : defaultName;
        }

        /// <summary>
        /// Get a substring of the first N characters.
        /// http://dotnetperls.com/truncate-string
        /// </summary>
        public static string Truncate(string source, int length)
        {
            if (source.Length > length)
            {
                source = source.Substring(0, length);
            }
            return source;
        }

        //static void ConvertExcelToCsv(string excelFilePath, string csvOutputFile, int worksheetNumber = 1)
        //{
        //    if (!File.Exists(excelFilePath)) throw new FileNotFoundException(excelFilePath);
        //    if (File.Exists(csvOutputFile)) throw new ArgumentException("File exists: " + csvOutputFile);

        //    // connection string
        //    var cnnStr = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;IMEX=1;HDR=NO\"", excelFilePath);
        //    var cnn = new OleDbConnection(cnnStr);

        //    // get schema, then data
        //    var dt = new DataTable();
        //    try
        //    {
        //        cnn.Open();
        //        var schemaTable = cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //        if (schemaTable.Rows.Count < worksheetNumber) throw new ArgumentException("The worksheet number provided cannot be found in the spreadsheet");
        //        string worksheet = schemaTable.Rows[worksheetNumber - 1]["table_name"].ToString().Replace("'", "");
        //        string sql = String.Format("select * from [{0}]", worksheet);
        //        var da = new OleDbDataAdapter(sql, cnn);
        //        da.Fill(dt);
        //    }
        //    catch (Exception e)
        //    {
        //        // ???
        //        throw e;
        //    }
        //    finally
        //    {
        //        // free resources
        //        cnn.Close();
        //    }

        //    // write out CSV data
        //    using (var wtr = new StreamWriter(csvOutputFile))
        //    {
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            bool firstLine = true;
        //            foreach (DataColumn col in dt.Columns)
        //            {
        //                if (!firstLine) { wtr.Write(","); } else { firstLine = false; }
        //                var data = row[col.ColumnName].ToString().Replace("\"", "\"\"");
        //                wtr.Write(String.Format("\"{0}\"", data));
        //            }
        //            wtr.WriteLine();
        //        }
        //    }
        //}
    }
}
