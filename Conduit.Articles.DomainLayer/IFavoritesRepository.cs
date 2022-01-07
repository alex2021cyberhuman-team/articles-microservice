using Conduit.Shared.Events.Models.Favorites;

namespace Conduit.Articles.DomainLayer;

public interface IFavoritesRepository
{
    Task FavoriteAsync(
        FavoriteArticleEventModel model);
    
    Task UnfavoriteAsync(
        UnfavoriteArticleEventModel model);
}
