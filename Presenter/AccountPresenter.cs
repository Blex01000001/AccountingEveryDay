using AccountingEveryDay.Interface;
using AccountingEveryDay.Models;
using CSV_Libary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AccountingEveryDay.Contract.AccountContract;
using static AccountingEveryDay.Contract.NoteContract;
using System.Configuration;

namespace AccountingEveryDay.Presenter
{
    internal class AccountPresenter : IAccountPresenter
    {
        IAccountView view = null;
        IDataRepository repository = null;
        string dataFilePath = ConfigurationManager.AppSettings["FoldorPath"];

        //string dataFilePath = "C:\\Users\\USERA\\Documents\\C#\\AccountingEveryDay_data\\";
        public AccountPresenter(IAccountView view)
        {
            this.view = view;
            repository = new DataRepository.DataRepository();
        }

        public void UpdateDates(ItemModel model)
        {
            repository.UpdateByDate(model);
            //CSV csv = new CSV();
            //csv.Write(dataFilePath + model.Date + "\\data.csv", model);
            this.view.DataResponse(model);
        }
    }
}
