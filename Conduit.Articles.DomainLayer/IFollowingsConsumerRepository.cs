using Conduit.Shared.Events.Models.Profiles.CreateFollowing;
using Conduit.Shared.Events.Models.Profiles.RemoveFollowing;

namespace Conduit.Articles.DomainLayer;

public interface IFollowingsConsumerRepository
{
    Task CreateAsync(
        CreateFollowingEventModel model);

    Task RemoveAsync(
        RemoveFollowingEventModel model);
}
