using ASPNZBat.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
//using RazorHtmlEmails;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Logging;

namespace ASPNZBat.Business
{
    using System;
    //using RazorHtmlEmails.Common;
    //using RazorHtmlEmails.RazorClassLib.Services;



    //https://docs.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?view=aspnetcore-2.1&tabs=visual-studio
    //https://app.sendgrid.com/guide/integrate/langs/csharp

    //https://dejanstojanovic.net/aspnet/2018/june/sending-email-in-aspnet-core-using-smtpclient-and-dependency-injection/

    public class EmailSender : IEmailSender

    {
        //private readonly IRegisterAccountService _registerAccountService;
        private readonly ILogger _logger;
        //private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;

        public EmailSender(
            IOptions<AuthMessageSenderOptions> optionsAccessor,

            ILogger<EmailSender> logger //You have to add in the type that you want logged, basically the name of the class that you are in.
                                        //IRazorViewToStringRenderer razorViewToStringRenderer,
                                        //IRegisterAccountService registerAccountService
            )
        {
            Options = optionsAccessor.Value;
            _logger = logger;
            //_razorViewToStringRenderer = razorViewToStringRenderer;
            //_registerAccountService = registerAccountService;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        //This is the public method for sending
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, email);
        }

        private async Task Execute(string apiKey, string subject, string message, string email)
        {
            //https://app.sendgrid.com/guide/integrate/langs/csharp
            //https://github.com/sendgrid/sendgrid-csharp#installation

            var Title = ("<h2>Vision College</h2><h4>The New Zealand Business Administration Course </h4>");
            //    string body = await _registerAccountService.GetEmailHTMLBody();

            _logger.LogInformation("Details: {Message}", email + " " + subject + " " + message);
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("Gary.d@visioncollege.ac.nz", "Gary Dix");
            var to = new EmailAddress(email);
            var plainTextContent = message;
            var htmlContent = Title + message;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var result = await client.SendEmailAsync(msg);

            _logger.LogInformation("Status displayed: {Message}", result.StatusCode);
            // _logger.LogInformation("Body displayed: {Message}", result.Body.Headers.Allow);
            //  _logger.LogInformation("Exception displayed: {Message}", result.);
            //_logger.LogInformation("Isfaulted displayed: {Message}", result.IsFaulted);
            //_logger.LogInformation("IsCompleted displayed: {Message}", result.IsCompleted);

            //  return null;
        }
    }

    /// <summary>
    /// A class to fetch the secure email key.
    /// </summary>

    public class AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
    }
}