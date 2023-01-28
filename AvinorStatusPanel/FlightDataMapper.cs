using AvinorFlydataClient;
using AvinorFlydataClient.Model;
using AvinorStatusPanel.ViewModels;

namespace AvinorStatusPanel;

public class FlightDataMapper
{
    private readonly FlydataClient _client;
    private readonly FlightStatusCache _statusCache;

    public FlightDataMapper(FlydataClient client, AirlineNameCache airlineCache, FlightStatusCache statusCache)
    {
        _client = client;
        _statusCache = statusCache;
    }

    public async Task<List<TableEntryViewModel>> Map(IEnumerable<Flight> flights)
    {
        var viewModels = new List<TableEntryViewModel>();
        foreach (var flight in flights)
        {
            var airportName = await _client.GetAirportName(flight.Airport);
            var time = flight.Schedule_time.ToLocalTime().ToString("HH:mm");
            var statusCode = await GetStatusCode(flight.Status);
            
            viewModels.Add(new TableEntryViewModel()
            {
                Destination = airportName.Name,
                Flight = flight.Flight_id,
                Gate = flight.Gate,
                Time = time,
                Iata = flight.Airline,
                Status = statusCode
            });
        }

        return viewModels;
    }

    private async Task<string> GetStatusCode(Status status)
    {
        if (status == null)
            return "";

        var statusText = await _statusCache.GetFlightStatus(status.Code);

        if (status.Code == "E")
        {
            return $"{statusText.StatusTextEn} {status.Time.ToLocalTime():HH:mm}";
        }
            
        return statusText.StatusTextEn;
    }
}