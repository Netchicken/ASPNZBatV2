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
    using Microsoft.AspNetCore.Identity;

    public class StudentsController : Controller
    {
        private readonly SeatBookingDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public StudentsController(SeatBookingDBContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            //get all entries from today
            DateTime today = DateTime.Today;
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



        // GET: Students
        public ViewResult Timetable()
        {
            //get all entries from today
            DateTime today = DateTime.Today;
            IEnumerable<SeatBooking> _allBookingsFromToday = _context.SeatBooking
                .Where(s => s.SeatDate > today)
                .OrderByDescending(s => s.SeatDate)
                .ToList();

            string[] StudentsInSessions = new string[15];

            foreach (var session in _allBookingsFromToday)
            {
                bool[] sess = new bool[16] { false, session.S1, session.S2, session.S3, session.S4, session.S5, session.S6, session.S7, session.S8, session.S9, session.S10, session.S11, session.S12, session.S13, session.S14, session.S15 };

                for (int i = 1; i < 16; i++)
                {

                    if (sess[i] == true)
                    {
                        StudentsInSessions[i] += session.Name + " *";
                    }
                }
            }


            return View(StudentsInSessions);
        }





    }
}
