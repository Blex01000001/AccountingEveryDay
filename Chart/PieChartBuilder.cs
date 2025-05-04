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
    internal class PieChartBuilder : AChart
    {
        private List<string> x = new List<string>();
        private List<int> y = new List<int>();

        public PieChartBuilder(List<ItemModel> datas, DateTime startTime, DateTime endTime, Dictionary<string, List<string>> conditions, List<string> groupCondition) : base(datas, startTime, endTime, conditions, groupCondition)
        {
            _grouped = DoGroupCondition(_datas, _groupCondition);
            var result = GroupHelper.GetXY<ItemModel>(_grouped);
            x = result.Item1;
            y = result.Item2;
        }
        public override AChart SetSeries()
        {
            CreatSeries(x.ToArray(), y.ToArray());
            return this;
        }

        protected override void CreatSeries(string[] xdata, int[] ydata, string SeriesName = "")
        {
            Series series = new Series(SeriesName);
            series.ChartType = (SeriesChartType)_chartType;
            series.ChartArea = _chartAreaName;
            series.Legend = _legendName;
            series.Points.DataBindXY(xdata, ydata);
            series.Label = "#AXISLABEL #VALY(#PERCENT)";
            series["PieLabelStyle"] = "Outside";
            series["PieLineColor"] = "Black";
            _chart.Series.Add(series);
        }
        public override AChart SetChartArea(string chartAreaName)
        {
            base.SetChartArea(chartAreaName);//先執行BASE，或是直接在這裡重寫
            return this;
        }
    }
}
