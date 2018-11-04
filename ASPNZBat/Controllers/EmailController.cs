using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ASPNZBat.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASPNZBat.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;
        private string CurrentUserEmail;
        private string CurrentUserName;


        public EmailController(IEmailSender emailSender, UserManager<IdentityUser> userManager)
        {
            _emailSender = emailSender;
            _userManager = userManager;

        }

        //Gets the current user
        [Authorize]
        private Task<IdentityUser> GetCurrentUserAsync()
        {   // _userManager.GetUserId(User)
            // var User =  _userManager.GetUserAsync(HttpContext.User);
            return _userManager.FindByIdAsync(_userManager.GetUserId(User)); //_userManager.GetUserAsync(User);
        }

        public void TestEmail()
        {
            //  CurrentUserEmail = _userManager.GetUserId(User);  //GetCurrentUserAsync().Result.Email;
            CurrentUserName = _userManager.GetUserName(User); //GetCurrentUserAsync().Result.UserName;

            _emailSender.SendEmailAsync(CurrentUserName, "Test", "Test Message");
            //   return View();
        }
    }
}