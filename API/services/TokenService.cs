using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Data;
using API.Entities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;

namespace API.Services
{
    public class TokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly UnitOfWork _uow;
        public TokenService(IConfiguration config, UnitOfWork uow)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])); 
            _uow = uow;
        }
        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                //add userName to token
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)

            };

            IList<string> roles = user.Permission.Split('|'); //get roles from user

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role))); //add roles to token

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature); //create signing credentials

            var tokenDescriptor = new SecurityTokenDescriptor //create token descriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7), //token expires in 7 days
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler(); //create token handler

            var token = tokenHandler.CreateToken(tokenDescriptor); //create token

            return tokenHandler.WriteToken(token); //return token
        }
    }
}