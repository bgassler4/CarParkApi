using System;

namespace CarParkApi.Services
{
    public interface IParkingCalculatorService
    {
        decimal CalculateParkingRate(DateTime entry, DateTime exit);
    }
}
