using ASPNZBat.Models;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;
using Calendar = Ical.Net.Calendar;

namespace ASPNZBat.Business.ICal
{
    using System.Linq;
    using Data;
    using DTO;

    public class CalService : ICalService
    {
        //global calendar that newEvent gets added to
        private Calendar calendar = new Calendar(); //this is not nice deal with it.
        private readonly SeatBookingDBContext _context;
        private IDBCallsSessionDataDTO _dbCallsSessionDataDTO;
        private List<TimetableBooking> alltimetableBookings = new List<TimetableBooking>();

        public CalService(IDBCallsSessionDataDTO dbCallsSessionDataDTO, SeatBookingDBContext context)
        {
            _dbCallsSessionDataDTO = dbCallsSessionDataDTO;
            _context = context;
        }

        /// <summary>
        /// Set up a calender of events to be submitted as a string
        /// </summary>
        public string CalendarBooking(IEnumerable<SeatBooking> seatBooking, bool isEventController)
        {
            //https://stackoverflow.com/questions/52950884/ical-net-viewing-event-data-from-calendar-in-net-and-c-sharp

            //book all the seats
            return GetBookedSeats(seatBooking, isEventController);
        }

        /// <summary>
        /// Generate the session details for the new event
        /// </summary>
        /// <param name="sessions"></param>
        /// <param name="morningSessStart"></param>
        /// <param name="morningSessEnd"></param>
        /// <param name="afternoonSessStart"></param>
        /// <param name="afternoonSessEnd"></param>
        /// <param name="eveningSessStart"></param>
        /// <param name="eveningSessEnd"></param>
        /// <returns></returns>
        public static string[] SessionDetails(out string[] sessions, out TimeSpan morningSessStart, out TimeSpan morningSessEnd,
            out TimeSpan afternoonSessStart, out TimeSpan afternoonSessEnd, out TimeSpan eveningSessStart,
            out TimeSpan eveningSessEnd)
        {
            //I hate hardcoding, this should be abstracted out to an admin section

            //arrays of data
            string[] days = new[] { " Monday", " Tuesday", " Wednesday", " Thursday", " Friday" };
            sessions = new[] { "Morning 9am - 11:30am", "Afternoon 12pm - 2:30pm", "Evening 5:30pm - 8:30pm" };
            string[] allSessionTimeStart = new[] { "9:00:00", "12:00:01", "17:30:00" };
            string[] allSessionTimeEnd = new[] { "11:30:00", "14:30:01", "20:30:00" };

            //time of session to add in to calender from string to timespan
            morningSessStart = TimeSpan.Parse(allSessionTimeStart[0]);
            morningSessEnd = TimeSpan.Parse(allSessionTimeEnd[0]);
            afternoonSessStart = TimeSpan.Parse(allSessionTimeStart[1]);
            afternoonSessEnd = TimeSpan.Parse(allSessionTimeEnd[1]);
            eveningSessStart = TimeSpan.Parse(allSessionTimeStart[2]);
            eveningSessEnd = TimeSpan.Parse(allSessionTimeEnd[2]);
            return days;
        }

        /// <summary>
        /// Get the current logged in users seat bookings
        /// </summary>
        /// <param name="seatBookings">List of Seatbookings</param>
        /// <param name="isEventController">is it coming from the event controller</param>
        public string GetBookedSeats(IEnumerable<SeatBooking> seatBookings, bool isEventController)
        {
            var days = SessionDetails(out var sessions, out var morningSessStart, out var morningSessEnd, out var afternoonSessStart, out var afternoonSessEnd, out var eveningSessStart, out var eveningSessEnd);

            //loop through all the booked sessions and create a cal event
            foreach (var seats in seatBookings)
            {
                //create a calendar event for each booking. Shows details about
                if (seats != null && seats.S1)
                {
                    NewEvent(seats, 1, 0, morningSessStart, morningSessEnd, days[0], sessions[0]);
                }
                if (seats != null && seats.S2)
                {
                    NewEvent(seats, 2, 0, afternoonSessStart, afternoonSessEnd, days[0], sessions[1]);
                }
                if (seats != null && seats.S3)
                {
                    NewEvent(seats, 3, 0, eveningSessStart, eveningSessEnd, days[0], sessions[2]);
                }
                if (seats != null && seats.S4)
                {
                    NewEvent(seats, 4, 1, morningSessStart, morningSessEnd, days[1], sessions[0]);
                }
                if (seats != null && seats.S5)
                {
                    NewEvent(seats, 5, 1, afternoonSessStart, afternoonSessEnd, days[1], sessions[1]);
                }
                if (seats != null && seats.S6)
                {
                    NewEvent(seats, 6, 1, eveningSessStart, eveningSessEnd, days[1], sessions[2]);
                }
                if (seats != null && seats.S7)
                {
                    NewEvent(seats, 7, 2, morningSessStart, morningSessEnd, days[2], sessions[0]);
                }
                if (seats != null && seats.S8)
                {
                    NewEvent(seats, 8, 2, afternoonSessStart, afternoonSessEnd, days[2], sessions[1]);
                }
                if (seats != null && seats.S9)
                {
                    NewEvent(seats, 9, 2, eveningSessStart, eveningSessEnd, days[2], sessions[2]);
                }
                if (seats != null && seats.S10)
                {
                    NewEvent(seats, 10, 3, morningSessStart, morningSessEnd, days[3], sessions[0]);
                }
                if (seats != null && seats.S11)
                {
                    NewEvent(seats, 11, 3, afternoonSessStart, afternoonSessEnd, days[3], sessions[1]);
                }
                if (seats != null && seats.S12)
                {
                    NewEvent(seats, 12, 3, eveningSessStart, eveningSessEnd, days[3], sessions[2]);
                }
                if (seats != null && seats.S13)
                {
                    NewEvent(seats, 13, 4, morningSessStart, morningSessEnd, days[4], sessions[0]);
                }
                if (seats != null && seats.S14)
                {
                    NewEvent(seats, 14, 4, afternoonSessStart, afternoonSessEnd, days[4], sessions[1]);
                }
                if (seats != null && seats.S15)
                {
                    NewEvent(seats, 15, 4, eveningSessStart, eveningSessEnd, days[4], sessions[2]);
                }
            }
            //if we are outputting to the Events controller 
            if (isEventController)
            {
                _dbCallsSessionDataDTO.SeatBookingsOutputToIndex = OutputEventsToIndex(calendar);
                return null;
            }
            //  otherwise we are outputting to the calender
            _dbCallsSessionDataDTO.SessionAndNames = timetableBookingsToStudentController(alltimetableBookings);

            // CalOutput(calendar); //for Unit test
            return OutputEvents(calendar);
        }


