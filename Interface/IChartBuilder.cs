using AccountingEveryDay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace AccountingEveryDay.Interface
{
    public interface IChartBuilder
    {
        void SetChartType(ChartType chartType);
        void SetChartArea(string chartAreaName);
        void SetChartTitle(string titleName);
        void SetLegend(string legendName);
        void SetSeries(string[] xdata, int[] ydata, string SeriesName = null);
        Chart GetChart();
    }
}
