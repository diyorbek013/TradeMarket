using Data.Data;
using Data.Entities;
using Data.Repositories;
using Library.Tests;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeMarket.Tests.DataTests
{
    [TestFixture]
    public class ProductRepositoryTests
    {
        [TestCase(1)]
        [TestCase(2)]
        public async Task ProductRepository_GetById_ReturnsSingleValue(int id)
        {
            using var context = new TradeMarketDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var productRepository = new ProductRepository(context);

            var product = await productRepository.GetById(id);

            var expected = ExpectedProducts.FirstOrDefault(x => x.Id == id);

            Assert.That(product, Is.EqualTo(expected).Using(new ProductEqualityComparer()), message: "GetById method works incorrect");
        }

        [Test]
        public async Task ProductRepository_GetAll_ReturnsAllValues()
        {
            using var context = new TradeMarketDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var productRepository = new ProductRepository(context);

            var products = await productRepository.GetAll();

            Assert.That(products, Is.EqualTo(ExpectedProducts).Using(new ProductEqualityComparer()), message: "GetAll method works incorrect");
        }

        [Test]
        public async Task ProductRepository_Add_AddsValueToDatabase()
        {
            using var context = new TradeMarketDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var productRepository = new ProductRepository(context);
            var product = new Product { Id = 3 };

            await productRepository.Add(product);
            await context.SaveChangesAsync();

            Assert.That(context.Products.Count(), Is.EqualTo(3), message: "Add method works incorrect");
        }

        [Test]
        public async Task ProductRepository_DeleteById_DeletesEntity()
        {
            using var context = new TradeMarketDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var productRepository = new ProductRepository(context);

            await productRepository.DeleteById(1);
            await context.SaveChangesAsync();

            Assert.That(context.Products.Count(), Is.EqualTo(1), message: "DeleteById works incorrect");
        }

        [Test]
        public async Task ProductRepository_Update_UpdatesEntity()
        {
            using var context = new TradeMarketDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var productRepository = new ProductRepository(context);
            var product = new Product
            {
                Id = 1,
                ProductCategoryId = 1,
                ProductName = "Yogurt",
                Price = 30
            };

            productRepository.Update(product);
            await context.SaveChangesAsync();

            Assert.That(product, Is.EqualTo(new Product
            {
                Id = 1,
                ProductCategoryId = 1,
                ProductName = "Yogurt",
                Price = 30
            }).Using(new ProductEqualityComparer()), message: "Update method works incorrect");
        }

        [Test]
        public async Task ProductRepository_GetByIdWithDetails_ReturnsWithIncludedEntities()
        {
            using var context = new TradeMarketDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var productRepository = new ProductRepository(context);

            var product = await productRepository.GetByIdWithDetails(1);

            var expected = ExpectedProducts.FirstOrDefault(x => x.Id == 1);
            var expectedReceiptDetailsCount = 2;

            Assert.That(product, Is.EqualTo(expected).Using(new ProductEqualityComparer()), message: "GetByIdWithDetails method works incorrect");
            Assert.That(product.ReceiptDetails.Count, Is.EqualTo(expectedReceiptDetailsCount), message: "GetByIdWithDetails method doesnt't return included entities");
            Assert.That(product.Category, Is.Not.Null, message: "GetByIdWithDetails method doesnt't return included entities");
        }

        [Test]
        public async Task ProductRepository_GetAllWithDetails_ReturnsWithIncludedEntities()
        {
            using var context = new TradeMarketDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var productRepository = new ProductRepository(context);

            var products = await productRepository.GetAllWithDetails();
            var product = products.FirstOrDefault(x => x.Id == 1);

            var expectedReceiptDetailsCount = 2;

            Assert.That(products, Is.EqualTo(ExpectedProducts).Using(new ProductEqualityComparer()), message: "GetAllWithDetails method works incorrect");
            Assert.That(product.ReceiptDetails.Count, Is.EqualTo(expectedReceiptDetailsCount), message: "GetAllWithDetails method doesnt't return included entities");
            Assert.That(product.Category, Is.Not.Null, message: "GetByIdWithDetails method doesnt't return included entities");
        }

        private static IEnumerable<Product> ExpectedProducts =>
            new[]
            {
                new Product { Id = 1, ProductCategoryId = 1, ProductName = "Name1", Price = 20 },
                new Product { Id = 2, ProductCategoryId = 2, ProductName = "Name2", Price = 50 }
            };
    }
}
