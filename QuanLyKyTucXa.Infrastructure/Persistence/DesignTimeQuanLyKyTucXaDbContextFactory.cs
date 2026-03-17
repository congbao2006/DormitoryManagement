using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace QuanLyKyTucXa.Infrastructure.Persistence;

public sealed class DesignTimeQuanLyKyTucXaDbContextFactory : IDesignTimeDbContextFactory<QuanLyKyTucXaDbContext>
{
    public QuanLyKyTucXaDbContext CreateDbContext(string[] args)
    {
        var connectionString = "Server=localhost;Port=3306;Database=QuanLyKyTucXaDb;User=root;Password=123456;";

        var optionsBuilder = new DbContextOptionsBuilder<QuanLyKyTucXaDbContext>();
        optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36)));

        return new QuanLyKyTucXaDbContext(optionsBuilder.Options);
    }
}