using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
using Rental.Api.Services.Storage;
using System;

namespace Rental.Services.Storage
{
    public static class StorageDependency
    {
        public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<StorageOptions>(configuration.GetSection("Storage"));

            services.AddSingleton<IMinioClient>(sp =>
            {
                var opt = sp.GetRequiredService<IOptions<StorageOptions>>().Value;
                var builder = new MinioClient()
                    .WithEndpoint(new Uri(opt.Endpoint).Host, new Uri(opt.Endpoint).Port)
                    .WithCredentials(opt.AccessKey, opt.SecretKey);

                if (opt.UseSSL)
                    builder = builder.WithSSL();

                return builder.Build();
            });

            services.AddScoped<IStorageService, MinioStorageService>();
            return services;
        }
    }
}
