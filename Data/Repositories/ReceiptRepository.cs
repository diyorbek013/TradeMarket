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
    public class ReceiptRepository : IReceiptRepository
    {
        private TradeMarketDbContext context;

        public ReceiptRepository(TradeMarketDbContext context)
        {
            this.context = context;
        }
        public Task AddAsync(Receipt entity)
        {
            context.Receipts.Add(entity);
            context.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public void Delete(Receipt entity)
        {
            context.Receipts.Remove(entity);
            context.SaveChangesAsync();
        }

        public Task DeleteByIdAsync(int id)
        {
            var result = context.Receipts.Find(id);
            context.Receipts.Remove(result);
            context.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Receipt>> GetAllAsync()
        {
            return Task.FromResult(context.Receipts.AsEnumerable<Receipt>());
        }

        public async Task<IEnumerable<Receipt>> GetAllWithDetailsAsync()
        {
            return await context.Receipts
               .Include(x => x.Customer)
               .Include(x => x.ReceiptDetails)
               .ThenInclude(x => x.Product)
               .ThenInclude(x => x.Category).ToListAsync();
        }

        public Task<Receipt> GetByIdAsync(int id)
        {
            return Task.FromResult(context.Receipts.Find(id));
        }

        public async Task<Receipt> GetByIdWithDetailsAsync(int id)
        {
            return await context.Receipts.AsNoTracking()
               .Include(x => x.Customer)
               .Include(x => x.ReceiptDetails)
               .ThenInclude(x => x.Product)
               .ThenInclude(x => x.Category)
               .FirstOrDefaultAsync(x => x.Id == id);

        }

        public void Update(Receipt entity)
        {
            context.Receipts.Update(entity);
            context.SaveChangesAsync();
        }
    }
}
