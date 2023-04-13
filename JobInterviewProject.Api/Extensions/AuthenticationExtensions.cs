using JobInterviewProject.Domain.Models;
using JobInterviewProject.Infrastructure.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JobInterviewProject.Api.Extensions;

public static class AuthenticationExtensions
{
    public static void AddAuthenticationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenValidationParemeters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:Secret"])),

            ValidateIssuer = true,
            ValidIssuer = configuration["JWT:Issuer"],

            ValidateAudience = true,
            ValidAudience = configuration["JWT:Audience"],

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        services.AddSingleton(tokenValidationParemeters);

        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        services.AddAuthorization();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = tokenValidationParemeters;
        });
    }
}
