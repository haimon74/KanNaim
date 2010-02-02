using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shimanni.Trader.DataStructure;
using Krs.Ats.IBNet;
using Shimanni.Common.Utils;
using Shimanni.Trader.Common;
using System.Xml.Serialization;
using System.Diagnostics;

namespace Shimanni.Trader.BL
{
    [Serializable() ]    
    public class Strategy : StrategyEntity
    {
        #region Constructors
        public Strategy(int strategyIndexInList):
            base(strategyIndexInList)
        {           
        }
        public Strategy():
            base()
        {
        }
        #endregion        

        #region Methods
        protected override void PlaceHedgingOrders(List<AssetEntity> HedgingAssetBidBased, List<AssetEntity> HedgingAssetAskBased)
        {
            ShimanniOrderEntity newOrder = new ShimanniOrder();
            double HedgeNeeded = Math.Abs(NetExcessExposure) - Math.Abs(MaxMarketMakingExposure);

            if (NetExcessExposure > MaxMarketMakingExposure + ExposuresLatitude && HedgingAssetBidBased.Count > 0)
            {
                AssetEntity asset = HedgingAssetBidBased[0];
                newOrder.ParentAsset = asset;
                newOrder.Size = CommonUtils.DoubleToIntGTOrETZero(HedgeNeeded * Math.Abs(asset.NormalizationRatio));

                if (asset.Beta > 0) SetHedgingOrder(ref newOrder, eSide.Sell);
                else if (asset.Beta < 0) SetHedgingOrder(ref newOrder, eSide.Buy);
            }
            else if (NetExcessExposure < -(MaxMarketMakingExposure + ExposuresLatitude) && HedgingAssetAskBased.Count > 0 )
            {
                AssetEntity asset = HedgingAssetAskBased[0];
                newOrder.ParentAsset = asset;
                newOrder.Size = CommonUtils.DoubleToIntGTOrETZero(HedgeNeeded * Math.Abs(asset.NormalizationRatio));

                if (asset.Beta > 0) SetHedgingOrder(ref newOrder, eSide.Buy);
                else if (asset.Beta < 0) SetHedgingOrder(ref newOrder, eSide.Sell);
            }
        }

        protected override void SetHedgingOrder(ref ShimanniOrderEntity newOrder, eSide pSide)
        {

            AssetEntity asset = newOrder.ParentAsset;

            int NumOfAllOnTheSameSide = (pSide == eSide.Buy ? asset.Buy.numOfAll : asset.Sell.numOfAll);
            if (NumOfAllOnTheSameSide > 15) return;

            newOrder.BrokerRoute = newOrder.ParentAsset.BrokerRoute;
            OrdersBooksEntity OpositeSideBook = (pSide == eSide.Buy ? asset.Buy : asset.Sell);
            newOrder.Size = CommonUtils.Min(newOrder.Size, asset.MaxTradeExpusureUnits, int.MaxValue, true);
            newOrder.Side = pSide;

            Debug.WriteLine("Function: SellHedgingOder: New Order" + newOrder.Price);
            
            if (OpositeSideBook.Ordinery.Count == 0)
            {
                newOrder.Type = eShimanniOrderType.AgresiveHedging;
                newOrder.Price = (int)(asset.MarketData.MidPrice * (1 + (int)pSide* asset.HedgingSpreadBP * 0.0001) / asset.MPV) * asset.MPV;
                OrdersManagement.PlaceOrder(newOrder);
                asset.PreStageTwo = eSide.Null;
            }
            else if (OpositeSideBook.Ordinery.Count > 0 && asset.PreStageTwo == eSide.Null)
            {
                newOrder.Price = OpositeSideBook.Ordinery[0].Price - (int)pSide * asset.MPV;
                newOrder.Type = eShimanniOrderType.SoftHedging;
                OrdersManagement.PlaceOrder(newOrder);
                
            }

        }

        //protected override void SettingSellingOrderBook(double BasePrice, AssetEntity AssetEntity, List<ShimanniOrderEntity> newOrderList)
        //{

        //    AssetEntity.OrderBooksLists.Sell.Sort(ShimanniOrderEntity.SellingOrdersComparison);

