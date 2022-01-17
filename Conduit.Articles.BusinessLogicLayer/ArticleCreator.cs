using Conduit.Articles.DomainLayer;
using Conduit.Articles.DomainLayer.Handlers;
using Conduit.Articles.DomainLayer.Models;
using Conduit.Articles.DomainLayer.Repositories;
using Conduit.Articles.DomainLayer.Utilities;
using Conduit.Shared.Events.Models.Articles.CreateArticle;
using Conduit.Shared.Events.Services;

namespace Conduit.Articles.BusinessLogicLayer;

public class ArticleCreator : IArticleCreator
{
    private readonly IArticleWriteRepository _articleWriteRepository;
    private readonly IEventProducer<CreateArticleEventModel> _eventProducer;
    private readonly IValidator<CreateArticle.Request> _validator;

    public ArticleCreator(
        IArticleWriteRepository articleWriteRepository,
        IValidator<CreateArticle.Request> validator,
        IEventProducer<CreateArticleEventModel> eventProducer)
    {
        _articleWriteRepository = articleWriteRepository;
        _validator = validator;
        _eventProducer = eventProducer;
    }

    public async Task<SingleArticle> CreateAsync(
        CreateArticle.Request article,
        CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(article, cancellationToken);
        var singleArticle =
            await _articleWriteRepository.CreateAsync(article,
                cancellationToken);
        await ProduceEvent(singleArticle, article);
        return singleArticle;
    }

    private async Task ProduceEvent(
        SingleArticle singleArticle,
        CreateArticle.Request request)
    {
        await _eventProducer.ProduceEventAsync(new()
        {
            Slug = singleArticle.Response.Article.Slug,
            Title = singleArticle.Response.Article.Title,
            Description = singleArticle.Response.Article.Description,
            Body = singleArticle.Response.Article.Body,
            TagList = singleArticle.Response.Article.TagList,
            CreatedAt = singleArticle.Response.Article.CreatedAt,
            UpdatedAt = singleArticle.Response.Article.UpdatedAt,
            AuthorId = request.CurrentUserId
        });
    }
}
