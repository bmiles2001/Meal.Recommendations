using Meal.Recommendations.Client.Contracts;
using Meal.Recommendations.Client.Features.RecipeSuggestions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

namespace Meal.Recommendations.Client;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<Infrastructure.App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddMudServices();
        builder.Services.AddScoped<INotification, Notification>();
        builder.Services.AddScoped<ISessionStorageService, SessionStorageService>();
        builder.Services.AddScoped<ISuggestionApi, SuggestionApi>();
        builder.Services.AddScoped<MealSuggestion.State>();
        builder.Services.AddScoped(sp => new HttpClient(new FakeSuggestionApiHandler()) { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        await builder.Build().RunAsync();
    }
}
