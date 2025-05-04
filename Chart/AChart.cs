using AccountingEveryDay.Interface;
using AccountingEveryDay.Models;
using AccountingEveryDay.Utility;
using GroupUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace AccountingEveryDay.ChartFolder
{
    abstract class AChart
    {
        protected List<GroupModel> _grouped;
        protected List<ItemModel> _datas;

        protected Dictionary<string, List<string>> _conditions;
        protected List<string> _groupCondition;
        protected Chart _chart;
        protected SeriesChartType _chartType;
        protected string _chartAreaName;
        protected string _legendName;
        protected DateTime _startTime;
        protected DateTime _endTime;
        public AChart(List<ItemModel> datas, DateTime startTime, DateTime endTime, Dictionary<string, List<string>> conditions, List<string> groupCondition)
        {
            this._startTime = startTime;
            this._endTime = endTime;

            this._datas = FilterDateByTime(datas, _startTime, _endTime);
            this._conditions = conditions;
            this._groupCondition = groupCondition;
            this._chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 400;

        }
        protected List<ItemModel> FilterDateByTime(List<ItemModel> datas, DateTime _startTime, DateTime _endTime)
        {
            return datas.Where(d => {
                if (DateTime.TryParseExact(d.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    return parsedDate.Date >= _startTime && parsedDate.Date <= _endTime;
                }
                Console.WriteLine($"false d.Date: {parsedDate}");
                return false;
            }).ToList();
        }

        protected List<GroupModel> DoGroupCondition(List<ItemModel> datas, List<string> groupCondition)
        {
            return GroupHelper.Recursion<ItemModel>(datas, groupCondition, "Amount");
            //GroupHelper.PrintObj<ItemModel>(_grouped);
        }
        protected List<ItemModel> DoWhereCondition(List<ItemModel> datas)
        {
            foreach (var prop in typeof(ItemModel).GetProperties())
            {
                string propName = prop.Name;
                //Console.WriteLine($"propName {propName}");
                if (_conditions.TryGetValue(propName, out List<string> filter))//如果conditions裡有才篩選
                {
                    datas = _datas.Where(x => filter.Count > 0 ? filter.Contains(prop.GetValue(x)) : true).ToList();
                }
            }
            return datas;
        }

        //protected AChart(List<ItemModel> datas)
        //{
        //    _datas = datas;
        //}

        public Chart GetChart()
        {
            return _chart;
        }
        public virtual AChart SetChartArea(string chartAreaName)
        {
            _chartAreaName = chartAreaName;
            ChartArea chartArea = new ChartArea(chartAreaName);
            chartArea.AxisX.Title = "AxisX.Title";
            chartArea.AxisY.Title = "AxisY.Title";
            chartArea.InnerPlotPosition = new ElementPosition(10, 10, 80, 80);
            chartArea.BackColor = Color.FromArgb(200, 200, 200);
            _chart.ChartAreas.Add(chartArea);
            return this;
        }
        public virtual AChart SetChartTitle(string chartTitle)
        {
            Title Title = new Title();
            Title.Text = chartTitle;
            Title.Font = new Font("微軟正黑體", 20, FontStyle.Bold);
            Title.ForeColor = Color.DarkBlue;
            Title.Alignment = ContentAlignment.TopCenter;
            _chart.Titles.Add(Title);
            return this;

        }
        public virtual AChart SetChartType(ChartType chartType)
        {
            _chartType = (SeriesChartType)chartType;
            return this;

        }
        public virtual AChart SetLegend(string legendName)
        {
            _legendName = legendName;
            Legend legend = new Legend(legendName);
            legend.Docking = Docking.Top;
            legend.Enabled = true;
            _chart.Legends.Add(legend);
            return this;    
        }
        public abstract AChart SetSeries();
        protected abstract void CreatSeries(string[] xdata, int[] ydata, string SeriesName = "");
    }
}
