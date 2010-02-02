using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shimanni.Common.Data;
using System.Xml.Serialization;
using System.Xml;
using System.Threading;

using Shimanni.Common.Utils;
using Shimanni.Trader.DataStructure;
using System.ComponentModel;

namespace Shimanni.Trader.DataStructure
{
    [Serializable()] 
    public abstract class AssetEntity : BaseDataEntity, IComparable
    {           
        #region Data Members
        private string                              _SymbolInDataSource;
        private string                              _SymbolInBroker;
        private eTypeOfAsset                        _TypeOfAsset; // Values are denoted by TypeOfAsset Enum
        private double                              _OpeningSpreadBP;
        private double                              _MMSpreadBP;
        private double                              _ClosingSpreadBP;
        private double                              _HedgingSpreadBP;
        private double                              _StickyPriceLatidudeBP;
        private double                              _StickySizeLatidude;
        private double                              _BasePrice;
        private double                              _MPV;
        private double                              _Multiplayer;
        private double                              _Beta;
        private int                                 _ShortAvailableAtBroker;
        private eDataProvider                       _DataProvider;
        private eBrokerRoute                        _BrokerRoute;
        private eExchangeRoute                      _ExchangeRouteForAddingLiquidityOrder;
        private eExchangeRoute                      _ExchangeRouteForMarketOrder;
        private int                                 _PortfolioPositionBroker;
        private int                                 _PortfolioPositionCalculated;        
        private eSide                       _PreStageTwo;
        private eSide                               _HedgingSide;
        protected OrdersBooksEntity                          _Buy;
        protected OrdersBooksEntity                          _Sell;
        private CombindOrderAndMarketDataList       _CombindList;        
        private int                                 _SumOfExecutions;
        private bool                                _NavWasUpdated; 
        private DateTime                            _DateOfNAV;
        private bool                                _CalcultedPortfolioPostionWasUtpdated;
        private int                                 _PositionAtInitiation;
        private string                              _ExpiryYear;
        private string                              _ExpiryMonth;
        private int                                 _NetSharesAveliabelsForShort;

        protected LevelOneMarketDataEntity _MarketData;        
        protected StrategyEntity                    _ParentStrategy;
        protected readonly List<LevelOneMarketDataEntity> _MarketDataList = new List<LevelOneMarketDataEntity>();
        #endregion                

        #region Contstructors
        public AssetEntity(StrategyEntity ParentStrategy)
        {
            _SymbolInDataSource = "Enter New Symbol";
            _TypeOfAsset = eTypeOfAsset.ChooseType;
            _OpeningSpreadBP = double.MaxValue;
            _MMSpreadBP = double.MaxValue;
            _ClosingSpreadBP = double.MaxValue;
            _HedgingSpreadBP = double.MaxValue;
            _StickyPriceLatidudeBP = double.MaxValue;
            _StickySizeLatidude = double.MaxValue;
            _BasePrice = double.MaxValue;
            _MPV = double.MaxValue; ;
            _Multiplayer = double.MaxValue;
            _Beta = double.MaxValue;
            _ShortAvailableAtBroker = int.MaxValue;
            _DataProvider = eDataProvider.ChooseDataProvider;
            _BrokerRoute = eBrokerRoute.ChooseBroker;
            _ExchangeRouteForAddingLiquidityOrder = eExchangeRoute.ChooseRoute;
            _ExchangeRouteForMarketOrder = eExchangeRoute.ChooseRoute;
            _PortfolioPositionBroker = int.MaxValue;
            _PortfolioPositionCalculated = int.MaxValue;
            _PreStageTwo = eSide.Null;
            _HedgingSide = eSide.Null;
                       
            _CombindList = new CombindOrderAndMarketDataList();
            _ParentStrategy = ParentStrategy;
            _ExpiryYear = null;
            _ExpiryMonth = null;
            _CalcultedPortfolioPostionWasUtpdated = false;
        }
        public AssetEntity()
        {
            _SymbolInDataSource = "Enter New Symbol";
            _TypeOfAsset = eTypeOfAsset.ChooseType;
            _OpeningSpreadBP = double.MaxValue;
            _MMSpreadBP = double.MaxValue;
            _ClosingSpreadBP = double.MaxValue;
            _HedgingSpreadBP = double.MaxValue;
            _MPV = double.MaxValue; ;
            _Multiplayer = double.MaxValue;
            _Beta = double.MaxValue;            
            _ShortAvailableAtBroker = int.MaxValue;
            _DataProvider = eDataProvider.ChooseDataProvider;
            _BrokerRoute = eBrokerRoute.ChooseBroker;
            _ExchangeRouteForAddingLiquidityOrder = eExchangeRoute.ChooseRoute;
            _ExchangeRouteForMarketOrder = eExchangeRoute.ChooseRoute;
            _PortfolioPositionBroker = int.MaxValue;
            _PortfolioPositionCalculated = int.MaxValue;
            _PreStageTwo = eSide.Null;
            _HedgingSide = eSide.Null;
            _CombindList = new CombindOrderAndMarketDataList();            
            _ExpiryYear = null;
            _ExpiryMonth = null;
            _CalcultedPortfolioPostionWasUtpdated = false;

        }
        #endregion

