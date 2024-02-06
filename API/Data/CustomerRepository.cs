using System.Security.AccessControl;
using System.Text.Json;
using API.DTOs;
using API.Entities;
using API.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class CustomerRepository
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CustomerRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<CustomerDto>> GetCustomerDtosAsync()
        {
            var v = await _context.Customers
                .Include(c => c.CustomerProductLists)
                .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            //get productNames for each customerProductList

            foreach (var item in v)
            {
                foreach (var customerProductListDto in item.CustomerProductListDtos)
                {
                    var productName = await _context.Products
                        .Where(p => p.Id == customerProductListDto.ProductId)
                        .Select(p => p.ProductName)
                        .SingleOrDefaultAsync();

                    customerProductListDto.ProductName = productName;
                }
            }

            return v;
        }
        public async Task<CustomerDto> GetCustomerDtoAsync(int id)
        {
            var customer = await _context.Customers
                .Where(c => c.Id == id)
                .Include(c => c.CustomerProductLists)
                .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            //get productNames for each customerProductList

            foreach (var customerProductListDto in customer.CustomerProductListDtos)
            {
                var productName = await _context.Products
                    .Where(p => p.Id == customerProductListDto.ProductId)
                    .Select(p => p.ProductName)
                    .SingleOrDefaultAsync();

                customerProductListDto.ProductName = productName;
            }

            return customer;
        }

        public async Task<int> AddCustomerAsync(Customer customer)
        {    
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();


            var advancedMode = false;
            if (advancedMode)
            {
                //add customerProductListAddDtos
                foreach (var customerProductListAdd in customer.CustomerProductLists)
                {
                    CustomerProductList customerProductList = new CustomerProductList
                    {
                        ProductId = customerProductListAdd.ProductId,
                        Version = customerProductListAdd.Version,
                        FirstDate = customerProductListAdd.FirstDate,
                        EndDate = customerProductListAdd.EndDate,
                        CustomerId = customer.Id,
                        Description = customerProductListAdd.Description
                    };

                    await _context.CustomerProductLists.AddAsync(customerProductList);
                }

            }

            return customer.Id;
        }

        public async Task<bool> UpdateCustomerAsync(int id, CustomerDto customerDto, List<int> customerProductListIdsToDel)
        {
            Customer customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return false;
            }

            customer.CompanyName = customerDto.CompanyName;
            customer.AddressPhone = customerDto.AddressPhone;
            customer.Fax = customerDto.Fax;
            customer.TaxOffice = customerDto.TaxOffice;
            customer.TaxNumber = customerDto.TaxNumber;
            customer.CustomerEmail = customerDto.CustomerEmail;
            customer.JsonData = customerDto.JsonData;

            //add customerProductListAddDtos

            foreach (var customerProductListAddDto in customerDto.CustomerProductListDtos)
            {
                CustomerProductList customerProductList = new CustomerProductList
                {
                    ProductId = customerProductListAddDto.ProductId,
                    Version = customerProductListAddDto.Version,
                    FirstDate = customerProductListAddDto.FirstDate,
                    EndDate = customerProductListAddDto.EndDate,
                    CustomerId = customer.Id,
                    Description = customerProductListAddDto.Description
                };

                await _context.CustomerProductLists.AddAsync(customerProductList);
            }

            _context.Entry(customer).State = EntityState.Modified;

            //delete customerProductListIdsToDel

            foreach (var customerProductListId in customerProductListIdsToDel)
            {
                CustomerProductList customerProductList = await _context.CustomerProductLists.FindAsync(customerProductListId);

                if (customerProductList != null)
                {
                    _context.CustomerProductLists.Remove(customerProductList);
                }
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            // Customer customer = await _context.Customers.FindAsync(id);
            //include customerProductLists

            Customer customer = await _context.Customers
                .Where(c => c.Id == id)
                .Include(c => c.CustomerProductLists)
                .SingleOrDefaultAsync();

            if (customer == null)
            {
                return false;
            }

            //remove customerProductLists

            foreach (var customerProductList in customer.CustomerProductLists)
            {
                _context.CustomerProductLists.Remove(customerProductList);
            }

            _context.Customers.Remove(customer);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
    // public class Customer 
    // {
    //     public int Id { get; set; }
    //     public string RowGuid { get; set; }
    //     public string CompanyName { get; set; }
    //     public string AddressPhone { get; set; }
    //     public string Fax { get; set; }
    //     public string TaxOffice { get; set; }
    //     public string TaxNumber { get; set; }
    //     public string CustomerEmail { get; set; }
    //     public string JsonData { get; set; }
    //     public IList<CustomerProductList> CustomerProductLists { get; set; }
    // }

    //     public class CustomerProductList
    // {
    //     public int Id { get; set; }
    //     public string RowGuid { get; set; }
    //     public int ProductId { get; set; }
    //     public string Version { get; set; }
    //     public DateTime FirstDate { get; set; }
    //     public DateTime EndDate { get; set; }
    //     public Customer Customer { get; set; }
    //     public int CustomerId { get; set; }
    //     public string Description { get; set; }
    // }