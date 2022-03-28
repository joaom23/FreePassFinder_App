using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreePasses_API.Models
{
    public class Response
    {
        public string Message { get; set; }
        public bool IsSucess { get; set; }
        public string Role { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
