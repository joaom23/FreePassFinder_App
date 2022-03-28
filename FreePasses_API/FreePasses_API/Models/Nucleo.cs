using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FreePasses_API.Models
{
    public class Nucleo
    {
        [Key]
        public string IdN { get; set; }

        public string IdCourse { get; set; }

        public double ? Latitude { get; set; }

        public double ? Longitude { get; set; }

        public bool IsOnline { get; set; }

        public string Informacao { get; set; }

        [ForeignKey(nameof(IdN))]
        public virtual IdentityUser IdCNavegation { get; set; }

        [ForeignKey(nameof(IdCourse))]
        public virtual NomesDeNucleos IdCourseNavegation { get; set; }

        

        //[InverseProperty(nameof(UsersNucleo.IdNucleoNavigation))]
        //public virtual ICollection<UsersNucleo> UsersNucleos { get; set; }
    }
}
