using System;
using System.Windows.Forms;

namespace WinMessages
{
    public class ShowMessage
    {
        private static MessageBoxOptions _options = MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;
        private static bool _loggerEnabled = true;
        private static bool _extendedMessages = true;

        private ShowMessage()
        {
            // static functions class
        }

        public static MessageBoxOptions Options
        {
            get { return _options; }
            set { _options = value; }
        }

        public static bool LoggerEnabled
        {
            get { return _loggerEnabled; }
            set { _loggerEnabled = value; }
        }

        public static bool ExtendedMessages
        {
            get { return _extendedMessages; }
            set { _extendedMessages = value; }
        }

        private static string GetRecursiveMessage(Exception exception, string input)
        {
            if (exception != null)
            {
                input += exception.Message + "\r\n\r\n";
                return GetRecursiveMessage(exception.InnerException, input);
            }
            else
            {
                return input;
            }

        }
        public static DialogResult AbortRetryIgnoreError(Form form, string msg, string title, Exception exception)
        {
            string recursiveMsg = "\r\n" + exception.Source +
                                  GetRecursiveMessage(exception, "") +
                                  exception.StackTrace;

            msg = (ExtendedMessages) ? recursiveMsg : msg;
            if (LoggerEnabled)
                WinLogger.Messages.Write("error", msg);

            return MessageBox.Show(form
                , msg
                , title
                , MessageBoxButtons.AbortRetryIgnore
                , MessageBoxIcon.Error
                , MessageBoxDefaultButton.Button3
                , Options);
        }
        public static DialogResult RetryCancelError(Form form, string msg, string title, Exception exception)
        {
            string recursiveMsg = "\r\n" + exception.Source +
                                  GetRecursiveMessage(exception, "") +
                                  exception.StackTrace;

            msg = (ExtendedMessages) ? recursiveMsg : msg;
            if (LoggerEnabled)
                WinLogger.Messages.Write("error", msg);

            return MessageBox.Show(form
                , msg
                , title
                , MessageBoxButtons.RetryCancel
                , MessageBoxIcon.Error
                , MessageBoxDefaultButton.Button2
                , Options);
        }
        public static DialogResult OkError(Form form, string msg, string title, Exception exception)
        {
            string recursiveMsg = "\r\n" + exception.Source +
                                  GetRecursiveMessage(exception, "") +
                                  exception.StackTrace;

            msg = (ExtendedMessages) ? recursiveMsg : msg;
            if (LoggerEnabled)
                WinLogger.Messages.Write("error", msg);

            try
            {
                return MessageBox.Show(form
                                       , msg
                                       , title
                                       , MessageBoxButtons.OK
                                       , MessageBoxIcon.Error
                                       , MessageBoxDefaultButton.Button1
                                       , Options);
            }
            catch
            {
                return MessageBox.Show(msg
                                       , title
                                       , MessageBoxButtons.OK
                                       , MessageBoxIcon.Error
                                       , MessageBoxDefaultButton.Button1
                                       , Options);
            }
        }
        public static DialogResult YesNoCancelWarning(Form form, string msg, string title, Exception exception)
        {
            string recursiveMsg = "\r\n" + exception.Source +
                                  GetRecursiveMessage(exception, "") +
                                  exception.StackTrace;

            msg = (ExtendedMessages) ? recursiveMsg : msg;
            if (LoggerEnabled)
                WinLogger.Messages.Write("warning", msg);

            return MessageBox.Show(form
                , msg
                , title
                , MessageBoxButtons.YesNoCancel
                , MessageBoxIcon.Warning
                , MessageBoxDefaultButton.Button3
                , Options);
        }
        public static DialogResult YesNoCancelQuestion(Form form, string msg, string title)
        {
            if (LoggerEnabled)
                WinLogger.Messages.Write("question", msg);

            return MessageBox.Show(form
                , msg
                , title
                , MessageBoxButtons.YesNoCancel
                , MessageBoxIcon.Question
                , MessageBoxDefaultButton.Button3
                , Options);
        }
        public static DialogResult OKCancelWarning(Form form, string msg, string title, Exception exception)
        {
            string recursiveMsg = "\r\n" + exception.Source +
                                  GetRecursiveMessage(exception, "") +
                                  exception.StackTrace;

            msg = (ExtendedMessages) ? recursiveMsg : msg;
            if (LoggerEnabled)
                WinLogger.Messages.Write("warning", msg);

            return MessageBox.Show(form
                , msg
                , title
                , MessageBoxButtons.OKCancel
                , MessageBoxIcon.Warning
                , MessageBoxDefaultButton.Button2
                , Options);
        }
        public static DialogResult OKCancelQuestion(Form form, string msg, string title)
        {
            if (LoggerEnabled)
                WinLogger.Messages.Write("question", msg);

            return MessageBox.Show(form
                , msg
                , title
                , MessageBoxButtons.OKCancel
                , MessageBoxIcon.Question
                , MessageBoxDefaultButton.Button2
                , Options);
        }
        public static DialogResult OKInformation(Form form, string msg, string title)
        {
            if (LoggerEnabled)
                WinLogger.Messages.Write("Info", msg);

            return MessageBox.Show(form
                , msg
                , title
                , MessageBoxButtons.OK
                , MessageBoxIcon.Information
                , MessageBoxDefaultButton.Button1
                , Options);
        }
    }
}