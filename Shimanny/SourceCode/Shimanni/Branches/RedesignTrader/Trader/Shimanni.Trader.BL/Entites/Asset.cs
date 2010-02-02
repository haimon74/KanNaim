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
        private double _HedgeRation;
        
        
        public Asset():
            base()
        {
            
            
            _Buy = new OrdersBooks(this, eSide.Buy) ;
            _Sell = new OrdersBooks(this,eSide.Sell); 
            _ParentStrategy = new Strategy();
            _MarketData = new LevelOneMarketData(this);
            _HedgeRation = double.MaxValue;
        }
        public Asset(StrategyEntity parentStrategy) :
            base(parentStrategy)
        {
            _Buy = new OrdersBooks(this,eSide.Buy);
            _Sell = new OrdersBooks(this, eSide.Sell); 
            _MarketData = new LevelOneMarketData(this);
            _HedgeRation = double.MaxValue;
        }
        #endregion        
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
                        return 1;
                    default:
                        return double.MaxValue;
                }
            }
            set
            {

                _HedgeRation = value;
            }
        }
        
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
