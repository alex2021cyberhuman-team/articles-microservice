﻿using Conduit.Shared.Validation;

namespace Conduit.Articles.DomainLayer;

public static class UpdateArticle
{
    public class Request
    {
        public Request(
            RequestBody body,
            string currentUsername,
            string slug,
            Guid currentUserId)
        {
            Body = body;
            CurrentUsername = currentUsername;
            Slug = slug;
            CurrentUserId = currentUserId;
        }

        [NestedValidation]
        public RequestBody Body { get; set; }

        public string CurrentUsername { get; set; }

        public string Slug { get; set; }

        public Guid CurrentUserId { get; set; }
    }

    public class RequestBody
    {
        [NestedValidation]
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
