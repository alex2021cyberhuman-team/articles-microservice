using System;
using System.Collections.Generic;

namespace Conduit.Articles.DomainLayer;

public class CreateArticle
{
    public class RequestBody
    {
        public Model Article { get; set; } = new();
    }

    public class Request
    {
        public Request(
            RequestBody body,
            Guid userId)
        {
            Body = body;
            UserId = userId;
        }

        public RequestBody Body { get; set; }

        public Guid UserId { get; set; }
    }
    
    public class Model
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public HashSet<string> TagList { get; set; } = new();
    }
}
