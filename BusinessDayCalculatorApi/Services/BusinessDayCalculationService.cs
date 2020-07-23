using System;
using BusinessDayCalculatorApi.ServiceInterfaces;

namespace BusinessDayCalculatorApi.Services
{
    public class BusinessDayCalculationService : IBusinessDayCalculationService
    {
        private readonly IPublicHolidayService _publicHolidayService;
        private const int DaysPerWeek = 7;
        private const int DaysInWeekend = 2;

        public BusinessDayCalculationService(IPublicHolidayService publicHolidayService)
        {
            _publicHolidayService = publicHolidayService;
        }

        public int CalculateNumberOfBusinessDaysBetweenTwoDates(DateTime startDate, DateTime endDate)
        {
            var numberOfBusinessDays = 0;

            if (startDate > endDate)
            {
                throw new ApplicationException("Start date cannot be greater than end date");
            }

            if (startDate == endDate)
            {
                return numberOfBusinessDays;
            }

            var numberOfDaysBetweenStartAndEndDate = endDate.Subtract(startDate).Days;

            var numberOfWeekendDaysBetweenStartAndEndDate = CalculateNumberOfWeekendDaysThatOccurDuringTimePeriod(startDate, endDate);

            var numberOfPublicHolidaysInTimePeriod = CalculateNumberOfPublicHolidaysThatOccurDuringTimePeriod(startDate, endDate);

            numberOfBusinessDays = numberOfDaysBetweenStartAndEndDate - (numberOfWeekendDaysBetweenStartAndEndDate + numberOfPublicHolidaysInTimePeriod);

            numberOfBusinessDays = RemoveEndDateFromCalculation(numberOfBusinessDays);

            return numberOfBusinessDays;
        }

        private int CalculateNumberOfPublicHolidaysThatOccurDuringTimePeriod(DateTime startDate, DateTime endDate)
        {
            var publicHolidayCount = 0;
            var publicHolidays = _publicHolidayService.GetAllPublicHolidays(startDate.Year, endDate.Year);

            foreach (var publicHolidayDate in publicHolidays)
            {
                if (publicHolidayDate > startDate && publicHolidayDate < endDate && !IsDayOfWeekAWeekend(publicHolidayDate))
                {
                    publicHolidayCount++;
                }
            }

            return publicHolidayCount;
        }

        private int CalculateNumberOfWeekendDaysThatOccurDuringTimePeriod(DateTime startDate, DateTime endDate)
        {
            var totalWeekendDays = 0;
            var daysInTimePeriod = endDate.Subtract(startDate).Days;
            var weeksInTimePeriod = daysInTimePeriod / DaysPerWeek;
            var remainingDays = daysInTimePeriod % DaysPerWeek;

            for (var date = endDate; date >= endDate.AddDays(-remainingDays); date = date.AddDays(-1))
            {
                if (IsDayOfWeekAWeekend(date))
                {
                    totalWeekendDays++;
                }
            }

            return (weeksInTimePeriod * DaysInWeekend) + totalWeekendDays;
        }

        private bool IsDayOfWeekAWeekend(DateTime currentDate)
        {
            return currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday;
        }

        // We subtract one day to exclude the end date from the business
        // day calculation. This should only occur if we actually have days to calculate
        private int RemoveEndDateFromCalculation(int numberOfBusinessDays)
        {
            if (numberOfBusinessDays > 0)
            {
                return numberOfBusinessDays - 1;
            }

            return numberOfBusinessDays;
        }
    }
}