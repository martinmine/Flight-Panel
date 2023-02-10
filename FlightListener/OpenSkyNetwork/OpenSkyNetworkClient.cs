using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace FlightListener.OpenSkyNetwork;

public class OpenSkyNetworkClient
{
    private readonly HttpClient _httpClient;

    public OpenSkyNetworkClient(string openSkyUsername, string openSkyPassword)
    {
        var authenticationBytes = Encoding.ASCII.GetBytes($"{openSkyUsername}:{openSkyPassword}");
        
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://opensky-network.org/api/");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
          Convert.ToBase64String(authenticationBytes));
    }

    public async Task<IEnumerable<State>> GetStates(double laMin, double loMin, double laMax, double loMax)
    {
        
        
        var query = $"states/all?lamin={laMin.ToString(CultureInfo.InvariantCulture)}&lomin={loMin.ToString(CultureInfo.InvariantCulture)}&lamax={laMax.ToString(CultureInfo.InvariantCulture)}&lomax={loMax.ToString(CultureInfo.InvariantCulture)}";
        Console.WriteLine("Querying planes");
        var response = await _httpClient.GetAsync(query);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"API responded with code {response.StatusCode}");
        }

        var responseContent = await response.Content.ReadFromJsonAsync<StateResponse>();

        //var fileJson = File.ReadAllText("MockedResponse.json");
        //var responseContent = JsonSerializer.Deserialize<StateResponse>(fileJson, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        if (responseContent.States == null)
            return Enumerable.Empty<State>();
            
        return Map(responseContent.States);
    }

    private static IEnumerable<State> Map(IEnumerable<JsonElement[]> statesResponse)
        => statesResponse.Select(Map).ToList();

    private static State Map(JsonElement[] obj)
    {
        if (obj.Length < 17)
            throw new ArgumentException($"Cannot parse JSON response as the required length {obj.Length} is not long enough");
        
        var state = new State
        {
            Icao24 = obj[0].GetNullableString(),
            Callsign = obj[1].GetNullableString(),
            OriginCountry = obj[2].GetNullableString(),
            TimePosition = obj[3].GetNullableInt32(),
            LastContact = obj[4].GetNullableInt32(),
            Longitude = obj[5].GetNullableDouble(),
            Latitude = obj[6].GetNullableDouble(),
            BaroAltitude = obj[7].GetNullableDouble(),
            OnGround = obj[8].GetNullableBoolean(),
            Velocity = obj[9].GetNullableDouble(),
            TrueTrack = obj[10].GetNullableDouble(),
            VerticalRate = obj[11].GetNullableDouble(),
            Sensors = null, // TODO
            GeoAltitude = obj[13].GetNullableDouble(),
            Squawk = obj[14].GetNullableString(),
            Spi = obj[15].GetNullableBoolean(),
            PositionSource = obj[16].GetNullableInt32()
            // Category = obj[17].GetNullableInt32()
        };

        return state;
    }
}