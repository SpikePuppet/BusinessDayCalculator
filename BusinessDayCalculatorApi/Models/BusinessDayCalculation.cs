using System;

namespace BusinessDayCalculatorApi.Models
{
    public class BusinessDayCalculation
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        
        public int BusinessDays { get; set; }
    }
}