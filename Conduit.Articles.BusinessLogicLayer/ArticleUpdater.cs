using Conduit.Articles.DomainLayer;
using Conduit.Articles.DomainLayer.Handlers;
using Conduit.Articles.DomainLayer.Models;
using Conduit.Articles.DomainLayer.Repositories;
using Conduit.Articles.DomainLayer.Utilities;
using Conduit.Shared.Events.Models.Articles.UpdateArticle;
using Conduit.Shared.Events.Services;

namespace Conduit.Articles.BusinessLogicLayer;

public class ArticleUpdater : IArticleUpdater
{
    private readonly IArticleWriteRepository _articleWriteRepository;
    private readonly IEventProducer<UpdateArticleEventModel> _eventProducer;
    private readonly IValidator<UpdateArticle.Request> _validator;

    public ArticleUpdater(
        IArticleWriteRepository articleWriteRepository,
        IValidator<UpdateArticle.Request> validator,
        IEventProducer<UpdateArticleEventModel> eventProducer)
    {
        _articleWriteRepository = articleWriteRepository;
        _validator = validator;
        _eventProducer = eventProducer;
    }

    public async Task<SingleArticle> UpdateAsync(
        UpdateArticle.Request article,
        CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(article, cancellationToken);
        var singleArticle =
            await _articleWriteRepository.UpdateAsync(article,
                cancellationToken);
        await ProduceEvent(singleArticle, article);
        return singleArticle;
    }
    
    private async Task ProduceEvent(
        SingleArticle singleArticle,
        UpdateArticle.Request request)
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
