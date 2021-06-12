namespace AirportService.Utils
{
    public class CityNotFoundException : AirportException
    {
        public CityNotFoundException(string city) : base("City " + city + " not found")
        {
        }
    }
}