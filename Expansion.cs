using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountingEveryDay
{
    public static class Expansion
    {
        public static Action callback;
        public static Form _form;
        public static System.Threading.Timer timer;
        public static void Debounce(this Form form, int delay, Action func)
        {
            callback = func;
            _form = form;
            if (timer == null)
            {
                timer = new System.Threading.Timer(action, null, delay, -1);
            }
            else
            {
                timer.Change(delay, -1);
            }
        }
        static void action(object state)
        {
            _form.Invoke(callback);
        }

    }
}
