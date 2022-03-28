using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Widget;
using AppFP.Droid;
using AppFP.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using AppFP;
using System.Net.Http;
using Newtonsoft.Json;
using AppFP.Models;
using Xamarin.Essentials;
using System.Net.Http.Headers;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]

namespace AppFP.Droid
{
    
    public class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter
    {
        private readonly string _token = Preferences.Get("Token", "NULL");
        List<CustomPin> customPins;

        private readonly HttpClient _client;
        private readonly HttpClientHandler _handler;

        public CustomMapRenderer(Context context) : base(context)
        {
            _handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
        };
            _client = new HttpClient(_handler);
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Xamarin.Forms.Maps.Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                NativeMap.InfoWindowClick -= OnInfoWindowClick;
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                customPins = formsMap.CustomPins;
            }
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);
            NativeMap.InfoWindowClick += OnInfoWindowClick;
            NativeMap.SetInfoWindowAdapter(this);
        }

        protected override MarkerOptions CreateMarker(Pin pin)
        {
            CustomPin p = pin as CustomPin;
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pin.Label);
            marker.SetSnippet(pin.Address);
            if (p.Name == "userPin")
            {
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.userPin));
            }
            else
            {
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin));
            }
            
            return marker;
        }

        async void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            var customPin = GetCustomPin(e.Marker);
            if (customPin == null)
            {
                throw new Exception("Custom pin not found");
            }

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await _client.GetAsync(IP_API.APIserver + "/api/nucleos/TotalFreePasses/" + customPin.Name); //Mudar dinamicamente o id do nucleo

            var responsebody = await response.Content.ReadAsStringAsync();

            var num = JsonConvert.DeserializeObject<TotalFreePass>(responsebody);

            var sellerInformation = await _client.GetAsync(IP_API.APIserver + "/api/nucleos/NucleoInformation/" + customPin.Id);

            var sellerInformationBody = await sellerInformation.Content.ReadAsStringAsync();

            var information = JsonConvert.DeserializeObject<Response>(sellerInformationBody);

            if (String.IsNullOrEmpty(information.Message))
            {
                await App.Current.MainPage.DisplayAlert("Free Passes do vendedor", $"Bclub: {num.BCblub} \nDuplex: {num.Duplex}", "OK");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Informação do Vendedor", $"{information.Message}\n\nBclub: {num.BCblub} freepasses \nDuplex: {num.Duplex} freepasses", "OK");
            }

        }

        public Android.Views.View GetInfoContents(Marker marker)
        {
            var inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as Android.Views.LayoutInflater;
            if (inflater != null)
            {
                Android.Views.View view;

                var customPin = GetCustomPin(marker);
                if (customPin == null)
                {
                    throw new Exception("Custom pin not found");
                }

                if (customPin.Name.Equals("NEEI"))
                {
                    view = inflater.Inflate(Resource.Layout.XamarinMapInfoNEEIWindow, null);
                }
                else if(customPin.Name.Equals("NEMEC"))
                {
                    view = inflater.Inflate(Resource.Layout.XamarinMapInfoNEMECWindow, null);
                }
                else if (customPin.Name.Equals("userPin"))
                {
                    view = inflater.Inflate(Resource.Layout.XamarinMapInfoUserWindow, null);
                }
                else
                {
                    view = inflater.Inflate(Resource.Layout.MapInfoWindow, null);
                }

                var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
                var infoSubtitle = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);

                if (infoTitle != null)
                {
                    infoTitle.Text = marker.Title;
                }
                if (infoSubtitle != null)
                {
                    infoSubtitle.Text = marker.Snippet;
                }

                return view;
            }
            return null;
        }

        public Android.Views.View GetInfoWindow(Marker marker)
        {
            return null;
        }

        CustomPin GetCustomPin(Marker annotation)
        {
            var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
            foreach (var pin in customPins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }
    }
}
