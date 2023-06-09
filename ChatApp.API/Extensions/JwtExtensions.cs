using System.Text;
using ChatApp.API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.API.Extensions;

public static class JwtExtensions
{
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var key = config["Token:Key"] ?? throw new ArgumentException("Jwt Key is required");
                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = symmetricSecurityKey,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["Token:Issuer"],
                    ValidateAudience = false,
                    ValidateLifetime = false,
                };
            });
        
        services.AddScoped<IJwtTokenBuilder, JwtTokenBuilder>();
    }
}