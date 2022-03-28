using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppFP
{
    [DesignTimeVisible(false)]
    [Obsolete]
    public partial class Primeira : MasterDetailPage
    {
        public Primeira()
        {
            InitializeComponent();
            SetValue(NavigationPage.HasNavigationBarProperty, false);
            this.Master = new Menu();
            this.Detail = new NavigationPage(new MainPageNucleos());
        }

        protected override bool OnBackButtonPressed() //Desativar botao de voltar atras do android
        {
            return true;
        }
    }
}
