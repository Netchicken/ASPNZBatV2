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
        private readonly ApplicationDbContext _adminContext;
        private readonly SeatBookingDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OverdueStudents(
            SeatBookingDBContext context,
            UserManager<IdentityUser> userManager
, ApplicationDbContext adminContext)
        {
            _context = context;
            _userManager = userManager;
            _adminContext = adminContext;
        }


        /// <summary>
        /// Get the student emails where they haven't got a current schedule
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> FindOverDueStudents(IEnumerable<string> studentsWithCurrentSchedules, IEnumerable<string> allStudents)
        {

            //subtract out the ones who have a schedule after today
            var noScheduleStudents = new List<string>();
            foreach (var student in allStudents)
            {
                //if the student is not in the list of current students with schedules and its not already in the list - seems to be multiples of the same
                if (!studentsWithCurrentSchedules.Contains(student) && !noScheduleStudents.Contains(student))
                {//add it to the list of No Schedule Students
                    noScheduleStudents.Add(student);
                }
            }

            return noScheduleStudents;
        }
        /// <summary>
        /// Get all student emails
        /// </summary>
        /// <returns>list of all students ever student emails</returns>
        public List<string> AllStudents()
        {
            //todo we need to be able to filter students so that we don't return all students ever. Either have the ability to cull them by admin, or by date. 
            return _adminContext.Users.Select(s => s.UserName).Distinct().ToList();
        }
        /// <summary>
        /// get all students who do have a schedule after today + 7 days 
        /// </summary>
        /// <returns></returns>
        public List<string> StudentsWithCurrentSchedules()
        {
            //using Distinct means that it has to compare each entry, and to do that it has to convert the email address to a char. So each email is a series of chars, all together in a list of emails
            return _context.SeatBooking
                    .Where(s => s.SeatDate > DateTime.Today.AddDays(7))
                    .Select(s => s.StudentEmail).Distinct().ToList();

        }
    }
}
