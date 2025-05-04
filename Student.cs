using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingEveryDay
{
    internal class Student : IDisposable
    {
        public void Dispose()
        {
            Console.WriteLine("Student物件記憶體被清除了!");

        }
    }
}
