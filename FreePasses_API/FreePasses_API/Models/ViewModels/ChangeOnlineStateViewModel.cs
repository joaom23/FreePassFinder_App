using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreePasses_API.Models.ViewModels
{
    public class ChangeOnlineStateViewModel
    {
        public string userId { get; set; }

        public bool State { get; set; }
    }
}
