using AccountingEveryDay.ChartFolder;
using AccountingEveryDay.Interface;
using AccountingEveryDay.Models;
//using AccountingEveryDay.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using static AccountingEveryDay.Contract.ChartAnalyzeContract;

namespace AccountingEveryDay.Presenter
{

    internal class ChartAnalyzePresenter : IChartAnalyzePresenter
    {
        IDataRepository repository = null;
        IChartAnalyzeView view;
        AccountingEveryDay.ChartFolder.AChart chartBuilder = null;
        //List<ItemModel> datas;
        //List<ItemModel> datas2;
        //Dictionary<string, Dictionary<string, int>> dic;


        public ChartAnalyzePresenter(IChartAnalyzeView view)
        {
            this.view = view;
            repository = new DataRepository.DataRepository();
        }
        public void GetGroupXY()
        {
            //(string[], int[]) datas = repository.GetGroupXY();
        }
        public void GetColumnDict()
        {
            //dic = repository.GetColumnDict();
        }

        public void GetChart(DateTime startTime, DateTime endTime, Dictionary<string, List<string>> conditions, List<string> groupCondition, int chartType)
        {
            List<ItemModel> datas2 = repository.GetDataByTime(startTime.AddDays(-7), endTime);
            Type type = Type.GetType($"AccountingEveryDay.ChartFolder.{(ChartType)chartType}ChartBuilder");
            AChart chartBuilder = (AChart)Activator.CreateInstance(type, datas2, startTime, endTime, conditions, groupCondition);//統一傳入一個從資料庫回來的result給AChart，資料的拆解 組裝由各AChart實作
            chartBuilder
                        .SetChartArea("AreaName")
                        //.SetChartTitle("TitleName")
                        .SetLegend("LegendName")
                        .SetChartType((ChartType)chartType)
                        .SetSeries();
            this.view.DataResponse(chartBuilder.GetChart());

            //this.view.DataResponse(chartBuilder.GetChart());

            //用AutoMapper把switch裡的封裝在AChart裡面

            //(string[], int[]) datas = (null, null);
            //(string[], int[]) preDatas = (null, null);

            //switch (chartType)
            //{
            //    case 17:
            //        chartBuilder = new ChartFolder.PieChartBuilder();
            //        datas = repository.GetGroupXY(startTime, endTime, conditions, groupCondition);
            //        break;
            //    case 3:
            //        chartBuilder = new ChartFolder.LineChartBuilder();
            //        datas = repository.GetGroupXY(startTime, endTime, conditions, new List<string> { "Date" });
            //        preDatas = repository.GetGroupXY(startTime.AddDays(-7), endTime.AddDays(-7), conditions, new List<string> { "Date" });
            //        break;
            //    case 12:
            //        chartBuilder = new ChartFolder.StackedColumn100ChartBuilder();
            //        dic = repository.GetColumnDict(startTime, endTime, conditions, groupCondition);
            //        break;
            //}
            //chartBuilder.SetChartArea("AreaName");
            //chartBuilder.SetChartTitle("TitleName");
            //chartBuilder.SetLegend("LegendName");
            //chartBuilder.SetChartType((ChartType)chartType);
            //switch (chartType)
            //{
            //    case 17:
            //        chartBuilder.SetSeries(datas.Item1, datas.Item2);
            //        break;
            //    case 3:
            //        string[] x = datas.Item1.Select(xx => DateTime.Parse(xx).ToString("MM-dd")).ToArray();
            //        int[] y = datas.Item2;
            //        string[] prex = preDatas.Item1.Select(xx => DateTime.Parse(xx).ToString("MM-dd")).ToArray();
            //        int[] prey = preDatas.Item2;

            //        chartBuilder.SetSeries(x, y, "Now");
            //        chartBuilder.SetSeries(prex, prey, "Pre");
            //        break;
            //    case 12:
            //        foreach (var item in dic)
            //        {
            //            string name = item.Key;
            //            var dic = item.Value;
            //            x = dic.Keys.ToArray();
            //            y = dic.Values.ToArray();
            //            chartBuilder.SetSeries(x, y, name);
            //        }
            //        break;
            //}
            //this.view.DataResponse(chartBuilder.GetChart());

        }
    }
}
