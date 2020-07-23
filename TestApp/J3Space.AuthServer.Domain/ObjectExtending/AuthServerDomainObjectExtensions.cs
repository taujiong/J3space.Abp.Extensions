using Volo.Abp.Threading;

namespace J3space.AuthServer.ObjectExtending
{
    public static class AuthServerDomainObjectExtensions
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                /* You can configure extension properties to entities or other object types
                 * defined in the depended modules.
                 * 
                 * If you are using EF Core and want to map the entity extension properties to new
                 * table fields in the database, then configure them in the AuthServerEfCoreEntityExtensionMappings
                 *
                 * Example:
                 *
                 * ObjectExtensionManager.Instance
                 *    .AddOrUpdateProperty<IdentityRole, string>("Title");
                 *
                 * See the documentation for more:
                 * https://docs.abp.io/en/abp/latest/Object-Extensions
                 */
            });
        }
    }
}