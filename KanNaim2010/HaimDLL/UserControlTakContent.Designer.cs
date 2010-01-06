namespace HaimDLL
{
    partial class UserControlTakContent
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
            this.groupBoxTakContent = new System.Windows.Forms.GroupBox();
            this.checkBoxEnableContent = new System.Windows.Forms.CheckBox();
            this.comboBoxTakPhoto = new System.Windows.Forms.ComboBox();
            this.textBoxTakTitle = new System.Windows.Forms.TextBox();
            this.buttonTitleFromContent = new System.Windows.Forms.Button();
            this.checkBoxTakPhoto = new System.Windows.Forms.CheckBox();
            this.textBoxTakContent = new System.Windows.Forms.TextBox();
            this.groupBoxTakContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxTakContent
            // 
            this.groupBoxTakContent.Controls.Add(this.checkBoxEnableContent);
            this.groupBoxTakContent.Controls.Add(this.comboBoxTakPhoto);
            this.groupBoxTakContent.Controls.Add(this.textBoxTakTitle);
            this.groupBoxTakContent.Controls.Add(this.buttonTitleFromContent);
            this.groupBoxTakContent.Controls.Add(this.checkBoxTakPhoto);
            this.groupBoxTakContent.Controls.Add(this.textBoxTakContent);
            this.groupBoxTakContent.Location = new System.Drawing.Point(3, 3);
            this.groupBoxTakContent.Name = "groupBoxTakContent";
            this.groupBoxTakContent.Size = new System.Drawing.Size(605, 177);
            this.groupBoxTakContent.TabIndex = 63;
            this.groupBoxTakContent.TabStop = false;
            this.groupBoxTakContent.Text = "תוכן התקציר";
            // 
            // checkBoxEnableContent
            // 
            this.checkBoxEnableContent.AutoSize = true;
            this.checkBoxEnableContent.Location = new System.Drawing.Point(521, 51);
            this.checkBoxEnableContent.Name = "checkBoxEnableContent";
            this.checkBoxEnableContent.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxEnableContent.Size = new System.Drawing.Size(74, 17);
            this.checkBoxEnableContent.TabIndex = 56;
            this.checkBoxEnableContent.Tag = "";
            this.checkBoxEnableContent.Text = "הזן טקסט";
            this.checkBoxEnableContent.UseVisualStyleBackColor = true;
            this.checkBoxEnableContent.CheckedChanged += new System.EventHandler(this.checkBoxEnableContent_CheckedChanged);
            // 
            // comboBoxTakPhoto
            // 
            this.comboBoxTakPhoto.Enabled = false;
            this.comboBoxTakPhoto.FormattingEnabled = true;
            this.comboBoxTakPhoto.Location = new System.Drawing.Point(18, 151);
            this.comboBoxTakPhoto.Name = "comboBoxTakPhoto";
            this.comboBoxTakPhoto.Size = new System.Drawing.Size(476, 21);
            this.comboBoxTakPhoto.TabIndex = 55;
            // 
            // textBoxTakTitle
            // 
            this.textBoxTakTitle.Location = new System.Drawing.Point(18, 21);
            this.textBoxTakTitle.MaxLength = 150;
            this.textBoxTakTitle.Name = "textBoxTakTitle";
            this.textBoxTakTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBoxTakTitle.Size = new System.Drawing.Size(476, 20);
            this.textBoxTakTitle.TabIndex = 36;
            // 
            // buttonTitleFromContent
            // 
            this.buttonTitleFromContent.Location = new System.Drawing.Point(505, 19);
            this.buttonTitleFromContent.Name = "buttonTitleFromContent";
            this.buttonTitleFromContent.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.buttonTitleFromContent.Size = new System.Drawing.Size(90, 23);
            this.buttonTitleFromContent.TabIndex = 37;
            this.buttonTitleFromContent.Text = "כותרת מתקציר";
            this.buttonTitleFromContent.UseVisualStyleBackColor = true;
            // 
            // checkBoxTakPhoto
            // 
            this.checkBoxTakPhoto.AutoSize = true;
            this.checkBoxTakPhoto.Location = new System.Drawing.Point(509, 155);
            this.checkBoxTakPhoto.Name = "checkBoxTakPhoto";
            this.checkBoxTakPhoto.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxTakPhoto.Size = new System.Drawing.Size(86, 17);
            this.checkBoxTakPhoto.TabIndex = 48;
            this.checkBoxTakPhoto.Tag = "";
            this.checkBoxTakPhoto.Text = "שייך תמונה";
            this.checkBoxTakPhoto.UseVisualStyleBackColor = true;
            this.checkBoxTakPhoto.CheckedChanged += new System.EventHandler(this.checkBoxTakPhoto_CheckedChanged);
            // 
            // textBoxTakContent
            // 
            this.textBoxTakContent.Enabled = false;
            this.textBoxTakContent.Location = new System.Drawing.Point(18, 49);
            this.textBoxTakContent.MaxLength = 1000;
            this.textBoxTakContent.Multiline = true;
            this.textBoxTakContent.Name = "textBoxTakContent";
            this.textBoxTakContent.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBoxTakContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxTakContent.Size = new System.Drawing.Size(476, 96);
            this.textBoxTakContent.TabIndex = 35;
            // 
            // UserControlTakContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxTakContent);
            this.Name = "UserControlTakContent";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(613, 184);
            this.groupBoxTakContent.ResumeLayout(false);
            this.groupBoxTakContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxTakContent;
        public System.Windows.Forms.ComboBox comboBoxTakPhoto;
        private System.Windows.Forms.TextBox textBoxTakTitle;
        private System.Windows.Forms.Button buttonTitleFromContent;
        public System.Windows.Forms.CheckBox checkBoxTakPhoto;
        private System.Windows.Forms.TextBox textBoxTakContent;
        public System.Windows.Forms.CheckBox checkBoxEnableContent;
    }
}
