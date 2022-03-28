using AppFP;
using AppFP.Helpers;
using AppFP.Models;
using AppFP.Views;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace AppFP.ViewModels
{
   public class ClientQRCodeViewModel : BaseViewModel
    {

        private string userId = Preferences.Get("Id", "NULL");
        public string UserId
        {
            get { return userId; }
            set
            {
        
                userId = value;
                OnPropertyChanged(nameof(UserId));

            }
        }
        public ClientQRCodeViewModel()
        {

        }
    }
}
