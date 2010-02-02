using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Shimanni.Common.Desktop
{
    public partial class CaptionedGrid : UserControl
    {
        public CaptionedGrid()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        public object DataSource
        {
            get { return dataGridView1.DataSource; }
            set { dataGridView1.DataSource = value; }
        }
    }
}
