using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Shimanni.Trader.Common;
using System.Diagnostics;

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
        private bool _BidPriceUpdated;
        public bool BidPriceWasUpdated
        {
            get { return _BidPriceUpdated; }
            set { _BidPriceUpdated = value; }
        }

        private bool _AskPriceUpdated;
        public bool AskPriceUpdate
        {
            get { return _AskPriceUpdated; }
            set { _AskPriceUpdated = value; }
        }

        protected AssetEntity _ParentAsset;
        #endregion

        #region Constructors
        public LevelOneMarketDataEntity(AssetEntity ParentAsset)
        {
            _BidPriceUpdated = false;
            _AskPriceUpdated = false;
            _BidPrice = double.MaxValue;
            _BidSize = double.MaxValue;
            _AskPrice = double.MaxValue;
            _AskSize = double.MaxValue;
            _ParentAsset = ParentAsset;
        }
        public LevelOneMarketDataEntity()
        {
            _BidPriceUpdated = false;
            _AskPriceUpdated = false;
            _BidPrice = double.MaxValue;
            _BidSize = double.MaxValue;
            _AskPrice = double.MaxValue;
            _AskSize = double.MaxValue;            

        }
        #endregion

        #region Properties
        [XmlIgnore]
        public double BidPrice
        {
            get
            {
                //Debug.WriteLine("Function: SellHedgingOder: New Order" + ParentAsset.SymbolInBroker.ToString());
                return _BidPrice;
            }
            set
            {

                string line =
                         "_BidPrice:\t" + _BidPrice.ToString() + "\t" +
                         "BidPrice:\t" + value.ToString() + "\t" +
                         "Symbol:\t" + Symbol;
                //TraderLog.WriteLineToTradingLog(line);
                ParentAsset.ParentStrategy.LogStream.WriteLineToLog(line);
                _BidPrice = value;
                BidPriceWasUpdated = true;
            }
        }
        [XmlIgnore]
        public double BidSize
        {
            get
            {
                return _BidSize;
            }
            set
            {
                
                if (BidPriceWasUpdated)
                {
                    BidPriceWasUpdated = false;
                }
                else
                {
                    string line1 =
                         "_BidSize:\t" + _BidSize.ToString() + "\t" +
                         "BidSize:\t" + value.ToString() + "\t" +
                         "Symbol:\t" + Symbol;
                    //string line2 =
                    //     "BidSize:\t" + value.ToString() + "\t" +
                    //     "BidPrice:\t" + BidPrice.ToString() + "\t" +
                    //     "Symbol:\t" + Symbol;
                    
                    TraderLog.WriteLineToTradingLog(line1);
                    ParentAsset.ParentStrategy.LogStream.WriteLineToLog(line1);
                  //  ParentAsset.ParentStrategy.LogStream.WriteLineToLog(line2);
                    
                    if (_BidSize == value)
                    {
                    	int x=1;
                    }
                   
                    
                    _BidSize = value;
                           
                    ParentAsset.ParentStrategy.RunStrategyInNewThread();

                    
                }
            }
        }
        [XmlIgnore]
        public double AskPrice
        {
            get
            {
                return _AskPrice;
            }
            set
            {
                string line =
                         "_AskPrice:\t" + _AskPrice.ToString() + "\t" +
                         "AskPrice:\t" + value.ToString() + "\t" +
                         "Symbol:\t" + Symbol;
                TraderLog.WriteLineToTradingLog(line);
                ParentAsset.ParentStrategy.LogStream.WriteLineToLog(line);

                AskPriceUpdate = true;
                _AskPrice = value;

                
                //if (this.ParentAsset.SymbolInBroker == "MYY")
                //WriteMarketDataToLogFile("_AskPrice", _AskPrice.ToString());
            }
        }
        [XmlIgnore]
        public double AskSize
        {
            get
            {
                return _AskSize;

            }
            set
            {
                if (AskPriceUpdate)
                {
                    AskPriceUpdate = false;
                }
                else
                {
                    
                    string line =
                         "_AskSize:\t" + _AskSize.ToString() + "\t" +
                         "AskSize:\t" + value.ToString() + "\t" +
                         "Symbol:\t" + Symbol;
                    TraderLog.WriteLineToTradingLog(line);
                    ParentAsset.ParentStrategy.LogStream.WriteLineToLog(line);
                    _AskSize = value;
                    ParentAsset.ParentStrategy.RunStrategyInNewThread();
                }
                
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
            if (TraderLog.WriteMarketData && TraderLog.StreamWriterForTradingLog != null)
            {
                string NewLine = DateTime.Now.ToString("hh:mm:ss.ffff") + "\t" +
                        "MarketData" + "\t" + variable + ":" + "\t" + vlaue + "\t" + Symbol;
                TraderLog.StreamWriterForTradingLog.WriteLine(NewLine);
            }
            if (TraderLog.WriteMarketData && TraderLog.StreamWriterForTradingLog != null)
            {
                string NewLine = DateTime.Now.ToString("hh:mm:ss.ffff") + "\t" +
                        "MarketData" + "\t" + variable + ":" + "\t" + vlaue + "\t" + Symbol;
                TraderLog.StreamWriterForTradingLog.WriteLine(NewLine);


            }

            
        }

        
        #endregion
    }
}
