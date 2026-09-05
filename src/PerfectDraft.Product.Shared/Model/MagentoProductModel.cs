namespace PerfectDraft.Product.Shared.Model
{
    public class MagentoProductModel
    {
        public required string Sku { get; set; }

        public required string Name { get; set; }

        public required string Currency { get; set; }

        public decimal Price { get; set; }

        public long Stock { get; set; }
    }
}
