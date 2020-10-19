using System.Collections.Generic;
using System.Threading.Tasks;
using J3space.Auth.EntityFrameworkCore.DbMigrations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace J3space.Auth.DbMigrator
{
    public class AuthDbMigrationService : ITransientDependency
    {
        private readonly IDataSeeder _dataSeeder;
        private readonly IEnumerable<IAuthDbSchemaMigrator> _dbSchemaMigrators;

        public AuthDbMigrationService(
            IDataSeeder dataSeeder,
            IEnumerable<IAuthDbSchemaMigrator> dbSchemaMigrators)
        {
            _dataSeeder = dataSeeder;
            _dbSchemaMigrators = dbSchemaMigrators;

            Logger = NullLogger<AuthDbMigrationService>.Instance;
        }

        public ILogger<AuthDbMigrationService> Logger { get; set; }

        public async Task MigrateAsync()
        {
            Logger.LogInformation("Started database migrations...");

            await MigrateDatabaseSchemaAsync();
            await SeedDataAsync();

            Logger.LogInformation("Successfully completed database migrations.");
        }

        private async Task MigrateDatabaseSchemaAsync()
        {
            Logger.LogInformation("Migrating schema for database...");

            foreach (var migrator in _dbSchemaMigrators)
            {
                await migrator.MigrateAsync();
            }
        }

        private async Task SeedDataAsync()
        {
            Logger.LogInformation("Executing database seed...");

            await _dataSeeder.SeedAsync();
        }
    }
}