using System.ComponentModel.DataAnnotations;
using Conduit.Articles.DomainLayer.Exceptions;

namespace Conduit.Articles.DomainLayer;

public class InvalidRequestException : BadRequestException
{
    public InvalidRequestException(
        IEnumerable<ValidationResult> validationResults)
    {
        ValidationResults = validationResults;
    }

    public IEnumerable<ValidationResult> ValidationResults { get; set; }
}
