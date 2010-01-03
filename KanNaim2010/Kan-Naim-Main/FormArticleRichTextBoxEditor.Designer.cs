    namespace Kan_Naim_Main
{
    partial class FormArticleRichTextBoxEditor
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
            this.userControlRichTextBoxEditor1 = new HaimDLL.UserControlRichTextBoxEditor();
            this.SuspendLayout();
            // 
            // userControlRichTextBoxEditor1
            // 
            this.userControlRichTextBoxEditor1.AutoScroll = true;
            this.userControlRichTextBoxEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlRichTextBoxEditor1.Location = new System.Drawing.Point(0, 0);
            this.userControlRichTextBoxEditor1.Name = "userControlRichTextBoxEditor1";
            this.userControlRichTextBoxEditor1.Size = new System.Drawing.Size(792, 806);
            this.userControlRichTextBoxEditor1.TabIndex = 0;
            this.userControlRichTextBoxEditor1.Load += new System.EventHandler(this.userControlRichTextBoxEditor1_Load);
            // 
            // FormArticleRichTextBoxEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 806);
            this.Controls.Add(this.userControlRichTextBoxEditor1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 840);
            this.Name = "FormArticleRichTextBoxEditor";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "עיצוב מאמר";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormArticleRichTextBoxEditor_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private HaimDLL.UserControlRichTextBoxEditor userControlRichTextBoxEditor1;
    }
}