using System.Timers;

namespace FlightListener;

public class AircraftRadar
{
    private readonly PeriodicTimer _timer;
    private readonly OpenSkyNetworkClient _client;
    private readonly IPlaneSpotter _planeSpotter;

    public AircraftRadar(IPlaneSpotter planeSpotter)
    {
        _planeSpotter = planeSpotter;
        _client = new OpenSkyNetworkClient();
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(15));

        LookForPlanes();
    }

    public async Task StartLookingForPlanes()
    {
        while (await _timer.WaitForNextTickAsync())
        {
            try
            {
                await LookForPlanes();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    private async Task LookForPlanes()
    {
        var planesAroundOslo = await _client.GetStates(
            59.660528F, 10.042022F,
            59.973843F, 11.113796F);

        var viewablePlanes = MapFilter.IsWithinInterestArea(planesAroundOslo);
        if (viewablePlanes.Any())
        {
            _planeSpotter.NotifyViewablePlane(viewablePlanes);
        }
    }
}