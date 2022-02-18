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
    public class ProductCategoryRepositoryTests
    {
        [TestCase(1)]
        [TestCase(2)]
        public async Task ProductCategoryRepository_GetById_ReturnsSingleValue(int id)
        {
            using var context = new TradeMarketDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var productCategoryRepository = new ProductCategoryRepository(context);
            var productCategory = await productCategoryRepository.GetById(id);

            var expected = ExpectedProductCategories.FirstOrDefault(x => x.Id == id);

            Assert.That(productCategory, Is.EqualTo(expected).Using(new ProductCategoryEqualityComparer()), message: "GetById method works incorrect");
        }

        [Test]
        public async Task ProductCategoryRepository_GetAll_ReturnsAllValues()
        {
            using var context = new TradeMarketDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var productCategoryRepository = new ProductCategoryRepository(context);
            var productCategories = await productCategoryRepository.GetAll();

            Assert.That(productCategories, Is.EqualTo(ExpectedProductCategories).Using(new ProductCategoryEqualityComparer()), message: "GetAll method works incorrect");
        }

        [Test]
        public async Task ProductCategoryRepository_Add_AddsValueToDatabase()
        {
            using var context = new TradeMarketDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var productCategoryRepository = new ProductCategoryRepository(context);
            var productCategory = new ProductCategory { Id = 3 };

            await productCategoryRepository.Add(productCategory);
            await context.SaveChangesAsync();

            Assert.That(context.ProductCategories.Count(), Is.EqualTo(3), message: "Add method works incorrect");
        }

        [Test]
        public async Task ProductCategoryRepository_DeleteById_DeletesEntity()
        {
            using var context = new TradeMarketDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var productCategoryRepository = new ProductCategoryRepository(context);

            await productCategoryRepository.DeleteById(1);
            await context.SaveChangesAsync();

            Assert.That(context.ProductCategories.Count(), Is.EqualTo(1), message: "DeleteById works incorrect");
        }

        [Test]
        public async Task ProductCategoryRepository_Update_UpdatesEntity()
        {
            using var context = new TradeMarketDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var productCategoryRepository = new ProductCategoryRepository(context);
            var productCategory = new ProductCategory
            {
                Id = 1,
                CategoryName = "Dairy food"
            };

            productCategoryRepository.Update(productCategory);
            await context.SaveChangesAsync();

            Assert.That(productCategory, Is.EqualTo(new ProductCategory
            {
                Id = 1,
                CategoryName = "Dairy food"
            }).Using(new ProductCategoryEqualityComparer()), message: "Update method works incorrect");
        }

        private static IEnumerable<ProductCategory> ExpectedProductCategories =>
            new[]
            {
                new ProductCategory { Id = 1, CategoryName = "Category1" },
                new ProductCategory { Id = 2, CategoryName = "Category2" }
            };
    }
}
