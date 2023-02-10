using FlightListener;
using Microsoft.AspNetCore.SignalR;

namespace AvinorStatusPanel.Hubs;

public class FlightHub : Hub
{
    private readonly IPlaneSpotter _aircraftStore;

    public FlightHub(IPlaneSpotter aircraftStore)
    {
        _aircraftStore = aircraftStore;
    }

    public async Task RequestFlights()
    {
        await Clients.All.SendAsync("UpdateFlightsView", _aircraftStore.SpottableFlights);
    }
}
