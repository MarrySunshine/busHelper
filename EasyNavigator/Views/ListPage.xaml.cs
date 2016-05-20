using EasyNavigator.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EasyNavigator.Models;
using EasyNavigator.Libs;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace EasyNavigator.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ListPage : Page
    {
        public ListPage()
        {
            this.InitializeComponent();
            loadRecord();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key_word"></param>
        /// <returns></returns>
        private async Task find(string key_word)
        {
            listView.ItemsSource = await SearchService.Instance.getResultByKeyWord(key_word);
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListView).SelectedIndex != -1)
            {
                var list_view_item = sender as ListView;
                var target_item = list_view_item.SelectedItem as Models.AddressModel;
                WebViewPage.Instance.navigateToPosition(target_item.Lng, target_item.Lat);
                saveRecord(target_item);
                MainPage.Instance.ShouldChangeAdaptiveUIState();
                MainPage.Instance.UpdateTile(target_item);
            }
        }

        private async void textBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                await find((sender as TextBox).Text);
            }
        }

        private async void saveRecord(AddressModel address)
        {
            using (var db = new DatabaseContext())
            {
                db.AddressItems.Add(address.ToSchema());
                try
                {
                    await db.SaveChangesAsync();
                }
                catch
                {
                    return;
                }
            }
        }

        private void loadRecord()
        {
            using (var db = new DatabaseContext())
            {
                var address_list = new List<AddressModel>();
                foreach (var item in db.AddressItems.AsEnumerable())
                {
                    address_list.Add(new AddressModel(item));
                }
                this.listView.ItemsSource = address_list;
            }
        }
    }
}
