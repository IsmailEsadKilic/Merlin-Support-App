using API.DTOs;
using API.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    // public class Product
    // {
    //     public int Id { get; set; }
    //     public string ProductName { get; set; }
    //     public string RowGuid { get; set; }
        
    // }
    public class ProductRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        //product

        public ProductRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetProductDtosAsync()
        {
            return await _context.Products
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetProductDtosByCustomerIdAsync(int id)
        {
            //first get customerProductLists
            var customerProductListsOfCustomer = await _context.CustomerProductLists.Where(c => c.CustomerId == id).ToListAsync();

            var productIds = new List<int>();

            foreach (var item in customerProductListsOfCustomer)
            {
                if (!productIds.Contains(item.ProductId))
                {
                    productIds.Add(item.ProductId);
                }
            }

            //then get products

            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            //check if customer has any valid license 

            foreach (var item in products)
            {
                var customerProductListsOfProduct = await _context.CustomerProductLists.Where(c => c.CustomerId == id && c.ProductId == item.Id).ToListAsync();

                foreach (var item2 in customerProductListsOfProduct)
                {
                    if (item2.EndDate > DateTime.Now)
                    {
                        item.CustomerHasValidLicense = true;
                    }
                }
            }

            return products;

            // public class ProductDto
            // {
            //     public int Id { get; set; }
            //     public string ProductName { get; set; }
            //     public string RowGuid { get; set; }
            //     public bool CustomerHasValidLicense { get; set; }
            // }
        }

        public async Task<ProductDto> GetProductDtoAsync(int id)
        {
            return await _context.Products
                .Where(p => p.Id == id)
                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<int> AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<bool> UpdateProductAsync(int id, ProductDto productDto)
        {
            Product product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return false;
            }

            product.ProductName = productDto.ProductName;
            

            _context.Entry(product).State = EntityState.Modified;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            Product product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);

            return await _context.SaveChangesAsync() > 0;
        }
        
        //////////////////////////////////////////////////////////////////////////////////////////

        //customerProductList

        //////////////////////////////////////////////////////////////////////////////////////////

        public async Task<IEnumerable<CustomerProductListDto>> GetCustomerProductListDtosAsync()
        {
            var v = await _context.CustomerProductLists
                .ProjectTo<CustomerProductListDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            
            //get customer name
            foreach (var item in v)
            {
                item.CustomerName = await _context.Customers.Where(c => c.Id == item.CustomerId).Select(c => c.CompanyName).FirstOrDefaultAsync();
            }

            //get product name
            foreach (var item in v)
            {
                item.ProductName = await _context.Products.Where(c => c.Id == item.ProductId).Select(c => c.ProductName).FirstOrDefaultAsync();
            }

            return v;
        }

        public async Task<CustomerProductListDto> GetCustomerProductListDtoAsync(int id)
        {
            var v = await _context.CustomerProductLists
                .Where(c => c.Id == id)
                .ProjectTo<CustomerProductListDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            //get customer name

            v.CustomerName = await _context.Customers.Where(c => c.Id == v.CustomerId).Select(c => c.CompanyName).FirstOrDefaultAsync();

            //get product name

            v.ProductName = await _context.Products.Where(c => c.Id == v.ProductId).Select(c => c.ProductName).FirstOrDefaultAsync();

            return v;
        }
        public async Task<int> AddCustomerProductListAsync(CustomerProductList customerProductList)
        {
            await _context.CustomerProductLists.AddAsync(customerProductList);
            await _context.SaveChangesAsync();
            return customerProductList.Id;
        }

        public async Task<bool> UpdateCustomerProductListAsync(int id, CustomerProductListDto customerProductListDto)
        {
            CustomerProductList customerProductList = await _context.CustomerProductLists.FindAsync(id);

            if (customerProductList == null)
            {
                return false;
            }

            _mapper.Map(customerProductListDto, customerProductList);

            _context.Entry(customerProductList).State = EntityState.Modified;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCustomerProductListAsync(int id)
        {
            CustomerProductList customerProductList = await _context.CustomerProductLists.FindAsync(id);

            if (customerProductList == null)
            {
                return false;
            }

            _context.CustomerProductLists.Remove(customerProductList);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}