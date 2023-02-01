using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Oauth2Provider.DAL;

namespace Oauth2Provider.Extensions;

public static class StartupExtensions
{
    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });
        services.AddControllers();
        return services;
    }
}