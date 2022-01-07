using Conduit.Articles.DomainLayer;
using Conduit.Shared.Events.Models.Articles.DeleteArticle;
using Conduit.Shared.Events.Services;

namespace Conduit.Articles.BusinessLogicLayer;

public class ArticleDeleter : IArticleDeleter
{
    private readonly IArticleWriteRepository _articleWriteRepository;
    private readonly IEventProducer<DeleteArticleEventModel> _eventProducer;

    public ArticleDeleter(
        IArticleWriteRepository articleWriteRepository,
        IEventProducer<DeleteArticleEventModel> eventProducer)
    {
        _articleWriteRepository = articleWriteRepository;
        _eventProducer = eventProducer;
    }

    public async Task DeleteAsync(
        DeleteArticle.Request article,
        CancellationToken cancellationToken)
    {
        await _articleWriteRepository.DeleteAsync(article, cancellationToken);
        await ProduceEventAsync(article);
    }

    private async Task ProduceEventAsync(
        DeleteArticle.Request article)
    {
        await _eventProducer.ProduceEventAsync(new()
        {
            AuthorUsername = article.CurrentUsername, Slug = article.Slug
        });
    }
}
