using AccountingEveryDay.Models;
using GroupUtils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace AccountingEveryDay.ChartFolder
{
    internal class StackedColumn100ChartBuilder : AChart
    {
        //Dictionary<string, Dictionary<string, double>> dic;
        private List<string> x = new List<string>();
        private List<int> y = new List<int>();

        public StackedColumn100ChartBuilder(List<ItemModel> datas, DateTime startTime, DateTime endTime, Dictionary<string, List<string>> conditions, List<string> groupCondition) : base(datas, startTime, endTime, conditions, groupCondition)
        {
        }
        public override AChart SetSeries()
        {
            _grouped = DoGroupCondition(_datas, _groupCondition);
            var result = GroupHelper.GetColumnDict<ItemModel>(_grouped);
            foreach (var item in result)
            {
                string name = item.Key;
                var dic = item.Value;
                x = dic.Keys.ToList();
                y = dic.Values.ToList();
                CreatSeries(x.ToArray(), y.ToArray(), name);
            }
            return this;
        }
        protected override void CreatSeries(string[] xdata, int[] ydata, string SeriesName = "")
        {
            Series series = new Series(SeriesName);
            series.ChartType = (SeriesChartType)_chartType;
            series.ChartArea = _chartAreaName;
            series.Legend = _legendName;
            series.Points.DataBindXY(xdata, ydata);
            series.Label = "#VALY\n(#PERCENT)";
            //series["PieLabelStyle"] = "Outside";
            //series["PieLineColor"] = "Black";
            _chart.Series.Add(series);
        }

    }
}
