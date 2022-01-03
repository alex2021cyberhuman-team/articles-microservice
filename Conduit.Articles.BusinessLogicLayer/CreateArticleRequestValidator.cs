using System.ComponentModel.DataAnnotations;
using Conduit.Articles.DomainLayer;

namespace Conduit.Articles.BusinessLogicLayer;

public class CreateArticleRequestValidator : IValidator<CreateArticle.Request>
{
    public Task<ICollection<ValidationResult>> ValidateAsync(
        CreateArticle.Request entityToValidate,
        CancellationToken cancellationToken = default)
    {
        var results = new List<ValidationResult>();
        entityToValidate.ValidateObject(results);
        return Task.FromResult<ICollection<ValidationResult>>(results);
    }
}
