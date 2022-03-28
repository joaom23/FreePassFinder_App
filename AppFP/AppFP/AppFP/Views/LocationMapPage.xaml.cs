using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace AppFP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationMapPage : ContentPage
    {
        CancellationTokenSource cts;

        private readonly Pin user = new Pin
        {
            Label = "WTF",
                       
        };
        private double move;

        private double GetMove()
        {
            return move;
        }

        private void SetMove(double value)
        {
            move = value;
        }

        public LocationMapPage()
        {
            double lat = 38.707162;
            double longi = -9.141640;
      
            InitializeComponent();
            SetMove(0.00001);
            map.Pins.Add(user);
            Device.StartTimer(TimeSpan.FromSeconds(1), () => 
            {
                
                user.Position = new Position(lat, longi);
                map.MoveToRegion(MapSpan.FromCenterAndRadius(user.Position, Distance.FromKilometers(.2)));
                //lat += 0.001;
                longi += 0.0001;
                return true;
            });
        }

        public async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if(location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest 
                    {
                        DesiredAccuracy = GeolocationAccuracy.Best,
                        Timeout = TimeSpan.FromSeconds(10)
                    });
                }

                if(location == null)
                {
                    LabelLocation.Text = "No GPS";
                }
                else
                {   

                   //Position p = new Position(location.Latitude, location.Longitude);
                    Position p = new Position(user.Position.Latitude, user.Position.Longitude);
                    MapSpan mapSpan = MapSpan.FromCenterAndRadius(p, Distance.FromKilometers(.444));
                    map.MoveToRegion(mapSpan);
                    LabelLocation.Text = $"{location.Latitude} {location.Longitude}";
                }

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            base.OnDisappearing();
        }
    }
}