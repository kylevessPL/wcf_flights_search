using System;
using System.Runtime.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace AirportService.Utils
{
    [DataContract]
    public class DateTimeRange
    {
        [DataMember]
        [NotNullValidator(MessageTemplate = "Start date is required")]
        public DateTime StartDate { get; set; }

        [DataMember]
        [NotNullValidator(MessageTemplate = "End date is required")]
        [PropertyComparisonValidator("StartDate", ComparisonOperator.GreaterThanEqual)]
        public DateTime EndDate { get; set; }
    }
}