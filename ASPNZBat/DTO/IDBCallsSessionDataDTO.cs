using ASPNZBat.Models;

namespace ASPNZBat.DTO
{
    using System.Collections.Generic;
    using Ical.Net;
    using Ical.Net.CalendarComponents;

    public interface IDBCallsSessionDataDTO
    {
        List<SeatBooking> lastSeatBooking { get; set; }
        Calendar SeatBookingsCalOutputToEmail { get; set; }
        List<CalendarEvent> SeatBookingsOutputToIndex { get; set; }
    }
}