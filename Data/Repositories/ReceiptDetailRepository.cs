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
    public class ReceiptDetailRepository : IReceiptDetailRepository
    {
        private TradeMarketDbContext context;

        public ReceiptDetailRepository(TradeMarketDbContext context)
        {
            this.context = context;
        }
        public Task AddAsync(ReceiptDetail entity)
        {
            context.ReceiptsDetails.Add(entity);
            context.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public void Delete(ReceiptDetail entity)
        {
            context.ReceiptsDetails.Remove(entity);
            context.SaveChangesAsync();
        }

        public Task DeleteByIdAsync(int id)
        {
            var result = context.ReceiptsDetails.Find(id);
            context.ReceiptsDetails.Remove(result);
            context.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public Task<IEnumerable<ReceiptDetail>> GetAllAsync()
        {
            return Task.FromResult(context.ReceiptsDetails.AsEnumerable<ReceiptDetail>());
        }

        public async Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
        {
            return await context.ReceiptsDetails
                .AsNoTracking()
                .Include(x => x.Product)
                .Include(x => x.Receipt)
                .Include(x => x.Product.Category)
                .ToListAsync();

        }

        public Task<ReceiptDetail> GetByIdAsync(int id)
        {
            var result = context.ReceiptsDetails.Find(id);
            return Task.FromResult(result);
        }

        public void Update(ReceiptDetail entity)
        {
            context.ReceiptsDetails.Update(entity);
            context.SaveChangesAsync();
        }
    }
}
