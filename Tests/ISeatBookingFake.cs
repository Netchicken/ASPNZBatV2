using System.Collections.Generic;
using ASPNZBat.Models;

namespace Tests
{
    interface ISeatBookingFake
    {
        IEnumerable<SeatBooking> GetAllSeatBookings();
    }
}