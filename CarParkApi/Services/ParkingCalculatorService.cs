using System;
using System.Reflection.Metadata.Ecma335;

namespace CarParkApi.Services
{
    public class ParkingCalculatorService : IParkingCalculatorService
    {
        // I made the assumption that for early bird and night rates
        // you must enter and leave within a day. For the weekend I assumed
        // you must enter and exit on the same weekend.

        // these could be config values or stored in db
        private readonly int longestDaysAllowed = 365; //longest amount of days rate will be calculated for

        // Early Bird 
        private readonly TimeSpan earlyBirdEntryStart = new TimeSpan(6, 0, 0); //6AM
        private readonly TimeSpan earlyBirdEntryEnd = new TimeSpan(9, 0, 0); //9AM
        private readonly TimeSpan earlyBirdExitStart = new TimeSpan(15, 30, 0); //3:30PM
        private readonly TimeSpan earlyBirdExitEnd = new TimeSpan(23, 30, 0); //11:30PM
        private readonly decimal earlyBirdRate = 13m;

        // Night
        private readonly TimeSpan nightEntryStart = new TimeSpan(18, 0, 0); //6PM
        private readonly TimeSpan nightExitStart = new TimeSpan(15, 30, 0); //3:30PM
        private readonly TimeSpan nightExitEnd = new TimeSpan(23, 30, 0); //11:30PM
        private readonly decimal nightRate = 6.5m;

        // weekend
        private readonly decimal weekendRate = 10.0m;

        public decimal CalculateParkingRate(DateTime entry, DateTime exit)
        {
            // If exit time is after entry time throw an exception
            if (entry > exit)
            {
                throw new ArgumentException("Exit time cannot be after entry. Please enter valid times.");
            }

            // if car has been parked in carpark over allowed number of days throw an exception
            if ((exit - entry).TotalDays > longestDaysAllowed)
            {
                throw new Exception($"Car has been parked in carpark for over {longestDaysAllowed} days. Please contact management.");
            }

            //Checks are in order from cheapest to most expensive options
            if (IsNightRateEligible(entry, exit))
            {
                return nightRate;
            }

            if (IsWeekendRateEligible(entry, exit))
            {
                return weekendRate;
            }

            if (IsEarlyBirdEligible(entry, exit))
            {
                return earlyBirdRate;
            }

            return CalculateStandardRate(entry, exit);
        }

        //returns true if car is eligible for night rate pricing -- false otherwise
        private bool IsNightRateEligible(DateTime entry, DateTime exit)
        {
            //check if car entered later than 6pm, and exited in the allowed times the next day
            return entry.TimeOfDay >= nightEntryStart
                && IsInTimeWindow(exit.TimeOfDay, nightExitStart, nightExitEnd) 
                && !IsWeekend(entry.DayOfWeek) // car must enter on a weekday
                && entry.Date.AddDays(1) == exit.Date; // exit must be exactly 1 day after entry
        }

        //returns true if car is eligible for early bird pricing -- false otherwise
        private bool IsEarlyBirdEligible(DateTime entry, DateTime exit)
        {
            return IsInTimeWindow(entry.TimeOfDay, earlyBirdEntryStart, earlyBirdEntryEnd) 
                && IsInTimeWindow(exit.TimeOfDay, earlyBirdExitStart, earlyBirdExitEnd) 
                && IsSameDay(entry.Date, exit.Date);
        }

        private bool IsWeekendRateEligible(DateTime entry, DateTime exit)
        {
            // to be weekend rate eligible car must enter and exit on a Saturday/Sunday of the same weekend
            // ie cannot enter Saturday July 7 and leave on Sunday 8 July 15
            return (IsWeekend(entry.DayOfWeek) && IsWeekend(exit.DayOfWeek) && exit.Day - entry.Day <= 1);
        }

        // calculating the standard rate of parking if no special rates are available
        private decimal CalculateStandardRate(DateTime entry, DateTime exit)
        {
            double total = 0;

            var timeDiff = exit - entry;

            // add $20 for each day parked
            var totalDays = Math.Floor((exit - entry).TotalDays);
            total += (totalDays * 20);

            var leftOverHours = timeDiff.TotalHours % 24;
            
            // add rate to the amount of extra hours parked
            if (leftOverHours < 1)
                total += 5;
            else if (leftOverHours < 2)
                total += 10;
            else if (leftOverHours < 3)
                total += 15;
            else
                total += 20;

            //return total rounded to 2 decimal places
            return Math.Round((decimal)total, 2);
        }



        // HELPER FUNCTIONS
        // returns true is day of week is Saturday or Sunday
        private bool IsWeekend(DayOfWeek dayOfWeek) 
        {
            return dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday;
        }

        // function for checking if time of day is between 2 other times
        // must be in after or equal start time and out before end time
        private bool IsInTimeWindow(TimeSpan timeOfDay, TimeSpan start, TimeSpan end)
        {
            var isBetween = timeOfDay >= start && timeOfDay < end;                
            return isBetween;
        }

        //function for checking if 2 dates are the same date
        private bool IsSameDay(DateTime start, DateTime end)
        {
            var equal = start == end;
            return equal;
        }
    }
}
