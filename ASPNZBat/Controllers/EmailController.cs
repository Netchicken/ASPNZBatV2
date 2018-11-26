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
    using System.Runtime.Serialization;
    using Business.ICal;
    using Data;
    using DTO;
    using Ical.Net;
    using Ical.Net.Serialization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using SendGrid;

    public class EmailController : Controller
    {
        private readonly ILogger _logger;
        private ICalService _calService;
        private IDBCallsSessionData _dbCallsSessionData;
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
            IOverdueStudents overdueStudents, IDBCallsSessionData dbCallsSessionData, ICalService calService, ILogger<EmailController> logger)
        {
            _context = context;
            _overdueStudents = overdueStudents;
            _dbCallsSessionData = dbCallsSessionData;
            _emailSender = emailSender;
            _userManager = userManager;
            _calService = calService;
            _logger = logger;
        }

        //Gets the current user
        [Authorize]
        private Task<IdentityUser> GetCurrentUserAsync()
        {
            // _userManager.GetUserId(User)
            // var User =  _userManager.GetUserAsync(HttpContext.User);
            return _userManager.FindByIdAsync(_userManager.GetUserId(User)); //_userManager.GetUserAsync(User);
        }

        public Task<IActionResult> TestEmail()
        {
            //  CurrentUserEmail = _userManager.GetUserId(User);  //GetCurrentUserAsync().Result.Email;
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

            // _emailSender.SendEmailAsync(CurrentUserName, "Update your Sessions", "You no longer have a session booked. Come back and make some.");

            //  return LogEmailsSent(sender);
        }

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

        public IActionResult TestCalendar()
        {

            //get the email
            CurrentUserName = _userManager.GetUserName(User);

            //https://stackoverflow.com/questions/46985663/use-ical-net-to-send-meeting-invite-for-microsoft-outlook
            //https://github.com/rianjs/ical.net/wiki

            string allevents = "";
            //get back all the calander events

            var seatBooking = _dbCallsSessionData.lastSeatBooking;
            //calendar as string to be outputted to screen
            allevents += _calService.testBooking(seatBooking);

            Calendar cal = new Calendar();
            cal = _dbCallsSessionData.SeatBookingsCalOutputToEmail;


            var serializer = new CalendarSerializer();
            foreach (var evCalEvent in cal.Events)
            {
                var serializedCalendar = serializer.SerializeToString(evCalEvent);
                _emailSender.SendEmailAsync(CurrentUserName, "NZBat Booking", serializedCalendar);
                //outputted to email and saved to show on screen
                allevents += serializedCalendar;
            }

            return Ok(allevents);
        }
    }
}