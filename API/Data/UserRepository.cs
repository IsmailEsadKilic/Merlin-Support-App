using API.CryptologyLibrary.Providers;
using API.CryptologyLibrary.Services;
using API.DTOs;
using API.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetUserDtosAsync()
        {
            return await _context.Users
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<UserDto> GetUserDtoAsync(int id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<int> AddUserAsync(User user)
        {
            //sanitize permission
            //"101|105|113", sorted and unique, only numbers
            var permission = user.Permission;
            var sanitizedPermission = string.Join("|", permission.Split('|').Select(int.Parse).Distinct().OrderBy(x => x));

            user.Permission = sanitizedPermission;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        public async Task<bool> UpdateUserAsync(int id, UserDto userDto)
        {
            User user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return false;
            }

            //sanitize permission
            //"101|105|113", sorted and unique, only numbers
            var permission = userDto.Permission;
            var sanitizedPermission = string.Join("|", permission.Split('|').Select(int.Parse).Distinct().OrderBy(x => x));

            user.Permission = sanitizedPermission;
            user.NameSurname = userDto.NameSurname;
            user.UserName = userDto.UserName;
            user.Email = userDto.Email;
            user.Gsml = userDto.Gsml;

            // var inputPassword = userDto.Password;
            // ICryptology cryptoMerlin = CryptologyServiceLocator.CryptologyProvider("UserCrypto");
            // var encryptedPassword = cryptoMerlin.Encrypt(inputPassword);
            // user.Password = encryptedPassword;

            _context.Entry(user).State = EntityState.Modified;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            User user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<PermissionProfile>> GetPermissionProfilesAsync()
        {
            return await _context.Profiles.ToListAsync();
        }
    }
}