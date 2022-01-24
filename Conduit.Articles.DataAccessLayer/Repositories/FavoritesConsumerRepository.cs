using System.Data;
using Conduit.Articles.DataAccessLayer.DbContexts;
using Conduit.Articles.DataAccessLayer.Models;
using Conduit.Articles.DomainLayer.Repositories;
using Conduit.Shared.Events.Models.Favorites;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Articles.DataAccessLayer.Repositories;

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
            await FirstOrDefaultAsync(model.UserId, model.ArticleId);

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
                x.Id == model.UserId );

        if (author == null)
        {
            throw new ArgumentNullException(nameof(author));
        }

        article.Favoriters.Add(author);
        // TODO: Make it with sql function and replace with functions all ef core
        await _articlesDbContext.Database.ExecuteSqlRawAsync(
            @"update article set favorites_count = favorites_count + 1 where article_id = {0}",
            article.Id);
        await _articlesDbContext.SaveChangesAsync();
        await transaction.CommitAsync();
    }

    public async Task UnfavoriteAsync(
        UnfavoriteArticleEventModel model)
    {
        await using var transaction =
            await _articlesDbContext.Database.BeginTransactionAsync();

        var article =
            await FirstOrDefaultAsync(model.UserId, model.ArticleId);

        if (article == null)
        {
            throw new ArgumentNullException(nameof(article));
        }

        if (article.Favoriters.Any() == false)
        {
            throw new ConstraintException();
        }

        article.Favoriters.Remove(article.Favoriters.First());
        await _articlesDbContext.Database.ExecuteSqlRawAsync(
            @"update article set favorites_count = favorites_count - 1 where article_id = {0}",
            article.Id);
        await _articlesDbContext.SaveChangesAsync();
        await transaction.CommitAsync();
    }

    private async Task<ArticleDbModel?> FirstOrDefaultAsync(
        Guid userId,
        Guid articleId)
    {
        return await _articlesDbContext.Article
            .Include(x => x.Favoriters.Where(y => y.Id == userId))
            .FirstOrDefaultAsync(x => x.Id == articleId);
    }
}
