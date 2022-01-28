using Conduit.Articles.DataAccessLayer.DbContexts;
using Conduit.Articles.DataAccessLayer.Models;
using Conduit.Articles.DataAccessLayer.Utilities;
using Conduit.Articles.DomainLayer.Exceptions;
using Conduit.Articles.DomainLayer.Models;
using Conduit.Articles.DomainLayer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Articles.DataAccessLayer.Repositories;

public class ArticleReadRepository : IArticleReadRepository
{
    private readonly ArticlesDbContext _context;

    public ArticleReadRepository(
        ArticlesDbContext context)
    {
        _context = context;
    }

    public async Task<SingleArticle> FindAsync(
        FindArticle.Request request,
        CancellationToken cancellationToken = default)
    {
        var article = await _context.Article
            .IncludeArticles(request.CurrentUserId)
            .FirstOrDefaultAsync(x => x.Slug == request.Slug,
                cancellationToken);

        var singleArticle =
            (article ?? throw new NotFoundException())
            .MapArticleToSingleArticle(article.Author.Followers.Any(),
                article.Favoriters.Any());

        return singleArticle;
    }

    public async Task<MultipleArticles> SearchAsync(
        SearchArticles.Request request,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Article.IncludeArticles(request.CurrentUserId);
        query = query.FilterArticles(request);
        return await ReturnMultipleArticles(query, request.Query.Offset,
            request.Query.Limit, cancellationToken);
    }

    public async Task<MultipleArticles> FeedAsync(
        FeedArticle.Request request,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Article.IncludeArticles(request.CurrentUserId);
        query = query.Where(x => x.Author.Followers.Any());
        return await ReturnMultipleArticles(query, request.Query.Offset,
            request.Query.Limit, cancellationToken);
    }

    private static async Task<MultipleArticles> ReturnMultipleArticles(
        IQueryable<ArticleDbModel> query,
        int queryOffset,
        int queryLimit,
        CancellationToken cancellationToken)
    {
        var articleCount = await query.CountAsync(cancellationToken);
        var articles = await query.SelectArticles(queryOffset, queryLimit)
            .ToListAsync(cancellationToken);
        var result = new MultipleArticles(articles, articleCount);
        return result;
    }
}
