using Contracts.Models;
using Domain.Interfaces.Engines;
using Domain.Interfaces.Storage.Repositories;
using Domain.Models;
using Helpers;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engines
{
    public class ProductEngine : EngineBase<ProductEngine>, IProductEngine
    {
        private readonly IProductRepository _productRepository;
        public ProductEngine(ILogger<ProductEngine> logger, IProductRepository productRepository) : base(logger)
        {
            _productRepository = productRepository.ThrowIfNull(nameof(productRepository));
        }

        public async Task<int> CreateProductAsync(ProductRequest productRequest)
        {
            var product = new Product(productRequest);
            return await _productRepository.InsertProductAsync(product).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync().ConfigureAwait(false);

            var productsResponse = products.Select(p => new ProductResponse()
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                TaxType = (Contracts.Enums.TaxType) p.TaxTypeId
            }).ToList();

            return productsResponse;
        }

        public async Task<ProductResponse> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId).ConfigureAwait(false);

            var productResponse =  new ProductResponse()
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                TaxType = (Contracts.Enums.TaxType) product.TaxTypeId
            };

            return productResponse;
        }
    }
}
