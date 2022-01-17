namespace Conduit.Articles.DomainLayer.Models;

public static class TagList
{
    public class Response
    {
        public ResponseBody Body { get; set; }
    }
    
    public class ResponseBody
    {
        public IEnumerable<string> Tags { get; set; } = Array.Empty<string>();
    }
}
