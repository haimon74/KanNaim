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

        public OrdersBooks(AssetEntity pParentAsset, eSide pSide): base(pParentAsset, pSide)
        {
            _Side = pSide;
            _ParentAsset = pParentAsset;
        }
    }
}
