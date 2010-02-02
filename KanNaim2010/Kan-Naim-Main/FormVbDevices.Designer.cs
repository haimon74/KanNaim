namespace Kan_Naim_Main
{
    partial class FormVbDevices
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
            this.buttonRecrdstart = new System.Windows.Forms.Button();
            this.buttonRecordPlay = new System.Windows.Forms.Button();
            this.buttonRecordStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonRecrdstart
            // 
            this.buttonRecrdstart.Location = new System.Drawing.Point(108, 27);
            this.buttonRecrdstart.Name = "buttonRecrdstart";
            this.buttonRecrdstart.Size = new System.Drawing.Size(75, 23);
            this.buttonRecrdstart.TabIndex = 0;
            this.buttonRecrdstart.Text = "הקלט";
            this.buttonRecrdstart.UseVisualStyleBackColor = true;
            this.buttonRecrdstart.Click += new System.EventHandler(this.buttonRecrdstart_Click);
            // 
            // buttonRecordPlay
            // 
            this.buttonRecordPlay.Location = new System.Drawing.Point(108, 190);
            this.buttonRecordPlay.Name = "buttonRecordPlay";
            this.buttonRecordPlay.Size = new System.Drawing.Size(75, 23);
            this.buttonRecordPlay.TabIndex = 1;
            this.buttonRecordPlay.Text = "השמע";
            this.buttonRecordPlay.UseVisualStyleBackColor = true;
            this.buttonRecordPlay.Click += new System.EventHandler(this.buttonRecordPlay_Click);
            // 
            // buttonRecordStop
            // 
            this.buttonRecordStop.Location = new System.Drawing.Point(108, 107);
            this.buttonRecordStop.Name = "buttonRecordStop";
            this.buttonRecordStop.Size = new System.Drawing.Size(75, 23);
            this.buttonRecordStop.TabIndex = 2;
            this.buttonRecordStop.Text = "הפסק";
            this.buttonRecordStop.UseVisualStyleBackColor = true;
            this.buttonRecordStop.Click += new System.EventHandler(this.buttonRecordStop_Click);
            // 
            // FormVbDevices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.buttonRecordStop);
            this.Controls.Add(this.buttonRecordPlay);
            this.Controls.Add(this.buttonRecrdstart);
            this.Name = "FormVbDevices";
            this.Text = "FormVbDevices";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonRecrdstart;
        private System.Windows.Forms.Button buttonRecordPlay;
        private System.Windows.Forms.Button buttonRecordStop;
    }
}