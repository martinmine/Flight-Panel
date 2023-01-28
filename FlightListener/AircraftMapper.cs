using FlightListener.Model;
using FlightListener.OpenSkyNetwork;

namespace FlightListener;

public class AircraftMapper
{
    public List<Aircraft> Map(IEnumerable<State> aircrafts)
    {
        return aircrafts != null ? aircrafts.Select(Map).ToList() : new List<Aircraft>();
    }

    private Aircraft Map(State aircraft) => new()
    {
        Callsign = aircraft.Callsign,
        Lat = aircraft.Latitude,
        Lng = aircraft.Longitude
    };
}