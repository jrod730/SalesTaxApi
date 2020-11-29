using Contracts.Enums;

namespace Contracts.Models
{
    public class ProductResponse
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public TaxType TaxType { get; set; }
    }
}
