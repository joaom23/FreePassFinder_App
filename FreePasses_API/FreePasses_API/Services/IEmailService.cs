using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreePasses_API.Services
{
    public interface IEmailService
    {
        Task sendEmailAsync(string email, string subject, string content);
    }

    public class SendGridEmailServices : IEmailService
    {
        private readonly IConfiguration _configuration;

        public SendGridEmailServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task sendEmailAsync(string toEmail, string subject, string content)
        {
            var apiKey = _configuration["SendGridApiKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("jmfernandes8@sapo.pt", "João Fernandes");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
