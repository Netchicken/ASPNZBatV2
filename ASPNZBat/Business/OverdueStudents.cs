using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNZBat.Data;
using Microsoft.AspNetCore.Identity;

namespace ASPNZBat.Business
{
    public class OverdueStudents
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




        public string FindOverDueStudents()
        {

            return "aa";
        }

    }
}
