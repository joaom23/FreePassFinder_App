using FreePasses_API.Models;
using FreePasses_API.Models.ViewModels;
using FreePasses_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FreePasses_API.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public UsersController(IUserService userService, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userService = userService;
            _userManager = userManager;
            _configuration = configuration;
        }
        
        [HttpPost("RegisterClient")]
        public async Task<IActionResult> RegisterClientAsync([FromBody] RegisterClientViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterClientAsync(viewModel);

                if (result.IsSucess)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest("Algumas propriedades não são válidas");
        }

        [HttpPost("RegisterNucleo")]
        public async Task<IActionResult> RegisterNucleoAsync([FromBody]RegisterNucleoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterNucleoAsync(viewModel);

                if (result.IsSucess)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest("Algumas propriedades não são válidas");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginAsync(viewModel);

                if (result.IsSucess)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            return BadRequest("Algumas propriedades não são válidas");
        }

        [HttpPost("ReciveEmailResetPassword")]
        public async Task<IActionResult> ReciveEmailResetPasswordAsync([FromBody] CustomerEmailViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.ReciveEmailForogtPasswordAsync(viewModel.Email);

                if (result.IsSucess)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            return BadRequest("Algumas propriedades não são válidas");
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromForm] ResetPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.ResetPasswordAsync(viewModel);

                if (result.IsSucess)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            return BadRequest("Algumas propriedades não são válidas");
        }


        [HttpGet("ConfirmarEmail")]
        public async Task<IActionResult> ConfirmarEmailAsync(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                return NotFound();
            }

            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);

            if (user == null)
            {
                return BadRequest(new Response
                {
                    IsSucess = false,
                    Message = "Utilizador não encontrado"
                });
            }

            var decodedToken = WebEncoders.Base64UrlDecode(token);

            var normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ConfirmEmailAsync(user, normalToken);

            if (result.Succeeded)
            {
                return Redirect($"{_configuration["AppUrl"]}/confirmaremail.html");
            }

            return BadRequest(new Response
            {
                IsSucess = false,
                Message = "Erro ao confirmar o email",
            });
        }

        [HttpGet("CheckValidUser/{id}")]
        public async Task<IActionResult> CheckValidUserAsync(string id)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.CheckValidUserAsync(id);
                if (result.IsSucess)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest("Erro");
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public IActionResult GetMainPageImage()
        {
            var image = System.IO.File.OpenRead("Recursos/MainPage/disco-icon.JPG");

            return File(image, "image/jpeg");
        }
    }
}
