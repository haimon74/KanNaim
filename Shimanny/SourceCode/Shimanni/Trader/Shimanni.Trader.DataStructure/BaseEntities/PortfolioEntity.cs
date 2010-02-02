using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shimanni.Common.Data;
using System.ComponentModel;
using Krs.Ats.IBNet;

namespace Shimanni.Trader.DataStructure
{
    public abstract class PortfolioEntity
    {
        public delegate void RegDelegate<T>(AssetEntity asset, T type);

        #region Data Members
        private BindingList<StrategyEntity>  _StrategiesList;
        private eStrategiesActivityState _StrategiesActivityState;
        #endregion        

        #region Constructors
        public PortfolioEntity()
        {
            _StrategiesList = new BindingList<StrategyEntity>();
            _StrategiesActivityState = eStrategiesActivityState.Activate_Strategies;
        }
        #endregion

        #region Properties
        public  eStrategiesActivityState StrategiesActivityState
        {
            get { return _StrategiesActivityState; }
            set { _StrategiesActivityState = value; }
        }        
        public  BindingList<StrategyEntity> StrategiesList
        {
            get
            {
                return _StrategiesList;
            }
            set
            {
                _StrategiesList = value;
            }
        }
        
        public abstract void StartStrategies(BindingList<StrategyEntity> list);
        public abstract bool AssetInputIntegretyCheck();
        #endregion
    }
}
