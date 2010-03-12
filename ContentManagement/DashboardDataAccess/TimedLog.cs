
using System;

namespace DashboardDataAccess
{
    public class TimedLog : IDisposable
    {
        private string _Message;
        private long _StartTicks;
        public TimedLog(string userName, string message)
        {
            this._Message = userName + '\t' + message;
            this._StartTicks = DateTime.Now.Ticks;
        }
        #region IDisposable Members

        void IDisposable.Dispose()
        {
            DateTime now = DateTime.Now;
            string msg = now.ToLongDateString() + now.ToLongTimeString() + this._Message + '\t' + 
                         TimeSpan.FromTicks(DateTime.Now.Ticks - this._StartTicks).TotalSeconds.ToString();
            //EntLibHelper.PerformanceLog(msg);
            System.Diagnostics.Debug.WriteLine(msg);
        }

        #endregion
    }
}
