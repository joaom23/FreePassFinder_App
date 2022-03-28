
using AppFP.Helpers;
using AppFP.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppFP.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public bool IsDebug = false;

        private ImageSource imagem = IP_API.APIserver + "/api/users/GetMainPageImage";

        public ImageSource Imagem
        {
            get { return imagem; }
            set
            {
                imagem = value;
                OnPropertyChanged(nameof(Imagem));
            }
        }
        public Command LoginPageCommnad { get; }
        public Command LoginClientCommand { get; }
        public Command RegisterClientPageCommand { get; }
        public MainPageViewModel()
        {
            LoginPageCommnad = new Command(LoginPage);
            LoginClientCommand = new Command(LoginClient);
            RegisterClientPageCommand = new Command(RegisterClientPage);
            Imagem = IP_API.APIserver + "/api/users/GetMainPageImage";
        }

        private async void LoginPage()
        {

            await App.Current.MainPage.Navigation.PushAsync(new LoginPage());

        }

        private async void LoginClient()
        {
            await App.Current.MainPage.Navigation.PushAsync(new CustomMapPage());
        }

        private async void RegisterClientPage()
        {
            await App.Current.MainPage.Navigation.PushAsync(new RegisterClientPage());
        }
    }
}
