using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace J3space.Admin.EntityFrameworkCore.DbMigrations
{
    public class AdminMigrationsDbContextFactory : IDesignTimeDbContextFactory<AdminMigrationsDbContext>
    {
        public AdminMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../J3space.Admin.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var builder = new DbContextOptionsBuilder<AdminMigrationsDbContext>()
                .UseMySql(configuration.GetConnectionString("Default"));

            return new AdminMigrationsDbContext(builder.Options);
        }
    }
}