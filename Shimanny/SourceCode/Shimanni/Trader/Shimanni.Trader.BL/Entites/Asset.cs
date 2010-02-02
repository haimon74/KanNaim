using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shimanni.Trader.DataStructure;
using System.Xml.Serialization;
using Shimanni.Trader.BL.Entites;
using Shimanni.Trader.BL;

namespace Shimanni.Trader.BL
{
    [Serializable()]
    public class Asset : AssetEntity
    {
        #region Constructors
        private double _ConstantHedgeRatio;
        
        public Asset():
            base()
        {
            
            
            _Buy = new OrdersBooks(this, eSide.Buy) ;
            _Sell = new OrdersBooks(this,eSide.Sell);
            _ParentStrategy = new Strategy();
            _MarketData = new LevelOneMarketData(this);
            _ConstantHedgeRatio = double.MaxValue;
        }
        public Asset(StrategyEntity parentStrategy) :
            base(parentStrategy)
        {
            _Buy = new OrdersBooks(this,eSide.Buy);
            _Sell = new OrdersBooks(this, eSide.Sell); 
            _MarketData = new LevelOneMarketData(this);
            _ConstantHedgeRatio = double.MaxValue;
        }
        #endregion        
        
        #region Propeties
        [XmlIgnore]
        public override int ShortAvaliableAtBroker
        {
            get
            {
                if (TypeOfAsset == eTypeOfAsset.Future)
                    return int.MaxValue-1;
                else
                    return _ShortAvailableAtBroker;
            }
            set
            {
                if (SymbolInBroker == Portfolio.Instance.DebugedSymbol)
                {
                    int x = 1;
                }
                _ShortAvailableAtBroker = value;
            }
        }
        [XmlIgnore]
        public override int PortfolioPositionAtLastShortUpdate
        {
            get
            {
             //   if (_PortfolioPositionAtLastShortUpdate = int.MaxValue && IBConectivityManagement.PositionsWereUpdated)
                    
                    return _PortfolioPositionAtLastShortUpdate;
            }
            set
            {
                _PortfolioPositionAtLastShortUpdate = value;
            }
        }

        [XmlIgnore]
        public override double NormalizationRatio 
        {
            
            get
            {
                switch (ParentStrategy.HedgingRatioMethod)
                {
                    case eHedgingRatioMethod.BasePrice:
                        return Beta / (BasePrice * Multiplayer);
                    case eHedgingRatioMethod.ContiniesPrice:
                        return Beta / (MarketData.MidPrice * Multiplayer);
                    case eHedgingRatioMethod.ConstantRatio:
                        return _ConstantHedgeRatio;
                    default:
                        return double.MaxValue;
                }
            }
            set
            {

                _ConstantHedgeRatio = value;
            }
        }
        #endregion

        #region Methods
        protected override void HandleDataRequestWhenAssetChange()
        {
            if (IBMarketDataManagement.DataSubscribtionList.ContainsValue(this))
            {

                int placeInList = IBMarketDataManagement.DataSubscribtionList.Values.IndexOf(this);
                IBConectivityManagement.Client.CancelMarketData(placeInList);
                IBMarketDataManagement.DataSubscribtionList.Values.Remove(this);
                IBMarketDataManagement.UpdateMarketDataSubscriptionList();
            }
        }
        #endregion        
    }
}
