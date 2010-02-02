using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Shimanni.Trader.DataStructure
{
    public delegate void MarketDataChangeDelegate(object sender, EventArgs e);

    [Serializable()]
    public class LevelOneMarketDataEntity
    {
        #region Data Members        
        private double _BidPrice;
        private double _BidSize;
        private double _AskPrice;
        private double _AskSize;

        protected AssetEntity _ParentAsset;
        #endregion

        #region Constructors
        public LevelOneMarketDataEntity(AssetEntity ParentAsset)
        {
            _BidPrice = double.MaxValue;
            _BidSize = double.MaxValue;
            _AskPrice = double.MaxValue;
            _AskSize = double.MaxValue;
            _ParentAsset = ParentAsset;
        }
        public LevelOneMarketDataEntity()
        {
            _BidPrice = double.MaxValue;
            _BidSize = double.MaxValue;
            _AskPrice = double.MaxValue;
            _AskSize = double.MaxValue;            

        }
        #endregion

        #region Properties
        public double BidPrice
        {
            get
            {
                return _BidPrice;
            }
            set
            {
                _BidPrice = value;
                WriteMarketDataToLogFile("_BidPrice" + "\t" + Symbol, _BidPrice.ToString());
            }
        }
        public double BidSize
        {
            get
            {
                return _BidSize;
            }
            set
            {
                _BidSize = value;
                WriteMarketDataToLogFile("_BidSize" + "\t" + Symbol, _BidSize.ToString());
                ParentAsset.ParentStrategy.RunStrategyInNewThread();
            }
        }
        public double AskPrice
        {
            get
            {
                return _AskPrice;
            }
            set
            {
                _AskPrice = value;
                WriteMarketDataToLogFile("_AskPrice", _AskPrice.ToString());
            }
        }
        public double AskSize
        {
            get
            {
                return _AskSize;

            }
            set
            {

                _AskSize = value;
                WriteMarketDataToLogFile("_AskSize", _AskSize.ToString());
                ParentAsset.ParentStrategy.RunStrategyInNewThread();
            }
        }
        public string Symbol
        {
            get
            {
                return ParentAsset.SymbolInDataSource;
            }
        }
        public double MidPrice
        {
            get
            {
                return (BidPrice + AskPrice) * 0.5;
            }
        }
        public double HalfBidAskSpread
        {
            get { return (AskPrice - BidPrice) * 0.5; }
        }
        [XmlIgnore]
        public AssetEntity ParentAsset
        {
            get
            {
                return _ParentAsset;
            }
            set
            {
                _ParentAsset = value;
            }
        }
        #endregion

        #region Events
        public event MarketDataChangeDelegate MarketDataChanged;
        #endregion

        #region Methods
        private void FireChanged()
        {
            if (MarketDataChanged != null)
            {
                EventArgs e = new EventArgs();
                MarketDataChanged(this, e);
            }
        }
        private void WriteMarketDataToLogFile(string variable, string vlaue)
        {
            //if (TraderLog.SW != null)
            //{
            //    string NewLine = DateTime.Now.ToString("hh:mm:ss.ffff") + "\t" +
            //            "MarketData" + "\t" + variable + ":" + "\t" + vlaue + "\t" + Symbol;
            //    TraderLog.SW.WriteLine(NewLine);
            //}
        }
        #endregion
    }
}
