using AccountingEveryDay.Attributies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AccountingEveryDay.Models
{
    internal class DataModel
    {
        [DisplayName("形式")]
        [GroupByAttribute]
        public static string[] Type { get; set; } = { "餐飲", "交通", "水電瓦斯", "電話網路", "居家房租", "衣褲鞋", "學習", "娛樂", "醫療保健", "銀行" };
        [DisplayName("付款形式")]
        [GroupByAttribute]
        public static string[] PaymentType { get; set; } = { "現金", "信用卡", "銀行轉帳" };
        [DisplayName("目標")]
        [GroupByAttribute]
        public static string[] Target { get; set; } = { "自己", "丈夫", "妻子", "父母", "孩子", "家人", "其他" };

        //subitem
        [DisplayName("餐飲")]
        [WhereAttribute]
        public static string[] DietItem { get; set; } = { "早餐", "午餐", "晚餐", "其他" };
        [DisplayName("交通")]
        [WhereAttribute]
        public static string[] TransportationItem { get; set; } = { "汽車", "機車", "捷運", "公車", "客運", "火車" };
        [DisplayName("水電")]
        [WhereAttribute]
        public static string[] UtilityItem { get; set; } = { "水費", "電費", "瓦斯費", "稅金" };
        [DisplayName("網路")]
        [WhereAttribute]
        public static string[] NetworkItem { get; set; } = { "電話", "網路", "第四台" };
        [DisplayName("居家")]
        [WhereAttribute]
        public static string[] HomeItem { get; set; } = { "房租", "家具", "修繕" };
        [DisplayName("衣褲鞋")]
        [WhereAttribute]
        public static string[] ClothingItem { get; set; } = { "衣服", "褲子", "鞋子", "裝飾" }; 
        [DisplayName("學習")]
        [WhereAttribute]
        public static string[] LearningItem { get; set; } = { "靜態", "動態" };
        [DisplayName("娛樂")]
        [WhereAttribute]
        public static string[] Entertainment { get; set; } = { "運動", "交際" };
        [DisplayName("醫療")]
        [WhereAttribute]
        public static string[] MedicalItem { get; set; } = { "看診", "成藥", "用品" };
        [DisplayName("銀行")]
        [WhereAttribute]
        public static string[] BankItem { get; set; } = { "貸款", "信用卡費", "定期定額" };
        [Hide]
        public static Dictionary<string, string[]> ItemTypes { get; } = new Dictionary<string, string[]>() 
        {
            {"餐飲",DietItem},
            {"交通", TransportationItem},
            {"水電瓦斯", UtilityItem},
            {"電話網路", NetworkItem},
            {"居家房租", HomeItem},
            {"衣褲鞋", ClothingItem},
            {"學習", LearningItem},
            {"娛樂", Entertainment},
            {"醫療保健", MedicalItem},
            {"銀行", BankItem}
        };
    }
}
