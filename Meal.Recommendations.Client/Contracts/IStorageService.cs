using Microsoft.JSInterop;

namespace Meal.Recommendations.Client.Contracts;

public interface IStorageService
{
    public IJSRuntime JS { get; }
}

public interface ISessionStorageService : IStorageService
{
    public async Task<TItem?> Get<TItem>(string key)
    {
        string? value = await JS.InvokeAsync<string>("sessionStorage.getItem", key);

        if (string.IsNullOrEmpty(value))
        {
            return default;
        }

        return System.Text.Json.JsonSerializer.Deserialize<TItem>(value);
    }

    public async Task Save<TItem>(string key, TItem value)
    {
        await JS.InvokeVoidAsync("sessionStorage.setItem", key, System.Text.Json.JsonSerializer.Serialize(value));
    }
}

public class SessionStorageService(IJSRuntime _js) : ISessionStorageService
{
    public IJSRuntime JS => _js;
}