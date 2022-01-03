using Conduit.Articles.DomainLayer;
using Conduit.Shared.Events.Models.Profiles.CreateFollowing;
using Conduit.Shared.Events.Models.Profiles.RemoveFollowing;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Articles.DataAccessLayer;

public class FollowingsConsumerRepository : IFollowingsConsumerRepository
{
    private readonly ArticlesDbContext _articlesDbContext;

    public FollowingsConsumerRepository(
        ArticlesDbContext articlesDbContext)
    {
        _articlesDbContext = articlesDbContext;
    }

    public async Task CreateAsync(
        CreateFollowingEventModel model)
    {
        var following = new FollowingDbModel
        {
            FollowedId = model.FollowedId, FollowerId = model.FollowerId
        };
        _articlesDbContext.Add(following);
        await _articlesDbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(
        RemoveFollowingEventModel model)
    {
        var following = await _articlesDbContext.Following.SingleAsync(x =>
            x.FollowedId == model.FollowedId &&
            x.FollowerId == model.FollowerId);
        _articlesDbContext.Remove(following);
        await _articlesDbContext.SaveChangesAsync();
    }
}
