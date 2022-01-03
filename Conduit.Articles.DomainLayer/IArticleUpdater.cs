using System.Threading;
using System.Threading.Tasks;

namespace Conduit.Articles.DomainLayer;

public interface IArticleUpdater
{
    Task<SingleArticle> UpdateAsync(
        UpdateArticle.Request article,
        CancellationToken cancellationToken);
}
