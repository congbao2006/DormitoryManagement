using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using QuanLyKyTucXa.Infrastructure.Persistence;

namespace QuanLyKyTucXa.IntegrationTests;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"QuanLyKyTucXaIntegrationDb-{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<QuanLyKyTucXaDbContext>));

            services.AddDbContext<QuanLyKyTucXaDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));

            using var scope = services.BuildServiceProvider().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<QuanLyKyTucXaDbContext>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        });
    }
}
