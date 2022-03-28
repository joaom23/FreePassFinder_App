using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FreePasses_API.Models
{
    public class FreePass
    {
        [Key]

        public string IdFP { get; set; }
        public string DiscoName { get; set; }
        public int TotalNumber { get; set; }
        public string CourseName { get; set; }
        public NomesDeNucleos Course { get; set; }
    }
}
