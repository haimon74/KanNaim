namespace Kan_Naim_Main
{
    partial class Sandbox
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
            this.customDataGridView1 = new UserControlsLibrary.WinControls.CustomDataGridView();
            this.SuspendLayout();
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.Location = new System.Drawing.Point(107, 114);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.Size = new System.Drawing.Size(484, 354);
            this.customDataGridView1.TabIndex = 0;
            this.customDataGridView1.Text = "customDataGridView1";
            // 
            // Sandbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 583);
            this.Controls.Add(this.customDataGridView1);
            this.Name = "Sandbox";
            this.Text = "Sandbox";
            this.Load += new System.EventHandler(this.Sandbox_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private UserControlsLibrary.WinControls.CustomDataGridView customDataGridView1;
    }
}