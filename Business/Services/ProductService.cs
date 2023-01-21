using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ProductService : IProductService
    {
        private IUnitOfWork @object;
        private IMapper mapper;
        public ProductService(IUnitOfWork @object, IMapper mapper)
        {
            this.@object = @object;
            this.mapper = mapper;
        }
        public Task AddAsync(ProductModel model)
        {
            Validation(model);

            @object.ProductRepository.AddAsync(mapper.Map<ProductModel, Product>(model));
            @object.SaveAsync();
            return Task.CompletedTask;
        }

        public Task AddCategoryAsync(ProductCategoryModel categoryModel)
        {
            Validation(categoryModel);

            @object.ProductCategoryRepository.AddAsync(
                mapper.Map<ProductCategoryModel, ProductCategory>(categoryModel));
            @object.SaveAsync();
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int modelId)
        {
            @object.ProductRepository.DeleteByIdAsync(modelId);
            @object.SaveAsync();
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            var product = await @object.ProductRepository.GetAllWithDetailsAsync();
            return mapper.Map<IEnumerable<Product>, IEnumerable<ProductModel>>(product);
        }

        public async Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync()
        {
            var productCategories = await @object.ProductCategoryRepository.GetAllAsync();
            return mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryModel>>(productCategories);
        }

        public async Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch)
        {
            var product = await @object.ProductRepository.GetAllWithDetailsAsync();

            if (filterSearch.CategoryId != null)
                product = product.Where(p => p.ProductCategoryId == filterSearch.CategoryId);

            if (filterSearch.MinPrice != null)
                product = product.Where(p => p.Price >= filterSearch.MinPrice);

            if (filterSearch.MaxPrice != null)
                product = product.Where(p => p.Price <= filterSearch.MaxPrice);

            return mapper.Map<IEnumerable<Product>, IEnumerable<ProductModel>>(product);
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            var product = await @object.ProductRepository.GetByIdWithDetailsAsync(id);
            return mapper.Map<Product, ProductModel>(product);
        }

        public Task RemoveCategoryAsync(int categoryId)
        {
            @object.ProductCategoryRepository.DeleteByIdAsync(categoryId);
            @object.SaveAsync();
            return Task.CompletedTask;
        }

        public Task UpdateAsync(ProductModel model)
        {
            Validation(model);

            @object.ProductRepository.Update(mapper.Map<ProductModel, Product>(model));
            @object.SaveAsync();
            return Task.CompletedTask;
        }

        public Task UpdateCategoryAsync(ProductCategoryModel categoryModel)
        {
            Validation(categoryModel);

            @object.ProductCategoryRepository.Update(mapper.Map<ProductCategoryModel, ProductCategory>(categoryModel));
            @object.SaveAsync();
            return Task.CompletedTask;
        }

        private void Validation(ProductModel model)
        {
            if (model == null)
                throw new MarketException("Product is null");
            if (model.ProductName == null || model.ProductName == "")
                throw new MarketException("Product name is empty");
            if (model.Price <= 0)
                throw new MarketException("Product price is less or equal 0");
        }

        private void Validation(ProductCategoryModel model)
        {
            if (model == null)
                throw new MarketException("Product category is null");
            if (model.CategoryName == null || model.CategoryName == "")
                throw new MarketException("Product category name is empty");
        }
    }
}
