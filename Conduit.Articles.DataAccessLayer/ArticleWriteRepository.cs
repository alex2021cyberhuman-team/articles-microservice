using Conduit.Articles.DomainLayer;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Articles.DataAccessLayer;

public class ArticleWriteRepository : IArticleWriteRepository
{
    private readonly ArticlesDbContext _context;
    private readonly ISlugilizator _slugilizator;

    public ArticleWriteRepository(
        ArticlesDbContext context,
        ISlugilizator slugilizator)
    {
        _context = context;
        _slugilizator = slugilizator;
    }

    public async Task<SingleArticle> CreateAsync(
        CreateArticle.Request article,
        CancellationToken cancellationToken = default)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync(cancellationToken);
        var model = article.Body.Article;
        var author =
            await _context.Author.FindAsync(new object[] { article.UserId },
                cancellationToken);
        var tags = await RetriveTagsAsync(cancellationToken, model.TagList);

        var articleDbModel = new ArticleDbModel
        {
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            AuthorId = article.UserId,
            Author = author!,
            Body = model.Body,
            Description = model.Description,
            Title = model.Title,
            Slug = _slugilizator.GetSlug(model.Title),
            Tags = tags
        };

        _context.Add(articleDbModel);
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return articleDbModel.MapArticle();
    }

    public async Task<SingleArticle> UpdateAsync(
        UpdateArticle.Request article,
        CancellationToken cancellationToken = default)
    {
        var old = await FindArticleDbModelAsync(article, cancellationToken);

        await using var transaction =
            await _context.Database.BeginTransactionAsync(cancellationToken);
        var model = article.Body.Article;

        var tags = await UpdateTags(cancellationToken, model, old);

        var articleDbModel = new ArticleDbModel
        {
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            AuthorId = article.UserId,
            Body = model.Body ?? old.Body,
            Description = model.Description ?? old.Description,
            Title = model.Title ?? old.Title,
            Slug = UpdateSlug(model, old),
            Tags = tags
        };

        _context.Update(articleDbModel);
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return articleDbModel.MapArticle();
    }

    private async Task<ArticleDbModel> FindArticleDbModelAsync(
        UpdateArticle.Request article,
        CancellationToken cancellationToken)
    {
        return await _context.Article.Include(x => x.Tags)
            .FirstAsync(x => x.Slug == article.Slug, cancellationToken);
    }

    private string UpdateSlug(
        UpdateArticle.Model model,
        ArticleDbModel old)
    {
        return model.Title is not null && model.Title != old.Title
            ? _slugilizator.GetSlug(model.Title)
            : old.Slug;
    }

    private async Task<ICollection<TagDbModel>> UpdateTags(
        CancellationToken cancellationToken,
        UpdateArticle.Model model,
        ArticleDbModel old)
    {
        var tags = model.TagList is null
            ? old.Tags
            : await RetriveTagsAsync(cancellationToken, model.TagList);
        return tags;
    }

    private async Task<List<TagDbModel>> RetriveTagsAsync(
        CancellationToken cancellationToken,
        IEnumerable<string> tagList)
    {
        var tagListClone = tagList.ToHashSet();
        var tags = await _context.Tag.Where(x => tagListClone.Contains(x.Name))
            .ToListAsync(cancellationToken);
        tags.ForEach(x => tagListClone.Remove(x.Name));
        tags.AddRange(tagListClone.Select(x => new TagDbModel { Name = x }));
        return tags;
    }
}