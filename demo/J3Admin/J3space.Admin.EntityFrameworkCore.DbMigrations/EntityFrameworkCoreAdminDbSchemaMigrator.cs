using System;
using System.Threading.Tasks;
using J3space.Admin.Domain.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace J3space.Admin.EntityFrameworkCore.DbMigrations
{
    public class EntityFrameworkCoreAdminDbSchemaMigrator : IAdminDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreAdminDbSchemaMigrator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            await _serviceProvider
                .GetRequiredService<AdminMigrationsDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}