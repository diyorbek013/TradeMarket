using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private TradeMarketDbContext context;

        public ProductRepository(TradeMarketDbContext context)
        {
            this.context = context;
        }
        public Task AddAsync(Product entity)
        {
            context.Products.Add(entity);
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public void Delete(Product entity)
        {
            context.Products.Remove(entity);
            context.SaveChanges();
        }

        public Task DeleteByIdAsync(int id)
        {
            var product = context.Products.Find(id);
            context.Remove(product);
            context.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return Task.FromResult(context.Products.AsEnumerable<Product>());
        }

        public Task<IEnumerable<Product>> GetAllWithDetailsAsync()
        {
            return Task.FromResult(context.Products
                .Include(customer => customer.Category)
                .Include(customer => customer.ReceiptDetails).AsEnumerable());
        }

        public Task<Product> GetByIdAsync(int id)
        {
            var result = context.Products.Find(id);
            return Task.FromResult(result);
        }

        public Task<Product> GetByIdWithDetailsAsync(int id)
        {
            return Task.FromResult(context.Products
                .Where(c => c.Id == id).AsQueryable()
                .Include(c => c.Category)
                .Include(c => c.ReceiptDetails).FirstOrDefault());
        }

        public void Update(Product entity)
        {
            context.Products.Update(entity);
            context.SaveChangesAsync();
        }
    }
}
