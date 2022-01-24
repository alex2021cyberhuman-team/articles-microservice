using Conduit.Shared.Events.Models.Favorites;

namespace Conduit.Articles.DomainLayer.Repositories;

public interface IFavoritesConsumerRepository
{
    Task FavoriteAsync(
        FavoriteArticleEventModel model);

    Task UnfavoriteAsync(
        UnfavoriteArticleEventModel model);
}