        //    double HedgingSellingPrice = (double)((int)(AssetEntity.MarketData.BidPrice * (1 - AssetEntity.HedgingSpread) / AssetEntity.MPV)) * AssetEntity.MPV;
        //    double MMSellingPrice = RoundingPriceToMatchMarket(AssetEntity, BasePrice * (1 + AssetEntity.MMSpread), eSide.Sell);
        //    double MMSellingPriceSticky = RoundingPriceToMatchMarket(AssetEntity, BasePrice * (1 + AssetEntity.MMSpread) * (1 + AssetEntity.StickyPriceLatidude), eSide.Sell);
        //    double ClosingSellingPrice = RoundingPriceToMatchMarket(AssetEntity, BasePrice * (1 + AssetEntity.ClosingSpread), eSide.Sell);
        //    double ClosingSellingPriceSticky = RoundingPriceToMatchMarket(AssetEntity, BasePrice * (1 + AssetEntity.ClosingSpread) * (1 + AssetEntity.StickyPriceLatidude), eSide.Sell);
        //    double OpeningSellingPrice = RoundingPriceToMatchMarket(AssetEntity, BasePrice * (1 + AssetEntity.OpeningSpread), eSide.Sell);
        //    double OpeningSellingPriceSticky = RoundingPriceToMatchMarket(AssetEntity, BasePrice * (1 + AssetEntity.OpeningSpread) * (1 + AssetEntity.StickyPriceLatidude), eSide.Sell);

        //    eSide Side = eSide.Sell;
        //    int MMOrderSum = 0;
        //    int OpeningOrderSum = 0;    
        //    int ClosingOrderSum = 0;
        //    int sumOfNewOrderList = 0;
        //    foreach (ShimanniOrderEntity order in newOrderList)
        //    {
        //        sumOfNewOrderList += order.Size;
        //    }

        //    for (int i = 0; i < AssetEntity.OrderBooksLists.Sell.Count; i++)
        //    {
        //        ShimanniOrderEntity order = AssetEntity.OrderBooksLists.Sell[AssetEntity.OrderBooksLists.Sell.Count - 1 - i];
        //        bool mmZone = order.Price == MMSellingPrice || (order.Price > MMSellingPrice && order.Price <= MMSellingPriceSticky && order.Price < ClosingSellingPrice);
        //        bool closingZone = order.Price == ClosingSellingPrice || (order.Price > ClosingSellingPrice && order.Price <= ClosingSellingPriceSticky && order.Price < OpeningSellingPrice);
        //        bool openingZone = order.Price == OpeningSellingPrice || (order.Price > OpeningSellingPrice && order.Price <= OpeningSellingPriceSticky);
        //        double orderPrice = order.Price;

        //        if (mmZone && (MMOrderSum + order.Size <= newOrderList[0].Size + AssetEntity.StickySizeLatidude && MMOrderSum < newOrderList[0].Size))
        //        {
        //            order.Type = eShimanniOrderType.MM;
        //            MMOrderSum += order.Size;
        //            if (MMOrderSum > newOrderList[0].Size)
        //            {
        //                ClosingOrderSum = MMOrderSum - newOrderList[0].Size;
        //            }
        //        }

        //        else if (closingZone && (ClosingOrderSum + order.Size <= newOrderList[1].Size + AssetEntity.StickySizeLatidude && ClosingOrderSum < newOrderList[1].Size))
        //        {
        //            order.Type = eShimanniOrderType.Closing;
        //            ClosingOrderSum += order.Size;
        //            if (ClosingOrderSum > newOrderList[1].Size)
        //            {
        //                OpeningOrderSum = ClosingOrderSum - newOrderList[1].Size;
        //            }
        //        }
        //        else if (openingZone && (OpeningOrderSum + order.Size <= newOrderList[2].Size + AssetEntity.StickySizeLatidude && OpeningOrderSum < newOrderList[2].Size))
        //        {
        //            order.Type = eShimanniOrderType.Opening;
        //            OpeningOrderSum += order.Size;
        //        }
        //        else
        //        {
        //            OrdersManagement.CancelOrder(order);
        //        }
        //    }
        //     if (AssetEntity.OrderBooksLists.SumOfSellOrders < sumOfNewOrderList && MMOrderSum < newOrderList[0].Size - AssetEntity.StickySizeLatidude)
        //    {
        //        //the second part of th MIN function insure that we will not send new orders when we have cancel pending orders still on our sell list
        //        int size = Math.Min(newOrderList[0].Size - MMOrderSum, sumOfNewOrderList - AssetEntity.OrderBooksLists.SumOfSellOrders); 
        //        if (size > 0 && AssetEntity.OrderBooksLists.numOfAllSellOrder < 15)
        //        {
        //            ShimanniOrderEntity NewOrder = new ShimanniOrder(size, newOrderList[0].Price, Side, AssetEntity);
        //            NewOrder.Type = eShimanniOrderType.MM;
        //            OrdersManagement.PlaceOrder(NewOrder);
        //        }
        //    }

