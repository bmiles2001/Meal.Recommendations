using System.ComponentModel.DataAnnotations;

namespace Meal.Recommendations.Client.Features.RecipeSuggestions;

/// <summary>
/// Requires a value to be present. Used to ensure selected values in a form.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public class RequireSelectedValueAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IEnumerable<object> values && values.Any())
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(ErrorMessage);
    }
}