        #region Properties
        public int NetSharesSoldSinceLastUpdatedOfShorts = 0;
        [XmlIgnore]
        public bool CalcultedPortfolioPostionWasUtpdated
        {
            get { return _CalcultedPortfolioPostionWasUtpdated; }
            set { _CalcultedPortfolioPostionWasUtpdated = value; }
        }
        public DateTime DateOfNAV
        {
            get { return _DateOfNAV; }
            set { _DateOfNAV = value; }
        }        
        [XmlIgnore]
        public bool NavWasUpdated
        {
            get { return _NavWasUpdated; }
            set { _NavWasUpdated = value; }
        }
        //todo: why do we need this variable
        [XmlIgnore]
        public int SumOfExecutionsAndStandingOrders
        {
            get { return _SumOfExecutions; }
            set { _SumOfExecutions = value; }
        }        
        public string SymbolInDataSource
        {
            get
            {
                return _SymbolInDataSource;
            }
            set
            {
                if (_SymbolInDataSource == value)
                    return;
                _SymbolInDataSource = value;

               HandleDataRequestWhenAssetChange();
            }
        }
        public string ExpiryYear
        {
            get
            {
                return _ExpiryYear;
            }
            set
            {
                if (_ExpiryYear == value)
                    return;
                _ExpiryYear = value;

                HandleDataRequestWhenAssetChange();
            }
        }
        public string ExpiryMonth
        {
                get
                {
                    return _ExpiryMonth;
                }
                set
            {
                if (_ExpiryMonth == value)
                    return;
                _ExpiryMonth = value;

                HandleDataRequestWhenAssetChange();
            }
        }
        //The code bellow comes to insure that every time that we change a symbol of an AssetEntity we also request data to 
        // the new symbol and gurenty that we took it of the list       
        public string SymbolInBroker
        {
            get
            {
                return _SymbolInBroker;
            }
            set
            {
                _SymbolInBroker = value;
            }
        }
        public eTypeOfAsset TypeOfAsset
        {
            get
            {
                return _TypeOfAsset;
            }
            set
            {
                _TypeOfAsset = value;
            }
        }
        [XmlIgnore]
        public double OpeningSpread
        {
            get
            {
                return _OpeningSpreadBP * 0.0001;
            }
        }
        [XmlIgnore]
        public double MMSpread
        {
            get
            {
                return _MMSpreadBP*0.0001;
            }
        }
        [XmlIgnore]
        public double ClosingSpread
        {
            get
            {
                return _ClosingSpreadBP * 0.0001;
            }
        }
        [XmlIgnore]
        public double HedgingSpread
        {
            get
            {
                return ((int)((_HedgingSpreadBP * 0.0001 * MarketData.MidPrice ) / MPV)) * MPV;
            }
        }
        [XmlIgnore]
        public double StickyPriceLatidude
        {
            get
            {
                return _StickyPriceLatidudeBP*0.0001;
            }
        }
        [XmlIgnore]
        public double StickySizeLatidude
        {
            get
            {
                return ParentStrategy.ExposuresLatitude * Math.Abs(this.Beta) / (this.MarketData.MidPrice * this.Multiplayer); 
            }

        }
        public double BasePrice
        {
            get
            {
                return _BasePrice;
            }
            set
            {
                _BasePrice = value;
            }
        }
        public double MPV
        {
            get
            {
                return _MPV;
            }
            set
            {
                _MPV = value;
            }
        }
        public double Multiplayer
        {
            get
            {
                return _Multiplayer;
            }
            set
            {
                _Multiplayer = value;
            }
        }
        public double Beta
        {
            get
            {
                return _Beta;
            }
            set
            {
                _Beta = value;
            }
        }
        /// <summary>
        /// Short avalible at the broker at the beging of trading 
        /// </summary>
        public int ShortAvaliableAtBroker
        {
            get
            {
                if (TypeOfAsset == eTypeOfAsset.Future)
                    return int.MaxValue;
                else
                    return _ShortAvailableAtBroker;
            }
            set
            {
                _ShortAvailableAtBroker = value;
            }
        }
        public int PortfolioPositionAtInitiation
        {
            get { return _PositionAtInitiation; }
            set { _PositionAtInitiation = value; }
        }
        /// <summary>
        /// If our postion at the beging is let say +100 and shorts avlible (at IB sight) are 100 it means that our shares create the short 
        /// "bank" and therefor when we will sell our share avilbilty will go to zero (if sold to another broker) 
        /// </summary>        
        public int NetSharesAveliabelsForShort
        {
            
            get { return Math.Max(ShortAvaliableAtBroker - Math.Max(PortfolioPositionCalculated,0),0); }
             
        }        
        public int NetSharesAvelibleForSell
        {
            get { 
                    
                    int var =    NetSharesAveliabelsForShort+Math.Max(PortfolioPositionCalculated,0) - Buy.sumofAll;
                    if (var < 0)
                    {
                        

                    }
                    return Math.Max(var,0);
                }

            //todo: there is a need to change sum of all sell orders to only of those that  were taken from the short balnce using the sumof shrot since last short update varibale
        }
        public eDataProvider DataProvider
        {
            get
            {
                return _DataProvider;
            }
            set
            {
                _DataProvider = value;
            }
        }
        public eBrokerRoute BrokerRoute
        {
            get
            {
                return _BrokerRoute;
            }
            set
            {
                _BrokerRoute = value;
            }
        }
        public abstract double NormalizationRatio {get;set;}
       
