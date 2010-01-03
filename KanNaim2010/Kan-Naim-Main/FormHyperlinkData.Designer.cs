namespace Kan_Naim_Main
{
    partial class FormHyperlinkData
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
            this.button1 = new System.Windows.Forms.Button();
            this.labelHyperlinkName = new System.Windows.Forms.Label();
            this.labelHyperlinkURL = new System.Windows.Forms.Label();
            this.labelHyperlinkText = new System.Windows.Forms.Label();
            this.textBoxHyperlinkText = new System.Windows.Forms.TextBox();
            this.textBoxHref = new System.Windows.Forms.TextBox();
            this.textBoxUrlName = new System.Windows.Forms.TextBox();
            this.checkBoxTarget = new System.Windows.Forms.CheckBox();
            this.buttonDelUrl = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(362, 101);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "שמור קישור";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelHyperlinkName
            // 
            this.labelHyperlinkName.AutoSize = true;
            this.labelHyperlinkName.Location = new System.Drawing.Point(12, 71);
            this.labelHyperlinkName.Name = "labelHyperlinkName";
            this.labelHyperlinkName.Size = new System.Drawing.Size(62, 13);
            this.labelHyperlinkName.TabIndex = 1;
            this.labelHyperlinkName.Text = "טקסט נוסף";
            // 
            // labelHyperlinkURL
            // 
            this.labelHyperlinkURL.AutoSize = true;
            this.labelHyperlinkURL.Location = new System.Drawing.Point(45, 41);
            this.labelHyperlinkURL.Name = "labelHyperlinkURL";
            this.labelHyperlinkURL.Size = new System.Drawing.Size(29, 13);
            this.labelHyperlinkURL.TabIndex = 2;
            this.labelHyperlinkURL.Text = "URL";
            // 
            // labelHyperlinkText
            // 
            this.labelHyperlinkText.AutoSize = true;
            this.labelHyperlinkText.Location = new System.Drawing.Point(11, 15);
            this.labelHyperlinkText.Name = "labelHyperlinkText";
            this.labelHyperlinkText.Size = new System.Drawing.Size(63, 13);
            this.labelHyperlinkText.TabIndex = 3;
            this.labelHyperlinkText.Text = "טקסט מוצג";
            // 
            // textBoxHyperlinkText
            // 
            this.textBoxHyperlinkText.Enabled = false;
            this.textBoxHyperlinkText.Location = new System.Drawing.Point(80, 8);
            this.textBoxHyperlinkText.Name = "textBoxHyperlinkText";
            this.textBoxHyperlinkText.ReadOnly = true;
            this.textBoxHyperlinkText.Size = new System.Drawing.Size(618, 20);
            this.textBoxHyperlinkText.TabIndex = 4;
            this.textBoxHyperlinkText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxHref
            // 
            this.textBoxHref.Location = new System.Drawing.Point(80, 38);
            this.textBoxHref.Name = "textBoxHref";
            this.textBoxHref.Size = new System.Drawing.Size(618, 20);
            this.textBoxHref.TabIndex = 5;
            // 
            // textBoxUrlName
            // 
            this.textBoxUrlName.Location = new System.Drawing.Point(80, 68);
            this.textBoxUrlName.Name = "textBoxUrlName";
            this.textBoxUrlName.Size = new System.Drawing.Size(618, 20);
            this.textBoxUrlName.TabIndex = 6;
            this.textBoxUrlName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // checkBoxTarget
            // 
            this.checkBoxTarget.AutoSize = true;
            this.checkBoxTarget.Location = new System.Drawing.Point(80, 101);
            this.checkBoxTarget.Name = "checkBoxTarget";
            this.checkBoxTarget.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxTarget.Size = new System.Drawing.Size(167, 17);
            this.checkBoxTarget.TabIndex = 7;
            this.checkBoxTarget.Tag = "target=\'_self\'";
            this.checkBoxTarget.Text = "אל תפתח קישור בחלון חדש";
            this.checkBoxTarget.UseVisualStyleBackColor = true;
            // 
            // buttonDelUrl
            // 
            this.buttonDelUrl.Location = new System.Drawing.Point(575, 101);
            this.buttonDelUrl.Name = "buttonDelUrl";
            this.buttonDelUrl.Size = new System.Drawing.Size(123, 23);
            this.buttonDelUrl.TabIndex = 8;
            this.buttonDelUrl.Text = "מחק קישור";
            this.buttonDelUrl.UseVisualStyleBackColor = true;
            this.buttonDelUrl.Click += new System.EventHandler(this.buttonDelUrl_Click);
            // 
            // FormHyperlinkData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 132);
            this.Controls.Add(this.buttonDelUrl);
            this.Controls.Add(this.checkBoxTarget);
            this.Controls.Add(this.textBoxUrlName);
            this.Controls.Add(this.textBoxHref);
            this.Controls.Add(this.textBoxHyperlinkText);
            this.Controls.Add(this.labelHyperlinkText);
            this.Controls.Add(this.labelHyperlinkURL);
            this.Controls.Add(this.labelHyperlinkName);
            this.Controls.Add(this.button1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormHyperlinkData";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "הזנת פרטי קישור";
            this.Load += new System.EventHandler(this.FormHyperlinkData_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Label labelHyperlinkName;
        private System.Windows.Forms.Label labelHyperlinkURL;
        private System.Windows.Forms.Label labelHyperlinkText;
        public System.Windows.Forms.TextBox textBoxHyperlinkText;
        public System.Windows.Forms.TextBox textBoxHref;
        public System.Windows.Forms.TextBox textBoxUrlName;
        public System.Windows.Forms.CheckBox checkBoxTarget;
        private System.Windows.Forms.Button buttonDelUrl;
    }
}