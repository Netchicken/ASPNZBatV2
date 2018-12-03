using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNZBat.Business.ICal
{
    using DTO;
    using Ical.Net;
    using Ical.Net.Serialization;
    using Models;

    public class GenerateCalendarEventsForControllers : IGenerateCalendarEventsForControllers
    {
        private IEmailSender _emailSender;
        private ICalService _calService;
        private IDBCallsSessionDataDTO _dbCallsSessionData;
        public GenerateCalendarEventsForControllers(IDBCallsSessionDataDTO dbCallsSessionData, ICalService calService, IEmailSender emailSender)
        {
            _dbCallsSessionData = dbCallsSessionData;
            _calService = calService;
            _emailSender = emailSender;
        }

        /// <summary>
        /// Events sent back as calendar events string
        /// </summary>
        /// <param name="CurrentUserName"></param>
        /// <returns></returns>
        public string CalendarEventsForEmail(string CurrentUserName)
        {
            //https://stackoverflow.com/questions/46985663/use-ical-net-to-send-meeting-invite-for-microsoft-outlook
            //https://github.com/rianjs/ical.net/wiki

            string allevents = "";
            //get back all the calander events

            var seatBooking = _dbCallsSessionData.lastSeatBooking;
            //calendar as string to be outputted to screen
            allevents += _calService.CalendarBooking(seatBooking, false);

            Calendar cal = new Calendar();
            cal = _dbCallsSessionData.SeatBookingsCalOutputToEmail;


            var serializer = new CalendarSerializer();
            foreach (var evCalEvent in cal.Events)
            {
                var serializedCalendar = serializer.SerializeToString(evCalEvent);
                _emailSender.SendEmailAsync(CurrentUserName, "NZBat Booking", serializedCalendar);
                //outputted to email and saved to show on screen
                allevents += serializedCalendar;
            }

            return allevents;
        }
        /// <summary>
        /// Generate the calendar events for the last month
        /// Probably NOT USED
        /// </summary>
        /// <param name="currentUserName"></param>
        /// <param name="bookingsLastMonth"></param>
        /// <returns></returns>
        public string GenerateCalendarEventsForSeatBookings(string currentUserName, IEnumerable<SeatBooking> bookingsLastMonth)
        {
            //get back all the calender events for the last month
            string allevents = null;
            //pass in all the booking events.
            var seatBooking = bookingsLastMonth;
            //calendar as string to be outputted to screen
            allevents += _calService.CalendarBooking(seatBooking, true);

            Calendar cal = new Calendar();
            cal = _dbCallsSessionData.SeatBookingsCalOutputToEmail;


            var serializer = new CalendarSerializer();
            foreach (var evCalEvent in cal.Events)
            {
                var serializedCalendar = serializer.SerializeToString(evCalEvent);
                _emailSender.SendEmailAsync(currentUserName, "NZBat Booking", serializedCalendar);
                //outputted to email and saved to show on screen
                allevents += serializedCalendar;
            }

            return allevents;
        }

    }
}
