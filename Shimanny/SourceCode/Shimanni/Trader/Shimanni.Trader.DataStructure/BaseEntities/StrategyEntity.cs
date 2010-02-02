using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;
using System.Threading;
using System.Timers;
using System.Collections.Specialized;
using System.IO;
using Krs.Ats.IBNet;

using Shimanni.Common.Utils;
using Shimanni.Common.Data;
using Shimanni.Trader.Common;

namespace Shimanni.Trader.DataStructure
{
    [Serializable()]
    public abstract class StrategyEntity : BaseDataEntity
    {
        #region Data Members
        protected PortfolioEntity _PortfolioParent;
        protected int _StrategyIndexInList;
        protected string _Name;
        protected eTypeOfStrategy _TypeOfStrategy;
        protected eStrategyState _State; // use eStrategyState i.e. closing, opening, not active, MarketMaking
        protected double _MaxMarketMakingExposure;
        protected double _MaxTradeExposure;
        protected double _MaxStrategyExposure;
        protected double _MaxHedgingExposure;
        protected bool _TwoStatgeHedging;
        protected double _Profit;
        protected double _ExcessStrategyExposure;
        protected double _SumValueOfHedgingOrders;
        protected List<ShimanniOrderEntity> _HedgingOrderList;
        protected double _NetEcxessExposure;
        protected double _ExposuresLatitude;
        protected bool _SubscribeToMarketData;
        protected List<AssetEntity> _AssetsList;
        protected object StrategyLocker = new object();
        protected bool _DataIntegrety;
        protected eHedgingState _HedgingState;
        protected double _MinReqProfitFromNewOrder;
        //protected bool _AnOrderWasRegected;
        protected eTypeOfAssetsRelationship _TypeOfAssetsRelationship;
        protected eHedgingRatioMethod _HedgingRatioMethod;
        protected List<ShimanniOrderEntity> OrdersToBeCanceled = new List<ShimanniOrderEntity>();
        protected double _CurrentHeDgingRatio;
        protected SingleStrategyLog _LogStream;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the StrategyEntity class.
        /// </summary>
        /// <param name="strategyIndexInList"></param>
        /// <param name="name"></param>
        /// <param name="state"></param>
        /// <param name="maxMarketMakingExposure"></param>
        /// <param name="maxTradeExposure"></param>
        /// <param name="maxStrategyExposure"></param>
        /// <param name="maxHedgingExposure"></param>
        /// <param name="profit"></param>
        /// <param name="excessStrategyExposure"></param>
        /// <param name="sumValueOfHedgingOrders"></param>
        /// <param name="exposuresLatitude"></param>
        /// <param name="subscribeToMarketData"></param>
        /// <param name="assetsList"></param>
        public StrategyEntity(PortfolioEntity PortfolioParent)
        {
            _StrategyIndexInList = int.MaxValue;
            _Name = "Enter StrategyEntity Name";
            _State = eStrategyState.NotActive;
            _MaxMarketMakingExposure = double.MaxValue;
            _MaxTradeExposure = double.MaxValue;
            _MaxStrategyExposure = double.MaxValue;
            _MaxHedgingExposure = double.MaxValue;
            _Profit = double.MaxValue;
            _ExcessStrategyExposure = double.MaxValue;
            _SumValueOfHedgingOrders = double.MaxValue;
            _HedgingOrderList = new List<ShimanniOrderEntity>();
            _ExposuresLatitude = double.MaxValue;
            _SubscribeToMarketData = false;
            _AssetsList = new List<AssetEntity>();
            _DataIntegrety = false;
           // _AnOrderWasRegected = false;
            _TypeOfAssetsRelationship = eTypeOfAssetsRelationship.Logaritmics;
            _HedgingRatioMethod = eHedgingRatioMethod.BasePrice;
            _MinReqProfitFromNewOrder = double.MaxValue;
            _PortfolioParent = PortfolioParent;
        }
        public StrategyEntity()
        {
            _StrategyIndexInList = int.MaxValue;
            _Name = "Enter StrategyEntity Name";
            _State =  eStrategyState.NotActive;
            _MaxMarketMakingExposure = double.MaxValue;
            _MaxTradeExposure = double.MaxValue;
            _MaxStrategyExposure = double.MaxValue;
            _MaxHedgingExposure = double.MaxValue;
            _Profit = double.MaxValue;
            _ExcessStrategyExposure = double.MaxValue;
            _SumValueOfHedgingOrders = double.MaxValue;
            _HedgingOrderList = new List<ShimanniOrderEntity>();
            _ExposuresLatitude = double.MaxValue;
            _SubscribeToMarketData = false;
            _AssetsList = new List<AssetEntity>();
            _DataIntegrety = false;
         //   _AnOrderWasRegected = false;
            _TypeOfAssetsRelationship = eTypeOfAssetsRelationship.Logaritmics;
            _HedgingRatioMethod = eHedgingRatioMethod.BasePrice;
            _MinReqProfitFromNewOrder = double.MaxValue;
            
            

        }
        #endregion

