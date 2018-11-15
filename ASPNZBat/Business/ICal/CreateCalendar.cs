using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ASPNZBat.Business.ICal;
using Ical.Net;
using Ical.Net;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
namespace ASPNZBat.Business.ICal
{
    public class CreateCalendar
    {


        //public void CreateCal()
        //{
        //    MailMessage message = new MailMessage();

        //    message.To.Add("johndoe@user.com");
        //    message.From = new MailAddress("info@company.com", "Company, Inc");
        //    message.Subject = "subject";
        //    message.Body = "emailbody";
        //    message.IsBodyHtml = true;

        //    var calendar = new Ical.Net.Calendar();
        //    foreach (var res in reg.Reservations)
        //    {
        //        calendar.Events.Add(new Event
        //        {
        //            Class = "PUBLIC",
        //            Summary = res.Summary,
        //            Created = new CalDateTime(DateTime.Now),
        //            Description = res.Details,
        //            Start = new CalDateTime(Convert.ToDateTime(res.BeginDate)),
        //            End = new CalDateTime(Convert.ToDateTime(res.EndDate)),
        //            Sequence = 0,
        //            Uid = Guid.NewGuid().ToString(),
        //            Location = res.Location,
        //        });
        //    }

        //var serializer = new CalendarSerializer(new SerializationContext());
        //var serializedCalendar = serializer.SerializeToString(calendar);
        //var bytesCalendar = Encoding.UTF8.GetBytes(serializedCalendar);
        //MemoryStream ms = new MemoryStream(bytesCalendar);
        //System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(ms, "event.ics", "text/calendar");
        //message.Attachments.Add(attachment);

        //    }

    }
}
