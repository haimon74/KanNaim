using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shimanni.Trader.DataStructure;
using System.ComponentModel;

namespace Shimanni.Trader.BL
{
    public class Portfolio : PortfolioEntity
    {
        #region Data Members
        private static Portfolio m_Instance = null;
        #endregion

        private double _MaintenenceMarginAtBroker;
        public double MaintenenceMarginAtBroker
        {
            get { return _MaintenenceMarginAtBroker; }
            set { _MaintenenceMarginAtBroker = value; }
        }
        List<Asset> _PortfolioAssets;
        

        public List<Asset> PortfolioAssets
        {
            get
            {
                return _PortfolioAssets;
            }
            set
            {
                _PortfolioAssets = value;
            }
        }
        private string _DebugedSymbol;
        public string DebugedSymbol
        {
            get
            {
                return _DebugedSymbol;
            }
            set
            {
                _DebugedSymbol = value;
            }
        }
        #region Constructors
        public Portfolio(): 
            base()
        {
            MaintenenceMarginAtBroker = double.MaxValue;
            PortfolioAssets = new List<Asset>();
        }
        #endregion

        #region Methods
        public override void StartStrategies(BindingList<StrategyEntity> list)
        {
            for (int i = 0; i < list.Count; i++)
            {

            }
        }
        public override bool AssetInputIntegretyCheck()
        {
            bool integretyConfirmed = true;
            List<AssetEntity> ActiveAssetsList = new List<AssetEntity>();
            foreach (StrategyEntity StrategyEntity in StrategiesList)
            {
                if (StrategyEntity.State != eStrategyState.NotActive)
                    foreach (AssetEntity AssetEntity in StrategyEntity.AssetsList)
                    {
                        ActiveAssetsList.Add(AssetEntity);
                    }
            }
            if (ActiveAssetsList.Count > 0)
            {
                for (int i = 0; i < ActiveAssetsList.Count; i++)
                {

                    for (int j = 0; j < ActiveAssetsList.Count; j++)
                    {
                        if (i != j)
                        {
                            if (ActiveAssetsList[i].SymbolInBroker == ActiveAssetsList[j].SymbolInBroker)
                            {
                                string str = string.Format("'SymbolInBroker' field in both strategies {0} and {1} equal to:{3}",
                                       ActiveAssetsList[i].ParentStrategy.Name, ActiveAssetsList[i].ParentStrategy.Name,
                                       ActiveAssetsList[i].ParentStrategy.Name, ActiveAssetsList[j].ParentStrategy.Name,
                                       ActiveAssetsList[j].SymbolInBroker);
                                integretyConfirmed = false;

                                //MessageBox.Show(str);
                                System.Diagnostics.Trace.WriteLine(str);
                            }
                            if (ActiveAssetsList[i].SymbolInDataSource == ActiveAssetsList[j].SymbolInDataSource)
                            {
                                string str = string.Format("'SymbolInDataSource' field in both strategies {0} and {1} equal to:{3}",
                                      ActiveAssetsList[i].ParentStrategy.Name, ActiveAssetsList[i].ParentStrategy.Name,
                                      ActiveAssetsList[i].ParentStrategy.Name, ActiveAssetsList[j].ParentStrategy.Name,
                                      ActiveAssetsList[j].SymbolInDataSource);
                                integretyConfirmed = false;
                                //MessageBox.Show(str);
                                System.Diagnostics.Trace.WriteLine(str);
                            }
                        }

                    }
                }
            }
            return integretyConfirmed;
        }
        #endregion

        #region Instance
        public static Portfolio Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new Portfolio();

                return m_Instance;
            }
        }
        #endregion
    }
}
