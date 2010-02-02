using System;
using System.Collections.Generic;
using System.Text;

namespace Shimanni.Trader.DataStructure
{
    public enum eExchangeRoute: int
    {
        ChooseRoute,
        BrokerGeneralRoute,
        ARCA,
        TelAviv,
        SMART,
        GLOBEX,
        CME,
        CFE,
    }
}
