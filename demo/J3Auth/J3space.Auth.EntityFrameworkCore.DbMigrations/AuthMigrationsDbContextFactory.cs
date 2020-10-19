using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace J3space.Auth.EntityFrameworkCore.DbMigrations
{
    public class AuthMigrationsDbContextFactory : IDesignTimeDbContextFactory<AuthMigrationsDbContext>
    {
        public AuthMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../J3space.Auth.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var builder = new DbContextOptionsBuilder<AuthMigrationsDbContext>()
                .UseMySql(configuration.GetConnectionString("Default"));

            return new AuthMigrationsDbContext(builder.Options);
        }
    }
}