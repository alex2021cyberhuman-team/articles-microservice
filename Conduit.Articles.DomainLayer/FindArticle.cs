namespace Conduit.Articles.DomainLayer;

public static class FindArticle
{
    public class Request
    {
        public Request(
            string slug)
        {
            Slug = slug;
        }
        
        public string Slug { get; set; }
    }
}
