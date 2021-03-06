﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ASPNZBat.Business;
using ASPNZBat.Business.ICal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPNZBat.Data;
using ASPNZBat.DTO;
using ASPNZBat.Migrations;
using ASPNZBat.Models;
using ASPNZBat.ViewModels;
using Ical.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ASPNZBat.Controllers
{
    public class SeatBookingsController : Controller
    {
        private IStudentNameDTO _studentNameDTO;
        private IGenerateCalendarEventsForControllers _generateCalendarEventsForControllers;
        private ICalService _calService;
        private IDBCallsSessionDataDTO _dbCallsSessionDataDTO;
        private readonly SeatBookingDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        //private readonly SignInManager<IdentityUser> _signInManager;
        //  private readonly AddUserToStudentTable _addUserToStudentTable;
        public ISessions _sessions { get; }



        public SeatBookingsController(
            SeatBookingDBContext context,
           UserManager<IdentityUser> userManager,
            ISessions sessions,
            IDBCallsSessionDataDTO dbCallsSessionData,
            ICalService calService, IGenerateCalendarEventsForControllers generateCalendarEventsForControllers, IStudentNameDTO studentNameDTO)
        {
            _context = context;
            _userManager = userManager;
            _sessions = sessions;
            _dbCallsSessionDataDTO = dbCallsSessionData;
            _calService = calService;
            _generateCalendarEventsForControllers = generateCalendarEventsForControllers;
            _studentNameDTO = studentNameDTO;
            //   _addUserToStudentTable = new AddUserToStudentTable(this);
        }




        // GET: SeatBookings
        public ViewResult Index()
        {

            //get the date of last month
            DateTime LastMonth = DateTime.Today;

            //todo change this to after today, they don't want to know old bookings. - or last week so they can see what they made comment out next line

            LastMonth = LastMonth.AddMonths(-1);

            //return the bookings for that user from that date to today
            var lastMonthbookings = _context.SeatBooking
                .Where(s => s.StudentEmail == _userManager.GetUserName(User) && s.SeatDate > LastMonth)
                .OrderByDescending(s => s.SeatDate)
                .ToList();

            //Get the current user
            //  var user = _userManager.GetUserName(User);

            //runs the method that generates the calender entries, but we don't want the return only the second line below.
            var IgnoreThisOutput = _calService.GetBookedSeats(lastMonthbookings, true);
            ViewData["BookedSessions"] = _dbCallsSessionDataDTO.SeatBookingsOutputToIndex;

            //     _generateCalendarEventsForControllers.GenerateCalendarEventsForSeatBookings(user, lastMonthbookings);

            //Sessions that are visible or not
            ViewData["SessionVisible"] = _sessions.GetAdminData();
            return View(lastMonthbookings);
        }


        [Authorize]
        // GET: SeatBookings/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seatBooking = _context.SeatBooking
                .Where(m => m.ID == id)
                .OrderByDescending(s => s.SeatDate);

            if (seatBooking == null)
            {
                return NotFound();
            }

            return View((SeatBooking)seatBooking);
        }

        [Authorize]
        // GET: SeatBookings/SessionDetails
        public IActionResult SessionDetails()
        {
            var seatBooking = _context.SeatBooking
                .OrderByDescending(s => s.SeatDate);
            return View(seatBooking);
        }


        // GET: SeatBookings/Create
        public IActionResult Create()
        {
            //return the future bookings for that user so we can stop repeated bookings to same date by deactivating radiobuttons for same booking

            List<DateTime> Bookeddate = new List<DateTime>();

            if (User != null)
            {
                Bookeddate = _context.SeatBooking
                             .Where(s => s.StudentEmail == _userManager.GetUserName(User) && s.SeatDate > DateTime.Today)
                              .Select(s => s.SeatDate)
                             //.OrderByDescending(s => s.SeatDate)
                             .ToList();

                ViewData["FutureSeatBookings"] = Bookeddate;
            }
            else
            {
                ViewData["FutureSeatBookings"] = null;
            }




            //get all the fture bookings for the user - to hide ones already made We could cut them back in sessions but I want to show them greyed out so users can then edit them.


            //Show the next 4 weeks

            List<DateTime> nextFourWeeksDate = new List<DateTime>();
            nextFourWeeksDate = _sessions.GetNextFourWeeks().ToList();
            ViewData["BookingsRemoved"] = "";

            //if there are any bookings - ie: they are logged in. 
            if (Bookeddate.Any())
            {

                //if there is already a booking for that date then remove it, so only show dates with no booking.
                for (int i = 0; i < nextFourWeeksDate.Count; i++)
                {
                    if (nextFourWeeksDate.Contains(Bookeddate[i]))
                    {
                        nextFourWeeksDate.Remove(Bookeddate[i]);
                        ViewData["BookingsRemoved"] = "Please use Edit to change existing Bookings";
                    }
                }
            }




            ViewData["ThisWeek"] = nextFourWeeksDate;

            //Show data for the next 4 weeks
            ViewData["CountSessions"] = _sessions.StatsSummary();

            //Sessions that are visible or not
            ViewData["SessionVisible"] = _sessions.GetAdminData();

            //  _addUserToStudentTable.AddUserToStudentTable();
            var settings = _context.SiteSettings.FirstOrDefault();
            ViewData["maxseats"] = settings.maxSeats;
            ViewData["nearlyFullSeats"] = settings.nearlyFullSeats;
            ViewData["plentySeats"] = settings.plentySeats;

            return View();
        }

        [Authorize]
        // POST: SeatBookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,SeatDate,S1,S2,S3,S4,S5,S6,S7,S8,S9,S10,S11,S12,S13,S14,S15 ")] SeatBooking seatBooking)
        {
            //Show data for the next 4 weeks
            ViewData["CheckFullSession"] = _sessions.GetSingleWeekStats(seatBooking.SeatDate);

            if (ModelState.IsValid)
            {

                //Save locally for sending to calendar in GenerateCalendarEvents
                _dbCallsSessionDataDTO.AllSeatBookings.Add(seatBooking);

                //get the user email
                seatBooking.StudentEmail = _userManager.GetUserName(User);

                //get the user name
                var name = _context.Students.Where(s => s.Email == seatBooking.StudentEmail).Select(s => s.Name).FirstOrDefault();

                //add to the booking
                seatBooking.Name = name.ToString();

                // get session with the same date and same email
                // UPDATE them if they exist
                //var matchSeatBooking = _context.SeatBooking.FirstOrDefault(s => s.SeatDate == seatBooking.SeatDate && s.StudentEmail == _userManager.GetUserName(User));

                //if (matchSeatBooking != null)
                //{
                //    //just need the id of the saved entry
                //    seatBooking.ID = matchSeatBooking.ID;
                //    matchSeatBooking.ID = 10000; //need to get rid of the other id throws an error
                //    //update the existing record
                //    _context.Update(seatBooking);
                //}
                //else
                //{//create a new record
                //    _context.Add(seatBooking);
                //}

                _context.Add(seatBooking);

                await _context.SaveChangesAsync();

                //delete out old session booking to keep db small
                await DeleteOldSessionBookings();

                return RedirectToAction(nameof(Index));
            }
            return View(seatBooking);
        }
        /// <summary>
        /// Delete out sessions over a month old. To keep the DB small (its only sqlite) 
        /// </summary>
        /// <returns></returns>
        public async Task DeleteOldSessionBookings()
        {
            //delete out old bookings get the date of last month
            DateTime LastMonth = DateTime.Today;

            LastMonth = LastMonth.AddMonths(-1);
            //get all records from over a month old. We could delete ALL old sessions, not just users.
            // var deleteSeatBooking = _context.SeatBooking
            //     .Where(s => s.StudentEmail == _userManager.GetUserName(User) && s.SeatDate < LastMonth)
            //     .ToList();

            var deleteSeatBooking = _context.SeatBooking
                .Where(s => s.SeatDate < LastMonth)
                .ToList();
            //delete them
            if (deleteSeatBooking.Count > 0)
            {
                foreach (var booking in deleteSeatBooking) _context.SeatBooking.Remove(booking);
                await _context.SaveChangesAsync();
            }

        }

        // GET: SeatBookings/Edit/5
        [Authorize]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Sessions that are visible or not
            ViewData["SessionVisible"] = _sessions.GetAdminData();

            var seatBooking = _context.SeatBooking
                .Where(b => b.ID == id)
               //.FindAsync(id)
               .OrderByDescending(s => s.SeatDate)
                .FirstOrDefault();

            if (seatBooking == null)
            {
                return NotFound();
            }
            return View(seatBooking);
        }

        // POST: SeatBookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("ID,SeatDate,S1,S2,S3,S4,S5,S6,S7,S8,S9,S10,S11,S12,S13,S14,S15, StudentEmail, Name")] SeatBooking seatBooking)
        {
            //get the date of last month
            DateTime LastMonth = DateTime.Today;
            LastMonth.AddMonths(-1);

            //if the date is older than lastmonth - catches when the date isn't being carried across
            if (id != seatBooking.ID && seatBooking.SeatDate < LastMonth && seatBooking.StudentEmail != string.Empty)
            {
                return NotFound();
            }
            //only run if there is an email address
            if (ModelState.IsValid && !string.IsNullOrEmpty(seatBooking.StudentEmail))
            {
                try
                {
                    Students name = _context.Students.FirstOrDefault(s => s.Email == seatBooking.StudentEmail);

                    seatBooking.Name = name.Name;

                    _context.Update(seatBooking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeatBookingExists(seatBooking.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(seatBooking);
        }

        // GET: SeatBookings/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seatBooking = await _context.SeatBooking
                .FirstOrDefaultAsync(m => m.ID == id);
            if (seatBooking == null)
            {
                return NotFound();
            }

            return View(seatBooking);
        }

        // POST: SeatBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seatBooking = await _context.SeatBooking.FindAsync(id);
            _context.SeatBooking.Remove(seatBooking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeatBookingExists(int id)
        {
            return _context.SeatBooking.Any(e => e.ID == id);
        }
    }
}
