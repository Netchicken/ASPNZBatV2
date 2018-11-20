using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;


namespace ASPNZBat.Business.ICal
{
    using Models;

    public interface ICalService
    {
        string testBooking(IEnumerable<SeatBooking> seatBooking);

    }
}
