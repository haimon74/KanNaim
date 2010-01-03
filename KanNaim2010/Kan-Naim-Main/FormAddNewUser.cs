using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kan_Naim_Main
{
    public partial class FormAddNewUser : Form
    {
        private static readonly DataClassesKanNaimDataContext Db = new DataClassesKanNaimDataContext();

        public FormAddNewUser()
        {
            InitializeComponent();
        }

        private void FormAddNewUser_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the '_10infoDataSetRoles.Table_LookupRoles' table. You can move, or remove it, as needed.
            this.table_LookupRolesTableAdapter.Fill(this._10infoDataSetRoles.Table_LookupRoles);

        }

        private void buttonClearForm_Click(object sender, EventArgs e)
        {
            textBoxShortName.Text = "";
            textBoxOtherPhone.Text = "";
            textBoxMobilePhone.Text = "";
            textBoxLongName.Text = "";
            textBoxLastName.Text = "";
            textBoxFirstName.Text = "";
            textBoxFax.Text = "";
            textBoxEmail.Text = "";
            textBoxAddress.Text = "";
            textBoxPassword.Text = "";
            textBoxUserName.Text = "";
        }

        private void buttonSaveNewUser_Click(object sender, EventArgs e)
        {
            var userRow = new Table_User
                              {
                                  FirstName = textBoxFirstName.Text,
                                  Address = textBoxAddress.Text,
                                  LastName = textBoxLastName.Text,
                                  Password = textBoxPassword.Text,
                                  PhoneNumber = textBoxMobilePhone.Text,
                                  RoleId = (int) comboBox1.SelectedValue,
                                  UserName = textBoxUserName.Text

                              };
            Db.Table_Users.InsertOnSubmit(userRow);
            Db.SubmitChanges();

            userRow = DataAccess.Filter.GetUserByUserNameOrPhone(textBoxUserName.Text).Single();

            var reporter = new Table_LookupReporter()
                               {
                                   Address = textBoxAddress.Text,
                                   Email = textBoxEmail.Text,
                                   Fax = textBoxFax.Text,
                                   FirstName = textBoxFirstName.Text,
                                   LastName = textBoxLastName.Text,
                                   MobilePhone = textBoxMobilePhone.Text,
                                   OtherPhone = textBoxOtherPhone.Text,
                                   PublishNameLong = textBoxLongName.Text,
                                   PublishNameShort = textBoxShortName.Text,
                                   UserId = userRow.UserId
                               };
            Db.Table_LookupReporters.InsertOnSubmit(reporter);
            Db.SubmitChanges();

        }

        private void buttonShowTable_Click(object sender, EventArgs e)
        {
            var formEditUserDetails = new FormEditUserDetails();
            formEditUserDetails.Show();
        }

    }
}
