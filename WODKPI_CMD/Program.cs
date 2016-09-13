using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WODKPI_CMD
{
    static class Program
    {
        #region REFUSAL CONSTS

        public static string refusalsDataSrc = @"C:\Users\y712969\Desktop\WODKPI Data\Ranker Text Files\CLEAN\R81_clean.txt";
        public static string refusalsConnStr = @"Data Source=ABCSTLT6602;Initial Catalog=WODKPI_Refusals;Integrated Security=True";
        public static string refusalsCreateTempFile = @"C:\Users\y712969\Desktop\Queries\WODKPI_Refusals\Non-Sequential\00 - createTempRefusals.sql";
        public static string refusalsDropTables = @"C:\Users\y712969\Desktop\Queries\WODKPI_Refusals\Non-Sequential\dropTables.sql";
        public static string refusalsCreateMiscTablesQueryPath = @"C:\Users\y712969\Desktop\Queries\WODKPI_Refusals";
        public static string refusalsDir = @"C:\Users\y712969\Desktop\WODKPI Data\CLEAN\";
        public static int[] refusalsColWidths = new int[]
        {
            6,  // rep#
            14, // repName
            11, // cust#
            26, // custName
            17, // inv#
            6,  // item#
            20, // itemDesc
            8,  // itemQty
            11, // item$
            12, // orderDate
            6,  // driver#
            15, // driverName
            24, // reason
            8,  // load#
            8,  // CE
            9,  // PDCN
            21  // branch
        };
        public static string[] refusalsColNames = new string[]
        {
            "rep#",         // 
            "repName",      //
            "cust#",        //
            "custName",     //
            "inv#",         //
            "item#",        //
            "itemDesc",     //
            "itemQty",      //
            "item$",        //
            "orderDate",    //
            "driver#",      //
            "driverName",   // 
            "reason",       //
            "load#",        //
            "CE",           //
            "PDCN",         //
            "branch"        //
        };

        #endregion
        #region OUT OF STOCKS CONSTS

        public static string outOfStocksDataSrc = @"C:\Users\y712969\Desktop\WODKPI Data\Ranker Text Files\CLEAN\OUT_clean.txt";
        public static string outOfStocksConnStr = @"Data Source=ABCSTLT6602;Initial Catalog=WODKPI_OutOfStocks;Integrated Security=True";
        public static string outOfStocksCreateTempFile = @"C:\Users\y712969\Desktop\Queries\WODKPI_OutOfStocks\Non-Sequential\00 - CreateTempOutOfStocks.sql";
        public static string outOfStocksDropTables = @"C:\Users\y712969\Desktop\Queries\WODKPI_OutOfStocks\Non-Sequential\dropTables.sql";
        public static string outOfStocksCreateMiscTablesQueryPath = @"C:\Users\y712969\Desktop\Queries\WODKPI_OutOfStocks\";
        public static int[] outOfStocksColWidths = new int[]
        {
            6,  // ACCT#
            26, // DBA
            6,  // ITEM#
            21, // DESC
            26, // QTY FILLED
            11, // C14 DIV 288
            17, // INV DATE
            7,  // INV#
            5,  // LOAD
            6,  // SM#
            15, // SM NAME
            14,  // PDCN
            21  // branch
        };
        public static string[] outOfStocksColNames = new string[]
        {
            "ACCT#",
            "DBA",
            "ITEM#",
            "DESC",
            "QTY FILLED",
            "C14 DIV 288",
            "INV DATE",
            "INV#",
            "LOAD",
            "SM#",
            "SM NAME",
            "PDCN",
            "Branch"
        };

        #endregion
        #region VOLUME CONSTS

        public static string volumeDataSrc = @"C:\Users\y712969\Desktop\WODKPI Data\Ranker Text Files\CLEAN\AN6_clean.txt";
        public static string volumeConnStr = @"Data Source=ABCSTLT6602;Initial Catalog=WODKPI_Volume;Integrated Security=True";
        public static string volumeCreateTempFile = @"C:\Users\y712969\Desktop\Queries\WODKPI_Volume\Non-Sequential\00 - createTempVolume.sql";
        public static string volumeDropTables = @"C:\Users\y712969\Desktop\Queries\WODKPI_Volume\Non-Sequential\dropTables.sql";
        public static string volumeCreateMiscTablesQueryPath = @"C:\Users\y712969\Desktop\Queries\WODKPI_Volume\";
        public static int[] volumeColWidths = new int[]
        {
            9,  // invoice date
            6,  // warehouse num
            6,  // item num
            22, // item desc
            12, // unit qty
            15, // CE qty
            15, // BE qty
            13, // BXBRCD
            25  // Branch
        };
        public static string[] volumeColNames = new string[]
        {
            "INV DATE",
            "WH#",
            "ITEM#",
            "DESC",
            "UNIT QTY",
            "CE QTY",
            "BE QTY",
            "PDCN",
            "Branch"
        };

        #endregion
        #region SUPPLY CHAIN LOSS CONSTS

        public static string supplyChainLossDataSrc = @"C:\Users\y712969\Desktop\WODKPI Data\Ranker Text Files\CLEAN\Z90_clean.txt";
        public static string supplyChainLossConnStr = @"Data Source=ABCSTLT6602;Initial Catalog=WODKPI_SupplyChainLoss;Integrated Security=True";
        public static string supplyChainLossCreateTempFile = @"C:\Users\y712969\Desktop\Queries\WODKPI_SupplyChainLoss\Non-Sequential\00 - createTempSupplyChainLoss.sql";
        public static string supplyChainLossDropTables = @"C:\Users\y712969\Desktop\Queries\WODKPI_SupplyChainLoss\Non-Sequential\dropTables.sql";
        public static string supplyChainLossCreateMiscTablesQueryPath = @"C:\Users\y712969\Desktop\Queries\WODKPI_SupplyChainLoss\";
        public static int[] supplyChainLossColWidths = new int[]
        {
            6,  // warehouse num
            6,  // item num
            22, // item desc
            9,  // qty
            3,  // transaction type
            3,  // reason code
            10, // reason desc
            9,  // reason date
            8,  // user
            16, // unit cost
            11, // exact cost
            7,  // one Day rate of sale
            12, // DOI (# on hand / odros)
            23  // branch

        };
        public static string[] supplyChainLossColNames = new string[]
        {
            "warehouseNum",
            "itemNum",
            "itemDesc",
            "qty",
            "transactionType",
            "reasonCode",
            "reasonDesc",
            "reasonDate",
            "user",
            "unitCost",
            "exactCost",
            "oneDayRateOfSale",
            "DOI",
            "branch"
        };

        #endregion

        static void Main(string[] args)
        {
            CleanData cd = new CleanData();
            SQL sql = new SQL();

            #region CLEAN DATA

            Console.WriteLine("BEGIN Copy Data  " + DateTime.Now);

            string[] srcPaths =
            {
                @"N:\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\Corporate KPI Database\Ranker Text Files\",
                @"N:\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\Corporate KPI Database\Imports\",
                @"N:\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\Corporate KPI Database\Softeon Reports\",
                @"N:\WOD Public Shared\Operations Shared\01 - Management Pillar\5.0 Product and Process Indicators\Corporate KPI Database\Roadnet Reports Pulled by Omnitracs\"
            };
            string[] dstPaths =
            {
                @"C:\Users\y712969\Desktop\WODKPI Data\Ranker Text Files\",
                @"C:\Users\y712969\Desktop\WODKPI Data\Imports\",
                @"C:\Users\y712969\Desktop\WODKPI Data\Softeon Reports\",
                @"C:\Users\y712969\Desktop\WODKPI Data\Roadnet Reports Pulled by Omnitracs"
            };

            for (int i = 0; i < srcPaths.Length; i++)
            {
                cd.ProcessDirectory(srcPaths[i], dstPaths[i]);
            }

            Console.WriteLine("END   Copy Data  " + DateTime.Now);
            Console.WriteLine();

            Console.WriteLine("BEGIN Clean Data " + DateTime.Now);

            cd.InitClean("all");

            Console.WriteLine("END   Clean Data " + DateTime.Now);
            #endregion

            #region QUERIES

            Console.WriteLine("BEGIN importing out of stocks " + DateTime.Now);
            //sql.executeSQLScript(outOfStocksDropTables, outOfStocksConnStr);
            sql.importRankerToTemp(
                outOfStocksCreateTempFile,
                outOfStocksCreateMiscTablesQueryPath,
                outOfStocksDataSrc,
                outOfStocksConnStr,
                "TempOutOfStocks",
                outOfStocksColWidths,
                outOfStocksColNames,
                outOfStocksDropTables
                );
            Console.WriteLine("END " + DateTime.Now);

            Console.WriteLine("BEGIN importing supply chain loss " + DateTime.Now);
            //sql.executeSQLScript(supplyChainLossDropTables, supplyChainLossConnStr);
            sql.importRankerToTemp(
                supplyChainLossCreateTempFile,
                supplyChainLossCreateMiscTablesQueryPath,
                supplyChainLossDataSrc,
                supplyChainLossConnStr,
                "TempSupplyChainLoss",
                supplyChainLossColWidths,
                supplyChainLossColNames,
                supplyChainLossDropTables);
            Console.WriteLine("END " + DateTime.Now);

            Console.WriteLine("BEGIN importing volume " + DateTime.Now);
            //sql.executeSQLScript(volumeDropTables, volumeConnStr);
            sql.importRankerToTemp(
                volumeCreateTempFile,
                volumeCreateMiscTablesQueryPath,
                volumeDataSrc,
                volumeConnStr,
                "TempVolume",
                volumeColWidths,
                volumeColNames,
                volumeDropTables);
            Console.WriteLine("END " + DateTime.Now);

            Console.WriteLine("BEGIN importing refusals " + DateTime.Now);
            //sql.executeSQLScript(refusalsDropTables, refusalsConnStr);
            sql.importRankerToTemp(
                refusalsCreateTempFile,
                refusalsCreateMiscTablesQueryPath,
                refusalsDataSrc,
                refusalsConnStr,
                "TempRefusals",
                refusalsColWidths,
                refusalsColNames,
                refusalsDropTables);
            Console.WriteLine("END " + DateTime.Now);

            #endregion

            Console.ReadKey();
        }
    }
}
