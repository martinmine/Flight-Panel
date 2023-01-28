using System.Text.Json;

namespace FlightListener.OpenSkyNetwork;

public class StateResponse
{
    public long Time { get; set; }
    public IEnumerable<JsonElement[]> States { get; set; }
}