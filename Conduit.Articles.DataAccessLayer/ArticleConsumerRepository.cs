using Conduit.Articles.DomainLayer;
using Conduit.Shared.Events.Models.Users.Register;
using Conduit.Shared.Events.Models.Users.Update;

namespace Conduit.Articles.DataAccessLayer;

public class ArticleConsumerRepository : IAuthorConsumerRepository
{
    private readonly ArticlesDbContext _articlesDbContext;

    public ArticleConsumerRepository(
        ArticlesDbContext articlesDbContext)
    {
        _articlesDbContext = articlesDbContext;
    }

    public async Task RegisterAsync(
        RegisterUserEventModel model)
    {
        var authorDbModel = new AuthorDbModel()
        {
            Id = model.Id,
            Username = model.Username,
            Image = model.Image,
            Bio = model.Biography
        };
        _articlesDbContext.Author.Add(authorDbModel);
        await _articlesDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(
        UpdateUserEventModel model)
    {
        var authorDbModel = new AuthorDbModel()
        {
            Id = model.Id,
            Username = model.Username,
            Image = model.Image,
            Bio = model.Biography
        };
        _articlesDbContext.Author.Update(authorDbModel);
        await _articlesDbContext.SaveChangesAsync();
    }
}
