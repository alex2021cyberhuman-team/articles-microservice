namespace Conduit.Articles.DomainLayer;

public interface IArticleWriteRepository
{
    Task<SingleArticle> CreateAsync(
        CreateArticle article,
        CancellationToken cancellationToken = default);
    
    Task<SingleArticle> UpdateAsync(
        UpdateArticle article,
        CancellationToken cancellationToken = default);
}
