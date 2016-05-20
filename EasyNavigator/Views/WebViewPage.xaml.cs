using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace EasyNavigator.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class WebViewPage : Page
    {
        static private WebViewPage instance = null;
        static public WebViewPage Instance {
            get
            {
                return instance;
            }
        }
        public WebViewPage()
        {
            this.InitializeComponent();
            instance = this;
        }

        private string[] position = new string[2];
        //获取当前位置，用经纬度表示
        public async Task<string[]> getLocation()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            Geoposition pos = null;
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:

                    var _cts = new CancellationTokenSource();
                    var token = _cts.Token;

                    Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 0 };

                    pos = await geolocator.GetGeopositionAsync().AsTask(token);
                    break;
            }
            position[0] = pos.Coordinate.Longitude.ToString();
            position[1] = pos.Coordinate.Latitude.ToString();
            return position;
        }
        //设置当前坐标为获取的坐标
        public async Task setLocation()
        {
            await webView.InvokeScriptAsync("setLocation", await getLocation());
        }
        //查询到目标地点的导航路径
        public async void navigateToPosition(string to_lng, string to_lat)
        {
            try
            {
                await webView.InvokeScriptAsync("navigateToPosition", new List<string>() { position[0], position[1], to_lng, to_lat });
            }
            catch
            {
                return;
            }
        }
        //
        private async void webView_ScriptNotify(object sender, NotifyEventArgs e)
        {
            var result = e.Value.Split('|');
            switch (result[0])
            {
                case "success":
                    if (result[1] == "map init")
                    {
                        await setLocation();
                    }
                    break;
                case "error":
                    await new MessageDialog(result[1]).ShowAsync();
                    break;
            }
        }
    }
}
