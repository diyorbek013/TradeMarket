using Data.Data;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private TradeMarketDbContext context;

        public ProductCategoryRepository(TradeMarketDbContext context)
        {
            this.context = context;
        }
        public Task AddAsync(ProductCategory entity)
        {
            context.ProductCategories.Add(entity);
            context.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public void Delete(ProductCategory entity)
        {
            context.ProductCategories.Remove(entity);
            context.SaveChangesAsync();
        }

        public Task DeleteByIdAsync(int id)
        {
            var p = context.ProductCategories.Find(id);
            context.ProductCategories.Remove(p);
            context.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public Task<IEnumerable<ProductCategory>> GetAllAsync()
        {
            return Task.FromResult(context.ProductCategories.AsEnumerable<ProductCategory>());
        }

        public async Task<ProductCategory> GetByIdAsync(int id)
        {
            return await context.ProductCategories.FindAsync(id);
        }

        public void Update(ProductCategory entity)
        {
            context.ProductCategories.Update(entity);
            context.SaveChangesAsync();
        }
    }
}
