using AccountingEveryDay.Attributies;
using AccountingEveryDay.Components;
using AccountingEveryDay.Contract;
using AccountingEveryDay.Models;
using AccountingEveryDay.Presenter;
using CSV_Libary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AccountingEveryDay.Contract.NoteContract;

namespace AccountingEveryDay.Forms
{
    public partial class 記帳本 : Form , INoteView
    {
        INotePresenter notePresenter = null;
        List<ItemModel> datas = new List<ItemModel>();
        public 記帳本()
        {
            InitializeComponent();
            notePresenter = new NotePresenter(this);
        }
        string remark = "";
        private void button1_Click(object sender, EventArgs e) //Controller
        {
            this.Debounce(870, () =>
            {
                dataGridView1.DataSource = null;
                dataGridView1.Columns.Clear();
                datas.Clear();
                GC.Collect();
                notePresenter.GetDataByTime(startPicker.Value, endPicker.Value);
            });
        }
        void INoteView.DataResponse(List<ItemModel> datas)
        {
            this.datas = datas;
            ConnectGridViewDataSource();
        }
        private void AddImgColumn(string columnName)
        {
            var img1Column = new DataGridViewImageColumn
            {
                Name = "_"+columnName,
                HeaderText = columnName,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            };
            dataGridView1.Columns.Add(img1Column);
        }
      
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (columnName == "_Delete")//column Delete
            {
                DeleteRow(e.RowIndex);
                return;
            }
            if(dataGridView1.Columns[e.ColumnIndex].GetType() ==  typeof(DataGridViewImageColumn))
            {
                int columnIndex = e.ColumnIndex - 2;
                int rowIndex = e.RowIndex;

                Console.WriteLine(dataGridView1.Rows[rowIndex].Cells[columnIndex].Value);

                ImgDialog imgDialog = new ImgDialog((string)dataGridView1.Rows[rowIndex].Cells[columnIndex - 2].Value);
                imgDialog.ShowDialog();
                imgDialog.Dispose();
            }
        }

        private void DeleteRow(int DeleteRow)
        {
            string deleteDate = (string)dataGridView1.Rows[DeleteRow].Cells[0].Value;

            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            GC.Collect();
            File.Delete(datas[DeleteRow].Img1Path);
            File.Delete(datas[DeleteRow].Img2Path);
            File.Delete(datas[DeleteRow].Img1MinPath);
            File.Delete(datas[DeleteRow].Img2MinPath);

            notePresenter.RemoveDataByRow(DeleteRow, deleteDate, datas);
        }
       
        private void ConnectGridViewDataSource()
        {
            dataGridView1.DataSource = datas;

            //利用attribute增加或隱藏欄位
            foreach (PropertyInfo properity in typeof(ItemModel).GetProperties())
            {
                if(properity.GetCustomAttribute(typeof(HideAttribute)) != null)
                {
                    this.dataGridView1.Columns[properity.Name].Visible = false;//隱藏路徑欄位

                }
                if(properity.GetCustomAttribute(typeof(PictureAttribute)) != null)
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
                        Name = "_"+properity.Name,
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


            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                PropertyInfo[] properities = typeof(ItemModel).GetProperties();
                var picProps = properities.Where(x => x.GetCustomAttribute(typeof(PictureAttribute)) != null).ToList();
                foreach(var prop in picProps)
                {
                    string ImgPath1 = (string)dataGridView1.Rows[i].Cells[prop.Name].Value;
                    dataGridView1.Rows[i].Cells[$"_{prop.Name}"].Value = Image.FromFile(ImgPath1);
                }
                dataGridView1.Rows[i].Cells["_Delete"].Value = Image.FromFile(@"C:\Users\USERA\Documents\C#\AccountingEveryDay\TRUSH.jpg");

                //將ComboBoxCell的值用反射填進去
                var comProps = properities.Where(x => x.GetCustomAttribute(typeof(ComboBoxAttribute)) != null).ToList();
                foreach (var prop in comProps)
                {
                    if(prop.Name == "Type")
                    {
                        dataGridView1.Rows[i].Cells["_" + prop.Name].Value = dataGridView1.Rows[i].Cells[prop.Name].Value;
                    }else if (prop.Name == "Item")
                    {
                        DataGridViewComboBoxCell comboBoxCell = (DataGridViewComboBoxCell)dataGridView1.Rows[i].Cells["_" + prop.Name];
                        comboBoxCell.DataSource = DataModel.ItemTypes[(string)dataGridView1.Rows[i].Cells["Type"].Value];
                        dataGridView1.Rows[i].Cells["_" + prop.Name].Value = dataGridView1.Rows[i].Cells[prop.Name].Value;
                    }
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int columnIndex = e.ColumnIndex;
            string columnName = dataGridView1.Columns[columnIndex].Name;
            string editDate = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
            string selectedValue = dataGridView1.Rows[rowIndex].Cells[columnIndex].Value == null ? "" : dataGridView1.Rows[rowIndex].Cells[columnIndex].Value.ToString();
            switch (columnName)
            {
                case "_Type":
                    //((DataGridViewComboBoxCell)dataGridView1.Rows[rowIndex].Cells[columnName]).DataSource = DataModel.Type;
                    ((DataGridViewComboBoxCell)dataGridView1.Rows[rowIndex].Cells["_Item"]).DataSource = DataModel.ItemTypes[selectedValue];
                    ((DataGridViewComboBoxCell)dataGridView1.Rows[rowIndex].Cells["_Item"]).Value = DataModel.ItemTypes[selectedValue][0];
                    datas[rowIndex].Type = selectedValue;
                    datas[rowIndex].Item = DataModel.ItemTypes[selectedValue][0];
                    break;
                case "_Item":
                    datas[rowIndex].Item = selectedValue;
                    break;
                case "Amount":
                    if (int.TryParse(selectedValue, out int amount))
                    {
                        datas[rowIndex].Amount = amount;
                    }
                    break;
                default:
                    PropertyInfo property = typeof(ItemModel).GetProperties().First(x => x.Name == columnName);
                    string content = dataGridView1.Rows[rowIndex].Cells[columnName].Value == null ? "" : dataGridView1.Rows[rowIndex].Cells[columnName].Value.ToString();
                    property.SetValue(datas[rowIndex], content);
                    break;
            }
            notePresenter.UpdateDates(editDate, datas);
            //UpdateCSV(editDate);
        }

    }
}
