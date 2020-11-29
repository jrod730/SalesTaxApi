using Contracts.Models;
using Domain.Interfaces.Engines;
using Domain.Interfaces.Storage.Repositories;
using Domain.Interfaces.Strategies;
using Domain.Models;
using Engines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests.Engines
{
    [TestClass]
    public class ReceiptEngineTests : TestBase<ReceiptEngine>
    {
        private Mock<IEnumerable<ITaxCalculationStrategy>> _taxCalculationStrategiesMock;
        private Mock<IProductRepository> _productRepositoryMock;
        private IReceiptEngine _receiptEngine;
        private Mock<ITaxCalculationStrategy> _noTaxCalculationStrategyMock;
        private Mock<ITaxCalculationStrategy> _basicTaxCalculationStratgeyMock;
        private Mock<ITaxCalculationStrategy> _importNoBasicTaxCalculationStrategyMock;
        private Mock<ITaxCalculationStrategy> _importTaxCalculationStrategyMock;

        [TestInitialize]
        public void Init()
        {
            TestBaseInit();
            _taxCalculationStrategiesMock = new Mock<IEnumerable<ITaxCalculationStrategy>>();
            _productRepositoryMock = new Mock<IProductRepository>();
            InitializeStrategies();

            _receiptEngine = new ReceiptEngine(LoggerMock.Object, _taxCalculationStrategiesMock.Object, _productRepositoryMock.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            TestBaseCleanup();
        }

        [TestMethod]
        public async Task ReceiptEngine_GenerateReceiptAsync_InputOne_StringReturnedAsExpected()
        {
            var productList = CreateCompleteProductList();
            var receiptRequest = CreateRequestForInputOne();
            var expectedResponse = CreateResponseStringForInputOne();

            SetupStrategyMockForInputOne(productList);

            _productRepositoryMock.Setup(x => x.GetAllProductsAsync()).ReturnsAsync(productList);

            var actualResponse = await _receiptEngine.GenerateReceiptAsync(receiptRequest).ConfigureAwait(false);

            Assert.AreEqual(expectedResponse, actualResponse, "Expected strings to be the same.");
        }

        [TestMethod]
        public async Task ReceiptEngine_GenerateReceiptAsync_InputTwo_StringReturnedAsExpected()
        {
            var productList = CreateCompleteProductList();
            var receiptRequest = CreateRequestForInputTwo();
            var expectedResponse = CreateResponseStringForInputTwo();

            SetupStrategyMockForInputTwo(productList);

            _productRepositoryMock.Setup(x => x.GetAllProductsAsync()).ReturnsAsync(productList);

            var actualResponse = await _receiptEngine.GenerateReceiptAsync(receiptRequest).ConfigureAwait(false);

            Assert.AreEqual(expectedResponse, actualResponse, "Expected strings to be the same.");
        }

        [TestMethod]
        public async Task ReceiptEngine_GenerateReceiptAsync_InputThree_StringReturnedAsExpected()
        {
            var productList = CreateCompleteProductList();
            var receiptRequest = CreateRequestForInputThree();
            var expectedResponse = CreateResponseStringForInputThree();

            SetupStrategyMockForInputThree(productList);

            _productRepositoryMock.Setup(x => x.GetAllProductsAsync()).ReturnsAsync(productList);

            var actualResponse = await _receiptEngine.GenerateReceiptAsync(receiptRequest).ConfigureAwait(false);

            Assert.AreEqual(expectedResponse, actualResponse, "Expected strings to be the same.");
        }

        #region General Setup

        private void InitializeStrategies()
        {
            _noTaxCalculationStrategyMock = new Mock<ITaxCalculationStrategy>();
            _basicTaxCalculationStratgeyMock = new Mock<ITaxCalculationStrategy>();
            _importTaxCalculationStrategyMock = new Mock<ITaxCalculationStrategy>();
            _importNoBasicTaxCalculationStrategyMock = new Mock<ITaxCalculationStrategy>();
        }

        private IEnumerable<Product> CreateCompleteProductList()
        {
            return new List<Product>
            {
                new Product
                {
                    ProductId = 1,
                    Name = "Book",
                    Price = 12.49m,
                    TaxTypeId = Domain.Enums.TaxType.None
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Music CD",
                    Price = 14.99m,
                    TaxTypeId = Domain.Enums.TaxType.Basic
                },
                new Product
                {
                    ProductId = 3,
                    Name = "Chocolate Bar",
                    Price = 0.85m,
                    TaxTypeId = Domain.Enums.TaxType.None
                },
                new Product
                {
                    ProductId = 4,
                    Name = "Imported box of chocolates",
                    Price = 10.00m,
                    TaxTypeId = Domain.Enums.TaxType.ImportNoBasic
                },
                new Product
                {
                    ProductId = 5,
                    Name = "Imported bottle of perfume",
                    Price = 47.50m,
                    TaxTypeId = Domain.Enums.TaxType.Import
                },
                new Product
                {
                    ProductId = 6,
                    Name = "Bottle of perfume",
                    Price = 18.99m,
                    TaxTypeId = Domain.Enums.TaxType.Basic
                },
                new Product
                {
                    ProductId = 7,
                    Name = "Packet of headache pills",
                    Price = 9.75m,
                    TaxTypeId = Domain.Enums.TaxType.None
                },
                new Product
                {
                    ProductId = 8,
                    Name = "Imported bottle of perfume",
                    Price = 27.99m,
                    TaxTypeId = Domain.Enums.TaxType.Import
                },
                new Product
                {
                    ProductId = 9,
                    Name = "Imported box of chocolates",
                    Price = 11.25m,
                    TaxTypeId = Domain.Enums.TaxType.ImportNoBasic
                }
            };
        }

        #endregion

        #region Setup For Input One

        private void SetupStrategyMockForInputOne(IEnumerable<Product> productList)
        {
            var bookPrice = productList.ElementAt(0).Price;
            var cdPrice = productList.ElementAt(1).Price;
            var chocolatePrice = productList.ElementAt(2).Price;
            var bookWithTax = 12.49m;
            var cdWithTax = 16.49m;
            var chocolatePriceWithTax = 0.85m;

            _noTaxCalculationStrategyMock.SetupGet(x => x.TaxType).Returns(Domain.Enums.TaxType.None);
            _noTaxCalculationStrategyMock.Setup(x => x.Calculate(bookWithTax)).Returns(bookWithTax);
            _noTaxCalculationStrategyMock.SetupGet(x => x.TaxType).Returns(Domain.Enums.TaxType.None);
            _noTaxCalculationStrategyMock.Setup(x => x.Calculate(bookWithTax)).Returns(bookWithTax);
            _basicTaxCalculationStratgeyMock.SetupGet(x => x.TaxType).Returns(Domain.Enums.TaxType.Basic);
            _basicTaxCalculationStratgeyMock.Setup(x => x.Calculate(cdPrice)).Returns(cdWithTax);
            _noTaxCalculationStrategyMock.SetupGet(x => x.TaxType).Returns(Domain.Enums.TaxType.None);
            _noTaxCalculationStrategyMock.Setup(x => x.Calculate(chocolatePrice)).Returns(chocolatePriceWithTax);

            var taxCalculationStrategyList = new List<ITaxCalculationStrategy>
            {
                _noTaxCalculationStrategyMock.Object,
                _noTaxCalculationStrategyMock.Object,
                _basicTaxCalculationStratgeyMock.Object,
                _noTaxCalculationStrategyMock.Object
            };

            _taxCalculationStrategiesMock.Setup(m => m.GetEnumerator())
                .Returns(() => taxCalculationStrategyList.GetEnumerator());
        }

        private IEnumerable<ReceiptRequest> CreateRequestForInputOne()
        {
            return new List<ReceiptRequest>
            {
                new ReceiptRequest
                {
                    ProductId = 1,
                    Quantity = 1
                },
                new ReceiptRequest
                {
                    ProductId = 1,
                    Quantity = 1
                },
                new ReceiptRequest
                {
                    ProductId = 2,
                    Quantity = 1
                },
                new ReceiptRequest
                {
                    ProductId = 3,
                    Quantity = 1
                }
            };
        }

        private string CreateResponseStringForInputOne()
        {
            return $"Book: 24.98 (2 @ 12.49) {Environment.NewLine}" +
                    $"Music CD: 16.49 {Environment.NewLine}" +
                    $"Chocolate Bar: 0.85 {Environment.NewLine}" +
                    $"Sales Taxes: 1.50 {Environment.NewLine}" +
                    $"Total: 42.32 {Environment.NewLine}";
        }

        #endregion

        #region Setup For Input Two

        private IEnumerable<ReceiptRequest> CreateRequestForInputTwo()
        {
            return new List<ReceiptRequest>
            {
                new ReceiptRequest
                {
                    ProductId = 4,
                    Quantity = 1
                },
                new ReceiptRequest
                {
                    ProductId = 5,
                    Quantity = 1
                }
            };
        }

        private string CreateResponseStringForInputTwo()
        {
            return $"Imported box of chocolates: 10.50 {Environment.NewLine}" +
                    $"Imported bottle of perfume: 54.65 {Environment.NewLine}" +
                    $"Sales Taxes: 7.65 {Environment.NewLine}" +
                    $"Total: 65.15 {Environment.NewLine}";
        }

        private void SetupStrategyMockForInputTwo(IEnumerable<Product> productList)
        {
            var importedChocolatesPrice = productList.ElementAt(3).Price;
            var importedPerfumePrice = productList.ElementAt(4).Price;
            var importedChocolatesWithTax = 10.50m;
            var importedPerfumeWithTax = 54.65m;

            _importNoBasicTaxCalculationStrategyMock.SetupGet(x => x.TaxType).Returns(Domain.Enums.TaxType.ImportNoBasic);
            _importNoBasicTaxCalculationStrategyMock.Setup(x => x.Calculate(importedChocolatesPrice)).Returns(importedChocolatesWithTax);
            _importTaxCalculationStrategyMock.SetupGet(x => x.TaxType).Returns(Domain.Enums.TaxType.Import);
            _importTaxCalculationStrategyMock.Setup(x => x.Calculate(importedPerfumePrice)).Returns(importedPerfumeWithTax);

            var taxCalculationStrategyList = new List<ITaxCalculationStrategy>
            {
                _importNoBasicTaxCalculationStrategyMock.Object,
                _importTaxCalculationStrategyMock.Object
            };

            _taxCalculationStrategiesMock.Setup(m => m.GetEnumerator())
                .Returns(() => taxCalculationStrategyList.GetEnumerator());
        }

        #endregion

        #region Setup For Input Three

        private IEnumerable<ReceiptRequest> CreateRequestForInputThree()
        {
            return new List<ReceiptRequest>
            {
                new ReceiptRequest
                {
                    ProductId = 8,
                    Quantity = 1
                },
                new ReceiptRequest
                {
                    ProductId = 6,
                    Quantity = 1
                },
                new ReceiptRequest
                {
                    ProductId = 7,
                    Quantity = 1
                },
                new ReceiptRequest
                {
                    ProductId = 9,
                    Quantity = 1
                },
                new ReceiptRequest
                {
                    ProductId = 9,
                    Quantity = 1
                }
            };
        }

        private string CreateResponseStringForInputThree()
        {
            return $"Imported bottle of perfume: 32.19 {Environment.NewLine}" +
                    $"Bottle of perfume: 20.89 {Environment.NewLine}" +
                    $"Packet of headache pills: 9.75 {Environment.NewLine}" +
                    $"Imported box of chocolates: 23.70 (2 @ 11.85) {Environment.NewLine}" +
                    $"Sales Taxes: 7.30 {Environment.NewLine}" +
                    $"Total: 86.53 {Environment.NewLine}";
        }

        private void SetupStrategyMockForInputThree(IEnumerable<Product> productList)
        {
            var importedPerfumePrice = productList.ElementAt(7).Price;
            var perfumePrice = productList.ElementAt(5).Price;
            var headachePillsPrice = productList.ElementAt(6).Price;
            var importedChocolatesPrice = productList.ElementAt(8).Price;
            var importedPerfumeWithTax = 32.19m;
            var perfumeWithTax = 20.89m;
            var headachePillsWithTax = 9.75m;
            var importedChocolatesWithTax = 11.85m;

            _importTaxCalculationStrategyMock.SetupGet(x => x.TaxType).Returns(Domain.Enums.TaxType.Import);
            _importTaxCalculationStrategyMock.Setup(x => x.Calculate(importedPerfumePrice)).Returns(importedPerfumeWithTax);
            _basicTaxCalculationStratgeyMock.SetupGet(x => x.TaxType).Returns(Domain.Enums.TaxType.Basic);
            _basicTaxCalculationStratgeyMock.Setup(x => x.Calculate(perfumePrice)).Returns(perfumeWithTax);
            _noTaxCalculationStrategyMock.SetupGet(x => x.TaxType).Returns(Domain.Enums.TaxType.None);
            _noTaxCalculationStrategyMock.Setup(x => x.Calculate(headachePillsPrice)).Returns(headachePillsWithTax);
            _importNoBasicTaxCalculationStrategyMock.SetupGet(x => x.TaxType).Returns(Domain.Enums.TaxType.ImportNoBasic);
            _importNoBasicTaxCalculationStrategyMock.Setup(x => x.Calculate(importedChocolatesPrice)).Returns(importedChocolatesWithTax);
            _importNoBasicTaxCalculationStrategyMock.SetupGet(x => x.TaxType).Returns(Domain.Enums.TaxType.ImportNoBasic);
            _importNoBasicTaxCalculationStrategyMock.Setup(x => x.Calculate(importedChocolatesPrice)).Returns(importedChocolatesWithTax);


            var taxCalculationStrategyList = new List<ITaxCalculationStrategy>
            {
                _importTaxCalculationStrategyMock.Object,
                _basicTaxCalculationStratgeyMock.Object,
                _noTaxCalculationStrategyMock.Object,
                _importNoBasicTaxCalculationStrategyMock.Object,
                _importNoBasicTaxCalculationStrategyMock.Object
            };

            _taxCalculationStrategiesMock.Setup(m => m.GetEnumerator())
                .Returns(() => taxCalculationStrategyList.GetEnumerator());
        }

        #endregion
    }
}




