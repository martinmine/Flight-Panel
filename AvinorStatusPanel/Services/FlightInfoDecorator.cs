using System.Globalization;
using AirLabs;
using AvinorFlydataClient;
using FlightListener.Model;
using FlightListener.OpenSkyNetwork;

namespace AvinorStatusPanel.Services;

public class FlightInfoDecorator
{
    private readonly OpenSkyNetworkClient _openSkyNetworkClient;
    private readonly FlydataClient _flydataClient;
    private readonly AirLabsClient _airLabsClient;

    public FlightInfoDecorator(FlydataClient flydataClient, OpenSkyNetworkClient openSkyNetworkClient, AirLabsClient airLabsClient)
    {
        _flydataClient = flydataClient;
        _openSkyNetworkClient = openSkyNetworkClient;
        _airLabsClient = airLabsClient;
    }

    public async Task AddMoreFlightInfo(Aircraft aircraft)
    {
        if (await AddFlightInfoFromAirLabs(aircraft))
            return;
        
        /*if (await AddFlightInfoFromOslDepartures(aircraft))
            return;
        
        if (await AddFlightInfoFromOslArrivals(aircraft))
            return;
        
        if (await AddFlightInfoFromOpenSkyNetwork(aircraft))
            return;*/
    }

    private async Task<bool> AddFlightInfoFromAirLabs(Aircraft aircraft)
    {
        try
        {
            var response = await _airLabsClient.GetFlight(aircraft.FlightIcao24);
            if (response == null)
                return false;

            aircraft.FlightNumber = response.flight_iata;
            aircraft.Origin = response.dep_city;
            aircraft.Destination = response.arr_city;
            aircraft.ArrivalTime = Parse(response.arr_time);
            aircraft.DepartureTime = Parse(response.dep_time);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    private DateTime? Parse(string dateString)
    {
        if (string.IsNullOrEmpty(dateString))
            return null;

        return DateTime.ParseExact(dateString, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
    }
    
    private async Task<bool> AddFlightInfoFromOpenSkyNetwork(Aircraft aircraft)
    {
        var start = DateTimeOffset.UtcNow.AddHours(-5).ToUnixTimeSeconds();
        var end = DateTimeOffset.UtcNow.AddHours(5).ToUnixTimeSeconds();

        var data = await _openSkyNetworkClient.GetFlights(aircraft.Icao24, start, end);

        var flightInfo = data?.FirstOrDefault();
        if (flightInfo == null)
            return false;
        
        aircraft.Destination = flightInfo.EstArrivalAirport;
        aircraft.Origin = flightInfo.EstDepartureAirport;
        return true;
    }

    private async Task<bool> AddFlightInfoFromOslDepartures(Aircraft aircraft)
    {
        var departures = await _flydataClient.GetOutboundFlights(1, 8, "OSL");

        var flightFromOsl = departures?.Flight?.FirstOrDefault(f => f.Flight_id == aircraft.FlightIcao24);
        
        if (flightFromOsl == null)
            return false;

        aircraft.Origin = "OSL";
        aircraft.Destination = flightFromOsl.Airport;
        aircraft.DepartureTime = flightFromOsl.Schedule_time;
        
        if (flightFromOsl.Status != null)
            aircraft.DepartureTime = flightFromOsl.Schedule_time;

        return true;
    }

    private async Task<bool> AddFlightInfoFromOslArrivals(Aircraft aircraft)
    {
        var departures = await _flydataClient.GetInboundFlights(1, 8, "OSL");

        var flightFromOsl = departures?.Flight?.FirstOrDefault(f => f.Flight_id == aircraft.FlightIcao24);
        if (flightFromOsl == null)
            return false;

        aircraft.Destination = "OSL";
        aircraft.Origin = flightFromOsl.Airport;
        aircraft.ArrivalTime = flightFromOsl.Schedule_time;
        
        if (flightFromOsl.Status != null)
            aircraft.ArrivalTime = flightFromOsl.Schedule_time;

        return true;
    }
}