        public Calendar CalOutput()
        {
            //just getting a calendar for the Unit Test
            return calendar;
        }
        /// <summary>
        /// Generate an event from the Seat number
        /// </summary>
        private void NewEvent(SeatBooking seats, int sessionID, int dayNext, TimeSpan morningSessStart, TimeSpan morningSessEnd, string day, string session)
        {
            //add the time of the sessions to the date to get DateTime
            DateTime sessionStart = seats.SeatDate.AddDays(dayNext).Add(morningSessStart);
            DateTime sessionEnd = seats.SeatDate.AddDays(dayNext).Add(morningSessEnd);
            string description = day + session;
            string seatId = sessionID.ToString();
            string sessID = seats.ID.ToString();

            string[] descArray = { day, session };


            //todo create a new class to pass all the bookings to so that the variable names actually match the data
            //Create a new event with event details
            var vEvent = new CalendarEvent
            {
                Start = new CalDateTime(sessionStart),
                End = new CalDateTime(sessionEnd),
                Description = description,
                Class = seatId, //session ID
                Summary = sessID, //the DB id of the week booking - use it to generate the click event for edit
                Name = "NZBAT at Vision College",
                Categories = descArray
            };

            calendar.Events.Add(vEvent); //adding in the new events

            //create a list of timetable bookings
            var Timetable = new TimetableBooking
            {
                StudentEmail = seats.StudentEmail,
                StudentName = seats.Name,
                SeatDate = sessionStart,
                SessionName = seatId
            };

            alltimetableBookings.Add(Timetable);
        }

        /// <summary>
        ///Generate the outputs
        ///
        /// </summary>
        /// <returns> Returns a string that goes to the screen</returns>
        public string OutputEvents(Calendar cal)
        {
            //send it back to be sent by email
            _dbCallsSessionDataDTO.SeatBookingsCalOutputToEmail = cal;

            //send it back to output on the screen
            var sb = new StringBuilder();

            foreach (var email in cal.Events)
            {
                sb.AppendLine("From: " + email.DtStart + " To " + email.DtEnd);
                sb.AppendLine("Desc: " + email.Description + " By " + email.Name);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Send the data to the Index page for users to check their booking, need to tie it into the Session numbers
        /// </summary>
        /// <param name="cal"></param>
        /// <returns>Returns a list of data to be looped through on the index page</returns>
        public List<CalendarEvent> OutputEventsToIndex(Calendar cal)
        {
            IOrderedEnumerable<CalendarEvent> orderedCal = cal.Events
                .OrderByDescending(d => d.DtStart)
                .ThenBy(s => s.Class); //session ID

            //send it back to output on the screen
            var sessions = new List<CalendarEvent>();
            //foreach (var email in orderedCal)
            //{
            //    sessions.Add(email.Class + " "+  email.DtStart.Date.ToShortDateString() + " " + email.Description);
            //}

            return orderedCal.ToList();
        }

        public SortedDictionary<DateTime, List<string>> timetableBookingsToStudentController(List<TimetableBooking> timetableBookings)
        {
            //Key date session S1
            //Value Name Email


            SortedDictionary<DateTime, List<string>> SessionAndNames = new SortedDictionary<DateTime, List<string>>();

            foreach (var item in timetableBookings)
            {
                DateTime key = item.SeatDate;
                string value = item.StudentName + " - " + item.StudentEmail + " - " + item.SessionName;

                if (!SessionAndNames.ContainsKey(key))
                {
                    SessionAndNames.Add(key,
                         new List<string>
                         {
                             value
                             });
                }
                else
                {
                    SessionAndNames[key].Add(value);
                }
            }

            return SessionAndNames;

        }
    }
}