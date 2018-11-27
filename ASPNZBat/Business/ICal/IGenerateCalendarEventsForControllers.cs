namespace ASPNZBat.Business.ICal
{
    using System.Collections.Generic;
    using Models;

    public interface IGenerateCalendarEventsForControllers
    {
        string CalendarEventsForEmail(string CurrentUserName);

        string GenerateCalendarEventsForSeatBookings(string CurrentUserName, IEnumerable<SeatBooking> BookingsLastMonth);
    }
}