using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Xml;


using Shimanni.Trader.DataStructure;
using Shimanni.Trader.BL;
using Shimanni.Common.Utils;

using Krs.Ats.IBNet;
using Krs.Ats.IBNet.Contracts;
using Krs.Ats.IBNet.Orders;

using Shimanni.Common.Desktop;
using Shimanni.Trader.Common;

namespace Shimanni.Trader.UI
{
    public partial class MainForm : Form
    {
        private string ProsharesNAVDate;
        List<CaptionedGrid> ControlLists = new List<CaptionedGrid>();
        CaptionedGrid AssetControl = new CaptionedGrid();
        CaptionedGrid strategyControl = new CaptionedGrid();
        CaptionedGrid marketDataControl = new CaptionedGrid();
        TabControl TabControl1 = new TabControl();
        TabPage inputData = new TabPage("Input Data");
        TabPage MarketData = new TabPage("Market Data");
        HorizentalSplit HorzSplit1 = new HorizentalSplit();
        // TableLayoutPanel xxx = new TableLayoutPanel();
        FlowLayoutPanel xxx = new FlowLayoutPanel();        
        BindingList<LevelOneMarketDataEntity> marketDataList = new BindingList<LevelOneMarketDataEntity>();
        static System.Threading.Timer FTPTimer = new System.Threading.Timer(new TimerCallback(DownloadShortsFromIB));
        static Thread updateMarketDataThird = new Thread(IBMarketDataManagement.UpdateMarketDataSubscriptionList);


        /// <summary>
        /// The last time we donwload the shrot file from IB
        /// </summary>
        static DateTime ShortUpdateTimeOnComputer = new DateTime(1, 1, 1);



        static object locker = new object();
        static int TWSport;


        public MainForm()
        {
            InitializeComponent();
            creatStrategyManagementGrid();
            SetPanelDisplayWithHorizentalAndVerticealSplits();
            ShowAssetTable();
            ShowMarketdataTable();
            TraderLog.BeginStreams();
            IBConectivityManagement.RegisterIBEvents();
            IBConectivityManagement.Client.Error += WriteErorrsMessageText;
            IBConectivityManagement.Client.ConnectionClosed += new EventHandler<ConnectionClosedEventArgs>(Client_ConnectionClosed);
            ChooseTWSPlatform.DataSource = Enum.GetValues(typeof(eSetupOfPlatformPort));

        }





        void Client_ConnectionClosed(object sender, ConnectionClosedEventArgs e)
        {
            
            ConectionStatusMessageTextBox();
        }

        private delegate void ConectionStatusChangedToClosed();

        private void ConectionStatusMessageTextBox()
        {
            if (ConectToIB.InvokeRequired)
            {
                ConectionStatusChangedToClosed method = ConectionStatusMessageTextBox;
                ConectToIB.Invoke(method);
            }
            else
            {

                ConectToIB.BackColor = Color.Red;
               
                if (IBConectivityManagement.RequestedToDisconect)
                {
                    Console.Beep(2000, 200);
                    ConectToIB.Text = "IB Disconneted Succesfuly";
                    ConectToIB.BackColor = Color.Red;
                    IBConectivityManagement.RequestedToDisconect =false;
                }
                else
                {
                    Console.Beep(2000, 200);
                    ConectToIB.Text = "IB conection was lost";
                }
                
            }

        }


        private static void DownloadShortsFromIB(object timer)
        {
            UpdateShorts();
        }


