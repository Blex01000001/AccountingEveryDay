using AccountingEveryDay.Attributies;
using AccountingEveryDay.Models;
using AccountingEveryDay.Presenter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AccountingEveryDay.Contract.LedgerAnalyzeContract;

namespace AccountingEveryDay.Forms
{
    public partial class 帳本分析 : Form, ILedgerAnalyzeView
    {
        ILedgerAnalyzePresenter ledgerAnalyzePresenter = null;

        List<ItemModel> datas = new List<ItemModel>();
        bool status = false;
        Dictionary<string, List<string>> filterConditions= new Dictionary<string, List<string>>();

        List<string> groupCondition = new List<string>();



        public 帳本分析()
        {
            InitializeComponent();
            ledgerAnalyzePresenter = new LedgerAnalyzePresenter(this);

            //itemFilterConditions = new List<string>();
            //typeFilterConditions = new List<string>();
            //paymentTypeFilterConditions = new List<string>();
            //targetFilterConditions = new List<string>();

            //filterConditions.Add("Type", typeFilterConditions);
            //groupCondition = new List<string>();
            PropertyInfo[] props = typeof(DataModel).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetCustomAttribute(typeof(HideAttribute)) != null)
                    continue;
                if (prop.GetCustomAttribute(typeof(WhereAttribute)) != null)
                    continue;
                comboBox1.Items.Add(prop.Name);
            }



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

            //Q1: 想找出這個月花在午餐和晚餐上各分別多少錢?
            //Q2: 想找出自己和家人這個月在交通上各分別花多少錢?
            //Q3: 想找出這個月各分別花費在現金 / 信用卡 / 電子支付下各分別多少錢 ?
            //Q4: 想找出所有和朋友一起的娛樂開銷個分別是多少錢 ?
            //Q5: 想找出這個月自己花費在飲食 / 娛樂上各分別是多少錢 ?
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
        void ILedgerAnalyzeView.DataResponse(List<ItemModel> datas)
        {
            this.datas = datas;
            ConnectGridViewDataSource();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Debounce(870, () =>
            {
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                datas.Clear();
                GC.Collect();
                Console.WriteLine($"button1_Click comboBox.SelectedText: {groupCondition}");

                ledgerAnalyzePresenter.GetDataByFilter(startPicker.Value, endPicker.Value, filterConditions, groupCondition);
            });
            //foreach (var item in itemFilterConditions)
            //{
            //    Console.Write($"itemFilterConditions: {item}");
            //}
            //Console.Write("\n");
        }

        private void ConnectGridViewDataSource()
        {
            dataGridView1.DataSource = datas;

            //利用attribute增加或隱藏欄位
            foreach (PropertyInfo properity in typeof(ItemModel).GetProperties())
            {
                if (properity.GetCustomAttribute(typeof(HideAttribute)) != null)
                {
                    this.dataGridView1.Columns[properity.Name].Visible = false;//隱藏路徑欄位

                }
                if (properity.GetCustomAttribute(typeof(PictureAttribute)) != null)
                {
                    this.dataGridView1.Columns[properity.Name].Visible = false;//隱藏路徑欄位

                    AddImgColumn(properity.Name);
                }
                if (properity.GetCustomAttribute(typeof(ComboBoxAttribute)) != null)
                {
                    this.dataGridView1.Columns[properity.Name].Visible = false;//隱藏路徑欄位
                    //add column

                    var comboBoxColumn = new DataGridViewComboBoxColumn
                    {
                        Name = "_" + properity.Name,
                        HeaderText = properity.Name,
                    };
                    var cba = (ComboBoxAttribute)properity.GetCustomAttribute(typeof(ComboBoxAttribute));
                    if (cba.DataSource != null)
                        comboBoxColumn.DataSource = cba.DataSource;

                    int columnIndex = dataGridView1.Columns[properity.Name].Index;
                    dataGridView1.Columns.Insert(columnIndex, comboBoxColumn);
                }
            }
            AddImgColumn("Delete");

            //AddImgColumn("Img1");
            //AddImgColumn("Img2");
            //AddComboBoxCellColumn(2, "Type", DataModel.Type);
            //AddComboBoxCellColumn(4, "Item");
            //AddComboBoxCellColumn("Paytype", DataModel.Paytype);

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                PropertyInfo[] properities = typeof(ItemModel).GetProperties();
                var picProps = properities.Where(x => x.GetCustomAttribute(typeof(PictureAttribute)) != null).ToList();
                foreach (var prop in picProps)
                {
                    string ImgPath1 = (string)dataGridView1.Rows[i].Cells[prop.Name].Value;
                    dataGridView1.Rows[i].Cells[$"_{prop.Name}"].Value = Image.FromFile(ImgPath1);
                }
                dataGridView1.Rows[i].Cells["_Delete"].Value = Image.FromFile(@"C:\Users\USERA\Documents\C#\AccountingEveryDay\TRUSH.jpg");

                //將ComboBoxCell的值用反射填進去
                var comProps = properities.Where(x => x.GetCustomAttribute(typeof(ComboBoxAttribute)) != null).ToList();
                foreach (var prop in comProps)
                {
                    if (prop.Name == "Type")
                    {
                        dataGridView1.Rows[i].Cells["_" + prop.Name].Value = dataGridView1.Rows[i].Cells[prop.Name].Value;
                    }
                    else if (prop.Name == "Item")
                    {
                        DataGridViewComboBoxCell comboBoxCell = (DataGridViewComboBoxCell)dataGridView1.Rows[i].Cells["_" + prop.Name];
                        comboBoxCell.DataSource = DataModel.ItemTypes[(string)dataGridView1.Rows[i].Cells["Type"].Value];
                        dataGridView1.Rows[i].Cells["_" + prop.Name].Value = dataGridView1.Rows[i].Cells[prop.Name].Value;
                    }
                }
            }
        }
        private void AddImgColumn(string columnName)
        {
            var img1Column = new DataGridViewImageColumn
            {
                Name = "_" + columnName,
                HeaderText = columnName,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            dataGridView1.Columns.Add(img1Column);
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            //ComboBox comboBox = (ComboBox)sender;
            //groupCondition = comboBox.SelectedItem.ToString();
            //Console.WriteLine($"comboBox.SelectedText: {comboBox.SelectedItem}");
            //Console.WriteLine($"groupCondition: {groupCondition}");
        }
    }
}
