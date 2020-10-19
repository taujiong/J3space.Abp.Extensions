using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace J3space.Auth.EntityFrameworkCore.DbMigrations
{
    public class EntityFrameworkCoreAuthDbSchemaMigrator : IAuthDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreAuthDbSchemaMigrator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            await _serviceProvider
                .GetRequiredService<AuthMigrationsDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}