using System.ComponentModel;
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
    
    public static string GetEnumDescription(this Enum en)
    {
        if (en == null) return null;

        var type = en.GetType();

        var memberInfo = type.GetMember(en.ToString());
        var description = (memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute),
            false).FirstOrDefault() as DescriptionAttribute)?.Description;

        return description;
    }
    
    public static bool IsRedirectUriStartWithHttps(this string redirectUri)
    {
        if(redirectUri != null && redirectUri.StartsWith("https")) return true;

        return false;
    }

}