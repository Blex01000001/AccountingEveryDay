using AccountingEveryDay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountingEveryDay.Contract
{
    internal class NoteContract
    {
        public NoteContract() { }
        public interface INoteView
        {
            void DataResponse(List<ItemModel> datas);
        }
        public interface INotePresenter
        {
            void GetDataByTime(DateTime startTime, DateTime endTime);
            void UpdateDates(string editDate, List<ItemModel> datas);
            void RemoveDataByRow(int removeRow, string deleteDate, List<ItemModel> datas);
        }
    }
}
