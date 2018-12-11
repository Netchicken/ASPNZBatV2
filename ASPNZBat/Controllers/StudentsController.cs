using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPNZBat.Data;
using ASPNZBat.Models;

namespace ASPNZBat.Controllers
{
    using Business;
    using Business.ICal;
    using DTO;
    using Microsoft.AspNetCore.Identity;
    using ViewModels;

    public class StudentsController : Controller
    {
        private ICalService _calService;
        private IDBCallsSessionDataDTO _dbCallsSessionDataDTO;
        private readonly SeatBookingDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public StudentsController(SeatBookingDBContext context, UserManager<IdentityUser> userManager, ICalService calService, IDBCallsSessionDataDTO dbCallsSessionDataDTO)
        {
            _context = context;
            _userManager = userManager;
            _calService = calService;
            _dbCallsSessionDataDTO = dbCallsSessionDataDTO;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            //get all entries from today
            //  DateTime today = DateTime.Today;
            //IEnumerable<SeatBooking> _allBookingsFromToday = _context.SeatBooking
            //    .Where(s => s.StudentEmail == _userManager.GetUserName(User) && s.SeatDate > today)
            //    .OrderByDescending(s => s.SeatDate)
            //    .ToList();
            ////get all the student details
            //List<Students> students = new List<Students>();
            //students.AddRange(_context.Students);

            //add the Name from student details to the student sessions add to new class
            //    List<SessionStudentVM> sessionStudent = new List<SessionStudentVM>();

            //foreach (var session in _allBookingsFromToday)
            //{
            //    foreach (var student in students)
            //    {
            //        if (student.Email == session.StudentEmail)
            //        {
            //            sessionStudent.Add(



            //                    );  

            //                );


            //        }


            //    }



            //}


            return View(_context.Students);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var students = await _context.Students
                .FirstOrDefaultAsync(m => m.ID == id);
            if (students == null)
            {
                return NotFound();
            }

            return View(students);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] Students students)
        {
            if (ModelState.IsValid)
            {
                students.Email = _userManager.GetUserName(User);
                _context.Add(students);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(students);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var students = await _context.Students.FindAsync(id);
            if (students == null)
            {
                return NotFound();
            }
            return View(students);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Name,Email")] Students students)
        {
            if (id != students.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(students);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentsExists(students.ID))
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
            return View(students);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var students = await _context.Students
                .FirstOrDefaultAsync(m => m.ID == id);
            if (students == null)
            {
                return NotFound();
            }

            return View(students);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var students = await _context.Students.FindAsync(id);
            _context.Students.Remove(students);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentsExists(string id)
        {
            return _context.Students.Any(e => e.ID == id);
        }

        // show the timetabling details of which session all students attend
        // GET: Students
        public ViewResult Timetable()
        {
            //get all entries including today
            DateTime Yesterday = DateTime.Today.AddDays(-7);

            var _allBookingsFromYesterday = _context.SeatBooking
              .Where(s => s.SeatDate > Yesterday).ToList();


            //runs the method that generates the calender entries, but we don't want the return only the second line below.
            var IgnoreThisOutput = _calService.GetBookedSeats(_allBookingsFromYesterday, false);
            var result = _dbCallsSessionDataDTO.SessionAndNames;


            return View(result);
        }
    }
}

//.GroupBy(g => new { g.SeatDate })
//.Select(a => new TimetableVM()
//{
//    SeatDate = a.Key.SeatDate,
//    S1 = a.Count(s => s.S2),
//    // Name = (s => s.Name),
//    S2 = a.Count(s => s.S2),
//    S3 = a.Count(s => s.S3),
//    S4 = a.Count(s => s.S4),
//    S5 = a.Count(s => s.S5),
//    S6 = a.Count(s => s.S6),
//    S7 = a.Count(s => s.S7),
//    S8 = a.Count(s => s.S8),
//    S9 = a.Count(s => s.S9),
//    S10 = a.Count(s => s.S10),
//    S11 = a.Count(s => s.S11),
//    S12 = a.Count(s => s.S12),
//    S13 = a.Count(s => s.S13),
//    S14 = a.Count(s => s.S14),
//    S15 = a.Count(s => s.S15)
//}).ToList();
//string[] StudentsInSessions = new string[15];.ToList()

//foreach (var session in _allBookingsFromToday)
//{
//    bool[] sess = new bool[16] { false, session.S1, session.S2, session.S3, session.S4, session.S5, session.S6, session.S7, session.S8, session.S9, session.S10, session.S11, session.S12, session.S13, session.S14, session.S15 };

//    for (int i = 1; i < 16; i++)
//    {

//        if (sess[i] == true)
//        {
//            StudentsInSessions[i] += session.Name + "</br>";
//        }
//    }
//}
