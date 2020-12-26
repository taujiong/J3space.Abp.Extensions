using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace J3space.Admin.EfCore
{
    public class AdminDbContextFactory : IDesignTimeDbContextFactory<AdminDbContext>
    {
        public AdminDbContext CreateDbContext(string[] args)
        {
            var connectionString = BuildConfiguration().GetConnectionString("Default");

            var builder = new DbContextOptionsBuilder<AdminDbContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new AdminDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}