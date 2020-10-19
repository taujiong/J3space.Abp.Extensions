using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;

namespace J3space.Auth.EntityFrameworkCore.DbMigrations
{
    public class AuthMigrationsDbContext : AbpDbContext<AuthMigrationsDbContext>
    {
        public AuthMigrationsDbContext(DbContextOptions<AuthMigrationsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureIdentity();
            modelBuilder.ConfigureIdentityServer();
        }
    }
}