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
    public partial class ListaVendidosPage : ContentPage
    {
        ListaVendidosViewModel viewModel;
        public ListaVendidosPage()
        {
            InitializeComponent();
            viewModel = new ListaVendidosViewModel();
            BindingContext = viewModel;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await viewModel.FreePassesVendidos();

        }
    }
}