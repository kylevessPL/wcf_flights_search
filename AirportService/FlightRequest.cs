using System.Runtime.Serialization;
using AirportService.Utils;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace AirportService
{
    [DataContract]
    public class FlightRequest
    {
        [DataMember]
        [NotNullValidator(MessageTemplate = "Departure city is required")]
        [StringLengthValidator(
            2, RangeBoundaryType.Inclusive,
            85, RangeBoundaryType.Inclusive,
            MessageTemplate = "Departure city must be between 2 and 85 characters long")]
        public string DepartureCity { get; set; }

        [DataMember]
        [NotNullValidator(MessageTemplate = "Arrival city is required")]
        [StringLengthValidator(
            2, RangeBoundaryType.Inclusive,
            85, RangeBoundaryType.Inclusive,
            MessageTemplate = "Arrival city must be between 2 and 85 characters long")]
        public string ArrivalCity { get; set; }

        [DataMember] public DateTimeRange DateTimeRange { get; set; }
    }
}