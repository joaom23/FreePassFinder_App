using AppFP.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppFP
{
    public partial class MainPage : ContentPage
    {
        MainPageViewModel viewmodel;
        public MainPage()
        {
            viewmodel = new MainPageViewModel();
            BindingContext = viewmodel;

            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

        }

        protected override bool OnBackButtonPressed() //Desativar botao de voltar atras do android
        {
            return true;
        }

    }
}
