using Conduit.Articles.DomainLayer;

namespace Conduit.Articles.BusinessLogicLayer;

public class ArticleUpdater : IArticleUpdater
{
    private readonly IArticleWriteRepository _articleWriteRepository;
    private readonly IValidator<UpdateArticle.Request> _validator;

    public ArticleUpdater(
        IArticleWriteRepository articleWriteRepository,
        IValidator<UpdateArticle.Request> validator)
    {
        _articleWriteRepository = articleWriteRepository;
        _validator = validator;
    }
    
    public async Task<SingleArticle> UpdateAsync(
        UpdateArticle.Request article,
        CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(article, cancellationToken);
        var singleArticle = await _articleWriteRepository.UpdateAsync(article, cancellationToken);
        return singleArticle;
    }
}
