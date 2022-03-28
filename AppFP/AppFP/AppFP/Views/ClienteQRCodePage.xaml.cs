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
    public partial class ClienteQRCodePage : ContentPage
    {
        string userId;
        ClientQRCodeViewModel viewModel;
        public ClienteQRCodePage(string userId)
        {
            InitializeComponent();           
            this.userId = userId;
            viewModel = new ClientQRCodeViewModel();
            BindingContext = viewModel;
        }
    }
}