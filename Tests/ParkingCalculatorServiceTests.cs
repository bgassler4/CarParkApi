using CarParkApi.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class ParkingCalculatorServiceTests
    {
        private ParkingCalculatorService service;
        private readonly int longestDaysAllowed = 365;
        public ParkingCalculatorServiceTests()
        {
            service = new ParkingCalculatorService();
        }

        public static IEnumerable<object[]> MismatchedTimes
        {
            get
            {
                return new[]
                {
                new object[] { DateTime.Parse("5/1/2020 8:30:52 AM"), DateTime.Parse("3/1/2020 4:40:52 PM") },
                new object[] { DateTime.Parse("5/1/2020 6:00:00 AM"), DateTime.Parse("2/1/2020 3:30:00 PM") },
                new object[] { DateTime.Parse("5/1/2020 6:00:00 AM"), DateTime.Parse("2/1/2019 3:30:00 PM") },
                new object[] { DateTime.Parse("5/1/2020 12:00:00 AM"), DateTime.Parse("4/5/2020 11:59:59 PM") },
                new object[] { DateTime.Parse("5/1/2020 5:00:00 AM"), DateTime.Parse("5/1/2020 4:59:59 AM") },
                };
            }
        }

        public static IEnumerable<object[]> ParkedTooLongTimes
        {
            get
            {
                return new[]
                {
                new object[] { DateTime.Parse("5/1/2020 8:30:52 AM"), DateTime.Parse("6/1/2021 4:40:52 PM") },
                new object[] { DateTime.Parse("5/1/2020 6:00:00 AM"), DateTime.Parse("5/8/2021 3:30:00 PM") },
                new object[] { DateTime.Parse("5/1/2020 6:00:00 AM"), DateTime.Parse("5/1/2021 3:30:00 PM") },
                new object[] { DateTime.Parse("5/1/2020 5:00:00 AM"), DateTime.Parse("5/8/2021 5:01:00 AM") },
                new object[] { DateTime.Parse("5/1/2020 5:00:00 AM"), DateTime.Parse("5/1/2021 5:00:01 AM") },
                };
            }
        }

        [Theory, MemberData(nameof(MismatchedTimes))]
        public void EntryAfterExitShouldThrowError(DateTime entry, DateTime exit)
        {
            var exception = Assert.Throws<ArgumentException>(() => service.CalculateParkingRate(entry, exit));

            Assert.Equal("Exit time cannot be after entry. Please enter valid times.", exception.Message);
        }

        [Theory, MemberData(nameof(ParkedTooLongTimes))]
        public void ParkedOverOneYearShouldThrowError(DateTime entry, DateTime exit)
        {
            var exception = Assert.Throws<Exception>(() => service.CalculateParkingRate(entry, exit));

            Assert.Equal($"Car has been parked in carpark for over {longestDaysAllowed} days. Please contact management.", exception.Message);
        }

        
    }
}