        //    if (AssetEntity.OrderBooksLists.SumOfSellOrders < sumOfNewOrderList && ClosingOrderSum < newOrderList[1].Size - AssetEntity.StickySizeLatidude)
        //    {
        //        int size = Math.Min(newOrderList[1].Size - ClosingOrderSum, sumOfNewOrderList - AssetEntity.OrderBooksLists.SumOfSellOrders);
        //        if (size > 0 && AssetEntity.OrderBooksLists.numOfAllSellOrder < 15)
        //        {
        //            ShimanniOrderEntity NewOrder = new ShimanniOrder(size, newOrderList[1].Price, Side, AssetEntity);
        //            NewOrder.Type = eShimanniOrderType.Closing;
        //            OrdersManagement.PlaceOrder(NewOrder);
        //        }
        //    }
        //    if (AssetEntity.OrderBooksLists.SumOfSellOrders < sumOfNewOrderList && OpeningOrderSum < newOrderList[2].Size - AssetEntity.StickySizeLatidude)
        //    {
        //        int size = Math.Min(newOrderList[2].Size - OpeningOrderSum,
        //                                sumOfNewOrderList - AssetEntity.OrderBooksLists.SumOfSellOrders);
        //        if (size > 0 && AssetEntity.OrderBooksLists.numOfAllSellOrder < 15)
        //        {
        //            ShimanniOrderEntity NewOrder = new ShimanniOrder(size, newOrderList[2].Price, Side, AssetEntity);
        //            NewOrder.Type = eShimanniOrderType.Opening;
        //            OrdersManagement.PlaceOrder(NewOrder);
        //        }
        //    }
        //}
        //protected override void SettingBuyingOrderBook(double BasePrice, AssetEntity Asset, List<ShimanniOrderEntity> newOrderList)
        //{

        //    double HedgingBuyingPrice = (double)(int)(Asset.MarketData.AskPrice * (1 + Asset.HedgingSpread) / Asset.MPV) * Asset.MPV;
        //    double MMBuyingPrice = RoundingPriceToMatchMarket(Asset, BasePrice * (1 - Asset.MMSpread), eSide.Buy);
        //    double MMBuyingPriceSticky = RoundingPriceToMatchMarket(Asset, BasePrice * (1 - Asset.MMSpread) * (1 - Asset.StickyPriceLatidude), eSide.Buy);
        //    double ClosingBuyingPrice = RoundingPriceToMatchMarket(Asset, BasePrice * (1 - Asset.ClosingSpread), eSide.Buy);
        //    double ClosingBuyingPriceSticky = RoundingPriceToMatchMarket(Asset, BasePrice * (1 - Asset.ClosingSpread) * (1 - Asset.StickyPriceLatidude), eSide.Buy);
        //    double OpeningBuyingPrice = RoundingPriceToMatchMarket(Asset, BasePrice * (1 - Asset.OpeningSpread), eSide.Buy);
        //    double OpeningBuyingPriceSticky = RoundingPriceToMatchMarket(Asset, BasePrice * (1 - Asset.OpeningSpread) * (1 - Asset.StickyPriceLatidude), eSide.Buy);

