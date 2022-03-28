using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FreePasses_API.Models.ViewModels
{
    public class TotalFreePassViewModel
    {
        public int BCblub { get; set; }
        public int Duplex { get; set; }
        public string Informacao { get; set; }
        public bool IsSucess { get; set; }
    }
}
