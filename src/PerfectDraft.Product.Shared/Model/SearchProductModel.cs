using System.Text.Json.Serialization;

namespace PerfectDraft.Product.Shared.Model
{
    public class SearchProductModel
    {
        [JsonPropertyName("objectID")]
        public required string Sku { get; set; }

        public required string Image { get; set; }

        public required string Title { get; set; }

        public decimal Price { get; set; }

        public bool InStock { get; set; }
    }
}
