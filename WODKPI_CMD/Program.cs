
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace WODKPI_CMD
{
    static class Program
    {
        public static string sclDataSrc = @"\\stlabcfil002\abi_wod$\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\WODKPI\WODKPI Data\S90 Rankers\S90_clean.txt";
        public static string superDataSrc = @"\\stlabcfil002\abi_wod$\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\WODKPI\SU6 Rankers\SU6_clean.txt";
        public static string connStr = @"Data Source=ABCSTLT6602;Initial Catalog=WOD_RAS;Integrated Security=True;";// False;User=srvWODKPI;PWD=password123456";
        public static string sclCreateTempFile = @"\\stlabcfil002\abi_wod$\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\WODKPI\WODKPI Data\Queries\createSCL.sql";
        public static string superCreateTempFile = @"\\stlabcfil002\abi_wod$\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\WODKPI\WODKPI Data\Queries\createSU6.sql";
        public static string sclInsertData = @"\\stlabcfil002\abi_wod$\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\WODKPI\WODKPI Data\Queries\updateSCL.sql";
        public static string superInsertData = @"\\stlabcfil002\abi_wod$\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\WODKPI\WODKPI Data\Queries\updateSU6.sql";

        public static string delVolAccessPth = @"\\stlabcfil002\abi_wod$\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\WODKPI\WODKPI Data\Queries\deleteVolumesAccess.sql";
        public static string insVolAccessPth = @"\\stlabcfil002\abi_wod$\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\WODKPI\WODKPI Data\Queries\importVolumesToAccess.sql";
        public static string delRefAccessPth = @"\\stlabcfil002\abi_wod$\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\WODKPI\WODKPI Data\Queries\deleteRefusalsAccess.sql";
        public static string insRefAccessPth = @"\\stlabcfil002\abi_wod$\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\WODKPI\WODKPI Data\Queries\importRefusalsToAccess.sql";
        public static string delOosAccessPth = @"\\stlabcfil002\abi_wod$\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\WODKPI\WODKPI Data\Queries\deleteOOSAccess.sql";
        public static string insOosAccessPth = @"\\stlabcfil002\abi_wod$\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\WODKPI\WODKPI Data\Queries\importOOSToAccess.sql";
        public static string delSclAccessPth = @"\\stlabcfil002\abi_wod$\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\WODKPI\WODKPI Data\Queries\deleteSCLAccess.sql";
        public static string insSclAccessPth = @"\\stlabcfil002\abi_wod$\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\WODKPI\WODKPI Data\Queries\importSCLToAccess.sql";

        public static List<int> lengths_SU6 = new List<int>
        {
            3,  // WH#
            9,  // inv date
            6,  // acct num
            26, // acct desc
            11, // inv num
            3,  // line num
            6,  // item num
            6,  // pdcn
            22, // item desc
            6,  // qty
            11, // price
            5,  // oos
            7,  // oz
            8,  // cpkg
            3,  // rc
            19, // rc desc *
            6,  // rep# *
            15, // rep name
            5,  // load #
            6,  // driver #
            15, // driver name
            25, // branch length + 4 padding
        };
        public static List<int> lengths_S90 = new List<int>
        {
            6,  // WH#
            6,  // ITEM#
            25, // ITEM DESC
            6,  // QTY
            3,  // TC
            3,  // RC
            10, // RC DESC
            9,  // DATE
            10, // USER
            14, // UNIT COST
            11, // EXACT COST
            7,  // ONE DAY
            15, // DAYS OUT OF INVENTORY
            15, // CE QTY
            25  // branch + 4 padding
        };
        public static List<string> superColNames = new List<string>
        {
            "WH#",
            "INV DATE",
            "ACCT NUM",
            "ACCT DESC",
            "INV NUM",
            "LINE NUM",
            "ITEM NUM",
            "PDCN",
            "ITEM DESC",
            "QTY",
            "PRICE",
            "OOS",
            "OZ",
            "C/PKG",
            "RC",
            "RC DESC",
            "REP#",
            "REP NAME",
            "LOAD #",
            "DRIVER #",
            "DRIVER NAME",
            "BRANCH"
        };
        public static List<string> sclColNames = new List<string>
        {
            "warehouseNum",
            "itemNum",
            "itemDesc",
            "qty",
            "transCode",
            "refCode",
            "refDesc",
            "sclDate",
            "userID",
            "unitCost",
            "exactCost",
            "oneDay",
            "DOI",
            "CE QTY",
            "branch"
        };
        public static List<string> branches = new List<string>
        {
            "Beach Cities",
            "Boston",
            "Canton",
            "Denver",
            "Eugene",
            "Hawaii",
            "Lima",
            "Loveland",
            "New Jersey",
            "New York",
            "Oakland",
            "Oklahoma City",
            "Portland",
            "Renton",
            "Riverside",
            "San Diego",
            "Sylmar",
            "Tulsa"
        };

        static void Main(string[] args)
        {

            CleanData cd = new CleanData();
            SQL sql = new SQL();
            TimeSpan ttlRuntime = DateTime.Now.TimeOfDay;
            TimeSpan dt = DateTime.Now.TimeOfDay;

            string copySrcPath =        @"\\stlabcfil002\abi_wod$\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\Corporate KPI Database\Ranker Text Files\";
            string superCleanDstPath =  @"\\stlabcfil002\abi_wod$\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\WODKPI\WODKPI Data\SU6 Rankers\SU6_clean.txt";
            string sclCleanDstPath =    @"\\stlabcfil002\abi_wod$\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\WODKPI\WODKPI Data\S90 Rankers\S90_clean.txt";
            string superProcessedPath = @"\\stlabcfil002\abi_wod$\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\WODKPI\WODKPI Data\SU6 Rankers\PROCESSED\";
            string sclProcessedPath =   @"\\stlabcfil002\abi_wod$\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\WODKPI\WODKPI Data\S90 Rankers\PROCESSED\";
            string superPendingPath =   @"\\stlabcfil002\abi_wod$\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\WODKPI\WODKPI Data\SU6 Rankers\PENDING\";
            string sclPendingPath =     @"\\stlabcfil002\abi_wod$\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\WODKPI\WODKPI Data\S90 Rankers\PENDING\";
            string volumePath =         @"\\stlabcfil002\abi_wod$\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\Corporate KPI Database\Backend\Volume Database.accdb";
            string refusalsPath =       @"\\stlabcfil002\abi_wod$\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\Corporate KPI Database\Backend\Refusals Database.accdb";
            string oosPath =            @"\\stlabcfil002\abi_wod$\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\Corporate KPI Database\Backend\Out of Stock Database.accdb";
            string sclPath =            @"\\stlabcfil002\abi_wod$\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\Corporate KPI Database\Backend\Supply Chain Loss Database.accdb";

            Console.WriteLine("BEGIN Copy Data - SU6 " + DateTime.Now);
            cd.CopyFiles(copySrcPath + @"SU6\", superProcessedPath, superPendingPath);
            dt = DateTime.Now.Subtract(dt).TimeOfDay;
            Console.WriteLine("END Copy Data - SU6 " + DateTime.Now);
            Console.WriteLine("Runtime: " + dt);
            Console.WriteLine();

            dt = DateTime.Now.TimeOfDay;
            Console.WriteLine("BEGIN Copy Data - S90 " + DateTime.Now);
            cd.CopyFiles(copySrcPath + @"S90\", sclProcessedPath, sclPendingPath);
            dt = DateTime.Now.Subtract(dt).TimeOfDay;
            Console.WriteLine("END Copy Data - S90 " + DateTime.Now);
            Console.WriteLine("Runtime: " + dt);
            Console.WriteLine();

            dt = DateTime.Now.TimeOfDay;
            Console.WriteLine("BEGIN Clean Data - SU6 " + DateTime.Now);
            cd.CleanFiles(superPendingPath, superProcessedPath, superCleanDstPath, "SU6", lengths_SU6);
            dt = DateTime.Now.Subtract(dt).TimeOfDay;
            Console.WriteLine("END   Clean Data - SU6 " + DateTime.Now);
            Console.WriteLine("Runtime: " + dt);
            Console.WriteLine();

            dt = DateTime.Now.TimeOfDay;
            Console.WriteLine("BEGIN Clean Data - S90 " + DateTime.Now);
            cd.CleanFiles(sclPendingPath, sclProcessedPath, sclCleanDstPath, "S90", lengths_S90);
            dt = DateTime.Now.Subtract(dt).TimeOfDay;
            Console.WriteLine("END   Clean Data - S90 " + DateTime.Now);
            Console.WriteLine("Runtime: " + dt);
            Console.WriteLine();

            dt = DateTime.Now.TimeOfDay;
            Console.WriteLine("BEGIN SQL UPLOAD - SU6 " + DateTime.Now);
            sql.importRankerToTemp(
                superCreateTempFile,
                superInsertData,
                "TempSU6",
                superCleanDstPath,
                connStr,
                lengths_SU6,
                superColNames
                );
            dt = DateTime.Now.Subtract(dt).TimeOfDay;
            Console.WriteLine("END SQL UPLOAD - SU6 " + DateTime.Now);
            Console.WriteLine("Runtime: " + dt.Minutes + ":" + dt.Seconds + ":" + dt.Milliseconds);
            Console.WriteLine();

            dt = DateTime.Now.TimeOfDay;
            Console.WriteLine("BEGIN SQL UPLOAD - S90 " + DateTime.Now);
            sql.importRankerToTemp(
                sclCreateTempFile,
                sclInsertData,
                "TempSCL",
                sclCleanDstPath,
                connStr,
                lengths_S90,
                sclColNames
                );
            dt = DateTime.Now.Subtract(dt).TimeOfDay;
            Console.WriteLine("END SQL UPLOAD - S90 " + DateTime.Now);
            Console.WriteLine("Runtime: " + dt.Minutes + ":" + dt.Seconds + ":" + dt.Milliseconds);
            Console.WriteLine();

            dt = DateTime.Now.TimeOfDay;
            Console.WriteLine("BEGIN WRITE TO ACCESS - VOLUMES " + DateTime.Now);
            foreach (string branch in branches)
            {
                Console.WriteLine("Writing to Volumes - " + branch + "...");
                sql.connectToAccess(volumePath, branch, delVolAccessPth);
                sql.connectToAccess(volumePath, branch, insVolAccessPth);
            }
            dt = DateTime.Now.Subtract(dt).TimeOfDay;
            Console.WriteLine("END WRITE TO ACCESS - VOLUMES " + DateTime.Now);
            Console.WriteLine("Runtime: " + dt.Minutes + ":" + dt.Seconds + ":" + dt.Milliseconds);
            Console.WriteLine();

            dt = DateTime.Now.TimeOfDay;
            Console.WriteLine("BEGIN WRITE TO ACCESS - REFUSALS " + DateTime.Now);
            foreach (string branch in branches)
            {
                Console.WriteLine("Writing to Refusals - " + branch + "...");
                sql.connectToAccess(refusalsPath, branch, delRefAccessPth);
                sql.connectToAccess(refusalsPath, branch, insRefAccessPth);
            }
            dt = DateTime.Now.Subtract(dt).TimeOfDay;
            Console.WriteLine("END WRITE TO ACCESS - REFUSALS " + DateTime.Now);
            Console.WriteLine("Runtime: " + dt.Minutes + ":" + dt.Seconds + ":" + dt.Milliseconds);
            Console.WriteLine();

            dt = DateTime.Now.TimeOfDay;
            Console.WriteLine("BEGIN WRITE TO ACCESS - OUT OF STOCKS " + DateTime.Now);
            foreach (string branch in branches)
            {
                Console.WriteLine("Writing to Out of Stocks - " + branch + "...");
                sql.connectToAccess(oosPath, branch, delOosAccessPth);
                sql.connectToAccess(oosPath, branch, insOosAccessPth);
            }
            dt = DateTime.Now.Subtract(dt).TimeOfDay;
            Console.WriteLine("END WRITE TO ACCESS - OUT OF STOCKS " + DateTime.Now);
            Console.WriteLine("Runtime: " + dt.Minutes + ":" + dt.Seconds + ":" + dt.Milliseconds);
            Console.WriteLine();

            dt = DateTime.Now.TimeOfDay;
            Console.WriteLine("BEGIN WRITE TO ACCESS - SUPPLY CHAIN LOSS " + DateTime.Now);
            foreach (string branch in branches)
            {
                Console.WriteLine("Writing to Supply Chain Loss - " + branch + "...");
                sql.connectToAccess(sclPath, branch, delSclAccessPth);
                sql.connectToAccess(sclPath, branch, insSclAccessPth);
            }
            dt = DateTime.Now.Subtract(dt).TimeOfDay;
            Console.WriteLine("END WRITE TO ACCESS - SUPPLY CHAIN LOSS " + DateTime.Now);
            Console.WriteLine("Runtime: " + dt.Minutes + ":" + dt.Seconds + ":" + dt.Milliseconds);
            Console.WriteLine();

            ExcelEngine.CombineWorkBooks(@"C:\Users\y712969\Desktop\test.xlsx", "", @"C:\Users\y712969\Desktop\Softeon Reports\OOS", false);


            ttlRuntime = DateTime.Now.Subtract(ttlRuntime).TimeOfDay;
            Console.WriteLine("TOTAL RUNTIME: " + ttlRuntime.Minutes + ":" + ttlRuntime.Seconds + ":" + ttlRuntime.Milliseconds);
            //Console.ReadLine();
        }
    }
}
