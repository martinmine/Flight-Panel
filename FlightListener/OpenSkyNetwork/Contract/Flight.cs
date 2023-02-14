namespace FlightListener.OpenSkyNetwork;

public class Flight
{
    /// <summary>
    /// Unique ICAO 24-bit address of the transponder in hex string representation. All letters are lower case.
    /// </summary>
    public string Icao24 { get; set; }
    
    /// <summary>
    /// Callsign of the vehicle (8 chars). Can be null if no callsign has been received. If the vehicle transmits multiple callsigns during the flight, we take the one seen most frequently.
    /// </summary>
    public string Callsign { get; set; }
    
    /// <summary>
    /// ICAO code of the estimated departure airport. Can be null if the airport could not be identified.
    /// </summary>
    public string EstDepartureAirport { get; set; }
    
    /// <summary>
    /// ICAO code of the estimated arrival airport. Can be null if the airport could not be identified.
    /// </summary>
    public string EstArrivalAirport { get; set; }
    
    /// <summary>
    /// Estimated time of arrival for the flight as Unix time (seconds since epoch).
    /// </summary>
    public long LastSeen { get; set; }
    
    /// <summary>
    /// Estimated time of departure for the flight as Unix time (seconds since epoch).
    /// </summary>
    public long FirstSeen { get; set; }
}