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

        private IStudentNameDTO _studentNameDTO;
        private readonly SeatBookingDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AddUserToStudentTable(SeatBookingDBContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        public bool AddUserToStudent(string Email)
        {
            //check if there is a Name in the Students table
            string StudentEmail = Email; // _userManager.GetUserName(_userManager.GetUserName(User));
            string StudentName = string.Empty;

            if (_studentNameDTO.IsExternal == true)
            {
                StudentName = _studentNameDTO.StudentGoogleNameLogin;
            }
            var student = _context.Students.FirstOrDefault(m => m.Email == StudentEmail);

            //No match lets add it in if we have the Student name from Google
            if (string.IsNullOrEmpty(student?.Email) && !string.IsNullOrEmpty(StudentName))
            {
                Students myStudent = new Students();
                myStudent.Name = StudentName;
                myStudent.Email = StudentEmail; //_userManager.GetUserName(_seatBookingsController.User);
                _context.Add(myStudent);
                _context.SaveChangesAsync();

                return true;
            }


            //todo otherwise we have to open the student create window and insert the username.

            return false;



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