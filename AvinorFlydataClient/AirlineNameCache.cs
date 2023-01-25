using AvinorFlydataClient.Model;

namespace AvinorFlydataClient;

public class AirlineNameCache
{
    private readonly Dictionary<string, string> _airlineNames;
    private readonly FlydataClient _client;

    public AirlineNameCache(FlydataClient client)
    {
        _client = client;
        _airlineNames = new Dictionary<string, string>();
    }

    public async Task<string> GetAirlineName(string code)
    {
        lock (_airlineNames)
        {
            if (_airlineNames.TryGetValue(code, out var coldAirlineName))
                return coldAirlineName;
        }

        var airlines = await _client.GetAllAirlineNames();
        lock (_airlineNames)
        {
            foreach (var airline in airlines)
            {
                _airlineNames[airline.Code] = airline.Name;
            }
        }

        return airlines.SingleOrDefault(airline => airline.Code == code)?.Name;
    }
}