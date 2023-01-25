using System.Xml.Serialization;
using AvinorFlydataClient.Model;

namespace AvinorFlydataClient;

public class FlydataClient
{
    private readonly HttpClient _httpClient;

    public FlydataClient()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://flydata.avinor.no/");
    }
    
    public async Task<List<AirlineName>> GetAirlineNames()
    {
        var airlines = await GetXmlObjectResponse<AirlineNames>("airlineNames.asp");
        return airlines?.AirlineName;
    }

    private async Task<T> GetXmlObjectResponse<T>(string uri)
    {
        var response = await _httpClient.GetStringAsync(uri);
        
        var serializer = new XmlSerializer(typeof(T));
        return (T)serializer.Deserialize(new StringReader(response));
    }

    public async Task<FlightStatus> GetFlightStatus(string code)
    {
        var response = await GetXmlObjectResponse<FlightStatuses>($"flightStatuses.asp?code={code}");
        return response?.FlightStatus?.FirstOrDefault();
    }

    public async Task<AirportName> GetAirportName(string code)
    {
        var response = await GetXmlObjectResponse<AirportNames>($"airportNames.asp?airport={code}");
        return response?.AirportName?.FirstOrDefault();
    }

    public async Task<List<AirlineName>> GetAllAirlineNames()
    {
        var response = await GetXmlObjectResponse<AirlineNames>("https://flydata.avinor.no/airlineNames.asp");
        return response?.AirlineName;
    }
    
    public Task<Flights> GetInboundFlights(int timeFrom, int timeTo, string airport) =>
        GetAllFlights(timeFrom, timeTo, airport, "A");

    public Task<Flights> GetOutboundFlights(int timeFrom, int timeTo, string airport) =>
        GetAllFlights(timeFrom, timeTo, airport, "D");

    private async Task<Flights> GetAllFlights(int timeFrom, int timeTo, string airport, string direction)
    {
        var uri = $"XmlFeed.asp?TimeFrom={timeFrom}&TimeTo={timeTo}&airport={airport}&direction={direction}";
        var response = await GetXmlObjectResponse<Airport>(uri);
        return response?.Flights;
    }
}