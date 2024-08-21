using System.ComponentModel.DataAnnotations;

namespace Meal.Recommendations.Client.Features.RecipeSuggestions;

public static class MealSuggestion
{
    public class Profile
    {
        public IEnumerable<string> Allergies { get; set; } = [];

        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = default!;


        [RequireSelectedValue]
        public IEnumerable<string> Proteins { get; set; } = [];
    }

    public class Request
    {
        [RequireSelectedValue]
        public IEnumerable<string> CookingDevices { get; set; } = [];

        [RequireSelectedValue]
        public IEnumerable<Profile> Profiles { get; set; } = [];
    }

    public record Result(string Title, string Description);

    public class State
    {
        public Request Request { get; } = new();

        public event Action? OnChange;

        public void ClearProfiles()
        {
            Request.Profiles = [];
            Request.CookingDevices = [];
            NotifyStateChanged();
        }

        public void DeleteProfile(Profile profile)
        {
            Request.Profiles = Request.Profiles.Except([profile]);
            NotifyStateChanged();
        }

        public void SetCookingDevices(IEnumerable<string> devices)
        {
            Request.CookingDevices = devices;
            NotifyStateChanged();
        }

        public void SaveProfile(Profile tasteProfile)
        {
            Profile? existingProfile = Request.Profiles.FirstOrDefault(profile => profile.Id == tasteProfile.Id);

            if (existingProfile is null)
            {
                Request.Profiles = Request.Profiles.Append(tasteProfile);
                return;
            }

            Request.Profiles = Request.Profiles
                .Except([existingProfile]) // remove the old profile
                .Append(tasteProfile); // append the new one

            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}