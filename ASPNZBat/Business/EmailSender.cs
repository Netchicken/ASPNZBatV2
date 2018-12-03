using ASPNZBat.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using RazorHtmlEmails;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Logging;

namespace ASPNZBat.Business
{


    //https://docs.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?view=aspnetcore-2.1&tabs=visual-studio
    //https://app.sendgrid.com/guide/integrate/langs/csharp

    //https://dejanstojanovic.net/aspnet/2018/june/sending-email-in-aspnet-core-using-smtpclient-and-dependency-injection/

    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;
        //private readonly SeatBookingDBContext _context;

        //private readonly UserManager<IdentityUser> _userManager;

        //private readonly SignInManager<IdentityUser> _signInManager;
        //   private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(User);

        //  //
        public EmailSender(
            IOptions<AuthMessageSenderOptions> optionsAccessor,
            //SeatBookingDBContext context,
            //UserManager<IdentityUser> userManager,
            ILogger<EmailSender> logger //You have to add in the type that you want logged, basically the name of the class that you are in.
            )
        {
            Options = optionsAccessor.Value;
            //_context = context;
            //_userManager = userManager;
            _logger = logger;
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




            _logger.LogInformation("Details: {Message}", email + " " + subject + " " + message);
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("Gary.d@visioncollege.ac.nz", "Gary Dix");
            var to = new EmailAddress(email);
            var plainTextContent = message;
            var htmlContent = message;
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