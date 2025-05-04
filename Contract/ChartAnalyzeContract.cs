using AccountingEveryDay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace AccountingEveryDay.Contract
{
    internal class ChartAnalyzeContract
    {
        public ChartAnalyzeContract() { }
        public interface IChartAnalyzeView
        {
            //void DataResponse((string[], int[]) datas);
            //void DataResponse(Dictionary<string, Dictionary<string, int>> columnDict);
            void DataResponse(Chart chart);
        }

        public interface IChartAnalyzePresenter
        {
            void GetGroupXY();
            void GetColumnDict();
            //void GetDataByFilter(DateTime startTime, DateTime endTime, Dictionary<string, List<string>> conditions, List<string> groupCondition, int chartType);
            void GetChart(DateTime startTime, DateTime endTime, Dictionary<string, List<string>> conditions, List<string> groupCondition, int chartType);
        }

    }
}
