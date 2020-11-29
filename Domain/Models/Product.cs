using Contracts.Models;
using Domain.Enums;

namespace Domain.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public TaxType TaxTypeId { get; set; }
        public decimal PriceWithTax { get; set; }
        public int Quantity { get; set; }

        public Product() { }

        public Product(ProductRequest productRequest)
        {
            Name = productRequest.Name;
            Price = productRequest.Price;
            TaxTypeId = (TaxType) productRequest.TaxType; 
        }
    }
}
