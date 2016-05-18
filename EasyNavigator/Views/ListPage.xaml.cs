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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace EasyNavigator.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ListPage : Page
    {
        public ViewModels.ListPageViewModel viewModel = ViewModels.ListPageViewModel.Instance;
        public ListPage()
        {
            this.InitializeComponent();
            find("生物岛");
        }

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
            }
        }

        private async void textBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                await find((sender as TextBox).Text);
            }
        }
    }
}
