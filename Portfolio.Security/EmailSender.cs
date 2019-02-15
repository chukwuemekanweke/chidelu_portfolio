using BoilerPlate.ModelLayer;
using BoilerPlate.Security.Interface;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlate.Security
{
    public class EmailSender : IEmailSender
    {

        public EmailSender(AuthMessageSenderOptions optionsAccessor)
        {
            Options = optionsAccessor;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public async Task<Response> SendEmailAsync(string email, string subject, string message)
        {
            return await Execute(Options.SendGridKey, subject, message, email);
        }

        public async Task<Response> Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("no-reply@boilerplate.com", "Booiler Plate"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(true, true);

            var response = await client.SendEmailAsync(msg);

            var statusCode = response.StatusCode;
            var content = await response.DeserializeResponseBodyAsync(response.Body);
            return response;
        }
       
    }
}
