using BusinessDayCalculatorApi.Models;
using BusinessDayCalculatorApi.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebEndpoints.Controllers
{
    [ApiController]
    [Route("calculate-business-days")]
    public class CalculateBusinessDaysController : ControllerBase
    {
        private readonly IBusinessDayCalculationService _businessDayCalculationService;

        public CalculateBusinessDaysController(IBusinessDayCalculationService businessDayCalculationService)
        {
            _businessDayCalculationService = businessDayCalculationService;
        }

        [HttpPost]
        public IActionResult Post(BusinessDayCalculation request)
        {
            request.BusinessDays = _businessDayCalculationService.CalculateNumberOfBusinessDaysBetweenTwoDates(request.StartDate, request.EndDate); 

            return Ok(request);
        }
    }
}