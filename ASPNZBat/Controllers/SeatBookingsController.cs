using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ASPNZBat.Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPNZBat.Data;
using ASPNZBat.DTO;
using ASPNZBat.Migrations;
using ASPNZBat.Models;
using ASPNZBat.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ASPNZBat.Controllers
{
    public class SeatBookingsController : Controller
    {
        private readonly SeatBookingDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public ISessions _sessions { get; }



        public SeatBookingsController(
            SeatBookingDBContext context,
           UserManager<IdentityUser> userManager,
            ISessions Sessions
            )
        {
            _context = context;
            _userManager = userManager;
            _sessions = Sessions;


        }




        // GET: SeatBookings
        public async Task<IActionResult> Index()
        {
            DateTime LastMonth = DateTime.Today;
            LastMonth = LastMonth.AddMonths(-1);

            return View(await _context.SeatBooking
                .Where(s => s.StudentEmail == _userManager.GetUserId(User) && s.SeatDate > LastMonth)
             .OrderByDescending(s => s.SeatDate)
              .ToListAsync());



            //var result = data.Where(d => d.Type == "Deposit")
            //.GroupBy(groupByClause)
            //.Select(g => new {
            //    DateKey = g.Key,
            //    TotalDepositAmount = g.Sum(d => d.Amount),
            //    DepositCount = g.Count(),
            //});



            //return View(await _context.SeatBooking
            //    .Where(s => s.StudentEmail == _userManager.GetUserId(User))
            //   .Select(s => new { s.SeatDate, s.S1, s.S2, s.S3, s.S4, s.S5, s.S6, s.S7, s.S8, s.S9, s.S10, s.S11, s.S12, s.S13, s.S14, s.S15 })
            //    .OrderByDescending(s => s.SeatDate)
            //    .ToListAsync());



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

            //todo make checkfullsession itereate through seatbooking.If seatbooking checked == checkfullSession 5 then cancel out with a message

            //SummaryStatsVM sb = new SummaryStatsVM();
            //sb = ConvertBoolToIntSession.ConvertToInt(seatBooking);




            //foreach (var session in sb)
            //{

            //}



            if (ModelState.IsValid)
            {
                seatBooking.StudentEmail = _userManager.GetUserId(User);

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
        public async Task<IActionResult> Edit(int id, [Bind("ID,SeatDate,S1,S2,S3,S4,S5,S6,S7,S8,S9,S10,S11,S12,S13,S14,S15")] SeatBooking seatBooking)
        {
            if (id != seatBooking.ID)
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
