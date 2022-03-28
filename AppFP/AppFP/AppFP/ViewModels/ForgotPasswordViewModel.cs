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
    public class ForgotPasswordViewModel : BaseViewModel
    {
        private readonly string _token = Preferences.Get("Token", "NULL");
        public bool isDebug = false;

        private string userEmail;
        public string UserEmail
        {
            get { return userEmail; }
            set
            {
                userEmail = value;
                OnPropertyChanged(nameof(UserEmail));
                ForgotPasswordCommand?.ChangeCanExecute();
            }
        }

        public Command ForgotPasswordCommand { get; set; }

        private readonly HttpClient _client;
        public ForgotPasswordViewModel()
        {
            _client = new HttpClient();
            
            ForgotPasswordCommand = new Command(async () => { await ForgotPassword(); }, CanExecuteForgotPassword);
        }

        public async Task ForgotPassword()
        {
            CustomerEmail email = new CustomerEmail
            {
                Email = userEmail
            };

            var data = new StringContent(
                   JsonConvert.SerializeObject(email, Newtonsoft.Json.Formatting.Indented),
                   Encoding.UTF8,
                   "application/json");

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var response = await _client.PostAsync(IP_API.APIserver + "/api/users/ReciveEmailResetPassword", data);

            var responsebody = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<Response>(responsebody);

            if (responseObject.IsSucess)
            {
                await App.Current.MainPage.DisplayAlert("Sucesso", responseObject.Message, "Ok");

                await App.Current.MainPage.Navigation.PushAsync(new LoginPage());
            }
            else
            {
                UserEmail = "";
                await App.Current.MainPage.DisplayAlert("Erro", responseObject.Message, "Ok");
            }

            }

        private bool CanExecuteForgotPassword()
        {
            if (isDebug)
            {
                return true;
            }

            if (string.IsNullOrEmpty(UserEmail))
            {
                return false;
            }

            return true;
        }
    }
}
