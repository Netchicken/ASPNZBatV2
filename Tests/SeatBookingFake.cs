using System;
using System.Collections.Generic;
using System.Text;
using ASPNZBat;
namespace Tests
{
    using ASPNZBat.Models;

    class SeatBookingFake : ISeatBookingFake
    { // //https://code-maze.com/unit-testing-aspnetcore-web-api/
        private readonly List<SeatBooking> _seatBooking;


        public SeatBookingFake()
        {
            _seatBooking = new List<SeatBooking>() {
            new SeatBooking()
            {
                ID = 1,
                S1 = true,
                S2 = true,
                S3 = true,
                S4 = true,
                S5 = true,
                S6 = true,
                S7 = true,
                S8 = true,
                S9 = true,
                S10 = true,
                S11 = true,
                S12 = true,
                S13 = true,
                S14 = true,
                S15 = true,
                StudentEmail = "aaa@aaa.com",
                Name = "Aaa Aaa",
                SeatDate = DateTime.Parse("2018-12-10 00:00:00")
            },
            new SeatBooking()
            {
                ID = 2,
                S1 = true,
                S2 = true,
                S3 = true,
                S4 = true,
                S5 = true,
                S6 = true,
                S7 = true,
                S8 = true,
                S9 = true,
                S10 = true,
                S11 = true,
                S12 = true,
                S13 = true,
                S14 = true,
                S15 = true,
                StudentEmail = "bbb@bbb.com",
                Name = "Bbb Bbb",
                SeatDate = DateTime.Parse("2018-17-10 00:00:00")
            },
            new SeatBooking()
            {
                ID = 3,
                S1 = true,
                S2 = true,
                S3 = true,
                S4 = true,
                S5 = true,
                S6 = true,
                S7 = true,
                S8 = true,
                S9 = true,
                S10 = true,
                S11 = true,
                S12 = true,
                S13 = true,
                S14 = true,
                S15 = true,
                StudentEmail = "ccc@ccc.com",
                Name = "Ccc Ccc",
                SeatDate = DateTime.Parse("2018-24-10 00:00:00")
            },
            new SeatBooking()
            {
                ID = 4,
                S1 = true,
                S2 = true,
                S3 = true,
                S4 = true,
                S5 = true,
                S6 = true,
                S7 = true,
                S8 = true,
                S9 = true,
                S10 = true,
                S11 = true,
                S12 = true,
                S13 = true,
                S14 = true,
                S15 = true,
                StudentEmail = "ccc@ccc.com",
                Name = "Ccc Ccc",
                SeatDate = DateTime.Parse("2018-31-10 00:00:00")
            }
        };
        }


        //ID	StudentIDID	SeatDate	S1	S10	S11	S12	S13	S14	S15	S16	S2	S3	S4	S5	S6	S7	S8	S9	SeatDateEnd	StudentEmail	Name
        //    3	NULL	2018-12-10 00:00:00	1	1	0	0	0	0	0	0	0	0	1	0	0	1	0	0	0001-01-01 00:00:00	intelproof @gmail.com    Gary Dix
        //  4	NULL    2018-12-17 00:00:00	0	0	0	1	0	0	0	0	1	0	0	0	1	0	1	0	0001-01-01 00:00:00	intelproof @gmail.com Gary Dix
        //   5	NULL    2018-12-24 00:00:00	1	0	0	1	0	0	0	0	0	0	0	0	1	1	0	0	0001-01-01 00:00:00	intelproof @gmail.com Gary Dix
        //   6	NULL    2018-12-31 00:00:00	0	1	0	0	0	0	0	0	1	0	1	0	0	0	1	0	0001-01-01 00:00:00	intelproof @gmail.com Gary Dix
        //   21	NULL    2018-12-10 00:00:00	0	1	0	0	0	0	0	0	0	0	1	0	0	1	0	0	0001-01-01 00:00:00	intelproof @gmail.com Gary Dix

        public IEnumerable<SeatBooking> GetAllSeatBookings()
        {
            return _seatBooking;
        }
    }
}
