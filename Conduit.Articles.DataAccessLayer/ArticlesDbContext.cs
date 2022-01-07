using Microsoft.EntityFrameworkCore;

namespace Conduit.Articles.DataAccessLayer;

public class ArticlesDbContext : DbContext
{
    protected ArticlesDbContext()
    {
    }

    public ArticlesDbContext(
        DbContextOptions<ArticlesDbContext> options) : base(options)
    {
    }

    public DbSet<TagDbModel> Tag => Set<TagDbModel>();

    public DbSet<ArticleDbModel> Article => Set<ArticleDbModel>();

    public DbSet<AuthorDbModel> Author => Set<AuthorDbModel>();

    public DbSet<FollowingDbModel> Following => Set<FollowingDbModel>();
}
