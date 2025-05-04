using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingEveryDay.Attributies
{
    internal class PictureAttribute:Attribute
    {
        string name;
        public PictureAttribute()
        {

        }


        public void SayHello()
        {
            Console.WriteLine();
        }
    }
}
