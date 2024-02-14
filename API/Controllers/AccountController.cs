using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using API.CryptologyLibrary.Providers;
using API.CryptologyLibrary.Services;
using API.Data;
using API.DTOs;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly TokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController(DataContext context, IMapper mapper, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;

        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] LoginDto loginDto)
        {
            var userName = loginDto.UserName;
            var user = _context.Users.SingleOrDefault(x => x.UserName == userName);

            if (user == null) return Unauthorized("Invalid password or username");

            var passwordInput = loginDto.Password;
            
            ICryptology cryptoMerlin = CryptologyServiceLocator.CryptologyProvider("UserCrypto");
            var passwordInputEncrypted = cryptoMerlin.Encrypt(passwordInput);

            var password = user.Password;
            if (password != passwordInputEncrypted) return Unauthorized("Invalid password or username");

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Token = _tokenService.CreateToken(user);
            return Ok(userDto);
        }

        [Authorize]
        [HttpPost("changePassword")] // PUT: api/account/change-password
        public async Task<ActionResult<bool>> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);

            if (userName == null)
            return BadRequest(userName);

            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == userName);

            if (user == null) return Unauthorized("Invalid password or username");

            var passwordInput = changePasswordDto.CurrentPassword;
            ICryptology cryptoMerlin = CryptologyServiceLocator.CryptologyProvider("UserCrypto");
            var passwordInputEncrypted = cryptoMerlin.Encrypt(passwordInput);

            var password = user.Password;
            if (password != passwordInputEncrypted) return Unauthorized("Invalid password or username");

            var newPassword = changePasswordDto.NewPassword;
            var newPasswordEncrypted = cryptoMerlin.Encrypt(newPassword);
            user.Password = newPasswordEncrypted;

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return BadRequest("Failed to change password");

            return Ok(result);
        }
    }

    public class LoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class ChangePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        [MinLength(4)]
        public string NewPassword { get; set; }
    }
}