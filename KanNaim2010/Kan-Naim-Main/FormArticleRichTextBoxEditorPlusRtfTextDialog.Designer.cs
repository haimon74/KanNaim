    namespace Kan_Naim_Main
{
        partial class FormArticleRichTextBoxEditorPlusRtfTextDialog
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.userControlRichTextBoxEditor1 = new HaimDLL.UserControlRichTextBoxEditor();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(548, 808);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(95, 117);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(48, 90);
            this.button1.TabIndex = 2;
            this.button1.Text = "move to text box";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1138, 116);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(42, 91);
            this.button2.TabIndex = 3;
            this.button2.Text = "move to rich text box";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2Click);
            // 
            // userControlRichTextBoxEditor1
            // 
            this.userControlRichTextBoxEditor1.AutoScroll = true;
            this.userControlRichTextBoxEditor1.Location = new System.Drawing.Point(564, 0);
            this.userControlRichTextBoxEditor1.Name = "userControlRichTextBoxEditor1";
            this.userControlRichTextBoxEditor1.Size = new System.Drawing.Size(631, 800);
            this.userControlRichTextBoxEditor1.TabIndex = 0;
            this.userControlRichTextBoxEditor1.Load += new System.EventHandler(this.userControlRichTextBoxEditor1_Load);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(95, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(48, 90);
            this.button3.TabIndex = 4;
            this.button3.Text = "view in browser";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // FormArticleRichTextBoxEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1192, 806);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.userControlRichTextBoxEditor1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(910, 840);
            this.Name = "FormArticleRichTextBoxEditor";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "עיצוב מאמר";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HaimDLL.UserControlRichTextBoxEditor userControlRichTextBoxEditor1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}