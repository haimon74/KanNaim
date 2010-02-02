using System;
using System.Collections.Generic;
using System.Text;
using Krs.Ats.IBNet;
using System.Threading;
using System.Net;
using System.Windows.Forms;
using System.IO;
using Shimanni.Trader.Common;
using Shimanni.Trader.DataStructure;

namespace Shimanni.Trader.BL
{
    public delegate void RegDelegate<T>(AssetEntity asset, T type);

    public static class IBConectivityManagement
    {
        #region Data Members
        private static bool     _OrderCanceledByBroker;
        private static string   _ErorrMassage;
        private static bool _RequestedToDisconect;
        private static string   _SymbolErorrMassage;
        private static IBClient _Client = new IBClient();
        private static bool _MarketDataConectionUSEquity = false; // todo: is this is the place to initilize it?
        public static bool  MarketDataConectionUSEquity
        {
            get { return _MarketDataConectionUSEquity; }
            set 
            {
                _MarketDataConectionUSEquity = value;
                if(value == false) OrdersManagement.CancelOrderList(OrdersManagement.AllOpenOrdres);
            
            }
        }
        private static bool _SystemIntegrity = false;
        public static bool ConectionTWSToIB
        {
            get { return _SystemIntegrity; }
            set { _SystemIntegrity = value; }
        }
        public static bool PositionsWereUpdated = false;
        #endregion

        #region Events Methods
        private static void Client_ReceiveFA(object sender, ReceiveFAEventArgs e)
        {
            MessageBox.Show(e.Xml);
        }
        private static void Client_UpdatePortfolio(object sender, UpdatePortfolioEventArgs e)
        {
            if (PositionsWereUpdated == false)
            {
            	//PositionsWereUpdated == true;
             //   foreach (AssetEntity asset in Portfolio.Instance.PortfolioAssets)
             //   {
             //       asset.PortfolioPositionCalculated = 0;
             //       asset.CalculatedPortfolioPostionWasUtpdated = true;
             //       if (asset.ShortAvaliableAtBroker != int.MaxValue) asset.PortfolioPositionAtLastShortUpdate = 0;
             //   }
            }
            if (Portfolio.Instance.StrategiesActivityState == eStrategiesActivityState.Activate_Strategies  && OrdersManagement.AllOpenOrdres.Count ==0)
                                                    Client_UpdatePortfolioExtracted1(e, new RegDelegate<UpdatePortfolioEventArgs>(Client_UpdatePortfolioExtracted));
        }
        #endregion
        

