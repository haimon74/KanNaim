using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;
using Shimanni.Trader.DataStructure;
using Krs.Ats.IBNet;
using Shimanni.Trader.Common;
using System.Diagnostics;

namespace Shimanni.Trader.BL
{
    public static class IBOrderManagement
    {
        #region Data Members
        public static object _locker = new object();

        private static Hashtable                   _IBOrderBook = new Hashtable();
        private static int                         _IBValidID;
        private static List<ShimanniOrderEntity>   _QueueOfOrders = new List<ShimanniOrderEntity>();
        private static List<DateTime>              _TimeTrack = new List<DateTime>();
        private static TimerCallback callBack = new TimerCallback(SendMessageInQueueAndeSetTimer);
        private static Timer                       NextOrder = new System.Threading.Timer(callBack, new object(),Timeout.Infinite,Timeout.Infinite);
        private static string                      _Account;
        private static double _BuyingPower;
        private static int _MaxOrderPerSecondAllowed = 45;
        private static double _SumValueOfOpenOrders;

        #endregion

        #region Methods
      
        private static void ClearingTimeTrackList()
        {
            lock (locker)
            {

            //    TraderLog.WriteLineToTradingLog("TimeTrackCount Before Clearing=\t" + TimeTrack.Count.ToString() + "\t" + "QueueOfOrders.Count\t" + QueueOfOrders.Count.ToString());
                while (TimeTrack.Count > 0 && DateTime.Now > TimeTrack[0].AddSeconds(1))
                {
                    TraderLog.WriteLineToTradingLog("TimeTrackCount in Clearing loop=\t" + TimeTrack.Count.ToString());
                    if (TimeTrack.Count>1) TraderLog.WriteLineToTradingLog("TimeTrack difference\t" + (TimeTrack[1]-TimeTrack[0]).Milliseconds.ToString());
                    TimeTrack.RemoveAt(0);
                }
                
           //    TraderLog.WriteLineToTradingLog("TimeTrackCount After Clearing=\t" + TimeTrack.Count.ToString() + "\t" + "QueueOfOrders.Count\t" + QueueOfOrders.Count.ToString());
            }
        }
        private static void PlaceOrderCancelation(ShimanniOrderEntity pOrder)
        {
            lock (locker)
            {
                if (pOrder.Status != eOrderStatus.PendingSubmit)
                {
                    IBConectivityManagement.Client.CancelOrder(pOrder.BorkerOrderID);
                    pOrder.Status = eOrderStatus.PendingCancel;
                    
                    
                    
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
                newOrder.TotalQuantity = pOrder.SizeAtInitiation;
                pOrder.BorkerOrderID = IBValidID;
                Contract contract = new Contract();
                AssetEntity Asset = pOrder.ParentAsset;
                contract.Symbol = Asset.SymbolInBroker;
                contract.Exchange = pOrder.Route.ToString();
                contract.Currency = "USD";

                string NewLine =  // DateTime.Now.ToString("hh:mm:ss.ffff") + "\t" +
                            "\tSymbol:\t" + Asset.SymbolInBroker +
                            "\tShort:\t" + Asset.ShortAvaliableAtBroker.ToString() +
                            "\tPortfolio Position:\t" + Asset.PortfolioPositionCalculated.ToString() +
                            "\tPortfolioPositionAtLastShortUpdate:\t" + Asset.PortfolioPositionAtLastShortUpdate.ToString() +
                            "\tSharease avaliable for sell:\t" + Asset.NetSharesAvelibleForSell.ToString() +
                            "\tSum of Ordinery:\t" + Asset.Sell.SumOfOrdinery.ToString() +
                            "\tSum of Hedging:\t" + Asset.Sell.SumOfHedging.ToString() +
                            "\tNum of QueueOfOrders:\t" + QueueOfOrders.Count.ToString();

                if (TraderLog.StreamWriterForTradingLog != null) TraderLog.WriteLineToTradingLog(NewLine);
                if (pOrder.ParentAsset.ParentStrategy.LogStream.LogStraemer != null) pOrder.ParentAsset.ParentStrategy.LogStream.WriteLineToLog(NewLine);

                if (Asset.TypeOfAsset == eTypeOfAsset.Future)
                {
                    contract.SecurityType = SecurityType.Future;
                    contract.Expiry = Asset.ExpiryYear + Asset.ExpiryMonth;

                }
                else if (Asset.TypeOfAsset == eTypeOfAsset.Equity)
                {
                    contract.SecurityType = SecurityType.Stock;
                    newOrder.OutsideRth = true;
                }


                IBConectivityManagement.Client.PlaceOrder(IBValidID, contract, newOrder);

                IBOrderBook.Add(IBValidID, pOrder);

                IBValidID++;
                pOrder.Status = eOrderStatus.PendingSubmit;
            }
        }

        private static void TransmitMessagesInQueue()
        {
            lock (locker)
            {
                if (QueueOfOrders.Count > 1) QueueOfOrders.Sort(ShimanniOrderEntity.QueueComparison);
                while (QueueOfOrders.Count > 0 && MaxOrderPerSecondAllowed > TimeTrack.Count )
                {
                    ShimanniOrderEntity order = QueueOfOrders[QueueOfOrders.Count - 1];
                    

                    if (order.Status == eOrderStatus.QueingForCancel)
                    {
                        PlaceOrderCancelation(order);
                    }
                    else if (order.Status == eOrderStatus.QueingForSubmit)
                    {
                        PlaceOrderInBroker(order);
                    }
                    TimeTrack.Add(DateTime.Now);
                    QueueOfOrders.RemoveAt(QueueOfOrders.Count - 1);
                }
                if (TimeTrack.Count == MaxOrderPerSecondAllowed && QueueOfOrders.Count > 0)
                        NextOrder.Change((TimeTrack[1] - TimeTrack[0]).Milliseconds, Timeout.Infinite);
            }
        }
        /// <summary>
        /// Converting a ShimanniOrderIntoAnIBOrder and sending it to IB
        /// </summary>
        /// <param name="pOrder"></param>        
    
        public static void SendMessageInQueueAndeSetTimer(object state)
        {
            lock (locker)
            {
                TransmitMessagesInQueue();
                
            }
        }
        /// <summary>
        /// Cleaning from TimeTrack List the items that are older than one second
        /// </summary>        
        public static void PlaceOrderInQueue(ShimanniOrderEntity pOrder)
        {
            lock(locker)
            {
                SumValueOfOpenOrders += pOrder.DollarValueAtInitiation;//The sum should not include Closing Orders because they do not reduce our marginb
                pOrder.Status = eOrderStatus.QueingForSubmit;
                QueueOfOrders.Add(pOrder);
                ClearingTimeTrackList();
                TransmitMessagesInQueue();
            }
        }
        public static void PlaceOrderCancelationInQueue(ShimanniOrderEntity pOrder)
        {
            lock (locker)
            {
                ClearingTimeTrackList();
                if (pOrder.Status == eOrderStatus.QueingForSubmit)
                {
                    pOrder.Status = eOrderStatus.Canceled;
                }
                else
                {
                    pOrder.Status = eOrderStatus.QueingForCancel;
                    QueueOfOrders.Add(pOrder);
                    ClearingTimeTrackList();
                    TransmitMessagesInQueue();
                }
            }
        }

        #endregion

        #region Events Methods
        public static void Client_OrderStatus(object sender, OrderStatusEventArgs e)
        {
            ShimanniOrderEntity order = new ShimanniOrder();
            
            if (IBOrderBook.Contains(e.OrderId))
            {
                order = (ShimanniOrderEntity)IBOrderBook[e.OrderId];
                order.Remains = e.Remaining;
                order.Status = (eOrderStatus)e.Status;
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
        public static void Client_NextValidId(object sender, NextValidIdEventArgs e)
        {
            IBValidID = e.OrderId;
        }
        #endregion

        #region Properties
        public static object locker
        {
            get { return _locker; }
            set { _locker = value; }
        }
        public static double NetBuyingPower
        {
            get
            {
                if (BuyingPower > SumValueOfOpenOrders)
                    return BuyingPower - SumValueOfOpenOrders;
                else
                {
                    return 0;
                }
            }
        }
        public static int MaxOrderPerSecondAllowed
        {
            get { return _MaxOrderPerSecondAllowed; }
            set { _MaxOrderPerSecondAllowed = value; }
        }
        
        /// <summary>
        /// for calculation of margin it sum all orders that increase position with plus sign and orders that reduce postion with a minus.
        /// </summary>
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
                lock(locker)
                {
                    return _QueueOfOrders;
                }
            }
        }
        public static List<DateTime> TimeTrack
        {
            get
            {
                lock (locker)
                {
                    return _TimeTrack;
                }
            }
            set
            {
                TraderLog.StreamWriterForSystemLog.WriteLine(DateTime.Now.ToString() + "\tCount Of TimeTrack Elements:\t" + TimeTrack.Count.ToString() + "\tCount Of OrdersQueue:\t" + QueueOfOrders.Count.ToString());
                _TimeTrack = value;
            }
        }
        #endregion
    }
}
