using System.ComponentModel.DataAnnotations;

namespace Conduit.Articles.DomainLayer;

public class InvalidRequestException : BadRequestException
{
    public InvalidRequestException(
        ICollection<ValidationResult> validationResults)
    {
        ValidationResults = validationResults;
    }

    public ICollection<ValidationResult> ValidationResults { get; set; }
}