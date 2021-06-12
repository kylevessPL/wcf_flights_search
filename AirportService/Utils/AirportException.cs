using System;

namespace AirportService.Utils
{
    public class AirportException : Exception
    {
        public AirportException()
        {
        }

        protected AirportException(string message) : base(message)
        {
        }
    }
}