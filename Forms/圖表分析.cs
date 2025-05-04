using AccountingEveryDay.Attributies;
using AccountingEveryDay.Models;
using AccountingEveryDay.Presenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static AccountingEveryDay.Contract.ChartAnalyzeContract;
using static AccountingEveryDay.Contract.LedgerAnalyzeContract;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AccountingEveryDay.Forms
{
    public partial class 圖表分析 : Form, IChartAnalyzeView
    {
        IChartAnalyzePresenter chartAnalyzePresenter = null;
        Dictionary<string, List<string>> filterConditions = new Dictionary<string, List<string>>();
        List<string> groupCondition = new List<string>();
        Dictionary<string, Dictionary<string, int>> columnDict = new Dictionary<string, Dictionary<string, int>>();
        List<ItemModel> datas = new List<ItemModel>();
        string[] strings;
        int[] ints;
        bool status = false;
        int chartType = 0;
        Chart chart = null;
        public 圖表分析()
        {
            InitializeComponent();
            chartAnalyzePresenter = new ChartAnalyzePresenter(this);

            PropertyInfo[] props = typeof(DataModel).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetCustomAttribute(typeof(HideAttribute)) != null)
                    continue;
                //if (prop.GetCustomAttribute(typeof(GroupByAttribute)) != null)
                //    continue;
                string[] propNames = (string[])prop.GetValue(null);
                FlowLayoutPanel LayoutPanel = new FlowLayoutPanel();
                //LayoutPanel.BorderStyle = BorderStyle.FixedSingle;
                LayoutPanel.Width = 500;
                LayoutPanel.Height = (propNames.Length / 8 + 1) * 25;

                FlowLayoutPanel labelPanel = new FlowLayoutPanel();
                //labelPanel.BorderStyle = BorderStyle.FixedSingle;
                labelPanel.Width = 55;
                labelPanel.Height = (propNames.Length / 8 + 1) * 25; ;

                Label label = new Label();
                label.Text = prop.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                label.Width = 55;
                label.AutoSize = true;
                int verticalMargin = (LayoutPanel.Height - label.Height) / 2;
                label.Margin = new Padding(0, verticalMargin + 3, 0, 0);
                labelPanel.Controls.Add(label);
                LayoutPanel.Controls.Add(labelPanel);

                FlowLayoutPanel checkPanel = new FlowLayoutPanel();
                //checkPanel.BorderStyle = BorderStyle.FixedSingle;
                checkPanel.Width = 430;
                checkPanel.Height = (propNames.Length / 8 + 1) * 22;
                LayoutPanel.Controls.Add(checkPanel);

                CheckBox checkAll = new CheckBox();
                List<CheckBox> checkboxes = new List<CheckBox>();
                checkAll.Text = "All";
                checkAll.AutoSize = true;
                checkAll.Tag = prop.Name;
                checkAll.CheckedChanged += (sender, e) => CheckAll_CheckedChanged(sender, null, checkAll, checkboxes);
                checkPanel.Controls.Add(checkAll);

                foreach (string propName in propNames)
                {
                    CheckBox checkBox = new CheckBox();
                    checkBox.Text = propName;
                    checkBox.AutoSize = true;
                    checkBox.CheckedChanged += (sender, e) => Checkboxes_CheckedChanged(sender, null, checkAll, checkboxes);
                    checkBox.Tag = prop.Name;
                    checkPanel.Controls.Add(checkBox);
                    checkboxes.Add(checkBox);
                }
                flowLayoutPanel2.Controls.Add(LayoutPanel);
            }
            PropertyInfo[] ItemModelprops = typeof(ItemModel).GetProperties();

            foreach (PropertyInfo prop in ItemModelprops)
            {
                Console.WriteLine($"prop.Name: {prop.Name}");
                if (prop.GetCustomAttribute(typeof(GroupByAttribute)) == null)
                    continue;
                //string[] propNames= (string[])prop.GetValue(null);

                FlowLayoutPanel LayoutPanel = new FlowLayoutPanel();
                //LayoutPanel.BorderStyle = BorderStyle.FixedSingle;
                LayoutPanel.Width = 500;
                LayoutPanel.Height = 25;

                FlowLayoutPanel labelPanel = new FlowLayoutPanel();
                //labelPanel.BorderStyle = BorderStyle.FixedSingle;
                labelPanel.Width = 55;
                labelPanel.Height = 25; ;

                Label label = new Label();
                label.Text = prop.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                label.Width = 55;
                label.AutoSize = true;
                int verticalMargin = (LayoutPanel.Height - label.Height) / 2;
                label.Margin = new Padding(0, verticalMargin + 3, 0, 0);
                labelPanel.Controls.Add(label);
                LayoutPanel.Controls.Add(labelPanel);

                FlowLayoutPanel checkPanel = new FlowLayoutPanel();
                //checkPanel.BorderStyle = BorderStyle.FixedSingle;
                checkPanel.Width = 430;
                checkPanel.Height = (prop.Name.Length / 8 + 1) * 22;
                LayoutPanel.Controls.Add(checkPanel);
                //foreach (string propName in prop.Name)
                //{
                CheckBox checkBox = new CheckBox();
                checkBox.Text = prop.Name;
                checkBox.AutoSize = true;
                checkBox.CheckedChanged += GroupCheckBox_CheckedChanged;
                checkPanel.Controls.Add(checkBox);
                //}
                flowLayoutPanel1.Controls.Add(LayoutPanel);
            }

        }
        private void CheckAll_CheckedChanged(object sender, EventArgs e, CheckBox checkAllBox, List<CheckBox> checkboxes)
        {
            if (status)
                return;
            status = true;
            CheckBox checkBox = (CheckBox)sender;

            string typeName;
            bool isChecked = checkAllBox.Checked;

            var GroupByAttributeProps = typeof(DataModel).GetProperties().Where(x => x.GetCustomAttribute(typeof(GroupByAttribute)) != null).Select(x => x.Name).ToList();
            if (GroupByAttributeProps.Contains((string)checkBox.Tag))
            {
                typeName = (string)checkBox.Tag;
            }
            else
            {
                typeName = "Item";
            }
            Console.WriteLine($"GroupByAttributeProps: {GroupByAttributeProps.Count}");



            foreach (CheckBox checkbox in checkboxes)
            {
                checkbox.Checked = isChecked;
                if (!filterConditions.TryGetValue(typeName, out List<string> conditions))
                {
                    filterConditions.Add(typeName, new List<string>());
                }
                UpdateFilterConditions(filterConditions[typeName], checkbox.Text, isChecked);
            }
            status = false;
            Console.Write($"\n");
            foreach (var item in filterConditions)
            {
                Console.Write($"{item.Key}");
                foreach (var item1 in item.Value)
                {
                    Console.Write($" {item1}");
                }
                Console.Write($"\n");
            }

        }
        private void Checkboxes_CheckedChanged(object sender, EventArgs e, CheckBox checkAllBox, List<CheckBox> checkboxes)
        {
            CheckBox checkBox = (CheckBox)sender;
            string typeName;
            var GroupByAttributeProps = typeof(DataModel).GetProperties().Where(x => x.GetCustomAttribute(typeof(GroupByAttribute)) != null).Select(x => x.Name).ToList();
            if (GroupByAttributeProps.Contains((string)checkBox.Tag))
            {
                typeName = (string)checkBox.Tag;
            }
            else
            {
                typeName = "Item";
            }
            Console.WriteLine($"GroupByAttributeProps: {GroupByAttributeProps.Count}");
            if (status)
                return;
            status = true;


            if (!filterConditions.TryGetValue(typeName, out List<string> conditions))
            {
                conditions = new List<string>();
                filterConditions[typeName] = conditions;
            }

            if (checkBox.Checked)
            {
                UpdateFilterConditions(conditions, checkBox.Text, true);
            }
            else
            {
                UpdateFilterConditions(conditions, checkBox.Text, false);
            }

            checkAllBox.Checked = checkboxes.All(cb => cb.Checked);
            status = false;
            Console.Write($"\n");
            foreach (var item in filterConditions)
            {
                Console.Write($"{item.Key}");
                foreach (var item1 in item.Value)
                {
                    Console.Write($" {item1}");
                }
                Console.Write($"\n");
            }

        }
        private void GroupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (checkBox.Checked)
            {
                groupCondition.Add(checkBox.Text);
            }
            else
            {
                groupCondition.Remove(checkBox.Text);
            }

            foreach (var item in groupCondition)
            {
                Console.WriteLine($"{item}");
            }
        }
        private void UpdateFilterConditions(List<string> conditions, string typeName, bool isCheck)
        {
            if (!isCheck)
            {
                conditions.Remove(typeName);
                return;
            }
            else if (!conditions.Contains(typeName))
            {
                conditions.Add(typeName);
            }

        }
        void IChartAnalyzeView.DataResponse(Chart chart)
        {
            this.chart = chart;
            UpdateChart();
            //this.strings = datas.Item1;
            //this.ints = datas.Item2;
            //Console.WriteLine($"DataResponse");
            //foreach (var item in strings)
            //{
            //    Console.WriteLine($"DataResponse: {item}");
            //}
            //foreach (var item in ints)
            //{
            //    Console.WriteLine($"DataResponse: {item}");
            //}
        }

        //void IChartAnalyzeView.DataResponse(Dictionary<string, Dictionary<string, int>> columnDict)
        //{
        //    this.columnDict = columnDict;
        //}
        //private void DrawStackedColumn100()
        //{
        //    chartAnalyzePresenter.GetDataByFilter(startPicker.Value, endPicker.Value, filterConditions, groupCondition);
        //    chartAnalyzePresenter.GetColumnDict();
        //    string[] x = this.strings;
        //    int[] y = this.ints;

        //    chart1.Series.Clear();
        //    chart1.ChartAreas.Clear();
        //    chart1.Legends.Clear();
        //    chart1.Titles.Clear();
        //    chart1.Annotations.Clear();

        //    ChartArea chartArea = new ChartArea("ColumnArea");
        //    chartArea.AxisX.Title = "AxisX.Title";
        //    chartArea.AxisX.TitleFont = new Font("微軟正黑體", 10);
        //    chartArea.AxisX.LabelStyle.Interval = 1;

        //    chartArea.AxisY.Title = "金額";
        //    chartArea.AxisY.TitleFont = new Font("微軟正黑體", 10);
        //    chartArea.InnerPlotPosition = new ElementPosition(10, 10, 80, 80);
        //    chartArea.BackColor = Color.FromArgb(200, 200, 200);
        //    chart1.ChartAreas.Add(chartArea);

        //    Legend legend = new Legend("Main Legeng");
        //    legend.Docking = Docking.Top;
        //    legend.Enabled = true;
        //    chart1.Legends.Add(legend);

        //    Title chartTitle = new Title();
        //    chartTitle.Text = "ChartTitle";
        //    chartTitle.Font = new Font("微軟正黑體", 20, FontStyle.Bold);
        //    chartTitle.ForeColor = Color.DarkBlue;
        //    chartTitle.Alignment = ContentAlignment.TopCenter;
        //    chart1.Titles.Add(chartTitle);


        //    foreach (var item in columnDict)
        //    {
        //        string name = item.Key;
        //        var dic = item.Value;
        //        var series = new Series(name)
        //        {
        //            ChartType = SeriesChartType.StackedColumn100,
        //        };

        //        foreach (var item1 in dic)
        //        {
        //            series.Points.AddXY(item1.Key, item1.Value);
        //        }
        //        chart1.Series.Add(series);
        //    }
        //}
        //private void DrawLine()
        //{
        //    chartAnalyzePresenter.GetDataByFilter(startPicker.Value, endPicker.Value, filterConditions, new List<string> { "Date" });
        //    chartAnalyzePresenter.GetGroupXY();
        //    List<ItemModel> datas = this.datas;
        //    string[] x = this.strings.Select(xx => DateTime.Parse(xx).ToString("MM-dd")).ToArray();
        //    int[] y = this.ints;

        //    chartAnalyzePresenter.GetDataByFilter(startPicker.Value.AddDays(-7), endPicker.Value.AddDays(-7), filterConditions, new List<string> { "Date" });
        //    chartAnalyzePresenter.GetGroupXY();
        //    List<ItemModel> predatas = this.datas;
        //    string[] prex = this.strings.Select(yy => DateTime.Parse(yy).ToString("MM-dd")).ToArray();
        //    int[] prey = this.ints;


        //    chart1.Series.Clear();
        //    chart1.ChartAreas.Clear();
        //    chart1.Legends.Clear();
        //    chart1.Titles.Clear();
        //    chart1.Annotations.Clear();

        //    ChartArea chartArea = new ChartArea("LineArea");
        //    chartArea.AxisX.Title = "AxisX.Title";
        //    chartArea.AxisY.Title = "AxisY.Title";
        //    chartArea.InnerPlotPosition = new ElementPosition(10, 5, 80, 80);
        //    chartArea.BackColor = Color.FromArgb(200, 200, 200);
        //    chartArea.AxisX.Interval = 1;
        //    chart1.ChartAreas.Add(chartArea);

        //    Legend legend = new Legend("Main Legeng");
        //    legend.Docking = Docking.Top;
        //    legend.Enabled = true;
        //    chart1.Legends.Add(legend);

        //    Title chartTitle = new Title();
        //    chartTitle.Text = "ChartTitle";
        //    chartTitle.Font = new Font("微軟正黑體", 20, FontStyle.Bold);
        //    chartTitle.ForeColor = Color.DarkBlue;
        //    chartTitle.Alignment = ContentAlignment.TopCenter;
        //    chart1.Titles.Add(chartTitle);

        //    Series nowSeries = new Series("NowSeries");
        //    nowSeries.ChartType = SeriesChartType.Line;
        //    nowSeries.ChartArea = "LineArea";
        //    nowSeries.Legend = "Main Legeng";
        //    nowSeries.BorderWidth = 2;
        //    nowSeries.BorderColor = Color.Blue;
        //    nowSeries.Points.DataBindXY(x, y);
        //    nowSeries.Label = "#VALY";
        //    chart1.Series.Add(nowSeries);

        //    Series preSeries = new Series("PreSeries");
        //    preSeries.ChartType = SeriesChartType.Line;
        //    preSeries.ChartArea = "LineArea";
        //    preSeries.Legend = "Main Legeng";
        //    preSeries.BorderWidth = 2;
        //    preSeries.BorderColor = Color.Red;
        //    preSeries.Points.DataBindXY(prex, prey);
        //    preSeries.Label = "#VALY";
        //    chart1.Series.Add(preSeries);
        //}
        //private void DrawPie()
        //{
        //    chartAnalyzePresenter.GetGroupXY();
        //    string[] x = this.strings;
        //    int[] y = this.ints;

        //    chart1.Series.Clear();
        //    chart1.ChartAreas.Clear();
        //    chart1.Legends.Clear();
        //    chart1.Titles.Clear();
        //    chart1.Annotations.Clear();

        //    ChartArea chartArea = new ChartArea("PieArea");
        //    chartArea.AxisX.Title = "AxisX.Title";
        //    chartArea.AxisY.Title = "AxisY.Title";
        //    chartArea.InnerPlotPosition = new ElementPosition(10, 10, 80, 80);
        //    chartArea.BackColor = Color.FromArgb(200, 200, 200);
        //    chart1.ChartAreas.Add(chartArea);

        //    Legend legend = new Legend("Main Legeng");
        //    legend.Docking = Docking.Top;
        //    legend.Enabled = true;
        //    chart1.Legends.Add(legend);

        //    Title chartTitle = new Title();
        //    chartTitle.Text = "ChartTitle";
        //    chartTitle.Font = new Font("微軟正黑體", 20, FontStyle.Bold);
        //    chartTitle.ForeColor = Color.DarkBlue;
        //    chartTitle.Alignment = ContentAlignment.TopCenter;
        //    chart1.Titles.Add(chartTitle);

        //    Series series = new Series("PieSeries");
        //    series.ChartType = SeriesChartType.Pie;
        //    series.ChartArea = "PieArea";
        //    series.Legend = "Main Legeng";
        //    series.Points.DataBindXY(x, y);
        //    series.Label = "#AXISLABEL #VALY(#PERCENT)";
        //    series["PieLabelStyle"] = "Outside";
        //    series["PieLineColor"] = "Black";
        //    chart1.Series.Add(series);
        //}
        private void button2_Click(object sender, EventArgs e)
        {
            this.Debounce(870, () =>
            {
                datas.Clear();
                GC.Collect();
                chartAnalyzePresenter.GetChart(startPicker.Value, endPicker.Value, filterConditions, groupCondition, chartType);
            });
        }
        private void UpdateChart()
        {
            flowLayoutPanel3.Controls.Clear();
            flowLayoutPanel3.Controls.Add(this.chart);
            Console.WriteLine($"chart: {this.chart}");
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == "" | comboBox1.SelectedItem == null)
                return;
            switch (comboBox1.SelectedItem.ToString())
            {
                case "Pie":
                    chartType = 17;
                    break;
                case "Line":
                    chartType = 3;
                    break;
                case "StackedColumn100":
                    chartType = 12;
                    break;
            }
        }
    }
}
