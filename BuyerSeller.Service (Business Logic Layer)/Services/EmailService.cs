using BuyerSeller.Service__Business_Logic_Layer_.Interfaces;
using Microsoft.Extensions.Configuration;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using BuyerSeller.Core.Utility;

namespace BuyerSeller.Service__Business_Logic_Layer_.Services
{
    /// <summary>
    /// Email notification service
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly IConfiguration _config;

        /// <summary>
        /// Create new instance of <see cref="EmailService"/>
        /// </summary>
        /// <param name="config"><see cref="IConfiguration"/> instance</param>
        public EmailService(IConfiguration config)
        {
            _config = config;
            _smtpClient = new SmtpClient();

            Connect();
        }

        /// <summary>
        /// Send templated email message
        /// </summary>
        /// <param name="recipient">Recipient</param>
        /// <param name="subject">Email Subject</param>
        /// <param name="body">Email Body</param>
        /// <returns></returns>
        public async Task SendEmailAsync(string recipient, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("Smtp")["Email"]));
            email.To.Add(MailboxAddress.Parse(recipient));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = string.Format(Constants.EMAIL_TEMPLATE_STRING, subject, body)
            };

            await _smtpClient.SendAsync(email);
        }

        private void Connect()
        {
            if (!int.TryParse(_config.GetSection("Smtp")["Port"], out int smtpPort))
            {
                throw new Exception("Invalid Smtp-Port");
            }
            else
            {
                _smtpClient.Connect(
                    _config.GetSection("Smtp")["Host"],
                    smtpPort
                );
                _smtpClient.Authenticate(
                    _config.GetSection("Smtp")["Email"],
                    _config.GetSection("Smtp")["Password"]
                );
            }
        }
    }
}
