using System;
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
        private readonly SignInManager<IdentityUser> _signInManager;
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

            //Show the next 4 weeks
            ViewData["ThisWeek"] = _sessions.GetNextFourWeeks();

            //Show data for the next 4 weeks
            ViewData["CountSessions"] = _sessions.StatsSummary();

            //Sessions that are visible or not
            ViewData["SessionVisible"] = _sessions.GetAdminData();

            //  _addUserToStudentTable.AddUserToStudentTable();

            return View();
        }

        [Authorize]
        // POST: SeatBookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,SeatDate,S1,S2,S3,S4,S5,S6,S7,S8,S9,S10,S11,S12,S13,S14,S15")] SeatBooking seatBooking)
        {

            //Show data for the next 4 weeks
            ViewData["CheckFullSession"] = _sessions.GetSingleWeekStats(seatBooking.SeatDate);

            //   CalService myCalService = new CalService();
            //   myCalService.seatBooking = seatBooking;
            //Calendar allevents = myCalService.testBooking(seatBooking);

            if (ModelState.IsValid)
            {
                //todo get session with the same date
                //  string SeatDateNew = seatBooking.SeatDate.ToLongDateString();
                //todo UPDATE them if they exist
                //   var DeleteSeatBooking = await _context.SeatBooking.FindAsync();
                //  _context.SeatBooking.Remove(DeleteSeatBooking);

                //Save locally for sending to calendar
                _dbCallsSessionDataDTO.lastSeatBooking.Add(seatBooking);

                seatBooking.StudentEmail = _userManager.GetUserName(User);

                _context.Add(seatBooking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(seatBooking);
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,SeatDate,S1,S2,S3,S4,S5,S6,S7,S8,S9,S10,S11,S12,S13,S14,S15, StudentEmail")] SeatBooking seatBooking)
        {
            //get the date of last month
            DateTime LastMonth = DateTime.Today;
            LastMonth.AddMonths(-1);

            //if the date is older than lastmonth - catches when the date isn't being carried across
            if (id != seatBooking.ID && seatBooking.SeatDate < LastMonth && seatBooking.StudentEmail != string.Empty)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
