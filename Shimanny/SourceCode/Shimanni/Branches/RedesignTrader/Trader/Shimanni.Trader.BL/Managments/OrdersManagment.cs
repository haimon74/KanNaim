using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shimanni.Trader.DataStructure;

namespace Shimanni.Trader.BL
{
    public static class OrdersManagement
    {
        #region Data Members
        private static int                            _ShimanniOrderID = 10000;
        private static SortedList<int, ShimanniOrderEntity> _ShimanniOrderList;
        public static List<ShimanniOrder> PendngSubmitOrdres = new List<ShimanniOrder>();
        public static double SumOfPendingOrdersValue
        {
             get
             {
                 double sum = 0;
                 foreach (ShimanniOrder order in PendngSubmitOrdres)
                 {
                     sum += order.Size * order.Price;
                 }
                 return sum;
              }
        }
        #endregion

        #region Methods
        
        public static void PlaceOrder(ShimanniOrderEntity pOrder)
        {
            ShimanniOrderID += 1;
            pOrder.ShimanniOrderID = ShimanniOrderID;
            AssetEntity AssetEntity = pOrder.ParentAsset;
            pOrder.Remains = (int)pOrder.Size;
            bool HedgingOrder = (pOrder.Type == eShimanniOrderType.AgresiveHedging ||   pOrder.Type == eShimanniOrderType.SoftHedging);
            // ajusting the price to bid and ask of the market
            
            if (pOrder.Side == eSide.Buy)
                pOrder.Price =  Math.Round(Math.Min(AssetEntity.MarketData.BidPrice + AssetEntity.MPV, ((int)(pOrder.Price / AssetEntity.MPV)) * AssetEntity.MPV), 2);
            else
                pOrder.Price = Math.Round(Math.Max(AssetEntity.MarketData.AskPrice - AssetEntity.MPV, ((int)(pOrder.Price / AssetEntity.MPV + 1)) * AssetEntity.MPV), 2);

            // assinging to the right lists
            
            if (pOrder.Side == eSide.Buy)
            {
                if (HedgingOrder)
                {
                    AssetEntity.Buy.Hedging.Add(pOrder);
                    pOrder.ListMembership = AssetEntity.Buy.Hedging; ;
                }
                else
                {
                    AssetEntity.Buy.Ordinery.Add(pOrder);
                    pOrder.ListMembership = AssetEntity.Buy.Ordinery;
                }
            }
            else
            {
                if (HedgingOrder)
                {
                    AssetEntity.Sell.Hedging.Add(pOrder);
                    pOrder.ListMembership = AssetEntity.Sell.Hedging; ;
                }
                else
                {
                    AssetEntity.Sell.Ordinery.Add(pOrder);
                    pOrder.ListMembership = AssetEntity.Sell.Ordinery;
                }
            }


            switch (AssetEntity.BrokerRoute)
            {
                case eBrokerRoute.InterActivBroker:
                    if (IBConectivityManagement.Client != null)
                    {
                        IBOrderManagement.PlaceOrderInQueue(pOrder);
                    }
                    break;
                default:    
                    break;               
            }

        }
        public static void CancelOrder(ShimanniOrderEntity pOrder)
        {
            if (pOrder.Status != eOrderStatus.PendingCancel && pOrder.Status != eOrderStatus.QueingForCancel && pOrder.Status != eOrderStatus.Canceled)
            {
                if (pOrder.ListMembership.Contains(pOrder))
                {

                    pOrder.NumberOfTimesCancelRequested += 1;
                    pOrder.StatusLastTimeChanged = DateTime.Now;

                    switch (pOrder.ParentAsset.BrokerRoute)
                    {
                        case eBrokerRoute.InterActivBroker:
                            IBOrderManagement.PlaceOrderCancelationInQueue(pOrder);
                            break;                        
                    }
                }
            }
        }
        public static void CancelOrderList(List<ShimanniOrderEntity> CancelList)
        {
            foreach (ShimanniOrderEntity order in CancelList)
            {
                OrdersManagement.CancelOrder(order);
            }
        }
        public static double ConvertOrderToExchangeComission(Asset asset, bool TakingLiquidity)
        {
            if (TakingLiquidity)
                switch (asset.ExchangeRouteForAddingLiquidityOrder)
                {
                    case eExchangeRoute.SMART:
                        return 0.3;
                    default:
                        return 0;
                }
            else
            {
                switch (asset.ExchangeRouteForMarketOrder)
                {
                    case eExchangeRoute.SMART:
                        return 0.2;
                    default:
                        return 0;
                }

            }
            
        } 


        #endregion

        #region Propeties


        public static int ShimanniOrderID
        {
            get
            {
                return _ShimanniOrderID;
            }
            set
            {
                _ShimanniOrderID = value;
            }
        }


        public static SortedList<int, ShimanniOrderEntity> ShimanniOrderList
        {
            get
            {
                return _ShimanniOrderList;
            }
            set
            {
                _ShimanniOrderList = value;
            }
        }
        #endregion
    }
}