        //    eSide Side = eSide.Buy;
        //    int MMOrderSum = 0;
        //    int OpeningOrderSum = 0;
        //    int ClosingOrderSum = 0;
        //    int sumOfNewOrderList = 0;
        //    foreach (ShimanniOrderEntity order in newOrderList)
        //    {
        //        sumOfNewOrderList += order.Size;
        //    }
        //    Asset.OrderBooksLists.Buy.Sort(ShimanniOrderEntity.BuyingOrdersComparison);
        //    for (int i = 0; i < Asset.OrderBooksLists.Buy.Count; i++)
        //    {
        //        ShimanniOrderEntity order = Asset.OrderBooksLists.Buy[Asset.OrderBooksLists.Buy.Count - i - 1];
        //        double price = order.Price;
        //        bool hedgingZone = order.Price >= HedgingBuyingPrice;
        //        bool mmZone = order.Price == MMBuyingPrice || (order.Price < MMBuyingPrice && order.Price >= MMBuyingPriceSticky && order.Price > ClosingBuyingPrice);
        //        bool closingZone = order.Price == ClosingBuyingPrice || (order.Price < ClosingBuyingPrice && order.Price >= ClosingBuyingPriceSticky && order.Price > OpeningBuyingPrice);
        //        bool openingZone = order.Price == OpeningBuyingPrice || (order.Price < OpeningBuyingPrice && order.Price >= OpeningBuyingPriceSticky);

        //        if (mmZone && (MMOrderSum + order.Size <= newOrderList[0].Size + Asset.StickySizeLatidude && MMOrderSum < newOrderList[0].Size))
        //        {
        //            order.Type = eShimanniOrderType.MM;
        //            MMOrderSum += order.Size;
        //            if (MMOrderSum > newOrderList[0].Size)
        //            {
        //                ClosingOrderSum = MMOrderSum - newOrderList[0].Size;
        //            }
        //        }
        //        else if (closingZone && (ClosingOrderSum + order.Size <= newOrderList[1].Size + Asset.StickySizeLatidude && ClosingOrderSum < newOrderList[1].Size))
        //        {
        //            order.Type = eShimanniOrderType.Closing;
        //            ClosingOrderSum += order.Size;
        //            if (ClosingOrderSum > newOrderList[1].Size)
        //            {
        //                OpeningOrderSum = ClosingOrderSum - newOrderList[1].Size;
        //            }
        //        }
        //        else if (openingZone && (OpeningOrderSum + order.Size <= newOrderList[2].Size + Asset.StickySizeLatidude && OpeningOrderSum < newOrderList[2].Size))
        //        {
        //            order.Type = eShimanniOrderType.Opening;
        //            OpeningOrderSum += order.Size;
        //            if (OpeningOrderSum > newOrderList[2].Size)
        //            {
        //                OpeningOrderSum = OpeningOrderSum - newOrderList[2].Size;
        //            }
        //        }
        //        else
        //        {
        //            OrdersManagement.CancelOrder(order);
        //        }
        //    }
        //    if (Asset.OrderBooksLists.SumOfBuyOrders < sumOfNewOrderList && MMOrderSum < newOrderList[0].Size - Asset.StickySizeLatidude)
        //    {
        //        int size = Math.Min(newOrderList[0].Size - MMOrderSum, sumOfNewOrderList - Asset.OrderBooksLists.SumOfSellOrders);
        //        if (size > 0 && Asset.OrderBooksLists.numOfAllBuyOrder < 15)
        //        {
        //            ShimanniOrderEntity NewOrder = new ShimanniOrder(size, newOrderList[0].Price, Side, Asset);
        //            NewOrder.Type = eShimanniOrderType.MM;
        //            OrdersManagement.PlaceOrder(NewOrder);
        //        }

        //    }
        //    if (Asset.OrderBooksLists.SumOfBuyOrders < sumOfNewOrderList && ClosingOrderSum < newOrderList[1].Size - Asset.StickySizeLatidude)
        //    {
        //        int size = Math.Min(newOrderList[1].Size - ClosingOrderSum, sumOfNewOrderList - Asset.OrderBooksLists.SumOfSellOrders);
        //        if (size > 0 && Asset.OrderBooksLists.numOfAllBuyOrder < 15)
        //        {
        //            ShimanniOrderEntity NewOrder = new ShimanniOrder(size, newOrderList[1].Price, Side, Asset);
        //            NewOrder.Type = eShimanniOrderType.Closing;
        //            OrdersManagement.PlaceOrder(NewOrder);
        //        }
        //    }
        //    if (Asset.OrderBooksLists.SumOfBuyOrders < sumOfNewOrderList && OpeningOrderSum < newOrderList[2].Size - Asset.StickySizeLatidude)
        //    {

