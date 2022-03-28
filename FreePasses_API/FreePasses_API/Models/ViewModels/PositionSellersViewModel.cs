using System;
using System.Collections.Generic;
using System.Text;

namespace FreePasses_API.ViewModels
{
    public class PositionSeller
    {
        public string userId { get; set; }
        public string Curso { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
   public class PositionSellersViewModel
    {
        public List<PositionSeller> Sellers { get; set; }
    
        public bool IsSuccess { get; set; }
    }
}
