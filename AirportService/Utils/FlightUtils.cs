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

        public static IEnumerable<IEnumerable<Flight>> GetAllMatchingConnections(
            IEnumerable<Flight> flights,
            string departureCity, string arrivalCity)
        {
            var connections = new List<IEnumerable<Flight>>();
            if (flights == null)
                return connections;
            var enumerable = flights.ToList();
            var maxLength = enumerable.Count;
            var stack = new Stack<int>(maxLength);
            var i = 0;
            while (stack.Count > 0 || i < enumerable.Count)
                if (i < enumerable.Count)
                {
                    if (stack.Count == maxLength)
                        i = stack.Pop() + 1;
                    stack.Push(i++);
                    if (!string.Equals(enumerable[stack.Last()].DepartureCity, departureCity,
                        StringComparison.OrdinalIgnoreCase))
                        continue;
                    var list = stack
                        .Reverse()
                        .Select(index => enumerable[index])
                        .TakeUntil(x => x.ArrivalCity.Equals(arrivalCity, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                    if (list.Where((item, index) =>
                            index == 0 ||
                            !item.ArrivalCity.Equals(departureCity, StringComparison.OrdinalIgnoreCase) &&
                            item.DepartureCity.Equals(list[index - 1].ArrivalCity) &&
                            item.DepartureDate >= list[index - 1].ArrivalDate &&
                            item.DepartureDate <= list[index - 1].ArrivalDate.AddDays(1)).Count().Equals(list.Count) &&
                        list.LastOrDefault().ArrivalCity.Equals(arrivalCity, StringComparison.OrdinalIgnoreCase))
                        connections.Add(list);
                }
                else
                {
                    i = stack.Pop() + 1;
                    if (stack.Count > 0)
                        i = stack.Pop() + 1;
                }

            return connections.Select(x => new HashSet<Flight>(x))
                .Distinct(HashSet<Flight>.CreateSetComparer());
        }
    }
}