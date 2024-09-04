namespace BuyerSeller.Service__Business_Logic_Layer_.Interfaces
{
    /// <summary>
    /// Interface for email service
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Send templated email message
        /// </summary>
        /// <param name="recipient">Recipient</param>
        /// <param name="subject">Email Subject</param>
        /// <param name="body">Email Body</param>
        /// <returns></returns>
        public Task SendEmailAsync(string recipient, string subject, string body);
    }
}
