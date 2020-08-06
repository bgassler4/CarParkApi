using CarParkApi.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests
{
    public class WeekendRateTests
    {
        private ParkingCalculatorService service;
        private readonly decimal weekendRate = 10m;
        public WeekendRateTests()
        {
            service = new ParkingCalculatorService();
        }

        public static IEnumerable<object[]> WeekendRateDates
        {
            get
            {
                return new[]
                {
                // Aug 1 is a Saturday, Aug 2 is a Sunday
                new object[] { DateTime.Parse("8/1/2020 12:00:00 PM"), DateTime.Parse("8/2/2020 11:59:59 PM") },
                new object[] { DateTime.Parse("8/1/2020 6:00:00 PM"), DateTime.Parse("8/2/2020 3:30:00 PM") },
                new object[] { DateTime.Parse("8/1/2020 6:00:00 PM"), DateTime.Parse("8/2/2020 11:29:59 PM") },
                new object[] { DateTime.Parse("8/1/2020 11:59:59 PM"), DateTime.Parse("8/2/2020 11:29:59 PM") },
                new object[] { DateTime.Parse("8/1/2020 11:59:59 PM"), DateTime.Parse("8/2/2020 3:30:00 AM") },
                new object[] { DateTime.Parse("8/1/2020 8:10:20 PM"), DateTime.Parse("8/2/2020 5:38:45 PM") },

                new object[] { DateTime.Parse("8/1/2020 12:00:00 PM"), DateTime.Parse("8/1/2020 11:59:59 PM") },
                new object[] { DateTime.Parse("8/1/2020 6:00:00 AM"), DateTime.Parse("8/1/2020 3:30:00 PM") },
                new object[] { DateTime.Parse("8/1/2020 6:00:00 AM"), DateTime.Parse("8/1/2020 11:59:59 PM") },
                new object[] { DateTime.Parse("8/1/2020 11:59:59 AM"), DateTime.Parse("8/1/2020 11:29:59 PM") },
                new object[] { DateTime.Parse("8/1/2020 11:59:59 AM"), DateTime.Parse("8/1/2020 3:30:00 PM") },
                new object[] { DateTime.Parse("8/1/2020 8:10:20 AM"), DateTime.Parse("8/1/2020 5:38:45 PM") },
                };
            }
        }

        public static IEnumerable<object[]> NotWeekendRateDates
        {
            get
            {
                return new[]
                {
                //July 31 is a Friday, Aug 1 is a Saturday, Aug 2 is a Sunday
                new object[] { DateTime.Parse("7/31/2020 12:00:00 PM"), DateTime.Parse("8/2/2020 11:59:59 PM") },
                new object[] { DateTime.Parse("7/31/2020 6:00:00 PM"), DateTime.Parse("8/2/2020 3:30:00 PM") },
                new object[] { DateTime.Parse("7/31/2020 6:00:00 PM"), DateTime.Parse("8/2/2020 11:29:59 PM") },
                new object[] { DateTime.Parse("7/31/2020 11:59:59 PM"), DateTime.Parse("8/2/2020 11:29:59 PM") },
                new object[] { DateTime.Parse("7/31/2020 11:59:59 PM"), DateTime.Parse("8/2/2020 3:30:00 AM") },
                new object[] { DateTime.Parse("7/31/2020 8:10:20 PM"), DateTime.Parse("8/2/2020 5:38:45 PM") },

                new object[] { DateTime.Parse("8/1/2020 12:00:00 PM"), DateTime.Parse("8/3/2020 11:59:59 PM") },
                new object[] { DateTime.Parse("8/1/2020 6:00:00 AM"), DateTime.Parse("8/3/2020 3:30:00 PM") },
                new object[] { DateTime.Parse("8/1/2020 6:00:00 AM"), DateTime.Parse("8/3/2020 11:59:59 PM") },
                new object[] { DateTime.Parse("8/1/2020 11:59:59 AM"), DateTime.Parse("8/3/2020 11:29:59 PM") },
                new object[] { DateTime.Parse("8/1/2020 11:59:59 AM"), DateTime.Parse("8/3/2020 3:30:00 PM") },
                new object[] { DateTime.Parse("8/1/2020 8:10:20 AM"), DateTime.Parse("8/3/2020 5:38:45 PM") },
                };
            }
        }


        [Theory, MemberData(nameof(WeekendRateDates))]
        public void ShouldGetWeekendRate(DateTime entry, DateTime exit)
        {
            var rate = service.CalculateParkingRate(entry, exit);
            Assert.Equal(weekendRate, rate);
        }

        [Theory, MemberData(nameof(NotWeekendRateDates))]
        public void ShouldNotGetWeekendRate(DateTime entry, DateTime exit)
        {
            var rate = service.CalculateParkingRate(entry, exit);
            Assert.NotEqual(weekendRate, rate);
        }
    }
}
