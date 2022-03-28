using AppFP.Helpers;
using AppFP.Models;
using AppFP.Services;
using AppFP.Views;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppFP
{
    public partial class App : Application
    {
        private readonly HttpClient _client;
        public App()
        {
            _client = new HttpClient();         
            InitializeComponent();
          
            MainPage = new NavigationPage (new MainPage());

        }

        protected override void OnStart()
        {
        }

        protected async override void OnSleep()
        {
            base.OnSleep();
            var _userId = Preferences.Get("Id", "NULL");
            var _token = Preferences.Get("Token", "NULL");
            if (_userId != "NULL")
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

            }          
        }

        protected override void OnResume()
        {
        }
    }
}
