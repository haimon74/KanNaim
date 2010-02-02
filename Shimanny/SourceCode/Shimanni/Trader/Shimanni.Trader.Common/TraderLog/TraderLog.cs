using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace Shimanni.Trader.Common
{
    public static class TraderLog
    {
        #region Data Members
        public static string fullPathToStrategyDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"ShimanniStrategy"));
        public static string filePathToParameters = fullPathToStrategyDirectory + @"\Parameters.xml";
        public static string filePathToParametersBackup = fullPathToStrategyDirectory + @"\ParametersBacup.xml";
        public static string filePathToUSAShorts = fullPathToStrategyDirectory + @"\usa.text";
        public static string filePathToProsharesNAV = fullPathToStrategyDirectory + @"\historical_nav.csv";

        public static bool WriteMarketData = true;



        public static StreamWriter StreamWriterForTradingLog;       
        public static StreamWriter StreamWriterForSystemLog;
        #endregion

        #region Consts
        private static readonly string TradingLogFileName = @"\" + "TradingLog" + DateTime.Today.ToString("yyyyMMdd") + ".txt";
        private static readonly string SystemLogFileName = @"\" + "SystemLog" + DateTime.Today.ToString("yyyyMMdd") + ".txt";
        private static string fullPathToLogFiles = 
            System.IO.Path.GetFullPath(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"ShimanniStrategy\LogFiles"));
        #endregion

        #region Methods
        private static void BeginStreamsExtracted(StreamWriter SW, string pFullPath, string pFilePath)
        {

        }
        public static void BeginStreams()
        {
            if (!Directory.Exists(@fullPathToLogFiles))
            {
                Directory.CreateDirectory(@fullPathToLogFiles);
            }
            else
            {
                StreamWriterForSystemLog = new StreamWriter(fullPathToLogFiles + SystemLogFileName, true);
                StreamWriterForSystemLog.AutoFlush = true;

                StreamWriterForTradingLog = new StreamWriter(fullPathToLogFiles + TradingLogFileName, true);
                StreamWriterForTradingLog.AutoFlush = true;
            }
        }
        public static void WriteLineToTradingLog(string text)
        {
            if (StreamWriterForTradingLog != null)
            {
                StreamWriterForTradingLog.WriteLine(DateTime.Now.ToString("hh:mm:ss.ffff") + "\t" + text);
                StreamWriterForTradingLog.Flush();
            }
        }
        public static void WriteLineToSystemLog(string text)
        {
            if (StreamWriterForSystemLog != null)
            {
                StreamWriterForSystemLog.WriteLine(DateTime.Now.ToString("hh:mm:ss.ffff") + "\t" + text);
                StreamWriterForSystemLog.Flush();
            }
        }

        #endregion
    }
}
