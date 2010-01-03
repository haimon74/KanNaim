using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HaimDLL
{
    public class Messages
    {
        private static readonly IDictionary ExceptionToCaptionCollection = new Dictionary<string,string>();

        Messages()
        {
            ExceptionToCaptionCollection["Exception"] = "Exception";
            ExceptionToCaptionCollection["ArgumentNullException"] = "Argument Null Exception";
            ExceptionToCaptionCollection["ArgumentOutOfRangeException"] = "Argument Out Of Range Exception";
            ExceptionToCaptionCollection["DivideByZeroException"] = "Divide By Zero Exception"; 
            ExceptionToCaptionCollection["ArgumentException"] = "Argument Exception";
            ExceptionToCaptionCollection["ApplicationException"] = "Application Exception";
            ExceptionToCaptionCollection["AccessViolationException"] = "Access Violation Exception"; 
            ExceptionToCaptionCollection["BadImageFormatException"] = "Bad Image Format Exception"; 
            ExceptionToCaptionCollection["IndexOutOfRangeException"] = "Index Out Of Range Exception";
            ExceptionToCaptionCollection["InvalidProgramException"] = "Invalid Program Exception";
            ExceptionToCaptionCollection["NullReferenceException"] = "Null Reference Exception";
            ExceptionToCaptionCollection["InvalidOperationException"] = "Invalid Operation Exception";
            ExceptionToCaptionCollection["InsufficientMemoryException"] = "Insufficient Memory Exception";
            ExceptionToCaptionCollection["OutOfMemoryException"] = "Out Of Memory Exception";
            ExceptionToCaptionCollection["SystemException"] = "System Exception";
            ExceptionToCaptionCollection["StackOverflowException"] = "Stack Overflow Exception";
            ExceptionToCaptionCollection["TimeoutException"] = "Timeout Exception"; 
            ExceptionToCaptionCollection["UnauthorizedAccessException"] = "Unauthorized Access Exception"; 
            //ExceptionToCaptionCollection["Exception"] = "";
            //ExceptionToCaptionCollection["Exception"] = "";
            //ExceptionToCaptionCollection["Exception"] = "";
            //ExceptionToCaptionCollection["Exception"] = "";
            //ExceptionToCaptionCollection["Exception"] = "";
        }
        public static void ExceptionMessage(Exception ex)
        {
            string caption = (string) ExceptionToCaptionCollection[ex.GetType().ToString()];
            MessageBox.Show(ex.Message, caption);
        }

        public static void ExceptionMessage(Exception ex, string caption)
        {
            MessageBox.Show(ex.Message,caption);
        }

        public static void ExceptionMessage(Exception ex, string msg, string caption)
        {
            if (caption.Length < 5)
                caption = (string) ExceptionToCaptionCollection[ex.GetType().ToString()];

            if (msg.Length < 5)
                msg = ex.Message;

            MessageBox.Show(msg, caption);
        }
    }
}
