using AccountingEveryDay.Interface;
using AccountingEveryDay.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace AccountingEveryDay.Utility
{
    public class ChartBuilder : IChartBuilder
    {
        protected List<ItemModel> datas;

        protected Chart _chart;
        protected SeriesChartType _chartType;
        protected string _chartAreaName;
        protected string _legendName;
        public ChartBuilder(List<ItemModel> datas)
        {
            this.datas = datas;
            _chart = new Chart();
            _chart.Width = 500;
            _chart.Height = 400;
        }
        public Chart GetChart()
        {
            return _chart;
        }

        public void SetChartArea(string chartAreaName)
        {
            _chartAreaName = chartAreaName;
            ChartArea chartArea = new ChartArea(chartAreaName);
            chartArea.AxisX.Title = "AxisX.Title";
            chartArea.AxisY.Title = "AxisY.Title";
            chartArea.InnerPlotPosition = new ElementPosition(10, 10, 80, 80);
            chartArea.BackColor = Color.FromArgb(200, 200, 200);
            _chart.ChartAreas.Add(chartArea);
        }

        public void SetChartTitle(string chartTitle)
        {
            Title Title = new Title();
            Title.Text = chartTitle;
            Title.Font = new Font("微軟正黑體", 20, FontStyle.Bold);
            Title.ForeColor = Color.DarkBlue;
            Title.Alignment = ContentAlignment.TopCenter;
            _chart.Titles.Add(Title);
        }

        public void SetChartType(ChartType chartType)
        {
            _chartType = (SeriesChartType)chartType;
        }

        public void SetLegend(string legendName)
        {
            _legendName = legendName;
            Legend legend = new Legend(legendName);
            legend.Docking = Docking.Top;
            legend.Enabled = true;
            _chart.Legends.Add(legend);
        }

        public void SetSeries(string[] xdata, int[] ydata, string SeriesName = "")
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
    }
}
