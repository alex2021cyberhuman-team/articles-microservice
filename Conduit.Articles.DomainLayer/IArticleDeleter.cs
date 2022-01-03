namespace Conduit.Articles.DomainLayer;

public interface IArticleDeleter
{
    Task DeleteAsync(
        DeleteArticle.Request article,
        CancellationToken cancellationToken);
}
