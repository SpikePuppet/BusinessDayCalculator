using System;

namespace BusinessDayCalculatorApi.ServiceInterfaces
{
    public interface IBusinessDayCalculationService
    {
        int CalculateNumberOfBusinessDaysBetweenTwoDates(DateTime startDate, DateTime endDate);
    }
}