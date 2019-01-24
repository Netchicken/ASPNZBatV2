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
        //Save the bookings locally for sending to calendar
        List<SeatBooking> AllSeatBookings { get; set; }
        Calendar SeatBookingsCalOutputToEmail { get; set; }
        List<CalendarEvent> SeatBookingsOutputToIndex { get; set; }
        SortedDictionary<DateTime, List<string>> SessionAndNames { get; set; }
        SortedDictionary<DateTime, List<string>> TimeTable { get; set; }
    }
}