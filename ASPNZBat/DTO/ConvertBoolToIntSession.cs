using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ASPNZBat.Models;
using ASPNZBat.ViewModels;

namespace ASPNZBat.DTO
{
    public static class ConvertBoolToIntSession
    {
        public static SummaryStatsVM ConvertToInt(SeatBooking sb)
        {
            SummaryStatsVM ssVM = new SummaryStatsVM();


            if (sb.S1 == true)
            {
                ssVM.S1 = 1;
            }
            if (sb.S2 == true)
            {
                ssVM.S2 = 1;
            }
            if (sb.S3 == true)
            {
                ssVM.S3 = 1;
            }
            if (sb.S4 == true)
            {
                ssVM.S4 = 1;
            }
            if (sb.S5 == true)
            {
                ssVM.S5 = 1;
            }
            if (sb.S6 == true)
            {
                ssVM.S6 = 1;
            }
            if (sb.S7 == true)
            {
                ssVM.S7 = 1;
            }
            if (sb.S8 == true)
            {
                ssVM.S8 = 1;
            }
            if (sb.S9 == true)
            {
                ssVM.S9 = 1;
            }
            if (sb.S10 == true)
            {
                ssVM.S10 = 1;
            }
            if (sb.S11 == true)
            {
                ssVM.S11 = 1;
            }
            if (sb.S12 == true)
            {
                ssVM.S12 = 1;
            }
            if (sb.S13 == true)
            {
                ssVM.S13 = 1;
            }
            if (sb.S14 == true)
            {
                ssVM.S14 = 1;
            }
            if (sb.S15 == true)
            {
                ssVM.S15 = 1;
            }

            return ssVM;
        }
    }
}
