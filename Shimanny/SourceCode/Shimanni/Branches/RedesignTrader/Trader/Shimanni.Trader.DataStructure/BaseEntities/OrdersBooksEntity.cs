using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;
using Shimanni.Trader.DataStructure;

namespace Shimanni.Trader.DataStructure
{
    public abstract class OrdersBooksEntity 
    {
        #region Data Members
        protected AssetEntity                         _ParentAsset;
        protected readonly List<ShimanniOrderEntity>  _OrdineryOrders;
        protected readonly List<ShimanniOrderEntity>  _HedgingOrders;
        protected eSide _Side;
        #endregion        
 
        #region Constructors



        /// <summary>
        /// Initializes a new instance of the OrderBooksEntity class.
        /// </summary>
        /// <param name="parentAsset"></param>
        public OrdersBooksEntity(AssetEntity parentAsset ,eSide pSide)
        {
            _Side = pSide;
            _ParentAsset = parentAsset;
            _OrdineryOrders = new List<ShimanniOrderEntity>();
            _HedgingOrders = new List<ShimanniOrderEntity>();
        }
        
        
        
        #endregion

        #region Properties
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
        public List<ShimanniOrderEntity> Ordinery
        {
            get
            {
                return _OrdineryOrders;
            }
        }
        public List<ShimanniOrderEntity> Hedging
        {
            get
            {
                return _HedgingOrders;
            }
        }
        public int SumOfOrdinery
        {
            get
            {
                int sum = 0;
                foreach (ShimanniOrderEntity Order in Ordinery)
                {
                    sum += Order.Remains;
                }
                return sum;
            }
        }
        public int SumOfHedging
        {
            get
            {
                int sum = 0;
                foreach (ShimanniOrderEntity Order in Hedging)
                {
                    sum += Order.Remains;
                }
                return sum;
            }
        }
        public int sumofAll { get { return SumOfOrdinery + SumOfHedging; } }
        public int numOfAll { get { return Ordinery.Count + Hedging.Count; } }
        public eSide Side
        {
            get { return _Side; }
            set { _Side = value; }
        }


   


        #endregion

    }
}
