using System.Linq;
using System.Reflection.PortableExecutable;
using ASPNZBat.Data;

using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using ASPNZBat.Models;
using ASPNZBat.ViewModels;

namespace ASPNZBat.Business
{
    public class Sessions : ISessions
    {
        private readonly SeatBookingDBContext _context;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        //   private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(User);

        public Sessions(
            SeatBookingDBContext context,
            UserManager<IdentityUser> userManager
        )
        {
            _context = context;
            _userManager = userManager;
        }

        public void CountSeats()
        {
            var AllSeats = _context.SeatBooking.Select(s => new { s.SeatDate, s.S1, s.S2, s.S3, s.S4, s.S5, s.S6, s.S7, s.S8, s.S9, s.S10, s.S11, s.S12, s.S13, s.S14, s.S15 }).GroupBy(s => s.SeatDate).OrderBy(s => s.Key)
                .ToList();

        }

        public DateTime GetMondayDate()
        {
            var Now = DateTime.Now.Date;
            switch (Now.DayOfWeek)
            {
                case DayOfWeek.Friday:
                    return DateTime.Now.AddDays(-4);

                case DayOfWeek.Monday:
                    return Now;

                case DayOfWeek.Saturday:
                    return DateTime.Now.AddDays(2);
                case DayOfWeek.Sunday:
                    return DateTime.Now.AddDays(1);
                case DayOfWeek.Thursday:
                    return DateTime.Now.AddDays(-3);
                case DayOfWeek.Tuesday:
                    return DateTime.Now.AddDays(-1);
                case DayOfWeek.Wednesday:
                    return DateTime.Now.AddDays(-2);
                default:
                    return DateTime.Now;

            }
        }

        public IEnumerable<DateTime> GetNextFourWeeks()
        {
            //Get this Mondays date
            DateTime Monday = GetMondayDate();

            //Generate the next 4 Mondays
            List<DateTime> Week = new List<DateTime>();

            Week.Add(Monday.Date);
            Week.Add(Monday.Date.AddDays(7));
            Week.Add(Monday.Date.AddDays(14));
            Week.Add(Monday.Date.AddDays(21));

            return Week;
        }

        /// <summary>
        /// Generate the summary stats for the Create View
        /// </summary>
        /// <param name="Week">Get Date of Monday This week</param>
        /// <returns></returns>

        public IEnumerable<SummaryStatsVM> StatsSummary()
        {
            List<DateTime> Week = (List<DateTime>)GetNextFourWeeks();
            List<SummaryStatsVM> SumStats = new List<SummaryStatsVM>();
            SumStats = _context.SeatBooking
                .Where(s => s.SeatDate >= Week[0])
                .GroupBy(d => d.SeatDate)
                .Select(s => new SummaryStatsVM()
                {
                    SeatDate = s.Key,
                    S1 = s.Count(c => c.S1 == true),
                    S2 = s.Count(c => c.S2 == true),
                    S3 = s.Count(c => c.S3 == true),
                    S4 = s.Count(c => c.S4 == true),
                    S5 = s.Count(c => c.S5 == true),
                    S6 = s.Count(c => c.S6 == true),
                    S7 = s.Count(c => c.S7 == true),
                    S8 = s.Count(c => c.S8 == true),
                    S9 = s.Count(c => c.S9 == true),
                    S10 = s.Count(c => c.S10 == true),
                    S11 = s.Count(c => c.S1 == true),
                    S12 = s.Count(c => c.S2 == true),
                    S13 = s.Count(c => c.S3 == true),
                    S14 = s.Count(c => c.S4 == true),
                    S15 = s.Count(c => c.S5 == true)

                }).ToList();


            return SumStats;
        }

        public SummaryStatsVM GetSingleWeekStats(DateTime WeekSelected)
        {

            List<SummaryStatsVM> WeeklyStats = new List<SummaryStatsVM>();
            SummaryStatsVM SingleWeekStats = new SummaryStatsVM();
            WeeklyStats.AddRange(StatsSummary());


            //for (int i = 0; i < 4; i++)
            //{
            //    if (WeeklyStats[i].SeatDate.Date.ToShortDateString() == WeekSelected.Date.ToShortDateString())
            //    {

            //        for (int j = 0; j < 16; i++)
            //        {
            //            SingleWeekStats[i] = WeeklyStats[i,j];
            //        }
            //    }
            //}




            foreach (var week in WeeklyStats)
            {
                if (week.SeatDate.Date.ToShortDateString() == WeekSelected.Date.ToShortDateString())
                {
                    foreach (SummaryStatsVM session in WeeklyStats)
                    {
                        SingleWeekStats = session;
                    }
                }
            }

            return SingleWeekStats;
        }

