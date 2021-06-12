using System.Collections.Generic;
using System.ServiceModel;
using AirportService.Utils;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;

namespace AirportService
{
    [ServiceContract]
    [ValidationBehavior]
    public interface IAirportService
    {
        [OperationContract]
        [FaultContract(typeof(ErrorResponse))]
        IEnumerable<IEnumerable<Flight>> GetConnections(FlightRequest request);
    }
}