namespace HaimDLL
{
    partial class UserControlTakBroadcastCmd
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.treeViewTakBroadcastCatSelector = new System.Windows.Forms.TreeView();
            this.buttonSaveBroadcastCmd = new System.Windows.Forms.Button();
            this.checkBoxTakBroadcastAllCatSelector = new System.Windows.Forms.CheckBox();
            this.groupBoxRecursiveCmd = new System.Windows.Forms.GroupBox();
            this.comboBoxTakRecDurationHours = new System.Windows.Forms.ComboBox();
            this.comboBoxTakRecDurationDays = new System.Windows.Forms.ComboBox();
            this.labelTakRecDurationHours = new System.Windows.Forms.Label();
            this.labelTakRecDurationDays = new System.Windows.Forms.Label();
            this.labelTakRepeatStart = new System.Windows.Forms.Label();
            this.radioButtonTakRepeatYearly = new System.Windows.Forms.RadioButton();
            this.radioButtonTakRepeatMonthly = new System.Windows.Forms.RadioButton();
            this.radioButtonTakRepeatWeekly = new System.Windows.Forms.RadioButton();
            this.radioButtonTakRepeatDaily = new System.Windows.Forms.RadioButton();
            this.datePickerTakRecurentStart = new System.Windows.Forms.DateTimePicker();
            this.timePickerTakRecurentStart = new System.Windows.Forms.DateTimePicker();
            this.checkBoxTakStart = new System.Windows.Forms.CheckBox();
            this.checkBoxTakRecursive = new System.Windows.Forms.CheckBox();
            this.timePickerTakStart = new System.Windows.Forms.DateTimePicker();
            this.datePickerTakStart = new System.Windows.Forms.DateTimePicker();
            this.groupBox10.SuspendLayout();
            this.groupBoxRecursiveCmd.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.treeViewTakBroadcastCatSelector);
            this.groupBox10.Controls.Add(this.buttonSaveBroadcastCmd);
            this.groupBox10.Controls.Add(this.checkBoxTakBroadcastAllCatSelector);
            this.groupBox10.Controls.Add(this.groupBoxRecursiveCmd);
            this.groupBox10.Controls.Add(this.checkBoxTakStart);
            this.groupBox10.Controls.Add(this.checkBoxTakRecursive);
            this.groupBox10.Controls.Add(this.timePickerTakStart);
            this.groupBox10.Controls.Add(this.datePickerTakStart);
            this.groupBox10.Location = new System.Drawing.Point(3, 3);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(605, 210);
            this.groupBox10.TabIndex = 62;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "פקודות לשידור";
            // 
            // treeViewTakBroadcastCatSelector
            // 
            this.treeViewTakBroadcastCatSelector.CheckBoxes = true;
            this.treeViewTakBroadcastCatSelector.Enabled = false;
            this.treeViewTakBroadcastCatSelector.Location = new System.Drawing.Point(18, 42);
            this.treeViewTakBroadcastCatSelector.Name = "treeViewTakBroadcastCatSelector";
            this.treeViewTakBroadcastCatSelector.RightToLeftLayout = true;
            this.treeViewTakBroadcastCatSelector.ShowNodeToolTips = true;
            this.treeViewTakBroadcastCatSelector.Size = new System.Drawing.Size(178, 164);
            this.treeViewTakBroadcastCatSelector.TabIndex = 62;
            // 
            // buttonSaveBroadcastCmd
            // 
            this.buttonSaveBroadcastCmd.Location = new System.Drawing.Point(263, 183);
            this.buttonSaveBroadcastCmd.Name = "buttonSaveBroadcastCmd";
            this.buttonSaveBroadcastCmd.Size = new System.Drawing.Size(159, 23);
            this.buttonSaveBroadcastCmd.TabIndex = 61;
            this.buttonSaveBroadcastCmd.Text = "שמור פקודות שידור";
            this.buttonSaveBroadcastCmd.UseVisualStyleBackColor = true;
            this.buttonSaveBroadcastCmd.Visible = false;
            // 
            // checkBoxTakBroadcastAllCatSelector
            // 
            this.checkBoxTakBroadcastAllCatSelector.AutoSize = true;
            this.checkBoxTakBroadcastAllCatSelector.Enabled = false;
            this.checkBoxTakBroadcastAllCatSelector.Location = new System.Drawing.Point(75, 19);
            this.checkBoxTakBroadcastAllCatSelector.Name = "checkBoxTakBroadcastAllCatSelector";
            this.checkBoxTakBroadcastAllCatSelector.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxTakBroadcastAllCatSelector.Size = new System.Drawing.Size(121, 17);
            this.checkBoxTakBroadcastAllCatSelector.TabIndex = 60;
            this.checkBoxTakBroadcastAllCatSelector.Tag = "";
            this.checkBoxTakBroadcastAllCatSelector.Text = "בחר כל הקטגוריות";
            this.checkBoxTakBroadcastAllCatSelector.UseVisualStyleBackColor = true;
            // 
            // groupBoxRecursiveCmd
            // 
            this.groupBoxRecursiveCmd.Controls.Add(this.comboBoxTakRecDurationHours);
            this.groupBoxRecursiveCmd.Controls.Add(this.comboBoxTakRecDurationDays);
            this.groupBoxRecursiveCmd.Controls.Add(this.labelTakRecDurationHours);
            this.groupBoxRecursiveCmd.Controls.Add(this.labelTakRecDurationDays);
            this.groupBoxRecursiveCmd.Controls.Add(this.labelTakRepeatStart);
            this.groupBoxRecursiveCmd.Controls.Add(this.radioButtonTakRepeatYearly);
            this.groupBoxRecursiveCmd.Controls.Add(this.radioButtonTakRepeatMonthly);
            this.groupBoxRecursiveCmd.Controls.Add(this.radioButtonTakRepeatWeekly);
            this.groupBoxRecursiveCmd.Controls.Add(this.radioButtonTakRepeatDaily);
            this.groupBoxRecursiveCmd.Controls.Add(this.datePickerTakRecurentStart);
            this.groupBoxRecursiveCmd.Controls.Add(this.timePickerTakRecurentStart);
            this.groupBoxRecursiveCmd.Enabled = false;
            this.groupBoxRecursiveCmd.Location = new System.Drawing.Point(211, 66);
            this.groupBoxRecursiveCmd.Name = "groupBoxRecursiveCmd";
            this.groupBoxRecursiveCmd.Size = new System.Drawing.Size(283, 111);
            this.groupBoxRecursiveCmd.TabIndex = 59;
            this.groupBoxRecursiveCmd.TabStop = false;
            this.groupBoxRecursiveCmd.Text = "הגדרת השידור המחזורי";
            // 
            // comboBoxTakRecDurationHours
            // 
            this.comboBoxTakRecDurationHours.FormattingEnabled = true;
            this.comboBoxTakRecDurationHours.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24"});
            this.comboBoxTakRecDurationHours.Location = new System.Drawing.Point(34, 83);
            this.comboBoxTakRecDurationHours.Name = "comboBoxTakRecDurationHours";
            this.comboBoxTakRecDurationHours.Size = new System.Drawing.Size(51, 21);
            this.comboBoxTakRecDurationHours.TabIndex = 68;
            // 
            // comboBoxTakRecDurationDays
            // 
            this.comboBoxTakRecDurationDays.FormattingEnabled = true;
            this.comboBoxTakRecDurationDays.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "50",
            "51",
            "52",
            "53",
            "54",
            "55",
            "56",
            "57",
            "58",
            "59",
            "60"});
            this.comboBoxTakRecDurationDays.Location = new System.Drawing.Point(151, 83);
            this.comboBoxTakRecDurationDays.Name = "comboBoxTakRecDurationDays";
            this.comboBoxTakRecDurationDays.Size = new System.Drawing.Size(51, 21);
            this.comboBoxTakRecDurationDays.TabIndex = 67;
            // 
            // labelTakRecDurationHours
            // 
            this.labelTakRecDurationHours.AutoSize = true;
            this.labelTakRecDurationHours.Location = new System.Drawing.Point(91, 86);
            this.labelTakRecDurationHours.Name = "labelTakRecDurationHours";
            this.labelTakRecDurationHours.Size = new System.Drawing.Size(42, 13);
            this.labelTakRecDurationHours.TabIndex = 66;
            this.labelTakRecDurationHours.Text = "בשעות";
            // 
            // labelTakRecDurationDays
            // 
            this.labelTakRecDurationDays.AutoSize = true;
            this.labelTakRecDurationDays.Location = new System.Drawing.Point(204, 86);
            this.labelTakRecDurationDays.Name = "labelTakRecDurationDays";
            this.labelTakRecDurationDays.Size = new System.Drawing.Size(64, 13);
            this.labelTakRecDurationDays.TabIndex = 64;
            this.labelTakRecDurationDays.Text = "משך בימים";
            // 
            // labelTakRepeatStart
            // 
            this.labelTakRepeatStart.AutoSize = true;
            this.labelTakRepeatStart.Location = new System.Drawing.Point(226, 51);
            this.labelTakRepeatStart.Name = "labelTakRepeatStart";
            this.labelTakRepeatStart.Size = new System.Drawing.Size(42, 13);
            this.labelTakRepeatStart.TabIndex = 60;
            this.labelTakRepeatStart.Text = "התחלה";
            // 
            // radioButtonTakRepeatYearly
            // 
            this.radioButtonTakRepeatYearly.AutoSize = true;
            this.radioButtonTakRepeatYearly.Location = new System.Drawing.Point(23, 19);
            this.radioButtonTakRepeatYearly.Name = "radioButtonTakRepeatYearly";
            this.radioButtonTakRepeatYearly.Size = new System.Drawing.Size(51, 17);
            this.radioButtonTakRepeatYearly.TabIndex = 63;
            this.radioButtonTakRepeatYearly.TabStop = true;
            this.radioButtonTakRepeatYearly.Text = "שנתי";
            this.radioButtonTakRepeatYearly.UseVisualStyleBackColor = true;
            // 
            // radioButtonTakRepeatMonthly
            // 
            this.radioButtonTakRepeatMonthly.AutoSize = true;
            this.radioButtonTakRepeatMonthly.Location = new System.Drawing.Point(80, 19);
            this.radioButtonTakRepeatMonthly.Name = "radioButtonTakRepeatMonthly";
            this.radioButtonTakRepeatMonthly.Size = new System.Drawing.Size(58, 17);
            this.radioButtonTakRepeatMonthly.TabIndex = 62;
            this.radioButtonTakRepeatMonthly.TabStop = true;
            this.radioButtonTakRepeatMonthly.Text = "חודשי";
            this.radioButtonTakRepeatMonthly.UseVisualStyleBackColor = true;
            // 
            // radioButtonTakRepeatWeekly
            // 
            this.radioButtonTakRepeatWeekly.AutoSize = true;
            this.radioButtonTakRepeatWeekly.Location = new System.Drawing.Point(144, 19);
            this.radioButtonTakRepeatWeekly.Name = "radioButtonTakRepeatWeekly";
            this.radioButtonTakRepeatWeekly.Size = new System.Drawing.Size(58, 17);
            this.radioButtonTakRepeatWeekly.TabIndex = 61;
            this.radioButtonTakRepeatWeekly.TabStop = true;
            this.radioButtonTakRepeatWeekly.Text = "שבועי";
            this.radioButtonTakRepeatWeekly.UseVisualStyleBackColor = true;
            // 
            // radioButtonTakRepeatDaily
            // 
            this.radioButtonTakRepeatDaily.AutoSize = true;
            this.radioButtonTakRepeatDaily.Location = new System.Drawing.Point(207, 19);
            this.radioButtonTakRepeatDaily.Name = "radioButtonTakRepeatDaily";
            this.radioButtonTakRepeatDaily.Size = new System.Drawing.Size(47, 17);
            this.radioButtonTakRepeatDaily.TabIndex = 60;
            this.radioButtonTakRepeatDaily.TabStop = true;
            this.radioButtonTakRepeatDaily.Text = "יומי";
            this.radioButtonTakRepeatDaily.UseVisualStyleBackColor = true;
            // 
            // datePickerTakRecurentStart
            // 
            this.datePickerTakRecurentStart.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.datePickerTakRecurentStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePickerTakRecurentStart.Location = new System.Drawing.Point(128, 51);
            this.datePickerTakRecurentStart.Name = "datePickerTakRecurentStart";
            this.datePickerTakRecurentStart.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.datePickerTakRecurentStart.RightToLeftLayout = true;
            this.datePickerTakRecurentStart.Size = new System.Drawing.Size(83, 20);
            this.datePickerTakRecurentStart.TabIndex = 45;
            // 
            // timePickerTakRecurentStart
            // 
            this.timePickerTakRecurentStart.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.timePickerTakRecurentStart.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.timePickerTakRecurentStart.Location = new System.Drawing.Point(34, 51);
            this.timePickerTakRecurentStart.Name = "timePickerTakRecurentStart";
            this.timePickerTakRecurentStart.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.timePickerTakRecurentStart.RightToLeftLayout = true;
            this.timePickerTakRecurentStart.Size = new System.Drawing.Size(88, 20);
            this.timePickerTakRecurentStart.TabIndex = 45;
            // 
            // checkBoxTakStart
            // 
            this.checkBoxTakStart.AutoSize = true;
            this.checkBoxTakStart.Location = new System.Drawing.Point(501, 33);
            this.checkBoxTakStart.Name = "checkBoxTakStart";
            this.checkBoxTakStart.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxTakStart.Size = new System.Drawing.Size(94, 17);
            this.checkBoxTakStart.TabIndex = 57;
            this.checkBoxTakStart.Tag = "";
            this.checkBoxTakStart.Text = "תחילת שידור";
            this.checkBoxTakStart.UseVisualStyleBackColor = true;
            this.checkBoxTakStart.CheckedChanged += new System.EventHandler(this.checkBoxTakStart_CheckedChanged);
            // 
            // checkBoxTakRecursive
            // 
            this.checkBoxTakRecursive.AutoSize = true;
            this.checkBoxTakRecursive.Location = new System.Drawing.Point(499, 66);
            this.checkBoxTakRecursive.Name = "checkBoxTakRecursive";
            this.checkBoxTakRecursive.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxTakRecursive.Size = new System.Drawing.Size(96, 17);
            this.checkBoxTakRecursive.TabIndex = 58;
            this.checkBoxTakRecursive.Tag = "";
            this.checkBoxTakRecursive.Text = "שידור מחזורי";
            this.checkBoxTakRecursive.UseVisualStyleBackColor = true;
            this.checkBoxTakRecursive.CheckedChanged += new System.EventHandler(this.checkBoxTakRecursive_CheckedChanged);
            // 
            // timePickerTakStart
            // 
            this.timePickerTakStart.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.timePickerTakStart.Enabled = false;
            this.timePickerTakStart.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.timePickerTakStart.Location = new System.Drawing.Point(317, 30);
            this.timePickerTakStart.Name = "timePickerTakStart";
            this.timePickerTakStart.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.timePickerTakStart.RightToLeftLayout = true;
            this.timePickerTakStart.Size = new System.Drawing.Size(88, 20);
            this.timePickerTakStart.TabIndex = 45;
            // 
            // datePickerTakStart
            // 
            this.datePickerTakStart.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.datePickerTakStart.Enabled = false;
            this.datePickerTakStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePickerTakStart.Location = new System.Drawing.Point(411, 30);
            this.datePickerTakStart.Name = "datePickerTakStart";
            this.datePickerTakStart.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.datePickerTakStart.RightToLeftLayout = true;
            this.datePickerTakStart.Size = new System.Drawing.Size(83, 20);
            this.datePickerTakStart.TabIndex = 45;
            // 
            // UserControlTakBroadcastCmd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox10);
            this.Name = "UserControlTakBroadcastCmd";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(611, 216);
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBoxRecursiveCmd.ResumeLayout(false);
            this.groupBoxRecursiveCmd.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox10;
        public System.Windows.Forms.TreeView treeViewTakBroadcastCatSelector;
        private System.Windows.Forms.Button buttonSaveBroadcastCmd;
        private System.Windows.Forms.CheckBox checkBoxTakBroadcastAllCatSelector;
        private System.Windows.Forms.GroupBox groupBoxRecursiveCmd;
        private System.Windows.Forms.ComboBox comboBoxTakRecDurationHours;
        private System.Windows.Forms.ComboBox comboBoxTakRecDurationDays;
        private System.Windows.Forms.Label labelTakRecDurationHours;
        private System.Windows.Forms.Label labelTakRecDurationDays;
        private System.Windows.Forms.Label labelTakRepeatStart;
        private System.Windows.Forms.RadioButton radioButtonTakRepeatYearly;
        private System.Windows.Forms.RadioButton radioButtonTakRepeatMonthly;
        private System.Windows.Forms.RadioButton radioButtonTakRepeatWeekly;
        private System.Windows.Forms.RadioButton radioButtonTakRepeatDaily;
        private System.Windows.Forms.DateTimePicker datePickerTakRecurentStart;
        private System.Windows.Forms.DateTimePicker timePickerTakRecurentStart;
        private System.Windows.Forms.CheckBox checkBoxTakStart;
        private System.Windows.Forms.CheckBox checkBoxTakRecursive;
        private System.Windows.Forms.DateTimePicker timePickerTakStart;
        private System.Windows.Forms.DateTimePicker datePickerTakStart;
    }
}
