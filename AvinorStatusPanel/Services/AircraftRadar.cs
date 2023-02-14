using System.Text.Json;
using FlightListener;
using FlightListener.Model;
using FlightListener.OpenSkyNetwork;

namespace AvinorStatusPanel.Services;

public class AircraftRadar : BackgroundService
{
    private readonly PeriodicTimer _timer;
    private readonly OpenSkyNetworkClient _client;
    private readonly IPlaneSpotter _planeSpotter;
    private readonly MapFilter _mapFilter;
    private readonly FlightInfoDecorator _flightInfoDecorator;

    public AircraftRadar(IPlaneSpotter planeSpotter, MapFilter mapFilter, OpenSkyNetworkClient client, FlightInfoDecorator flightInfoDecorator)
    {
        _planeSpotter = planeSpotter;
        _mapFilter = mapFilter;
        _client = client;
        _flightInfoDecorator = flightInfoDecorator;
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(15));
    }

    private async Task StartLookingForPlanes()
    {
        do
        {
            try
            {
                await LookForPlanes();
            }
            catch (Exception e)
            {
                // TODO: Log this instead
                Console.WriteLine(e);
            }
        } while (await _timer.WaitForNextTickAsync());
    }

    private async Task LookForPlanes()
    {
        var planesAroundOslo = await _client.GetStates(
            59.660528F, 10.042022F,
            59.973843F, 11.113796F);

        var mapper = new AircraftMapper();
        var mapped = mapper.Map(planesAroundOslo);
        
        var viewablePlanes = _mapFilter.IsWithinInterestArea(mapped);

        foreach (var flight in viewablePlanes)
        {
            await _flightInfoDecorator.AddMoreFlightInfo(flight);
        }
       
        await _planeSpotter.NotifyViewablePlane(viewablePlanes);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        /*var flight = new Aircraft() { Icao24 = "47a039", FlightIcao24 = "NOZ941" };
        
        await _flightInfoDecorator.AddMoreFlightInfo(flight);
        var json = System.Text.Json.JsonSerializer.Serialize(flight);
        Console.WriteLine(json);
*/
    /*    var fakePlaneJson = "{\"FlightIcao24\":\"NOZ941\",\"Icao24\":\"47a039\",\"FlightNumber\":\"DY941\",\"Lat\":0,\"Lng\":0,\"Origin\":\"Copenhagen\",\"Destination\":\"Oslo\",\"DepartureTime\":\"2023-02-10T15:30:00\",\"ArrivalTime\":\"2023-02-10T16:45:00\"}";
        var fakePlane = JsonSerializer.Deserialize<Aircraft>(fakePlaneJson);
        //await Task.Delay(10000);
        await _planeSpotter.NotifyViewablePlane(new List<Aircraft>
            { fakePlane });*/
        await StartLookingForPlanes();
        /* await Task.Delay(10000);
         await _planeSpotter.NotifyViewablePlane(new List<Aircraft>
             { new() { Callsign = "SA213", Lng = 12, Lat = 23 } });*/
    }
}