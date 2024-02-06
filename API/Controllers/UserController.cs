using API.CryptologyLibrary.Providers;
using API.CryptologyLibrary.Services;
using API.Data;
using API.DTOs;
using API.Entities;
using API.enums;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    public class UserController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _uow;

        public UserController(IMapper mapper, UnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        [HttpGet]
        public async Task<ActionResult<UserDto>> GetUsersAsync()
        {
            var users = await _uow.UserRepository.GetUserDtosAsync();

            foreach (var user in users)
            {
                user.Password = "";
            }

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserAsync(int id)
        {
            var user = await _uow.UserRepository.GetUserDtoAsync(id);
            user.Password = "";
            return Ok(user);
        }

        [HttpPut("add")]
        public async Task<ActionResult<UserDto>> AddUserAsync(UserDto userDto)
        {
            var existingUser = await _uow.UserRepository.GetUserDtoAsync(userDto.Id);

            if (existingUser != null)
            {
                return BadRequest("User already exists");
            }

            User user = _mapper.Map<User>(userDto);
            user.RowGuid = Guid.NewGuid().ToString();

            //handle password

            ICryptology cryptoMerlin = CryptologyServiceLocator.CryptologyProvider("UserCrypto");
            var passwordInputEncrypted = cryptoMerlin.Encrypt(userDto.Password);
            user.Password = passwordInputEncrypted;

            var id = await _uow.UserRepository.AddUserAsync(user);

            if (await _uow.Complete() || id > 0)
            {
                return Ok(await _uow.UserRepository.GetUserDtoAsync(id));
            }
            else
            {
                return BadRequest("Failed to add user");
            }
        }

        [HttpPost("update/{id}")]
        public async Task<ActionResult<UserDto>> UpdateUserAsync(int id, UserDto userDto)
        {
            var ok = await _uow.UserRepository.UpdateUserAsync(id, userDto);

            if (ok)
            {
                return Ok(await _uow.UserRepository.GetUserDtoAsync(id));
            }
            else
            {
                return BadRequest("Failed to update user");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteUserAsync(int id)
        {
            var ok = await _uow.UserRepository.DeleteUserAsync(id);

            if (ok)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest("Failed to delete user");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet("permdict")]
        public ActionResult<Dictionary<string, string>> GetPermDict()
        {
            var en = UserPermissionHelper.GetPermissionsDict();
            return Ok(en);
        }

    }
}