        #region Methods
        private static void Client_UpdatePortfolioExtracted1(UpdatePortfolioEventArgs e, RegDelegate<UpdatePortfolioEventArgs> del)
        {
            string newline = DateTime.Now.ToString("hh:mm:ss.ffff") + "\tPortfolio Position At Broker:" + e.Position.ToString() + "\t" + e.Contract.Symbol;
            TraderLog.StreamWriterForTradingLog.WriteLine(newline);
            PositionsWereUpdated = true;
            
            foreach (AssetEntity AssetEntity in Portfolio.Instance.PortfolioAssets)
            {
                del.Invoke(AssetEntity, e);
            }
        }
        private static void Client_UpdatePortfolioExtracted(AssetEntity asset, UpdatePortfolioEventArgs e)
        {            
            
            if (e.Contract.Symbol == asset.SymbolInBroker)
            {
                asset.PortfolioPositionAtBroker = e.Position;
                asset.PortfolioPositionCalculated = e.Position;
            }
        }
        public static void WriteErorrsToLogFile(object sender, Krs.Ats.IBNet.ErrorEventArgs e)
        {
            ShimanniOrderEntity order = new ShimanniOrder();

            if (IBMarketDataManagement.DataSubscribtionList.ContainsKey(e.TickerId))
                _SymbolErorrMassage = ((AssetEntity)IBMarketDataManagement.DataSubscribtionList[e.TickerId]).SymbolInBroker;
            else if (IBOrderManagement.IBOrderBook.ContainsKey(e.TickerId))
            {
                order = (ShimanniOrderEntity)IBOrderManagement.IBOrderBook[e.TickerId];
                _SymbolErorrMassage = order.ShimanniOrderID.ToString();
            }
            _ErorrMassage = DateTime.Now.ToString("hh:mm:ss.ffff") + "\t" + "Log Type:" + "\t" + "IB Erorr\t" + "Symbol:\t" + _SymbolErorrMassage + "\t" + "Error:\t" + e.ErrorMsg + "\tErrorCode:" + e.ErrorCode.ToString() + "\tOrderType:\t" + EnumDescConverter.GetEnumDescription(order.Type) + "\tSymbol:" + order.ParentAsset.SymbolInBroker;
            if(!((int)e.ErrorCode == 202))
                        TraderLog.StreamWriterForTradingLog.WriteLine(_ErorrMassage);

            switch ((int)e.ErrorCode)
            {
                case 100:


                    break;


                
                
                case 135:   //135 - Can't find order with ID
                    if (order.Status != eOrderStatus.Canceled) order.Status = eOrderStatus.Canceled;
                    break;

                case 201:
                    //Order rejected - reason:YOUR ORDER IS NOT ACCEPTED. IT WOULD LEAD TO REG T CALL

                    break;
                case 202:

                   //if (order.Status != eOrderStatus.Canceled) order.Status = eOrderStatus.Canceled;
                    break;
                case 161:   // 161 - Cancel attempted when order is not in a cancellable state. Order permId = 

                    if (order.Status != eOrderStatus.Canceled) order.Status = eOrderStatus.Canceled;
                    break;
                case 404:       //Shares for this order are not immediately available for short sale.<br> The order will be held while we attempt to locate the shares.

                    order.ParentAsset.ShortAvaliableAtBroker = 0;
                    OrdersManagement.CancelOrder(order);
                    break;
                case 1100:  //Connectivity between IB and TWS has been lost.
                    ConectionTWSToIB = false;
                    
                    
                    break;
                case 1101:    //Connectivity between IB and TWS has been restored- data lost/ Market and account data subscription requests must be resubmitted
                    foreach (Asset asset in Portfolio.Instance.PortfolioAssets)
                    {
                        asset.PortfolioPositionAtLastShortUpdate = int.MaxValue;
                        asset.PortfolioPositionCalculated = int.MaxValue;
                    }
                    
                    ConectionTWSToIB = true;
                    IBMarketDataManagement.UpdateMarketDataSubscriptionList();
                    break;

                case 1102:      //Connectivity between IB and TWS has been restored- data maintained.

                    ConectionTWSToIB = true;
                    break;

                case 2103:      //Market data farm connection is broken:usfarm
                    if(e.ErrorMsg == "Market data farm connection is broken:usfarm") MarketDataConectionUSEquity = false;
                    break;
                case 2104:
                    if (e.ErrorMsg =="Market data farm connection is OK:usfarm") MarketDataConectionUSEquity = true;

                    break;
                
                default:

                    break;
                    
            }
        }
        public static bool RequestedToDisconect
        {
            get
            {
               return _RequestedToDisconect ;
            }
            set
            {
                _RequestedToDisconect = value; ;
            }
        }
        public static void RegisterIBEvents()
        {
            Client.TickPrice -= IBMarketDataManagement.IBInsertTickPriceIntoDataStructure;
            Client.TickSize -= IBMarketDataManagement.IBInsertTickSizeIntoDataStructure;            
            Client.ExecDetails -= IBOrderManagement.Client_ExecDetails;
            Client.OrderStatus -= IBOrderManagement.Client_OrderStatus;
            Client.NextValidId -= IBOrderManagement.Client_NextValidId;
            Client.Error -= WriteErorrsToLogFile;
            IBConectivityManagement.Client.UpdatePortfolio -= Client_UpdatePortfolio;

            
            Client.TickPrice += IBMarketDataManagement.IBInsertTickPriceIntoDataStructure;
            Client.TickSize += IBMarketDataManagement.IBInsertTickSizeIntoDataStructure;            
            Client.ExecDetails += new EventHandler<ExecDetailsEventArgs>(IBOrderManagement.Client_ExecDetails);
            Client.OrderStatus += new EventHandler<OrderStatusEventArgs>(IBOrderManagement.Client_OrderStatus);
            Client.NextValidId += new EventHandler<NextValidIdEventArgs>(IBOrderManagement.Client_NextValidId);
            Client.Error += new EventHandler<Krs.Ats.IBNet.ErrorEventArgs>(WriteErorrsToLogFile);
            IBConectivityManagement.Client.UpdatePortfolio += new EventHandler<UpdatePortfolioEventArgs>(Client_UpdatePortfolio);
            IBConectivityManagement.Client.ReceiveFA += new EventHandler<ReceiveFAEventArgs>(Client_ReceiveFA);
            Client.UpdateAccountValue += new EventHandler<UpdateAccountValueEventArgs>(Client_UpdateAccountValue);
            //Client.OpenOrder += new EventHandler<OpenOrderEventArgs>(Client_OpenOrder);
            Client.TickGeneric += new EventHandler<TickGenericEventArgs>(Client_TickGeneric);
            

        }

