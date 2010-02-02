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
using Shimanni.Trader.BL.Entites;

namespace Shimanni.Trader.BL
{
    [Serializable() ]    
    public class Strategy : StrategyEntity
    {
        #region Constructors
        public Strategy(PortfolioEntity PortfolioEntity) :
            base(PortfolioEntity)
        {
            Strategy  AAA =  this;
            
            if (LogStream == null ) _LogStream = new SingleStrategyLog(PortfolioParent.StrategiesList.Count+1);
        }
        public Strategy():
            base()
        {
            Strategy AAA = this;
            _PortfolioParent = Portfolio.Instance;
            
            
        }
        #endregion        

        #region Methods
        protected override void PlaceHedgingOrders(List<AssetEntity> HedgingAssetBidBased, List<AssetEntity> HedgingAssetAskBased)
        {
            ShimanniOrderEntity newOrder = new ShimanniOrder();
            double NetHedgeNeeded = Math.Min(Math.Abs(NetExcessExposure) - Math.Abs(MaxMarketMakingExposure),IBOrderManagement.NetBuyingPower);
           // AssetEntity asset = new Asset(0);

            

            if (NetExcessExposure > MaxMarketMakingExposure + ExposuresLatitude && HedgingAssetBidBased.Count > 0)
            {
                AssetEntity asset = HedgingAssetBidBased[0];
               // if (HedgingAssetBidBased[0].Beta > 0)
                    


                newOrder.ParentAsset = asset;
                newOrder.SizeAtInitiation = CommonUtils.DoubleToIntGTOrETZero(NetHedgeNeeded * Math.Abs(asset.NormalizationRatio));
                if (newOrder.SizeAtInitiation < 1)
                    return;
                if (asset.Beta > 0) SetHedgingOrder(ref newOrder, eSide.Sell);
                else if (asset.Beta < 0) SetHedgingOrder(ref newOrder, eSide.Buy);
            }
            else if (NetExcessExposure < -(MaxMarketMakingExposure + ExposuresLatitude) && HedgingAssetAskBased.Count > 0)
            {
                AssetEntity asset = HedgingAssetAskBased[0];
                newOrder.ParentAsset = asset;
                newOrder.SizeAtInitiation = CommonUtils.DoubleToIntGTOrETZero(NetHedgeNeeded * Math.Abs(asset.NormalizationRatio));
                if (newOrder.SizeAtInitiation < 1)
                    return;

                if (asset.Beta < 0) SetHedgingOrder(ref newOrder, eSide.Sell);
                else if (asset.Beta > 0) SetHedgingOrder(ref newOrder, eSide.Buy);
            }
            else
            {
                
            }
        }

        protected override void SetHedgingOrder(ref ShimanniOrderEntity newOrder, eSide pSide)
        {

            AssetEntity asset = newOrder.ParentAsset;

            if (asset.SymbolInBroker == Portfolio.Instance.DebugedSymbol)
            {
            	int x=1;
            }

            int NumOfAllOnTheSameSide = (pSide == eSide.Buy ? asset.Buy.numOfAll : asset.Sell.numOfAll);
            if (NumOfAllOnTheSameSide > 10) return;
            
            newOrder.BrokerRoute = newOrder.ParentAsset.BrokerRoute;
            OrdersBooksEntity OpositeSideBook = (pSide == eSide.Sell ? asset.Buy : asset.Sell);
            OrdersBooksEntity SameSideBook = (pSide == eSide.Buy ? asset.Buy : asset.Sell);
            newOrder.SizeAtInitiation = 
                CommonUtils.Min(
                newOrder.SizeAtInitiation, 
                  2*asset.MaxTradeExpusureUnits - SameSideBook.SumOfHedging, // at one point in time we are max side exposure is twice trade exposure and therefor such an exposure could result in two hedging orders. only for the over night we should could have a higher exposure and this constrained will make hedging process slawer
                (pSide == eSide.Sell? asset.NetSharesAvelibleForSell: int.MaxValue),
                (int) (IBOrderManagement.NetBuyingPower/asset.MarketData.MidPrice/asset.Multiplayer - (int)pSide * asset.PortfolioPositionCalculated),
                (int) (newOrder.ParentAsset.MaxStrategyExposureUnits -(int)pSide * asset.PortfolioPositionCalculated),
                true);

            if (newOrder.SizeAtInitiation < asset.StickySizeLatidude || newOrder.SizeAtInitiation < 1) return;

            newOrder.Side = pSide;

            Debug.WriteLine("Function: SellHedgingOder: New Order" + newOrder.Price);
            
            if (OpositeSideBook.numOfAll == 0 )
            {
                newOrder.Type = eShimanniOrderType.AgresiveHedging;
                newOrder.Price = (int)((asset.MarketData.MidPrice + (int)pSide* asset.HedgingSpreadDollarTerms ) / asset.MPV) * asset.MPV;
                OrdersManagement.PlaceOrder(newOrder);
                asset.PreStageTwo = eSide.Null;
            }
            else if (asset.PreStageTwo == eSide.Null)
            {
                if (OpositeSideBook.Hedging.Count > 0)
                {
                     OpositeSideBook.Hedging.Sort(ShimanniOrderEntity.OrdersComparisonInABook);
                     newOrder.Price = OpositeSideBook.Hedging[0].Price - (int)pSide * asset.MPV; // it takes
                }
                else if  (OpositeSideBook.Ordinery.Count > 0)
                {
                   OpositeSideBook.Ordinery.Sort(ShimanniOrderEntity.OrdersComparisonInABook);
                   newOrder.Price = OpositeSideBook.Ordinery[0].Price - (int)pSide * asset.MPV; // it takes
                }
                                    
                newOrder.Type = eShimanniOrderType.SoftHedging;
                OrdersManagement.PlaceOrder(newOrder);
            }
            Debug.WriteLine("Function: SellHedgingOder: New Order" + newOrder.Price);
        }

  

