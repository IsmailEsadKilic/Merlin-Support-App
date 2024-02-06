using System.Security.AccessControl;
using API.DTOs;
using API.Entities;
using API.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class TestEntityRepository
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public TestEntityRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<TestEntityDto>> GetTestEntityDtosAsync()
        {
            var tests = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "Test 1" },
                new TestEntity { Id = 2, Name = "Test 2" },
                new TestEntity { Id = 3, Name = "Test 3" },
                new TestEntity { Id = 4, Name = "Test 4" },
                new TestEntity { Id = 5, Name = "Test 5" },
            };
            
            var testDtos = _mapper.Map<IEnumerable<TestEntityDto>>(tests);

            await _context.SaveChangesAsync();

            return testDtos;
        }
    }
}