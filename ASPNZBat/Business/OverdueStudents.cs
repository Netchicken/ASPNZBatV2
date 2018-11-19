using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNZBat.Data;
using Microsoft.AspNetCore.Identity;

namespace ASPNZBat.Business
{
    public class OverdueStudents : IOverdueStudents
    {
        private readonly SeatBookingDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OverdueStudents(
            SeatBookingDBContext context,
            UserManager<IdentityUser> userManager
        )
        {
            _context = context;
            _userManager = userManager;
        }



        /// <summary>
        /// Get the student emails where they haven't got a current schedule
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> FindOverDueStudents()
        {
            //get all students who do have a schedule after today + 7 days 
            var studentsWithCurrentSchedules = _context.SeatBooking
                .Where(s => s.SeatDate > DateTime.Today.AddDays(7))
                .Select(s => s.StudentEmail.Distinct()).ToList();

            //get all students in total
            var allStudents = _context.SeatBooking
              .Select(s => s.StudentEmail).ToList();

            //subtract out the ones who have a schedule after today
            var noScheduleStudents = new List<string>();
            foreach (var student in allStudents)
            {
                //if the student is not in the list of current students
                if (!studentsWithCurrentSchedules.Contains(student))
                {//add it to the list of No Schedule Students
                    noScheduleStudents.Add(student);
                }
            }
            return noScheduleStudents;
        }


    }
}
