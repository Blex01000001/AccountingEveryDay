using AccountingEveryDay.Models;
using CSV_Libary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Globalization;
using GroupUtils;

namespace AccountingEveryDay.ChartFolder
{
    internal class LineChartBuilder : AChart
    {
        protected List<ItemModel> _preDatas;
        protected List<ItemModel> _nowDatas;
        protected List<GroupModel> _preGrouped;
        protected List<GroupModel> _nowProuped;

        private List<string> x = new List<string>();
        private List<int> y = new List<int>();
        private List<string> prex = new List<string>();
        private List<int> prey = new List<int>();


        public LineChartBuilder(List<ItemModel> datas, DateTime startTime, DateTime endTime, Dictionary<string, List<string>> conditions, List<string> groupCondition) : base(datas, startTime, endTime, conditions, groupCondition)
        {
            _nowDatas = FilterDateByTime(_datas, _startTime, _endTime);
            _preDatas = FilterDateByTime(_datas, _startTime.AddDays(-7), _endTime.AddDays(-7));
            _nowProuped = DoGroupCondition(_nowDatas, new List<string> { "Date" });
            _preGrouped = DoGroupCondition(_preDatas, new List<string> { "Date" });
            var nowResult = GroupHelper.GetXY<ItemModel>(_nowProuped);
            var preResult = GroupHelper.GetXY<ItemModel>(_preGrouped);
            x = nowResult.Item1.Select(xx => DateTime.Parse(xx).ToString("MM-dd")).ToList();
            y = nowResult.Item2;
            prex = preResult.Item1.Select(xx => DateTime.Parse(xx).ToString("MM-dd")).ToList();
            prey = preResult.Item2;
        }


        public override AChart SetSeries()
        {
            CreatSeries(x.ToArray(), y.ToArray(), "Now");
            CreatSeries(prex.ToArray(), prey.ToArray(), "Pre");
            return this;
        }
        protected override void CreatSeries(string[] xdata, int[] ydata, string SeriesName = "")
        {
            Series series = new Series(SeriesName);
            series.ChartType = (SeriesChartType)_chartType;
            series.ChartArea = _chartAreaName;
            series.Legend = _legendName;
            series.Points.DataBindXY(xdata, ydata);
            series.Label = "#AXISLABEL #VALY";
            series["PieLabelStyle"] = "Outside";
            series["PieLineColor"] = "Black";
            _chart.Series.Add(series);
        }
    }
}
