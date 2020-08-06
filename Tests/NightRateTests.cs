using CarParkApi.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace Tests
{

    public class NightRateTests
    {
        private ParkingCalculatorService service;
        private readonly decimal nightRate = 6.5m;
        public NightRateTests()
        {
            service = new ParkingCalculatorService();
        }


        public static IEnumerable<object[]> NightRateDates
        {
            get
            {
                return new[]
                {
                // July 24 2020 is a Friday, July 25 is a Saturday
                new object[] { DateTime.Parse("7/24/2020 8:30:52 PM"), DateTime.Parse("7/25/2020 4:40:52 PM") },
                new object[] { DateTime.Parse("7/24/2020 6:00:00 PM"), DateTime.Parse("7/25/2020 3:30:00 PM") },
                new object[] { DateTime.Parse("7/24/2020 6:00:00 PM"), DateTime.Parse("7/25/2020 11:29:59 PM") },
                new object[] { DateTime.Parse("7/24/2020 11:59:59 PM"), DateTime.Parse("7/25/2020 11:29:59 PM") },
                new object[] { DateTime.Parse("7/24/2020 11:59:59 PM"), DateTime.Parse("7/25/2020 3:30:00 PM") },
                new object[] { DateTime.Parse("7/24/2020 8:10:20 PM"), DateTime.Parse("7/25/2020 5:38:45 PM") },
                };
            }
        }

        public static IEnumerable<object[]> NotNightRateDates
        {
            get
            {
                return new[]
                {
                new object[] { DateTime.Parse("7/24/2020 4:30:52 PM"), DateTime.Parse("7/24/2020 4:40:52 PM") },
                new object[] { DateTime.Parse("7/24/1982 6:00:00 AM"), DateTime.Parse("7/24/1982 3:29:59 PM") },
                new object[] { DateTime.Parse("7/24/2020 6:00:00 PM"), DateTime.Parse("7/24/2020 11:29:59 PM") },
                new object[] { DateTime.Parse("7/24/2020 12:00:00 AM"), DateTime.Parse("7/25/2020 11:29:59 PM") },
                new object[] { DateTime.Parse("7/23/2020 11:59:59 PM"), DateTime.Parse("7/24/2020 3:29:59 PM") },
                new object[] { DateTime.Parse("7/24/2020 8:10:20 PM"), DateTime.Parse("7/25/2020 11:30:00 PM") },
                //night rate not eligible for weekends
                new object[] { DateTime.Parse("7/25/2020 8:10:20 PM"), DateTime.Parse("7/26/2020 5:38:45 PM") },
                };
            }
        }

        [Theory, MemberData(nameof(NightRateDates))]
        public void ShouldGetNightRate(DateTime entry, DateTime exit)
        {
            var rate = service.CalculateParkingRate(entry, exit);
            Assert.Equal(nightRate, rate);
        }

        [Theory, MemberData(nameof(NotNightRateDates))]
        public void ShouldNotGetNightRate(DateTime entry, DateTime exit)
        {
            var rate = service.CalculateParkingRate(entry, exit);
            Assert.NotEqual(nightRate, rate);
        }
    }
}
