using System.Collections.Generic;
using BusinessDayCalculatorApi.Models;

namespace BusinessDayCalculatorApi.ServiceInterfaces
{
    public interface IPublicHolidayDataService
    {
        List<PublicHolidayRecord> GetAllPublicHolidayRecords();
    }
}