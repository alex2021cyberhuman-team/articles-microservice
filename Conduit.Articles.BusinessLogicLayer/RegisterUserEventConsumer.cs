using Conduit.Shared.Events.Models.Users.Register;
using Conduit.Shared.Events.Services;

namespace Conduit.Articles.BusinessLogicLayer;

public class RegisterUserEventConsumer : IEventConsumer<RegisterUserEventModel>
{
    public Task ConsumeAsync(
        RegisterUserEventModel message)
    {
        throw new NotImplementedException();
    }
}
