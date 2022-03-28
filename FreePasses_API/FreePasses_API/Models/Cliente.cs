using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FreePasses_API.Models
{
    public class Cliente
    {
        [Key]
        public string IdC { get; set; }

        [ForeignKey(nameof(IdC))]
        public virtual IdentityUser IdCNavegation { get; set; }
    }
}
