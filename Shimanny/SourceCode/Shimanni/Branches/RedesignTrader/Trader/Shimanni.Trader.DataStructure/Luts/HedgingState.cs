 using System;
using System.Collections.Generic;
using System.Text;

namespace Shimanni.Trader.DataStructure
{
    public enum eHedgingState
    {
        Non,
        StageZeroBuy,
        StageZeroSell,
        StageOneBuy,
        StageOneSell,
        PreStageTwoBuy,
        PreStageTwoSell,
        StageTwoBuy,
        StageTwoSell,

    }
}
