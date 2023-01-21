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
    public class PersonRepository : IPersonRepository
    {
        private TradeMarketDbContext context;
        public PersonRepository(TradeMarketDbContext context)
        {
            this.context = context;
        }
        public Task AddAsync(Person entity)
        {
            context.Persons.Add(entity);
            context.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public void Delete(Person entity)
        {
            context.Persons.Remove(entity);
            context.SaveChangesAsync();
        }

        public Task DeleteByIdAsync(int id)
        {
            var person = context.Persons.Find(id);
            context.Remove(person);
            context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Person>> GetAllAsync()
        {
            return Task.FromResult(context.Persons.AsEnumerable<Person>());
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            return await context.Persons.FindAsync(id);
        }

        public void Update(Person entity)
        {
            context.Persons.Update(entity);
            context.SaveChangesAsync();
        }
    }
}
