using AvinorFlydataClient;
using AvinorStatusPanel;

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

app.Run();
