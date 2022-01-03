using System.Linq.Expressions;
using Conduit.Articles.DomainLayer;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Articles.DataAccessLayer;

public class ArticleReadRepository : IArticleReadRepository
{
    private static readonly Expression<Func<ArticleDbModel, ArticleModel>>
        Expression = x => new()
        {
            Slug = x.Slug,
            Title = x.Title,
            Description = x.Description,
            Body = x.Body,
            TagList = x.Tags.Select(y => y.Name).ToHashSet(),
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            // TODO: MAKE FAVORITES
            Favorited = false,
            Author = new()
            {
                Username = x.Author.Username,
                Bio = x.Author.Bio,
                Following = x.Author.Followers.Any()
            }
        };

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
        var article = await _context.Article.Include(x => x.Tags)
            .Include(x => x.Author)
            .ThenInclude(x =>
                x.Followers.Where(y => y.FollowerId == request.CurrentUserId))
            .FirstOrDefaultAsync(x => x.Slug == request.Query.Slug,
                cancellationToken);

        // TODO: MAKE FAVORITES
        var result =
            (article ?? throw new NotFoundException()).MapArticle(
                article.Author.Followers.Any());

        return result;
    }

    public async Task<MultipleArticles> SearchAsync(
        SearchArticles.Request request,
        CancellationToken cancellationToken = default)
    {
        var query = Include(request.CurrentUserId);
        query = FilterQuery(request, query);
        return await ReturnMultipleArticles(query, request.Query.Offset, request.Query.Limit, cancellationToken);
    }

    public async Task<MultipleArticles> FeedAsync(
        FeedArticle.Request request,
        CancellationToken cancellationToken = default)
    {
        var query = Include(request.CurrentUserId);
        query = query.Where(x => x.Author.Followers.Any());
        return await ReturnMultipleArticles(query, request.Query.Offset, request.Query.Limit, cancellationToken);
    }

    private static async Task<MultipleArticles> ReturnMultipleArticles(
        IQueryable<ArticleDbModel> query,
        int queryOffset,
        int queryLimit,
        CancellationToken cancellationToken)
    {
        var articleCount = await query.CountAsync(cancellationToken);
        var articles = await ToListAsync(query, queryOffset,
            queryLimit, cancellationToken);
        var result = new MultipleArticles(articles, articleCount);
        return result;
    }

    private static IQueryable<ArticleDbModel> FilterQuery(
        SearchArticles.Request request,
        IQueryable<ArticleDbModel> query)
    {
        return query.Where(x =>
            (request.Query.Author == null ||
             x.Author.Username == request.Query.Author) &&
            (request.Query.Tag == null ||
             x.Tags.Select(y => y.Name).Contains(request.Query.Tag)));
    }

    private IQueryable<ArticleDbModel> Include(
        Guid requestCurrentUserId)
    {
        return _context.Article.Include(x => x.Tags).Include(x => x.Author)
            .ThenInclude(x =>
                x.Followers.Where(y => y.FollowerId == requestCurrentUserId));
    }

    private static async Task<List<ArticleModel>> ToListAsync(
        IQueryable<ArticleDbModel> query,
        int queryOffset,
        int queryLimit,
        CancellationToken cancellationToken)
    {
        return await query.Select(Expression).OrderBy(x => x.CreatedAt)
            .Skip(queryOffset).Take(queryLimit)
            .ToListAsync(cancellationToken);
    }
}
