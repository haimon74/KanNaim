using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Shimanni.Trader.Common;

namespace Shimanni.Trader.DataStructure
{
    public abstract class ShimanniOrderEntity : IComparable
    {
        #region Data Members
        private DateTime                        _TimePlacedAtTheMarket;
        // Takes Value of eShimanniOrderType. MMOrder, Opening Order, Closing Order Hedging order
        private eShimanniOrderType              _Type; 
        private int                             _ShimanniOrderID;
        private int                             _BorkerOrderID;
        private eExchangeRoute                  _ExchangeRoute;        
        private DateTime                        _StatusLastTimeChanged;
        private int                             _NumberOfTimesCancelRequested;        
        private int                             _SizeAtInitiation;
        private double                          _Price;
        private eSide                           _Side;
        private int                             _Filled;
        private int                             _Remains;        
        private SortedList<ShimanniOrderEntity, int>  _CancelBetchList;
        private List<ShimanniOrderEntity>             _ListMembership;
        private bool                            _IsHedgingOrder;

        protected eOrderStatus                  _Status; // Trace the status of an order
        protected AssetEntity                   _ParentAsset;
        protected eBrokerRoute                  _BrokerRoute; //         
        protected string _Symbol;
        protected object _Lock;
        #endregion

        #region Constractors
        public ShimanniOrderEntity(int size, double price, eSide side, AssetEntity ParentAsset, int pShimanniOrderID, int pBrokerOrderID)
        {
            _Type = eShimanniOrderType.Null;
            _ShimanniOrderID = pShimanniOrderID;
            _BorkerOrderID = pBrokerOrderID;
            _ExchangeRoute = eExchangeRoute.ChooseRoute;
            _BrokerRoute = ParentAsset.BrokerRoute;
            _Status = eOrderStatus.None;
            _Symbol = ParentAsset.SymbolInBroker;
            _SizeAtInitiation = size;
            _Price = price;
            _Side = side;
            _ParentAsset = ParentAsset;
            _CancelBetchList = new SortedList<ShimanniOrderEntity, int>();
            _ListMembership = new List<ShimanniOrderEntity>();
        }
        public ShimanniOrderEntity(int size, double price, eSide side, AssetEntity parentAsset)
        {
            _Type = eShimanniOrderType.Null;
            _ShimanniOrderID = int.MaxValue;
            _BorkerOrderID = int.MaxValue;
            _ExchangeRoute = eExchangeRoute.ChooseRoute;
            _BrokerRoute = parentAsset.BrokerRoute;
            _Status = eOrderStatus.None;
            _Symbol = parentAsset.SymbolInBroker;
            _SizeAtInitiation = size;
            _Price = price;
            _Side = side;
            _ParentAsset = parentAsset;
            _CancelBetchList = new SortedList<ShimanniOrderEntity, int>();
            _ListMembership = new List<ShimanniOrderEntity>();
        }
        public ShimanniOrderEntity(double price, int size)
        {            
            _Type = eShimanniOrderType.Null;
            _ShimanniOrderID = int.MaxValue;
            _BorkerOrderID = int.MaxValue;
            _ExchangeRoute = eExchangeRoute.ChooseRoute;            
            _Status = eOrderStatus.None;            
            _SizeAtInitiation = size;
            _Price = price;
            _Side = eSide.Null;

            _CancelBetchList = new SortedList<ShimanniOrderEntity, int>();
            _ListMembership = new List<ShimanniOrderEntity>();
        }
        public ShimanniOrderEntity()
        {            
            _Type = eShimanniOrderType.Null;
            _ShimanniOrderID = int.MinValue;
            _BorkerOrderID = int.MaxValue;
            _ExchangeRoute = eExchangeRoute.ChooseRoute;            
            _Status = eOrderStatus.None;            
            _SizeAtInitiation = int.MaxValue;
            _Price = double.MaxValue;
            _Side = eSide.Null;

            _CancelBetchList = new SortedList<ShimanniOrderEntity, int>();
            _ListMembership = new List<ShimanniOrderEntity>();
        }
        #endregion

        #region Properties
        public object Lock
        {
            get { return _Lock; }
            set { _Lock = value; }
        }
        public DateTime TimePlacedAtTheMarket
        {
            get { return _TimePlacedAtTheMarket; }
            set { _TimePlacedAtTheMarket = value; }
        }
        public eShimanniOrderType Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;