        #region Propeties
    [XmlIgnore]
        public PortfolioEntity PortfolioParent
        {
            get
            {
                return _PortfolioParent;
            }
            set
            {
                _PortfolioParent = value;
            }
        }
        [XmlIgnore]
        public SingleStrategyLog LogStream
        {
            get { return _LogStream; }
            set { _LogStream = value; }
        }
        [XmlIgnore]
        public double CurrentHeDgingRatio
        {
            get { return _CurrentHeDgingRatio; }
            set { _CurrentHeDgingRatio = value; }
        }
        public double MinReqProfitFromNewOrder
        {
            get { return _MinReqProfitFromNewOrder; }
            set {

                
                _MinReqProfitFromNewOrder = value; }
        }
        public eTypeOfStrategy TypeOfStrategy
        {
            get { return _TypeOfStrategy; }
            set { _TypeOfStrategy = value; }
        }
  //      public abstract bool AnOrderWasRegected { get; set; }
        [XmlIgnore]
        public eHedgingState HedgingState
        {
            get { return _HedgingState; }
            set { _HedgingState = value; }
        }
        public eTypeOfAssetsRelationship TypeOfAssetsRelationship
        {
            get
            {
                return _TypeOfAssetsRelationship;
            }
            set
            {
                _TypeOfAssetsRelationship = value;
            }
        }
        [XmlIgnore]
        public int StrategyIndexInList
        {
            get
            {
                if (PortfolioParent != null)
                    return PortfolioParent.StrategiesList.IndexOf(this);
                else
                    return int.MaxValue;
            }
            set
            {
                
                
                    
            }
        }
        public string Name
        {
            get
            {
                return _Name;

            }
            set
            {
                
                _Name = value;
            }
        }
        public eStrategyState State
        {
            get
            {
                return _State;
            }
            set
            {
                _State = value;
            }
        }
        public double MaxMarketMakingExposure
        {
            get
            {
                return _MaxMarketMakingExposure;
            }
            set
            {
                _MaxMarketMakingExposure = value;
            }
        }
        public double MaxTradeExposure
        {
            get
            {
                return _MaxTradeExposure;
            }
            set
            {
                _MaxTradeExposure = value;
            }
        }
        public double MaxStrategyExposure
        {
            get
            {
                if (State == eStrategyState.MarketMaking)
                {
                    return MaxMarketMakingExposure;
                }
                else
                {
                    return _MaxStrategyExposure;
                }
            }
            set
            {
                _MaxStrategyExposure = value;
            }
        }
        public double MaxHedgingExposure
        {
            get
            {
                return _MaxHedgingExposure;
            }
            set
            {
                _MaxHedgingExposure = value;
            }
        }
        public double ExposuresLatitude
        {
            get
            {
                return _ExposuresLatitude;
            }
            set
            {
                _ExposuresLatitude = value;
            }
        }
        /// <summary>
        /// Allow for two statges hedging. if false only the second stage will be processed 
        /// First stage: send an  oreder to buy or sell without canceling existing orders with a limit that does not confilct
        /// Second Stage: send a psedue-market-order after first canceling all conflicting orders
        /// </summary>
        [XmlIgnore]
        public double Profit
        {
            get
            {
                double sum = 0;
                foreach (AssetEntity asset in this.AssetsList)
                {
                    sum += asset.MarketData.MidPrice * asset.PortfolioPositionCalculated;
                }

                return sum;
            }
            set
            {
                _Profit = value;
            }
        }
        [XmlIgnore]
        public double ExcessStrategyExposure
        {
            get
            {
                double strategyExposure = 0;
                for (int i = 0; i < AssetsList.Count; i++)
                {
                    if (AssetsList[i].PortfolioPositionCalculated == int.MaxValue)
                    {
                        return double.MaxValue;
                    }
                    else
                    {
                        strategyExposure += AssetsList[i].PortfolioPositionCalculated  / AssetsList[i].NormalizationRatio;;
                    }
                }
                return strategyExposure;
            }
        }
        [XmlIgnore]
        public double SumValueOfHedgingOrders
        {
            get
            {
                double sumOf = 0;

                for (int i = 0; i < AssetsList.Count; i++)
                {
                    sumOf += (AssetsList[i].Sell.SumOfHedging - AssetsList[i].Buy.SumOfHedging) / AssetsList[i].NormalizationRatio;
                }

                return sumOf;
            }
        }
        public double NetExcessExposure
        {
            get
            {
                if (ExcessStrategyExposure == int.MaxValue || SumValueOfHedgingOrders == int.MaxValue)

                    return int.MaxValue;
                else
                    return ExcessStrategyExposure - SumValueOfHedgingOrders; 
            }
        }
        public bool SubscribeToMarketData
        {
            get
            {
                return _SubscribeToMarketData;
            }
            set
            {
                _SubscribeToMarketData = value;
            }
        }
        
