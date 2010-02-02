using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shimanni.Trader.DataStructure;
using Shimanni.Trader.BL;
using System.ComponentModel;
using Krs.Ats.IBNet;
using Shimanni.Trader.Common;

namespace Shimanni.Trader.BL
{
    public class ShimanniOrder : ShimanniOrderEntity
    {
        #region Constructors
        public ShimanniOrder(int size, double price, eSide side, AssetEntity ParentAsset, int pShimanniOrderID, int pBrokerOrderID):
            base(size, price, side, ParentAsset, pShimanniOrderID, pBrokerOrderID)
        {
            Lock = new object();
        }
        public ShimanniOrder(int size, double price, eSide side, AssetEntity ParentAsset):
            base(size, price, side, ParentAsset)
        {
            Lock =  new object();
        }
        public ShimanniOrder(double price, int size):
            base(price, size)
        {
            Lock = new object();
            _ParentAsset = new Asset();
            _BrokerRoute = _ParentAsset.BrokerRoute;
            _Symbol = _ParentAsset.SymbolInBroker;
        }
        public ShimanniOrder():
            base()
        {
            Lock =  new object();
            _ParentAsset = new Asset();
            _BrokerRoute = _ParentAsset.BrokerRoute;
            _Symbol = _ParentAsset.SymbolInBroker;
        }
        #endregion
        
        #region Properties
        public override eExchangeRoute Route
        {
            get
            {
                if (this.Side == eSide.Sell)
                {
                    if (this.Price > this.ParentAsset.MarketData.BidPrice)
                        return this.ParentAsset.ExchangeRouteForAddingLiquidityOrder; 
                    else
                        return this.ParentAsset.ExchangeRouteForMarketOrder;
                }
                else
                {
                    if (this.Price < this.ParentAsset.MarketData.AskPrice)
                        return this.ParentAsset.ExchangeRouteForAddingLiquidityOrder;
                    else
                        return this.ParentAsset.ExchangeRouteForMarketOrder;
                }
           }
        }


        
        public override eOrderStatus Status
        {
            get
            {
                    return _Status;
            }
            set
            {

                string NewLine =
                            "\tID:\t" + this.ShimanniOrderID.ToString() +
                            "\tSymbol:\t" + this.Symbol +
                            "\tSide:\t" + Side.ToString() +
                            "\tPrice:\t" + Price.ToString() +
                            "\tSize:\t" + SizeAtInitiation.ToString() +
                            "\tTypeOf:\t" + EnumDescConverter.GetEnumDescription(Type) +
                            "\tStatus:\t" + value.ToString();
                if (TraderLog.StreamWriterForTradingLog != null) TraderLog.WriteLineToTradingLog(NewLine);
                if (ParentAsset.ParentStrategy.LogStream != null) ParentAsset.ParentStrategy.LogStream.WriteLineToLog(NewLine);
                
                this.StatusLastTimeChanged = DateTime.Now;
                if ((int)ParentAsset.PreStageTwo == -(int)this.Side)
                {
                    if ((this.Side == eSide.Buy && ParentAsset.Buy.numOfAll == 0) 
                        ||  (this.Side == eSide.Sell && ParentAsset.Sell.numOfAll == 0))
                        ParentAsset.PreStageTwo = eSide.Null;
                }
                switch (value)
                {
                    case eOrderStatus.PendingSubmit:
                        int x = 1;
                        
                        break;
                    
                    case eOrderStatus.Submitted:

                        // because of a bug some times submited get more then once for on oreder - the result 
                        // was that after an order was canceled submited arived and made it a live again. then when tried to cancel there was a "not cancelable" error

                        if (_Status == eOrderStatus.PendingSubmit)  
                        {
                            if (this.Type == eShimanniOrderType.SoftHedging) OrdersManagement.CancelOrder(this);
                            if (this.Type == eShimanniOrderType.AgresiveHedging) OrdersManagement.CancelOrder(this);

                        }
                        else if (_Status == eOrderStatus.PendingCancel)
                        {
                            _Status = eOrderStatus.PendingCancel;
                            if (TraderLog.StreamWriterForTradingLog != null) TraderLog.WriteLineToTradingLog(NewLine);
                            if (ParentAsset.ParentStrategy.LogStream != null) ParentAsset.ParentStrategy.LogStream.WriteLineToLog(NewLine);
                            return;
                        }
                        break;
                    
                    case eOrderStatus.PendingCancel:
                        break;

                    case eOrderStatus.Inactive:

                            RomoveOrderFromLists();

                        break;
                    
                    case eOrderStatus.Canceled :
                        
                        if (this.Type == eShimanniOrderType.SoftHedging)
                        {
                            if (this.Side == eSide.Buy)
                            {
                                ParentAsset.Sell.CancelAll();
                                this.ParentAsset.PreStageTwo = eSide.Buy;
                            }
                            else
                            {
                                ParentAsset.Buy.CancelAll();
                                this.ParentAsset.PreStageTwo = eSide.Sell;
                            }
                        }
                        RomoveOrderFromLists();
                        if (_Status == eOrderStatus.QueingForSubmit)
                        {


                        }
                        else
                        {

                            ParentAsset.ParentStrategy.RunStrategyInNewThread();
                        }

                        break;

                    case eOrderStatus.Filled:
                        RomoveOrderFromLists();
                        break;
                    case eOrderStatus.PreSubmitted:
                        OrdersManagement.CancelOrder(this);

                        break;
                    case eOrderStatus.QueingForCancel:

                        break;
                    case eOrderStatus.QueingForSubmit:

                        break;
                    default:

                        break;

                }

                _Status = value;
                
                StatusLastTimeChanged = DateTime.Now;
                //     WriteOrderToLogFile("_Status" ,EnumDescConverter.GetEnumDescription(_Status));
                
                
            }
        }
        #region Mehtods
        public override void RomoveOrderFromLists()
        {
            
            this.ListMembership.Remove(this);
            if (IBOrderManagement.IBOrderBook.Contains(this)) IBOrderManagement.IBOrderBook.Remove(this);
            if (OrdersManagement.AllOpenOrdres.Contains(this)) OrdersManagement.AllOpenOrdres.Remove(this);
          //  lock (IBOrderManagement.locker)
           // {
                if (IBOrderManagement.QueueOfOrders.Contains(this)) IBOrderManagement.QueueOfOrders.Remove(this);
          //  }
                IBOrderManagement.SumValueOfOpenOrders -= this.DollarValueRemains;
            //todo:    
            if (IBOrderManagement.SumValueOfOpenOrders < 0)
                {
                    int x = 1;
                }

        }
        #endregion


        #endregion
    }
}
