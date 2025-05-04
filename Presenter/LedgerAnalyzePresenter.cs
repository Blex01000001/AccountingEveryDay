using AccountingEveryDay.Interface;
using AccountingEveryDay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AccountingEveryDay.Contract.ChartAnalyzeContract;
using static AccountingEveryDay.Contract.LedgerAnalyzeContract;

namespace AccountingEveryDay.Presenter
{
    internal class LedgerAnalyzePresenter : ILedgerAnalyzePresenter
    {
        ILedgerAnalyzeView view;
        IDataRepository repository = null;

        public LedgerAnalyzePresenter(ILedgerAnalyzeView view)
        {
            this.view = view;
            repository = new DataRepository.DataRepository();

        }
        public void GetDataByFilter(DateTime startTime, DateTime endTime, Dictionary<string, List<string>> conditions, List<string> groupCondition)
        {
            List<ItemModel> datas = repository.GetDataByFilter(startTime, endTime, conditions, groupCondition);
            this.view.DataResponse(datas);
        }

        //public void GetDataByFilter(DateTime startTime, DateTime endTime, List<string> conditions)
        //{
        //    List<ItemModel> datas = repository.GetDataByFilter(startTime, endTime, conditions);
        //    this.view.DataResponse(datas);

        //}

    }
}
