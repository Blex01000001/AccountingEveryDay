using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountingEveryDay.Components
{
    public partial class ImgDialog : Form
    {
        public ImgDialog(string path)
        {
            InitializeComponent();
            pictureBox1.Image = Image.FromFile(path);

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void ImgDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            pictureBox1.Image.Dispose();
            // ? => 如果pictureBox1 = null 就不執行Dispose()
        }
    }
}
