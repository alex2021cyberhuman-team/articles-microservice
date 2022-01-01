namespace Conduit.Articles.DataAccessLayer;

public class FollowingDbModel
{
    public Guid FollowerId { get; set; }

    public AuthorDbModel Follower { get; set; } = null!;

    public Guid FollowedId { get; set; }

    public AuthorDbModel Followed { get; set; } = null!;
}