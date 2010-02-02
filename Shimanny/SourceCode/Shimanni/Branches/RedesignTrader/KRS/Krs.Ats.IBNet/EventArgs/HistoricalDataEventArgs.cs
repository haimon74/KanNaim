using System;

namespace Krs.Ats.IBNet
{
    /// <summary>
    /// Historical Data Event Arguments
    /// </summary>
    [Serializable()]
    public class HistoricalDataEventArgs : EventArgs
    {
        private readonly double close;
        private readonly int count;
        private readonly DateTime date;
        private readonly bool hasGaps;
        private readonly double high;
        private readonly double low;
        private readonly double open;
        private readonly int requestId;
        private readonly int volume;
        private readonly double wap;
        private readonly int recordNumber;
        private readonly int recordTotal;

        /// <summary>
        /// Full Constructor
        /// </summary>
        /// <param name="requestId">The ticker Id of the request to which this bar is responding.</param>
        /// <param name="date">The date-time stamp of the start of the bar.
        /// The format is determined by the reqHistoricalData() formatDate parameter.</param>
        /// <param name="open">Bar opening price.</param>
        /// <param name="high">High price during the time covered by the bar.</param>
        /// <param name="low">Low price during the time covered by the bar.</param>
        /// <param name="close">Bar closing price.</param>
        /// <param name="volume">Volume during the time covered by the bar.</param>
        /// <param name="count">When TRADES historical data is returned, represents the number of trades that
        /// occurred during the time period the bar covers.</param>
        /// <param name="wap">Weighted average price during the time covered by the bar.</param>
        /// <param name="hasGaps">Whether or not there are gaps in the data.</param>
        /// <param name="recordNumber">Current Record Number out of Record Total.</param>
        /// <param name="recordTotal">Total Records Returned by Historical Request.</param>
        public HistoricalDataEventArgs(int requestId, DateTime date, double open, double high, double low, double close,
                                       int volume, int count, double wap, bool hasGaps, int recordNumber, int recordTotal)
        {
            this.requestId = requestId;
            this.hasGaps = hasGaps;
            this.wap = wap;
            this.count = count;
            this.volume = volume;
            this.close = close;
            this.low = low;
            this.high = high;
            this.open = open;
            this.date = date;
            this.recordNumber = recordNumber;
            this.recordTotal = recordTotal;
        }

        /// <summary>
        /// The ticker Id of the request to which this bar is responding.
        /// </summary>
        public int RequestId
        {
            get { return requestId; }
        }

        /// <summary>
        /// The date-time stamp of the start of the bar.
        /// The format is determined by the reqHistoricalData() formatDate parameter.
        /// </summary>
        public DateTime Date
        {
            get { return date; }
        }

        /// <summary>
        /// Bar opening price.
        /// </summary>
        public double Open
        {
            get { return open; }
        }

        /// <summary>
        /// High price during the time covered by the bar.
        /// </summary>
        public double High
        {
            get { return high; }
        }

        /// <summary>
        /// Low price during the time covered by the bar.
        /// </summary>
        public double Low
        {
            get { return low; }
        }

        /// <summary>
        /// Bar closing price.
        /// </summary>
        public double Close
        {
            get { return close; }
        }

        /// <summary>
        /// Volume during the time covered by the bar.
        /// </summary>
        public int Volume
        {
            get { return volume; }
        }

        /// <summary>
        /// When TRADES historical data is returned, represents the number of trades that
        /// occurred during the time period the bar covers.
        /// </summary>
        public int Count
        {
            get { return count; }
        }

        /// <summary>
        /// Weighted average price during the time covered by the bar.
        /// </summary>
        public double Wap
        {
            get { return wap; }
        }

        /// <summary>
        /// Whether or not there are gaps in the data.
        /// </summary>
        public bool HasGaps
        {
            get { return hasGaps; }
        }

        /// <summary>
        /// Current Record Number out of Record Total
        /// </summary>
        public int RecordNumber
        {
            get { return recordNumber; }
        }

        /// <summary>
        /// Total records returned by query
        /// </summary>
        public int RecordTotal
        {
            get { return recordTotal; }
        } 

    }
}