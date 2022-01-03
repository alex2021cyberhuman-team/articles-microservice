namespace Conduit.Articles.DomainLayer;

public interface IArticleCreator
{
    Task<SingleArticle> CreateAsync(
        CreateArticle.Request article,
        CancellationToken cancellationToken);
}
