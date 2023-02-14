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
        FlightIcao24 = aircraft.Callsign.Trim(),
        Icao24 = aircraft.Icao24,
        Lat = aircraft.Latitude,
        Lng = aircraft.Longitude
    };
}