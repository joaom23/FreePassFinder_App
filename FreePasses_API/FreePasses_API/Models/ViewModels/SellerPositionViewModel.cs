using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreePasses_API.Models.ViewModels
{
    public class SellerPositionViewModel
    {
        public string userId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
