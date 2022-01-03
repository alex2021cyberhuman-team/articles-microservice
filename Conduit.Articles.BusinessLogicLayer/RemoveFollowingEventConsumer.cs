using Conduit.Shared.Events.Models.Profiles.RemoveFollowing;
using Conduit.Shared.Events.Services;

namespace Conduit.Articles.BusinessLogicLayer;

public class RemoveFollowingEventConsumer : IEventConsumer<RemoveFollowingEventModel>
{
    public Task ConsumeAsync(
        RemoveFollowingEventModel message)
    {
        throw new NotImplementedException();
    }
}
