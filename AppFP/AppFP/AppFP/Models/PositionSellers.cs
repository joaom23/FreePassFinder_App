using System;
using System.Collections.Generic;
using System.Text;

namespace AppFP.Models
{
    public class PositionSeller
    {
        public string userId { get; set; }
        public string Curso { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }

    public class PositionSellers
    {
        public List<PositionSeller> Sellers { get; set; }

        public bool IsSuccess { get; set; }
    }
}
