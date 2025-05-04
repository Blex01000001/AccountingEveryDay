using AccountingEveryDay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AccountingEveryDay.Interface
{
    internal interface IDataRepository
    {
        List<ItemModel> GetDataByTime(DateTime startTime, DateTime endTime);
        List<ItemModel> GetDataByFilter(DateTime startTime, DateTime endTime, List<string> conditions);
        List<ItemModel> GetDataByFilter(DateTime startTime, DateTime endTime, Dictionary<string, List<string>> conditions, List<string> groupCondition);
        (string[], int[]) GetGroupXY(DateTime startTime, DateTime endTime, Dictionary<string, List<string>> conditions, List<string> groupCondition);
        Dictionary<string, Dictionary<string, int>> GetColumnDict(DateTime startTime, DateTime endTime, Dictionary<string, List<string>> conditions, List<string> groupCondition);


        void UpdateByDate(List<ItemModel> newDatas);
        void UpdateByDate(ItemModel model);
        //DataModel getDataByPrice(string npriceme);
        //DataModel getDataBySize(string size);
        //DataModel checkDataQtyByName(string name);

    }
}
