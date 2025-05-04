using AccountingEveryDay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace AccountingEveryDay.Attributies
{
    internal class ComboBoxAttribute : Attribute
    {
        public string[] DataSource { get; set; }
        public ComboBoxAttribute(string type = "")
        {
            if(type=="Type")
            {
                DataSource = DataModel.Type;
            }
        }
    }
}
