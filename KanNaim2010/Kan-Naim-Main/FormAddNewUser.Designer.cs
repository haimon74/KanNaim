namespace Kan_Naim_Main
{
    partial class FormAddNewUser
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
            this.components = new System.ComponentModel.Container();
            this.buttonSaveNewUser = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.labelUserName = new System.Windows.Forms.Label();
            this.labelEmail = new System.Windows.Forms.Label();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tableLookupRolesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._10infoDataSetRoles = new Kan_Naim_Main._10infoDataSetRoles();
            this.table_LookupRolesTableAdapter = new Kan_Naim_Main._10infoDataSetRolesTableAdapters.Table_LookupRolesTableAdapter();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.labelPass = new System.Windows.Forms.Label();
            this.textBoxEmail = new System.Windows.Forms.TextBox();
            this.textBoxMobilePhone = new System.Windows.Forms.TextBox();
            this.labelMobilePhone = new System.Windows.Forms.Label();
            this.textBoxOtherPhone = new System.Windows.Forms.TextBox();
            this.labelOtherPhone = new System.Windows.Forms.Label();
            this.textBoxFax = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxFirstName = new System.Windows.Forms.TextBox();
            this.labelFirstName = new System.Windows.Forms.Label();
            this.textBoxLastName = new System.Windows.Forms.TextBox();
            this.labelLastName = new System.Windows.Forms.Label();
            this.textBoxShortName = new System.Windows.Forms.TextBox();
            this.labelEditorShortName = new System.Windows.Forms.Label();
            this.textBoxLongName = new System.Windows.Forms.TextBox();
            this.labelLongName = new System.Windows.Forms.Label();
            this.textBoxAddress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonClearForm = new System.Windows.Forms.Button();
            this.buttonShowTable = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tableLookupRolesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._10infoDataSetRoles)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSaveNewUser
            // 
            this.buttonSaveNewUser.Location = new System.Drawing.Point(158, 318);
            this.buttonSaveNewUser.Name = "buttonSaveNewUser";
            this.buttonSaveNewUser.Size = new System.Drawing.Size(121, 38);
            this.buttonSaveNewUser.TabIndex = 0;
            this.buttonSaveNewUser.Text = "שמור";
            this.buttonSaveNewUser.UseVisualStyleBackColor = true;
            this.buttonSaveNewUser.Click += new System.EventHandler(this.buttonSaveNewUser_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(65, 63);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "סוג משתמש";
            this.label7.Visible = false;
            // 
            // labelUserName
            // 
            this.labelUserName.AutoSize = true;
            this.labelUserName.Location = new System.Drawing.Point(65, 99);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(65, 13);
            this.labelUserName.TabIndex = 8;
            this.labelUserName.Text = "שם משתמש";
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(65, 172);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(44, 13);
            this.labelEmail.TabIndex = 9;
            this.labelEmail.Text = "אימייל";
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Location = new System.Drawing.Point(158, 96);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBoxUserName.Size = new System.Drawing.Size(121, 20);
            this.textBoxUserName.TabIndex = 12;
            // 
            // comboBox1
            // 
            this.comboBox1.DataSource = this.tableLookupRolesBindingSource;
            this.comboBox1.DisplayMember = "RoleDescription";
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(158, 60);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 13;
            this.comboBox1.ValueMember = "RoleId";
            this.comboBox1.Visible = false;
            // 
            // tableLookupRolesBindingSource
            // 
            this.tableLookupRolesBindingSource.DataMember = "Table_LookupRoles";
            this.tableLookupRolesBindingSource.DataSource = this._10infoDataSetRoles;
            // 
            // _10infoDataSetRoles
            // 
            this._10infoDataSetRoles.DataSetName = "_10infoDataSetRoles";
            this._10infoDataSetRoles.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // table_LookupRolesTableAdapter
            // 
            this.table_LookupRolesTableAdapter.ClearBeforeFill = true;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(158, 131);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBoxPassword.Size = new System.Drawing.Size(121, 20);
            this.textBoxPassword.TabIndex = 15;
            // 
            // labelPass
            // 
            this.labelPass.AutoSize = true;
            this.labelPass.Location = new System.Drawing.Point(65, 134);
            this.labelPass.Name = "labelPass";
            this.labelPass.Size = new System.Drawing.Size(40, 13);
            this.labelPass.TabIndex = 14;
            this.labelPass.Text = "סיסמה";
            // 
            // textBoxEmail
            // 
            this.textBoxEmail.Location = new System.Drawing.Point(158, 169);
            this.textBoxEmail.Name = "textBoxEmail";
            this.textBoxEmail.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBoxEmail.Size = new System.Drawing.Size(121, 20);
            this.textBoxEmail.TabIndex = 16;
            // 
            // textBoxMobilePhone
            // 
            this.textBoxMobilePhone.Location = new System.Drawing.Point(158, 207);
            this.textBoxMobilePhone.Name = "textBoxMobilePhone";
            this.textBoxMobilePhone.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBoxMobilePhone.Size = new System.Drawing.Size(121, 20);
            this.textBoxMobilePhone.TabIndex = 18;
            // 
            // labelMobilePhone
            // 
            this.labelMobilePhone.AutoSize = true;
            this.labelMobilePhone.Location = new System.Drawing.Point(65, 210);
            this.labelMobilePhone.Name = "labelMobilePhone";
            this.labelMobilePhone.Size = new System.Drawing.Size(44, 13);
            this.labelMobilePhone.TabIndex = 17;
            this.labelMobilePhone.Text = "סלולרי";
            // 
            // textBoxOtherPhone
            // 
            this.textBoxOtherPhone.Location = new System.Drawing.Point(158, 252);
            this.textBoxOtherPhone.Name = "textBoxOtherPhone";
            this.textBoxOtherPhone.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBoxOtherPhone.Size = new System.Drawing.Size(121, 20);
            this.textBoxOtherPhone.TabIndex = 20;
            // 
            // labelOtherPhone
            // 
            this.labelOtherPhone.AutoSize = true;
            this.labelOtherPhone.Location = new System.Drawing.Point(65, 255);
            this.labelOtherPhone.Name = "labelOtherPhone";
            this.labelOtherPhone.Size = new System.Drawing.Size(47, 13);
            this.labelOtherPhone.TabIndex = 19;
            this.labelOtherPhone.Text = "טלפון 2";
            // 
            // textBoxFax
            // 
            this.textBoxFax.Location = new System.Drawing.Point(483, 252);
            this.textBoxFax.Name = "textBoxFax";
            this.textBoxFax.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBoxFax.Size = new System.Drawing.Size(121, 20);
            this.textBoxFax.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(390, 255);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "פקס";
            // 
            // textBoxFirstName
            // 
            this.textBoxFirstName.Location = new System.Drawing.Point(483, 60);
            this.textBoxFirstName.Name = "textBoxFirstName";
            this.textBoxFirstName.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBoxFirstName.Size = new System.Drawing.Size(121, 20);
            this.textBoxFirstName.TabIndex = 24;
            this.textBoxFirstName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelFirstName
            // 
            this.labelFirstName.AutoSize = true;
            this.labelFirstName.Location = new System.Drawing.Point(390, 63);
            this.labelFirstName.Name = "labelFirstName";
            this.labelFirstName.Size = new System.Drawing.Size(51, 13);
            this.labelFirstName.TabIndex = 23;
            this.labelFirstName.Text = "שם פרטי";
            // 
            // textBoxLastName
            // 
            this.textBoxLastName.Location = new System.Drawing.Point(483, 96);
            this.textBoxLastName.Name = "textBoxLastName";
            this.textBoxLastName.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBoxLastName.Size = new System.Drawing.Size(121, 20);
            this.textBoxLastName.TabIndex = 26;
            this.textBoxLastName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelLastName
            // 
            this.labelLastName.AutoSize = true;
            this.labelLastName.Location = new System.Drawing.Point(390, 99);
            this.labelLastName.Name = "labelLastName";
            this.labelLastName.Size = new System.Drawing.Size(63, 13);
            this.labelLastName.TabIndex = 25;
            this.labelLastName.Text = "שם משפחה";
            // 
            // textBoxShortName
            // 
            this.textBoxShortName.Location = new System.Drawing.Point(483, 135);
            this.textBoxShortName.Name = "textBoxShortName";
            this.textBoxShortName.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBoxShortName.Size = new System.Drawing.Size(121, 20);
            this.textBoxShortName.TabIndex = 28;
            this.textBoxShortName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelEditorShortName
            // 
            this.labelEditorShortName.AutoSize = true;
            this.labelEditorShortName.Location = new System.Drawing.Point(390, 138);
            this.labelEditorShortName.Name = "labelEditorShortName";
            this.labelEditorShortName.Size = new System.Drawing.Size(74, 13);
            this.labelEditorShortName.TabIndex = 27;
            this.labelEditorShortName.Text = "שם עורך קצר";
            // 
            // textBoxLongName
            // 
            this.textBoxLongName.Location = new System.Drawing.Point(483, 169);
            this.textBoxLongName.Name = "textBoxLongName";
            this.textBoxLongName.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBoxLongName.Size = new System.Drawing.Size(121, 20);
            this.textBoxLongName.TabIndex = 30;
            this.textBoxLongName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelLongName
            // 
            this.labelLongName.AutoSize = true;
            this.labelLongName.Location = new System.Drawing.Point(390, 172);
            this.labelLongName.Name = "labelLongName";
            this.labelLongName.Size = new System.Drawing.Size(80, 13);
            this.labelLongName.TabIndex = 29;
            this.labelLongName.Text = "שם עורך ארוך";
            // 
            // textBoxAddress
            // 
            this.textBoxAddress.Location = new System.Drawing.Point(483, 211);
            this.textBoxAddress.Name = "textBoxAddress";
            this.textBoxAddress.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBoxAddress.Size = new System.Drawing.Size(121, 20);
            this.textBoxAddress.TabIndex = 32;
            this.textBoxAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(390, 214);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "כתובת";
            // 
            // buttonClearForm
            // 
            this.buttonClearForm.Location = new System.Drawing.Point(483, 318);
            this.buttonClearForm.Name = "buttonClearForm";
            this.buttonClearForm.Size = new System.Drawing.Size(121, 38);
            this.buttonClearForm.TabIndex = 33;
            this.buttonClearForm.Text = "נקה טופס";
            this.buttonClearForm.UseVisualStyleBackColor = true;
            this.buttonClearForm.Click += new System.EventHandler(this.buttonClearForm_Click);
            // 
            // buttonShowTable
            // 
            this.buttonShowTable.Location = new System.Drawing.Point(320, 318);
            this.buttonShowTable.Name = "buttonShowTable";
            this.buttonShowTable.Size = new System.Drawing.Size(121, 38);
            this.buttonShowTable.TabIndex = 34;
            this.buttonShowTable.Text = "רשימה...";
            this.buttonShowTable.UseVisualStyleBackColor = true;
            this.buttonShowTable.Visible = false;
            this.buttonShowTable.Click += new System.EventHandler(this.buttonShowTable_Click);
            // 
            // FormAddNewUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 402);
            this.Controls.Add(this.buttonShowTable);
            this.Controls.Add(this.buttonClearForm);
            this.Controls.Add(this.textBoxAddress);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxLongName);
            this.Controls.Add(this.labelLongName);
            this.Controls.Add(this.textBoxShortName);
            this.Controls.Add(this.labelEditorShortName);
            this.Controls.Add(this.textBoxLastName);
            this.Controls.Add(this.labelLastName);
            this.Controls.Add(this.textBoxFirstName);
            this.Controls.Add(this.labelFirstName);
            this.Controls.Add(this.textBoxFax);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxOtherPhone);
            this.Controls.Add(this.labelOtherPhone);
            this.Controls.Add(this.textBoxMobilePhone);
            this.Controls.Add(this.labelMobilePhone);
            this.Controls.Add(this.textBoxEmail);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.labelPass);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.textBoxUserName);
            this.Controls.Add(this.labelEmail);
            this.Controls.Add(this.labelUserName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.buttonSaveNewUser);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAddNewUser";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "הוספת משתמש חדש";
            this.Load += new System.EventHandler(this.FormAddNewUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tableLookupRolesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._10infoDataSetRoles)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSaveNewUser;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelUserName;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.TextBox textBoxUserName;
        private System.Windows.Forms.ComboBox comboBox1;
        private _10infoDataSetRoles _10infoDataSetRoles;
        private System.Windows.Forms.BindingSource tableLookupRolesBindingSource;
        private Kan_Naim_Main._10infoDataSetRolesTableAdapters.Table_LookupRolesTableAdapter table_LookupRolesTableAdapter;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label labelPass;
        private System.Windows.Forms.TextBox textBoxEmail;
        private System.Windows.Forms.TextBox textBoxMobilePhone;
        private System.Windows.Forms.Label labelMobilePhone;
        private System.Windows.Forms.TextBox textBoxOtherPhone;
        private System.Windows.Forms.Label labelOtherPhone;
        private System.Windows.Forms.TextBox textBoxFax;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxFirstName;
        private System.Windows.Forms.Label labelFirstName;
        private System.Windows.Forms.TextBox textBoxLastName;
        private System.Windows.Forms.Label labelLastName;
        private System.Windows.Forms.TextBox textBoxShortName;
        private System.Windows.Forms.Label labelEditorShortName;
        private System.Windows.Forms.TextBox textBoxLongName;
        private System.Windows.Forms.Label labelLongName;
        private System.Windows.Forms.TextBox textBoxAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonClearForm;
        private System.Windows.Forms.Button buttonShowTable;
    }
}