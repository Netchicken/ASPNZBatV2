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
    using Models;

    public interface ICalService
    {
        string CalendarBooking(IEnumerable<SeatBooking> seatBooking);
        List<string> OutputEventsToIndex(Calendar cal);
    }
}
