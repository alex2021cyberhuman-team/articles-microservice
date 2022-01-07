﻿using Conduit.Articles.DomainLayer;
using Conduit.Shared.Events.Models.Favorites;
using Conduit.Shared.Events.Services;

namespace Conduit.Articles.BusinessLogicLayer;

public class UnfavoriteArticleEventConsumer : IEventConsumer<UnfavoriteArticleEventModel>
{
    private readonly IFavoritesConsumerRepository _favoritesConsumerRepository;

    public UnfavoriteArticleEventConsumer(
        IFavoritesConsumerRepository favoritesConsumerRepository)
    {
        _favoritesConsumerRepository = favoritesConsumerRepository;
    }
    
    public async Task ConsumeAsync(
        UnfavoriteArticleEventModel message)
    {
        await _favoritesConsumerRepository.UnfavoriteAsync(message);
    }
}