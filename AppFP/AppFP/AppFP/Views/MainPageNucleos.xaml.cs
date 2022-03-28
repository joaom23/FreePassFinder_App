using AppFP.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppFP
{
    public partial class MainPageNucleos : ContentPage
    {
        MenuViewModel viewModel;
        private readonly string _email = Preferences.Get("Email", "NULL");
        public MainPageNucleos()
        {
            InitializeComponent();
            viewModel = new MenuViewModel();
            BindingContext = viewModel;

            bemvindo.Text += _email;

            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                Task.Run(() =>
                {
                   Device.BeginInvokeOnMainThread(async() =>
                    {
                        await viewModel.GetSellerLocation();
                    });
                });

                var doAgain = Convert.ToBoolean(Preferences.Get("doAgain", "NULL"));

                return doAgain;
            });
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await viewModel.GetInformation();

        }
    }
}
