using AvinorFlydataClient;
using AvinorStatusPanel;
using AvinorStatusPanel.Hubs;
using AvinorStatusPanel.Services;
using FlightListener;

var builder = WebApplication.CreateBuilder(args);

var client = new FlydataClient();
//var s = await client.GetAirlineNames();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddSingleton<FlydataClient>();
builder.Services.AddSingleton<FlightDataMapper>();
builder.Services.AddSingleton<AirlineNameCache>();
builder.Services.AddSingleton<FlightStatusCache>();
builder.Services.AddSingleton<IPlaneSpotter, SpottableAicraftStore>();
builder.Services.AddHostedService<AircraftRadar>();
builder.Services.AddTransient<MapFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapHub<FlightHub>("/flightHub");

app.Run();
