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
    public static class IBConectivityManagement
    {
        #region Data Members
        private static bool     _OrderCanceledByBroker;
        private static string   _ErorrMassage;
        private static bool _RequestedToDisconect;
        private static string   _SymbolErorrMassage;
        private static IBClient _Client = new IBClient();
        #endregion

        #region Events Methods
        private static void Client_ReceiveFA(object sender, ReceiveFAEventArgs e)
        {
            MessageBox.Show(e.Xml);
        }
        private static void Client_UpdatePortfolio(object sender, UpdatePortfolioEventArgs e)
        {
            
            
            string newline = DateTime.Now.ToString("hh:mm:ss.ffff") + "\tPortfolio Position At Broker:" + e.Position.ToString() + "\t" + e.Contract.Symbol;
            TraderLog.StreamWriterForTradingLog.WriteLine(newline);

            if (Portfolio.Instance.StrategiesActivityState == eStrategiesActivityState.Activate_Strategies)
            {
                foreach (StrategyEntity StrategyEntity in Portfolio.Instance.StrategiesList)
                {
                    foreach (AssetEntity AssetEntity in StrategyEntity.AssetsList)
                    {
                        if (e.Contract.Symbol == AssetEntity.SymbolInBroker)
                        {
                            AssetEntity.PortfolioPositionAtBroker = e.Position;
                            if (!AssetEntity.CalcultedPortfolioPostionWasUtpdated)
                            {
                                AssetEntity.PortfolioPositionCalculated = e.Position;
                                AssetEntity.CalcultedPortfolioPostionWasUtpdated = true;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Methods
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

            TraderLog.StreamWriterForTradingLog.WriteLine(_ErorrMassage);

            switch ((int)e.ErrorCode)
            {
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


            

        }

        static void Client_UpdateAccountValue(object sender, UpdateAccountValueEventArgs e)
        {
            TraderLog.WriteLineToStream(TraderLog.StreamWriterForTradingLog, e.Key + "\t" + e.Value);
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
