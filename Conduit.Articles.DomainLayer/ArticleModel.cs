namespace Conduit.Articles.DomainLayer;

public class ArticleModel
{
    public ArticleModel(
        string slug,
        string title,
        string description,
        string body,
        ISet<string> tagList,
        DateTime createdAt,
        DateTime updatedAt,
        bool favorited,
        int favoritesCount,
        AuthorModel author)
    {
        Slug = slug;
        Title = title;
        Description = description;
        Body = body;
        TagList = tagList;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Favorited = favorited;
        FavoritesCount = favoritesCount;
        Author = author;
    }

    public ArticleModel()
    {
    }

    public string Slug { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public ISet<string> TagList { get; set; } = new HashSet<string>();

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    // TODO: Make favorites and likes and comments
    public bool Favorited { get; set; }

    public int FavoritesCount { get; set; }

    public AuthorModel Author { get; set; } = new();
}

/*
"article": {
    "slug": "how-to-train-your-dragon",
    "title": "How to train your dragon",
    "description": "Ever wonder how?",
    "body": "It takes a Jacobian",
    "tagList": ["dragons", "training"],
    "createdAt": "2016-02-18T03:22:56.637Z",
    "updatedAt": "2016-02-18T03:48:35.824Z",
    "favorited": false,
    "favoritesCount": 0,
    "author": {
        "username": "jake",
        "bio": "I work at statefarm",
        "image": "https://i.stack.imgur.com/xHWG8.jpg",
        "following": false
    }
} 
 */
