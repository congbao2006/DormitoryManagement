using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuanLyKyTucXa.Application.Abstractions.Repositories;
using QuanLyKyTucXa.Infrastructure.Persistence;
using QuanLyKyTucXa.Infrastructure.Repositories;

namespace QuanLyKyTucXa.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseProvider = configuration["DatabaseProvider"] ?? "MySql";
        var mySqlConnectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Server=localhost;Port=3306;Database=QuanLyKyTucXaDb;User=root;Password=123456;";
        var sqliteConnectionString = configuration.GetConnectionString("SqliteConnection")
            ?? "Data Source=QuanLyKyTucXa.local.db";

        services.AddDbContext<QuanLyKyTucXaDbContext>(options =>
        {
            if (string.Equals(databaseProvider, "Sqlite", StringComparison.OrdinalIgnoreCase))
            {
                options.UseSqlite(sqliteConnectionString);
                return;
            }

            options.UseMySql(mySqlConnectionString, ServerVersion.AutoDetect(mySqlConnectionString));
        });

        services.AddScoped<IQuanLyKyTucXaRepository, QuanLyKyTucXaRepository>();

        return services;
    }
}