        public eExchangeRoute ExchangeRouteForAddingLiquidityOrder
{
		get
		{
				return _ExchangeRouteForAddingLiquidityOrder;
		}
		set
		{
				_ExchangeRouteForAddingLiquidityOrder = value;
		}
}
        public eExchangeRoute ExchangeRouteForMarketOrder
        {
            get
            {
                return _ExchangeRouteForMarketOrder;
            }
            set
            {
                _ExchangeRouteForMarketOrder = value;
            }
        }
        [XmlIgnore]
        public int PortfolioPositionAtBroker
        {
            get
            {
                return _PortfolioPositionBroker;
            }
            set
            {
                _PortfolioPositionBroker = value;
            }
        }
        [XmlIgnore]
        public int PortfolioPositionCalculated
        {
            get
            {
                return _PortfolioPositionCalculated;
            }
            set
            {
                _PortfolioPositionCalculated = value;
                ParentStrategy.RunStrategyInNewThread();
                //todo: To ask Yossi: I call run strategy which use PortfolioPositionCalculated. Is it good or legal?
            }
        }
        public double OpeningSpreadBP
        {
            get
            {
                return _OpeningSpreadBP;
            }
            set
            {
                _OpeningSpreadBP = value;
            }
        }
        public double MMSpreadBP
        {
            get
            {
                return _MMSpreadBP ;
            }
            set
            {
                _MMSpreadBP = value;
            }
        }
        public double ClosingSpreadBP
        {
            get
            {
                return _ClosingSpreadBP;
            }
            set
            {
                _ClosingSpreadBP = value;
            }
        }
        public double HedgingSpreadBP
        {
            get
            {
                return _HedgingSpreadBP;
            }
            set
            {
                _HedgingSpreadBP = value;
            }
        }
        public double StickyPriceLatidudeBP
        {
            get
            {
                return _StickyPriceLatidudeBP;
            }
            set
            {
                _StickyPriceLatidudeBP = value;
            }
        }
        //public double StickySizeLatidude1
        //{
        //    get
        //    {
        //        return _StickySizeLatidude;
        //    }
        //    set
        //    {
        //        _StickySizeLatidude = value ;
        //    }
        //}
        public string ExpiryDate
        {
            get
            {
                return  ExpiryYear + ExpiryMonth;
            }
        }
        /// <summary>
        /// An indicator either StrategyEntity is in PreStageTwo. 
        /// Can get the values of eSide (Buy Sell or Null)
        /// </summary>
        [XmlIgnore] 
        public eSide PreStageTwo
        {
            get
            {
                return _PreStageTwo;
            }
            set
            {
                _PreStageTwo = value;
            }
        }
        [XmlIgnore] 
        public eSide HedgingSide
        {
            get
            {
                if (Sell.Hedging.Count > 0)
                    return eSide.Sell;
                else if (Buy.Hedging.Count > 0)
                    return eSide.Buy;
                else
                    return eSide.Null;
            }
        }        /// <summary>
        /// Convert  MaxTradeExpusure field in the StrategyEntity class to the equivalents in AssetEntity units:  
        /// Units Exposure = Dollar Value Exposure * Beta / (price * Multiplayer)
        /// </summary>
        [XmlIgnore] 
        public int MaxTradeExpusureUnits
        {
            get { return CommonUtils.DoubleToIntGTOrETZero(ParentStrategy.MaxTradeExposure * Math.Abs(this.Beta) / (this.MarketData.MidPrice * this.Multiplayer)); }
        }
        /// <summary>
        /// Convert  MaxMarketMakingExposure field in the StrategyEntity class to the equivalents in AssetEntity units:  
        /// Units Exposure = Dollar Value Exposure * Beta / (price * Multiplayer)
        /// </summary>
        [XmlIgnore] 
        public double MaxMarketMakingExposureUnits
        {
            get { return ParentStrategy.MaxMarketMakingExposure * Math.Abs(this.Beta) / (this.MarketData.MidPrice * this.Multiplayer); }
        }
        /// <summary>
        /// Convert  MaxStrategyExposure field in the StrategyEntity class to the equivalents in AssetEntity units:  
        /// Units Exposure = Dollar Value Exposure * Beta / (price * Multiplayer)
        /// </summary>
        [XmlIgnore] 
        public double MaxStrategyExposureUnits
        {
            get { return ParentStrategy.MaxStrategyExposure * Math.Abs(this.Beta) / (this.MarketData.MidPrice * this.Multiplayer); }
        }
        [XmlIgnore] 
        public double SignOfBeta
        {
            get { return Math.Sign(this.Beta); }
        }
        public LevelOneMarketDataEntity MarketData
        {
            get
            {
                return _MarketData;
            }
            set
            {
                _MarketData = value;
            }
        }
        [XmlIgnore]
        public StrategyEntity ParentStrategy
        {
            get
            {
                return _ParentStrategy;
            }
            set
            {
                _ParentStrategy = value;
            }
        }
        [XmlIgnore]
        public List<LevelOneMarketDataEntity> MarketDataList
        {
            get
            {
                return _MarketDataList;
            }
        }
        [XmlIgnore] 
        public OrdersBooksEntity Buy
        {
            get
            {
                return _Buy;
            }
            set
            {
                _Buy = value;
            }
        }
        public OrdersBooksEntity Sell
        {
            get
            {
                return _Sell;
            }
            set
            {
                _Sell = value;
            }
        }
        public CombindOrderAndMarketDataList CombindList
        {
            get
            {
                return _CombindList;
            }
            set
            {
                _CombindList = value;
            }
        }
        [XmlIgnore]
        private double BrokerComission
        {
            get
            {
                switch (BrokerRoute)
                {
                    case eBrokerRoute.InterActivBroker:
                        return 0.03;
                    default:
                        return double.MaxValue;
                }
            }

        }
        [XmlIgnore]
        public double AddingLiquidityTotalFees
        {
            get 
            {
                if (BrokerComission == double.MaxValue)
                { 
                    return double.MaxValue;
                }
                else
                {
                    double total = BrokerComission;
                    switch (ExchangeRouteForAddingLiquidityOrder)
                    {
                        case eExchangeRoute.SMART:
                            return total += 0.2;
                        default:
                            return double.MaxValue;
                    }    
                }
                
            }
        }
        [XmlIgnore]
        public double TakeingLiqtuidityTotalFees
        {
            get
            {
                if (BrokerComission == double.MaxValue)
                {
                    return double.MaxValue;
                }
                else
                {
                    double total = BrokerComission;
                    switch (ExchangeRouteForAddingLiquidityOrder)
                    {
                        case eExchangeRoute.SMART:
                            return total += 0.3;
                        default:
                            return double.MaxValue;
                    }
                }

            }
        }
        
