using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;
using Shimanni.Trader.DataStructure;
using Krs.Ats.IBNet;
using Shimanni.Trader.Common;

namespace Shimanni.Trader.BL
{
    public static class IBOrderManagement
    {
        #region Data Members
        private static object locker = new object();

        private static Hashtable                   _IBOrderBook = new Hashtable();
        private static List<ShimanniOrderEntity>   _CancelPendingList = new List<ShimanniOrderEntity>();
        private static int                         _IBValidID;
        private static List<ShimanniOrderEntity>   _QueueOfOrders = new List<ShimanniOrderEntity>();
        private static List<DateTime>              _TimeTrack = new List<DateTime>();
        private static Timer                       _NextOrder = new System.Threading.Timer(new TimerCallback(TransmitNextMessageInQueueRupperForTimer));
        private static Timer                       _CheckCancelOrderList = new System.Threading.Timer(new TimerCallback(RemoveFromCancelList));        
        private static string                      _Account;
        private static double _BuyingPower;
        public static double NetBuyingPower
        {
            get
            {

                if (BuyingPower > SumValueOfOpenOrders)
                    return BuyingPower - SumValueOfOpenOrders;
                else
                {
                    // todo: handle exception here
                    //  throw new x("BuyingPower is smaller then SumValueOfOpenOrders") ;
                    return 0;
                }
            }
        }
        private static double _SumValueOfOpenOrders;

        #endregion

        #region Methods
        private static void RemoveFromCancelList(object timer)
        {
            lock (locker)
            {
                for (int i = 0; i < CancelPendingList.Count; i++)
                {
                    TimeSpan delta = DateTime.Now - CancelPendingList[i].StatusLastTimeChanged;

                    if (delta.Seconds > 5)
                    {

                        TraderLog.StreamWriterForTradingLog.WriteLine(DateTime.Now + "\tOrder:\t" + CancelPendingList[i].ShimanniOrderID + "\tMassege: Order Was removed because canclelation confermation didn't arive");
                        if (CancelPendingList[i].Status != eOrderStatus.Canceled)
                        {
                            CancelPendingList[i].Status = eOrderStatus.Canceled;
                        }
                    }
                }
            }
        }                       
        /// <summary>
        /// Setting the route of the order based on its price reletive to the market. If adding liquidity is possible that the option that will be choosen
        /// </summary>
        /// <param name="pOrder"></param>
        /// <returns></returns>
        private static string ChooseRoute(ShimanniOrderEntity pOrder)
        {
            if (pOrder.Side == eSide.Sell)
            {
                if (pOrder.Price > pOrder.ParentAsset.MarketData.BidPrice)
                {
                    return ((eExchangeRoute)pOrder.ParentAsset.ExchangeRouteForAddingLiquidityOrder).ToString(); ;
                }
                else
                {
                    return ((eExchangeRoute)pOrder.ParentAsset.ExchangeRouteForMarketOrder).ToString();
                }
            }
            else
            {
                if (pOrder.Price < pOrder.ParentAsset.MarketData.AskPrice)
                {
                    return ((eExchangeRoute)pOrder.ParentAsset.ExchangeRouteForAddingLiquidityOrder).ToString();
                }
                else
                {
                    return ((eExchangeRoute)pOrder.ParentAsset.ExchangeRouteForMarketOrder).ToString();
                }
            }
        }
        private static void ClearingTimeTrackList()
        {
           //todo: need to be optimized. to check removeall and remove range and to  check ither this is the best list structure we can ask for
            
            for (int i = 0; i < TimeTrack.Count; i++)
            {
                bool elapsedIsGtThenSecond = (DateTime.Now.Subtract(_TimeTrack[0]) > new TimeSpan(10000));

                if (elapsedIsGtThenSecond)
                    TimeTrack.RemoveAt(0);
                else
                    break;

            }
        }
        private static void PlaceOrderCancelation(ShimanniOrderEntity pOrder)
        {
            lock (locker)
            {
                if (pOrder.Status == eOrderStatus.Submitted)
                {
                    IBConectivityManagement.Client.CancelOrder(pOrder.BorkerOrderID);
                    pOrder.Status = eOrderStatus.PendingCancel;
                    TimeTrack.Add(DateTime.Now);
                    CancelPendingList.Add(pOrder);
                    pOrder.StatusLastTimeChanged = DateTime.Now;
                }
            }
        }
        private static void PlaceOrderInBroker(ShimanniOrderEntity pOrder)
        {
            lock (locker)
            {
                Order newOrder = new Order();

                newOrder.Account = IBOrderManagement._Account;
                newOrder.Action = (pOrder.Side == eSide.Buy ? ActionSide.Buy : ActionSide.Sell);

                newOrder.LimitPrice = pOrder.Price;
                newOrder.OrderType = OrderType.Limit;
                newOrder.TotalQuantity = pOrder.Size;
                pOrder.BorkerOrderID = IBValidID;
                Contract contract = new Contract();
                contract.Symbol = pOrder.ParentAsset.SymbolInBroker;
                contract.Exchange = ChooseRoute(pOrder);
                contract.Currency = "USD";

                if  (TraderLog.StreamWriterForTradingLog != null)
                {
                    string NewLine = DateTime.Now.ToString("hh:mm:ss.ffff") + "\t" +
                            "\tSymbol:\t" + pOrder.ParentAsset.SymbolInBroker +
                            "\tShort:\t" + pOrder.ParentAsset.ShortAvaliableAtBroker.ToString() +
                            "\tPortfolio Position:\t" + pOrder.ParentAsset.PortfolioPositionCalculated.ToString()+
                            "\tSharease avaliable for sell:\t" + pOrder.ParentAsset.NetSharesAvelibleForSell.ToString() +
                            "\tSum of sells sell:\t" + pOrder.ParentAsset.Sell.sumofAll.ToString();

                    TraderLog.StreamWriterForTradingLog.WriteLine(NewLine);
                    TraderLog.StreamWriterForTradingLog.Flush();
                }

                


                if (pOrder.ParentAsset.TypeOfAsset == eTypeOfAsset.Future)
                {
                    contract.SecurityType = SecurityType.Future;
                    contract.Expiry = pOrder.ParentAsset.ExpiryYear + pOrder.ParentAsset.ExpiryMonth;

                }
                else if (pOrder.ParentAsset.TypeOfAsset == eTypeOfAsset.Equity)
                {
                    contract.SecurityType = SecurityType.Stock;
                    newOrder.OutsideRth = true;
                }


                IBConectivityManagement.Client.PlaceOrder(IBValidID, contract, newOrder);

                
                IBOrderBook.Add(IBValidID, pOrder);

                IBValidID++;
                pOrder.Status = eOrderStatus.PendingSubmit;
                if (false)//(TraderLog.StreamWriterForTradingLog != null)
                {
                    string NewLine = DateTime.Now.ToString("hh:mm:ss.ffff") + "\t" +
                            "\tShimanni ID:\t" + pOrder.ShimanniOrderID.ToString() +
                            "\tIB ID:\t" + IBValidID.ToString() +"\tAfter";

                    TraderLog.StreamWriterForTradingLog.WriteLine(NewLine);
                    TraderLog.StreamWriterForTradingLog.Flush();
                }
            }
        }

