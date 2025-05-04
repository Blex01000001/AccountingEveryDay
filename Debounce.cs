using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountingEveryDay
{
    internal class Debounce
    {
        public static Action callback;
        public static Form _form;
        public static System.Threading.Timer timer;

        public int delay;
        public static TimerCallback timerCallback;
        public Debounce(Action callback, int delay)
        {
            //this.callback = callback;
            //this.delay = delay;

        }
        public static void debounce(Form form, Action func, int delay)
        {
            callback = func;
            _form = form;
            if (timer == null)
            {
                //timer = new System.Threading.Timer(action, null, 1500, -1);
                timer = new System.Threading.Timer(action, null, delay, -1);
            }
            else
            {
                timer.Change(delay, -1);
            }
        }
        static void action(object state)
        {
            //Console.WriteLine($"CurrentThread.ManagedThreadId: {Thread.CurrentThread.ManagedThreadId}");
            //Console.WriteLine($"GetDomainID: {Program.GetCurrentThreadId()}");
            //Console.WriteLine($"CurrentThread.CurrentCulture: {Thread.CurrentThread.CurrentCulture}");
            //Console.WriteLine($"CurrentThread.CurrentUICulture: {Thread.CurrentThread.CurrentUICulture}");
            _form.Invoke(new Action(() => callback()));
            //callback.Invoke();
        }
    }
}
