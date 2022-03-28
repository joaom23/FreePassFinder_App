using FreePasses_API.Models;
using FreePasses_API.Models.ViewModels;
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
    //[Authorize(Roles = "Nucleo")]
    [Route("/api/[controller]")]
    [ApiController]
    public class NucleosController : Controller
    {
        private INucleoService _nucleoService;
        private UserManager<IdentityUser> _userManager;

        public NucleosController(INucleoService nucleoService, UserManager<IdentityUser> userManager)
        {
            _nucleoService = nucleoService;
            _userManager = userManager;
        }

        [HttpPost("VenderFreePass")]
        public async Task<IActionResult> VenderFreePassAsync([FromBody] VenderFreePassViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _nucleoService.VenderFreePassAsync(viewModel);
                if (result.IsSucess)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest("Algumas propriedades não são válidas");
        }

        [HttpPost("UpdateSellerPosition")]
        public async Task<IActionResult> UpdateSellerPositionAsync([FromBody] SellerPositionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _nucleoService.UpdateSellerPositionAsync(viewModel);
                if (result.IsSucess)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest("Algumas propriedades não são válidas");
        }

        [HttpPost("ChangeState")]
        public async Task<IActionResult> ChangeStateAsync([FromBody] ChangeOnlineStateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _nucleoService.ChangeOnlineStateAsync(viewModel);
                if (result.IsSucess)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest("Erro");
        }

        [HttpPost("AddInformation")]
        public async Task<IActionResult> AddInformationAsync([FromBody] NucleoInformationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _nucleoService.AddInformationAsync(viewModel);
                if (result.IsSucess)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest("Erro");
        }

        [HttpGet("NucleoInformation/{id}")]
        public async Task<IActionResult> GetNucleoInformationAsync(string id)
        {
            if (ModelState.IsValid)
            {
                var result = await _nucleoService.GetNucleoInformationAsync(id);
                if (result.IsSucess)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest("Algumas propriedades não são válidas");
        }

        [HttpGet("FreePassesVendidos/{vendedorId}")]
        public async Task<IActionResult> GetFreePassesVendidosAsync(string vendedorId)
        {
            if (ModelState.IsValid)
            {
                var result = await _nucleoService.GetNFreePassesVendidosAsync(vendedorId);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest("Algumas propriedades não são válidas");
        }

        [HttpGet("TotalFreePasses/{nuc}")]
        public async Task<IActionResult> TotalFreePassesAsync(string nuc)
        {
            if (ModelState.IsValid)
            {
                var result = await _nucleoService.TotalFreePassAsync(nuc);
                if (result.IsSucess)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest("Algumas propriedades não são válidas");
        }

        [HttpGet]
        [Route("[action]/{filename}")]
        [AllowAnonymous]
        public IActionResult GetNucleoImage(string filename)
        {
            var image = System.IO.File.OpenRead("Recursos/FotoNucleos/" + filename + ".JPG");

            return File(image, "image/jpeg");
        }
    }
}
