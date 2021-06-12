using System;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using AirportClient.AirportService;
using AirportClient.Utils;
using AirportCommons;

namespace AirportClient
{
    internal static class Program
    {
        public static void Main()
        {
            var client = new AirportServiceClient();
            for (;;)
            {
                Console.WriteLine("Choose option:");
                Console.WriteLine("1). Find flights");
                Console.WriteLine("2). Quit");
                var operationChoice = GetOperationChoice();
                if (operationChoice == 2) break;

                var departureCity = GetCity("departure");
                var arrivalCity = GetCity("arrival");
                var dateTimeChoice = GetDateTimeChoice();
                DateTimeRange dateTimeRange = null;
                if (dateTimeChoice.Equals("y"))
                {
                    var startDate = GetDateTime("departure");
                    var endDate = GetDateTime("departure");
                    dateTimeRange = new DateTimeRange
                    {
                        StartDate = startDate,
                        EndDate = endDate
                    };
                }

                try
                {
                    foreach (var (item, index) in client.GetConnections(new FlightRequest
                    {
                        DepartureCity = departureCity,
                        ArrivalCity = arrivalCity,
                        DateTimeRange = dateTimeRange
                    }).WithIndex())
                    {
                        Console.WriteLine("Connection #" + index + ":");
                        item.ToList().ForEach(PrintFlight);
                        Console.WriteLine();
                    }
                }
                catch (FaultException<ErrorResponse> ex)
                {
                    Console.WriteLine(
                        "Server responded with error message: " + ex.Detail.Type + ":" + ex.Detail.Message);
                }
            }

            client.Close();
        }

        private static string GetCity(string variant)
        {
            for (;;)
            {
                Console.WriteLine("Type " + variant + " city: ");
                var city = Console.ReadLine();
                if (!city.IsNullOrBlank()) return city?.Trim();
                Console.WriteLine("City not provided");
            }
        }

        private static DateTime GetDateTime(string variant)
        {
            DateTime dateTime;
            for (;;)
            {
                Console.WriteLine("Type " + variant + " datetime (yyyy-MM-dd HH:mm format): ");
                if (DateTime.TryParseExact(Console.ReadLine(),
                    "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture, DateTimeStyles.None,
                    out dateTime))
                    return dateTime;
                Console.WriteLine("Not a valid datetime");
            }
        }

        private static int GetOperationChoice()
        {
            for (;;)
            {
                var choice = int.TryParse(Console.ReadLine(), out var outValue)
                    ? outValue
                    : default;
                if ((choice == 1) | (choice == 2)) return choice;
                Console.WriteLine("Wrong option");
            }
        }

        private static string GetDateTimeChoice()
        {
            for (;;)
            {
                Console.WriteLine("Set datetime range? (y/n): ");
                var datetimeChoice = Console.ReadLine();
                if (!datetimeChoice.IsNullOrBlank() &&
                    (datetimeChoice.ToLower().Equals("y") || datetimeChoice.ToLower().Equals("n")))
                    return datetimeChoice.ToLower().Trim();
                Console.WriteLine("Wrong option");
            }
        }

        private static void PrintFlight(Flight flight)
        {
            Console.WriteLine("Departure city: " + flight.DepartureCity +
                              ", departure date: " + flight.DepartureDate +
                              ", arrival city: " + flight.ArrivalCity +
                              ", arrival date: " + flight.ArrivalDate);
        }
    }
}