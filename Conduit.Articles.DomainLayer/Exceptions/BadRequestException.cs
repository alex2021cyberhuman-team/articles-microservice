using System.ComponentModel.DataAnnotations;

namespace Conduit.Articles.DomainLayer.Exceptions;

public class BadRequestException : ApplicationException
{
    public IEnumerable<ValidationResult> ValidationResults { get; set; } =
        Array.Empty<ValidationResult>();
}
