namespace Conduit.Articles.DomainLayer;

public interface IArticleReadRepository
{
    Task<SingleArticle> FindAsync(
        FindArticle.Request request,
        CancellationToken cancellationToken = default);

    Task<MultipleArticles> SearchAsync(
        SearchArticles.Request request,
        CancellationToken cancellationToken = default);

    Task<MultipleArticles> FeedAsync(
        FeedArticle.Request request,
        CancellationToken cancellationToken = default);
}
