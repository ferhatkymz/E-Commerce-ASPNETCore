using System.Net;
using System.Net.Mail;

namespace EF_MVC.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }

    //tabi benim smtp servisini program.cs eklemem gerek
    public class SmtpEmailService : IEmailService
    {
        private IConfiguration _configuration;
        public SmtpEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body)//kime,başlık,içerik
        {
            using (var client = new SmtpClient(_configuration["Email:Host"]))
            {
                client.UseDefaultCredentials = false;//kendi ayarlarımı uygulicam
                client.Credentials = new NetworkCredential(_configuration["Email:UserName"], _configuration["Email:Password"]);
                client.Port = 587;
                client.EnableSsl = true;
                var mailmessage = new MailMessage//mesaj içeriğini oluştur.
                {
                    From = new MailAddress(_configuration["Email:UserName"]!),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailmessage.To.Add(toEmail);//mesaj kime gidecek
                await client.SendMailAsync(mailmessage);
            }
        }
    }

    //yarın kendi apimi oluşturduğumda 

    // public class ApiEmailService:IEmailService{}
}