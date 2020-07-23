using System;
using System.Collections.Generic;
using System.Linq;
using BusinessDayCalculatorApi.Models;
using BusinessDayCalculatorApi.ServiceInterfaces;
using BusinessDayCalculatorApi.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BusinessDayCalculatorApiTests.UnitTests
{
    [TestClass]
    public class PublicHolidayServiceTests
    {
        private readonly Mock<IPublicHolidayDataService> _publicHolidayDataService = new Mock<IPublicHolidayDataService>();
        private PublicHolidayService _publicHolidayService;

        [TestInitialize]
        public void SetupTestEnvironment()
        {
            _publicHolidayService = new PublicHolidayService(_publicHolidayDataService.Object);
        }

        [TestMethod]
        public void GetAllPublicHolidaysBetweenTwoYears_StartAndEndYearAreTheSameAndOnePublicHolidayExistsInDataSourceWhichOccursOnTheTwentyFifthOfAprilEveryYear_ReturnsOnePublicHolidayDateForTheTwentyFifthOfApril()
        {
            //Arrange
            var startYear = 2020;
            var endYear = 2020;

            var expectedDate = new DateTime(2020, 04, 25);

            _publicHolidayDataService.Setup(x => x.GetAllPublicHolidayRecords())
                .Returns(new List<PublicHolidayRecord>
                {
                    new PublicHolidayRecord
                    {
                        SetDate = true,
                        AdjustForWeekend = false,
                        Day = 25,
                        Month = 4,
                        DayOfWeek = DayOfWeek.Saturday,
                        WeekOffset = 0
                    }
                });

            //Act
            var results = _publicHolidayService.GetAllPublicHolidays(startYear, endYear);

            //Assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(expectedDate, results.FirstOrDefault());
        }

        [TestMethod]
        public void GetAllPublicHolidaysBetweenTwoYears_StartAndEndYearAreTheSameAndOnePublicHolidayExistsInDataSourceWhichOccursOnTheTwentyFifthOfAprilEveryYearButOccursTheNextMondayIfItOccursOnAWeekend_ReturnsOnePublicHolidayDateForTheTwentySeventhOfApril()
        {
            //Arrange
            var startYear = 2020;
            var endYear = 2020;

            var expectedDate = new DateTime(2020, 04, 27);

            _publicHolidayDataService.Setup(x => x.GetAllPublicHolidayRecords())
                .Returns(new List<PublicHolidayRecord>
                {
                    new PublicHolidayRecord
                    {
                        SetDate = true,
                        AdjustForWeekend = true,
                        Day = 25,
                        Month = 4,
                        DayOfWeek = DayOfWeek.Saturday,
                        WeekOffset = 0
                    }
                });

            //Act
            var results = _publicHolidayService.GetAllPublicHolidays(startYear, endYear);

            //Assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(expectedDate, results.FirstOrDefault());
        }

        [TestMethod]
        public void GetAllPublicHolidaysBetweenTwoYears_StartAndEndYearAreTheSameAndOnePublicHolidayExistsInDataSourceWhichOccursOnTheThirdTuesdayOfJuly_ReturnsOnePublicHolidayDateForTheTwentyFirstOfJuly()
        {
            //Arrange
            var startYear = 2020;
            var endYear = 2020;

            var expectedDate = new DateTime(2020, 07, 21);

            _publicHolidayDataService.Setup(x => x.GetAllPublicHolidayRecords())
                .Returns(new List<PublicHolidayRecord>
                {
                    new PublicHolidayRecord
                    {
                        SetDate = false,
                        AdjustForWeekend = true,
                        Day = 25,
                        Month = 7,
                        DayOfWeek = DayOfWeek.Tuesday,
                        WeekOffset = 3
                    }
                });

            //Act
            var results = _publicHolidayService.GetAllPublicHolidays(startYear, endYear);

            //Assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(expectedDate, results.FirstOrDefault());
        }

        [TestMethod]
        public void GetAllPublicHolidaysBetweenTwoYears_OneYearBetweenStartAndEndDateOnePublicHolidayExistsInDataSourceWhichOccursOnTheTwentyFifthOfAprilEveryYear_ReturnsTwoPublicHolidayDateForTheTwentyFifthOfApril()
        {
            //Arrange
            var startYear = 2019;
            var endYear = 2020;

            var expectedDate2019 = new DateTime(2019, 04, 25);
            var expectedDate2020 = new DateTime(2020, 04, 25);

            _publicHolidayDataService.Setup(x => x.GetAllPublicHolidayRecords())
                .Returns(new List<PublicHolidayRecord>
                {
                    new PublicHolidayRecord
                    {
                        SetDate = true,
                        AdjustForWeekend = false,
                        Day = 25,
                        Month = 4,
                        DayOfWeek = DayOfWeek.Saturday,
                        WeekOffset = 0
                    }
                });

            //Act
            var results = _publicHolidayService.GetAllPublicHolidays(startYear, endYear);

            //Assert
            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(expectedDate2019, results.FirstOrDefault());
            Assert.AreEqual(expectedDate2020, results.LastOrDefault());
        }

        [TestMethod]
        public void GetAllPublicHolidaysBetweenTwoYears_OneYearBetweenStartAndEndDateOnePublicHolidayExistsInDataSourceWhichOccursOnTheTwentyFifthOfAprilEveryYearButOccursTheNextMondayIfItOccursOnAWeekend_ReturnsOnePublicHolidayDateForTheTwentySeventhOfAprilAndOneOnTheTwentyFifth()
        {
            //Arrange
            var startYear = 2019;
            var endYear = 2020;

            var expectedDate2019 = new DateTime(2019, 04, 25);
            var expectedDate2020 = new DateTime(2020, 04, 27);

            _publicHolidayDataService.Setup(x => x.GetAllPublicHolidayRecords())
                .Returns(new List<PublicHolidayRecord>
                {
                    new PublicHolidayRecord
                    {
                        SetDate = true,
                        AdjustForWeekend = true,
                        Day = 25,
                        Month = 4,
                        DayOfWeek = DayOfWeek.Saturday,
                        WeekOffset = 0
                    }
                });

            //Act
            var results = _publicHolidayService.GetAllPublicHolidays(startYear, endYear);

            //Assert
            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(expectedDate2019, results.FirstOrDefault());
            Assert.AreEqual(expectedDate2020, results.LastOrDefault());
        }

        [TestMethod]
        public void GetAllPublicHolidaysBetweenTwoYears_OneYearBetweenStartAndEndDateAndOnePublicHolidayExistsInDataSourceWhichOccursOnTheThirdTuesdayOfJuly_ReturnsTwoPublicHolidayDateForTheTheThirdTuesdayInJuly()
        {
            //Arrange
            var startYear = 2019;
            var endYear = 2020;

            var thirdTuesdayInJuly2019 = new DateTime(2019, 07, 16);
            var thirdTuesdayInJuly2020 = new DateTime(2020, 07, 21);

            _publicHolidayDataService.Setup(x => x.GetAllPublicHolidayRecords())
                .Returns(new List<PublicHolidayRecord>
                {
                    new PublicHolidayRecord
                    {
                        SetDate = false,
                        AdjustForWeekend = true,
                        Day = 25,
                        Month = 7,
                        DayOfWeek = DayOfWeek.Tuesday,
                        WeekOffset = 3
                    }
                });

            //Act
            var results = _publicHolidayService.GetAllPublicHolidays(startYear, endYear);

            //Assert
            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(thirdTuesdayInJuly2019, results.FirstOrDefault());
            Assert.AreEqual(thirdTuesdayInJuly2020, results.LastOrDefault());
        }


        [TestMethod]
        public void GetAllPublicHolidaysBetweenTwoYears_StartAndEndYearAreTheSameAndTwoPublicHolidayExistsInDataSourceWhichOccursOnTheTwentyFifthOfAprilEveryYearButOccursTheNextMondayIfItOccursOnAWeekendAndTheOtherOccursTheSecondMondayInJune_ReturnsTwoPublicHolidayDateForTheTwentySeventhOfAprilAndTheTenthOfJune()
        {
            //Arrange
            var startYear = 2020;
            var endYear = 2020;

            var expectedDateForAprilHoliday = new DateTime(2020, 04, 27);
            var expectedDateForJuneHoliday = new DateTime(2020, 06, 08);

            _publicHolidayDataService.Setup(x => x.GetAllPublicHolidayRecords())
                .Returns(new List<PublicHolidayRecord>
                {
                    new PublicHolidayRecord
                    {
                        SetDate = true,
                        AdjustForWeekend = true,
                        Day = 25,
                        Month = 4,
                        DayOfWeek = DayOfWeek.Saturday,
                        WeekOffset = 0
                    },
                    new PublicHolidayRecord
                    {
                        SetDate = false,
                        AdjustForWeekend = true,
                        Day = 25,
                        Month = 6,
                        DayOfWeek = DayOfWeek.Monday,
                        WeekOffset = 2
                    }
                });

            //Act
            var results = _publicHolidayService.GetAllPublicHolidays(startYear, endYear);

            //Assert
            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(expectedDateForAprilHoliday, results.FirstOrDefault());
            Assert.AreEqual(expectedDateForJuneHoliday, results.LastOrDefault());
        }
    }
}