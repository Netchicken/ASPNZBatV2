using ASPNZBat.Models;
using ASPNZBat.ViewModels;
using System;
using System.Collections.Generic;

namespace ASPNZBat.Business
{
    public interface ISessions
    {
        void CountSeats();
        DateTime GetMondayDate();
        IEnumerable<DateTime> GetNextFourWeeks();
        IEnumerable<SummaryStatsVM> StatsSummary();
        SummaryStatsVM GetSingleWeekStats(DateTime WeekSelected);
        List<bool> GetAdminData();
    }
}