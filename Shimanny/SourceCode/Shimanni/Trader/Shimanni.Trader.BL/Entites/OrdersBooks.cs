using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;
using Shimanni.Trader.DataStructure;

namespace Shimanni.Trader.BL.Entites
{
    public class OrdersBooks : OrdersBooksEntity
    {
        /// <summary>
        /// Initializes a new instance of the OrdersBooks class.
        /// </summary>
        public OrdersBooks(): base()
        {
            _ParentAsset = new Asset();
        }
        public OrdersBooks(AssetEntity pParentAsset, eSide pSide): base(pParentAsset, pSide)
        {
            _Side = pSide;
            _ParentAsset = pParentAsset;
        }
        public override int AvaliableForClosing
        {
            get
            {
                    return Math.Max(-(int)_Side * ParentAsset.PortfolioPositionCalculated -      SumOfAll, 0);
            }
        }
        public override void CancelAll()
        {
            for (int j = 0; j < Hedging.Count; j++)
                OrdersManagement.CancelOrder(Hedging[j]);
            for (int i = 0; i < Ordinery.Count; i++)
                OrdersManagement.CancelOrder(Ordinery[i]);
        }
        public override void CancelHedging()
        {
            for (int i = 0; i < Hedging.Count; i++)
                OrdersManagement.CancelOrder(Hedging[i]);
        }
        public override void CancelOrdinery()
        {
            for (int i = 0; i < Ordinery.Count; i++)
                OrdersManagement.CancelOrder(Ordinery[i]);
        }
        public override void CacnelAllQueueingOrders()
        {
            for (int i = 0; i < Ordinery.Count; i++)
                if (Ordinery[i].Status == eOrderStatus.QueingForCancel || Ordinery[i].Status == eOrderStatus.QueingForSubmit)
                    OrdersManagement.CancelOrder(Ordinery[i]);
            for (int j = 0; j < Hedging.Count; j++)
                if (Hedging[j].Status == eOrderStatus.QueingForCancel || Hedging[j].Status == eOrderStatus.QueingForSubmit)
                    OrdersManagement.CancelOrder(Hedging[j]);
        }
    }

}
