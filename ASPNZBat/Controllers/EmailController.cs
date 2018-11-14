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
        private readonly IOverdueStudents _overdueStudents;
        private readonly UserManager<IdentityUser> _userManager;
        private string CurrentUserEmail;
        private string CurrentUserName;


        public EmailController(
            IEmailSender emailSender,
            UserManager<IdentityUser> userManager,
            IOverdueStudents overdueStudents
            )
        {
            _overdueStudents = overdueStudents;
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

        public IActionResult TestEmail()
        {
            //  CurrentUserEmail = _userManager.GetUserId(User);  //GetCurrentUserAsync().Result.Email;
            CurrentUserName = _userManager.GetUserName(User);



            List<string> sender = new List<string>();
            foreach (string student in _overdueStudents.FindOverDueStudents())
            {
                //make sure you only send 1 to a student not heaps
                if (!sender.Contains(student))
                {
                    _emailSender.SendEmailAsync(student, "Update your Sessions", "You no longer have a session booked. Come back and make some.");
                    sender.Add(student);
                }

            }

            // _emailSender.SendEmailAsync(CurrentUserName, "Update your Sessions", "You no longer have a session booked. Come back and make some.");

            //empty sender in case it still contains names next time
            sender.Clear();
            return Ok("No Data");
            //   return View();

        }


    }
}