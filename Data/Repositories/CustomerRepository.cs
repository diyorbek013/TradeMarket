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
    public class CustomerRepository : ICustomerRepository
    {
        private TradeMarketDbContext context;

        public CustomerRepository(TradeMarketDbContext context)
        {
            this.context = context;
        }
        public Task AddAsync(Customer entity)
        {
            context.AddAsync(entity);
            context.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public void Delete(Customer entity)
        {
            context.Remove(entity); 
            context.SaveChangesAsync();
        }

        public Task DeleteByIdAsync(int id)
        {
            var customer = context.Customers.FirstOrDefault(c => c.Id == id);
            context.Remove(customer);
            context.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Customer>> GetAllAsync()
        {
            return Task.FromResult(context.Customers.AsEnumerable<Customer>());
            
        }

        public Task<IEnumerable<Customer>> GetAllWithDetailsAsync()
        {
            return Task.FromResult(context.Customers
                .AsNoTracking().Include(x => x.Person)
                .Include(x => x.Receipts)
                .ThenInclude(x => x.ReceiptDetails).AsEnumerable());
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await context.Customers.FindAsync(id);
        }

        public async Task<Customer> GetByIdWithDetailsAsync(int id)
        {
            return await context.Customers.AsNoTracking()
                .Include(x => x.Person)
                .Include(x => x.Receipts)
                .ThenInclude(x => x.ReceiptDetails)
                .FirstOrDefaultAsync(x => x.Id == id); ;
        }

        public void Update(Customer entity)
        {
            context.Customers.Update(entity);
            context.SaveChangesAsync();
        }
    }
}
