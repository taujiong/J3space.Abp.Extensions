using System.Threading.Tasks;

namespace J3space.Admin.Domain.Data
{
    public interface IAdminDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}