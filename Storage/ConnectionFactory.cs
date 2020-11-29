using Domain;
using Domain.Interfaces.Storage;
using Helpers;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using System;
using System.Data;

namespace Storage
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly ILogger<ConnectionFactory> _logger;

        public ConnectionFactory(ILogger<ConnectionFactory> logger)
        {
            _logger = logger.ThrowIfNull(nameof(logger));
        }

        public IDbConnection GetDbConnection()
        {
            _logger.LogInformation("Establishing Db Connection.");
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = Environment.CurrentDirectory + "\\SalesTaxDb.db";
            return new SqliteConnection(connectionStringBuilder.ConnectionString);
        }
    }
}
