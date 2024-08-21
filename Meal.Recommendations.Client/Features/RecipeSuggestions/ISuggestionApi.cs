using System.Net.Http.Json;
using System.Text.Json;

namespace Meal.Recommendations.Client.Features.RecipeSuggestions;

public interface ISuggestionApi
{
    Task<IEnumerable<MealSuggestion.Result>?> GetMealSuggestions(MealSuggestion.Request request);
}

public class SuggestionApi(HttpClient httpClient) : ISuggestionApi
{
    public const string SubmitMealRecommendation = "/post-url";

    public async Task<IEnumerable<MealSuggestion.Result>?> GetMealSuggestions(MealSuggestion.Request request)
    {
        var results = await httpClient.PostAsJsonAsync(SubmitMealRecommendation, request);

        if (results.IsSuccessStatusCode)
        {
            return await results.Content.ReadFromJsonAsync<IEnumerable<MealSuggestion.Result>>();
        }

        return null;
    }
}

public class FakeSuggestionApiHandler : HttpMessageHandler
{
    public static readonly int LoadingDelayTime = Random.Shared.Next(2000, 2500);
    public static readonly TimeSpan LoadingDelay = TimeSpan.FromMilliseconds(LoadingDelayTime);

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await Task.Delay(LoadingDelay, cancellationToken);

        string responseContent = string.Empty;
        HttpResponseMessage response = new()
        {
            StatusCode = System.Net.HttpStatusCode.OK
        };

        switch (request.RequestUri!.AbsolutePath)
        {
            case "/post-url":
                responseContent = JsonSerializer.Serialize<MealSuggestion.Result[]>([
                    new MealSuggestion.Result("Spicy Chicken Tacos", "Grilled chicken tacos with a kick of heat, topped with fresh salsa and creamy guacamole."),
                    new MealSuggestion.Result("Vegan Buddha Bowl", "A colorful mix of quinoa, roasted veggies, avocado, and tahini dressing."),
                    new MealSuggestion.Result("Classic Beef Stroganoff", "Tender strips of beef in a rich, creamy mushroom sauce served over egg noodles."),
                    new MealSuggestion.Result("Mediterranean Salad", "A fresh mix of cucumbers, tomatoes, feta cheese, and olives, tossed in a tangy vinaigrette."),
                    new MealSuggestion.Result("Lemon Herb Salmon", "Pan-seared salmon fillets with a zesty lemon herb sauce, served with steamed asparagus.")
                ]);

                break;
        }

        response.Content = new StringContent(responseContent);
        return await Task.FromResult(response);
    }
}