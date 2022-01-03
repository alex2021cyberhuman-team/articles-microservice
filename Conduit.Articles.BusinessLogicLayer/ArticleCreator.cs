using Conduit.Articles.DomainLayer;

namespace Conduit.Articles.BusinessLogicLayer;

public class ArticleCreator : IArticleCreator
{
    private readonly IArticleWriteRepository _articleWriteRepository;

    public ArticleCreator(
        IArticleWriteRepository articleWriteRepository)
    {
        _articleWriteRepository = articleWriteRepository;
    }

    public async Task<SingleArticle> CreateAsync(
        CreateArticle.Request article,
        CancellationToken cancellationToken)
    {
        // валидация 
        // сохраниение и получение
        var singleArticle = await _articleWriteRepository.CreateAsync(article, cancellationToken);
        // триггер события
        return singleArticle;
    }
}
