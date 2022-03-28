using AppFP.Helpers;
using AppFP.Models;
using AppFP.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace AppFP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomMapPage : ContentPage
    {
        private readonly string _userId = Preferences.Get("Id", "NULL");
        private readonly string _token = Preferences.Get("Token", "NULL");

        CustomMap customMap;
        CustomPin userPin;
        CustomPin pinNEEI;
        CustomPin pinNEMEC;

        private readonly HttpClient _client;
        public CustomMapPage()
        {
            _client = new HttpClient();
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);

            double lat = 38.707162;
            double longi = -9.141640;

            Grid grid = new Grid();

            Button buttonQR = new Button
            {
                WidthRequest = 120,
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                BackgroundColor = Color.AntiqueWhite,
                TextColor = Color.Coral,
                Text = "Obter QR Code",
            };

            buttonQR.Clicked += GerarQRCode;

            Button buttonRealocar = new Button
            {
                WidthRequest = 120,
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                BackgroundColor = Color.AntiqueWhite,
                TextColor = Color.Coral,
                Text = "Realocar Mapa"
            };

            buttonRealocar.Clicked += RealocarMapa;

            customMap = new CustomMap
            {
                MapType = MapType.Street
            };

            grid.Children.Add(customMap);          
            grid.Children.Add(buttonQR);
            grid.Children.Add(buttonRealocar);
            Content = grid;

            customMap.CustomPins = new List<CustomPin>();

            //Fução é executada a cada 10 segundos

            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                Task.Run(async () =>
               {
                   //await GetUserLocation();
                   await InitialiazeAndUpdatePinsOnMap();
               });
                return true;
            });
        }

        private void RealocarMapa(object sender, EventArgs e)
        {
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(userPin.Position, Distance.FromKilometers(.2)));
        }

        private async void GerarQRCode(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushAsync(new ClienteQRCodePage(_userId));
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await InitialiazeAndUpdatePinsOnMap();
        }

        public async Task<Position> GetUserLocation()
        {
            var location = await Geolocation.GetLastKnownLocationAsync();

            if (location == null)
            {
                location = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Best,
                    Timeout = TimeSpan.FromSeconds(10)
                });
            };

            Position p = new Position(location.Latitude, location.Longitude);

            return p;
        }

        public async Task InitialiazeAndUpdatePinsOnMap()
        {
            if (customMap.Pins.Count != 0)
            {
                customMap.Pins.Clear();
            }

            var userPosition = GetUserLocation().Result;

            userPin = new CustomPin
            {
                Type = PinType.Place,
                //Position = new Position(38.707162, -9.139630),
                Position = new Position(userPosition.Latitude,userPosition.Longitude),
                Label = "Você está aqui",
                Name = "userPin"
            };

            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(userPin.Position, Distance.FromKilometers(0.2)));
            customMap.CustomPins.Add(userPin);
            customMap.Pins.Add(userPin);
            //customMap.MoveToRegion(MapSpan.FromCenterAndRadius(userPin.Position, Distance.FromKilometers(0.2)));

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await _client.GetAsync(IP_API.APIserver + "/api/customer/GetSellersPosition");

            var responsebody = await response.Content.ReadAsStringAsync();

            var sellersResponse = JsonConvert.DeserializeObject<PositionSellers>(responsebody);

            if (sellersResponse.IsSuccess)
            {
                //await App.Current.MainPage.DisplayAlert("Teste", "Existem vendedores", "OK");
                
                foreach (var item in sellersResponse.Sellers.ToList())
                {
                    switch (item.Curso)
                    {
                        case "NEEI":

                            CustomPin pinNEEI = new CustomPin
                            {
                                Type = PinType.Place,
                                Position = new Position(item.Latitude, item.Longitude),
                                Label = "NEEI",
                                Name = "NEEI",
                                Id = item.userId
                            };

                            customMap.CustomPins.Add(pinNEEI);
                            customMap.Pins.Add(pinNEEI);
                            break;

                        case "NEMEC":

                            CustomPin pinNEMEC = new CustomPin
                            {
                                Type = PinType.Place,
                                Position = new Position(item.Latitude, item.Longitude),
                                Label = "NEMEC",
                                Name = "NEMEC",
                                Id = item.userId
                            };

                            customMap.CustomPins.Add(pinNEMEC);
                            customMap.Pins.Add(pinNEMEC);
                            break;
                    }
                }

                //return sellersResponse.Sellers;
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Teste", "Não existem vendedores", "OK");
                //return null;
            }
        }

        protected override bool OnBackButtonPressed() //Desativar botao de voltar atras do android
        {
            return true;
        }

        private async void Logout(object sender, EventArgs e)
        {
            Preferences.Remove("Email");
            Preferences.Remove("Id");
            Preferences.Remove("Token");
            await App.Current.MainPage.Navigation.PushAsync(new MainPage());
        }
    }
}