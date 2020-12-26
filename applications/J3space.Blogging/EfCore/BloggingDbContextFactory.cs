using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace J3space.Blogging.EfCore
{
    public class BloggingDbContextFactory : IDesignTimeDbContextFactory<BloggingDbContext>
    {
        public BloggingDbContext CreateDbContext(string[] args)
        {
            var connectionString = BuildConfiguration().GetConnectionString("Default");

            var builder = new DbContextOptionsBuilder<BloggingDbContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new BloggingDbContext(builder.Options);
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