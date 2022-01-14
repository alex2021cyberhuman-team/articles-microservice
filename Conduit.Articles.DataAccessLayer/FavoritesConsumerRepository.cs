using System.Data;
using Conduit.Articles.DataAccessLayer.Models;
using Conduit.Articles.DomainLayer;
using Conduit.Shared.Events.Models.Favorites;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Articles.DataAccessLayer;

public class FavoritesConsumerRepository : IFavoritesConsumerRepository
{
    private readonly ArticlesDbContext _articlesDbContext;

    public FavoritesConsumerRepository(
        ArticlesDbContext articlesDbContext)
    {
        _articlesDbContext = articlesDbContext;
    }

    public async Task FavoriteAsync(
        FavoriteArticleEventModel model)
    {
        await using var transaction =
            await _articlesDbContext.Database.BeginTransactionAsync();

        var article =
            await FirstOrDefaultAsync(model.CurrentUserId, model.ArticleSlug);

        if (article == null)
        {
            throw new ArgumentNullException(nameof(article));
        }

        if (article.Favoriters.Any())
        {
            throw new ConstraintException();
        }

        var author =
            await _articlesDbContext.Author.FirstOrDefaultAsync(x =>
                x.Id == model.CurrentUserId);

        if (author == null)
        {
            throw new ArgumentNullException(nameof(author));
        }

        article.Favoriters.Add(author);
        article.FavoritesCount += 1;
        await _articlesDbContext.SaveChangesAsync();
        await transaction.CommitAsync();
    }

    public async Task UnfavoriteAsync(
        UnfavoriteArticleEventModel model)
    {
        await using var transaction =
            await _articlesDbContext.Database.BeginTransactionAsync();

        var article =
            await FirstOrDefaultAsync(model.CurrentUserId, model.ArticleSlug);

        if (article == null)
        {
            throw new ArgumentNullException(nameof(article));
        }

        if (article.Favoriters.Any() == false)
        {
            throw new ConstraintException();
        }

        article.Favoriters.Remove(article.Favoriters.First());
        article.FavoritesCount -= 1;
        await _articlesDbContext.SaveChangesAsync();
        await transaction.CommitAsync();
    }

    private async Task<ArticleDbModel?> FirstOrDefaultAsync(
        Guid userId,
        string slug)
    {
        return await _articlesDbContext.Article
            .Include(x => x.Favoriters.Where(y => y.Id == userId))
            .FirstOrDefaultAsync(x => x.Slug == slug);
    }
}
