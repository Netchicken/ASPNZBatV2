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
    public class CalService //: ICalService
    {
        Calendar calendar = new Calendar();
        /// <summary>
        /// Set up a calender of events to be submitted to something
        /// </summary>
        /// <param name="booking"></param>
        public Calendar testBooking(SeatBooking booking)
        {
            //https://stackoverflow.com/questions/52950884/ical-net-viewing-event-data-from-calendar-in-net-and-c-sharp

            //book all the seats
            GetBookedSeats(booking);
            return calendar;
        }

        private void GetBookedSeats(SeatBooking seats)
        {
            string Description = null;
            string[] days = new[] { " Monday", " Tuesday", " Wednesday", " Thursday", " Friday" };
            string[] Sessions = new[] { "Morning 9am - 11:30am", "Afternoon 12pm = 2:30pm", "Evening 5:30pm - 8:30pm" };
            string[] AllSessionTimeStart = new[] { "9:00:00", "12:00:01", "17:30:00" };
            string[] AllSessionTimeEnd = new[] { "11:30:00", "14:30:01", "20:30:00" };

            //time of session to add in to calender
            TimeSpan MorningSessStart = TimeSpan.Parse(AllSessionTimeStart[0]);
            TimeSpan MorningSessEnd = TimeSpan.Parse(AllSessionTimeEnd[0]);
            TimeSpan AfternoonSessStart = TimeSpan.Parse(AllSessionTimeStart[1]);
            TimeSpan AfternoonSessEnd = TimeSpan.Parse(AllSessionTimeEnd[1]);
            TimeSpan EveningSessStart = TimeSpan.Parse(AllSessionTimeStart[2]);
            TimeSpan EveningSessEnd = TimeSpan.Parse(AllSessionTimeEnd[2]);

            DateTime SessionStart = new DateTime();
            DateTime SessionEnd = new DateTime();
            SeatBooking nextDaySeats = null;
            if (seats.S1)
            {
                NewEvent(seats, 0, MorningSessStart, MorningSessEnd, days[0], Sessions[0]);
            }
            if (seats.S2)
            {
                NewEvent(seats, 0, AfternoonSessStart, AfternoonSessEnd, days[0], Sessions[1]);
            }
            if (seats.S3)
            {
                NewEvent(seats, 0, EveningSessStart, EveningSessEnd, days[0], Sessions[2]);
            }
            if (seats.S4)
            {

                NewEvent(seats, 1, MorningSessStart, MorningSessEnd, days[1], Sessions[0]);
            }
            if (seats.S5)
            {

                NewEvent(seats, 1, AfternoonSessStart, AfternoonSessEnd, days[1], Sessions[1]);
            }
            if (seats.S6)
            {

                NewEvent(seats, 1, EveningSessStart, EveningSessEnd, days[1], Sessions[2]);
            }
            if (seats.S7)
            {

                NewEvent(seats, 2, MorningSessStart, MorningSessEnd, days[2], Sessions[0]);
            }
            if (seats.S8)
            {

                NewEvent(seats, 2, AfternoonSessStart, AfternoonSessEnd, days[2], Sessions[1]);
            }
            if (seats.S9)
            {

                NewEvent(seats, 2, EveningSessStart, EveningSessEnd, days[2], Sessions[2]);
            }
            if (seats.S10)
            {

                NewEvent(seats, 3, MorningSessStart, MorningSessEnd, days[3], Sessions[0]);
            }
            if (seats.S11)
            {

                NewEvent(seats, 3, AfternoonSessStart, AfternoonSessEnd, days[3], Sessions[1]);
            }
            if (seats.S12)
            {

                NewEvent(seats, 3, EveningSessStart, EveningSessEnd, days[3], Sessions[2]);
            }
            if (seats.S13)
            {

                NewEvent(seats, 4, MorningSessStart, MorningSessEnd, days[4], Sessions[0]);
            }
            if (seats.S14)
            {

                NewEvent(seats, 4, AfternoonSessStart, AfternoonSessEnd, days[4], Sessions[1]);
            }
            if (seats.S15)
            {

                NewEvent(seats, 4, EveningSessStart, EveningSessEnd, days[4], Sessions[2]);
            }
        }

        private void NewEvent(SeatBooking seats, int DayNext, TimeSpan morningSessStart, TimeSpan morningSessEnd, string day,
            string session)
        {

            //add the time of the sessions
            DateTime sessionStart = seats.SeatDate.AddDays(DayNext).Add(morningSessStart);
            DateTime sessionEnd = seats.SeatDate.AddDays(DayNext).Add(morningSessEnd);
            string description = day + " " + session;
            var vEvent = new CalendarEvent
            {
                Start = new CalDateTime(sessionStart),
                End = new CalDateTime(sessionEnd),
                Description = description,
                Name = "NZBAT at Vision College"
            };
            calendar.Events.Add(vEvent);
        }


        /// <summary>
        ///Generates calendar object using CalendarInviteObject
        /// </summary>
        /// <param name="model"></param>
        /// <returns>ResponseMessageResult</returns>
        //public IHttpActionResult CalendarBookingFileResult(CalendarInviteViewModel model)
        //{
        //    var bytes = GetCalendarBookingBytes(model);
        //    return IcsFileContentResult(model, bytes);
        //}

        //private IHttpActionResult IcsFileContentResult(CalendarInviteViewModel entity, MemoryStream memory)
        //{
        //    var filename = entity.FileName;


        //    var message = new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        Content = new ByteArrayContent(memory.ToArray())
        //    };

        //    var encoder = Encoding.GetEncoding("us-ascii", new EncoderReplacementFallback(string.Empty), new DecoderExceptionFallback());
        //    string asciiFileName = encoder.GetString(encoder.GetBytes(filename));

        //    // Set content headers
        //    message.Content.Headers.ContentLength = memory.Length;
        //    message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        //    message.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
        //    {
        //        FileName = asciiFileName
        //    };

        //    return new System.Web.Http.Results.ResponseMessageResult(message);
        //}

        /// <summary>
        /// Creates calendar Objects and events
        /// </summary>
        /// <param name="model"></param>
        /// <returns>a MemoryStream</returns>
        //private MemoryStream GetCalendarBookingBytes(CalendarInviteViewModel model)
        //{

        //    var iCal = new Calendar();


        //    var evt = iCal.Create<CalendarEvent>();
        //    evt.Summary = model.Title;
        //    evt.Start = new CalDateTime(model.StartDate);
        //    evt.End = new CalDateTime(model.EndDate);
        //    evt.Description = model.Body;
        //    evt.Location = model.Address;

        //    if (model.StartDate.TimeOfDay.Hours == 0)
        //    {
        //        evt.IsAllDay = true;
        //    }

        //    evt.Uid = new Guid().ToString();
        //    //evt.Organizer = new Organizer(organizer);
        //    evt.Alarms.Add(new Alarm
        //    {
        //        Duration = new TimeSpan(30, 0, 0),
        //        Trigger = new Trigger(new TimeSpan(30, 0, 0)),
        //        Action = AlarmAction.Display,
        //        Description = "Reminder"
        //    });
        //    SerializationContext ctx = new SerializationContext();
        //    ISerializerFactory factory = new SerializerFactory();
        //    var serializer = factory.Build(iCal.GetType(), ctx) as IStringSerializer;

        //    var output = serializer.SerializeToString(iCal);
        //    var bytes = Encoding.UTF8.GetBytes(output);

        //    MemoryStream ms = new MemoryStream(bytes);
        //    return ms;
        //}



        //public MemoryStream GenerateIcsFile()
        //{

        //    var iCal = new Calendar();

        //    // Create the event, and add it to the iCalendar
        //    var evt = iCal.Create<CalendarEvent>();

        //    // Set information about the event
        //    evt.Start = CalDateTime.Today.AddHours(8);
        //    evt.End = evt.Start.AddHours(18); // This also sets the duration
        //    evt.Description = "The event description";
        //    evt.Location = "Event location";
        //    evt.Summary = "18 hour event summary";

        //    // Set information about the second event
        //    evt = iCal.Create<CalendarEvent>();
        //    evt.Start = CalDateTime.Today.AddDays(5);
        //    evt.End = evt.Start.AddDays(1);
        //    evt.IsAllDay = true;
        //    evt.Summary = "All-day event";


        //    // Create a serialization context and serializer factory.
        //    // These will be used to build the serializer for our object.
        //    SerializationContext ctx = new SerializationContext();
        //    ISerializerFactory factory = new SerializerFactory();
        //    // Get a serializer for our object
        //    var serializer = factory.Build(iCal.GetType(), ctx) as IStringSerializer;

        //    var output = serializer.SerializeToString(iCal);
        //    var bytes = Encoding.UTF8.GetBytes(output);



        //}




    }
}