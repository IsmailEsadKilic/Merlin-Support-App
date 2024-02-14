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
    public class UserController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _uow;

        public UserController(IMapper mapper, UnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        [Authorize]
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

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserAsync(int id)
        {
            var user = await _uow.UserRepository.GetUserDtoAsync(id);
            user.Password = "";
            return Ok(user);
        }

        [Authorize(Roles = "112, 119, 113")]
        [HttpPut("add")]
        public async Task<ActionResult<UserDto>> AddUserAsync(UserDto userDto)
        {
            var existingUser = await _uow.UserRepository.GetUserDtoAsync(userDto.Id);

            if (existingUser != null)
            {
                return BadRequest("User already exists");
            }

            //check if userDto has any permissions

            if (userDto.Permission != null)
            {
                //check if request has role number 119
                if (!User.IsInRole("119") && !User.IsInRole("113"))
                {
                    return BadRequest("You cannot add users with permissions");
                }
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

        [Authorize(Roles = "113, 120, 127")]
        [HttpPost("update/{id}")]
        public async Task<ActionResult<UserDto>> UpdateUserAsync(int id, UserDto userDto)
        {
            //check if user exists
            UserDto existingUser = await _uow.UserRepository.GetUserDtoAsync(id);

            //check if user is admin


            if (existingUser == null)
            {
                return BadRequest("User does not exist");
            }

            //check if existing user has any permissions

            if (existingUser.Permission != null)
            {
                //check if request has role number 120
                if (!User.IsInRole("120") && !User.IsInRole("113"))
                {
                    return BadRequest("User has permissions. You cannot update users with permissions");
                }
            }

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

        [Authorize(Roles = "113, 121, 126")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteUserAsync(int id)
        {
            //check if user exists

            UserDto existingUser = await _uow.UserRepository.GetUserDtoAsync(id);

            if (existingUser == null)
            {
                return BadRequest("User does not exist");
            }

            //check if existing user has any permissions

            if (existingUser.Permission != null)
            {
                //check if request has role number 121
                if (!User.IsInRole("121") && !User.IsInRole("113"))
                {
                    return BadRequest("User has permissions. You cannot delete users with permissions");
                }
            }

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

        [Authorize]
        [HttpGet("permdict")]
        public ActionResult<Dictionary<string, string>> GetPermDict()
        {
            var en = UserPermissionHelper.GetPermissionsDict();
            return Ok(en);
        }

        [Authorize]
        [HttpGet("permissionProfiles")]
        public async Task<ActionResult<List<PermissionProfile>>> GetProfilesAsync()
        {
            return Ok(await _uow.UserRepository.GetPermissionProfilesAsync());
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Authorize(Roles = "113, 157")]
        [HttpPut("permissionProfiles/add")]
        public async Task<ActionResult<PermissionProfile>> AddProfileAsync(PermissionProfile profile)
        {
            Console.WriteLine("Profile: " + profile.ProfileName + " " + profile.Permission);
            var existingProfile = await _uow.UserRepository.GetPermissionProfileAsync(profile.Id);

            if (existingProfile != null)
            {
                return BadRequest("Profile already exists");
            }

            var id = await _uow.UserRepository.AddPermissionProfileAsync(profile);

            if (await _uow.Complete() || id > 0)
            {
                return Ok(await _uow.UserRepository.GetPermissionProfileAsync(id));
            }
            else
            {
                return BadRequest("Failed to add profile");
            }
        }

        [Authorize(Roles = "113, 158")]
        [HttpDelete("permissionProfiles/{id}")]
        public async Task<ActionResult<bool>> DeleteProfileAsync(int id)
        {
            var existingProfile = await _uow.UserRepository.GetPermissionProfileAsync(id);

            if (existingProfile == null)
            {
                return BadRequest("Profile does not exist");
            }

            var ok = await _uow.UserRepository.DeletePermissionProfileAsync(id);

            if (ok)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest("Failed to delete profile");
            }
        }

    }
}