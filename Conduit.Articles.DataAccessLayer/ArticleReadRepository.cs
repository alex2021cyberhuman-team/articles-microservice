using Conduit.Articles.DomainLayer;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Articles.DataAccessLayer;

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
        SearchArticle.Request request,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Article.Include(x => x.Tags).Include(x => x.Author)
            .ThenInclude(x =>
                x.Followers.Where(y => y.FollowerId == request.CurrentUserId))
            .Where(x =>
                (request.Query.Author == null ||
                 x.Author.Username == request.Query.Author) &&
                (request.Query.Tag == null ||
                 x.Tags.Select(y => y.Name).Contains(request.Query.Tag)));
        var articleCount = await query.CountAsync(cancellationToken);
        var articles = await query.Select(x => new ArticleModel
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
            }).OrderBy(x => x.CreatedAt).Skip(request.Query.Offset)
            .Take(request.Query.Limit).ToListAsync(cancellationToken);
        var result = new MultipleArticles(articles, articleCount);

        return result;
    }

    public async Task<MultipleArticles> FeedAsync(
        FeedArticle.Request request,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Article.Include(x => x.Tags).Include(x => x.Author)
            .ThenInclude(x =>
                x.Followers.Where(y => y.FollowerId == request.CurrentUserId))
            .Where(x => x.Author.Followers.Any());
        var articleCount = await query.CountAsync(cancellationToken);
        var articles = await query.Select(x => new ArticleModel
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
            }).OrderBy(x => x.CreatedAt).Skip(request.Query.Offset)
            .Take(request.Query.Limit).ToListAsync(cancellationToken);
        var result = new MultipleArticles(articles, articleCount);

        return result;
    }
}
