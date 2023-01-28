using FlightListener;

var writer = new AircraftConsoleWriter();
var filter = new MapFilter();
var radar = new AircraftRadar(writer, filter);
await radar.StartLookingForPlanes();

/*
var client = new OpenSkyNetworkClient();

var states = await client.GetStates(
    59.660528F, 10.042022F,
    59.973843F, 11.113796F);

foreach (var state in states)
{
    Console.WriteLine("=== AIRCRAFT ===");
    Console.WriteLine($"LAT: {state.Latitude}");
    Console.WriteLine($"LNG: {state.Longitude}");
    Console.WriteLine($"SGN: {state.Callsign}");
}
Console.WriteLine("Response");*/