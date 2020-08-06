using CarParkApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CarParkApi.Controllers
{
    [ApiController]
    [Route("api/carpark")]
    public class CarParkController : Controller
    {
        private IParkingCalculatorService parkingCalculatorService;
        public CarParkController(IParkingCalculatorService _parkingCalculatorService)
        {
            parkingCalculatorService = _parkingCalculatorService;
        }


        [HttpGet]
        public decimal CalculateParkingCost(DateTime entry, DateTime exit)
        {
            try
            {
                return parkingCalculatorService.CalculateParkingRate(entry, exit);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