        static void Client_TickGeneric(object sender, TickGenericEventArgs e)
        {
         //   TraderLog.WriteLineToTradingLog(e.TickType.ToString() + "TickType");
            if ( e.TickType == TickType.Shortable)
            {
                string NewLine = DateTime.Now.ToString("hh:mm:ss.ffff") + "\t" +
                             
                             "\tShortable:\t" + e.Value.ToString();
                TraderLog.StreamWriterForTradingLog.WriteLine(NewLine);
                TraderLog.StreamWriterForTradingLog.Flush();
            }

            // throw new NotImplementedException();
        }

        static void Client_OpenOrder(object sender, OpenOrderEventArgs e)
        {
            
      //    //  ShimanniOrder order = new ShimanniOrder();
      //       
      //      //foreach(StrategyEntity strategy in Portfolio.Instance.StrategiesList)
      //      //{
      //      //	foreach(AssetEntity asset in strategy.AssetsList)
      //      //    {
      //      //          if (asset.SymbolInBroker == e.Contract.Symbol)
	     //      //           order.ParentAsset = asset;
      //      //        break;
      //      //    }
      //      //}

      //      //order.Side (e.Order.Action == ActionSide.Buy ? eSide.Buy : eSide.Sell);
      //      //order.Price = e.order.LimitPrice;
      //      //order.Size = e.Order.TotalQuantity;


      ////      if (IBOrderManagement.IBOrderBook.ContainsKey(e.Order.OrderId))
      //      {
      //          order = (ShimanniOrder)IBOrderManagement.IBOrderBook[e.Order.OrderId];
      //       //todo:   TraderLog.WriteLineToStream(TraderLog.StreamWriterForTradingLog, "OrderId:\t" + e.Order.OrderId.ToString() + "ShimanniID\t" + order.ShimanniOrderID.ToString());
      //      }
      //      else
      //      {
      //         // TraderLog.WriteLineToStream(TraderLog.StreamWriterForTradingLog, "OrderId:\t" + e.Order.OrderId.ToString() + "\t" + "is not in broker open orders list");
      //      }
            

            ///throw new NotImplementedException();
        }


        static void Client_UpdateAccountValue(object sender, UpdateAccountValueEventArgs e)
        {
          //  TraderLog.WriteLineToStream(TraderLog.StreamWriterForTradingLog, e.Key + "\t" + e.Value);

            switch (e.Key)
            {
                case "MaintMarginReq" :
                    Portfolio.Instance.MaintenenceMarginAtBroker =  double.Parse(e.Value);
                    break;
                case "BuyingPower":
                    IBOrderManagement.BuyingPower = double.Parse(e.Value);
                    break;
                default:
                    break;

            }
            //throw new NotImplementedException();
        }
     
        public static IBClient Client
        {
            get
            {
                return _Client;
            }
            set
            {
                _Client = value;
            }
        }
        #endregion
    }
}
