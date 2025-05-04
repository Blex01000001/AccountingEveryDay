using AccountingEveryDay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingEveryDay.Contract
{
    internal class AccountContract
    {
        public interface IAccountView
        {
            void DataResponse(ItemModel model);
        }
        public interface IAccountPresenter
        {
            void UpdateDates(ItemModel model);
        }
    }

}
