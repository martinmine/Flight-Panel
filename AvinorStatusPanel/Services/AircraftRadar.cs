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

    public AircraftRadar(IPlaneSpotter planeSpotter, MapFilter mapFilter, OpenSkyNetworkClient client)
    {
        _planeSpotter = planeSpotter;
        _mapFilter = mapFilter;
        _client = client;
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
       
        await _planeSpotter.NotifyViewablePlane(viewablePlanes);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await StartLookingForPlanes();
       /* await Task.Delay(10000);
        await _planeSpotter.NotifyViewablePlane(new List<Aircraft>
            { new() { Callsign = "SA213", Lng = 12, Lat = 23 } });*/
    }
}