namespace HaimDLL
{
    partial class UserControlTakFill
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ucTakBroadcastCmd1 = new HaimDLL.UserControlTakBroadcastCmd();
            this.ucTakContent1 = new HaimDLL.UserControlTakContent();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ucTakBroadcastCmd1);
            this.groupBox1.Controls.Add(this.ucTakContent1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(628, 437);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "מילוי תקציר";
            // 
            // ucTakBroadcastCmd1
            // 
            this.ucTakBroadcastCmd1.Location = new System.Drawing.Point(6, 209);
            this.ucTakBroadcastCmd1.Name = "ucTakBroadcastCmd1";
            this.ucTakBroadcastCmd1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ucTakBroadcastCmd1.Size = new System.Drawing.Size(611, 216);
            this.ucTakBroadcastCmd1.TabIndex = 1;
            // 
            // ucTakContent1
            // 
            this.ucTakContent1.Location = new System.Drawing.Point(6, 19);
            this.ucTakContent1.Name = "ucTakContent1";
            this.ucTakContent1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ucTakContent1.Size = new System.Drawing.Size(613, 184);
            this.ucTakContent1.TabIndex = 0;
            this.ucTakContent1.Load += new System.EventHandler(this.ucTakContent1_Load);
            // 
            // UserControlTakFill
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "UserControlTakFill";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(635, 445);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public UserControlTakContent ucTakContent1;
        private System.Windows.Forms.GroupBox groupBox1;
        public UserControlTakBroadcastCmd ucTakBroadcastCmd1;
    }
}
