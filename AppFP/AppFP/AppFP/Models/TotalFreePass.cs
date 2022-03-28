using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppFP.Models
{
    public class TotalFreePass
    {
        public int BCblub { get; set; }
        public int Duplex { get; set; }
        public string Informacao { get; set; }
        public bool IsSucess { get; set; }
    }
}
