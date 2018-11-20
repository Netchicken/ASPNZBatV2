using ASPNZBat.Models;

namespace ASPNZBat.DTO
{
    using System.Collections.Generic;
    using Ical.Net;

    public interface IDBCallsSessionData
    {
        List<SeatBooking> lastSeatBooking { get; set; }
        Calendar SeatBookingsCalOutputToEmail { get; set; }
    }
}