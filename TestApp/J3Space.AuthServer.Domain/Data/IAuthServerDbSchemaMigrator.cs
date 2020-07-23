using System.Threading.Tasks;

namespace J3space.AuthServer.Data
{
    public interface IAuthServerDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}