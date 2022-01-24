using System.Linq.Expressions;
using Conduit.Articles.DataAccessLayer.DbContexts;
using Conduit.Articles.DataAccessLayer.Models;
using Conduit.Articles.DataAccessLayer.Utilities;
using Conduit.Articles.DomainLayer.Exceptions;
using Conduit.Articles.DomainLayer.Models;
using Conduit.Articles.DomainLayer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Articles.DataAccessLayer.Repositories;

public static class ArticleQueryableExtensions
{
    private static readonly Expression<Func<ArticleDbModel, ArticleModel>>
        SelectExpression = x => new()
        {
            Slug = x.Slug,
            Title = x.Title,
            Description = x.Description,
            Body = x.Body,
            TagList = x.Tags.Select(y => y.Name).ToHashSet(),
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Favorited = x.Favoriters.Any(),
            Author = new()
            {
                Username = x.Author.Username,
                Bio = x.Author.Bio,
                Following = x.Author.Followers.Any()
            }
        };

    public static IQueryable<ArticleDbModel> IncludeArticles(
        this IQueryable<ArticleDbModel> source,
        Guid? userId)
    {
        return source.Include(x => x.Tags).Include(x => x.Author)
            .ThenInclude(x => x.Followeds.Where(y => y.Id == userId))
            .Include(x => x.Favoriters.Where(y => y.Id == userId));
    }

    public static IQueryable<ArticleModel> SelectArticles(
        this IQueryable<ArticleDbModel> source,
        int queryOffset,
        int queryLimit)
    {
        return source.Select(SelectExpression).OrderBy(x => x.CreatedAt)
            .Skip(queryOffset).Take(queryLimit);
    }

    public static IQueryable<ArticleDbModel> FilterArticles(
        this IQueryable<ArticleDbModel> source,
        SearchArticles.Request request)
    {
        return source.Where(x =>
            (request.Query.Author == null ||
             x.Author.Username == request.Query.Author) &&
            (request.Query.Tag == null ||
             x.Tags.Select(y => y.Name).Contains(request.Query.Tag)) &&
            (request.Query.Favorited == null ||
             x.Favoriters.Any(y => y.Username == request.Query.Favorited)));
    }
}

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
