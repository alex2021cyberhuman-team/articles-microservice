using System.ComponentModel.DataAnnotations;

namespace Conduit.Articles.BusinessLogicLayer;

public static class ValidatorExtensions
{
    public static void ValidateObject<T>(
        this T entityToValidate,
        ICollection<ValidationResult> results)
    {
        if (entityToValidate == null)
        {
            throw new ArgumentNullException(nameof(entityToValidate));
        }

        var validationContext = new ValidationContext(entityToValidate);
        _ = Validator.TryValidateObject(entityToValidate, validationContext,
            results, true);
    }
}
