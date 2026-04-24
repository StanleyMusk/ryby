using FishingTrip.Application.Services;
using FishingTrip.Infrastructure.Composition;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents();
builder.Services.AddSingleton<TripManagementService>(_ => AppCompositionRoot.CreateTripManagementService());

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<FishingTrip.App.Components.App>();

app.Run();
