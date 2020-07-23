using System;

namespace BusinessDayCalculatorApi.Models
{
    public class PublicHolidayRecord
    {
        public bool SetDate { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }

        public bool AdjustForWeekend { get; set; }

        public DayOfWeek DayOfWeek { get; set; }
        
        public int WeekOffset { get; set; }
    }
}