using FreePasses_API.Models;
using FreePasses_API.Models.ViewModels;
using FreePasses_API.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreePasses_API.Services
{
    public interface INucleoService
    {
        Task<Response> VenderFreePassAsync(VenderFreePassViewModel viewModel);
        Task<TotalFreePassViewModel> TotalFreePassAsync(string nuc);
        Task<Response> UpdateSellerPositionAsync(SellerPositionViewModel viewModel);
        Task<Response> AddInformationAsync(NucleoInformationViewModel viewModel);
        Task<Response> ChangeOnlineStateAsync(ChangeOnlineStateViewModel viewModel);
        Task<Response> GetNucleoInformationAsync(string nucleoId);

        Task<ReturnVendidosViewModel> GetNFreePassesVendidosAsync(string vendedorId);
    }

    public class NucleoService : INucleoService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly APIContext _context;

        public NucleoService(UserManager<IdentityUser> userManager, APIContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        //Muda o estado do vendedor de online para offline e vice versa
        public async Task<Response> ChangeOnlineStateAsync(ChangeOnlineStateViewModel viewModel)
        {
            var nuc = await _context.Nucleos.FirstOrDefaultAsync(x => x.IdN == viewModel.userId);

            if(nuc != null)
            {
                nuc.IsOnline = viewModel.State;
                _context.Update(nuc);
                await _context.SaveChangesAsync();

                return new Response
                {
                    IsSucess = true
                };

            }

            return new Response
            {
                IsSucess = false
            };
        }

        //O vendedor atualiza a sua posição de X em X tempo
        public async Task<Response> UpdateSellerPositionAsync(SellerPositionViewModel viewModel)
        {
            var user = await _context.Nucleos.FirstOrDefaultAsync(x => x.IdN == viewModel.userId);
            
            if(user!= null)
            {
                user.Latitude = viewModel.Latitude;
                user.Longitude = viewModel.Longitude;

                _context.Nucleos.Update(user);
                await _context.SaveChangesAsync();

                return new Response
                {
                    IsSucess = true
                };
            }

            return new Response
            {
                IsSucess = false
            };
        }

        //Retorna os free passes de cada vendedor
        public async Task<TotalFreePassViewModel>TotalFreePassAsync(string nuc)
        {
            var nucleo = await _context.Nucleos.FirstOrDefaultAsync(x => x.IdCourseNavegation.CourseName == nuc);
            var total = await _context.FreePasses.Where(x=>x.CourseName == nuc).ToListAsync();
            var auxBClub = 0;
            var auxDuplex = 0;

            if(total == null)
            {
                return new TotalFreePassViewModel{
                    IsSucess = false
                };
            }

            foreach (var item in total)
            {
                switch (item.DiscoName)
                {
                    case "BClub":
                        auxBClub = item.TotalNumber;
                        break;
                    case "Duplex":
                        auxDuplex = item.TotalNumber;
                        break;
                    default:
                        break;
                }
            }

            return new TotalFreePassViewModel
            {
               BCblub = auxBClub,
               Duplex = auxDuplex,
               IsSucess = true,
               Informacao = nucleo.Informacao
               
            };
        }

        // "Vende" um o mais free passes
        public async Task<Response> VenderFreePassAsync(VenderFreePassViewModel viewModel)
        { 
            var freePass = _context.FreePasses.FirstOrDefault(x => x.DiscoName == viewModel.Disco && x.CourseName == viewModel.CourseName);

            var auxTotal = freePass.TotalNumber;

            freePass.TotalNumber -= viewModel.Numero;

            if(freePass.TotalNumber < 0)
            {
                return new Response
                {
                    Message = "Não foi possível continuar, apenas existem" + auxTotal + "FreePasses disponíveis!",
                    IsSucess = false
                };
            }

            var customer = _context.Clientes.Include(x=>x.IdCNavegation).FirstOrDefault(x => x.IdC == viewModel.userId);

            var venda = new ClientBuyFreePasse
            {
                userId = viewModel.userId,
                Disco = viewModel.Disco,
                Number = viewModel.Numero,
                Data = DateTime.Now,
                userEmail = customer.IdCNavegation.Email,
                VendedorId = viewModel.VendedorId
            };

            _context.Update(freePass);
            _context.ClientBuyFreePasses.Add(venda);
           await _context.SaveChangesAsync();

            return new Response
            {
                Message = "Foram vendidos " + viewModel.Numero + " FreePasses com sucesso! Ainda tem " + freePass.TotalNumber + " FreePasses.",
                IsSucess = true
            };
        }

        public async Task<Response> AddInformationAsync(NucleoInformationViewModel viewModel)
        {
            var nuc = await _context.Nucleos.FirstOrDefaultAsync(x => x.IdN == viewModel.NucleoId);

            if(nuc!= null)
            {               
                nuc.Informacao = viewModel.Information;
                _context.Nucleos.Update(nuc);
                await _context.SaveChangesAsync();

                return new Response
                {
                    IsSucess = true,
                    Message = "Informação atualizada com sucesso!"
                };
            }

            return new Response
            {
                IsSucess = false,
                Message = "Ocorreu um erro ao alterar a informação"
            };
        }

        public async Task<Response> GetNucleoInformationAsync(string nucleoId)
        {
            var nuc = await _context.Nucleos.FirstOrDefaultAsync(x => x.IdN == nucleoId);

            if (nuc != null)
            {
                if (string.IsNullOrEmpty(nuc.Informacao) || nuc.Informacao == null)
                {
                    nuc.Informacao = "";
                }

                return new Response
                {
                    IsSucess = true,
                    Message = nuc.Informacao
                };
            }

            return new Response
            {
                IsSucess = false,
                Message = "Ocorreu um erro ao obter a informação"
            };
        }

        public async Task<ReturnVendidosViewModel> GetNFreePassesVendidosAsync(string vendedorId)
        {
            ReturnVendidosViewModel vendidos = new ReturnVendidosViewModel
            {
                ClientBuyFreePasses = await _context.ClientBuyFreePasses.Where(x => x.VendedorId == vendedorId).ToListAsync()
            };

            if(vendidos != null)
            {
                vendidos.IsSuccess = true;
                return vendidos;
            }

            vendidos.IsSuccess = false;
            vendidos.Message = "Ainda não vendeu nenhum free pass";
            return vendidos;

        }
    }
}
