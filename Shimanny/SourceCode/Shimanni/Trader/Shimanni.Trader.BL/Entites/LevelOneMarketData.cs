using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shimanni.Trader.DataStructure;

namespace Shimanni.Trader.BL
{
    public class LevelOneMarketData : LevelOneMarketDataEntity
    {
        public LevelOneMarketData():
            base()
        {
            _ParentAsset = new Asset();
        }

        public LevelOneMarketData(Asset asset) :
            base(asset)
        {
            
        }
    }
}