        [XmlIgnore]
        
        
        public double EffectiveAskPrice
        {
            get
            {
                return MarketData.AskPrice + TakeingLiqtuidityTotalFees;
            }
        }
        [XmlIgnore]
        public double EffectiveBidPrice
        {
            get
            {
                return MarketData.BidPrice - TakeingLiqtuidityTotalFees;
            }
        }
        [XmlIgnore]
        //todo: deal with it to incluse are we best ask
        public int EffectiveAskSize
        {
            get
            {
                return (int)Math.Max(MarketData.AskSize - Buy.SumOfHedging,0); 
            }
        }
        [XmlIgnore]
        public int EffectiveBidSize
        {
            get
            {
                return (int)Math.Max(Math.Min(MarketData.BidSize,NetSharesAvelibleForSell),0);
            }
        }
        [XmlIgnore]
        public double AskBasedPriceDeviation
        {
        	get 
            {	
                if (Beta < 0)
                    return (EffectiveBidPrice / BasePrice - 1) * Beta;
                else
                    return (EffectiveAskPrice / BasePrice - 1) * Beta;
            }
        }
        public bool BestBid
        {
            get
            {
                if (Buy.Hedging.Count > 0)
                    return true;
                else if (Buy.Ordinery.Count > 0 && MarketData.BidPrice < Buy.Ordinery[0].Price)
                    return true;
                else
                    return false;
            }
        }
        public bool BestAsk
        {
            get
            {
                if (Sell.Hedging.Count > 0)
                    return true;
                else if (Sell.Ordinery.Count > 0 && MarketData.AskPrice > Sell.Ordinery[0].Price)
                    return true;
                else
                    return false;
            }
        }
        [XmlIgnore]
        public double BidBasedPriceDeviation
        {
        	get 
            {	
                if (Beta > 0)
                    return (EffectiveBidPrice / BasePrice - 1) * Beta;
                else
                    return (EffectiveAskPrice / BasePrice - 1) * Beta;
            }
        }
        [XmlIgnore]
        public double EffectiveAskSizeNormolized
        {
            get
            {
                return Math.Abs(EffectiveAskSize * NormalizationRatio);
                
            }
        }
        [XmlIgnore]
        public double EffectiveBidSizeNormolized
        {
            get
            {
                return Math.Abs(EffectiveBidSize * NormalizationRatio);
            }
        }
        
