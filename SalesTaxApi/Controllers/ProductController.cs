using Contracts.Enums;
using Contracts.Models;
using Domain.Interfaces.Engines;
using Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SalesTaxApi.Controllers
{
    [Route("[controller]")]
    public class ProductController : ApiControllerBase<ProductController>
    {
        private readonly IProductEngine _productEngine;
        public ProductController(ILogger<ProductController> logger, IProductEngine productEngine) : base(logger)
        {
            _productEngine = productEngine.ThrowIfNull(nameof(productEngine));
        }

        /// <summary>
        /// Creates a new product and returns the product id of newly created product.
        /// </summary>
        /// <remarks>
        /// sample request:
        ///
        ///     POST 
        ///     {
        ///        "Name": "A Least One Letter",
        ///        "Price": 7.72,
        ///        "TaxType": 0, [0 = None, 1 = Basic, 2 = Import, 3 = ImportNoBasic ]
        ///     }
        ///     
        /// </remarks>
        /// <param name="product"></param>
        /// <returns>Returns product id.</returns>
        /// <response code="201">Product id for created product</response>
        /// <response code="400">Bad Request when request is null, product name is not at least one letter, productId  is less than zero or tax type is nonexistent</response>
        /// <response code="500">Internal server error when an error occurs during processing request</response>
        [HttpPost, Route("create-product")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<int>> CreateProductAsync(ProductRequest product)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(product.Name))
                {
                    return BadRequest("Product must have a name that contains at least one letter.");
                }

                if (product.Price <= 0)
                {
                    return BadRequest("Product price should be greater than 0");
                }

                if (!Enum.IsDefined(typeof(TaxType), product.TaxType))
                {
                    return BadRequest("TaxType prodived must be within TaxType range");
                }
                
                return StatusCode(201, await _productEngine.CreateProductAsync(product).ConfigureAwait(false));
            }
            catch (Exception exception) when (exception is SqlException || exception is ArgumentNullException)
            {
                return StatusCode(500, "Sorry an error occurred processing your request.");
            }
        }

        /// <summary>
        /// Retrieves all products from the database.
        /// </summary>
        /// <returns>Returns all products in db.</returns>
        /// <response code="200">Return all products in db</response>
        /// <response code="500">Internal server error when an error occurs during processing request</response>

        [HttpGet, Route("all-products")]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAllProductsAsync()
        {
            try
            {
                return Ok(await _productEngine.GetAllProductsAsync().ConfigureAwait(false));
            }
            catch (Exception exception) when (exception is SqlException || exception is ArgumentNullException)
            {
                return StatusCode(500, "Sorry an error occurred processing your request.");
            }
        }

        /// <summary>
        /// Retrieves one product by id from the database.
        /// </summary>
        /// <returns>Returns product by id</returns>
        /// <response code="200">Returns product by id</response>
        /// <response code="500">Internal server error when an error occurs during processing request</response>

        [HttpGet, Route("{productId}")]
        public async Task<ActionResult<ProductResponse>> GetProductByIdAsync(int productId)
        {
            try
            {
                return Ok(await _productEngine.GetProductByIdAsync(productId).ConfigureAwait(false));
            }
            catch (Exception exception) when (exception is SqlException || exception is ArgumentNullException)
            {
                return StatusCode(500, "Sorry an error occurred processing your request.");
            }
        }
    }
}
