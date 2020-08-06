using CarParkApi.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class EarlyBirdTests
    {
        private ParkingCalculatorService service;
        private readonly decimal earlyBirdRate = 13m;
        public EarlyBirdTests()
        {
            service = new ParkingCalculatorService();
        }

        public static IEnumerable<object[]> EarlyBirdDates
        {
            get
            {
                return new[]
                {
                new object[] { DateTime.Parse("5/1/2020 8:30:52 AM"), DateTime.Parse("5/1/2020 4:40:52 PM") },
                new object[] { DateTime.Parse("5/1/2020 6:00:00 AM"), DateTime.Parse("5/1/2020 3:30:00 PM") },
                new object[] { DateTime.Parse("5/1/2020 6:00:00 AM"), DateTime.Parse("5/1/2020 11:29:59 PM") },
                new object[] { DateTime.Parse("5/1/2020 8:59:59 AM"), DateTime.Parse("5/1/2020 11:29:59 PM") },
                new object[] { DateTime.Parse("5/1/2020 8:59:59 AM"), DateTime.Parse("5/1/2020 3:30:00 PM") },
                new object[] { DateTime.Parse("5/1/2020 8:10:20 AM"), DateTime.Parse("5/1/2020 5:38:45 PM") },
                };
            }
        }

        public static IEnumerable<object[]> NotEarlyBirdDates
        {
            get
            {
                return new[]
                {
                // incorrect times
                new object[] { DateTime.Parse("5/1/2020 10:30:52 AM"), DateTime.Parse("5/1/2020 4:40:52 PM") },
                new object[] { DateTime.Parse("5/1/2020 6:00:00 AM"), DateTime.Parse("5/1/2020 3:29:59 PM") },
                new object[] { DateTime.Parse("5/1/2020 5:59:59 AM"), DateTime.Parse("5/1/2020 11:30:00 PM") },
                new object[] { DateTime.Parse("5/1/2020 9:00:01 AM"), DateTime.Parse("5/1/2020 11:30:00 PM") },
                new object[] { DateTime.Parse("5/1/2020 9:00:00 AM"), DateTime.Parse("5/1/2020 11:30:01 PM") },
                new object[] { DateTime.Parse("5/1/2020 5:10:20 AM"), DateTime.Parse("5/1/2020 5:38:45 PM") },

                //incorrect dates
                new object[] { DateTime.Parse("5/1/2020 8:30:52 AM"), DateTime.Parse("5/2/2020 4:40:52 PM") },
                new object[] { DateTime.Parse("5/1/2020 6:00:00 AM"), DateTime.Parse("5/2/2020 3:30:00 PM") },
                new object[] { DateTime.Parse("5/1/2020 6:00:00 AM"), DateTime.Parse("5/2/2020 11:30:00 PM") },
                new object[] { DateTime.Parse("5/1/2020 8:59:59 AM"), DateTime.Parse("5/2/2020 11:30:00 PM") },
                new object[] { DateTime.Parse("5/1/2020 9:00:00 AM"), DateTime.Parse("5/2/2020 3:30:00 PM") },
                new object[] { DateTime.Parse("5/1/2020 8:10:20 AM"), DateTime.Parse("5/2/2020 5:38:45 PM") },
                new object[] { DateTime.Parse("5/1/2020 8:10:20 AM"), DateTime.Parse("5/2/2020 5:38:45 PM") },
                };
            }
        }

        [Theory, MemberData(nameof(EarlyBirdDates))]

        public void ShouldGetEarlyBirdRate(DateTime entry, DateTime exit)
        {
            var rate = service.CalculateParkingRate(entry, exit);
            Assert.Equal(earlyBirdRate, rate);
        }

        [Theory, MemberData(nameof(NotEarlyBirdDates))]
        public void ShouldNotGetEarlyBirdRate(DateTime entry, DateTime exit)
        {
            var rate = service.CalculateParkingRate(entry, exit);
            Assert.NotEqual(earlyBirdRate, rate);
        }
    }
}
