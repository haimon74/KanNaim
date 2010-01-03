using System;
using System.Linq;
using System.Windows.Forms;

namespace Kan_Naim_Main
{
    
    public partial class FormLogin : Form
    {
        private const string ConStr = HaimDLL.Constants.MyLocalMsSqlKanNaimConnectionString;
        static readonly DataClassesKanNaimDataContext Db = new DataClassesKanNaimDataContext(ConStr);
        static Table_User _user = null;
        static Table_LookupRole _role = null;
        static int _tries = 0;
        static string _currentUser = null;
        static int _currentRoleId = 0;

        public FormLogin()
        {
            InitializeComponent();
        }

        private void GetLoginData()
        {
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ActivateAdminForm()
        {
            var frm1 = new FormAdministrator();
            frm1.Show();
            frm1.Focus();
            Hide();            
        }
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            ((Button)sender).Text = "בבדיקה...";
            if (_tries == 3)
                return;
            
            try
            {
                _user = (from c in Db.Table_Users
                        where (c.UserName == textBoxUserName.Text && c.Password == textBoxPassword.Text)
                        select c).Single();
                _role = (from c in Db.Table_LookupRoles
                         where _user.RoleId == c.RoleId
                         select c).Single();
                _currentUser = _user.FirstName + _user.LastName;
                _currentRoleId = _role.RoleId;
                ActivateAdminForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("שם משתמש ו/או סיסמה שגויים");
                ((Button)sender).Enabled = true;
                ((Button)sender).Text = "כניסה";
                _tries++;
            }
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            ActivateAdminForm();

            var testForm = FormEditArtical.GetFormEditNewArtical("חדשות ישראל  | News", "משה נעים");
            //var testForm = new FormArticleRichTextBoxEditor();
            testForm.Show();

            //FormTest ft = new FormTest("חדשות ישראל  | News");
            //ft.Show();
            
            //NewControlsTesting test = new NewControlsTesting();
            //test.Show();
            //test.Focus();
        }
    }
}
