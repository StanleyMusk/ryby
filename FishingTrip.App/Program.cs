using FishingTrip.Application.Services;
using FishingTrip.Infrastructure.Composition;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSingleton<TripManagementService>(_ => AppCompositionRoot.CreateTripManagementService());

var app = builder.Build();

app.Use(async (context, next) =>
{
    context.Response.Headers["Permissions-Policy"] = "unload=(self)";
    await next();
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<FishingTrip.App.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();
