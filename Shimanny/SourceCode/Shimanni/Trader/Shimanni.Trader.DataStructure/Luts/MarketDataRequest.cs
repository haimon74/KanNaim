using System;
using System.Collections.Generic;
using System.Text;

namespace Shimanni.Trader.DataStructure
{
    public enum eMarketDataRequest: int
    {
        NotSubscribed = 0,
        PendingConfermation = 1,
        Subscribed =2,
    }
}
