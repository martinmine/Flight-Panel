using System.Text.RegularExpressions;
using AvinorFlydataClient;
using AvinorFlydataClient.Model;
using AvinorStatusPanel.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AvinorStatusPanel.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty(SupportsGet = true)]
        public List<TableEntryViewModel> TableEntries { get; set; }
        
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var client = new FlydataClient();

            var airlineCache = new AirlineNameCache(client);
            var statusCache = new FlightStatusCache(client);

            var departures = await client.GetOutboundFlights(1, 8, "OSL");

            TableEntries = new List<TableEntryViewModel>();

            foreach (var flight in departures.Flight.Where(t => t.Schedule_time > DateTime.UtcNow))
            {
                TableEntries.Add(new TableEntryViewModel()
                {
                    Destination = (await client.GetAirportName(flight.Airport)).Name,
                    Flight = flight.Flight_id,
                    Gate = flight.Gate,
                    Time = flight.Schedule_time.ToLocalTime().ToString("HH:mm"),
                    Iata = Regex.Replace(flight.Flight_id, @"[\d-]", string.Empty),
                    Status = await GetStatusCode(statusCache, flight.Status)
                });
            }
            
            return Page();
        }

        private async Task<string> GetStatusCode(FlightStatusCache statusClient, Status status)
        {
            if (status == null)
                return "";

            var statusText = await statusClient.GetFlightStatus(status.Code);

            if (status.Code == "E")
            {
                return $"{statusText.StatusTextEn} {status.Time.ToLocalTime():HH:mm}";
            }
            
            return statusText.StatusTextEn;
        }
    }
}