using AvinorFlydataClient;
using AvinorStatusPanel.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AvinorStatusPanel.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly FlydataClient _client;
        private readonly FlightDataMapper _flightDataMapper;
        
        [BindProperty(SupportsGet = true)]
        public List<TableEntryViewModel> TableEntries { get; set; }
        
        public IndexModel(ILogger<IndexModel> logger, FlydataClient client, FlightDataMapper flightDataMapper)
        {
            _logger = logger;
            _client = client;
            _flightDataMapper = flightDataMapper;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var departures = await _client.GetOutboundFlights(1, 8, "OSL");
            var pendingDepartures = departures.Flight.Where(t => t.Schedule_time > DateTime.UtcNow);
            TableEntries = await _flightDataMapper.Map(pendingDepartures);

            return Page();
        }
    }
}