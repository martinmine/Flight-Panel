using AvinorStatusPanel.Hubs;
using FlightListener.Model;
using Microsoft.AspNetCore.SignalR;

namespace FlightListener;

public class SpottableAicraftStore : IPlaneSpotter
{
    public Dictionary<string, Aircraft> SpottableFlights { get; private set; }
    private IHubContext<FlightHub> _flightHub;

    public SpottableAicraftStore(IHubContext<FlightHub> flightHub)
    {
        SpottableFlights = new();
      /*  {
            { "DY1503", new Aircraft() { Callsign = "DY1503", Lng = 1.23, Lat = 4.56 } },
            { "QR8447", new Aircraft() { Callsign = "QR8447", Lng = 4.46, Lat = 7.89 } }
        };*/

        _flightHub = flightHub;
    }

    public async Task NotifyViewablePlane(IList<Aircraft> planes)
    {
        var mappedList = planes.ToDictionary(key => key.FlightIcao24, value => value);

        var containsNewValues = mappedList.Keys.Except(SpottableFlights.Keys).Any();
        var hasRemovedFlights = SpottableFlights.Keys.Except(mappedList.Keys).Any();

        if (containsNewValues || hasRemovedFlights)
        {
            SpottableFlights = mappedList;
        }
        
        if (containsNewValues)
        {
           await _flightHub.Clients.All.SendAsync("FastenSeatbelt");
        }

        if (containsNewValues || hasRemovedFlights)
        {
            await _flightHub.Clients.All.SendAsync("UpdateFlightsView", SpottableFlights);
            // Print state
            Console.WriteLine("Oh there are planes on the sky!");
        
            foreach (var plane in SpottableFlights.Values)
            {
                Console.WriteLine("=== AIRCRAFT ===");
                Console.WriteLine($"LAT: {plane.Lat}");
                Console.WriteLine($"LNG: {plane.Lng}");
                Console.WriteLine($"SGN: {plane.FlightIcao24}");
            }
        }
    }
}