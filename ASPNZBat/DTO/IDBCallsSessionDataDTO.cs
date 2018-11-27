using ASPNZBat.Models;

namespace ASPNZBat.DTO
{
    using System.Collections.Generic;
    using Ical.Net;

    public interface IDBCallsSessionDataDTO
    {
        List<SeatBooking> lastSeatBooking { get; set; }
        Calendar SeatBookingsCalOutputToEmail { get; set; }
        List<string> SeatBookingsOutputToIndex { get; set; }
    }
}