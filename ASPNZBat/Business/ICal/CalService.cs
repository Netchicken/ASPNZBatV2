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
    using System.Collections;
    using System.Linq;
    using DTO;
    using Migrations;

    public class CalService : ICalService
    {
        //global calendar that newEvent gets added to
        Calendar calendar = new Calendar();
        private IDBCallsSessionDataDTO _dbCallsSessionData;

        public CalService(IDBCallsSessionDataDTO dbCallsSessionData)
        {
            _dbCallsSessionData = dbCallsSessionData;
        }

        /// <summary>
        /// Set up a calender of events to be submitted as a string
        /// </summary>
        public string CalendarBooking(IEnumerable<SeatBooking> seatBooking)
        {
            //https://stackoverflow.com/questions/52950884/ical-net-viewing-event-data-from-calendar-in-net-and-c-sharp

            //book all the seats
            return GetBookedSeats(seatBooking);
        }

        /// <summary>
        /// Get the current logged in users seat bookings
        /// </summary>
        /// <param name="seatBookings">List of Seatbookings</param>
        private string GetBookedSeats(IEnumerable<SeatBooking> seatBookings)
        {
            //I hate hardcoding, this should be abstracted out to an admin section

            //arrays of data
            string[] days = new[] { " Monday", " Tuesday", " Wednesday", " Thursday", " Friday" };
            string[] sessions = new[] { "Morning 9am - 11:30am", "Afternoon 12pm = 2:30pm", "Evening 5:30pm - 8:30pm" };
            string[] allSessionTimeStart = new[] { "9:00:00", "12:00:01", "17:30:00" };
            string[] allSessionTimeEnd = new[] { "11:30:00", "14:30:01", "20:30:00" };

            //time of session to add in to calender from string to timespan
            TimeSpan morningSessStart = TimeSpan.Parse(allSessionTimeStart[0]);
            TimeSpan morningSessEnd = TimeSpan.Parse(allSessionTimeEnd[0]);
            TimeSpan afternoonSessStart = TimeSpan.Parse(allSessionTimeStart[1]);
            TimeSpan afternoonSessEnd = TimeSpan.Parse(allSessionTimeEnd[1]);
            TimeSpan eveningSessStart = TimeSpan.Parse(allSessionTimeStart[2]);
            TimeSpan eveningSessEnd = TimeSpan.Parse(allSessionTimeEnd[2]);


            //  SeatBooking nextDaySeats = null;

            //loop through all the booked sessions and crate a cal event
            foreach (var seats in seatBookings)
            {

                //create a calendar event for each booking. Shows details about 
                if (seats != null && seats.S1)
                {
                    NewEvent(seats, 0, morningSessStart, morningSessEnd, days[0], sessions[0]);
                }
                if (seats != null && seats.S2)
                {
                    NewEvent(seats, 0, afternoonSessStart, afternoonSessEnd, days[0], sessions[1]);
                }
                if (seats != null && seats.S3)
                {
                    NewEvent(seats, 0, eveningSessStart, eveningSessEnd, days[0], sessions[2]);
                }
                if (seats != null && seats.S4)
                {

                    NewEvent(seats, 1, morningSessStart, morningSessEnd, days[1], sessions[0]);
                }
                if (seats != null && seats.S5)
                {

                    NewEvent(seats, 1, afternoonSessStart, afternoonSessEnd, days[1], sessions[1]);
                }
                if (seats != null && seats.S6)
                {

                    NewEvent(seats, 1, eveningSessStart, eveningSessEnd, days[1], sessions[2]);
                }
                if (seats != null && seats.S7)
                {

                    NewEvent(seats, 2, morningSessStart, morningSessEnd, days[2], sessions[0]);
                }
                if (seats != null && seats.S8)
                {

                    NewEvent(seats, 2, afternoonSessStart, afternoonSessEnd, days[2], sessions[1]);
                }
                if (seats != null && seats.S9)
                {

                    NewEvent(seats, 2, eveningSessStart, eveningSessEnd, days[2], sessions[2]);
                }
                if (seats != null && seats.S10)
                {

                    NewEvent(seats, 3, morningSessStart, morningSessEnd, days[3], sessions[0]);
                }
                if (seats != null && seats.S11)
                {

                    NewEvent(seats, 3, afternoonSessStart, afternoonSessEnd, days[3], sessions[1]);
                }
                if (seats != null && seats.S12)
                {

                    NewEvent(seats, 3, eveningSessStart, eveningSessEnd, days[3], sessions[2]);
                }
                if (seats != null && seats.S13)
                {

                    NewEvent(seats, 4, morningSessStart, morningSessEnd, days[4], sessions[0]);
                }
                if (seats != null && seats.S14)
                {

                    NewEvent(seats, 4, afternoonSessStart, afternoonSessEnd, days[4], sessions[1]);
                }
                if (seats != null && seats.S15)
                {

                    NewEvent(seats, 4, eveningSessStart, eveningSessEnd, days[4], sessions[2]);
                }
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
            string seatId = seats.ID.ToString();

            //Create a new event with event details
            var vEvent = new CalendarEvent
            {
                Start = new CalDateTime(sessionStart),
                End = new CalDateTime(sessionEnd),
                Description = seatId + ": " + description,
                Name = "NZBAT at Vision College"

            };

            calendar.Events.Add(vEvent); //adding in the new events
        }
        /// <summary>
        ///Generate the outputs
        /// 
        /// </summary>
        /// <returns> Returns a string that goes to the screen</returns>
        public string OutputEvents(Calendar cal)
        {
            //send it back to be sent by email 
            _dbCallsSessionData.SeatBookingsCalOutputToEmail = cal;

            //send it back to output on the screen
            var sb = new StringBuilder();

            foreach (var email in cal.Events)
            {
                sb.AppendLine("From: " + email.DtStart + " To " + email.DtEnd);
                sb.AppendLine("Desc: " + email.Description + " By " + email.Name);
            }

            //  File.WriteAllText(@"C:\Users\Gary.001\Desktop", sb.ToString(), Encoding.UTF8);

            return sb.ToString();
        }
        /// <summary>
        /// Send the data to the Index page for users to check their booking, need to tie it into the Session numbers
        /// </summary>
        /// <param name="cal"></param>
        /// <returns>Returns a list of data to be looped through on the index page</returns>
        public List<string> OutputEventsToIndex(Calendar cal)
        {
            //send it back to output on the screen
            var sessions = new List<string>();
            foreach (var email in cal.Events)
            {
                sessions.Add(email.Description);
            }

            _dbCallsSessionData.SeatBookingsOutputToIndex = sessions;
            return sessions;
        }

    }
}