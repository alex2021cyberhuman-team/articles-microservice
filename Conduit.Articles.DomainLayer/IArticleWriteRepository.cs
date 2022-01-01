namespace Conduit.Articles.DomainLayer;

public interface IArticleWriteRepository
{
    Task<SingleArticle> CreateAsync(
        CreateArticle.Request article,
        CancellationToken cancellationToken = default);
    
    Task<SingleArticle> UpdateAsync(
        UpdateArticle.Request article,
        CancellationToken cancellationToken = default);
}
