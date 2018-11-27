using ASPNZBat.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASPNZBat.Controllers
{
    using Business.ICal;
    using Data;
    using DTO;
    using Microsoft.Extensions.Logging;

    public class EmailController : Controller
    {
        private IGenerateCalendarEventsForControllers _generateCalendarEventsForControllers;
        private readonly ILogger _logger;
        private ICalService _calService;
        private IDBCallsSessionDataDTO _dbCallsSessionData;
        private readonly SeatBookingDBContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IOverdueStudents _overdueStudents;
        private readonly UserManager<IdentityUser> _userManager;
        private string CurrentUserEmail;
        private string CurrentUserName;

        public EmailController(
            SeatBookingDBContext context,
            IEmailSender emailSender,
            UserManager<IdentityUser> userManager,
            IOverdueStudents overdueStudents,
            IDBCallsSessionDataDTO dbCallsSessionData,
            ICalService calService,
            ILogger<EmailController> logger,
            IGenerateCalendarEventsForControllers generateCalendarEventsForControllers)
        {
            _context = context;
            _overdueStudents = overdueStudents;
            _dbCallsSessionData = dbCallsSessionData;
            _emailSender = emailSender;
            _userManager = userManager;
            _calService = calService;
            _logger = logger;
            _generateCalendarEventsForControllers = generateCalendarEventsForControllers;
        }

        //Gets the current user
        [Authorize]
        private Task<IdentityUser> GetCurrentUserAsync()
        {
            // _userManager.GetUserId(User)
            // var User =  _userManager.GetUserAsync(HttpContext.User);
            return _userManager.FindByIdAsync(_userManager.GetUserId(User));
        }

        /// <summary>
        /// Send Emails to overdue students - students who havn't a current booking
        /// </summary>

        public Task<IActionResult> TestEmail()
        {
            CurrentUserName = _userManager.GetUserName(User);

            _logger.LogInformation("TestEmail method username = {name}", CurrentUserName);

            List<string> sender = new List<string>();
            foreach (string student in _overdueStudents.FindOverDueStudents(_overdueStudents.StudentsWithCurrentSchedules(), _overdueStudents.AllStudents()))
            {
                //make sure you only send 1 to a student not heaps
                if (!sender.Contains(student))
                {
                    _emailSender.SendEmailAsync(student, "Update your Sessions",
                            "You no longer have a session booked. Come back and make some.");
                    //add the user to the dict so it doesn't go again.
                    sender.Add(student);
                }
            }

            var log = LogEmailsSent(sender);
            return Task.FromResult<IActionResult>(log);
        }

        /// <summary>
        /// Log the emails to the screen for testing
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        private IActionResult LogEmailsSent(List<string> sender)
        {
            //   I want to see who the emails went to.
            string StudentNames = "";

            if (sender.Count == 0) //no emails sent
            {
                _logger.LogInformation("No Emails sent");

                return Ok("No Emails sent");
            }
            else
            {
                foreach (var student in sender)
                {
                    StudentNames += student + " ";
                }

                //empty sender in case it still contains names next time
                sender.Clear();
                _logger.LogInformation("Emails sent to  {Message}", StudentNames);
                return Ok("Emails sent to " + StudentNames);
            }
        }

        /// <summary>
        /// Create Calendar bookings to send out to people
        /// </summary>
        public IActionResult TestCalendar()
        {
            //get the email of the current user to send to
            CurrentUserName = _userManager.GetUserName(User);

            var allevents = _generateCalendarEventsForControllers.CalendarEventsForEmail(CurrentUserName);

            return Ok(allevents);
        }
    }
}