using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Data.Data
{
    //TODO: create class UnitOfWork
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TradeMarketDbContext context;
        private ICustomerRepository _customerRepository;
        private IPersonRepository _personRepository;
        private IProductRepository _productRepository;
        private IProductCategoryRepository _productCategoryRepository;
        private IReceiptRepository _receiptRepository;
        private IReceiptDetailRepository _receiptDetailRepository;
        public UnitOfWork(TradeMarketDbContext context)
        {
            this.context = context;
        }
        public ICustomerRepository CustomerRepository
        {
            get
            {
                return _customerRepository ??= new CustomerRepository(context);
            }
        }

        public IPersonRepository PersonRepository
        {
            get
            {
                return _personRepository ??= new PersonRepository(context);
            }
        }

        public IProductRepository ProductRepository
        {
            get
            {
                return _productRepository ??= new ProductRepository(context);
            }
        }

        public IProductCategoryRepository ProductCategoryRepository
        {
            get
            {
                return _productCategoryRepository ??= new ProductCategoryRepository(context);
            }
        }

        public IReceiptRepository ReceiptRepository
        {
            get
            {
                return _receiptRepository ??= new ReceiptRepository(context);
            }
        }

        public IReceiptDetailRepository ReceiptDetailRepository
        {
            get
            {
                return _receiptDetailRepository ??= new ReceiptDetailRepository(context);
            }
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
