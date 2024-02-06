using System.Text;
using API.Data;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddCors();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<UnitOfWork>(); //add unit of work as a scoped service
            services.AddScoped<TokenService>(); //add token service as a scoped service
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => //add jwt bearer authentication
                {
                    options.TokenValidationParameters = new TokenValidationParameters //set token validation parameters
                    {
                        ValidateIssuerSigningKey = true, //validate issuer signing key
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])), //set issuer signing key
                        ValidateIssuer = false, //don't validate issuer
                        ValidateAudience = false //don't validate audience
                    };
                });

            return services;
        }
    }
}