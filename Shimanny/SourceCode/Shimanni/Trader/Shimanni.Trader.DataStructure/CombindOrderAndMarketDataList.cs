using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shimanni.Trader.DataStructure
{
    public class CombindOrderAndMarketData : IComparable
    {
        #region Data Members
        private double _Price;
        private double _Size;
        private eOrderStatus _Status;
        private eSide _Side;
        private int _OrderID;
        #endregion

        #region Constructors
        public CombindOrderAndMarketData(double price, double size, eOrderStatus status, eSide side, int orderID)
        {
            _Price = price;
            _Size = size;
            _Status = status;
            _Side = side;
            OrderID = orderID;
        }
        public CombindOrderAndMarketData()
        {
            _Price = double.MaxValue;
            _Size = double.MaxValue;
            _Status = eOrderStatus.None;
            _Side = eSide.Null;
            OrderID = int.MaxValue;
        }
        #endregion

        #region Properties
        public double Price
        {
            get
            {
                return _Price;
            }
            set
            {
                _Price = value;
            }
        }
        public double Size
        {
            get
            {
                return _Size;
            }
            set
            {
                _Size = value;
            }
        }
        public eOrderStatus Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
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
        public int OrderID
        {
            get
            {
                return _OrderID;
            }
            set
            {
                _OrderID = value;
            }
        }
        #endregion

        #region Static
        public static Comparison<CombindOrderAndMarketData> SizeComperision =
            delegate(CombindOrderAndMarketData Combind1, CombindOrderAndMarketData Combind2)
            {
                return Combind1.Size.CompareTo(Combind2.Size);
            };
        #endregion

        #region Methods
        public int CompareTo(object other)
        {
            return Price.CompareTo(((CombindOrderAndMarketData)other).Price);
        }
        #endregion
    }
    public class CombindOrderAndMarketDataList : List<CombindOrderAndMarketData>
    {


    }

    

}
