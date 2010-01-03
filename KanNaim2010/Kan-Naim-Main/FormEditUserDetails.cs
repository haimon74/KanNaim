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
    public partial class FormEditUserDetails : Form
    {
        public FormEditUserDetails()
        {
            InitializeComponent();
        }

        private void FormEditUserDetails_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the '_10infoDataSetUsers.DataTableUsersDetails' table. You can move, or remove it, as needed.
            this.dataTableUsersDetailsTableAdapter.Fill(this._10infoDataSetUsers.DataTableUsersDetails);

        }
    }
}
