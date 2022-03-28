using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppFP.Models
{
    public class Response
    {
        //public IEnumerable<string> Errors { get; set; }
        public string Message { get; set; }
        public bool IsSucess { get; set; }

        public string Role { get; set; }
    }
}
