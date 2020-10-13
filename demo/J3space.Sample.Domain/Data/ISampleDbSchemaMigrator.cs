using System.Threading.Tasks;

namespace J3space.Sample.Data
{
    public interface ISampleDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}