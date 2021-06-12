using System.Runtime.Serialization;

namespace AirportService.Utils
{
    [DataContract]
    public class ErrorResponse
    {
        [DataMember] public ErrorType Type { get; set; }

        [DataMember] public string Message { get; set; }
    }
}