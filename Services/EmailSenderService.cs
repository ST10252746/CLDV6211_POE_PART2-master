using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;
using ST10242546_CLDV6211_POE_.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ST10242546_CLDV6211_POE_.Services
{
    public class EmailSenderService : IEmailSender
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly SendGridSettings _sendGridSettings;



        public EmailSenderService(ISendGridClient sendGridClient, IOptions<SendGridSettings> sendGridSettings)
        {
            _sendGridSettings = sendGridSettings.Value;
            _sendGridClient = sendGridClient;
        }



        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_sendGridSettings.FromEmail, _sendGridSettings.EmailName),
                Subject = subject,
                HtmlContent = htmlMessage
            };



            msg.AddTo(email);
            await _sendGridClient.SendEmailAsync(msg);




        }
    }
}
