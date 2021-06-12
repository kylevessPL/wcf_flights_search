using System;
using System.Collections.Generic;
using System.Linq;
using AirportCommons;
using AirportService.Utils;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

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
                .Where(predicate)
                .OrderBy(x => x.DepartureDate).ToList();
            if (!database.Any(s =>
                string.Equals(s.DepartureCity, request.DepartureCity, StringComparison.OrdinalIgnoreCase)))
                throw new CityNotFoundException(request.DepartureCity);
            if (!database.Any(
                s => string.Equals(s.ArrivalCity, request.ArrivalCity, StringComparison.OrdinalIgnoreCase)))
                throw new CityNotFoundException(request.ArrivalCity);
            IEnumerable<IEnumerable<Flight>> result = new List<IEnumerable<Flight>>();
            database.Where(flight =>
                    flight.DepartureCity.Equals(request.DepartureCity, StringComparison.OrdinalIgnoreCase)).WithIndex()
                .ForEach(item =>
                {
                    var (_, index) = item;
                    var subList = database.GetRange(index, database.Count - index);
                    result = result.Concat(
                        FlightUtils.GetAllFlightConnections(subList, subList.Count, request.ArrivalCity));
                });
            if (result.IsNullOrEmpty()) throw new NoConnectionsFoundException();
            return result;
        }
    }
}