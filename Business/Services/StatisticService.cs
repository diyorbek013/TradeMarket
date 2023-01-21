using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class StatisticService : IStatisticService
    {
        private IUnitOfWork @object;
        private IMapper mapper;

        public StatisticService(IUnitOfWork @object, IMapper mapper)
        {
            this.@object = @object;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ProductModel>> GetCustomersMostPopularProductsAsync(int productCount, int customerId)
        {
            Dictionary<Product, int> topProducts = new Dictionary<Product, int>();

            var receipts = (await @object.ReceiptRepository.GetAllWithDetailsAsync())
                .Where(r => r.CustomerId == customerId);


            foreach (var receipt in receipts)
            {
                foreach(var receiptDetail in receipt.ReceiptDetails)
                {
                    if (topProducts.ContainsKey(receiptDetail.Product))
                    {
                        topProducts[receiptDetail.Product] += receiptDetail.Quantity;
                    }
                    else
                    {
                        var resultProduct = receiptDetail.Product;
                        ICollection<ReceiptDetail> productReceiptDetails = receipt.ReceiptDetails
                            .Where(rd => rd.ProductId == resultProduct.Id).ToList();
                        
                        resultProduct.ReceiptDetails = productReceiptDetails;

                        topProducts.Add(resultProduct, receiptDetail.Quantity);
                    }
                }
            }

            var products = topProducts.OrderByDescending(p => p.Value).Select(p => p.Key).Take(productCount);

            var result = mapper.Map<IEnumerable<Product>, IEnumerable<ProductModel>>(products);

            return mapper.Map<IEnumerable<Product>, IEnumerable<ProductModel>>(products);
        }

        public async Task<decimal> GetIncomeOfCategoryInPeriod(int categoryId, DateTime startDate, DateTime endDate)
        {
            var receipts = (await @object.ReceiptRepository.GetAllWithDetailsAsync())
                .Where(r => r.OperationDate >= startDate && r.OperationDate <= endDate);

            decimal income = 0;
            foreach(var receipt in receipts)
            {
                foreach (var receiptsDetail in receipt.ReceiptDetails
                    .Where(r => r.Product.ProductCategoryId == categoryId))
                {
                    income += receiptsDetail.DiscountUnitPrice * receiptsDetail.Quantity;
                }
            }
            return income;
        }

        public async Task<IEnumerable<ProductModel>> GetMostPopularProductsAsync(int productCount)
        {
            Dictionary<Product, int> popularProducts = new Dictionary<Product, int>();

            var receiptDetails = await @object.ReceiptDetailRepository.GetAllWithDetailsAsync();

            foreach (var receiptDetail in receiptDetails)
            {
                if (popularProducts.ContainsKey(receiptDetail.Product))
                    popularProducts[receiptDetail.Product] += receiptDetail.Quantity;
                else
                    popularProducts.Add(receiptDetail.Product, receiptDetail.Quantity);
            }

            var products = popularProducts.OrderByDescending(x => x.Value).Select(p => p.Key).Take(productCount);

            return mapper.Map<IEnumerable<Product>, IEnumerable<ProductModel>>(products);

        }

        public async Task<IEnumerable<CustomerActivityModel>> GetMostValuableCustomersAsync(int customerCount, DateTime startDate, DateTime endDate)
        {
            Dictionary<Customer, decimal> topCustomers = new Dictionary<Customer, decimal>();

            var receipts = (await @object.ReceiptRepository.GetAllWithDetailsAsync())
                .Where(x => x.OperationDate >= startDate && x.OperationDate <= endDate);

            foreach (var receipt in receipts)
            {
                decimal receiptSum = 0;

                foreach (var receiptDetail in receipt.ReceiptDetails)
                {
                    receiptSum += receiptDetail.DiscountUnitPrice * receiptDetail.Quantity;
                }

                if (topCustomers.ContainsKey(receipt.Customer))
                    topCustomers[receipt.Customer] += receiptSum;
                else
                    topCustomers.Add(receipt.Customer, receiptSum);
            }

            var customers = topCustomers.OrderByDescending(x => x.Value).Take(customerCount);
            
            List<CustomerActivityModel> customerActivityModels = new List<CustomerActivityModel>();
            foreach (var customer in customers)
            {
                CustomerActivityModel customerActivityModel = new CustomerActivityModel()
                {
                    CustomerId = customer.Key.Id,
                    CustomerName = customer.Key.Person.Name + " " + customer.Key.Person.Surname,
                    ReceiptSum = customer.Value
                };
                customerActivityModels.Add(customerActivityModel);
            }

            return customerActivityModels;
        }
    }
}
