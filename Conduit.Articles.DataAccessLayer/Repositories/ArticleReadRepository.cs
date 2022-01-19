﻿using System.Linq.Expressions;
using Conduit.Articles.DataAccessLayer.DbContexts;
using Conduit.Articles.DataAccessLayer.Models;
using Conduit.Articles.DataAccessLayer.Utilities;
using Conduit.Articles.DomainLayer.Exceptions;
using Conduit.Articles.DomainLayer.Models;
using Conduit.Articles.DomainLayer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Articles.DataAccessLayer.Repositories;

public class ArticleReadRepository : IArticleReadRepository
{
    private static readonly Expression<Func<ArticleDbModel, ArticleModel>>
        SelectExpression = x => new()
        {
            Slug = x.Slug,
            Title = x.Title,
            Description = x.Description,
            Body = x.Body,
            TagList = x.Tags.Select(y => y.Name).ToHashSet(),
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Favorited = x.Favoriters.Any(),
            Author = new()
            {
                Username = x.Author.Username,
                Bio = x.Author.Bio,
                Following = x.Author.Followers.Any()
            }
        };

    private readonly ArticlesDbContext _context;

    public ArticleReadRepository(
        ArticlesDbContext context)
    {
        _context = context;
    }

    public async Task<SingleArticle> FindAsync(
        FindArticle.Request request,
        CancellationToken cancellationToken = default)
    {
        var article = await _context.Article.Include(x => x.Tags)
            .Include(x => x.Author)
            .ThenInclude(x =>
                x.Followers.Where(y => y.Id == request.CurrentUserId))
            .FirstOrDefaultAsync(x => x.Slug == request.Slug,
                cancellationToken);

        var result =
            (article ?? throw new NotFoundException()).MapArticle(
                article.Author.Followers.Any(),
                article.Favoriters.Any());

        return result;
    }

    public async Task<MultipleArticles> SearchAsync(
        SearchArticles.Request request,
        CancellationToken cancellationToken = default)
    {
        var query = Include(request.CurrentUserId);
        query = FilterQuery(request, query);
        return await ReturnMultipleArticles(query, request.Query.Offset,
            request.Query.Limit, cancellationToken);
    }

    public async Task<MultipleArticles> FeedAsync(
        FeedArticle.Request request,
        CancellationToken cancellationToken = default)
    {
        var query = Include(request.CurrentUserId);
        query = query.Where(x => x.Author.Followers.Any());
        return await ReturnMultipleArticles(query, request.Query.Offset,
            request.Query.Limit, cancellationToken);
    }

    private static async Task<MultipleArticles> ReturnMultipleArticles(
        IQueryable<ArticleDbModel> query,
        int queryOffset,
        int queryLimit,
        CancellationToken cancellationToken)
    {
        var articleCount = await query.CountAsync(cancellationToken);
        var articles = await ToListAsync(query, queryOffset, queryLimit,
            cancellationToken);
        var result = new MultipleArticles(articles, articleCount);
        return result;
    }

    private static IQueryable<ArticleDbModel> FilterQuery(
        SearchArticles.Request request,
        IQueryable<ArticleDbModel> query)
    {
        return query.Where(x =>
            (request.Query.Author == null ||
             x.Author.Username == request.Query.Author) &&
            (request.Query.Tag == null ||
             x.Tags.Select(y => y.Name).Contains(request.Query.Tag)));
    }

    private IQueryable<ArticleDbModel> Include(
        Guid? requestCurrentUserId)
    {
        return _context.Article
            .Include(x => x.Favoriters.Where(y => y.Id == requestCurrentUserId))
            .Include(x => x.Tags).Include(x => x.Author).ThenInclude(x =>
                x.Followers.Where(y => y.Id == requestCurrentUserId));
    }

    private static async Task<List<ArticleModel>> ToListAsync(
        IQueryable<ArticleDbModel> query,
        int queryOffset,
        int queryLimit,
        CancellationToken cancellationToken)
    {
        return await query.Select(SelectExpression).OrderBy(x => x.CreatedAt)
            .Skip(queryOffset).Take(queryLimit).ToListAsync(cancellationToken);
    }
}