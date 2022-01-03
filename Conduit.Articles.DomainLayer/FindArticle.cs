using System.ComponentModel.DataAnnotations;

namespace Conduit.Articles.DomainLayer;

public static class FindArticle
{
    public class Request
    {
        public Request(
            QueryParams query,
            Guid currentUserId)
        {
            Query = query;
            CurrentUserId = currentUserId;
        }

        public QueryParams Query { get; set; }

        public Guid CurrentUserId { get; set; }
    }

    public class QueryParams
    {
        [Required]
        public string Slug { get; set; } = string.Empty;
    }
}
