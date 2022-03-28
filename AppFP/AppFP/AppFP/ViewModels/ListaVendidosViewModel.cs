
using AppFP.Helpers;
using AppFP.Models;
using AppFP.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppFP.ViewModels
{
    public class ListaVendidosViewModel : BaseViewModel
    {
        private readonly string _token = Preferences.Get("Token", "NULL");
        private readonly HttpClient _client;
        private readonly string _userId = Preferences.Get("Id", "NULL");

        public ObservableCollection<VendidosInformation> vendidos;

        public ObservableCollection<VendidosInformation> Vendidos
        {
            get { return vendidos; }
            set
            {
                vendidos = value;
                OnPropertyChanged(nameof(Vendidos));
            }
        }


        public ListaVendidosViewModel()
        {
            vendidos = new ObservableCollection<VendidosInformation>();
            _client = new HttpClient();
        }

        public async Task FreePassesVendidos()
        {
            vendidos.Clear();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            var responsee = await _client.GetAsync(IP_API.APIserver + "/api/nucleos/freepassesvendidos/" + _userId);

            var responsebody = await responsee.Content.ReadAsStringAsync();

            var vendas = JsonConvert.DeserializeObject<ReturnVendidos>(responsebody);

            foreach (var v in vendas.ClientBuyFreePasses)
            {
                var vendaAux = new VendidosInformation 
                {
                    Email = v.userEmail,
                    Quantidade = "Vendidos: " + v.Number.ToString(),
                    Data = v.Data,
                    Disco = "Discoteca: " + v.Disco
                    
                };

                vendidos.Add(vendaAux);
            }

        }
    }
}
