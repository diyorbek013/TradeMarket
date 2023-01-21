using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        //Inject customer service via constructor
        private ICustomerService customerService;
        public CustomersController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> GetAll()
        {
            var customers = JsonSerializer.Serialize(await customerService.GetAllAsync());
            return Ok(customers);
        }

        //GET: api/customers/1
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerModel>> GetById(int id)
        {
            var customers = JsonSerializer.Serialize(await customerService.GetByIdAsync(id));
            return Ok(customers);
        }
        
        //GET: api/customers/products/1
        [HttpGet("products/{id}")]
        public async Task<ActionResult<CustomerModel>> GetByProductId(int id)
        {
            var customers = JsonSerializer.Serialize(await customerService.GetCustomersByProductIdAsync(id));
            return Ok(customers);
        }

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] CustomerModel value)
        {
            await customerService.AddAsync(value); 
            return Ok(value);
        }

        // PUT: api/customers/1
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int Id, [FromBody] CustomerModel value)
        {
            value.Id = Id;
            await customerService.UpdateAsync(value);
            return Ok(value);
        }

        // DELETE: api/customers/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await customerService.DeleteAsync(id);
            return Ok();
        }
    }
}
