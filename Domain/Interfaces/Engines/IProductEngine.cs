using Contracts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Engines
{
    public interface IProductEngine
    {
        Task<int> CreateProductAsync(ProductRequest product);
        Task<IEnumerable<ProductResponse>> GetAllProductsAsync();
        Task<ProductResponse> GetProductByIdAsync(int productId);
    }
}
