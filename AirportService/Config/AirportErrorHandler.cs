using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using AirportService.Utils;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;

namespace AirportService.Config
{
    public class AirportErrorHandler : IErrorHandler
    {
        public bool HandleError(Exception error)
        {
            Console.Error.WriteLine(error.ToString());
            return true;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            switch (error)
            {
                case FaultException<ValidationFault> validationException:
                {
                    var faultException = new FaultException<ErrorResponse>(new ErrorResponse
                    {
                        Type = ErrorType.Validation,
                        Message = string.Join(", ", validationException.Detail.Details
                            .Select(x => x.Message)
                            .ToArray())
                    });
                    var messageFault = faultException.CreateMessageFault();
                    fault = Message.CreateMessage(version, messageFault, faultException.Action);
                    break;
                }
                case ArgumentException _:
                {
                    var faultException = new FaultException<ErrorResponse>(new ErrorResponse
                    {
                        Type = ErrorType.Validation,
                        Message = "Bad arguments provided"
                    });
                    var messageFault = faultException.CreateMessageFault();
                    fault = Message.CreateMessage(version, messageFault, faultException.Action);
                    break;
                }
                case AirportException airportException:
                {
                    var faultException = new FaultException<ErrorResponse>(new ErrorResponse
                    {
                        Type = ErrorType.Business,
                        Message = airportException.Message
                    });
                    var messageFault = faultException.CreateMessageFault();
                    fault = Message.CreateMessage(version, messageFault, faultException.Action);
                    break;
                }
            }
        }
    }
}