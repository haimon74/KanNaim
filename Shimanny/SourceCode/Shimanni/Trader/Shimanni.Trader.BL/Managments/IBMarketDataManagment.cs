using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Shimanni.Trader.Common;
using Krs.Ats.IBNet;
using Krs.Ats.IBNet.Contracts;
using Krs.Ats.IBNet.Orders;
using Shimanni.Trader.DataStructure;


namespace Shimanni.Trader.BL
{
    public static class IBMarketDataManagement
    {
        #region Data Members
        public static object locker = new object();
        public static SortedList<int, AssetEntity> DataSubscribtionList = new SortedList<int, AssetEntity>();
        
        #endregion

        #region Methods
        public static void IBInsertTickPriceIntoDataStructure(object sender, TickPriceEventArgs e)
        {
            lock (locker)
            {
                if (DataSubscribtionList.Count > 0)
                {

                    if (e.TickType == TickType.AskPrice)
                    {
                        DataSubscribtionList[e.TickerId].MarketData.AskPrice = e.Price;
                    }
                    else if (e.TickType == TickType.BidPrice)
                    {
                        DataSubscribtionList[e.TickerId].MarketData.BidPrice = e.Price;
                    }
                    

                }
            }
        }
        public static void IBInsertTickSizeIntoDataStructure(object sender, TickSizeEventArgs e)
        {

            if (DataSubscribtionList.Count > 0)
            {
                
                if (e.TickType == TickType.AskSize)
                {
                    DataSubscribtionList[e.TickerId].MarketData.AskSize = e.Size * 100;
                }
                else if (e.TickType == TickType.BidSize)
                {
                    DataSubscribtionList[e.TickerId].MarketData.BidSize = e.Size * 100;
                }
            }
        }
        public static void UpdateMarketDataSubscriptionList()
        {
            
            

            // IBConectivityManagement.Client.RequestAutoOpenOrders(true);
            lock (locker)
            {
                
                //todo: complite getting shortable
                if (Portfolio.Instance.AssetInputIntegretyCheck())
                {
                    // adding AssetEntity to market subscription list
                    if (IBConectivityManagement.Client.Connected)
                    {
                        System.Collections.ObjectModel.Collection<GenericTickType> tickType = new System.Collections.ObjectModel.Collection<GenericTickType>();
                        tickType.Add(GenericTickType.Shortable);
                        foreach (StrategyEntity StrategyEntity in Portfolio.Instance.StrategiesList)
                        {
//todo:                            if (StrategyEntity.LogStream == null && StrategyEntity.StrategyIndexInList != int.MaxValue) StrategyEntity.LogStream = new SingleStrategyLog(StrategyEntity.StrategyIndexInList);

                            if (StrategyEntity.SubscribeToMarketData == true)
                            {
                                foreach (AssetEntity AssetEntity in StrategyEntity.AssetsList)
                                {
                                    if (!DataSubscribtionList.ContainsValue(AssetEntity))
                                    {
                                        if (AssetEntity.TypeOfAsset == eTypeOfAsset.Equity)
                                        {
                                    //      IBConectivityManagement.Client.RequestMarketData(DataSubscribtionList.Count, new Equity(AssetEntity.SymbolInDataSource), null, false);
                                            IBConectivityManagement.Client.RequestMarketData(DataSubscribtionList.Count, new Equity(AssetEntity.SymbolInDataSource),tickType, false);
                                        }
                                        else if (AssetEntity.TypeOfAsset == eTypeOfAsset.Future)
                                        {
                                            IBConectivityManagement.Client.RequestMarketData(DataSubscribtionList.Count, new Future(AssetEntity.SymbolInDataSource, EnumDescConverter.GetEnumDescription(AssetEntity.ExchangeRouteForAddingLiquidityOrder), AssetEntity.ExpiryDate), null, false);
                                        }
                                        DataSubscribtionList.Add(DataSubscribtionList.Count, AssetEntity);

                                    }
                                }

                            }
                        }


                        //substracting AssetEntity from market data subscription list

                        for (int j = 0; j < DataSubscribtionList.Count; j++)
                        {
                            bool IsOnList = false;
                            for (int i = 0; i < Portfolio.Instance.StrategiesList.Count; i++)
                            {
                                IsOnList = Portfolio.Instance.StrategiesList[i].AssetsList.Contains(DataSubscribtionList[j]);
                                if (IsOnList) break;
                            }

                            if (IsOnList == false)
                            {

                                IBConectivityManagement.Client.CancelMarketData(j);
                                DataSubscribtionList.Remove(DataSubscribtionList.Keys[j]);

                            }
                        }
                    }
                    else
                    {
                        DataSubscribtionList = null;
                    }
                }

            }

        }
        #endregion
    }
}
