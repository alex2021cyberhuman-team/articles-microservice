﻿using Conduit.Articles.DomainLayer;
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
        var author = await _context.Author.FindAsync(
            new object[] { article.CurrentUserId }, cancellationToken);
        var tags = await GetTagsAsync(model.TagList, cancellationToken);

        var articleDbModel = new ArticleDbModel
        {
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            AuthorId = article.CurrentUserId,
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
        var old =
            await FindArticleDbModelAsync(article.Slug, cancellationToken);

        await using var transaction =
            await _context.Database.BeginTransactionAsync(cancellationToken);
        var model = article.Body.Article;

        var tags = await UpdateTagsAsync(model, old, cancellationToken);

        var articleDbModel = new ArticleDbModel
        {
            UpdatedAt = DateTime.UtcNow,
            AuthorId = article.CurrentUserId,
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

    public async Task DeleteAsync(
        DeleteArticle.Request article,
        CancellationToken cancellationToken = default)
    {
        var model =
            await FindArticleDbModelAsync(article.Slug, cancellationToken);
        _context.Remove(model);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task<ArticleDbModel> FindArticleDbModelAsync(
        string articleSlug,
        CancellationToken cancellationToken)
    {
        var item = await _context.Article.Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Slug == articleSlug, cancellationToken);
        if (item is null)
        {
            throw new NotFoundException();
        }

        return item;
    }

    private string UpdateSlug(
        UpdateArticle.Model model,
        ArticleDbModel old)
    {
        return model.Title is not null && model.Title != old.Title
            ? _slugilizator.GetSlug(model.Title)
            : old.Slug;
    }

    private async Task<ICollection<TagDbModel>> UpdateTagsAsync(
        UpdateArticle.Model model,
        ArticleDbModel old,
        CancellationToken cancellationToken)
    {
        var tags = model.TagList is null
            ? old.Tags
            : await GetTagsAsync(model.TagList, cancellationToken);
        return tags;
    }

    private async Task<List<TagDbModel>> GetTagsAsync(
        IEnumerable<string> tagList,
        CancellationToken cancellationToken)
    {
        var tagListClone = tagList.ToHashSet();
        var tags = await _context.Tag.Where(x => tagListClone.Contains(x.Name))
            .ToListAsync(cancellationToken);
        tags.ForEach(x => tagListClone.Remove(x.Name));
        tags.AddRange(tagListClone.Select(x => new TagDbModel { Name = x }));
        return tags;
    }
}
