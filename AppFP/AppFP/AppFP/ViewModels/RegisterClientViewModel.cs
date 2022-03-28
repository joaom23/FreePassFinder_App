using AppFP;
using AppFP.Helpers;
using AppFP.Models;
using AppFP.Views;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace AppFP.ViewModels
{
   public class RegisterClientViewModel : BaseViewModel
    {
        private static bool isDebug = false;

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
        
                email = value;
                OnPropertyChanged(nameof(Email));
                RegisterCommand?.ChangeCanExecute();
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {

                password = value;
                OnPropertyChanged(nameof(Password));
                RegisterCommand?.ChangeCanExecute();
            }
        }

        private string confirmpassword;
        public string ConfirmPassword
        {
            get { return confirmpassword; }
            set
            {

                confirmpassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
                RegisterCommand?.ChangeCanExecute();
            }
        }

        public Command RegisterCommand { get; }
        private readonly HttpClient _client;

        public RegisterClientViewModel()
        {
            _client = new HttpClient();
            RegisterCommand = new Command(async () => { await Regist(); }, CanExecuteLogin);

        }

        public async Task Regist()
        {
            RegisterClient client = new RegisterClient();

            if (isDebug)
            {
                
                client.Email = "teste2@gmail.com";
                client.Password = "teste2.123";
                client.ConfirmPassword = "teste2.123";
            }
            else
            {
                client.Email = Email;
                client.Password = Password;
                client.ConfirmPassword = ConfirmPassword;
             }
            

            var data = new StringContent(
                    JsonConvert.SerializeObject(client, Newtonsoft.Json.Formatting.Indented),
                    Encoding.UTF8,
                    "application/json");

            var response = await _client.PostAsync(IP_API.APIserver + "/api/users/registerclient", data);

            var responsebody = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<Response>(responsebody);    

            if (responseObject.IsSucess)
            {
                await App.Current.MainPage.DisplayAlert("Sucesso", responseObject.Message, "Ok");

                await App.Current.MainPage.Navigation.PushAsync(new MainPage());

            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Erro", responseObject.Message, "Ok");
                Password = "";
                ConfirmPassword = "";
            }
        }

        private bool CanExecuteLogin()
        {
            if (isDebug)
            {
                return true;
            }

            if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(ConfirmPassword))
            {
                return false;
            }

            return true;
        }
    }
}
