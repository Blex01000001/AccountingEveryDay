using AccountingEveryDay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingEveryDay.Contract
{
    internal class LedgerAnalyzeContract
    {
        public LedgerAnalyzeContract() { }
        public interface ILedgerAnalyzeView
        {
            void DataResponse(List<ItemModel> datas);
        }
        public interface ILedgerAnalyzePresenter
        {
            void GetDataByFilter(DateTime startTime, DateTime endTime, Dictionary<string, List<string>> conditions, List<string> groupCondition);

        }
    }
}