        //todo: remove it
        public eHedgingRatioMethod HedgingRatioMethod
        {
            get { return _HedgingRatioMethod; }
            set { _HedgingRatioMethod = value; }
        }
        [XmlIgnore]
        public List<ShimanniOrderEntity> HedgingOrderList
        {
            get
            {
                return _HedgingOrderList;
            }
            set
            {
                _HedgingOrderList = value;
            }
        }
        public List<AssetEntity> AssetsList
        {
            get
            {
                return _AssetsList;
            }
            set
            {
                _AssetsList = value;
            }
        }
        public bool DataIntegrety
        {
            get { return _DataIntegrety; }
            set { _DataIntegrety = value; }
        }
 
        #endregion

        #region Methods
        
        
        
        /// <summary>
        /// The methods takes the Price as a double and in the case that the Price is above (bellow) the market best bid (ask)
        /// it round it to be the best bid (ask) on the market(as low (high) as posible) ,
        /// In the case that is bellow (above) the market best offer it will round it down to the closest tick
        /// </summary>
        /// <param name="AssetEntity"></param>
        /// <param name="Price"></param>
        /// <returns></returns>
        protected static double RoundingPriceToMatchMarket(AssetEntity AssetEntity, double Price, eSide Side)
        {
            if ((int)Side == (int)eSide.Buy)
            {
                
                return Math.Round(Math.Min(AssetEntity.MarketData.BidPrice + AssetEntity.MPV, ((int)(Price / AssetEntity.MPV))  * AssetEntity.MPV),2);
            }
            else
            {
                return Math.Round(Math.Max(AssetEntity.MarketData.AskPrice - AssetEntity.MPV, ((int)(Price / AssetEntity.MPV + 1) )* AssetEntity.MPV),2);
            }

        }
        
        #region Orders Methods
        protected abstract void SetHedgingOrder(ref ShimanniOrderEntity newOrder, eSide pSide);
        protected abstract void PlaceHedgingOrders( List<AssetEntity> HedgingAssetBidBased, List<AssetEntity> HedgingAssetAskBased);
        /// <summary>
        /// this methods is called whenever we have a market event or an execution.
        /// Its get pure orders list as a parameter and then cancel and submit orders.
        /// It aims at keeping our que and canceling as little as orders as posible.
        /// </summary>
        /// <param name="BasePrice">The price drerived from the hedging AssetEntity and is at par with hedging AssetEntity price</param>
        /// <param name="AssetEntity"> the AssetEntity that we rearinge its order book</param>
        /// <param name="newOrderList"> a list of pure orders 
        /// entry 0:  MMOrder
        /// entry 1: Closing
        /// enrty 2: Opening</param>
  //      protected abstract void SettingSellingOrderBook(double BasePrice, AssetEntity AssetEntity, List<ShimanniOrderEntity> newOrderList);
    //    protected abstract void SettingBuyingOrderBook(double BasePrice, AssetEntity AssetEntity, List<ShimanniOrderEntity> newOrderList);
        protected abstract void SettingOrderBook(double BasePrice, AssetEntity Asset, List<ShimanniOrderEntity> newOrderList, eSide side);
        /// <summary>
        /// the function take two AssetEntity, one is the soposed hedging AssetEntity and create an order list
        /// of price and sized derived from market data to the "Side" that was requested
        /// </summary>
        /// <param name="AssetEntity"></param>
        /// <param name="HedgingAsset"></param>
        /// <param name="Side"></param>
        /// <returns></returns>
        protected abstract double ReletivePrice(eHedgingRatioMethod Methods, AssetEntity HedgingAsset, AssetEntity AssetEntity, double hedgingAssetPrice);
        protected abstract List<ShimanniOrderEntity> GetBasicsOrders(AssetEntity AssetEntity, AssetEntity HedgingAsset, eSide Side);
        #endregion

        protected abstract void RunStrategy();
        protected abstract bool GetDataIntegrety();

        public abstract void MultiEquetyStrategyChoose2OfManny();
        public abstract void AddStrategy(object sender, TickSizeEventArgs e);        
        public abstract void RunStrategyInNewThread();
        public abstract void printStrategyStateToLog();
        #endregion
    }
}


