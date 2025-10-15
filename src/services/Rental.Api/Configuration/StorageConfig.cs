//using Microsoft.Extensions.Options;
//using Minio;

//namespace Rental.Api.Configuration
//{
//    public static class StorageConfig
//    {
//        public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
//        {
//            services.Configure<StorageOptions>(configuration.GetSection("Storage"));

//            services.AddSingleton<IMinioClient>(sp =>
//            {
//                var opt = sp.GetRequiredService<IOptions<StorageOptions>>().Value;
//                var builder = new MinioClient()
//                    .WithEndpoint(new Uri(opt.Endpoint).Host, new Uri(opt.Endpoint).Port)
//                    .WithCredentials(opt.AccessKey, opt.SecretKey);

//                if (opt.UseSSL) builder = builder.WithSSL();

//                return builder.Build();
//            });

//            return services;
//        }
//    }
//}
