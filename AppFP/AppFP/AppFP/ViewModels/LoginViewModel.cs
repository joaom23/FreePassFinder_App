using AppFP;
using AppFP.Helpers;
using AppFP.Models;
using AppFP.Views;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace AppFP.ViewModels
{
   public class LoginViewModel : BaseViewModel
    {
        private static bool isDebug = false;

        private string username;
        public string Username
        {
            get { return username; }
            set
            {
        
                username = value;
                OnPropertyChanged(nameof(Username));
                LoginCommand?.ChangeCanExecute();
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
                LoginCommand?.ChangeCanExecute();
            }
        }
        public Command ForgotPasswordPageCommand { get; }
        public Command LoginCommand { get; }
        private readonly HttpClient _client;
        public LoginViewModel()
        {
            _client = new HttpClient();
            LoginCommand = new Command(async () => { await Login(); }, CanExecuteLogin);
            ForgotPasswordPageCommand = new Command(ForgotPasswordPage);
        }

        public async Task Login()
        {
            UserLogin user = new UserLogin();

            if (isDebug)
            {

                //user.Email = "neei4@gmail.com";
                //user.Password = "neei.123";

                //user.Email = "nemec1@gmail.com";
                //user.Password = "nemec.123";

                user.Email = "teste4@gmail.com";
                user.Password = "teste.123";
            }
            else
            {
                user.Email = Username;
                user.Password = Password;
             }
            

            var data = new StringContent(
                    JsonConvert.SerializeObject(user, Newtonsoft.Json.Formatting.Indented),
                    Encoding.UTF8,
                    "application/json");

            var response = await _client.PostAsync(IP_API.APIserver + "/api/users/login", data);

            var responsebody = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<Response>(responsebody);    

            if (responseObject.IsSucess)
            {
                var token = responseObject.Message;

                var readToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

                var userId = readToken.Claims.FirstOrDefault(x => x.Type == "Id").Value;
                var userEmail = readToken.Claims.FirstOrDefault(x => x.Type == "Email").Value;
                var userRole = readToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;

                Preferences.Set("Id", userId);
                Preferences.Set("Token", responseObject.Message);
                Preferences.Set("Email", userEmail);

                if (userRole == "Nucleo")
                {
                    var curso = readToken.Claims.FirstOrDefault(x => x.Type == "Curso").Value;
                    Preferences.Set("Foto", curso);
                    Preferences.Set("Curso", curso);

                    //Mudar estado para online
                    await ChangeState(userId);

                    //Se for um nucleo vai para a pagina inicial
                    await App.Current.MainPage.Navigation.PushAsync(new Primeira());
                    return;
                }

                //Se for cliente vai para a pagina do mapa
                await App.Current.MainPage.Navigation.PushAsync(new CustomMapPage());
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Erro", responseObject.Message, "Ok");
                Password = "";
            }
        }

        private void ForgotPasswordPage()
        {
              App.Current.MainPage.Navigation.PushAsync(new ForgotPasswordPage());
        }

        private async Task ChangeState(string _userId)
        {
            var changeSate = new ChangeOnlineState
            {
                userId = _userId,
                State = true
            };

            var data = new StringContent(
                   JsonConvert.SerializeObject(changeSate, Newtonsoft.Json.Formatting.Indented),
                   Encoding.UTF8,
                   "application/json");

            var response = await _client.PostAsync(IP_API.APIserver + "/api/nucleos/changestate", data);

            var responsebody = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<Response>(responsebody);

            }

        private bool CanExecuteLogin()
        {
            if (isDebug)
            {
                return true;
            }

            if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Username))
            {
                return false;
            }

            return true;
        }
    }
}
