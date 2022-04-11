using Conduit.Shared.Events.Models.Likes.Favorite;
using Conduit.Shared.Events.Models.Likes.Unfavorite;

namespace Conduit.Articles.DomainLayer.Repositories;

public interface IFavoritesConsumerRepository
{
    Task FavoriteAsync(
        FavoriteArticleEventModel model);

    Task UnfavoriteAsync(
        UnfavoriteArticleEventModel model);
}
