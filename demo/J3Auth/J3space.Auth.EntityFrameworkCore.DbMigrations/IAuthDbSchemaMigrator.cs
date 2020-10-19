using System.Threading.Tasks;

namespace J3space.Auth.EntityFrameworkCore.DbMigrations
{
    public interface IAuthDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}