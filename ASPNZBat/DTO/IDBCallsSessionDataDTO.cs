using ASPNZBat.Models;

namespace ASPNZBat.DTO
{
    using System;
    using System.Collections.Generic;
    using Business;
    using Ical.Net;
    using Ical.Net.CalendarComponents;

    public interface IDBCallsSessionDataDTO
    {
        List<SeatBooking> lastSeatBooking { get; set; }
        Calendar SeatBookingsCalOutputToEmail { get; set; }
        List<CalendarEvent> SeatBookingsOutputToIndex { get; set; }
        SortedDictionary<DateTime, List<string>> SessionAndNames { get; set; }
        SortedDictionary<DateTime, List<string>> TimeTable { get; set; }
    }
}