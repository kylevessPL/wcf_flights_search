namespace AirportService.Utils
{
    public class NoConnectionsFoundException : AirportException
    {
        public NoConnectionsFoundException() : base("No connections found")
        {
        }
    }
}