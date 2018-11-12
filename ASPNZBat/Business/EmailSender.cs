﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNZBat.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.AspNetCore.Identity;

namespace ASPNZBat.Business
{
    //https://docs.microsoft.com/en-us/aspnet/core/security/authentication/accconfirm?view=aspnetcore-2.1&tabs=visual-studio
    //https://app.sendgrid.com/guide/integrate/langs/csharp
    //netchicken Chicken1!
    //API key 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


    public class EmailSender : IEmailSender
    {

        private readonly SeatBookingDBContext _context;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        //   private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(User);


        public EmailSender(
            IOptions<AuthMessageSenderOptions> optionsAccessor,
            SeatBookingDBContext context,
            UserManager<IdentityUser> userManager
        )
        {
            Options = optionsAccessor.Value;
            _context = context;
            _userManager = userManager;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            //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
            return Execute(Options.SendGridKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {

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
            msg.TrackingSettings = new TrackingSettings
            {
                ClickTracking = new ClickTracking { Enable = false }
            };

            return client.SendEmailAsync(msg);
        }




    }
}



public class AuthMessageSenderOptions
{
    public string SendGridUser { get; set; }
    public string SendGridKey { get; set; }
}

