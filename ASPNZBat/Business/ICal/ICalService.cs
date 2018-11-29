using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;


namespace ASPNZBat.Business.ICal
{
    using Ical.Net;
    using Ical.Net.CalendarComponents;
    using Models;

    public interface ICalService
    {
        string CalendarBooking(IEnumerable<SeatBooking> seatBooking, bool isEventController);
        List<CalendarEvent> OutputEventsToIndex(Calendar cal);
        string GetBookedSeats(IEnumerable<SeatBooking> seatBookings, bool isEventController);
    }
}
