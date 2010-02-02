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
        }
        public ShimanniOrder(int size, double price, eSide side, AssetEntity ParentAsset):
            base(size, price, side, ParentAsset)
        {            
        }
        public ShimanniOrder(double price, int size):
            base(price, size)
        {
            _ParentAsset = new Asset();
            _BrokerRoute = _ParentAsset.BrokerRoute;
            _Symbol = _ParentAsset.SymbolInBroker;
        }
        public ShimanniOrder():
            base()
        {
            _ParentAsset = new Asset();
            _BrokerRoute = _ParentAsset.BrokerRoute;
            _Symbol = _ParentAsset.SymbolInBroker;
        }
        #endregion
        
        #region Properties
        public override eOrderStatus Status
        {
            get
            {
                return _Status;
            }
            set
            {

                StrategyEntity StrategyEntity = ParentAsset.ParentStrategy;
                this.StatusLastTimeChanged = DateTime.Now;


                switch (value)
                {
                    case eOrderStatus.PendingSubmit:

                        OrdersManagement.PendngSubmitOrdres.Add(this);
                        
                        break;
                    
                    case eOrderStatus.Submitted:

                        if (this.Type == eShimanniOrderType.SoftHedging) OrdersManagement.CancelOrder(this);
                        break;
                    
                    case eOrderStatus.PendingCancel:
                        break;

                    case eOrderStatus.Inactive:

                        if (OrdersManagement.PendngSubmitOrdres.Contains(this)) OrdersManagement.PendngSubmitOrdres.Remove(this);
                        RomoveOrderFromLists();

                        break;
                    
                    case eOrderStatus.Canceled :
                        
                        if (_Status == eOrderStatus.PendingCancel && this.Type == eShimanniOrderType.SoftHedging)
                        {
                            if (this.Side == eSide.Buy)
                            {
                                OrdersManagement.CancelOrderList(this.ParentAsset.Sell.Ordinery);
                                this.ParentAsset.PreStageTwo = eSide.Buy;
                            }
                            else
                            {
                                OrdersManagement.CancelOrderList(this.ParentAsset.Buy.Ordinery);
                                this.ParentAsset.PreStageTwo = eSide.Sell;
                            }
                        }
                        RomoveOrderFromLists();
                        break;

                    case eOrderStatus.Filled:

                        RomoveOrderFromLists();
                        if (OrdersManagement.PendngSubmitOrdres.Contains(this)) OrdersManagement.PendngSubmitOrdres.Remove(this);


                        break;
                }
                
                

                


                _Status = value;
                //     WriteOrderToLogFile("_Status" ,EnumDescConverter.GetEnumDescription(_Status));
                if (TraderLog.StreamWriterForTradingLog != null)
                {
                    string NewLine = DateTime.Now.ToString("hh:mm:ss.ffff") + "\t" +
                            "\tID:\t" + this.ShimanniOrderID.ToString() +
                            "\tSymbol:\t" + this.Symbol +
                            "\tSide:\t" + Side.ToString() +
                            "\tPrice:\t" + Price.ToString() +
                            "\tSize:\t" + Size.ToString() +
                            "\tTypeOf:\t" + EnumDescConverter.GetEnumDescription(Type) +
                            "\tStatus:\t" + value.ToString();

                    TraderLog.StreamWriterForTradingLog.WriteLine(NewLine);
                    TraderLog.StreamWriterForTradingLog.Flush();
                }
                if (value == eOrderStatus.PendingSubmit || value == eOrderStatus.PendingCancel || value == eOrderStatus.QueingForSubmit || value == eOrderStatus.QueingForCancel)
                {
                    // StrategyEntity.RunStrategyInNewThread();
                }
            }
        }

        protected override void RomoveOrderFromLists()
        {
            this.ListMembership.Remove(this);
            if (IBOrderManagement.CancelPendingList.Contains(this)) IBOrderManagement.CancelPendingList.Remove(this);
            
            IBOrderManagement.SumValueOfOpenOrders -= this.Remains;
            
        }
        #endregion
    }
}