        /// <summary>
        /// for negative Beta takes EffectiveAskPrice and for  posetive beta it takes EffectiveBidPrice
        /// </summary>
        public double InverseBidPrice
        {
            get
            {
                if (Beta > 0)
                    return EffectiveBidPrice;
                else
                    return EffectiveAskPrice;
            }
        }
        /// <summary>
        /// For negative Beta takes EffectiveBidPrice and for  posetive beta it takes EffectiveAskPrice
        /// </summary>
        public double InverseAskPrice
        {
            get
            {
                if (Beta > 0) 
                    return EffectiveAskPrice;
                else
                    return EffectiveBidPrice;
            }
        }
        /// <summary>
        /// for negative Beta takes EffectiveAskSize and for  posetive beta it takes EffectiveBidSize
        /// </summary>
        public double InverseBidSize
        {
            get
            {
                if (Beta > 0)
                    return EffectiveBidSize;
                else
                    return EffectiveAskSize;
            }
        }
        /// <summary>
        /// For negative Beta takes EffectiveBidSize and for  posetive beta it takes EffectiveAskSize
        /// </summary>
        public double InverseAskSize
        {
            get
            {
                if (Beta > 0)
                    return EffectiveAskSize;
                else
                    return EffectiveBidSize;
            }
        }
        /// <summary>
        /// for negative Beta takes EffectiveAskSizeNormolized and for  posetive beta it takes EffectiveBidSizeNormolized
        /// </summary>
        public double InverseBidSizeNormolized
        {
            get
            {
                if (Beta > 0)
                    return EffectiveBidSizeNormolized;
                else
                    return EffectiveAskSizeNormolized;
            }
        }
        /// <summary>
        /// For negative Beta takes EffectiveBidSizeNormolized and for  posetive beta it takes EffectiveAskSizeNormolized
        /// </summary>
        public double InverseAskSizeNormolized
        {
            get
            {
                if (Beta > 0)
                    return EffectiveAskSizeNormolized;
                else
                    return EffectiveBidSizeNormolized;
            }
        }




