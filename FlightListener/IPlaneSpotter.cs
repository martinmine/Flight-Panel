using FlightListener.Model;

namespace FlightListener;

public interface IPlaneSpotter
{
    Dictionary<string, Aircraft> SpottableFlights { get; }
    public Task NotifyViewablePlane(IList<Aircraft> planes);
}