namespace Caching_with_Redis.Controllers.Products.Request
{
    public class ProuductRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }
}