        //        int size = Math.Min(newOrderList[2].Size - OpeningOrderSum, sumOfNewOrderList - Asset.OrderBooksLists.SumOfSellOrders);
        //        if (size > 0 && Asset.OrderBooksLists.numOfAllBuyOrder < 15)
        //        {
        //            ShimanniOrderEntity NewOrder = new ShimanniOrder(size, newOrderList[2].Price, Side, Asset);
        //            NewOrder.Type = eShimanniOrderType.Opening;
        //            OrdersManagement.PlaceOrder(NewOrder);
        //        }
        //    }

        //}

        protected override void SettingOrderBook(double BasePrice, AssetEntity Asset, List<ShimanniOrderEntity> newOrderList, eSide pSide)
        {
            OrdersBooksEntity Books;
            
            double HedgingBuyingPrice = (double)(int)(Asset.MarketData.AskPrice * (1 + Asset.HedgingSpread) / Asset.MPV) * Asset.MPV;
            double MMDelta =  BasePrice *  Asset.MMSpread;
            double MMStickyDelta  = BasePrice * Math.Min(((1 + Asset.MMSpread) * (1 + Asset.StickyPriceLatidude) - 1), Asset.ClosingSpread);
            double ClosingDelta = BasePrice * Asset.ClosingSpread;
            double ClosingStickyDelta = BasePrice * Math.Min(((1 + Asset.ClosingSpread) * (1 + Asset.StickyPriceLatidude) - 1), Asset.OpeningSpread);
            double OpeningDelta = BasePrice * Asset.OpeningSpread;
            double OpeningStickyDelta =  BasePrice * ((1 + Asset.OpeningSpread) * (1 + Asset.StickyPriceLatidude) - 1);

            int MMSum = 0;
            int OpeningSum = 0;
            int ClosingSum = 0;
            int sumOfNewOrderList = 0;
            foreach (ShimanniOrderEntity order in newOrderList) { sumOfNewOrderList += order.Size; }
            
            if (pSide == eSide.Buy) 
                    Books = Asset.Buy;
            else 
                    Books = Asset.Sell;
            
            List<ShimanniOrderEntity> Ordinery = Books.Ordinery;
            Ordinery.Sort(ShimanniOrderEntity.OrdersComparisonInABook);
            
            for (int i = 0; i < Ordinery.Count; i++)
            {
                ShimanniOrderEntity order = Ordinery[i];
                double price = order.Price;
                double DeltaPrice = (order.Price - BasePrice) * ( pSide == eSide.Buy? 1:-1) ;
                bool mmZone =  MMStickyDelta> DeltaPrice &&  DeltaPrice >= MMDelta ;
                bool closingZone = ClosingStickyDelta> DeltaPrice &&  DeltaPrice >= ClosingDelta ;
                bool openingZone = OpeningStickyDelta > DeltaPrice &&  DeltaPrice >= OpeningDelta ;;

                if (mmZone && (MMSum + order.Size <= newOrderList[0].Size + Asset.StickySizeLatidude && MMSum < newOrderList[0].Size))
                {
                    order.Type = eShimanniOrderType.MM;
                    MMSum += order.Size;
                    if (MMSum > newOrderList[0].Size)
                    {
                        ClosingSum = MMSum - newOrderList[0].Size;
                    }
                }
                else if (closingZone && (ClosingSum + order.Size <= newOrderList[1].Size + Asset.StickySizeLatidude && ClosingSum < newOrderList[1].Size))
                {
                    order.Type = eShimanniOrderType.Closing;
                    ClosingSum += order.Size;
                    if (ClosingSum > newOrderList[1].Size)
                    {
                        OpeningSum = ClosingSum - newOrderList[1].Size;
                    }
                }
                else if (openingZone && (OpeningSum + order.Size <= newOrderList[2].Size + Asset.StickySizeLatidude && OpeningSum < newOrderList[2].Size))
                {
                    order.Type = eShimanniOrderType.Opening;
                    OpeningSum += order.Size;
                    if (OpeningSum > newOrderList[2].Size)
                    {
                        OpeningSum = OpeningSum - newOrderList[2].Size;
                    }
                }
                else
                {
                    OrdersManagement.CancelOrder(order);
                }
            }
            if (Books.sumofAll < sumOfNewOrderList && MMSum < newOrderList[0].Size - Asset.StickySizeLatidude  && Books.numOfAll < 5)
            {
                int size = Math.Min(newOrderList[0].Size - MMSum, sumOfNewOrderList - Books.sumofAll);
                if (size > 0 )
                {
                    ShimanniOrderEntity NewOrder = new ShimanniOrder(size, newOrderList[0].Price, pSide, Asset);
                    NewOrder.Type = eShimanniOrderType.MM;
                    OrdersManagement.PlaceOrder(NewOrder);
                }

            }
            if (Books.sumofAll < sumOfNewOrderList && ClosingSum < newOrderList[1].Size - Asset.StickySizeLatidude  && Books.numOfAll < 5)
            {
                int size = Math.Min(newOrderList[1].Size - ClosingSum, sumOfNewOrderList - Books.sumofAll);
                if (size > 0 )
                {
                    ShimanniOrderEntity NewOrder = new ShimanniOrder(size, newOrderList[1].Price, pSide, Asset);
                    NewOrder.Type = eShimanniOrderType.Closing;
                    OrdersManagement.PlaceOrder(NewOrder);
                }
            }
            if (Books.sumofAll < sumOfNewOrderList && OpeningSum < newOrderList[2].Size - Asset.StickySizeLatidude  && Books.numOfAll < 5)
            {

                int size = Math.Min(newOrderList[2].Size - OpeningSum, sumOfNewOrderList - Books.sumofAll);
                if (size > 0 )
                {
                    ShimanniOrderEntity NewOrder = new ShimanniOrder(size, newOrderList[2].Price, pSide, Asset);
                    NewOrder.Type = eShimanniOrderType.Opening;
                    OrdersManagement.PlaceOrder(NewOrder);
                }
            }

        }        

