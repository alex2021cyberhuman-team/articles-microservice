using Conduit.Articles.DomainLayer;

namespace Conduit.Articles.DataAccessLayer;

public static class ArticleMapper
{
    public static SingleArticle MapArticle(
        this ArticleDbModel model,
        bool following = false,
        bool favorited = false)
    {
        return new(new(model.Slug, model.Title, model.Description, model.Body,
            model.Tags.Select(x => x.Name).ToHashSet(), model.CreatedAt,
            model.UpdatedAt,
            // TODO: MAKE FAVORITED
            favorited, model.FavoritesCount,
            new(model.Author.Username, model.Author.Bio, model.Author.Image,
                following)));
    }
}
