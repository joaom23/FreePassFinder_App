using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppFP.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppFP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterClientPage : ContentPage
    {
        RegisterClientViewModel viewmModel;
        public RegisterClientPage()
        {
            InitializeComponent();
            viewmModel = new RegisterClientViewModel();
            BindingContext = viewmModel;
        }
    }
}