        protected override bool GetDataIntegrety()
        {
            if (NetExcessExposure == double.MaxValue)
                return false;
            foreach (AssetEntity item in AssetsList)
            {
                DataIntegrety = true;

                if ((item.MarketData.AskPrice == double.MaxValue || item.MarketData.AskPrice == 0))
                    return false;
                else if ((item.MarketData.AskSize == double.MaxValue || item.MarketData.AskSize == 0))
                    return false;
                else if ((item.MarketData.BidPrice == double.MaxValue || item.MarketData.BidPrice == 0))
                    return false;
                else if ((item.MarketData.BidSize == double.MaxValue || item.MarketData.BidSize == 0))
                    return false;

                else if (item.PortfolioPositionCalculated == int.MaxValue)
                    return false;
                else if (item.TypeOfAsset == eTypeOfAsset.ChooseType)
                    return false;
            }
            if (!IBConectivityManagement.Client.Connected)
                return false;
            //else if (AnOrderWasRegected)
            //        return false;
            else
                return true;
        }
        
        protected override double ReletivePrice(eHedgingRatioMethod Methods, AssetEntity HedgingAsset, AssetEntity AssetEntity, double hedgingAssetPrice)
        {
            switch (Methods)
            {
                case eHedgingRatioMethod.ContiniesPrice:
                    return Math.Pow(hedgingAssetPrice / HedgingAsset.BasePrice, HedgingAsset.Beta / AssetEntity.Beta) * AssetEntity.BasePrice;
                case eHedgingRatioMethod.BasePrice:
                    return ((hedgingAssetPrice / HedgingAsset.BasePrice - 1) * (HedgingAsset.Beta / AssetEntity.Beta) + 1) * AssetEntity.BasePrice;
                case eHedgingRatioMethod.ConstantRatio:
                    return 1;
                default:
                    //todo: make an error massage heare
                    return 1;
            }
        }
        protected override List<ShimanniOrderEntity> GetBasicsOrdersCalaculationList(AssetEntity asset, AssetEntity HedgingAsset, eSide Side)
        {
            ///(Side == eSide.Buy ? -1 : 1)
            double SideConvertedToSign = (Side == eSide.Buy ? -1 : 1);
            double hedgingEffectiveSize;
            double hedgingAssetPrice;
            List<ShimanniOrderEntity> BasicOrderList = new List<ShimanniOrderEntity>();

            if (asset.HedgingSide == Side || asset.HedgingSide == eSide.Null)
            {
                // choosing the refernce side of the hedging AssetEntity cosidering betas of both assets 

                if (asset.Beta * HedgingAsset.Beta > 0)
                {
                    hedgingAssetPrice = (Side == eSide.Buy ? HedgingAsset.EffectiveBidPrice : HedgingAsset.EffectiveAskPrice);
                    hedgingEffectiveSize = (Side == eSide.Buy ? HedgingAsset.EffectiveBidSize : HedgingAsset.EffectiveAskSize);
                }
                else
                {
                    hedgingAssetPrice = (Side == eSide.Sell ? HedgingAsset.EffectiveBidPrice : HedgingAsset.EffectiveAskPrice);
                    hedgingEffectiveSize = (Side == eSide.Sell ? HedgingAsset.EffectiveBidSize : HedgingAsset.EffectiveAskSize);
                }

                // using the refernce price of the hedging AssetEntity we now derive ourpices
                //todo:
                double Price = ReletivePrice(HedgingRatioMethod, HedgingAsset, asset, hedgingAssetPrice);
                double MMPrice = RoundingPriceToMatchMarket(asset, Price * (1 + SideConvertedToSign * asset.MMSpread), Side);
                double ClosingPrice = RoundingPriceToMatchMarket(asset, Price * (1 + SideConvertedToSign * asset.ClosingSpread), Side);
                double OpeningPrice = RoundingPriceToMatchMarket(asset, Price * (1 + SideConvertedToSign * asset.OpeningSpread), Side);

                
                // using the refernce Size of the hedging AssetEntity we now derive size

                double assetHedgingRatio = Math.Abs(asset.NormalizationRatio / HedgingAsset.NormalizationRatio);
                double MaintenenceRestriction = IBOrderManagement.NetBuyingPower / (asset.Multiplayer * Price + HedgingAsset.MarketData.MidPrice * HedgingAsset.Multiplayer / assetHedgingRatio);
                // mmOrder is a Market Makeing Order (as opose to "taking") and trying to hedge a postion through all assets
                int sizeOfMMOrder = (int)CommonUtils.Min(
                                        SideConvertedToSign * NetExcessExposure * asset.NormalizationRatio,
                                        (Side == eSide.Sell ? asset.MaxTradeExpusureUnits : double.MaxValue), //todo: not sure what this condition serve
                                        asset.MaxMarketMakingExposureUnits,
                                        (Side == eSide.Sell ? asset.NetSharesAvelibleForSell : int.MaxValue),
                                        MaintenenceRestriction,
                                        true);

                int sizeOfClosingOrder = (int)CommonUtils.Min(
                                        SideConvertedToSign * asset.PortfolioPositionCalculated - sizeOfMMOrder,
                                        (HedgingAsset.TypeOfAsset == eTypeOfAsset.NotTradable ? int.MaxValue : hedgingEffectiveSize * assetHedgingRatio),
                                        asset.MaxTradeExpusureUnits,
                                        (Side == eSide.Sell ? asset.NetSharesAvelibleForSell - sizeOfMMOrder : int.MaxValue),
                                        true);

                
                int sizeofOpeningOrder = (int)CommonUtils.Min(
                                    asset.MaxStrategyExposureUnits - Math.Abs(asset.PortfolioPositionCalculated),
                                    asset.MaxTradeExpusureUnits,
                                    (HedgingAsset.TypeOfAsset == eTypeOfAsset.NotTradable ? int.MaxValue : hedgingEffectiveSize * assetHedgingRatio - sizeOfClosingOrder - sizeOfMMOrder),
                                    (Side == eSide.Sell ? asset.NetSharesAvelibleForSell - sizeOfClosingOrder - sizeOfMMOrder : int.MaxValue),
                                    MaintenenceRestriction - sizeOfMMOrder,
                                    true);


                BasicOrderList.Add(new ShimanniOrder(sizeOfMMOrder, MMPrice, Side, asset));
                BasicOrderList.Add(new ShimanniOrder(sizeOfClosingOrder, ClosingPrice, Side, asset));
                if (State == eStrategyState.ClosingOnly)
                    BasicOrderList.Add(new ShimanniOrder(0, OpeningPrice, Side, asset));
                else
                    BasicOrderList.Add(new ShimanniOrder(sizeofOpeningOrder, OpeningPrice, Side, asset));
            }
            else
            {
                BasicOrderList.Add(new ShimanniOrder(0, double.MaxValue, Side, asset));
                BasicOrderList.Add(new ShimanniOrder(0, double.MaxValue, Side, asset));
                BasicOrderList.Add(new ShimanniOrder(0, double.MaxValue, Side, asset));

            }
            return BasicOrderList;
        }
        protected override void RunStrategy()
        {
            lock (this.StrategyLocker)
            {
                this.MultiEquetyStrategyChoose2OfManny(); ;
            }
        }
          

