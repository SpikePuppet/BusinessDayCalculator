using System;
using System.Collections.Generic;

namespace BusinessDayCalculatorApi.ServiceInterfaces
{
    public interface IPublicHolidayService
    {
        List<DateTime> GetAllPublicHolidays(int startYear, int endYear);
    }
}