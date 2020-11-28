using J3space.Blogging.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace J3space.Blogging.EfCore
{
    public class BloggingDbContext : AbpDbContext<BloggingDbContext>
    {
        public BloggingDbContext(DbContextOptions<BloggingDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureBlogging();
        }
    }
}