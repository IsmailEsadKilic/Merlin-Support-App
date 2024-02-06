using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [AllowAnonymous]
    public class CustomerController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _uow;

        public CustomerController(IMapper mapper, UnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;

        }

        [HttpGet]
        public async Task<ActionResult<CustomerDto>> GetCustomersAsync()
        {
            return Ok(await _uow.CustomerRepository.GetCustomerDtosAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerAsync(int id)
        {
            return Ok(await _uow.CustomerRepository.GetCustomerDtoAsync(id));
        }

        [HttpPut("add")]
        public async Task<ActionResult<CustomerDto>> AddCustomerAsync(CustomerDto customerDto)
        {   
            //customer has a list of customerProductList
            
            Customer customer = _mapper.Map<Customer>(customerDto);
            customer.RowGuid = Guid.NewGuid().ToString();

            var id = await _uow.CustomerRepository.AddCustomerAsync(customer);

            if (id > 0)
            {
                return Ok(await _uow.CustomerRepository.GetCustomerDtoAsync(id));
            }
            else
            {
                return BadRequest("Failed to add customer");
            }
        }

        [HttpPost("update/{id}")]
        public async Task<ActionResult<CustomerDto>> UpdateCustomerAsync(int id, CustomerUpdateRequest updateRequest)
        {
            var ok = await _uow.CustomerRepository.UpdateCustomerAsync(id, updateRequest.customerDto, updateRequest.customerProductListIdsToDel);

            if (ok)
            {
                return Ok(await _uow.CustomerRepository.GetCustomerDtoAsync(id));
            }
            else
            {
                return BadRequest("Failed to update customer");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteCustomerAsync(int id)
        {
            var ok = await _uow.CustomerRepository.DeleteCustomerAsync(id);

            if (ok)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest("Failed to delete customer");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet("CustomPropertyDescriptors")]
        public async Task<ActionResult<List<CustomPropertyDescriptor>>> GetCustomPropertyDescriptors()
        {
            string jsonFilePath = "./CustomerCustomData.json";
            
            using (StreamReader reader = new StreamReader(jsonFilePath))
            {
                string jsonContent = await reader.ReadToEndAsync();
                 List<CustomPropertyDescriptor> propertyDescriptors = JsonSerializer.Deserialize<List<CustomPropertyDescriptor>>(jsonContent);
                return Ok(propertyDescriptors);
            }
        }
    }
    public class CustomerUpdateRequest
    {
        public CustomerDto customerDto { get; set; }
        public List<int> customerProductListIdsToDel { get; set; } // ids of customerProductList to delete
    }
}
        //         public class CustomerProductList
        // {
        //     public int Id { get; set; }
        //     public string RowGuid { get; set; }
        //     public int ProductId { get; set; }
        //     public string Version { get; set; }
        //     public DateTime FirstDate { get; set; }
        //     public DateTime EndDate { get; set; }
        //     public int CustomerId { get; set; } // foreign key
        //     public string Description { get; set; }
        // }
