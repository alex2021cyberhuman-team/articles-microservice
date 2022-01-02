using Conduit.Articles.DomainLayer;

namespace Conduit.Articles.DataAccessLayer;

public class  ArticleReadRepository : IArticleReadRepository
{
    public Task<SingleArticle> FindAsync(
        FindArticle.Request request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<MultipleArticles> SearchAsync(
        SearchArticle.Request request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<MultipleArticles> FeedAsync(
        FeedArticle.Request request,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}