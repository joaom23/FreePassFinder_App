using AppFP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppFP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VenderFreePassesPage : ContentPage
    {
        VenderFreePassViewmodel viewModel;
        public VenderFreePassesPage()
        {
            InitializeComponent();
            viewModel = new VenderFreePassViewmodel();
            BindingContext = viewModel;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await viewModel.GetNumberFreePass();

        }
        public void OnScanResultQR(ZXing.Result result)
        {


            Device.BeginInvokeOnMainThread(() =>
            {
                Task.Run(async () =>
                {
                    var userId = result.Text.ToString();

                    await viewModel.CheckValidUser(userId);

                });
            });
        }
        protected override bool OnBackButtonPressed() //Desativar botao de voltar atras do android
        {
            if(viewModel.IsVisible == "true")
            {
                viewModel.IsVisible = "false";
            }

            return true;
        }
    }
}