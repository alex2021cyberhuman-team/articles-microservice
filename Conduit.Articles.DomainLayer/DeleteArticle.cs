namespace Conduit.Articles.DomainLayer;

public static class DeleteArticle
{
    public class Request
    {
        public Request(
            string currentUsername,
            Guid currentUserId,
            string slug)
        {
            CurrentUsername = currentUsername;
            CurrentUserId = currentUserId;
            Slug = slug;
        }

        public string CurrentUsername { get; set; }

        public Guid CurrentUserId { get; set; }
        public string Slug { get; set; }
    }
}
