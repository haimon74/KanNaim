using System;
using System.IO;

namespace WinLogger
{
    public class Messages
    {
        private static string _path = "c:\\logger\\msg.log";
        private const string HorizontalLine = "================================================================";

        private delegate void TryCatchDelegate(object[] args);

        
        private  Messages()
        {
            // static class
        }

        public static string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        private static bool ExecuteTryCatch(TryCatchDelegate func, object[] args)
        {
            try
            {
                func(args);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static void WriteToLoggerFile(object[] args)
        {
            string msg = (string)args[0];
            FileStream fs = new FileStream(_path, FileMode.Append, FileAccess.Write, FileShare.None, 4096, FileOptions.WriteThrough);
            StreamWriter wr = new StreamWriter(fs);
            wr.Write(msg);
        }

        public static bool Write(string msgType, string content)
        {
            
            string msg = String.Format("\r\n{0} Message Type - {1}:\r\n\r\n{2}\r\n\r\n{3}\r\n\r\n", 
                DateTime.Now, msgType, content, HorizontalLine);
            
            return ExecuteTryCatch(WriteToLoggerFile, new object[]{msg});
        }
    }
}
