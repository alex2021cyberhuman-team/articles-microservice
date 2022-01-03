using System.ComponentModel.DataAnnotations;
using Conduit.Shared.Validation;

namespace Conduit.Articles.DomainLayer;

public static class CreateArticle
{
    public class RequestBody
    {
        [NestedValidation]
        public Model Article { get; set; } = new();
    }

    public class Request
    {
        public Request(
            RequestBody body,
            string currentUsername,
            Guid currentUserId)
        {
            Body = body;
            CurrentUsername = currentUsername;
            CurrentUserId = currentUserId;
        }

        [NestedValidation]
        public RequestBody Body { get; set; }

        public string CurrentUsername { get; set; }

        public Guid CurrentUserId { get; set; }
    }

    public class Model
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Body { get; set; } = string.Empty;

        public HashSet<string> TagList { get; set; } = new();
    }
}
