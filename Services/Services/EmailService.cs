using ERP_Project.Models;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace ERP_Project.Services.Services
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, string signature, string fromEmail = null, string fromName = null)
        {
            string emailBody = body + "<br><br>" + signature;

            fromEmail ??= _emailSettings.FromEmail;
            fromName ??= _emailSettings.FromName;   

            using (var smtpClient = new SmtpClient(_emailSettings.Host, _emailSettings.Port))
            {
                smtpClient.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),  
                    Subject = subject,
                    Body = emailBody,
                    IsBodyHtml = true  
                };

                mailMessage.To.Add(toEmail); 

             
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}
