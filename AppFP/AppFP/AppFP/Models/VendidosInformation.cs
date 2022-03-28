using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppFP.Models
{
    public class VendidosInformation
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Quantidade { get; set; }

        public string Disco { get; set; }
        public DateTime Data { get; set; }
    }
}
