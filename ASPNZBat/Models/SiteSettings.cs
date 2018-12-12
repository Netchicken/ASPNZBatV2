using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNZBat.Models
{
    public class SiteSettings
    {
        public string Id { get; set; }
        public string maxSeats { get; set; }
        public string nearlyFullSeats { get; set; }
        public string plentySeats { get; set; }
    }
}
