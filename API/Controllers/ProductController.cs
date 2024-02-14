using System.Text.Json;
using API.Data;
using API.DTOs;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;

namespace API.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _uow;

        public ProductController(IMapper mapper, UnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;

        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ProductDto>> GetProductsAync()
        {
            return Ok(await _uow.ProductRepository.GetProductDtosAsync());
        }

        [Authorize]
        [HttpGet("customer/{id}")]
        public async Task<ActionResult<List<ProductDto>>> GetProductsByCustomerIdAsync(int id)
        {
            return Ok(await _uow.ProductRepository.GetProductDtosByCustomerIdAsync(id));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductAsync(int id)
        {
            return Ok(await _uow.ProductRepository.GetProductDtoAsync(id));
        }

        [Authorize(Roles = "113, 140")]
        [HttpPut("add")]
        public async Task<ActionResult<ProductDto>> AddProductAsync(ProductDto productDto)
        {
            Product product = _mapper.Map<Product>(productDto);
            product.RowGuid = Guid.NewGuid().ToString();

            var id = await _uow.ProductRepository.AddProductAsync(product);

            if (id > 0)
            {
                return Ok(await _uow.ProductRepository.GetProductDtoAsync(id));
            }
            else
            {
                return BadRequest("Failed to add product");
            }

        }

        [Authorize(Roles = "141, 113")]
        [HttpPost("update/{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProductAsync(int id, ProductDto productDto)
        {
            var ok = await _uow.ProductRepository.UpdateProductAsync(id, productDto);

            if (ok)
            {
                return Ok(await _uow.ProductRepository.GetProductDtoAsync(id));
            }
            else
            {
                return BadRequest("Failed to update product");
            }
        }

        [Authorize(Roles = "142, 113")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteProductAsync(int id)
        {
            var ok = await _uow.ProductRepository.DeleteProductAsync(id);

            if (ok)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest("Failed to delete product");
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Authorize]
        [HttpGet("customerProductList")]
        public async Task<ActionResult<List<CustomerProductListDto>>> GetCustomerProductListsAsync()
        {
            return Ok(await _uow.ProductRepository.GetCustomerProductListDtosAsync());
        }

        [Authorize]
        [HttpGet("customerProductList/{id}")]
        public async Task<ActionResult<CustomerProductListDto>> GetCustomerProductListAsync(int id)
        {
            return Ok(await _uow.ProductRepository.GetCustomerProductListDtoAsync(id));
        }

        [Authorize(Roles = "113, 148")]
        [HttpPut("customerProductList/add")]
        public async Task<ActionResult<ProductDto>> AddCustomerProductListAsync(CustomerProductListDto customerProductListDto)
        {
            CustomerProductList customerProductList = _mapper.Map<CustomerProductList>(customerProductListDto);
            customerProductList.RowGuid = Guid.NewGuid().ToString();

            var id = await _uow.ProductRepository.AddCustomerProductListAsync(customerProductList);

            if (id > 0)
            {
                return Ok(await _uow.ProductRepository.GetCustomerProductListDtoAsync(id));
            }
            else
            {
                return BadRequest("Failed to add customer product list");
            }
        }

        [Authorize(Roles = "149, 113")]
        [HttpPost("customerProductList/update/{id}")]
        public async Task<ActionResult<CustomerProductListDto>> UpdateCustomerProductListAsync(int id, CustomerProductListDto customerProductListDto)
        {
            var ok = await _uow.ProductRepository.UpdateCustomerProductListAsync(id, customerProductListDto);

            if (ok)
            {
                return Ok(await _uow.ProductRepository.GetCustomerProductListDtoAsync(id));
            }
            else
            {
                return BadRequest("Failed to update customer product list");
            }
        }

        [Authorize(Roles = "150, 151, 113")]
        [HttpDelete("customerProductList/{id}")]
        public async Task<ActionResult<bool>> DeleteCustomerProductListAsync(int id)
        {
            var ok = await _uow.ProductRepository.DeleteCustomerProductListAsync(id);

            if (ok)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest("Failed to delete customer product list");
            }
        }
    }
}