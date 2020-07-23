using System;
using System.Collections.Generic;
using System.IO;
using BusinessDayCalculatorApi.Models;
using BusinessDayCalculatorApi.ServiceInterfaces;

namespace BusinessDayCalculatorApi.Services
{
    public class PublicHolidayCsvFileService : IPublicHolidayDataService
    {
        private readonly string _csvDataSourceFileName;

        public PublicHolidayCsvFileService(string filename)
        {
            _csvDataSourceFileName = filename;
        }

        public List<PublicHolidayRecord> GetAllPublicHolidayRecords()
        {
            var publicHolidayCsvRecords = File.ReadAllLines(_csvDataSourceFileName);
            var publicHolidayRecords = new List<PublicHolidayRecord>();

            foreach (var publicHolidayCsvRecord in publicHolidayCsvRecords)
            {
                var publicHoldayFields = publicHolidayCsvRecord.Split(',');

                publicHolidayRecords.Add(new PublicHolidayRecord
                {
                    SetDate = bool.Parse(publicHoldayFields[0]),
                    Month = int.Parse(publicHoldayFields[1]),
                    Day = int.Parse(publicHoldayFields[2]),
                    AdjustForWeekend = bool.Parse(publicHoldayFields[3]),
                    DayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), publicHoldayFields[4]),
                    WeekOffset = int.Parse(publicHoldayFields[5])
                });
            }

            return publicHolidayRecords;
        }
    }
}