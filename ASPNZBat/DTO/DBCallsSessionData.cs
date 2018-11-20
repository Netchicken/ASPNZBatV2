using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNZBat.DTO
{
    using Ical.Net;
    using Models;
    //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-2.1#dependency-injection
    //https://stackoverflow.com/questions/38138100/what-is-the-difference-between-services-addtransient-service-addscoped-and-serv

    //How to keep state during a session - can't use static methods Singleton which creates a single instance throughout the application. It creates the instance for the first time and reuses the same object in the all calls. Scoped lifetime services are created once per request within the scope. It is equivalent to Singleton in the current scope.

    public class DBCallsSessionData : IDBCallsSessionData
    {

        public List<SeatBooking> lastSeatBooking { get; set; }
        public Calendar SeatBookingsCalOutputToEmail { get; set; }


        public DBCallsSessionData()
        {
            //need to instantiate the list
            lastSeatBooking = new List<SeatBooking>();

        }
    }
}
