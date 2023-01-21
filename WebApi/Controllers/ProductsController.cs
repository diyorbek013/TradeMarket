using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductService productService;
        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        // Get all Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetAll()
        {
            return Content(JsonConvert.SerializeObject(
                await productService.GetAllAsync()));
        }

        [HttpGet("{id}")]
        // Get Products by id
        public async Task<ActionResult<ProductModel>> GetById(int id)
        {
            return Content(JsonConvert.SerializeObject(
                await productService.GetByIdAsync(id)));
        }

        [HttpGet("products")]
        // Get with filters
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetByFilter([FromQuery] FilterSearchModel filterSearchModel)
        {
            return Content(
                JsonConvert.SerializeObject(
                    await productService.GetByFilterAsync(
                        filterSearchModel)));
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<ProductCategoryModel>>> Get()
        {
            return Content(JsonConvert.SerializeObject(
                await productService
                .GetAllProductCategoriesAsync()));
        }

        [HttpPost]
        // Add product
        public async Task<ActionResult> Add([FromBody] ProductModel productModel)
        {
            await productService.AddAsync(productModel);
            return Ok(productModel);
        }

        [HttpPost("categories")]
        public async Task<ActionResult> AddCategory([FromBody] ProductCategoryModel productCategoryModel)
        {
            await productService.AddCategoryAsync(productCategoryModel);

            return Ok(productCategoryModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await productService.DeleteAsync(id);
            return Ok();
        }

        [HttpDelete("categories/{id}")]
        public async Task<ActionResult> DeleteCategories(int id)
        {
            await productService.RemoveCategoryAsync(id);
            return Ok();
        }

        [HttpPut("{id}")]
        // PUT: api/customers/1
        public async Task<ActionResult> Update(int id, [FromBody] ProductModel productModel)
        {
            productModel.Id = id;

            await productService.UpdateAsync(productModel);
            return Ok();
        }

        [HttpPut("categories/{id}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] ProductCategoryModel productCategoryModel)
        {
            productCategoryModel.Id = id;
            await productService.UpdateCategoryAsync(productCategoryModel);
            return Ok();
        }

        


    }
}
