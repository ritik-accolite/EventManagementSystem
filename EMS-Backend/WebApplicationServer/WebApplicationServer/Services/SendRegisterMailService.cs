using System.Net;
using System.Net.Mail;
using System.Reflection.Metadata.Ecma335;
using WebApplicationServer.Models.ViewModels;
using WebApplicationServer.Services.IService;

namespace WebApplicationServer.Services
{
    public class SendRegisterMailService : ISendRegisterSuccessMailService
    {
        private readonly IConfiguration _configuration;

        public SendRegisterMailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
  
        public async Task<bool> SendRegisterSuccessMailAsync(string email, string Subject, string message)
        {
            bool status = false;
            try
            {
                string secretKey = _configuration.GetValue<string>("AppSettings:SecretKey");
                string fromEmail = _configuration.GetValue<string>("AppSettings:EmailSettings:From");
                string smtpServer = _configuration.GetValue<string>("AppSettings:EmailSettings:SmtpServer");
                int port = _configuration.GetValue<int>("AppSettings:EmailSettings:Port");
                bool enableSSL = _configuration.GetValue<bool>("AppSettings:EmailSettings:EnableSSL");

                MailMessage mailMessage = new MailMessage()
                {
                    From = new MailAddress(fromEmail),
                    Subject = Subject,
                    Body = message,
                    BodyEncoding = System.Text.Encoding.ASCII,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(email);

                SmtpClient smtpClient = new SmtpClient(smtpServer)
                {
                    Port = port,
                    Credentials = new NetworkCredential(fromEmail, secretKey),
                    EnableSsl = enableSSL
                };

                await smtpClient.SendMailAsync(mailMessage);
                status = true;
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
    }
}