        public List<bool> GetAdminData()
        {
            //AdminDataVM VisibilityList = (AdminDataVM)_context.AdminData
            //    .Select (a => new { a.isVisibleS1L, a.isVisibleS2L, a.isVisibleS3L, a.isVisibleS4L, a.isVisibleS5L, a.isVisibleS6L, a.isVisibleS7L, a.isVisibleS8L, a.isVisibleS9L, a.isVisibleS10L, a.isVisibleS11L, a.isVisibleS12L, a.isVisibleS13L, a.isVisibleS14L, a.isVisibleS15L });

            //https://stackoverflow.com/questions/20906520/casting-errors-when-attempting-to-return-an-iqueryablemytype 

            // take the anonymous output and turn it back into a type
            var VisibilityList = from a in _context.AdminData
                                 select new AdminDataVM
                                 {
                                     isVisibleS1L = a.isVisibleS1L,
                                     isVisibleS2L = a.isVisibleS2L,
                                     isVisibleS3L = a.isVisibleS3L,
                                     isVisibleS4L = a.isVisibleS4L,
                                     isVisibleS5L = a.isVisibleS5L,
                                     isVisibleS6L = a.isVisibleS6L,
                                     isVisibleS7L = a.isVisibleS7L,
                                     isVisibleS8L = a.isVisibleS8L,
                                     isVisibleS9L = a.isVisibleS9L,
                                     isVisibleS10L = a.isVisibleS10L,
                                     isVisibleS11L = a.isVisibleS11L,
                                     isVisibleS12L = a.isVisibleS12L,
                                     isVisibleS13L = a.isVisibleS13L,
                                     isVisibleS14L = a.isVisibleS14L,
                                     isVisibleS15L = a.isVisibleS15L
                                 };
            List<bool> adminData = new List<bool>();

            adminData.Add(false);
            adminData.Add(VisibilityList.Select(v => v.isVisibleS1L).Single());
            adminData.Add(VisibilityList.Select(v => v.isVisibleS2L).Single());
            adminData.Add(VisibilityList.Select(v => v.isVisibleS3L).Single());
            adminData.Add(VisibilityList.Select(v => v.isVisibleS4L).Single());
            adminData.Add(VisibilityList.Select(v => v.isVisibleS5L).Single());
            adminData.Add(VisibilityList.Select(v => v.isVisibleS6L).Single());
            adminData.Add(VisibilityList.Select(v => v.isVisibleS7L).Single());
            adminData.Add(VisibilityList.Select(v => v.isVisibleS8L).Single());
            adminData.Add(VisibilityList.Select(v => v.isVisibleS9L).Single());
            adminData.Add(VisibilityList.Select(v => v.isVisibleS10L).Single());
            adminData.Add(VisibilityList.Select(v => v.isVisibleS11L).Single());
            adminData.Add(VisibilityList.Select(v => v.isVisibleS12L).Single());
            adminData.Add(VisibilityList.Select(v => v.isVisibleS13L).Single());
            adminData.Add(VisibilityList.Select(v => v.isVisibleS14L).Single());
            adminData.Add(VisibilityList.Select(v => v.isVisibleS15L).Single());



            //adminData[0] = false;
            //adminData[1] = VisibilityList.isVisibleS1L;
            //adminData[2] = VisibilityList.isVisibleS2L;
            //adminData[3] = VisibilityList.isVisibleS3L;
            //adminData[4] = VisibilityList.isVisibleS4L;
            //adminData[5] = VisibilityList.isVisibleS5L;
            //adminData[6] = VisibilityList.isVisibleS6L;
            //adminData[7] = VisibilityList.isVisibleS7L;
            //adminData[8] = VisibilityList.isVisibleS8L;
            //adminData[9] = VisibilityList.isVisibleS9L;
            //adminData[10] = VisibilityList.isVisibleS10L;
            //adminData[11] = VisibilityList.isVisibleS11L;
            //adminData[12] = VisibilityList.isVisibleS12L;
            //adminData[13] = VisibilityList.isVisibleS13L;
            //adminData[14] = VisibilityList.isVisibleS14L;
            //adminData[15] = VisibilityList.isVisibleS15L;

            //adminData[0] = false;
            //adminData[1] = VisibilityList.Select(v=>v.isVisibleS1L);
            //adminData[2] = VisibilityList[0].isVisibleS2L;
            //adminData[3] = VisibilityList[0].isVisibleS3L;
            //adminData[4] = VisibilityList[0].isVisibleS4L;
            //adminData[5] = VisibilityList[0].isVisibleS5L;
            //adminData[6] = VisibilityList[0].isVisibleS6L;
            //adminData[7] = VisibilityList[0].isVisibleS7L;
            //adminData[8] = VisibilityList[0].isVisibleS8L;
            //adminData[9] = VisibilityList[0].isVisibleS9L;
            //adminData[10] = VisibilityList[0].isVisibleS10L;
            //adminData[11] = VisibilityList[0].isVisibleS11L;
            //adminData[12] = VisibilityList[0].isVisibleS12L;
            //adminData[13] = VisibilityList[0].isVisibleS13L;
            //adminData[14] = VisibilityList[0].isVisibleS14L;
            //adminData[15] = VisibilityList[0].isVisibleS15L;

            return adminData;
        }
    }

}

