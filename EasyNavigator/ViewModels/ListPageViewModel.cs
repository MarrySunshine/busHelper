using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyNavigator.ViewModels
{
    public class ListPageViewModel
    {
        static private ListPageViewModel instance = null;
        static public ListPageViewModel Instance
        {
            get
            {
                return instance == null ? (instance = new ListPageViewModel()) : instance;
            }
        }

        public ObservableCollection<Models.AddressModel> source = new ObservableCollection<Models.AddressModel>();
    }
}
