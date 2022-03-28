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
    class VenderFreePassViewmodel : BaseViewModel
    {
        private readonly string _curso = Preferences.Get("Curso", "NULL");
        private readonly string _vendedorId = Preferences.Get("Id", "NULL");
        private readonly string _token = Preferences.Get("Token", "NULL");

        public bool qrCodeConfirmed = false;
        public string _userId;

        private string isVisible = "false";
        public string IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }

        }     

        private string iconName = "notconfirm_icon.png";
        public string IconName
        {
            get { return iconName; }
            set
            { 
                iconName = value;
                OnPropertyChanged(nameof(IconName));
            }

        }


        public List<FreePass> freePasses;
        public List<FreePass> FreePasses
        {
            get { return freePasses; }
        }

        FreePass selectedFreePass;
        public FreePass SelectedFreePass
        {
            get { return selectedFreePass; }
            set
            {
                if(SelectedFreePass != value)
                {
                    selectedFreePass = value;
                    OnPropertyChanged(nameof(SelectedFreePass));
                    VenderFreePassCommand?.ChangeCanExecute();
                }
            }
        }
        private int? numero;
        public int? Numero
        {
            get { return numero; }
            set
            {

                numero = value;
                OnPropertyChanged(nameof(Numero));
                VenderFreePassCommand?.ChangeCanExecute();
            }
        }

        private string num;
        public string Num
        {
            get { return num; }
            set
            {

                num = value;
                OnPropertyChanged(nameof(Num));
                VenderFreePassCommand?.ChangeCanExecute();
            }
        }

        private readonly HttpClient _client;
        public Command ShowNumberFreePassCommand { get; }
        public Command VenderFreePassCommand { get; }
        public Command ScanQRCodePageCommand { get; }

        public VenderFreePassViewmodel()
        {
            freePasses = new List<FreePass>()
            {
                new FreePass
            {
                Disco = "BClub",
                CourseName = "",
                Numero = 0              
            },

            new FreePass
            {
                Disco = "Duplex",
                CourseName = "",
                Numero = 0
            }
        };

            _client = new HttpClient();
            //LoginCommand = new Command(Login,CanExecuteLogin);
            ShowNumberFreePassCommand = new Command(async () => { await GetNumberFreePass(); });
            VenderFreePassCommand = new Command(async () => { await VenderFreePass(); }, CanExecuteVenderFreePass);
            ScanQRCodePageCommand = new Command(ScanQRCodePage);

        }

        private bool CanExecuteVenderFreePass()
        {
            if (!qrCodeConfirmed || Num == null || SelectedFreePass == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ScanQRCodePage()
        {
            IsVisible = "true";
        }
        private async Task VenderFreePass()
        {
            if(Numero <= 0)
            {
                await App.Current.MainPage.DisplayAlert("Erro", "Insira um número válido", "OK");
                Numero = null;
                return;
            }

            var vendaFreePass = new FreePass
            {
                CourseName = _curso,
                Disco = SelectedFreePass.Disco,
                Numero = (int)Numero,
                userId = _userId,
                vendedorId = _vendedorId
                
            };

            SelectedFreePass = null;
            Numero = null;
            qrCodeConfirmed = false;
            IconName = "notconfirm_icon.png";

            var data = new StringContent(
                    JsonConvert.SerializeObject(vendaFreePass, Newtonsoft.Json.Formatting.Indented),
                    Encoding.UTF8,
                    "application/json");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await _client.PostAsync(IP_API.APIserver + "/api/nucleos/venderfreepass", data);

            var responsebody = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<Response>(responsebody);

            if (responseObject.IsSucess)
            {
                await App.Current.MainPage.DisplayAlert("Informação", responseObject.Message, "OK");
                await GetNumberFreePass();

            }
        }

        public async Task GetNumberFreePass()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await _client.GetAsync(IP_API.APIserver + "/api/nucleos/TotalFreePasses/" + _curso);
            //var response = await _client.GetAsync(IP_API.APIserver + "/api/nucleos/TotalFreePasses/NEEI");

            var responsebody = await response.Content.ReadAsStringAsync();

            var num = JsonConvert.DeserializeObject<TotalFreePass>(responsebody);

            Num = "BClub: " + num.BCblub + " Duplex: " + num.Duplex;
        }

        public async Task<bool> CheckValidUser(string id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await _client.GetAsync(IP_API.APIserver + "/api/users/CheckValidUser/" + id);

            var responsebody = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<Response>(responsebody);

            if (responseObject.IsSucess)
            {
                qrCodeConfirmed = true;
                _userId = id;
                IsVisible = "false";
                IconName = "confirm_icon.png";

                return true;               
            }
            

            return false;
        }
    }
}
