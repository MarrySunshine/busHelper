using BusHelper.Services;
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
using Windows.UI.Xaml.Controls.Maps;
using Windows.Devices.Geolocation;
using Windows.UI.Core;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace BusHelper
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private BusInfoService busInfoService = BusInfoService.Instance;

        //private MainPage rootPage = MainPage.current;
        private double lati;
        private double lgtd;
        private double to_lati;
        private double to_lgtd;
        private uint _desireAccuracyInMetersValue = 0;
        MapIcon pos_now;
        RandomAccessStreamReference mapIconStreamReference;
        public MainPage()
        {
            this.InitializeComponent();
            lati = 47.604;
            lgtd = -122.329;
            mapIconStreamReference = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/MapPin.png"));
            MapControl.Loaded += init;
            //myMap.Loaded += MyMap_Loaded;
            MapControl.MapTapped += MyMap_MapTapped;
            //button_Click();
            //MapControl.MapTapped += map_tapped;
        }
        private CancellationTokenSource _cts = null;
        async private void init(object sender, RoutedEventArgs e)
        {
            try
            {
                // Request permission to access location
                var accessStatus = await Geolocator.RequestAccessAsync();

                switch (accessStatus)
                {
                    case GeolocationAccessStatus.Allowed:

                        // Get cancellation token
                        _cts = new CancellationTokenSource();
                        CancellationToken token = _cts.Token;

                        this.NotifyUser("加载中...", NotifyType.StatusMessage);

                        // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0), DesiredAccuracy.Default is used.
                        Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = _desireAccuracyInMetersValue };

                        // Carry out the operation
                        Geoposition pos = await geolocator.GetGeopositionAsync().AsTask(token);

                        UpdateLocationData(pos);
                        this.NotifyUser("当前位置", NotifyType.StatusMessage);
                        //rootPage.NotifyUser(lati.ToString() + lgtd.ToString(), NotifyType.StatusMessage);
                        //myMap.Loaded += MyMap_Loaded;
                        MapControl.Center =
                new Geopoint(new BasicGeoposition()
                {
                    //Geopoint for Seattle 
                    //Latitude = 47.604,
                    //Longitude = -122.329

                    Latitude = lati,//23.11705530622489,
                    Longitude = lgtd//113.2759952545166
                });
                        MapControl.ZoomLevel = 15;
                        MapIcon mapIcon1 = new MapIcon();
                        pos_now = new MapIcon();
                        
                        mapIcon1.Location = MapControl.Center;
                        mapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
                        pos_now.NormalizedAnchorPoint = new Point(0.5, 1.0);
                        mapIcon1.Title = "当前位置";
                        pos_now.Title = "目标位置";
                        mapIcon1.Image = mapIconStreamReference;
                        pos_now.Image = mapIconStreamReference;
                        mapIcon1.ZIndex = 0;
                        MapControl.MapElements.Add(mapIcon1);
                        MapControl.MapElements.Add(pos_now);
                        pos_now.Visible = false;
                        break;

                    case GeolocationAccessStatus.Denied:
                        this.NotifyUser("Access to location is denied.", NotifyType.ErrorMessage);
                        //LocationDisabledMessage.Visibility = Visibility.Visible;
                        UpdateLocationData(null);
                        break;

                    case GeolocationAccessStatus.Unspecified:
                        this.NotifyUser("Unspecified error.", NotifyType.ErrorMessage);
                        UpdateLocationData(null);
                        break;
                }
            }
            catch (TaskCanceledException)
            {
                this.NotifyUser("Canceled.", NotifyType.StatusMessage);
            }
            catch (Exception ex)
            {
                this.NotifyUser(ex.ToString(), NotifyType.ErrorMessage);
            }
            finally
            {
                _cts = null;
            }
        }
        public enum NotifyType
        {
            StatusMessage,
            ErrorMessage
        };
        public void NotifyUser(string strMessage, NotifyType type)
        {
            switch (type)
            {
                case NotifyType.StatusMessage:
                    StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Green);
                    break;
                case NotifyType.ErrorMessage:
                    StatusBorder.Background = new SolidColorBrush(Windows.UI.Colors.Red);
                    break;
            }
            StatusBlock.Text = strMessage;

            // Collapse the StatusBlock if it has no text to conserve real estate.
            StatusBorder.Visibility = (StatusBlock.Text != String.Empty) ? Visibility.Visible : Visibility.Collapsed;
            if (StatusBlock.Text != String.Empty)
            {
                StatusBorder.Visibility = Visibility.Visible;
                StatusPanel.Visibility = Visibility.Visible;
            }
            else
            {
                StatusBorder.Visibility = Visibility.Collapsed;
                StatusPanel.Visibility = Visibility.Collapsed;
            }
        }
        private void UpdateLocationData(Geoposition position)
        {
            if (position == null)
            {
                lati = 47.604;
                lgtd = -122.329;
            }
            else
            {
                lati = position.Coordinate.Point.Position.Latitude;
                lgtd = position.Coordinate.Point.Position.Longitude;
                to_lati = lati;
                to_lgtd = lgtd;

            }
        }
        private void MyMap_Loaded(object sender, RoutedEventArgs e)
        {

            MapControl.Center =
                new Geopoint(new BasicGeoposition()
                {
                    //Geopoint for Seattle 
                    //Latitude = 47.604,
                    //Longitude = -122.329

                    Latitude = lati,//23.11705530622489,
                    Longitude = lgtd//113.2759952545166
                });
            MapControl.ZoomLevel = 15;
            //  init();
        }

        private void MyMap_MapTapped(Windows.UI.Xaml.Controls.Maps.MapControl sender, Windows.UI.Xaml.Controls.Maps.MapInputEventArgs args)
        {
            var tappedGeoPosition = args.Location.Position;
            text_to.Text = "目标位置";
            pos_now.Location = args.Location;
            
            pos_now.Visible = true;
            string status = "MapTapped at \nLatitude:" + tappedGeoPosition.Latitude + "\nLongitude: " + tappedGeoPosition.Longitude;
            to_lati = tappedGeoPosition.Latitude;
            to_lgtd = tappedGeoPosition.Longitude;
            this.NotifyUser(status, NotifyType.StatusMessage);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //rootPage = MainPage.Current;
        }

        private void TrafficFlowVisible_Checked(object sender, RoutedEventArgs e)
        {
            MapControl.TrafficFlowVisible = true;
        }

        private void trafficFlowVisibleCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MapControl.TrafficFlowVisible = false;
        }

        private void button_pos_Click(object sender, RoutedEventArgs e)
        {
            MapControl.Center =
                new Geopoint(new BasicGeoposition()
                {
                    //Geopoint for Seattle 
                    //Latitude = 47.604,
                    //Longitude = -122.329

                    Latitude = lati,//23.11705530622489,
                    Longitude = lgtd//113.2759952545166
                });
            MapControl.ZoomLevel = 15;
        }


        private async void button_Click(object sender, RoutedEventArgs e)
        {
            // Start at Microsoft in Redmond, Washington.
            BasicGeoposition startLocation = new BasicGeoposition() { Latitude = lati, Longitude = lgtd };

            // End at the city of Seattle, Washington.
            BasicGeoposition endLocation = new BasicGeoposition() { Latitude = to_lati, Longitude = to_lgtd };

            // Get the route between the points.
            MapRouteFinderResult routeResult =
                  await MapRouteFinder.GetDrivingRouteAsync(
                  new Geopoint(startLocation),
                  new Geopoint(endLocation),
                  MapRouteOptimization.Time,
                  MapRouteRestrictions.None);

            if (routeResult.Status == MapRouteFinderStatus.Success)
            {
                System.Text.StringBuilder routeInfo = new System.Text.StringBuilder();

                // Display summary info about the route.
                routeInfo.Append("Total estimated time (minutes) = ");
                routeInfo.Append(routeResult.Route.EstimatedDuration.TotalMinutes.ToString());
                routeInfo.Append("\nTotal length (kilometers) = ");
                routeInfo.Append((routeResult.Route.LengthInMeters / 1000).ToString());

                // Display the directions.
                routeInfo.Append("\n\nDIRECTIONS\n");

                foreach (MapRouteLeg leg in routeResult.Route.Legs)
                {
                    foreach (MapRouteManeuver maneuver in leg.Maneuvers)
                    {
                        routeInfo.AppendLine(maneuver.InstructionText);
                    }
                }

                // Load the text box.
                tbOutputText.Text = routeInfo.ToString();
            }
            else
            {
                tbOutputText.Text =
                      "A problem occurred: " + routeResult.Status.ToString();
            }
            ShowRouteOnMap();
        }
        private IList<MapRouteView> MapWithRoute { get; }
        private async void ShowRouteOnMap()
        {
            // Start at Microsoft in Redmond, Washington.
            BasicGeoposition startLocation = new BasicGeoposition() { Latitude = lati, Longitude = lgtd };

            // End at the city of Seattle, Washington.
            BasicGeoposition endLocation = new BasicGeoposition() { Latitude = to_lati, Longitude = to_lgtd };


            // Get the route between the points.
            MapRouteFinderResult routeResult =
                  await MapRouteFinder.GetDrivingRouteAsync(
                  new Geopoint(startLocation),
                  new Geopoint(endLocation),
                  MapRouteOptimization.Time,
                  MapRouteRestrictions.None);

            if (routeResult.Status == MapRouteFinderStatus.Success)
            {
                // Use the route to initialize a MapRouteView.
                MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);
                viewOfRoute.RouteColor = Colors.Yellow;
                viewOfRoute.OutlineColor = Colors.Black;

                // Add the new MapRouteView to the Routes collection
                // of the MapControl.
                MapControl.Routes.Add(viewOfRoute);

                // Fit the MapControl to the route.
                
                await MapControl.TrySetViewBoundsAsync(
                      routeResult.Route.BoundingBox,
                      null,
                      Windows.UI.Xaml.Controls.Maps.MapAnimationKind.None);
            }
        }


        
        /*
        private async void button_Click(object sender, RoutedEventArgs e)
        {
            var result = await busInfoService.getBusInfo("广州", "B4");
        }*/
    }
}
