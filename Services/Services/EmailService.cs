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

            // Use the email settings for 'FromEmail' and 'FromName' if not provided
            fromEmail ??= _emailSettings.FromEmail; // Use default 'FromEmail' if not passed
            fromName ??= _emailSettings.FromName;   // Use default 'FromName' if not passed

            using (var smtpClient = new SmtpClient(_emailSettings.Host, _emailSettings.Port))
            {
                smtpClient.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),  // Use dynamic or default 'From'
                    Subject = subject,
                    Body = emailBody,
                    IsBodyHtml = true  // Ensuring that the email body is HTML
                };

                mailMessage.To.Add(toEmail); // Add recipient's email

                // Send the email asynchronously
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}
