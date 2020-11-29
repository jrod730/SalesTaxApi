using Contracts.Enums;

namespace Contracts.Models
{
    public class ProductRequest
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public TaxType TaxType { get; set; }
    }
}
