using Conduit.Shared.Events.Services;

namespace Conduit.Articles.BusinessLogicLayer;

public class UpdateUserEventConsumer : IEventConsumer<UpdateUserEventConsumer>
{
    public Task ConsumeAsync(
        UpdateUserEventConsumer message)
    {
        throw new NotImplementedException();
    }
}
