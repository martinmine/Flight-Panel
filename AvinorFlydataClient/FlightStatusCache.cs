using AvinorFlydataClient.Model;

namespace AvinorFlydataClient;

public class FlightStatusCache
{
    private readonly FlydataClient _client;
    private readonly Dictionary<string, FlightStatus> _cache;

    public FlightStatusCache(FlydataClient client)
    {
        _client = client;
        _cache = new Dictionary<string, FlightStatus>();
;    }
    
    public async Task<FlightStatus> GetFlightStatus(string code)
    {
        lock (_cache)
        {
            if (_cache.TryGetValue(code, out var coldStatus))
                return coldStatus;
        }

        var status = await _client.GetFlightStatus(code);

        lock (_cache)
        {
            _cache[code] = status;
        }

        return status;
    }
}