using Conduit.Articles.DomainLayer;

namespace Conduit.Articles.BusinessLogicLayer;

public class ArticleCreator : IArticleCreator
{
    private readonly IArticleWriteRepository _articleWriteRepository;
    private readonly IValidator<CreateArticle.Request> _validator;

    public ArticleCreator(
        IArticleWriteRepository articleWriteRepository,
        IValidator<CreateArticle.Request> validator)
    {
        _articleWriteRepository = articleWriteRepository;
        _validator = validator;
    }

    public async Task<SingleArticle> CreateAsync(
        CreateArticle.Request article,
        CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(article, cancellationToken);
        var singleArticle = await _articleWriteRepository.CreateAsync(article, cancellationToken);
        return singleArticle;
    }
}
