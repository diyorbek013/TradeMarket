using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CustomerService : ICustomerService
    {
        private IUnitOfWork @object;
        private IMapper mapper;

        public CustomerService(IUnitOfWork @object, IMapper mapper)
        {
            this.@object = @object;
            this.mapper = mapper;
        } 

        public Task AddAsync(CustomerModel model)
        {
            Validation(model);
            var customer = mapper.Map<CustomerModel, Customer>(model);
            @object.CustomerRepository.AddAsync(customer);
            @object.SaveAsync();
            return Task.CompletedTask;

        }

        public async Task DeleteAsync(int modelId)
        {
            await @object.CustomerRepository.DeleteByIdAsync(modelId);
            await @object.SaveAsync();
        }

        public async Task<IEnumerable<CustomerModel>> GetAllAsync()
        {
            var customer = await @object.CustomerRepository.GetAllWithDetailsAsync();
            return mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerModel>>(customer);
        }

        public async Task<CustomerModel> GetByIdAsync(int id)
        {
            var customer = await @object.CustomerRepository.GetByIdWithDetailsAsync(id);
            return mapper.Map<Customer, CustomerModel>(customer);
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
        {
            var customer = (await @object.CustomerRepository.GetAllWithDetailsAsync())
                .Where(r => r.Receipts
                .Any(r => r.ReceiptDetails
                .Any(r => r.ProductId == productId)));

            return mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerModel>>(customer);
        }

        public Task UpdateAsync(CustomerModel model)
        {
            Validation(model);
            @object.CustomerRepository.Update(mapper.Map<CustomerModel, Customer>(model));
            @object.SaveAsync();
            return Task.CompletedTask;
        }
        private void Validation(CustomerModel model)
        {
            if (model == null)
                throw new MarketException("Model is null");
            else if (model.Name == null || model.Name == "")
                throw new MarketException("Name is Empty");
            else if (model.Surname == null || model.Surname == "")
                throw new MarketException("Surname is Empty");
            else if (model.BirthDate >= DateTime.Now || model.BirthDate < new DateTime(1900, 1, 1))
                throw new MarketException("Invalid Birthdate");
            else if (model.DiscountValue < 0 || model.DiscountValue > 100)
                throw new MarketException("Invalid DiscountValue");

        }
    }
}
