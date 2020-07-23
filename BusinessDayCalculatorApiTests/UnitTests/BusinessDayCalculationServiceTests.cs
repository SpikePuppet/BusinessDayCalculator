using System;
using System.Collections.Generic;
using BusinessDayCalculatorApi.ServiceInterfaces;
using BusinessDayCalculatorApi.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BusinessDayCalculatorApiTests.UnitTests
{
    [TestClass]
    public class BusinessDayCalculationServiceTests
    {
        private readonly Mock<IPublicHolidayService> _publicHolidayService = new Mock<IPublicHolidayService>();
        private BusinessDayCalculationService _businessDayCalculationService;


        [TestInitialize]
        public void SetupTestEnvironment()
        {
            _publicHolidayService.Setup(x => x.GetAllPublicHolidays(It.IsAny<int>(), It.IsAny<int>())).Returns(new List<DateTime>());
            _businessDayCalculationService = new BusinessDayCalculationService(_publicHolidayService.Object);
        }

        [ExpectedException(typeof(ApplicationException))]
        [TestMethod]
        public void CalculateAmountOfBusinessDaysBetweenTwoDates_StartDateIsGreaterThanEndDate_ThrowsApplicationException()
        {
            // Arrange
            var startDate = new DateTime(2020, 1, 1);
            var endDate = new DateTime(2019, 1, 1);

            // Act
            var result = _businessDayCalculationService.CalculateNumberOfBusinessDaysBetweenTwoDates(startDate, endDate);

            // Assert
            // By Exception
        }

        [TestMethod]
        public void CalculateAmountOfBusinessDaysBetweenTwoDates_StartDateAndEndDateAreEqual_ReturnsBusinessDayCountOfZero()
        {
            // Arrange
            var startDate = new DateTime(2020, 1, 1);
            var endDate = new DateTime(2020, 1, 1);

            // Act
            var result = _businessDayCalculationService.CalculateNumberOfBusinessDaysBetweenTwoDates(startDate, endDate);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CalculateAmountOfBusinessDaysBetweenTwoDates_EndDateTheDayAfterStartDate_ReturnsBusinessDayCountOfZero()
        {
            // Arrange
            var startDate = new DateTime(2020, 1, 1);
            var endDate = new DateTime(2020, 1, 2);

            // Act
            var result = _businessDayCalculationService.CalculateNumberOfBusinessDaysBetweenTwoDates(startDate, endDate);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CalculateAmountOfBusinessDaysBetweenTwoDates_EndDateIsTwoDaysAfterStartDate_ReturnsBusinessDayCountOfOne()
        {
            // Arrange
            var startDate = new DateTime(2020, 1, 1);
            var endDate = new DateTime(2020, 1, 3);

            // Act
            var result = _businessDayCalculationService.CalculateNumberOfBusinessDaysBetweenTwoDates(startDate, endDate);

            // Assert
            Assert.AreEqual(1, result);
        }
        

        [TestMethod]
        public void CalculateAmountOfBusinessDaysBetweenTwoDates_EndDateOnAWeekendAndTheDayAfterStartDate_ReturnsBusinessDayCountOfZero()
        {
            // Arrange
            var startDate = new DateTime(2020, 1, 3);
            var endDate = new DateTime(2020, 1, 4);

            // Act
            var result = _businessDayCalculationService.CalculateNumberOfBusinessDaysBetweenTwoDates(startDate, endDate);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CalculateAmountOfBusinessDaysBetweenTwoDates_EndDateOnAWeekendAndIsTwoDaysAfterStartDate_ReturnsBusinessDayCountOfZero()
        {
            // Arrange
            var startDate = new DateTime(2020, 1, 3);
            var endDate = new DateTime(2020, 1, 5);

            // Act
            var result = _businessDayCalculationService.CalculateNumberOfBusinessDaysBetweenTwoDates(startDate, endDate);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CalculateAmountOfBusinessDaysBetweenTwoDates_StartAndEndDateAreFiveDaysApartIncludingEndDate_ReturnsBusinessDayCountOfThree()
        {
            // Arrange
            var startDate = new DateTime(2020, 1, 6);
            var endDate = new DateTime(2020, 1, 10);

            // Act
            var result = _businessDayCalculationService.CalculateNumberOfBusinessDaysBetweenTwoDates(startDate, endDate);

            // Assert
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void CalculateAmountOfBusinessDaysBetweenTwoDates_StartAndEndDateAreFiveDaysApartIncludingEndDateWithTwoDaysOverTheWeekend_ReturnsBusinessDayCountOfOne()
        {
            // Arrange
            var startDate = new DateTime(2020, 1, 9);
            var endDate = new DateTime(2020, 1, 13);

            // Act
            var result = _businessDayCalculationService.CalculateNumberOfBusinessDaysBetweenTwoDates(startDate, endDate);

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void CalculateAmountOfBusinessDaysBetweenTwoDates_StartAndEndDateAreFiveDaysApartIncludingEndDateWithOnePublicHoliday_ReturnsBusinessDayCountOfTwo()
        {
            // Arrange
            var startDate = new DateTime(2020, 1, 6);
            var endDate = new DateTime(2020, 1, 10);

            _publicHolidayService.Setup(x => x.GetAllPublicHolidays(It.Is<int>(y => y == 2020),
                It.Is<int>(y => y == 2020))).Returns(new List<DateTime>{new DateTime(2020, 1, 8)});

            // Act
            var result = _businessDayCalculationService.CalculateNumberOfBusinessDaysBetweenTwoDates(startDate, endDate);

            // Assert
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void CalculateAmountOfBusinessDaysBetweenTwoDates_StartAndEndDateAreFiveDaysApartIncludingEndDateWithBothTheWeekendAndAPublicHolidayOcurringDuringTheWeek_ReturnsBusinessDayCountOfZero()
        {
            // Arrange
            var startDate = new DateTime(2020, 1, 9);
            var endDate = new DateTime(2020, 1, 13);

            _publicHolidayService.Setup(x => x.GetAllPublicHolidays(It.Is<int>(y => y == 2020),
                It.Is<int>(y => y == 2020))).Returns(new List<DateTime>{new DateTime(2020, 1, 10)});

            // Act
            var result = _businessDayCalculationService.CalculateNumberOfBusinessDaysBetweenTwoDates(startDate, endDate);

            // Assert
            Assert.AreEqual(0, result);
        }
        
        [TestMethod]
        public void CalculateAmountOfBusinessDaysBetweenTwoDates_StartAndEndDateEncompassTheMonthOfJanuary2020WithEightWeekendDaysTotal_ReturnsBusinessDayCountOfTwentyOne()
        {
            // Arrange
            var startDate = new DateTime(2020, 1, 1);
            var endDate = new DateTime(2020, 1, 31);

            // Act
            var result = _businessDayCalculationService.CalculateNumberOfBusinessDaysBetweenTwoDates(startDate, endDate);

            // Assert
            Assert.AreEqual(21, result);
        }

        [TestMethod]
        public void CalculateAmountOfBusinessDaysBetweenTwoDates_StartAndEndDateEncompassTheMonthOfJanuary2020WithEightWeekendDaysTotalAndThreePublicHolidays_ReturnsBusinessDayCountOfEighteen()
        {
            // Arrange
            var startDate = new DateTime(2020, 1, 1);
            var endDate = new DateTime(2020, 1, 31);

            _publicHolidayService.Setup(x => x.GetAllPublicHolidays(It.Is<int>(y => y == 2020),
                It.Is<int>(y => y == 2020))).Returns(new List<DateTime>
                {
                    new DateTime(2020, 1, 10), 
                    new DateTime(2020, 1, 15),
                    new DateTime(2020, 1, 16)
                });

            // Act
            var result = _businessDayCalculationService.CalculateNumberOfBusinessDaysBetweenTwoDates(startDate, endDate);

            // Assert
            Assert.AreEqual(18, result);
        }
    }
}
