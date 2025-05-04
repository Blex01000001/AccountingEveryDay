using AccountingEveryDay.Forms;
using AccountingEveryDay.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountingEveryDay
{
    internal static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(SingletonForm.GetInstance("記一筆"));

            // datas.Group(x=>x.Payment,x=>x.Target) => Func<ItemModel,String>()

            //List<ItemModel> itemModels = new List<ItemModel>()
            //{
            //    new ItemModel(){PaymentType ="信用卡", Target = "自己"},
            //    new ItemModel(){PaymentType ="現金", Target = "家人"},
            //};

            //Test(x => x.PaymentType);
            //Test3();

        }
        static void Test2(params string[] conditions)
        {
            List<ItemModel> itemModels = new List<ItemModel>()
            {
                new ItemModel(){PaymentType ="信用卡", Target = "自己"},
                new ItemModel(){PaymentType ="現金", Target = "家人"},
                new ItemModel(){PaymentType ="信用卡", Target = "父母"},
                new ItemModel(){PaymentType ="現金", Target = "朋友"},
                new ItemModel(){PaymentType ="LINE PAY", Target = "自己"},
                new ItemModel(){PaymentType ="LINE PAY", Target = "家人"},
                new ItemModel(){PaymentType ="LINE PAY", Target = "父母"},
                new ItemModel(){PaymentType ="LINE PAY", Target = "朋友"},
            };
            IEnumerable<IGrouping<object, ItemModel>> groupItemModels = null;
            foreach (string item in conditions)
            {
                groupItemModels = itemModels.GroupBy(x => typeof(ItemModel).GetProperty(item).GetValue(x));
            }
            var ss = groupItemModels.Select(x => new {name = x.Key, count = x.Count()}).ToList();
            foreach (var item1 in ss)
            {
                Console.WriteLine($"name: {item1.name} count: {item1.count}");
            }

        }
        static void Test3()
        {
            List<ItemModel> itemModels = new List<ItemModel>()
            {
                new ItemModel(){Date = "1", PaymentType ="信用卡", Target = "自己"},
                new ItemModel(){Date = "1", PaymentType ="信用卡", Target = "自己"},
                new ItemModel(){Date = "2", PaymentType ="現金", Target = "家人"},
                new ItemModel(){Date = "3", PaymentType ="信用卡", Target = "父母"},
                new ItemModel(){Date = "4", PaymentType ="現金", Target = "朋友"},
                new ItemModel(){Date = "5", PaymentType ="LINE PAY", Target = "自己"},
                new ItemModel(){Date = "6", PaymentType ="LINE PAY", Target = "家人"},
                new ItemModel(){Date = "7", PaymentType ="LINE PAY", Target = "父母"},
                new ItemModel(){Date = "8", PaymentType ="LINE PAY", Target = "朋友", Type = "hh"},
                new ItemModel(){Date = "9", PaymentType ="LINE PAY", Target = "朋友", Type = "aa"},
                new ItemModel(){Date = "10", PaymentType ="LINE PAY", Target = "朋友", Type = "ss"},
                new ItemModel(){Date = "10", PaymentType ="LINE PAY", Target = "朋友", Type = "ss"},
            };
            List<string> groupByFields = new List<string> { "PaymentType", "Target" };
            // **動態建立 GroupBy Key**
            var groupKeySelector = CreateGroupKeySelector<ItemModel>(groupByFields);
            var grouped = itemModels.GroupBy(groupKeySelector).Select(x => new {name = x.Key, count = x.Count()}).ToList();
            foreach (var item1 in grouped)
            {
                Console.WriteLine($"name: {item1.name} count: {item1.count}");
            }
            Console.WriteLine($"\n\n");
            var grouped1 = itemModels.GroupBy(groupKeySelector).ToList();
            foreach (var group in grouped1)
            {
                Console.WriteLine($"Group: {string.Join(", ", group.Key.GetType().GetProperties().Select(p => $"{p.Name}={p.GetValue(group.Key)}"))}");
                foreach (var person in group)
                {
                    Console.WriteLine($"  {person.Date}");
                }
            }

        }

        static Func<T, object> CreateGroupKeySelector<T>(List<string> fieldNames)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "x");

            // **建立匿名類別 new { Age = x.Age, City = x.City }**
            var bindings = fieldNames.Select(fieldName =>
            {
                PropertyInfo prop = typeof(T).GetProperty(fieldName);
                if (prop == null)
                    throw new ArgumentException($"找不到屬性 {fieldName} in {typeof(T).Name}");

                return Expression.Bind(prop, Expression.Property(param, prop));
            });

            // **產生 new { ... }**
            Expression body = Expression.MemberInit(Expression.New(typeof(T)), bindings);

            // **轉換成 Lambda**
            return Expression.Lambda<Func<T, object>>(body, param).Compile();
        }
        static void Test(params Expression<Func<ItemModel, string>>[] conditions)
        {
            List<ItemModel> itemModels = new List<ItemModel>()
            {
                new ItemModel(){PaymentType ="信用卡", Target = "自己"},
                new ItemModel(){PaymentType ="現金", Target = "家人"},
                new ItemModel(){PaymentType ="信用卡", Target = "父母"},
                new ItemModel(){PaymentType ="現金", Target = "朋友"},
                new ItemModel(){PaymentType ="LINE PAY", Target = "自己"},
                new ItemModel(){PaymentType ="LINE PAY", Target = "家人"},
                new ItemModel(){PaymentType ="LINE PAY", Target = "父母"},
                new ItemModel(){PaymentType ="LINE PAY", Target = "朋友"},
            };
            //typeof(ItemModel).GetProperty("PaymentType").GetValue()
            //Expression item = conditions[0];

            foreach (var item in conditions)
            {
                MemberExpression memberExpression = (MemberExpression)item.Body;
                if (memberExpression.Member is PropertyInfo propertyInfo)
                {
                    var groupItemModels = itemModels.GroupBy(x => typeof(ItemModel).GetProperty("PaymentType").GetValue(x)).Select(x => new {name = x.Key, count = x.Count()}).ToList();
                    Console.WriteLine($"count: {groupItemModels.Count}");
                    Console.WriteLine($"groupItemModels: {groupItemModels}");
                    foreach (var item1 in groupItemModels)
                    {
                        Console.WriteLine($"name: {item1.name} count: {item1.count}");
                    }
                }

            }

            var aa = typeof(ItemModel).GetProperty("PaymentType").GetValue(itemModels[0]);
            Console.WriteLine($"111name: {aa}");

            //var ss = itemModels.GroupBy(x => x.PaymentType).Select(x => new
            //{
            //    Name = x.Key,
            //    Count = x.Count()
            //}).ToList();

            //foreach (var item in groupItemModels)
            //{
            //    Console.WriteLine($"{item.Name} {item.Count}");
            //}


            //itemModels = itemModels.GroupBy(x =>
            //{
            //    string str = "";
            //    foreach (var item in conditions)
            //    {
            //        MemberExpression memberExpression = (MemberExpression)item.Body;
            //        if (memberExpression.Member is PropertyInfo propertyInfo)
            //        {
            //            str += memberExpression.ToString();
            //        }
            //    }
            //    return str;
            //}).ToList();
            //foreach (var item in conditions)
            //{
            //    MemberExpression memberExpression = (MemberExpression)item.Body;
            //    if (memberExpression.Member is PropertyInfo propertyInfo)
            //    {

            //        var groupItemModels = itemModels.GroupBy(x => memberExpression).ToList();

            //    }
            //}

            // 上面執行完會產生
            // {
            //   PaymentType = 
            // }

            // ExpendoObject => IDictionary


            //foreach (ItemModel i in itemModels)
            //{
            //    Console.WriteLine($"PropertyName:{propertyInfo.Name} PropertyValue:{propertyInfo.GetValue(i)}");
            //}
        }
        static void Group(List<ItemModel> datas,Expression<Func<ItemModel,string>> condition, Expression<Func<ItemModel, string>> condition2)
        {
            //foreach (ItemModel item in datas)
            //{
            //    Console.WriteLine(condition.Compile().Invoke(item));
            //    Console.WriteLine(condition2.Compile().Invoke(item));

            //}



            MemberExpression memberExpression = (MemberExpression)condition.Body;
            if (memberExpression.Member is PropertyInfo propertyInfo)
            {
                foreach (ItemModel item in datas)
                {
                    Console.WriteLine($"PropertyName:{propertyInfo.Name} PropertyValue:{propertyInfo.GetValue(item)}");

                }
            }
            //else if(memberExpression.Member is FieldInfo fieldInfo)
            //{

            //}
            //foreach (ItemModel item in datas) { 
            //    Console.WriteLine(condition(item));
            //    Console.WriteLine(condition2(item));

            //}
        }

    }
}