                if (_Type == eShimanniOrderType.SoftHedging || _Type == eShimanniOrderType.AgresiveHedging)
                    _IsHedgingOrder = true;
                else
                    _IsHedgingOrder = false;

            }
        }
        public int ShimanniOrderID
        {
            get
            {
                return _ShimanniOrderID;
            }
            set
            {
                _ShimanniOrderID = value;
                //    WriteOrderToLogFile("_ShimanniOrderID",_ShimanniOrderID.ToString());
            }
        }
        public int BorkerOrderID
        {
            get
            {
                return _BorkerOrderID;
            }
            set
            {
                _BorkerOrderID = value;
            }
        }
        public eExchangeRoute ExchangeRoute
        {
            get
            {
                return _ExchangeRoute;
            }
            set
            {
                _ExchangeRoute = value;
            }
        }
        public eBrokerRoute BrokerRoute
        {
            get
            {
                return _BrokerRoute;
            }
            set
            {
                _BrokerRoute = value;
            }
        }
        public abstract eOrderStatus Status {get; set;}
        public abstract eExchangeRoute Route { get;}
        public DateTime StatusLastTimeChanged
        {
            get
            {
                return _StatusLastTimeChanged;
            }
            set
            {
                _StatusLastTimeChanged = value;
            }
        }
        public int NumberOfTimesCancelRequested
        {
            get
            {
                return _NumberOfTimesCancelRequested;
            }
            set
            {
                _NumberOfTimesCancelRequested = value;
            }
        }
        public string Symbol
        {
            get
            {
                return ParentAsset.SymbolInBroker;
            }
        }
        public int SizeAtInitiation
        {
            get
            {
                return _SizeAtInitiation;
            }
            set
            {
                _Remains = value;
                _SizeAtInitiation = value;
                if (_SizeAtInitiation < 2)
                {
                  //  WriteOrderToLogFile("Size", _Size.ToString());
                }

            }
        }
        public double DollarValueAtInitiation
        {
            get { return SizeAtInitiation * Price; }
        }
        public double DollarValueRemains
        {
            get { return Remains * Price; }
        }
        public double Price
        {
            get
            {
                return _Price;
            }
            set
            {
                _Price = value;
               // WriteOrderToLogFile("_Price", _Price.ToString());
            }
        }
        public int Filled
        {
            get
            {
                return _Filled;
            }
            set
            {
                _Filled = value;
              //  WriteOrderToLogFile("_Filled", _Filled.ToString());
            }
        }
        public int Remains
        {
            get
            {
                return _Remains;
            }
            set
            {
                _Remains = value;

                //  WriteOrderToLogFile("_Remains",_Remains.ToString());
            }
        }
        public eSide Side
        {
            get
            {
                return _Side;
            }
            set
            {
                _Side = value;
            }
        }
        [XmlIgnore]
        public AssetEntity ParentAsset
        {
            get
            {
                return _ParentAsset;
            }
            set
            {
                _ParentAsset = value;
            }
        }
        [XmlIgnore]
        public SortedList<ShimanniOrderEntity, int> CancelBetchList
        {
            get
            {
                return _CancelBetchList;
            }
            set
            {
                _CancelBetchList = value;
            }
        }
        [XmlIgnore]
        public List<ShimanniOrderEntity> ListMembership
        {
            get
            {
                return _ListMembership;
            }
            set
            {
                _ListMembership = value;
            }
        }
        public bool IsHedgingOrder
        {
            get
            {

                return _IsHedgingOrder;
            }

        }
        
        #endregion

        #region IComparable
        

        /// <summary>
        /// The higher the price the higher the rating. in case of equal Prices the 
        /// ower shimanni ID number the higher the rating since orders are served as FIFO
        /// </summary>
        /// <param name="asset1"></param>
        /// <param name="asset2"></param>
        /// <returns></returns>
        private static int BuyingOrdersComparer(ShimanniOrderEntity Order1, ShimanniOrderEntity Order2)
        {
           
            if (Order1.Price > Order2.Price)
                return 1;
            else if (Order1.Price < Order2.Price)
                return -1;
            else
            {
                if (Order1.ShimanniOrderID < Order2.ShimanniOrderID)
                    return 1;
                else if (Order1.ShimanniOrderID > Order2.ShimanniOrderID)
                    return -1;
                else
                    return 0;
            }
        }
        /// <summary>
        /// The lower the price the higher the rating. in case of equal Prices the 
        /// lower shimanni ID number the higher the rating since orders are served as FIFO
        /// </summary>
        /// <param name="asset1"></param>
        /// <param name="asset2"></param>
        /// <returns></returns>
        //private static int SellingOrdersComparer(ShimanniOrderEntity Order1, ShimanniOrderEntity Order2)
        //{
        //    if (Order1.Price < Order2.Price)
        //        return 1;
        //    else if (Order1.Price > Order2.Price)
        //        return -1;
        //    else
        //    {
        //        if (Order1.ShimanniOrderID < Order2.ShimanniOrderID)
        //            return 1;
        //        else if (Order1.ShimanniOrderID > Order2.ShimanniOrderID)
        //            return -1;
        //        else
        //            return 0;
        //    }
        //}
        private static int OrdersComparerInABook(ShimanniOrderEntity Order1, ShimanniOrderEntity Order2)
        {
            int side = (Order1.Side == eSide.Buy ? -1 : +1);
            if (Order1.Price > Order2.Price)
                return 1 * side;
            else if (Order1.Price < Order2.Price)
                return -1 * side;
            else
            {
                if (Order1.ShimanniOrderID < Order2.ShimanniOrderID)
                    return - 1;
                else if (Order1.ShimanniOrderID > Order2.ShimanniOrderID)
                    return +1;
                else
                    return 0;
            }
        }
        public static Comparison<ShimanniOrderEntity> OrdersComparisonInABook =

        delegate(ShimanniOrderEntity Order1, ShimanniOrderEntity Order2)
        {
            return OrdersComparerInABook(Order1, Order2);
        };



        //public static Comparison<ShimanniOrderEntity> SellingOrdersComparison =

        //delegate(ShimanniOrderEntity Order1, ShimanniOrderEntity Order2)
        //{
        //    return SellingOrdersComparer(Order1, Order2);
        //};


        public static Comparison<ShimanniOrderEntity> BuyingOrdersComparison =

        delegate(ShimanniOrderEntity Order1, ShimanniOrderEntity Order2)
        {
            return BuyingOrdersComparer(Order1, Order2);
        };


        public int CompareTo(object other)
        {
            return Price.CompareTo(((ShimanniOrderEntity)other).Price);
        }


        /// <summary>
        /// when orders have the same type we use there magnitude to rate them 
        /// </summary>
        /// <param name="Order1"></param>
        /// <param name="Order2"></param>
        /// <param name="?"></param>
        /// <returns></returns>

        private static int QueueComparerWhenTypeIsTheSame(ShimanniOrderEntity Order1, ShimanniOrderEntity Order2)
        {
            AssetEntity Asset1 = Order1.ParentAsset;
            AssetEntity Asset2 = Order2.ParentAsset;
            if (Asset1.Beta * Order1.Price * Order1.Remains * Asset1.Multiplayer / Asset1.Beta > Asset2.Beta * Order2.Price * Order2.Remains * Asset2.Multiplayer / Asset2.Beta)
                return 1;
            else if (Asset1.Beta * Order1.Price * Order1.Remains * Asset1.Multiplayer / Asset1.Beta < Asset2.Beta * Order2.Price * Order2.Remains * Asset2.Multiplayer / Asset2.Beta)
                return -1;
            else
                return 0;
        }

        private static int QueuePriorety(ShimanniOrderEntity order)
        {
            if (order.Type == eShimanniOrderType.SoftHedging || order.Type == eShimanniOrderType.AgresiveHedging)
                return 10;
            else if (order.Status == eOrderStatus.QueingForCancel)
                return 9;
            else if (order.Type == eShimanniOrderType.MM)
                return 8;
            else if (order.Type == eShimanniOrderType.Closing)
                return 7;
            else if (order.Type == eShimanniOrderType.Opening)
                return 6;
            else
                return 0;

        }
        private static int QueueComparer(ShimanniOrderEntity Order1, ShimanniOrderEntity Order2)
        {
            int PriorityOfOrder1 = QueuePriorety(Order1);
            int PriorityOfOrder2 = QueuePriorety(Order2);

            if (PriorityOfOrder1 > PriorityOfOrder2)
                return 1;
            else if (PriorityOfOrder1 < PriorityOfOrder2)
                return -1;
            else
                return QueueComparerWhenTypeIsTheSame(Order1, Order2);
        }
        public static Comparison<ShimanniOrderEntity> QueueComparison =

     delegate(ShimanniOrderEntity Order1, ShimanniOrderEntity Order2)
     {
         return QueueComparer(Order1, Order2);
     };



        #endregion

        #region Methods
        public abstract void RomoveOrderFromLists();
        
        private void WriteOrderToLogFile(string variable, string vlaue)
        {
            if (TraderLog.StreamWriterForTradingLog != null)
            {
                string NewLine = DateTime.Now.ToString("hh:mm:ss.ffff") + "\t" +
                        "Order" + "\t" + variable + ":" + "\t" + vlaue + "\t" + Symbol + "\t" + "ID" + "\t" + ShimanniOrderID + "\t" + Side.ToString();
                TraderLog.StreamWriterForTradingLog.WriteLine(NewLine);
                TraderLog.StreamWriterForTradingLog.Flush();
            }

        }
        #endregion
    }
}
