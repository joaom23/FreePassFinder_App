using AppFP.Helpers;
using AppFP.Models;
using AppFP.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppFP.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        private readonly string _fotoUser = Preferences.Get("Foto", "NULL");
        private readonly HttpClient _client;
        private readonly string _userId = Preferences.Get("Id", "NULL");
        private readonly string _token = Preferences.Get("Token", "NULL");

        private string info;
        public string Info
        {
            get { return info; }
            set
            {

                info = value;
                OnPropertyChanged(nameof(Info));
            }
        }

        private string information;
        public string Information
        {
            get { return information; }
            set
            {

                information = value;
                OnPropertyChanged(nameof(Information));
                AddInformationCommnad?.ChangeCanExecute();
            }
        }

        private ImageSource fotoUser;
        public ImageSource FotoUser
        {
            get { return fotoUser; }
            set
            {
                fotoUser = value;
                OnPropertyChanged(nameof(FotoUser));
            }
        }

        private ImageSource banner;
        public ImageSource Banner
        {
            get { return banner; }
            set
            {
                banner = value;
                OnPropertyChanged(nameof(Banner));
            }
        }
        public Command ListaVendidosPageCommand { get; }
        public Command LogoutCommand { get; }
        public Command VenderFreePassCommand { get; }
        public Command AddInformationCommnad { get; set; }
        public MenuViewModel()
        {
            Preferences.Set("doAgain", "true");
            VenderFreePassCommand = new Command(VenderFreePassPage);
            LogoutCommand = new Command(Logout);
            AddInformationCommnad = new Command(async () => { await AddInformation(); }, CanExecuteAddInformation);
            ListaVendidosPageCommand = new Command(ListaVendidosPage);
            FotoUser = ImageSource.FromUri(new Uri(IP_API.APIserver + "/api/nucleos/getnucleoimage/" + _fotoUser));
            _client = new HttpClient();
        }

        public async void VenderFreePassPage()
        {
            await App.Current.MainPage.Navigation.PushAsync(new VenderFreePassesPage());
        }

        public async void ListaVendidosPage()
        {
            await App.Current.MainPage.Navigation.PushAsync(new ListaVendidosPage());
        }

        public async Task GetSellerLocation()
        {
            var location = await Geolocation.GetLastKnownLocationAsync();

            if (location == null)
            {
                location = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Best,
                    Timeout = TimeSpan.FromSeconds(5)
                });
            }

            if (location != null)
            {
                await SendLocationAsyc(location.Latitude, location.Longitude);
            }
        }

        public async Task SendLocationAsyc(double latitude, double longitude)
        {
            var position = new SellerPosition
            {
                userId = _userId,
                Latitude = latitude,
                Longitude = longitude
            };

            var data = new StringContent(
                    JsonConvert.SerializeObject(position, Newtonsoft.Json.Formatting.Indented),
                    Encoding.UTF8,
                    "application/json");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await _client.PostAsync(IP_API.APIserver + "/api/nucleos/updatesellerposition", data);

            var responsebody = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<Response>(responsebody);

            if (responseObject.IsSucess && Preferences.Get("doAgain", "NULL") == "true")
            {
                //await App.Current.MainPage.DisplayAlert("Sucesso", "Localização Enviada", "Ok");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Erro", "Localização não Enviada", "Ok");
            }
        }

        public async Task AddInformation()
        {
            NucleoInformation email = new NucleoInformation
            {
                Information = information,
                NucleoId = _userId
            };

            var data = new StringContent(
                   JsonConvert.SerializeObject(email, Newtonsoft.Json.Formatting.Indented),
                   Encoding.UTF8,
                   "application/json");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await _client.PostAsync(IP_API.APIserver + "/api/nucleos/addinformation", data);

            var responsebody = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<Response>(responsebody);

            if (responseObject.IsSucess)
            {
                await App.Current.MainPage.DisplayAlert("Sucesso", responseObject.Message, "Ok");
                await GetInformation();
                Information = "";
                //await App.Current.MainPage.Navigation.PushAsync(new LoginPage());
            }
            else
            {
                Information = "";
                await App.Current.MainPage.DisplayAlert("Erro", responseObject.Message, "Ok");
            }

        }

        public async Task GetInformation()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await _client.GetAsync(IP_API.APIserver + "/api/nucleos/nucleoinformation/" + _userId);

            var responsebody = await response.Content.ReadAsStringAsync();

            var info = JsonConvert.DeserializeObject<Response>(responsebody);

            if (string.IsNullOrEmpty(info.Message) || info == null)
            {
                Info = "Ainda não defeniu nenhum informação sobre a sua localização";
            }
            else
            {
                Info = info.Message;
            }
        }

        private bool CanExecuteAddInformation()
        {
            if (string.IsNullOrEmpty(Information))
            {
                return false;
            }
            return true;
        }

        public async void Logout()
        {
            await ChangeState(_userId);
            Preferences.Set("doAgain", "false");
            Preferences.Remove("Id");
            Preferences.Remove("Token");
            await App.Current.MainPage.Navigation.PushAsync(new MainPage());
        }

        private async Task ChangeState(string _userId)
        {
            var changeSate = new ChangeOnlineState
            {
                userId = _userId,
                State = false
            };

            var data = new StringContent(
                   JsonConvert.SerializeObject(changeSate, Newtonsoft.Json.Formatting.Indented),
                   Encoding.UTF8,
                   "application/json");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await _client.PostAsync(IP_API.APIserver + "/api/nucleos/changestate", data);

            var responsebody = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<Response>(responsebody);

        }
    }
}
