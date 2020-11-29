using Dapper;
using Domain.Interfaces.Storage;
using Domain.Interfaces.Storage.Repositories;
using Domain.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Storage.Repositories
{
    public class ProductRepository : RepositoryBase<ProductRepository>, IProductRepository
    {
        public ProductRepository(ILogger<ProductRepository> logger, IConnectionFactory connectionFactory) 
            : base (logger, connectionFactory)
        { 
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            Logger.LogInformation("Retrieving all products");

            using (var connection = ConnectionFactory.GetDbConnection())
            {
                //CAST is due to Sqllite's handling of datatypes values with decimal values of .00 will be returned as INT
                var cmdText = "SELECT ProductId, Name, CAST(Price AS DOUBLE) AS Price, TaxTypeId " +
                    "FROM Product ";

                try
                {
                    var products = await connection.QueryAsync<Product>(cmdText).ConfigureAwait(false);
                    Logger.LogInformation("All Product successfully retrieved!");

                    return products;
                }
                catch (SqlException sqlException)
                {
                    Logger.LogError(sqlException, "Error occurred retrieving all products");
                    throw;
                }
            }
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            Logger.LogInformation("Retrieving product - ProductId: {ProductId}", productId);

            using (var connection = ConnectionFactory.GetDbConnection())
            {
                var parameters = new { ProductId = productId};

                //CAST is due to Sqllite's handling of datatypes values with decimal values of .00 will be returned as INT
                var cmdText = "SELECT ProductId, Name, CAST(Price AS DOUBLE) AS Price, TaxTypeId " +
                    "FROM Product " + 
                    "WHERE ProductId = @ProductId";

                try
                {
                    var product = await connection.QuerySingleAsync<Product>(cmdText, parameters).ConfigureAwait(false);
                    Logger.LogInformation("Product successfully retrieved!");

                    return product;
                }
                catch (SqlException sqlException)
                {
                    Logger.LogError(sqlException, "Error occurred retrieving product - ProductId: {ProductId}", productId);
                    throw;
                }
            }
        }

        public async Task<int> InsertProductAsync(Product product)
        {
            Logger.LogInformation("Inserting Product");

            using (var connection = ConnectionFactory.GetDbConnection())
            {
                var parameters = new
                {
                    Name = product.Name,
                    Price = product.Price,
                    TaxTypeId = product.TaxTypeId
                };

                var cmdText = "INSERT INTO [Product] (Name, Price, TaxTypeId) VALUES (@Name, @Price, @TaxTypeId); " +
                    "SELECT last_insert_rowid(); ";

                try
                {
                    var productId = await connection.ExecuteScalarAsync<int>(cmdText, parameters).ConfigureAwait(false);
                    Logger.LogInformation("New product successfully created - ProductId: {ProductId}", productId);

                    return productId;
                }
                catch (SqlException sqlException)
                {
                    Logger.LogError(sqlException, "Error occurred inserting product");
                    throw;
                }
            }
        }
    }
}