        public static void UpdateShorts()
        {
            Monitor.Enter(locker);
            FileStream ShortStockFile = new FileStream(@TraderLog.filePathToUSAShorts, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr = new StreamReader(ShortStockFile);

            try
            {
                string st = "";

                st = sr.ReadLine();
                int skipTwoFirstRows = 0;
                while (st != null)
                {
                    st = sr.ReadLine();
                    skipTwoFirstRows++;


                    if (st == null)
                        break;
                    if (skipTwoFirstRows < 2)
                        continue;
                    int NumberOfShares = 0;
                    string symbol = null;
                    char seperator = '|';
                    int CharIndexInLine = 0;
                    int SeperatoreIndex0 = 0;
                    int SeperatoreIndex1 = 0;
                    int SeperatoreCounter = 0;
                    while (CharIndexInLine < st.Length)
                    {
                        char charAtIndex = st[CharIndexInLine];
                        if (charAtIndex.Equals(seperator) && symbol == null)
                        {
                            symbol = st.Substring(0, CharIndexInLine);
                            SeperatoreCounter = 1;
                            break;
                        }
                        CharIndexInLine++;
                    }

                    while (CharIndexInLine < st.Length)
                    {
                        char charAtIndex = st[CharIndexInLine];
                        if (charAtIndex.Equals(seperator))
                        {
                            SeperatoreCounter++;

                            SeperatoreIndex0 = SeperatoreIndex1;
                            SeperatoreIndex1 = CharIndexInLine;
                            if (SeperatoreCounter == 11)
                            {

                                if (st[SeperatoreIndex0 + 1].Equals('>'))
                                {
                                    NumberOfShares = 10000000;
                                    break;
                                }
                                else
                                {
                                    string numberOfSharesInt = st.Substring(SeperatoreIndex0 + 1, SeperatoreIndex1 - SeperatoreIndex0 - 1);
                                    NumberOfShares = int.Parse(numberOfSharesInt);
                                }
                            }
                        }
                        CharIndexInLine++;
                    }
                    foreach (Strategy strategy in Portfolio.Instance.StrategiesList)
                    {
                        foreach (Asset asset in strategy.AssetsList)
                        {
                            if (asset.SymbolInDataSource.Equals(symbol))
                            {
                                asset.ShortAvaliableAtBroker = NumberOfShares;
                                asset.NetSharesSoldSinceLastUpdatedOfShorts = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (TraderLog.StreamWriterForSystemLog != null)
                {
                    string NewLine = DateTime.Now.ToString("hh:mm:ss.ffff") + "\t" +
                            "Error Message From FILE:" + "\t" + ex.Message + "\tError Source    :" + ex.StackTrace;
                    TraderLog.StreamWriterForSystemLog.WriteLine(NewLine);
                    TraderLog.StreamWriterForSystemLog.Flush();
                }
            }
            finally
            {

                sr.Close();
                ShortStockFile.Close();

                foreach (Strategy strategy in Portfolio.Instance.StrategiesList)
                {
                    foreach (Asset asset in strategy.AssetsList)
                    {
                        if (asset.ShortAvaliableAtBroker == int.MaxValue)
                        {
                            asset.ShortAvaliableAtBroker = 0;
                            asset.NetSharesSoldSinceLastUpdatedOfShorts = 0;
                        }
                    }
                }


                Monitor.Exit(locker);
            }

        }



        #region methods that build the UI

        int StrategyNumberInList = int.MaxValue;
        private void ShowMarketdataTable()
        {

            DataGridView Grid = new DataGridView();
            marketDataList.Clear();
            Grid.AllowUserToAddRows = false;

            if (Portfolio.Instance.StrategiesList.Count != 0)
            {
                foreach (Asset asset in Portfolio.Instance.StrategiesList[StrategyNumberInList].AssetsList)
                {
                    marketDataList.Add(asset.MarketData);
                }
                Grid.DataSource = marketDataList;
            }
            else
                Grid.DataSource = new BindingList<LevelOneMarketData>();

            Grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            Grid.AutoGenerateColumns = false;
            CommonUtils.SeprateGridHeaderTextByUperCase(Grid);




            for (int i = 0; i < Grid.Columns.Count; i++)
            {
                Grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            Grid.Invalidate();


        }

        void ShowOrders()
        {
            if (StrategyNumberInList != int.MaxValue)
            {
                xxx.Controls.Clear();
                foreach (CaptionedGrid item in ControlLists)
                {
                    inputData.Controls.Remove(item);
                }
                ControlLists.Clear();

                for (int i = 0; i < Portfolio.Instance.StrategiesList[StrategyNumberInList].AssetsList.Count; i++)
                {
                    AssetEntity asset = Portfolio.Instance.StrategiesList[StrategyNumberInList].AssetsList[i];
                    asset.CombindList.Clear();
                    xxx.Controls.Add(new CaptionedGrid());
                    CaptionedGrid OrdersListControl = (CaptionedGrid)xxx.Controls[i];
                    OrdersListControl.label1.Text = "Asset: " + asset.SymbolInDataSource;

                    foreach (ShimanniOrder item in asset.Buy.Ordinery)
                    {
                        asset.CombindList.Add(new CombindOrderAndMarketData(item.Price, item.Size, item.Status, eSide.Buy, item.ShimanniOrderID));
                    }

                    foreach (ShimanniOrder item in asset.Sell.Ordinery)
                    {
                        asset.CombindList.Add(new CombindOrderAndMarketData(item.Price, item.Size, item.Status, eSide.Sell, item.ShimanniOrderID));
                    }
                    foreach (ShimanniOrder item in asset.Buy.Hedging)
                    {
                        asset.CombindList.Add(new CombindOrderAndMarketData(item.Price, item.Size, item.Status, eSide.Buy, item.ShimanniOrderID));
                    }
                    foreach (ShimanniOrder item in asset.Sell.Hedging)
                    {
                        asset.CombindList.Add(new CombindOrderAndMarketData(item.Price, item.Size, item.Status, eSide.Sell, item.ShimanniOrderID));
                    }

                    asset.CombindList.Add(new CombindOrderAndMarketData(asset.MarketData.AskPrice, asset.MarketData.AskSize, eOrderStatus.MarketData, eSide.Sell, -1000));
                    asset.CombindList.Add(new CombindOrderAndMarketData(asset.MarketData.BidPrice, asset.MarketData.BidSize, eOrderStatus.MarketData, eSide.Buy, -1000));

                    asset.CombindList.Sort();
                    OrdersListControl.dataGridView1.AutoSize = false;
                    OrdersListControl.dataGridView1.DataSource = asset.CombindList;
                    // OrdersListControl.AutoSizeMode = AutoSizeMode.GrowAndShrink; 
                    OrdersListControl.Width = 300;
                    OrdersListControl.dataGridView1.Columns["Price"].Width = 60;
                    OrdersListControl.dataGridView1.Columns[1].Width = 60;
                    OrdersListControl.dataGridView1.Columns[2].Width = 80;
                    OrdersListControl.dataGridView1.Columns[3].Width = 40;

                    OrdersListControl.Dock = DockStyle.Left;
                    OrdersListControl.Anchor = AnchorStyles.Left;
                    OrdersListControl.BorderStyle = BorderStyle.Fixed3D;
                    // OrdersListControl.AutoSizeMode = AutoSizeMode.GrowAndShrink;

                    OrdersListControl.Invalidate();
                    ControlLists.Add(OrdersListControl);
                    inputData.Controls.Add(OrdersListControl);
                    xxx.Controls.Add(OrdersListControl);


                }

                HorzSplit1.tableLayoutPanel1.Controls.Add(AssetControl, 0, 0);
                HorzSplit1.tableLayoutPanel1.Controls.Add(xxx, 0, 1);
                xxx.Dock = DockStyle.Fill;
                xxx.AutoScroll = true;
                xxx.Show();
                HorzSplit1.tableLayoutPanel1.Show();

            }

        }

        private void ShowAssetTable()
        {
            DataGridView Grid = AssetControl.dataGridView1;

            if (Portfolio.Instance.StrategiesList.Count != 0)
                Grid.DataSource = Portfolio.Instance.StrategiesList[StrategyNumberInList].AssetsList;
            else
                Grid.DataSource = new List<Asset>();

            AssetControl.dataGridView1.AllowUserToDeleteRows = true;
            Grid.Columns.Remove("MarketData");
            // Grid.Columns.Remove("CombindList");
            Grid.Columns.Remove("ParentStrategy");
            Grid.Columns.Remove("Buy");
            Grid.Columns.Remove("Sell");
            Grid.Columns.Remove("SignOfBeta");
            Grid.Columns.Remove("PreStageTwo");
            Grid.Columns.Remove("HedgingSide");
            Grid.Columns.Remove("MaxStrategyExposureUnits");
            Grid.Columns.Remove("MaxMarketMakingExposureUnits");
            Grid.Columns.Remove("MaxTradeExpusureUnits");
            Grid.Columns.Remove("OpeningSpread");
            Grid.Columns.Remove("MMSpread");
            Grid.Columns.Remove("ClosingSpread");
            Grid.Columns.Remove("HedgingSpread");
            Grid.Columns.Remove("StickyPriceLatidude");
            Grid.Columns.Remove("StickySizeLatidude");
            Grid.Columns.Remove("ExpiryDate");
            Grid.Columns.Remove("NetSharesAvelibleForSell");




            ReplaceIntegerColumnWithEnumComboColumn<eBrokerRoute>(Grid, "BrokerRoute", "BrokerRoute");
            ReplaceIntegerColumnWithEnumComboColumn<eExchangeRoute>(Grid, "ExchangeRouteForAddingLiquidityOrder", "ExchangeRouteForAddingLiquidityOrder");
            ReplaceIntegerColumnWithEnumComboColumn<eExchangeRoute>(Grid, "ExchangeRouteForMarketOrder", "ExchangeRouteForMarketOrder");
            ReplaceIntegerColumnWithEnumComboColumn<eTypeOfAsset>(Grid, "TypeOfAsset", "TypeOfAsset");
            ReplaceIntegerColumnWithEnumComboColumn<eDataProvider>(Grid, "DataProvider", "DataProvider");
            //  ReplaceIntegerColumnWithEnumComboColumn<eSide>(Grid, "HedgingSide", "HedgingSide");
            // ReplaceIntegerColumnWithEnumComboColumn<eHedgingState>(Grid, "HedgingState", "HedgingState");


            Grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            Grid.AutoGenerateColumns = false;
            CommonUtils.SeprateGridHeaderTextByUperCase(Grid);
            for (int i = 0; i < Grid.Columns.Count; i++)
            {
                if (Grid.Columns[i].ValueType == typeof(int) || Grid.Columns[i].ValueType == typeof(double))
                    Grid.Columns[i].Width = 60;

            }

            Grid.Invalidate();

        }
        private static void ReplaceIntegerColumnWithEnumComboColumn<EnumType>(DataGridView Grid, string pColToRemove, string pColName)
        {
            DataGridViewComboBoxColumn newCol = new DataGridViewComboBoxColumn();
            Grid.Columns.Remove(pColToRemove);
            // Grid.Columns[pColToRemove].Visible = false;
            newCol.Name = pColName;

            newCol.DataSource = Enum.GetValues(typeof(EnumType));
            //newCol.DataSource = Methods.EnumToList.List<EnumType>();
            newCol.HeaderText = CommonUtils.SeperateStringByUperCase(pColName);
            //   newCol.DisplayMember = "EnumString";
            // newCol.ValueMember = "EnumValue";
            newCol.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            newCol.DataPropertyName = pColToRemove;
            Grid.Columns.Add(newCol);
        }
        public void creatStrategyManagementGrid()
        {

            DataGridView Grid = strategyControl.dataGridView1;
            Grid.DataSource = Portfolio.Instance.StrategiesList;

            //for (int i = 0; i < Grid.Columns.Count; i++)
            //{
            //    Grid.Columns[i].Visible = false;
            //}
            //Grid.Columns["Profit"].Visible = true;
            //Grid.Columns["ExcessStrategyExposure"].Visible = true;
            //Grid.Columns["Name"].Visible = true;
            //Grid.Columns["SubscribeToMarketData"].Visible = true;
            Grid.Columns.Remove("HedgingState");
            Grid.Columns.Remove("SumValueOfHedgingOrders");
            Grid.Columns.Remove("TwoStatgeHedging");
            Grid.Columns.Remove("NetExcessExposure");
            Grid.Columns.Remove("Profit");

            ReplaceIntegerColumnWithEnumComboColumn<eStrategyState>(Grid, "State", "State");
            ReplaceIntegerColumnWithEnumComboColumn<eTypeOfAssetsRelationship>(Grid, "TypeOfAssetsRelationship", "TypeOfAssetsRelationship");
            ReplaceIntegerColumnWithEnumComboColumn<eTypeOfStrategy>(Grid, "TypeOfStrategy", "TypeOfStrategy");
            ReplaceIntegerColumnWithEnumComboColumn<eHedgingRatioMethod>(Grid, "HedgingRatioMethod", "HedgingRatioMethod");



            strategyControl.dataGridView1.AllowUserToDeleteRows = true;
            //strategyControl.dataGridView1.Height =  1000; //to check if it works

            Grid.Columns[0].Width = 60;
            Grid.Columns[1].Width = 60;
            Grid.Columns[2].Width = 60;
            Grid.Columns[3].Width = 140;

            Grid.Columns[4].Width = 60;
            Grid.Columns[5].Width = 60;
            Grid.Columns[6].Width = 60;
            Grid.Columns[7].Width = 60;
            Grid.Columns[8].Width = 60;
            Grid.Columns[9].Width = 60;
            Grid.Columns[10].Width = 60;
            Grid.Columns[12].Width = 140;
            Grid.Columns[13].Width = 60;
            //Grid.Columns[15].Width = 60;
            // Grid.Columns[14].Width = 60;
            //  Grid.Columns[16].Width = 100;


            Grid.RowEnter += new DataGridViewCellEventHandler(this.RowSelectedInStrategyGrid);



            //Grid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ShowAssetTable);
            CommonUtils.SeprateGridHeaderTextByUperCase(Grid);


            Grid.Invalidate();
        }
        public void SetPanelDisplayWithTab()
        {


            splitContainer1.Panel2.Controls.Clear();



            AssetControl.label1.Text = "Asset Table";
            AssetControl.dataGridView1.AllowUserToAddRows = false;
            AssetControl.Dock = DockStyle.Top;
            AssetControl.AutoSizeMode = AutoSizeMode.GrowAndShrink;



            marketDataControl.label1.Text = "Market Data";
            marketDataControl.dataGridView1.AllowUserToAddRows = false;
            marketDataControl.Dock = DockStyle.Top;
            TabControl1.Dock = DockStyle.Fill;


            inputData.Controls.Add(AssetControl);
            //  inputData.Controls.Add(marketDataControl);
            ShowOrders();

            TabControl1.Dock = DockStyle.Fill;
            TabControl1.TabPages.Add(inputData);
            splitContainer1.Panel2.Controls.Add(TabControl1);
            TabControl1.Show();

            splitContainer1.Panel1.Controls.Clear();
            strategyControl.label1.Text = "Strategy Management Control";
            strategyControl.dataGridView1.AllowUserToAddRows = false;
            strategyControl.Dock = DockStyle.Top;

            this.splitContainer1.Panel1.Controls.Add(strategyControl);
            strategyControl.Show();

        }

        public void SetPanelDisplayWithHorizentalAndVerticealSplits()
        {

            splitContainer1.Panel2.Controls.Clear();



            AssetControl.label1.Text = "Asset Table";
            AssetControl.dataGridView1.AllowUserToAddRows = false;
            AssetControl.Dock = DockStyle.Top;
            AssetControl.AutoSizeMode = AutoSizeMode.GrowAndShrink;


            ShowOrders();


            HorzSplit1.Dock = DockStyle.Fill;
            splitContainer1.Panel2.Controls.Add(HorzSplit1); HorzSplit1.Show();


            splitContainer1.Panel1.Controls.Clear();
            strategyControl.label1.Text = "Strategy Management Control";
            strategyControl.dataGridView1.AllowUserToAddRows = false;
            strategyControl.Dock = DockStyle.Top;

            this.splitContainer1.Panel1.Controls.Add(strategyControl);
            strategyControl.Show();

        }
        #endregion


        #region UI driven events


        public void RowSelectedInStrategyGrid(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            StrategyNumberInList = e.RowIndex;

            AssetControl.dataGridView1.DataSource = Portfolio.Instance.StrategiesList[StrategyNumberInList].AssetsList;
            ShowMarketdataTable();
            ShowOrders();
        }

        private static XmlSerializer GetXMLSerializer()
        {
            Type[] myArrayOfTypes = new Type[] { typeof(AssetEntity), typeof(StrategyEntity), typeof(LevelOneMarketDataEntity),
            typeof(ShimanniOrderEntity), typeof(Asset), typeof(Strategy), typeof(LevelOneMarketData),
            typeof(ShimanniOrder), typeof(List<StrategyEntity>), typeof(List<AssetEntity>), typeof(List<Asset>),
            typeof(List<Strategy>), typeof(BindingList<StrategyEntity>), typeof(BindingList<AssetEntity>), typeof(BindingList<Asset>),
            typeof(BindingList<Strategy>)};
            XmlSerializer xs = new XmlSerializer(typeof(BindingList<StrategyEntity>), myArrayOfTypes);
            return xs;
        }


        private void LoadParametersButton_Click(object sender, EventArgs e)
        {


            if (File.Exists(@TraderLog.filePathToParameters) == true)
            {
                StreamReader sr = new StreamReader(@TraderLog.filePathToParameters);
                Portfolio.Instance.StrategiesList = (BindingList<StrategyEntity>)GetXMLSerializer().Deserialize(sr);

                foreach (Strategy strategy in Portfolio.Instance.StrategiesList)
                {                    
                    foreach (Asset asset in strategy.AssetsList)
                    {
                        asset.ParentStrategy = strategy;
                        asset.MarketData.ParentAsset = asset;
                        //asset.OrderBooksLists.ParentAsset = asset;
                    }
                }
                strategyControl.dataGridView1.DataSource = Portfolio.Instance.StrategiesList;

                sr.Close();
                strategyControl.dataGridView1.Refresh();

            }

            //todo: what is it doing here
            DownloadShortsFromIB();
            UpdateShorts();
            Load_Proshares_NAV_E();

            FTPTimer.Change(0, 600000);
        }

        private void SaveParametersToXML_Click(object sender, EventArgs e)
        {
            Monitor.Enter(locker);
            TextWriter Writer = new StreamWriter(@TraderLog.filePathToParameters);
            try
            {
                XmlSerializer xs = GetXMLSerializer();

                xs.Serialize(Writer, Portfolio.Instance.StrategiesList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.InnerException.ToString());
            }
            finally
            {
                Writer.Close();
                Monitor.Exit(locker);
            }
        }

        private void AddNewStrategy_Click(object sender, EventArgs e)
        {
            Portfolio.Instance.StrategiesList.Add(new Strategy(Portfolio.Instance.StrategiesList.Count));

            //to do: to check either this cahnged bug
            strategyControl.dataGridView1.Refresh();
        }


        private void AddNewAssetToStrategy_Click(object sender, EventArgs e)
        {
            if (StrategyNumberInList != int.MaxValue)
            {
                Portfolio.Instance.StrategiesList[StrategyNumberInList].AssetsList.Add(new Asset(Portfolio.Instance.StrategiesList[StrategyNumberInList]));
            }


            AssetControl.dataGridView1.DataSource = Portfolio.Instance.StrategiesList[StrategyNumberInList].AssetsList;


            ShowOrders();
            //ShowMarketdataTable(); 

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void UpdateMarketDataSubscription_Click(object sender, EventArgs e)
        {

            if (IBConectivityManagement.Client.Connected)
            {
                IBMarketDataManagement.UpdateMarketDataSubscriptionList();
                IBConectivityManagement.Client.RequestAccountUpdates(true, IBOrderManagement.Account);
            }
            else
            {
                MessageBox.Show("IB is not conected");
                Console.Beep(2000, 200);
            }
        }

        #endregion



        private void ConectToIB_Click(object sender, EventArgs e)
        {
            //IBConectivityManagement.ConectToIB();
            if (!IBConectivityManagement.Client.Connected)
            {
                try
                {
                    Console.Beep(500, 200);
                    IBConectivityManagement.Client.Connect("127.0.0.1", TWSport, 0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    TraderLog.StreamWriterForTradingLog.WriteLine(DateTime.Now.ToString("hh:mm:ss.ffff") + "Log Type:\t" + ex.Message);
                }
                finally
                {
                    //Thread.Sleep(200);
                    if (IBConectivityManagement.Client.Connected == true)
                    {
                        ConectToIB.Text = "IB Conected";
                        ConectToIB.BackColor = Color.Green;

                        //todo: not the right place for the next loop. better to move it to a method and should be called upon each time
                        foreach (Strategy strategy in Portfolio.Instance.StrategiesList)
                        {
                            foreach (Asset asset in strategy.AssetsList)
                            {
                                asset.PortfolioPositionAtBroker = 0;
                                if (!asset.CalcultedPortfolioPostionWasUtpdated) asset.PortfolioPositionCalculated = 0;
                            }
                        }
                        IBConectivityManagement.Client.RequestAllOpenOrders();
                        updateMarketDataThird.Start();
                    }
                    else
                    {
                        Console.Beep(500, 200);
                        ConectToIB.Text = "Fail To Conect";
                        ConectToIB.BackColor = Color.Red;
                    }
                }
            }
            else
            {
                IBConectivityManagement.Client.Disconnect();
                IBConectivityManagement.RequestedToDisconect = true;
                
            }
        }


        public void WriteErorrsMessageText(object sender, Krs.Ats.IBNet.ErrorEventArgs e)
        {
            WriteMessageToTextBox(e.ErrorMsg);

            //DebugingTextBox.Text.Insert(0,IBConectivityManagement._ErorrMassage + Environment.NewLine);

        }

        private delegate void WriteMessageToTextBoxDelegate(string message);

        private void WriteMessageToTextBox(string message)
        {
            if (DebugingTextBox.InvokeRequired)
            {
                WriteMessageToTextBoxDelegate method = WriteMessageToTextBox;
                DebugingTextBox.Invoke(method, message);
            }
            else
            {
                if (message != "Order Canceled - reason:")
                {
                    if (DebugingTextBox.Text.Length > 1100)
                        DebugingTextBox.Text.Remove(1000);
                    DebugingTextBox.Text = message + Environment.NewLine + DebugingTextBox.Text;
                }
            }
        }

        private void HaltAllStrategy_Click(object sender, EventArgs e)
        {
            if (Portfolio.Instance.StrategiesActivityState == eStrategiesActivityState.Activate_Strategies)
            {
                if (IBConectivityManagement.Client.Connected == true)
                {
                    Portfolio.Instance.StrategiesActivityState = eStrategiesActivityState.Halt_Strategies;
                    HaltAllStrategy.Text = eStrategiesActivityState.Halt_Strategies.ToString();
                }
                else
                {
                    MessageBox.Show("IB Is Not Conected");

                }
            }
            else if (Portfolio.Instance.StrategiesActivityState == eStrategiesActivityState.WaitingForOrdersCancelation)
            {
                MessageBox.Show("Still Waiting For Orders Cancelation");
            }
            else
            {
                foreach (Strategy strategy in Portfolio.Instance.StrategiesList)
                {
                    foreach (Asset asset in strategy.AssetsList)
                    {
                        OrdersManagement.CancelOrderList(asset.Sell.Ordinery);
                        OrdersManagement.CancelOrderList(asset.Buy.Ordinery);
                        OrdersManagement.CancelOrderList(asset.Sell.Hedging);
                        OrdersManagement.CancelOrderList(asset.Buy.Hedging);
                    }



                }

                Portfolio.Instance.StrategiesActivityState = eStrategiesActivityState.Activate_Strategies;
                HaltAllStrategy.Text = eStrategiesActivityState.Activate_Strategies.ToString();
            }
        }

        private void UpdatePortfolioPostions_Click(object sender, EventArgs e)
        {
            //                    UpdatePortfolioPostions();
        }



        private static void RegestrateIBEventsExtracted()
        {

        }

        private void Load_Proshares_NAV_E()
        {
            Monitor.Enter(locker);
            string st = "";

            DateTime lastDateAvaliable = DateTime.Today.AddDays(-1);

            //  DateTime yesterday = today.AddDays(-1);
            //  string ProsharesNAVDate = yesterday.ToString().Substring(0,10);


            WebClient client = new WebClient();
            client.DownloadFile(@"https://accounts.profunds.com/etfdata/historical_nav.csv", @TraderLog.filePathToProsharesNAV);
            FileStream NAVFile = new FileStream(@TraderLog.filePathToProsharesNAV, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr = new StreamReader(NAVFile);

            if (!Directory.Exists(TraderLog.fullPathToStrategyDirectory))
            {
                Directory.CreateDirectory(TraderLog.fullPathToStrategyDirectory);
            }

            try
            {
                foreach (Strategy strategy in Portfolio.Instance.StrategiesList)
                {
                    foreach (Asset asset in strategy.AssetsList)
                    {
                        asset.NavWasUpdated = false;
                    }
                }
                while (true)
                {
                    st = sr.ReadLine();
                    if (st == null)
                        break;

                    string dateOfNAVFile = st.Substring(0, 10);
                    string symbol = null;

                    if (dateOfNAVFile.Equals(ProsharesNAVDate))
                    {
                        int[] commaPositions = new int[4] { -1, -1, -1, -1 };
                        int commaPositionsCounter = 0;
                        for (int i = 0; i < st.Length; i++)
                        {
                            char stemp = st[i];
                            if (stemp.ToString().Equals(",") && commaPositionsCounter < 4)
                            {
                                commaPositions[commaPositionsCounter] = i;
                                commaPositionsCounter++;
                            }
                        }

                        symbol = st.Substring(commaPositions[1] + 1, commaPositions[2] - commaPositions[1] - 1);
                        double NAV = double.Parse(st.Substring(commaPositions[2] + 1, commaPositions[3] - commaPositions[2] - 1));

                        foreach (Strategy strategy in Portfolio.Instance.StrategiesList)
                        {
                            foreach (Asset asset in strategy.AssetsList)
                            {
                                if (asset.SymbolInDataSource.Equals(symbol))
                                {
                                    asset.BasePrice = NAV;
                                    asset.NavWasUpdated = true;
                                    asset.DateOfNAV = DateTime.Parse(dateOfNAVFile);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);

                TraderLog.WriteLineToStream(TraderLog.StreamWriterForSystemLog, ex.StackTrace.ToString());


            }
            finally
            {
                sr.Close();

                string messageText = "Assets which were not updated:\n" +
                                     "-------------------------------";

                foreach (Strategy strategy in Portfolio.Instance.StrategiesList)
                {
                    foreach (Asset asset in strategy.AssetsList)
                    {
                        if (asset.NavWasUpdated == false)
                        {
                            messageText += "Staradegy: " + strategy.Name + "\t\t" + "Asset:\t" + asset.SymbolInBroker + "\n";
                            //MessageBox.Show("Base Price For " + asset.SymbolInBroker + "Was Not Updated");
                        }
                    }
                }

                MessageBox.Show(messageText);

                Monitor.Exit(locker);
            }
        }
        private void Load_Proshares_NAV_Click(object sender, EventArgs e)
        {

            Load_Proshares_NAV_E();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {



            ProsharesNAVDate =
                ((System.Windows.Forms.DateTimePicker)sender).Value.ToString("MM/dd/yyyy");
            //+ "/"
            //+ ((System.Windows.Forms.DateTimePicker)sender).Value.Day.ToString()
            //+ "/"
            //+ ((System.Windows.Forms.DateTimePicker)sender).Value.Year.ToString();

        }





        private void DownloadShortsFromIB()
        {
            Monitor.Enter(locker);

            string ftpUserID = "shortstock";
            string ftpPassword = "shira1";
            FtpWebRequest reqFTP;

            try
            {
                //filePath = <<The full path where the file is to be created.>>, 
                //fileName = <<Name of the file to be created(Need not be the name of the file on FTP server).>>
                if (!Directory.Exists(TraderLog.fullPathToStrategyDirectory))
                {
                    Directory.CreateDirectory(TraderLog.fullPathToStrategyDirectory);
                }

                FileStream outputStream = new FileStream(TraderLog.filePathToUSAShorts, FileMode.Create);
                Uri fileLocation = new Uri("ftp://shortstock@ftp2.interactivebrokers.com/../usa.txt");

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(fileLocation);
                reqFTP.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                reqFTP.KeepAlive = false;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                /// last time short file was updated on server
                DateTime TimeModifiedOnServer = response.LastModified;
                response.Close();


                if (ShortUpdateTimeOnComputer < TimeModifiedOnServer)
                {
                    FtpWebRequest reqFTPDownload;
                    reqFTPDownload = (FtpWebRequest)FtpWebRequest.Create(fileLocation);
                    reqFTPDownload.Method = WebRequestMethods.Ftp.DownloadFile;
                    reqFTPDownload.UseBinary = true;
                    reqFTPDownload.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                    response = (FtpWebResponse)reqFTPDownload.GetResponse();

                    Stream ftpStream = response.GetResponseStream();
                    long cl = response.ContentLength;

                    int bufferSize = 2048;
                    int readCount;
                    byte[] buffer = new byte[bufferSize];
                    string time = reqFTPDownload.Headers.ToString();

                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                    while (readCount > 0)
                    {
                        outputStream.Write(buffer, 0, readCount);
                        readCount = ftpStream.Read(buffer, 0, bufferSize);
                    }

                    ftpStream.Close();
                }
                else
                {
                    TraderLog.WriteLineToStream(TraderLog.StreamWriterForSystemLog, "USA Shorts did not needed updates");

                }


                outputStream.Close();
                response.Close();


            }
            catch (Exception ex)
            {
                if (TraderLog.StreamWriterForSystemLog != null)
                {
                    string NewLine = DateTime.Now.ToString("hh:mm:ss.ffff") + "\t" +
                            "Error Message From FTP:" + "\t" + ex.Message + "\tError Source    :" + ex.StackTrace;
                    TraderLog.StreamWriterForSystemLog.WriteLine(NewLine);
                    TraderLog.StreamWriterForSystemLog.Flush();
                    MessageBox.Show(ex.Message);
                }
            }
            finally
            {
                Monitor.Exit(locker);
            }
        }

        private void ChooseTWSPlatform_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IBConectivityManagement.Client.Connected)
                TWSport = (int)Enum.Parse(typeof(eSetupOfPlatformPort), ChooseTWSPlatform.SelectedItem.ToString());

            switch (TWSport)
            {
                case 7496:
                    IBOrderManagement.Account = "U148521";
                    break;
                case 7497:
                    IBOrderManagement.Account = "DU26593";
                    break;
                case 7498:
                    IBOrderManagement.Account = "DU29632";
                    break;
                default:

                    break;
            }

        }

        private void DownloadShorts_Click(object sender, EventArgs e)
        {
            DownloadShortsFromIB();
            UpdateShorts();
        }

    }
}