        /// <summary>
        /// Converting a ShimanniOrderIntoAnIBOrder and sending it to IB
        /// </summary>
        /// <param name="pOrder"></param>        
        public static void TransmitNextMessageInQueueRupperForTimer(object timer)
        {
            TransmitNextMessageInQueue((Timer)timer);
        }
        public static void TransmitNextMessageInQueue(Timer timer)
        {
            lock (locker)
            {

                if ((QueueOfOrders.Count > 0))
                {
                    if (QueueOfOrders.Count > 1)
                    {
                        TimeSpan dif = TimeTrack[1] - TimeTrack[0];
                        int elapse = dif.Seconds * 1000 + dif.Milliseconds;
                        NextOrder.Change(elapse, Timeout.Infinite);
                    }


                    QueueOfOrders.Sort(ShimanniOrderEntity.QueueComparison);
                    ShimanniOrderEntity order = QueueOfOrders[QueueOfOrders.Count - 1];

                    if (order.Status == eOrderStatus.QueingForCancel)
                    {
                        PlaceOrderCancelation(order);
                    }
                    else if (order.Status == eOrderStatus.QueingForSubmit)
                    {
                        PlaceOrderInBroker(order);
                    }

                    QueueOfOrders.RemoveAt(QueueOfOrders.Count - 1);
                }
                else
                    NextOrder.Change(int.MaxValue, Timeout.Infinite);
            }
        }
        /// <summary>
        /// Cleaning from TimeTrack List the items that are older than one second
        /// </summary>        
        public static void PlaceOrderInQueue(ShimanniOrderEntity pOrder)
        {
            {
                if (pOrder.Type != eShimanniOrderType.Closing ) SumValueOfOpenOrders += pOrder.Size;
                ClearingTimeTrackList();
                if (TimeTrack.Count < 49)
                {
                    PlaceOrderInBroker(pOrder);
                    TimeTrack.Add(DateTime.Now);

                }
                else
                {
                    QueueOfOrders_Add(pOrder);
                    pOrder.Status = eOrderStatus.QueingForSubmit;
                }
            }
        }
        #endregion

