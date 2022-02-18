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
    internal class ReceiptDetailRepositoryTests
    {
        [TestCase(1)]
        [TestCase(5)]
        public async Task ReceiptDetailRepository_GetById_ReturnsSingleValue(int id)
        {
            using var context = new TradeMarketDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var receiptDetailRepository = new ReceiptDetailRepository(context);
            var receiptDetail = await receiptDetailRepository.GetById(id);

            var expected = ExpectedReceiptsDetails.FirstOrDefault(x => x.Id == id);

            Assert.That(receiptDetail, Is.EqualTo(expected).Using(new ReceiptDetailEqualityComparer()), message: "GetById method works incorrect");
        }

        [Test]
        public async Task ReceiptDetailRepository_GetAll_ReturnsAllValues()
        {
            using var context = new TradeMarketDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var receiptDetailRepository = new ReceiptDetailRepository(context);
            var receiptDetails = await receiptDetailRepository.GetAll();

            Assert.That(receiptDetails, Is.EqualTo(ExpectedReceiptsDetails).Using(new ReceiptDetailEqualityComparer()), message: "GetAll method works incorrect");
        }

        [Test]
        public async Task ReceiptDetailRepository_Add_AddsValueToDatabase()
        {
            using var context = new TradeMarketDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var receiptDetailRepository = new ReceiptDetailRepository(context);
            var receiptDetail = new ReceiptDetail { Id = 6, ReceiptId = 2, ProductId = 1 };

            await receiptDetailRepository.Add(receiptDetail);
            await context.SaveChangesAsync();

            Assert.That(context.ReceiptsDetails.Count(), Is.EqualTo(6), message: "Add method works incorrect");
        }

        [Test]
        public async Task ReceiptDetailRepository_DeleteById_DeletesEntity()
        {
            using var context = new TradeMarketDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var receiptDetailRepository = new ReceiptDetailRepository(context);

            await receiptDetailRepository.DeleteById(1);
            await context.SaveChangesAsync();

            Assert.That(context.ReceiptsDetails.Count(), Is.EqualTo(4), message: "DeleteById works incorrect");
        }

        [Test]
        public async Task ReceiptDetailRepository_Update_UpdatesEntity()
        {
            using var context = new TradeMarketDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var receiptDetailRepository = new ReceiptDetailRepository(context);
            var receiptDetail = new ReceiptDetail
            {
                Id = 1,
                ReceiptId = 1,
                ProductId = 1,
                UnitPrice = 20,
                DiscountUnitPrice = 16,
                Quantity = 5
            };

            receiptDetailRepository.Update(receiptDetail);
            await context.SaveChangesAsync();

            Assert.That(receiptDetail, Is.EqualTo(new ReceiptDetail
            {
                Id = 1,
                ReceiptId = 1,
                ProductId = 1,
                UnitPrice = 20,
                DiscountUnitPrice = 16,
                Quantity = 5
            }).Using(new ReceiptDetailEqualityComparer()), message: "Update method works incorrect");
        }

        [Test]
        public async Task ReceiptDetailRepository_GetAllWithDetails_ReturnsWithIncludedEntities()
        {
            using var context = new TradeMarketDbContext(UnitTestHelper.GetUnitTestDbOptions());

            var receiptDetailRepository = new ReceiptDetailRepository(context);

            var receiptDetails = await receiptDetailRepository.GetAllWithDetails();
            var receiptDetail = receiptDetails.FirstOrDefault(x => x.Id == 1);

            Assert.That(receiptDetails, Is.EqualTo(ExpectedReceiptsDetails).Using(new ReceiptDetailEqualityComparer()), message: "GetAllWithDetails method works incorrect");
            Assert.That(receiptDetail.Product, Is.Not.Null, message: "GetByIdWithDetails method doesnt't return included entities");
            Assert.That(receiptDetail.Receipt, Is.Not.Null, message: "GetByIdWithDetails method doesnt't return included entities");
            Assert.That(receiptDetail.Product.Category, Is.Not.Null, message: "GetByIdWithDetails method doesnt't return included entities");
        }

        private static IEnumerable<ReceiptDetail> ExpectedReceiptsDetails =>
            new[]
            {
                new ReceiptDetail { Id = 1, ReceiptId = 1, ProductId = 1, UnitPrice = 20, DiscountUnitPrice = 16, Quantity = 3 },
                new ReceiptDetail { Id = 2, ReceiptId = 1, ProductId = 2, UnitPrice = 50, DiscountUnitPrice = 40, Quantity = 1 },
                new ReceiptDetail { Id = 3, ReceiptId = 2, ProductId = 2, UnitPrice = 50, DiscountUnitPrice = 40, Quantity = 2 },
                new ReceiptDetail { Id = 4, ReceiptId = 3, ProductId = 1, UnitPrice = 20, DiscountUnitPrice = 18, Quantity = 2 },
                new ReceiptDetail { Id = 5, ReceiptId = 3, ProductId = 2, UnitPrice = 50, DiscountUnitPrice = 45, Quantity = 5 }
            };
    }
}
