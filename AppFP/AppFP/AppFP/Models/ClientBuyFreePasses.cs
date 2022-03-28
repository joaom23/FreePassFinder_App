using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppFP.Models
{
    public class ClientBuyFreePasse
    {
        [Key]
        public int IdCompra { get; set; }
        public string userId { get; set; }
        public string userEmail { get; set; }
        public int Number { get; set; }
        public string Disco { get; set; }
        public DateTime Data { get; set; }
        public string VendedorId { get; set; }
    }
}
