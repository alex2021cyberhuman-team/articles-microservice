using System;
using System.Collections.Generic;

namespace Conduit.Articles.DomainLayer;

public class UpdateArticle
{
    public class Request
    {
        public Request(
            RequestBody body,
            Guid userId,
            string slug)
        {
            Body = body;
            UserId = userId;
            Slug = slug;
        }

        public RequestBody Body { get; set; }

        public Guid UserId { get; set; }

        public string Slug { get; set; }
    }

    public class RequestBody
    {
        public Model Article { get; set; } = new();
    }

    public class Model
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Body { get; set; }

        public HashSet<string>? TagList { get; set; }
    }
}
