using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace J3space.Sample.DbMigrator
{
    public class SampleDbMigrationService : ITransientDependency
    {
        private readonly IDataSeeder _dataSeeder;

        public SampleDbMigrationService(IDataSeeder dataSeeder)
        {
            _dataSeeder = dataSeeder;
            Logger = NullLogger<SampleDbMigrationService>.Instance;
        }

        public ILogger<SampleDbMigrationService> Logger { get; set; }

        public async Task MigrateAsync()
        {
            Logger.LogInformation("Started database migrations...");

            await SeedDataAsync();

            Logger.LogInformation("Successfully completed database migrations.");
        }

        private async Task SeedDataAsync()
        {
            Logger.LogInformation("Executing host database seed...");

            await _dataSeeder.SeedAsync();
        }
    }
}