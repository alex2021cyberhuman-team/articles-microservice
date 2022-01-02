using System.ComponentModel;

namespace Conduit.Articles.DomainLayer;

public static class SearchArticle
{
    public class Request
    {
        public Request(
            QueryParams query)
        {
            Query = query;
        }

        public QueryParams Query { get; set; }
    }
    
    public class QueryParams
    {
        public string? Tag { get; set; }
        
        public string? Author { get; set; }
        
        public string? Favorited { get; set; }
        
        [DefaultValue(20)]
        public int Limit { get; set; } = 20;
        
        [DefaultValue(0)]
        public int Offset { get; set; } = 0;
    }
}