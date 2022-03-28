using FreePasses_API.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreePasses_API.Services
{
    public interface ICustomerService
    {
        Task<PositionSellersViewModel> GetSellersPositionAsync();
        Task<PositionSellersViewModel> UpdateSellersOnMapAsync();
    }

    public class CustomerService : ICustomerService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly APIContext _context;

        public CustomerService(UserManager<IdentityUser> userManager, APIContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        //Retorna a posição de todos os vendedores online
        public async Task<PositionSellersViewModel> GetSellersPositionAsync()
        {
            var sellers = await _context.Nucleos.Include(x => x.IdCourseNavegation).Where(x => x.IsOnline == true).ToListAsync();

            if (sellers == null || sellers.Count == 0)
            {
                return new PositionSellersViewModel
                {
                    Sellers = null,
                    IsSuccess = false
                };
            }

            if (sellers != null)
            {
                var sellersList = new PositionSellersViewModel();
                sellersList.Sellers = new List<PositionSeller>();

                foreach (var seller in sellers)
                {
                    var auxSeller = new PositionSeller
                    {
                        userId = seller.IdN,
                        Curso = seller.IdCourseNavegation.CourseName,
                        Latitude = seller.Latitude,
                        Longitude = seller.Longitude
                    };

                    sellersList.Sellers.Add(auxSeller);
                }

                sellersList.IsSuccess = true;
                return sellersList;
            }

            return new PositionSellersViewModel
            {
                Sellers = null,
                IsSuccess = false
            };
        }

        public Task<PositionSellersViewModel> UpdateSellersOnMapAsync()
        {
            throw new NotImplementedException();
        }
    }
}
