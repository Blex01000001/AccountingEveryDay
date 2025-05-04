using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;
using System.Xml.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AccountingEveryDay.Attributies;

namespace AccountingEveryDay.Models
{
    public class ItemModel
    {
        [DisplayName("日期")]
        public string Date { get; set; }

        [DisplayName("型式")]
        [ComboBox("Type")]
        [GroupByAttribute]
        public string Type { get; set; }

        [ComboBox]
        [DisplayName("項目")]
        [GroupByAttribute]
        public string Item{ get; set; }

        public int Amount 
        {
            get
            {
                return _amount;
            }
            set 
            { 
                if(value<0)
                    _amount = 0;
                else
                {
                    _amount = value;
                }
            }
        }
        private int _amount;
        [GroupByAttribute]
        [DisplayName("目標")]
        public string Target { get; set; }
        [GroupByAttribute]
        [DisplayName("支付方式")]
        public string PaymentType { get; set; }
        public string Note{ get; set; }
        [Hide]
        public string Img1Path { get; set; }
        [Hide]
        public string Img2Path { get; set; }
        [Picture]
        public string Img1MinPath { get; set; }
        [Picture]
        public string Img2MinPath { get; set; }

        public ItemModel()
        {
            //20241208
            //1.改成顯示圖片
            //2.十張10MB以上圖片
        }
    }
}
