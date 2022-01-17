using Conduit.Articles.DomainLayer.Models;

namespace Conduit.Articles.DomainLayer.Repositories;

public interface IArticleWriteRepository
{
    Task<SingleArticle> CreateAsync(
        CreateArticle.Request article,
        CancellationToken cancellationToken = default);

    Task<SingleArticle> UpdateAsync(
        UpdateArticle.Request article,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        DeleteArticle.Request article,
        CancellationToken cancellationToken = default);
}
