using Conduit.Shared.Events.Models.Profiles.CreateFollowing;
using Conduit.Shared.Events.Services;

namespace Conduit.Articles.BusinessLogicLayer;

public class CreateFollowingEventConsumer : IEventConsumer<CreateFollowingEventModel>
{
    public Task ConsumeAsync(
        CreateFollowingEventModel message)
    {
        throw new NotImplementedException();
    }
}
