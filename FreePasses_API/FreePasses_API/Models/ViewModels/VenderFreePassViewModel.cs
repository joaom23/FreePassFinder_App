using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreePasses_API.Models.ViewModels
{
    public class VenderFreePassViewModel
    {
        public int Numero { get; set; }

        public string Disco { get; set; }

        public string CourseName { get; set; }
        public string userId { get; set; }
        public string VendedorId { get; set; }
    }
}
