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
    public partial class Form_Administrator : Form
    {
        public Form_Administrator()
        {
            InitializeComponent();
        }

        private void Form_Administrator_Load(object sender, EventArgs e)
        {
            ToolStripMenuItemAddArticle_Click(sender, e);
        }

        private void כתבותToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxPhoto2x_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox8_Enter(object sender, EventArgs e)
        {

        }

        private void tabPageArticle_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void ToolStripMenuItemAddArticle_Click(object sender, EventArgs e)
        {
            string category = this.treeView1.SelectedNode.Text;
            FormEditArtical newArt = FormEditArtical.GetFormEditNewArtical(category, "משה נעים");
            newArt.Show();
            newArt.Focus();            
        }

        private void ToolStripMenuItemActiveCategories_Click(object sender, EventArgs e)
        {
            Form_CategoriesManager categoryManager = new Form_CategoriesManager();
            categoryManager.Show();
            categoryManager.Focus();
        }
    }
}
