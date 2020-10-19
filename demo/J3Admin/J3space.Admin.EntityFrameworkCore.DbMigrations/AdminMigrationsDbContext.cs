using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace J3space.Admin.EntityFrameworkCore.DbMigrations
{
    public class AdminMigrationsDbContext : AbpDbContext<AdminMigrationsDbContext>
    {
        public AdminMigrationsDbContext(DbContextOptions<AdminMigrationsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureAuditLogging();
            builder.ConfigureFeatureManagement();
            builder.ConfigurePermissionManagement();
            builder.ConfigureSettingManagement();
        }
    }
}