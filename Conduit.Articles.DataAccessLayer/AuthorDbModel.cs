namespace Conduit.Articles.DataAccessLayer;

public class AuthorDbModel
{
    public Guid Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string? Bio { get; set; }

    public string? Image { get; set; }

    public ICollection<FollowingDbModel> Followers { get; set; } = null!;

    public ICollection<FollowingDbModel> Followeds { get; set; } = null!;
}