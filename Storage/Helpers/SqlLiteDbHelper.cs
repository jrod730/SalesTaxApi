using Domain.Interfaces.Storage;
using Domain.Interfaces.Storage.Helpers;
using Helpers;
using Microsoft.Extensions.Logging;
using System;

namespace Storage.Helpers
{
    public class SqlLiteDbHelper : ISqlLiteDbHelper
	{
		private readonly ILogger<SqlLiteDbHelper> _logger;
		private readonly IConnectionFactory _connectionFactory;

		public SqlLiteDbHelper(ILogger<SqlLiteDbHelper> logger, IConnectionFactory connectionFactory)
		{
			_logger = logger.ThrowIfNull(nameof(logger));
			_connectionFactory = connectionFactory.ThrowIfNull(nameof(connectionFactory));
		}

		public void SetupDb()
        {
			try
			{
				using (var connection = _connectionFactory.GetDbConnection())
				{
					connection.Open();

					var setupCommands = connection.CreateCommand();

					_logger.LogInformation("Dropping existing tables");
					setupCommands.CommandText = "DROP TABLE IF EXISTS [Product] ";
					setupCommands.ExecuteNonQuery();
					setupCommands.CommandText = "DROP TABLE IF EXISTS [TaxType] ";
					setupCommands.ExecuteNonQuery();

					_logger.LogInformation("Creating new tables");
					setupCommands.CommandText = CreateTaxTypeTableProc();
					setupCommands.ExecuteNonQuery();
					setupCommands.CommandText = CreateProductTableProc();
					setupCommands.ExecuteNonQuery();

					_logger.LogInformation("Seeding db");
					using (var transaction = connection.BeginTransaction())
					{
						setupCommands.Transaction = transaction;
						setupCommands.CommandText = SeedTaxTypeTableProc();
						setupCommands.ExecuteNonQuery();
						setupCommands.CommandText = SeedProductTableProc();
						setupCommands.ExecuteNonQuery();

						transaction.Commit();
					}
				}
			}
			catch (Exception exception) when (exception is InvalidOperationException)
			{
				_logger.LogError(exception, "An error occurred while setting up the application db");
				throw;
			}
        }

        #region Create Tables Stored Procedures

        private static string CreateProductTableProc()
        {
			return
				"CREATE TABLE [Product] " +
				"( " +
					"[ProductId] INTEGER PRIMARY KEY AUTOINCREMENT, " +
					"[Name] VARCHAR(50) NOT NULL, " +
					"[Price] DECIMAL(10,2) NOT NULL, " +
					"[TaxTypeId] INTEGER NOT NULL, FOREIGN KEY (TaxTypeId) REFERENCES [TaxType] (TaxTypeId) " +
				");";
		}

		private static string CreateTaxTypeTableProc()
		{
			return
				"CREATE TABLE [TaxType] " +
				"( " +
					"[TaxTypeId] INTEGER PRIMARY KEY AUTOINCREMENT, " +
					"[Name] VARCHAR(25) NOT NULL " +
				");";
		}

        #endregion

        #region Seed Db Table Methods

        private static string SeedProductTableProc()
		{
			return
				"INSERT INTO [Product] (Name, Price, TaxTypeId) VALUES('Book', 12.49, 0); " +
				"INSERT INTO [Product] (Name, Price, TaxTypeId) VALUES('Music CD', 14.99, 1); " +
				"INSERT INTO [Product] (Name, Price, TaxTypeId) VALUES('Chocolate Bar', 0.85, 0); " +
				"INSERT INTO [Product] (Name, Price, TaxTypeId) VALUES('Imported box of chocolates', 10.00, 3); " +
				"INSERT INTO [Product] (Name, Price, TaxTypeId) VALUES('Imported bottle of perfume', 47.50, 2); " +
				"INSERT INTO [Product] (Name, Price, TaxTypeId) VALUES('Bottle of perfume', 18.99, 1); " +
				"INSERT INTO [Product] (Name, Price, TaxTypeId) VALUES('Packet of headache pills', 9.75, 0); " +
				"INSERT INTO [Product] (Name, Price, TaxTypeId) VALUES('Imported bottle of perfume', 27.99, 2); " +
				"INSERT INTO [Product] (Name, Price, TaxTypeId) VALUES('Imported box of chocolates', 11.25, 3);";
		}

		private static string SeedTaxTypeTableProc()
		{ 
			return
				"INSERT INTO  [TaxType] VALUES (0, 'None'); " +
				"INSERT INTO  [TaxType] VALUES (1, 'Basic'); " +
				"INSERT INTO  [TaxType] VALUES (2, 'Import'); " +
				"INSERT INTO  [TaxType] VALUES (3, 'ImportNoBasic');";
		}

		#endregion
	}
}
