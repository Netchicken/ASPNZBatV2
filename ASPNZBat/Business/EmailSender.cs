using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ASPNZBat.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ASPNZBat.Business
{
    using Microsoft.Extensions.Logging;

    //https://docs.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?view=aspnetcore-2.1&tabs=visual-studio
    //https://app.sendgrid.com/guide/integrate/langs/csharp

    //https://dejanstojanovic.net/aspnet/2018/june/sending-email-in-aspnet-core-using-smtpclient-and-dependency-injection/


    public class EmailSender : IEmailSender
    {

        private readonly ILogger _logger;
        private readonly SeatBookingDBContext _context;

        private readonly UserManager<IdentityUser> _userManager;

        private readonly SignInManager<IdentityUser> _signInManager;
        //   private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(User);

        //  //     
        public EmailSender(
            IOptions<AuthMessageSenderOptions> optionsAccessor,
            SeatBookingDBContext context,
            UserManager<IdentityUser> userManager,
            ILogger<EmailSender> logger //You have to add in the type that you want logged, basically the name of the class that you are in.
            )
        {
            Options = optionsAccessor.Value;
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        //This is the public method for sending
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var result = Execute(Options.SendGridKey, subject, message, email);

            _logger.LogInformation("Status displayed: {Message}", result.Status);
            _logger.LogInformation("CompletedSuccessfully displayed: {Message}", result.IsCompletedSuccessfully);
            _logger.LogInformation("Exception displayed: {Message}", result.Exception);

            return result;

        }

        private Task Execute(string apiKey, string subject, string message, string email)
        {
            //https://github.com/sendgrid/sendgrid-csharp#installation



            //var from = new EmailAddress("test@example.com", "Example User");
            //subject = "Sending with SendGrid is Fun";
            //var to = new EmailAddress("intelproof@gmail.com", "Example User");
            //var plainTextContent = "and easy to do anywhere, even with C#";
            //var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            //return client.SendEmailAsync(msg);


            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("Gary.d@visioncollege.ac.nz", "Gary Dix"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            //msg.TrackingSettings = new TrackingSettings
            //{
            //    ClickTracking = new ClickTracking { Enable = false }
            //};

            return client.SendEmailAsync(msg);
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