        #endregion

        
        #region Methods
        protected abstract void HandleDataRequestWhenAssetChange();
        #endregion

        #region IComparable

        //private class BestBidComparison: IComparer<AssetEntity>
        //{
        //    int IComparer.Compare(object a, object b)
        //    {
        //        if (((AssetEntity)a).BidBasedPriceDeviation > ((AssetEntity)a).BidBasedPriceDeviation)
        //            return 1;
        //        else if (((AssetEntity)a).BidBasedPriceDeviation > ((AssetEntity)a).BidBasedPriceDeviation)
        //            return -1;
        //        else
        //            return 0;

        //    }

        //}

        private static int SizeComparer(AssetEntity asset1, AssetEntity asset2, int sign)
        {
            if (asset1.Beta < 0 && asset2.Beta < 0)
            {
                if (asset1.EffectiveAskSizeNormolized > asset2.EffectiveAskSizeNormolized)
                    return 1 * sign;
                else if (asset1.EffectiveAskSizeNormolized < asset2.EffectiveAskSizeNormolized)
                    return -1 * sign;
                else
                    return 0;
            }
            else if ((asset1.Beta > 0 && asset2.Beta > 0))
            {
                if (asset1.EffectiveBidSizeNormolized > asset2.EffectiveBidSizeNormolized)
                    return 1 * sign;
                else if (asset1.EffectiveBidSizeNormolized < asset2.EffectiveBidSizeNormolized)
                    return -1 * sign;
                else
                    return 0;
            }
            else if (asset1.Beta < 0 && asset2.Beta > 0)
            {
                if (asset1.EffectiveAskSizeNormolized > asset2.EffectiveBidSizeNormolized)
                    return 1 * sign;
                else if (asset1.EffectiveAskSizeNormolized < asset2.EffectiveBidSizeNormolized)
                    return -1 * sign;
                else
                    return 0;
            }
            else if (asset1.Beta > 0 && asset2.Beta < 0)
            {
                if (asset1.EffectiveBidSizeNormolized > asset2.EffectiveAskSizeNormolized)
                    return 1 * sign;
                else if (asset1.EffectiveBidSizeNormolized < asset2.EffectiveAskSizeNormolized)
                    return -1 * sign;
                else
                    return 0;
            }
            else
                return 0;
        }
        /// <summary>
        /// The highest the rating the more desire the AssetEntity based first on price and then on size
        /// </summary>
        /// <param name="asset1"></param>
        /// <param name="asset2"></param>
        /// <returns></returns>
        private static int BidBasedComparer(AssetEntity asset1, AssetEntity asset2)
        {
            int sign = -1;
            //todo: to change the size based on the inverse size normolized
            if ((asset1.BidBasedPriceDeviation > asset2.BidBasedPriceDeviation))
                return +1 * sign;
            else if (asset1.BidBasedPriceDeviation < asset2.BidBasedPriceDeviation)
                return -1 * sign;
            else
                return SizeComparer(asset1, asset2, sign);
        }
        /// <summary>
        /// The highest the rating the more desire the AssetEntity based first on price and then on size
        /// </summary>
        /// <param name="asset1"></param>
        /// <param name="asset2"></param>
        /// <returns></returns>
        private static int AskBasedPriceDeviationComparer(AssetEntity asset1, AssetEntity asset2)
        {
            int sign = -1;

            if (asset1.AskBasedPriceDeviation < asset2.AskBasedPriceDeviation)
                return +1 * sign;
            else if (asset1.AskBasedPriceDeviation > asset2.AskBasedPriceDeviation)
                return -1 * sign;
            else
                return SizeComparer(asset1, asset2, sign);
        }
        
        
        
        
        public static Comparison<AssetEntity> BestBidComparison =
       
        delegate(AssetEntity asset1, AssetEntity asset2)
            {
                return BidBasedComparer(asset1,asset2);
            };


        public static Comparison<AssetEntity> BestAskComparison =

        delegate(AssetEntity asset1, AssetEntity asset2)
        {
            return AskBasedPriceDeviationComparer(asset1, asset2);
        };

        public int CompareTo(object other)
        {
            return SymbolInDataSource.CompareTo(((AssetEntity)other).SymbolInDataSource);
        }


        #endregion
    }  
       
}    
