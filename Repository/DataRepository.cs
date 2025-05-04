using AccountingEveryDay.Interface;
using AccountingEveryDay.Models;
using CSV_Libary;
using GroupUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AccountingEveryDay.DataRepository
{
    internal class DataRepository : IDataRepository
    {
        List<ItemModel> datas;
        List<GroupModel> grouped;
        public DataRepository()
        {
            datas = new List<ItemModel>();
            grouped = new List<GroupModel>();
        }
        public List<ItemModel> GetDataByTime(DateTime startTime, DateTime endTime)
        {
            List<ItemModel> datas = new List<ItemModel>();
            TimeSpan timeSpan = endTime - startTime;
            CSV csv = new CSV();  // Model
            for (int i = 0; i < timeSpan.Days + 1; i++)
            {
                string date = startTime.AddDays(i).ToString("yyyy-MM-dd");
                string dataFilePath = "C:\\Users\\USERA\\Documents\\C#\\AccountingEveryDay_data\\" + date + "\\data.csv";
                List<ItemModel> itemModel = csv.Read<ItemModel>(dataFilePath);
                if (itemModel == null)
                    continue;
                datas.AddRange(itemModel);
            }
            return datas;
        }

        public void UpdateByDate(List<ItemModel> newDatas)
        {
            string dataFilePath = "C:\\Users\\USERA\\Documents\\C#\\AccountingEveryDay_data\\";
            foreach (var item in newDatas)
            {
                ItemModel model = new ItemModel();
                model.Date = item.Date;
                model.Type = item.Type;
                model.Item = item.Item;
                model.Amount = item.Amount;
                model.Target = item.Target;
                model.PaymentType = item.PaymentType;
                model.Note = item.Note;
                model.Img1Path = item.Img1Path;
                model.Img2Path = item.Img2Path;
                model.Img1MinPath = item.Img1MinPath;
                model.Img2MinPath = item.Img2MinPath;

                CSV csv = new CSV();
                csv.Write(dataFilePath + item.Date + "\\data.csv", model);
            }
        }
        public void UpdateByDate(ItemModel newDatas)
        {
            string dataFilePath = "C:\\Users\\USERA\\Documents\\C#\\AccountingEveryDay_data\\";

            CSV csv = new CSV();
            csv.Write(dataFilePath + newDatas.Date + "\\data.csv", newDatas);

        }
        List<ItemModel> IDataRepository.GetDataByFilter(DateTime startTime, DateTime endTime, List<string> conditions)
        {
            List<ItemModel> datas = GetDataByTime(startTime, endTime);
            TimeSpan timeSpan = endTime - startTime;
            CSV csv = new CSV();  // Model
            for (int i = 0; i < timeSpan.Days + 1; i++)
            {
                string date = startTime.AddDays(i).ToString("yyyy-MM-dd");
                string dataFilePath = "C:\\Users\\USERA\\Documents\\C#\\AccountingEveryDay_data\\" + date + "\\data.csv";
                List<ItemModel> itemModel = csv.Read<ItemModel>(dataFilePath);
                if (itemModel == null)
                    continue;
                datas.AddRange(itemModel);
            }
            //DbSet<Rent> Rent
            // db.Rent.Where
            foreach (var item in conditions)
            {
                Console.Write($"{item} ");
            }
            Console.Write($"\n");
            datas = datas.Where(x => conditions.Count > 0 ? conditions.Contains(x.Item) : true).ToList();
            var groupedData = datas.GroupBy(x => x.Type).Select(x => new
            {
                name = x.Key,
            }).ToList();

            return datas;
        }
        //private List<GroupModel> GroupByList<T>(List<T> datas, List<string> propNames, int depth = 0, int parSumItem = -1)
        //{
        //    if (propNames.Count < depth + 1)
        //        return null;
        //        //return datas.ToList();
        //    List<string> orderByList = new List<string> { "Year", "Month" };
        //    string propName = propNames[depth];
        //    return datas.GroupBy(x => typeof(T).GetProperty(propName).GetValue(x))
        //        .Select(x => new GroupModel
        //        {
        //            ParentSumItem = parSumItem < 0 ? x.Sum(y => (int)typeof(T).GetProperty("Amount").GetValue(y)) : parSumItem,
        //            GroupName = (string)x.Key,
        //            SumItem = x.Sum(y => (int)typeof(T).GetProperty("Amount").GetValue(y)),
        //            Count = x.Count(),
        //            Groups = GroupByList(x.ToList(), propNames, depth + 1, x.Sum(y => (int)typeof(T).GetProperty("Amount").GetValue(y)))
        //        })
        //        .OrderByDescending(x => orderByList.Contains(propName) == true ? 1 : x.SumItem)
        //        //.OrderBy(x => orderByList.Contains(propName) == true ? x.GroupName : 1)
        //        .ToList();
        //}
        public List<ItemModel> GetDataByFilter(DateTime startTime, DateTime endTime, Dictionary<string, List<string>> conditions, List<string> groupCondition)
        {
            //datas = GetDataByTime(startTime, endTime);
            //foreach (var prop in typeof(ItemModel).GetProperties())
            //{
            //    string propName = prop.Name;
            //    //Console.WriteLine($"propName {propName}");
            //    if (conditions.TryGetValue(propName, out List<string> filter))//如果conditions裡有才篩選
            //    {
            //        datas = datas.Where(x => filter.Count > 0 ? filter.Contains(prop.GetValue(x)) : true).ToList();
            //    }
            //}
            //grouped = GroupByList<ItemModel>(datas, groupCondition);
            //Console.WriteLine($"\n======Group======");
            //printObj(grouped);
            return datas;
        }
        public (string[], int[]) GetGroupXY(DateTime startTime, DateTime endTime, Dictionary<string, List<string>> conditions, List<string> groupCondition)
        {
            //GetDataByFilter(startTime, endTime, conditions, groupCondition);
            //List<string> x = new List<string>();
            //List<int> y = new List<int>();
            //foreach (GroupModel groupModel in grouped)
            //{
            //    x.Add(groupModel.GroupName);
            //    y.Add(groupModel.SumItem);
            //}
            //return (x.ToArray(), y.ToArray());
            return (null,null);
        }
        private string[] GetGroupName(List<GroupModel> datas)
        {
            List<string> x = new List<string>();
            foreach (GroupModel groupModel in datas)
            {
                x.Add(groupModel.GroupName);
            }
            return x.ToArray();
        }
        //private int[] GetSumItem(List<GroupModel> datas)
        //{
        //    List<int> x = new List<int>();
        //    foreach (GroupModel groupModel in datas)
        //    {
        //        x.Add(groupModel.SumItem);
        //    }
        //    return x.ToArray();
        //}

        public Dictionary<string, Dictionary<string, int>> GetColumnDict(DateTime startTime, DateTime endTime, Dictionary<string, List<string>> conditions, List<string> groupCondition)
        {
            //GetDataByFilter(startTime, endTime, conditions, groupCondition);
            //var secGroupNames = GroupModel.GetNameList<ItemModel>(grouped)[1];
            //var dataDict = secGroupNames.ToDictionary(
            //    secName => secName,
            //    secName => grouped.ToDictionary(
            //        groupModel => groupModel.GroupName,
            //        groupModel =>
            //        {
            //            var names = GetGroupName(groupModel.Groups);
            //            var sums = GetSumItem(groupModel.Groups);
            //            int idx = Array.IndexOf(names, secName);
            //            return idx >= 0 ? sums[idx] : 0;
            //        }
            //    )
            //);
            //return dataDict;
            return null;
        }
        //public void printObj(List<GroupModel> obj, int depth = 0)
        //{
        //    if (obj == null)
        //        return;
        //    foreach (var item in obj)
        //    {
        //        Type type = item.GetType(); // 取得物件的型別
        //        PropertyInfo[] properties1 = type.GetProperties(); // 取得所有屬性
        //        if (type == typeof(ItemModel))
        //            continue;
        //        string name = type.GetProperty("GroupName").GetValue(item).ToString();
        //        int parSumItem = (int)type.GetProperty("ParentSumItem").GetValue(item);
        //        int sumItem = (int)type.GetProperty("SumItem").GetValue(item);
        //        string count = type.GetProperty("Count").GetValue(item).ToString();
        //        double pre = ((double)sumItem / (double)parSumItem) * 100;

        //        Console.WriteLine($"{new string('\t', depth)}{name}\t{sumItem.ToString()}\t{pre.ToString("0.0")}%");
        //        foreach (var prop in properties1)
        //        {
        //            //Console.WriteLine($"{prop.Name}: {prop.GetValue(item)}");
        //            if (prop.GetValue(item) is System.Collections.IEnumerable && !(prop.GetValue(item) is string))
        //            {
        //                //Console.WriteLine($"是可列舉的 {prop.GetValue(item).GetType()}");
        //                printObj((List<GroupModel>)prop.GetValue(item), depth + 1);
        //            }
        //        }
        //    }
        //}

    }
}
