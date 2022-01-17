using Conduit.Articles.BusinessLogicLayer;
using Conduit.Articles.DataAccessLayer;
using Conduit.Articles.DataAccessLayer.DbContexts;
using Conduit.Articles.DataAccessLayer.Repositories;
using Conduit.Articles.DataAccessLayer.Utilities;
using Conduit.Articles.DomainLayer;
using Conduit.Articles.DomainLayer.Handlers;
using Conduit.Articles.DomainLayer.Models;
using Conduit.Articles.DomainLayer.Repositories;
using Conduit.Articles.DomainLayer.Utilities;
using Conduit.Articles.PresentationLayer;
using Conduit.Shared.Events.Models.Articles.CreateArticle;
using Conduit.Shared.Events.Models.Articles.DeleteArticle;
using Conduit.Shared.Events.Models.Articles.UpdateArticle;
using Conduit.Shared.Events.Models.Favorites;
using Conduit.Shared.Events.Models.Profiles.CreateFollowing;
using Conduit.Shared.Events.Models.Profiles.RemoveFollowing;
using Conduit.Shared.Events.Models.Users.Register;
using Conduit.Shared.Events.Models.Users.Update;
using Conduit.Shared.Events.Services.RabbitMQ;
using Conduit.Shared.Startup;
using Conduit.Shared.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

#region ServicesConfiguration

var services = builder.Services;
var environment = builder.Environment;
var configuration = builder.Configuration;

services.AddControllers();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new() { Title = "Conduit.Person.WebApi", Version = "v1" });
});

services.AddJwtServices(configuration.GetSection("Jwt").Bind)
    .AddDbContext<ArticlesDbContext>(optionsBuilder =>
    {
        if (environment.IsDevelopment())
        {
            optionsBuilder.EnableDetailedErrors().EnableSensitiveDataLogging();
        }

        optionsBuilder.UseSnakeCaseNamingConvention()
            .UseNpgsql(configuration.GetConnectionString("Articles"));
    }).AddScoped<IArticleCreator, ArticleCreator>()
    .AddScoped<IArticleDeleter, ArticleDeleter>()
    .AddScoped<IArticleReadRepository, ArticleReadRepository>()
    .AddScoped<IArticleUpdater, ArticleUpdater>()
    .AddScoped<IArticleWriteRepository, ArticleWriteRepository>()
    .AddScoped<IAuthorConsumerRepository, AuthorConsumerRepository>()
    .AddScoped<IFavoritesConsumerRepository, FavoritesConsumerRepository>()
    .AddScoped<IFollowingsConsumerRepository, FollowingsConsumerRepository>()
    .AddScoped<ITagRepository, TagRepository>()
    .AddScoped<ISlugilizator, Slugilizator>()
    .AddScoped<IValidator<UpdateArticle.Request>,
        UpdateArticleRequestValidator>()
    .AddScoped<IValidator<CreateArticle.Request>,
        CreateArticleRequestValidator>()
    .AddW3CLogging(configuration.GetSection("W3C").Bind).AddHttpClient()
    .AddHttpContextAccessor()
    .RegisterRabbitMqWithHealthCheck(configuration.GetSection("RabbitMQ").Bind)
    .RegisterProducer<CreateArticleEventModel>()
    .RegisterProducer<DeleteArticleEventModel>()
    .RegisterProducer<UpdateArticleEventModel>()
    .RegisterConsumer<CreateFollowingEventModel, CreateFollowingEventConsumer>()
    .RegisterConsumer<RemoveFollowingEventModel, RemoveFollowingEventConsumer>()
    .RegisterConsumer<UnfavoriteArticleEventModel,
        UnfavoriteArticleEventConsumer>()
    .RegisterConsumer<RegisterUserEventModel, RegisterUserEventConsumer>()
    .RegisterConsumer<FavoriteArticleEventModel, FavoriteArticleEventConsumer>()
    .RegisterConsumer<UpdateUserEventModel, UpdateUserEventConsumer>()
    .AddHealthChecks().AddDbContextCheck<ArticlesDbContext>();

#endregion

var app = builder.Build();

#region AppConfiguration

app.UseExceptionHandler(applicationBuilder =>
{
    applicationBuilder.UseMiddleware<ExceptionFilter>();
});

if (environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        c.SwaggerEndpoint("/swagger/v1/swagger.json",
            "Conduit.Person.WebApi v1"));
    IdentityModelEventSource.ShowPII = true;
}

app.UseW3CLogging();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var initializationScope = app.Services.CreateScope();

await initializationScope.WaitHealthyServicesAsync(TimeSpan.FromHours(1));
await initializationScope.InitializeDatabase();
await initializationScope.InitializeQueuesAsync();

#endregion

app.Run();
