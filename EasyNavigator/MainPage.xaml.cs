using EasyNavigator.Services;
using EasyNavigator.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Data.Xml.Dom;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace EasyNavigator
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        static private MainPage instance;
        static public MainPage Instance { get { return instance; } }
        public MainPage()
        {
            this.InitializeComponent();
            listFrame.Navigate(typeof(Views.ListPage));
            webViewFrame.Navigate(typeof(Views.WebViewPage));
            instance = this;

            AdaptiveUI.CurrentStateChanged += (sender, e) =>
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = e.NewState.Name == "OnlyRight" ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            };

            SystemNavigationManager.GetForCurrentView().BackRequested += (sender, e) =>
            {
                VisualStateManager.GoToState(this, "OnlyLeft", true);
            };
        }
        public void ShouldChangeAdaptiveUIState()
        {
            if (AdaptiveUI.CurrentState.Name == "OnlyLeft")
            {
                VisualStateManager.GoToState(this, "OnlyRight", true);
            }
        }
        public void UpdateTile(Models.AddressModel address)
        {
            var doc = new XmlDocument();
            doc.LoadXml(File.ReadAllText("Views/TileTemplate.xml"));

            var list = doc.GetElementsByTagName("text");
            for (var i = 0; i < list.Count(); i += 4)
            {
                list[i + 2].InnerText = address.Name;
                list[i + 3].InnerText = address.Address;
            }

            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.Update(new TileNotification(doc));
        }
        private void updateXmlNodeListContent(XmlNodeList list, string content)
        {
            list.All((node) =>
            {
                node.InnerText = content;
                return true;
            });
        }
    }
}
