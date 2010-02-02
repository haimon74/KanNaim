using System;
using System.Collections.Generic;
using System.Text;

namespace Shimanni.Trader.DataStructure
{
    public enum eShimanniOrderType: int
    {
        AgresiveHedging,
        SoftHedging,
        MM ,
        Closing ,
        Opening,
        Null ,
    }
}
