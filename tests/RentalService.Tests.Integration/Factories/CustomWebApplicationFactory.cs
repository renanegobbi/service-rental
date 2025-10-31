using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using RentalService.Tests.Integration.Utils;
using Testcontainers.PostgreSql;


namespace RentalService.Tests.Integration.Factories
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup>, IAsyncLifetime where TStartup : class
    {
        private readonly PostgreSqlContainer _postgresContainer;

        public CustomWebApplicationFactory()
        {
            _postgresContainer = new PostgreSqlBuilder()
                .WithDatabase("RentalServiceDbTest")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .WithImage("postgres:16-alpine")
                .WithPortBinding(15433, 5432)
                .WithName("postgres-rental-service-tests")
                .Build();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string?>(
                        "ConnectionStrings:DefaultConnection",
                        _postgresContainer.GetConnectionString())
                });
            });

            builder.ConfigureServices(services =>
            {
                DatabaseSeeder.ApplyInitScript(_postgresContainer.GetConnectionString());
            });
        }

        public async Task InitializeAsync()
        {
            await _postgresContainer.StartAsync();
            Console.WriteLine($"PostgreSQL container: {_postgresContainer.Hostname}:{_postgresContainer.GetMappedPublicPort(5432)}");
        }

        public new async Task DisposeAsync()
        {
            await _postgresContainer.StopAsync();
        }
    }
}
