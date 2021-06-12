using System;
using System.Collections.Generic;
using System.Linq;
using AirportCommons;
using AirportService.Utils;

namespace AirportService
{
    public class AirportService : IAirportService
    {
        private const string CsvDatabasePath = "/Data/flights.csv";

        public IEnumerable<IEnumerable<Flight>> GetConnections(FlightRequest request)
        {
            Func<Flight, bool> predicate = _ => true;
            if (request.DateTimeRange != null) predicate.And(FlightUtils.BuildDateTimePredicate(request.DateTimeRange));
            var database = FlightUtils.ReadCsv(CsvDatabasePath)
                .OrderBy(x => x.DepartureDate)
                .Where(predicate).ToList();
            if (!database.Any(s =>
                string.Equals(s.DepartureCity, request.DepartureCity, StringComparison.OrdinalIgnoreCase)))
                throw new CityNotFoundException(request.DepartureCity);
            if (!database.Any(
                s => string.Equals(s.ArrivalCity, request.ArrivalCity, StringComparison.OrdinalIgnoreCase)))
                throw new CityNotFoundException(request.ArrivalCity);
            return FlightUtils.GetAllMatchingConnections(database, request.DepartureCity, request.ArrivalCity)
                .IfEmpty(x => throw new NoConnectionsFoundException());
        }
    }
}