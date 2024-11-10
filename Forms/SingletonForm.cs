using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using AccountingEveryDay.Components;
using System.IO;

namespace AccountingEveryDay.Forms
{
    internal class SingletonForm
    {
        private static Form ActiveForm = null;
        private static Dictionary<string, Form> dic = new Dictionary<string, Form>();
        private SingletonForm()
        {

        }

        static public Form GetInstance(string btnStr)
        {
            if (ActiveForm != null)
                ActiveForm.Hide();
            if (dic.ContainsKey(btnStr))
            {
                ActiveForm = dic[btnStr];
                return ActiveForm;
            }
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type type = assembly.GetType($"AccountingEveryDay.Forms.{btnStr.ToString()}");
            Form formValue = (Form)Activator.CreateInstance(type);
            ActiveForm = formValue;
            dic.Add(btnStr, formValue);

            FieldInfo[] fields = ActiveForm.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            //FieldInfo fieldNav = fields.First(x => x.FieldType.Name == "NavBar");
            FieldInfo fieldNav = fields.First(x => x.FieldType == typeof(NavBar));
            NavBar nav = (NavBar)fieldNav.GetValue(formValue);
            nav.DisableBtn(formValue.Text);

            return ActiveForm;
        }
    }
}
