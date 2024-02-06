using AutoMapper;

namespace API.Data
{
    public class UnitOfWork
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public UnitOfWork(DataContext context, IMapper mapper, IConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _config = config;
        }

        public TestEntityRepository TestEntityRepository => new TestEntityRepository(_context, _mapper);
        public CustomerRepository CustomerRepository => new CustomerRepository(_context, _mapper);
        public UserRepository UserRepository => new UserRepository(_context, _mapper);
        public TeamRepository TeamRepository => new TeamRepository(_context, _mapper);
        public TicketRepository TicketRepository => new TicketRepository(_context, _mapper, _config);
        public ProductRepository ProductRepository => new ProductRepository(_context, _mapper);
        public UploadFileRepository UploadFileRepository => new UploadFileRepository(_context, _mapper);
        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}