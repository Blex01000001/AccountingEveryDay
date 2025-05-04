using AccountingEveryDay.Interface;
using AccountingEveryDay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AccountingEveryDay.Contract.NoteContract;
using System.IO;
using System.Windows.Forms;

namespace AccountingEveryDay.Presenter
{
    internal class NotePresenter : INotePresenter
    {
        INoteView view = null;
        IDataRepository repository = null;

        public NotePresenter(INoteView view)
        {
            this.view = view;
            repository = new DataRepository.DataRepository();
        }

        public void GetDataByTime(DateTime startTime, DateTime endTime)
        {
            List<ItemModel> datas = repository.GetDataByTime(startTime, endTime);
            this.view.DataResponse(datas);
        }
        public void UpdateDates(string editDate, List<ItemModel> datas)
        {
            List<ItemModel> newDatas = datas.Where(x => x.Date == editDate).ToList();
            string deletePath = @"C:\Users\USERA\Documents\C#\AccountingEveryDay_data\" + editDate + "\\data.csv";
            File.Delete(deletePath);
            repository.UpdateByDate(newDatas);
        }
        public void RemoveDataByRow(int removeRow, string deleteDate, List<ItemModel> datas)
        {
            datas.RemoveAt(removeRow);
            string deletePath = @"C:\Users\USERA\Documents\C#\AccountingEveryDay_data\" + deleteDate + "\\data.csv";
            File.Delete(deletePath);
            UpdateDates(deleteDate, datas);
            this.view.DataResponse(datas);
        }



        void INotePresenter.UpdateDates(string editDate, List<ItemModel> datas)
        {
            throw new NotImplementedException();
        }

        void INotePresenter.RemoveDataByRow(int removeRow, string deleteDate, List<ItemModel> datas)
        {
            throw new NotImplementedException();
        }
    }
}
