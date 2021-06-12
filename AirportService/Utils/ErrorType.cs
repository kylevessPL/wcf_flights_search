using System.Runtime.Serialization;

namespace AirportService.Utils
{
    [DataContract]
    public enum ErrorType
    {
        [EnumMember] Validation = 0,

        [EnumMember] Business = 1
    }
}