        protected override void SettingOrderBook(double BasePrice, AssetEntity Asset, List<ShimanniOrderEntity> newOrderList, eSide pSide)
        {
            OrdersBooksEntity Books;
          //  MinReqProfitFromNewOrder = 2;
            if (Asset.SymbolInBroker != Portfolio.Instance.DebugedSymbol)
            {
              //  return;
            }
            
            double MMDelta =  BasePrice *  Asset.MMSpread;
            double MMStickyDelta  = BasePrice * Math.Min(((1 + Asset.MMSpread) * (1 + Asset.StickyPriceLatidude) - 1), Asset.ClosingSpread);
            double ClosingDelta = BasePrice * Asset.ClosingSpread;
            double ClosingStickyDelta = BasePrice * Math.Min(((1 + Asset.ClosingSpread) * (1 + Asset.StickyPriceLatidude) - 1), Asset.OpeningSpread);
            double OpeningDelta = BasePrice * Asset.OpeningSpread;
            double OpeningStickyDelta =  BasePrice * ((1 + Asset.OpeningSpread) * (1 + Asset.StickyPriceLatidude) - 1);
            if (Asset.SymbolInBroker == Portfolio.Instance.DebugedSymbol)
            {
                int x = 1;
            }
            int MMSum = 0;
            int OpeningSum = 0;
            int ClosingSum = 0;
            int sumOfNewOrderList = 0;
            foreach (ShimanniOrderEntity order in newOrderList) { sumOfNewOrderList += order.SizeAtInitiation; }
            
            if (pSide == eSide.Buy) 
                    Books = Asset.Buy;
            else 
                    Books = Asset.Sell;
            
            List<ShimanniOrderEntity> Ordinery = Books.Ordinery;
            Ordinery.Sort(ShimanniOrderEntity.OrdersComparisonInABook);
            
            for (int i = 0; i < Ordinery.Count; i++)
            {
                ShimanniOrderEntity order = Ordinery[i];
                string st = "count:\t" + Ordinery.Count.ToString() + "\tOrder Index:\t" + Convert.ToString(i) + "\tSymbol:\t" + Asset.SymbolInBroker + "\tPrice:\t" + order.Price.ToString() + "\tSizeAtInitiation:\t" + order.SizeAtInitiation.ToString();
                this.LogStream.WriteLineToLog(st);
                TraderLog.WriteLineToTradingLog(st);
                this.printStrategyStateToLog();

                

                double price = order.Price;
                double DeltaPrice = (BasePrice - order.Price) * (int)pSide;
                bool mmZone = price == newOrderList[0].Price; //MMStickyDelta> DeltaPrice &&  DeltaPrice >= MMDelta ;
                bool closingZone = price == newOrderList[1].Price; //ClosingStickyDelta> DeltaPrice &&  DeltaPrice >= ClosingDelta ;
                bool openingZone = price == newOrderList[2].Price; //OpeningStickyDelta > DeltaPrice &&  DeltaPrice >= OpeningDelta ;;

                if (mmZone && MMSum + order.SizeAtInitiation <= newOrderList[0].SizeAtInitiation)
                {
                    order.Type = eShimanniOrderType.MM;
                    MMSum += order.SizeAtInitiation;
                }
                else if (closingZone && ClosingSum + order.SizeAtInitiation <= newOrderList[1].SizeAtInitiation )
                {
                    order.Type = eShimanniOrderType.Closing;
                    ClosingSum += order.SizeAtInitiation;
                }
                else if (openingZone && OpeningSum +order.SizeAtInitiation <= newOrderList[2].SizeAtInitiation )
                {
                    order.Type = eShimanniOrderType.Opening;
                    OpeningSum += order.SizeAtInitiation;
                }
                else if(order.Status != eOrderStatus.PendingCancel)
                {
                    if (Asset.SymbolInBroker == Portfolio.Instance.DebugedSymbol)
                    {
                        int x = 1;
                    }
                    OrdersManagement.CancelOrder(order);
                }
            }
            if (Asset.SymbolInBroker == Portfolio.Instance.DebugedSymbol)
            {
                int x = 1;
            }
            if (MMSum < newOrderList[0].SizeAtInitiation && Books.numOfAll < 10)
            {
                int size = Math.Max(newOrderList[0].SizeAtInitiation - MMSum,0);
                double expectedProfit = size * newOrderList[0].Price * Asset.MMSpread - Math.Max(1, size * Asset.AddingLiquidityTotalFees );
                if (expectedProfit > MinReqProfitFromNewOrder)
                {
                    ShimanniOrderEntity NewOrder = new ShimanniOrder(size, newOrderList[0].Price, pSide, Asset);
                    NewOrder.Type = eShimanniOrderType.MM;
                    OrdersManagement.PlaceOrder(NewOrder);
                }
            }
            if ( ClosingSum < newOrderList[1].SizeAtInitiation && Books.numOfAll < 10)
            {
                int size = Math.Max(newOrderList[1].SizeAtInitiation - ClosingSum,0);
                double expectedProfit = size * newOrderList[1].Price * Asset.ClosingSpread - Math.Max(1, size * (Asset.AddingLiquidityTotalFees + Asset.TakeingLiqtuidityTotalFees / CurrentHeDgingRatio));
                if (expectedProfit > MinReqProfitFromNewOrder)
                {
                    ShimanniOrderEntity NewOrder = new ShimanniOrder(size, newOrderList[1].Price, pSide, Asset);
                    NewOrder.Type = eShimanniOrderType.Closing;
                    OrdersManagement.PlaceOrder(NewOrder);
                }
            }
           
            
            if (OpeningSum < newOrderList[2].SizeAtInitiation  && Books.numOfAll < 10)
            {
                int size = newOrderList[2].SizeAtInitiation - OpeningSum;
                double portionExpectedtobeSoldinMM =  Asset.MaxMarketMakingExposureUnits  - Math.Max(0, (int)pSide* this.ExcessStrategyExposure*Asset.Beta /(Asset.MarketData.MidPrice * Asset.Multiplayer) );
                double SharesOfHedgingAssetExpectedToBeSold = Math.Max(0, (size - portionExpectedtobeSoldinMM)/ CurrentHeDgingRatio);
                double expectedProfit =
                    size * newOrderList[2].Price * Asset.OpeningSpread + 
                    portionExpectedtobeSoldinMM * Math.Max(0,Asset.MarketData.HalfBidAskSpread - Asset.MPV) * 2 - 
                    Math.Max(1, size * Asset.AddingLiquidityTotalFees) -  
                    Math.Max(1, SharesOfHedgingAssetExpectedToBeSold* Asset.TakeingLiqtuidityTotalFees );

                    if (expectedProfit > MinReqProfitFromNewOrder)
                {
                    ShimanniOrderEntity NewOrder = new ShimanniOrder(size, newOrderList[2].Price, pSide, Asset);
                    NewOrder.Type = eShimanniOrderType.Opening;
                    OrdersManagement.PlaceOrder(NewOrder);
                }
            }
            if ( Asset.SymbolInBroker == Portfolio.Instance.DebugedSymbol)
            {
                int x = 1;
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
                    DataIntegrety = false;
                else if ((item.MarketData.AskSize == double.MaxValue || item.MarketData.AskSize == 0))
                    DataIntegrety = false;
                else if ((item.MarketData.BidPrice == double.MaxValue || item.MarketData.BidPrice == 0))
                    DataIntegrety = false;
                else if ((item.MarketData.BidSize == double.MaxValue || item.MarketData.BidSize == 0))
                    DataIntegrety = false;
                else if (false) //(item.PortfolioPositionCalculated == int.MaxValue)
                    DataIntegrety = false;
                else if (item.TypeOfAsset == eTypeOfAsset.ChooseType)
                    DataIntegrety = false;
                else if (IBConectivityManagement.MarketDataConectionUSEquity == false)
                    DataIntegrety = false;
            }
            if (!IBConectivityManagement.Client.Connected)
            {
                DataIntegrety = false;
            }
            else
            {
            
                if (DataIntegrety == false)
                {
                    foreach (AssetEntity item in AssetsList)
                    {
                        item.Buy.CancelAll();
                        item.Buy.CancelAll();
                    }
                }
            }
            
            
                return DataIntegrety;
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
        protected override List<ShimanniOrderEntity> GetBasicsOrders(AssetEntity asset, AssetEntity HedgingAsset, eSide Side )
        {
            BasicOrderSet OrderSet = new BasicOrderSet();
            if (asset.SymbolInBroker == Portfolio.Instance.DebugedSymbol)
            {
                int x = 1;
            }
            ///(Side == eSide.Buy ? -1 : 1)
            double SideConvertedToSign = (Side == eSide.Buy ? -1 : 1);
            double hedgingEffectiveSize;
            double hedgingAssetPrice;
            int sizeOfClosing;
            int sizeofOpening;

            List<ShimanniOrderEntity> BasicOrderList = new List<ShimanniOrderEntity>();

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

            CurrentHeDgingRatio = Math.Abs(asset.NormalizationRatio / HedgingAsset.NormalizationRatio);
            double MaintenenceRestriction = IBOrderManagement.NetBuyingPower / 
                        (asset.Multiplayer * Price + HedgingAsset.MarketData.MidPrice * HedgingAsset.Multiplayer / CurrentHeDgingRatio);
            // mmOrder is a Market Makeing Order (as opose to "taking") and trying to hedge a postion through all assets
            
            int sizeOfMM = (int)CommonUtils.Min(
                                    SideConvertedToSign * NetExcessExposure * asset.NormalizationRatio,
                                    (Side == eSide.Sell ? asset.MaxTradeExpusureUnits : double.MaxValue), //todo: not sure what this condition serve
                                    asset.MaxMarketMakingExposureUnits,
                                    (Side == eSide.Sell ? asset.NetSharesAvelibleForSell : int.MaxValue),
                                    MaintenenceRestriction,
                                    true);
            if (asset != HedgingAsset)
            {
                sizeOfClosing = (int)CommonUtils.Min(
                                    SideConvertedToSign * asset.PortfolioPositionCalculated - sizeOfMM,
                                    (HedgingAsset.TypeOfAsset == eTypeOfAsset.NotTradable ? int.MaxValue : hedgingEffectiveSize * CurrentHeDgingRatio),
                                    asset.MaxTradeExpusureUnits,
                                    (Side == eSide.Sell ? asset.NetSharesAvelibleForSell - sizeOfMM : int.MaxValue),
                                    (Side == eSide.Sell ? asset.Sell.AvaliableForClosing : asset.Buy.AvaliableForClosing) - sizeOfMM,
                                    true);
                Debug.WriteIf(asset.SymbolInBroker == "", sizeOfClosing, "sizeOfClosing");

                sizeofOpening = (int)CommonUtils.Min(
                                    asset.MaxStrategyExposureUnits - Math.Abs(asset.PortfolioPositionCalculated),
                                    asset.MaxTradeExpusureUnits,
                                    (HedgingAsset.TypeOfAsset == eTypeOfAsset.NotTradable ? int.MaxValue : hedgingEffectiveSize * CurrentHeDgingRatio - sizeOfClosing - sizeOfMM),
                                    (Side == eSide.Sell ? asset.NetSharesAvelibleForSell - sizeOfClosing - sizeOfMM : int.MaxValue),
                                    MaintenenceRestriction - sizeOfMM,
                                    true);
            }
            else
            {
                sizeOfClosing = 0;
                sizeofOpening = 0;
            }

            BasicOrderList.Add(new ShimanniOrder(sizeOfMM, MMPrice, Side, asset));
            BasicOrderList.Add(new ShimanniOrder(sizeOfClosing, ClosingPrice, Side, asset));
            if (State == eStrategyState.ClosingOnly)
                BasicOrderList.Add(new ShimanniOrder(0, OpeningPrice, Side, asset));
            else
                BasicOrderList.Add(new ShimanniOrder(sizeofOpening, OpeningPrice, Side, asset));
            if (asset.SymbolInBroker == Portfolio.Instance.DebugedSymbol)
            {
                int x = 1;
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
          

       

        private void TradeSideSelector(List<AssetEntity> SideBasedList, AssetEntity AssetI, eSide pSideOfList)
        {
            if (SideBasedList.Count > 0)
            {
                double basePrice = 1;
                eSide SideOfTrade; 
                
                if (pSideOfList == eSide.Buy)
                    SideOfTrade = eSide.Sell;
                else
                    SideOfTrade = eSide.Buy;
                        
                
                
                
                
                if (SideOfTrade == eSide.Buy)
                { }
                else 
                { }
                
                if (AssetI.SymbolInBroker == Portfolio.Instance.DebugedSymbol)
                 {int x = 1;}
                
                if (AssetI != SideBasedList[0])
                {
                    SettingOrderBook(basePrice, AssetI, GetBasicsOrders(AssetI, SideBasedList[0], SideOfTrade), SideOfTrade);
                }
                else if (AssetI == SideBasedList[0])
                {
                    if (SideBasedList.Count > 1)
                    {
                        SettingOrderBook(basePrice, AssetI, GetBasicsOrders(AssetI, SideBasedList[1], SideOfTrade), SideOfTrade);
                    }
                    else if ((SideBasedList.Count == 0))
                    {
                        SettingOrderBook(basePrice, AssetI, GetBasicsOrders(AssetI, SideBasedList[0], SideOfTrade), SideOfTrade);
                    }
                }
            }
        }
        public override void MultiEquetyStrategyChoose2OfManny()
        {
            int x = 0;
            for (int j = 0; j < AssetsList.Count; j++)
            {
                if (Portfolio.Instance.DebugedSymbol == AssetsList[j].SymbolInBroker)
                {
                    x = 1;
                    this.LogStream.WriteMarketData = true;
                }
            }

 //           if (x == 0) return;
            
            
            foreach (AssetEntity item in AssetsList)
            {
                //item.Buy.CacnelAllQueueingOrders();
                //item.Sell.CacnelAllQueueingOrders();

                if (Portfolio.Instance.DebugedSymbol == item.SymbolInBroker)
                {}
            }
            
            if (State != eStrategyState.NotActive )
            {
                List<AssetEntity> AskBasedList = new List<AssetEntity>();
                List<AssetEntity> BidBasedList = new List<AssetEntity>();
                
                foreach (Asset asset in AssetsList)
                {
                    if (asset.SymbolInBroker == Portfolio.Instance.DebugedSymbol)
                    {
                        int y = 1;
                    }
                    
                    if (!asset.BestAsk && asset.EffectiveAskSize >0)
                    {
                        if (asset.Beta > 0 )
                            AskBasedList.Add(asset);
                        else 
                            BidBasedList.Add(asset);
                    }   
                    if (!asset.BestBid && asset.EffectiveBidSize > 0) // we want to insure that on one hand we are not the best bid and also that other bids are efective in terms of postion and short available
                    {
                        if (asset.Beta > 0)
                        	BidBasedList.Add(asset);
                        else 
                            AskBasedList.Add(asset);
                    }
                }

                if (AskBasedList.Count > 1) AskBasedList.Sort(Asset.BestAskComparison);
                if (BidBasedList.Count > 1) BidBasedList.Sort(Asset.BestBidComparison);
                 PlaceHedgingOrders(BidBasedList, AskBasedList);

                for (int i = 0; i < AssetsList.Count; i++)
                {

             //       if (!(Portfolio.Instance.DebugedSymbol == AssetsList[i].SymbolInBroker)) continue;

                    TradeSideSelector(BidBasedList, AssetsList[i],eSide.Buy);
                    TradeSideSelector(AskBasedList, AssetsList[i],eSide.Sell);
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
        public override void printStrategyStateToLog()
        {
            
            string line = "\n";

            foreach (AssetEntity item in AssetsList)
            {
                line +=   "Symbol:\t" + item.SymbolInBroker +
                         "\tAsk:\t" + item.MarketData.AskPrice.ToString() + "\t" + item.MarketData.AskSize.ToString() + "\tSum Of All Sell Orders:\t" + item.Sell.SumOfAll.ToString() + 
                         "\tBid:\t" + item.MarketData.BidPrice.ToString() + "\t" + item.MarketData.BidSize.ToString() + "\tSum Of All Buy Orders:\t" + item.Buy.SumOfAll.ToString()+
                         "\tPosition:\t" + item.PortfolioPositionCalculated.ToString() + 
                         "\n";
                
            }
            this.LogStream.WriteLineToLog(line);
        }

        #endregion
    }

}
