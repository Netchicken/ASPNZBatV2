using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Ical.Net.Serialization;
using Ical.Net.DataTypes;
using Ical.Net.CalendarComponents;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Web.Http;
using ASPNZBat.Models;
using Calendar = Ical.Net.Calendar;

namespace ASPNZBat.Business.ICal
{
    public class CalService : ICalService
    {
        //  public SeatBooking seatBooking { get; set; }

        Calendar calendar = new Calendar();



        /// <summary>
        /// Set up a calender of events to be submitted to something
        /// </summary>
        /// <param name="booking"></param>
        public string testBooking(SeatBooking seatBooking)
        {
            //https://stackoverflow.com/questions/52950884/ical-net-viewing-event-data-from-calendar-in-net-and-c-sharp

            //book all the seats
            return GetBookedSeats(seatBooking);
            //   return calendar;
        }
        /// <summary>
        /// Get the current logged in users seat bookings
        /// </summary>
        /// <param name="seats"></param>
        private string GetBookedSeats(SeatBooking seats)
        {
            //I hate hardcoding, this should be abstracted out to an admin section

            //arrays of data
            string Description = null;
            string[] days = new[] { " Monday", " Tuesday", " Wednesday", " Thursday", " Friday" };
            string[] Sessions = new[] { "Morning 9am - 11:30am", "Afternoon 12pm = 2:30pm", "Evening 5:30pm - 8:30pm" };
            string[] AllSessionTimeStart = new[] { "9:00:00", "12:00:01", "17:30:00" };
            string[] AllSessionTimeEnd = new[] { "11:30:00", "14:30:01", "20:30:00" };

            //time of session to add in to calender from string to timespan
            TimeSpan MorningSessStart = TimeSpan.Parse(AllSessionTimeStart[0]);
            TimeSpan MorningSessEnd = TimeSpan.Parse(AllSessionTimeEnd[0]);
            TimeSpan AfternoonSessStart = TimeSpan.Parse(AllSessionTimeStart[1]);
            TimeSpan AfternoonSessEnd = TimeSpan.Parse(AllSessionTimeEnd[1]);
            TimeSpan EveningSessStart = TimeSpan.Parse(AllSessionTimeStart[2]);
            TimeSpan EveningSessEnd = TimeSpan.Parse(AllSessionTimeEnd[2]);

            DateTime SessionStart = new DateTime();
            DateTime SessionEnd = new DateTime();
            SeatBooking nextDaySeats = null;

            //create a calendar event for each booking. Shows details about 
            if (seats != null && seats.S1)
            {
                NewEvent(seats, 0, MorningSessStart, MorningSessEnd, days[0], Sessions[0]);
            }
            if (seats != null && seats.S2)
            {
                NewEvent(seats, 0, AfternoonSessStart, AfternoonSessEnd, days[0], Sessions[1]);
            }
            if (seats != null && seats.S3)
            {
                NewEvent(seats, 0, EveningSessStart, EveningSessEnd, days[0], Sessions[2]);
            }
            if (seats != null && seats.S4)
            {

                NewEvent(seats, 1, MorningSessStart, MorningSessEnd, days[1], Sessions[0]);
            }
            if (seats != null && seats.S5)
            {

                NewEvent(seats, 1, AfternoonSessStart, AfternoonSessEnd, days[1], Sessions[1]);
            }
            if (seats != null && seats.S6)
            {

                NewEvent(seats, 1, EveningSessStart, EveningSessEnd, days[1], Sessions[2]);
            }
            if (seats != null && seats.S7)
            {

                NewEvent(seats, 2, MorningSessStart, MorningSessEnd, days[2], Sessions[0]);
            }
            if (seats != null && seats.S8)
            {

                NewEvent(seats, 2, AfternoonSessStart, AfternoonSessEnd, days[2], Sessions[1]);
            }
            if (seats != null && seats.S9)
            {

                NewEvent(seats, 2, EveningSessStart, EveningSessEnd, days[2], Sessions[2]);
            }
            if (seats != null && seats.S10)
            {

                NewEvent(seats, 3, MorningSessStart, MorningSessEnd, days[3], Sessions[0]);
            }
            if (seats != null && seats.S11)
            {

                NewEvent(seats, 3, AfternoonSessStart, AfternoonSessEnd, days[3], Sessions[1]);
            }
            if (seats != null && seats.S12)
            {

                NewEvent(seats, 3, EveningSessStart, EveningSessEnd, days[3], Sessions[2]);
            }
            if (seats != null && seats.S13)
            {

                NewEvent(seats, 4, MorningSessStart, MorningSessEnd, days[4], Sessions[0]);
            }
            if (seats != null && seats.S14)
            {

                NewEvent(seats, 4, AfternoonSessStart, AfternoonSessEnd, days[4], Sessions[1]);
            }
            if (seats != null && seats.S15)
            {

                NewEvent(seats, 4, EveningSessStart, EveningSessEnd, days[4], Sessions[2]);
            }

            return OutputEvents(calendar);
        }
        /// <summary>
        /// Generate an event from the Seat number
        /// </summary>

        private void NewEvent(SeatBooking seats, int DayNext, TimeSpan morningSessStart, TimeSpan morningSessEnd, string day, string session)
        {

            //add the time of the sessions to the date to get DateTime
            DateTime sessionStart = seats.SeatDate.AddDays(DayNext).Add(morningSessStart);
            DateTime sessionEnd = seats.SeatDate.AddDays(DayNext).Add(morningSessEnd);
            string description = day + " " + session;

            //Create a new event with event details
            var vEvent = new CalendarEvent
            {
                Start = new CalDateTime(sessionStart),
                End = new CalDateTime(sessionEnd),
                Description = description,
                Name = "NZBAT at Vision College"
            };

            calendar.Events.Add(vEvent);
        }
        //todo send the calendar events
        public string OutputEvents(Calendar cal)
        {
            var sb = new StringBuilder();

            foreach (var email in cal.Events)
            {
                sb.AppendLine("From: " + email.DtStart.ToString() + " To " + email.DtEnd);
                sb.AppendLine("Desc: " + email.Description.ToString() + " By " + email.Name);
            }

            //  File.WriteAllText(@"C:\Users\Gary.001\Desktop", sb.ToString(), Encoding.UTF8);

            return sb.ToString();
        }

    }
}