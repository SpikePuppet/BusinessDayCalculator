using System;
using System.Collections.Generic;
using System.Linq;
using BusinessDayCalculatorApi.Models;
using BusinessDayCalculatorApi.ServiceInterfaces;

namespace BusinessDayCalculatorApi.Services
{
    public class PublicHolidayService : IPublicHolidayService
    {
        private readonly IPublicHolidayDataService _publicHolidayDataService;
        private const int DaysPerWeek = 7;

        public PublicHolidayService(IPublicHolidayDataService publicHolidayDataService)
        {
            _publicHolidayDataService = publicHolidayDataService;
        }

        public List<DateTime> GetAllPublicHolidays(int startYear, int endYear)
        {
            var publicHolidays = new List<DateTime>();
            var publicHolidayRecords = _publicHolidayDataService.GetAllPublicHolidayRecords();
            var years = endYear - startYear == 0 
                ? new List<int>{ startYear } 
                : Enumerable.Range(startYear, (endYear - startYear) + 1).ToList();

            foreach(var year in years) 
            {
                foreach (var publicHolidayRecord in publicHolidayRecords)
                {
                    if (publicHolidayRecord.SetDate)
                    {    
                        var publicHoliday = new DateTime(year, publicHolidayRecord.Month, publicHolidayRecord.Day);

                        if (publicHolidayRecord.AdjustForWeekend) 
                        {
                            while (IsDayOfWeekAWeekend(publicHoliday)) 
                            {
                                publicHoliday = publicHoliday.AddDays(1);
                            }                            
                        }

                        publicHolidays.Add(publicHoliday);                                         
                    }
                    else
                    {   
                        publicHolidays.Add(CalculateRelativePublicHoliday(year, publicHolidayRecord));
                    }
                }
            }

            return publicHolidays;
        }

        private DateTime CalculateRelativePublicHoliday(int year, PublicHolidayRecord publicHolidayRecord)
        {
            var beginningOfMonth = new DateTime(year, publicHolidayRecord.Month, 1);

            // We continue to iterate through the days until we hit the first matching
            // DayOfWeek.
            while (beginningOfMonth.DayOfWeek != publicHolidayRecord.DayOfWeek)
            {
                beginningOfMonth = beginningOfMonth.AddDays(1);
            }

            // We take away one week because we can consider the first DayOfWeek found
            // in the above while loop.
            var remainingDays = DaysPerWeek * (publicHolidayRecord.WeekOffset - 1);

            return beginningOfMonth.AddDays(remainingDays);
        }

        private bool IsDayOfWeekAWeekend(DateTime currentDate)
        {
            return currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}