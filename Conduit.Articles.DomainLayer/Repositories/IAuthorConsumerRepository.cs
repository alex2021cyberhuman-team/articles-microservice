using Conduit.Shared.Events.Models.Users.Register;
using Conduit.Shared.Events.Models.Users.Update;

namespace Conduit.Articles.DomainLayer.Repositories;

public interface IAuthorConsumerRepository
{
    Task RegisterAsync(
        RegisterUserEventModel model);

    Task UpdateAsync(
        UpdateUserEventModel model);
}
