namespace HaimDLL
{
    partial class UserControlEditPreferedLinks
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
            this.buttonClearForm = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.textBoxAlternativeText = new System.Windows.Forms.TextBox();
            this.labelAlternativeText = new System.Windows.Forms.Label();
            this.textBoxOrderPlace = new System.Windows.Forms.TextBox();
            this.labelOrderPlace = new System.Windows.Forms.Label();
            this.buttonSearchPhoto = new System.Windows.Forms.Button();
            this.textBoxPhotoId = new System.Windows.Forms.TextBox();
            this.labelPhotoId = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonUrl = new System.Windows.Forms.RadioButton();
            this.radioButtonArticleId = new System.Windows.Forms.RadioButton();
            this.textBoxArticleId = new System.Windows.Forms.TextBox();
            this.textBoxUrl = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonClearForm
            // 
            this.buttonClearForm.Location = new System.Drawing.Point(142, 262);
            this.buttonClearForm.Name = "buttonClearForm";
            this.buttonClearForm.Size = new System.Drawing.Size(94, 23);
            this.buttonClearForm.TabIndex = 24;
            this.buttonClearForm.Text = "נקה טופס";
            this.buttonClearForm.UseVisualStyleBackColor = true;
            this.buttonClearForm.Click += new System.EventHandler(this.buttonClearForm_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(344, 262);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(94, 23);
            this.buttonSave.TabIndex = 23;
            this.buttonSave.Text = "שמירה";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // textBoxAlternativeText
            // 
            this.textBoxAlternativeText.Location = new System.Drawing.Point(34, 204);
            this.textBoxAlternativeText.Name = "textBoxAlternativeText";
            this.textBoxAlternativeText.Size = new System.Drawing.Size(430, 20);
            this.textBoxAlternativeText.TabIndex = 22;
            this.textBoxAlternativeText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelAlternativeText
            // 
            this.labelAlternativeText.AutoSize = true;
            this.labelAlternativeText.Location = new System.Drawing.Point(481, 207);
            this.labelAlternativeText.Name = "labelAlternativeText";
            this.labelAlternativeText.Size = new System.Drawing.Size(69, 13);
            this.labelAlternativeText.TabIndex = 21;
            this.labelAlternativeText.Text = "טקסט חלופי";
            // 
            // textBoxOrderPlace
            // 
            this.textBoxOrderPlace.Location = new System.Drawing.Point(364, 170);
            this.textBoxOrderPlace.Name = "textBoxOrderPlace";
            this.textBoxOrderPlace.Size = new System.Drawing.Size(100, 20);
            this.textBoxOrderPlace.TabIndex = 20;
            // 
            // labelOrderPlace
            // 
            this.labelOrderPlace.AutoSize = true;
            this.labelOrderPlace.Location = new System.Drawing.Point(482, 173);
            this.labelOrderPlace.Name = "labelOrderPlace";
            this.labelOrderPlace.Size = new System.Drawing.Size(68, 13);
            this.labelOrderPlace.TabIndex = 19;
            this.labelOrderPlace.Text = "מספר מיקום";
            // 
            // buttonSearchPhoto
            // 
            this.buttonSearchPhoto.Location = new System.Drawing.Point(251, 15);
            this.buttonSearchPhoto.Name = "buttonSearchPhoto";
            this.buttonSearchPhoto.Size = new System.Drawing.Size(94, 23);
            this.buttonSearchPhoto.TabIndex = 18;
            this.buttonSearchPhoto.Text = "חפש תמונה...";
            this.buttonSearchPhoto.UseVisualStyleBackColor = true;
            this.buttonSearchPhoto.Click += new System.EventHandler(this.buttonSearchPhoto_Click);
            // 
            // textBoxPhotoId
            // 
            this.textBoxPhotoId.Location = new System.Drawing.Point(364, 15);
            this.textBoxPhotoId.Name = "textBoxPhotoId";
            this.textBoxPhotoId.Size = new System.Drawing.Size(100, 20);
            this.textBoxPhotoId.TabIndex = 17;
            // 
            // labelPhotoId
            // 
            this.labelPhotoId.AutoSize = true;
            this.labelPhotoId.Location = new System.Drawing.Point(482, 18);
            this.labelPhotoId.Name = "labelPhotoId";
            this.labelPhotoId.Size = new System.Drawing.Size(68, 13);
            this.labelPhotoId.TabIndex = 16;
            this.labelPhotoId.Text = "מספר תמונה";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonUrl);
            this.groupBox1.Controls.Add(this.radioButtonArticleId);
            this.groupBox1.Controls.Add(this.textBoxArticleId);
            this.groupBox1.Controls.Add(this.textBoxUrl);
            this.groupBox1.Location = new System.Drawing.Point(21, 43);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(553, 114);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "קישור מטרה";
            // 
            // radioButtonUrl
            // 
            this.radioButtonUrl.AutoSize = true;
            this.radioButtonUrl.Location = new System.Drawing.Point(451, 44);
            this.radioButtonUrl.Name = "radioButtonUrl";
            this.radioButtonUrl.Size = new System.Drawing.Size(92, 17);
            this.radioButtonUrl.TabIndex = 9;
            this.radioButtonUrl.Text = "קישור ל URL";
            this.radioButtonUrl.UseVisualStyleBackColor = true;
            // 
            // radioButtonArticleId
            // 
            this.radioButtonArticleId.AutoSize = true;
            this.radioButtonArticleId.Checked = true;
            this.radioButtonArticleId.Location = new System.Drawing.Point(460, 19);
            this.radioButtonArticleId.Name = "radioButtonArticleId";
            this.radioButtonArticleId.Size = new System.Drawing.Size(83, 17);
            this.radioButtonArticleId.TabIndex = 8;
            this.radioButtonArticleId.TabStop = true;
            this.radioButtonArticleId.Text = "מספר מאמר";
            this.radioButtonArticleId.UseVisualStyleBackColor = true;
            // 
            // textBoxArticleId
            // 
            this.textBoxArticleId.Location = new System.Drawing.Point(343, 16);
            this.textBoxArticleId.Name = "textBoxArticleId";
            this.textBoxArticleId.Size = new System.Drawing.Size(100, 20);
            this.textBoxArticleId.TabIndex = 4;
            // 
            // textBoxUrl
            // 
            this.textBoxUrl.Enabled = false;
            this.textBoxUrl.Location = new System.Drawing.Point(13, 44);
            this.textBoxUrl.Multiline = true;
            this.textBoxUrl.Name = "textBoxUrl";
            this.textBoxUrl.Size = new System.Drawing.Size(430, 61);
            this.textBoxUrl.TabIndex = 7;
            // 
            // UserControlEditPreferedLinks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonClearForm);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxAlternativeText);
            this.Controls.Add(this.labelAlternativeText);
            this.Controls.Add(this.textBoxOrderPlace);
            this.Controls.Add(this.labelOrderPlace);
            this.Controls.Add(this.buttonSearchPhoto);
            this.Controls.Add(this.textBoxPhotoId);
            this.Controls.Add(this.labelPhotoId);
            this.Controls.Add(this.groupBox1);
            this.Name = "UserControlEditPreferedLinks";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Size = new System.Drawing.Size(600, 307);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonClearForm;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TextBox textBoxAlternativeText;
        private System.Windows.Forms.Label labelAlternativeText;
        private System.Windows.Forms.TextBox textBoxOrderPlace;
        private System.Windows.Forms.Label labelOrderPlace;
        private System.Windows.Forms.Button buttonSearchPhoto;
        private System.Windows.Forms.TextBox textBoxPhotoId;
        private System.Windows.Forms.Label labelPhotoId;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonUrl;
        private System.Windows.Forms.RadioButton radioButtonArticleId;
        private System.Windows.Forms.TextBox textBoxArticleId;
        private System.Windows.Forms.TextBox textBoxUrl;

    }
}
