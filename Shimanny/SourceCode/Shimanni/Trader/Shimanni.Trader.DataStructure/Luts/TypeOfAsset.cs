using System;
using System.Collections.Generic;
using System.Text;

namespace Shimanni.Trader.DataStructure
{
    public enum eTypeOfAsset
    {
        ChooseType = 0,
        Synthetic = 1,
        Tradable = 2,
        NotTradable = 3,
        Option = 4,
        Future = 5,
        Equity = 6,   
    }
}
