using Contracts.Models;
using Domain.Interfaces.Engines;
using Domain.Interfaces.Storage.Repositories;
using Domain.Interfaces.Strategies;
using Domain.Models;
using Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engines
{
    public class ReceiptEngine : EngineBase <ReceiptEngine>, IReceiptEngine
    {
        private readonly IEnumerable<ITaxCalculationStrategy> _taxCalculationStrategies;
        private readonly IProductRepository _productRepository;

        public ReceiptEngine(ILogger<ReceiptEngine> logger, IEnumerable<ITaxCalculationStrategy> taxCalculationStrategies, IProductRepository productRepository) 
            : base (logger)
        {
            _taxCalculationStrategies = taxCalculationStrategies.ThrowIfNull(nameof(taxCalculationStrategies));
            _productRepository = productRepository.ThrowIfNull(nameof(productRepository));
        }

        public async Task<string> GenerateReceiptAsync(IEnumerable<ReceiptRequest> receiptRequest)
        {
            var products = await _productRepository.GetAllProductsAsync().ConfigureAwait(false);

            //Real world application a sProc should be created to do the heavy lifting for matching the request ids to the database ids. 
            //DataTable could be created and passed as type table param which can be joined on for data retrieval. Which should be more effecient .
            products = products.Where(p => receiptRequest.Any(r => r.ProductId == p.ProductId));

            CalculatePriceWithTax(products);
            var productListWithQuantities = SetQuantityOnProductList(products, receiptRequest);
            return GenerateReceiptString(productListWithQuantities);
        }

        private void CalculatePriceWithTax(IEnumerable<Product> products)
        {
            try
            {
                foreach (var product in products)
                {
                    product.PriceWithTax = _taxCalculationStrategies.First(x => x.TaxType == product.TaxTypeId).Calculate(product.Price);
                }
            }
            catch (Exception exception) when (exception is ArgumentNullException || exception is InvalidOperationException)
            {
                Logger.LogError(exception, "An error occurred while selecting strategy - Products: {Products}", products);
                throw;
            }
        }

        private string GenerateReceiptString(IEnumerable<Product> products)
        {
            var stringBuilder = new StringBuilder();
            var salesTaxTotal = 0m;
            var totalWithTax = 0m;

            try 
            {
                foreach (var product in products)
                {

                    if (product.Quantity == 1)
                    {
                        var lineOfReceipt = $"{product.Name}: {product.PriceWithTax} {Environment.NewLine}";
                        salesTaxTotal += product.PriceWithTax - product.Price;
                        totalWithTax += product.PriceWithTax;

                        stringBuilder.Append(lineOfReceipt);
                    }
                    else
                    {
                        var totalPriceWithTax = product.PriceWithTax * product.Quantity;
                        var totalPrice = product.Price * product.Quantity;
                        salesTaxTotal += totalPriceWithTax - totalPrice;
                        totalWithTax += totalPriceWithTax;

                        var lineOfReceipt = $"{product.Name}: {totalPriceWithTax} ({product.Quantity} @ {product.PriceWithTax}) {Environment.NewLine}";

                        stringBuilder.Append(lineOfReceipt);
                    }
                }

                stringBuilder.Append($"Sales Taxes: {salesTaxTotal} {Environment.NewLine}");
                stringBuilder.Append($"Total: {totalWithTax} {Environment.NewLine}");

                return stringBuilder.ToString();
            }
            catch (Exception exception) when (exception is ArgumentOutOfRangeException)
            {
                Logger.LogError(exception, "An error occurred while trying to generate string for receipt");
                throw;
            }
        }

        private IEnumerable<Product> SetQuantityOnProductList(IEnumerable<Product> products, IEnumerable<ReceiptRequest> receiptRequest)
        {
            try
            {
                //Group request by productIds - Protect against users selecting multiple of same item.
                var receiptRequestGroupedByProductId = receiptRequest.GroupBy(r => r.ProductId)
                    .Select(grouping => new { ProductID = grouping.Key, ReceiptRequests = grouping.ToList() })
                    .ToList();

                //Calculate total quantity and set on product list.
                var productListWithQuantities = new List<Product>();
                foreach (var receiptRequestGroup in receiptRequestGroupedByProductId)
                {
                    var totalQuantity = receiptRequestGroup.ReceiptRequests.Select(r => r.Quantity).Aggregate((total, next) => total + next);
                    productListWithQuantities.AddRange(
                        products.Where(p => p.ProductId == receiptRequestGroup.ProductID)
                        .Select(x => {
                            x.Quantity = totalQuantity;
                            return x;
                        }));
                }

                return productListWithQuantities;
            }
            catch (Exception exception) when (exception is ArgumentNullException)
            {
                Logger.LogError(exception, "An error occurred while setting quantity on product list - Products: {Products} - Request: {Request}", 
                    products, receiptRequest);
                throw;
            }
        }
    }
}
