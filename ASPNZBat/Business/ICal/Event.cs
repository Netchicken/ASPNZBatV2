using System;
using NodaTime;

namespace ASPNZBat.Business.ICal
{
    public class Event
    {
        public string Class { get; set; }

        public string UId { get; set; }
        public Instant Start { get; set; }
        public Instant End { get; set; }
        public DateTime SeatDate { get; set; }


        public bool S1 { get; set; }
        public bool S2 { get; set; }
        public bool S3 { get; set; }
        public bool S4 { get; set; }
        public bool S5 { get; set; }
        public bool S6 { get; set; }
        public bool S7 { get; set; }
        public bool S8 { get; set; }
        public bool S9 { get; set; }
        public bool S10 { get; set; }
        public bool S11 { get; set; }
        public bool S12 { get; set; }
        public bool S13 { get; set; }
        public bool S14 { get; set; }
        public bool S15 { get; set; }



    }
    //Summary = res.Summary,
    //    Created = new CalDateTime(DateTime.Now),
    //    Description = res.Details,
    //    Start = new CalDateTime(Convert.ToDateTime(res.BeginDate)),
    //    End = new CalDateTime(Convert.ToDateTime(res.EndDate)),
    //    Sequence = 0,
    //    Uid = Guid.NewGuid().ToString(),
    //    Location = res.Location,

}
