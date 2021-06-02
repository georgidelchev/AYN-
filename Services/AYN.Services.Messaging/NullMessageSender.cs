using System.Collections.Generic;
using System.Threading.Tasks;

namespace AYN.Services.Messaging
{
    public class NullMessageSender : IEmailSender
    {
        public Task SendEmailAsync(
            string from,
            string fromName,
            string to,
            string subject,
            string htmlContent,
            IEnumerable<EmailAttachment> attachments = null)
            => Task.CompletedTask;
    }
}
