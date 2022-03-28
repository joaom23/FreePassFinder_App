using System;
using System.Collections.Generic;
using System.Text;

namespace FreePasses_API.Models
{
    public class ReturnVendidosViewModel
    {
        public List<ClientBuyFreePasse> ClientBuyFreePasses { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}
