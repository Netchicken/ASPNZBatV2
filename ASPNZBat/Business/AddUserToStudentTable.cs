namespace ASPNZBat.Business
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Areas.Identity.Pages.Account;
    using Controllers;
    using Data;
    using DTO;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models;


    public class AddUserToStudentTable : IAddUserToStudentTable
    {
        private readonly SeatBookingDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AddUserToStudentTable(SeatBookingDBContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        /// <summary>
        /// Add users to the local DB NOTE this is mostly for Google, as it doesn't use the Register, it just logs in.
        /// </summary>
        /// <param name="Email">there will always be an email from the login</param>
        /// <param name="Name">There will only be a name if they login with Google. </param>
        /// <returns></returns>
        public void AddUserToStudentDB(string Email, string Name)
        {
            //check if there is a Name in the Students table
            string studentEmail = Email; // _userManager.GetUserName(_userManager.GetUserName(User));
            string studentName = Name;

            //Check if they in the DB
            var student = _context.Students.FirstOrDefault(m => m.Email == studentEmail);

            //No entry 
            if (student == null)
            {
                //No match lets add it in if we have the Student name from Google

                Students myStudent = new Students
                {
                    Name = studentName,
                    Email = studentEmail
                };
                _context.Add(myStudent);
                _context.SaveChangesAsync();
            }
        }

    }
}


//namespace ASPNZBat.Controllers
//{
//    using System.Linq;
//    using Models;

//    public class AddUserToStudentTable
//    {
//        private SeatBookingsController _seatBookingsController;

//        public AddUserToStudentTable(SeatBookingsController seatBookingsController)
//        {
//            _seatBookingsController = seatBookingsController;
//        }

//        private void AddUserToStudentTable()
//        {
//            //check if there is a Name in the Students table
//            string StudentEmail = _seatBookingsController._userManager.GetUserName(_seatBookingsController.User);
//            string StudentName = _seatBookingsController._studentNameDTO.StudentGoogleNameLogin;
//            var student = _seatBookingsController._context.Students.FirstOrDefault(m => m.Email == StudentEmail);

//            //No match lets add it in
//            if (string.IsNullOrEmpty(student?.Email) && !string.IsNullOrEmpty(StudentName))
//            {
//                Students myStudent = new Students();
//                myStudent.Name = StudentName;
//                myStudent.Email = _seatBookingsController._userManager.GetUserName(_seatBookingsController.User);
//                _seatBookingsController._context.Add(myStudent);
//                _seatBookingsController._context.SaveChangesAsync();
//            }
//        }
//    }
//}