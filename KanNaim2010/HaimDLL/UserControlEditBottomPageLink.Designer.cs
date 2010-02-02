namespace HaimDLL
{
    partial class UserControlEditBottomPageLink
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
            this.buttonSave = new System.Windows.Forms.Button();
            this.checkBoxLinkStatus = new System.Windows.Forms.CheckBox();
            this.labelVisibleText = new System.Windows.Forms.Label();
            this.textBoxDisplayText = new System.Windows.Forms.TextBox();
            this.textBoxAlternativeText = new System.Windows.Forms.TextBox();
            this.labelAlternativeText = new System.Windows.Forms.Label();
            this.textBoxURL = new System.Windows.Forms.TextBox();
            this.labelURL = new System.Windows.Forms.Label();
            this.comboBoxCategory = new System.Windows.Forms.ComboBox();
            this.labelCategory = new System.Windows.Forms.Label();
            this.labelOrder = new System.Windows.Forms.Label();
            this.textBoxOrder = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(92, 229);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "שמור";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // checkBoxLinkStatus
            // 
            this.checkBoxLinkStatus.AutoSize = true;
            this.checkBoxLinkStatus.Checked = true;
            this.checkBoxLinkStatus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLinkStatus.Location = new System.Drawing.Point(188, 233);
            this.checkBoxLinkStatus.Name = "checkBoxLinkStatus";
            this.checkBoxLinkStatus.Size = new System.Drawing.Size(79, 17);
            this.checkBoxLinkStatus.TabIndex = 1;
            this.checkBoxLinkStatus.Text = "לינק פעיל";
            this.checkBoxLinkStatus.UseVisualStyleBackColor = true;
            // 
            // labelVisibleText
            // 
            this.labelVisibleText.AutoSize = true;
            this.labelVisibleText.Location = new System.Drawing.Point(364, 51);
            this.labelVisibleText.Name = "labelVisibleText";
            this.labelVisibleText.Size = new System.Drawing.Size(63, 13);
            this.labelVisibleText.TabIndex = 2;
            this.labelVisibleText.Text = "טקסט מוצג";
            // 
            // textBoxDisplayText
            // 
            this.textBoxDisplayText.Location = new System.Drawing.Point(23, 44);
            this.textBoxDisplayText.MaxLength = 50;
            this.textBoxDisplayText.Name = "textBoxDisplayText";
            this.textBoxDisplayText.Size = new System.Drawing.Size(318, 20);
            this.textBoxDisplayText.TabIndex = 3;
            this.textBoxDisplayText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxAlternativeText
            // 
            this.textBoxAlternativeText.Location = new System.Drawing.Point(23, 81);
            this.textBoxAlternativeText.MaxLength = 100;
            this.textBoxAlternativeText.Name = "textBoxAlternativeText";
            this.textBoxAlternativeText.Size = new System.Drawing.Size(318, 20);
            this.textBoxAlternativeText.TabIndex = 5;
            this.textBoxAlternativeText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelAlternativeText
            // 
            this.labelAlternativeText.AutoSize = true;
            this.labelAlternativeText.Location = new System.Drawing.Point(364, 88);
            this.labelAlternativeText.Name = "labelAlternativeText";
            this.labelAlternativeText.Size = new System.Drawing.Size(69, 13);
            this.labelAlternativeText.TabIndex = 4;
            this.labelAlternativeText.Text = "טקסט חלופי";
            // 
            // textBoxURL
            // 
            this.textBoxURL.Location = new System.Drawing.Point(23, 116);
            this.textBoxURL.MaxLength = 100;
            this.textBoxURL.Multiline = true;
            this.textBoxURL.Name = "textBoxURL";
            this.textBoxURL.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBoxURL.Size = new System.Drawing.Size(318, 56);
            this.textBoxURL.TabIndex = 7;
            // 
            // labelURL
            // 
            this.labelURL.AutoSize = true;
            this.labelURL.Location = new System.Drawing.Point(364, 123);
            this.labelURL.Name = "labelURL";
            this.labelURL.Size = new System.Drawing.Size(64, 13);
            this.labelURL.TabIndex = 6;
            this.labelURL.Text = "קישור URL";
            // 
            // comboBoxCategory
            // 
            this.comboBoxCategory.FormattingEnabled = true;
            this.comboBoxCategory.Location = new System.Drawing.Point(165, 11);
            this.comboBoxCategory.Name = "comboBoxCategory";
            this.comboBoxCategory.Size = new System.Drawing.Size(176, 21);
            this.comboBoxCategory.TabIndex = 8;
            this.comboBoxCategory.Visible = false;
            // 
            // labelCategory
            // 
            this.labelCategory.AutoSize = true;
            this.labelCategory.Location = new System.Drawing.Point(364, 14);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(50, 13);
            this.labelCategory.TabIndex = 9;
            this.labelCategory.Text = "קטגוריה";
            this.labelCategory.Visible = false;
            // 
            // labelOrder
            // 
            this.labelOrder.AutoSize = true;
            this.labelOrder.Location = new System.Drawing.Point(364, 190);
            this.labelOrder.Name = "labelOrder";
            this.labelOrder.Size = new System.Drawing.Size(38, 13);
            this.labelOrder.TabIndex = 10;
            this.labelOrder.Text = "מיקום";
            // 
            // textBoxOrder
            // 
            this.textBoxOrder.Location = new System.Drawing.Point(285, 187);
            this.textBoxOrder.MaxLength = 2;
            this.textBoxOrder.Name = "textBoxOrder";
            this.textBoxOrder.Size = new System.Drawing.Size(56, 20);
            this.textBoxOrder.TabIndex = 11;
            this.textBoxOrder.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // UserControlEditBottomPageLink
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxOrder);
            this.Controls.Add(this.labelOrder);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.comboBoxCategory);
            this.Controls.Add(this.textBoxURL);
            this.Controls.Add(this.labelURL);
            this.Controls.Add(this.textBoxAlternativeText);
            this.Controls.Add(this.labelAlternativeText);
            this.Controls.Add(this.textBoxDisplayText);
            this.Controls.Add(this.labelVisibleText);
            this.Controls.Add(this.checkBoxLinkStatus);
            this.Controls.Add(this.buttonSave);
            this.Name = "UserControlEditBottomPageLink";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(452, 267);
            this.Load += new System.EventHandler(this.UserControlEditBottomPageLink_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.CheckBox checkBoxLinkStatus;
        private System.Windows.Forms.Label labelVisibleText;
        private System.Windows.Forms.TextBox textBoxDisplayText;
        private System.Windows.Forms.TextBox textBoxAlternativeText;
        private System.Windows.Forms.Label labelAlternativeText;
        private System.Windows.Forms.TextBox textBoxURL;
        private System.Windows.Forms.Label labelURL;
        private System.Windows.Forms.ComboBox comboBoxCategory;
        private System.Windows.Forms.Label labelCategory;
        private System.Windows.Forms.Label labelOrder;
        private System.Windows.Forms.TextBox textBoxOrder;
    }
}
