using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Rental.Core.Authorization;
using System.Security.Claims;
using System.Text;

namespace Rental.Services.Identity
{
    public static class JwtConfig
    {
        public static void AddJwtConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidIssuer = appSettings.Issuer,
                    RoleClaimType = ClaimTypes.Role
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationPolicies.AdminOnly,
                    policy => policy.RequireRole(UserRoles.Admin));

                options.AddPolicy(AuthorizationPolicies.ManagerOnly,
                    policy => policy.RequireRole(UserRoles.Manager));

                options.AddPolicy(AuthorizationPolicies.UserOnly,
                    policy => policy.RequireRole(UserRoles.User));

                options.AddPolicy(AuthorizationPolicies.AdminOrManager,
                    policy => policy.RequireRole(UserRoles.Admin, UserRoles.Manager));
            });
        }

        public static void UseAuthConfiguration(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
