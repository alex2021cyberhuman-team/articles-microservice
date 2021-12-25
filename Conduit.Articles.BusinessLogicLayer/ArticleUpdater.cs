using Conduit.Articles.DomainLayer;

namespace Conduit.Articles.BusinessLogicLayer;

public class ArticleUpdater : IArticleUpdater
{
    private readonly IArticleWriteRepository _articleWriteRepository;

    public ArticleUpdater(
        IArticleWriteRepository articleWriteRepository)
    {
        _articleWriteRepository = articleWriteRepository;
    }
    
    public async Task<SingleArticle> UpdateAsync(
        UpdateArticle article,
        CancellationToken cancellationToken)
    {
        // валидация
        // маппинг
        // обновление и получение
        var singleArticle = await _articleWriteRepository.UpdateAsync(article, cancellationToken);
        // триггер события
        return singleArticle;
    }
}
