using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AirportCommons;

namespace AirportService.Utils
{
    public static class FlightUtils
    {
        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        public static IEnumerable<Flight> ReadCsv(string path)
        {
            return File.ReadAllLines(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + path)
                .Skip(1)
                .Select(x => x.Split(','))
                .Select(x => new Flight
                {
                    DepartureCity = x[0],
                    DepartureDate = DateTime.ParseExact(x[1], DateTimeFormat, CultureInfo.InvariantCulture),
                    ArrivalCity = x[2],
                    ArrivalDate = DateTime.ParseExact(x[3], DateTimeFormat, CultureInfo.InvariantCulture)
                });
        }

        public static Func<Flight, bool> BuildDateTimePredicate(DateTimeRange dateTimeRange)
        {
            return flight =>
                flight.DepartureDate >= dateTimeRange.StartDate &&
                flight.ArrivalDate <= dateTimeRange.EndDate;
        }

        public static IEnumerable<IEnumerable<Flight>> GetAllFlightConnections(IEnumerable<Flight> source, int length,
            string targetCity)
        {
            if (length == 1) return source.Select(t => new[] {t});
            var comparables = source.ToList();
            return GetAllFlightConnections(comparables, length - 1, targetCity)
                .SelectMany(t => comparables
                        .Where(o => o.DepartureCity.Equals(t.Last().ArrivalCity, StringComparison.OrdinalIgnoreCase))
                        .TakeUntil(o => o.ArrivalCity.Equals(targetCity, StringComparison.OrdinalIgnoreCase)),
                    (t1, t2) => t1.Concat(new[] {t2}));
        }
    }
}