        private void HelperMethods(AssetEntity HedgingAsset, AssetEntity AssetI , eSide side)
        {
            double priceDeviation = (side == eSide.Buy ? HedgingAsset.BidBasedPriceDeviation : HedgingAsset.AskBasedPriceDeviation);
            int ChoosingMethod = (side == eSide.Buy ? 1 : -1);


            double basePrice = (priceDeviation / AssetI.Beta + 1) * AssetI.BasePrice;
            if (AssetI.Beta * ChoosingMethod > 0)
            {
                SettingOrderBook(basePrice, AssetI, GetBasicsOrdersCalaculationList(AssetI, HedgingAsset, eSide.Buy), eSide.Buy);
            }
            else
            {
                SettingOrderBook(basePrice, AssetI, GetBasicsOrdersCalaculationList(AssetI, HedgingAsset, eSide.Sell),eSide.Sell);
            }
        }
        public override void MultiEquetyStrategyChoose2OfManny()
        {
            if (State != eStrategyState.NotActive )
            {
                List<AssetEntity> AskBasedList = new List<AssetEntity>();
                List<AssetEntity> BidBasedList = new List<AssetEntity>();
                foreach (Asset asset in AssetsList)
                {
                    
                    if (!asset.BestAsk && asset.EffectiveAskSize >0)
                    {
                        if (asset.Beta > 0 )
                        {
                            AskBasedList.Add(asset);
                        }
                        else if (asset.Beta <= 0)
                        {
                            BidBasedList.Add(asset);
                        }
                    }   
                    if (!asset.BestBid && asset.EffectiveBidSize > 0) // we want to insure that on one hand we are not the best bid and also that other bids are efective in terms of postion and short available
                    {
                        if (asset.Beta > 0)
                        {
                        	BidBasedList.Add(asset);
                        }
                        else if (asset.Beta <= 0)
                        {
                            AskBasedList.Add(asset);
                        }
                    }
                }

                if (AskBasedList.Count > 1) AskBasedList.Sort(Asset.BestAskComparison);
                if (BidBasedList.Count > 1) BidBasedList.Sort(Asset.BestBidComparison);

                 PlaceHedgingOrders(BidBasedList, AskBasedList);
                

                for (int i = 0; i < AssetsList.Count; i++)
                {

                    AssetEntity AssetI = AssetsList[i];

                    if (BidBasedList.Count > 0 && AssetI != BidBasedList[0])
                    {
                        HelperMethods(BidBasedList[0], AssetI, eSide.Buy);
                    }
                    else if (BidBasedList.Count > 1 && AssetI == BidBasedList[0])
                    {
                        HelperMethods(BidBasedList[1], AssetI, eSide.Buy);
                    }

                    if (AskBasedList.Count > 0 && AssetI != AskBasedList[0])
                    {
                        HelperMethods(AskBasedList[0], AssetI, eSide.Sell);
                    }
                    else if (AskBasedList.Count > 1 && AssetI == AskBasedList[0])
                    {
                        HelperMethods(AskBasedList[1], AssetI, eSide.Sell);
                    }
                }
            }
        }          
        public override void AddStrategy(object sender, TickSizeEventArgs e)
        {
            MultiEquetyStrategyChoose2OfManny();    
        }                                
        public override void RunStrategyInNewThread()
        {
            if (GetDataIntegrety() && Portfolio.Instance.StrategiesActivityState == eStrategiesActivityState.Halt_Strategies)
            {
                RunStrategy();
            }

        }
        #endregion
    }
}
