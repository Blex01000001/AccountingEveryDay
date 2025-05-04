using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingEveryDay.Models
{
    //internal class GroupModel
    //{
    //    private static List<List<string>> NameList = new List<List<string>>();
    //    public int ParentSumItem { get; set; }
    //    public string GroupName { get; set; }
    //    public int SumItem { get; set; }
    //    public int Count { get; set; }
    //    public List<GroupModel> Groups { get; set; } = new List<GroupModel>();
    //    public static List<List<string>> GetNameList<T>(List<GroupModel> groups)
    //    {
    //        NameList.Clear();
    //        return GetNameRe<T>(groups).Select(x => x.Distinct().ToList()).ToList();
    //    }
    //    private static List<List<string>> GetNameRe<T>(List<GroupModel> groups, int depth = 0)
    //    {
    //        foreach (GroupModel groupModel in groups)
    //        {
    //            if (GroupModel.NameList.Count <= depth)
    //                GroupModel.NameList.Add(new List<string>());
    //            GroupModel.NameList[depth].Add(groupModel.GroupName);
    //            if (groupModel.Groups == null)
    //                continue;
    //            if (groupModel.Groups.GetType() == typeof(T))
    //                continue;
    //            GetNameRe<T>(groupModel.Groups, depth + 1);
    //        }
    //        return NameList;
    //    }
    //}
}
