using FreePasses_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreePasses_API.Controllers
{
    [Authorize(Roles = "Cliente")]
    [Route("/api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ICustomerService _customerservice;
        private UserManager<IdentityUser> _userManager;

        public CustomerController(ICustomerService nucleoService, UserManager<IdentityUser> userManager)
        {
            _customerservice = nucleoService;
            _userManager = userManager;
        }

        [HttpGet("GetSellersPosition")]
        public async Task<IActionResult> GetSellersPosition()
        {
            if (ModelState.IsValid)
            {
                var result = await _customerservice.GetSellersPositionAsync();

                if (result.IsSuccess)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest("Houve algum problema");
        }
    }
}
