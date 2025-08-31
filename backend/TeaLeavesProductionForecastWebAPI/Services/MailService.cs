using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace TeaLeavesProductionForecastWebAPI.Services
{
    public class MailService : IMailService
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _user;
        private readonly string _pass;

        public MailService(IConfiguration config)
        {
            // Try config first, then fallback to environment variables
            _host = config["SMTP_HOST"] ?? Environment.GetEnvironmentVariable("SMTP_HOST")
                    ?? throw new ArgumentNullException("SMTP_HOST is missing");
            _port = int.TryParse(config["SMTP_PORT"] ?? Environment.GetEnvironmentVariable("SMTP_PORT"), out var port) ? port : 587;
            _user = config["SMTP_USER"] ?? Environment.GetEnvironmentVariable("SMTP_USER")
                    ?? throw new ArgumentNullException("SMTP_USER is missing");
            _pass = config["SMTP_PASS"] ?? Environment.GetEnvironmentVariable("SMTP_PASS")
                    ?? throw new ArgumentNullException("SMTP_PASS is missing");
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using var client = new SmtpClient(_host, _port)
            {
                Credentials = new NetworkCredential(_user, _pass),
                EnableSsl = true
            };

            var mail = new MailMessage(_user, to, subject, body)
            {
                IsBodyHtml = true
            };

            await client.SendMailAsync(mail);
        }
    }
}
