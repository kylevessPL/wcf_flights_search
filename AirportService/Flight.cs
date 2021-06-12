using System;
using System.Runtime.Serialization;

namespace AirportService
{
    [DataContract]
    public class Flight
    {
        [DataMember] public string DepartureCity { get; set; }

        [DataMember] public DateTime DepartureDate { get; set; }

        [DataMember] public string ArrivalCity { get; set; }

        [DataMember] public DateTime ArrivalDate { get; set; }
    }
}