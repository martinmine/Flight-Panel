using System.Timers;
using FlightListener.OpenSkyNetwork;

namespace FlightListener;

public class AircraftRadar
{
    private readonly PeriodicTimer _timer;
    private readonly OpenSkyNetworkClient _client;
    private readonly IPlaneSpotter _planeSpotter;
    private readonly MapFilter _mapFilter;

    public AircraftRadar(IPlaneSpotter planeSpotter, MapFilter mapFilter)
    {
        _planeSpotter = planeSpotter;
        _mapFilter = mapFilter;
        _client = new OpenSkyNetworkClient();
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(15));
    }

    public async Task StartLookingForPlanes()
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
        if (viewablePlanes.Any())
        {
            _planeSpotter.NotifyViewablePlane(viewablePlanes);
        }
    }
}