        #region Events Methods
        public static void PlaceOrderCancelationInQueue(ShimanniOrderEntity pOrder)
        {
            lock (locker)
            {

                // cleaning all orders that are older then one second from the list that memorize them

                ClearingTimeTrackList();
                if (pOrder.Status == eOrderStatus.QueingForSubmit)
                {
                    pOrder.Status = eOrderStatus.Canceled;
                    QueueOfOrders.Remove(pOrder);
                }

                else if (TimeTrack.Count < 49)
                {
                    PlaceOrderCancelation(pOrder);

                }
                else
                {
                    QueueOfOrders_Add(pOrder);
                    pOrder.Status = eOrderStatus.QueingForCancel;
                }
            }
        }
        public static void Client_OrderStatus(object sender, OrderStatusEventArgs e)
        {
            if (IBOrderBook.Contains(e.OrderId))
            {
                ShimanniOrderEntity order = (ShimanniOrderEntity)IBOrderBook[e.OrderId];
                order.Remains = e.Remaining;
                order.Status = (eOrderStatus)e.Status;

                if (e.Status == OrderStatus.Filled || e.Status == OrderStatus.Canceled)
                {
                    IBOrderBook.Remove(e.OrderId);

                }
            }
        }
        public static void Client_ExecDetails(object sender, ExecDetailsEventArgs e)
        {
            lock (locker)
            {
                if (IBOrderBook.Contains(e.OrderId))
                {
                    ShimanniOrderEntity order = (ShimanniOrderEntity)IBOrderBook[e.OrderId];
                    SumValueOfOpenOrders -= e.Execution.Shares * order.Price;

                    if (order.Side == eSide.Buy)
                    {
                        order.ParentAsset.PortfolioPositionCalculated += e.Execution.Shares;
                        //order.ParentAsset.SumOfExecutionsAndStandingOrders += e.Execution.Shares;
                       // order.ParentAsset.NetSharesSoldSinceLastUpdatedOfShorts += e.Execution.Shares;
                        

                    }
                    else
                    {
                        order.ParentAsset.PortfolioPositionCalculated -= e.Execution.Shares;
                    }
                    TraderLog.StreamWriterForTradingLog.WriteLine(DateTime.Now.ToString("hh:mm:ss.ffff") + "\torder number:" + order.ShimanniOrderID.ToString() + "\t" + "number of shares executed\t" + e.Execution.Shares);

                }
            }
        }
        public static void QueueOfOrders_Add(ShimanniOrderEntity order)
        {

            TimeSpan dif = TimeTrack[1] - TimeTrack[0];
            int elapse = dif.Seconds * 1000 + dif.Milliseconds;

            if (QueueOfOrders.Count == 1 && TimeTrack.Count > 0)
                NextOrder.Change(elapse, Timeout.Infinite);
            else
            {
                TransmitNextMessageInQueue(NextOrder);
            }
            QueueOfOrders.Add(order);

        }
        public static void Client_NextValidId(object sender, NextValidIdEventArgs e)
        {
            IBValidID = e.OrderId;

        }
        #endregion

        #region Properties
        public static double SumValueOfOpenOrders
        {
            get { return _SumValueOfOpenOrders; }
            set { _SumValueOfOpenOrders = value; }
        }
        public static double BuyingPower
        {
            get { return _BuyingPower; }
            set { _BuyingPower = value; }
        }
        /// <summary>
        /// Should be thread-safe
        /// </summary>

        public static string Account
        {
            get { return _Account; }
            set { _Account = value; }
        }
        public static List<ShimanniOrderEntity> CancelPendingList
        {
            get
            {
                return _CancelPendingList;
            }
            set
            {
                _CancelPendingList = value;
            }
        }
        public static Hashtable IBOrderBook
        {
            get
            {
                return _IBOrderBook;
            }
            set
            {
                _IBOrderBook = value;
            }
        }
        public static int IBValidID
        {
            get
            {
                return _IBValidID;
            }
            set
            {
                _IBValidID = value;
            }
        }
        public static List<ShimanniOrderEntity> QueueOfOrders
        {
            get
            {
                return _QueueOfOrders;
            }
            set
            {
                _QueueOfOrders = value;
            }
        }
        public static List<DateTime> TimeTrack
        {
            get
            {


                return _TimeTrack;
            }
            set
            {
                TraderLog.StreamWriterForSystemLog.WriteLine(DateTime.Now.ToString() + "\tCount Of TimeTrack Elements:\t" + TimeTrack.Count.ToString() + "\tCount Of OrdersQueue:\t" + QueueOfOrders.Count.ToString());
                _TimeTrack = value;
            }
        }
        public static Timer CheckCancelOrderList
        {
            get { return IBOrderManagement._CheckCancelOrderList; }
            set { IBOrderManagement._CheckCancelOrderList = value; }
        }
        public static Timer NextOrder
        {
            get { return IBOrderManagement._NextOrder; }
            set { IBOrderManagement._NextOrder = value; }
        }
        #endregion
    }
}
