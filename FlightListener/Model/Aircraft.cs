namespace FlightListener.Model;

public class Aircraft
{
    public string FlightIcao24 { get; set; }
    public string Icao24 { get; set; } 
    public string FlightNumber { get; set; }
    public double Lat { get; set; }
    public double Lng { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public DateTime? DepartureTime { get; set; }
    public DateTime? ArrivalTime { get; set; }

}