using AccountingEveryDay.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountingEveryDay.Components
{
    public partial class NavBar : UserControl
    {
        public NavBar()
        {
            InitializeComponent();
            Assembly assembly = Assembly.GetExecutingAssembly();
            //  x.IsSubclassOf(typeof(Form))
            Type[] filteredTypes = assembly.GetTypes().Where(x =>x.BaseType == typeof(Form)  && x.FullName.Contains("Forms") ).ToArray();

            for (int i = 0; i < filteredTypes.Length; i++)
            {
                Button btn = new Button();
                string str = filteredTypes[i].Name;
                btn.Name = "Btn" + i;
                btn.Size = new System.Drawing.Size((flowLayoutPanel1 .Width/ filteredTypes.Length)-10, 50);
                btn.Text = str;
                Console.WriteLine(btn.Text);
                btn.UseVisualStyleBackColor = true;
                btn.Click += button1_Click_1;
                flowLayoutPanel1.Controls.Add(btn);
            }
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string btnStr = btn.Text;
            //Enum.TryParse<FormType>(btn.Text, out FormType formType);
            SingletonForm.GetInstance(btnStr).Show();
        }

        public void DisableBtn(string formText)
        {
            Button button = flowLayoutPanel1.Controls.OfType<Button>().First(x => x.Text == formText);
            button.Enabled = false;
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
