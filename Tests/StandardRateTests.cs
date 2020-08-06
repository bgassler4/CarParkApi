using CarParkApi.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class StandardRateTests
    {
        private ParkingCalculatorService service;
        public StandardRateTests()
        {
            service = new ParkingCalculatorService();
        }

        public static IEnumerable<object[]> StandardRates
        {
            get
            {
                return new[]
                {
                // Same day tests -- Aug 6th is a Thursday
                new object[] { DateTime.Parse("8/6/2020 8:30:00 AM"), DateTime.Parse("8/6/2020 8:40:52 AM"), 5m },
                new object[] { DateTime.Parse("8/6/2020 10:00:00 AM"), DateTime.Parse("8/6/2020 11:10:00 AM"), 10m },
                new object[] { DateTime.Parse("8/6/2020 10:00:00 AM"), DateTime.Parse("8/6/2020 12:30:00 PM"), 15m },
                new object[] { DateTime.Parse("8/6/2020 6:00:00 AM"), DateTime.Parse("8/6/2020 2:59:59 PM"), 20m },

                // 1 day tests
                new object[] { DateTime.Parse("8/6/2020 8:30:00 AM"), DateTime.Parse("8/7/2020 8:40:52 AM"), 25m },
                new object[] { DateTime.Parse("8/6/2020 10:00:00 AM"), DateTime.Parse("8/7/2020 11:10:00 AM"), 30m },
                new object[] { DateTime.Parse("8/6/2020 10:00:00 AM"), DateTime.Parse("8/7/2020 12:30:00 PM"), 35m },
                new object[] { DateTime.Parse("8/6/2020 6:00:00 AM"), DateTime.Parse("8/7/2020 2:59:59 PM"), 40m },
                

                //multiple day tests
                new object[] { DateTime.Parse("8/6/2020 8:30:00 AM"), DateTime.Parse("8/10/2020 8:40:52 AM"), 85m },
                new object[] { DateTime.Parse("8/6/2020 10:00:00 AM"), DateTime.Parse("8/11/2020 11:10:00 AM"), 110m },
                new object[] { DateTime.Parse("8/6/2020 10:00:00 AM"), DateTime.Parse("8/12/2020 12:30:00 PM"), 135m },
                new object[] { DateTime.Parse("8/6/2020 6:00:00 AM"), DateTime.Parse("8/13/2020 5:00:00 AM"), 140m },
                };
            }
        }


        [Theory, MemberData(nameof(StandardRates))]
        public void ShouldBeStandardRate(DateTime entry, DateTime exit, decimal expectedRate)
        {
            var rate = service.CalculateParkingRate(entry, exit);
            Assert.Equal(expectedRate, rate);
        }
    }
}
