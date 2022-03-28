using FreePasses_API.Models;
using FreePasses_API.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FreePasses_API.Services
{
    public interface IUserService
    {
        Task<Response> RegisterNucleoAsync(RegisterNucleoViewModel viewModel);
        Task<Response> RegisterClientAsync(RegisterClientViewModel viewModel);
        Task<Response> LoginAsync(LoginViewModel viewmodel);
        Task<Response> CheckValidUserAsync(string userId);
        Task<Response> ReciveEmailForogtPasswordAsync(string email);
        Task<Response> ResetPasswordAsync(ResetPasswordViewModel viewmodel);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly APIContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _mailservice;

        public UserService(UserManager<IdentityUser> userManager, APIContext context, IConfiguration configuration, IEmailService mailService)
        {
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
            _mailservice = mailService;
        }

        //Login
        public async Task<Response> LoginAsync(LoginViewModel viewmodel)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == viewmodel.Email);

            if (user == null)
            {
                return new Response
                {
                    Message = "Não existe nenhum utilizador com esse Email",
                    IsSucess = false
                };
            
            }

            var result = await _userManager.CheckPasswordAsync(user, viewmodel.Password);

            if (!result)
            {
                return new Response
                {
                    Message = "Palavra passe incorreta",
                    IsSucess = false
                };
            }
            var claims = new List<Claim>();
            var userRoles = await _userManager.GetRolesAsync(user);
            var nuc = await _context.Nucleos.Include(x=>x.IdCourseNavegation).FirstOrDefaultAsync(x => x.IdN == user.Id);

            //Verificar se o user é nucleo ou vendedor
            if (await _userManager.IsInRoleAsync(user, "Nucleo"))
            {
                //É um nucleo
                nuc.IsOnline = true;

                _context.Nucleos.Update(nuc);
                await _context.SaveChangesAsync();

                claims.Add(new Claim("Email", viewmodel.Email));
                claims.Add(new Claim("Id", user.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Name, user.Id));
                claims.Add(new Claim("Curso", nuc.IdCourseNavegation.CourseName));
                claims.Add(new Claim(ClaimTypes.Role, "Nucleo"));
            }
            else
            {
                //É um cliente
                if (!user.EmailConfirmed)
                {
                    return new Response
                    {
                        IsSucess = false,
                        Message = "Email não confirmado, por favor confirme o seu email."
                    };
                }

                claims.Add(new Claim("Email", viewmodel.Email));
                claims.Add(new Claim("Id", user.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Name, user.Id));
                claims.Add(new Claim(ClaimTypes.Role, "Cliente"));
            }

            var keybuffer = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

            var token = new JwtSecurityToken(
               issuer: _configuration["AuthSettings:Issuer"],
               audience: _configuration["AuthSettings:Audience"],
               claims: claims,
               expires: DateTime.Now.AddDays(10),
               signingCredentials: new SigningCredentials(keybuffer, SecurityAlgorithms.HmacSha256)
               );

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new Response
            {
                Message = tokenAsString,
                IsSucess = true
            };
        }

        //Registo de clientes
        public async Task<Response> RegisterClientAsync(RegisterClientViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new NullReferenceException("Viewmode is null");
            }

            var checkExistingUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == viewModel.Email);

            if (checkExistingUser != null)
            {
                return new Response
                {
                    IsSucess = false,
                    Message = $"Já existe um utlizador com o email {viewModel.Email}"
                };
            }

            if (viewModel.Password != viewModel.ConfirmPassword)
            {
                return new Response
                {
                    Message = "As palavras passes não coincidem",
                    IsSucess = false
                };
            }

            IdentityResult result = null;
            IdentityUser user = new IdentityUser();
            try
            {
                user.Email = viewModel.Email;
                user.UserName = viewModel.Email;
                
                result = await _userManager.CreateAsync(user, viewModel.Password);
                await _userManager.AddToRoleAsync(user, "Cliente");

                Cliente c = new Cliente
                {
                    IdC = user.Id
                };

                _context.Clientes.Add(c);
                await _context.SaveChangesAsync();

            }
            catch
            {
                return new Response
                {
                    Message = "Erro ao criar o utilizador"
                };
            }

            if (result.Succeeded)
            {
                //Enviar email de confirmação

                var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var endodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);

                var validEmailToken = WebEncoders.Base64UrlEncode(endodedEmailToken);

                var url = $"{_configuration["AppUrl"]}/api/users/ConfirmarEmail?userid={user.Id}&token={validEmailToken}";

                await _mailservice.sendEmailAsync(user.Email, "Confirme o seu Email", "<h1>Bem vindo ao Free Pass Finder </h1>" +
                    $"<p>Por favor confira o seu email carregando no seguinte link <a href='{url}'>Confirmar Email</a> </p>");

                return new Response
                {
                    Message = "Conta criada com sucesso! Por favor confirme o seu email.",
                    IsSucess = true
                };
            }

            return new Response
            {
                Message = "Erro ao criar o utilizador",
                IsSucess = false
            };
        }

        //Registo dos nucleos

        public async Task<Response> RegisterNucleoAsync(RegisterNucleoViewModel viewModel)
        {           
            if (viewModel == null)
            {
                throw new NullReferenceException("Viewmode is null");
            }

            var checkExistingUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == viewModel.Email);

            if (checkExistingUser != null)
            {
                return new Response
                {
                    IsSucess = false,
                    Message = $"Já existe um utlizador com o email {viewModel.Email}"
                };           
            }

            if (viewModel.Password != viewModel.ConfirmPassword)
            {
                return new Response
                {
                    Message = "As palavras passes não coincidem",
                    IsSucess = false
                };
            }

            IdentityResult result = null;

            try
            {

                IdentityUser user = new IdentityUser
            {
                Email = viewModel.Email,
                UserName = viewModel.Email
            };

             result = await _userManager.CreateAsync(user, viewModel.Password);
               await _userManager.AddToRoleAsync(user, "Nucleo");

                var nome = _context.NomesDeNucleos.FirstOrDefault(x => x.CourseName == viewModel.Curso);

                Nucleo nuc = new Nucleo
                {
                    IdN = user.Id,
                    IdCourse = nome.Id,
                    IsOnline = false,
                    Latitude = null,
                    Longitude = null
                };

                _context.Nucleos.Add(nuc);
                await _context.SaveChangesAsync();

            }
            catch
            {
                return new Response
                {
                    Message = "Erro ao criar o utilizador"
                };
            }

            if (result.Succeeded)
            { 
                return new Response
                {
                    Message = "Utilizador criado com sucesso!",
                    IsSucess = true
                };
            }          

            return new Response
            {
                Message = "Erro ao criar o utilizador",
                IsSucess = false
            };
        }

        public async Task<Response> CheckValidUserAsync(string userId)
        {
            var user = await _context.Clientes.FirstOrDefaultAsync(x => x.IdC == userId);

            if (user != null)
            {
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

        public async Task<Response> ReciveEmailForogtPasswordAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return new Response
                {
                    IsSucess = false
                };
            }
                
            var user = _userManager.Users.FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                return new Response
                {
                    IsSucess = false,
                    Message = $"Não existe nenhum utilizador associado ao email {email}"
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = Encoding.UTF8.GetBytes(token);

            var validToken = WebEncoders.Base64UrlEncode(encodedToken);

            var url = $"{_configuration["AppUrl"]}/recuperarpassword.html?permitido=ok&email={email}&token={validToken}";

            await _mailservice.sendEmailAsync(email, "Recuperar Password", "<h1>Siga as seguintes intruções para recuperar a sua password</h1>" +
                $"<p>Para recuparar a sua password <a href='{url}'>clique aqui</a> </p>");

            return new Response
            {
                IsSucess = true,
                Message = "O link para recuperar a sua password foi enviado para o seu email."
            };

        }

        public async Task<Response> ResetPasswordAsync(ResetPasswordViewModel viewmodel)
        {            
                var user = _userManager.Users.FirstOrDefault(x => x.Email == viewmodel.Email);

                if (user == null)
                {
                    return new Response
                    {
                        IsSucess = false,
                        Message = $"Não existe nenhum utilizador associado ao email {viewmodel.Email}"
                    };
                }

                if (viewmodel.NewPassword != viewmodel.ConfirmNewPassword)
                {
                    return new Response
                    {
                        IsSucess = false,
                        Message = "Palavras passe não coincidem",
                    };
                }

                var decodedToken = WebEncoders.Base64UrlDecode(viewmodel.Token);
                var normalToken = Encoding.UTF8.GetString(decodedToken);

                var result = await _userManager.ResetPasswordAsync(user, normalToken, viewmodel.NewPassword);

                if (result.Succeeded)
                {
                    return new Response
                    {
                        IsSucess = true,
                        Message = "Password reposta com sucesso!",
                    };
                }

                return new Response
                {
                    IsSucess = false,
                    Message = "Ups, algo correu mal na recuperação da password",
                };
            }
        }
}
