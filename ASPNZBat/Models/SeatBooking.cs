using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace ASPNZBat.Models
{
    using System.Collections;

    public class SeatBooking
    {
        public int ID { get; set; }
        public Students StudentID { get; set; }
        public string StudentEmail { get; set; }
        public string Name { get; set; }
        public DateTime SeatDate { get; set; }
        public DateTime SeatDateEnd { get; set; }


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
        // public bool S16 { get; set; }
        //public IEnumerator GetEnumerator()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
