﻿using System.ComponentModel.DataAnnotations;
using Conduit.Articles.DomainLayer;
using Conduit.Articles.DomainLayer.Models;
using Conduit.Articles.DomainLayer.Utilities;

namespace Conduit.Articles.BusinessLogicLayer;

public class UpdateArticleRequestValidator : IValidator<UpdateArticle.Request>
{
    public Task<ICollection<ValidationResult>> ValidateAsync(
        UpdateArticle.Request entityToValidate,
        CancellationToken cancellationToken = default)
    {
        var results = new List<ValidationResult>();
        entityToValidate.ValidateObject(results);
        return Task.FromResult<ICollection<ValidationResult>>(results);
    }
}
