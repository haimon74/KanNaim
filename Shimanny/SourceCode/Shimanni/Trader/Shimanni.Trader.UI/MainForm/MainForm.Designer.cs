namespace Shimanni.Trader.UI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.DownloadShorts = new System.Windows.Forms.Button();
            this.ChooseTWSPlatform = new System.Windows.Forms.ComboBox();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.TimePicker = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.Load_Proshares_NAV = new System.Windows.Forms.Button();
            this.StategiesState = new System.Windows.Forms.Button();
            this.UpdatePortfolioPostions = new System.Windows.Forms.Button();
            this.OpenMarketSimulationWIndow = new System.Windows.Forms.Button();
            this.ConectToIB = new System.Windows.Forms.Button();
            this.UpdateMarketDataSubscription = new System.Windows.Forms.Button();
            this.DebugingTextBox = new System.Windows.Forms.TextBox();
            this.LoadParametersButton = new System.Windows.Forms.Button();
            this.SaveParametersToXML = new System.Windows.Forms.Button();
            this.AddNewAssetToStrategy = new System.Windows.Forms.Button();
            this.AddNewStrategy = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.assetsListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.strategyBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.strategiesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.strategiesListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.strategiesListBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.assetsListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.strategyBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.strategiesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.strategiesListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.strategiesListBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.DownloadShorts);
            this.panel1.Controls.Add(this.ChooseTWSPlatform);
            this.panel1.Controls.Add(this.dateTimePicker2);
            this.panel1.Controls.Add(this.TimePicker);
            this.panel1.Controls.Add(this.dateTimePicker1);
            this.panel1.Controls.Add(this.Load_Proshares_NAV);
            this.panel1.Controls.Add(this.StategiesState);
            this.panel1.Controls.Add(this.UpdatePortfolioPostions);
            this.panel1.Controls.Add(this.OpenMarketSimulationWIndow);
            this.panel1.Controls.Add(this.ConectToIB);
            this.panel1.Controls.Add(this.UpdateMarketDataSubscription);
            this.panel1.Controls.Add(this.DebugingTextBox);
            this.panel1.Controls.Add(this.LoadParametersButton);
            this.panel1.Controls.Add(this.SaveParametersToXML);
            this.panel1.Controls.Add(this.AddNewAssetToStrategy);
            this.panel1.Controls.Add(this.AddNewStrategy);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1156, 140);
            this.panel1.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(758, 16);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(112, 20);
            this.textBox1.TabIndex = 17;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // DownloadShorts
            // 
            this.DownloadShorts.Location = new System.Drawing.Point(151, 54);
            this.DownloadShorts.Name = "DownloadShorts";
            this.DownloadShorts.Size = new System.Drawing.Size(75, 23);
            this.DownloadShorts.TabIndex = 16;
            this.DownloadShorts.Text = "DownloadShorts";
            this.DownloadShorts.UseVisualStyleBackColor = true;
            this.DownloadShorts.Click += new System.EventHandler(this.DownloadShorts_Click);
            // 
            // ChooseTWSPlatform
            // 
            this.ChooseTWSPlatform.FormattingEnabled = true;
            this.ChooseTWSPlatform.Items.AddRange(new object[] {
            "Devolping",
            "Paper Trading",
            "Real Trading"});
            this.ChooseTWSPlatform.Location = new System.Drawing.Point(595, 105);
            this.ChooseTWSPlatform.Name = "ChooseTWSPlatform";
            this.ChooseTWSPlatform.Size = new System.Drawing.Size(121, 21);
            this.ChooseTWSPlatform.TabIndex = 15;
            this.ChooseTWSPlatform.SelectedIndexChanged += new System.EventHandler(this.ChooseTWSPlatform_SelectedIndexChanged);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "DTS_TIMEFORMAT";
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker2.Location = new System.Drawing.Point(547, 11);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.ShowUpDown = true;
            this.dateTimePicker2.Size = new System.Drawing.Size(86, 20);
            this.dateTimePicker2.TabIndex = 14;
            // 
            // TimePicker
            // 
            this.TimePicker.CustomFormat = "DTS_TIMEFORMAT";
            this.TimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.TimePicker.Location = new System.Drawing.Point(547, 40);
            this.TimePicker.Name = "TimePicker";
            this.TimePicker.ShowUpDown = true;
            this.TimePicker.Size = new System.Drawing.Size(86, 20);
            this.TimePicker.TabIndex = 13;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(547, 67);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 11;
            this.dateTimePicker1.Value = new System.DateTime(2009, 6, 19, 0, 0, 0, 0);
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // Load_Proshares_NAV
            // 
            this.Load_Proshares_NAV.Location = new System.Drawing.Point(382, 98);
            this.Load_Proshares_NAV.Name = "Load_Proshares_NAV";
            this.Load_Proshares_NAV.Size = new System.Drawing.Size(118, 34);
            this.Load_Proshares_NAV.TabIndex = 10;
            this.Load_Proshares_NAV.Text = "Load Proshares NAV";
            this.Load_Proshares_NAV.UseVisualStyleBackColor = true;
            this.Load_Proshares_NAV.Click += new System.EventHandler(this.Load_Proshares_NAV_Click);
            // 
            // StategiesState
            // 
            this.StategiesState.Location = new System.Drawing.Point(385, 54);
            this.StategiesState.Name = "StategiesState";
            this.StategiesState.Size = new System.Drawing.Size(109, 33);
            this.StategiesState.TabIndex = 9;
            this.StategiesState.Text = "StategiesState";
            this.StategiesState.UseVisualStyleBackColor = true;
            this.StategiesState.Click += new System.EventHandler(this.ChangeStrategiesState_Click);
            // 
            // UpdatePortfolioPostions
            // 
            this.UpdatePortfolioPostions.Location = new System.Drawing.Point(265, 98);
            this.UpdatePortfolioPostions.Name = "UpdatePortfolioPostions";
            this.UpdatePortfolioPostions.Size = new System.Drawing.Size(109, 33);
            this.UpdatePortfolioPostions.TabIndex = 8;
            this.UpdatePortfolioPostions.Text = "Update Portfolio Postions";
            this.UpdatePortfolioPostions.UseVisualStyleBackColor = true;
            this.UpdatePortfolioPostions.Click += new System.EventHandler(this.UpdatePortfolioPostions_Click);
            // 
            // OpenMarketSimulationWIndow
            // 
            this.OpenMarketSimulationWIndow.Location = new System.Drawing.Point(385, 8);
            this.OpenMarketSimulationWIndow.Name = "OpenMarketSimulationWIndow";
            this.OpenMarketSimulationWIndow.Size = new System.Drawing.Size(109, 33);
            this.OpenMarketSimulationWIndow.TabIndex = 7;
            this.OpenMarketSimulationWIndow.Text = "Open Market Simulation WIndow";
            this.OpenMarketSimulationWIndow.UseVisualStyleBackColor = true;
            // 
            // ConectToIB
            // 
            this.ConectToIB.Location = new System.Drawing.Point(263, 54);
            this.ConectToIB.Name = "ConectToIB";
            this.ConectToIB.Size = new System.Drawing.Size(109, 33);
            this.ConectToIB.TabIndex = 6;
            this.ConectToIB.Text = "ConectToIB";
            this.ConectToIB.UseVisualStyleBackColor = true;
            this.ConectToIB.Click += new System.EventHandler(this.ConectToIB_Click);
            // 
            // UpdateMarketDataSubscription
            // 
            this.UpdateMarketDataSubscription.Location = new System.Drawing.Point(265, 7);
            this.UpdateMarketDataSubscription.Name = "UpdateMarketDataSubscription";
            this.UpdateMarketDataSubscription.Size = new System.Drawing.Size(109, 33);
            this.UpdateMarketDataSubscription.TabIndex = 5;
            this.UpdateMarketDataSubscription.Text = "Update Market Data Subscription";
            this.UpdateMarketDataSubscription.UseVisualStyleBackColor = true;
            this.UpdateMarketDataSubscription.Click += new System.EventHandler(this.UpdateMarketDataSubscription_Click);
            // 
            // DebugingTextBox
            // 
            this.DebugingTextBox.Location = new System.Drawing.Point(968, 0);
            this.DebugingTextBox.Multiline = true;
            this.DebugingTextBox.Name = "DebugingTextBox";
            this.DebugingTextBox.ReadOnly = true;
            this.DebugingTextBox.Size = new System.Drawing.Size(346, 100);
            this.DebugingTextBox.TabIndex = 4;
            // 
            // LoadParametersButton
            // 
            this.LoadParametersButton.Location = new System.Drawing.Point(12, 98);
            this.LoadParametersButton.Name = "LoadParametersButton";
            this.LoadParametersButton.Size = new System.Drawing.Size(109, 33);
            this.LoadParametersButton.TabIndex = 3;
            this.LoadParametersButton.Text = "Load Parameters";
            this.LoadParametersButton.UseVisualStyleBackColor = true;
            this.LoadParametersButton.Click += new System.EventHandler(this.LoadParametersButton_Click);
            // 
            // SaveParametersToXML
            // 
            this.SaveParametersToXML.Location = new System.Drawing.Point(135, 7);
            this.SaveParametersToXML.Name = "SaveParametersToXML";
            this.SaveParametersToXML.Size = new System.Drawing.Size(109, 33);
            this.SaveParametersToXML.TabIndex = 2;
            this.SaveParametersToXML.Text = "Save Parameters";
            this.SaveParametersToXML.UseVisualStyleBackColor = true;
            this.SaveParametersToXML.Click += new System.EventHandler(this.SaveParametersToXML_Click);
            // 
            // AddNewAssetToStrategy
            // 
            this.AddNewAssetToStrategy.Location = new System.Drawing.Point(12, 54);
            this.AddNewAssetToStrategy.Name = "AddNewAssetToStrategy";
            this.AddNewAssetToStrategy.Size = new System.Drawing.Size(109, 33);
            this.AddNewAssetToStrategy.TabIndex = 1;
            this.AddNewAssetToStrategy.Text = "Add New Asset";
            this.AddNewAssetToStrategy.UseVisualStyleBackColor = true;
            this.AddNewAssetToStrategy.Click += new System.EventHandler(this.AddNewAssetToStrategy_Click);
            // 
            // AddNewStrategy
            // 
            this.AddNewStrategy.Location = new System.Drawing.Point(12, 7);
            this.AddNewStrategy.Name = "AddNewStrategy";
            this.AddNewStrategy.Size = new System.Drawing.Size(109, 33);
            this.AddNewStrategy.TabIndex = 0;
            this.AddNewStrategy.Text = "Add New Strategy";
            this.AddNewStrategy.UseVisualStyleBackColor = true;
            this.AddNewStrategy.Click += new System.EventHandler(this.AddNewStrategy_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 140);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Size = new System.Drawing.Size(1156, 434);
            this.splitContainer1.SplitterDistance = 367;
            this.splitContainer1.TabIndex = 1;
            // 
            // assetsListBindingSource
            // 
            this.assetsListBindingSource.DataMember = "AssetsList";
            // 
            // strategyBindingSource
            // 
            this.strategyBindingSource.DataSource = typeof(Shimanni.Trader.BL.Strategy);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1156, 574);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.Text = "IBTraderAppication";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.assetsListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.strategyBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.strategiesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.strategiesListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.strategiesListBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button AddNewStrategy;
        private System.Windows.Forms.Button AddNewAssetToStrategy;
        private System.Windows.Forms.DataGridViewTextBoxColumn currentlExposureDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button SaveParametersToXML;
        private System.Windows.Forms.Button LoadParametersButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxStrategyExposureDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxTradeExposureDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxMMExposureDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn maxHedgingExposureDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn strategyStateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn profitDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn rowIndexDataGridViewTextBoxColumn;
        //private Shimanni.MultiAssets.StrategiesPortfolio.ShimanniDataSet shimanniDataSet;
        private System.Windows.Forms.BindingSource strategiesBindingSource;
        private System.Windows.Forms.BindingSource strategiesListBindingSource1;
        private System.Windows.Forms.BindingSource strategiesListBindingSource;
        private System.Windows.Forms.BindingSource assetsListBindingSource;
        private System.Windows.Forms.BindingSource strategyBindingSource;
        private System.Windows.Forms.Button UpdateMarketDataSubscription;
        private System.Windows.Forms.Button ConectToIB;
        private System.Windows.Forms.Button OpenMarketSimulationWIndow;
        public System.Windows.Forms.TextBox DebugingTextBox;
        private System.Windows.Forms.Button UpdatePortfolioPostions;
        private System.Windows.Forms.Button StategiesState;
        private System.Windows.Forms.Button Load_Proshares_NAV;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DateTimePicker TimePicker;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.ComboBox ChooseTWSPlatform;
        private System.Windows.Forms.Button DownloadShorts;
        private System.Windows.Forms.TextBox textBox1;
    }
}


    
