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
    public partial class FormRemindPasswordByUserNameOrPhone : Form
    {
        private static DataClassesKanNaimDataContext Db = new DataClassesKanNaimDataContext();
        public FormRemindPasswordByUserNameOrPhone()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var user = (from c in Db.Table_Users
                        where c.UserName == textBox1.Text || c.PhoneNumber == textBox1.Text
                        select c).Single();
            var reporter = DataAccess.Lookup.GetLookupReporterFromUserId(user.UserId);
            string email = reporter.Email;
            string password = user.Password;

            label2.Text = password;
            // TODO - send by email email ?
        }
    }
}
