using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FreePasses_API.Models
{
    public class NomesDeNucleos
    {
        public NomesDeNucleos(string Id, string CourseName)
        {
            this.Id = Id;
            this.CourseName = CourseName;
        }

        [Key]
        public string Id { get; set; }

        public string CourseName { get; set; }

        [InverseProperty(nameof(Nucleo.IdCourseNavegation))]
        public virtual ICollection<Nucleo> Nucleos { get; set; }
        public ICollection<FreePass> FreePasses { get; set; }

    }
}
