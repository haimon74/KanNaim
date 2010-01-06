namespace HaimDLL
{
    partial class UserControlUploadVideo
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
            this._buttonSaveVideoToArchive = new System.Windows.Forms.Button();
            this._labelVideoAlternateText = new System.Windows.Forms.Label();
            this._textBoxVideoAlternateText = new System.Windows.Forms.TextBox();
            this._labelVideoDescription = new System.Windows.Forms.Label();
            this._textBoxVideoDescription = new System.Windows.Forms.TextBox();
            this._labelEmbedVideoText = new System.Windows.Forms.Label();
            this._textBoxVideoEmbedUrl = new System.Windows.Forms.TextBox();
            this._labelVideoCaption = new System.Windows.Forms.Label();
            this._textBoxVideoCaption = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonLoadFromArchive = new System.Windows.Forms.Button();
            this.buttonClearForm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _buttonSaveVideoToArchive
            // 
            this._buttonSaveVideoToArchive.Location = new System.Drawing.Point(356, 330);
            this._buttonSaveVideoToArchive.Name = "_buttonSaveVideoToArchive";
            this._buttonSaveVideoToArchive.Size = new System.Drawing.Size(117, 44);
            this._buttonSaveVideoToArchive.TabIndex = 17;
            this._buttonSaveVideoToArchive.Text = "שמור ווידאו בארכיון";
            this._buttonSaveVideoToArchive.UseVisualStyleBackColor = true;
            this._buttonSaveVideoToArchive.Click += new System.EventHandler(this._buttonSaveVideoToArchive_Click);
            // 
            // _labelVideoAlternateText
            // 
            this._labelVideoAlternateText.AutoSize = true;
            this._labelVideoAlternateText.Location = new System.Drawing.Point(517, 271);
            this._labelVideoAlternateText.Name = "_labelVideoAlternateText";
            this._labelVideoAlternateText.Size = new System.Drawing.Size(69, 13);
            this._labelVideoAlternateText.TabIndex = 16;
            this._labelVideoAlternateText.Text = "טקסט חלופי";
            // 
            // _textBoxVideoAlternateText
            // 
            this._textBoxVideoAlternateText.Location = new System.Drawing.Point(3, 268);
            this._textBoxVideoAlternateText.MaxLength = 300;
            this._textBoxVideoAlternateText.Multiline = true;
            this._textBoxVideoAlternateText.Name = "_textBoxVideoAlternateText";
            this._textBoxVideoAlternateText.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._textBoxVideoAlternateText.Size = new System.Drawing.Size(470, 45);
            this._textBoxVideoAlternateText.TabIndex = 15;
            // 
            // _labelVideoDescription
            // 
            this._labelVideoDescription.AutoSize = true;
            this._labelVideoDescription.Location = new System.Drawing.Point(517, 188);
            this._labelVideoDescription.Name = "_labelVideoDescription";
            this._labelVideoDescription.Size = new System.Drawing.Size(78, 13);
            this._labelVideoDescription.TabIndex = 14;
            this._labelVideoDescription.Text = "תאור הווידאו";
            // 
            // _textBoxVideoDescription
            // 
            this._textBoxVideoDescription.Location = new System.Drawing.Point(3, 185);
            this._textBoxVideoDescription.MaxLength = 500;
            this._textBoxVideoDescription.Multiline = true;
            this._textBoxVideoDescription.Name = "_textBoxVideoDescription";
            this._textBoxVideoDescription.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._textBoxVideoDescription.Size = new System.Drawing.Size(470, 68);
            this._textBoxVideoDescription.TabIndex = 13;
            // 
            // _labelEmbedVideoText
            // 
            this._labelEmbedVideoText.AutoSize = true;
            this._labelEmbedVideoText.Location = new System.Drawing.Point(517, 72);
            this._labelEmbedVideoText.Name = "_labelEmbedVideoText";
            this._labelEmbedVideoText.Size = new System.Drawing.Size(73, 13);
            this._labelEmbedVideoText.TabIndex = 12;
            this._labelEmbedVideoText.Text = "תוכן הקישור";
            // 
            // _textBoxVideoEmbedUrl
            // 
            this._textBoxVideoEmbedUrl.Location = new System.Drawing.Point(3, 72);
            this._textBoxVideoEmbedUrl.MaxLength = 1000;
            this._textBoxVideoEmbedUrl.Multiline = true;
            this._textBoxVideoEmbedUrl.Name = "_textBoxVideoEmbedUrl";
            this._textBoxVideoEmbedUrl.Size = new System.Drawing.Size(470, 95);
            this._textBoxVideoEmbedUrl.TabIndex = 11;
            // 
            // _labelVideoCaption
            // 
            this._labelVideoCaption.AutoSize = true;
            this._labelVideoCaption.Location = new System.Drawing.Point(517, 43);
            this._labelVideoCaption.Name = "_labelVideoCaption";
            this._labelVideoCaption.Size = new System.Drawing.Size(84, 13);
            this._labelVideoCaption.TabIndex = 10;
            this._labelVideoCaption.Text = "כותרת הווידאו";
            // 
            // _textBoxVideoCaption
            // 
            this._textBoxVideoCaption.Location = new System.Drawing.Point(3, 36);
            this._textBoxVideoCaption.MaxLength = 150;
            this._textBoxVideoCaption.Name = "_textBoxVideoCaption";
            this._textBoxVideoCaption.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this._textBoxVideoCaption.Size = new System.Drawing.Size(470, 20);
            this._textBoxVideoCaption.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(477, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "מלא פירטי הוידאו";
            // 
            // buttonLoadFromArchive
            // 
            this.buttonLoadFromArchive.Location = new System.Drawing.Point(189, 330);
            this.buttonLoadFromArchive.Name = "buttonLoadFromArchive";
            this.buttonLoadFromArchive.Size = new System.Drawing.Size(117, 44);
            this.buttonLoadFromArchive.TabIndex = 19;
            this.buttonLoadFromArchive.Text = "ייבא מארכיון";
            this.buttonLoadFromArchive.UseVisualStyleBackColor = true;
            this.buttonLoadFromArchive.Click += new System.EventHandler(this.buttonLoadFromArchive_Click);
            // 
            // buttonClearForm
            // 
            this.buttonClearForm.Location = new System.Drawing.Point(22, 330);
            this.buttonClearForm.Name = "buttonClearForm";
            this.buttonClearForm.Size = new System.Drawing.Size(117, 44);
            this.buttonClearForm.TabIndex = 20;
            this.buttonClearForm.Text = "נקה טופס";
            this.buttonClearForm.UseVisualStyleBackColor = true;
            this.buttonClearForm.Click += new System.EventHandler(this.buttonClearForm_Click);
            // 
            // UserControlUploadVideo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonClearForm);
            this.Controls.Add(this.buttonLoadFromArchive);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._buttonSaveVideoToArchive);
            this.Controls.Add(this._labelVideoAlternateText);
            this.Controls.Add(this._textBoxVideoAlternateText);
            this.Controls.Add(this._labelVideoDescription);
            this.Controls.Add(this._textBoxVideoDescription);
            this.Controls.Add(this._labelEmbedVideoText);
            this.Controls.Add(this._textBoxVideoEmbedUrl);
            this.Controls.Add(this._labelVideoCaption);
            this.Controls.Add(this._textBoxVideoCaption);
            this.Name = "UserControlUploadVideo";
            this.Size = new System.Drawing.Size(601, 398);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _buttonSaveVideoToArchive;
        private System.Windows.Forms.Label _labelVideoAlternateText;
        public System.Windows.Forms.TextBox _textBoxVideoAlternateText;
        private System.Windows.Forms.Label _labelVideoDescription;
        public System.Windows.Forms.TextBox _textBoxVideoDescription;
        private System.Windows.Forms.Label _labelEmbedVideoText;
        public System.Windows.Forms.TextBox _textBoxVideoEmbedUrl;
        private System.Windows.Forms.Label _labelVideoCaption;
        public System.Windows.Forms.TextBox _textBoxVideoCaption;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonLoadFromArchive;
        private System.Windows.Forms.Button buttonClearForm;
    }
}
