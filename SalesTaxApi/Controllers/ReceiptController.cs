using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Models;
using Domain.Interfaces.Engines;
using Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SalesTaxApi.Controllers
{
    [Route("[controller]")]
    public class ReceiptController : ApiControllerBase<ReceiptController>
    {
        private readonly IReceiptEngine _receiptEngine;
        public ReceiptController(ILogger<ReceiptController> logger, IReceiptEngine receiptEngine) 
            : base (logger)
        {
            _receiptEngine = receiptEngine.ThrowIfNull(nameof(receiptEngine));
        }

        /// <summary>
        /// Generates a receipt for a given product id with quanity.
        /// </summary>
        /// <remarks>
        /// Input 1 request:
        ///
        ///     POST /Receipt
        ///     {
        ///        "ProductId": 1,
        ///        "Quantity": 1
        ///     },
        ///     {
        ///        "ProductId": 1,
        ///        "Quantity": 1
        ///     },
        ///     {
        ///        "ProductId": 2,
        ///        "Quantity": 1
        ///     },
        ///     {
        ///        "ProductId": 3,
        ///        "Quantity": 1
        ///     }
        ///
        /// Input 2 request:
        ///
        ///     POST /Receipt
        ///     {
        ///        "ProductId": 4,
        ///        "Quantity": 1
        ///     },
        ///     {
        ///        "ProductId": 5,
        ///        "Quantity": 1
        ///     }
        /// Input 3 request:
        ///
        ///     POST /Receipt
        ///     {
        ///        "ProductId": 8,
        ///        "Quantity": 1
        ///     },
        ///     {
        ///        "ProductId": 6,
        ///        "Quantity": 1
        ///     },
        ///     {
        ///        "ProductId": 7,
        ///        "Quantity": 1
        ///     },
        ///     {
        ///        "ProductId": 9,
        ///        "Quantity": 1
        ///     },
        ///     {
        ///        "ProductId": 9,
        ///        "Quantity": 1
        ///     }
        ///     
        /// </remarks>
        /// <param name="receiptRequest"></param>
        /// <returns>Returns receipt string.</returns>
        /// <response code="201">Newly created receipt</response>
        /// <response code="400">Bad Request when request is null, empty, productId  is less than zero or quantity is less than zero</response>
        /// <response code="500">Internal server error when an error occurs during processing request</response>

        [HttpPost, Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<string>> GetReceiptForCheckOutAsync(IEnumerable<ReceiptRequest> receiptRequest)
        {
            try
            {
                if (receiptRequest == null || !receiptRequest.Any())
                {
                    return BadRequest("There must be at least on product in order to generate receipt");
                }

                if (receiptRequest.Any(r => r.ProductId <= 0))
                {
                    return BadRequest("Product id needs to be greater than 0 in order to generate a receipt");
                }

                if (receiptRequest.Any(r => r.Quantity <= 0))
                {
                    return BadRequest("Product need quantity greater than 0 in order to generate a receipt");
                }

                return StatusCode(201, await _receiptEngine.GenerateReceiptAsync(receiptRequest).ConfigureAwait(false));
            }
            catch (Exception exception) 
            when (exception is SqlException || exception is ArgumentNullException || exception is InvalidOperationException 
            || exception is InvalidCastException || exception is ArgumentOutOfRangeException)
            {
                Logger.LogError(exception, "An error occurred while retrieving receipt.");
                return StatusCode(500, "Sorry an error occurred processing your request.");
            }
        }
    }
}
