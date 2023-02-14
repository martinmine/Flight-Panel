using System.Collections.Concurrent;
using System.Net.Http.Json;

namespace AirLabs;

public class AirLabsClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly IDictionary<string, (DateTime timeAdded, Flight flight)> _flightCache;
    
    private const int CacheTtlInMinutes = 15;
    
    public AirLabsClient(string apiKey)
    {
        _apiKey = apiKey;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://airlabs.co/api/v9/");
        _flightCache = new ConcurrentDictionary<string, (DateTime timeAdded, Flight flight)>();
    }

    public async Task<Flight> GetFlight(string flightIcao24)
    {
        var query = $"flight?flight_icao={flightIcao24.ToUpper()}&api_key={_apiKey}";

        if (_flightCache.TryGetValue(query, out var cacheHit))
        {
            var timeInCache = DateTime.UtcNow - cacheHit.timeAdded;
            if (timeInCache.TotalMinutes < CacheTtlInMinutes)
                return cacheHit.flight;
        }

        var response = await _httpClient.GetAsync(query);
        var flight = await response.Content.ReadFromJsonAsync<ResponseContainer>();

        _flightCache.Add(query, (DateTime.UtcNow, flight?.response));
        
        return flight?.response;
    }
}

public class ResponseContainer
{
    public Flight response { get; set; }
}

public class Flight
{
    /// <summary>
    /// Aircraft ICAO type.
    /// </summary>
    public string aircraft_icao { get; set; }
    
    /// <summary>
    /// ISO 2 country code from Countries DB.
    /// </summary>
    public string flag { get; set; } 
    
    /// <summary>
    /// Aircraft Geo-Latitude for now.
    /// </summary>
    public double lat { get; set; } 
    
    /// <summary>
    /// Aircraft Geo-Longitude for now.
    /// </summary>
    public double lng { get; set; } 
    
    /// <summary>
    /// Aircraft head direction for now.
    /// </summary>
    public int dir { get; set; } 
    
    /// <summary>
    /// Airline IATA code.
    /// </summary>
    public string airline_iata { get; set; } 
    
    /// <summary>
    /// Flight IATA code-number.
    /// </summary>
    public string flight_iata { get; set; } 
    
    /// <summary>
    /// Flight number only.
    /// </summary>
    public string flight_number { get; set; } 
    
    /// <summary>
    /// Departure airport IATA code.
    /// </summary>
    public string dep_iata { get; set; } 
    
    /// <summary>
    /// Departure time in the airport time zone.
    /// </summary>
    public string dep_time { get; set; } 
    
    /// <summary>
    /// Arrival airport IATA code.
    /// </summary>
    public string arr_iata { get; set; }
    
    /// <summary>
    /// Arrival time in the airport time zone.
    /// </summary>
    public string arr_time { get; set; } 
    
    /// <summary>
    /// Aircraft manufacturer name.
    /// </summary>
    public string manufacturer { get; set; } 
    
    public string dep_city { get; set; }
    public string arr_city { get; set; }
}